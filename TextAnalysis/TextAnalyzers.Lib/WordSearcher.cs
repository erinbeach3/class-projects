using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextAnalyzers.Lib
{
	public class WordSearcher
	{
		public static WordSearcher FromFile(string filePath)
		{
			if (!File.Exists(filePath)) throw new ArgumentException($"File not found: {filePath}");
			return new WordSearcher(File.ReadAllText(filePath));
		}

		public WordSearcher(string document)
		{
			if (string.IsNullOrEmpty(document)) throw new ArgumentNullException(nameof(document));
			Document = document;
		}

		public string Document { get; private set; }

		public IEnumerable<WordLocation> Search(string searchText, bool isCaseSensitive = true)
		{
			StringComparison scomp = isCaseSensitive ? StringComparison.InvariantCulture :
				StringComparison.InvariantCultureIgnoreCase;
			int order = 1, ndx = Document.IndexOf(searchText, scomp);
			while (ndx >= 0)
			{
				yield return new WordLocation { Word = searchText, Location = ndx, FoundOrder = order++ };
				ndx = Document.IndexOf(searchText, ndx + searchText.Length, scomp);
			}
		}

		public Task<IEnumerable<WordLocation>> SearchAsync(string searchText, bool isCaseSensitive = true)
		{
			return Task<IEnumerable<WordLocation>>.Factory.StartNew(() => Search(searchText, isCaseSensitive));
		}

		public IEnumerable<WordLocation> WildcardSearch(string searchText, bool isCaseSensitive = true)
		{
			RegexOptions ops = isCaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
			string rx = searchText.Replace("%", @"\w*").Replace("_", @"\w+");
			int order = 1;
			foreach(Match m in Regex.Matches(Document, rx, ops))
			{
				if (m.Success)
				{
					yield return new WordLocation { Word = m.Value, FoundOrder = order++, Location = m.Index };
				}
			}
		}

		public Task<IEnumerable<WordLocation>> WildcardSearchAsync(string searchText, bool isCaseSensitive = true)
		{
			return Task<IEnumerable<WordLocation>>.Factory.StartNew(() => WildcardSearch(searchText, isCaseSensitive));
		}

		public int GetWordCount(string searchText, bool isCaseSensitive = true)
		{
			return Search(searchText, isCaseSensitive).Count();
		}

	}
}
