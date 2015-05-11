using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TSMECS
{
    class Program
    {
        static void Main(string[] args)
        {
            //LocalLog.writeLog("hello world");
            SpiltSentenceBolt bolt = new SpiltSentenceBolt();
            bolt.run();
        }
    }
}
