using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FileIO
{
	class Program
	{
		const string FILENAME = "The Hobbit.txt";

		static void Main(string[] args)
		{
			//ShowFileSystemInfo();
			//ReadStationsRandomAccess();
			//ReadLargeTextFile();
			ReadLargeBinaryFile();
			//ReadStationsFromText();
			//ReadStationsFromXml();
			//ReadStationsFromXmlUsingReflection();
		}

		static void ShowFileSystemInfo()
		{
			DriveInfo drive = new DriveInfo("c");
			Console.Write($"Drive {drive.Name}:  Format is {drive.DriveFormat};  ");
			Console.WriteLine($"{ drive.AvailableFreeSpace:N0} bytes available out of {drive.TotalSize:N0}");
			DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
			Console.WriteLine($"Directory {dirInfo.Name} was last written to at {dirInfo.LastWriteTime}");
			FileInfo fInfo = dirInfo.GetFiles().FirstOrDefault();
			if (fInfo == null) Console.WriteLine("Directory is empty."); else
				Console.WriteLine($"File {fInfo.Name} is {fInfo.Length:N0} bytes in size.");

			string fpath = Path.Combine(dirInfo.FullName, fInfo.Name);
			// Get the subfolder three levels back
			// from the current directory
			fpath = Path.GetFullPath("../../..");
			Console.WriteLine(fpath);
			Console.WriteLine();
		}

		static void ReadUsingFileClass()
		{
			// Since file is located in current folder,
			// no need to build a full path string:
			byte[] hobbitBytes = File.ReadAllBytes(FILENAME);
			string[] hobbitLines = File.ReadAllLines(FILENAME);
			string hobbitText = File.ReadAllText(FILENAME);
		}

		static void ReadLargeTextFile()
		{
			// We don't want to read all lines of an extremely
			// large text file into memory, so lets read
			// line-by-line:
			foreach(string line in File.ReadLines(FILENAME))
			{
				// do something with this line ...
			}
		}

		static void ReadLargeBinaryFile()
		{
			using(FileStream file = File.OpenRead("stations.bin")) 
			{
				byte[] buffer = new byte[Station.RecordSize];
				int bytesRead = file.Read(buffer, 0, Station.RecordSize);
				while(bytesRead == Station.RecordSize)
				{
					// Wrap a MemoryStream around the buffer:
					using(MemoryStream stream = new MemoryStream(buffer))
					{
						Station s = Station.ReadStation(stream);
						// Do something with the station ...
					}
					bytesRead = file.Read(buffer, 0, Station.RecordSize);
				}
			}
		}

		static void ReadStationsFromIndexLoop(long maxIndex, Func<int,Station> stationFromIndex)
		{
			Console.WriteLine($"{maxIndex:N0} stations found.");
			while(true)
			{
				Console.Write("Enter Station Index: ");
				string sndx = Console.ReadLine();
				if (int.TryParse(sndx, out int ndx) && (ndx > 0) && (ndx <= maxIndex))
				{
					Station s = stationFromIndex(ndx - 1);
					s.Report();
					Console.WriteLine();
				}
				else break;
			}
		}

		static void ReadStationsRandomAccess()
		{
			using(FileStream fs = File.OpenRead("stations.bin"))
			{
				long stationCount = fs.Length / Station.RecordSize;
				ReadStationsFromIndexLoop(stationCount, (ndx) => Station.ReadStation(fs, ndx));
			}
		}

		static void ReadStationsFromText()
		{
			var stations = File.ReadLines("Stations.txt").Skip(1).Select(l => Station.Parse(l)).ToList();
			ReadStationsFromIndexLoop(stations.Count, (ndx) => stations[ndx]);
		}

		static void ReadStationsFromXml()
		{
			XElement xml = XElement.Load("Stations.xml");
			var stations = xml.Elements().Select(e => Station.Parse(e)).ToList();
			ReadStationsFromIndexLoop(stations.Count, (ndx) => stations[ndx]);
		}

		static void ReadStationsFromXmlUsingReflection()
		{
			XElement xml = XElement.Load("Stations.xml");
			var stations = xml.Elements().Select(e => Station.ParseUsingReflection(e)).ToList();
			ReadStationsFromIndexLoop(stations.Count, (ndx) => stations[ndx]);
		}
	}
}
