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

		List<string> years = new List<string>();
		List<string> years2 = new List<string>();

		SearchViewController searchViewController;

		bool buttonClickable;
		bool button2Clickable;

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
				Frame = new CGRect (20, Frame.Height*1/8, this.Bounds.Width/2 - 40, 50),
				Hidden = true,
				Model = new YearPickerViewModel (new List<string> { "Loading Years..." }, yearButton)
			};


			// This is the second picker that denotes the highest year in the range of years
			yearPicker2 = new UIPickerView {
				Frame = new CGRect (this.Bounds.Width/2 + 20, Frame.Height*1/8, this.Bounds.Width/2 - 40, 50),
				Hidden = true,
				Model = new Year2PickerViewModel (new List<string> { "Loading Years..." }, yearButton2)
			};

			buttonClickable = false;
			button2Clickable = false;

			Add (yearLabel);
			Add (yearButton);
			Add (yearButton2);
			Add (yearPicker);
			Add (yearPicker2);
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
				if (buttonClickable && button2Clickable) {
					yearPicker2.Hidden = false;
					yearButton2.Hidden = true;
				} else {
					var alert = new UIAlertView ("Improper Order of Selection", "Choose a starting year before selecting the ending year.", null, "Okay", null);
					alert.Show ();
				}
			};
		}

		private void SetupPropertyChanged ()
		{
			SearchParameters.PropertyChanged += async (sender, e) => {
				if (e.PropertyName == "Make") {
					buttonClickable = false;
					button2Clickable = false;

					var connected = CrossConnectivity.Current.IsConnected;
					if (connected) {
						BTProgressHUD.Show ("Filtering Parts");
						years = await API.GetPickerData (SearchParameters.Make);
						yearPicker.Model = new YearPickerViewModel (years, yearButton);
						years2.Clear();
						BTProgressHUD.Dismiss ();

					} else {
						var alert = new UIAlertView ("No Internet Connection", "Please establish an internet connection before querying for parts.", null, "Okay", null);
						alert.Show ();
					}

					buttonClickable = true;
				}

				else if (e.PropertyName == "Year") {
					button2Clickable = false;
					yearButton.SetTitle (SearchParameters.Year, UIControlState.Normal);

					// This portion was added on 4/23/2016 to give the set of selectable years in the second picker
					// The set will start at the selected year of the first picker
					// Edited on 4/25 after finally being tested. years.IndexOf(...) was added in today
					if (!SearchParameters.Year.Equals(""))
					{
						years2.Clear();
						for(int i = years.IndexOf(SearchParameters.Year); i < years.Count; i++)
						{
							years2.Add(years[i]);
						}
						yearPicker2.Model = new Year2PickerViewModel (years2, yearButton2);
						SearchParameters.Year2 = SearchParameters.Year;
						yearButton2.SetTitle (SearchParameters.Year2, UIControlState.Normal);
						button2Clickable = true;
					}
				}
				else if (e.PropertyName == "Year2")
				{
					yearButton2.SetTitle (SearchParameters.Year2, UIControlState.Normal);
				}
			};
		}
	}
}