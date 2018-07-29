using System.Collections.Generic;

namespace Interfaces
{
	public class SecondCharComparer : IComparer<string>
	{
		public static readonly SecondCharComparer Instance = new SecondCharComparer();

		private SecondCharComparer() { }

		int IComparer<string>.Compare(string x, string y)
		{
			// Skipping null checks
			// and assuming all lengths > 2
			char cx = x[1], cy = y[1];
			return Comparer<char>.Default.Compare(cx, cy);
		}
	}
}
