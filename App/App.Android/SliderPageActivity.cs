
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;

namespace App.Android
{
	[Activity (Label = "Willie's Cycle", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]			
	public class SliderPageActivity : FragmentActivity, MyPagerAdapter.AdapterListener
	{
		private string[] searchString = new string[3]; //0: make, 1: year, 2: part

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature (WindowFeatures.ActionBar);
			ActionBar.Hide ();
			SetContentView (Resource.Layout.ViewPagerActivity);

			MyPagerAdapter pageAdapter = new MyPagerAdapter (SupportFragmentManager);
			pageAdapter.setListener (this);
			var pager = FindViewById<ViewPager> (Resource.Id.viewpager);
			pager.Adapter = pageAdapter;

			pager.SetCurrentItem (0, true);
		
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
		
	}
		
}

