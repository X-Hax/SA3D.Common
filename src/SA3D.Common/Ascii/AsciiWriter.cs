using SA3D.Common.Lookup;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA3D.Common.Ascii
{
	/// <summary>
	/// 
	/// </summary>
	public class AsciiWriter
	{
		private readonly StringBuilder _stringBuilder;
		private readonly HashSet<string> _writtenlabels;


		/// <summary>
		/// Creates a new Ascii writer
		/// </summary>
		public AsciiWriter()
		{
			_stringBuilder = new();
			_writtenlabels = [];
		}


		/// <summary>
		/// Writes a string to the writer
		/// </summary>
		/// <param name="value"></param>
		public void Write(string value)
		{
			_stringBuilder.Append(value);
		}

		/// <summary>
		/// Writes an empty line to the writer
		/// </summary>
		public void WriteLine()
		{
			_stringBuilder.AppendLine();
		}

		/// <summary>
		/// Writes a line to the writer
		/// </summary>
		/// <param name="value">The line to write</param>
		public void WriteLine(string value)
		{
			_stringBuilder.AppendLine(value);
		}

		/// <summary>
		/// Writes empty lines to the writer
		/// </summary>
		/// <param name="newlines">The number of newlines to append after the value</param>
		public void WriteLine(int newlines)
		{
			for(int i = 0; i < newlines; i++)
			{
				_stringBuilder.AppendLine();
			}
		}

		/// <summary>
		/// Writes a line to the writer
		/// </summary>
		/// <param name="value">The line to write</param>
		/// <param name="newlines">The number of newlines to append after the value</param>
		public void WriteLine(string value, int newlines)
		{
			_stringBuilder.Append(value);
			for(int i = 0; i < newlines; i++)
			{
				_stringBuilder.AppendLine();
			}
		}


		private string GetLabel(ILabel label)
		{
			return label.Label.MakeIdentifier();
		}

		private bool SetupLabel(ILabel label)
		{
			return _writtenlabels.Add(GetLabel(label));
		}

		private string GetCheckLabel(ILabel? label)
		{
			if(label == null)
			{
				return "NULL";
			}

			string cString = GetLabel(label);

			if(!_writtenlabels.Contains(cString))
			{
				throw new InvalidOperationException($"No object with the name \"{label.Label}\" has been written yet!");
			}

			return cString;
		}


		/// <summary>
		/// Writes an object to the writer
		/// </summary>
		/// <param name="data">The data to write</param>
		public void WriteObject(IAsciiSerializable? data)
		{
			if(data == null || (data is ILabel label && !SetupLabel(label)))
			{
				return;
			}

			data.Write(this);
		}

		/// <summary>
		/// Writes an object to the writer
		/// </summary>
		/// <param name="data">The data to write</param>
		/// <param name="context">The context to write with</param>
		public void WriteObject<C>(IAsciiSerializable<C>? data, C context)
		{
			if(data == null || (data is ILabel label && !SetupLabel(label)))
			{
				return;
			}

			data.Write(this, context);
		}

		/// <summary>
		/// Writes an array to the writer
		/// </summary>
		/// <typeparam name="T">The array element type</typeparam>
		/// <param name="type">Type of the array</param>
		/// <param name="array">The array</param>
		public void WriteArray<T>(string type, LabeledArray<T>? array) where T : IAsciiSerializable
		{
			if(array == null || !SetupLabel(array))
			{
				return;
			}

			using(WriteStructBlock(type, array))
			{
				foreach(T item in array)
				{
					item.Write(this);
				}
			}
		}

		/// <summary>
		/// Writes an array to the writer
		/// </summary>
		/// <typeparam name="T">The array element type</typeparam>
		/// <typeparam name="C">The context type</typeparam>
		/// <param name="type">Type of the array</param>
		/// <param name="array">The array</param>
		/// <param name="context">The context to write with</param>
		public void WriteArray<T, C>(string type, LabeledArray<T> array, C context) where T : IAsciiSerializable<C>
		{
			if(array == null || !SetupLabel(array))
			{
				return;
			}

			using(WriteStructBlock(type, array))
			{
				foreach(T item in array)
				{
					item.Write(this, context);
				}
			}
		}


		/// <summary>
		/// Writes a property name with its value
		/// </summary>
		/// <param name="propertyName">Name of the property</param>
		/// <param name="value">Value of the property</param>
		/// <param name="comma">Writes a comma at the end of the property line</param>
		public void WritePropertyLine(string propertyName, string value, bool comma = true)
		{
			string padding = " ";
			if(propertyName.Length < 12)
			{
				padding = new string(' ', 12 - propertyName.Length);
			}

			WriteLine($"{propertyName}{padding}{value}{(comma ? "," : string.Empty)}");
		}

		/// <summary>
		/// Writes a property name with its value
		/// </summary>
		/// <param name="propertyName">Name of the property</param>
		/// <param name="obj">Object to link to the property</param>
		public void WriteObjectPropertyLine(string propertyName, ILabel? obj)
		{
			WritePropertyLine(propertyName, GetCheckLabel(obj));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="endNewlineCount"></param>
		/// <returns></returns>
		public AsciiWriterBlockToken WriteBlock(string? prefix = null, int endNewlineCount = 2)
		{
			return new AsciiWriterBlockToken(this, prefix, endNewlineCount);
		}

		/// <summary>
		/// Writes an encapsulating struct block with an object definition line at the start
		/// </summary>
		/// <param name="type">Type of the struct</param>
		/// <param name="obj">Object with the label</param>
		/// <returns></returns>
		public AsciiWriterBlockToken WriteStructBlock(string type, ILabel obj)
		{
			WritePropertyLine(type, GetLabel(obj) + "[]", false);
			return WriteBlock();
		}

		/// <summary>
		/// Writes an encapsulating struct block with an object definition line at the start.
		/// <br/>Returns null if object was already written 
		/// </summary>
		/// <param name="type">Type of the struct</param>
		/// <param name="obj">Object with the label</param>
		/// <returns></returns>
		public AsciiWriterBlockToken? WriteStructBlockWithReference(string type, ILabel obj)
		{
			if(!SetupLabel(obj))
			{
				return null;
			}

			return WriteStructBlock(type, obj);
		}

		/// <summary>
		/// Writes an encapsulating object block
		/// </summary>
		/// <param name="type">Object type to write</param>
		/// <returns></returns>
		public AsciiWriterBlockToken WriteObjectBlock(string type)
		{
			AsciiWriterBlockToken result = WriteBlock(type + "_", 4);
			WriteLine();
			return result;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return _stringBuilder.ToString();
		}
	}
}
