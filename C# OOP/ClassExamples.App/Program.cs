using ClassExamples.Contacts;
using ClassExamples.Math;
using System;
using System.Collections.Generic;

namespace ClassExamples.App
{
	class Program
	{
		private static Random _random = new Random();
		static void Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			FunWithFractions();
			ManagingContacts();
			UsingStatistics();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
		}

		static void FunWithFractions()
		{
			Fraction frA = new Fraction(1, 2),
				frB = new Fraction(3, 4);
			// implicit & explicit casting:
			double valueA = frA, valueB = (double)frB;

			Fraction sum = frA + frB;
			Fraction ratio = frA / frB;
			Fraction product = frA * frB;
			Fraction difference = frA - frB;
			Fraction unity = Fraction.Unity, zero = Fraction.Zero;
			Fraction result = unity * zero;
			bool test1 = result == Fraction.Zero,
				test2 = sum != product;
			//TODO: implement >, >=, <, and <= operators
		}

		static void ManagingContacts()
		{
			ContactList contacts = new ContactList();
			Contact c = new Contact { Name = "Casey", EMailAddress = "bigmomma@gmail.com" };
			c.PhoneNumbers.Add(new PhoneNumber { Number = "555-555-5555", Type = PhoneType.Mobile });
			c.PhoneNumbers.Add(new PhoneNumber { Number = "666-666-6666", Type = PhoneType.Home });
			contacts.Add(c);
			c = new Contact { Name="Ralph", EMailAddress = "lampshade@hotmail.com" };
			c.PhoneNumbers.Add(new PhoneNumber { Number = "888-888-8888", Type = PhoneType.Business });
			contacts.Add(c);
			string fPath = "Contacts.bin";
			contacts.Serialize(fPath);

			ContactList copy = new ContactList(fPath);
			foreach (Contact contact in copy) Console.WriteLine(c.Name);

			bool result = c.AddPhoneNumber(new PhoneNumber { Number = "888-888-8888", Type = PhoneType.Business });
			if (!result) Console.WriteLine("Number was a duplicate.");
		}

		static List<double> CreateValues(int count)
		{
			List<double> r = new List<double>();
			for (int i = 0; i < count; ++i) r.Add(_random.NextDouble());
			return r;
		}

		static void UsingStatistics()
		{
			List<double> values = CreateValues(1000);
			Statistics stats = new Statistics(values);
			Console.WriteLine($"  N: {stats.N}  Mean: {stats.Mean:F4}  " +
				$"StdDev: {stats.StandardDeviation:F4}  " +
				$"Min: {stats.Minimum:F4} Max: {stats.Maximum:F4}");

			double[] arr = CreateValues(1000).ToArray();
			stats = new Statistics(arr);
			Console.WriteLine($"  N: {stats.N}  Mean: {stats.Mean:F4}  " +
				$"StdDev: {stats.StandardDeviation:F4}  " +
				$"Min: {stats.Minimum:F4} Max: {stats.Maximum:F4}");

			Statistics stats2 = new Statistics(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
		}
	}
}
