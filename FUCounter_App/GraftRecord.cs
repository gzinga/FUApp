using System;

namespace FUCounter_App
{
	public class GraftRecord
	{

	private int _hairCount; 
	private	int _txdHairCount;
	private	int _terminalHairCount;
	private	int _txdTerminalHairCount;
	private	bool _discard;
	private	int _groupNumber;


		public int HairCount
		{
			get
			{
				return _hairCount;
			}
			set
			{
				_hairCount = value;
			}

		}

		public int TxdHairCount
		{
			get
			{
				return _txdHairCount;
			}
			set
			{
				_txdHairCount = value;
			}

		}

		public int TerminalHairCount
		{
			get
			{
				return _terminalHairCount;
			}
			set
			{
				_terminalHairCount = value;
			}

		}


		public int TxdTerminalHairCount
		{
			get
			{
				return _txdTerminalHairCount;
			}
			set
			{
				_txdTerminalHairCount = value;
			}

		}

		public bool Discard
		{
			get
			{
				return _discard;
			}
			set
			{
				_discard = value;
			}

		}

		public int GroupNumber
		{
			get
			{
				return _groupNumber;
			}
			set
			{
				_groupNumber = value;
			}

		}



		public GraftRecord (int hairCount, int txdHairCount, int terminalHairCount, int txdTerminalHairCount, bool discard, int groupNumber)
		{
			_hairCount = hairCount;
			_txdHairCount = txdHairCount;
			_terminalHairCount = terminalHairCount;
			_txdTerminalHairCount = txdTerminalHairCount;
			_discard = discard;
			_groupNumber = groupNumber;
		}
	}
}

