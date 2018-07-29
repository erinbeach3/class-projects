using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace MutexDemo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private enum RunState
		{
			Stopped, Running, Paused
		}

		const int minWaitTime = 100, maxWaitTime = 500;
		private ManualResetEvent mre;
		private RunState state;
		private long totalCycles;
		private bool quit;
		private List<Thread> threads;
		private Random random = new Random();
		public MainWindow()
		{
			InitializeComponent();
		}

		private void startstop_Click(object sender, RoutedEventArgs e)
		{
			switch(state)
			{
				case RunState.Stopped:	Start(); break;
				case RunState.Running:	Pause(); break;
				case RunState.Paused:	Resume(); break;
			}
		}

		private void reset_Click(object sender, RoutedEventArgs e)
		{
			Reset();
		}

		private void Start()
		{
			const int maxThreads = 4;
			mre = new ManualResetEvent(true);
			quit = false;
			totalCycles = 0;
			startstop.Content = "Pause";
			reset.IsEnabled = true;
			state = RunState.Running;
			threads = new List<Thread>(maxThreads);
			for(int i=0;i<maxThreads;++i)
			{
				ParameterizedThreadStart pts = new ParameterizedThreadStart(RunThread);
				Thread t = new Thread(pts);
				t.Name = string.Concat("Thread", (i + 1));
				t.IsBackground = true;	// Closing app will automatically shut down the threads.
				t.Start(i);
				threads.Add(t);
			}
		}

		private void Pause()
		{
			mre.Reset();
			state = RunState.Paused;
			startstop.Content = "Resume";
		}

		private void Resume()
		{
			state = RunState.Running;
			mre.Set();
			startstop.Content = "Pause";
		}

		private object tLock = new object();
		private Random CreateRandom()
		{
			// Random is not thread-safe, so we must access it within a lock:
			lock(tLock)
			{
				return new Random(random.Next());
			}
		}

		private void UpdateQuadrant(int quadrant, double sum, int nCycles)
		{
			QuadrantView quadrantView = null;
			switch(quadrant)
			{
				case 0:	quadrantView = quad1; break;
				case 1:	quadrantView = quad2; break;
				case 2:	quadrantView = quad3; break;
				case 3:	quadrantView = quad4; break;
			}
			void doUpdate()	// local method
			{
				status.Text = $"{Interlocked.Read(ref totalCycles)} Cycles Completed";
				quadrantView.UpdateProgress(sum, nCycles);
			}
			// We cannot "touch" UI objects directly from a non-UI thread.
			// Dispatcher.Invoke will do the update on the UI thread.
			Dispatcher.Invoke(doUpdate);
		}

		private void Reset()
		{
			startstop.IsEnabled = false;
			reset.IsEnabled = false;
			// Make all threads wait, then release them with quit = true:
			mre.Reset();
			void cleanUp()
			{
				mre.Close();
				mre = null;
				threads = null;
				state = RunState.Stopped;
				startstop.Content = "Start";
				startstop.IsEnabled = true;
				status.Text = $"Threads reset after {totalCycles} cycles.";
				totalCycles = 0;
			}
			// Dispatcher timer will not block the UI thread:
			DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromMilliseconds(maxWaitTime), DispatcherPriority.Background, (o, e) => 
			{
				((DispatcherTimer)o).Stop();
				quit = true;
				mre.Set();
				threads.ForEach(t => t.Join());
				cleanUp();
			}, Dispatcher);
			timer.Start();
		}

		#region Thread Method

		private void RunThread(object quad)
		{
			int quadrant = (int)quad;
			Random rand = CreateRandom();
			double increment = 5 * rand.NextDouble();
			double sum = 0;
			const double maxSum = 100;
			int nCycles = 0;
			while(!quit)
			{
				sum += increment;
				if (sum >= maxSum)
				{
					sum = maxSum;
					increment = -increment;
				} else
				if (sum <= 0)
				{
					sum = 0;
					increment = -increment;
				}
				nCycles++;
				Interlocked.Increment(ref totalCycles);
				UpdateQuadrant(quadrant, sum, nCycles);
				Thread.Sleep(rand.Next(minWaitTime, maxWaitTime));
				mre.WaitOne();
			}
			System.Diagnostics.Debug.WriteLine($"{Thread.CurrentThread.Name} is exiting.");
		}

		#endregion
	}
}
