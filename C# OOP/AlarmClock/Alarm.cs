using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlarmClock
{
	public class Alarm : IDisposable
	{
		private TimeSpan duration = TimeSpan.Zero;
		private DateTime? startTime;
		private CancellationTokenSource ctSource = null;

		public Alarm(TimeSpan duration, string message = "")
		{
			if (duration.Ticks <= 0) throw new ArgumentNullException($"{nameof(duration)} must be > 0.");
			this.duration = duration;
			Message = message;
		}

		public bool IsRunning => startTime.HasValue;

		public string Message { get; set; }

		public TimeSpan Duration => duration;

		public TimeSpan ElapsedTime
		{
			get
			{
				if (!IsRunning) return TimeSpan.Zero;
				return DateTime.Now - startTime.Value;
			}
		}

		public TimeSpan RemainingTime => Duration - ElapsedTime;

		public event EventHandler<string> Elapsed;

		public void Start()
		{
			if (IsRunning) return;
			ctSource = new CancellationTokenSource();
			startTime = DateTime.Now;
			BeginWait();
		}

		public void Stop()
		{
			if (!IsRunning) return;
			ctSource.Cancel();
		}

		private async void BeginWait()
		{
			try
			{
				await Task.Delay(duration, ctSource.Token);
				Elapsed?.Invoke(this, Message);
			}
			catch(TaskCanceledException)
			{
				Console.WriteLine("The Alarm has been cancelled.");
			}
			finally
			{
				ctSource.Dispose();
				ctSource = null;
				startTime = null;
			}
		}

		void IDisposable.Dispose()
		{
			if (ctSource != null)
			{
				ctSource.Dispose();
				ctSource = null;
			}
		}

		~Alarm()
		{
			((IDisposable)this).Dispose();
		}
	}
}
