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
	public class PersonSetRelationForms : System.Windows.Forms.Form	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button CANCEL;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.ComboBox cboPerson2;
		private System.Windows.Forms.ComboBox cboPerson1;
		private System.Windows.Forms.ComboBox cboRelation;
		private System.Windows.Forms.Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PersonSetRelationForms()		{
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cboRelation = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cboPerson2 = new System.Windows.Forms.ComboBox();
			this.cboPerson1 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.CANCEL = new System.Windows.Forms.Button();
			this.OK = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cboRelation);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.cboPerson2);
			this.groupBox1.Controls.Add(this.cboPerson1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(576, 104);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			// 
			// cboRelation
			// 
			this.cboRelation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboRelation.Location = new System.Drawing.Point(144, 40);
			this.cboRelation.Name = "cboRelation";
			this.cboRelation.Size = new System.Drawing.Size(384, 21);
			this.cboRelation.Sorted = true;
			this.cboRelation.TabIndex = 11;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(120, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "Relation:";
			// 
			// cboPerson2
			// 
			this.cboPerson2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPerson2.Location = new System.Drawing.Point(144, 64);
			this.cboPerson2.Name = "cboPerson2";
			this.cboPerson2.Size = new System.Drawing.Size(384, 21);
			this.cboPerson2.Sorted = true;
			this.cboPerson2.TabIndex = 9;
			// 
			// cboPerson1
			// 
			this.cboPerson1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPerson1.Location = new System.Drawing.Point(144, 16);
			this.cboPerson1.Name = "cboPerson1";
			this.cboPerson1.Size = new System.Drawing.Size(384, 21);
			this.cboPerson1.Sorted = true;
			this.cboPerson1.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(120, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Person:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Person:";
			// 
			// CANCEL
			// 
			this.CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CANCEL.Location = new System.Drawing.Point(488, 120);
			this.CANCEL.Name = "CANCEL";
			this.CANCEL.Size = new System.Drawing.Size(88, 32);
			this.CANCEL.TabIndex = 4;
			this.CANCEL.Text = "Cancel";
			// 
			// OK
			// 
			this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OK.Location = new System.Drawing.Point(392, 120);
			this.OK.Name = "OK";
			this.OK.Size = new System.Drawing.Size(88, 32);
			this.OK.TabIndex = 3;
			this.OK.Text = "OK";
			// 
			// PersonSetRelationForms
			// 
			this.AcceptButton = this.OK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CANCEL;
			this.ClientSize = new System.Drawing.Size(594, 159);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.CANCEL);
			this.Controls.Add(this.OK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "PersonSetRelationForms";
			this.Text = "Relation";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public bool OpenDialog(CPerson P1, CGeneaManager GeneaManager){

			GeneaManager.Persons.FillComboBox(this.cboPerson1	, P1.Guid );
			GeneaManager.Persons.FillComboBox(this.cboPerson2	, null);
			GeneaManager.FillComboBoxWithRelation(this.cboRelation);

			if(this.ShowDialog()==DialogResult.OK){

				CPerson p1   = (CPerson)((CSLib.CObjectBoxItem)cboPerson1.SelectedItem).Value;
				CPerson p2   = (CPerson)((CSLib.CObjectBoxItem)cboPerson2.SelectedItem).Value;
				ERelation Relation = (ERelation)((CSLib.CObjectBoxItem)this.cboRelation.SelectedItem).Value;

				p1.SetRelation(p2,Relation);
				
				return true;
			}
			else{
				return false;
			}
		}

		
	}
}
