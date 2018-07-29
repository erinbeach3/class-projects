using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MutexDemo
{
	public enum RunState { Started, Paused, Stopped };

	public interface IPeriodicTask
	{
		void RunCycle();
	}

	public class PeriodicTaskRunner<T>
		where T : IPeriodicTask
	{
		private TimeSpan _interval;
		private RunState _state;
		private List<Thread> _threads;
		private ManualResetEvent _mre;
		private bool _quitting;
		private long _totalCycles;

		public PeriodicTaskRunner(Action<T> task, TimeSpan interval)
		{
			if (interval <= TimeSpan.Zero) throw new ArgumentException($"{nameof(interval)} must be > 0.");
			_interval = interval;
			_state = RunState.Stopped;
		}

		public PeriodicTaskRunner(Action<T> task, int milliSeconds): this(task, TimeSpan.FromMilliseconds(milliSeconds)) { }

		public RunState State => _state;
		public bool IsRunning => _state != RunState.Stopped;
		public bool IsPaused => _state == RunState.Paused;
		public long TotalCycles => Interlocked.Read(ref _totalCycles);

		public void Start(params T[] values)
		{
			if (values == null || values.Length == 0 || IsRunning) return;
			_threads = new List<Thread>(values.Length);
			_mre = new ManualResetEvent(true);
			_state = RunState.Started;
			for (int i = 0; i < values.Length; ++i)
			{
				ParameterizedThreadStart pts = new ParameterizedThreadStart(RunThread);
				Thread t = new Thread(pts)
				{
					Name = $"TaskRunnerThread{(i + 1)}",
					IsBackground = true
				};
				t.Start(values[i]);
				_threads.Add(t);
			}
		}

		public void Pause()
		{
			if (_state != RunState.Started) return;
			_mre.Reset();
			_state = RunState.Paused;
		}

		public void Resume()
		{
			if (!IsPaused) return;
			_state = RunState.Started;
			_mre.Set();
		}

		/// <summary>
		/// Stop all of the threads.
		/// </summary>
		/// <returns>An awaitable task to stop all threads managed by this task runner.</returns>
		/// <remarks>
		/// Stopping the threads may take as long as the assigned interval.
		/// This method will complete with a true value when all threads are confirmed as stopped.
		/// If stopping of any thread cannot be confirmed, the method will complete with false.
		/// </remarks>
		public Task<bool> Stop(bool useInterrupt = false)
		{
			Func<bool> func = EndThreads;
			if (useInterrupt) func = EndThreadsViaInterrupt;
			return Task<bool>.Factory.StartNew(func);
		}

		private bool EndThreadsViaInterrupt()
		{
			_threads.ForEach(t => t.Interrupt());
			return FinishAndCleanUp(_interval);
		}

		private bool EndThreads()
		{
			if (!IsRunning) return false;
			if (!IsPaused) _mre.Reset();
			_quitting = true;
			_mre.Set();
			TimeSpan timeout = _interval + TimeSpan.FromMilliseconds(10);
			return FinishAndCleanUp(timeout);
		}

		private bool FinishAndCleanUp(TimeSpan maxWait)
		{
			try
			{
				return _threads.All(t => t.Join(maxWait));
			}
			finally
			{
				_mre.Dispose();
				_mre = null;
				_threads = null;
				_state = RunState.Stopped;
				_quitting = false;
			}
		}

		private void RunThread(object state)
		{
			IPeriodicTask task = (IPeriodicTask)state;
			while(!_quitting)
			{
				task.RunCycle();
				Interlocked.Increment(ref _totalCycles);
				try
				{
					Thread.Sleep(_interval);
				}
				catch(ThreadInterruptedException)
				{
					return;
				}
				_mre.WaitOne();
			}
		}
	}
}
