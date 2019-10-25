using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;

namespace CMCS.Common.Entities.Fuel
{
    /// <summary>
    /// 入厂煤批次表
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("fultbinfactorybatch")]
    public class CmcsInFactoryBatch : EntityBase1
    {
        private String _Batch;
        /// <summary>
        /// 入厂批次号
        /// </summary>
        public String Batch { get { return _Batch; } set { _Batch = value; } }

        private String _InCoalCode;
        /// <summary>
        /// 来煤编码、自编码
        /// </summary>
        public String InCoalCode { get { return _InCoalCode; } set { _InCoalCode = value; } }

        private String _BatchType;
        /// <summary>
        /// 批次类型 汽车、火车、船
        /// </summary>
        public String BatchType { get { return _BatchType; } set { _BatchType = value; } }

        private String _LmybId;
        /// <summary>
        /// 调运计划
        /// </summary>
        public String LmybId { get { return _LmybId; } set { _LmybId = value; } }

        private String _SupplierId;
        /// <summary>
        /// 供煤单位
        /// </summary>
        public String SupplierId { get { return _SupplierId; } set { _SupplierId = value; } }

        private String _SendSupplierId;
        /// <summary>
        /// 发货单位
        /// </summary>
        public String SendSupplierId { get { return _SendSupplierId; } set { _SendSupplierId = value; } }

        private String _StationId;
        /// <summary>
        /// 发站 多对一
        /// </summary>
        public String StationId { get { return _StationId; } set { _StationId = value; } }

        private String _MineId;
        /// <summary>
        /// 矿点 多对一
        /// </summary>
        public String MineId { get { return _MineId; } set { _MineId = value; } }

        private String _FuelKindName;
        /// <summary>
        /// 煤种名称
        /// </summary>
        public String FuelKindName { get { return _FuelKindName; } set { _FuelKindName = value; } }

        private String _FuelKindId;
        /// <summary>
        /// 关联：煤种
        /// </summary>
        public String FuelKindId { get { return _FuelKindId; } set { _FuelKindId = value; } }

        private String _TransportTypeName;
        /// <summary>
        /// 运输方式
        /// </summary>
        public String TransportTypeName { get { return _TransportTypeName; } set { _TransportTypeName = value; } }

        private String _TransportTypeId;
        /// <summary>
        /// 运输方式
        /// </summary>
        public String TransportTypeId { get { return _TransportTypeId; } set { _TransportTypeId = value; } }

        private DateTime _DispatchDate;
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime DispatchDate { get { return _DispatchDate; } set { _DispatchDate = value; } }

        private DateTime _PlanArriveDate;
        /// <summary>
        /// 通知到达时间
        /// </summary>
        public DateTime PlanArriveDate { get { return _PlanArriveDate; } set { _PlanArriveDate = value; } }

        private DateTime _FactArriveDate;
        /// <summary>
        /// 实际到达时间
        /// </summary>
        public DateTime FactArriveDate { get { return _FactArriveDate; } set { _FactArriveDate = value; } }

        private Int32 _TransportNumber;
        /// <summary>
        /// 火车:车皮数.汽车:车辆数
        /// </summary>
        public Int32 TransportNumber { get { return _TransportNumber; } set { _TransportNumber = value; } }

        private Decimal _TicketQty;
        /// <summary>
        /// 矿发量(吨)
        /// </summary>
        public Decimal TicketQty { get { return _TicketQty; } set { _TicketQty = value; } }

        private Decimal _SuttleWeight;
        /// <summary>
        /// 净重(吨)
        /// </summary>
        public Decimal SuttleWeight { get { return _SuttleWeight; } set { _SuttleWeight = value; } }

        private Decimal _KgWeight;
        /// <summary>
        /// 扣矸(吨)
        /// </summary>
        public Decimal KgWeight { get { return _KgWeight; } set { _KgWeight = value; } }

        private Decimal _KsWeight;
        /// <summary>
        /// 扣水(吨)
        /// </summary>
        public Decimal KsWeight { get { return _KsWeight; } set { _KsWeight = value; } }

        private Decimal _CheckQty;
        /// <summary>
        /// 验收量(吨)
        /// </summary>
        public Decimal CheckQty { get { return _CheckQty; } set { _CheckQty = value; } }

        private Decimal _RailLost;
        /// <summary>
        /// 路损(吨)
        /// </summary>
        public Decimal RailLost { get { return _RailLost; } set { _RailLost = value; } }

        private Decimal _MarginQty;
        /// <summary>
        /// 盈亏量(吨)
        /// </summary>
        public Decimal MarginQty { get { return _MarginQty; } set { _MarginQty = value; } }

        private String _Runner;
        /// <summary>
        /// 接车人员
        /// </summary>
        public String Runner { get { return _Runner; } set { _Runner = value; } }

        private DateTime _RunDate;
        /// <summary>
        /// 接车时间
        /// </summary>
        public DateTime RunDate { get { return _RunDate; } set { _RunDate = value; } }

        private String _AuditingUserAccount;
        /// <summary>
        /// 审批人
        /// </summary>
        public String AuditingUserAccount { get { return _AuditingUserAccount; } set { _AuditingUserAccount = value; } }

        private DateTime _AuditingDate;
        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime AuditingDate { get { return _AuditingDate; } set { _AuditingDate = value; } }

        private Int32 _IsCheck;
        /// <summary>
        /// 是否核对铁路大票
        /// </summary>
        public Int32 IsCheck { get { return _IsCheck; } set { _IsCheck = value; } }

        private Int32 _IsQty = 0;
        /// <summary>
        /// 状态 0：初始 1：计量审核结束
        /// </summary>
        public Int32 IsQty { get { return _IsQty; } set { _IsQty = value; } }

        private Int32 _IsAssay = 0;
        /// <summary>
        /// 状态 0：初始 1：质检化验结束
        /// </summary>
        public Int32 IsAssay { get { return _IsAssay; } set { _IsAssay = value; } }

        private Int32 _IsDeposit = 0;
        /// <summary>
        /// 是否堆放完毕
        /// </summary>
        public Int32 IsDeposit { get { return _IsDeposit; } set { _IsDeposit = value; } }

        private Int32 _IsAdjusted = 0;
        /// <summary>
        /// 1,0没生成数据 或 需要更新
        /// </summary>
        public Int32 IsAdjusted { get { return _IsAdjusted; } set { _IsAdjusted = value; } }

        private Int32 _RCHYType_QT = 0;
        /// <summary>
        /// 入厂化验是否直接填写化验结果 0:否、1:是
        /// </summary>
        public Int32 RCHYType_QT { get { return _RCHYType_QT; } set { _RCHYType_QT = value; } }

        private Int32 _RCHYType_BM = 1;
        /// <summary>
        /// 入厂化验是否走采制化三级流程 0:否、1:是
        /// </summary>
        public Int32 RCHYType_BM { get { return _RCHYType_BM; } set { _RCHYType_BM = value; } }

        private Int32 _IsFinish_BM = 0;
        /// <summary>
        /// 采制化三级流程是否结束 0:否、1:是
        /// </summary>
        public Int32 IsFinish_BM { get { return _IsFinish_BM; } set { _IsFinish_BM = value; } }

        private Int32 _IsFinish_QT = 0;
        /// <summary>
        /// 直接填写化验是否结束 0:否、1:是
        /// </summary>
        public Int32 IsFinish_QT { get { return _IsFinish_QT; } set { _IsFinish_QT = value; } }

        private Decimal _QCal;
        /// <summary>
        /// 预估煤质-发热量
        /// </summary>
        public Decimal QCal { get { return _QCal; } set { _QCal = value; } }

        private Decimal _Stad;
        /// <summary>
        /// 预估煤质-硫分
        /// </summary>
        public Decimal Stad { get { return _Stad; } set { _Stad = value; } }

        private Decimal _Vad;
        /// <summary>
        /// 预估煤质-挥发分
        /// </summary>
        public Decimal Vad { get { return _Vad; } set { _Vad = value; } }

        private String _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get { return _Remark; } set { _Remark = value; } }

        private Int32 _IsUpload = 0;
        /// <summary>
        /// 是否上传
        /// </summary>
        public Int32 IsUpload { get { return _IsUpload; } set { _IsUpload = value; } }

        private DateTime _BACKBATCHDATE;
        /// <summary>
        /// 归批时间
        /// </summary>
        public DateTime BACKBATCHDATE { get { return _BACKBATCHDATE; } set { _BACKBATCHDATE = value; } }

        private Int32 _BatchCreateType;
        /// <summary>
        /// 批次创建类型 0 手动录入 1 智能化自动创建
        /// </summary>
        public Int32 BatchCreateType { get { return _BatchCreateType; } set { _BatchCreateType = value; } }

        private String _DataFrom;
        /// <summary>
        /// DataFrom
        /// </summary>
        public String DataFrom { get { return _DataFrom; } set { _DataFrom = value; } }
    }
}
