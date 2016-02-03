using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;
using BigTed;
using Plugin.Connectivity;

namespace App.iOS
{
	[Register ("PartNameView")]
	public class PartNameView : UIView
	{
		UIButton goUpButton;
		UILabel stepThreeLabel;
		UILabel partNameLabel;
		PickerButton partNameButton;
		UIPickerView partNamePicker;
		SearchButton searchButton;

		SearchViewController searchViewController;

		bool buttonClickable;

		public PartNameView (CGRect frame, SearchViewController searchViewController)
		{
			Frame = frame;
			this.searchViewController = searchViewController;

			SetupUserInterface ();
			SetupEventHandlers ();
			SetupPropertyChanged ();
		}

		private void SetupUserInterface ()
		{
			BackgroundColor = UIColor.Clear.FromHexString ("#4997D0", 1.0f);

			goUpButton = new UIButton {
				Font = UIFont.FromName ("SegoeUI-Light", 17f),
				Frame = new CGRect (0, 5, this.Bounds.Width, 15),
			};
			goUpButton.SetTitle ("Go back to \"Select a Year\".", UIControlState.Normal);
			goUpButton.SetTitleColor (UIColor.White, UIControlState.Normal);
			goUpButton.Center = new CGPoint (this.Bounds.Width / 2, 15);
			goUpButton.TouchUpInside += SetupGoUpTapped;

			stepThreeLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 42.5f),
				Frame = new CGRect (0, 70, this.Bounds.Width, 45),
				Text = "Step 3",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			partNameLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 32f),
				Frame = new CGRect (0, 120, this.Bounds.Width, 40),
				Text = "Choose a part.",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			partNameButton = new PickerButton {
				Frame = new CGRect (40, 175, this.Bounds.Width - 80, 30)
			};
			partNameButton.SetTitleColor (UIColor.Clear.FromHexString("#9B9B9B", 1.0f), UIControlState.Normal);

			searchButton = new SearchButton {
				Frame = new CGRect (40, 220, this.Bounds.Width - 80, 30)
			};
			searchButton.SetTitle ("Search", UIControlState.Normal);
			searchButton.SetTitleColor (UIColor.White, UIControlState.Normal);

			partNamePicker = new UIPickerView {
				Frame = new CGRect (0, 165, this.Bounds.Width, 40),
				Hidden = true,
			};

			buttonClickable = false;

			Add (goUpButton);
			Add (stepThreeLabel);
			Add (partNameLabel);
			Add (partNameButton);
			Add (partNamePicker);
			Add (searchButton);
		}

		private void SetupEventHandlers ()
		{
			partNameButton.TouchUpInside += (sender, e) => {
				if (buttonClickable) {
					partNamePicker.Hidden = false;
					partNameButton.Hidden = true;
					searchButton.Hidden = true;
				} else {
					var alert = new UIAlertView ("One Moment Please", "Interaction with the Willie's Cycles inventory is taking longer than expected.", null, "Okay", null);
					alert.Show ();
				}
			};

			searchButton.TouchUpInside += async (sender, e) => {
				await HandleSearchButtonTapped ();
			};
		}

		private void SetupGoUpTapped (object sender, EventArgs e)
		{
			searchViewController.StepThreeGoUp ();
		}

		private void SetupPropertyChanged ()
		{
			SearchParameters.PropertyChanged += async (sender, e) => {
				if (e.PropertyName == "Year") {
					buttonClickable = false;
					BTProgressHUD.Show ("Filtering Parts");
					var partNames = await API.GetPickerData (SearchParameters.Year, SearchParameters.Make);
					partNamePicker.Model = new PartNamePickerViewModel (partNames, partNameButton, searchButton);
					BTProgressHUD.Dismiss ();
					buttonClickable = true;
				}

				if (e.PropertyName == "PartName") {
					partNameButton.SetTitle (SearchParameters.PartName, UIControlState.Normal);
				}
			};
		}

		private async Task HandleSearchButtonTapped ()
		{
			var partName = SearchParameters.PartName;
			var make = SearchParameters.Make [0].ToString ();
			var year = SearchParameters.Year;

			if (string.IsNullOrEmpty (partName) || string.IsNullOrEmpty (make) || string.IsNullOrEmpty (year) || string.Equals (partName, "Loading") || string.Equals (year, "Loading")) {
				var alertView = new UIAlertView ("Error", "Select a valid make, year, and part name before searching.", null, "Okay", null);
				alertView.Show ();
			} else {
				var connected = CrossConnectivity.Current.IsConnected;
				if (connected) {
					BTProgressHUD.Show ();
					var parts = await API.GetParts (partName, make, year);
					BTProgressHUD.Dismiss ();

					searchViewController.NavigationController.PushViewController (new SearchResultsTableViewController (parts), true);
				} else {
					var alert = new UIAlertView ("No Internet Connection", "Please establish an internet connection before querying for parts.", null, "Okay", null);
					alert.Show ();
				}
			}
		}
	}
}