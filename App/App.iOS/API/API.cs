using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace App.iOS
{
	public class API
	{
		private const string TOKEN = "y8fN9sLekaKFNvi2apo409MxBv0e";
		private const string BASE_URL = "http://173.186.190.173:1336/";
		//TEST SERVER BASE URL private const string BASE_URL = "131.204.27.105";

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



		//4/23/2016 addition begin
		public static async Task<List<string>> GetPickerData (string year, string year2, string make)
		{
			var request = string.Format ("api/Parts?year={0}&year2={1}&make={2}&token={3}", year, year2, make, TOKEN);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			return JsonConvert.DeserializeObject <List<string>> (json);
		}
		//4/23/2016 addition end


		/*
		public static async Task<List<Part>> GetParts (string partName, string make, string year)
		{
			var request = string.Format ("api/Parts?year={0}&make={1}&partName={2}&token={3}", year, make, partName, TOKEN);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			Console.WriteLine (json);
			return JsonConvert.DeserializeObject <List<Part>> (json);
		}*/


		public static async Task<List<Part>> GetParts (string partName, string make, string year, string year2)
		{
			var request = string.Format ("api/Parts?year={0}&year2={1}&make={2}&partName={3}&token={4}", year, year2, make, partName, TOKEN);

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
	/*
	public class API
	{
		//private const string pass = "Password = Williescycles1;";
		private const string BASE_URL = "Data Source=WIN-NU9I51T7GFV\\SQLEXPRESS;Initial Catalog=master;UserID=sa;Password=Williescycle1";
		private const string KEY = "y8fN9sLekaKFNvi2apo409MxBv0e";

		public static async Task<List<string>> GetPickerData (string make)
		{
			List<string> result = new List<string>();

			SqlConnection conn = new SqlConnection(BASE_URL);

			try
			{
				conn.Open();
			}
			catch (Exception e)
			{
				return null;
			}

			using (SqlCommand myCommand = new SqlCommand("SELECT DISTINCT YR FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' ORDER BY YR", conn))
			{
				using (SqlDataReader myReader = myCommand.ExecuteReader())
				{

					while (myReader.Read())
					{
						result.Add(myReader.GetString(0));
					}
				}

			}
			conn.Close();


			return result;
		}

		public static async Task<List<string>> GetPickerData (string year, string make)
		{
			List<string> result = new List<string>();

			using (SqlConnection conn = new SqlConnection(BASE_URL))
			{
				conn.Open();

				using (SqlCommand myCommand = new SqlCommand("SELECT DISTINCT PartName FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' AND YR LIKE " + year + " ORDER BY PartName", conn))
				{
					using (SqlDataReader myReader = myCommand.ExecuteReader())
					{

						while (myReader.Read())
						{
							result.Add(myReader.GetString(0).Substring(0, myReader.GetString(0).IndexOf('-')));
						}
					}
				}
				conn.Close();
			}

			return result;
		}

		public static async Task<List<Part>> GetParts (string partName, string make, string year)
		{
			List<Part> result = new List<Part>();

			using (SqlConnection conn = new SqlConnection(BASE_URL))
			{
				conn.Open();

				if (partName.Contains("'"))
				{
					partName = partName.Replace("'", "''");
				}
				using (SqlCommand myCommand = new SqlCommand("SELECT * FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' AND YR LIKE " + year + " AND PartName LIKE '" + partName + "%'", conn))
				{
					using (SqlDataReader myReader = myCommand.ExecuteReader())
					{

						while (myReader.Read())
						{
							result.Add(new Part()
								{
									PartName = myReader.GetString(myReader.GetOrdinal("PartName")),
									Year = myReader.GetString(myReader.GetOrdinal("YR")),
									Make = myReader.GetString(myReader.GetOrdinal("Make")),
									Model = make,
									Price = myReader.GetString(myReader.GetOrdinal("Price")),
									Interchange = myReader.GetString(myReader.GetOrdinal("Interchange")),

								});
						}
					}
				}
				conn.Close();
			}

			return result;
		}

		public static async Task<string> VerifyCompletedPayment (string transactionJson, Part partSold)
		{
			var request = string.Format ("api/Payment?year={0}&make={1}&model={2}&partName={3}&location={4}&seqNumber={5}&price={6}&transaction={7}&modify={8}&token={9}",
				partSold.Year, partSold.Make, partSold.Model, partSold.PartName, partSold.Location, partSold.ID, partSold.Price, transactionJson, 0, KEY);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			return JsonConvert.DeserializeObject<string> (json);
		}
	}*/
}