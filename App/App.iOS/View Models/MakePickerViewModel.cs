using System;
using System.Collections.ObjectModel;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class MakePickerViewModel : UIPickerViewModel
	{
		ObservableCollection<string> items;
		UIButton selectedButton;

		public MakePickerViewModel (UIButton pickerButton)
		{
			selectedButton = pickerButton;

			items = new ObservableCollection<string> ();
			items.Add ("Honda");
			items.Add ("Kawasaki");
			items.Add ("Suzuki");
			items.Add ("Yamaha");
		}

		public override nint GetComponentCount (UIPickerView pickerView)
		{
			return 1;
		}

		public override nint GetRowsInComponent (UIPickerView pickerView, System.nint component)
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
			SearchParameters.Make = items [(int) row];
			SearchParameters.Year = "";
			//SearchParameters.Year2 = "";
			SearchParameters.PartName = "";
			selectedButton.SetTitle (SearchParameters.Make, UIControlState.Normal);
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