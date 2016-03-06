
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
      * This is the class that handles the Part search fragment of Willies Cycle Application.
      */
	public class SearchPartFragment : global::Android.Support.V4.App.Fragment
	{
		List<string> mParts = new List<string>(){"None"};

          //Variable to describle which Object is this Fragment's listener.
		private PartListener mListener;
		string part = "none";

          //This is an interface to be implemented by a parent class that uses this Fragment.
		public interface PartListener
		{
			void passPart(string part);
		}

          //Called by an Object to become the listener of this class.
		public void setListener(PartListener listener)
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
					List<string> parts = new List<string>(args.GetStringArrayList ("parts"));
					if (parts != null) 
					{
						mParts = parts;
					}
				}
				catch (Exception e){
				}

			}

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.PartFragment, container, false);

               //Adapter to feed the Spinner the proper Lists
			ArrayAdapter<string> partAdapter = new ArrayAdapter<string> (this.Activity, global::Android.Resource.Layout.SimpleListItem1, mParts);
			Spinner partSpinner =  view.FindViewById<Spinner> (Resource.Id.part_spinner);
			partSpinner.Adapter = partAdapter;

               //Creates the header to specify the Typeface of it.
			TextView header = view.FindViewById<TextView> (Resource.Id.partHeader);
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			header.SetTypeface (f, TypefaceStyle.Normal);

               //This calls the method below and actually sets based on the current item that is selected in the spinner.
			partSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (ItemSelectedHandler);

               //This is the implementation of the search button in the page fragment.
			Button searchButton = view.FindViewById<Button> (Resource.Id.Search_button);
			searchButton.SetTypeface (f, TypefaceStyle.Normal);

               //When the button is clicked, the passPart method implemented by the Listener is called.
			searchButton.Click += delegate {
				mListener.passPart (part);

			};

			return view;
		}
          /*
           * Is called in the on createView above.
           * It gets the Part that is currently selected in the spinner and then passes
           * it back to the listener that created the Fragment.
           */
		void ItemSelectedHandler( object sender, AdapterView.ItemSelectedEventArgs e) 
		{
			Spinner spinner = (Spinner)sender;
			var partToPass = Convert.ToString (spinner.GetItemAtPosition (e.Position));
			part = partToPass;
		}
          /*
           * Creates a new Part Fragment with the specified paramenter for the
           * parts list, saves it in a bundle, and then puts the bundle in the
           * fragment arguments.
           */
		public static SearchPartFragment newInstance(List<string> partsIn)
		{
			SearchPartFragment pf = new SearchPartFragment ();

			Bundle bdl = new Bundle ();
			bdl.PutStringArrayList ("parts", partsIn);
			pf.Arguments = bdl;

			return pf;
		}
	}
}

