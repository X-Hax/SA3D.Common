using System;

namespace SA3D.Common.Ini.Attributes
{
	/// <summary>
	/// Marks a property or field to always be included
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class IniAlwaysIncludeAttribute : Attribute { }
}
