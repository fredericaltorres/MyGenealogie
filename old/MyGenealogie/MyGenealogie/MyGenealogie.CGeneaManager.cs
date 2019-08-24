using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace MyGenealogie{

	public class CGeneaManager{

		public CDisplayOptions DisplayOptions;
		public string RootDir;
		public CPerson RootPerson ;
		public CPersons Persons;
		public CUnions Unions;

		public CGeneaManager(string RootDir){

			DisplayOptions	= new CDisplayOptions();
			this.RootDir	= RootDir;


			LoadPersons();
			LoadUnions();

			
			
			
			RootPerson		= new CPerson(this.DisplayOptions.Person1);
		}		
		public void LoadPersons(){

			Persons			= new CPersons();
			Persons.LoadPersons();
			CPerson.Persons = Persons; // Pass the collection of person to the CPerson class
		}
		public void LoadUnions(){

			Unions			= new CUnions();
			Unions.LoadUnions();
			CPerson.Unions  = Unions;  // Pass the collection of union  to the CPerson class
		}
		public void FillComboBoxSexe(System.Windows.Forms.ComboBox cbo, string strSelected){
			
			cbo.Items.Add(new CSLib.CObjectBoxItem("M","Male"));
			cbo.Items.Add(new CSLib.CObjectBoxItem("F","Female"));

			if(strSelected=="M"){
				cbo.SelectedIndex = 1;
			}
			else{
				cbo.SelectedIndex = 0;
			}
			
		}
		public void FillComboBoxWithRelation(System.Windows.Forms.ComboBox cbo){
			
			cbo.Items.Add(new CSLib.CObjectBoxItem(ERelation.FatherOf,"Father Of"));
			cbo.Items.Add(new CSLib.CObjectBoxItem(ERelation.MotherOf,"Mother Of"));

			cbo.Items.Add(new CSLib.CObjectBoxItem(ERelation.ChildOfFather,"Child Of Father"));
			cbo.Items.Add(new CSLib.CObjectBoxItem(ERelation.ChildOfMother,"Child of Mother"));

//			cbo.Items.Add(new CSLib.CObjectBoxItem(ERelation.DaugtherOfFather,"Daugther Of Father"));
//			cbo.Items.Add(new CSLib.CObjectBoxItem(ERelation.DaugtherOfMother,"Daugther Of Mother"));
		}

			  
//		public  void Draw(System.Drawing.Graphics g, System.Drawing.Font font){
//
//			foreach(CUnion u in Unions){
//				u.Displayed = false;
//			}
//			foreach(CPerson p in Persons){
//				p.Displayed = false;
//			}
//
//			CGraphicObjectCommon.PrepareDraw();
//			RootPerson.Draw(g,font,true);
//		
//		}
//		public void CreateControl(System.Windows.Forms.Form ParentForm, System.Windows.Forms.ToolTip MainToolTip, System.EventHandler ev,System.Windows.Forms.ContextMenu PersonContextMenu){
//
//			CGraphicObjectCommon.PrepareDraw();
//
//			CPerson StartPerson = this.Persons[RootPerson.Guid]; // Do not use the RootPerson instance which is duplicated from the one in the collection Persons
//
//			StartPerson.CreateControl(ParentForm, MainToolTip, CGraphicObjectCommon.ECCO_SHOW_UNION+CGraphicObjectCommon.ECCO_INC_Y+CGraphicObjectCommon.ECCO_SHOW_CHILDREN,ev,PersonContextMenu);
//		}
	}
}
