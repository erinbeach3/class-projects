using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataStructures
{
	internal static class ListDemo
	{
		private static Random _random = new Random();

		// Build a sorted list of unique random integers
		internal static List<int> BuildSortedList()
		{
			List<int> r = new List<int>(1000);
			for(int i=0;i<1000;++i)
			{
				int value = _random.Next();
				int ndx = r.BinarySearch(value);
				if (ndx < 0) r.Insert(~ndx, value);
			}
			return r;
		}

		internal static List<Station> LoadSortedStations()
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
