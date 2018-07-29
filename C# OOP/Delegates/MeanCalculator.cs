using System;

namespace Delegates
{
	// Represents a method that calculates a mean value
	public delegate double MeanCalculationStrategy(double[] values);

	public class MeanCalculator
	{
		public MeanCalculator(MeanCalculationStrategy strategy)
		{
			Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
		}

		private MeanCalculationStrategy Strategy { get; set; }

		public double CalculateMean(params double[] values)
		{
			if (values == null || values.Length == 0) return 0;
			return Strategy(values);
		}
	}
}
