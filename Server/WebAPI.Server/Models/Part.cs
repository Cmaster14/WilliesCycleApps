using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Server.Models
{
    public class Part
    {
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public string Interchange { get; set; }
        public string Price { get; set; }
        public string Location { get; set; }
        public int PKParts { get; set; }
    }
}