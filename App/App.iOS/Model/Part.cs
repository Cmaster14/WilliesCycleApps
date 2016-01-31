using System;

namespace App.iOS
{
	public class Part
	{
		public string ID { get; set; }
		public string Year { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string PartName { get; set; }
		public string PartNumber { get; set; }
		public string Interchange { get; set; }
		public string Price { get; set; }
		public string Location { get; set; }

		public override string ToString ()
		{
			return string.Format ("{0} {1} {2}", Year, Make, PartName);
		}
	}
}