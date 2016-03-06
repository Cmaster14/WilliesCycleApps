
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
     /*
      * Creator Unknown
      * Edited by Jeremy Woods
      * 
      * This line changes attributes of the Android Manifest. Also states that app will only be in Portrait mode.
      */
	[Activity (Label = "Willie's Cycle", MainLauncher = true, Icon = "@drawable/logo", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]			
	/*
      * This is the Activity that will coordinate all of the different Fragments in the application. It implements the 
      * Listeners of all of the fragments so that it can facilitate communication between the fragment spinners.
      */
     public class SearchCriteriaActivity : FragmentActivity, SearchMakeFragment.MakeListener, SearchYearFragment.YearListener, SearchPartFragment.PartListener
	{
          private List<string> mYears = new List<string>() { "none" };
          private List<string> mParts = new List<string>() { "none" };
          private string make = "make", year = "year", part = "part";
		private string[] searchString = new string[3]; //0: make, 1: year, 2: part

          protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ViewPagerActivity);

               //Step 1: Create the all of the Search fragments
               SearchMakeFragment fragment = new SearchMakeFragment();
               SearchYearFragment fragment2 = new SearchYearFragment();
               SearchPartFragment fragment3 = new SearchPartFragment();
               
               //Step 2: Make their listeners this Activity.
               fragment.setListener(this);
               fragment2.setListener(this);
               fragment3.setListener(this);

               //Step 3: Add the Activities to the proper fragment of the ViewPagerActivity.
               global::Android.Support.V4.App.FragmentTransaction transaction = SupportFragmentManager.BeginTransaction();
               transaction.Add(Resource.Id.frame_container, fragment);
               transaction.Add(Resource.Id.frame_container2, fragment2, "year");
               transaction.Add(Resource.Id.frame_container3, fragment3, "part");
               transaction.Commit();
		
		}

          //This method takes all of the search criteria and sends them to
          //a PartsActivity to be used and then starts the Activity.
		public void search(string makeIn, string yearIn, string partIn)
		{
			searchString[0] = makeIn;
			searchString[1] = yearIn;
			searchString[2] = partIn;
			var partsActivity = new Intent (this, typeof(PartsActivity));
			StartActivity (partsActivity);
		}

          /*
           * Because this method implements MakeListener it must have this method.
           * This method takes a make selected in the spinner by the user, gets the
           * Years corresponding to that make, creates a new SearchYearFragment
           * that has the resulting years and then replaces the proper container with
           * the new Search YearFragment.
           */
          public async void passMake(string makeIn)
          {
               if (!make.Equals(makeIn))
               {
                    make = makeIn;
                    mYears = await getYears(make);

                    global::Android.Support.V4.App.FragmentTransaction transaction = SupportFragmentManager.BeginTransaction();
                    SearchYearFragment newYears = SearchYearFragment.newInstance(mYears);
                    newYears.setListener(this);

                    transaction.Replace(Resource.Id.frame_container2, newYears, "year");
                    transaction.Commit();

                    SupportFragmentManager.ExecutePendingTransactions();
               }
          }
          /*
           * Because this method implements YearListener it must have this method.
           * This method takes a year selected in the spinner by the user, gets the
           * Parts corresponding to that year, creates a new SearchPartsFragment
           * that has the resulting parts and then replaces the proper container with
           * the new SearchPartsFragment.
           */
          public async void passYear(string yearIn)
          {
               if (!year.Equals(yearIn))
               {
                    year = yearIn;
                    mParts = await getParts(year);

                    global::Android.Support.V4.App.FragmentTransaction transaction = SupportFragmentManager.BeginTransaction();
                    SearchPartFragment newParts = SearchPartFragment.newInstance(mParts);
                    newParts.setListener(this);

                    transaction.Replace(Resource.Id.frame_container3, newParts, "part");
                    transaction.Commit();

                    SupportFragmentManager.ExecutePendingTransactions();
               }
          }
           /*
            * Because this method implements PartListener it must have this method.
            * This method takes a part selected in the spinner by the user, takes
            * the make, year, and part and passes them to the search() method of this
            * class.
            */
          public void passPart(string partIn)
          {
               part = partIn;
               if (this != null)
               {
                    this.search(make, year, part);
               }
          }
          /*
           * Passes in the make and then uses it as a paramenter
           * to the proper API class method.
           */
          private async Task<List<string>> getYears(string make)
          {
               List<string> years = await API.GetPickerData(make);
               return years;
          }

          /*
           * Passes in the make and year and then uses them as the paramenters
           * to the proper API class method.
           */
          private async Task<List<string>> getParts(string year)
          {
               List<string> parts = await API.GetPickerData(year, make);
               parts = parts.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
               return parts;
          }
	}
		
}

