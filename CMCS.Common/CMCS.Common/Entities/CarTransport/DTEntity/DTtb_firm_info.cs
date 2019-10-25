using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DapperDber.Attrs;

namespace CMCS.Common.Entities.CarTransport.DTEntity
{
    /// <summary>
    /// 矿企信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("tb_firm_info")]
    public class DTtb_firm_info
    {
        public DTtb_firm_info()
        {
            address = "";
            contact = "";
            postcode = "1";
            phone = "";
            longitude = 0;
            latitude = 0;
            descript = "";
            status = 0;
            firmType = 1;
        }

        /// <summary>
        /// 主键
        /// </summary>
        [DapperPrimaryKey]
        [DapperIgnoreAttribute]
        public int ID { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string postcode { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string contact { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public float longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public float latitude { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string descript { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 企业类型 0：本企业 1：供货企业 2：收货企业
        /// </summary>
        public int firmType { get; set; }

    }
}
