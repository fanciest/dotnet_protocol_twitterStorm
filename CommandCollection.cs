using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSMECS
{
    class Log
    {
        string command;
        string msg;
        public Log(string _command, string _msg)
        {
            this.command = _command;
            this.msg = _msg;
        }
    }
    class Ack
    {
        string command;
        string id;
        public Ack(string _command, string _id)
        {
            this.command = _command;
            this.id = _id;
        }
    }
    class Fail
    {
        string command;
        string id;
        public Fail(string _command, string _id)
        {
            this.command = _command;
            this.id = _id;
        }
    }
    class Sync
    {
        string command;
        public Sync(string _command){this.command = _command;}
    }
}
