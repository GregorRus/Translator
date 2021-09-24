using System;
using System.IO;

using TranslatorLib;

namespace Translator
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = args[1];
            using StreamReader streamReader = new(filename);

            Transliterator transliterator = new(streamReader);
            Lexer lexer = new(transliterator);

            for (var liter = lexer.TakeElement();
                liter.Type != TokenType.EndOfFile;
                liter = lexer.TakeElement())
            {
                Console.WriteLine(liter);
            }
        }
    }
}
