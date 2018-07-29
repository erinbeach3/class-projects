using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine(ThreadMsg(1));
			ReturnsFast();
			Console.WriteLine(ThreadMsg(2));
			Console.ReadKey();
		}

		static string ThreadMsg(int n) =>
			$"  {n}: {Thread.CurrentThread.ManagedThreadId} "+
			$"{Thread.CurrentThread.IsBackground}";

		private static async void ReturnsFast()
		{
			int testFunc()
			{
				Thread.Sleep(1000);
				Console.WriteLine(ThreadMsg(3));
				return 250;
			}
			int value = await Task<int>.Factory.StartNew(testFunc);
			Console.WriteLine(ThreadMsg(4));
		}
	}
}
