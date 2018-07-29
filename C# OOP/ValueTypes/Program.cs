using System;

namespace ValueTypes
{
	interface IHaveValue
	{
		double Value { get; set; }
	}

	struct MyStruct : IHaveValue
	{
		public double Value { get; set; }
	}

	class MyClass : IHaveValue
	{
		public double Value { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			int i = 5;
			MyStruct s = new MyStruct { Value = 5 };
			MyClass c = new MyClass { Value = 5 };
			ChangeMe(i);
			ChangeMe(s);
			ChangeMe(c);
			Console.WriteLine("Int: {0}", i);
			Console.WriteLine("Struct: {0}", s.Value);
			Console.WriteLine("Class: {0}", c.Value);
			Console.WriteLine();
			ChangeMe((IHaveValue)c);
			ChangeMe((IHaveValue)s);
			Console.WriteLine("Struct: {0}", s.Value);
			Console.WriteLine("Class: {0}", c.Value);
			ChangeMe(ref s);
			Console.WriteLine(s.Value);
		}

		static void ChangeMe(int i)
		{
			i = 10;
		}

		static void ChangeMe(MyStruct s)
		{
			s.Value = 10;
		}

		static void ChangeMe(ref MyStruct s)
		{
			s.Value = 50;
		}

		static void ChangeMe(MyClass c)
		{
			c.Value = 10;
		}

		// Generic version
		static void ChangeMe<T>(T t)
			where T : IHaveValue
		{
			t.Value = 10;
		}

		static void StructsAndTheNewKeyword()
		{
			MyStruct m1;
			// Technically, we do not need to use "new" to create a MyStruct.
			// However, we cannot use it until it is initialized:
			//m1.Value = 5;	// error
			// so we initialize it thus:
			m1 = new MyStruct();

			// Some structs, like DateTime, have multiple constructors.
			// For these, the new keyword lets us call the correct constructor:
			DateTime d1 = new DateTime(2018, 1, 31, 8, 59, 59);
		}
	}
}
