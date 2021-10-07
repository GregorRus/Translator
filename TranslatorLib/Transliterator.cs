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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLib
{
    public enum LiterType
    {
        Letter,
        Digit,
        Whitespace,
        Delimeter,
        Special,

        Ignorable,
        Prohibited,
        Other
    }

    public struct LiterLocation
    {
        public LiterLocation(int lineIndex, int columnIndex) : this()
        {
            Line = lineIndex;
            Column = columnIndex;
        }

        public int Line { get; init; }
        public int Column { get; init; }

        public override string ToString()
        {
            return $"({Line}:{Column})";
        }
    }

    public struct Liter : IStageElement
    {
        public Liter(char character, LiterType type, LiterLocation location) : this()
        {
            Character = character;
            Type = type;
            Location = location;
        }

        public static Liter CreateEndOfLine(LiterLocation location) => new('\n', LiterType.Delimeter, location);
        public static Liter CreateEndOfFile(LiterLocation location) => new('\0', LiterType.Delimeter, location);

        public char Character { get; init; }
        public LiterType Type { get; init; }
        public LiterLocation Location { get; init; }

        public override string ToString()
        {
            return $"['{SanitizedCharacter}', {Type}, {Location}]";
        }

        public bool IsLast()
        {
            return Character == '\0' && Type == LiterType.Delimeter;
        }

        public string SanitizedCharacter => Character switch
        {
            '\n' => "end_of_line",
            '\0' => "end_of_file",
            _ => Character.ToString()
        };
    }

    public class Transliterator : IStage<Liter>
    {
        private readonly TextReader TextReader;
        private int LineIndex;
        private int ColumnIndex;

        private Liter CurrentLiter;

        public Liter CurrentElement => CurrentLiter;

        public Transliterator(TextReader textReader)
        {
            TextReader = textReader;
            LineIndex = 1;
            ColumnIndex = 1;
        }

        public Liter TakeElement()
        {
            LiterLocation location = new(LineIndex, ColumnIndex);
            int character = TextReader.Read();

            ++ColumnIndex;
            if (character == '\n')
            {
                ++LineIndex;
                ColumnIndex = 1;

                CurrentLiter = Liter.CreateEndOfLine(location);
            }
            else if (character == -1)
            {
                CurrentLiter = Liter.CreateEndOfFile(location);
            }
            else
            {
                CurrentLiter = AnalyzeCharacter((char)character, location);
            }

            return CurrentLiter;
        }

        private static LiterType SwitchLiterType(char character)
        {
            return character switch
            {
                char c when char.IsLetter(c) => LiterType.Letter,
                char c when char.IsDigit(c) => LiterType.Digit,
                char c when char.IsWhiteSpace(c) => LiterType.Whitespace,
                char c when "*!?&|^$;:.,()[]{}-+/=∧∨¬".Contains(c) => LiterType.Special,
                char c when "\n\0".Contains(c) => LiterType.Delimeter,

                _ => LiterType.Other,
            };

        }

        private static Liter AnalyzeCharacter(char character, LiterLocation location)
        {
            return new Liter(character, SwitchLiterType(character), location);
        }
    }
}
