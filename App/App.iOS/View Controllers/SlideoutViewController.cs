using System;
using CoreGraphics;
using Foundation;
using UIKit;
using FlyoutNavigation;
using MonoTouch.Dialog;

namespace App.iOS
{
	[Register ("SlideoutViewController")]
	public class SlideoutViewController : UIViewController
	{
		FlyoutNavigationController flyout;

		public SlideoutViewController ()
		{
		}

		public override void ViewDidLoad ()
		{
			flyout = new FlyoutNavigationController {
				NavigationRoot = new RootElement ("Willie's Cycles") {
					new Section () {
						new StyledStringElement ("Search for Parts") { Font = UIFont.FromName ("SegoeUI-Light", 20f) }
					},
					new Section () {
						new StyledStringElement ("About") { Font = UIFont.FromName ("SegoeUI-Light", 20f) },
						// new StyledStringElement ("Photo Gallery") { Font = UIFont.FromName ("SegoeUI-Light", 20f) },
						new StyledStringElement ("Terms of Service") { Font = UIFont.FromName ("SegoeUI-Light", 20f) },
						new StyledStringElement ("Licensure") { Font = UIFont.FromName ("SegoeUI-Light", 20f) }
					}
				}
			};

			var searchViewController = new UINavigationController (new SearchViewController (flyout));
			searchViewController.NavigationBar.BarTintColor = UIColor.Clear.FromHexString ("#094074", 1.0f);
			searchViewController.NavigationBar.TintColor = UIColor.White;
			searchViewController.NavigationBar.Translucent = false;
			searchViewController.NavigationBar.TitleTextAttributes = new UIStringAttributes () { Font = UIFont.FromName ("SegoeUI-Light", 20f), ForegroundColor = UIColor.White };

			var aboutViewController = new AboutViewController (flyout);
			// var photoGalleryViewController = new PhotoGalleryViewController (flyout);
			var disclaimerViewController = new DisclaimerViewController (flyout);
			var licensureViewController = new LicensureViewController (flyout);

			flyout.ViewControllers = new UIViewController[] {
				searchViewController,
				aboutViewController,
				// photoGalleryViewController,
				disclaimerViewController,
				licensureViewController
			};

			View.AddSubview (flyout.View);
		}

		public override bool PrefersStatusBarHidden ()
		{
			return true;
		}
	}
}

