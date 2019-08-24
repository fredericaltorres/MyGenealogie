using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace MyGenealogie{

	public class CPerson : CGraphicObject {

		public string FolderName;
		public string LastName;
		public string FirstName;
		public string MiddleNames;

		// Read from XML
		//public string Guid; Inherited
		public string Sexe;
		public string BirthDate;
		public string BirthCity;
		public string BirthCountry;
		public string DeathDate;
		public string DeathCity;
		public string DeathCountry;
		public string Comment;
		public string FatherGuid;
		public string MotherGuid;
		public string CreationDate;

		public bool IsMan(){

			return this.Sexe.ToUpper() == "M";
		}
		public CUnions GetUnions(){

			CUnions us = new CUnions();

			foreach(CUnion u in Unions){

				if(IsMan()){

					if(this.Guid == u.ManGuid){

						us.Add(u);
					}
				}
				else{
					if(this.Guid == u.WomanGuid){

						us.Add(u);
					}
				}
			}
			return us;
		}
		public CPersons GetChildren(){

			CPersons Chilren = new CPersons();

			foreach(CPerson c in Persons){

				if(IsMan()){

					if(c.FatherGuid==this.Guid){

						Chilren.Add(c);
					}
				}
				else{
					if(c.MotherGuid==this.Guid){

						Chilren.Add(c);
					}
				}
			}
			return Chilren;
		}
		public CPerson Father{

			get{
				foreach(CPerson c in Persons){

					if(this.FatherGuid == c.Guid) {

						return c;
					}
				}
				return null;
			}
		}
		public CPerson Mother{

			get{
				foreach(CPerson c in Persons){

					if(this.MotherGuid == c.Guid) {

						return c;
					}
				}
				return null;
			}
		}
		private void LoadXML(){

			try{
				XMLDoc = new System.Xml.XmlDocument();
				XMLDoc.Load(XMLFileName);

				ReadXMLProperty("Guid");
				ReadXMLProperty("Sexe");
				ReadXMLProperty("BirthDate");
				ReadXMLProperty("BirthCity");
				ReadXMLProperty("BirthCountry");
				ReadXMLProperty("DeathDate");
				ReadXMLProperty("DeathCity");
				ReadXMLProperty("DeathCountry");
				ReadXMLProperty("Comment");
				ReadXMLProperty("FatherGuid");
				ReadXMLProperty("MotherGuid");
				ReadXMLProperty("CreationDate");
			}
			catch{
				
				throw new System.Exception(string.Format(GeneaTools.ERR_1001,this.FolderName));				
			}
		}
		public void Save(){

			SaveXML();
			ResetInternalData();
		}
		protected override void SaveXML(){
			try{

				WriteXMLProperty("Guid");
				WriteXMLProperty("Sexe");
				WriteXMLProperty("BirthDate");
				WriteXMLProperty("BirthCity");
				WriteXMLProperty("BirthCountry");
				WriteXMLProperty("DeathDate");
				WriteXMLProperty("DeathCity");
				WriteXMLProperty("DeathCountry");
				WriteXMLProperty("Comment");
				WriteXMLProperty("FatherGuid");
				WriteXMLProperty("MotherGuid");
				WriteXMLProperty("CreationDate");
				base.SaveXML();
			}
			catch{
				
				throw new System.Exception(string.Format(GeneaTools.ERR_1004,this.FolderName));				
			}
		}
		public override string Text {
			get{
				return MyGenealogie.GeneaTools.CapitalizeString(this.LastName) + " " + MyGenealogie.GeneaTools.CapitalizeString(this.FirstName);				
			}
		}
		public override string ToolTipText {
			get{
				string s=null;
				s += MyGenealogie.GeneaTools.CapitalizeString(this.LastName) + " ";
				s += MyGenealogie.GeneaTools.CapitalizeString(this.FirstName) + CSLib.CSLibGlobal.NewLine; 
				s += this.BirthDate + " " + this.BirthCity + ", " + this.BirthCountry + CSLib.CSLibGlobal.NewLine; 
				s += this.DeathDate + " " + this.DeathCity + ", " + this.DeathCountry + CSLib.CSLibGlobal.NewLine; 
				s += this.Sexe.ToUpper()=="M" ? "Man" : "Woman" + CSLib.CSLibGlobal.NewLine;
				return s;
			}
		}
		public CPerson(string strFolderName){

			FolderName = strFolderName;
			ObjectType = EObjectType.PERSON;

			string [] NameParts = strFolderName.Split(',');

			if(NameParts.Length==1){

				throw new System.Exception(string.Format(GeneaTools.ERR_1000,strFolderName));				
			}
			else{
				this.LastName    = NameParts[0].Trim();
				this.FirstName   = NameParts[1].Trim();
				if(NameParts.Length>=3)this.MiddleNames = NameParts[2].Trim();
				LoadXML();
			}
		}
		public bool ImageExist{
			get {
				return CSLib.File.CTextFile.Exist(ImageFullPath);
			}
		}
		public string ImageFullPath{
			get {
				return MyGenealogie.GeneaTools.PersonFolder + @"\" + FolderName + @"\"+FolderName +".jpg";
			}
		}
		public override string XMLFileName {
			get {
					return MyGenealogie.GeneaTools.PersonFolder + @"\" + FolderName + @"\p.xml";
				}
		}
		public string PersonsFolder {
			get {
				return MyGenealogie.GeneaTools.PersonFolder;
			}
		}
		public override void CreateControl(
					System.Windows.Forms.Form			ParentForm, 
					System.Windows.Forms.ToolTip		MainToolTip,					
					System.EventHandler					evClick,
					System.EventHandler					evDoubleClick,
					System.Windows.Forms.ContextMenu	PersonContextMenu,
					int									CreateControlOptions,
					int									x,
					int									y
			){
			base.CreateControl(ParentForm,MainToolTip,evClick,evDoubleClick,PersonContextMenu,CreateControlOptions,x,y);
		}
		public void SetRelation(CPerson P, ERelation Relation){

			switch(Relation){

				case ERelation.ChildOfMother	: 
					this.MotherGuid = P.Guid;
					this.SaveXML();
					break;

				case ERelation.ChildOfFather		: 
					this.FatherGuid = P.Guid;
					this.SaveXML();
				break;

				case ERelation.FatherOf : 					
					P.FatherGuid =  this.Guid;
					P.SaveXML();
					break;

				case ERelation.MotherOf	: 
					P.MotherGuid =  this.Guid;
					P.SaveXML();
					break;



			}
		}
//		public override void Draw(System.Drawing.Graphics g, System.Drawing.Font font, bool booIncY){
//
//			if(booIncY){
//				CellNextY+=2; 
//			}
//			else{
//			//	System.Diagnostics.Debugger.Break();
//			}
//
//			base.Draw(g,font,booIncY);
//			//CGraphicObjectCommon.PushCellNextX();
//			CPersons Children = this.GetChildren();
//			CGraphicObjectCommon.PushCellNextY();
//
//			int i=0;
//			foreach(CPerson c in Children){
//				
//				c.Draw(g,font,i==0);				
//				i++;
//				if(i<Children.Count){
//					CellNextX+=1;
//				}
//				
//			}
//			CGraphicObjectCommon.PopCellNextY();
//			//CGraphicObjectCommon.PopCellNextX();
//		}

	}
}
