using System;
using System.Configuration;
using System.IO;

namespace Statics
{
	public static class Logger
	{
		static Logger()
		{
			// Notice that ConfigurationManager is a static class.
			string logFileName = ConfigurationManager.AppSettings["LogFileName"];
			string folderName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			logPath = Path.Combine(folderName, logFileName);
			Log("******************** Begin Logging *******************");
		}

		private static readonly string logPath;

		private static readonly object fileLock = new object();

		public static string LogPath => logPath;

		public static void Log(string message)
		{
			lock(fileLock)
			{
				using(FileStream file = new FileStream(logPath, FileMode.OpenOrCreate))
				{
					file.Seek(0, SeekOrigin.End);	// goto end of file.
					using (StreamWriter w = new StreamWriter(file) { AutoFlush = true }) w.WriteLine(string.Join("\t", TimeStamp, message));
				}
			}
		}

		public static string ReadLog(bool clear)
		{
			lock(fileLock)
			{
				string r = File.ReadAllText(logPath);
				if (clear) File.Delete(logPath);
				return r;
			}
		}

		private static string TimeStamp => DateTime.Now.ToString("MM:dd:yyyy hh:mm:ss");
	}
}
