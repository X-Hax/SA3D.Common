namespace SA3D.Common.Lookup
{
	/// <summary>
	/// Label interface.
	/// </summary>
	public interface ILabel
	{
		/// <summary>
		/// Label prefix for automatic label generation
		/// </summary>
		public string LabelPrefix { get; }

		/// <summary>
		/// Object label / C struct label
		/// </summary>
		public string Label { get; set; }
	}
}
