using System.Collections;

namespace Enumerables
{
	class MySequence: IEnumerable
	{
		private int[] _values;
		public MySequence(params int[] values)
		{
			_values = values;
		}

		public IEnumerator GetEnumerator()
		{
			return new ValueEnumerator(_values);
		}

		private class ValueEnumerator : IEnumerator
		{
			private int[] _values;
			private int _index = -1;
			public ValueEnumerator(int[] values)
			{
				_values = values;
			}
			public object Current
			{
				get
				{
					return _values[_index];
				}
			}

			public bool MoveNext()
			{
				return (++_index < _values.Length);
			}

			public void Reset()
			{
				_index = -1;
			}
		}
	}
}
