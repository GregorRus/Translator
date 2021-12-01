/*
 *  MIT License
 *
 *  Copyright (c) 2021 Chibirev Grigoriy
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */


namespace TranslatorExplorer
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
            this.components = new System.ComponentModel.Container();
            this.SourceRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ResultRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ResultLabel = new System.Windows.Forms.Label();
            this.SourceLabel = new System.Windows.Forms.Label();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ProcessComboBox = new System.Windows.Forms.ComboBox();
            this.SourceToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.StageStatusStrip = new System.Windows.Forms.StatusStrip();
            this.StageToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.Hash1Label = new System.Windows.Forms.Label();
            this.Hash2Label = new System.Windows.Forms.Label();
            this.Hash3Label = new System.Windows.Forms.Label();
            this.Hash1RichTextBox = new System.Windows.Forms.RichTextBox();
            this.Hash2RichTextBox = new System.Windows.Forms.RichTextBox();
            this.Hash3RichTextBox = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.StageStatusStrip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SourceRichTextBox
            // 
            this.SourceRichTextBox.AcceptsTab = true;
            this.SourceRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SourceRichTextBox.EnableAutoDragDrop = true;
            this.SourceRichTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SourceRichTextBox.Location = new System.Drawing.Point(3, 39);
            this.SourceRichTextBox.Name = "SourceRichTextBox";
            this.SourceRichTextBox.Size = new System.Drawing.Size(391, 206);
            this.SourceRichTextBox.TabIndex = 0;
            this.SourceRichTextBox.Text = "";
            this.SourceRichTextBox.MouseHover += new System.EventHandler(this.SourceRichTextBox_MouseHover);
            // 
            // ResultRichTextBox
            // 
            this.ResultRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultRichTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ResultRichTextBox.Location = new System.Drawing.Point(400, 39);
            this.ResultRichTextBox.Name = "ResultRichTextBox";
            this.ResultRichTextBox.ReadOnly = true;
            this.ResultRichTextBox.Size = new System.Drawing.Size(391, 206);
            this.ResultRichTextBox.TabIndex = 1;
            this.ResultRichTextBox.Text = "";
            // 
            // ResultLabel
            // 
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Location = new System.Drawing.Point(400, 0);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Padding = new System.Windows.Forms.Padding(8);
            this.ResultLabel.Size = new System.Drawing.Size(65, 36);
            this.ResultLabel.TabIndex = 2;
            this.ResultLabel.Text = "Result";
            // 
            // SourceLabel
            // 
            this.SourceLabel.AutoSize = true;
            this.SourceLabel.Location = new System.Drawing.Point(3, 0);
            this.SourceLabel.Name = "SourceLabel";
            this.SourceLabel.Padding = new System.Windows.Forms.Padding(8);
            this.SourceLabel.Size = new System.Drawing.Size(70, 36);
            this.SourceLabel.TabIndex = 3;
            this.SourceLabel.Text = "Source";
            // 
            // ProcessButton
            // 
            this.ProcessButton.AutoSize = true;
            this.ProcessButton.Location = new System.Drawing.Point(160, 3);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(100, 34);
            this.ProcessButton.TabIndex = 4;
            this.ProcessButton.Text = "&Process";
            this.ProcessButton.UseVisualStyleBackColor = true;
            this.ProcessButton.Click += new System.EventHandler(this.ProcessButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ResultLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SourceLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SourceRichTextBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ResultRichTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(794, 292);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ProcessComboBox);
            this.flowLayoutPanel1.Controls.Add(this.ProcessButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(400, 251);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(391, 38);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // ProcessComboBox
            // 
            this.ProcessComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProcessComboBox.FormattingEnabled = true;
            this.ProcessComboBox.Items.AddRange(new object[] {
            "Transliterator",
            "Lexer",
            "Syntax Analyzer"});
            this.ProcessComboBox.Location = new System.Drawing.Point(3, 6);
            this.ProcessComboBox.Name = "ProcessComboBox";
            this.ProcessComboBox.Size = new System.Drawing.Size(151, 28);
            this.ProcessComboBox.TabIndex = 5;
            // 
            // SourceToolTip
            // 
            this.SourceToolTip.AutomaticDelay = 100;
            // 
            // StageStatusStrip
            // 
            this.StageStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StageStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StageToolStripStatusLabel});
            this.StageStatusStrip.Location = new System.Drawing.Point(0, 428);
            this.StageStatusStrip.Name = "StageStatusStrip";
            this.StageStatusStrip.Size = new System.Drawing.Size(800, 22);
            this.StageStatusStrip.TabIndex = 0;
            this.StageStatusStrip.Text = "statusStrip1";
            // 
            // StageToolStripStatusLabel
            // 
            this.StageToolStripStatusLabel.Name = "StageToolStripStatusLabel";
            this.StageToolStripStatusLabel.Size = new System.Drawing.Size(0, 16);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.StageStatusStrip, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.Controls.Add(this.Hash1Label, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.Hash2Label, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.Hash3Label, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.Hash1RichTextBox, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.Hash2RichTextBox, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.Hash3RichTextBox, 2, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 301);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(794, 121);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // Hash1Label
            // 
            this.Hash1Label.AutoSize = true;
            this.Hash1Label.Location = new System.Drawing.Point(3, 0);
            this.Hash1Label.Name = "Hash1Label";
            this.Hash1Label.Size = new System.Drawing.Size(107, 20);
            this.Hash1Label.TabIndex = 0;
            this.Hash1Label.Text = "Первое слово";
            // 
            // Hash2Label
            // 
            this.Hash2Label.AutoSize = true;
            this.Hash2Label.Location = new System.Drawing.Point(267, 0);
            this.Hash2Label.Name = "Hash2Label";
            this.Hash2Label.Size = new System.Drawing.Size(104, 20);
            this.Hash2Label.TabIndex = 1;
            this.Hash2Label.Text = "Второе слово";
            // 
            // Hash3Label
            // 
            this.Hash3Label.AutoSize = true;
            this.Hash3Label.Location = new System.Drawing.Point(531, 0);
            this.Hash3Label.Name = "Hash3Label";
            this.Hash3Label.Size = new System.Drawing.Size(141, 20);
            this.Hash3Label.TabIndex = 2;
            this.Hash3Label.Text = "Знаки препинания";
            // 
            // Hash1RichTextBox
            // 
            this.Hash1RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Hash1RichTextBox.Location = new System.Drawing.Point(3, 39);
            this.Hash1RichTextBox.Name = "Hash1RichTextBox";
            this.Hash1RichTextBox.Size = new System.Drawing.Size(258, 79);
            this.Hash1RichTextBox.TabIndex = 3;
            this.Hash1RichTextBox.Text = "";
            // 
            // Hash2RichTextBox
            // 
            this.Hash2RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Hash2RichTextBox.Location = new System.Drawing.Point(267, 39);
            this.Hash2RichTextBox.Name = "Hash2RichTextBox";
            this.Hash2RichTextBox.Size = new System.Drawing.Size(258, 79);
            this.Hash2RichTextBox.TabIndex = 4;
            this.Hash2RichTextBox.Text = "";
            // 
            // Hash3RichTextBox
            // 
            this.Hash3RichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Hash3RichTextBox.Location = new System.Drawing.Point(531, 39);
            this.Hash3RichTextBox.Name = "Hash3RichTextBox";
            this.Hash3RichTextBox.Size = new System.Drawing.Size(260, 79);
            this.Hash3RichTextBox.TabIndex = 5;
            this.Hash3RichTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.StageStatusStrip.ResumeLayout(false);
            this.StageStatusStrip.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox SourceRichTextBox;
        private System.Windows.Forms.RichTextBox ResultRichTextBox;
        private System.Windows.Forms.Label ResultLabel;
        private System.Windows.Forms.Label SourceLabel;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox ProcessComboBox;
        private System.Windows.Forms.ToolTip SourceToolTip;
        private System.Windows.Forms.StatusStrip StageStatusStrip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStripStatusLabel StageToolStripStatusLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label Hash1Label;
        private System.Windows.Forms.Label Hash2Label;
        private System.Windows.Forms.Label Hash3Label;
        private System.Windows.Forms.RichTextBox Hash1RichTextBox;
        private System.Windows.Forms.RichTextBox Hash2RichTextBox;
        private System.Windows.Forms.RichTextBox Hash3RichTextBox;
    }
}

