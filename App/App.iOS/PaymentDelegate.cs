using System;
using System.Threading.Tasks;
using Foundation;
using PayPalMobileXamarinBindings;
using BigTed;
using UIKit;

namespace App.iOS
{
	public class PaymentDelegate : PayPalPaymentDelegate
	{
		Part part;

		public PaymentDelegate (Part part)
		{
			this.part = part;
		}

		public override void PayPalPaymentDidCancel (PayPalPaymentViewController paymentViewController)
		{
			paymentViewController.DismissViewController (true, null);
		}

		public override void DidCompletePayment (PayPalPaymentViewController paymentViewController, PayPalPayment completedPayment)
		{
			VerifyCompletedPayment (completedPayment);

			paymentViewController.DismissViewController (true, null);
		}

		// Send confirmation to your server; your server should verify the proof of payment
		// and give the user their goods or services. If the server is not reachable, save
		// the confirmation and try again later.
		private async Task VerifyCompletedPayment (PayPalPayment completedPayment) 
		{
			// We should send this to our server to ensure we store all payments made
			var error = new NSError ();
			var confirmation = completedPayment.Confirmation.ToString ();
			var jsonConfirmation = NSJsonSerialization.Serialize (completedPayment.Confirmation, NSJsonWritingOptions.PrettyPrinted, out error).ToString ();
			Console.WriteLine (jsonConfirmation);

			var paymentStatus = await API.VerifyCompletedPayment (jsonConfirmation, part);
			Console.WriteLine (paymentStatus);
			var alert = new UIAlertView ("Payment Result", paymentStatus, null, "Okay", null);
			alert.Show ();
		}
	}
}