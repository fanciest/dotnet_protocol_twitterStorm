using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using LitJson;

namespace TSMECS
{
    class Spout
    {
        private Queue<JsonData> command_queue = new Queue<JsonData>();
        private void initialComponent(Conf config, Context context)
        {
            string information = StandardIO.readMsg();
            InitialHandshake handshake = InitialHandshake.Parse(information);
            config = handshake.conf;
            context = handshake.context;
            int process_id = Process.GetCurrentProcess().Id;
            PID temp = new PID();
            temp.pid = process_id;
            string json_pid = JsonMapper.ToJson(temp);
            StandardIO.sendMsg(json_pid);
            string file = handshake.pidDir + "/" + process_id.ToString();
            if (!System.IO.File.Exists(file))
            {
                System.IO.FileStream f = new System.IO.FileStream(file, System.IO.FileMode.CreateNew);
            }
        }
        public void initialize(Conf stormconfig, Context context) 
        { // do nothing
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
       
        public void nextTuple() 
        {
           
        }

        public void sync() 
        {
            Sync sync_instance = new Sync("sync");
            string s = JsonMapper.ToJson(sync_instance);
            StandardIO.sendMsg(s + "\n");
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
                    JsonData jd = readCommand();
                    string com = (string)jd["command"];
                    if (com.Equals("next"))
                        this.nextTuple();
                    else if (com.Equals("ack"))
                    {
                        string id = (string)jd["id"];
                        this.ack(id);
                    }
                    else if (com.Equals("fail")) 
                    {
                        string id = (string)jd["id"];
                        this.fail(id);
                    }
                    sync();
                }
            }
            catch (Exception e)
            {
                log(e.Message);
            }
        }

        private JsonData readCommand()
        {
            if(this.command_queue.Count>0)
                return command_queue.Dequeue();
            string s = StandardIO.readMsg();
            JsonData jd = JsonMapper.ToObject(s);
            while (jd.IsArray) 
            {
                command_queue.Enqueue(jd);
                jd = JsonMapper.ToObject(StandardIO.readMsg());
            }
            return jd;
        }
        private void log(string s)
        {
            Log l = new Log("log", s);
            string info = JsonMapper.ToJson(l);
            StandardIO.sendMsg(info + "\n");
        }

      
    }
}
