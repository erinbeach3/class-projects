#pragma warning disable CS4014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TextAnalyzers.Lib;

namespace TextAnalyzer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		WordSearcher _wordSearcher;
		List<Tuple<WordLocation, TextRange>> _ranges;
		public MainWindow()
		{
			InitializeComponent();
			textArea.DragEnter += TextArea_DragEnter;
			textArea.Drop += TextArea_Drop;
			textArea.PreviewDragOver += TextArea_PreviewDragOver;
		}

		#region Drag / Drop
		private void TextArea_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
		}

		private void TextArea_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effects = DragDropEffects.Copy;
		}

		private void TextArea_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetData(DataFormats.FileDrop) is string[] paths && paths.Length > 0)
			{
				_wordSearcher = WordSearcher.FromFile(paths[0]);
				paragraph.Inlines.Clear();
				paragraph.Inlines.Add(new Run { Text = _wordSearcher.Document });
				prompt.Visibility = Visibility.Collapsed;
			}
		}

		#endregion

		private async void search_Click(object sender, RoutedEventArgs e)
		{
			search.IsEnabled = false;
			Cursor = Cursors.Wait;
			string word = searchText.Text;
			bool isCaseSensitive = caseSensitive.IsChecked == true;
			IEnumerable<WordLocation> wordLocs = (useWildcards.IsChecked == true) ?
				await _wordSearcher.WildcardSearchAsync(word, isCaseSensitive) :
				await _wordSearcher.SearchAsync(word, isCaseSensitive);
			List<WordLocation> locations = wordLocs.ToList();
			status.Text = $"{locations.Count} found";
			searchResults.ItemsSource = locations;
			List<Tuple<WordLocation, TextRange>> ranges = new List<Tuple<WordLocation, TextRange>>();
			TextPointer pointer = paragraph.ContentStart;
			while(pointer != null)
			{
				if (pointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
				{
					foreach(WordLocation loc in locations)
					{
						TextPointer start = pointer.GetPositionAtOffset(loc.Location),
							end = start.GetPositionAtOffset(loc.Word.Length);
						ranges.Add(Tuple.Create(loc, new TextRange(start, end)));
					}
					break;
				}
				pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			// Hilite all finds yellow:
			Action hilite = () =>
			{
				if (_ranges != null) _ranges.ForEach(tr => tr.Item2.ClearAllProperties());
				_ranges = ranges;
				if (locations.Count > 0) searchResults.SelectedIndex = 0;
				_ranges.ForEach((tr) => 
				{
					tr.Item2.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
					tr.Item2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Navy);
				});
				search.IsEnabled = true;
				Cursor = Cursors.Arrow;
			};
			Dispatcher.BeginInvoke(hilite, DispatcherPriority.Background);
		}

		private void searchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (searchResults.SelectedItem is WordLocation loc)
			{
				Tuple<WordLocation, TextRange> t = _ranges[searchResults.SelectedIndex];
				textArea.Selection.Select(t.Item2.Start, t.Item2.End);
				Rect srect = textArea.Selection.Start.GetCharacterRect(LogicalDirection.Forward),
					erect = textArea.Selection.End.GetCharacterRect(LogicalDirection.Forward);
				textArea.ScrollToVerticalOffset(textArea.VerticalOffset + (srect.Top + erect.Bottom - textArea.ViewportHeight) / 2);
			}
		}

		private void searchText_TextChanged(object sender, TextChangedEventArgs e)
		{
			search.IsEnabled = !string.IsNullOrEmpty(searchText.Text);
		}

        private void caseSensitive_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void useWildcards_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
