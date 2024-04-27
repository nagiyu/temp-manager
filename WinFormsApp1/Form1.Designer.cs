using OHMService;

namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            checkedListBox1 = new CheckedListBox();
            button1 = new Button();
            textBox1 = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(HardwareInfo.GetHardwareNameList().ToArray());
            checkedListBox1.Location = new Point(12, 12);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(423, 184);
            checkedListBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(12, 202);
            button1.Name = "button1";
            button1.Size = new Size(137, 23);
            button1.TabIndex = 1;
            button1.Text = "Get Tempreture";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Click_GetTempreture;
            // 
            // button2
            // 
            button2.Location = new Point(155, 202);
            button2.Name = "button2";
            button2.Size = new Size(137, 23);
            button2.TabIndex = 4;
            button2.Text = "Stop";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Click_Stop;
            // 
            // button3
            // 
            button3.Location = new Point(298, 202);
            button3.Name = "button3";
            button3.Size = new Size(137, 23);
            button3.TabIndex = 5;
            button3.Text = "Clear";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Click_Clear;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 231);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(776, 207);
            textBox1.TabIndex = 3;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick_1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(checkedListBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox checkedListBox1;
        private Button button1;
        private TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private Button button2;
        private Button button3;
    }
}
