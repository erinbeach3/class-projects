using System.IO;

namespace Generics
{
	public class Prognosticator<T> where T : class
	{
		public void Prognosticate(T value)
		{
			//if (value == null) value = new T();		// we cannot assume that T has a default constructor.
			object o = value;
			Stream s = value as Stream;
			int? hc = value?.GetHashCode();
		}
	}
}
