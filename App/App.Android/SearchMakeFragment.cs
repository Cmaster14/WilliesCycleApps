
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using App.Portable;
using Android.Graphics;

namespace App.Android
{
     /*
      * This is the class that handles the Make search fragment of Willies Cycle Application.
      */
	public class SearchMakeFragment : global::Android.Support.V4.App.Fragment
	{
          //This is an interface to be implemented by a parent class that uses this Fragment.
		public interface MakeListener 
		{			
			void passMake (string make);
		}
          
          //Variable to describle which Object is this Fragment's listener.
		private MakeListener mListener;

          //Called by an Object to become the listener of this class.
		public void setListener(MakeListener listener)
		{
			mListener = listener;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.MakeFragment, container, false);

               //The Makes are desired to be hard coded in instead of fetched from the database because
               //these are the main 4 Makes Willie's deals in.
			var makes = new List<string> (new string[] { "Honda", "Kawasaki", "Suzuki", "Yamaha" });

               //Adapter to feed the Spinner the proper Lists
			ArrayAdapter<string> makeAdapter = new ArrayAdapter<string> (this.Activity, global::Android.Resource.Layout.SimpleListItem1, makes);
			Spinner makeSpinner =  view.FindViewById<Spinner> (Resource.Id.make_spinner);
			makeSpinner.Adapter = makeAdapter;

               //Creates the header to specify the Typeface of it.
			TextView header = view.FindViewById<TextView> (Resource.Id.makeHeader);
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			header.SetTypeface (f, TypefaceStyle.Normal);

               //This calls the method below and actually sets based on the current item that is selected in the spinner.
			makeSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (ItemSelectedHandler);
			
			return view;
		}

          /*
           * Is called in the on createView above.
           * It gets the ake that is currently selected in the spinner and then passes
           * it back to the listener that created the Fragment.
           */
		void ItemSelectedHandler( object sender, AdapterView.ItemSelectedEventArgs e) 
		{
			Spinner spinner = (Spinner)sender;
			var makeToPass = Convert.ToString (spinner.GetItemAtPosition (e.Position));
			mListener.passMake (makeToPass);

		}
	}
}

