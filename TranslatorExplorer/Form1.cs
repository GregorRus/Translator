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

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            switch (ProcessComboBox.SelectedItem)
            {
                case "Transpiler":
                    Liters = Transliterator.Process(SourceRichTextBox.Lines);
                    StringBuilder stringBuilder = new();
                    foreach (var liter in Liters)
                    {
                        stringBuilder.AppendLine(liter.ToString());
                    }
                    ResultRichTextBox.Text = stringBuilder.ToString();
                    break;
            }
        }

        private void SourceRichTextBox_MouseHover(object sender, EventArgs e)
        {
            switch (ProcessComboBox.SelectedItem)
            {
                case "Transpiler":
                    Point point = SourceRichTextBox.PointToClient(MousePosition);
                    int index = SourceRichTextBox.GetCharIndexFromPosition(point);
                    if (index < Liters.Count)
                    {
                        SourceToolTip.SetToolTip(sender as Control, Liters?[index].ToString());
                    }
                    break;
            }
        }
    }
}
