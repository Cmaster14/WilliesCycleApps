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
using Android.Widget;
using Android.OS;

namespace App.Portable
{
     //public class API
     //{
     //     //private const string BASE_URL = "http://ec2-54-213-92-252.us-west-2.compute.amazonaws.com:80/";
     //     private const string KEY = "y8fN9sLekaKFNvi2apo409MxBv0e";
     //     private const string BASE_URL = "http://173.186.190.173:1336/";

     //     public static async Task<List<string>> GetPickerData(string make)
     //     {
     //          var makeAbbreviation = make[0].ToString();
     //          var request = string.Format("api/Parts?make={0}&token={1}", makeAbbreviation, KEY);
     //          var client = new HttpClient();
               
     //          client = new HttpClient()
     //          {
     //               BaseAddress = new Uri(BASE_URL),
     //          };
               
     //          client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

     //          client.Timeout = TimeSpan.FromSeconds(3);
     //          var json = "hello";
     //          try {
     //               json = await client.GetStringAsync(request);
     //          }
     //          catch
     //          {
     //               return null;
     //          }
     //          return JsonConvert.DeserializeObject<List<string>>(json);
     //     }

     //     public static async Task<List<string>> GetPickerData(string year, string make)
     //     {
     //          var makeAbbreviation = make[0].ToString();
     //          var request = string.Format("api/Parts?year={0}&make={1}&token={2}", year, makeAbbreviation, KEY);

     //          var client = new HttpClient()
     //          {
     //               BaseAddress = new Uri(BASE_URL),
     //          };
     //          client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

     //          var json = await client.GetStringAsync(request);
     //          return JsonConvert.DeserializeObject<List<string>>(json);
     //     }

     //     public static async Task<List<Part>> GetParts(string partName, string make, string year)
     //     {
     //          var request = string.Format("api/Parts?year={0}&make={1}&partName={2}&token={3}", year, make, partName, KEY);

     //          var client = new HttpClient()
     //          {
     //               BaseAddress = new Uri(BASE_URL),
     //          };
     //          client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

     //          var json = await client.GetStringAsync(request);
     //          if (json == null)
     //          {
     //               Part part = new Part { Make = "No matches found." };
     //               List<Part> noresult = new List<Part>() { part };
     //               return noresult;
     //          }
     //          else
     //          {
     //               return JsonConvert.DeserializeObject<List<Part>>(json);
     //          }
     //     }

     //     public static async Task<string> VerifyCompletedPayment(string transactionJson, Part partSold)
     //     {
     //          var request = string.Format("api/Payment?year={0}&make={1}&model={2}&partName={3}&location={4}&seqNumber={5}&price={6}&transaction={7}&modify={8}&token={9}",
     //               partSold.Year, partSold.Make, partSold.Model, partSold.PartName, partSold.Location, partSold.ID, partSold.Price, transactionJson, 0, KEY);

     //          var client = new HttpClient()
     //          {
     //               BaseAddress = new Uri(BASE_URL),
     //          };
     //          client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

     //          var json = await client.GetStringAsync(request);
     //          return JsonConvert.DeserializeObject<string>(json);
     //     }
     //}
     public class API
     {
          private const string pass = "Password = Williescycles1;";
          //private const string BASE_URL = "Data Source=willies.database.windows.net;Initial Catalog='Willies Database';User ID=seniordesign;" + pass;
          private const string BASE_URL = "Data Source=sdesign7.cse.eng.auburn.edu;Initial Catalog='master';User ID=sa;Password=;";
          private const string KEY = "y8fN9sLekaKFNvi2apo409MxBv0e";

          public static async Task<List<string>> GetPickerData(string make)
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

          public static async Task<List<string>> GetPickerData(string year, string make)
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
                                   try
                                   {
                                        result.Add(myReader.GetString(0).Substring(0, myReader.GetString(0).LastIndexOf('-')));
                                   }
                                   catch (Exception e)
                                   {
                                        Console.WriteLine(myReader.GetString(0));
                                        result.Add(myReader.GetString(0));
                                   }
                              }
                         }
                    }
                    conn.Close();
               }

               return result;
          }

          public static async Task<List<Part>> GetParts(string partName, string make, string year)
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

          public static async Task<string> VerifyCompletedPayment(string transactionJson, Part partSold)
          {
               var request = string.Format("api/Payment?year={0}&make={1}&model={2}&partName={3}&location={4}&seqNumber={5}&price={6}&transaction={7}&modify={8}&token={9}",
                    partSold.Year, partSold.Make, partSold.Model, partSold.PartName, partSold.Location, partSold.ID, partSold.Price, transactionJson, 0, KEY);

               var client = new HttpClient()
               {
                    BaseAddress = new Uri(BASE_URL),
               };
               client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

               var json = await client.GetStringAsync(request);
               return JsonConvert.DeserializeObject<string>(json);
          }
     }
}