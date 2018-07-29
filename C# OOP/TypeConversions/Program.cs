using System;

namespace TypeConversions
{
	class Program
	{
		static void Main(string[] args)
		{
		}

		class A { }

		class B : A { }

		class C : B { }

		interface IQ { }

		class Q : A, IQ  { }

		static void ClassConversions()
		{
			A a = new A();
			B b = new B();
			C c = new C();
			Q q = new Q();
			a = b;  // allowed. b is a A.
			b = c;  // allowed. c is a B.
			a = c;  // allowed. c is a A.
			a = q;  // allowed. q is a A.
							// These are not allowed:
							//b = a;  // a is not B
							//c = a;	// a is not C
							//q = a;	// a is not Q
							//b = q;	// q is not b

			bool test;
			object o = q;
			test = a is B;	// false
			test = c is A;  // true
			test = o is B;  // false;
			test = c is B;  // true
			test = b is null; // false

			q = a as Q;	// upcast fails.  q == null
			c = b as C; // upcast fails.  c == null
			a = c;			// a valid implicit downcast
			c = a as C;	// upcast succeeds

			if (o is A a2)
			{
				// do something with a2
			}

			double d = 5.7;
			object obj = d;
			if (obj is double d2)
			{
				// use d2
			}

			try
			{
				q = (Q)a;
			}
			catch(InvalidCastException)
			{
				Console.WriteLine("The cast is not possible.");
			}

		}

		static void InterfaceConversions()
		{

			A a = new A();
			B b = new B();
			C c = new C();
			Q q = new Q();
			bool test = a is IQ;  // false
			test = q is IQ;       // true
			IQ iq = b as IQ;      // assigns null
			iq = q as IQ;         // succeeds
			object o = c;
			iq = (IQ)o;		// Throws InvalidCastException

		}
	}
}
