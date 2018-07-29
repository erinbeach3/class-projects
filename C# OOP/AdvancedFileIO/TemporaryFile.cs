using System;
using System.IO;

namespace AdvancedFileIO
{
	public class TemporaryFile : IDisposable
	{
		private string _filePath;
		public TemporaryFile()
		{
			_filePath = Path.GetTempFileName();
		}

		public string FilePath
		{
			get
			{
				if (string.IsNullOrEmpty(_filePath))
					throw new ObjectDisposedException(nameof(TemporaryFile));
				return _filePath;
			}
		}

		public static implicit operator string(TemporaryFile tf) => tf.FilePath;

		public void Dispose()
		{
			if (File.Exists(FilePath))
			{
				try
				{
					File.Delete(FilePath);
				}
				catch { }
				finally { _filePath = null; }
			}
		}
	}
}
