using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// 
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace CMCS.DapperDber.Dbs.AccessDb
{
    /// <summary>
    /// Access 数据库访问对象
    /// </summary>
    public class AccessDapperDber : BaseDber
    {
        private OleDbConnection connection;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public new OleDbConnection Connection
        {
            get { lock (base.objLock) { return connection; } }
        }

        /// <summary>
        /// AccessDapperDber
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public AccessDapperDber(string connectionString)
            : base(new OleDbConnection(connectionString), new OleDbDataAdapter(), new AccessSqlBuilder())
        {
            this.connection = base.Connection as OleDbConnection;
        }
    }
}
