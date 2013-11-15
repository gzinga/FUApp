using System;
using System.Collections;
using System.Xml;

namespace FUCounter_App
{

	public class GroupData
	{
		public int group;
		public ArrayList _allRecords;
		public int[] FA;
		public int[] FT;
		public double totalTX;
		public double totalDX;
		public int totalHair;
		public int totalTXHair;
		public bool Active;

		public GroupData(int _group)
		{
			group = _group;
			_allRecords = new ArrayList ();
			FA = new int[4];
			FT = new int[4];
			totalTX = 0;
			totalDX = 0;
			totalHair = 0;
			totalTXHair = 0;
			Active = false;
		}

		public void AddRecordTop(GraftRecord rec)
		{
			_allRecords.Add(rec);
			// calculates FUs
			FA [rec.HairCount-1]++;
			FT [rec.TerminalHairCount-1]++;
			// calculates total dx and tx
			totalHair = FT [0] + 2*FT [1] + 3*FT [2] + 4*FT [3];
			totalTXHair += rec.TxdTerminalHairCount;
			totalTX = (double)totalTXHair / (double)totalHair;
			totalTX *= 100;
			if (rec.Discard == true)
				totalDX++;
		}

	}



	public class CaseCount
	{
		private string _notes;
		private DateTime _date;
		public int[] TFA;
		public int[] TFT;
		public ArrayList AllGroups;
		public double totalTX;
		public double totalDX;
		public int totalHair;
		public int totalTXHair;

		public CaseCount (DateTime date)
		{
			TFA = new int[4];
			TFT = new int[4];
			AllGroups = new ArrayList ();
			_date = date;
			_notes = "";
			totalTX = 0;
			totalDX = 0;
			totalHair = 0;
			totalTXHair = 0;
			for (int i=0;i<12;i++)
				AllGroups.Add (new GroupData (i));
		}

		public void AddRecordTop(GraftRecord rec)
		{
			((GroupData)AllGroups[rec.GroupNumber-1]).Active = true;
			//insert the record
			((GroupData)AllGroups[rec.GroupNumber-1]).AddRecordTop (rec);
		
			totalDX = 0;
			totalHair = 0;
			totalTXHair = 0;
			// calculates FUs
			TFA[rec.HairCount-1]++;
			TFT[rec.TerminalHairCount-1]++;
			foreach (object obj in AllGroups) 
			{
				GroupData group = (GroupData)obj;
				totalHair += group.totalHair;
				totalTXHair += group.totalTXHair;
				totalDX += group.totalDX;
			}
			totalTX = (double)totalTXHair / (double)totalHair;
			totalTX *= 100;
		}

		public int GetNumRecords()
		{
			int sum = 0;
			foreach (object obj in AllGroups)
			{
				sum += ((GroupData)obj)._allRecords.Count;
			}
			return sum;
		}
	}
}

