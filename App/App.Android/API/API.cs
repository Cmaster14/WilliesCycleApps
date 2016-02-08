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
using Mono.Security.Protocol;

namespace App.Portable
{
	public class API
	{
          private const string BASE_URL = "Data Source=willies.database.windows.net;Initial Catalog='Willies Database';User ID=seniordesign;Password=Williescycles1;";
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
                    try
                    {
                         conn.Open();
                    }
                    catch (Exception e)
                    {

                    }

                    using (SqlCommand myCommand = new SqlCommand("SELECT DISTINCT PartName FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' AND YR LIKE " + year + " ORDER BY PartName", conn))
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
               }

               return result;
		}

		public static async Task<List<Part>> GetParts (string partName, string make, string year)
		{
               List<Part> result = new List<Part>();

               using (SqlConnection conn = new SqlConnection(BASE_URL))
               {
                    try
                    {
                         conn.Open();
                    }
                    catch (Exception e)
                    {

                    }

                    using (SqlCommand myCommand = new SqlCommand("SELECT * FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' AND YR LIKE " + year + " AND PartName LIKE '" + partName + "'", conn))
                    {
                         using (SqlDataReader myReader = myCommand.ExecuteReader())
                         {

                              while (myReader.Read())
                              {
                                   result.Add(new Part()
                                   {
                                        PartName = partName,
                                        Year = year,
                                        Make = myReader.GetString(myReader.GetOrdinal("Make")),
                                        Price = myReader.GetString(myReader.GetOrdinal("Price")),

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
	}
}