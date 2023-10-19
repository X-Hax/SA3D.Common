using System.ComponentModel;

namespace SA3D.Common.Ini
{
	/// <summary>
	/// Ini Settings for a collection
	/// </summary>
	public class IniCollectionSettings
	{
		/// <summary>
		/// Serializer mode of the ini collection
		/// </summary>
		public IniCollectionMode Mode { get; }

		/// <summary>
		/// Format of the collection
		/// </summary>
		public string Format { get; set; }

		/// <summary>
		/// The index of the first item in the collection. Does not apply to Dictionary objects or <see cref="IniCollectionMode.SingleLine"/>.
		/// </summary>
		public int StartIndex { get; set; }

		/// <summary>
		/// A <see cref="TypeConverter"/> used to convert indexes/keys to and from <see cref="string"/>.
		/// </summary>
		public TypeConverter? KeyConverter { get; set; }

		/// <summary>
		/// A <see cref="TypeConverter"/> used to convert values to and from <see cref="string"/>.
		/// </summary>
		public TypeConverter? ValueConverter { get; set; }

		/// <summary>
		/// Creates new collection settings.
		/// </summary>
		/// <param name="mode">Serializer mode of the ini collection</param>
		public IniCollectionSettings(IniCollectionMode mode)
		{
			Mode = mode;
			Format = string.Empty;
		}
	}


}
