﻿/*
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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TranslatorLib;

namespace TranslatorExplorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Elements = new();
        }

        private readonly List<IStageElement> Elements;
        private ITreeElement RootTreeElement;

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            string ProcessFullText<E>(IStage<E> stage) where E : IStageElement
            {
                Elements.Clear();
                StringBuilder stringBuilder = new();
                for (E element = stage.TakeElement(); ;
                    element = stage.TakeElement())
                {
                    Elements.Add(element);
                    stringBuilder.AppendLine(element.ToString());
                    if (element.IsLast())
                    {
                        break;
                    }
                }
                return stringBuilder.ToString();
            }

            string ProcessFullTree<E>(ITreeStage<E> stage) where E : ITreeElement<E>
            {
                void ProcessNode(StringBuilder stringBuilder, E node, int level = 0)
                {
                    for (int i = 0; i < level; ++i)
                    {
                        stringBuilder.Append("    ");
                    }
                    stringBuilder.AppendLine(node.ToString());
                    foreach (E child in node.Childs)
                    {
                        ProcessNode(stringBuilder, child, level + 1);
                    }
                }

                StringBuilder stringBuilder = new();
                E rootNode = stage.TakeTreeRootElement();
                RootTreeElement = rootNode;
                ProcessNode(stringBuilder, rootNode);
                return stringBuilder.ToString();
            }

            try
            {
                using StringReader reader = new(SourceRichTextBox.Text);

                HashTableList hashTables = new();

                ResultRichTextBox.Text = ProcessComboBox.SelectedItem switch
                {
                    "Transliterator" => ProcessFullText(new Transliterator(reader)),
                    "Lexer" => ProcessFullText(new Lexer(new(reader), hashTables)),
                    "Syntax Analyzer" => ProcessFullTree(new SyntaxAnalyzer(new(new(reader), hashTables))),
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
            Point point = SourceRichTextBox.PointToClient(MousePosition);
            int index = SourceRichTextBox.GetCharIndexFromPosition(point);

            int line = SourceRichTextBox.GetLineFromCharIndex(index);
            int column = index - SourceRichTextBox.GetFirstCharIndexFromLine(line);
            // Assuming line and column counting begins with 1
            ++line; ++column;

            switch (ProcessComboBox.SelectedItem)
            {
                case "Transliterator":
                    if (index < Elements.Count)
                    {
                        SourceToolTip.SetToolTip(sender as Control, Elements[index].ToString());
                    }
                    return;
                case "Lexer":
                    foreach (Token el in Elements)
                    {
                        if (el.Location.Begin.Line <= line
                            && el.Location.End.Line >= line
                            && el.Location.Begin.Column <= column
                            && el.Location.End.Column >= column)
                        {
                            SourceToolTip.SetToolTip(sender as Control, el.ToString());
                            return;
                        }
                    }
                    return;
                case "Syntax Analyzer":
                    return;
            }
        }
    }
}
