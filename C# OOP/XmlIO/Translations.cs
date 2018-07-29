using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlIO
{
	public class Translations
	{
		private const string LanguageAttribute = "language";
		private const string WordElement = "Word";
		private List<string> _words;
		public Translations(XElement source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (source.Name != nameof(Translations)) throw new ArgumentException("Wrong element.");
			if (!source.HasAttributes || source.FirstAttribute.Name.LocalName != "language")
				throw new ArgumentException($"{nameof(source)} must have a {LanguageAttribute} first.");
			_words = new List<string>();
			Language = source.FirstAttribute.Value;
			foreach(XElement e in source.Elements(WordElement))
			{
				_words.Add(e.Value);
			}
		}

		public string Language { get; private set; }

		public IEnumerable<string> Words => _words;

		public string this[int ndx] => _words[ndx];

		public XElement ToXml()
		{
			XElement r = new XElement(nameof(Translations));
			r.Add(new XAttribute(LanguageAttribute, Language));
			foreach(string w in _words)
			{
				r.Add(new XElement(WordElement, w));
			}
			return r;
		}
	}
}
