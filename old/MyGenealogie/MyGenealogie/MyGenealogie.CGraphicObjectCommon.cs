using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace MyGenealogie{

	public enum EObjectType {

		PERSON,
		UNION,
	};

	public enum ERelation {

		FatherOf,
		MotherOf,
		ChildOfFather,
		ChildOfMother,
		//DaugtherOfFather,
		//DaugtherOfMother,
	};
	
	
	public class CGraphicObjectCommon {

		public const int ECCO_NONE					= 0;
		public const int ECCO_INC_Y					= 1;
		public const int ECCO_SHOW_UNION			= 2;
		public const int ECCO_SHOW_CHILDREN			= 4;
		public const int ECCO_SHOW_PARENT			= 8;


		public static int CellWidth  = 150;
		public static int CellHeight = 24;

//		public static int CellNextX  = 0;
//		public static int CellNextY  = 0;

		public static System.Collections.Stack Stack = new System.Collections.Stack();

//		public static void PushCellNextX(){
//
//			Stack.Push(CellNextX);
//		}
//		public static void PopCellNextX(){
//
//			CellNextX = (int)Stack.Pop();
//		}
//		public static void PushCellNextY(){
//
//			Stack.Push(CellNextY);
//		}
//		public static void PopCellNextY(){
//
//			CellNextY = (int)Stack.Pop();
//		}

		protected EObjectType ObjectType;
		
		protected Pen pen;

		public virtual string Text			{ get { return "None"; } }
		public virtual string ToolTipText	{ get { return "None"; } }
		
		public static void PrepareDraw(){

			Stack.Clear();
			//CellNextX = CellNextY = 0;
		}
	}

	public class CGraphicObject : CGraphicObjectCommon {

		public static CPersons Persons; // This need to move to an extra layer
		public static CUnions  Unions;

		public		string								Guid;
		public		bool								Displayed;

		protected	System.Xml.XmlDocument XMLDoc;
		protected	System.Drawing.Rectangle			m_Position;
		protected	System.Windows.Forms.Label			m_Label;
		protected	int									m_X;
		protected	int									m_Y;

		private		System.Windows.Forms.ToolTip		m_MainToolTip;

		private		bool								m_Selected;

		
		public virtual string XMLFileName {
			get{
				   return "";
			   }
		}
		protected virtual void SaveXML(){
			try{				
				XMLDoc.Save(XMLFileName);
			}
			catch{				
				throw new System.Exception(string.Format(GeneaTools.ERR_1002,XMLFileName));
			}
		}
		protected void ReadXMLProperty(string strProperty){

			System.Xml.XmlNode Node = XMLDoc.SelectSingleNode("/Person/"+strProperty);

			CSLib.LateBinder lb = new CSLib.LateBinder (this);

			if(Node!=null){				
				string strValue = Node.InnerText;
				lb.SetProperty(strProperty,strValue);
			}
			else{
				lb.SetProperty(strProperty,null);			
			}
		}
		protected void WriteXMLProperty(string strProperty){

			System.Xml.XmlNode Node = XMLDoc.SelectSingleNode("/Person/"+strProperty);

			CSLib.LateBinder lb = new CSLib.LateBinder (this);
			string strValue		= lb.GetProperty(strProperty).ToString();
			Node.InnerText		= strValue;
		}
		public CGraphicObject(){
			
			pen = new Pen (Color.Black,1);
		}		
		public System.Windows.Forms.Label Label{
			get{
				return m_Label;
			}
			set {
				m_Label = value;
			}
		}
		public int X {
			get{
				return m_X;	
			}
			set {
				m_X = value;
				if(this.Label!=null){
					this.Label.Left = X;
				}
			}
		}
		public int Y {
			get{
				return m_Y;	
			}
			set {
				m_Y = value;
				if(this.Label!=null){
					this.Label.Top = Y;
				}
			}
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
		public virtual void CreateControl(

					System.Windows.Forms.Form			ParentForm,
					System.Windows.Forms.ToolTip		MainToolTip,
					System.EventHandler					evClick,
					System.EventHandler					evDoubleClick,
					System.Windows.Forms.ContextMenu	PersonContextMenu,
					int									CreateControlOptions,
					int									x,
					int									y
			){

			this.m_X					= x;
			this.m_Y					= y;
			this.Label					= new System.Windows.Forms.Label();
			this.Label.BackColor		= this.ObjectType == EObjectType.PERSON ? System.Drawing.Color.White : System.Drawing.Color.LightBlue;
			this.Label.ForeColor		= this.ObjectType == EObjectType.PERSON ? System.Drawing.Color.Black : System.Drawing.Color.Black;
			this.Label.BorderStyle		= System.Windows.Forms.BorderStyle.FixedSingle;
			this.Label.TextAlign		= System.Drawing.ContentAlignment.MiddleCenter;
			this.Label.FlatStyle		= System.Windows.Forms.FlatStyle.System;

			if(this.ObjectType == EObjectType.PERSON){

				this.Label.Location			= new System.Drawing.Point(X,Y);
				//this.Label.Size				= new System.Drawing.Size(CellWidth-3,CellHeight);
			}
			else{
				this.Label.Location			= new System.Drawing.Point(X,Y);
				//this.Label.Size				= new System.Drawing.Size(CellWidth-3,CellHeight);
			}

			this.Label.Name				= "GraphicObject:"+this.Guid;
			this.Label.TabIndex			= 0;
			this.Label.Text				= this.Text;
			this.Label.Tag				= this.Guid;
			this.Label.ContextMenu		= PersonContextMenu;
			this.Label.AutoSize			= true;
			this.Label.Click		   += evClick;
			this.Label.DoubleClick	   += evDoubleClick;
			
			m_MainToolTip				= MainToolTip;
			ResetInternalData();
			ParentForm.Controls.Add(this.Label);

			this.Displayed = true;
		}
		public void ResetInternalData(){

			m_MainToolTip.SetToolTip(this.Label, this.ToolTipText);
		}
		
		public bool Selected {
			get {
				return m_Selected;
			}
			set{
				m_Selected = value;

				if(m_Label!=null){

					if(!Selected){

						this.Label.BackColor = this.ObjectType == EObjectType.PERSON ? System.Drawing.Color.White : System.Drawing.Color.LightBlue;
					}
					else{
						this.Label.BackColor = System.Drawing.Color.Bisque;
					}
				}
			}
		}
	}
}
