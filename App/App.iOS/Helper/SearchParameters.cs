using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace App.iOS
{
	// Partial Source: http://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist
	public class SearchParameters
	{
		private static string make;
		private static string year;
		private static string year2;
		private static string partName;

		public static event PropertyChangedEventHandler PropertyChanged;

		public static string Make
		{
			get { return make; }
			set { make = value; OnPropertyChanged("Make"); }
		}
			
		public static string Year
		{
			get { return year; }
			set { year = value; year2 = value; OnPropertyChanged("Year"); }
		}

		public static string Year2 
		{
			get { return year2; }
			set { year2 = value; OnPropertyChanged ("Year2"); }
		}	

		public static string PartName
		{
			get { return partName; }
			set { partName = value; OnPropertyChanged("PartName"); }
		}
			
		public static void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(null, new PropertyChangedEventArgs(name));
			}
		}
	}
}