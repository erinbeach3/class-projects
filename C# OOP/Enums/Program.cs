using System;

namespace Enums
{
	// Simple enum based on integer:
	enum Arrows
	{
		Up,
		Down,
		Left,
		Right
	}

	// enums based on other integral types:
	enum Small : byte { One, Two, Three, Four };
	enum Medium : short { One, Two, Three, Four };
	enum Large : int { One, Two, Three, Four };
	enum Huge : long { One, Two, Three, Four };


	class Program
	{
		static void Main(string[] args)
		{
			Large two = Large.One + 1;
			Console.WriteLine(two);
		}

		private static void Sizes()
		{
			Console.Write(sizeof(Small));
			Console.Write(sizeof(Medium));
			Console.Write(sizeof(Large));
			Console.Write(sizeof(Huge));
		}

		// bitmask enums
		// nested within program class

		[Flags]
		public enum DayCombinations
		{
			Sunday		= 0x0001,
			Monday		= 0x0002,
			Tuesday		= 0x0004,
			Wednesday = 0x0008,
			Thursday	= 0x0010,
			Friday		= 0x0020,
			Saturday	= 0x0040,
			AllDays		= 0x007f
		}

		static void UseFlags()
		{
			DayCombinations weekend = DayCombinations.Saturday | DayCombinations.Sunday;
			DayCombinations weekdays = DayCombinations.AllDays & ~weekend;
			Console.WriteLine(weekend);
			Console.WriteLine(weekend.HasFlag(DayCombinations.Wednesday));
			Console.WriteLine(weekdays);
		}
	}
}
