using SA3D.Common.Converters;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Common.JsonConverters
{
	/// <summary>
	/// Json converter for <see cref="Vector4"/>.
	/// </summary>
	public class Vector4JsonConverter : JsonConverter<Vector4>
	{
		/// <inheritdoc/>
		public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Vector4!");
			}

			return Vector4Converter.ConvertFrom(reader.GetString()!);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
		{
            writer.WriteStringValue(Vector4Converter.ConvertTo(value));
		}
	}
}
