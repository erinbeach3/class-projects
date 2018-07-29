using System.Windows;
using System.Windows.Controls;

namespace MutexDemo
{
	/// <summary>
	/// Interaction logic for QuadrantView.xaml
	/// </summary>
	public partial class QuadrantView : UserControl
	{
		public QuadrantView()
		{
			InitializeComponent();
			SizeChanged += QuadrantView_SizeChanged;
		}

		private void QuadrantView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (!e.NewSize.IsEmpty)
			{
				pbar.Height = 0.8 * e.NewSize.Height;
				pbar.Foreground = Foreground;
			}
		}

		public void UpdateProgress(double value, int nCycles)
		{
			pbar.Value = value;
			cycles.Text = $"{nCycles} completed";
		}
	}
}
