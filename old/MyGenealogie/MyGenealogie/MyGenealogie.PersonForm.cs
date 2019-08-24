using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MyGenealogie{
	/// <summary>
	/// Summary description for MyGenealogie.
	/// </summary>
	public class PersonForm : System.Windows.Forms.Form	{

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button CANCEL;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.TextBox BirthDate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox BirthCity;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox BirthCountry;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox DeathCountry;
		private System.Windows.Forms.TextBox DeathCity;
		private System.Windows.Forms.TextBox DeathDate;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox Comment;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cboFather;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox cboMother;
		private System.Windows.Forms.ComboBox cboSexe;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PersonForm()		{
			InitializeComponent();
			//
		}

		protected override void Dispose( bool disposing )		{
			if( disposing )			{
				if(components != null)				{
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboMother = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.cboFather = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.Comment = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.DeathCountry = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.DeathCity = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.DeathDate = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.BirthCountry = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BirthCity = new System.Windows.Forms.TextBox();
			this.cboSexe = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.BirthDate = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.CANCEL = new System.Windows.Forms.Button();
			this.OK = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cboMother);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.cboFather);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.Comment);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.DeathCountry);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.DeathCity);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.DeathDate);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.BirthCountry);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.BirthCity);
			this.groupBox1.Controls.Add(this.cboSexe);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.BirthDate);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(5, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(544, 440);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// cboMother
			// 
			this.cboMother.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMother.Location = new System.Drawing.Point(144, 360);
			this.cboMother.Name = "cboMother";
			this.cboMother.Size = new System.Drawing.Size(384, 21);
			this.cboMother.Sorted = true;
			this.cboMother.TabIndex = 9;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 368);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(120, 16);
			this.label10.TabIndex = 23;
			this.label10.Text = "Mother:";
			// 
			// cboFather
			// 
			this.cboFather.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboFather.Location = new System.Drawing.Point(144, 336);
			this.cboFather.Name = "cboFather";
			this.cboFather.Size = new System.Drawing.Size(384, 21);
			this.cboFather.Sorted = true;
			this.cboFather.TabIndex = 8;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 344);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(120, 16);
			this.label9.TabIndex = 21;
			this.label9.Text = "Father:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 192);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(120, 16);
			this.label8.TabIndex = 20;
			this.label8.Text = "Comment:";
			// 
			// Comment
			// 
			this.Comment.Location = new System.Drawing.Point(144, 192);
			this.Comment.Multiline = true;
			this.Comment.Name = "Comment";
			this.Comment.Size = new System.Drawing.Size(384, 136);
			this.Comment.TabIndex = 7;
			this.Comment.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 144);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 16);
			this.label5.TabIndex = 18;
			this.label5.Text = "Death Country:";
			// 
			// DeathCountry
			// 
			this.DeathCountry.Location = new System.Drawing.Point(144, 144);
			this.DeathCountry.Name = "DeathCountry";
			this.DeathCountry.Size = new System.Drawing.Size(384, 20);
			this.DeathCountry.TabIndex = 5;
			this.DeathCountry.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(120, 16);
			this.label6.TabIndex = 16;
			this.label6.Text = "Death City:";
			// 
			// DeathCity
			// 
			this.DeathCity.Location = new System.Drawing.Point(144, 120);
			this.DeathCity.Name = "DeathCity";
			this.DeathCity.Size = new System.Drawing.Size(384, 20);
			this.DeathCity.TabIndex = 4;
			this.DeathCity.Text = "";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 96);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(120, 16);
			this.label7.TabIndex = 14;
			this.label7.Text = "Death Date:";
			// 
			// DeathDate
			// 
			this.DeathDate.Location = new System.Drawing.Point(144, 96);
			this.DeathDate.Name = "DeathDate";
			this.DeathDate.Size = new System.Drawing.Size(384, 20);
			this.DeathDate.TabIndex = 3;
			this.DeathDate.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 16);
			this.label4.TabIndex = 12;
			this.label4.Text = "Birth Country:";
			// 
			// BirthCountry
			// 
			this.BirthCountry.Location = new System.Drawing.Point(144, 72);
			this.BirthCountry.Name = "BirthCountry";
			this.BirthCountry.Size = new System.Drawing.Size(384, 20);
			this.BirthCountry.TabIndex = 2;
			this.BirthCountry.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 16);
			this.label2.TabIndex = 10;
			this.label2.Text = "Birth City:";
			// 
			// BirthCity
			// 
			this.BirthCity.Location = new System.Drawing.Point(144, 48);
			this.BirthCity.Name = "BirthCity";
			this.BirthCity.Size = new System.Drawing.Size(384, 20);
			this.BirthCity.TabIndex = 1;
			this.BirthCity.Text = "";
			// 
			// cboSexe
			// 
			this.cboSexe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSexe.Location = new System.Drawing.Point(144, 168);
			this.cboSexe.Name = "cboSexe";
			this.cboSexe.Size = new System.Drawing.Size(112, 21);
			this.cboSexe.Sorted = true;
			this.cboSexe.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Birth Date:";
			// 
			// BirthDate
			// 
			this.BirthDate.Location = new System.Drawing.Point(144, 24);
			this.BirthDate.Name = "BirthDate";
			this.BirthDate.Size = new System.Drawing.Size(384, 20);
			this.BirthDate.TabIndex = 0;
			this.BirthDate.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 170);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Sexe:";
			// 
			// CANCEL
			// 
			this.CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CANCEL.Location = new System.Drawing.Point(448, 464);
			this.CANCEL.Name = "CANCEL";
			this.CANCEL.Size = new System.Drawing.Size(88, 32);
			this.CANCEL.TabIndex = 11;
			this.CANCEL.Text = "Cancel";
			// 
			// OK
			// 
			this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OK.Location = new System.Drawing.Point(352, 464);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(88, 32);
			this.OK.TabIndex = 10;
			this.OK.Text = "OK";
			// 
			// PersonForm
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CANCEL;
			this.ClientSize = new System.Drawing.Size(552, 501);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.CANCEL);
			this.Controls.Add(this.OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PersonForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MyGenealogie";
			this.Load += new System.EventHandler(this.MyGenealogie_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void MyGenealogie_Load(object sender, System.EventArgs e){
		
		}

		public bool OpenDialog(CPerson p, CGeneaManager GeneaManager){

			this.BirthDate.Text			= p.BirthDate;
			this.BirthCountry.Text		= p.BirthCountry;
			this.BirthCity.Text			= p.BirthCity;
			this.DeathDate.Text			= p.DeathDate;
			this.DeathCountry.Text		= p.DeathCountry;
			this.DeathCity.Text			= p.DeathCity;
			this.Comment.Text			= p.Comment;
			this.Text					= p.FolderName;

			GeneaManager.Persons.FillComboBox(this.cboFather , p.FatherGuid , "M");
			GeneaManager.Persons.FillComboBox(this.cboMother , p.MotherGuid , "F");

			GeneaManager.FillComboBoxSexe(this.cboSexe, p.Sexe);

			if(this.ShowDialog() == DialogResult.OK){

				p.BirthDate			= this.BirthDate.Text;
				p.BirthCountry		= this.BirthCountry.Text;
				p.BirthCity			= this.BirthCity.Text;
				p.DeathDate			= this.DeathDate.Text;
				p.DeathCountry		= this.DeathCountry.Text;
				p.DeathCity			= this.DeathCity.Text;
				p.Comment			= this.Comment.Text;
				p.Sexe				= ((CSLib.CObjectBoxItem)cboSexe.SelectedItem).Value == "M" ? "M" : "F";

				if(cboFather.SelectedItem!=null){

					CPerson Father   = (CPerson)((CSLib.CObjectBoxItem)cboFather.SelectedItem).Value;
					p.FatherGuid = Father.Guid;
				}

				if(cboMother.SelectedItem!=null){

					CPerson Mother   = (CPerson)((CSLib.CObjectBoxItem)cboMother.SelectedItem).Value;
					p.MotherGuid = Mother.Guid;
				}

				

				p.Save();

				return true;
			}
			else{
				return false;
			}
		}
	}
}
