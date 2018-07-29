using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XmlIO
{
	class Program
	{
		static void Main(string[] args)
		{
			XElement xml = XElement.Load("TranslationSet.xml");
			List<Translations> txls = xml.Elements().Select(e => new Translations(e)).ToList();
			Console.OutputEncoding = Encoding.UTF8;	// required for Cyrillic characters
			for(int nWord=0;nWord<2;++nWord)
			{
				foreach(Translations tx in txls)
				{
					Console.WriteLine($"{tx.Language}: {tx[nWord]}");
				}
				Console.WriteLine();
			}
			xml = new XElement("TranslationSet");
			foreach(Translations tx in txls)
			{
				xml.Add(tx.ToXml());
			}
			xml.Save("TranslationSetCopy.xml");
		}
	}
}
