
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
	[Activity(Theme= "@android:style/Theme.Holo.Light.NoActionBar")]
	public class dialog_list_item : DialogFragment
	{
		string textHeader;
		private DialogListener mListener;

		public interface DialogListener 
		{
			void contactPressed (bool pressed);
			void purchasePressed (bool pressed);
		}

		public void setListener(DialogListener listener)
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
					string make = args.GetString ("make");
					string year = args.GetString ("year");
					string partName = args.GetString ("partName");
					string price = args.GetString ("price");
					textHeader = string.Format("{1} {0}\n{2}  ${3}", make, year, partName, price);
				}
				catch (Exception e){
				}

			}
		}
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			
			base.OnCreateView (inflater, container, savedInstanceState);
			var view = inflater.Inflate (Resource.Layout.item_dialog, container, false);
			TextView header = view.FindViewById<TextView> (Resource.Id.partText);
			header.Text = textHeader;
			Typeface f = Typeface.CreateFromAsset (Application.Context.Assets, "SegoeUILight.ttf");
			header.SetTypeface (f, TypefaceStyle.Normal);
			var contactButton = view.FindViewById<Button> (Resource.Id.dialog_contact);
			contactButton.SetTypeface (f, TypefaceStyle.Normal);
			contactButton.Click += delegate {
				mListener.contactPressed(true);
				Dismiss();
			};
			var purcahseButton = view.FindViewById<Button> (Resource.Id.dialog_purchase);
			purcahseButton.SetTypeface (f, TypefaceStyle.Normal);
			purcahseButton.Click += delegate {
				mListener.purchasePressed(true);
				Dismiss();

			};
			return view;
		}
	}
}

