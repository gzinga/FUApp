using System;
using System.Collections;

namespace FUCounter_App
{

	public class GroupData
	{
		public int group;
		public double txRate;
		public double dxRate;
		public bool active;
		public int numHair;
		public int numTxHair;
		public int NumDiscHair;
		private ArrayList _allRecords;
		public GroupData()
		{
			group = 0;
			txRate = 0.0;
			dxRate = 0.0;
			active = false;
			numHair = 0;
			numTxHair = 0;
			NumDiscHair = 0;
			_allRecords = new ArrayList ();
		}
		public void AddRecordTop(GraftRecord rec)
		{
			_allRecords.Add(rec);
			// calculates FUs
			FA [rec.HairCount-1]++;
			FT [rec.TerminalHairCount-1]++;
			// calculates total dx and tx
			totalHair = FT [0] + FT [1] + FT [2] + FT [3];
			totalTXHair = totalTXHair+rec.TxdTerminalHairCount;
			totalTX = (double)totalTXHair / (double)totalHair;
			totalTX *= 100;
			// calculates transactions
			//AllGroups[rec.GroupNumber-1]
		}

	}



	public class CaseCount
	{
		private ArrayList _allRecords;
		private string _notes;
		private DateTime _date;
		public int[] FA;
		public int[] FT;
		public ArrayList AllGroups;
		public double totalTX;
		public double totoalDX;
		public int totalHair;
		public int totalTXHair;

		public CaseCount (DateTime date)
		{
			FA = new int[4];
			FT = new int[4];
			_allRecords = new ArrayList ();
			AllGroups = new ArrayList ();
			_date = date;
			_notes = "";
			totalTX = 0;
			totoalDX = 0;
			totalHair = 0;
			totalTXHair = 0;
		}


		public CaseCount ()
		{
			FA = new int[4];
			FT = new int[4];
			_allRecords = new ArrayList ();
			AllGroups = new ArrayList ();
			_date = DateTime.Today;
			_notes = "";
		}


		public void AddRecordTop(GraftRecord rec)
		{
			_allRecords.Add(rec);
			// calculates FUs
			FA [rec.HairCount-1]++;
			FT [rec.TerminalHairCount-1]++;
			// calculates total dx and tx
			totalHair = FT [0] + FT [1] + FT [2] + FT [3];
			totalTXHair = totalTXHair+rec.TxdTerminalHairCount;
			totalTX = (double)totalTXHair / (double)totalHair;
			totalTX *= 100;
			// calculates transactions
			//AllGroups[rec.GroupNumber-1]
		}

		public int GetNumRecords()
		{
			return _allRecords.Count;
		}
	}
}

