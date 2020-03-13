using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.CarTransport.Weighter.Core;
using System.Threading;
using System.IO;
using CMCS.CarTransport.DAO;
using CMCS.Common.DAO;
using System.IO.Ports;
using CMCS.Common.Utilities;
using CMCS.CarTransport.Weighter.Enums;
using CMCS.Common.Entities;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common;
using CMCS.CarTransport.Weighter.Frms.Sys;
using DevComponents.DotNetBar.Controls;
using CMCS.Common.Views;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.Common.Enums;
using CMCS.Common.Entities.Sys;
using HikVisionSDK.Core;
using CMCS.Common.Entities.BaseInfo;
using System.Net.Sockets;
using LED.YIBO;
using CMCS.CarTransport.Weighter.Frms.Transport.Print;
using LED.Listen.Enums;
using LED.Listen;

namespace CMCS.CarTransport.Weighter.Frms
{
	public partial class FrmWeighter : DevComponents.DotNetBar.Metro.MetroForm
	{
		/// <summary>
		/// 窗体唯一标识符
		/// </summary>
		public static string UniqueKey = "FrmWeighter";

		public FrmWeighter()
		{
			InitializeComponent();
		}

		#region Vars

		CarTransportDAO carTransportDAO = CarTransportDAO.GetInstance();
		WeighterDAO weighterDAO = WeighterDAO.GetInstance();
		CommonDAO commonDAO = CommonDAO.GetInstance();

		/// <summary>
		/// 等待上传的抓拍
		/// </summary>
		Queue<string> waitForUpload = new Queue<string>();

		IocControler iocControler;
		/// <summary>
		/// 语音播报
		/// </summary>
		VoiceSpeaker voiceSpeaker = new VoiceSpeaker();

		bool inductorCoil1 = false;
		/// <summary>
		/// 地感1状态 true=有信号  false=无信号
		/// </summary>
		public bool InductorCoil1
		{
			get
			{
				return inductorCoil1;
			}
			set
			{
				inductorCoil1 = value;

				panCurrentWeight.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感1信号.ToString(), value ? "1" : "0");
			}
		}

		int inductorCoil1Port;
		/// <summary>
		/// 地感1端口
		/// </summary>
		public int InductorCoil1Port
		{
			get { return inductorCoil1Port; }
			set { inductorCoil1Port = value; }
		}

		bool inductorCoil2 = false;
		/// <summary>
		/// 地感2状态 true=有信号  false=无信号
		/// </summary>
		public bool InductorCoil2
		{
			get
			{
				return inductorCoil2;
			}
			set
			{
				inductorCoil2 = value;

				panCurrentWeight.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地感2信号.ToString(), value ? "1" : "0");
			}
		}

		int inductorCoil2Port;
		/// <summary>
		/// 地感2端口
		/// </summary>
		public int InductorCoil2Port
		{
			get { return inductorCoil2Port; }
			set { inductorCoil2Port = value; }
		}

		int infraredSensor1Port;
		/// <summary>
		/// 对射1端口
		/// </summary>
		public int InfraredSensor1Port
		{
			get { return infraredSensor1Port; }
			set { infraredSensor1Port = value; }
		}

		bool useInfraredSensor1 = false;
		/// <summary>
		/// 是否启用对射1
		/// </summary>
		public bool UseInfraredSensor1
		{
			get
			{
				return useInfraredSensor1;
			}
			set
			{
				useInfraredSensor1 = value;
			}
		}

		bool infraredSensor1 = false;
		/// <summary>
		/// 对射1状态 true=遮挡  false=连通
		/// </summary>
		public bool InfraredSensor1
		{
			get
			{
				return infraredSensor1;
			}
			set
			{
				infraredSensor1 = value;

				panCurrentWeight.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.对射1信号.ToString(), value ? "1" : "0");
			}
		}

		int infraredSensor2Port;
		/// <summary>
		/// 对射2端口
		/// </summary>
		public int InfraredSensor2Port
		{
			get { return infraredSensor2Port; }
			set { infraredSensor2Port = value; }
		}

		bool useInfraredSensor2 = false;
		/// <summary>
		/// 是否启用对射2
		/// </summary>
		public bool UseInfraredSensor2
		{
			get
			{
				return useInfraredSensor2;
			}
			set
			{
				useInfraredSensor2 = value;
			}
		}

		bool infraredSensor2 = false;
		/// <summary>
		/// 对射2状态 true=遮挡  false=连通
		/// </summary>
		public bool InfraredSensor2
		{
			get
			{
				return infraredSensor2;
			}
			set
			{
				infraredSensor2 = value;

				panCurrentWeight.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.对射2信号.ToString(), value ? "1" : "0");
			}
		}

		int infraredSensor3Port;
		/// <summary>
		/// 对射3端口
		/// </summary>
		public int InfraredSensor3Port
		{
			get { return infraredSensor3Port; }
			set { infraredSensor3Port = value; }
		}

		bool useInfraredSensor3 = false;
		/// <summary>
		/// 是否启用对射3
		/// </summary>
		public bool UseInfraredSensor3
		{
			get
			{
				return useInfraredSensor3;
			}
			set
			{
				useInfraredSensor3 = value;
			}
		}

		bool infraredSensor3 = false;
		/// <summary>
		/// 对射3状态 true=遮挡  false=连通
		/// </summary>
		public bool InfraredSensor3
		{
			get
			{
				return infraredSensor3;
			}
			set
			{
				infraredSensor3 = value;

				panCurrentWeight.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.对射3信号.ToString(), value ? "1" : "0");
			}
		}

		bool wbSteady = false;
		/// <summary>
		/// 地磅仪表稳定状态
		/// </summary>
		public bool WbSteady
		{
			get { return wbSteady; }
			set
			{
				wbSteady = value;

				this.panCurrentWeight.Style.ForeColor.Color = (value ? Color.Lime : Color.Red);

				panCurrentWeight.Refresh();

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_稳定.ToString(), value ? "1" : "0");
			}
		}

		double wbMinWeight = 0;
		/// <summary>
		/// 地磅仪表最小称重 单位:吨
		/// </summary>
		public double WbMinWeight
		{
			get { return wbMinWeight; }
			set
			{
				wbMinWeight = value;
			}
		}

		bool autoHandMode = true;
		/// <summary>
		/// 自动模式=true  手动模式=false
		/// </summary>
		public bool AutoHandMode
		{
			get { return autoHandMode; }
			set
			{
				autoHandMode = value;

				btnSelectAutotruck_BuyFuel.Visible = !value;
				btnSelectAutotruck_Goods.Visible = !value;

				btnSaveTransport_BuyFuel.Visible = !value;
				btnSaveTransport_Goods.Visible = !value;

				btnReset_BuyFuel.Visible = !value;
				btnReset_Goods.Visible = !value;
			}
		}

		bool ledIsSend = false;
		/// <summary>
		/// LED信息是否已发送
		/// </summary>
		public bool LedIsSend
		{
			get { return ledIsSend; }
			set { ledIsSend = value; }
		}

		public static PassCarQueuer passCarQueuer = new PassCarQueuer();

		ImperfectCar currentImperfectCar;
		/// <summary>
		/// 识别或选择的车辆凭证
		/// </summary>
		public ImperfectCar CurrentImperfectCar
		{
			get { return currentImperfectCar; }
			set
			{
				currentImperfectCar = value;

				if (value != null)
					panCurrentCarNumber.Text = value.Voucher;
				else
					panCurrentCarNumber.Text = "等待车辆";
			}
		}

		eFlowFlag currentFlowFlag = eFlowFlag.等待车辆;
		/// <summary>
		/// 当前业务流程标识
		/// </summary>
		public eFlowFlag CurrentFlowFlag
		{
			get { return currentFlowFlag; }
			set
			{
				currentFlowFlag = value;

				lblFlowFlag.Text = value.ToString();
			}
		}

		CmcsAutotruck currentAutotruck;
		/// <summary>
		/// 当前车
		/// </summary>
		public CmcsAutotruck CurrentAutotruck
		{
			get { return currentAutotruck; }
			set
			{
				currentAutotruck = value;

				if (value != null)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), value.Id);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), value.CarNumber);
				}
				else
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), string.Empty);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), string.Empty);
				}
			}
		}


		CmcsAdvance currentAdvance;
		/// <summary>
		/// 当前预制信息
		/// </summary>
		public CmcsAdvance CurrentAdvance
		{
			get { return currentAdvance; }
			set
			{
				currentAdvance = value;

				if (value != null)
				{
					this.CurrentAutotruck = carTransportDAO.GetAutotruckByCarNumber(value.CarNumber);
					if (value.CarType == eCarType.入厂煤.ToString())
					{
						txtTagId_BuyFuel.Text = value.Tag;
						txtCarNumber_BuyFuel.Text = value.CarNumber;
						superTabControl2.SelectedTab = superTabItem_BuyFuel;
						this.timer_BuyFuel_Cancel = false;
						this.CurrentFlowFlag = eFlowFlag.验证信息;
					}

					else if (value.CarType == eCarType.其他物资.ToString())
					{
						txtTagId_Goods.Text = value.Tag;
						txtCarNumber_Goods.Text = value.CarNumber;
						superTabControl2.SelectedTab = superTabItem_Goods;
						this.timer_Goods_Cancel = false;
						this.CurrentFlowFlag = eFlowFlag.验证信息;
					}

					panCurrentCarNumber.Text = value.CarNumber;
				}
				else
				{
					txtCarNumber_BuyFuel.ResetText();
					txtCarNumber_Goods.ResetText();

					txtTagId_BuyFuel.ResetText();
					txtTagId_Goods.ResetText();

					panCurrentCarNumber.ResetText();
				}
			}
		}

		string rfId = "";
		/// <summary>
		/// 当前标签卡号
		/// </summary>
		public string RfId
		{
			get { return rfId; }
			set
			{
				rfId = value;
				if (string.IsNullOrEmpty(value))
					FrmDebugConsole.GetInstance().Output("当前标签号重置");
			}
		}

		bool weightCheck = false;
		/// <summary>
		/// 称重验证
		/// </summary>
		public bool WeightCheck
		{
			get { return weightCheck; }
			set { weightCheck = value; }
		}
		#endregion

		/// <summary>
		/// 窗体初始化
		/// </summary>
		private void InitForm()
		{
#if DEBUG
			lblFlowFlag.Visible = true;
			//FrmDebugConsole.GetInstance().Show();
#else
            lblFlowFlag.Visible = false;
            FrmDebugConsole.GetInstance().Show();
#endif

			// 默认自动
			sbtnChangeAutoHandMode.Value = true;
		}

		private void FrmWeighter_Load(object sender, EventArgs e)
		{
		}

		private void FrmWeighter_Shown(object sender, EventArgs e)
		{
			InitHardware();

			InitForm();
		}

		private void FrmWeighter_FormClosing(object sender, FormClosingEventArgs e)
		{
			// 卸载设备
			UnloadHardware();
		}

		#region 设备相关

		#region IO控制器

		void Iocer_StatusChange(bool status)
		{
			// 接收设备状态 
			InvokeEx(() =>
			{
				slightIOC.LightColor = (status ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.IO控制器_连接状态.ToString(), status ? "1" : "0");
			});
		}

		/// <summary>
		/// IO控制器接收数据时触发
		/// </summary>
		/// <param name="receiveValue"></param>
		void Iocer_Received(int[] receiveValue)
		{
			// 接收地感状态  
			InvokeEx(() =>
			{
				//地感未接入
				//this.InductorCoil1 = (receiveValue[this.InductorCoil1Port - 1] == 1);
				//this.InductorCoil2 = (receiveValue[this.InductorCoil2Port - 1] == 1);
				if (this.UseInfraredSensor1)
					this.InfraredSensor1 = (receiveValue[this.InfraredSensor1Port - 1] == 1);
				if (this.UseInfraredSensor2)
					this.InfraredSensor2 = (receiveValue[this.InfraredSensor2Port - 1] == 1);
			});
		}

		/// <summary>
		/// 前方升杆
		/// </summary>
		void FrontGateUp()
		{
			if (this.CurrentImperfectCar == null) return;

			if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
			{
				this.iocControler.Gate2Up();
				this.iocControler.GreenLight2();
			}
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
			{
				this.iocControler.Gate1Up();
				this.iocControler.GreenLight1();
			}
		}

		/// <summary>
		/// 前方降杆
		/// </summary>
		void FrontGateDown()
		{
			if (this.CurrentImperfectCar == null) return;

			if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
			{
				this.iocControler.Gate2Down();
				this.iocControler.RedLight2();
			}
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
			{
				this.iocControler.Gate1Down();
				this.iocControler.RedLight1();
			}
		}

		/// <summary>
		/// 后方升杆
		/// </summary>
		void BackGateUp()
		{
			if (this.CurrentImperfectCar == null) return;

			if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
			{
				this.iocControler.Gate1Up();
				this.iocControler.GreenLight1();
			}
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
			{
				this.iocControler.Gate2Up();
				this.iocControler.GreenLight2();
			}
		}

		/// <summary>
		/// 后方降杆
		/// </summary>
		void BackGateDown()
		{
			if (this.CurrentImperfectCar == null) return;

			if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
			{
				this.iocControler.Gate1Down();
				this.iocControler.RedLight1();
			}
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
			{
				this.iocControler.Gate2Down();
				this.iocControler.RedLight2();
			}
		}

		#endregion

		#region 读卡器
		/// <summary>
		/// 读卡器1读卡成功
		/// </summary>
		/// <param name="rfid"></param>
		void Rwer1_OnReadSuccess(string rfid)
		{
			InvokeEx(() =>
			{
				if (string.IsNullOrEmpty(rfid))
				{
					FrmDebugConsole.GetInstance().Output("读卡器1读卡成功卡号为空");
					Log4Neter.Info("读卡器1读卡成功卡号为空");
					return;
				}
				if (this.CurrentFlowFlag == eFlowFlag.等待车辆)
				{
					this.RfId = rfid;
					passCarQueuer.Enqueue(eDirection.Way1, rfid);
					this.CurrentImperfectCar = passCarQueuer.Dequeue();
					this.CurrentAdvance = carTransportDAO.GetAdvanceByTagId(this.CurrentImperfectCar.Voucher);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.上磅方向.ToString(), "1");
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前卡号.ToString(), this.RfId);
					FrmDebugConsole.GetInstance().Output("读卡器1读卡成功，卡号：" + this.RfId + "车号：" + this.CurrentAdvance.CarNumber);
				}
				else if (this.CurrentFlowFlag == eFlowFlag.称重验证)
				{
					if (rfid == this.RfId)
					{
						WeightCheck = true;
						FrmDebugConsole.GetInstance().Output("读卡器1验证成功，车号：" + this.CurrentAdvance.CarNumber);
					}
					else
					{
						WeightCheck = false;
						this.voiceSpeaker.Speak("两次刷卡信息不一样", 1, false);
					}
					////不需要验证 直接保存
					//WeightCheck = true;

				}
			});
		}

		void Error1(string error)
		{
			Log4Neter.Info(string.Format("读卡器1:{0}", error));
		}

		void Rwer1_OnStatusChange(bool status)
		{
			// 接收设备状态 
			InvokeEx(() =>
			{
				slightRwer1.LightColor = (status ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.读卡器1_连接状态.ToString(), status ? "1" : "0");
			});
		}

		/// <summary>
		/// 读卡器2读卡成功
		/// </summary>
		/// <param name="rfid"></param>
		void Rwer2_OnReadSuccess(string rfid)
		{
			InvokeEx(() =>
			{
				if (string.IsNullOrEmpty(rfid))
				{
					FrmDebugConsole.GetInstance().Output("读卡器2读卡成功卡号为空");
					Log4Neter.Info("读卡器2读卡成功卡号为空");
					return;
				}
				if (this.CurrentFlowFlag == eFlowFlag.等待车辆)
				{
					this.RfId = rfid;
					passCarQueuer.Enqueue(eDirection.Way2, rfid);
					this.CurrentImperfectCar = passCarQueuer.Dequeue();
					this.CurrentAdvance = carTransportDAO.GetAdvanceByTagId(this.CurrentImperfectCar.Voucher);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.上磅方向.ToString(), "2");
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前卡号.ToString(), this.RfId);
					FrmDebugConsole.GetInstance().Output("读卡器2读卡成功，卡号：" + this.RfId + "车号：" + this.CurrentAdvance.CarNumber);
				}
				else if (this.CurrentFlowFlag == eFlowFlag.称重验证)
				{
					if (rfid == this.RfId)
					{
						WeightCheck = true;
						FrmDebugConsole.GetInstance().Output("读卡器2验证成功，车号：" + this.CurrentAdvance.CarNumber);
					}
					else
					{
						WeightCheck = false;
						this.voiceSpeaker.Speak("两次刷卡信息不一样", 1, false);
						FrmDebugConsole.GetInstance().Output("读卡器2验证失败，卡号：" + rfid);
					}
					//不需要验证 直接保存
					//WeightCheck = true;

				}
			});
		}

		void Error2(string error)
		{
			Log4Neter.Info(string.Format("读卡器2:{0}", error));
		}

		void Rwer2_OnStatusChange(bool status)
		{
			// 接收设备状态 
			InvokeEx(() =>
			{
				slightRwer2.LightColor = (status ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.读卡器2_连接状态.ToString(), status ? "1" : "0");
			});
		}

		#endregion

		#region LED显示屏

		#region LED1控制卡

		void LED_OnStatusChange(bool status)
		{
			// 接收设备状态 
			InvokeEx(() =>
			{
				slightLED1.LightColor = (status ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.LED屏1_连接状态.ToString(), status ? "1" : "0");
			});
		}

		/// <summary>
		/// LED1上一次显示内容
		/// </summary>
		string LED1PrevLedFileContent = string.Empty;
		bool LED1Status = false;
		/// <summary>
		/// 更新LED1动态区域（重量与车号）
		/// </summary>
		/// <param name="value1">第一行内容</param>
		/// <param name="value2">第二行内容</param>
		private void UpdateLedShow(string value1 = "", string value2 = "")
		{
			if (this.LED1PrevLedFileContent == value1 + value2 || !LED1Status) return;
			this.LED1PrevLedFileContent = value1 + value2;
			Hardwarer.Led.Send(value1, value2);
			FrmDebugConsole.GetInstance().Output("更新LED第一行内容：" + value1 + "第二行内容" + value2);
		}

		#endregion

		#endregion

		#region 地磅仪表

		/// <summary>
		/// 重量稳定事件
		/// </summary>
		/// <param name="steady"></param>
		void Wber_OnSteadyChange(bool steady)
		{
			InvokeEx(() =>
			  {
				  this.WbSteady = steady;
			  });
		}

		/// <summary>
		/// 地磅仪表状态变化
		/// </summary>
		/// <param name="status"></param>
		void Wber_OnStatusChange(bool status)
		{
			// 接收设备状态 
			InvokeEx(() =>
			{
				slightWber.LightColor = (status ? Color.Green : Color.Red);

				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_连接状态.ToString(), status ? "1" : "0");
			});
		}

		void Wber_OnWeightChange(double weight)
		{
			InvokeEx(() =>
			{
				panCurrentWeight.Text = weight.ToString();
			});
		}

		#endregion

		#region 海康视频

		/// <summary>
		/// 海康网络摄像机
		/// </summary>
		IPCer iPCer1 = new IPCer();
		IPCer iPCer2 = new IPCer();
		IPCer iPCer3 = new IPCer();
		/// <summary>
		/// 执行摄像头抓拍，并保存数据
		/// </summary>
		/// <param name="transportId">运输记录Id</param>
		private void CamareCapturePicture(string transportId)
		{
			try
			{
				// 抓拍照片服务器发布地址
				string pictureWebUrl = commonDAO.GetCommonAppletConfigString("汽车智能化_抓拍照片发布路径");

				// 摄像机1
				string picture1FileName = Path.Combine(SelfVars.CapturePicturePath, Guid.NewGuid().ToString() + ".bmp");
				if (iPCer1.CapturePicture(picture1FileName))
				{
					CmcsTransportPicture transportPicture = new CmcsTransportPicture()
					{
						CaptureTime = DateTime.Now,
						CaptureType = CommonAppConfig.GetInstance().AppIdentifier,
						TransportId = transportId,
						PicturePath = pictureWebUrl + Path.GetFileName(picture1FileName)
					};

					if (commonDAO.SelfDber.Insert(transportPicture) > 0) waitForUpload.Enqueue(picture1FileName);
				}

				// 摄像机2
				string picture2FileName = Path.Combine(SelfVars.CapturePicturePath, "Camera", Guid.NewGuid().ToString() + ".bmp");
				if (iPCer2.CapturePicture(picture2FileName))
				{
					CmcsTransportPicture transportPicture = new CmcsTransportPicture()
					{
						CaptureTime = DateTime.Now,
						CaptureType = CommonAppConfig.GetInstance().AppIdentifier,
						TransportId = transportId,
						PicturePath = pictureWebUrl + Path.GetFileName(picture1FileName)
					};

					if (commonDAO.SelfDber.Insert(transportPicture) > 0) waitForUpload.Enqueue(picture2FileName);
				}
				// 摄像机3
				string picture3FileName = Path.Combine(SelfVars.CapturePicturePath, "Camera", Guid.NewGuid().ToString() + ".bmp");
				if (iPCer2.CapturePicture(picture3FileName))
				{
					CmcsTransportPicture transportPicture = new CmcsTransportPicture()
					{
						CaptureTime = DateTime.Now,
						CaptureType = CommonAppConfig.GetInstance().AppIdentifier,
						TransportId = transportId,
						PicturePath = pictureWebUrl + Path.GetFileName(picture1FileName)
					};

					if (commonDAO.SelfDber.Insert(transportPicture) > 0) waitForUpload.Enqueue(picture3FileName);
				}
			}
			catch (Exception ex)
			{
				Log4Neter.Error("摄像机抓拍", ex);
			}
		}

		/// <summary>
		/// 上传抓拍照片到服务器共享文件夹
		/// </summary>
		private void UploadCapturePicture()
		{
			string serverPath = commonDAO.GetCommonAppletConfigString("汽车智能化_抓拍照片服务器共享路径");
			if (string.IsNullOrEmpty(serverPath)) return;

			string fileName = string.Empty;
			do
			{
				fileName = waitForUpload != null && waitForUpload.Count > 0 ? this.waitForUpload.Dequeue() : null;
				if (!string.IsNullOrEmpty(fileName))
				{
					try
					{
						if (File.Exists(serverPath)) File.Copy(fileName, Path.Combine(serverPath, Path.GetFileName(fileName)), true);
					}
					catch (Exception ex)
					{
						Log4Neter.Error("上传抓拍照片", ex);

						break;
					}
				}

			} while (fileName != null);
		}

		/// <summary>
		/// 清理过期的抓拍照片
		/// </summary> 
		public void ClearExpireCapturePicture()
		{
			foreach (string item in Directory.GetFiles(SelfVars.CapturePicturePath).Where(a =>
			{
				return new FileInfo(a).LastWriteTime < DateTime.Now.AddMonths(-10);
			}))
			{
				try
				{
					File.Delete(item);
				}
				catch { }
			}
		}

		#endregion

		#region 设备初始化与卸载

		/// <summary>
		/// 初始化外接设备
		/// </summary>
		private void InitHardware()
		{
			try
			{
				bool success = false;

				this.InductorCoil1Port = commonDAO.GetAppletConfigInt32("IO控制器_地感1端口");
				this.InductorCoil2Port = commonDAO.GetAppletConfigInt32("IO控制器_地感2端口");
				this.InfraredSensor1Port = commonDAO.GetAppletConfigInt32("IO控制器_对射1端口");
				this.UseInfraredSensor1 = commonDAO.GetAppletConfigString("IO控制器_对射1启用") == "1";
				this.InfraredSensor2Port = commonDAO.GetAppletConfigInt32("IO控制器_对射2端口");
				this.UseInfraredSensor2 = commonDAO.GetAppletConfigString("IO控制器_对射2启用") == "1";

				this.WbMinWeight = commonDAO.GetAppletConfigDouble("地磅仪表_最小称重");

				// IO控制器
				Hardwarer.Iocer.OnReceived += new IOC.PCI1761.PCI1761Iocer.ReceivedEventHandler(Iocer_Received);
				Hardwarer.Iocer.OnStatusChange += new IOC.PCI1761.PCI1761Iocer.StatusChangeHandler(Iocer_StatusChange);
				success = Hardwarer.Iocer.OpenCom();
				if (!success) MessageBoxEx.Show("IO控制器连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				this.iocControler = new IocControler(Hardwarer.Iocer);

				// 地磅仪表
				Hardwarer.Wber.OnStatusChange += new WB.TOLEDO.YAOHUA.TOLEDO_YAOHUAWber.StatusChangeHandler(Wber_OnStatusChange);
				Hardwarer.Wber.OnSteadyChange += new WB.TOLEDO.YAOHUA.TOLEDO_YAOHUAWber.SteadyChangeEventHandler(Wber_OnSteadyChange);
				Hardwarer.Wber.OnWeightChange += new WB.TOLEDO.YAOHUA.TOLEDO_YAOHUAWber.WeightChangeEventHandler(Wber_OnWeightChange);
				success = Hardwarer.Wber.OpenCom(commonDAO.GetAppletConfigInt32("地磅仪表_串口"), commonDAO.GetAppletConfigInt32("地磅仪表_波特率"), commonDAO.GetAppletConfigInt32("地磅仪表_数据位"), commonDAO.GetAppletConfigInt32("地磅仪表_停止位"));

				TaskSimpleScheduler taskSimpleScheduler = new TaskSimpleScheduler();
				taskSimpleScheduler.StartNewTask("读卡器1", () =>
				{
					// 读卡器1
					Hardwarer.Rwer1.OnStatusChange += new RW.Hawkvor.HawkvorRwer.StatusChangeHandler(Rwer1_OnStatusChange);
					Hardwarer.Rwer1.OnReadSucess += new RW.Hawkvor.HawkvorRwer.ReadSucessHandler(Rwer1_OnReadSuccess);
					Socket listener1 = Hardwarer.Rwer1.CreateListening(commonDAO.GetAppletConfigString("读卡器1_Ip"), commonDAO.GetAppletConfigInt32("读卡器1_端口"));
					if (listener1 != null) Hardwarer.Rwer1.StartListening(listener1, Error1);
					else MessageBoxEx.Show("读卡器1连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}, 0, OutputError);
				taskSimpleScheduler.StartNewTask("读卡器2", () =>
				{
					// 读卡器2
					Hardwarer.Rwer2.OnStatusChange += new RW.Hawkvor.HawkvorRwer.StatusChangeHandler(Rwer2_OnStatusChange);
					Hardwarer.Rwer2.OnReadSucess += new RW.Hawkvor.HawkvorRwer.ReadSucessHandler(Rwer2_OnReadSuccess);
					Socket listener2 = Hardwarer.Rwer2.CreateListening(commonDAO.GetAppletConfigString("读卡器2_Ip"), commonDAO.GetAppletConfigInt32("读卡器2_端口"));
					if (listener2 != null) Hardwarer.Rwer2.StartListening(listener2, Error2);
					else MessageBoxEx.Show("读卡器2连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				}, 0, OutputError);
				#region 海康视频

				IPCer.InitSDK();
				string ip = commonDAO.GetAppletConfigString("刻录机IP");
				string username = commonDAO.GetAppletConfigString("刻录机用户名");
				string pwd = commonDAO.GetAppletConfigString("刻录机密码");
				int port = commonDAO.GetAppletConfigInt32("刻录机端口号");
				int channel1 = commonDAO.GetAppletConfigInt32("刻录机通道号1");
				int channel2 = commonDAO.GetAppletConfigInt32("刻录机通道号2");
				int channel3 = commonDAO.GetAppletConfigInt32("刻录机通道号3");

				if (iPCer1.Login(ip, port, username, pwd))
					//if (iPCer1.Login("192.168.1.180", 8000, "admin", "Bossien1234"))
					iPCer1.StartPreview(panVideo1.Handle, 1);

				if (iPCer2.Login(ip, port, username, pwd))
					iPCer2.StartPreview(panVideo2.Handle, channel2);

				if (iPCer3.Login(ip, port, username, pwd))
					iPCer3.StartPreview(panVideo3.Handle, channel3);

				//CmcsCamare video1 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "摄像机1" });
				//if (video1 != null)
				//{
				//    if (iPCer1.Login(video1.Ip, video1.Port, video1.UserName, video1.Password))
				//        iPCer1.StartPreview(panVideo1.Handle, video1.Channel);
				//}

				//CmcsCamare video2 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "摄像机2" });
				//if (video2 != null)
				//{
				//    if (iPCer2.Login(video2.Ip, video2.Port, video2.UserName, video2.Password))
				//        iPCer2.StartPreview(panVideo2.Handle, video2.Channel);
				//}

				//CmcsCamare video3 = commonDAO.SelfDber.Entity<CmcsCamare>("where EquipmentCode=:EquipmentCode", new { EquipmentCode = "摄像机3" });
				//if (video3 != null)
				//{
				//    if (iPCer2.Login(video3.Ip, video3.Port, video3.UserName, video3.Password))
				//        iPCer2.StartPreview(panVideo3.Handle, video3.Channel);
				//}

				#endregion

				#region LED控制卡1
				bool useLed = commonDAO.GetAppletConfigString("启用LED显示屏") == "1";
				string ledSocketIP = commonDAO.GetAppletConfigString("LED显示屏_IP地址");
				int ledSocketCom = commonDAO.GetAppletConfigInt32("LED显示屏_端口");
				if (!string.IsNullOrEmpty(ledSocketIP) && useLed)
				{
					Hardwarer.Led.OnStatusChange += new YiBoDD251.StatusChangeHandler(LED_OnStatusChange);
					success = Hardwarer.Led.CreateListening(ledSocketIP, ledSocketCom);

					if (success)
					{
						LED1Status = true;
						UpdateLedShow("等待上磅", "");
					}
					else
					{
						MessageBoxEx.Show("LED1控制卡连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}

				#endregion


				timer1.Enabled = true;
			}
			catch (Exception ex)
			{
				Log4Neter.Error("设备初始化", ex);
			}
		}
		void OutputError(string text, Exception ex)
		{
			Log4Neter.Error(text, ex);
		}
		/// <summary>
		/// 卸载设备
		/// </summary>
		private void UnloadHardware()
		{
			// 注意此段代码
			Application.DoEvents();

			try
			{
				Hardwarer.Iocer.OnReceived -= new IOC.PCI1761.PCI1761Iocer.ReceivedEventHandler(Iocer_Received);
				Hardwarer.Iocer.OnStatusChange -= new IOC.PCI1761.PCI1761Iocer.StatusChangeHandler(Iocer_StatusChange);

				Hardwarer.Iocer.CloseCom();
			}
			catch { }
			try
			{
				Hardwarer.Rwer1.Close();
			}
			catch { }
			try
			{
				Hardwarer.Rwer2.Close();
			}
			catch { }
			try
			{
				Hardwarer.Led.Close();
			}
			catch { }

			try
			{
				IPCer.CleanupSDK();
			}
			catch { }
		}

		#endregion

		#endregion

		#region 道闸控制按钮

		/// <summary>
		/// 道闸1升杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate1Up_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate1Up();
		}

		/// <summary>
		/// 道闸1降杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate1Down_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate1Down();
		}

		/// <summary>
		/// 道闸2升杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate2Up_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate2Up();
		}

		/// <summary>
		/// 道闸2降杆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGate2Down_Click(object sender, EventArgs e)
		{
			if (this.iocControler != null) this.iocControler.Gate2Down();
		}

		#endregion

		#region 公共业务

		/// <summary>
		/// 慢速任务
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer2_Tick(object sender, EventArgs e)
		{
			timer2.Stop();
			// 三分钟执行一次
			timer2.Interval = 180000;

			try
			{
				// 入厂煤
				LoadTodayBuyFuelTransport();
				// 其他物资
				LoadTodayGoodsTransport();
				// 上传抓拍照片
				UploadCapturePicture();
				// 清理抓拍照片
				if (DateTime.Now.Hour == 12) ClearExpireCapturePicture();
			}
			catch (Exception ex)
			{
				Log4Neter.Error("timer2_Tick", ex);
			}
			finally
			{
				timer2.Start();
			}
		}

		/// <summary>
		/// 有车辆在上磅的道路上
		/// </summary>
		/// <returns></returns>
		bool HasCarOnEnterWay()
		{
			if (this.CurrentImperfectCar == null) return false;

			if (this.CurrentImperfectCar.PassWay == eDirection.UnKnow)
				return false;
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
				return this.InductorCoil1 || this.InfraredSensor1;
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
				return this.InductorCoil2 || this.InfraredSensor2;

			return true;
		}

		/// <summary>
		/// 有车辆在下磅的道路上
		/// </summary>
		/// <returns></returns>
		bool HasCarOnLeaveWay()
		{
			if (this.CurrentImperfectCar == null) return false;

			if (this.CurrentImperfectCar.PassWay == eDirection.UnKnow)
				return false;
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way1)
				return this.InductorCoil2 || this.InfraredSensor2;
			else if (this.CurrentImperfectCar.PassWay == eDirection.Way2)
				return this.InductorCoil1 || this.InfraredSensor1;

			return true;
		}

		/// <summary>
		/// 切换手动/自动模式
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void sbtnChangeAutoHandMode_ValueChanged(object sender, EventArgs e)
		{
			this.AutoHandMode = sbtnChangeAutoHandMode.Value;
		}

		#endregion

		#region 入厂煤业务

		bool timer_BuyFuel_Cancel = true;

		CmcsBuyFuelTransport currentBuyFuelTransport;
		/// <summary>
		/// 当前运输记录
		/// </summary>
		public CmcsBuyFuelTransport CurrentBuyFuelTransport
		{
			get { return currentBuyFuelTransport; }
			set
			{
				currentBuyFuelTransport = value;

				if (value != null)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), value.Id);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.矿点.ToString(), value.MineName);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤区域.ToString(), value.UnloadArea);

					txtFuelKindName_BuyFuel.Text = value.FuelKindName;
					txtMineName_BuyFuel.Text = value.MineName;
					txtSupplierName_BuyFuel.Text = value.SupplierName;
					txtUnLoadArea_BuyFuel.Text = value.UnloadArea;
					txtUnLoadType_BuyFuel.Text = value.UnloadType;
					txtGrossWeight_BuyFuel.Text = value.GrossWeight.ToString("F2");
					txtTicketWeight_BuyFuel.Text = value.TicketWeight.ToString("F2");
					txtTareWeight_BuyFuel.Text = value.TareWeight.ToString("F2");
					txtDeductWeight_BuyFuel.Text = value.DeductWeight.ToString("F2");
					txtKzWeight_BuyFuel.Text = (value.KsWeight + value.KgWeight).ToString("F2");
					txtKsWeight_BuyFuel.Text = (value.AutoKsWeight + value.KsWeight).ToString("F2");
					txtKgWeight_BuyFuel.Text = value.KgWeight.ToString("F2");
					txtCheckWeight_BuyFuel.Text = value.CheckWeight.ToString("F2");
					txtProfitAndLoss_BuyFuel.Text = value.ProfitAndLossWeight.ToString("F2");
				}
				else
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), string.Empty);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.矿点.ToString(), string.Empty);
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.卸煤区域.ToString(), string.Empty);

					txtFuelKindName_BuyFuel.ResetText();
					txtMineName_BuyFuel.ResetText();
					txtSupplierName_BuyFuel.ResetText();
					txtUnLoadArea_BuyFuel.ResetText();
					txtUnLoadType_BuyFuel.ResetText();
					txtGrossWeight_BuyFuel.ResetText();
					txtTicketWeight_BuyFuel.ResetText();
					txtTareWeight_BuyFuel.ResetText();
					txtDeductWeight_BuyFuel.ResetText();
					txtKzWeight_BuyFuel.ResetText();
					txtKsWeight_BuyFuel.ResetText();
					txtKgWeight_BuyFuel.ResetText();
					txtCheckWeight_BuyFuel.ResetText();
					txtProfitAndLoss_BuyFuel.ResetText();
				}
			}
		}

		/// <summary>
		/// 选择车辆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSelectAutotruck_BuyFuel_Click(object sender, EventArgs e)
		{
			FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select(" order by CreateDate desc");
			if (frm.ShowDialog() == DialogResult.OK)
			{
				if (this.InfraredSensor1)
					passCarQueuer.Enqueue(eDirection.Way1, frm.Output.CarNumber);
				else if (this.InfraredSensor2)
					passCarQueuer.Enqueue(eDirection.Way2, frm.Output.CarNumber);
				else
					passCarQueuer.Enqueue(eDirection.UnKnow, frm.Output.CarNumber);

				this.CurrentImperfectCar = passCarQueuer.Dequeue();
				this.CurrentAdvance = carTransportDAO.GetAdvanceByCarNumber(this.CurrentImperfectCar.Voucher);
				this.CurrentFlowFlag = eFlowFlag.验证信息;
			}
		}

		/// <summary>
		/// 保存入厂煤运输记录
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSaveTransport_BuyFuel_Click(object sender, EventArgs e)
		{
			if (!SaveBuyFuelTransport()) MessageBoxEx.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// 保存运输记录
		/// </summary>
		/// <returns></returns>
		bool SaveBuyFuelTransport()
		{
			if (this.CurrentBuyFuelTransport == null) return false;

			try
			{
				if (this.CurrentBuyFuelTransport.GrossWeight > 0 && (Math.Abs(this.CurrentBuyFuelTransport.GrossWeight - (decimal)Hardwarer.Wber.Weight)) < 10)
				{
					this.voiceSpeaker.Speak("重量已存在", 1, false);
					return false;
				}
				if (weighterDAO.SaveBuyFuelTransport(this.CurrentBuyFuelTransport.Id, (decimal)Hardwarer.Wber.Weight, DateTime.Now, CommonAppConfig.GetInstance().AppIdentifier))
				{
					this.CurrentBuyFuelTransport = commonDAO.SelfDber.Get<CmcsBuyFuelTransport>(this.CurrentBuyFuelTransport.Id);//刷新

					this.CurrentFlowFlag = eFlowFlag.等待离开;
					LedIsSend = false;
					//UpdateLedShow(this.CurrentBuyFuelTransport.CarNumber, this.CurrentBuyFuelTransport.SuttleWeight.ToString());
					UpdateLedShow("称重完成", "请下磅");
					this.voiceSpeaker.Speak("过磅完毕请下磅", 1, false);
					// 入厂煤
					LoadTodayBuyFuelTransport();

					CamareCapturePicture(this.CurrentBuyFuelTransport.Id);

					FrontGateUp();

					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

				Log4Neter.Error("保存运输记录", ex);
			}

			return false;
		}

		/// <summary>
		/// 重置入厂煤运输记录
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReset_BuyFuel_Click(object sender, EventArgs e)
		{
			ResetBuyFuel();
		}

		/// <summary>
		/// 重置信息
		/// </summary>
		void ResetBuyFuel()
		{
			//定时器 全局变量
			this.timer_BuyFuel_Cancel = true;
			this.WeightCheck = false;
			this.CurrentFlowFlag = eFlowFlag.等待车辆;
			this.CurrentAutotruck = null;
			this.CurrentAdvance = null;
			this.CurrentBuyFuelTransport = null;
			this.RfId = "";
			txtTagId_BuyFuel.ResetText();
			//FrontGateDown();
			//BackGateDown();

			// 最后重置
			this.CurrentImperfectCar = null;

			commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车Id.ToString(), "");
			commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前车号.ToString(), "");
			commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.矿点.ToString(), "");
			commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前卡号.ToString(), "");
			commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_称重重量.ToString(), "");

			this.LedIsSend = false;

			UpdateLedShow("等待车辆", "");
			SendLED(true);
		}

		/// <summary>
		/// 入厂煤运输记录业务定时器
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_BuyFuel_Tick(object sender, EventArgs e)
		{
			if (this.timer_BuyFuel_Cancel)
				return;

			timer_BuyFuel.Stop();
			timer_BuyFuel.Interval = 2000;

			try
			{
				UpdateLedShow("车号:" + this.CurrentAutotruck.CarNumber, "重量:" + Hardwarer.Wber.Weight.ToString("#0.######"));
				switch (this.CurrentFlowFlag)
				{
					case eFlowFlag.验证信息:
						#region
						try
						{
							if (weighterDAO.CheckRfid(this.RfId))
							{
								this.voiceSpeaker.Speak("当前卡号正在另一汽车衡使用", 1, false);
								FrmDebugConsole.GetInstance().Output("当前卡号正在另一汽车衡使用");
								//ResetBuyFuel();
								this.CurrentFlowFlag = eFlowFlag.等待离开;
								break;
							}
						}
						catch (Exception ex)
						{
							Log4Neter.Error("验证卡号", ex);
						}

						if (this.CurrentAutotruck != null && this.CurrentAdvance != null)
						{
							if (this.CurrentAutotruck.IsUse == 1)
							{
								this.CurrentBuyFuelTransport = commonDAO.SelfDber.Get<CmcsBuyFuelTransport>(this.CurrentAdvance.TransportId);
								if (this.CurrentBuyFuelTransport != null)
								{
									if (this.CurrentBuyFuelTransport.IsUse == 1)
									{
										if (this.CurrentBuyFuelTransport.SuttleWeight == 0)
										{
											this.CurrentFlowFlag = eFlowFlag.等待上磅;
											LedIsSend = false;
											BackGateUp();

											UpdateLedShow("车号:" + this.CurrentAutotruck.CarNumber, "请上磅");
											this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 请上磅", 1, false);
										}
										else
										{
											UpdateLedShow("车号:" + this.CurrentAutotruck.CarNumber, "已过磅");
											this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已过磅", 1, false);

											timer_BuyFuel.Interval = 20000;
										}
									}
									else
									{
										UpdateLedShow("车号:" + this.CurrentAutotruck.CarNumber, "无效运输记录");
										this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 无效的运输记录", 1, false);
										timer1.Interval = 20000;
									}
								}
								else
								{
									UpdateLedShow("车号:" + this.CurrentAutotruck.CarNumber, "未排队");
									this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 未找到排队记录", 1, false);

									timer_BuyFuel.Interval = 20000;
								}
							}
							else
							{
								UpdateLedShow("车号:" + this.CurrentAutotruck.CarNumber, "已停用");
								this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已停用，禁止通过", 1, false);

								timer1.Interval = 20000;
							}
						}
						else
						{
							this.voiceSpeaker.Speak("车辆未登记，禁止通过", 1, false);
						}
						#endregion
						break;

					case eFlowFlag.等待上磅:
						#region

						// 当地磅仪表重量大于最小称重且来车方向的对射无信号，则判定车已经完全上磅
						if (Hardwarer.Wber.Weight >= this.WbMinWeight && !HasCarOnEnterWay())
						{
							//BackGateDown();

							this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 请再次刷卡", 1, false);
							this.CurrentFlowFlag = eFlowFlag.称重验证;
						}
						// 降低灵敏度
						timer_BuyFuel.Interval = 4000;
						SendLED();
						#endregion
						break;
					case eFlowFlag.称重验证:
						if (WeightCheck)
						{
							this.CurrentFlowFlag = eFlowFlag.等待稳定;
							this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 开始过磅", 1, false);
						}

						break;
					case eFlowFlag.等待稳定:
						#region
						LedIsSend = false;
						// 提高灵敏度
						timer_BuyFuel.Interval = 1000;

						//btnSaveTransport_BuyFuel.Enabled = this.WbSteady;
						if (this.WbSteady)
						{
							commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_称重重量.ToString(), Hardwarer.Wber.Weight.ToString());
							if (this.AutoHandMode)
							{
								// 自动模式
								if (!SaveBuyFuelTransport())
								{
									UpdateLedShow("车号:" + this.CurrentAutotruck.CarNumber, "过磅失败");
									this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 过磅失败，请联系管理员", 1, false);
								}
							}
							else
							{
								// 手动模式 
							}
							//SendLED();
						}
						#endregion
						break;

					case eFlowFlag.等待离开:
						#region

						SendLED();
						// 当前地磅重量小于最小称重且所有地感、对射无信号时重置
						if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnLeaveWay()) ResetBuyFuel();

						// 降低灵敏度
						timer_BuyFuel.Interval = 1000;

						#endregion
						break;
				}
				commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.地磅仪表_实时重量.ToString(), Hardwarer.Wber.Weight.ToString());
				// 当前地磅重量小于最小称重且所有地感、对射无信号时重置
				//if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnEnterWay() && !HasCarOnLeaveWay() && this.CurrentFlowFlag != eFlowFlag.等待上磅
				//    && this.CurrentImperfectCar != null) ResetBuyFuel();
			}
			catch (Exception ex)
			{
				Log4Neter.Error("timer_BuyFuel_Tick", ex);
			}
			finally
			{
				timer_BuyFuel.Start();
			}
		}

		/// <summary>
		/// 加载运输记录
		/// </summary>
		void LoadTodayBuyFuelTransport()
		{
			try
			{
				//未完成运输记录
				IList<View_BuyFuelTransport> UnFinishlist = carTransportDAO.GetUnFinishBuyFuelTransport();
				superGridControl1_BuyFuel.PrimaryGrid.DataSource = UnFinishlist;
				//指定日期已完成的入厂煤记录
				IList<View_BuyFuelTransport> Finishlist = carTransportDAO.GetFinishedBuyFuelTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
				superGridControl2_BuyFuel.PrimaryGrid.DataSource = Finishlist;

				labNumber_BuyFuel.Text = string.Format("已登记:{0}  已称重:{1}  已回皮:{2}  未回皮:{3}", UnFinishlist.Count + Finishlist.Count(), UnFinishlist.Where(a => a.GrossWeight > 0).Count() + Finishlist.Count(), Finishlist.Where(a => a.GrossWeight > 0 && a.TareWeight > 0).Count(), UnFinishlist.Where(a => a.GrossWeight > 0 && a.TareWeight == 0).Count());
			}
			catch (Exception ex)
			{
				Log4Neter.Error("加载运输记录", ex);
			}
		}

		/// 打印磅单
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsmiPrint_Click(object sender, EventArgs e)
		{
			GridRow gridRow = superGridControl2_BuyFuel.PrimaryGrid.ActiveRow as GridRow;
			if (gridRow == null) return;
			CmcsBuyFuelTransport entity = Dbers.GetInstance().SelfDber.Get<CmcsBuyFuelTransport>(superGridControl2_BuyFuel.PrimaryGrid.GetCell(gridRow.Index, superGridControl2_BuyFuel.PrimaryGrid.Columns["clmId"].ColumnIndex).Value.ToString());
			//CmcsBuyFuelTransport entity = gridRow.DataItem as CmcsBuyFuelTransport;
			FrmPrintWeb frm = new FrmPrintWeb(entity, null, eCarType.入厂煤);
			frm.ShowDialog();
		}

		/// <summary>
		/// 发送采样机LED
		/// <param name="isDefault">是否默认</param>
		/// </summary>
		/// <returns></returns>
		public bool SendLED(bool isDefault = false)
		{
			bool success = false;
			try
			{
				if (LedIsSend) return false;
				if (!CommonAppConfig.GetInstance().AppIdentifier.Contains("1") && !CommonAppConfig.GetInstance().AppIdentifier.Contains("2")) return success;

				success = Hardwarer.LedListenCYJ.Init(commonDAO.GetAppletConfigString("采样机LED显示屏_IP地址"));
				if (!success) Log4Neter.Info("采样机LED连接失败:" + Hardwarer.LedListenCYJ.ErrStr);

				string AppIdentifierLeft = "汽车智能化-#2过衡端";
				string AppIdentifierRight = "汽车智能化-#1过衡端";

				string leftCarNumber = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.当前车号.ToString());
				string leftMineName = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.矿点.ToString());
				string leftWeight = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.地磅仪表_称重重量.ToString());
				string leftUnloadArea = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.卸煤区域.ToString());

				string rightCarNumber = commonDAO.GetSignalDataValue(AppIdentifierRight, eSignalDataName.当前车号.ToString());
				string rightMineName = commonDAO.GetSignalDataValue(AppIdentifierRight, eSignalDataName.矿点.ToString());
				string rightWeight = commonDAO.GetSignalDataValue(AppIdentifierRight, eSignalDataName.地磅仪表_称重重量.ToString());
				string rightUnloadArea = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.卸煤区域.ToString());

				string oneContentLeft = string.Empty;
				string twoContentLeft = string.Empty;
				string threeContentLeft = string.Empty;

				string oneContentRight = string.Empty;
				string twoContentRight = string.Empty;
				string threeContentRight = string.Empty;
				if (!isDefault)
				{
					oneContentLeft = string.Format("{0}({1})", leftCarNumber, leftWeight);//车号 重量
					twoContentLeft = string.Format("{0}", leftMineName);//矿点
					threeContentLeft = string.Format("{0}", leftUnloadArea);//卸煤区域

					oneContentRight = string.Format("{0}({1})", rightCarNumber, rightWeight);
					twoContentRight = string.Format("{0}", rightMineName);
					threeContentRight = string.Format("{0}", rightUnloadArea);
				}
				else
				{
					string ledLeftContent = commonDAO.GetCommonAppletConfigString("采样机LED显示屏_左内容");
					string ledRightContent = commonDAO.GetCommonAppletConfigString("采样机LED显示屏_右内容");
					string[] ledLeftContents = ledLeftContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					if (ledLeftContents.Length >= 1)
						oneContentLeft = ledLeftContents[0];
					if (ledLeftContents.Length >= 2)
						twoContentLeft = ledLeftContents[1];
					if (ledLeftContents.Length >= 3)
						threeContentLeft = ledLeftContents[2];

					string[] ledRightContents = ledRightContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);
					if (ledRightContents.Length >= 1)
						oneContentRight = ledLeftContents[0];
					if (ledRightContents.Length >= 2)
						twoContentRight = ledLeftContents[1];
					if (ledRightContents.Length >= 3)
						threeContentRight = ledLeftContents[2];
				}
				success = Hardwarer.LedListenCYJ.InitProgram();
				if (!success) Log4Neter.Info("采样机节目初始化失败:" + Hardwarer.LedListenCYJ.ErrStr);
				////二号衡边框
				//success = Hardwarer.LedListenCYJ.AddWaterBorder(0, 0, 288, 32, 1);
				//if (!success) Log4Neter.Info("二号衡边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				////一号衡边框
				//success = Hardwarer.LedListenCYJ.AddWaterBorder(288, 0, 288, 32, 2);
				//if (!success) Log4Neter.Info("一号衡边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);

				//右文字边框
				success = Hardwarer.LedListenCYJ.AddWaterBorder(0, 0, 288, 96, 1);
				//左文字边框
				success = Hardwarer.LedListenCYJ.AddWaterBorder(288, 0, 288, 96, 2);

				//左第一行
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(oneContentLeft, 1, 1, 286, 31, 22, eInitStyle.立即显示, 3, 2);
				//左第二行
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(twoContentLeft, 1, 33, 286, 31, 22, eInitStyle.立即显示, 4, 2);
				//左第三行
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(threeContentLeft, 1, 64, 286, 31, 22, eInitStyle.立即显示, 5, 2);

				//右第一行
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(oneContentRight, 289, 1, 286, 31, 22, eInitStyle.立即显示, 6, 2);
				//右第二行
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(twoContentRight, 289, 33, 286, 31, 22, eInitStyle.立即显示, 7, 2);
				//右第三行
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(threeContentRight, 289, 64, 286, 31, 22, eInitStyle.立即显示, 8, 2);

				success = Hardwarer.LedListenCYJ.Send(true);
				if (!success) Log4Neter.Info("采样机节目发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				LedIsSend = true;
			}
			catch (Exception ex)
			{
				Log4Neter.Error("LED大屏", ex);
			}
			return success;
		}

		public bool SendLED_Old()
		{
			bool success = false;
			try
			{
				if (LedIsSend) return false;
				if (!CommonAppConfig.GetInstance().AppIdentifier.Contains("1") && !CommonAppConfig.GetInstance().AppIdentifier.Contains("2")) return success;

				success = Hardwarer.LedListenCYJ.Init(commonDAO.GetAppletConfigString("采样机LED显示屏_IP地址"));
				if (!success) Log4Neter.Info("采样机连接失败:" + Hardwarer.LedListenCYJ.ErrStr);

				string AppIdentifierLeft = "汽车智能化-#2过衡端";
				string AppIdentifierRight = "汽车智能化-#1过衡端";

				string leftCarNumber = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.当前车号.ToString());
				string leftMineName = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.矿点.ToString());
				string leftWeight = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.地磅仪表_称重重量.ToString());
				string leftUnloadArea = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.卸煤区域.ToString());

				string rightCarNumber = commonDAO.GetSignalDataValue(AppIdentifierRight, eSignalDataName.当前车号.ToString());
				string rightMineName = commonDAO.GetSignalDataValue(AppIdentifierRight, eSignalDataName.矿点.ToString());
				string rightWeight = commonDAO.GetSignalDataValue(AppIdentifierRight, eSignalDataName.地磅仪表_称重重量.ToString());
				string rightUnloadArea = commonDAO.GetSignalDataValue(AppIdentifierLeft, eSignalDataName.卸煤区域.ToString());

				string sendleftcontent = string.Format("{0}(重量:{1}) {2}({3}卸煤)", leftCarNumber, leftWeight, leftMineName, leftUnloadArea);
				string sendrightcontent = string.Format("{0}(重量:{1}) {2}({3}卸煤)", rightCarNumber, rightWeight, rightMineName, rightUnloadArea);

				if (leftCarNumber.Length < 2) sendleftcontent = "前后刷卡 停车熄火";
				if (rightCarNumber.Length < 2) sendrightcontent = "前后刷卡 停车熄火";

				success = Hardwarer.LedListenCYJ.InitProgram();
				if (!success) Log4Neter.Info("采样机节目初始化失败:" + Hardwarer.LedListenCYJ.ErrStr);
				//二号衡边框
				success = Hardwarer.LedListenCYJ.AddWaterBorder(0, 0, 288, 32, 1);
				if (!success) Log4Neter.Info("二号衡边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				//一号衡边框
				success = Hardwarer.LedListenCYJ.AddWaterBorder(288, 0, 288, 32, 2);
				if (!success) Log4Neter.Info("一号衡边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				//二号衡文字边框
				success = Hardwarer.LedListenCYJ.AddWaterBorder(0, 32, 288, 64, 3);
				if (!success) Log4Neter.Info("二号衡文字边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				//一号衡文字边框
				success = Hardwarer.LedListenCYJ.AddWaterBorder(288, 32, 288, 64, 4);
				if (!success) Log4Neter.Info("一号衡文字边框发送失败:" + Hardwarer.LedListenCYJ.ErrStr);

				//二号衡标题
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea("二号汽车衡", 1, 1, 286, 30, 18, eInitStyle.立即显示, 5, 2);
				if (!success) Log4Neter.Info("二号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);

				//一号衡标题
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea("一号汽车衡", 289, 1, 286, 30, 18, eInitStyle.立即显示, 6, 2);
				if (!success) Log4Neter.Info("一号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);

				//二号衡内容
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(sendleftcontent, 1, 33, 286, 62, 22, eInitStyle.连续左移, 7, 0);
				if (!success) Log4Neter.Info("二号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				//一号衡内容
				success = Hardwarer.LedListenCYJ.SendMultiLineTextToImageTextArea(sendrightcontent, 289, 33, 286, 62, 22, eInitStyle.连续左移, 8, 0);
				if (!success) Log4Neter.Info("一号衡标题发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				success = Hardwarer.LedListenCYJ.Send(true);
				if (!success) Log4Neter.Info("采样机节目发送失败:" + Hardwarer.LedListenCYJ.ErrStr);
				LedIsSend = true;
			}
			catch (Exception ex)
			{
				Log4Neter.Error("LED大屏", ex);
			}
			return success;
		}
		#endregion

		#region 其他物资业务

		bool timer_Goods_Cancel = true;

		CmcsGoodsTransport currentGoodsTransport;
		/// <summary>
		/// 当前运输记录
		/// </summary>
		public CmcsGoodsTransport CurrentGoodsTransport
		{
			get { return currentGoodsTransport; }
			set
			{
				currentGoodsTransport = value;

				if (value != null)
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), value.Id);

					txtSupplyUnitName_Goods.Text = value.SupplyUnitName;
					txtReceiveUnitName_Goods.Text = value.ReceiveUnitName;
					txtGoodsTypeName_Goods.Text = value.GoodsTypeName;

					txtFirstWeight_Goods.Text = value.FirstWeight.ToString("F2");
					txtSecondWeight_Goods.Text = value.SecondWeight.ToString("F2");
					txtSuttleWeight_Goods.Text = value.SuttleWeight.ToString("F2");
				}
				else
				{
					commonDAO.SetSignalDataValue(CommonAppConfig.GetInstance().AppIdentifier, eSignalDataName.当前运输记录Id.ToString(), string.Empty);

					txtSupplyUnitName_Goods.ResetText();
					txtReceiveUnitName_Goods.ResetText();
					txtGoodsTypeName_Goods.ResetText();

					txtFirstWeight_Goods.ResetText();
					txtSecondWeight_Goods.ResetText();
					txtSuttleWeight_Goods.ResetText();
				}
			}
		}

		/// <summary>
		/// 选择车辆
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSelectAutotruck_Goods_Click(object sender, EventArgs e)
		{
			FrmUnFinishTransport_Select frm = new FrmUnFinishTransport_Select("where CarType='" + eCarType.其他物资.ToString() + "' order by CreateDate desc");
			if (frm.ShowDialog() == DialogResult.OK)
			{
				if (this.InfraredSensor1)
					passCarQueuer.Enqueue(eDirection.Way1, frm.Output.CarNumber);
				else if (this.InfraredSensor2)
					passCarQueuer.Enqueue(eDirection.Way2, frm.Output.CarNumber);
				else
					passCarQueuer.Enqueue(eDirection.UnKnow, frm.Output.CarNumber);

				this.CurrentImperfectCar = passCarQueuer.Dequeue();
				this.CurrentAdvance = carTransportDAO.GetAdvanceByCarNumber(this.CurrentImperfectCar.Voucher);
				this.CurrentFlowFlag = eFlowFlag.验证信息;
			}
		}

		/// <summary>
		/// 保存排队记录
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSaveTransport_Goods_Click(object sender, EventArgs e)
		{
			if (!SaveGoodsTransport()) MessageBoxEx.Show("保存失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		/// <summary>
		/// 保存运输记录
		/// </summary>
		/// <returns></returns>
		bool SaveGoodsTransport()
		{
			if (this.CurrentGoodsTransport == null) return false;

			try
			{
				if (weighterDAO.SaveGoodsTransport(this.CurrentGoodsTransport.Id, (decimal)Hardwarer.Wber.Weight, DateTime.Now, CommonAppConfig.GetInstance().AppIdentifier))
				{
					this.CurrentGoodsTransport = commonDAO.SelfDber.Get<CmcsGoodsTransport>(this.CurrentGoodsTransport.Id);

					FrontGateUp();

					btnSaveTransport_Goods.Enabled = false;
					this.CurrentFlowFlag = eFlowFlag.等待离开;

					//UpdateLedShow("称重完毕", "请下磅");
					this.voiceSpeaker.Speak("称重完毕请下磅", 1, false);

					LoadTodayGoodsTransport();

					CamareCapturePicture(this.CurrentGoodsTransport.Id);

					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBoxEx.Show("保存失败\r\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

				Log4Neter.Error("保存运输记录", ex);
			}

			return false;
		}

		/// <summary>
		/// 重置信息
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReset_Goods_Click(object sender, EventArgs e)
		{
			ResetGoods();
		}

		/// <summary>
		/// 重置信息
		/// </summary>
		void ResetGoods()
		{
			this.timer_Goods_Cancel = true;

			this.CurrentFlowFlag = eFlowFlag.等待车辆;

			this.CurrentAutotruck = null;
			this.CurrentGoodsTransport = null;

			txtTagId_Goods.ResetText();

			btnSaveTransport_Goods.Enabled = false;

			//FrontGateDown();
			//BackGateDown();

			UpdateLedShow("  等待车辆");

			// 最后重置
			this.CurrentImperfectCar = null;
		}


		/// <summary>
		/// 加载运输记录
		/// </summary>
		void LoadTodayGoodsTransport()
		{
			superGridControl1_Goods.PrimaryGrid.DataSource = null;
			superGridControl2_Goods.PrimaryGrid.DataSource = null;
			//未完成运输记录
			IList<CmcsGoodsTransport> UnFinishlist = carTransportDAO.GetUnFinishGoodsTransport();
			superGridControl1_Goods.PrimaryGrid.DataSource = UnFinishlist;
			//指定日期已完成的入厂煤记录
			IList<CmcsGoodsTransport> Finishlist = carTransportDAO.GetFinishedGoodsTransport(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
			superGridControl2_Goods.PrimaryGrid.DataSource = Finishlist;

			labNumber_Goods.Text = string.Format("已登记:{0}  已称重:{1}  已回皮:{2}  未回皮:{3}", UnFinishlist.Count + UnFinishlist.Count(), UnFinishlist.Where(a => a.FirstWeight > 0).Count(), Finishlist.Where(a => a.FirstWeight > 0 && a.SecondWeight > 0).Count(), UnFinishlist.Where(a => a.FirstWeight > 0 && a.SecondWeight == 0).Count());
		}


		/// <summary>
		/// 其他物资运输记录业务定时器
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer_Goods_Tick(object sender, EventArgs e)
		{
			if (this.timer_Goods_Cancel) return;

			timer_Goods.Stop();
			timer_Goods.Interval = 2000;

			try
			{
				switch (this.CurrentFlowFlag)
				{
					case eFlowFlag.验证信息:
						#region
						if (this.CurrentAutotruck != null)
						{
							if (this.CurrentAutotruck.IsUse == 1)
							{
								// 查找该车未完成的运输记录
								CmcsUnFinishTransport unFinishTransport = carTransportDAO.GetUnFinishTransportByAutotruckId(this.CurrentAutotruck.Id, eCarType.其他物资.ToString());
								if (unFinishTransport != null)
								{
									this.CurrentGoodsTransport = commonDAO.SelfDber.Get<CmcsGoodsTransport>(unFinishTransport.TransportId);
									if (this.CurrentGoodsTransport != null)
									{
										if (this.CurrentGoodsTransport.SuttleWeight == 0)
										{
											BackGateUp();

											this.CurrentFlowFlag = eFlowFlag.等待上磅;

											UpdateLedShow(this.CurrentAutotruck.CarNumber, "请上磅");
											this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 请上磅", 1, false);
										}
										else
										{
											UpdateLedShow(this.CurrentAutotruck.CarNumber, "已称重");
											this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已称重", 1, false);

											timer_BuyFuel.Interval = 20000;
										}
									}
									else
									{
										commonDAO.SelfDber.Delete<CmcsUnFinishTransport>(unFinishTransport.Id);
									}
								}
								else
								{
									UpdateLedShow(this.CurrentAutotruck.CarNumber, "未排队");
									this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 未找到排队记录", 1, false);

									timer_Goods.Interval = 20000;
								}
							}
							else
							{
								UpdateLedShow(this.CurrentAutotruck.CarNumber, "已停用");
								this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 已停用，禁止通过", 1, false);

								timer_Goods.Interval = 20000;
							}
						}
						else
						{
							UpdateLedShow(this.CurrentImperfectCar.Voucher, "未登记");

							// 方式一:车号识别
							this.voiceSpeaker.Speak("车牌号 " + this.CurrentImperfectCar.Voucher + " 未登记，禁止通过", 1, false);
							//// 方式二:刷卡方式
							//this.voiceSpeaker.Speak("卡号未登记，禁止通过", 1, false);

							timer_Goods.Interval = 20000;
						}

						#endregion
						break;

					case eFlowFlag.等待上磅:
						#region

						// 当地磅仪表重量大于最小称重且来车方向的地感与对射均无信号，则判定车已经完全上磅
						if (Hardwarer.Wber.Weight >= this.WbMinWeight && !HasCarOnEnterWay())
						{
							//BackGateDown();

							this.CurrentFlowFlag = eFlowFlag.等待稳定;
						}

						// 降低灵敏度
						timer_Goods.Interval = 4000;

						#endregion
						break;

					case eFlowFlag.等待稳定:
						#region

						// 提高灵敏度
						timer_Goods.Interval = 1000;

						btnSaveTransport_Goods.Enabled = this.WbSteady;

						UpdateLedShow(this.CurrentAutotruck.CarNumber, Hardwarer.Wber.Weight.ToString("#0.######"));

						if (this.WbSteady)
						{
							if (this.AutoHandMode)
							{
								// 自动模式
								if (!SaveGoodsTransport())
								{
									UpdateLedShow(this.CurrentAutotruck.CarNumber, "称重失败");
									this.voiceSpeaker.Speak("车牌号 " + this.CurrentAutotruck.CarNumber + " 称重失败，请联系管理员", 1, false);
								}
							}
							else
							{
								// 手动模式 
							}
						}

						#endregion
						break;

					case eFlowFlag.等待离开:
						#region

						// 当前地磅重量小于最小称重且所有地感、对射无信号时重置
						if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnLeaveWay()) ResetGoods();

						// 降低灵敏度
						timer_Goods.Interval = 4000;

						#endregion
						break;
				}

				// 当前地磅重量小于最小称重且所有地感、对射无信号时重置
				if (Hardwarer.Wber.Weight < this.WbMinWeight && !HasCarOnEnterWay() && !HasCarOnLeaveWay() && this.CurrentFlowFlag != eFlowFlag.等待上磅
					&& this.CurrentImperfectCar != null) ResetGoods();
			}
			catch (Exception ex)
			{
				Log4Neter.Error("timer_Goods_Tick", ex);
			}
			finally
			{
				timer_Goods.Start();
			}
		}

		#endregion

		#region 其他函数

		Font directionFont = new Font("微软雅黑", 16);

		Pen redPen1 = new Pen(Color.Red, 1);
		Pen greenPen1 = new Pen(Color.Lime, 1);
		Pen redPen3 = new Pen(Color.Red, 3);
		Pen greenPen3 = new Pen(Color.Lime, 3);

		/// <summary>
		/// 当前仪表重量面板绘制
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panCurrentWeight_Paint(object sender, PaintEventArgs e)
		{
			PanelEx panel = sender as PanelEx;

			int height = 12;

			//// 绘制地感1
			//e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, 1, 15, height);
			//e.Graphics.DrawLine(this.InductorCoil1 ? redPen3 : greenPen3, 15, panel.Height - height, 15, panel.Height - 1);
			//// 绘制地感2
			//e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, 1, 25, height);
			//e.Graphics.DrawLine(this.InductorCoil2 ? redPen3 : greenPen3, 25, panel.Height - height, 25, panel.Height - 1);
			// 绘制对射1
			e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, 35, 1, 35, height);
			e.Graphics.DrawLine(this.InfraredSensor1 ? redPen1 : greenPen1, 35, panel.Height - height, 35, panel.Height - 1);

			// 绘制对射2
			e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, 1, panel.Width - 35, height);
			e.Graphics.DrawLine(this.InfraredSensor2 ? redPen1 : greenPen1, panel.Width - 35, panel.Height - height, panel.Width - 35, panel.Height - 1);

			// 上磅方向
			eDirection direction = eDirection.UnKnow;
			if (this.CurrentImperfectCar != null) direction = this.CurrentImperfectCar.PassWay;
			e.Graphics.DrawString("﹥>", directionFont, direction == eDirection.Way1 ? Brushes.Red : Brushes.Lime, 2, 17);
			e.Graphics.DrawString("<﹤", directionFont, direction == eDirection.Way2 ? Brushes.Red : Brushes.Lime, panel.Width - 47, 17);
		}

		private void superGridControl_BeginEdit(object sender, DevComponents.DotNetBar.SuperGrid.GridEditEventArgs e)
		{
			// 取消进入编辑
			e.Cancel = true;
		}

		/// <summary>
		/// 设置行号
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void superGridControl_GetRowHeaderText(object sender, DevComponents.DotNetBar.SuperGrid.GridGetRowHeaderTextEventArgs e)
		{
			e.Text = (e.GridRow.RowIndex + 1).ToString();
		}

		/// <summary>
		/// Invoke封装
		/// </summary>
		/// <param name="action"></param>
		public void InvokeEx(Action action)
		{
			if (this.IsDisposed || !this.IsHandleCreated) return;

			this.Invoke(action);
		}

		#endregion

		#region DataGridView
		/// <summary>
		/// 加载完成事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void superGridControl1_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
		{
			try
			{
				foreach (GridRow gridRow in e.GridPanel.Rows)
				{
					View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
					if (entity == null) return;
					gridRow.Cells["clmDeductWeight"].Value = entity.KsWeight + entity.KgWeight;
					gridRow.Cells["clmKsWeight"].Value = entity.AutoKsWeight + entity.KsWeight;
				}
			}
			catch (Exception ex)
			{
				Log4Neter.Error("GridView1加载完成事件", ex);
			}
		}
		/// <summary>
		/// 加载完成事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void superGridControl2_BuyFuel_DataBindingComplete(object sender, GridDataBindingCompleteEventArgs e)
		{
			foreach (GridRow gridRow in e.GridPanel.Rows)
			{
				try
				{
					View_BuyFuelTransport entity = gridRow.DataItem as View_BuyFuelTransport;
					if (entity == null) return;
					gridRow.Cells["clmDeductWeight"].Value = entity.KsWeight + entity.KgWeight;
					gridRow.Cells["clmKsWeight"].Value = entity.AutoKsWeight + entity.KsWeight;
				}
				catch (Exception ex)
				{
					Log4Neter.Error("已完成运输记录加载完成事件", ex);
				}
			}
		}
		#endregion
	}
}
