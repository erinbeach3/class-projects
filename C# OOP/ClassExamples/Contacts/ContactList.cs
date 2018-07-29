using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ClassExamples.Contacts
{
	public class ContactList : IEnumerable
	{
		private List<Contact> contacts = new List<Contact>();

		public ContactList() { }

		public ContactList(Stream stream)
		{
			LoadFromStream(stream);
		}

		public ContactList(string filePath)
		{
			using (Stream stream = File.OpenRead(filePath)) LoadFromStream(stream);
		}

		private void LoadFromStream(Stream stream)
		{
			contacts.Clear();
			using (BinaryReader reader = new BinaryReader(stream))
			{
				int count = reader.ReadInt32();
				for (int i = 0; i < count; ++i) contacts.Add(new Contact(reader));
			}
		}

		public int Count => contacts.Count;

		public void Add(Contact contact)
		{
			if (contact == null) throw new ArgumentNullException(nameof(contact));
			contacts.Add(contact);
		}

		public bool Remove(Contact c) => contacts.Remove(c);

		public bool Contains(Contact c) => contacts.Contains(c);

		public Contact this[int index]
		{
			get { return contacts[index]; }
		}

		public void Serialize(Stream stream)
		{
			using (BinaryWriter writer = new BinaryWriter(stream))
			{
				writer.Write(contacts.Count);
				foreach (Contact c in contacts) c.Serialize(writer);
			}
		}

		public void Serialize(string filePath)
		{
			using (FileStream fstream = File.OpenWrite(filePath)) Serialize(fstream);
		}

		#region IEnumerable implementation

		public IEnumerator GetEnumerator()
		{
			return contacts.GetEnumerator();
		}

		#endregion
	}
}
