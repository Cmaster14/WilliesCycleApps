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
		UILabel yearLabel;
		PickerButton yearButton;
		PickerButton yearButton2;
		UIPickerView yearPicker;
		UIPickerView yearPicker2;

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
			BackgroundColor = UIColor.Clear.FromHexString ("#094074", 1.0f);

			yearLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 30f),
				Frame = new CGRect (0, 0, this.Bounds.Width, 40),
				Text = "Choose a range of years.",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			yearButton = new PickerButton {
				Frame = new CGRect (20, Frame.Height*1/8 + 10, this.Bounds.Width/2 - 40, 30)
			};

			yearButton2 = new PickerButton {
				Frame = new CGRect (this.Bounds.Width/2 + 20, Frame.Height*1/8 + 10, this.Bounds.Width/2 - 40, 30)
			};

			yearButton.SetTitleColor (UIColor.Clear.FromHexString("#9B9B9B", 1.0f), UIControlState.Normal);
			yearButton2.SetTitleColor (UIColor.Clear.FromHexString("#9B9B9B", 1.0f), UIControlState.Normal);

			yearPicker = new UIPickerView {
				Frame = new CGRect (0, 50, this.Bounds.Width/2, 40),
				Hidden = true,
				Model = new YearPickerViewModel (new List<string> { "Loading Years..." }, yearButton)
			};

			yearPicker2 = new UIPickerView {
				Frame = new CGRect (this.Bounds.Width/2, 50, this.Bounds.Width/2, 40),
				Hidden = true,
				Model = new YearPickerViewModel (new List<string> { "Loading Years..." }, yearButton2)
			};

			buttonClickable = false;

			Add (yearLabel);
			Add (yearButton);
			Add (yearButton2);
			Add (yearPicker);
		}

		private void SetupEventHandlers ()
		{
			yearButton.TouchUpInside += (sender, e) => {
				if (buttonClickable) {
					yearPicker.Hidden = false;
					yearButton.Hidden = true;
				} else {
					var alert = new UIAlertView ("Improper Order of Selection", "Choose a Make before selecting the Year.", null, "Okay", null);
					alert.Show ();
				}
			};
			yearButton2.TouchUpInside += (sender, e) => {
				if (buttonClickable) {
					yearPicker2.Hidden = false;
					yearButton2.Hidden = true;
				} else {
					var alert = new UIAlertView ("Improper Order of Selection", "Choose a Make before selecting the Year.", null, "Okay", null);
					alert.Show ();
				}
			};
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
						//yearPicker2.Model = new YearPickerViewModel (years, yearButton2);
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

				/*if (e.PropertyName == "Year2") { 
					yearButton2.SetTitle (SearchParameters.Year2, UIControlState.Normal);
				}*/
			};
		}
	}
}