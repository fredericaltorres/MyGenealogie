using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace MyGenealogie{

	public class CUnion : CGraphicObject {

		public string ManGuid;
		public string WomanGuid;
		public string StartDate;
		public string EndDate;

		public CUnion(){

			ObjectType	= EObjectType.UNION;
		}
		public CUnion(string strFolderName){

			string [] NameParts = strFolderName.Split(' ');
			ManGuid		= NameParts[0];
			WomanGuid	= NameParts[1];
			ObjectType	= EObjectType.UNION;

			this.LoadXML();
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

			base.CreateControl(ParentForm, MainToolTip, evClick,evDoubleClick, PersonContextMenu , CreateControlOptions,x,y);
		}
		public CPerson GetTheOther(CPerson p){

			if(p.Guid==this.ManGuid){
				return Persons[this.WomanGuid];
			}
			else{
				return Persons[this.ManGuid];
			}
		}
		public override string Text {
			get{
				return "Union";	
			}
		}
		public override string ToolTipText {
			get{
				string s="";
				s	+=	((CPerson)Persons[this.ManGuid]).Text   + CSLib.CSLibGlobal.NewLine;
				s	+=	((CPerson)Persons[this.WomanGuid]).Text + CSLib.CSLibGlobal.NewLine;
				return s;
			}
		}
		public bool Create(string ManGuid, string WomanGuid,string StartDate, string EndDate){

			this.ManGuid		=	ManGuid;
			this.WomanGuid		=	WomanGuid;
			this.StartDate		=	StartDate;
			this.EndDate		=	EndDate;

			CSLib.File.CTextFile.CreateDir(FolderPath);
			CSLib.File.CTextFile.CopyFile(MyGenealogie.GeneaTools.XMLUnionFileTemplate,XMLFileName); 

			LoadXML();

			// we have to do this 2 time, because loading the template will override the data
			this.Guid			=	CSLib.CWindows.CreateGUIDKey();
			this.ManGuid		=	ManGuid;
			this.WomanGuid		=	WomanGuid;
			this.StartDate		=	StartDate;
			this.EndDate		=	EndDate;

			WriteXMLProperty("Guid");
			WriteXMLProperty("ManGuid");
			WriteXMLProperty("WomanGuid");
			WriteXMLProperty("StartDate");
			WriteXMLProperty("EndDate");

			this.SaveXML();

			return true;
		}
		public string FolderName{
			get{
					return ManGuid + " " + WomanGuid;
			}
		}
		public string FolderPath{
			get{
					return MyGenealogie.GeneaTools.UnionFolder + @"\" + FolderName;
			}
		}
		public override string XMLFileName {
			get {
					return FolderPath + @"\u.xml";
				}
		}
		public string UnionFolder {
			get {
				return MyGenealogie.GeneaTools.UnionFolder;
			}
		}		
		private void LoadXML(){

			try{
				XMLDoc = new System.Xml.XmlDocument();
				XMLDoc.Load(XMLFileName);

				ReadXMLProperty("Guid");
				ReadXMLProperty("ManGuid");
				ReadXMLProperty("WomanGuid");
				ReadXMLProperty("StartDate");
				ReadXMLProperty("EndDate");
			}
			catch{
				
				throw new System.Exception(string.Format(GeneaTools.ERR_1001,this.FolderName));				
			}
		}
	}
}