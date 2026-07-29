using SA3D.Common.Converters;
using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Common.JsonConverters
{
	/// <summary>
	/// Json converter for <see cref="Quaternion"/>.
	/// </summary>
	public sealed class QuaternionJsonConverter : JsonConverter<Quaternion>
	{
		/// <inheritdoc/>
		public override Quaternion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Quaternion!");
			}

			return QuaternionConverter.ConvertFrom(reader.GetString()!, null);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Quaternion value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(QuaternionConverter.ConvertTo(value));
		}
	}
}
