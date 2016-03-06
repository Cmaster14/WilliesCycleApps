
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
     /*
      * This is the class that handles the Make search fragment of Willies Cycle Application.
      */
	public class SearchYearFragment : global::Android.Support.V4.App.Fragment
	{
		List<string> mYears = new List<string>(){"None"};

          //Variable to describle which Object is this Fragment's listener.
		private YearListener mListener;

          //This is an interface to be implemented by a parent class that uses this Fragment.
		public interface YearListener 
		{
			void passYear(string year);
		}

          //Called by an Object to become the listener of this class.
		public void setListener(YearListener listener)
		{
			mListener = listener;
		}
          //If a Fragment is created and has argments, it sets the List from the arguments
          //as the List of the Fragment and be put in its Spinner.
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

               //Adapter to feed the Spinner the proper Lists
			ArrayAdapter<string> yearAdapter = new ArrayAdapter<string> (this.Activity, global::Android.Resource.Layout.SimpleListItem1, mYears);
			Spinner yearSpinner =  view.FindViewById<Spinner> (Resource.Id.year_spinner);
			yearSpinner.Adapter = yearAdapter;

               //Creates the header to specify the Typeface of it.
               TextView header = view.FindViewById<TextView> (Resource.Id.yearHeader);
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			header.SetTypeface (f, TypefaceStyle.Normal);

               //This calls the method below and actually sets based on the current item that is selected in the spinner.
			yearSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (ItemSelectedHandler);

			return view;
		}
          /*
           * Is called in the on createView above.
           * It gets the Year that is currently selected in the spinner and then passes
           * it back to the listener that created the Fragment.
           */
		void ItemSelectedHandler( object sender, AdapterView.ItemSelectedEventArgs e) 
		{
			Spinner spinner = (Spinner)sender;
			var yearToPass = Convert.ToString (spinner.GetItemAtPosition (e.Position));
			mListener.passYear (yearToPass);


			//TO-DO:
			//Need to share data between the fragments, most likely through savedstate in the activity
		}
          /*
           * Creates a new Year Fragment with the specified paramenter for the
           * parts list, saves it in a bundle, and then puts the bundle in the
           * fragment arguments.
           */
		public static SearchYearFragment newInstance(List<string> yearsIn)
		{
			SearchYearFragment yf = new SearchYearFragment ();

			Bundle bdl = new Bundle ();
			bdl.PutStringArrayList ("years", yearsIn);
			yf.Arguments = bdl;

			return yf;
		}
	}
}

