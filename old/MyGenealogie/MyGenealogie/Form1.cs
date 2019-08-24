using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MyGenealogie {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : CSLib.Forms.Form 	{
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuGeneratePersoneXML;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuQuit;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuViewRefresh;

		
		private MyGenealogie.CGeneaManager GeneaManager;
		private System.Windows.Forms.MenuItem mnuTest;

		private System.Windows.Forms.Label m_Label;
		private System.Windows.Forms.ToolTip MainToolTip;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.ContextMenu PersonContextMenu;
		private System.Windows.Forms.MenuItem mnuUnion;
		private System.Windows.Forms.MenuItem mnuUnionNew;
		private System.Windows.Forms.MenuItem mnuUnionEdit;
		private System.Windows.Forms.MenuItem mnuUnionDelete;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.PictureBox pictureBox1;

		private CPerson SelectedPerson;
		private CUnion  SelectedUnion;
		private System.Windows.Forms.MenuItem mnuViewNew;
		private System.Windows.Forms.MenuItem mnuViewOpen;
		private System.Windows.Forms.MenuItem mnuViewSave;
		private CGraphicObject SelectedGraphicObject;
		private CView View;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem mnuViewAddChildren;
		private System.Windows.Forms.MenuItem mnuViewAddUnion;
		private System.Windows.Forms.MenuItem mnuViewAddParent;
		private System.Windows.Forms.MenuItem mnuPersonSetRelation;
		private System.Windows.Forms.MenuItem mnuPersonnew;
		private System.Windows.Forms.MenuItem mnuPersonEdit;
		private bool ControlKeyPressed;


		public Form1(){
			InitializeComponent();
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuQuit = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mnuViewNew = new System.Windows.Forms.MenuItem();
			this.mnuViewOpen = new System.Windows.Forms.MenuItem();
			this.mnuViewSave = new System.Windows.Forms.MenuItem();
			this.mnuViewRefresh = new System.Windows.Forms.MenuItem();
			this.mnuTools = new System.Windows.Forms.MenuItem();
			this.mnuGeneratePersoneXML = new System.Windows.Forms.MenuItem();
			this.mnuTest = new System.Windows.Forms.MenuItem();
			this.MainToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
			this.PersonContextMenu = new System.Windows.Forms.ContextMenu();
			this.mnuUnion = new System.Windows.Forms.MenuItem();
			this.mnuUnionNew = new System.Windows.Forms.MenuItem();
			this.mnuUnionEdit = new System.Windows.Forms.MenuItem();
			this.mnuUnionDelete = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuPersonSetRelation = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuViewAddChildren = new System.Windows.Forms.MenuItem();
			this.mnuViewAddUnion = new System.Windows.Forms.MenuItem();
			this.mnuViewAddParent = new System.Windows.Forms.MenuItem();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.mnuPersonnew = new System.Windows.Forms.MenuItem();
			this.mnuPersonEdit = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem4,
																					  this.mnuTools});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem3,
																					  this.mnuQuit});
			this.menuItem1.Text = "Person";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 0;
			this.menuItem3.Text = "-";
			// 
			// mnuQuit
			// 
			this.mnuQuit.Index = 1;
			this.mnuQuit.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
			this.mnuQuit.Text = "Quit";
			this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuViewNew,
																					  this.mnuViewOpen,
																					  this.mnuViewSave,
																					  this.mnuViewRefresh});
			this.menuItem4.Text = "View";
			// 
			// mnuViewNew
			// 
			this.mnuViewNew.Index = 0;
			this.mnuViewNew.Text = "New";
			this.mnuViewNew.Click += new System.EventHandler(this.mnuViewNew_Click);
			// 
			// mnuViewOpen
			// 
			this.mnuViewOpen.Index = 1;
			this.mnuViewOpen.Text = "Open";
			// 
			// mnuViewSave
			// 
			this.mnuViewSave.Index = 2;
			this.mnuViewSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mnuViewSave.Text = "Save";
			this.mnuViewSave.Click += new System.EventHandler(this.mnuViewSave_Click);
			// 
			// mnuViewRefresh
			// 
			this.mnuViewRefresh.Index = 3;
			this.mnuViewRefresh.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.mnuViewRefresh.Text = "Refresh";
			this.mnuViewRefresh.Click += new System.EventHandler(this.mnuViewRefresh_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.Index = 2;
			this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuGeneratePersoneXML,
																					 this.mnuTest});
			this.mnuTools.Text = "Tools";
			// 
			// mnuGeneratePersoneXML
			// 
			this.mnuGeneratePersoneXML.Index = 0;
			this.mnuGeneratePersoneXML.Text = "Generate Persone XML";
			this.mnuGeneratePersoneXML.Click += new System.EventHandler(this.mnuGeneratePersoneXML_Click);
			// 
			// mnuTest
			// 
			this.mnuTest.Index = 1;
			this.mnuTest.Shortcut = System.Windows.Forms.Shortcut.F12;
			this.mnuTest.Text = "Test";
			this.mnuTest.Click += new System.EventHandler(this.mnuTest_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 654);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanel1,
																						  this.statusBarPanel2});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(992, 24);
			this.statusBar1.TabIndex = 0;
			this.statusBar1.Text = "statusBar1";
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 488;
			// 
			// statusBarPanel2
			// 
			this.statusBarPanel2.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel2.Text = "statusBarPanel2";
			this.statusBarPanel2.Width = 488;
			// 
			// PersonContextMenu
			// 
			this.PersonContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.mnuUnion,
																							  this.menuItem2,
																							  this.menuItem5});
			// 
			// mnuUnion
			// 
			this.mnuUnion.Index = 0;
			this.mnuUnion.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuUnionNew,
																					 this.mnuUnionEdit,
																					 this.mnuUnionDelete});
			this.mnuUnion.Text = "Union";
			// 
			// mnuUnionNew
			// 
			this.mnuUnionNew.Index = 0;
			this.mnuUnionNew.Text = "New";
			this.mnuUnionNew.Click += new System.EventHandler(this.mnuUnionNew_Click);
			// 
			// mnuUnionEdit
			// 
			this.mnuUnionEdit.Index = 1;
			this.mnuUnionEdit.Text = "Edit";
			// 
			// mnuUnionDelete
			// 
			this.mnuUnionDelete.Index = 2;
			this.mnuUnionDelete.Text = "Delete";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuPersonSetRelation,
																					  this.mnuPersonnew,
																					  this.mnuPersonEdit});
			this.menuItem2.Text = "Person";
			// 
			// mnuPersonSetRelation
			// 
			this.mnuPersonSetRelation.Index = 0;
			this.mnuPersonSetRelation.Text = "Set Relation";
			this.mnuPersonSetRelation.Click += new System.EventHandler(this.mnuNewPersonFather_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuViewAddChildren,
																					  this.mnuViewAddUnion,
																					  this.mnuViewAddParent});
			this.menuItem5.Text = "View";
			// 
			// mnuViewAddChildren
			// 
			this.mnuViewAddChildren.Index = 0;
			this.mnuViewAddChildren.Text = "Add Children";
			this.mnuViewAddChildren.Click += new System.EventHandler(this.mnuViewAddChildren_Click);
			// 
			// mnuViewAddUnion
			// 
			this.mnuViewAddUnion.Index = 1;
			this.mnuViewAddUnion.Text = "Add Union";
			this.mnuViewAddUnion.Click += new System.EventHandler(this.mnuViewAddUnion_Click);
			// 
			// mnuViewAddParent
			// 
			this.mnuViewAddParent.Index = 2;
			this.mnuViewAddParent.Text = "Add Parent";
			this.mnuViewAddParent.Click += new System.EventHandler(this.mnuViewAddParent_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(416, 360);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(176, 160);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// mnuPersonnew
			// 
			this.mnuPersonnew.Index = 1;
			this.mnuPersonnew.Text = "New";
			this.mnuPersonnew.Click += new System.EventHandler(this.mnuPersonnew_Click);
			// 
			// mnuPersonEdit
			// 
			this.mnuPersonEdit.Index = 2;
			this.mnuPersonEdit.Text = "Edit";
			this.mnuPersonEdit.Click += new System.EventHandler(this.mnuPersonEdit_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(992, 678);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.statusBar1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MyGenealogie";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Closed += new System.EventHandler(this.Form1_Closed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Form1());
		}
		private void mnuGeneratePersoneXML_Click(object sender, System.EventArgs e){

			GeneaTools t = new MyGenealogie.GeneaTools();
			t.GeneratePersoneXML();
		}
		private void mnuQuit_Click(object sender, System.EventArgs e){

			this.Close();
		}
		private void mnuViewRefresh_Click(object sender, System.EventArgs e){

			if(this.View!=null){

				View.Show();
				this.Invalidate();
			}
		}
		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e){
			
			//if(View.MustShowRelation){

				View.ShowRelation(e.Graphics);
			//}
		}
		private void Form1_Load(object sender, System.EventArgs e){

			GeneaManager = new MyGenealogie.CGeneaManager(@"N:\MyGenalogie");

			this.LoadUserInfo();

			View = new CView(this,this.MainToolTip,new System.EventHandler(GraphicObject_Click),new System.EventHandler(GraphicObject_DoubleClick),this.PersonContextMenu,GeneaManager.Persons);
			View.FileName = @"C:\DVT\MyGenalogie.db\CurrentView.View";			
			View.Load();
			mnuViewRefresh_Click(null,null);			
		}
		private void Form1_Closed(object sender, System.EventArgs e){

			this.SaveUserInfo();
		}
		private void mnuTest_Click(object sender, System.EventArgs e){
			
//			GeneaManager.CreateControl(this,this.MainToolTip,new System.EventHandler(GraphicObject_Click),this.PersonContextMenu);

//			m_Label					= new System.Windows.Forms.Label();
//			m_Label.BackColor		= System.Drawing.Color.White;
//			m_Label.BorderStyle		= System.Windows.Forms.BorderStyle.FixedSingle;
//			m_Label.FlatStyle		= System.Windows.Forms.FlatStyle.System;
//			m_Label.Location		= new System.Drawing.Point(11, 11);
//			m_Label.Name			= "m_Label";
//			m_Label.Size			= new System.Drawing.Size(128, 32);
//			m_Label.TabIndex		= 0;
//			m_Label.Text			= "My Test";
//
//			this.MainToolTip.SetToolTip(m_Label, "TORRES Frederic, Antoine Leon"+CSLib.CSLibGlobal.NewLine+"1964-"+CSLib.CSLibGlobal.NewLine+"qwew ewq eqw eqwe qwelqweqwewqeqe");
//
//			m_Label.Click		   += new System.EventHandler(m_Label_Click);
//			this.Controls.Add(m_Label);
		}

	
		private void GraphicObject_DoubleClick(object sender, System.EventArgs e){

			mnuPersonEdit_Click(sender,e);
		}
		private void GraphicObject_Click(object sender, System.EventArgs e){

			if(SelectedPerson!=null){
				SelectedPerson.Selected = false;
			}
			if(SelectedUnion!=null){
				SelectedUnion.Selected = false;
			}

			System.Windows.Forms.Label l	= (System.Windows.Forms.Label)sender;
			SelectedPerson					= GeneaManager.Persons[l.Tag.ToString()];
			if(SelectedPerson!=null){
				
				SelectedGraphicObject = SelectedPerson;
				SelectedPerson.Selected = true;
				ShowPersonDetail(SelectedPerson);
			}
			SelectedUnion					= GeneaManager.Unions[l.Tag.ToString()];
			if(SelectedUnion!=null){
				
				SelectedGraphicObject = SelectedUnion;
				SelectedUnion.Selected = true;
				ShowUnionDetail(SelectedUnion);
			}
		}
		private void ShowUnionDetail(CUnion SelectedUnion){

		}
		private void ShowPersonDetail(CPerson SelectedPerson){

			this.Status(SelectedPerson.Text);

			if(SelectedPerson.ImageExist){
				
				this.pictureBox1.Image = System.Drawing.Image.FromFile(SelectedPerson.ImageFullPath);
			}
			else{
				this.pictureBox1.Image = null;
			}
		}
		private void mnuUnionNew_Click(object sender, System.EventArgs e){

			frmUnion f = new frmUnion();

			if(f.CreateUnion(this.SelectedPerson, this.GeneaManager)){

				this.GeneaManager.LoadUnions();
				mnuViewRefresh_Click(null,null);
			}
		}
		private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e){

			switch(e.KeyCode){

				case System.Windows.Forms.Keys.ControlKey:
					 ControlKeyPressed = true;
				break;
				case System.Windows.Forms.Keys.Right:
					if(SelectedGraphicObject!=null){
						SelectedGraphicObject.Label.Left +=2;
					}
				break;

				case System.Windows.Forms.Keys.Left:
					if(SelectedGraphicObject!=null){
						SelectedGraphicObject.Label.Left -=2;
					}
				break;

				case System.Windows.Forms.Keys.Down:
					if(SelectedGraphicObject!=null){
						SelectedGraphicObject.Label.Top +=2;
					}
				break;

				case System.Windows.Forms.Keys.Up:
					if(SelectedGraphicObject!=null){
						SelectedGraphicObject.Label.Top -=2;
					}
				break;
			}
		}
		private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e){

			switch(e.KeyCode){

				case System.Windows.Forms.Keys.ControlKey:

						ControlKeyPressed = false;
				break;
			}
		}
		private void mnuViewNew_Click(object sender, System.EventArgs e) {

			PersonSelectorForm f = new PersonSelectorForm();
			CPerson p = f.OpenDialog(this.GeneaManager);
			if(p!=null){

				View = new CView(this,this.MainToolTip,new System.EventHandler(GraphicObject_Click),new System.EventHandler(GraphicObject_DoubleClick),this.PersonContextMenu,GeneaManager.Persons);
				View.FileName = @"C:\DVT\MyGenalogie.db\CurrentView.View";
				View.Add(p,0,0);
				mnuViewRefresh_Click(null,null);
			}	
		}
		private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e){

			if((e.Button==System.Windows.Forms.MouseButtons.Left)&&(ControlKeyPressed)){

				if(SelectedGraphicObject!=null){

					SelectedGraphicObject.X = e.X;
					SelectedGraphicObject.Y = e.Y;
					mnuViewRefresh_Click(null,null);
				}
			}
		}
		private void mnuViewSave_Click(object sender, System.EventArgs e){

			this.View.Save();
		}
		private void mnuViewAddChildren_Click(object sender, System.EventArgs e){

			if(SelectedPerson!=null){

				this.View.AddChildren(SelectedPerson);
				mnuViewRefresh_Click(null,null);
			}
		}
		private void mnuViewAddUnion_Click(object sender, System.EventArgs e){
		
			if(SelectedPerson!=null){

				this.View.AddUnion(SelectedPerson);
				mnuViewRefresh_Click(null,null);
			}
		}
		private void mnuViewAddParent_Click(object sender, System.EventArgs e){

			if(SelectedPerson!=null){

				this.View.AddParent(SelectedPerson);
				mnuViewRefresh_Click(null,null);
			}		
		}
		private void mnuNewPersonFather_Click(object sender, System.EventArgs e){

			if(SelectedPerson!=null){

				PersonSetRelationForms f= new PersonSetRelationForms();

				if(f.OpenDialog(SelectedPerson,this.GeneaManager)){

					mnuViewRefresh_Click(null,null);
				}		
			}
		}
		private void Form1_Resize(object sender, System.EventArgs e){
			mnuViewRefresh_Click(null,null);		
			pictureBox1.Left = this.Width - pictureBox1.Width;
			pictureBox1.Top = 0;
		}
		private void mnuPersonnew_Click(object sender, System.EventArgs e){

			CSLib.CSLibGlobal.MsgBoxWarning(GeneaTools.ERR_1005);		
		}
		private void mnuPersonEdit_Click(object sender, System.EventArgs e){

			if(SelectedPerson!=null){

				PersonForm f = new PersonForm();

				if(f.OpenDialog(SelectedPerson, this.GeneaManager)){

					mnuViewRefresh_Click(null,null);
				}
			}
		}
	}
}
