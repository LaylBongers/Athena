using System;
using System.Globalization;
using System.Linq;
using Athena.Toolbox.World2D;
using Newtonsoft.Json;

namespace Athena.Toolbox
{
	[JsonConverter(typeof(Vector2FConverter))]
	public struct Vector2F : IEquatable<Vector2F>
	{
		public static readonly Vector2F Zero = new Vector2F();

		public Vector2F(float x, float y)
		{
			X = x;
			Y = y;
		}

		public float X { get; }
		public float Y { get; }

		public bool Equals(Vector2F other)
		{
			return Y.Equals(other.Y) && X.Equals(other.X);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Vector2F && Equals((Vector2F) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Y.GetHashCode()*397) ^ X.GetHashCode();
			}
		}

		public static bool operator ==(Vector2F left, Vector2F right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector2F left, Vector2F right)
		{
			return !left.Equals(right);
		}

		public static Vector2F operator +(Vector2F left, Vector2F right)
		{
			return new Vector2F(left.X + right.X, left.Y + right.Y);
		}
	}

	public sealed class Vector2FConverter : JsonConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			// Load the data we're looking at from the stream
			var values = ((string)reader.Value)
				// Split the data, accept both ',' and ' ' as separators
				.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
				// Parse the separate bits into floats (do not remove the template parameters, Unity's fault)
				.Select(float.Parse).ToArray();

			// Stuff the data into a Vector2
			return new Vector2F(values[0], values[1]);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			// Write a simple representation of our value to the string
			var vector = (Vector2F)value;
			writer.WriteValue(
				vector.X.ToString(CultureInfo.InvariantCulture) + "," +
				vector.Y.ToString(CultureInfo.InvariantCulture));
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector2F);
		}
	}
}