using SA3D.Common.Converters;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Common.JsonConverters
{
	/// <summary>
	/// Json converter for <see cref="Vector2"/>.
	/// </summary>
	public class Vector2JsonConverter : JsonConverter<Vector2>
	{
		/// <inheritdoc/>
		public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Vector2!");
			}

			return Vector2Converter.ConvertFrom(reader.GetString()!);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(Vector2Converter.ConvertTo(value));
		}
	}
}
