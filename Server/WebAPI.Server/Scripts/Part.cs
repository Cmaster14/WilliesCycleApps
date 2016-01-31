using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    /// <summary>
    /// Object created for each part in the database. 
    /// Information is pulled from the labels in the database and populates the object.
    /// </summary>
    public class Part
    {
        public string Year { get; set; }
        public string Make { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public string Interchange { get; set; }
        public double Price { get; set; }
    }
}