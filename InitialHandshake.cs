using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using LitJson;

namespace TSMECS
{
   public  class PID 
    {
        public int pid{get;set;}
    }
    public class Conf
{
        private Dictionary<string, object> config_map;

        public Conf() 
        {
            config_map = new Dictionary<string, object>();
        }

        public bool addItem(string key, object value) 
        {
            try 
            {
                config_map.Add(key, value);
                return true;
            }
            catch (Exception e) 
            {
                //Console.WriteLine("Error in Config Add item into map: "+e.Message);
                return false;
            }
        }
}

public class TaskComponent
{
    private Dictionary<string,string> component_list;
    public TaskComponent() 
    {
        component_list = new Dictionary<string, string>();
    }
    public bool addItem(string key, string value) 
    {
        try
        {
            component_list.Add(key, value);
            return true;
        }
        catch (Exception e)
        {
           // Console.WriteLine("Error in TaskComponent Add item into map: " + e.Message);
            return false;
        }
    }
}

public class Context
{
    public TaskComponent  component { get; set; }
    public int taskid { get; set; }
    public Context() 
    {
        component = new TaskComponent();
        taskid = -1;
    }
}

public class InitialHandshake
{
    public Conf conf { get; set; }
    public Context context { get; set; }
    public string pidDir { get; set; }

    public InitialHandshake() 
    {
        conf = new Conf();
        context = new Context();
        pidDir = null;
    }

    public static InitialHandshake Parse(string s) 
    {
        try
        {
            //LocalLog.writeLog(s);
            //Console.WriteLine(s);
            s = formatAdjust(s);
            //Console.WriteLine("Format String Sucess");
            JsonData jd = JsonMapper.ToObject(s);
            InitialHandshake handshake = new InitialHandshake();
            //Console.WriteLine("Initial Handshake Success");
            // test 测试keys的读取
            //Console.WriteLine("Test for keys");
            //foreach (string str_item in jd.getKeys())
            //{
            //    Console.WriteLine(str_item);
            //}
            /////////////////////////////////////////////////////
            if (jd["pidDir"].IsString)
                handshake.pidDir = (string)jd["pidDir"];
            else
            {
               // Console.WriteLine("PidDir load fail");
            }
            //////////////////////////////////////////////////////
            JsonData temp_config_json = jd["conf"];
            foreach (string str_item in temp_config_json.getKeys())
            {
                string key = str_item;
                if (jd["conf"][key] != null)
                {
                    object value = (object)jd["conf"][key];
                    if (!value.ToString().Equals("empty"))
                    {
                        handshake.conf.addItem(key, value);
                       // Console.WriteLine(key + "   " + value);
                    }
                }
            }

            JsonData temp_context_json = jd["context"];
            if (temp_context_json.hasKey("taskid"))
            {
                //Console.WriteLine("has task id");
                JsonData temp_task_id = jd["context"]["taskid"];
                if (temp_task_id.IsInt || temp_task_id.IsLong)
                    handshake.context.taskid = (int)jd["context"]["taskid"];
            }
            else
            {
                //Console.WriteLine("no task id");
            }
            if (temp_context_json.hasKey("task->component"))
            {
               // Console.WriteLine("has task component");
                JsonData temp_task_components = jd["context"]["task->component"];
                foreach (string str_item in temp_task_components.getKeys())
                {
                    string key = str_item;
                    //Console.WriteLine(key);
                    if (temp_task_components[key] != null)
                    {
                        string value = (string)temp_task_components[key];
                        handshake.context.component.addItem(key, value);
                        //Console.WriteLine(key + "   " + value);
                    }

                }
            }
            else
            {
                //Console.WriteLine("no task component");
            }
            return handshake;
        }
        catch (Exception e) 
        {
            LocalLog.writeLog(e.Message);
           // Console.WriteLine(e.Message);
            return new InitialHandshake();
        }
    }

    private static string formatAdjust(string s)
    {
        string result = null;
        char c = '\\';
        int length = s.Length;
        result = result + s[0];
        for (int i = 1; i < length - 1; i++) 
        {
            if (s[i].Equals(c) && Char.IsLetter(s[i + 1]) && (!s[i-1].Equals(c)))
                result = result + c + c;
            else
                result = result + s[i];
        }
        result = result + s[length - 1];
        result.Replace("null", "\"empty\"");
        result.Replace("nil", "\"empty\"");
        return result;
    }
}

}
