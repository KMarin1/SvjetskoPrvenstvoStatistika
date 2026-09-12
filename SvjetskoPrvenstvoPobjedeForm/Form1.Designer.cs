using System.Windows.Forms;

namespace SvjetskoPrvenstvoPobjedeForm
{
    partial class Form1 : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ddlCulture = new System.Windows.Forms.ComboBox();
            this.ddlChampionshipType = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblChampionshipType = new System.Windows.Forms.Label();
            this.lblCulture = new System.Windows.Forms.Label();
            this.cbRepresentation = new System.Windows.Forms.ComboBox();
            this.lblRepresentation = new System.Windows.Forms.Label();
            this.pnlFavorite = new System.Windows.Forms.Panel();
            this.pnlAll = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ddlCulture
            // 
            this.ddlCulture.AllowDrop = true;
            this.ddlCulture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCulture.FormattingEnabled = true;
            this.ddlCulture.Location = new System.Drawing.Point(12, 43);
            this.ddlCulture.Name = "ddlCulture";
            this.ddlCulture.Size = new System.Drawing.Size(49, 21);
            this.ddlCulture.TabIndex = 0;
            // 
            // ddlChampionshipType
            // 
            this.ddlChampionshipType.AllowDrop = true;
            this.ddlChampionshipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlChampionshipType.FormattingEnabled = true;
            this.ddlChampionshipType.Location = new System.Drawing.Point(12, 83);
            this.ddlChampionshipType.Name = "ddlChampionshipType";
            this.ddlChampionshipType.Size = new System.Drawing.Size(121, 21);
            this.ddlChampionshipType.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(15, 110);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "Spremi";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblChampionshipType
            // 
            this.lblChampionshipType.AutoSize = true;
            this.lblChampionshipType.Location = new System.Drawing.Point(12, 67);
            this.lblChampionshipType.Name = "lblChampionshipType";
            this.lblChampionshipType.Size = new System.Drawing.Size(55, 13);
            this.lblChampionshipType.TabIndex = 3;
            this.lblChampionshipType.Text = "Prvenstvo";
            // 
            // lblCulture
            // 
            this.lblCulture.AutoSize = true;
            this.lblCulture.Location = new System.Drawing.Point(15, 24);
            this.lblCulture.Name = "lblCulture";
            this.lblCulture.Size = new System.Drawing.Size(31, 13);
            this.lblCulture.TabIndex = 4;
            this.lblCulture.Text = "Jezik";
            // 
            // cbRepresentation
            // 
            this.cbRepresentation.DisplayMember = "DisplayName";
            this.cbRepresentation.FormattingEnabled = true;
            this.cbRepresentation.Location = new System.Drawing.Point(204, 43);
            this.cbRepresentation.Name = "cbRepresentation";
            this.cbRepresentation.Size = new System.Drawing.Size(121, 21);
            this.cbRepresentation.TabIndex = 5;
            this.cbRepresentation.ValueMember = "FifaCode";
            // 
            // lblRepresentation
            // 
            this.lblRepresentation.AutoSize = true;
            this.lblRepresentation.Location = new System.Drawing.Point(204, 23);
            this.lblRepresentation.Name = "lblRepresentation";
            this.lblRepresentation.Size = new System.Drawing.Size(78, 13);
            this.lblRepresentation.TabIndex = 6;
            this.lblRepresentation.Text = "Reprezentacija";
            // 
            // pnlFavorite
            // 
            this.pnlFavorite.Location = new System.Drawing.Point(204, 83);
            this.pnlFavorite.Name = "pnlFavorite";
            this.pnlFavorite.Size = new System.Drawing.Size(247, 395);
            this.pnlFavorite.TabIndex = 7;
            // 
            // pnlAll
            // 
            this.pnlAll.Location = new System.Drawing.Point(500, 83);
            this.pnlAll.Name = "pnlAll";
            this.pnlAll.Size = new System.Drawing.Size(257, 395);
            this.pnlAll.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 530);
            this.Controls.Add(this.pnlAll);
            this.Controls.Add(this.pnlFavorite);
            this.Controls.Add(this.lblRepresentation);
            this.Controls.Add(this.cbRepresentation);
            this.Controls.Add(this.lblCulture);
            this.Controls.Add(this.lblChampionshipType);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.ddlChampionshipType);
            this.Controls.Add(this.ddlCulture);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlCulture;
        private System.Windows.Forms.ComboBox ddlChampionshipType;
        private System.Windows.Forms.Button btnSubmit;
        private Label lblChampionshipType;
        private Label lblCulture;
        private ComboBox cbRepresentation;
        private Label lblRepresentation;
        private Panel pnlFavorite;
        private Panel pnlAll;
    }
}

