using System;
using System.Collections.Generic;

namespace ClassExamples.Math
{
	public struct Statistics
	{
		private int n;
		private double min, max, mean, stddev;

		public Statistics(IEnumerable<double> values)
		{
			if (values == null) throw new ArgumentNullException(nameof(values));
			n = 0;
			min = int.MaxValue;
			max = int.MinValue;
			double sum = 0, sum2 = 0;
			foreach (double v in values)
			{
				n++;
				sum += v;
				sum2 += v * v;
				if (v < min) min = v;
				if (v > max) max = v;
			}
			mean = (n > 0) ? sum / n : 0;
			stddev = (n > 1) ? System.Math.Sqrt(sum2 - (sum * sum / n) / (n - 1)) : 0;
		}

		public Statistics(params double[] values): this((IEnumerable<double>)values) { }

		// Properties implemented using get syntax
		//public int N { get { return n; } }
		//public double Minimum { get { return min; } }
		//public double Maximum { get { return max; } }
		//public double Mean { get { return mean; } }
		//public double StandardDeviation { get { return stddev; } }

		// Properties implemented using lambda syntax:
		public int N => n;
		public double Minimum => min;
		public double Maximum => max;
		public double Mean => mean;
		public double StandardDeviation => stddev;

	}
}
