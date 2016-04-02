
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
	public class MyPagerAdapter : FragmentStatePagerAdapter, SearchMakeFragment.MakeListener, SearchYearFragment.YearListener, SearchPartFragment.PartListener
	{
		private List<global::Android.Support.V4.App.Fragment> fragments;
		int fragmentCount;
		private List<string> mYears = new List<string>(){"none"};
		private List<string> mParts = new List<string>(){"none"};
		private string make = "make", year = "year", part = "part";
		private AdapterListener mListener = null;

		public interface AdapterListener
		{
			void search (string make, string year, string part);
		}

		public void setListener(AdapterListener listener)
		{
			mListener = listener;
		}

		public MyPagerAdapter(global::Android.Support.V4.App.FragmentManager fm) : base(fm)
		{
			this.fragments = new List<global::Android.Support.V4.App.Fragment>();

			fragments.Add (new SearchMakeFragment ());
			fragments.Add (new SearchYearFragment ());
			fragments.Add (new SearchPartFragment ());


			fragmentCount = fragments.Count;
		}

		public override int Count {
			get {
				return fragmentCount;
			}
		}

		public int getCount()
		{
			return fragmentCount;
		}

		public override global::Android.Support.V4.App.Fragment GetItem (int position)
		{
			switch (position) 
			{
			case 0:
				SearchMakeFragment mf = new SearchMakeFragment ();
				mf.setListener (this);
				return mf;
			case 1:
				SearchYearFragment yf = SearchYearFragment.newInstance (mYears);
				yf.setListener (this);
				return yf;
			case 2:
				SearchPartFragment pf = SearchPartFragment.newInstance (mParts);
				pf.setListener (this);
				return pf;
			}
			return fragments [position];
		}

		public override int GetItemPosition(Java.Lang.Object none)
		{
			return PositionNone;
		}

		public async void passMake(string makeIn)
		{
			if (!make.Equals (makeIn)) {
				make = makeIn;
				mYears = await getYears (make);
				NotifyDataSetChanged ();
			}
		}

		public async void passYear(string yearIn, string yearhigh)
		{
			if (!year.Equals(yearIn))
			{
				year = yearIn;
				mParts = await getParts (year);
				NotifyDataSetChanged ();
			}
		}

		public void passPart(string partIn)
		{
			part = partIn;
			if (mListener != null) 
			{
				mListener.search (make, year, part);
			}

		}

		private async Task<List<string>> getYears(string make)
		{
			List<string> years = await API.GetPickerData (make);

			return years;
		}

		private async Task<List<string>> getParts(string year)
		{
			List<string> parts = await API.GetPickerData (year, make);
			parts = parts.Where (s => !string.IsNullOrWhiteSpace (s)).Distinct ().ToList ();
			return parts;
		}
	}
}

