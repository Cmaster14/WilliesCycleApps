using System.Collections.Generic;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class SearchResultsTableViewController : UIViewController
	{
		List<Part> parts;

		public SearchResultsTableViewController (List<Part> parts)
		{
			this.parts = parts;
			for (int i = 0; i < this.parts.Count; i++){
				int hyphenIndex = this.parts[i].PartName.IndexOf("-"); // Added to fix -xx issue in search results display
				this.parts[i].PartName = this.parts[i].PartName.Substring(0,hyphenIndex);
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationController.NavigationBarHidden = false;

			Title = "Results";

			var table = new UITableView {
				Frame = new CoreGraphics.CGRect (0, 0, View.Bounds.Width, View.Bounds.Height),
				Source = new SearchResultsTableViewSource (this, parts)
			};

			View.Add (table);
		}
	}
}

