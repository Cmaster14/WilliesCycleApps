using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Portable
{
	public class API
	{
		private const string BASE_URL = "http://ec2-54-213-92-252.us-west-2.compute.amazonaws.com:80/";
		private const string KEY = "y8fN9sLekaKFNvi2apo409MxBv0e";

		public static async Task<List<string>> GetPickerData (string make)
		{
               List<string> result = new List<string>();

               using (var conn = new SqlConnection("Server=tcp:willies.database.windows.net,1433;Database=Willies Database;User ID=seniordesign@willies;Password=Williescycles1;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
               {
                    try
                    {
                         conn.Open();
                    }
                    catch (Exception e)
                    {
                         
                    }

                    SqlDataReader myReader = null;
                    SqlCommand myCommand = new SqlCommand("SELECT DISTINCT YR FROM Parts Where Make LIKE 'H%'", conn);
                    myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                         result.Add(myReader.GetString(0));
                    }

                    conn.Close();

               }
               return result;
		}

		public static async Task<List<string>> GetPickerData (string year, string make)
		{
			var makeAbbreviation = make[0].ToString ();
			var request = string.Format ("api/Parts?year={0}&make={1}&token={2}", year, makeAbbreviation, KEY);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			return JsonConvert.DeserializeObject <List<string>> (json);
		}

		public static async Task<List<Part>> GetParts (string partName, string make, string year)
		{
			var request = string.Format ("api/Parts?year={0}&make={1}&partName={2}&token={3}", year, make, partName, KEY);

			var client = new HttpClient () {
				BaseAddress = new Uri (BASE_URL),
			};
			client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			var json = await client.GetStringAsync (request);
			if(json == null){
				Part part = new Part{ Make = "No matches found." };
				List<Part> noresult = new List<Part>(){part};
				return noresult;
			} else {
				return JsonConvert.DeserializeObject <List<Part>> (json);
			}
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
	}
}