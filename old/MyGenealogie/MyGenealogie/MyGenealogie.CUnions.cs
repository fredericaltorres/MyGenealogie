using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;


namespace MyGenealogie{

	public class CUnions : CSLib.CObjectCollection {

		public CUnion Add(CUnion p){

			return (CUnion)this.Add(p.Guid,p);
		}
		public CUnion Add(string strFolderName){
			try{
				CUnion p = new CUnion(strFolderName);
				return this.Add(p);
			}
			catch(System.Exception e){

				GeneaTools.ShowError(e.Message);
				return null;
			}
		}
		public CUnion this[string strGuid]{
			get {
				return (CUnion)base[strGuid.ToUpper()];
			}
		}
		public void LoadUnions(){

			CSLib.File.CTextFile f		= new CSLib.File.CTextFile();
			CSLib.CVariables Uniones	= f.GetDirList(MyGenealogie.GeneaTools.UnionFolder,"*.*");

			foreach(CSLib.CVariable Unione in Uniones){

				this.Add(new CUnion(Unione.Name));
			}
		}
	}
}
