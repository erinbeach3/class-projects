using System;

namespace Methods
{
	class Program
	{
		static void Main(string[] args)
		{
			MethodOne("Pete Wilson", 57.5);
			MethodOne(57.5, "Pete Wilson");
			MethodTwo("Pete Wilson");						// use default value and ival
			MethodTwo("Pete Wilson", 57.5);			// specify value, use default ival
			MethodTwo("Pete Wilson", 57.5, 27);  // specify both value and ival

			// Call MethodTwo using named parameters:
			MethodTwo(value: 27.6, ival: 137, name: "Pete Wilson");

			// Call MethodThree with 0 or more values:
			MethodThree("Pete Wilson");
			MethodThree("Pete Wilson", 1.0);
			MethodThree("Pete Wilson", 1.0, 2.0, 10.0, 500.0);

			// Call MethodThree with an actual array of double:
			double[] values = new double[] { 1.0, 2.0, 3.0 };
			MethodThree("Pete Wilson", values);

			if (double.TryParse("abcd", out double d))
			{
				Console.WriteLine($"abcd parsed to {d}.");
			}
			else Console.WriteLine($"abcd cannot be parsed so d is {d}.");

			// v must be initialized before calling MethodFive
			double v = 15.0;	
			MethodFive(string.Empty, ref v);
			Console.WriteLine($"The new value of v is {v}.");
		}

		static void MethodOne(string name, double value)
		{
			Console.WriteLine("MethodOne called.");
		}

		// Overload of MethodOne: unique parameter list
		static void MethodOne(double value, string name)
		{
			Console.WriteLine("Overload of MethodOne called.");
		}

		// Example of an optional parameter
		static void MethodTwo(string name, double value = 10.0, int ival = 100)
		{
			Console.WriteLine($"MethodTwo called with value = {value} and ival = {ival}");
		}

		static void MethodThree(string name, params double[] values)
		{
			Console.WriteLine($"MethodThree called with values: {String.Join(",", values)}");
		}
		
		static void MethodFour(string name, out double d)
		{
			//if (DateTime.Now.Second > 30)
			//{
			//	double test = d;  // This is an error - we cannot assume d is initialized.
			//	return;           // This is an error - we MUST assign d a value before returning.
			//}
			d = double.PositiveInfinity;
		}

		static void MethodFive(string name, ref double d)
		{
			if (d > 0) d = Math.PI; else d = Math.E;
		}

		static void TryParse()
		{
			if (double.TryParse("13.8e-4", out double d))
			{
				Console.WriteLine($"double Value is: {d}");
			}
			if (int.TryParse("1217", out int i))
			{
				Console.WriteLine($"int value is {i}");
			}
		}

		void MethodX(double d) { }
		void MethodX(out double d) { d = 0; }
		//void MethodX(ref double d) { }
	}
}
