using System.Collections.Generic;
using Foundation;
using UIKit;

namespace App.iOS
{
	public class SearchResultsTableViewSource : UITableViewSource
	{
		List<Part> parts;
		UIViewController controller;
		DetailViewController detailViewController;

		public SearchResultsTableViewSource (UIViewController nav, List<Part> results)
		{
			controller = nav;
			parts = results;
		}

		public override System.nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override System.nint RowsInSection (UITableView tableview, System.nint section)
		{
			return parts.Count;
		}

		public override System.nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 50;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var part = parts[indexPath.Row];
			detailViewController = new DetailViewController (part);
			controller.NavigationController.PushViewController (detailViewController, true);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("RESULT_CELL") as PartCell;

			if (cell == null) {
				cell = new PartCell (UITableViewCellStyle.Default, "RESULT_CELL");
			}

			var part = parts [indexPath.Row];
			cell.UpdateCell (part);

			return cell;
		}
	}
}

