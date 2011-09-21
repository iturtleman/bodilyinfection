namespace BoundaryCreator
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pointsFile = new System.Windows.Forms.Button();
            this.animPreFile = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.generate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 43);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(828, 411);
            this.textBox1.TabIndex = 0;
            this.textBox1.WordWrap = false;
            // 
            // pointsFile
            // 
            this.pointsFile.Location = new System.Drawing.Point(12, 12);
            this.pointsFile.Name = "pointsFile";
            this.pointsFile.Size = new System.Drawing.Size(409, 23);
            this.pointsFile.TabIndex = 1;
            this.pointsFile.Text = "Open Collision Points File (.colpoints)";
            this.pointsFile.UseVisualStyleBackColor = true;
            this.pointsFile.Click += new System.EventHandler(this.pointsFile_Click);
            // 
            // animPreFile
            // 
            this.animPreFile.Location = new System.Drawing.Point(431, 12);
            this.animPreFile.Name = "animPreFile";
            this.animPreFile.Size = new System.Drawing.Size(409, 23);
            this.animPreFile.TabIndex = 2;
            this.animPreFile.Text = "Open AnimPre File (.animpre)";
            this.animPreFile.UseVisualStyleBackColor = true;
            this.animPreFile.Click += new System.EventHandler(this.animPreFile_Click);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(431, 465);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(409, 23);
            this.save.TabIndex = 3;
            this.save.Text = "Save Output Anim File (.anim)";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // generate
            // 
            this.generate.Location = new System.Drawing.Point(13, 465);
            this.generate.Name = "generate";
            this.generate.Size = new System.Drawing.Size(408, 23);
            this.generate.TabIndex = 4;
            this.generate.Text = "Generate";
            this.generate.UseVisualStyleBackColor = true;
            this.generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 500);
            this.Controls.Add(this.generate);
            this.Controls.Add(this.save);
            this.Controls.Add(this.animPreFile);
            this.Controls.Add(this.pointsFile);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button pointsFile;
        private System.Windows.Forms.Button animPreFile;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button generate;
    }
}

