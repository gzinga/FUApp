// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using System.Xml;

namespace FUCounter_App
{
	public class EULA
	{
		public EULA(){}
		public bool Agreed{ get; set;}
		public DateTime TimeStamp{ get; set;}
	}


	public partial class EULAViewController : UIViewController
	{
		public EULAViewController (IntPtr handle) : base (handle)
		{
		}

		public FUCounter_AppViewController mainView;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//EULA eula = new	EULA ();
			//var doc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			//string fileName = doc + "/RR_EULA.txt";

			//System.Xml.Serialization.XmlSerializer reader = 
			//	new System.Xml.Serialization.XmlSerializer(typeof(EULA));
			//System.IO.StreamReader file = null;

			//try{
			//	file = new System.IO.StreamReader(fileName);
			//}
			//catch(Exception e)
			//{
				//if (e.GetType() ==  System.IO.FileNotFoundException)
				return;
			//}
			//eula = (EULA)reader.Deserialize(file);
			//file.Close();
			//if (eula.Agreed == true) {
				// we can skip the EULA and go to the main view
				//mainView = this.Storyboard.InstantiateViewController (typeof(FUCounter_AppViewController).Name) as FUCounter_AppViewController;
				//PresentViewController(mainView, false, null); 
			//	mainView = new FUCounter_AppViewController ();
			//	PresentViewController(mainView, false, null); 
				//View = mainView.View;
				//UIView.Transition ((UIViewController)this, mainView, 1.0, UIViewAnimationCurve.EaseIn, null);
			//}
		}

		partial void NotAgreeAction (MonoTouch.Foundation.NSObject sender)
		{
			UIAlertView alert = new UIAlertView ("EULA", "SINCE YOU DID NOT AGREE TO RESTORATION ROBOTICS EULA, YOU WON'T BE ABLE TO USE THIS APP", null, "OK", null);
			alert.Show();
		}

		partial void AgreeAction (MonoTouch.Foundation.NSObject sender)
		{
			//EULA eula = new EULA();
			//eula.Agreed = true;
			//eula.TimeStamp = DateTime.Today;
			// saves a file with the info
			//var doc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			//string fileName = doc + "/RR_EULA.txt";

			//System.Xml.Serialization.XmlSerializer writer = 
			//	new System.Xml.Serialization.XmlSerializer(typeof(EULA));

			//System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
			//writer.Serialize(file, eula);
			//file.Close();
		}
	}
}
