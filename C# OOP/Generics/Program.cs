using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Generics
{
	class Program
	{
		static void Main(string[] args)
		{
		}

		static void ListExamples()
		{
			List<int> ints = new List<int>();
			List<string> strings = new List<string>();
			List<Stream> streams = new List<Stream>();
			List<DateTime> dates = new List<DateTime>();
			List<HttpClient> clients = new List<HttpClient>();

			ints.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
			strings.Add("Hello World!");
			streams.Add(new MemoryStream());
			dates.Add(DateTime.Now);
			clients.Add(new HttpClient());

			Wrapper<double> dw = new Wrapper<double>(Math.PI);
			double pi = dw.Value;

		}

		static void Tuples()
		{
			var t2 = new Tuple<int, string>(25, "Yes");
			var t2a = Tuple.Create(25, "Yes");
			var t3 = Tuple.Create(57.6, 27m, new List<string>());

			Console.WriteLine(t2.Item2);  // Yes
			Console.WriteLine(t3.Item1);	// 57.6
		}

		static void ValueTuples()
		{
			int start = 1, middle = 5, end = 10;
			var vtuple = (start, middle, end);
			Console.WriteLine(vtuple.start);
			Console.WriteLine(vtuple.middle);
			Console.WriteLine(vtuple.end);
		}

		static void Accumulators()
		{
			Accumulator<int> a1 = new Accumulator<int>();
			//Accumulator<int> a2 = new Accumulator(1, 2, 3, 4, 5);)
		}

	}
}
