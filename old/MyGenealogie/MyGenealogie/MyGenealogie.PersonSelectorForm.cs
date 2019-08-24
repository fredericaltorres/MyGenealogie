using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MyGenealogie
{
	/// <summary>
	/// Summary description for MyGenealogie.
	/// </summary>
	public class PersonSelectorForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button CANCEL;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cboMAN;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PersonSelectorForm()		{

			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.CANCEL = new System.Windows.Forms.Button();
			this.OK = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboMAN = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// CANCEL
			// 
			this.CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CANCEL.Location = new System.Drawing.Point(488, 72);
			this.CANCEL.Name = "CANCEL";
			this.CANCEL.Size = new System.Drawing.Size(88, 32);
			this.CANCEL.TabIndex = 4;
			this.CANCEL.Text = "Cancel";
			// 
			// OK
			// 
			this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OK.Location = new System.Drawing.Point(392, 72);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(88, 32);
			this.OK.TabIndex = 3;
			this.OK.Text = "OK";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cboMAN);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(568, 56);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
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
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Person:";
			// 
			// PersonSelectorForm
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CANCEL;
			this.ClientSize = new System.Drawing.Size(592, 117);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.CANCEL);
			this.Controls.Add(this.OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PersonSelectorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Person Selector";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		public CPerson OpenDialog(CGeneaManager GeneaManager){

			GeneaManager.Persons.FillComboBox(cboMAN	, "");

			if(this.ShowDialog()==DialogResult.OK){

				CPerson Man   = (CPerson)((CSLib.CObjectBoxItem)cboMAN.SelectedItem).Value;				
				return Man;
			}
			else{
				return null;
			}
		}	
	}

}
