using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common;
using CMCS.Common.Entities;
using CMCS.Common.Views;
using CMCS.DapperDber.Util;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Enums;
using CMCS.DapperDber.Dbs.SqlServerDb;
using System.Data;

namespace CMCS.CarTransport.DAO
{
    /// <summary>
    /// 视频监控业务
    /// </summary>
    public class VideoDAO
    {
        private static VideoDAO instance;

        public static VideoDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new VideoDAO();
            }

            return instance;
        }

        private VideoDAO()
        { }

        public OracleDapperDber SelfDber
        {
            get { return Dbers.GetInstance().SelfDber; }
        }
        public SqlServerDapperDber SqlServerDber
        {
            get { return Dbers.GetInstance().SqlServerDber; }
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();
        CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();

        /// <summary>
        /// 更新摄像机参数
        /// </summary>
        /// <param name="code">设备编码</param>
        /// <param name="ip">IP</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="port">端口号</param>
        /// <param name="channel">通道号</param>
        /// <returns></returns>
        public bool InsertVideo(string code, string ip, string username, string pwd, string port, string channel)
        {
            CmcsCamare video = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = code });
            if (video != null)
            {
                video.Ip = ip;
                video.UserName = username;
                video.Password = pwd;
                video.Port = Convert.ToInt32(port);
                video.Channel = Convert.ToInt32(channel);
                return commonDAO.SelfDber.Update(video) > 0;
            }
            else
            {
                video = new CmcsCamare()
                {
                    Ip = ip,
                    UserName = username,
                    Password = pwd,
                    Port = Convert.ToInt32(port),
                    Channel = Convert.ToInt32(channel)
                };
                return commonDAO.SelfDber.Insert(video) > 0;
            }
        }

        /// <summary>
        /// 根据编码获取摄像机参数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CmcsCamare GetVideoByName(string code)
        {
            return commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = code });
        }


        /// <summary>
        /// 根据名称获取摄像机参数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CmcsCamare GetVideoByCode(string name)
        {
            return commonDAO.SelfDber.Entity<CmcsCamare>("where Name=:Name", new { Name = name });
        }

        /// <summary>
        /// 更新摄像机
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public bool InsertVideo(CmcsCamare video)
        {
            CmcsCamare entity = commonDAO.SelfDber.Entity<CmcsCamare>("where Id=:Id", new { Id = video.Id });
            if (entity == null)
                return commonDAO.SelfDber.Insert(video) > 0;
            else
                return commonDAO.SelfDber.Update(video) > 0;
        }
        /// <summary>
        /// 删除摄像机
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public bool DelVideo(CmcsCamare video)
        {
            int res = 0;
            string id = string.Empty;
            id = video.Id;

            IList<CmcsCamare> list = commonDAO.SelfDber.Entities<CmcsCamare>("where ParentId=:ParentId", new { ParentId = id });
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    DelVideo(item);
                }
            }
            else
            {
                commonDAO.SelfDber.Delete<CmcsCamare>(id);
                res++;
            }

            return res > 0;
        }

        /// <summary>
        /// 根据父摄像头编码获取下级节点摄像头编码（父编码+2位逐级递增的数值）
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string GetCameraNewChildCode(string strCode)
        {
            string strNewCode = strCode;

            for (int i = 1; i < 100; i++)
            {
                strNewCode = strCode + i;
                if (i < 10)
                {
                    strNewCode = strCode + "0" + i;
                }
                //判断该编码是否已经存在
                var count = commonDAO.SelfDber.Count<CmcsCamare>("where Code=:Code", new { Code = strNewCode });
                if (count == 0) break;
            }

            return strNewCode;
        }

        /// <summary>
        /// 获取摄像头当前深度
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public int GetCameraDepth(CmcsCamare video)
        {
            int count = 1;
            string parentid = video.ParentId;
            while (true)
            {
                CmcsCamare res = commonDAO.SelfDber.Entity<CmcsCamare>("where ParentId=:ParentId", new { ParentId = parentid });

                if (res == null)
                {
                    break;
                }
                else
                {
                    count++;
                    parentid = res.Id;
                }
            }
            return count;
        }
        /// <summary>
        /// 获取汽车衡重量
        /// </summary>
        /// <returns></returns>
        public decimal[] GetAllQchWeight()
        {
            decimal[] weight = new decimal[] { 0, 0, 0, 0 };
            string Sql = string.Format("select signalprefix,signalname,signalvalue from cmcstbsignaldata where signalname='地磅仪表_实时重量' order by signalprefix");
            DataTable data = commonDAO.SelfDber.ExecuteDataTable(Sql);
            if (data != null && data.Rows.Count > 0)
            {
                for (int i = 0; i < weight.Length; i++)
                {
                    weight[i] = data.Rows[i]["signalvalue"] != DBNull.Value ? Convert.ToDecimal(data.Rows[i]["signalvalue"]) : 0;
                }
            }
            return weight;
        }
    }
}
