using System;
using System.Collections.Generic;
using System.Windows;

namespace MutexDemo
{
	/// <summary>
	/// Interaction logic for MainWindow2.xaml
	/// </summary>
	public partial class MainWindow2 : Window
	{
		private List<TaskData> _tasks = new List<TaskData>(4);
		private PeriodicTaskRunner<TaskData> _taskRunner;
		public MainWindow2()
		{
			InitializeComponent();
			Random rand = new Random();
			for (int i = 0; i < 4; ++i) _tasks.Add(new TaskData(this, i, new Random(rand.Next())));
			_taskRunner = new PeriodicTaskRunner<TaskData>(RunTask, 500);
		}

		private void startstop_Click(object sender, RoutedEventArgs e)
		{
			switch(_taskRunner.State)
			{
				case RunState.Stopped:
					_tasks.ForEach(t => t.Reset());
					_taskRunner.Start(_tasks.ToArray());
					startstop.Content = "Pause";
					reset.IsEnabled = true;
					break;
				case RunState.Started:
					_taskRunner.Pause();
					startstop.Content = "Resume";
					break;
				case RunState.Paused:
					_taskRunner.Resume();
					startstop.Content = "Pause";
					break;
			}
		}

		private async void reset_Click(object sender, RoutedEventArgs e)
		{
			startstop.IsEnabled = false;
			reset.IsEnabled = false;
			bool result = await _taskRunner.Stop(useInterrupt.IsChecked == true);
			if (result)
			{
				startstop.Content = "Start";
				startstop.IsEnabled = true;
				status.Text = $"Threads reset after {_taskRunner.TotalCycles:N0} cycles.";
			} else
			{
				status.Text = "An error occurred.  Please restart the application.";
			}
		}

		private void UpdateQuadrant(int quadrant, double sum, int nCycles)
		{
			QuadrantView qv = null;
			switch (quadrant)
			{
				case 0: qv = quad1; break;
				case 1: qv = quad2; break;
				case 2: qv = quad3; break;
				case 3: qv = quad4; break;
			}
			void doUpdate()
			{
				status.Text = $"{_taskRunner.TotalCycles} Cycles Completed";
				qv.UpdateProgress(sum, nCycles);
			}
			// We cannot "touch" UI objects directly from a non-UI thread.
			// Dispatcher.Invoke will do the update on the UI thread.
			Dispatcher.Invoke(doUpdate);
		}


		public class TaskData : IPeriodicTask
		{
			private MainWindow2 _window;
			public int Index { get; private set; }
			public int NCycles { get; private set; }
			public Random Random { get; private set; }
			public double Increment { get; private set; }
			public double Sum { get; private set; }
			private const double MaxSum = 100;

			public TaskData(MainWindow2 window, int index, Random r)
			{
				_window = window;
				Index = index;
				Random = r;
			}

			public void Reset()
			{
				NCycles = 0;
				Increment = 5 * Random.NextDouble();
				Sum = 0;
			}

			public void RunCycle()
			{
				Sum += Increment;
				if (Sum >= MaxSum)
				{
					Sum = MaxSum;
					Increment *= -1;
				}
				if (Sum <= 0)
				{
					Sum = 0;
					Increment *= -1;
				}
				NCycles++;
				_window.UpdateQuadrant(Index, Sum, NCycles);
			}
		}

		private void RunTask(TaskData data)
		{
			data.RunCycle();
		}
	}
}
