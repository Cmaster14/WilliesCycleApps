using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	[Register ("MakeView")]
	public class MakeView : UIView
	{
		//UILabel stepOneLabel;
		UILabel makeLabel;
		PickerButton makeButton;
		UIPickerView makePicker;
		UIButton goDownButton;

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

			/*stepOneLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 42.5f),
				Frame = new CGRect (0, 70, Frame.Width, 45),
				Text = "Step 1",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};*/

			makeLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 32f),
				Frame = new CGRect (0, 0, Frame.Width, 40),
				Text = "Choose a make.",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			makeButton = new PickerButton {
				Frame = new CGRect (40, Frame.Height*1/5 + 10, Frame.Width - 80, 30)
			};
			makeButton.SetTitleColor (UIColor.Clear.FromHexString("#9B9B9B", 1.0f), UIControlState.Normal);

			makePicker = new UIPickerView {
				Frame = new CGRect (0, Frame.Height*1/5, Frame.Width, 40),
				Hidden = true,
				Model = new MakePickerViewModel (makeButton)
			};

			goDownButton = new UIButton {
				Font = UIFont.FromName ("SegoeUI-Light", 17f),
				Frame = new CGRect (0, this.Bounds.Height - 30, this.Bounds.Width, 15),
			};
			goDownButton.SetTitle ("Continue to \"Select a Year\".", UIControlState.Normal);
			goDownButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			goDownButton.Center = new CGPoint (this.Bounds.Width / 2, this.Bounds.Height - 30);
			//goDownButton.TouchUpInside += SetupGoDownTapped;

			//Add (stepOneLabel);
			Add (makeLabel);
			Add (makeButton);
			Add (makePicker);
			//Add (goDownButton);
		}

		private void SetupEventHandlers ()
		{
			makeButton.TouchUpInside += (sender, e) => {
				makePicker.Hidden = false;
				makeButton.Hidden = true;
			};
		}

		private void SetupGoDownTapped (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (SearchParameters.Make)) {
				var alertView = new UIAlertView ("Whoops", "Select a valid make before continuing.", null, "Okay", null);
				alertView.Show ();
			} else {
				//searchViewController.StepOneSwipeUp ();
			}
		}
	}
}