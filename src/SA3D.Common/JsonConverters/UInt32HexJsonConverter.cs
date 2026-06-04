using SA3D.Common.Converters;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Common.JsonConverters
{
	/// <summary>
	/// Json converter for uint32 hexadecimal
	/// </summary>
	public class UInt32HexJsonConverter : JsonConverter<uint>
	{
		/// <inheritdoc/>
		public override uint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Int32-hexadecimal");
			}

			return UInt32HexConverter.ConvertFrom(reader.GetString()!, null);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, uint value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(UInt32HexConverter.ConvertTo(value));
		}
	}
}
