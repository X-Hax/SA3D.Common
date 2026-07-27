namespace SA3D.Common.IO
{
	/// <summary>
	/// File context interface
	/// </summary>
	public interface IFileContext
	{
		/// <summary>
		/// Path to the file being read/written
		/// </summary>
		public string? Filepath { get; }
	}

	/// <summary>
	/// File context structure
	/// </summary>
	public struct FileContext : IFileContext
	{
		/// <inheritdoc/>
		public string? Filepath { get; set; }

		/// <summary>
		/// Create a file context from file path
		/// </summary>
		/// <param name="filepath">The filepath</param>
		public FileContext(string? filepath)
		{
			Filepath = filepath;
		}
	}

	/// <summary>
	/// File context structure with additional context
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public struct FileContext<T> : IFileContext where T : unmanaged
	{
		/// <inheritdoc/>
		public string? Filepath { get; set; }

		/// <summary>
		/// Additional context
		/// </summary>
		public T Context { get; set; }

		/// <summary>
		/// Create a file context from a file context without additional 
		/// </summary>
		/// <param name="context">Base context</param>
		public FileContext(FileContext context)
		{
			Filepath = context.Filepath;
			Context = default;
		}

		/// <summary>
		/// Create a file context from file path
		/// </summary>
		/// <param name="filepath">The filepath</param>
		/// <param name="context">Additional context</param>
		public FileContext(string? filepath, T context)
		{
			Filepath = filepath;
			Context = context;
		}
	}
}
