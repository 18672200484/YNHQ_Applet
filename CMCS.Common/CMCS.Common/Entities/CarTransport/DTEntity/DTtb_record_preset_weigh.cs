using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DapperDber.Attrs;

namespace CMCS.Common.Entities.CarTransport.DTEntity
{
    /// <summary>
    /// 称重预置信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("tb_record_preset_weigh")]
    public class DTtb_record_preset_weigh
    {
        /// <summary>
        ///  主键
        /// </summary>
        [DapperPrimaryKey]
        [DapperIgnoreAttribute]
        public int ID { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public string cardserial { get; set; }

        /// <summary>
        /// 车号
        /// </summary>
        public string licenseplate { get; set; }

        /// <summary>
        /// 驾驶员	
        /// </summary>
        public string driver { get; set; }

        /// <summary>
        /// 发货单位
        /// </summary>
        public string sendcorpCode { get; set; }

        /// <summary>
        /// 收货单位
        /// </summary>
        public string recvcorpCode { get; set; }

        /// <summary>
        /// 货物名称
        /// </summary>
        public string oretypeCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string productCode { get; set; }

        /// <summary>
        /// 承运单位
        /// </summary>
        public string carrierCode { get; set; }

        /// <summary>
        /// 进出标志
        /// </summary>
        public int direction { get; set; }

        /// <summary>
        /// 卡类型
        /// </summary>
        public int cardType { get; set; }

        /// <summary>
        /// 货单号
        /// </summary>
        public string orderNo { get; set; }

        /// <summary>
        /// 发货重量
        /// </summary>
        public float presetWeight { get; set; }

    }
}
