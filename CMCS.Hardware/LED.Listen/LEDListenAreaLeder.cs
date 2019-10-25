
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LED.Listen;
using LED.Listen.Enums;

namespace LED.Listen
{
    public class LEDListenAreaLeder
    {
        //使用步骤  初始化LED屏  初始化节目   向节目添加内容  发送节目

        #region Vars
        private string ledIp = string.Empty;

        /// <summary>
        /// LED屏IP
        /// </summary>
        public string LedIp
        {
            get { return ledIp; }
            set { ledIp = value; }
        }

        private int ledWidth = 576;

        /// <summary>
        /// LED屏宽度
        /// </summary>
        public int LedWidth
        {
            get { return ledWidth; }
            set { ledWidth = value; }
        }

        private int ledHeight = 96;

        /// <summary>
        /// LED屏高度
        /// </summary>
        public int LedHeight
        {
            get { return ledHeight; }
            set { ledHeight = value; }
        }
        private bool initState = false;

        /// <summary>
        /// 初始化状态
        /// </summary>
        public bool InitState
        {
            get { return initState; }
            set
            {
                initState = value;
                SetStatus(value);
            }
        }
        private bool status = false;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Status
        {
            get { return status; }
        }

        private int hProgram = 0;
        /// <summary>
        /// 节目句柄
        /// </summary>
        public int HProgram
        {
            get
            {
                return hProgram;
            }
            set { hProgram = value; }
        }

        private int areaNo = 0;
        /// <summary>
        /// 区域号
        /// </summary>
        public int AreaNo
        {
            get
            {
                return areaNo;
            }
            set { areaNo = value; }
        }

        /// <summary>
        /// 当前错误信息
        /// </summary>
        public string ErrStr;

        public delegate void ScanErrorEventHandler(Exception error);
        public event ScanErrorEventHandler OnScanError;

        public delegate void StatusChangeHandler(bool status);
        public event StatusChangeHandler OnStatusChange;

        /// <summary>
        /// 定义一通讯参数结构体变量用于对设定的LED通讯，具体对此结构体元素赋值说明见COMMUNICATIONINFO结构体定义部份注示
        /// </summary>
        LedDll.COMMUNICATIONINFO CommunicationInfo = new LedDll.COMMUNICATIONINFO();

        #endregion

        #region 公用

        /// <summary>
        /// 设置连接状态
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(bool status)
        {
            if (this.OnStatusChange != null) this.OnStatusChange(status);
            this.status = status;
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="ex"></param>
        public void ScanError(Exception ex)
        {
            if (this.OnScanError != null) this.OnScanError(ex);
            SetStatus(false);
        }


        /// <summary>
        /// 初始化LED屏（设置屏参）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool Init(string ip)
        {
            this.LedIp = ip;
            int nResult;

            CommunicationInfo.SendType = 0;//设为固定IP通讯模式，即TCP通讯
            CommunicationInfo.IpStr = ip;//给IpStr赋值LED控制卡的IP
            CommunicationInfo.LedNumber = 1;//LED屏号为1，注意socket通讯和232通讯不识别屏号，默认赋1就行了，485必需根据屏的实际屏号进行赋值

            nResult = LedDll.LV_SetBasicInfo(ref CommunicationInfo, 1, this.LedWidth, this.LedHeight);//设置屏参，屏的颜色为2即为双基色，64为屏宽点数，32为屏高点数，具体函数参数说明见函数声明注示
            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                ErrStr = LedDll.LS_GetError(nResult);
            }
            this.InitState = nResult == 0;
            SetStatus(this.InitState);
            return this.InitState;
        }
        /// <summary>
        /// 初始化节目
        /// </summary>
        /// <returns></returns>
        public bool InitProgram()
        {
            int nResult;
            if (hProgram == 0)
                //根据传的参数创建节目句柄，64是屏宽点数，32是屏高点数，2是屏的颜色，注意此处屏宽高及颜色参数必需与设置屏参的屏宽高及颜色一致，否则发送时会提示错误
                hProgram = HProgram = LedDll.LV_CreateProgram(this.LedWidth, this.LedHeight, 1);
            nResult = LedDll.LV_AddProgram(HProgram, 1, 0, 1);//添加一个节目，参数说明见函数声明注示
            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                string ErrStr;
                ErrStr = LedDll.LS_GetError(nResult);
            }
            return nResult == 0;
        }

        /// <summary>
        /// 发送节目内容
        /// </summary>
        /// <param name="isdelprogram">是否删除节目</param>
        /// <returns></returns>
        public bool Send(bool isdelprogram = false)
        {
            int nResult;
            nResult = LedDll.LV_Send(ref CommunicationInfo, HProgram);//发送，见函数声明注示
            if (isdelprogram)
            {
                LedDll.LV_DeleteProgram(HProgram);//删除节目内存对象，详见函数声明注示
                this.HProgram = 0;
            }
            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                ErrStr = LedDll.LS_GetError(nResult);
            }
            return nResult == 0;
        }

        #endregion

        #region 卸煤沟
        /// <summary>
        /// 发送单行文本
        /// </summary>
        /// <param name="row1">文本内容</param>
        /// <param name="area">区域 left right</param>
        /// <param name="isdelprogram">是否删除节目</param>
        /// <returns></returns>
        public bool SendSingleLineTextArea(string row1, string area)
        {
            int nResult;

            LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
            if (area == "left")
            {
                AreaRect.left = 0;
            }
            else
            {
                AreaRect.left = this.LedWidth / 2;
            }
            AreaRect.top = 0;
            AreaRect.width = this.LedWidth / 2;
            AreaRect.height = this.LedHeight;

            LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
            FontProp.FontName = "宋体";
            FontProp.FontSize = 14;
            FontProp.FontColor = LedDll.COLOR_RED;
            FontProp.FontBold = 0;

            nResult = LedDll.LV_QuickAddSingleLineTextArea(HProgram, 1, this.AreaNo, ref AreaRect, LedDll.ADDTYPE_STRING, row1, ref FontProp, 4);//快速通过字符添加一个单行文本区域，函数见函数声明注示

            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                string ErrStr;
                ErrStr = LedDll.LS_GetError(nResult);
            }

            return nResult == 0;
        }

        /// <summary>
        /// 向卸煤沟LED屏发送单行文本
        /// </summary>
        /// <param name="row1"></param>
        /// <returns></returns>
        public bool SendSingleTextAreaUnloadArea(string row1)
        {
            int nResult;

            LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
            AreaRect.left = 0;
            AreaRect.top = 0;
            AreaRect.width = this.LedWidth / 2;
            AreaRect.height = this.LedHeight;

            LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
            FontProp.FontName = "宋体";
            FontProp.FontSize = 14;
            FontProp.FontColor = LedDll.COLOR_RED;
            FontProp.FontBold = 0;

            nResult = LedDll.LV_QuickAddSingleLineTextArea(HProgram, 1, AreaNo, ref AreaRect, LedDll.ADDTYPE_STRING, row1, ref FontProp, 4);//快速通过字符添加一个单行文本区域，函数见函数声明注示

            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                ErrStr = LedDll.LS_GetError(nResult);
            }

            return nResult == 0;
        }

        /// <summary>
        /// 添加静止的文本
        /// </summary>
        /// <param name="content"></param>
        /// <param name="areano"></param>
        /// <returns></returns>
        public bool LV_AddStaticTextToImageTextArea(string content, int areano)
        {
            int nResult;

            LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
            AreaRect.left = 0;
            AreaRect.top = 0;
            AreaRect.width = this.LedWidth / 2;
            AreaRect.height = this.LedHeight;

            LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
            FontProp.FontName = "宋体";
            FontProp.FontSize = 14;
            FontProp.FontColor = LedDll.COLOR_RED;
            FontProp.FontBold = 0;

            nResult = LedDll.LV_AddStaticTextToImageTextArea(HProgram, 1, AreaNo, 0, content, ref FontProp, 65535, 2, 1);//快速通过字符添加一个单行文本区域，函数见函数声明注示

            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                ErrStr = LedDll.LS_GetError(nResult);
            }

            return nResult == 0;
        }

        #endregion

        #region 采样机

        /// <summary>
        /// 向LED屏指定区域发送单行文本
        /// </summary>
        /// <param name="row1">文字</param>
        /// <param name="XValue">X坐标</param>
        /// <param name="YValue">Y坐标</param>
        /// <param name="Width">长度</param>
        /// <param name="Height">宽度</param>
        /// <param name="FontSize">字号</param>
        /// <param name="InStyle">特技</param>
        /// <returns></returns>
        public bool SendSingleTextByArea(string row1, int XValue, int YValue, int Width, int Height, int FontSize, eInitStyle InStyle, int areano)
        {
            int nResult;

            LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
            AreaRect.left = XValue;
            AreaRect.top = YValue;
            AreaRect.width = Width;
            AreaRect.height = Height;

            LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
            FontProp.FontName = "宋体";
            FontProp.FontSize = FontSize;
            FontProp.FontColor = LedDll.COLOR_RED;
            FontProp.FontBold = 0;

            LedDll.PLAYPROP PlayProp = new LedDll.PLAYPROP();
            PlayProp.InStyle = Convert.ToInt32(InStyle);
            PlayProp.DelayTime = 1;
            PlayProp.Speed = 4;
            //可以添加多个子项到图文区，如下添加可以选一个或多个添加
            nResult = LedDll.LV_QuickAddSingleLineTextArea(HProgram, 1, areano, ref AreaRect, LedDll.ADDTYPE_STRING, row1, ref FontProp, 0);

            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                ErrStr = LedDll.LS_GetError(nResult);
            }

            return nResult == 0;
        }

        /// <summary>
        /// 添加边框
        /// </summary>
        /// <param name="XValue">X坐标</param>
        /// <param name="YValue">Y坐标</param>
        /// <param name="Width">长度</param>
        /// <param name="Height">宽度</param>
        /// <returns></returns>
        public bool AddWaterBorder(int XValue, int YValue, int Width, int Height, int areano)
        {
            int nResult;

            LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
            AreaRect.left = XValue;
            AreaRect.top = YValue;
            AreaRect.width = Width;
            AreaRect.height = Height;

            LedDll.WATERBORDERINFO border = new LedDll.WATERBORDERINFO();//流水边框属性结构体
            border.Flag = 0;
            border.BorderType = 0;
            border.BorderValue = 0;
            border.BorderStyle = 0;
            border.BorderSpeed = 0;
            border.BorderColor = 255;

            nResult = LedDll.LV_AddWaterBorder(HProgram, 1, areano, ref AreaRect, ref border);

            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                ErrStr = LedDll.LS_GetError(nResult);
            }
            return nResult == 0;
        }

        /// <summary>
        /// 发送多行文本（自动换行）
        /// </summary>
        /// <param name="content">发送文本内容</param>
        /// <param name="XValue">X坐标</param>
        /// <param name="YValue">Y坐标</param>
        /// <param name="Width">长度</param>
        /// <param name="Height">宽度</param>
        /// <param name="FontSize">字体大小</param>
        /// <param name="InStyle">显示方式</param>
        /// <param name="areano">区域号</param>
        /// <param name="a">水平对齐样式，0.左对齐  1.右对齐  2.水平居中</param>
        /// <returns></returns>
        public bool SendMultiLineTextToImageTextArea(string content, int XValue, int YValue, int Width, int Height, int FontSize, eInitStyle InStyle, int areano, int nAlignment)
        {
            int nResult;
            LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
            AreaRect.left = XValue;
            AreaRect.top = YValue;
            AreaRect.width = Width;
            AreaRect.height = Height;

            LedDll.LV_AddImageTextArea(HProgram, 1, areano, ref AreaRect, 0);

            LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
            FontProp.FontName = "宋体";
            FontProp.FontSize = FontSize;
            FontProp.FontColor = LedDll.COLOR_RED;
            FontProp.FontBold = 0;

            LedDll.PLAYPROP PlayProp = new LedDll.PLAYPROP();
            PlayProp.InStyle = Convert.ToInt32(InStyle);
            PlayProp.DelayTime = 3;
            PlayProp.Speed = 4;
            //可以添加多个子项到图文区，如下添加可以选一个或多个添加

            nResult = LedDll.LV_AddMultiLineTextToImageTextArea(HProgram, 1, areano, LedDll.ADDTYPE_STRING, content, ref FontProp, ref PlayProp, nAlignment, 1);
            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                ErrStr = LedDll.LS_GetError(nResult);
            }
            return nResult == 0;
        }

        #endregion

        public void test()
        {
            int nResult;

            LedDll.AREARECT AreaRect = new LedDll.AREARECT();//区域坐标属性结构体变量
            AreaRect.left = 0;
            AreaRect.top = 0;
            AreaRect.width = 64;
            AreaRect.height = 16;

            LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
            FontProp.FontName = "宋体";
            FontProp.FontSize = 12;
            FontProp.FontColor = LedDll.COLOR_RED;
            FontProp.FontBold = 0;

            nResult = LedDll.LV_QuickAddSingleLineTextArea(hProgram, 1, 1, ref AreaRect, LedDll.ADDTYPE_STRING, "上海灵信视觉技术股份有限公司", ref FontProp, 4);//快速通过字符添加一个单行文本区域，函数见函数声明注示

            AreaRect.left = 64;
            AreaRect.top = 16;
            AreaRect.width = 64;
            AreaRect.height = 16;
            LedDll.DIGITALCLOCKAREAINFO DigitalClockAreaInfo = new LedDll.DIGITALCLOCKAREAINFO();
            DigitalClockAreaInfo.TimeColor = LedDll.COLOR_RED;

            DigitalClockAreaInfo.ShowStrFont.FontName = "宋体";
            DigitalClockAreaInfo.ShowStrFont.FontSize = 12;
            DigitalClockAreaInfo.IsShowHour = 1;
            DigitalClockAreaInfo.IsShowMinute = 1;


            nResult = LedDll.LV_AddDigitalClockArea(hProgram, 1, 2, ref AreaRect, ref DigitalClockAreaInfo);//注意区域号不能一样，详见函数声明注示

            nResult = LedDll.LV_Send(ref CommunicationInfo, hProgram);//发送，见函数声明注示
            LedDll.LV_DeleteProgram(hProgram);//删除节目内存对象，详见函数声明注示
            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                string ErrStr;
                ErrStr = LedDll.LS_GetError(nResult);
            }
        }
    }
}
