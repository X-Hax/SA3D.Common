namespace SA3D.Common.Ini
{
    /// <summary>
    /// How an ini collection is serialized
    /// </summary>
    public enum IniCollectionMode
    {
        /// <summary>
        /// The collection is serialized normally.
        /// </summary>
        Normal,

        /// <summary>
        /// The collection is serialized using only the index in the ini entry's key.
        /// </summary>
        IndexOnly,

        /// <summary>
        /// The collection is serialized using the collection's name and index in the ini entry's key, with no square brackets.
        /// </summary>
        NoSquareBrackets,

        /// <summary>
        /// <see cref="IniCollectionSettings.Format"/> property is used with <seealso cref="string.Join(string?, string?[])"/> to create the ini entry's value. The key is the collection's name.
        /// </summary>
        SingleLine
    }
}
