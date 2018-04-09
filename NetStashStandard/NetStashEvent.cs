using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetStashStandard
{
   // [Newtonsoft.Json.JsonObject]
    public class NetStashEvent
    {
        //[Newtonsoft.Json.JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; }
        //[Newtonsoft.Json.JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        //[Newtonsoft.Json.JsonProperty(PropertyName = "exception-details")]
        public string ExceptionDetails { get; set; }
        //[Newtonsoft.Json.JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        //[Newtonsoft.Json.JsonProperty(PropertyName = "level")]
        public string Level { get; set; }

        //[Newtonsoft.Json.JsonProperty(PropertyName = "machine-name")]
        public string Machine { get; set; }
        //[Newtonsoft.Json.JsonProperty(PropertyName = "mac-address")]
        public string MacAddress { get; set; }

        //[Newtonsoft.Json.JsonProperty(PropertyName = "method")]
        public string Method { get; set; }
        //[Newtonsoft.Json.JsonProperty(PropertyName = "app-version")]
        public string AppVersion { get; set; }

        //[Newtonsoft.Json.JsonProperty(PropertyName = "fields")]
        public Dictionary<string, string> Fields { get; set; }

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
                jsonFields.Add(string.Format("\"{0}\":\"{1}\" ", p.Name, o == null ? "" : o.ToString()));
            }
            return "{" + String.Join(",", jsonFields) + "}";
        }
    }
}
