//#define ALTSYNTAX
using System;
using System.Collections.Generic;

namespace Delegates
{
	public class Accumulator
	{
		public Accumulator(double threshold)
		{
			if (threshold <= 0) throw new ArgumentException($"{nameof(threshold)} must be > 0.");
			Threshold = threshold;
		}

#if ALTSYNTAX

		private List<EventHandler> _teHandlers = new List<EventHandler>();
		public event EventHandler ThresholdExceeded
		{
			add { _teHandlers.Add(value); }
			remove { _teHandlers.Remove(value); }
		}

#else
		public event EventHandler ThresholdExceeded;
#endif
		public double Threshold { get; private set; }
		public double Sum { get; private set; }
		public int Count { get; private set; }
		public void Reset()
		{
			Sum = 0;
			Count = 0;
		}

		public void Accumulate(double amount)
		{
			Sum += amount;
			Count++;
			if (Sum > Threshold)
			{
#if ALTSYNTAX
				foreach (EventHandler eh in _teHandlers) eh.Invoke(this, EventArgs.Empty);
#else
				ThresholdExceeded?.Invoke(this, EventArgs.Empty);
#endif
				Reset();
			}
		}
	}
}
