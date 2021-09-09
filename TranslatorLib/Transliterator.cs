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

    public struct Liter
    {
        public Liter(char character, LiterType type, LiterLocation location) : this()
        {
            Character = character;
            Type = type;
            Location = location;
        }

        public static Liter CreateEndOfLine(LiterLocation location) => new('n', LiterType.Delimeter, location);
        public static Liter CreateEndOfFile(LiterLocation location) => new('0', LiterType.Delimeter, location);

        public char Character { get; init; }
        public LiterType Type { get; init; }
        public LiterLocation Location { get; init; }

        public override string ToString()
        {
            return $"['{Character}', {Type}, {Location}]";
        }
    }

    public static class Transliterator
    {
        public static List<Liter> Process(StreamReader streamReader)
        {
            List<Liter> Liters = new();

            string line = streamReader.ReadLine();
            int lineIndex = 1;
            while (line != null)
            {
                for (int columnIndex = 1; columnIndex <= line.Length; ++columnIndex)
                {
                    char character = line[columnIndex - 1];
                    LiterLocation location = new(lineIndex, columnIndex);
                    Liters.Add(AnalyzeCharacter(character, location));
                }
                Liters.Add(Liter.CreateEndOfLine(new LiterLocation(lineIndex, line.Length + 1)));

                line = streamReader.ReadLine();
                ++lineIndex;
            }
            Liters.Add(Liter.CreateEndOfFile(new LiterLocation(lineIndex + 1, 0)));

            return Liters;
        }

        public static async Task<List<Liter>> ProcessAsync(StreamReader streamReader)
        {
            List<Liter> Liters = new();

            string line = await streamReader.ReadLineAsync();
            int lineIndex = 1;
            while (line != null)
            {
                for (int columnIndex = 1; columnIndex <= line.Length; ++columnIndex)
                {
                    char character = line[columnIndex - 1];
                    LiterLocation location = new(lineIndex, columnIndex);
                    Liters.Add(AnalyzeCharacter(character, location));
                }
                Liters.Add(Liter.CreateEndOfLine(new LiterLocation(lineIndex, line.Length + 1)));

                line = streamReader.ReadLine();
                ++lineIndex;
            }
            Liters.Add(Liter.CreateEndOfFile(new LiterLocation(lineIndex + 1, 0)));

            return Liters;
        }

        public static List<Liter> Process(string[] lines)
        {
            List<Liter> Liters = new();

            for (int lineIndex = 1; lineIndex <= lines.Length; ++lineIndex)
            {
                string line = lines[lineIndex - 1];

                for (int columnIndex = 1; columnIndex <= line.Length; ++columnIndex)
                {
                    char character = line[columnIndex - 1];
                    LiterLocation location = new(lineIndex, columnIndex);
                    Liters.Add(AnalyzeCharacter(character, location));
                }
                Liters.Add(Liter.CreateEndOfLine(new LiterLocation(lineIndex, line.Length + 1)));
            }
            Liters.Add(Liter.CreateEndOfFile(new LiterLocation(lines.Length + 1, 0)));

            return Liters;
        }

        private static Liter AnalyzeCharacter(char character, LiterLocation location)
        {
            LiterType type = character switch
            {
                char c when char.IsLetter(c) => LiterType.Letter,
                char c when char.IsDigit(c) => LiterType.Digit,
                char c when char.IsWhiteSpace(c) => LiterType.Whitespace,
                char c when "*!?&|^$;:.,()[]{}-+/=".Contains(c) => LiterType.Special,

                _ => LiterType.Other,
            };
            return new Liter(character, type, location);
        }
    }
}
