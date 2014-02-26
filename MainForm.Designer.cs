namespace MultiBound
{
    partial class MainForm
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
            this.rtConsole = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtConsole
            // 
            this.rtConsole.BackColor = System.Drawing.SystemColors.WindowText;
            this.rtConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.rtConsole.Location = new System.Drawing.Point(0, 0);
            this.rtConsole.Name = "rtConsole";
            this.rtConsole.ReadOnly = true;
            this.rtConsole.Size = new System.Drawing.Size(434, 214);
            this.rtConsole.TabIndex = 0;
            this.rtConsole.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 214);
            this.Controls.Add(this.rtConsole);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "MultiBound";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtConsole;
    }
}

