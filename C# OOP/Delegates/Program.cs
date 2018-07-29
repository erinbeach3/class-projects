using System;
using System.Linq;

namespace Delegates
{
	class Program
	{
		public delegate int Incrementer(int value);

		public delegate void KeyPressHandler(ConsoleKeyInfo pressed);

		public delegate double ValueTransform(double value);

		static void Main(string[] args)
		{
			//DemoEventHandler();
			//DemoMeanCalculator();
			DemoAccumulator();
		}

		static void DelegateAssignment()
		{
			ValueTransform vtx = Cube;
			double v25 = vtx(5);
		}

		static double Cube(double x)
		{
			return x * x * x;
		}

		public delegate T Comparer<T>(T tLeft, T tRight, Predicate<T> test);

		private static int Test(int i1, int i2, Predicate<int> test) 
		{
			return test(i1) ? i1 : i2;
		}

		private static double Test(double d1, double d2, Predicate<double> test)
		{
			return test(d1) ? d1 : d2;
		}

		public static void GenericDelegate()
		{
			Comparer<int> iCmp = Test;
			Comparer<double> dCmp = Test;
			iCmp(27, 52, (i) => i > 30);
			dCmp(Math.PI, Math.E, (d) => d > 3);
		}



		#region EventHandler

		private static void DemoEventHandler()
		{
			EventHandler handler = MyEventHandler;
			handler(new object(), EventArgs.Empty);
		}

		private static void MyEventHandler(object sender, EventArgs e)
		{
			Console.WriteLine($"{nameof(MyEventHandler)} invoked.");
		}

		#endregion

		#region MeanCalculator

		private static void DemoMeanCalculator()
		{
			MeanCalculator arithCalc = new MeanCalculator(ArithmeticMean);
			MeanCalculator geoCalc = new MeanCalculator(GeometricMean);
			MeanCalculator medianCalc = new MeanCalculator(Median);
			double[] values = new double[10];
			for (int i = 0; i < values.Length; ++i) values[i] = i + 1;
			Console.WriteLine("Arithmetic Mean: {0:F2}: ", arithCalc.CalculateMean(values));
			Console.WriteLine("Geometric Mean: {0:F2}", geoCalc.CalculateMean(values));
			Console.WriteLine("Median: {0:F2}", medianCalc.CalculateMean(values));
		}

		private static double ArithmeticMean(double[] values)
		{
			double sum = values.Sum();
			return sum / values.Length;
		}

		private static double GeometricMean(double[] values)
		{
			double product = 1;
			foreach (double v in values) product *= v;
			return Math.Pow(product, 1.0 / values.Length);
		}

		private static double Median(double[] values)
		{
			int mid = values.Length / 2;
			if (values.Length % 2 == 0) return (values[mid] + values[mid - 1]) / 2; else return values[mid];
		}

		#endregion

		#region Accumulator 

		static bool thresholdExceeded = false;
		private static void DemoAccumulator()
		{
			Accumulator a = new Accumulator(10000);
			a.ThresholdExceeded += HandleThresholdExceeded;

			a.ThresholdExceeded += (o, e) =>
			{
				// Handle the event locally
			};

			Random r = new Random();
			do
			{
				double v = 100 * (r.NextDouble() - 0.4);
				a.Accumulate(v);
			} while (!thresholdExceeded);
		}

		private static void HandleThresholdExceeded(object sender, EventArgs e)
		{
			Accumulator a = sender as Accumulator;
			Console.WriteLine($"Threshold exceeded after {a.Count} calls.");
			a.ThresholdExceeded -= HandleThresholdExceeded;
			thresholdExceeded = true;
		}

		#endregion

	}
}
