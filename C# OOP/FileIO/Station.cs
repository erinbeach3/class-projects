using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

#if DATASTRUCTURES
namespace DataStructures
#else
namespace FileIO
#endif
{
	public class Station
	{
		public static readonly IComparer<Station> StationComparer = new StationByStationIdComparer();
		public const int RecordSize = 180;
		private const string NULL = "NULL";
		private static readonly Type StationType = typeof(Station);
		private static readonly Dictionary<string, PropertyInfo> _properties;
		static Station()
		{
			_properties = StationType.GetProperties().ToDictionary(pi => pi.Name.ToLower());
		}

		#region Data Properties

		public double DataCoverage { get; set; }

		public double Elevation { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public DateTime MinDate { get; set; }

		public DateTime MaxDate { get; set; }

		public string Name { get; set; }

		public int StationId { get; set; }

		public string NoaaId { get; set; }

		public string Country { get; set; }

		public string State { get; set; }

		#endregion

		public void Report()
		{
			foreach (PropertyInfo p in StationType.GetProperties())
			{
				object value = p.GetValue(this);
				Console.WriteLine($"{p.Name}: {value}");
			}
		}

		internal static Station ReadStation(Stream stream, int index)
		{
			long pos = RecordSize * index;
			if (pos >= stream.Length) return null;
			stream.Position = pos;
			return ReadStation(stream);
		}

		internal static Station ReadStation(Stream stream)
		{
			Station s = new Station();
			using (BinaryReader rdr = new BinaryReader(stream, Encoding.Default, true))
			{
				s.DataCoverage = rdr.ReadDouble();
				s.Elevation = rdr.ReadDouble();
				s.Latitude = rdr.ReadDouble();
				s.Longitude = rdr.ReadDouble();
				s.MinDate = DateTime.Parse(rdr.ReadString());
				s.MaxDate = DateTime.Parse(rdr.ReadString());
				s.Name = rdr.ReadString();
				s.StationId = rdr.ReadInt32();
				s.NoaaId = rdr.ReadString();
				s.Country = rdr.ReadString();
			}
			return s;
		}

		private static DateTime ParseDate(string dstr)
		{
			string[] dp = dstr.Split('-');
			return new DateTime(int.Parse(dp[0]), int.Parse(dp[1]), int.Parse(dp[2]));
		}

		internal static Station Parse(string tsv)
		{
			string[] parts = tsv.Split('\t');
			if (parts.Length != 11) throw new ArgumentException($"Unexpected string: {tsv}");
			Station s = new Station();
			s.StationId = int.Parse(parts[0]);
			s.NoaaId = parts[1];
			s.Name = parts[2];
			s.Country = parts[3];
			s.DataCoverage = double.Parse(parts[4]);
			s.Elevation = double.Parse(parts[5]);
			s.MinDate = ParseDate(parts[6]);
			s.MaxDate = ParseDate(parts[7]);
			s.Latitude = double.Parse(parts[8]);
			s.Longitude = double.Parse(parts[9]);
			s.State = (parts[10] == NULL) ? string.Empty : parts[10];
			return s;
		}

		internal static Station Parse(XElement xml)
		{
			Station s = new Station();
			foreach (XElement e in xml.Elements())
			{
				switch (e.Name.LocalName)
				{
					case "stationid": s.StationId = int.Parse(e.Value); break;
					case "noaaId": s.NoaaId = e.Value; break;
					case "name": s.Name = e.Value; break;
					case "country": s.Country = e.Value; break;
					case "datacoverage": s.DataCoverage = double.Parse(e.Value); break;
					case "elevation": s.Elevation = double.Parse(e.Value); break;
					case "mindate": s.MinDate = ParseDate(e.Value); break;
					case "maxdate": s.MaxDate = ParseDate(e.Value); break;
					case "latitude": s.Latitude = double.Parse(e.Value); break;
					case "longitude": s.Longitude = double.Parse(e.Value); break;
					case "state": s.State = (e.Value == NULL) ? string.Empty : e.Value; break;
				}
			}
			return s;
		}

		internal static Station ParseUsingReflection(XElement xml)
		{
			Station s = new Station();
			foreach (XElement e in xml.Elements())
			{
				PropertyInfo pi = _properties[e.Name.LocalName.ToLower()];
				object value = e.Value;
				switch (pi.PropertyType.Name)
				{
					case "String": value = (e.Value == NULL) ? string.Empty : e.Value; break;
					case "DateTime": value = ParseDate(e.Value); break;
					case "Double": value = double.Parse(e.Value); break;
					case "Int32": value = int.Parse(e.Value); break;
				}
				pi.SetValue(s, value);
			}
			return s;
		}

		private class StationByStationIdComparer : Comparer<Station>
		{
			public override int Compare(Station x, Station y)
			{
				return Comparer<int>.Default.Compare(x.StationId, y.StationId);
			}
		}
	}
}
