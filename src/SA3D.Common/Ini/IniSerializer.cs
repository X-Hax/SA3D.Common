using System;
using System.ComponentModel;
using static SA3D.Common.Ini.Deserialization.Deserialize;
using static SA3D.Common.Ini.IniSerializeHelper;
using static SA3D.Common.Ini.Serialization.Serialize;

namespace SA3D.Common.Ini
{
	/// <summary>
	/// Provides methods for converting data to an Ini dictionary.
	/// </summary>
	public static class IniSerializer
	{
		#region Serializing

		/// <summary>
		/// Converts the provided data to an Ini Dictionary.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <param name="converter">The converter to use for the data object itself</param>
		/// <returns>The converted Ini Dictionary</returns>
		public static IniDictionary Serialize(object? data, IniCollectionSettings collectionSettings, TypeConverter? converter)
		{
			IniDictionary ini = new() { { string.Empty, new IniGroup() } };
			SerializeInternal("value", data, ini, string.Empty, true, collectionSettings, converter);
			return ini;
		}

		/// <summary>
		/// Converts the provided data to an Ini Dictionary.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <returns>The converted Ini Dictionary</returns>
		public static IniDictionary Serialize(object? data, IniCollectionSettings collectionSettings)
		{
			return Serialize(data, collectionSettings, null);
		}

		/// <summary>
		/// Converts the provided data to an Ini Dictionary.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <param name="converter">The converter to use for the data object itself</param>
		/// <returns>The converted Ini Dictionary</returns>
		public static IniDictionary Serialize(object? data, TypeConverter? converter)
		{
			return Serialize(data, _initialCollectionSettings, converter);
		}

		/// <summary>
		/// Converts the provided data to an Ini Dictionary.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <returns>The converted Ini Dictionary</returns>
		public static IniDictionary Serialize(object? data)
		{
			return Serialize(data, _initialCollectionSettings, null);
		}


		/// <summary>
		/// Converts the provided data to an Ini Dictionary and writes it to a file.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <param name="filepath">Path to the file to write to.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <param name="converter">The converter to use for the data object itself</param>
		public static void SerializeToFile(object? data, string filepath, IniCollectionSettings collectionSettings, TypeConverter? converter)
		{
			IniFile.WriteToFile(Serialize(data, collectionSettings, converter), filepath);
		}

		/// <summary>
		/// Converts the provided data to an Ini Dictionary and writes it to a file.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <param name="filepath">Path to the file to write to.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		public static void SerializeToFile(object? data, string filepath, IniCollectionSettings collectionSettings)
		{
			IniFile.WriteToFile(Serialize(data, collectionSettings), filepath);
		}

		/// <summary>
		/// Converts the provided data to an Ini Dictionary and writes it to a file.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <param name="filepath">Path to the file to write to.</param>
		/// <param name="converter">The converter to use for the data object itself</param>
		public static void SerializeToFile(object? data, string filepath, TypeConverter? converter)
		{
			IniFile.WriteToFile(Serialize(data, converter), filepath);
		}

		/// <summary>
		/// Converts the provided data to an Ini Dictionary and writes it to a file.
		/// </summary>
		/// <param name="data">Data to convert.</param>
		/// <param name="filepath">Path to the file to write to.</param>
		public static void SerializeToFile(object? data, string filepath)
		{
			IniFile.WriteToFile(Serialize(data), filepath);
		}

		#endregion

		#region Deserializing

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="ini">Ini data to convert.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static object? Deserialize(Type type, IniDictionary ini, IniCollectionSettings collectionSettings, TypeConverter? converter)
		{
			return DeserializeInternal(type, ini, collectionSettings, converter);
		}

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="ini">Ini data to convert.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <returns>The converted data.</returns>
		public static object? Deserialize(Type type, IniDictionary ini, IniCollectionSettings collectionSettings)
		{
			return DeserializeInternal(type, ini, collectionSettings, null);
		}

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="ini">Ini data to convert.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static object? Deserialize(Type type, IniDictionary ini, TypeConverter? converter)
		{
			return DeserializeInternal(type, ini, _initialCollectionSettings, converter);
		}

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="ini">Ini data to convert.</param>
		/// <returns>The converted data.</returns>
		public static object? Deserialize(Type type, IniDictionary ini)
		{
			return DeserializeInternal(type, ini, _initialCollectionSettings, null);
		}


		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static object? DeserializeFromFile(Type type, string filepath, IniCollectionSettings collectionSettings, TypeConverter? converter)
		{
			return Deserialize(type, IniFile.ReadFromFile(filepath), collectionSettings, converter);
		}

		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <returns>The converted data.</returns>
		public static object? DeserializeFromFile(Type type, string filepath, IniCollectionSettings collectionSettings)
		{
			return Deserialize(type, IniFile.ReadFromFile(filepath), collectionSettings);
		}

		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static object? DeserializeFromFile(Type type, string filepath, TypeConverter? converter)
		{
			return Deserialize(type, IniFile.ReadFromFile(filepath), converter);
		}

		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <param name="type">Type to convert to.</param>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <returns>The converted data.</returns>
		public static object? DeserializeFromFile(Type type, string filepath)
		{
			return Deserialize(type, IniFile.ReadFromFile(filepath));
		}

		#endregion

		#region Generic Deserializing

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="ini">Ini data to convert.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static T? Deserialize<T>(IniDictionary ini, IniCollectionSettings collectionSettings, TypeConverter? converter)
		{
			return (T?)Deserialize(typeof(T), ini, collectionSettings, converter);
		}

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="ini">Ini data to convert.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <returns>The converted data.</returns>
		public static T? Deserialize<T>(IniDictionary ini, IniCollectionSettings collectionSettings)
		{
			return (T?)Deserialize(typeof(T), ini, collectionSettings);
		}

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="ini">Ini data to convert.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static T? Deserialize<T>(IniDictionary ini, TypeConverter? converter)
		{
			return (T?)Deserialize(typeof(T), ini, converter);
		}

		/// <summary>
		/// Converts an Ini dictionary to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="ini">Ini data to convert.</param>
		/// <returns>The converted data.</returns>
		public static T? Deserialize<T>(IniDictionary ini)
		{
			return (T?)Deserialize(typeof(T), ini);
		}


		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static T? DeserializeFromFile<T>(string filepath, IniCollectionSettings collectionSettings, TypeConverter? converter)
		{
			return Deserialize<T>(IniFile.ReadFromFile(filepath), collectionSettings, converter);
		}

		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <param name="collectionSettings">Settings to use for converting collections.</param>
		/// <returns>The converted data.</returns>
		public static T? DeserializeFromFile<T>(string filepath, IniCollectionSettings collectionSettings)
		{
			return Deserialize<T>(IniFile.ReadFromFile(filepath), collectionSettings);
		}

		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <param name="converter">Type converter to use to convert to type.</param>
		/// <returns>The converted data.</returns>
		public static T? DeserializeFromFile<T>(string filepath, TypeConverter? converter)
		{
			return Deserialize<T>(IniFile.ReadFromFile(filepath), converter);
		}

		/// <summary>
		/// Converts Ini data from a file to an object of specified type.
		/// </summary>
		/// <typeparam name="T">Type to convert to.</typeparam>
		/// <param name="filepath">Path to the ini file that should be converted.</param>
		/// <returns>The converted data.</returns>
		public static T? DeserializeFromFile<T>(string filepath)
		{
			return Deserialize<T>(IniFile.ReadFromFile(filepath));
		}

		#endregion

	}
}
