using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading
{
	class Program
	{
		/*
		 *	This application uses three different strategies to create four threads that independently update
		 *	four quadrants of the Console window.
	  */

		private static object _lock = new object();
		static void Main(string[] args)
		{
			Console.WriteLine();
			//Console.WriteLine($"# Processors: {Environment.ProcessorCount}");
			//InitThreads();
			//InitThreadsFromPool();
			//InitThreadsAsync();
			ParallelExample();
			Console.ReadLine();
		}

		private static void InitThreads()
		{
			for (int i = 0; i < Environment.ProcessorCount; ++i)
			{
				ParameterizedThreadStart pts = new ParameterizedThreadStart(RunThread);
				Thread thread = new Thread(pts);
				thread.Name = "Thread #" + (i + 1).ToString();
				thread.IsBackground = true;
				thread.Start(i + 1);
			}
		}

		private static void InitThreadsFromPool()
		{
			for(int i=0;i<4;++i)
			{
				WaitCallback cb = new WaitCallback(RunThread);
				ThreadPool.QueueUserWorkItem(cb, (i + 1));
			}
		}

		private static async void InitThreadsAsync()
		{
			List<Task> tasks = new List<Task>();
			for(int i=0;i<4;++i)
			{
				int v = i + 1;
				Task t = Task.Run(() =>
				{
					RunThread(v);
				});
				tasks.Add(t);
			}
			await Task.WhenAll(tasks.ToArray());
		}

		private static void RunThread(object state)
		{
			int nThread = (int)state;
			int xPos = 0, yPos = 0;
			switch(nThread)
			{
				case 1: xPos = 0;  yPos = 0; break;
				case 2:	xPos = Console.WindowWidth / 2; yPos = 0; break;
				case 3:	xPos = 0; yPos = Console.WindowHeight / 2; break;
				case 4:	xPos = Console.WindowWidth / 2; yPos = Console.WindowHeight / 2; break;
			}
			// We need each random generator to have a different seed, so sleep briefly before instantiating:
			Thread.Sleep(50);
			Random rand = new Random();
			long nLoops = 0;
			string blank = new string(' ', 30);
			while(true)
			{
				double nSeconds = rand.NextDouble() * 2;
				Thread.Sleep(TimeSpan.FromSeconds(3));
				lock(_lock)
				{
					if (nLoops == 0)
					{
						Console.SetCursorPosition(xPos, yPos);
						Console.Write($"{Thread.CurrentThread.Name} ({Thread.CurrentThread.ManagedThreadId})");
					}
					Console.SetCursorPosition(xPos, yPos + 2);
					Console.Write(blank);
					Console.SetCursorPosition(xPos, yPos + 2);
					Console.Write($"Loop {++nLoops}");
				}
			}
		}

		private static void ParallelExample()
		{
			Parallel.For(0, 20, (n) =>
			{
				Console.WriteLine($" {Thread.CurrentThread.ManagedThreadId}: {n}");
			});
		}
	}
}
