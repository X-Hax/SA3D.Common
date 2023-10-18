using System;
using System.Collections.Generic;
using static SA3D.Common.Ini.Deserialization.Deserialize;
using static SA3D.Common.Ini.IniSerializeHelper;

namespace SA3D.Common.Ini.Deserialization
{
    internal class DictionaryDeserializer<TKey, TValue> : ICollectionDeserializer
    {
        public void Deserialize(object listObj, IniGroup group, string groupName, IniCollectionSettings collectionSettings, string name, IniDictionary ini, string fullname)
        {
            Type keytype = typeof(TKey);
            Type valuetype = typeof(TValue);
            IDictionary<TKey, TValue> list = (IDictionary<TKey, TValue>)listObj;

            if(collectionSettings.Mode == IniCollectionMode.SingleLine)
            {
                throw new InvalidOperationException("Cannot deserialize IDictionary<TKey, TValue> with IniCollectionMode.SingleLine!");
            }

            List<string> items = new();

            if(!valuetype.IsComplexType(collectionSettings.ValueConverter))
            {
                foreach(IniNameValue item in group)
                {
                    string? value = collectionSettings.Mode.IndexFromName(name, item.Key);
                    if(value == null)
                    {
                        continue;
                    }

                    items.Add(value);
                }

                foreach(string item in items)
                {
                    if(keytype.ConvertFromString(item, collectionSettings.KeyConverter) is not TKey key)
                    {
                        throw new InvalidCastException("Failed to convert key from string");
                    }

                    string valueString = collectionSettings.Mode.IndexToName(name, item);
                    if(valuetype.ConvertFromString(valueString, collectionSettings.ValueConverter) is not TValue value)
                    {
                        throw new InvalidCastException("Failed to convert value from string");
                    }

                    list.Add(key, value);
                    group.Remove(valueString);
                }
            }
            else
            {
                foreach(IniNameGroup item in ini)
                {
                    string? value = collectionSettings.Mode.IndexFromName(name, item.Key);
                    if(value == null)
                    {
                        continue;
                    }

                    items.Add(value);
                }

                foreach(string item in items)
                {
                    if(keytype.ConvertFromString(item, collectionSettings.KeyConverter) is not TKey key)
                    {
                        throw new InvalidCastException("Failed to convert key from string");
                    }

                    string valueString = collectionSettings.Mode.IndexToName(name, item);
                    if(DeserializeInternal("value", valuetype, valuetype.GetDefaultValue(), ini, valueString, true, _defaultCollectionSettings, collectionSettings.ValueConverter)
                        is not TValue value)
                    {
                        throw new InvalidCastException("Failed to convert value from string");
                    }

                    list.Add(key, value);
                    group.Remove(valueString);
                }
            }
        }

    }
}
