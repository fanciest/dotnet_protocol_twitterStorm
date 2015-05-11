using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSMECS
{
    /// <summary>
    /// 定义的标准输出输入类
    /// </summary>
    public static class StandardIO
    {
        //协议中定义的结束符
         const string END_FLAG="end";

        /// <summary>
        /// 标准读入函数。
        /// 根据协议，读取数据通过标准接口（stdin 与 stdout）的读取。
        /// 其中，当一行中信息为end时，代表读取结束
        /// </summary>
        /// <returns>
        /// 返回读取的信息，制作成string类型的字符串传出
        /// </returns>
        public static string readMsg() 
        {
            string msg=null;
            while (true) 
            {
                string context = Console.ReadLine();
                LocalLog.writeLog(context);
                if (context.Equals(END_FLAG))
                    break;
                msg = msg + context + "\n";
            }
            return msg; 
        }

        /// <summary>
        /// 输出标准信息
        /// </summary>
        /// <param name="msg"> 需要输出的字符串</param>
        /// <returns>根据协议，在输出的字符串后加上结束符"end"</returns>
        public static void sendMsg(string msg) 
        {
            Console.WriteLine(msg);
            Console.WriteLine(END_FLAG);
            Console.Out.Flush();
        }
    }
}
