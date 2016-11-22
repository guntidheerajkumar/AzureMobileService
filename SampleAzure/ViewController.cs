using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using Foundation;
using CoreGraphics;
using BigTed;

namespace SampleAzure
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var btnAdd = new UIButton(UIButtonType.Custom);
			btnAdd.SetBackgroundImage(UIImage.FromBundle("Add"), UIControlState.Normal);
			btnAdd.Frame = new CGRect(0, 0, 19, 19);
			btnAdd.TouchUpInside += (sender, e) =>
			{
				var employeeViewController = UIStoryboard.FromName("Main", null).InstantiateViewController("EmployeeViewController");
				this.NavigationController.PushViewController(employeeViewController, true);
			};
			this.NavigationItem.SetRightBarButtonItem(null, true);
			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(btnAdd), true);
		}

		public async override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			BTProgressHUD.Show("Loading",-1,ProgressHUD.MaskType.Black);
			var azureService = new AzureService();
			var employees = await azureService.GetEmployees();
			var source = new EmployeeTableSource(employees);
			source.navigationController = this.NavigationController;
			EmployeeTableView.Source = source;
			EmployeeTableView.ReloadData();
			BTProgressHUD.Dismiss();

		}
	}

	public class EmployeeTableSource : UITableViewSource
	{
		public UINavigationController navigationController;
		List<EmployeeInfo> TableItems;
		string CellIdentifier = "TableCell";

		public EmployeeTableSource(List<EmployeeInfo> items)
		{
			TableItems = items;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return TableItems.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
			EmployeeInfo item = TableItems[indexPath.Row];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{ cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier); }

			cell.TextLabel.Text = item.Name;
			cell.DetailTextLabel.Text = item.DOB.ToShortDateString();

			return cell;
		}

		public async override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, Foundation.NSIndexPath indexPath)
		{
			switch (editingStyle)
			{
				case UITableViewCellEditingStyle.Delete:
					var azureService = new AzureService();
					await azureService.DeleteEmployee(TableItems[indexPath.Row]);
					TableItems.RemoveAt(indexPath.Row);
					tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					tableView.ReloadData();
					break;
			}
		}
		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true; 
		} 

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			Constants.Id = TableItems[indexPath.Row].Id;
			var employeeDetailsViewController = UIStoryboard.FromName("Main", null).InstantiateViewController("EditEmployeeController");
			navigationController.PushViewController(employeeDetailsViewController, true);
		}
	}
}
