using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.AssayDevice.Entities;
using CMCS.Common;
using CMCS.Common.Entities.AssayDevice;

namespace CMCS.DumblyConcealer.Tasks.AssayDevice
{
    public class AssayDevice_DAO
    {
        private static AssayDevice_DAO instance;

        public static AssayDevice_DAO GetInstance()
        {
            if (instance == null)
            {
                instance = new AssayDevice_DAO();
            }
            return instance;
        }

        private AssayDevice_DAO()
        {

        }

        /// <summary>
        /// 保存测硫仪数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public int SaveToSulfurStdAssay(Action<string, eOutputType> output)
        {
            int res = 0;
            var list = DcDbers.GetInstance().AssayDevice_Dber.Entities<sulfurstdassay>("where TESTDATE>= :TESTDATE and MANNUMB is not null", new { TESTDATE = DateTime.Now.AddDays(0 - DcDbers.GetInstance().AssayDevice_Days).Date });
            foreach (sulfurstdassay entity in list)
            {
                string pkid = entity.TESTDATE.ToShortDateString() + " " + entity.SPARE5;
                CmcsSulfurStdAssay item = Dbers.GetInstance().SelfDber.Entity<CmcsSulfurStdAssay>("where PKID=:PKID", new { PKID = pkid });
                if (item == null)
                {
                    item.SampleNumber = entity.MANNUMB;
                    item.FacilityNumber = entity.FACILITYNUMBER;
                    item.ContainerWeight = entity.TARE;
                    item.SampleWeight = entity.SAMPLEMASS;
                    item.Stad = entity.SAD;
                    item.AssayUser = entity.TESTMAN;
                    item.AssayTime = Convert.ToDateTime(entity.TESTDATE.ToShortDateString() + " " + entity.SPARE5);
                    item.OrderNumber = 0;
                    item.IsEffective = 0;
                    item.PKID = pkid;
                    res += Dbers.GetInstance().SelfDber.Insert<CmcsSulfurStdAssay>(item);
                }
                else
                {
                    item.SampleNumber = entity.MANNUMB.ToString();
                    item.FacilityNumber = entity.FACILITYNUMBER;
                    item.ContainerWeight = entity.TARE;
                    item.SampleWeight = entity.SAMPLEMASS;
                    item.Stad = entity.SAD;
                    item.AssayUser = entity.TESTMAN;
                    item.AssayTime = Convert.ToDateTime(entity.TESTDATE.ToShortDateString() + " " + entity.SPARE5);
                    item.OrderNumber = 0;
                    res += Dbers.GetInstance().SelfDber.Update<CmcsSulfurStdAssay>(item);
                }
            }
            output(string.Format("生成标准测硫仪数据 {0} 条（集中管控）", res), eOutputType.Normal);
            return res;
        }

        /// <summary>
        /// 保存量热仪数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public int SaveToHeatStdAssay(Action<string, eOutputType> output)
        {
            int res = 0;
            var list = DcDbers.GetInstance().AssayDevice_Dber.Entities<heatstdassay>("where CSTIME>= :CSTIME and HANDNUM is not null", new { CSTIME = DateTime.Now.AddDays(0 - DcDbers.GetInstance().AssayDevice_Days).Date });
            foreach (heatstdassay entity in list)
            {
                string pkid = entity.PKID;
                CmcsHeatStdAssay item = Dbers.GetInstance().SelfDber.Entity<CmcsHeatStdAssay>("where PKID=:PKID", new { PKID = pkid });
                if (item == null)
                {
                    item.SampleNumber = entity.HANDNUM;
                    item.FacilityNumber = entity.FACILITYNUMBER;
                    item.ContainerWeight = 0;
                    item.SampleWeight = entity.SYZL;
                    item.Qbad = entity.DTFRL / 1000m;
                    item.AssayUser = entity.HYY;
                    item.AssayTime = entity.CSTIME;
                    item.IsEffective = 0;
                    item.PKID = pkid;
                    res += Dbers.GetInstance().SelfDber.Insert<CmcsHeatStdAssay>(item);
                }
                else
                {
                    item.SampleNumber = entity.HANDNUM;
                    item.FacilityNumber = entity.FACILITYNUMBER;
                    item.ContainerWeight = 0;
                    item.SampleWeight = entity.SYZL;
                    item.Qbad = entity.DTFRL / 1000m;
                    item.AssayUser = entity.HYY;
                    item.AssayTime = entity.CSTIME;
                    res += Dbers.GetInstance().SelfDber.Update<CmcsHeatStdAssay>(item);
                }

            }
            output(string.Format("生成标准量热仪数据 {0} 条（集中管控）", res), eOutputType.Normal);
            return res;
        }


        /// <summary>
        /// 保存水分仪数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public int SaveToMoistureStdAssay(Action<string, eOutputType> output)
        {
            int res = 0;
            var list = DcDbers.GetInstance().AssayDevice_Dber.Entities<moisturestdassay>("where TESTDATE>= :TESTDATE and MANNUMB is not null", new { TESTDATE = DateTime.Now.AddDays(0 - DcDbers.GetInstance().AssayDevice_Days).Date });
            foreach (moisturestdassay entity in list)
            {
                string pkid = entity.PKID;

                CmcsMoistureStdAssay item = Dbers.GetInstance().SelfDber.Entity<CmcsMoistureStdAssay>("where PKID=:PKID", new { PKID = pkid });
                if (item == null)
                {
                    item.SampleNumber = entity.MANNUMB;
                    item.FacilityNumber = entity.FACILITYNUMBER;
                    item.ContainerWeight = entity.POTMASS;
                    item.SampleWeight = entity.SAMPLEMASS;
                    item.WaterPer = entity.WATERPER;
                    item.AssayUser = entity.TESTMAN;
                    item.IsEffective = 0;
                    item.PKID = pkid;
                    item.AssayTime = entity.TESTDATE.AddSeconds(Convert.ToInt32(entity.AUTONUMB.Substring(9)));
                    item.WaterType = entity.WATERTYPE.Contains("全水") ? "全水分" : "分析水";
                    res += Dbers.GetInstance().SelfDber.Insert<CmcsMoistureStdAssay>(item);
                }
                else
                {
                    item.SampleNumber = entity.MANNUMB;
                    item.FacilityNumber = entity.FACILITYNUMBER;
                    item.ContainerWeight = entity.POTMASS;
                    item.SampleWeight = entity.SAMPLEMASS;
                    item.WaterPer = entity.WATERPER;
                    item.AssayUser = entity.TESTMAN;
                    item.AssayTime = entity.TESTDATE.AddSeconds(Convert.ToInt32(entity.AUTONUMB.Substring(9)));
                    item.WaterType = entity.WATERTYPE.Contains("全水") ? "全水分" : "分析水";
                    res += Dbers.GetInstance().SelfDber.Update<CmcsMoistureStdAssay>(item);
                }
            }
            output(string.Format("生成标准水分仪数据 {0} 条（集中管控）", res), eOutputType.Normal);
            return res;
        }
    }
}
