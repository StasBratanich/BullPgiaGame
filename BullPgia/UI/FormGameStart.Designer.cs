using System.Drawing;
using System.Windows.Forms;

namespace GameBoard
{
    partial class FormGameStart
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
            this.NumberOfChancesBtn = new System.Windows.Forms.Button();
            this.StartBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.NumberOfChancesBtn.Location = new System.Drawing.Point(12, 42);
            this.NumberOfChancesBtn.Name = "button1";
            this.NumberOfChancesBtn.Size = new System.Drawing.Size(382, 23);
            this.NumberOfChancesBtn.TabIndex = 0;
            this.NumberOfChancesBtn.Text = "Number of chances: 4";
            this.NumberOfChancesBtn.UseVisualStyleBackColor = true;
            this.NumberOfChancesBtn.Click += new System.EventHandler(this.buttonSetGuesses_Click);
            // 
            // button2
            // 
            this.StartBtn.AutoSize = true;
            this.StartBtn.Location = new System.Drawing.Point(319, 118);
            this.StartBtn.Name = "button2";
            this.StartBtn.Size = new System.Drawing.Size(75, 23);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // StartGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 153);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.NumberOfChancesBtn);
            this.Name = "StartGame";
            this.Text = "Bool Pgia";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button NumberOfChancesBtn;
        private Button StartBtn;
        private byte m_NumberOfTries = 4;
    }
}

