namespace VoiceAssistant
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.MessageForm = new System.Windows.Forms.Label();
            this.ClearLogButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ConfidenceBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.RecogniseButton = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.MessageForm);
            this.panel1.Location = new System.Drawing.Point(312, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 449);
            this.panel1.TabIndex = 1;
            // 
            // MessageForm
            // 
            this.MessageForm.AutoSize = true;
            this.MessageForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MessageForm.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MessageForm.Location = new System.Drawing.Point(3, 10);
            this.MessageForm.MaximumSize = new System.Drawing.Size(380, 0);
            this.MessageForm.Name = "MessageForm";
            this.MessageForm.Size = new System.Drawing.Size(36, 17);
            this.MessageForm.TabIndex = 0;
            this.MessageForm.Text = "Log:";
            // 
            // ClearLogButton
            // 
            this.ClearLogButton.Location = new System.Drawing.Point(3, 107);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(286, 51);
            this.ClearLogButton.TabIndex = 1;
            this.ClearLogButton.Text = "Cleal Log";
            this.ClearLogButton.UseVisualStyleBackColor = true;
            this.ClearLogButton.Click += new System.EventHandler(this.ClearLogButton_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 44);
            this.label1.TabIndex = 8;
            this.label1.Text = "Требуемая точность";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConfidenceBox
            // 
            this.ConfidenceBox.Location = new System.Drawing.Point(145, 56);
            this.ConfidenceBox.Multiline = true;
            this.ConfidenceBox.Name = "ConfidenceBox";
            this.ConfidenceBox.Size = new System.Drawing.Size(144, 46);
            this.ConfidenceBox.TabIndex = 9;
            this.ConfidenceBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ConfidenceBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConfidenceBox_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.RecogniseButton);
            this.panel2.Controls.Add(this.ClearLogButton);
            this.panel2.Controls.Add(this.ConfidenceBox);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(13, 298);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(293, 163);
            this.panel2.TabIndex = 10;
            // 
            // RecogniseButton
            // 
            this.RecogniseButton.Location = new System.Drawing.Point(3, 3);
            this.RecogniseButton.Name = "RecogniseButton";
            this.RecogniseButton.Size = new System.Drawing.Size(286, 51);
            this.RecogniseButton.TabIndex = 7;
            this.RecogniseButton.Text = "Start recognise";
            this.RecogniseButton.UseVisualStyleBackColor = true;
            this.RecogniseButton.Click += new System.EventHandler(this.RecogniseButton_Click);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(136, 144);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(131, 67);
            this.TestButton.TabIndex = 11;
            this.TestButton.Text = "Start Server";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 54);
            this.button1.TabIndex = 12;
            this.button1.Text = "Stop Server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(734, 473);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(750, 230);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label MessageForm;
        private System.Windows.Forms.Button ClearLogButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ConfidenceBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button RecogniseButton;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.Button button1;
    }
}

