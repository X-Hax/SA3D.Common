using J113D.Json;

namespace SA3D.Common.Lookup
{
	/// <summary>
	/// Label interface.
	/// </summary>
	public interface ILabel : IReferenceable
	{
		/// <summary>
		/// Label prefix for automatic label generation
		/// </summary>
		public string LabelPrefix { get; }

		/// <summary>
		/// Object label / C struct label
		/// </summary>
		public string Label { get; set; }

		string IReferenceable.ReferenceName
		{
			get => Label;
			set => Label = value;
		}
	}
}
