
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
	public class SearchMakeFragment : global::Android.Support.V4.App.Fragment
	{
		public interface MakeListener 
		{			
			void passMake (string make);
		}
		private MakeListener mListener;

		public void setListener(MakeListener listener)
		{
			mListener = listener;
		}

//		public override async void OnCreate (Bundle savedInstanceState)
//		{
//			base.OnCreate (savedInstanceState);
//
//		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.MakeFragment, container, false);
			var makes = new List<string> (new string[] { "Honda", "Kawasaki", "Suzuki", "Yamaha" });
			ArrayAdapter<string> makeAdapter = new ArrayAdapter<string> (this.Activity, global::Android.Resource.Layout.SimpleListItem1, makes);
			Spinner makeSpinner =  view.FindViewById<Spinner> (Resource.Id.make_spinner);
			makeSpinner.Adapter = makeAdapter;
			TextView header = view.FindViewById<TextView> (Resource.Id.makeHeader);
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			header.SetTypeface (f, TypefaceStyle.Normal);

			makeSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (ItemSelectedHandler);
			
			return view;
		}
		void ItemSelectedHandler( object sender, AdapterView.ItemSelectedEventArgs e) 
		{
			Spinner spinner = (Spinner)sender;
			var makeToPass = Convert.ToString (spinner.GetItemAtPosition (e.Position));
			mListener.passMake (makeToPass);

		}
	}
}

