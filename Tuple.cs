using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace TSMECS
{
    /// <summary>
    /// 根据协议创建的tuple的类型
    /// </summary>
    public class Tuple
    {
        private string id; // tuple 的id，唯一标识
        private string comp; //创建该tuple的组件id
        private string stream;//创建该tuple的stream id
        private int task; //创建该tuple的task id
        private List<Object> tuple; // tuple中的内容

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_comp"></param>
        /// <param name="_stream"></param>
        /// <param name="_task"></param>
        /// <param name="_tuple"></param>
        public  Tuple(string _id, string _comp, string _stream, int _task, List<Object> _tuple)
        {
            this.id = _id;
            this.comp = _comp;
            this.stream = _stream;
            this.task = _task;
            this.tuple = _tuple;
        }

        public Tuple() { }
        /// <summary>
        /// 根据协议重写Tostring()函数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result ="<"+ this.GetType().FullName;
            result = result + " "+ "id="+this.id; //1. 加入id信息
            result = result + " " + "comp=" + this.comp; //2.加入comp信息
            result = result + " " + "stream=" + this.stream; //3.加入stream信息
            result = result + " " + "task=" + this.task.ToString(); // 4.加入task信息
            String str = "[";
            foreach (Object o in this.tuple) 
            {
                str = str + o.ToString() + ",";
            }
            //因为最后多加了一个','
            str.Substring(0, str.Length - 1);
            str = str + "]";
            result =result +" "+"tuple="+str+">";
            return result;
        }

        public string getTupleId() 
        {
            return this.id;
        }

        public string getValue() 
        {
            return this.tuple[0].ToString();
        }
    }
}
