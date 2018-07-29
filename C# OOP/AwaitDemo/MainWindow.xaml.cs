//#define USEEVENT
using System;
using System.Windows;

namespace AwaitDemo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DataLoader _loader = new DataLoader();
		public MainWindow()
		{
			InitializeComponent();
			_loader.DataReady += _loader_DataReady;
		}

		private void _loader_DataReady(object sender, EventArgs e)
		{
			textBox.Text = _loader.Data;
			btn.IsEnabled = true;
		}

#if USEEVENT
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_loader.BeginLoad();
			btn.IsEnabled = false;
		}
#else
		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			btn.IsEnabled = false;
			textBox.Text = await _loader.LoadAsync();
			btn.IsEnabled = true;
		}
#endif
	}
}
