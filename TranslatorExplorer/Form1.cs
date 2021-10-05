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

using System;
using System.IO;
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

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            static string ProcessFullText<E>(IStage<E> stage) where E : IStageElement
            {
                StringBuilder stringBuilder = new();
                for (E element = stage.TakeElement(); ;
                    element = stage.TakeElement())
                {
                    stringBuilder.AppendLine(element.ToString());
                    if (element.IsLast())
                    {
                        break;
                    }
                }
                return stringBuilder.ToString();
            }

            try
            {
                using StringReader reader = new(SourceRichTextBox.Text);

                Transliterator transliterator = new(reader);
                Lexer lexer = new(transliterator);

                ResultRichTextBox.Text = ProcessComboBox.SelectedItem switch
                {
                    "Transliterator" => ProcessFullText(transliterator),
                    "Lexer" => ProcessFullText(lexer),
                    _ => throw new NotImplementedException()
                };

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
            //switch (ProcessComboBox.SelectedItem)
            //{
            //    case "Transpiler":
            //        Point point = SourceRichTextBox.PointToClient(MousePosition);
            //        int index = SourceRichTextBox.GetCharIndexFromPosition(point);
            //        if (index < Liters.Count)
            //        {
            //            SourceToolTip.SetToolTip(sender as Control, Liters[index].ToString());
            //        }
            //        break;
            //}
        }
    }
}
