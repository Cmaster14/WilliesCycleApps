
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using App.Portable;

namespace App.Android
{
	[Activity (Label = "Willie's Cycle", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]			
	public class SliderPageActivity : FragmentActivity, SearchMakeFragment.MakeListener, SearchYearFragment.YearListener, SearchPartFragment.PartListener

	{
          private List<string> mYears = new List<string>() { "none" };
          private List<string> mParts = new List<string>() { "none" };
          private string make = "make", year = "year", part = "part";
		private string[] searchString = new string[3]; //0: make, 1: year, 2: part
          ArrayAdapter<global::Android.Support.V4.App.Fragment> adapter;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ViewPagerActivity);

               adapter = new ArrayAdapter<global::Android.Support.V4.App.Fragment>(this, Resource.Layout.ViewPagerActivity);

               SearchMakeFragment fragment = new SearchMakeFragment();
               SearchYearFragment fragment2 = new SearchYearFragment();
               SearchPartFragment fragment3 = new SearchPartFragment();

               adapter.Add(fragment);
               adapter.Add(fragment2);
               adapter.Add(fragment3);
               
               fragment.setListener(this);
               fragment.Arguments = Intent.Extras;
               fragment2.setListener(this);
               fragment2.Arguments = Intent.Extras;
               fragment3.setListener(this);
               fragment3.Arguments = Intent.Extras;

               global::Android.Support.V4.App.FragmentTransaction transaction = SupportFragmentManager.BeginTransaction();
               transaction.Add(Resource.Id.frame_container, fragment);
               transaction.Add(Resource.Id.frame_container2, fragment2, "year");
               transaction.Add(Resource.Id.frame_container3, fragment3, "part");
               transaction.Commit();
		
		}
		public void search(string makeIn, string yearIn, string partIn)
		{
			searchString[0] = makeIn;
			searchString[1] = yearIn;
			searchString[2] = partIn;
			var partsActivity = new Intent (this, typeof(PartsActivity));
			partsActivity.PutExtra ("search", searchString);
			StartActivity (partsActivity);
		}

          public async void passMake(string makeIn)
          {
               if (!make.Equals(makeIn))
               {
                    make = makeIn;
                    mYears = await getYears(make);
                    SearchYearFragment yf;
                    yf = (SearchYearFragment) SupportFragmentManager.FindFragmentByTag("year");
                    yf.setYears(mYears);
               }
          }
          public async void passYear(string yearIn)
          {
               if (!year.Equals(yearIn))
               {
                    yearIn = mYears[0];
                    year = yearIn;
                    mParts = await getParts(year);
                    adapter.NotifyDataSetChanged();
               }
          }

          public void passPart(string partIn)
          {
               part = partIn;
               if (this != null)
               {
                    this.search(make, year, part);
               }
          }

          private async Task<List<string>> getYears(string make)
          {
               List<string> years = await API.GetPickerData(make);
               return years;
          }

          private async Task<List<string>> getParts(string year)
          {
               List<string> parts = await API.GetPickerData(year, make);
               parts = parts.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
               return parts;
          }
	}
		
}

