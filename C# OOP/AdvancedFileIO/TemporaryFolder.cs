using System;
using System.IO;

namespace AdvancedFileIO
{
	public sealed class TemporaryFolder : IDisposable
	{
		private static readonly Random _random = new Random();
		private string _folderPath;
		public TemporaryFolder()
		{
			string path = Path.GetTempPath();
			do
			{
				_folderPath = path + _random.Next().ToString();
				if (!Directory.Exists(_folderPath)) break;
			} while (true);
			Directory.CreateDirectory(_folderPath);
		}

		public string FolderPath
		{
			get
			{
				if (_folderPath == null)
					throw new ObjectDisposedException(nameof(TemporaryFolder));
				return _folderPath;
			}
		}

		public static implicit operator string(TemporaryFolder folder) => folder.FolderPath;

		public void Dispose()
		{
			if (_folderPath != null) DeleteFolder();
			_folderPath = null;
		}

		private void DeleteFolder()
		{
			try
			{
				Directory.Delete(_folderPath, true);
			}
			catch { }
		}
	}
}
