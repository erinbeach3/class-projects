using System;
using System.Collections.Generic;

namespace Generics
{
	public class GenExample
	{
		public T? Nullify<T>(T value) where T: struct
		{
			return new Nullable<T>(value);
		}

		public T CreateNew<T>() where T : new()
		{
			return new T();
		}

		public void WriteAll<T>(T value) where T : IEnumerable<double>
		{
			foreach (double d in value) Console.WriteLine(d);
		}

		public void DisposeOf<T>(T value) where T : IDisposable
		{
			value?.Dispose();
		}

		public int Compare<T>(T value1, T value2) where T: IComparable, ICloneable
		{
			T v1 = (T)value1.Clone(), v2 = (T)value2.Clone();
			return v1.CompareTo(v2);
		}
	}
}
