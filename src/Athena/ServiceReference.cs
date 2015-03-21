using System;
using Newtonsoft.Json;

namespace Athena
{
	/// <summary>
	///     Represents information to reference to a service within serialized data.
	/// </summary>
	public sealed class ServiceReference
	{
		/// <summary>
		///     Gets or sets the name of the service being referenced. For human readability of files only.
		/// </summary>
		[JsonProperty("name")]
		public string FriendlyName { get; set; }

		/// <summary>
		///     Gets or sets the guid of the service being referenced. Used for lookup.
		/// </summary>
		[JsonProperty("guid")]
		public Guid Guid { get; set; }
	}
}