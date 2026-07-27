namespace SA3D.Common.Ascii
{
	/// <summary>
	/// Interface for de/serialization ascii data
	/// </summary>
	public interface IAsciiSerializable
	{
		/// <summary>
		/// Writes the data to the ascii writer
		/// </summary>
		/// <param name="writer"></param>
		public void Write(AsciiWriter writer);
	}

	/// <summary>
	/// Interface for de/serialization ascii data
	/// </summary>
	public interface IAsciiSerializable<C>
	{
		/// <summary>
		/// Writes the data to the ascii writer
		/// </summary>
		/// <param name="writer">Writer to write with</param>
		/// <param name="context">Context to write with</param>
		public void Write(AsciiWriter writer, C context);
	}
}
