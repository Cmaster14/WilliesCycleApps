﻿using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	[Register ("MakeView")]
	public class MakeView : UIView
	{
		UILabel makeLabel;
		PickerButton makeButton;
		UIPickerView makePicker;

		SearchViewController searchViewController;

		public MakeView (CGRect frame, SearchViewController searchViewController)
		{
			Frame = frame;
			this.searchViewController = searchViewController;

			SetupUserInterface ();
			SetupEventHandlers ();
		}

		private void SetupUserInterface ()
		{
			BackgroundColor = UIColor.Clear.FromHexString ("#094074", 1.0f);

			makeLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 32f),
				Frame = new CGRect (0, 0, Frame.Width, 40),
				Text = "Choose a make.",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			makeButton = new PickerButton {
				Frame = new CGRect (40, Frame.Height*1/8 + 10, Frame.Width - 80, 30)
			};
			makeButton.SetTitleColor (UIColor.Clear.FromHexString("#9B9B9B", 1.0f), UIControlState.Normal);

			makePicker = new UIPickerView {
				Frame = new CGRect (0, Frame.Height*1/8, Frame.Width, 40),
				Hidden = true,
				Model = new MakePickerViewModel (makeButton)
			};

			Add (makeLabel);
			Add (makeButton);
			Add (makePicker);
		}

		private void SetupEventHandlers ()
		{
			makeButton.TouchUpInside += (sender, e) => {
				makePicker.Hidden = false;
				makeButton.Hidden = true;
			};
		}
	}
}