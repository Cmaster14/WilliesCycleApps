﻿using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class DisclaimerView : UIView
	{
		UIScrollView scrollView;
		UILabel licensureLabel;
		UITextView textView;

		public DisclaimerView (CGRect frame)
		{
			Frame = frame;

			SetupUserInterface ();
		}

		private void SetupUserInterface ()
		{
			scrollView = new UIScrollView {
				BackgroundColor = UIColor.Clear.FromHexString ("#336699", 1.0f),
				Frame = new CGRect (0, 0, Frame.Width, Frame.Height)
			};

			licensureLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 30f),
				Frame = new CGRect (0, 20, Frame.Width, 35),
				Text = "Terms of Service",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			textView = new UITextView {
				BackgroundColor = UIColor.Clear,
				Font = UIFont.FromName ("SegoeUI-Light", 15f), 
				Frame = new CGRect (20, 50, Frame.Width - 40, Frame.Height * 1.75),
				Text = "Willie’s Cycle will assume no damage liability from the sales of used or new parts. This damage includes personal injury, property, vehicle, monetary, punitive, emotional, and mental damage. All risk and liability is assumed by the purchaser or the installer of any product sold by Willie’s Cycle. Willie’s Cycle cannot be held liable for any of the following reasons: damage to person or property, including damage or injury to driver, passenger, or any other person, animal or property damage that may occur have occurred after purchasing any product from Willie’s Cycle. Willie’s Cycle will only accept returns for parts to be used in store credit or, at our discretion. Willie’s Cycle will give refunds due to applications not being as described. Willie’s Cycle will only give credit or refund up to full purchase amount of the product. Willie’s Cycle may charge a restock fee for returned parts. This restock fee is a minimum of 20% of the purchase price. All parts are expected to have normal wear, and in no way do we consider used parts to be in any condition other than used functional parts with wear, unless otherwise noted. Any returns must be sent with return authorization. RA# may be granted by providing the following: Date of purchase, transaction number, year, model, make, and VIN number of vehicle. Returns can be rejected if purchase was made on account of buyer error. Amount of refund or credit is determined solely by Willie’s Cycle. Returned item must be received by Willie’s Cycle within 30 days of original purchase date. All returned items must be returned in the same condition as they were received.",
				TextColor = UIColor.White,
				UserInteractionEnabled = false
			};

			scrollView.ContentSize = new CGSize (Frame.Width, Frame.Height * 1.75);

			scrollView.Add (licensureLabel);
			scrollView.Add (textView);
			Add (scrollView);
		}
	}
}

