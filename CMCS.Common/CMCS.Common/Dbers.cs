using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CMCS.DapperDber.Dbs.AccessDb;
// 
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.DapperDber.Dbs.SqlServerDb;
using CMCS.Common.DapperDber_etc;

namespace CMCS.Common
{
    /// <summary>
    /// 数据库访问
    /// </summary>
    public class Dbers
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        protected static object objLock = new object();

        private static Dbers instance;

        public static Dbers GetInstance()
        {
            lock (objLock)
            {
                if (instance == null)
                {
                    instance = new Dbers();
                }

                return instance;
            }
        }

        private Dbers()
        {
            CommonAppConfig appConfig = CommonAppConfig.GetInstance();

            SelfDber = new OracleDapperDber_iEAA(appConfig.SelfConnStr);
            SelfDber.SqlWatch += new DapperDber.Dbs.BaseDber.SqlWatchEventHandler(FuelMisDber_SqlWatch);

            SqlServerDber = new SqlServerDapperDber(appConfig.SqlServerConnStr);
            SqlServerDber.SqlWatch += new DapperDber.Dbs.BaseDber.SqlWatchEventHandler(FuelMisDber_SqlWatch);

        }

        void FuelMisDber_SqlWatch(string type, string sql)
        {
            //BasisPlatform.Util.Log4netUtil.Info(sql);
        }

        /// <summary>
        /// 燃料集中管控 Oracle 访问
        /// </summary>
        public OracleDapperDber_iEAA SelfDber;


        /// <summary>
        /// 大唐数据库访问
        /// </summary>
        public SqlServerDapperDber SqlServerDber;


    }
}
