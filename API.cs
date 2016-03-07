using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.iOS
{
	public class API
	{
		private const string TOKEN = "y8fN9sLekaKFNvi2apo409MxBv0e";
		private const string BASE_URL = "http://173.186.190.173:1336/";

		public static async Task<List<string>> GetPickerData (string make)
		{
			var request = string.Format ("api/Parts?make={0}&token={1}", make, TOKEN);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			return JsonConvert.DeserializeObject <List<string>> (json);
		}

		public static async Task<List<string>> GetPickerData (string year, string make)
		{
			var request = string.Format ("api/Parts?year={0}&make={1}&token={2}", year, make, TOKEN);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			return JsonConvert.DeserializeObject <List<string>> (json);
		}

		public static async Task<List<Part>> GetParts (string partName, string make, string year)
		{
			var request = string.Format ("api/Parts?year={0}&make={1}&partName={2}&token={3}", year, make, partName/*.Substring(0, partName.IndexOf("-"))*/, TOKEN);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			Console.WriteLine (json);
			return JsonConvert.DeserializeObject <List<Part>> (json);
		}

		public static async Task<string> VerifyCompletedPayment (string transactionJson, Part partSold)
		{
			var request = string.Format ("api/Payment?year={0}&make={1}&model={2}&partName={3}&location={4}&seqNumber={5}&price={6}&transaction={7}&modify={8}&token={9}",
				partSold.Year, partSold.Make, partSold.Model, partSold.PartName, partSold.Location, partSold.ID, partSold.Price, transactionJson, 0, TOKEN);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			return JsonConvert.DeserializeObject<string> (json);
		}
	}
}