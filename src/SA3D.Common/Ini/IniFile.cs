using System;
using System.Collections.Generic;
using System.IO;

namespace SA3D.Common.Ini
{
	/// <summary>
	/// Used to write and read Ini dictionaries from strings
	/// </summary>
	public static class IniFile
	{
		#region Loading

		/// <summary>
		/// Reads a string enumerator and converts them to an Ini dictionary.
		/// </summary>
		/// <param name="contents">String enumerator holding the contents.</param>
		/// <returns>The converted contents.</returns>
		public static IniDictionary Read(IEnumerable<string> contents)
		{
			IniDictionary result = [];
			IniGroup current = [];

			result.Add(string.Empty, current);

			string curgroup = string.Empty;
			int lineNumber = 0;
			foreach(string rawLine in contents)
			{
				lineNumber++;
				string line = System.Text.RegularExpressions.Regex.Unescape(rawLine);

				bool startswithbracket = false;
				int firstequals = -1;
				int endbracket = -1;

				for(int c = 0; c < line.Length; c++)
				{
					switch(line[c])
					{
						case '=':
							if(firstequals == -1)
							{
								firstequals = c;
							}

							break;
						case '[':
							if(c == 0)
							{
								startswithbracket = true;
							}

							break;
						case ']':
							endbracket = c;
							break;
						case ';': // comment character
							line = line[..c];
							break;
						default:
							break;
					}
				}

				if(startswithbracket & endbracket != -1)
				{
					curgroup = line[1..endbracket];
					current = [];
					try
					{
						result.Add(curgroup, current);
					}
					catch(ArgumentException ex)
					{
						throw new InvalidDataException($"INI File error: Group \"{curgroup}\" already exists.\nline {lineNumber}", ex);
					}
				}
				else if(!string.IsNullOrWhiteSpace(line))
				{
					string key;
					string value = string.Empty;
					if(firstequals > -1)
					{
						key = line[..firstequals];
						value = line[(firstequals + 1)..];
					}
					else
					{
						key = line;
					}

					try
					{
						current.Add(key, value);
					}
					catch(ArgumentException ex)
					{
						throw new InvalidDataException($"INI File error: Value \"{key}\" already exists in group \"{curgroup}\".\nline {lineNumber}", ex);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Reads given strings and converts them to an Ini dictionary.
		/// </summary>
		/// <param name="contents">Content to read.</param>
		/// <returns>The converted contents.</returns>
		public static IniDictionary Read(params string[] contents)
		{
			return Read((IEnumerable<string>)contents);
		}

		/// <summary>
		/// Reads lines of a stream and converts them to an Ini dictionary.
		/// </summary>
		/// <param name="stream">The Stream to read from.</param>
		/// <returns>The converted dictionary.</returns>
		public static IniDictionary ReadFromStream(Stream stream)
		{
			using(StreamReader reader = new(stream))
			{
				return Read(reader.StreamReaderAsLineEnumerable());
			}
		}

		/// <summary>
		/// Reads the lines of a file and converts them to an Ini dictionary.
		/// </summary>
		/// <param name="filepath">Path to the file to read.</param>
		/// <returns>The converted file.</returns>
		public static IniDictionary ReadFromFile(string filepath)
		{
			using(FileStream stream = File.OpenRead(filepath))
			{
				return ReadFromStream(stream);
			}
		}

		#endregion

		#region Writing

		/// <summary>
		/// Writes the contents of an Ini dictionary to a writer.
		/// </summary>
		/// <param name="ini">The Ini dictionary to write the contents of.</param>
		/// <param name="writer">The writer to write to.</param>
		public static void Write(this IniDictionary ini, TextWriter writer)
		{
			foreach(IniNameGroup group in ini)
			{
				if(!string.IsNullOrEmpty(group.Key))
				{
					writer.Write('[');

					writer.Write(group.Key
						.Replace(@"\", @"\\")
						.Replace("\n", @"\n")
						.Replace("\r", @"\r")
						.Replace(";", @"\;"));

					writer.WriteLine(']');
				}

				foreach(IniNameValue value in group.Value)
				{
					string escapedkey = value.Key
						.Replace(@"\", @"\\")
						.Replace("=", @"\=")
						.Replace("\n", @"\n")
						.Replace("\r", @"\r")
						.Replace(";", @"\;");

					if(escapedkey.StartsWith('['))
					{
						writer.Write(@"\");
					}

					writer.Write(escapedkey);
					writer.Write('=');

					writer.WriteLine(value.Value
						.Replace(@"\", @"\\")
						.Replace("\n", @"\n")
						.Replace("\r", @"\r")
						.Replace(";", @"\;"));
				}

				writer.WriteLine();
			}
		}

		/// <summary>
		/// Writes the contents of an Ini dictionary to a file.
		/// </summary>
		/// <param name="ini">The Ini dictionary to write the contents of.</param>
		/// <param name="filepath">Path of the file to write to</param>
		public static void WriteToFile(this IniDictionary ini, string filepath)
		{
			using(StreamWriter writer = File.CreateText(filepath))
			{
				ini.Write(writer);
			}
		}

		/// <summary>
		/// Converts the contents of an ini dictionary to a string array, where very string is a line as it would be written to a file.
		/// </summary>
		/// <param name="ini">The ini dictionary to convert the contents of.</param>
		/// <returns>The file lines.</returns>
		public static string[] WriteToFileLines(this IniDictionary ini)
		{
			using(StringWriter writer = new())
			{
				ini.Write(writer);
				return writer.ToString().Split(writer.NewLine);
			}
		}

		#endregion

		/// <summary>
		/// Combines two Ini dictionaries
		/// </summary>
		/// <param name="dictA"></param>
		/// <param name="dictB"></param>
		/// <returns></returns>
		public static IniDictionary Combine(IniDictionary dictA, IniDictionary dictB)
		{
			IniDictionary result = [];

			foreach(IniNameGroup group in dictA)
			{
				result.Add(group.Key, new(group.Value));
			}

			foreach(IniNameGroup group in dictB)
			{
				if(result.TryGetValue(group.Key, out IniGroup? value))
				{
					foreach(IniNameValue item in group.Value)
					{
						value[item.Key] = item.Value;
					}
				}
				else
				{
					result.Add(group.Key, new IniGroup(group.Value));
				}
			}

			return result;
		}


	}
}