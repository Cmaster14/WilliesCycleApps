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
     /*
      * This class is used to access Willie's database to obtain years, and parts to be selected by the
      * Customer. This class is called in the SearchCriteriaActivity methods getYears() and getParts().
      * This class is also called by the PartsActivity method FetchPartsFromServer ().
      * 
      *   Created by Unknown
      *   Last Modified Jeremy Woods 3-5-2016
      */
     public class API
	{
          
          //This is a Connection string used to connect to a Microsoft Azure SQL database that contains the a dummy database for Willie's Cycles.
          private const string BASE_URL = "Data Source=willies.database.windows.net;Initial Catalog='Willies Database';User ID=seniordesign;Password=Williescycles1;";
		//This is currently on used for accessing Paypal functions.
          private const string KEY = "y8fN9sLekaKFNvi2apo409MxBv0e";

          //This method takes in a make from the passing method and returns a List of years for the parts that have the specified make.
		public static async Task<List<string>> GetPickerData (string make)
		{
               List<string> result = new List<string>();

               //Creates an sql connection using the given connection string.
               SqlConnection conn = new SqlConnection(BASE_URL);
             
               //Must always tests a connection so avoid unhandled exceptions.
               try
               {
                    conn.Open();
               }
               catch (Exception e){
                    //If this connection fails we would like to display a dialog box notifying the user that the database is down.
               }
               
               /*This creates an SQL query that finds each distinct year that pertains to the specified make. We only look at the
                * the first letter of the Make to search because the database format takes the first letter of the Make - Model of the
                * part. We also sort the years in ascending order.
                */
               using (SqlCommand myCommand = new SqlCommand("SELECT DISTINCT YR FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' ORDER BY YR", conn))
               {
                    //This reads the database using the above command.
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                         while (myReader.Read())
                         {
                              //While there are items in the Reader add them to the result string.
                              result.Add(myReader.GetString(0));
                         }
                    }
               }
               conn.Close();
               return result;
		}

          //This method takes in a make and a year from the passing method and returns a List of parts that have the specified make and year.
		public static async Task<List<string>> GetPickerData (string year, string make)
		{
               List<string> result = new List<string>();

               //This method takes in a make from the passing method and returns a string of years for the parts that have the specified make.
               using (SqlConnection conn = new SqlConnection(BASE_URL))
               {
                    //Must always tests a connection so avoid unhandled exceptions.
                    try
                    {
                         conn.Open();
                    }
                    catch (Exception e)
                    {
                         //If this connection fails we would like to display a dialog box notifying the user that the database is down.
                    }
                    /*This creates an SQL query that finds each distinct part that pertains to the specified make and year. We only look at the
                     * the first letter of the Make to search because the database format takes the first letter of the Make - Model of the
                     * part. We also sort the partss in ascending order.
                     */
                    using (SqlCommand myCommand = new SqlCommand("SELECT DISTINCT PartName FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' AND YR LIKE " + year + " ORDER BY PartName", conn))
                    {
                         //This reads the database using the above command.
                         using (SqlDataReader myReader = myCommand.ExecuteReader())
                         {

                              while (myReader.Read())
                              {
                                   //While there are items in the Reader add them to the result string.
                                   result.Add(myReader.GetString(0).Substring(0, myReader.GetString(0).IndexOf('-')));
                              }
                         }
                    }
                    conn.Close();
               }
               return result;
		}

          //This method takes in a make and a year from the passing method and returns a List of parts that have the specified make and year.
		public static async Task<List<Part>> GetParts (string partName, string make, string year)
		{
               List<Part> result = new List<Part>();

               //This method takes in a make from the passing method and returns a string of years for the parts that have the specified make.
               using (SqlConnection conn = new SqlConnection(BASE_URL))
               {
                    //Must always tests a connection so avoid unhandled exceptions.
                    try
                    {
                         conn.Open();
                    }
                    catch (Exception e)
                    {
                         //If this connection fails we would like to display a dialog box notifying the user that the database is down.
                    }

                    //To circumvent an error when searching parts that contain an apostrophe, single apostrophes are escaped to ensure the
                    //proper entry into the search.
                    if (partName.Contains("'"))
                    {
                         partName = partName.Replace("'", "''");
                    }
                    /*This creates an SQL query that finds each distinct part with the same name that pertains to the specified make and year. 
                     * We only look at the first letter of the Make to search because the database format takes the first letter of the Make
                     * - Model of the part.
                     */
                    using (SqlCommand myCommand = new SqlCommand("SELECT * FROM Parts Where Make LIKE '" + make.Substring(0, 1) + "%' AND YR LIKE " + year + " AND PartName LIKE '" + partName + "%'", conn))
                    {
                         //This reads the database using the above command.
                         using (SqlDataReader myReader = myCommand.ExecuteReader())
                         {

                              while (myReader.Read())
                              {
                                   result.Add(new Part()
                                   {
                                        /*While there are items in the Reader add them to the result string. We take each item
                                         * and get its information to create Part objects. All of the fields get their
                                         * values directly from the database except for the model. In the database "Make" corresponds
                                         * to the first letter of a make followed by the Model. Because of the ambiguity on where exactly
                                         * all of the values were used, the model attribute was added here to contain the string literal
                                         * for the make the customer selects so that it may be displayed later through the part attributes.
                                         */
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

          //This method uses Json strings to handle the payment interaction with PayPal, unsure of the exact function currently.
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