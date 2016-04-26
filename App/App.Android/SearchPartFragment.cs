
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
	public class SearchPartFragment : global::Android.Support.V4.App.Fragment
	{
		List<string> mParts = new List<string>(){"None"};
		private PartListener mListener;
		string part = "none";

		public interface PartListener
		{
			void passPart(string part);
		}

		public void setListener(PartListener listener)
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
			ArrayAdapter<string> partAdapter = new ArrayAdapter<string> (this.Activity, global::Android.Resource.Layout.SimpleListItem1, mParts);
			Spinner partSpinner =  view.FindViewById<Spinner> (Resource.Id.part_spinner);
			partSpinner.Adapter = partAdapter;
			TextView header = view.FindViewById<TextView> (Resource.Id.partHeader);
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			header.SetTypeface (f, TypefaceStyle.Normal);
			partSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (ItemSelectedHandler);
			Button searchButton = view.FindViewById<Button> (Resource.Id.Search_button);
			searchButton.SetTypeface (f, TypefaceStyle.Normal);
			searchButton.Click += delegate {
				mListener.passPart (part);

			};
               Button termsButton = view.FindViewById<Button> (Resource.Id.Terms_of_service);
               termsButton.SetTypeface(f, TypefaceStyle.Normal);
               termsButton.Click += delegate {
                    var terms = new Intent(this.Activity, typeof(TermsActivity));
                    StartActivity(terms);
                    Toast.MakeText(this.Activity, "Terms and Conditions", ToastLength.Short).Show();
               };
               Button licensure = view.FindViewById<Button> (Resource.Id.Licensure);
               licensure.SetTypeface(f, TypefaceStyle.Normal);
               licensure.Click += delegate {
                    var license = new Intent (this.Activity, typeof(LicensureActivity));
                    StartActivity(license);
                    Toast.MakeText(this.Activity, "Licensure", ToastLength.Short).Show();
               };

			return view;
		}
		void ItemSelectedHandler( object sender, AdapterView.ItemSelectedEventArgs e) 
		{
			Spinner spinner = (Spinner)sender;
			var partToPass = Convert.ToString (spinner.GetItemAtPosition (e.Position));
			part = partToPass;
		}
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

