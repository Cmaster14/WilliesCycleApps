using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class PickerButton : UIButton
	{
		public PickerButton ()
		{
			var backgroundLayer = this.Layer;
			backgroundLayer.MasksToBounds = true;
			backgroundLayer.CornerRadius = 5;
			backgroundLayer.BackgroundColor = UIColor.White.CGColor;

			Font = UIFont.FromName ("SegoeUI-Light", 21f);
		}
	}
}

