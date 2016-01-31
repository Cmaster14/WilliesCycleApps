using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class YearPickerViewModel : UIPickerViewModel
	{
		List<string> items;
		UIButton selectedButton;

		public YearPickerViewModel (List<string> pickerData, UIButton pickerButton)
		{
			items = pickerData;
			selectedButton = pickerButton;
		}

		public override System.nint GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override System.nint GetRowsInComponent (UIPickerView pickerView, System.nint component)
		{
			return items.Count;
		}

		public override string GetTitle (UIPickerView pickerView, System.nint row, System.nint component)
		{
			var item = items [(int) row];
			return item;
		}

		public override void Selected (UIPickerView pickerView, System.nint row, System.nint component)
		{
			SearchParameters.PartName = "";
			SearchParameters.Year = items [(int) row];
			selectedButton.Hidden = false;
			pickerView.Hidden = true;
		}

		public override NSAttributedString GetAttributedTitle (UIPickerView pickerView, System.nint row, System.nint component)
		{
			var title = items [(int) row];
			var font = UIFont.FromName ("SegoeUI-Light", 17f);
			var attributedTitle = new NSAttributedString (title, font, UIColor.White, null, null, null, (NSLigatureType) 1, 0, (NSUnderlineStyle) 0, null, 0, NSUnderlineStyle.None);

			return attributedTitle;
		}
	}
}
