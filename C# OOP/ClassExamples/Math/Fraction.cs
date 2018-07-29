using System;

namespace ClassExamples.Math
{
	public class Fraction
	{
		public static readonly Fraction Unity = new Fraction(1, 1);
		public static readonly Fraction Zero = new Fraction(0, 1);
		public static readonly Fraction MaxValue = new Fraction(int.MaxValue, 1);
		public static readonly Fraction MinValue = new Fraction(int.MinValue, 1);

		private int numerator;
		private int denominator;

		public Fraction(int numerator, int denominator)
		{
			if (denominator == 0) throw new ArgumentException("denominator cannot be zero");
			this.numerator = numerator;
			this.denominator = denominator;
		}

		public Fraction(Fraction fraction)
		{
			if (fraction == null) throw new ArgumentNullException(nameof(fraction));
			numerator = fraction.numerator;
			denominator = fraction.denominator;
		}

		public int Numerator { get { return numerator; } }

		public int Denominator { get { return denominator; } }

		public double Ratio => (double)numerator / denominator;

		public Fraction Negation => new Fraction(-Numerator, Denominator);

		public Fraction Reciprocal
		{
			get
			{
				if (Numerator == 0) throw 
						new InvalidOperationException($"Cannot take reciprocal of {this.ToString()} because the numerator is zero.");
				return new Fraction(Denominator, Numerator);
			}
		}

		public override string ToString()
		{
			return $"{Numerator} / {Denominator}";
		}

		public override bool Equals(object obj)
		{
			Fraction f = obj as Fraction;
			if (f is null) return false;
			// NOTE:  avoid comparing floating point types
			return (numerator * f.denominator == denominator * f.numerator);
		}

		// When we override Equals, the compiler generates a warning
		// if we do not also override GetHashCode.
		public override int GetHashCode()
		{
			return Ratio.GetHashCode();
		}

		public static Fraction operator + (Fraction frA, Fraction frB)
		{
			if (frA == null || frB == null) return null;
			int lcm = FindLeastCommonMultiple(frA.Denominator, frB.Denominator);
			int num1 = frA.Numerator * lcm / frA.Denominator,
				num2 = frB.Numerator * lcm / frB.Denominator;
			return new Fraction(num1 + num2, lcm);
		}

		public static Fraction operator - (Fraction frA, Fraction frB)
		{
			return frA + frB.Negation;
		}

		public static Fraction operator * (Fraction frA, Fraction frB)
		{
			if (frA == null || frB == null) return null;
			return new Fraction(frA.Numerator * frB.Numerator, frA.Denominator * frB.Denominator);
		}

		public static Fraction operator / (Fraction frA, Fraction frB)
		{
			if (frA == null || frB == null) return null;
			return frA * frB.Reciprocal;
		}

		public static bool operator == (Fraction frA, Fraction frB)
		{
			// WARNING:  do not use == within this method.
			if (ReferenceEquals(frA, frB)) return true;
			if (frA is null || frB is null) return false;
			return frA.Equals(frB);
		}

		public static bool operator != (Fraction frA, Fraction frB)
		{
			return !(frA == frB);
		}

		public static implicit operator double (Fraction fr)
		{
			return fr == null ? 0 : fr.Ratio;
		}

		private static int FindLeastCommonMultiple(int n1, int n2)
		{
			if (n1 == 0 || n2 == 0) throw new ArgumentException("Zeros not allowed!");
			if (n1 == n2) return n1;  // simple case
			if (n1 > n2) // Swap
			{
				int temp = n1;
				n1 = n2;
				n2 = temp;
			}
			// Now we know that n2 > n1
			for (int i = 1; i < n1; ++i)
			{
				if ((n2 * i) % n1 == 0) return i * n2;
			}
			return n1 * n2;
		}

	}
}
