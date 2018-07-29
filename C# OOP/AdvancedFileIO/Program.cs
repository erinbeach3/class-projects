using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace AdvancedFileIO
{
	class Program
	{
		static void Main(string[] args)
		{
			//Console.WriteLine($"{CountDocuments(false)} documents found.");
			//Console.WriteLine($"{CountDocuments(true)} documents found (recursive).");

			CopyZipFile();

		}

		static int CountDocuments(bool recurse)
		{
			string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments,
				Environment.SpecialFolderOption.None);

			DirectoryInfo info = new DirectoryInfo(documentsFolder);
			FileInfo[] files = info.GetFiles();

			return CountDocumentsInFolder(documentsFolder, recurse);
		}

		static int CountDocumentsInFolder(string folder, bool recurse)
		{
			try
			{
				int r = Directory.GetFiles(folder).Count();
				if (recurse)
				{
					foreach (string subFolder in Directory.GetDirectories(folder))
						r += CountDocumentsInFolder(subFolder, recurse);
				}
				return r;
			}
			catch (UnauthorizedAccessException) { }
			return 0;
		}

		private static void CopyZipFile()
		{
			const string subFolder = "ZipOut";
			if (Directory.Exists(subFolder)) Directory.Delete(subFolder, true);
			Directory.CreateDirectory(subFolder);
			foreach (string fpath in EnumerateZipFileContents("scripts.zip"))
			{
				string srcName = Path.GetFileName(fpath); // just the file name
				string destPath = Path.Combine(subFolder, srcName); // create relative file path
				File.Copy(fpath, destPath);
			}
		}

		private static IEnumerable<string> EnumerateZipFileContents(string pathToZipFile)
		{
			if (string.IsNullOrEmpty(pathToZipFile)) throw new ArgumentNullException(nameof(pathToZipFile));
			if (!File.Exists(pathToZipFile)) throw new FileNotFoundException($"Zip file '{pathToZipFile}' not found.");
			using (TemporaryFolder folder = new TemporaryFolder())
			{
				ZipFile.ExtractToDirectory(pathToZipFile, folder);
				foreach (string filepath in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
				{
					yield return filepath;
				}
			}
		}

		private static void CreateSpreadsheet()
		{
			SLDocument document = new SLDocument();
			document.SetCellValue("A1", "This is just a test!");
			document.AutoFitColumn(1);
			document.SaveAs("JustATest.xlsx");
		}
	}
}
