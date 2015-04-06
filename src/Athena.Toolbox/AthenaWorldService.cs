using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Athena.Toolbox
{
	[Service("Athena World Service", "C4B112C1-0391-4A22-9CC2-81795B871060")]
	[DependConstraint(typeof (IWorldService))]
	public sealed class AthenaWorldService : IService, IWorldService
	{
		private readonly Dictionary<Guid, Type> _behaviorLookup = new Dictionary<Guid, Type>();
		private readonly Dictionary<Guid, Type> _dataLookup = new Dictionary<Guid, Type>();
		private readonly List<object> _dependencies = new List<object>();
		private EventWaitHandle _waitHandle;
		public Entity Root { get; set; } = new Entity();

		/// <summary>
		///     Gets a lock object that can be used to synchronize access to the world.
		/// </summary>
		public object Lock { get; } = new object();

		public void Initialize()
		{
			_waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

			// Populate the dependencies so we can inject them on update
			_dependencies.Add(_game);
			_dependencies.AddRange(_game.Services);

			var world = _config.Default.GetValue("defaultWorld");
			if (world != null)
				Root = LoadWorld(File.ReadAllText(world));
		}

		public void Dispose()
		{
			_waitHandle.Dispose();
			Root.Dispose();
		}

		public void WaitForUpdate()
		{
			_waitHandle.WaitOne();
		}

		public void Update(TimeSpan elapsed)
		{
			// When updating the world should be locked
			lock (Lock)
			{
				Root.ForceInitialize(_dependencies);

				// We're done updating so signal that we're done
				_waitHandle.Set();
			}
		}

		public Entity LoadWorld(string json)
		{
			var fileModel = JsonConvert.DeserializeObject<EntityInfo>(json);
			return ConvertInfo(fileModel);
		}

		public void RegisterDataType(Type type)
		{
			PerformRegister<EntityDataAttribute>(type, (t, a) => _dataLookup.Add(a.Guid, t));
		}

		public void RegisterBehaviorType(Type type)
		{
			if (!typeof (IBehavior).IsAssignableFrom(type))
				throw new InvalidOperationException("Type does not implement IBehavior.");

			PerformRegister<EntityBehaviorAttribute>(type, (t, a) => _behaviorLookup.Add(a.Guid, t));
		}

		private static void PerformRegister<TAttribute>(Type type, Action<Type, TAttribute> callback)
			where TAttribute : Attribute
		{
			// Find the attribute
			var attribute = type.GetCustomAttribute<TAttribute>();
			if (attribute == null)
			{
				throw new InvalidOperationException("Type lacks an " + typeof (TAttribute).Name + " attribute.");
			}

			// Find the constructor
			var constructor = type.GetConstructor(new Type[0]);
			if (constructor == null)
				throw new InvalidOperationException("Type lacks an empty constructor.");

			// Pass our found data back
			callback(type, attribute);
		}

		private Entity ConvertInfo(EntityInfo info)
		{
			var entity = new Entity
			{
				Identifier = info.Identifier
			};

			// Add in all the requested data
			foreach (var requestedData in info.Data)
			{
				// Find the type with the requested guid
				Type dataType;
				if (!_dataLookup.TryGetValue(requestedData.TypeGuid, out dataType))
				{
					throw new InvalidOperationException("Could not find Data type " + requestedData.FriendlyName + ".");
				}

				// Actually construct the data type
				var constructor = dataType.GetConstructor(new Type[0]);
				Debug.Assert(constructor != null);
				var data = constructor.Invoke(new object[0]);

				// Set all the properties in the data
				foreach (var requestedProperty in requestedData.Properties)
				{
					// Find the property matching the requested
					var property = dataType.GetProperty(requestedProperty.Key);

					if (property == null)
						throw new InvalidOperationException("Property " + requestedProperty.Key + " could not be found.");

					property.SetMethod.Invoke(data, new[] {requestedProperty.Value.ToObject(property.PropertyType)});
				}

				// Finally, add it
				entity.Data.Add(data);
			}

			// Add in all the requested behaviors
			foreach (var requestedBehavior in info.Behaviors)
			{
				// Find the type with the requested guid
				Type behaviorType;
				if (!_behaviorLookup.TryGetValue(requestedBehavior.TypeGuid, out behaviorType))
				{
					throw new InvalidOperationException("Could not find Behavior type " + requestedBehavior.FriendlyName + ".");
				}

				// Actually construct the data type
				var constructor = behaviorType.GetConstructor(new Type[0]);
				Debug.Assert(constructor != null);
				var behavior = (IBehavior) constructor.Invoke(new object[0]);

				// Finally, add it
				entity.Behaviors.Add(behavior);
			}

			// Add any children that are requested
			foreach (var requestedChild in info.Children)
			{
				entity.Children.Add(ConvertInfo(requestedChild));
			}

			return entity;
		}

		// ReSharper disable All
		private class EntityInfo
		{
			[JsonProperty("identifier")]
			public string Identifier { get; set; }

			[JsonProperty("data")]
			public List<DataInfo> Data { get; } = new List<DataInfo>();

			[JsonProperty("behaviors")]
			public List<BehaviorInfo> Behaviors { get; } = new List<BehaviorInfo>();

			[JsonProperty("children")]
			public List<EntityInfo> Children { get; } = new List<EntityInfo>();
		}

		private class DataInfo
		{
			[JsonProperty("name")]
			public string FriendlyName { get; set; }

			[JsonProperty("typeGuid", Required = Required.Always)]
			public Guid TypeGuid { get; set; }

			[JsonProperty("properties")]
			public Dictionary<string, JToken> Properties { get; } = new Dictionary<string, JToken>();
		}

		private class BehaviorInfo
		{
			[JsonProperty("name")]
			public string FriendlyName { get; set; }

			[JsonProperty("typeGuid", Required = Required.Always)]
			public Guid TypeGuid { get; set; }
		}

		// ReSharper restore All

		#region Dependencies

		[Depend] private IConfigService _config;
		[Depend] private Game _game;

		#endregion
	}
}