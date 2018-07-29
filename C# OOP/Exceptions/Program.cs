using System;

namespace Exceptions
{

	// https://msdn.microsoft.com/en-us/library/system.exception(v=vs.110).aspx

	class Program
	{
		static void Main(string[] args)
		{
			TryCatchFinally();
			TryCatchFinally("Ouch!");
			Console.WriteLine();
			Console.WriteLine();
			TryFinally();
			Console.WriteLine();
			Console.WriteLine();
			MultiCatch();
			Console.WriteLine();
			Console.WriteLine();
			ExceptionFiltering();
			Console.WriteLine();
			Console.WriteLine();
			CallStack(5);
			Console.WriteLine();
			Console.WriteLine();
			Rethrow();
		}

		private static void TryCatchFinally(string msg = null)
		{
			bool exHandled = false;
			try
			{
				DoSomethingThatMightThrow(msg);
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Exception Handled: {ex.Message}");
				exHandled = true;
			}
			finally
			{
				Console.Write("Finally block: ");
				if (exHandled) Console.WriteLine("Exception was handled");
				else
					Console.WriteLine("No Exception occured");
			}
		}

		private static void TryFinally()
		{
			try
			{
				DoSomethingThatMightThrow();
			}
			finally
			{
				Console.WriteLine("The finally block always (always!) runs.");
			}
		}

		private static void MultiCatch()
		{
			try
			{
				DoSomethingThatMightThrow("MultiCatch!");
			}
			catch(InvalidOperationException ex)
			{
				Console.WriteLine($"Invalid Operation: {ex.Message}");
			}
			catch(IndexOutOfRangeException ex)
			{
				Console.WriteLine($"Index was out of range: {ex.Message}");
			}
			catch(Exception ex)
			{
				// This handler must be last!
				Console.WriteLine($"A plain ole exception occurred: {ex.Message}");
			}
			// finally is optional
		}

		// https://www.thomaslevesque.com/2015/06/21/exception-filters-in-c-6/

		private static void ExceptionFiltering()
		{
			const string Msg = "The computer might explode!";
			try
			{
				DoSomethingThatMightThrow(Msg);
			}
			catch(Exception ex) when (ex.Message == "Wrong Answer")
			{
				System.Diagnostics.Debug.WriteLine("Invalid Response Provided");
				throw;
			}
			catch(Exception ex) when (ex.Message == Msg)
			{
				System.Diagnostics.Debug.WriteLine("A dangerous situation avoided.");
			}
		}

		private static void CallStack(int depth)
		{
			if (depth > 0) CallStack(depth - 1);
			try
			{
				DoSomethingThatMightThrow("CallStack is recursive.");
			}
			catch(Exception ex)
			{
				Console.WriteLine($"CallStack caught an exception: {ex.Message}.  Here is the stack trace:");
				Console.WriteLine(ex.StackTrace);
			}
		}

		private static void Rethrow()
		{
			try
			{
				DoSomethingThatMightThrow("This is the end!");
			}
			catch(Exception ex)
			{
				Console.WriteLine($"Rethrow caught an exception: {ex.Message}");
				System.Diagnostics.Debug.WriteLine(ex);
				throw;
			}
		}

		private static void DoSomethingThatMightThrow(string msg = null)
		{
			if (msg != null) throw new Exception(msg);
		}

		private static void CatchAll()
		{
			try
			{
				DoSomethingThatMightThrow();
			}
			catch(Exception)
			{
				Console.WriteLine("Something bad happened.");
				Environment.Exit(1);
			}
		}
	}
}
