using System;
using System.Globalization;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;
using MessageUI;
using Newtonsoft.Json;
using PayPalMobileXamarinBindings;
using Plugin.Connectivity;

namespace App.iOS
{
	public class DetailViewController : UIViewController
	{
		Part part;
		string partString;
		UIGestureRecognizer payTextTouched;

		PayPalConfiguration paymentConfiguration;
		PayPalPaymentDelegate paymentDelegate;

		public DetailViewController (Part selectedPart)
		{
			part = selectedPart;
			partString = string.Format ("{0} {1} {2}", part.Year, part.Make, part.Model);

			Title = "Part Details";

			paymentDelegate = new PaymentDelegate (part);
			paymentConfiguration = new PayPalConfiguration () {
				AcceptCreditCards = false,
				LanguageOrLocale = "en",
				MerchantName = "Willie's Cycles",
				MerchantUserAgreementURL = new NSUrl (Path.Combine (NSBundle.MainBundle.BundlePath, "Licensure.html"), false),
				MerchantPrivacyPolicyURL = NSUrl.FromString ("https://www.google.com"),
				PayPalShippingAddressOption = PayPalShippingAddressOption.PayPal
			} ;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			PayPalMobile.PreconnectWithEnvironment ("PayPalEnvironmentProduction");
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White;

			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
			part.PartName.ToLower ();

			var scrollView = new UIScrollView {
				Frame = new CGRect (0, 0, 320, View.Frame.Height * 1.5)
			} ;

			var partNameLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 25f),
				Frame = new CGRect (20, 5, View.Bounds.Width, 30),
				Text = textInfo.ToTitleCase (part.PartName.ToLower ())
			} ;

			var partMakeLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 23f),
				Frame = new CGRect (20, 35, View.Bounds.Width, 25),
				Text = partString
			};

			var priceLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 24f),
				Frame = new CGRect (View.Bounds.Width - 80, 17.5, 70, 30),
				Text = string.Format ("${0}", part.Price),
				TextAlignment = UITextAlignment.Right
			} ;

			var williesGuarentee = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 15f),
				Frame = new CGRect (0, 65, View.Bounds.Width, 20),
				Text = "Willie's Guarentee",
				TextAlignment = UITextAlignment.Center
			} ;

			var guarenteeOne = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 11f),
				Frame = new CGRect (20, 85, View.Bounds.Width - 40, 50),
				Lines = 10,
				Text = "Why go anywhere else? With over 10 million satisified customers in more than 28 years, Willie’s is your best bet for the quality part you are looking for."
			} ;

			var guarenteeTwo = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 11f),
				Frame = new CGRect (20, 140, View.Bounds.Width - 40, 50),
				Lines = 5,
				Text = "Willie’s ships parts daily to many satisified customers. We can ship international, next day air, two day air, three day select, and to post office boxes."
			} ;

			var guarenteeThree = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 11f),
				Frame = new CGRect (20, 205, View.Bounds.Width - 40, 50),
				Lines = 5,
				Text = "We give full refunds or exchange on parts arriving defective and will accept returns on incorrect parts. We do have a restock fee of only 20% if you misorder a part."
			} ;

			var contactButton = new ContactUsButton {
				Frame = new CGRect (40, 260, View.Bounds.Width - 80, 40)
			} ;
			contactButton.SetTitle ("Contact Us", UIControlState.Normal);
			contactButton.SetTitleColor (UIColor.White, UIControlState.Normal);

			contactButton.TouchUpInside += CancelButtonTapped;

			var payButton = new SearchButton {
				Frame = new CGRect (40, 310, View.Bounds.Width - 80, 40)
			} ;
			payButton.SetTitle ("Buy Part", UIControlState.Normal);
			payButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			payButton.TouchUpInside += PaymentButtonTapped;

			var payText = new UILabel {
				Frame = new CGRect (40, 350, View.Bounds.Width - 80, 50),
				Text = "By purchasing part(s) from Willie's Cycles, you agree to the Terms of Service (tap to view).",
				Font = UIFont.FromName ("SegoeUI-Light", 12f),
				Lines = 5,
				TextAlignment = UITextAlignment.Center,
				UserInteractionEnabled = true
			};

			payTextTouched = new UITapGestureRecognizer (PayTextTapped) {
				NumberOfTapsRequired = 1
			};
			payText.AddGestureRecognizer (payTextTouched);

			scrollView.Add (partNameLabel);
			scrollView.Add (priceLabel);
			scrollView.Add (partMakeLabel);
			scrollView.Add (williesGuarentee);
			scrollView.Add (guarenteeOne);
			scrollView.Add (guarenteeTwo);
			scrollView.Add (guarenteeThree);
			scrollView.Add (contactButton);
			scrollView.Add (payButton);
			scrollView.Add (payText);
			scrollView.ContentSize = new CGSize (View.Frame.Width, View.Frame.Height * 1.25);

			View.Add (scrollView);
		}

		private void CancelButtonTapped (object sender, EventArgs e)
		{
			if (MFMailComposeViewController.CanSendMail) {
				var mailController = new MFMailComposeViewController ();
				mailController.SetToRecipients (new string[] { "willie@williescycle.com" });
				mailController.SetSubject ("Part Information Inquiry");
				mailController.SetMessageBody ("", false);
				mailController.Finished += (object s, MFComposeResultEventArgs args) => {
					DismissViewController (true, null);
				} ;

				PresentViewController (mailController, true, null);
			}  else {
				var alert = new UIAlertView ("Whoops!", "There was an error. To contact us, email us at willie@williescycle.com.", null, "Okay", null);
				alert.Show ();
			}
		}

		private void PaymentButtonTapped (object sender, EventArgs e)
		{
			var connected = CrossConnectivity.Current.IsConnected;
			if (connected) {
				PresentPayment ();
			} else {
				var alert = new UIAlertView ("No Internet Connection", "Please establish an internet connection before buying parts.", null, "Okay", null);
				alert.Show ();
			}
		}

		private void PresentPayment ()
		{
			var partNameForReal = string.Format ("{0} {1} {2} {3}", part.Year, part.Make, part.Model, part.PartName);

			var payment = new PayPalPayment () {
				Amount = new NSDecimalNumber (part.Price.ToString ()),
				CurrencyCode = "USD",
				Intent = PayPalPaymentIntent.Sale,
				ShortDescription = partNameForReal,
				Items = new NSObject[] { new PayPalItem { Currency = "USD", Name = partNameForReal, Price = new NSDecimalNumber (part.Price), Quantity = 1  } }
			} ;

			if (!payment.Processable) {
				var alert = new UIAlertView ("Error", "Could not create payment for this item.", null, "Okay", null);
				alert.Show ();
			}

			var paymentViewController = new PayPalPaymentViewController (payment, paymentConfiguration, paymentDelegate);
			PresentViewController (paymentViewController, true, null);
		}

		private void PayTextTapped ()
		{
			NavigationController.PushViewController (new InlineDisclaimerViewController (), true);
		}
	}
}