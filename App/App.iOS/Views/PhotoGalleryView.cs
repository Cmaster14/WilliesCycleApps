using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class PhotoGalleryView : UIView
	{
		UIScrollView scrollView;

		UILabel photoGallery;
		UIImageView imageViewOne;
		UIImageView imageViewTwo;
		UIImageView imageViewThree;

		public PhotoGalleryView (CGRect frame)
		{
			Frame = frame;

			SetupUserInterface ();
		}

		private void SetupUserInterface ()
		{
			scrollView = new UIScrollView {
				BackgroundColor = UIColor.Clear.FromHexString ("#FAC05E", 1.0f),
				Frame = new CGRect (0, 0, Frame.Width, Frame.Height * 2)
			};

			photoGallery = new UILabel {
				Font = UIFont.FromName ("SegoeUI-Light", 30f),
				Frame = new CGRect (0, 10, Frame.Width, 35),
				Text = "Photo Gallery",
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White
			};

			imageViewOne = new UIImageView {
				Frame = new CGRect ((Bounds.Width - 256) / 2, 55, 256, 192),
				Image = UIImage.FromFile ("ImageTwo.jpg")
			};

			imageViewTwo = new UIImageView {
				Frame = new CGRect ((Bounds.Width - 256) / 2, 255, 256, 192),
				Image = UIImage.FromFile ("ImageThree.jpg")
			};

			imageViewThree = new UIImageView {
				Frame = new CGRect ((Bounds.Width - 256) / 2, 455, 256, 192),
				Image = UIImage.FromFile ("ImageOne.jpg")
			};

			scrollView.Add (photoGallery);
			scrollView.Add (imageViewOne);
			scrollView.Add (imageViewTwo);
			scrollView.Add (imageViewThree);

			Add (scrollView);
		}
	}
}