using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;
using Plugin.Connectivity;
using BigTed;

namespace App.iOS
{
	[Register ("YearView")]
	public class YearView : UIView
	{
		UIButton goUpButton;
		UIButton goDownButton;
		UILabel stepTwoLabel;
		UILabel yearLabel;
		PickerButton yearButton;
		UIPickerView yearPicker;

		SearchViewController searchViewController;

		bool buttonClickable;

		public YearView (CGRect frame, SearchViewController searchViewController)
		{
			Frame = frame;
			this.searchViewController = searchViewController;

			SetupUserInterface ();
			SetupEventHandlers ();
			SetupPropertyChanged ();
		}

		private void SetupUserInterface ()
		{
			BackgroundColor = UIColor.Clear.FromHexString ("#336699", 1.0f);

			goUpButton = new UIButton {
				Font = UIFont.FromName ("SegoeUI-Light", 17f),
				Frame = new CGRect (0, 5, this.Bounds.Width, 15),
			};
			goUpButton.SetTitle ("Go back to \"Select a Make\".", UIControlState.Normal);
			goUpButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			goUpButton.Center = new CGPoint (this.Bounds.Width / 2, 15);
			goUpButton.TouchUpInside += SetupGoUpTapped;

			stepTwoLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 42.5f),
				Frame = new CGRect (0, 70, this.Bounds.Width, 45),
				Text = "Step 2",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			yearLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 32f),
				Frame = new CGRect (0, 120, this.Bounds.Width, 40),
				Text = "Choose a year.",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			yearButton = new PickerButton {
				Frame = new CGRect (40, 175, this.Bounds.Width - 80, 30)
			};
			yearButton.SetTitleColor (UIColor.Clear.FromHexString("#9B9B9B", 1.0f), UIControlState.Normal);

			yearPicker = new UIPickerView {
				Frame = new CGRect (0, 165, this.Bounds.Width, 40),
				Hidden = true,
				Model = new YearPickerViewModel (new List<string> { "Loading Years..." }, yearButton)
			};

			goDownButton = new UIButton {
				Font = UIFont.FromName ("SegoeUI-Light", 17f),
				Frame = new CGRect (0, this.Bounds.Height - 30, this.Bounds.Width, 15),
			};
			goDownButton.SetTitle ("Continue to \"Select a Part\".", UIControlState.Normal);
			goDownButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			goDownButton.Center = new CGPoint (this.Bounds.Width / 2, this.Bounds.Height - 30);
			goDownButton.TouchUpInside += SetupGoDownTapped;

			buttonClickable = false;

			Add (goUpButton);
			Add (stepTwoLabel);
			Add (yearLabel);
			Add (yearButton);
			Add (yearPicker);
			Add (goDownButton);
		}

		private void SetupEventHandlers ()
		{
			yearButton.TouchUpInside += (sender, e) => {
				if (buttonClickable) {
					yearPicker.Hidden = false;
					yearButton.Hidden = true;
				} else {
					var alert = new UIAlertView ("One Moment Please", "Interaction with the Willie's Cycles inventory is taking longer than expected.", null, "Okay", null);
					alert.Show ();
				}
			};
		}
			
		private void SetupGoUpTapped (object sender, EventArgs e)
		{
			searchViewController.StepTwoSwipeDown ();
		}

		private void SetupGoDownTapped (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (SearchParameters.Make) || string.IsNullOrEmpty (SearchParameters.Year)) {
				var alertView = new UIAlertView ("Whoops", "Select a valid year before continuing.", null, "Okay", null);
				alertView.Show ();
			} else {
				searchViewController.StepTwoSwipeUp ();
			}
		}

		private void SetupPropertyChanged ()
		{
			SearchParameters.PropertyChanged += async (sender, e) => {
				if (e.PropertyName == "Make") {
					buttonClickable = false;

					var connected = CrossConnectivity.Current.IsConnected;
					if (connected) {
						BTProgressHUD.Show ("Filtering Parts");
						var years = await API.GetPickerData (SearchParameters.Make);
						yearPicker.Model = new YearPickerViewModel (years, yearButton);
						BTProgressHUD.Dismiss ();
					} else {
						var alert = new UIAlertView ("No Internet Connection", "Please establish an internet connection before querying for parts.", null, "Okay", null);
						alert.Show ();
					}

					buttonClickable = true;
				}

				if (e.PropertyName == "Year") {
					yearButton.SetTitle (SearchParameters.Year, UIControlState.Normal);
				}
			};
		}
	}
}