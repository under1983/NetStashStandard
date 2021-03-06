﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetStashStandard.Storage.Proxy
{
    public class LogProxy //: BaseProxy
    {
        private BaseProxy baseProxy = new BaseProxy();
        public LogProxy() //: base(Core)
        {
            if (NetStashStandard.Log.NetStashLog.TypeNet == Log.TypeNet.NetCore)
                baseProxy.BaseProxyCore();
            else
                baseProxy.BaseProxyNet();
        }

        public void Add(NetStashEvent log)
        {
            Entities.Log addLog = new Entities.Log();
            //addLog.Message = Newtonsoft.Json.JsonConvert.SerializeObject(log);

            //Microsoft.AspNetCore.Formatter.Json
            string JsonString = log.GetJson();
            addLog.Message = JsonString;

            using (IDbConnection db = NetStashStandard.Log.NetStashLog.TypeNet == Log.TypeNet.NetCore ? baseProxy.GetConnectionSqlite() : baseProxy.GetConnection())

            using (IDbCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Log (Message) VALUES (@Message)";
                cmd.CommandType = CommandType.Text;
                IDbDataParameter pMessage = cmd.CreateParameter();
                pMessage.ParameterName = "@Message";
                pMessage.Value = addLog.Message;
                cmd.Parameters.Add(pMessage);
                cmd.Prepare();
                db.Open();
                cmd.ExecuteNonQuery();
            }
        }


//        public NetStashEvent Get(out long id)
//        {
//            Entities.Log getLog = null;
////#if NETCOREAPP2_0
//          using (IDbConnection db = base.GetConnectionSqlite())
////#else
////          using (IDbConnection db = base.GetConnection())
////#endif
//            using (IDbCommand cmd = db.CreateCommand())
//            {
//                cmd.CommandText = "SELECT IdLog, Message from Log order by IdLog asc LIMIT 1";
//                cmd.CommandType = CommandType.Text;
//                cmd.Prepare();
//                db.Open();

//                IDataReader reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    getLog = new Entities.Log();
//                    getLog.IdLog = reader.GetInt64(reader.GetOrdinal("IdLog"));
//                    getLog.Message = reader.IsDBNull(reader.GetOrdinal("Message")) ? null : reader.GetString(reader.GetOrdinal("Message"));

//                }
//            }

//            if (getLog == null)
//            {
//                id = -1;
//                return null;
//            }

//            id = getLog.IdLog;

//            return Newtonsoft.Json.JsonConvert.DeserializeObject<NetStashEvent>(getLog.Message);
//        }

        public Dictionary<long, string> GetList(int count = 100)
        {
            Dictionary<long, string> ret = new Dictionary<long, string>();

            List<Entities.Log> evs = new List<Entities.Log>();

            //#if NETCOREAPP2_0
            //          using (IDbConnection db = base.GetConnectionSqlite())
            //#else
            //           using (IDbConnection db = base.GetConnection())
            //#endif
            using (IDbConnection db = NetStashStandard.Log.NetStashLog.TypeNet == Log.TypeNet.NetCore ? baseProxy.GetConnectionSqlite() : baseProxy.GetConnection())
            using (IDbCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = "SELECT IdLog, Message from Log order by IdLog asc LIMIT " + count;
                cmd.CommandType = CommandType.Text;
                cmd.Prepare();
                db.Open();

                IDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Entities.Log log = new Entities.Log();

                    log.IdLog = reader.GetInt64(reader.GetOrdinal("IdLog"));
                    log.Message = reader.IsDBNull(reader.GetOrdinal("Message")) ? null : reader.GetString(reader.GetOrdinal("Message"));

                    evs.Add(log);
                }
            }

            foreach (Entities.Log item in evs)
                ret.Add(item.IdLog, item.Message);

            return ret;
        }

        public void Delete(long id)
        {
            if (id < 0) return;

            //#if NETCOREAPP2_0
            //          using (IDbConnection db = base.GetConnectionSqlite())
            //#else
            //           using (IDbConnection db = base.GetConnection())
            //#endif
            using (IDbConnection db = NetStashStandard.Log.NetStashLog.TypeNet == Log.TypeNet.NetCore ? baseProxy.GetConnectionSqlite() : baseProxy.GetConnection())
            using (IDbCommand cmd = db.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Log WHERE IdLog = @IdLog";
                cmd.CommandType = CommandType.Text;
                IDbDataParameter pIdLog = cmd.CreateParameter();
                pIdLog.ParameterName = "@IdLog";
                pIdLog.Value = id;
                cmd.Parameters.Add(pIdLog);
                cmd.Prepare();
                db.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
