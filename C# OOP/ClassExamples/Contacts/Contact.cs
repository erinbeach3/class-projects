using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClassExamples.Contacts
{
	public class Contact
	{
		private string emailAddress = string.Empty;
		public Contact()
		{
			PhoneNumbers = new List<PhoneNumber>();
		}

		internal Contact(BinaryReader reader) : this()
		{
			Name = reader.ReadString();
			EMailAddress = reader.ReadString();
			int nPhones = reader.ReadInt32();
			for (int i = 0; i < nPhones; ++i) PhoneNumbers.Add(new PhoneNumber(reader));
		}

		public string Name { get; set; }

		// Encapsulation Exercise:  Expose PhoneNumbers as a read-only list.
		// Add/Remove PhoneNumbers via Methods.
		public List<PhoneNumber> PhoneNumbers { get; private set; }

		public string EMailAddress
		{
			get { return emailAddress; }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					emailAddress = string.Empty;
					return;
				}
				// expression from regexlib.com
				const string emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
				if (Regex.Match(value, emailRegex).Success)
				{
					emailAddress = value;
				}
				else throw new FormatException("Email format is incorrect.");
			}
		}

		internal void Serialize(BinaryWriter writer)
		{
			writer.Write(Name);
			writer.Write(EMailAddress);
			writer.Write(PhoneNumbers.Count);
			foreach(PhoneNumber p in PhoneNumbers)
			{
				p.Serialize(writer);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is Contact c)
			{
				return string.Equals(Name, c.Name) &&
							 PhoneNumbers.Count == c.PhoneNumbers.Count &&
							 PhoneNumbers.All(p => c.PhoneNumbers.Contains(p));
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public bool AddPhoneNumber(PhoneNumber phoneNumber)
		{
			if (phoneNumber == null) return false;
			if (PhoneNumbers.Contains(phoneNumber)) return false;
			PhoneNumbers.Add(phoneNumber);
			return true;
		}
	}
}
