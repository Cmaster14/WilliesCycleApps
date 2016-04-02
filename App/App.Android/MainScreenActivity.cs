using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using App.Portable;

namespace App.Android
{
	[Activity(Label = "Willies Cycles")]
	public class MainScreenActivity : Activity
	{
		Bundle bundle = new Bundle ();
		string[] searchCriteria = new string[3]; //[0] - Year; [1] - Make; [2] - Part Name

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
			var years = populateYears ();
			var makes = new List<string> (new string[] { "Select a Make", "H-100R", "Honda", "Yamaha", "Suzuki", "Kawasaki" });
			var partNames = new List<string> (new string[] { "Select a Part Name", "FRT SEAT", "FRAME", "REAR FRAME" });
			var yearAdapter = new ArrayAdapter (this, global::Android.Resource.Layout.SimpleSpinnerItem, years);
			var makeAdapter = new ArrayAdapter (this, global::Android.Resource.Layout.SimpleListItem1, makes);
			var partNameAdapter = new ArrayAdapter (this, global::Android.Resource.Layout.SimpleSpinnerItem, partNames);
			bool noSearch = true;

			Spinner yearSpinner = FindViewById<Spinner> (Resource.Id.yearSpinner);
			yearSpinner.Adapter = yearAdapter;
			Spinner makeSpinner = FindViewById<Spinner> (Resource.Id.makeSpinner);
			makeSpinner.Adapter = makeAdapter;
			Spinner partNameSpinner = FindViewById<Spinner> (Resource.Id.partNameSpinner);
			partNameSpinner.Adapter = partNameAdapter;
			Button searchButton = FindViewById<Button> (Resource.Id.searchButton);
			yearSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				if (/*yearSpinner.GetItemAtPosition */(e.Position) != 0) {
					searchCriteria [0] = (string)yearSpinner.GetItemAtPosition (e.Position);
					if (searchCriteria [0] != null & searchCriteria [1] != null & searchCriteria [2] != null) {
						noSearch = false;
					}
				}
			};
			makeSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				if (/*makeSpinner.GetItemAtPosition*/ (e.Position) != 0) {
					searchCriteria [1] = (string)makeSpinner.GetItemAtPosition (e.Position);
					if (searchCriteria [0] != null & searchCriteria [1] != null & searchCriteria [2] != null) {
						noSearch = false;
					}
				}
			};
			partNameSpinner.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => {
				if (/*partNameSpinner.GetItemAtPosition*/ (e.Position) != 0) {
					searchCriteria [2] = (string)partNameSpinner.GetItemAtPosition (e.Position);
					if (searchCriteria [0] != null & searchCriteria [1] != null & searchCriteria [2] != null) {
						noSearch = false;
					}
				}
			};
			searchButton.Click += (sender, e) => {
				if (noSearch) {
					Toast.MakeText (this, "Please fill all Search Criteria", ToastLength.Long).Show ();
				} else {
					var partsActivity = new Intent (this, typeof(PartsActivity));
					partsActivity.PutExtra ("search", searchCriteria);
					StartActivity (partsActivity);
				}
			};
			
			
		}
		public List<string> populateYears()
		{
			const int yearLimit = 75;
			int currentYear = DateTime.Now.Year + 1;
			var years = new List<string>();
			years.Add ("Select a Year");
			for (int i = 1; i < yearLimit; i++) {
				years.Add((currentYear - i).ToString());
			}
			return years;
		}
	}
}