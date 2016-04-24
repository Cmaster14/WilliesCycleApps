using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI.Server.Models;
using WebAPI.Server.Database;
using System.Globalization;

namespace WebAPI.Server.Controllers
{
    public class PartsController : ApiController
    {
        private static string key = "y8fN9sLekaKFNvi2apo409MxBv0e";

        // api/Parts
        /// <summary>
        /// Use Connector to connect to DB.
        /// </summary>
        /// <param name="token">The private key.</param>
        /// <returns>A list of parts that was created from the DB on Willie's Server.</returns>
        public IEnumerable<Part> Get (string token)
        {
            if (!token.Equals(key))
            {
                return new List<Part>();
            }
            else
            {
                Connector connector = new Connector();
                return connector.Get("SELECT * FROM Parts");
            }
        }

        // api/Parts
        /// <summary>
        /// Use Connector to connect to DB. Formulate query to pass to DB.
        /// </summary>
        /// <param name="year">The year of the part.</param>
        /// <param name="make">The make of the part.</param>
        /// <param name="partName">The name of the part.</param>
        /// <param name="token">The private key.</param>
        /// <returns>A list of parts satisfying the query conditions that was 
        /// created from the DB on Willie's Server.</returns>
        public IEnumerable<Part> Get(string year, string make, string partName, string token)
        {
            if (!token.Equals(key))
            {
                return new List<Part>();
            }
            else
            {
                Connector connector = new Connector();
                if (make != null && make.Length > 1)
                {
                    make = make.Substring(0, 1);
                }

                var queryPartName = partName;
                if (queryPartName != null && queryPartName.Contains("'"))
                {
                    queryPartName = queryPartName.Replace("'", "''");
                }

                if (queryPartName != null && !queryPartName.Equals(""))
                {
                    var list = connector.Get("SELECT * FROM Parts WHERE YR = \'" + year + "\' AND "
                        + "PartName = \'" + queryPartName + "\' AND " + "Make like \'" + make + "%\'");

                    return list;
                }
                else
                {
                    return new List<Part>();
                }
            }
        }
        
        // This addition was put in to facilitate querying for a range of years, it returns a list of parts for a range of years
        /// 4/23/2016 addition 1 begin

        public IEnumerable<Part> Get(string year, string year2, string make, string partName, string token)
        {
            if (!token.Equals(key))
            {
                return new List<Part>();
            }
            else
            {
                Connector connector = new Connector();
                if (make != null && make.Length > 1)
                {
                    make = make.Substring(0, 1);
                }

                var queryPartName = partName;
                if (queryPartName != null && queryPartName.Contains("'"))
                {
                    queryPartName = queryPartName.Replace("'", "''");
                }

                if (queryPartName != null && !queryPartName.Equals(""))
                {
                    var list = connector.Get("SELECT * FROM Parts WHERE YR BETWEEN \'" + year + "\' AND \'" + year2 + "\' AND "
                        + "PartName = \'" + queryPartName + "\' AND " + "Make like \'" + make + "%\'");

                    return list;
                }
                else
                {
                    return new List<Part>();
                }
            }
        }
        
        /// 4/23/2016 addition 1 end

        // api/Parts
        /// <summary>
        /// Use Connector to connect to DB. Formulate query to pass to DB.
        /// </summary>
        /// <param name="make">The make to check.</param>
        /// <param name="token">The private key.</param>
        /// <returns>A list of valid years for the selected make.</returns>
        public IEnumerable<string> GetYearSpinner(string make, string token)
        {
            if (!token.Equals(key))
            {
                return new List<string>();
            }
            else
            {
                Connector connector = new Connector();
                if (make != null && make.Length > 1)
                {
                    make = make.Substring(0, 1);
                }
                return connector.GetYearSpinner("SELECT DISTINCT YR FROM Parts WHERE Make like \'" + make + "%\'");
            }
        }

        // api/Parts
        /// <summary>
        /// Use Connector to connect to DB. Formulate query to pass to DB.
        /// </summary>
        /// <param name="year">The year to check.</param>
        /// <param name="make">The make to check.</param>
        /// <param name="token">The private key.</param>
        /// <returns>A list of valid part names for the selected year and make.</returns>
        public IEnumerable<string> GetPartNameSpinner(string year, string make, string token)
        {
            if (!token.Equals(key))
            {
                return new List<string>();
            }
            else
            {
                Connector connector = new Connector();
                if (make != null && make.Length > 1)
                {
                    make = make.Substring(0, 1);
                }

                var list = connector.GetPartNameSpinner("SELECT DISTINCT PartName FROM Parts WHERE YR = \'" + year + "\' AND "
                    + "Make like \'" + make + "%\'");

                for (int i = 0; i < list.Count; i++)
                {
                    var partName = list[i];

                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    partName = textInfo.ToTitleCase(partName.ToLower());

                    if (partName != null && !partName.Equals(""))
                    {
                        list[i] = partName;
                    }
                }

                list.Sort();

                return list;
            }
        }
        
        // This addition was made so that the PartName list would be populated for the range of years and not just a single year
        //4/23/2016 addition 2 begin
        public IEnumerable<string> GetPartNameSpinner(string year, string year2, string make, string token)
        {
            if (!token.Equals(key))
            {
                return new List<string>();
            }
            else
            {
                Connector connector = new Connector();
                if (make != null && make.Length > 1)
                {
                    make = make.Substring(0, 1);
                }

                var list = connector.GetPartNameSpinner("SELECT DISTINCT PartName FROM Parts WHERE YR = \'" + year + "\' AND \'" + year2 + "\' AND "
                    + "Make like \'" + make + "%\'");

                for (int i = 0; i < list.Count; i++)
                {
                    var partName = list[i];

                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    partName = textInfo.ToTitleCase(partName.ToLower());

                    if (partName != null && !partName.Equals(""))
                    {
                        list[i] = partName;
                    }
                }

                list.Sort();

                return list;
            }
        }
        //4/23/2016 addition 2 end
    }
}
