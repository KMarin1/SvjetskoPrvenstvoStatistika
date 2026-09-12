namespace SvjetskoPrvenstvoPobjedeForm
{
    partial class IgracControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblIme = new System.Windows.Forms.Label();
            this.lblPozicija = new System.Windows.Forms.Label();
            this.lblKapetan = new System.Windows.Forms.Label();
            this.pbZvjezdica = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbZvjezdica)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIme
            // 
            this.lblIme.AutoSize = true;
            this.lblIme.Location = new System.Drawing.Point(14, 175);
            this.lblIme.Name = "lblIme";
            this.lblIme.Size = new System.Drawing.Size(35, 13);
            this.lblIme.TabIndex = 0;
            this.lblIme.Text = "label1";
            // 
            // lblPozicija
            // 
            this.lblPozicija.AutoSize = true;
            this.lblPozicija.Location = new System.Drawing.Point(175, 175);
            this.lblPozicija.Name = "lblPozicija";
            this.lblPozicija.Size = new System.Drawing.Size(35, 13);
            this.lblPozicija.TabIndex = 1;
            this.lblPozicija.Text = "label1";
            // 
            // lblKapetan
            // 
            this.lblKapetan.AutoSize = true;
            this.lblKapetan.Location = new System.Drawing.Point(17, 247);
            this.lblKapetan.Name = "lblKapetan";
            this.lblKapetan.Size = new System.Drawing.Size(35, 13);
            this.lblKapetan.TabIndex = 2;
            this.lblKapetan.Text = "label1";
            // 
            // pbZvjezdica
            // 
            this.pbZvjezdica.Location = new System.Drawing.Point(55, 175);
            this.pbZvjezdica.Name = "pbZvjezdica";
            this.pbZvjezdica.Size = new System.Drawing.Size(21, 20);
            this.pbZvjezdica.TabIndex = 3;
            this.pbZvjezdica.TabStop = false;
            // 
            // IgracControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbZvjezdica);
            this.Controls.Add(this.lblKapetan);
            this.Controls.Add(this.lblPozicija);
            this.Controls.Add(this.lblIme);
            this.Name = "IgracControl";
            this.Size = new System.Drawing.Size(225, 276);
            ((System.ComponentModel.ISupportInitialize)(this.pbZvjezdica)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblIme;
        private System.Windows.Forms.Label lblPozicija;
        private System.Windows.Forms.Label lblKapetan;
        private System.Windows.Forms.PictureBox pbZvjezdica;
    }
}
