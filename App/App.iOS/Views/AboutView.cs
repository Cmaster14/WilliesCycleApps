using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class AboutView : UIView
	{
		UILabel williesLabel;
		UILabel aboutLabel;
		UILabel contactUsLabel;
		UITextView emailLabel;
		UITextView phoneLabel;

		public AboutView (CGRect frame)
		{
			Frame = frame;
			SetupUserInterface ();
		}

		private void SetupUserInterface ()
		{
			BackgroundColor = UIColor.Clear.FromHexString ("#4997D0", 1.0f);

			williesLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 30f),
				Frame = new CGRect (0, 20, Frame.Width, 35),
				Text = "Willie's Cycles",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			aboutLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 15f),
				Frame = new CGRect (20, 50, Frame.Width - 40, 200),
				Lines = 15,
				Text = "Willie's Cycles offers the highest quality of used motorcycles parts on the market today. Since 1977, we have provided superior service to our customers and have assisted them in achieving their goals. Our 28 plus years of experience and commitment to excellence have earned us the reputation as the one of the best motorcycle salvage businesses in the nation.",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			contactUsLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 28f),
				Frame = new CGRect (0, 250, this.Bounds.Width, 30),
				Text = "Contact Us",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			emailLabel = new UITextView {
				BackgroundColor = UIColor.Clear,
				Editable = false,
				DataDetectorTypes = UIDataDetectorType.All,
				Font = UIFont.FromName ("SegoeUI-Light", 18f),
				Frame = new CGRect (0, 285, this.Bounds.Width, 30),
				Text = "dan@williescycle.com",
				TextAlignment = UITextAlignment.Center,
				TintColor = UIColor.White
			};

			phoneLabel = new UITextView {
				BackgroundColor = UIColor.Clear,
				Editable = false,
				DataDetectorTypes = UIDataDetectorType.All,
				Font = UIFont.FromName ("SegoeUI-Light", 18f),
				Frame = new CGRect (0, 320, this.Bounds.Width, 30),
				Text = "800-334-4045",
				TextAlignment = UITextAlignment.Center,
				TintColor = UIColor.White
			};

			Add (williesLabel);
			Add (aboutLabel);
			Add (contactUsLabel);
			Add (emailLabel);
			Add (phoneLabel);
		}
	}
}

