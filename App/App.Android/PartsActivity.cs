using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using App.Portable;
using PayPal.Droid.Sdk.Payments;

namespace App.Android
{
     [Activity(Label = "Search Results ", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait, Theme = "@android:style/Theme.Holo.Light")]
	public class PartsActivity : Activity, dialog_list_item.DialogListener
	{
		string[] searchCriteria = null;
		ListView listview;
		Part selectedPart;
		List<Part> parts;
		PayPalConfiguration config = new PayPalConfiguration ()
			.Environment(PayPalConfiguration.EnvironmentProduction)
			.ClientId("AekAtpGCqK5rRyQWjkkGLeB_xUBMMMlVy6LZAOksXyL_JTy_j12zU0cWH_u2AO-xxGpdTeekj-Z9GfD6").AcceptCreditCards(false);
		
		protected override async void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			RequestWindowFeature (WindowFeatures.ActionBar);
			ActionBar.SetIcon (global::Android.Resource.Color.Transparent);
			SetContentView (Resource.Layout.PartsListLayout);

			listview = FindViewById<ListView> (Resource.Id.List);
			searchCriteria = Intent.GetStringArrayExtra ("search");
			var hasExtra = Intent.HasExtra("search");
			parts = await FetchPartsFromServer ();
			listview.Adapter = new PartListViewAdapter (this, parts);
			listview.ItemClick += OnListItemClick;
               try
               {
                    this.ActionBar.Title = "Search Results for " + parts[0].Make + " " + parts[0].PartName.Substring(0, parts[0].PartName.LastIndexOf('-'));
               }
               catch
               {
                    this.ActionBar.Title = "Search Results for " + parts[0].Make + " " + parts[0].PartName;
               }
               this.ActionBar.Subtitle = " from " + searchCriteria[1] + " to " + searchCriteria[3];
		}
		protected override void OnDestroy ()
		{
			StopService(new Intent(this, typeof(PayPalService)));
			base.OnDestroy ();
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			selectedPart = parts[e.Position];
			FragmentTransaction transaction = FragmentManager.BeginTransaction();
			dialog_list_item itemDialog = new dialog_list_item ();
			itemDialog.SetStyle (DialogFragmentStyle.NoTitle, 0);
			itemDialog.setListener (this);
			Bundle bdl = new Bundle ();
			bdl.PutString ("make", selectedPart.Make);
			bdl.PutString ("year", selectedPart.Year);
			bdl.PutString ("partName", selectedPart.PartName);
			bdl.PutString ("price", selectedPart.Price);
			itemDialog.Arguments = bdl;
			itemDialog.SetMenuVisibility (false);
			itemDialog.Show (transaction, "dialog");
		}

		private async Task<List<Part>> FetchPartsFromServer ()
		{
               var listOfParts = new List<Part>();
               for (int i = int.Parse(searchCriteria[1]); i <= int.Parse(searchCriteria[3]); i++)
               {
                    listOfParts.AddRange(await API.GetParts (searchCriteria[2], searchCriteria[0], i.ToString()));
               }
			return listOfParts;
		}

		public void contactPressed (bool pressed) 
		{
			if (pressed) {
				launchEmail ();
			}
		}

		public void purchasePressed (bool pressed)
		{
			Intent serviceIntent = new Intent (this, typeof(PayPalService));
			serviceIntent.PutExtra (PayPalService.ExtraPaypalConfiguration, config);
			StartService (serviceIntent);
			if (pressed) {
				var partNameForReal = string.Format ("{0} {1} {2} {3}", selectedPart.Year, selectedPart.Make, selectedPart.Model, selectedPart.PartName);
				PayPalPayment payment = new PayPalPayment (new Java.Math.BigDecimal(selectedPart.Price), "USD", partNameForReal, PayPalPayment.PaymentIntentSale);
				payment.EnablePayPalShippingAddressesRetrieval (true);
				Intent intent = new Intent (this, typeof(PaymentActivity));
				intent.PutExtra (PayPalService.ExtraPaypalConfiguration, config);
				intent.PutExtra (PaymentActivity.ExtraPayment, payment);
				StartActivityForResult (intent, 0);
			}
		}	

		private void launchEmail()
		{
			var email = new Intent (global::Android.Content.Intent.ActionSend);
			email.PutExtra (global::Android.Content.Intent.ExtraEmail, new string[] { "dan@willies.com" });
			email.PutExtra (global::Android.Content.Intent.ExtraSubject, "Information Request");
			string message = string.Format ("Requesting more information about:\n ID:{0} - Year:{1} - Make:{2} - Model:{3} - Part:{4} - Part#:{5} - Interchange:{6} - Price:{7} - Inventory Loc:{8} \n\nCustomer Question: \n\n",
				                 selectedPart.ID.ToString (), selectedPart.Year, selectedPart.Make, selectedPart.Model, selectedPart.PartName, selectedPart.PartNumber, selectedPart.Interchange,
				                 selectedPart.Price, selectedPart.Location);
			email.PutExtra (global::Android.Content.Intent.ExtraText, message);
			email.SetType ("message/rfc822");
			StartActivity (email);
			
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok) {
				PaymentConfirmation confirm = (PaymentConfirmation)data.GetParcelableExtra (PaymentActivity.ExtraResultConfirmation);
				if (confirm != null) {
					try {
						Log.Info ("payment", confirm.ToJSONObject ().ToString (4));

						//API.VerifyCompletedPayment(confirm.ToJSONObject().ToString(), selectedPart);

					} catch (Exception e) {
						Log.Error ("payment", "an extremely unlikely failure occurred: ", e);
					}
				} else if (resultCode == Result.Canceled) {
					Log.Info ("payment", "The user canceled");
				} else if (resultCode == (Result) PaymentActivity.ResultExtrasInvalid) {
					Log.Info ("payment", "An invalid Payment or PayPalConfiguration was submitted, Please see the docs");
				}
		}
	}
	}
}

