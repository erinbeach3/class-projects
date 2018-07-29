using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
	internal static class HashSetDemo
	{
		private static Random _random = new Random();

		private static IEnumerable<int> GetIntegers(int maxVal, int count)
		{
			for (int i = 0; i < count; ++i) yield return _random.Next(maxVal);
		}

		private static HashSet<int> FactorsOf(int number, int max = 100)
		{
			HashSet<int> r = new HashSet<int>();
			for(int i=1;i<=max;++i)
			{
				if ((i % number) == 0) r.Add(i);
			}
			return r;
		}

		private static void WriteSet(IEnumerable<int> set)
		{
			Console.WriteLine(string.Join(" ", set.ToArray()));
		}

		internal static void FindDistinctValues()
		{
			// 500 values in the range 0-19 - we expect many duplicates
			Console.WriteLine("Using HashSet:");
			HashSet<int> values = new HashSet<int>(GetIntegers(20, 500));
			Console.WriteLine(values.Count);
			WriteSet(values);
			Console.WriteLine("Using SortedSet:");
			SortedSet<int> sset = new SortedSet<int>(GetIntegers(20, 500));
			Console.WriteLine(sset.Count);
			WriteSet(sset);
		}



		internal static void UnionWith()
		{
			HashSet<int> tens = FactorsOf(10), twelves = FactorsOf(12);
			Console.Write("Tens: ");
			WriteSet(tens);
			Console.Write("Twelves: ");
			WriteSet(twelves);
			tens.UnionWith(twelves);
			Console.Write("Tens UNION Twelves: ");
			WriteSet(tens);
		}

		internal static void IntersectWith()
		{
			HashSet<int> tens = FactorsOf(10, 1000), elevens = FactorsOf(11, 1000);
			tens.IntersectWith(elevens);
			Console.Write("Tens INTERSECT elevens: ");
			WriteSet(tens);
		}

		internal static void AddPerformance()
		{
			const int SIZE = 1000000;
			void clock(Action a)
			{
				DateTime start = DateTime.Now;
				a();
				TimeSpan elapsed = DateTime.Now - start;
				Console.WriteLine($"{elapsed.TotalMilliseconds:F3} ms");
			}

			HashSet<int> set = new HashSet<int>();
			List<int> list = new List<int>(SIZE);
			Console.Write("Hash.Add: ");
			clock(() =>
			{
				for (int i = 0; i < SIZE; ++i) set.Add(i);
			});
			Console.Write("List.Add: ");
			clock(() =>
			{
				for (int i = 0; i < SIZE; ++i) list.Add(i);
			});
			list = new List<int>();
			Console.Write("List (w/o capacity): ");
			clock(() =>
			{
				for (int i = 0; i < SIZE; ++i) list.Add(i);
			});
		}
	}
}
