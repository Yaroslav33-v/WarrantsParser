using System;
using System.Threading.Tasks;

namespace WarrantyParser
{
    internal class Program
    {
        static async Task Main()
        {
            ParserWorker parserWiene = new ParserWorker(new WieneParser());
            Console.WriteLine("Начата работа программы");
            await parserWiene.Start();
        }
    }
}
