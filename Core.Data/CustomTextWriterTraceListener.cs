using System;
using System.Diagnostics;

namespace com.fabioscagliola.Core.Data
{
    public class CustomTextWriterTraceListener : TextWriterTraceListener
    {
        public CustomTextWriterTraceListener(string fileName) : base(fileName) { }

        public override void WriteLine(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                base.WriteLine(message);
            }
            else
            {
                base.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("s"), message));
            }
        }

    }
}

