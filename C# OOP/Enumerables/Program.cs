using System;
using System.Collections;

namespace Enumerables
{
	class Program
	{
		static void Main(string[] args)
		{
			MySequence seq = new MySequence(1, 2, 3, 4, 5);
			IEnumerable ienum = seq as IEnumerable;
			IEnumerable ienum2 = (IEnumerable)seq;
			int nSeq = 0;
			foreach(int o in seq)
			{
				if (0 < nSeq++) Console.Write(',');
				Console.Write(o);
			}
			Console.WriteLine();
			MyGenericSequence<double> genSeq = new MyGenericSequence<double>(1.0, 2.0, 3.0, 4.0, 5.0);
			nSeq = 0;
			foreach(double v in genSeq)
			{
				if (0 < nSeq++) Console.Write(',');
				Console.Write($"{v:N2}");
			}
			Console.WriteLine();
			MyGenericSequence<string> genStr = new MyGenericSequence<string>("a", "b", "c", "d", "e");
			nSeq = 0;
			foreach(string v in genStr)
			{
				if (0 < nSeq++) Console.Write(',');
				Console.Write(v);
			}
			Console.WriteLine();
		}
	}
}
