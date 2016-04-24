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
			BackgroundColor = UIColor.Clear.FromHexString ("#094074", 1.0f);

			partNameLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 32f),
				Frame = new CGRect (0, 0, this.Bounds.Width, 40),
				Text = "Choose a part.",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			partNameButton = new PickerButton {
				Frame = new CGRect (40, Frame.Height*1/8 + 10, this.Bounds.Width - 80, 30)
			};
			partNameButton.SetTitleColor (UIColor.Clear.FromHexString("#9B9B9B", 1.0f), UIControlState.Normal);

			searchButton = new SearchButton {
				Frame = new CGRect (40, Frame.Height*1/5 + 40, this.Bounds.Width - 80, 30)
			};
			searchButton.SetTitle ("Search", UIControlState.Normal);
			searchButton.SetTitleColor (UIColor.White, UIControlState.Normal);

			partNamePicker = new UIPickerView {
				Frame = new CGRect (0, Frame.Height*1/8, this.Bounds.Width, 40),
				Hidden = true,
			};

			buttonClickable = false;

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
					var alert = new UIAlertView ("Improper Order of Selection", "Choose a Range of Years before selecting the Make.", null, "Okay", null);
					alert.Show ();
				}
			};

			searchButton.TouchUpInside += async (sender, e) => {
				await HandleSearchButtonTapped ();
			};
		}

		private void SetupPropertyChanged ()
		{

			// The code below was changed so that the query to filter the part names to display in the 
			// partName picker only once the second year picker has made a selection
			SearchParameters.PropertyChanged += async (sender, e) => {
				if (e.PropertyName == "Year2") {
					buttonClickable = false;
					BTProgressHUD.Show ("Filtering Parts");
					var partNames = await API.GetPickerData (SearchParameters.Year, SearchParameters.Year2, SearchParameters.Make);
					partNamePicker.Model = new PartNamePickerViewModel (partNames, partNameButton, searchButton);
					BTProgressHUD.Dismiss ();
					buttonClickable = true;
				}
				if (e.PropertyName == "PartName") {
					string name;
					int hyphenIndex = SearchParameters.PartName.IndexOf("-");
					if (hyphenIndex > 0)
						name = SearchParameters.PartName.Substring(0, hyphenIndex);
					else
						name = SearchParameters.PartName;
					partNameButton.SetTitle (name, UIControlState.Normal);
				}
			};
		}

		private async Task HandleSearchButtonTapped ()
		{
			var partName = SearchParameters.PartName;
			var make = SearchParameters.Make [0].ToString ();
			var year = SearchParameters.Year;
			if (string.IsNullOrEmpty (partName) || string.IsNullOrEmpty (make) || string.IsNullOrEmpty (year) || string.Equals (partName, "Loading") || string.Equals (year, "Loading")) {
				var alertView = new UIAlertView ("Error", "Select a valid make, range of years, and part name before searching.", null, "Okay", null);
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