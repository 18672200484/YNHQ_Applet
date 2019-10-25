using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace CMCS.DapperDber.Dbs.SQLiteDb
{
    /// <summary>
    /// SQLite 数据库访问对象
    /// </summary>
    public class SQLiteDapperDber : BaseDber
    {
        private SQLiteConnection connection;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public new SQLiteConnection Connection
        {
            get { lock (base.objLock) { return connection; } }
        } 

        /// <summary>
        /// SQLiteDapperDber
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SQLiteDapperDber(string connectionString)
            : base(new SQLiteConnection(connectionString), new SQLiteDataAdapter(), new SQLiteSqlBuilder())
        {
            this.connection = base.Connection as SQLiteConnection;
        }
    }
}
