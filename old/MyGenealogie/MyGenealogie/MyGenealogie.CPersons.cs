using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;




namespace MyGenealogie{

	public class CPersons : CSLib.CObjectCollection {

		public CPerson Add(CPerson p){

			return (CPerson)this.Add(p.Guid.ToUpper(),p);
		}
		public CPerson Add(string strFolderName){
			try{
				CPerson p = new CPerson(strFolderName);
				return this.Add(p);
			}
			catch(System.Exception e){

				GeneaTools.ShowError(e.Message);
				return null;
			}
		}
		public CPerson this[string strGuid]{
			get {
				return (CPerson)base[strGuid.ToUpper()];
			}
		}
		public void LoadPersons(){

			CSLib.File.CTextFile f		= new CSLib.File.CTextFile();
			CSLib.CVariables Persones	= f.GetDirList(MyGenealogie.GeneaTools.PersonFolder,"*.*");

			foreach(CSLib.CVariable Persone in Persones){

				this.Add(Persone.Name);
			}

			//CPerson p = this["b07bf2bb-7209-4a62-bdf2-a9e2e668803f"];
			//p.Comment = p.LastName;
		}
		public void FillComboBox(System.Windows.Forms.ComboBox cbo, string strGuid){

			FillComboBox(cbo, strGuid, "");
		}
		public void FillComboBox(System.Windows.Forms.ComboBox cbo, string strGuid, string strSexe){

			foreach(CPerson p in this){

				bool booAdd=false;

				switch(strSexe){
					case "":
							booAdd = true; 
					break;
					case "M":
							booAdd = p.Sexe == "M"; 
					break;
					case "F":
							booAdd = p.Sexe == "F"; 
					break;
				}
				if(booAdd){

					CSLib.CObjectBoxItem i = new CSLib.CObjectBoxItem(p,p.Text);
					int index = cbo.Items.Add(i);
				}
			}
			for(int i =0 ; i<cbo.Items.Count; i++){

				CSLib.CObjectBoxItem	it = (CSLib.CObjectBoxItem)cbo.Items[i];
				CPerson					p  = (CPerson)it.Value;

				if(p.Guid==strGuid){

					cbo.SelectedIndex=i;
					return;
				}
			}			
		}
		
//		public CPerson IsInArea(int x, int y){
//
//			foreach(CPerson p in this){
//
//				if(p.IsInArea(x,y)){
//
//					return p;
//				}
//			}
//			return null;
//		}
	}
}
