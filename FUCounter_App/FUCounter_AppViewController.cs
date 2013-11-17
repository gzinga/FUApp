using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml;

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
			MasterRecord = new CaseCount (DateTime.Today, PatientID.Text);
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


		private void UpdateTableView(bool clear)
		{
			if (clear == true) {
				String[] tableItems = new string[1];
				tableItems[0] =string.Format("TOTAL # {0}, TX: {1:0.0} , DX: {2:0.0}", 0, 0, 0);
				ResultsView.Source = new TableSource (tableItems);
				ResultsView.ReloadData ();
				return;
			}
			ArrayList tableList = new ArrayList ();
			string a =string.Format("TOTAL # {0}, TX: {1:0.0} , DX: {2:0.0}", MasterRecord.totalHair, MasterRecord.totalTX, MasterRecord.totalDX);
			int count = 0;
			tableList.Add(a);
			foreach (object obj in MasterRecord.AllGroups) 
			{
				GroupData group = (GroupData)obj;
				if (group.Active == false)
					continue;
				a = string.Format ("Group {0}, total TX: {1:0.0} , DX: {2:0.0}", group.group + 1,
					(group).totalTX,
					(group).totalDX);
				count++;
				tableList.Add(a);
			}
			String[] tableItems1 = (String[]) tableList.ToArray( typeof( string ) );
			ResultsView.Source = new TableSource (tableItems1);
			ResultsView.ReloadData ();
		}


		private void UpdateFsInformation(GraftRecord rec)
		{

			switch (rec.TerminalHairCount) 
			{
			case 1:
				F1T.Text = Convert.ToString(MasterRecord.TFT[0]);
			break;
			case 2:
				F2T.Text = Convert.ToString(MasterRecord.TFT[1]);
				break;
			case 3:
				F3T.Text = Convert.ToString(MasterRecord.TFT[2]);
				break;
			case 4:
				F4T.Text = Convert.ToString(MasterRecord.TFT[3]);
				break;
			}

			switch (rec.HairCount) 
			{
				case 1:
				F1A.Text = Convert.ToString(MasterRecord.TFA[0]);
				break;
				case 2:
				F2A.Text = Convert.ToString(MasterRecord.TFA[1]);
				break;
				case 3:
				F3A.Text = Convert.ToString(MasterRecord.TFA[2]);
				break;
				case 4:
				F4A.Text = Convert.ToString(MasterRecord.TFA[3]);
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
			SaveRecords(null);
			UpdateTableView(false);
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


		partial void SaveRecords (MonoTouch.Foundation.NSObject sender)
		{
			Type[] extraTypes = {typeof(GroupData),typeof(GraftRecord)};
			var doc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			System.Xml.Serialization.XmlSerializer writer = 
				new System.Xml.Serialization.XmlSerializer(typeof(CaseCount),extraTypes);

			System.IO.StreamWriter file = new System.IO.StreamWriter(doc + "/" + MasterRecord.PatientID + ".xml");
			writer.Serialize(file, MasterRecord);
			file.Close();
		}


		partial void LoadRecords (MonoTouch.Foundation.NSObject sender)
		{
			UpdateTableView(true);
			Type[] extraTypes = {typeof(GroupData),typeof(GraftRecord)};
			var doc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			System.Xml.Serialization.XmlSerializer reader = 
				new System.Xml.Serialization.XmlSerializer(typeof(CaseCount),extraTypes);

			System.IO.StreamReader file = new System.IO.StreamReader(doc + "/" + MasterRecord.PatientID + ".xml");
			MasterRecord = (CaseCount)reader.Deserialize(file);
			file.Close();
			UpdateTableView(false);
			NewRecord();
		}

		partial void ClearAll (MonoTouch.Foundation.NSObject sender)
		{
			MasterRecord = new CaseCount();
			UpdateTableView(true);
			ViewDidLoad();
		}
		partial void PatientIDEditEnd (MonoTouch.Foundation.NSObject sender)
		{
			MasterRecord.PatientID = PatientID.Text;
		}


		partial void SaveFUFile (MonoTouch.Foundation.NSObject sender)
		{
			// converts master record into FU file
			FUCounterDataSet FU1 = new FUCounterDataSet(Convert.ToInt16(RecordBox.Text));
			FU1.Subject.MicroscopicNotes = MasterRecord.Notes;
			FU1.Subject.PatientID = MasterRecord.PatientID;
			FU1.Subject.ProcedureDate = MasterRecord.Date;
			FU1.Subject.Protocol = TextBoxProtocol.Text;
			int i = 0;
			foreach (object Group in MasterRecord.AllGroups)
			{
				foreach (object record in ((GroupData)Group)._allRecords)
				{
					GraftRecord rec = (GraftRecord)record;
					FUCounter FUrec = new FUCounter();
					FUrec.Discarded = rec.Discard;
					FUrec.GroupNumber = rec.GroupNumber;
					FUrec.HairCount = rec.HairCount;
					FUrec.TerminalHairCount = rec.TerminalHairCount;
					FUrec.TxdHairCount = rec.TxdHairCount;
					FUrec.TxdTerminalHairCount = rec.TxdTerminalHairCount;
					FU1.data[i] = FUrec;
					i++;
				}
			}
			SaveFUFileUsingXMLWriter(FU1);
			return;
			/*
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = false;
			settings.NewLineHandling = NewLineHandling.None;
			//settings.OtherProperties = values;

			using (XmlWriter writer = XmlWriter.Create(CreateStream(), settings))
			{
				_serializer.Serialize(o, writer);
			}
*/
			//now saves it
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = false;
			settings.NewLineHandling = NewLineHandling.None;

			Type[] extraTypes = {typeof(FUCounter),typeof(Subject)};
			var doc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			System.Xml.Serialization.XmlSerializer writer = 
				new System.Xml.Serialization.XmlSerializer(typeof(FUCounterDataSet),extraTypes);

			System.IO.StreamWriter file = new System.IO.StreamWriter(doc + "/" + MasterRecord.PatientID + ".FU1");
			writer.Serialize(file, FU1);
			file.Close();
		}



		private void SaveFUFileUsingXMLWriter(FUCounterDataSet FU1)
		{
			var doc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			string fileName = doc + "/" + MasterRecord.PatientID + ".FU1";

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			settings.NewLineChars = "\r\n";
			using (XmlWriter writer = XmlWriter.Create(fileName,settings))
			{
				writer.WriteStartDocument(true);
				writer.WriteStartElement("FUCounterDataSet","http://www.restorationrobotics.com/FUCounterDataSet.xsd");
				FUCounter[] allRec = FU1.data;
				foreach (FUCounter rec in allRec) 
				{
					if (rec == null)
						continue;
					writer.WriteStartElement("FUCounter");

					writer.WriteElementString("HairCount", rec.HairCount.ToString());
					writer.WriteElementString("TxdHairCount", rec.TxdHairCount.ToString());
					writer.WriteElementString("TerminalHairCount", rec.TerminalHairCount.ToString());
					writer.WriteElementString("TxdTerminalHairCount", rec.TxdTerminalHairCount.ToString());
					writer.WriteElementString("Discarded", rec.Discarded.ToString().ToLower());
					writer.WriteElementString("GroupNumber", rec.GroupNumber.ToString());

					writer.WriteEndElement();
				}

				string dateXSD = FU1.Subject.ProcedureDate.ToString ("yyyy-MM-ddTHH:mm:ss+HH:mm");
				writer.WriteStartElement("Subject");
				writer.WriteElementString("PatientId ", FU1.Subject.PatientID);
				writer.WriteElementString("ProcedureDate", dateXSD);
				writer.WriteElementString("Protocol", FU1.Subject.Protocol);
				writer.WriteElementString("MicroscopyNotes", FU1.Subject.MicroscopicNotes);
				writer.WriteEndElement();

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}

		}
	}
}

