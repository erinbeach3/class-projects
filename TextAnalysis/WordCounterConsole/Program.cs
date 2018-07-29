using System;
using System.IO;
using TextAnalyzers.Lib;

namespace WordCounterConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			// Reality Checks
			if (args.Length != 1)
			{
				Console.WriteLine("1 argument is expected.");
				return;
			}
			if (!File.Exists(args[0]))
			 {
				Console.WriteLine($"File '{args[0]}' not found.");
				return;
			}
			WordSearcher searcher = WordSearcher.FromFile(args[0]);
			do
			{
				string input = PromptForSearchWord();
				if (string.IsNullOrWhiteSpace(input)) return;
				int nOccur = searcher.GetWordCount(input);
				Console.WriteLine($"The text '{input}' occurs {nOccur} times.");
			} while (true);
		}

		private static string PromptForSearchWord()
		{
			Console.Write("Please enter text to search for: ");
			return Console.ReadLine();
		}
	}
}
