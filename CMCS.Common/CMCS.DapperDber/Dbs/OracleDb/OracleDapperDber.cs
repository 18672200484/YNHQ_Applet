using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;

namespace CMCS.DapperDber.Dbs.OracleDb
{
    /// <summary>
    /// Oracle 数据库访问对象
    /// </summary>
    public class OracleDapperDber : BaseDber
    {
        private OracleConnection connection;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public new OracleConnection Connection
        {
            get { return connection; }
        }

        /// <summary>
        /// OracleDapperDber
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public OracleDapperDber(string connectionString)
            : base(new OracleConnection(connectionString), new OracleDataAdapter(), new OracleSqlBuilder())
        {
            this.connection = base.Connection as OracleConnection;
        }
    }
}
