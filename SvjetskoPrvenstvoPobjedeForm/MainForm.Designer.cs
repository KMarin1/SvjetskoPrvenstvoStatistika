namespace SvjetskoPrvenstvoPobjedeForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.pbLoading = new System.Windows.Forms.ProgressBar();
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
            this.cbRepresentation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRepresentation.FormattingEnabled = true;
            this.cbRepresentation.Location = new System.Drawing.Point(204, 43);
            this.cbRepresentation.Name = "cbRepresentation";
            this.cbRepresentation.Size = new System.Drawing.Size(180, 21);
            this.cbRepresentation.TabIndex = 5;
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
            // pbLoading
            //
            this.pbLoading.Location = new System.Drawing.Point(204, 70);
            this.pbLoading.MarqueeAnimationSpeed = 30;
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(180, 12);
            this.pbLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbLoading.TabIndex = 7;
            this.pbLoading.Visible = false;
            //
            // MainForm
            //
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 150);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.lblRepresentation);
            this.Controls.Add(this.cbRepresentation);
            this.Controls.Add(this.lblCulture);
            this.Controls.Add(this.lblChampionshipType);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.ddlChampionshipType);
            this.Controls.Add(this.ddlCulture);
            this.Name = "MainForm";
            this.Text = "Svjetsko prvenstvo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ddlCulture;
        private System.Windows.Forms.ComboBox ddlChampionshipType;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblChampionshipType;
        private System.Windows.Forms.Label lblCulture;
        private System.Windows.Forms.ComboBox cbRepresentation;
        private System.Windows.Forms.Label lblRepresentation;
        private System.Windows.Forms.ProgressBar pbLoading;
    }
}
