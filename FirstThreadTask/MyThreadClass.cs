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
        public delegate void MyEventHandler(long Max, int Progress, char ch);
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
            var text=File.ReadAllText(Path1);
            var Counter = text.Length;
            using (var Fs = new FileStream(Path1, FileMode.Open))
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(Path2, FileMode.Open), Encoding.Unicode))
                {
                    char ch;
                    int ProgressBarValue = 0;
                    for (int i = 0; i < Counter; i++)
                    {
                        Thread.Sleep(50);
                        ch = text[i];
                        sw.Write(ch);
                        ProgressBarValue++;
                        if (MyEvent!=null)
                            MyEvent.Invoke(Counter, ProgressBarValue, ch);
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
