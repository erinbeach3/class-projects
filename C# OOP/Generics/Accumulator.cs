using System.Collections.Generic;

namespace Generics
{
	public class Accumulator<T>
	{
		private List<T> values;
		public Accumulator()
		{
			values = new List<T>();
		}
		public Accumulator(params T[] values)
		{
			this.values = new List<T>(values);
		}
		public void Add(T value)
		{
			values.Add(value);
		}
	}
}
