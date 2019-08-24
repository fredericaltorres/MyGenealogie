using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace MyGenealogie {

	public class CDisplayOptions : CSLib.COptionsManager {

		public string Person1 = "SEMEAC, Pierre";
		public int CellWidth  = 128;
		public int CellHeight =  64;
	}

	public class GeneaTools {

		public const string ERR_1000 = "Cannot parse name '{0}'";
		public const string ERR_1001 = "Cannot parse xml for '{0}'";
		public const string ERR_1002 = "Cannot save xml for '{0}'";
		public const string ERR_1003 = "'{0}' already in the view ";
		public const string ERR_1004 = "Cannot save xml for '{0}'";
		public const string ERR_1005 = "Not implemented - create the person manually";

		public static string CapitalizeString(string s){

			if(s==null){
				return null;
			}
			if(s==""){
				return "";
			}
			s = s.ToLower();
			return s.Substring(0,1).ToUpper() +  s.Substring(1);
		}
		public static void ShowError(string strText,params string[] Defines){
			
			CSLib.CSLibGlobal.MsgBoxError(string.Format(strText,Defines));
		}
		public string Now {
			get {
				return DateTime.Now.ToString("yyyy-mm-dd");
			}
		}
		public static string MyGenalogyDBDbFolder {
			get {
				return CSLib.CSLibGlobal.Environ("MyGenalogyDB");
			}
		}
		public static string PersonFolder {
			get {
				return MyGenalogyDBDbFolder+@"\person";
			}
		}
		public static string UnionFolder {
			get {
				return MyGenalogyDBDbFolder+@"\union";
			}
		}
		public static string XMLPersonFileTemplate {
			get {
				return MyGenalogyDBDbFolder+@"\template\p.xml";
			}
		}
		public static string XMLUnionFileTemplate {
			get {
				return MyGenalogyDBDbFolder+@"\template\u.xml";
			}
		}
		public void GeneratePersoneXML(){
			
			CSLib.File.CTextFile f = new CSLib.File.CTextFile();
			CSLib.CVariables Persones = f.GetDirList(PersonFolder,"*.*");

			string strXMLPersonTemplate = f.LoadFile(XMLPersonFileTemplate);
			
			foreach(CSLib.CVariable Persone in Persones){

				string strPersonPath		= PersonFolder + @"\" + Persone.Name;
				string strPersonXMLFile		= strPersonPath + @"\p.xml";

				if(!CSLib.File.CTextFile.Exist(strPersonXMLFile)){

					string strXML = strXMLPersonTemplate;
					strXML = CSLib.CSLibGlobal.PreProcess(strXML,"GUID",CSLib.CWindows.CreateGUIDKey(),"CREATION_DATE", this.Now);
					f.LogFile(strPersonXMLFile, strXML, true);

					System.Threading.Thread.Sleep(11);
				}
			}
		}
	}
}
