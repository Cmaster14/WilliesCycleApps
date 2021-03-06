﻿using System;
using System.Linq;
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
		UILabel partNameLabel;
		PickerButton partNameButton;
		UIPickerView partNamePicker;
		SearchButton searchButton;

		SearchViewController searchViewController;

		List<string> partNames = new List<string>();
		List<Part> parts = new List<Part>();

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
					var alert = new UIAlertView ("Improper Order of Selection", "Choose a Range of Years before selecting the Part Name.", null, "Okay", null);
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
					partNames.Clear();
					if(SearchParameters.Year == SearchParameters.Year2)
					{
						partNames = await API.GetPickerData (SearchParameters.Year, SearchParameters.Make);
						partNames = partNames.Distinct().ToList();
					}
					else
					{ // Similar to the android application, the way that we get the range of years to be implemented
						// by running a for loop for each possible year between the first and last year
						for (int i = int.Parse(SearchParameters.Year); i <= int.Parse(SearchParameters.Year2); i++)
						{
							partNames.AddRange(await API.GetPickerData (i.ToString(), SearchParameters.Make));
						}
						partNames.Distinct().ToList();
						partNames = partNames.Distinct().ToList();

					}
					partNamePicker.Model = new PartNamePickerViewModel (partNames, partNameButton, searchButton);

					BTProgressHUD.Dismiss ();
					if (!SearchParameters.Year2.Equals(""))
					{
						buttonClickable = true;
					}
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
			parts.Clear ();
			var partName = SearchParameters.PartName;
			var make = SearchParameters.Make [0].ToString ();
			var year = SearchParameters.Year;
			var year2 = SearchParameters.Year2;
			if (string.IsNullOrEmpty (partName) || string.IsNullOrEmpty (make) || string.IsNullOrEmpty (year) || string.Equals (partName, "Loading") || string.Equals (year, "Loading")) {
				var alertView = new UIAlertView ("Error", "Select a valid make, range of years, and part name before searching.", null, "Okay", null);
				alertView.Show ();
			} else {
				var connected = CrossConnectivity.Current.IsConnected;
				if (connected) {
					BTProgressHUD.Show ();
					// This is the statement that can be used if the API is updated with the overloads that
					// allow for the multiple year query
					//parts = await API.GetParts (partName, make, year, year2);
					if(SearchParameters.Year == SearchParameters.Year2)
					{
						parts = await API.GetParts (SearchParameters.PartName, SearchParameters.Make, SearchParameters.Year);
					}
					else
					{
						for (int i = int.Parse(SearchParameters.Year); i <= int.Parse(SearchParameters.Year2); i++)
						{
							parts.AddRange(await API.GetParts (SearchParameters.PartName, SearchParameters.Make, i.ToString()));
						}
					}
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