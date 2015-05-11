using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace TSMECS
{
 //"command": "emit",
 //   // The ids of the tuples this output tuples should be anchored to
 //   "anchors": ["1231231", "-234234234"],
 //   // The id of the stream this tuple was emitted to. Leave this empty to emit to default stream.
 //   "stream": "1",
 //   // If doing an emit direct, indicate the task to send the tuple to
 //   "task": 9,
 //   // All the values in this tuple
 //   "tuple": ["field1", 2, 3]

    public class BasicBoltEmit
    {
        protected string command;
        protected List<string> anchors;
        protected List<string> tuple;
        public BasicBoltEmit(string _command, List<string> _anchors, List<string> _tuple)
        {
            command = _command;
            anchors = _anchors;
            tuple = _tuple;
        }

        public void emit()
        {
            StringBuilder str_builder = new StringBuilder();
            JsonWriter jw = new JsonWriter(str_builder);
            jw.WriteObjectStart();
            jw.WritePropertyName("command");
            jw.Write("emit");
            jw.WritePropertyName("anchors");
            jw.WriteArrayStart();
            foreach (string temp_str in anchors)
                jw.Write(temp_str);
            jw.WriteArrayEnd();
            jw.WritePropertyName("tuple");
            jw.WriteArrayStart();
            foreach (string temp_str in tuple)
                jw.Write(temp_str);
            jw.WriteArrayEnd();
            jw.WriteObjectEnd();
            string info = str_builder.ToString();
            StandardIO.sendMsg(info);
            Utility.readTaskIds();
        }
    }

    public class BoltEmit 
    {
        string command;
        List<string> anchors;
        string stream;
        int task;
        List<string> tuple;

        public BoltEmit(string _command, List<string> _anchors, string _stream, int _task, List<string> _tuple) 
        {
            command = _command;
            anchors = _anchors;
            stream = _stream;
            task = _task;
            tuple = _tuple;
        }
        
    }

   
    //"command": "emit",
    //// The id for the tuple. Leave this out for an unreliable emit. The id can
    //// be a string or a number.
    //"id": "1231231",
    //// The id of the stream this tuple was emitted to. Leave this empty to emit to default stream.
    //"stream": "1",
    //// If doing an emit direct, indicate the task to send the tuple to
    //"task": 9,
    //// All the values in this tuple
    //"tuple": ["field1", 2, 3]
    public class SpoutEmit 
    {
        public string command;
        string id;
        string stream;
        int task;
        List<Object> tuple;
        public SpoutEmit(string _command,string _id, string _stream, int _task, List<Object> _tuple) 
        {
            command = _command;
            id = _id;
            stream = _stream;
            task = _task;
            tuple = _tuple;
        }
       public void emit() 
       {
            StandardIO.sendMsg( JsonMapper.ToJson(this)+"\n");
       }
    }
}
