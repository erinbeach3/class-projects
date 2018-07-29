using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine();
			//HashSetDemo.FindDistinctValues();
			HashSetDemo.UnionWith();
			HashSetDemo.IntersectWith();
			//HashSetDemo.AddPerformance();
			Console.WriteLine();
		}

		internal static void PushMultiThreaded<T>(Stack<T> stack, T value)
		{
			ICollection c = stack as ICollection;
			lock (c.SyncRoot)
			{
				stack.Push(value);
			}
		}

	}
}
