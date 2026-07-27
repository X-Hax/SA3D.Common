using SA3D.Common.Converters;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Common.JsonConverters
{
	/// <summary>
	/// Json converter for <see cref="Vector3"/>.
	/// </summary>
	public class Vector3JsonConverter : JsonConverter<Vector3>
	{
		/// <inheritdoc/>
		public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Vector3!");
			}

			return Vector3Converter.ConvertFrom(reader.GetString()!, null);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(Vector3Converter.ConvertTo(value));
		}
	}
}
