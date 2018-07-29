namespace Interfaces
{
	class Program
	{
		static void Main(string[] args)
		{
			Egg egg = new Egg();	// Incubate is not accessible from egg.
			IHatchable h = (IHatchable)egg; // Incubate is accessible from h.

			using (MyPrecious p = new MyPrecious())
			{
				p.MakeMeInvisible();
			}
			// p.Dispose has been called, even
			// if an exception is thrown.


		}
	}
}
