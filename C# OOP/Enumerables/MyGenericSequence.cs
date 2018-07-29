using System.Collections;
using System.Collections.Generic;

namespace Enumerables
{
	class MyGenericSequence<TypeName> : IEnumerable<TypeName>
	{
		private TypeName[] _TypeNames;
		public MyGenericSequence(params TypeName[] TypeNames)
		{
			_TypeNames = TypeNames;
		}

		public IEnumerator<TypeName> GetEnumerator()
		{
			return new TypeNameEnumerator(_TypeNames);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new TypeNameEnumerator(_TypeNames);
		}

		private class TypeNameEnumerator : IEnumerator<TypeName>
		{
			TypeName[] _TypeNames;
			int _index = -1;
			public TypeNameEnumerator(TypeName[] TypeNames)
			{
				_TypeNames = TypeNames;
			}

			public TypeName Current => _TypeNames[_index];

			object IEnumerator.Current => _TypeNames[_index];

			public void Dispose()
			{
				// noTypeName to do
			}

			public bool MoveNext()
			{
				return (++_index < _TypeNames.Length);
			}

			public void Reset()
			{
				_index = -1;
			}
		}
	}
}
