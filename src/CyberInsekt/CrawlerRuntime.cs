using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CyberInsekt
{
    public class CrawlerRuntime
    {
        private static readonly CrawlerRuntime _current = new CrawlerRuntime();
        private CrawlerRuntime()
        {
            TraceWriteLine = ConsoleWriter.WriteLine;
        }


        public static CrawlerRuntime Current
        {
            get { return _current; }
        }

        public Action<string, TraceLevel> TraceWriteLine { get; set; }
    }

    static class ConsoleWriter
    {
        public static void WriteLine(string message, TraceLevel level)
        {
            var foregroundColor = Console.ForegroundColor;
            try
            {
                switch (level)
                {
                     case TraceLevel.Verbose:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        
                        break;
                    case TraceLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case TraceLevel.Info:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                    case TraceLevel.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Trace.WriteLine(message);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                }
                Console.WriteLine(message);

            }
            finally
            {
                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}
