using System;

namespace Properties
{
	class Program
	{
		static void Main(string[] args)
		{
			Widget w = new Widget() { Name = "MyWidget", Value = 1507 };

			Console.WriteLine($"Widget: Name={w.Name}, Value={w.Value}, DOB={w.DateOfBirth}, SkyPreference={w.SkyPreference}");
		}
	}
}
