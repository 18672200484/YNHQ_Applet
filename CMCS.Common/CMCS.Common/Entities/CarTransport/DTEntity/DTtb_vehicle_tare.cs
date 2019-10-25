using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DapperDber.Attrs;

namespace CMCS.Common.Entities.CarTransport.DTEntity
{
    /// <summary>
    /// 车辆信息
    /// </summary>
    [Serializable]
    [CMCS.DapperDber.Attrs.DapperBind("tb_vehicle_tare")]
    public class DTtb_vehicle_tare
    {
        /// <summary>
        /// 单号  主键	与设备序列号做联合主键
        /// </summary>
        [DapperPrimaryKey]
        [DapperIgnoreAttribute]
        public int Id { get; set; }

        /// <summary>
        /// 车牌
        /// </summary>
        public string licenseplate { get; set; }

        /// <summary>
        /// 驾驶员
        /// </summary>
        public string driver { get; set; }

        /// <summary>
        /// 皮重
        /// </summary>
        public float tare { get; set; }

        /// <summary>
        /// 皮重时间
        /// </summary>
        public DateTime taretime { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string descript { get; set; }

        /// <summary>
        /// 是否上传 0:未上传 1:已上传
        /// </summary>
        public int uploadStatus { get; set; }

        /// <summary>
        /// 车厢长度
        /// </summary>
        public float vlength { get; set; }

        /// <summary>
        /// 车厢宽度
        /// </summary>
        public float vwidth { get; set; }

        /// <summary>
        /// 车厢底高
        /// </summary>
        public float vheight { get; set; }

        /// <summary>
        /// 拉筋数量
        /// </summary>
        public float stretchCount { get; set; }

        /// <summary>
        /// 拉筋位置1
        /// </summary>
        public float stretch1 { get; set; }

        /// <summary>
        /// 拉筋位置2
        /// </summary>
        public float stretch2 { get; set; }

        /// <summary>
        /// 拉筋位置3
        /// </summary>
        public float stretch3 { get; set; }

        /// <summary>
        /// 拉筋位置4
        /// </summary>
        public float stretch4 { get; set; }

        /// <summary>
        /// 拉筋位置5
        /// </summary>
        public float stretch5 { get; set; }

    }
}
