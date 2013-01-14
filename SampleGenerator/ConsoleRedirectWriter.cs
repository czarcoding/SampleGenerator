using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SampleGen
{
    class ConsoleRedirectWriter : RedirectWriter
    {
        TextWriter consoleTextWriter;

        public ConsoleRedirectWriter()
        {
            consoleTextWriter = Console.Out;
            this.OnWrite += delegate(string text)
            {
                consoleTextWriter.Write(text);
            };
            Console.SetOut(this);
        }

        public void release()
        {
            Console.SetOut(consoleTextWriter);
        }
    }
}
