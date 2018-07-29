using System;

namespace Statics
{
	public class StaticObserver
	{
		// static constructor
		static StaticObserver()
		{
			Console.WriteLine("This is the static constructor.");
		}

		// instance constructor
		public StaticObserver()
		{
			Console.WriteLine("This is the instance constructor.");
		}
	}
}
