using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class SearchButton : UIButton
	{
		public SearchButton ()
		{
			var backgroundLayer = this.Layer;
			backgroundLayer.MasksToBounds = true;
			backgroundLayer.CornerRadius = 5;
			backgroundLayer.BackgroundColor = UIColor.Clear.FromHexString ("#094074", 1.0f).CGColor;

			Font = UIFont.FromName ("SegoeUI-Light", 21f);
		}
	}
}
