using System;

namespace Generics
{
	public class Wrapper<T>
	{
		public Wrapper(T t)
		{
			Value = t;
		}

		public T Value { get; private set; }
	}

	public class RefWrapper<T>
		where T : class
	{
		public RefWrapper(T t)
		{
			Value = t ?? throw new ArgumentException();
		}

		public T Value { get; private set; }
	}
}
