
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace App.Android
{
	public class SearchYearFragment : global::Android.Support.V4.App.Fragment
	{
		List<string> mYears = new List<string>(){"None"};
		private YearListener mListener;

		public interface YearListener 
		{
			void passYear(string year);
		}

		public void setListener(YearListener listener)
		{
			mListener = listener;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			var args = Arguments;
			if (args != null) 
			{
				try{
					List<string> years = new List<string>(args.GetStringArrayList ("years"));
					if (years != null) 
					{
						mYears = years;
					}
				}
				catch (Exception e){
				}

			}
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.YearFragment, container, false);
			ArrayAdapter<string> yearAdapter = new ArrayAdapter<string> (this.Activity, global::Android.Resource.Layout.SimpleListItem1, mYears);
			Spinner yearSpinner =  view.FindViewById<Spinner> (Resource.Id.year_spinner);
			yearSpinner.Adapter = yearAdapter;
			TextView header = view.FindViewById<TextView> (Resource.Id.yearHeader);
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			header.SetTypeface (f, TypefaceStyle.Normal);
			yearSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (ItemSelectedHandler);
			return view;
		}

		void ItemSelectedHandler( object sender, AdapterView.ItemSelectedEventArgs e) 
		{
			Spinner spinner = (Spinner)sender;
			var yearToPass = Convert.ToString (spinner.GetItemAtPosition (e.Position));
			mListener.passYear (yearToPass);


			//TO-DO:
			//Need to share data between the fragments, most likely through savedstate in the activity
		}

		public static SearchYearFragment newInstance(List<string> yearsIn)
		{
			SearchYearFragment yf = new SearchYearFragment ();

			Bundle bdl = new Bundle ();
			bdl.PutStringArrayList ("years", yearsIn);
			yf.Arguments = bdl;

			return yf;
		}
		public void setYears(List<string> yearsIn)
		{
			mYears = yearsIn;
		}
	}
}

