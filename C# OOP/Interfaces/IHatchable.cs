using System;
using System.Threading;

namespace Interfaces
{
	public interface IHatchable
	{
		int Count { get; set; }
		void Incubate(TimeSpan duration);
		event EventHandler<int> Hatch;
	}

	public interface IFlyable : IHatchable
	{
		bool CanItFly { get; }
		bool WillItFly { get; }
		void TryToFly();
		void Soar(double howHigh);
	}

	public class Egg : IHatchable
	{
		public int Count { get; set; }

		public event EventHandler<int> Hatch;

		void IHatchable.Incubate(TimeSpan duration)
		{
			Thread.Sleep(duration);
			Hatch?.Invoke(this, Count);
		}
	}

	public class Scheme { /* TODO */ }

	public class GrandScheme : Scheme //, IHatchable
	{
	}

}
