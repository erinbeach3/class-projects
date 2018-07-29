namespace Disposers
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var v = new ValuableResource("TestFile.txt"))
			{
				v.Write("Hello IDisposable!");
			}
			var v2 = new ValuableResource("TestFile2.txt");
			v2.Write("I will be finalized.");
		}
	}
}
