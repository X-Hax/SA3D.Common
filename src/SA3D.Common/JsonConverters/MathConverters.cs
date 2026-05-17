using System.Text.Json;

namespace SA3D.Common.JsonConverters
{
	/// <summary>
	/// Json math converter utility class
	/// </summary>
	public static class MathConverters
	{
		/// <summary>
		/// Adds converters for various math structs to the options
		/// </summary>
		/// <param name="options">The options to add the converters to</param>
		public static void AddMathConverters(this JsonSerializerOptions options)
		{
			options.Converters.Add(new Vector2JsonConverter());
			options.Converters.Add(new Vector3JsonConverter());
			options.Converters.Add(new Vector4JsonConverter());
			options.Converters.Add(new QuaternionJsonConverter());
			options.Converters.Add(new Matrix4x4JsonConverter());
		}
	}
}
