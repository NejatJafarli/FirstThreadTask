using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FirstThreadTask
{
    public class MyThreadClass : IDisposable
    {
        public delegate void MyEventHandler(long Max, int Progress,char ch);
        public event MyEventHandler MyEvent;
        public string Path1 { get; set; }
        public string Path2 { get; set; }
        public MyThreadClass(string filepath1, string filepath2)
        {
            Path1 = filepath1;
            Path2 = filepath2;
        }
        public void Do()
        {
            using (var Fs = new FileStream(Path1, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(Fs, Encoding.Unicode))
                {
                    using (StreamWriter sw = new StreamWriter(new FileStream(Path2, FileMode.Open), Encoding.Unicode))
                    {
                        char ch;
                        int val;
                        var Counter = sr.BaseStream.Length;
                        int ProgressBarValue = 0;
                        while (true)
                        {
                            val = Fs.ReadByte();

                            if (val < 0)
                                break;
                            ch = (char)val;
                            ProgressBarValue += 1;
                            if (MyEvent != null)
                                MyEvent(Counter, ProgressBarValue,ch);

                            sw.Write(ch);
                            Thread.Sleep(30);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~MyThreadClass()
        {
            Dispose();
        }
    }
}
