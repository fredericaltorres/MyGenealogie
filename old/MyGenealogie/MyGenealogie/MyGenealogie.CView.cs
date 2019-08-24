using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace MyGenealogie {

	public class CView {

		public string FileName;

		System.Windows.Forms.Form			ParentForm;
		System.Windows.Forms.ToolTip		MainToolTip;
		System.EventHandler					EventHandlerClick;
		System.EventHandler					EventHandlerDoubleClick;
		System.Windows.Forms.ContextMenu	PersonContextMenu;

		public bool MustShowRelation;
		private CPersons Persons;
		private CPersons GlobalPersons;

		public CView(

				System.Windows.Forms.Form			ParentForm,
				System.Windows.Forms.ToolTip		MainToolTip,					
				System.EventHandler					EventHandlerClick,
				System.EventHandler					EventHandlerDoubleClick,
				System.Windows.Forms.ContextMenu	PersonContextMenu,
				CPersons GlobalPersons
			){

			this.ParentForm					=	ParentForm;
			this.MainToolTip				=	MainToolTip;
			this.EventHandlerClick			=	EventHandlerClick;
			this.EventHandlerDoubleClick	=	EventHandlerDoubleClick;
			this.PersonContextMenu			=	PersonContextMenu;
			this.GlobalPersons				=	GlobalPersons;
			Clear();			
		}
		public void Add(CPerson FirstPerson, int x, int y){

			if(this.Persons.Exist(FirstPerson.Guid)){

				GeneaTools.ShowError(GeneaTools.ERR_1003,FirstPerson.Text);
			}
			else{
				this.Persons.Add(FirstPerson);
				FirstPerson.X = x;
				FirstPerson.Y = y;
			}
		}
		public void Clear(){

			Persons = new CPersons();
		}
		public void Save(){

			System.Text.StringBuilder t = new System.Text.StringBuilder(16384);

			foreach(CPerson p in this.Persons){

				t.AppendFormat("{0},{1},{2},{3}{4}",p.Guid,p.X,p.Y,p.Text,CSLib.CSLibGlobal.CR);
			}
			CSLib.File.CTextFile f = new CSLib.File.CTextFile();
			f.LogFile(this.FileName,t.ToString(),true);
		}
		public void Load(){

			CSLib.File.CTextFile f = new CSLib.File.CTextFile();

			if(CSLib.File.CTextFile.Exist(FileName)){

				string [] PersonsInfo =  f.LoadFile(this.FileName).Split(CSLib.CSLibGlobal.CR[0]);

				foreach(string s in PersonsInfo){

					string [] item =  s.Split(',');
					CPerson p = this.GlobalPersons[item[0]];
					if(p==null){
					}
					else{
						this.Add(p, CSLib.CSLibGlobal.CInt(item[1]), CSLib.CSLibGlobal.CInt(item[2]));
					}
				}
			}
		}
		public void Show(){
			
			foreach(CPerson p in this.Persons){

				if(!p.Displayed){
					p.CreateControl( this.ParentForm, this.MainToolTip, this.EventHandlerClick,this.EventHandlerDoubleClick, this.PersonContextMenu,CGraphicObjectCommon.ECCO_NONE,p.X,p.Y);
				}
			}
			MustShowRelation=true;
		}
//		public virtual void Draw(System.Drawing.Graphics g, System.Drawing.Font font, bool booIncY){
//			
//			// Store the graphic position
//			m_Position.X			= X;
//			m_Position.Y			= Y;
//			m_Position.Width		= CellWidth-3;
//			m_Position.Height		= CellHeight;
//
//			g.DrawRectangle(pen,X,Y,CellWidth-3,CellHeight);
//			g.DrawString(Text,font,new SolidBrush(pen.Color),X+1,Y+1);
//		}
//		public System.Windows.Forms.Label Label{
//			get{
//				return m_Label;
//			}
//			set {
//				m_Label = value;
//				if(m_Label==null){
//					System.Diagnostics.Debugger.Break();
//				}
//			}
//		}
		public void ShowRelation(System.Drawing.Graphics g){

			MustShowRelation = false;
			ShowUnionRelation(g);
			ShowParentRelation(g);
		}
		public void ShowParentRelation(System.Drawing.Graphics g){

			Pen ParentPen = new Pen (Color.Green,1);

			foreach(CPerson Man in this.Persons){

				if(Man.Displayed){

					CPerson Father = Man.Father;

					if(Father!=null){

						if(Father.Displayed){

							g.DrawLine(ParentPen,Man.X+(Man.Label.Width/2),Man.Y,Father.X+(Father.Label.Width/2),Father.Label.Top+Father.Label.Height);
						}
					}


					CPerson Mother = Man.Mother;

					if(Mother!=null){

						if(Mother.Displayed){

							g.DrawLine(ParentPen,Man.X+(Man.Label.Width/2),Man.Y,Mother.X+(Mother.Label.Width/2),Mother.Label.Top+Mother.Label.Height);
						}
					}

				}
			}
		}

		public void ShowUnionRelation(System.Drawing.Graphics g){

			Pen UnionPen = new Pen (Color.Blue,1);

			foreach(CPerson Man in this.Persons){

				if(Man.IsMan()){

					if(Man.Displayed){

						foreach(CUnion u in Man.GetUnions()){

							CPerson Woman = u.GetTheOther(Man);

							if(Woman.Displayed){

								g.DrawLine(UnionPen,Man.X+Man.Label.Width,Man.Y+(Man.Label.Height/2),Woman.X,Woman.Y+(Woman.Label.Height/2));
							}
						}
					}
				}
			}
		}
		public void AddChildren(CPerson p){

			int x = p.X;
			foreach(CPerson c in p.GetChildren()){

				this.Add(c,x,p.Y+CGraphicObjectCommon.CellHeight);
				Show();
				x+=c.Label.Width+1;
			}
		}
		public void AddParent(CPerson p){

			CPerson Father = p.Father;
			if(Father!=null){
				this.Add(Father,p.X,p.Y-CGraphicObjectCommon.CellHeight);
			}
			CPerson Mother = p.Mother;
			if(Mother!=null){
				this.Add(Mother,p.X+p.Label.Width+1,p.Y-CGraphicObjectCommon.CellHeight);
			}
		}
		public void AddUnion(CPerson p){

			foreach(CUnion u in p.GetUnions()){
				
				this.Add(u.GetTheOther(p),p.X+p.Label.Width+1,p.Y);
				
			}
		}
	}
}
