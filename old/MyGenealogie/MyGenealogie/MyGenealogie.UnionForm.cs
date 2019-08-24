using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MyGenealogie{
	/// <summary>
	/// Summary description for MyGenealogie.
	/// </summary>
	public class frmUnion : System.Windows.Forms.Form 	{

		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Button CANCEL;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox StartDate;
		private System.Windows.Forms.TextBox EndDate;
		private System.Windows.Forms.ComboBox cboMAN;
		private System.Windows.Forms.ComboBox cboWoman;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmUnion(){

			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )	{
			if( disposing ) {
				if(components != null) {
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
			this.OK = new System.Windows.Forms.Button();
			this.CANCEL = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboWoman = new System.Windows.Forms.ComboBox();
			this.cboMAN = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.EndDate = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.StartDate = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// OK
			// 
			this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OK.Location = new System.Drawing.Point(400, 152);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(88, 32);
			this.OK.TabIndex = 0;
			this.OK.Text = "OK";
			// 
			// CANCEL
			// 
			this.CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CANCEL.Location = new System.Drawing.Point(496, 152);
			this.CANCEL.Name = "CANCEL";
			this.CANCEL.Size = new System.Drawing.Size(88, 32);
			this.CANCEL.TabIndex = 1;
			this.CANCEL.Text = "Cancel";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cboWoman);
			this.groupBox1.Controls.Add(this.cboMAN);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.EndDate);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.StartDate);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(576, 136);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// cboWoman
			// 
			this.cboWoman.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboWoman.Location = new System.Drawing.Point(144, 40);
			this.cboWoman.Name = "cboWoman";
			this.cboWoman.Size = new System.Drawing.Size(384, 21);
			this.cboWoman.Sorted = true;
			this.cboWoman.TabIndex = 9;
			// 
			// cboMAN
			// 
			this.cboMAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboMAN.Location = new System.Drawing.Point(144, 16);
			this.cboMAN.Name = "cboMAN";
			this.cboMAN.Size = new System.Drawing.Size(384, 21);
			this.cboMAN.Sorted = true;
			this.cboMAN.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 88);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(120, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "End Date:";
			// 
			// EndDate
			// 
			this.EndDate.Location = new System.Drawing.Point(144, 88);
			this.EndDate.Name = "EndDate";
			this.EndDate.Size = new System.Drawing.Size(384, 20);
			this.EndDate.TabIndex = 6;
			this.EndDate.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 64);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 16);
			this.label3.TabIndex = 5;
			this.label3.Text = "Start Date:";
			// 
			// StartDate
			// 
			this.StartDate.Location = new System.Drawing.Point(144, 64);
			this.StartDate.Name = "StartDate";
			this.StartDate.Size = new System.Drawing.Size(384, 20);
			this.StartDate.TabIndex = 4;
			this.StartDate.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Woman:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Man:";
			// 
			// frmUnion
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CANCEL;
			this.ClientSize = new System.Drawing.Size(592, 191);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.CANCEL);
			this.Controls.Add(this.OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "frmUnion";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Union";
			this.Load += new System.EventHandler(this.MyGenealogie_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void MyGenealogie_Load(object sender, System.EventArgs e){
		
		}
		public bool CreateUnion(CPerson UnionMember1, CGeneaManager GeneaManager){

			GeneaManager.Persons.FillComboBox(cboMAN	, UnionMember1.IsMan() ? UnionMember1.Guid : null);
			GeneaManager.Persons.FillComboBox(cboWoman	, UnionMember1.IsMan() ? null : UnionMember1.Guid);

			if(this.ShowDialog()==DialogResult.OK){

				CPerson Man   = (CPerson)((CSLib.CObjectBoxItem)cboMAN.SelectedItem).Value;
				CPerson Woman = (CPerson)((CSLib.CObjectBoxItem)cboWoman.SelectedItem).Value;

				CUnion u = new CUnion();
				u.Create(Man.Guid,Woman.Guid,this.StartDate.Text, this.EndDate.Text);
				return true;
			}
			else{
				return false;
			}
		}
	}
}
