using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace MutexDemo
{
	public class Program
	{
		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int FlashWindow(IntPtr hwnd, int invert);
		private static int FlashWindow(IntPtr hwnd, bool invert)
		{
			return FlashWindow(hwnd, invert ? 1 : 0);
		}

		[STAThread]
		static void Main(string[] args)
		{
			using (Mutex m = new Mutex(true, "MutexDemo", out bool isNew))
			{
				if (!isNew)
				{
					Process p = Process.GetProcessesByName(
						Process.GetCurrentProcess().ProcessName)
						.FirstOrDefault();
					if (p != null)
					{
						FlashWindow(p.MainWindowHandle, true);
					}
					return;
				}
				App app = new App();
				MainWindow w = new MainWindow();
				//MainWindow2 w = new MainWindow2();
				app.Run(w);
			}
		}
	}
}
