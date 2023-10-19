namespace SA3D.Common.Ini.Deserialization
{
	internal interface ICollectionDeserializer
	{
		public abstract void Deserialize(
			object listObj,
			IniGroup group,
			string groupName,
			IniCollectionSettings collectionSettings,
			string name,
			IniDictionary ini,
			string fullname);
	}
}
