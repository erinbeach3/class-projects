using System;

namespace Properties
{
	public class Widget
	{
		private string name;
		private DateTime dob = DateTime.Now;

		// Read-write property using get/set syntax with a backing field.
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		// Automatic property - it has no backing field and no code.
		public int Value { get; set; }

		// Readonly property using lambda syntax
		public DateTime DateOfBirth => dob;

		// Automatic read-only property with initializer
		// Note that the accessors can have different visibility
		public string SkyPreference { get; private set; } = "Clear Blue";
	}
}
