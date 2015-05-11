using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using LitJson;
using System.Diagnostics;

namespace TSMECS
{
   

   public class Bolt
    {
        protected Queue<Tuple> tuple_queue = new Queue<Tuple>();

        public void initialComponent(Conf config, Context context) 
        {
            //LocalLog.writeLog("IntialComponent");
            string information = StandardIO.readMsg();
            //string information = Console.ReadLine();
            //LocalLog.writeLog(information);
            InitialHandshake handshake = InitialHandshake.Parse(information);
            config = handshake.conf;
            context = handshake.context;
            int process_id = Process.GetCurrentProcess().Id;
            PID temp = new PID();
            temp.pid = process_id;

            string json_pid = JsonMapper.ToJson(temp);
            try
            {
                //Console.WriteLine(handshake.pidDir+"  "+process_id);
                string dir_path = @handshake.pidDir;
                System.IO.Directory.CreateDirectory(dir_path);
                string file = handshake.pidDir + @"\" + process_id.ToString();
                if (!System.IO.File.Exists(file))
                {
                    System.IO.FileStream f = new System.IO.FileStream(file, System.IO.FileMode.CreateNew);
                }
            }
            catch (Exception e)
            {
                // do nothing
            }
            StandardIO.sendMsg(json_pid);
        }
        public void initialize(Conf stormconfig, Context context) 
        {
            // do nothing
            //等待重写
        }

        public void process(Tuple tuple) 
        {
            // do nothing
            //等待重写
        }

        public void run() 
        {
            LocalLog.writeLog("Bolt run");
            Conf config = new Conf();
            Context context = new Context();
            initialComponent(config, context);
            this.initialize(config, context);
            try
            {
                while (true) 
                {
                    Tuple tup = readTuple();
                    this.process(tup);
                    ack(tup.getTupleId());
                }
            }
            catch (Exception e) 
            {
                log(e.Message);
            }
        }

        public Tuple readTuple()
        {
           return  getTupleFromJson(Utility.readCommand());
        }

        public Tuple  getTupleFromJson(JsonData jd)
        {
            string id = (string)jd["id"];
            string comp = (string)jd["comp"];
            string stream = (string)jd["stream"];
            int task = (int)jd["task"];
            List<Object> tuples = new List<object>();
            JsonData jd_set = jd["tuple"];
            int set_cnt = jd_set.Count;
            foreach (JsonData jd_item in jd_set) 
            {
                tuples.Add((Object)jd_item);
            }
            Tuple t = new Tuple(id,comp,stream,task,tuples);
            return t;
        }

        public void log(string s)
        {
            Log l = new Log("log", s);
            string info = JsonMapper.ToJson(l);
            StandardIO.sendMsg(info+"\n");
        }

        public void ack(string s) 
        {
            Ack a = new Ack("ack", s);
            string info = JsonMapper.ToJson(a);
            StandardIO.sendMsg(info + "\n");
        }

        public void fail(string s) 
        {
            Fail f = new Fail("fail", s);
            string info = JsonMapper.ToJson(f);
            StandardIO.sendMsg(info + "\n");
        }
    }

    public class SpiltSentenceBolt  : Bolt
    {
        private Tuple anchor;
        public SpiltSentenceBolt() 
        {
            anchor = new Tuple();
        }
        public void run()
        {
            
            Conf config = null;
            Context context = null;
            initialComponent(config, context);
            this.initialize(config, context);
            try
            {
                while (true)
                {
                    Tuple tup = readTuple();
                    anchor = tup;
                    this.process(tup);
                    ack(tup.getTupleId());
                }
            }
            catch (Exception e)
            {
                log(e.Message);
            }
        }

        public void process(Tuple t) 
        {
            string sentence = t.getValue();
            string[] words = sentence.Split(' ');
            
            List<string> anchor_set = new List<string>();
            anchor_set.Add(anchor.getTupleId());
            
            foreach (string w in words) 
            {
                //这里两行代码，是构造返回的tuple的。返回的格式是一个string的list，
                //在本实例中，所返回是含有1个word的字符串。
                List<string> str_list = new List<string>();
                str_list.Add(w);
                //因为stream 和 task 缺省，所以调用basicBolt
               BasicBoltEmit Emit = new BasicBoltEmit("emit", anchor_set, str_list);
               Emit.emit();
            }
        }
    }
}
