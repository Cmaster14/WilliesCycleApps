using System;
using CoreGraphics;
using Foundation;
using UIKit;
using FlyoutNavigation;

namespace App.iOS
{
	public class InlineDisclaimerViewController : UIViewController
	{
		UIScrollView scrollView;
		DisclaimerView disclaimerView;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Terms of Service";

			scrollView = new UIScrollView {
				BackgroundColor = UIColor.Black,
				Frame = new CGRect (0, 0, 320, View.Bounds.Height * 3)
			};

			disclaimerView = new DisclaimerView (View.Bounds) {
				Frame = new CGRect (0, 0, View.Bounds.Width, View.Bounds.Height)
			};

			scrollView.Add (disclaimerView);

			View.Add (scrollView);
		}

		public override bool PrefersStatusBarHidden ()
		{
			return true;
		}
	}
}