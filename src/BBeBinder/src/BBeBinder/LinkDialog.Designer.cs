namespace BBeBinder
{
    partial class LinkDialog
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.m_OkBtn = new System.Windows.Forms.Button();
            this.m_CancelBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.linkEdit = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "http://",
            "https://"});
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(75, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // m_OkBtn
            // 
            this.m_OkBtn.Location = new System.Drawing.Point(174, 97);
            this.m_OkBtn.Name = "m_OkBtn";
            this.m_OkBtn.Size = new System.Drawing.Size(75, 23);
            this.m_OkBtn.TabIndex = 2;
            this.m_OkBtn.Text = "OK";
            this.m_OkBtn.UseVisualStyleBackColor = true;
            // 
            // m_CancelBtn
            // 
            this.m_CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_CancelBtn.Location = new System.Drawing.Point(255, 97);
            this.m_CancelBtn.Name = "m_CancelBtn";
            this.m_CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.m_CancelBtn.TabIndex = 3;
            this.m_CancelBtn.Text = "Cancel";
            this.m_CancelBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // linkEdit
            // 
            this.linkEdit.FormattingEnabled = true;
            this.linkEdit.Location = new System.Drawing.Point(94, 11);
            this.linkEdit.Name = "linkEdit";
            this.linkEdit.Size = new System.Drawing.Size(237, 21);
            this.linkEdit.TabIndex = 5;
            // 
            // LinkDialog
            // 
            this.AcceptButton = this.m_OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_CancelBtn;
            this.ClientSize = new System.Drawing.Size(343, 133);
            this.Controls.Add(this.linkEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_CancelBtn);
            this.Controls.Add(this.m_OkBtn);
            this.Controls.Add(this.comboBox1);
            this.Name = "LinkDialog";
            this.Text = "LinkDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button m_OkBtn;
        private System.Windows.Forms.Button m_CancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox linkEdit;
    }
}