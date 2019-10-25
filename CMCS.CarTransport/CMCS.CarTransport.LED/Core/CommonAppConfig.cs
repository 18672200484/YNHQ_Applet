using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CMCS.CarTransport.LED.Core
{
    /// <summary>
    /// 程序配置
    /// </summary>
    public class CommonAppConfig
    {
        private static string ConfigXmlPath = "Common.AppConfig.xml";

        private static CommonAppConfig instance;

        public static CommonAppConfig GetInstance()
        {
            return instance;
        }

        static CommonAppConfig()
        {
            instance = CMCS.Common.Utilities.XOConverter.LoadConfig<CommonAppConfig>(ConfigXmlPath);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            CMCS.Common.Utilities.XOConverter.SaveConfig(instance, ConfigXmlPath);
        }

        private string appIdentifier;
        /// <summary>
        /// 程序唯一标识
        /// </summary>
        [Description("程序唯一标识")]
        public string AppIdentifier
        {
            get { return appIdentifier; }
            set { appIdentifier = value; }
        }

        private string selfConnStr;
        /// <summary>
        /// 集中管控Oracle数据库连接字符串
        /// </summary>
        [Description("集中管控Oracle数据库连接字符串")]
        public string SelfConnStr
        {
            get { return selfConnStr; }
            set { selfConnStr = value; }


        }

        private string startup;
        /// <summary>
        /// 开机启动
        /// </summary>
        [Description("开机启动")]
        public string StartUp
        {
            get { return startup; }
            set { startup = value; }
        }

        private string model;
        /// <summary>
        /// 模式
        /// </summary>
        [Description("模式")]
        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        private string ledIpCYJ;
        /// <summary>
        /// 采样机LED
        /// </summary>
        [Description("采样机LED")]
        public string LedIpCYJ
        {
            get { return ledIpCYJ; }
            set { ledIpCYJ = value; }
        }

        private string ledIp1;
        /// <summary>
        /// LED屏1IP
        /// </summary>
        [Description("LED1IP")]
        public string LedIp1
        {
            get { return ledIp1; }
            set { ledIp1 = value; }
        }

        private string ledIp2;
        /// <summary>
        /// LED屏2IP
        /// </summary>
        [Description("LED2IP")]
        public string LedIp2
        {
            get { return ledIp2; }
            set { ledIp2 = value; }
        }

        private string ledIp3;
        /// <summary>
        /// LED屏3IP
        /// </summary>
        [Description("LED3IP")]
        public string LedIp3
        {
            get { return ledIp3; }
            set { ledIp3 = value; }
        }

        private string ledIp4;
        /// <summary>
        /// LED屏4IP
        /// </summary>
        [Description("LED4IP")]
        public string LedIp4
        {
            get { return ledIp4; }
            set { ledIp4 = value; }
        }

        private string ledIp5;
        /// <summary>
        /// LED屏5IP
        /// </summary>
        [Description("LED5IP")]
        public string LedIp5
        {
            get { return ledIp5; }
            set { ledIp5 = value; }
        }

        private string ledIp6;
        /// <summary>
        /// LED屏6IP
        /// </summary>
        [Description("LED6IP")]
        public string LedIp6
        {
            get { return ledIp6; }
            set { ledIp6 = value; }
        }


        private string cYJLeftContent;
        /// <summary>
        /// 采样机左边内容
        /// </summary>
        [Description("采样机左边内容")]
        public string CYJLeftContent
        {
            get { return cYJLeftContent; }
            set { cYJLeftContent = value; }
        }

        private string cYJRightContent;
        /// <summary>
        /// 采样机右边内容
        /// </summary>
        [Description("采样机右边内容")]
        public string CYJRightContent
        {
            get { return cYJRightContent; }
            set { cYJRightContent = value; }
        }

        private string ledContent1;
        /// <summary>
        /// LED屏1内容
        /// </summary>
        [Description("LED屏1内容")]
        public string LedContent1
        {
            get { return ledContent1; }
            set { ledContent1 = value; }
        }

        private string ledContent2;
        /// <summary>
        /// LED屏2内容
        /// </summary>
        [Description("LED屏2内容")]
        public string LedContent2
        {
            get { return ledContent2; }
            set { ledContent2 = value; }
        }

        private string ledContent3;
        /// <summary>
        /// LED屏3内容
        /// </summary>
        [Description("LED屏3内容")]
        public string LedContent3
        {
            get { return ledContent3; }
            set { ledContent3 = value; }
        }

        private string ledContent4;
        /// <summary>
        /// LED屏4内容
        /// </summary>
        [Description("LED屏4内容")]
        public string LedContent4
        {
            get { return ledContent4; }
            set { ledContent4 = value; }
        }

        private string ledContent5;
        /// <summary>
        /// LED屏5内容
        /// </summary>
        [Description("LED屏5内容")]
        public string LedContent5
        {
            get { return ledContent5; }
            set { ledContent5 = value; }
        }

        private string ledContent6;
        /// <summary>
        /// LED屏6内容
        /// </summary>
        [Description("LED屏6内容")]
        public string LedContent6
        {
            get { return ledContent6; }
            set { ledContent6 = value; }
        }
    }
}
