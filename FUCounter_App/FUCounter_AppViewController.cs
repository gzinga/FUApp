using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections;

namespace FUCounter_App
{
	public class TableSource : UITableViewSource {
		protected string[] tableItems;
		protected string cellIdentifier = "TableCell";
		public TableSource (string[] items)
		{
			tableItems = items;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems.Length;
		}
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			// request a recycled cell to save memory
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
			cell.TextLabel.Text = tableItems[indexPath.Row];
			return cell;
		}
	}



	public partial class FUCounter_AppViewController : UIViewController
	{
		private int _workflowCounter = 0;
		public CaseCount MasterRecord;
		public bool redFlegEntry;
		public FUCounter_AppViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			ProcedureDate.Text = DateTime.Today.ToString();
			MasterRecord = new CaseCount (DateTime.Today);
			NewRecord ();
			F1A.Text = F1T.Text = F2A.Text = F2T.Text = F3A.Text = F3T.Text = F3T.Text = F4T.Text = F4A.Text = "0";
			redFlegEntry = false;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion

		private void NewRecord()
		{
			RecordBox.Text = Convert.ToString(MasterRecord.GetNumRecords () + 1);
			HairCountBox.Text = "1";
			TxdHairCountBox.Text = "0";
			TerminalHairCountBox.Text = "1";
			TxdTerminalHairCount.Text = "0";
			DiscardedSwitch.On = false;
			_workflowCounter = 1;
			redFlegEntry = false;
			LabelHairCount.BackgroundColor = UIColor.Orange;
		}



		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return true;
		}


		private void UpdateTableView()
		{
			string[] tableItems = new string[10];
			//tableItems[0] = "total TX: " + MasterRecord.totalTX + ", DX: " + MasterRecord.totoalDX;
			string a =string.Format("total TX: {0:0.0} , DX: {1:0.0}", MasterRecord.totalTX, MasterRecord.totoalDX);
			tableItems [0] = a;
			ResultsView.Source = new TableSource(tableItems);
			ResultsView.ReloadData ();
		}


		private void UpdateFsInformation(GraftRecord rec)
		{
			switch (rec.TerminalHairCount) 
			{
			case 1:
				F1T.Text = Convert.ToString(MasterRecord.FT[0]);
			break;
			case 2:
				F2T.Text = Convert.ToString(MasterRecord.FT[1]);
				break;
			case 3:
				F3T.Text = Convert.ToString(MasterRecord.FT[2]);
				break;
			case 4:
				F4T.Text = Convert.ToString(MasterRecord.FT[3]);
				break;
			}

			switch (rec.HairCount) 
			{
				case 1:
				F1A.Text = Convert.ToString(MasterRecord.FA[0]);
				break;
				case 2:
				F2A.Text = Convert.ToString(MasterRecord.FA[1]);
				break;
				case 3:
				F3A.Text = Convert.ToString(MasterRecord.FA[2]);
				break;
				case 4:
				F4A.Text = Convert.ToString(MasterRecord.FA[3]);
				break;
			}


		}


		partial void KeyEnterTouch (MonoTouch.Foundation.NSObject sender)
		{
			if (redFlegEntry) 
			{
				UIAlertView alert = new UIAlertView ("Entry", "Entry is invalid, please correct it", null, "OK", null);
				alert.Show();
				return;
			}
			if (_workflowCounter == 1)
			{
				// this means we have n terminal grafts and they are all not transected
				TxdHairCountBox.Text = "0";
				TxdTerminalHairCount.Text = "0";
				TerminalHairCountBox.Text = HairCountBox.Text;
				DiscardedSwitch.On = false;
			}
			else if (_workflowCounter == 2)
			{
				// this means we have n terminal grafts and some are transected
				TxdTerminalHairCount.Text = TxdHairCountBox.Text;
				TerminalHairCountBox.Text = HairCountBox.Text;
				DiscardedSwitch.On = false;
			}
				GraftRecord rec = new GraftRecord(Convert.ToInt16(HairCountBox.Text),Convert.ToInt16(TxdHairCountBox.Text),
			                                   Convert.ToInt16(TerminalHairCountBox.Text),
			                                   Convert.ToInt16(TxdHairCountBox.Text),DiscardedSwitch.On,
			                                   GroupNumber.SelectedSegment+1);
			MasterRecord.AddRecordTop(rec);
			UpdateFsInformation(rec);
			UpdateTableView();
			NewRecord();
		}


		private void RunWorkflow(string txt)
		{
			TxdHairCountBox.BackgroundColor = UIColor.White; 
			TerminalHairCountBox.BackgroundColor = UIColor.White;
			TxdTerminalHairCount.BackgroundColor = UIColor.White; 
			redFlegEntry = false;

			//resets the label color
			LabelHairCount.BackgroundColor = UIColor.White;
			LabelTerminalHairCount.BackgroundColor = UIColor.White;
			LabelTxdHairCount.BackgroundColor = UIColor.White;
			LabelTxdTerminalHairCount.BackgroundColor = UIColor.White;

			switch (_workflowCounter) 
			{
			case 1:
				HairCountBox.Text = txt;
				TerminalHairCountBox.Text = txt;
				LabelTxdHairCount.BackgroundColor = UIColor.Orange;
				_workflowCounter++;
				break;
			case 2:
				TxdHairCountBox.Text = txt;
				TxdTerminalHairCount.Text = txt;
				if (Convert.ToInt16 (TxdHairCountBox.Text) > Convert.ToInt16 (HairCountBox.Text)) {
					TxdHairCountBox.BackgroundColor = UIColor.Red; 
					redFlegEntry = true;
				} else {
					LabelTerminalHairCount.BackgroundColor = UIColor.Orange;
					_workflowCounter++;
				}
				break;
			case 3:
				TerminalHairCountBox.Text = txt;

				if (Convert.ToInt16 (TerminalHairCountBox.Text) > Convert.ToInt16 (HairCountBox.Text)) 
				{
					TerminalHairCountBox.BackgroundColor = UIColor.Red; 
					redFlegEntry = true;
				}
				else if (Convert.ToInt16 (TerminalHairCountBox.Text) < Convert.ToInt16 (TxdTerminalHairCount.Text)) 
				{
					TxdTerminalHairCount.BackgroundColor = UIColor.Red; 
					redFlegEntry = true;
				}
				else{
					LabelTxdTerminalHairCount.BackgroundColor = UIColor.Orange;
					_workflowCounter++;
				}
				break;
			case 4:
				TxdTerminalHairCount.Text = txt;
				if (Convert.ToInt16 (TerminalHairCountBox.Text) < Convert.ToInt16 (TxdTerminalHairCount.Text)) {
					TxdTerminalHairCount.BackgroundColor = UIColor.Red; 
					redFlegEntry = true;
				} else {
					_workflowCounter++;
				}
				break;
			case 5:
				DiscardedSwitch.On = txt == "0" ? false : true;
				_workflowCounter = 0;
				break;
			}

		}

		partial void Button0Click (MonoTouch.Foundation.NSObject sender)
		{
			RunWorkflow(Key0.TitleLabel.Text);

		}


		partial void Button1Click (MonoTouch.Foundation.NSObject sender)
		{

			RunWorkflow(Key1.TitleLabel.Text);
		}


		partial void Button2Click (MonoTouch.Foundation.NSObject sender)
		{

			RunWorkflow(Key2.TitleLabel.Text);
		}

		partial void Button3Click (MonoTouch.Foundation.NSObject sender)
		{
			RunWorkflow(Key3.TitleLabel.Text);

		}


		partial void Button4Click (MonoTouch.Foundation.NSObject sender)
		{

			RunWorkflow(Key4.TitleLabel.Text);

		}


		partial void Button5Click (MonoTouch.Foundation.NSObject sender)
		{

			RunWorkflow(Key5.TitleLabel.Text);
		}


		partial void Button6Click (MonoTouch.Foundation.NSObject sender)
		{

			RunWorkflow(Key6.TitleLabel.Text);
		}



	}
}

