using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TranslatorLib;

namespace TranslatorExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Liter> Liters;
        List<Token> Tokens;

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            try
            {
                Liters = Transliterator.Process(SourceRichTextBox.Lines);
                if (ProcessComboBox.SelectedItem.Equals("Transpiler"))
                {
                    StringBuilder stringBuilder = new();
                    foreach (var liter in Liters)
                    {
                        stringBuilder.AppendLine(liter.ToString());
                    }
                    ResultRichTextBox.Text = stringBuilder.ToString();
                    goto success;
                }

                Tokens = Lexer.Process(Liters);
                if (ProcessComboBox.SelectedItem.Equals("Lexer"))
                {
                    StringBuilder stringBuilder = new();
                    foreach (var token in Tokens)
                    {
                        stringBuilder.AppendLine(token.ToString());
                    }
                    ResultRichTextBox.Text = stringBuilder.ToString();
                    goto success;
                }

                success:
                StageToolStripStatusLabel.Text = "Работа выполнена без ошибок";
                StageStatusStrip.BackColor = Color.FromArgb(0xFF, 0xB1, 0xFF, 0x54);
            }
            catch (Exception exc)
            {
                StageToolStripStatusLabel.Text = "Есть ошибка";
                StageStatusStrip.BackColor = Color.FromArgb(0xFF, 0xFF, 0x5C, 0x52);
                ResultRichTextBox.Text = $"Exception:\n{exc}";
            }
        }

        private void SourceRichTextBox_MouseHover(object sender, EventArgs e)
        {
            switch (ProcessComboBox.SelectedItem)
            {
                case "Transpiler" when Liters != null:
                    Point point = SourceRichTextBox.PointToClient(MousePosition);
                    int index = SourceRichTextBox.GetCharIndexFromPosition(point);
                    if (index < Liters.Count)
                    {
                        SourceToolTip.SetToolTip(sender as Control, Liters[index].ToString());
                    }
                    break;
            }
        }
    }
}
