using System;
using System.Globalization;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class PartCell : UITableViewCell
	{
		private UILabel TopLabel { get; set; }
		private UILabel BottomLabel { get; set; }
		private UILabel RightLabel { get; set; }

		public PartCell (UITableViewCellStyle style, string reuseIdentifier) : base (style, reuseIdentifier)
		{
			TopLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 16f),
				TextAlignment = UITextAlignment.Left
			};

			BottomLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 14f),
				TextAlignment = UITextAlignment.Left
			};

			RightLabel = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 14f),
				TextAlignment = UITextAlignment.Right
			};

			Add (TopLabel);
			Add (BottomLabel);
			Add (RightLabel);
		}

		public void UpdateCell (Part part)
		{
			var bottomText = string.Format ("{0} {1}", part.Model, part.Interchange);//{1} {2}", part.Year, part.Make, part.Model);

			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
			TopLabel.Text = textInfo.ToTitleCase (part.PartName.ToLower ());
			BottomLabel.Text = bottomText;
			RightLabel.Text = string.Format ("${0}", part.Price);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			TopLabel.Frame = new CGRect (15, 5, Bounds.Width, 25);
			BottomLabel.Frame = new CGRect (15, 30, Bounds.Width, 15);
			RightLabel.Frame = new CGRect (Bounds.Width - 65, 15, 40, 18);
		}
	}
}

