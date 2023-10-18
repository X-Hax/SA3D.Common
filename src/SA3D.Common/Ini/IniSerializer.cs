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
        /// <param name="filename">Path to the file to write to.</param>
        /// <param name="collectionSettings">Settings to use for converting collections.</param>
        /// <param name="converter">The converter to use for the data object itself</param>
        public static void SerializeToFile(object? data, string filename, IniCollectionSettings collectionSettings, TypeConverter? converter)
        {
            IniFile.Write(Serialize(data, collectionSettings, converter), filename);
        }

        /// <summary>
        /// Converts the provided data to an Ini Dictionary and writes it to a file.
        /// </summary>
        /// <param name="data">Data to convert.</param>
        /// <param name="filename">Path to the file to write to.</param>
        /// <param name="collectionSettings">Settings to use for converting collections.</param>
        public static void SerializeToFile(object? data, string filename, IniCollectionSettings collectionSettings)
        {
            IniFile.Write(Serialize(data, collectionSettings), filename);
        }

        /// <summary>
        /// Converts the provided data to an Ini Dictionary and writes it to a file.
        /// </summary>
        /// <param name="data">Data to convert.</param>
        /// <param name="filename">Path to the file to write to.</param>
        /// <param name="converter">The converter to use for the data object itself</param>
        public static void SerializeToFile(object? data, string filename, TypeConverter? converter)
        {
            IniFile.Write(Serialize(data, converter), filename);
        }

        /// <summary>
        /// Converts the provided data to an Ini Dictionary and writes it to a file.
        /// </summary>
        /// <param name="data">Data to convert.</param>
        /// <param name="filename">Path to the file to write to.</param>
        public static void SerializeToFile(object? data, string filename)
        {
            IniFile.Write(Serialize(data), filename);
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
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <param name="collectionSettings">Settings to use for converting collections.</param>
        /// <param name="converter">Type converter to use to convert to type.</param>
        /// <returns>The converted data.</returns>
        public static object? DeserializeFromFile(Type type, string filename, IniCollectionSettings collectionSettings, TypeConverter? converter)
        {
            return Deserialize(type, IniFile.Read(filename), collectionSettings, converter);
        }

        /// <summary>
        /// Converts Ini data from a file to an object of specified type.
        /// </summary>
        /// <param name="type">Type to convert to.</param>
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <param name="collectionSettings">Settings to use for converting collections.</param>
        /// <returns>The converted data.</returns>
        public static object? DeserializeFromFile(Type type, string filename, IniCollectionSettings collectionSettings)
        {
            return Deserialize(type, IniFile.Read(filename), collectionSettings);
        }

        /// <summary>
        /// Converts Ini data from a file to an object of specified type.
        /// </summary>
        /// <param name="type">Type to convert to.</param>
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <param name="converter">Type converter to use to convert to type.</param>
        /// <returns>The converted data.</returns>
        public static object? DeserializeFromFile(Type type, string filename, TypeConverter? converter)
        {
            return Deserialize(type, IniFile.Read(filename), converter);
        }

        /// <summary>
        /// Converts Ini data from a file to an object of specified type.
        /// </summary>
        /// <param name="type">Type to convert to.</param>
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <returns>The converted data.</returns>
        public static object? DeserializeFromFile(Type type, string filename)
        {
            return Deserialize(type, IniFile.Read(filename));
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
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <param name="collectionSettings">Settings to use for converting collections.</param>
        /// <param name="converter">Type converter to use to convert to type.</param>
        /// <returns>The converted data.</returns>
        public static T? DeserializeFromFile<T>(string filename, IniCollectionSettings collectionSettings, TypeConverter? converter)
        {
            return Deserialize<T>(IniFile.Read(filename), collectionSettings, converter);
        }

        /// <summary>
        /// Converts Ini data from a file to an object of specified type.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <param name="collectionSettings">Settings to use for converting collections.</param>
        /// <returns>The converted data.</returns>
        public static T? DeserializeFromFile<T>(string filename, IniCollectionSettings collectionSettings)
        {
            return Deserialize<T>(IniFile.Read(filename), collectionSettings);
        }

        /// <summary>
        /// Converts Ini data from a file to an object of specified type.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <param name="converter">Type converter to use to convert to type.</param>
        /// <returns>The converted data.</returns>
        public static T? DeserializeFromFile<T>(string filename, TypeConverter? converter)
        {
            return Deserialize<T>(IniFile.Read(filename), converter);
        }

        /// <summary>
        /// Converts Ini data from a file to an object of specified type.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="filename">Path to the ini file that should be converted.</param>
        /// <returns>The converted data.</returns>
        public static T? DeserializeFromFile<T>(string filename)
        {
            return Deserialize<T>(IniFile.Read(filename));
        }

        #endregion

    }
}
