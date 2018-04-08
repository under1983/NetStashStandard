using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetStashStandard
{
   // [Newtonsoft.Json.JsonObject]
    public class NetStashEvent
    {
       // [Newtonsoft.Json.JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; }
       // [Newtonsoft.Json.JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

       // [Newtonsoft.Json.JsonProperty(PropertyName = "exception-details")]
        public string ExceptionDetails { get; set; }
       // [Newtonsoft.Json.JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

       // [Newtonsoft.Json.JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

      //  [Newtonsoft.Json.JsonProperty(PropertyName = "level")]
        public string Level { get; set; }

       // [Newtonsoft.Json.JsonProperty(PropertyName = "machine-name")]
        public string Machine { get; set; }
       // [Newtonsoft.Json.JsonProperty(PropertyName = "machine-ip")]
        public string MachineIP { get; set; }

       // [Newtonsoft.Json.JsonProperty(PropertyName = "app")]
        public string App { get; set; }
       // [Newtonsoft.Json.JsonProperty(PropertyName = "app-version")]
        public string AppVersion { get; set; }
       // [Newtonsoft.Json.JsonProperty(PropertyName = "module")]
        public string Module { get; set; }

        //[Newtonsoft.Json.JsonProperty(PropertyName = "fields")]
        //public Dictionary<string, string> Fields { get; set; }

       // [Newtonsoft.Json.JsonProperty(PropertyName = "oldvalue")]
        public string OldValue { get; set; }
       // [Newtonsoft.Json.JsonProperty(PropertyName = "newvalue")]
        public string NewValue { get; set; }

        public NetStashEvent()
        {
            Timestamp = DateTime.Now;
        }

        public string GetJson()
        {
            List<string> jsonFields = new List<string>();
            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                object o = GetType().GetProperty(p.Name).GetValue(this);
                //jsonFields.Add(StringFormat(p.Name) + ':' + StringFormat(o == null ? "" : o.ToString()));
                jsonFields.Add(string.Format("\"{0}\":\"{1}\" ", p.Name, o == null ? "" : o.ToString()));
            }
            return "{" + String.Join(",", jsonFields) + "}";
        }

        //private string StringFormat(string s)
        //{
        //    return '"' + s + '"';
        //}
    }
}
