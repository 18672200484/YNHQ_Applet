
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LED.Listen;

namespace CMCS.CarTransport.Weighter.Core
{
   public class ListenLED
    {
        private string ledIp = string.Empty;

        /// <summary>
        /// LED屏IP
        /// </summary>
        public string LedIp
        {
            get { return ledIp; }
            set { value=ledIp;  }
        }

        private int ledWidth = 576;

        /// <summary>
        /// LED屏宽度
        /// </summary>
        public int LedWidth
        {
            get { return ledWidth; }
            set { value = ledWidth; }
        }

        private int ledHeight = 96;

        /// <summary>
        /// LED屏高度
        /// </summary>
        public int LedHeight
        {
            get { return ledHeight; }
            set { value = ledHeight; }
        }


        private bool initState = false;

        /// <summary>
        /// 初始化状态
        /// </summary>
        public bool InitState
        {
            get { return initState; }
            set { value=initState;  }
        }

       /// <summary>
       /// 初始化LED屏（设置屏参）
       /// </summary>
       /// <param name="ip"></param>
       /// <returns></returns>
       public bool Init(string ip)
       {
           this.LedIp=ip;
           int nResult;
           LedDll.COMMUNICATIONINFO CommunicationInfo = new LedDll.COMMUNICATIONINFO();//定义一通讯参数结构体变量用于对设定的LED通讯，具体对此结构体元素赋值说明见COMMUNICATIONINFO结构体定义部份注示
           //TCP通讯********************************************************************************
           CommunicationInfo.SendType = 0;//设为固定IP通讯模式，即TCP通讯
           CommunicationInfo.IpStr = ip;//给IpStr赋值LED控制卡的IP
           CommunicationInfo.LedNumber = 1;//LED屏号为1，注意socket通讯和232通讯不识别屏号，默认赋1就行了，485必需根据屏的实际屏号进行赋值
           
           nResult = LedDll.LV_SetBasicInfo(ref CommunicationInfo, 1, this.LedWidth, this.LedHeight);//设置屏参，屏的颜色为2即为双基色，64为屏宽点数，32为屏高点数，具体函数参数说明见函数声明注示
           if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
           {
               string ErrStr;
               ErrStr = LedDll.LS_GetError(nResult);
           }
           this.InitState=nResult==0;
          return this.InitState;
       }

       /// <summary>
       /// 发送单行文本
       /// </summary>
       /// <param name="row1">文本内容</param>
       /// <param name="area">区域 left right</param>
       /// <returns></returns>
       public bool SendSingleLineTextArea(string row1,string area)
       {
           int nResult;
           LedDll.COMMUNICATIONINFO CommunicationInfo = new LedDll.COMMUNICATIONINFO();//定义一通讯参数结构体变量用于对设定的LED通讯，具体对此结构体元素赋值说明见COMMUNICATIONINFO结构体定义部份注示
           
           CommunicationInfo.SendType = 0;//设为固定IP通讯模式，即TCP通讯
           CommunicationInfo.IpStr = this.LedIp;//给IpStr赋值LED控制卡的IP
           CommunicationInfo.LedNumber = 1;//LED屏号为1，注意socket通讯和232通讯不识别屏号，默认赋1就行了，485必需根据屏的实际屏号进行赋值
           
           int hProgram;//节目句柄
           hProgram = LedDll.LV_CreateProgram(this.LedWidth, this.LedHeight, 1);//根据传的参数创建节目句柄，64是屏宽点数，32是屏高点数，2是屏的颜色，注意此处屏宽高及颜色参数必需与设置屏参的屏宽高及颜色一致，否则发送时会提示错误
           //此处可自行判断有未创建成功，hProgram返回NULL失败，非NULL成功,一般不会失败

           nResult = LedDll.LV_AddProgram(hProgram, 1, 0, 1);//添加一个节目，参数说明见函数声明注示
           if (nResult != 0)
           {
               string ErrStr;
               ErrStr = LedDll.LS_GetError(nResult);
             
           }
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
           AreaRect.width = this.LedWidth/2;
           AreaRect.height = this.LedHeight;

           LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
           FontProp.FontName = "宋体";
           FontProp.FontSize = 14;
           FontProp.FontColor = LedDll.COLOR_RED;
           FontProp.FontBold = 0;
           //int nsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(LedDll.FONTPROP));

           nResult = LedDll.LV_QuickAddSingleLineTextArea(hProgram, 1, 1, ref AreaRect, LedDll.ADDTYPE_STRING, row1, ref FontProp, 4);//快速通过字符添加一个单行文本区域，函数见函数声明注示
           
           nResult = LedDll.LV_Send(ref CommunicationInfo, hProgram);//发送，见函数声明注示
           LedDll.LV_DeleteProgram(hProgram);//删除节目内存对象，详见函数声明注示
           if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
           {
               string ErrStr;
               ErrStr = LedDll.LS_GetError(nResult);
           }
           
           return nResult == 0;
       }

       /// <summary>
       /// 发送多行文本（自动换行）
       /// </summary>
       /// <param name="content">文本内容</param>
       /// <param name="area">区域 left right</param>
       /// <returns></returns>
       public bool SendMultiLineTextToImageTextArea(string content,string area)
       {
           int nResult;
           LedDll.COMMUNICATIONINFO CommunicationInfo = new LedDll.COMMUNICATIONINFO();//定义一通讯参数结构体变量用于对设定的LED通讯，具体对此结构体元素赋值说明见COMMUNICATIONINFO结构体定义部份注示
           //TCP通讯********************************************************************************
           CommunicationInfo.SendType = 0;//设为固定IP通讯模式，即TCP通讯
           CommunicationInfo.IpStr = this.ledIp;//给IpStr赋值LED控制卡的IP
           CommunicationInfo.LedNumber = 1;//LED屏号为1，注意socket通讯和232通讯不识别屏号，默认赋1就行了，485必需根据屏的实际屏号进行赋值
           
           int hProgram;//节目句柄
           hProgram = LedDll.LV_CreateProgram(this.LedWidth, this.LedHeight, 1);//根据传的参数创建节目句柄，64是屏宽点数，32是屏高点数，2是屏的颜色，注意此处屏宽高及颜色参数必需与设置屏参的屏宽高及颜色一致，否则发送时会提示错误
           //此处可自行判断有未创建成功，hProgram返回NULL失败，非NULL成功,一般不会失败

           nResult = LedDll.LV_AddProgram(hProgram, 1, 0, 1);//添加一个节目，参数说明见函数声明注示
           if (nResult != 0)
           {
               string ErrStr;
               ErrStr = LedDll.LS_GetError(nResult);
           }
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

           LedDll.LV_AddImageTextArea(hProgram, 1, 1, ref AreaRect, 0);

           LedDll.FONTPROP FontProp = new LedDll.FONTPROP();//文字属性
           FontProp.FontName = "宋体";
           FontProp.FontSize = 14;
           FontProp.FontColor = LedDll.COLOR_RED;
           FontProp.FontBold = 0;

           LedDll.PLAYPROP PlayProp = new LedDll.PLAYPROP();
           PlayProp.InStyle = 0;
           PlayProp.DelayTime = 3;
           PlayProp.Speed = 4;
           //可以添加多个子项到图文区，如下添加可以选一个或多个添加
         
           nResult = LedDll.LV_AddMultiLineTextToImageTextArea(hProgram, 1, 1, LedDll.ADDTYPE_STRING, content, ref FontProp, ref PlayProp, 0, 0);
           
           nResult = LedDll.LV_Send(ref CommunicationInfo, hProgram);//发送，见函数声明注示
           LedDll.LV_DeleteProgram(hProgram);//删除节目内存对象，详见函数声明注示
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
           LedDll.COMMUNICATIONINFO CommunicationInfo = new LedDll.COMMUNICATIONINFO();//定义一通讯参数结构体变量用于对设定的LED通讯，具体对此结构体元素赋值说明见COMMUNICATIONINFO结构体定义部份注示

           CommunicationInfo.SendType = 0;//设为固定IP通讯模式，即TCP通讯
           CommunicationInfo.IpStr = this.LedIp;//给IpStr赋值LED控制卡的IP
           CommunicationInfo.LedNumber = 1;//LED屏号为1，注意socket通讯和232通讯不识别屏号，默认赋1就行了，485必需根据屏的实际屏号进行赋值

           int hProgram;//节目句柄
           hProgram = LedDll.LV_CreateProgram(this.LedWidth, this.LedHeight, 1);//根据传的参数创建节目句柄，64是屏宽点数，32是屏高点数，2是屏的颜色，注意此处屏宽高及颜色参数必需与设置屏参的屏宽高及颜色一致，否则发送时会提示错误
           //此处可自行判断有未创建成功，hProgram返回NULL失败，非NULL成功,一般不会失败

           nResult = LedDll.LV_AddProgram(hProgram, 1, 0, 1);//添加一个节目，参数说明见函数声明注示
           if (nResult != 0)
           {
               string ErrStr;
               ErrStr = LedDll.LS_GetError(nResult);

           }
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
           //int nsize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(LedDll.FONTPROP));

           nResult = LedDll.LV_QuickAddSingleLineTextArea(hProgram, 1, 1, ref AreaRect, LedDll.ADDTYPE_STRING, row1, ref FontProp, 4);//快速通过字符添加一个单行文本区域，函数见函数声明注示

           nResult = LedDll.LV_Send(ref CommunicationInfo, hProgram);//发送，见函数声明注示
           LedDll.LV_DeleteProgram(hProgram);//删除节目内存对象，详见函数声明注示
           if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
           {
               string ErrStr;
               ErrStr = LedDll.LS_GetError(nResult);
           }

           return nResult == 0;
       }
    }
}
