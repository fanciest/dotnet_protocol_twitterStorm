using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace TSMECS
{
    public class Utility
    {
        public static Queue<JsonData> pending_commands = new Queue<JsonData>();
        public static Queue<JsonData> pending_taskids = new Queue<JsonData>();
        public static JsonData readTaskIds()
        {
            if (pending_taskids.Count != 0)
                return pending_taskids.Dequeue();
            else 
            {
                JsonData temp_jd = JsonMapper.ToObject( StandardIO.readMsg());
                while (!temp_jd.IsArray) 
                {
                    pending_commands.Enqueue(temp_jd);
                    temp_jd = JsonMapper.ToObject(StandardIO.readMsg());
                }
                return temp_jd;
            }
        }

        public static JsonData readCommand()
        {
             if (pending_commands .Count != 0)
                return pending_commands.Dequeue();
            else 
            {
                JsonData temp_jd = JsonMapper.ToObject( StandardIO.readMsg());
                while (temp_jd.IsArray) 
                {
                    pending_taskids.Enqueue(temp_jd);
                    temp_jd = JsonMapper.ToObject(StandardIO.readMsg());
                }
                return temp_jd;
            }
        }

    }
}
