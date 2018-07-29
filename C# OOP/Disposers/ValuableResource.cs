using System;
using System.IO;

namespace Disposers
{
	class ValuableResource : IDisposable
	{
		private string _filepath;
		private FileStream _file;
		public ValuableResource(string fpath)
		{
			_filepath = fpath;
			if (File.Exists(fpath)) File.Delete(fpath);
			// open file:
			_file = new FileStream(fpath, FileMode.Create);
		}

		// Finalizer
		~ValuableResource()
		{
			Console.WriteLine("Finalizing ValuableResource: {0}", _filepath);
			Dispose(false);
		}

		public void Write(string message)
		{
			if (IsDisposed)
				throw new ObjectDisposedException("Attempted to write using a disposed ValuableResource");
			StreamWriter w = new StreamWriter(_file);
			w.WriteLine(message);
			w.Flush();
		}

		// Implements the Dispose pattern
		protected virtual void Dispose(bool disposing)
		{
			if (_file == null) return;
			if (disposing)
			{
				_file.Close();
				_file = null;
			}
			// Here we would free unmanaged resources such as memory pointers
		}

		// Implements IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
			IsDisposed = true;
		}

		public bool IsDisposed { get; private set; }
	}
}
