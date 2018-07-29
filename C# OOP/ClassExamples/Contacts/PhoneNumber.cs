using System.IO;

namespace ClassExamples.Contacts
{
	public enum PhoneType { Mobile, Home, Business }

	public class PhoneNumber
	{
		string number = string.Empty;
		public PhoneNumber() { }

		internal PhoneNumber(BinaryReader reader)
		{
			Number = reader.ReadString();
			Type = (PhoneType)reader.ReadInt32();
		}

		public string Number
		{
			get { return number; }
			set
			{
				number = value ?? string.Empty;
			}
		}
		public PhoneType Type { get; set; }

		internal void Serialize(BinaryWriter writer)
		{
			writer.Write(Number);
			writer.Write((int)Type);
		}

		public override bool Equals(object obj)
		{
			if (obj is PhoneNumber p)
			{
				return Type == p.Type &&
					string.Equals(Number, p.Number);
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return Number.GetHashCode();
		}
	}
}
