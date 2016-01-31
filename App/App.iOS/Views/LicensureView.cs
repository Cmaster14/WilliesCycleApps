using System;
using System.IO;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class LicensureView : UIView
	{
		UIScrollView scrollView;
		UILabel licensureLabel;
		UIWebView webView;

		public LicensureView (CGRect frame)
		{
			Frame = frame;

			SetupUserInterface ();
		}

		private void SetupUserInterface ()
		{
			scrollView = new UIScrollView {
				BackgroundColor = UIColor.Clear.FromHexString ("#094074", 1.0f),
				Frame = new CGRect (0, 0, Frame.Width, Frame.Height * 2)
			};

			licensureLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 25f),
				Frame = new CGRect (0, 20, Frame.Width, 35),
				Text = "Open-Source",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			string localHtmlUrl = Path.Combine (NSBundle.MainBundle.BundlePath, "Licensure.html");
			var url = new NSUrl (localHtmlUrl, false);
			var request = new NSUrlRequest (url);

			webView = new UIWebView () {
				Frame = new CGRect (0, 55, Frame.Width, Frame.Height - 55)
			};
			webView.LoadRequest (request);

			scrollView.ContentSize = new CGSize (Frame.Width, Frame.Height * 2);

			scrollView.Add (licensureLabel);
			scrollView.Add (webView);
			Add (scrollView);
		}
	}
}

