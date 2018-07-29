using System;
using System.Threading;

namespace Statics
{
	class Program
	{
		static void Main(string[] args)
		{
			StaticObserver so = new StaticObserver();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine($"The log file is: {Logger.LogPath}");
			Logger.Log("This is message #1");
			Thread.Sleep(1000);
			Logger.Log("This is message #2");
			Console.WriteLine();
			Console.WriteLine("The Log content is:");
			Console.WriteLine(Logger.ReadLog(true));
		}
	}
}
