using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WienerParserAttempt
{
    internal class Program
    {
        static async Task Main()
        {
            ParserWorker parser_wiene = new ParserWorker(new WieneParser());
            await parser_wiene.Start();
        }
    }
}
