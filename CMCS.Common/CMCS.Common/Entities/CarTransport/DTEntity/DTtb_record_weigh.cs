using CMCS.DapperDber.Attrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMCS.Common.Entities.CarTransport.DTEntity
{
    /// <summary>
    /// 称重信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("tb_record_weigh")]
    public class DTtb_record_weigh
    {
        /// <summary>
        /// 单号  主键	与设备序列号做联合主键
        /// </summary>
        [DapperPrimaryKey]
        public int weightID { get; set; }

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
        /// 货单号	
        /// </summary>
        public string orderNo { get; set; }

        /// <summary>
        /// 进出 1:销售 2:外购
        /// </summary>
        public int direction { get; set; }

        /// <summary>
        /// 毛重
        /// </summary>
        public decimal totalweight { get; set; }

        /// <summary>
        /// 皮重
        /// </summary>
        public decimal tare { get; set; }

        /// <summary>
        /// 扣重（扣水）
        /// </summary>
        public decimal buckle { get; set; }

        /// <summary>
        /// 扣重（扣石）
        /// </summary>
        public decimal buckleStone { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        public decimal neatweight { get; set; }

        /// <summary>
        /// 矿发重量
        /// </summary>
        public decimal shipWeigh { get; set; }

        /// <summary>
        /// 盈亏（净重-矿发重量）
        /// </summary>
        public decimal pl { get; set; }

        /// <summary>
        /// 净重大写	
        /// </summary>
        public string neatStr { get; set; }

        /// <summary>
        /// 单价	
        /// </summary>
        public decimal unitfee { get; set; }

        /// <summary>
        /// 金额	
        /// </summary>
        public decimal salefee { get; set; }

        /// <summary>
        /// 金额大写	
        /// </summary>
        public string salefeeStr { get; set; }

        /// <summary>
        /// 司磅员	
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 过磅时间	
        /// </summary>
        public DateTime weightime { get; set; }

        /// <summary>
        /// 打印次数	
        /// </summary>
        public int printCount { get; set; }

        /// <summary>
        /// 设备序列号	
        /// </summary>
        public string deviceserial { get; set; }

        /// <summary>
        /// 进厂时间	
        /// </summary>
        public DateTime oldWeighTime { get; set; }

        /// <summary>
        /// 进厂司磅员	
        /// </summary>
        public string oldOperator { get; set; }

        /// <summary>
        /// 进厂设备序列号	
        /// </summary>
        public string oldDeviceserial { get; set; }

        /// <summary>
        /// 入厂单号	
        /// </summary>
        public int oldWeightId { get; set; }

        /// <summary>
        /// 状态	0:称重磅单 1：入厂称重未出厂 2: 入厂已删除
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 备注	
        /// </summary>
        public string descript { get; set; }

        /// <summary>
        /// 图片路径	
        /// </summary>
        public string imgFilePath { get; set; }

        /// <summary>
        /// ICS上传标志	0:未上传 1:已上传
        /// </summary>
        public string upLoadData { get; set; }

        /// <summary>
        /// 是否上传	0:未上传  1:已上传
        /// </summary>
        public string uploadStatus { get; set; }

    }
}
