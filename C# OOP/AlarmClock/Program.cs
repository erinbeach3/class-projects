using System;
using System.Threading.Tasks;

namespace AlarmClock
{
	class Program
	{
		static bool _timesUp;
		static void Main(string[] args)
		{
			Alarm alarm = new Alarm(TimeSpan.FromSeconds(5), "Time's Up!");
			alarm.Elapsed += Alarm_Elapsed;
			alarm.Start();
			Console.Write("Alarm has been started ...");
			while(true)
			{
				if (_timesUp) return;
				Console.Write(".");
				Task.Delay(1000).Wait();
			}
		}

		private static void Alarm_Elapsed(object sender, string e)
		{
			Console.WriteLine(e);
			_timesUp = true;
		}
	}
}
