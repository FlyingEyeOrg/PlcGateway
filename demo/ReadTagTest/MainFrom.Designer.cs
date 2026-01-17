namespace ReadTagTest
{
    partial class MainFrom
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
            this.HostIPTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PLCIPTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.EipStartButton = new System.Windows.Forms.Button();
            this.ConnectPLCButton = new System.Windows.Forms.Button();
            this.DisconnectPLCButton = new System.Windows.Forms.Button();
            this.EipStopButton = new System.Windows.Forms.Button();
            this.ConnectIdTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LogRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ReadTagButton = new System.Windows.Forms.Button();
            this.ReadTagAddressTextBox = new System.Windows.Forms.TextBox();
            this.WriteTagAddressTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.WriteTagButton = new System.Windows.Forms.Button();
            this.WriteDataTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.UseReadTagExtCheckBox = new System.Windows.Forms.CheckBox();
            this.CleanTagButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // HostIPTextBox
            // 
            this.HostIPTextBox.Location = new System.Drawing.Point(59, 6);
            this.HostIPTextBox.Name = "HostIPTextBox";
            this.HostIPTextBox.Size = new System.Drawing.Size(142, 21);
            this.HostIPTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "主机IP";
            // 
            // PLCIPTextBox
            // 
            this.PLCIPTextBox.Location = new System.Drawing.Point(59, 33);
            this.PLCIPTextBox.Name = "PLCIPTextBox";
            this.PLCIPTextBox.Size = new System.Drawing.Size(142, 21);
            this.PLCIPTextBox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 270);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "读取地址";
            // 
            // EipStartButton
            // 
            this.EipStartButton.Location = new System.Drawing.Point(12, 60);
            this.EipStartButton.Name = "EipStartButton";
            this.EipStartButton.Size = new System.Drawing.Size(75, 23);
            this.EipStartButton.TabIndex = 2;
            this.EipStartButton.Text = "开启协议栈";
            this.EipStartButton.UseVisualStyleBackColor = true;
            this.EipStartButton.Click += new System.EventHandler(this.EipStartButton_Click);
            // 
            // ConnectPLCButton
            // 
            this.ConnectPLCButton.Location = new System.Drawing.Point(12, 89);
            this.ConnectPLCButton.Name = "ConnectPLCButton";
            this.ConnectPLCButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectPLCButton.TabIndex = 2;
            this.ConnectPLCButton.Text = "连接PLC";
            this.ConnectPLCButton.UseVisualStyleBackColor = true;
            this.ConnectPLCButton.Click += new System.EventHandler(this.ConnectPLCButton_Click);
            // 
            // DisconnectPLCButton
            // 
            this.DisconnectPLCButton.Location = new System.Drawing.Point(12, 118);
            this.DisconnectPLCButton.Name = "DisconnectPLCButton";
            this.DisconnectPLCButton.Size = new System.Drawing.Size(75, 23);
            this.DisconnectPLCButton.TabIndex = 2;
            this.DisconnectPLCButton.Text = "PLC断开";
            this.DisconnectPLCButton.UseVisualStyleBackColor = true;
            this.DisconnectPLCButton.Click += new System.EventHandler(this.DisconnectPLCButton_Click);
            // 
            // EipStopButton
            // 
            this.EipStopButton.Location = new System.Drawing.Point(12, 147);
            this.EipStopButton.Name = "EipStopButton";
            this.EipStopButton.Size = new System.Drawing.Size(75, 23);
            this.EipStopButton.TabIndex = 2;
            this.EipStopButton.Text = "关闭协议栈";
            this.EipStopButton.UseVisualStyleBackColor = true;
            this.EipStopButton.Click += new System.EventHandler(this.EipStopButton_Click);
            // 
            // ConnectIdTextBox
            // 
            this.ConnectIdTextBox.Location = new System.Drawing.Point(140, 91);
            this.ConnectIdTextBox.Name = "ConnectIdTextBox";
            this.ConnectIdTextBox.Size = new System.Drawing.Size(58, 21);
            this.ConnectIdTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "连接ID";
            // 
            // LogRichTextBox
            // 
            this.LogRichTextBox.Location = new System.Drawing.Point(207, 6);
            this.LogRichTextBox.Name = "LogRichTextBox";
            this.LogRichTextBox.Size = new System.Drawing.Size(581, 432);
            this.LogRichTextBox.TabIndex = 5;
            this.LogRichTextBox.Text = "";
            // 
            // ReadTagButton
            // 
            this.ReadTagButton.Location = new System.Drawing.Point(126, 294);
            this.ReadTagButton.Name = "ReadTagButton";
            this.ReadTagButton.Size = new System.Drawing.Size(75, 23);
            this.ReadTagButton.TabIndex = 2;
            this.ReadTagButton.Text = "读取数据";
            this.ReadTagButton.UseVisualStyleBackColor = true;
            this.ReadTagButton.Click += new System.EventHandler(this.ReadTagButton_Click);
            // 
            // ReadTagAddressTextBox
            // 
            this.ReadTagAddressTextBox.Location = new System.Drawing.Point(69, 267);
            this.ReadTagAddressTextBox.Name = "ReadTagAddressTextBox";
            this.ReadTagAddressTextBox.Size = new System.Drawing.Size(132, 21);
            this.ReadTagAddressTextBox.TabIndex = 0;
            // 
            // WriteTagAddressTextBox
            // 
            this.WriteTagAddressTextBox.Location = new System.Drawing.Point(69, 333);
            this.WriteTagAddressTextBox.Name = "WriteTagAddressTextBox";
            this.WriteTagAddressTextBox.Size = new System.Drawing.Size(132, 21);
            this.WriteTagAddressTextBox.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 336);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "写入地址";
            // 
            // WriteTagButton
            // 
            this.WriteTagButton.Location = new System.Drawing.Point(126, 387);
            this.WriteTagButton.Name = "WriteTagButton";
            this.WriteTagButton.Size = new System.Drawing.Size(75, 23);
            this.WriteTagButton.TabIndex = 2;
            this.WriteTagButton.Text = "写入数据";
            this.WriteTagButton.UseVisualStyleBackColor = true;
            this.WriteTagButton.Click += new System.EventHandler(this.WriteTagButton_Click);
            // 
            // WriteDataTextBox
            // 
            this.WriteDataTextBox.Location = new System.Drawing.Point(69, 360);
            this.WriteDataTextBox.Name = "WriteDataTextBox";
            this.WriteDataTextBox.Size = new System.Drawing.Size(132, 21);
            this.WriteDataTextBox.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 363);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "写入数据";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "PLC IP";
            // 
            // UseReadTagExtCheckBox
            // 
            this.UseReadTagExtCheckBox.AutoSize = true;
            this.UseReadTagExtCheckBox.Location = new System.Drawing.Point(12, 298);
            this.UseReadTagExtCheckBox.Name = "UseReadTagExtCheckBox";
            this.UseReadTagExtCheckBox.Size = new System.Drawing.Size(102, 16);
            this.UseReadTagExtCheckBox.TabIndex = 6;
            this.UseReadTagExtCheckBox.Text = "UseReadTagExt";
            this.UseReadTagExtCheckBox.UseVisualStyleBackColor = true;
            // 
            // CleanTagButton
            // 
            this.CleanTagButton.Location = new System.Drawing.Point(14, 236);
            this.CleanTagButton.Name = "CleanTagButton";
            this.CleanTagButton.Size = new System.Drawing.Size(75, 23);
            this.CleanTagButton.TabIndex = 7;
            this.CleanTagButton.Text = "清除缓存";
            this.CleanTagButton.UseVisualStyleBackColor = true;
            this.CleanTagButton.Click += new System.EventHandler(this.CleanTagButton_Click);
            // 
            // MainFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CleanTagButton);
            this.Controls.Add(this.UseReadTagExtCheckBox);
            this.Controls.Add(this.LogRichTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ConnectIdTextBox);
            this.Controls.Add(this.DisconnectPLCButton);
            this.Controls.Add(this.ConnectPLCButton);
            this.Controls.Add(this.EipStopButton);
            this.Controls.Add(this.WriteTagButton);
            this.Controls.Add(this.ReadTagButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EipStartButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.WriteDataTextBox);
            this.Controls.Add(this.WriteTagAddressTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReadTagAddressTextBox);
            this.Controls.Add(this.PLCIPTextBox);
            this.Controls.Add(this.HostIPTextBox);
            this.Name = "MainFrom";
            this.Text = "EIP标签读取测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox HostIPTextBox;
        private Label label1;
        private TextBox PLCIPTextBox;
        private Label label2;
        private Button EipStartButton;
        private Button ConnectPLCButton;
        private Button DisconnectPLCButton;
        private Button EipStopButton;
        private TextBox ConnectIdTextBox;
        private Label label3;
        private RichTextBox LogRichTextBox;
        private Button ReadTagButton;
        private TextBox ReadTagAddressTextBox;
        private TextBox WriteTagAddressTextBox;
        private Label label4;
        private Button WriteTagButton;
        private TextBox WriteDataTextBox;
        private Label label5;
        private Label label6;
        private CheckBox UseReadTagExtCheckBox;
        private Button CleanTagButton;
    }
}