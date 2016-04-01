using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class PartNamePickerViewModel : UIPickerViewModel
	{
		List<string> items;
		UIButton selectedButton;
		UIButton searchButton;

		public PartNamePickerViewModel (List<string> pickerData, UIButton pickerButton, UIButton searchForItemsButton)
		{
			items = pickerData;

			selectedButton = pickerButton;
			searchButton = searchForItemsButton;
		}

		public override nint GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override nint GetRowsInComponent (UIPickerView pickerView, nint component)
		{
			return items.Count;
		}

		public override string GetTitle (UIPickerView pickerView, nint row, nint component)
		{
			var item = items [(int) row];
			return item;
		}

		public override void Selected (UIPickerView pickerView, nint row, nint component)
		{
			SearchParameters.PartName = items [(int) row];

			selectedButton.Hidden = false;
			searchButton.Hidden = false;
			pickerView.Hidden = true;
		}

		public override NSAttributedString GetAttributedTitle (UIPickerView pickerView, nint row, nint component)
		{
			var title = items [(int) row];
			int hyphenIndex = title.IndexOf ("-"); // Added to attempt to solve issue with displaying -xx in picker
			if (hyphenIndex > 0)
				title = title.Substring (0, hyphenIndex);
			var font = UIFont.FromName ("SegoeUI-Light", 17f);
			var attributedTitle = new NSAttributedString (title, font, UIColor.White, null, null, null, (NSLigatureType) 1, 0, (NSUnderlineStyle) 0, null, 0, NSUnderlineStyle.None);

			return attributedTitle;
		}
	}
}
