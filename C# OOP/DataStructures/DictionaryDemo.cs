using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataStructures
{
	internal static class DictionaryDemo
	{
		internal static Dictionary<int, Station> ReadStationDictionary()
		{
			using (FileStream file = File.OpenRead("stations.bin"))
			{
				int count = (int)(file.Length / Station.RecordSize);
				Dictionary<int, Station> stations = new Dictionary<int, Station>(count);
				while (file.Position < file.Length)
				{
					Station s = Station.ReadStation(file);
					stations.Add(s.StationId, s);
				}
				return stations;
			}
		}

		internal static void LoopThroughTheDictionary()
		{
			Dictionary<int, Station> stations = ReadStationDictionary();
			foreach (int key in stations.Keys)
			{
				Station s = stations[key];
				// Do something with the station
			}
		}

		internal static Dictionary<string, Station> ReadStationDictionaryUsingLinq()
		{
			return File.ReadLines("stations.txt").Select(l => Station.Parse(l)).ToDictionary(s => s.NoaaId);
		}

		internal static List<Station> LoadSortedList()
		{
			List<Station> r = File.ReadLines("stations.txt").Select(l => Station.Parse(l)).ToList();
			// We can use this:
			r.Sort((sx, sy) => Comparer<int>.Default.Compare(sx.StationId, sy.StationId));
			// or this:
			r.Sort(Station.StationComparer);
			return r;
		}
	}
}
