using System;

namespace SA3D.Common.Ini.Attributes
{
	/// <summary>
	/// Defines a custom Ini name for a property or field
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class IniNameAttribute : Attribute
	{
		/// <param name="name">The custom ini name to use</param>
		public IniNameAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Name of the property
		/// </summary>
		public string Name { get; }
	}
}
