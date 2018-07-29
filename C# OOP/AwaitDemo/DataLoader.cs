using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AwaitDemo
{
	public class DataLoader
	{
		public string Data { get; private set; }

		public event EventHandler DataReady;

		public async void BeginLoad()
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("http://www.google.com");
			HttpResponseMessage msg = await client.GetAsync("");
			string content = await msg.Content.ReadAsStringAsync();
			Data = content;
			DataReady?.Invoke(this, EventArgs.Empty);
		}

		public async Task<string> LoadAsync()
		{
			HttpClient client = new HttpClient { BaseAddress = new Uri("http://www.google.com") };
			HttpResponseMessage msg = await client.GetAsync("");
			string content = await msg.Content.ReadAsStringAsync();
			return content;
		}

	}
}
