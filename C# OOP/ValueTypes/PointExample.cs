using System;

namespace ValueTypes
{
	class PointExample
	{

		public struct VPoint { public int X; public int Y; }

		public class RPoint { public int X; public int Y; }

		public static void Demo()
		{
			VPoint vp = new VPoint { X = 10, Y = -10 };
			RPoint rp = new RPoint { X = 10, Y = -10 };
			ClearPoints(vp, rp);
			Console.WriteLine($"VPoint: X={vp.X}, Y={vp.Y}"); // VPoint: X=10, Y=-10
			Console.WriteLine($"RPoint: X={rp.X}, Y={rp.Y}");	// RPoint: X=0, Y=0
		}

		private static void ClearPoints(VPoint vp, RPoint rp)
		{
			vp.X = vp.Y = 0;	// We are modifying a copy of the original.
			rp.X = rp.Y = 0;	// We are modifying the original.
		}
	}
}
