//using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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



        public void BaseProxyNet()
        {

            lock (_lock)
            {
                if (initialized) return;

                if (!File.Exists(dbFilePath))
                {
                    System.Data.SQLite.SQLiteConnection.CreateFile(dbFilePath);

                    CheckedDirectory();

                    CreateTable(GetConnection());
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
                    new Microsoft.Data.Sqlite.SqliteConnection(string.Format("Data Source={0}", dbFilePath));

                    CheckedDirectory();

                    CreateTable(GetConnectionSqlite());

                }

                initialized = true;
            }
        }

        internal void CreateTable(IDbConnection Connection)
        {
            using (IDbConnection cnn = Connection)
            {
                cnn.Open();
                IDbCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "CREATE TABLE \"Log\" ([IdLog] integer, [Message] nvarchar, PRIMARY KEY(IdLog));";
                cmd.ExecuteNonQuery();
            }
        }

        internal IDbConnection GetConnectionSqlite()
        {
            return new Microsoft.Data.Sqlite.SqliteConnection(string.Format("Data Source={0}", dbFilePath));
        }


        internal IDbConnection GetConnection()
        {
            return new System.Data.SQLite.SQLiteConnection(string.Format("Data Source={0}", dbFilePath));
        }

        internal void CheckedDirectory()
        {
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
