using System;

namespace SA3D.Common.Ini
{
    /// <summary>
    /// Extension methods for <see cref="IniCollectionMode"/>.
    /// </summary>
    public static class IniCollectionModeExtensions
    {
        /// <summary>
        /// Converts an index to an ini qualified name.
        /// </summary>
        /// <param name="mode">Conversion mode.</param>
        /// <param name="name">Name of the collection.</param>
        /// <param name="index">Index to the element of the collection.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string IndexToName(this IniCollectionMode mode, string name, string index)
        {
            string format = mode switch
            {
                IniCollectionMode.Normal => "{0}[{1}]",
                IniCollectionMode.IndexOnly => "{1}",
                IniCollectionMode.NoSquareBrackets => "{0}{1}",
                IniCollectionMode.SingleLine or _ => throw new InvalidOperationException($"{mode} mode not mode not supporting name formatting!"),
            };
            return string.Format(format, name, index);
        }

        /// <summary>
        /// Converts an ini qualified name to an index.
        /// </summary>
        /// <param name="mode">Conversion mode.</param>
        /// <param name="name">Name of the collection.</param>
        /// <param name="full">Full element reference.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string? IndexFromName(this IniCollectionMode mode, string name, string full)
        {
            switch(mode)
            {
                case IniCollectionMode.Normal:
                    if(!full.StartsWith($"{name}["))
                    {
                        return null;
                    }

                    return full.Substring(name.Length + 1, full.Length - (name.Length + 2));

                case IniCollectionMode.IndexOnly:
                    if(string.IsNullOrEmpty(full))
                    {
                        return null;
                    }

                    return full;

                case IniCollectionMode.NoSquareBrackets:
                    if(!full.StartsWith(name))
                    {
                        return null;
                    }

                    return full[name.Length..];
                case IniCollectionMode.SingleLine:
                default:
                    throw new InvalidOperationException($"{mode} mode not supporting name formatting!");
            }

        }
    }
}
