using System;
using System.IO;

using TranslatorLib;

namespace Translator
{
    class Program
    {
        static async void Main(string[] args)
        {
            string filename = args[1];
            using StreamReader streamReader = new(filename);

            var liters = await Transliterator.ProcessAsync(streamReader);

            foreach (var liter in liters)
            {
                Console.WriteLine(liter);
            }
        }
    }
}
