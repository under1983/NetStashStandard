//using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NetStashStandard.Storage.Proxy
{
    public class BaseProxy
    {
        static string dbFilePath = "./NetStash.db";
        static bool initialized = false;
        static object _lock = new object();
        

        internal static bool IsRunningOnMono()
        {
            return false;//Type.GetType("Mono.Runtime") != null;
        }

        public void BaseProxyNet()
        {

            lock (_lock)
            {
                if (initialized) return;

                if (!File.Exists(dbFilePath))
                {
                    if (!IsRunningOnMono())
                    {
                        //if (Core)
                        //    new Microsoft.Data.Sqlite.SqliteConnection(string.Format("Data Source={0}", dbFilePath));
                        //else
                            System.Data.SQLite.SQLiteConnection.CreateFile(dbFilePath);

                        if (!Directory.Exists("x86"))
                        {
                            Directory.CreateDirectory("x86");
                            SaveToDisk("NetStash.x86.SQLite.Interop.dll", "x86\\SQLite.Interop.dll");
                        }

                        if (!Directory.Exists("x64"))
                        {
                            Directory.CreateDirectory("x64");
                            SaveToDisk("NetStash.x64.SQLite.Interop.dll", "x64\\SQLite.Interop.dll");
                        }

                        //if (Core)
                        //{
                        //    using (Microsoft.Data.Sqlite.SqliteConnection cnn = (Microsoft.Data.Sqlite.SqliteConnection)GetConnectionSqlite())
                        //    {
                        //        var command = cnn.CreateCommand();
                        //        command.CommandText = "CREATE TABLE \"Log\" ([IdLog] integer, [Message] nvarchar, PRIMARY KEY(IdLog));";
                        //        cnn.Open();
                        //        command.ExecuteNonQuery();

                        //    }
                        //}
                        //else
                        //{
                            using (System.Data.SQLite.SQLiteConnection cnn = (System.Data.SQLite.SQLiteConnection)GetConnection())
                            {
                                cnn.Open();
                                System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("CREATE TABLE \"Log\" ([IdLog] integer, [Message] nvarchar, PRIMARY KEY(IdLog));", cnn);
                                cmd.ExecuteNonQuery();
                            }
                        //}


                    }
                    
                }

                initialized = true;
            }
        }
        public void BaseProxyCore()
        {

            lock (_lock)
            {
                if (initialized) return;

                if (!File.Exists(dbFilePath))
                {
                    if (!IsRunningOnMono())
                    {
                        //if (Core)
                            new Microsoft.Data.Sqlite.SqliteConnection(string.Format("Data Source={0}", dbFilePath));
                        //else
                        //    System.Data.SQLite.SQLiteConnection.CreateFile(dbFilePath);

                        if (!Directory.Exists("x86"))
                        {
                            Directory.CreateDirectory("x86");
                            SaveToDisk("NetStash.x86.SQLite.Interop.dll", "x86\\SQLite.Interop.dll");
                        }

                        if (!Directory.Exists("x64"))
                        {
                            Directory.CreateDirectory("x64");
                            SaveToDisk("NetStash.x64.SQLite.Interop.dll", "x64\\SQLite.Interop.dll");
                        }

                        //if (Core)
                        //{
                            using (Microsoft.Data.Sqlite.SqliteConnection cnn = (Microsoft.Data.Sqlite.SqliteConnection)GetConnectionSqlite())
                            {
                                var command = cnn.CreateCommand();
                                command.CommandText = "CREATE TABLE \"Log\" ([IdLog] integer, [Message] nvarchar, PRIMARY KEY(IdLog));";
                                cnn.Open();
                                command.ExecuteNonQuery();

                            }
                        //}
                        //else
                        //{
                        //    using (System.Data.SQLite.SQLiteConnection cnn = (System.Data.SQLite.SQLiteConnection)GetConnection())
                        //    {
                        //        cnn.Open();
                        //        System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("CREATE TABLE \"Log\" ([IdLog] integer, [Message] nvarchar, PRIMARY KEY(IdLog));", cnn);
                        //        cmd.ExecuteNonQuery();
                        //    }
                        //}


                    }
                    
                }

                initialized = true;
            }
        }

        internal IDbConnection GetConnectionSqlite()
        {
            if (!IsRunningOnMono())
                return new Microsoft.Data.Sqlite.SqliteConnection(string.Format("Data Source={0}", dbFilePath));
            else
                return new Microsoft.Data.Sqlite.SqliteConnection(string.Format("Data Source={0}", dbFilePath));
        }


        internal IDbConnection GetConnection()
        {
            if (!IsRunningOnMono())
                return new System.Data.SQLite.SQLiteConnection(string.Format("Data Source={0}", dbFilePath));
            else
                return new System.Data.SQLite.SQLiteConnection(string.Format("Data Source={0}", dbFilePath));
        }

        private void SaveToDisk(string file, string name)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
            using (FileStream fileStream = new FileStream(name, FileMode.CreateNew))
                for (int i = 0; i < stream.Length; i++)
                    fileStream.WriteByte((byte)stream.ReadByte());
        }
    }

}
