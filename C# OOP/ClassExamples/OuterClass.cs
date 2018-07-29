using System;
using System.Collections.Generic;

namespace ClassExamples
{
	public class OuterClass
	{
		static readonly Random random = new Random();
		public static readonly IComparer<OuterClass> OuterClassComparer = new InnerClass();

		private int secretValue = random.Next();

		private class InnerClass : IComparer<OuterClass>
		{
			public int Compare(OuterClass x, OuterClass y)
			{
				return Comparer<int>.Default.Compare(x.secretValue, y.secretValue);
			}
		}
	}
}
