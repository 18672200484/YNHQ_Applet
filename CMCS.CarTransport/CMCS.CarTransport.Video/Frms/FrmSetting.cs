using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using System.IO.Ports;
using CMCS.Common.DAO;
using CMCS.Common;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.Common.Utilities;
using CMCS.CarTransport.Out.Core;
using CMCS.Common.Entities.BaseInfo;
using CMCS.CarTransport.DAO;

namespace CMCS.CarTransport.Out.Frms
{
    public partial class FrmSetting : DevComponents.DotNetBar.Metro.MetroForm
    {
        CommonDAO commonDAO = CommonDAO.GetInstance();
        VideoDAO videoDAO = VideoDAO.GetInstance();
        CommonAppConfig commonAppConfig = CommonAppConfig.GetInstance();
        /// <summary>
        /// 新增 修改 标识
        /// </summary>
        string strEditMode = string.Empty;
        /// <summary>
        /// 选中的实体
        /// </summary>
        public CmcsCamare Output;

        public FrmSetting()
        {
            InitializeComponent();
        }

        void InitForm()
        {
            InitComPortComboBoxs(cmbIocerCom, cmbRwer1Com, cmbRwer2Com);
            InitBandrateComboBoxs(cmbIocerBandrate);
            InitNumberAscComboBoxs(5, 8, cmbIocerDataBits);
            InitNumberAscComboBoxs(1, 15, cmbInductorCoil1Port, cmbInductorCoil2Port, cmbInductorCoil3Port, cmbInductorCoil4Port, cmbGate1UpPort, cmbGate1DownPort, cmbGate2UpPort, cmbGate2DownPort, cmbSignalLight1Port, cmbSignalLight2Port);
            InitStopBitsComboBoxs(cmbIocerStopBits);
            InitParityComboBoxs(cmbIocerParity);
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            advTree1.Nodes.Clear();

            CmcsCamare rootEntity = Dbers.GetInstance().SelfDber.Entity<CmcsCamare>("where ParentId is null");
            DevComponents.AdvTree.Node rootNode = CreateNode(rootEntity);

            LoadData(rootEntity, rootNode);

            advTree1.Nodes.Add(rootNode);
            addCmcsCamare(rootEntity);
            UnEnable();
            //CMCS.CarTransport.Queue.Utilities.Helper.ControlReadOnly(this);
        }

        private void FrmSetting_Shown(object sender, EventArgs e)
        {
            InitForm();

            LoadAppConfig();
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns></returns>
        private bool TestDBConnect()
        {
            if (string.IsNullOrEmpty(txtSelfConnStr.Text.Trim()))
            {
                MessageBoxEx.Show("请先输入数据库连接字符串", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                OracleDapperDber dber = new OracleDapperDber(txtSelfConnStr.Text.Trim());
                dber.Connection.Open();
                dber.Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("数据库连接失败，" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        void LoadData(CmcsCamare entity, DevComponents.AdvTree.Node node)
        {
            if (entity == null || node == null) return;

            foreach (CmcsCamare item in Dbers.GetInstance().SelfDber.Entities<CmcsCamare>("where ParentId=:ParentId order by Sequence asc", new { ParentId = entity.Id }))
            {
                DevComponents.AdvTree.Node newNode = CreateNode(item);
                node.Nodes.Add(newNode);
                LoadData(item, newNode);
            }
        }

        DevComponents.AdvTree.Node CreateNode(CmcsCamare entity)
        {
            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node(entity.Name);
            node.Tag = entity;
            node.Expanded = true;
            return node;
        }

        private void advTree1_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            //advTree1_NodeClick(sender, e);
        }

        private void advTree1_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            Return();
        }
        void Return()
        {
            if (advTree1.SelectedNode.Parent == null) return;
            this.Output = (advTree1.SelectedNode.Tag as CmcsCamare);
            addCmcsCamare(Output);
            strEditMode = "edit";
            Enable();
        }

        void addCmcsCamare(CmcsCamare item)
        {
            txtCode.Text = item.Code;
            txt_Name.Text = item.Name;
            txtip.Text = item.Ip;
            txtUserName.Text = item.UserName;
            txtPassWord.Text = item.Password;
            dbPort.Value = item.Port;
            dbChannel.Value = item.Channel;
            cmbType.Text = item.Type;
            txtEquipmentCode.Text = item.EquipmentCode;
            txt_ReMark.Text = item.Remark;
            dbSequence.Value = item.Sequence;
        }

        void UnEnable()
        {
            CMCS.CarTransport.Queue.Utilities.Helper.ControlReadOnly(tabControlPanel8);
            //BtnAdd.Enabled = false;
            //BtnEdit.Enabled = false;
            //BtnDel.Enabled = false;
            //txtCode.Enabled = false;
            //txt_Name.Enabled = false;
            //txtip.Enabled = false;
            //txtUserName.Enabled = false;
            //txtPassWord.Enabled = false;
            //dbPort.Enabled = false;
            //dbChannel.Enabled = false;
            //cmbType.Enabled = false;
            //txtEquipmentCode.Enabled = false;
            //txt_ReMark.Enabled = false;
            //dbSequence.Enabled = false;
        }

        void Enable()
        {
            CMCS.CarTransport.Queue.Utilities.Helper.NoControlReadOnly(tabControlPanel8);

            //BtnAdd.Enabled = true;
            //BtnEdit.Enabled = true;
            //BtnDel.Enabled = true;
            //txtCode.Enabled = true;
            //txt_Name.Enabled = true;
            //txtip.Enabled = true;
            //txtUserName.Enabled = true;
            //txtPassWord.Enabled = true;
            //dbPort.Enabled = true;
            //dbChannel.Enabled = true;
            //cmbType.Enabled = true;
            //txtEquipmentCode.Enabled = true;
            //txt_ReMark.Enabled = true;
            //dbSequence.Enabled = true;
        }


        void Clear()
        {
            txtCode.ResetText();
            txt_Name.ResetText();
            txtip.ResetText();
            txtUserName.ResetText();
            txtPassWord.ResetText();
            dbPort.Value = 0;
            dbChannel.Value = 0;
            cmbType.ResetText();
            txtEquipmentCode.ResetText();
            txt_ReMark.ResetText();
            dbSequence.Value = 0;
        }

        /// <summary>
        /// 验证页面控件值的逻辑合法性
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            if (GlobalVars.AdminAccount != "admin")
            {
                if (strEditMode == "add" && Output.Id == "-1")
                {
                    MessageBoxEx.Show("非管理员不可新增2级节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else if (strEditMode == "edit")
                {
                    if (Output.Id == "-1" || Output.ParentId == "-1")
                    {
                        MessageBoxEx.Show("非管理员不可修改1、2级节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }
            if (strEditMode == "add")
            {
                try
                {
                    if (videoDAO.GetCameraDepth(Output) == 4)
                    {
                        MessageBoxEx.Show("不可新增4级节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                catch (Exception)
                {
                }

            }
            if (strEditMode == "add")
            {
                CmcsCamare video = videoDAO.GetVideoByName(txt_Name.Text);

                if (video != null)
                {
                    MessageBoxEx.Show("已有相同摄像头名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                video = videoDAO.GetVideoByCode(txtCode.Text);

                if (video != null)
                {
                    MessageBoxEx.Show("已有相同摄像头编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            else if (strEditMode == "edit")
            {
                CmcsCamare video = videoDAO.GetVideoByName(txt_Name.Text);

                if (video != null && video.Id != Output.Id)
                {
                    MessageBoxEx.Show("已有相同摄像头名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                video = videoDAO.GetVideoByCode(txtCode.Text);

                if (video != null && video.Code != Output.Code)
                {
                    MessageBoxEx.Show("已有相同摄像头编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        void LoadAppConfig()
        {
            txtAppIdentifier.Text = commonAppConfig.AppIdentifier;
            txtSelfConnStr.Text = commonAppConfig.SelfConnStr;
            chbStartup.Checked = (commonDAO.GetAppletConfigString("开机启动") == "1");

            //CmcsCamare video = videoDAO.GetVideoByCode("一号汽车衡一号摄像机");
            //if (video != null)
            //{
            //    txtqch1_ip1.Value = video.Ip;
            //    txtqch1_UserName1.Text = video.UserName;
            //    txtqch1_Password1.Text = video.Password;
            //    txtqch1_Port1.Text = video.Port.ToString();
            //    txtqch1_Channel1.Text = video.Channel.ToString();
            //}
            //video = videoDAO.GetVideoByCode("一号汽车衡二号摄像机");
            //if (video != null)
            //{
            //    txtqch1_ip1.Value = video.Ip;
            //    txtqch1_UserName1.Text = video.UserName;
            //    txtqch1_Password1.Text = video.Password;
            //    txtqch1_Port1.Text = video.Port.ToString();
            //    txtqch1_Channel1.Text = video.Channel.ToString();
            //}
            #region 加载设置

            ////一号汽车衡
            //txtqch1_ip1.Value = commonDAO.GetAppletConfigString("一号汽车衡_IP地址1");
            //txtqch1_UserName1.Text = commonDAO.GetAppletConfigString("一号汽车衡_用户名1");
            //txtqch1_Password1.Text = commonDAO.GetAppletConfigString("一号汽车衡_密码1");
            //txtqch1_Port1.Text = commonDAO.GetAppletConfigString("一号汽车衡_端口号1");
            //txtqch1_Channel1.Text = commonDAO.GetAppletConfigString("一号汽车衡_通道号1");

            //txtqch1_ip2.Value = commonDAO.GetAppletConfigString("一号汽车衡_IP地址2");
            //txtqch1_UserName2.Text = commonDAO.GetAppletConfigString("一号汽车衡_用户名2");
            //txtqch1_Password2.Text = commonDAO.GetAppletConfigString("一号汽车衡_密码2");
            //txtqch1_Port2.Text = commonDAO.GetAppletConfigString("一号汽车衡_端口号2");
            //txtqch1_Channel2.Text = commonDAO.GetAppletConfigString("一号汽车衡_通道号2");

            //txtqch1_ip3.Value = commonDAO.GetAppletConfigString("一号汽车衡_IP地址3");
            //txtqch1_UserName3.Text = commonDAO.GetAppletConfigString("一号汽车衡_用户名3");
            //txtqch1_Password3.Text = commonDAO.GetAppletConfigString("一号汽车衡_密码3");
            //txtqch1_Port3.Text = commonDAO.GetAppletConfigString("一号汽车衡_端口号3");
            //txtqch1_Channel3.Text = commonDAO.GetAppletConfigString("一号汽车衡_通道号3");
            ////二号汽车衡
            //txtqch2_ip1.Value = commonDAO.GetAppletConfigString("二号汽车衡_IP地址1");
            //txtqch2_UserName1.Text = commonDAO.GetAppletConfigString("二号汽车衡_用户名1");
            //txtqch2_Password1.Text = commonDAO.GetAppletConfigString("二号汽车衡_密码1");
            //txtqch2_Port1.Text = commonDAO.GetAppletConfigString("二号汽车衡_端口号1");
            //txtqch2_Channel1.Text = commonDAO.GetAppletConfigString("二号汽车衡_通道号1");

            //txtqch2_ip2.Value = commonDAO.GetAppletConfigString("二号汽车衡_IP地址2");
            //txtqch2_UserName2.Text = commonDAO.GetAppletConfigString("二号汽车衡_用户名2");
            //txtqch2_Password2.Text = commonDAO.GetAppletConfigString("二号汽车衡_密码2");
            //txtqch2_Port2.Text = commonDAO.GetAppletConfigString("二号汽车衡_端口号2");
            //txtqch2_Channel2.Text = commonDAO.GetAppletConfigString("二号汽车衡_通道号2");

            //txtqch2_ip3.Value = commonDAO.GetAppletConfigString("二号汽车衡_IP地址3");
            //txtqch2_UserName3.Text = commonDAO.GetAppletConfigString("二号汽车衡_用户名3");
            //txtqch2_Password3.Text = commonDAO.GetAppletConfigString("二号汽车衡_密码3");
            //txtqch2_Port3.Text = commonDAO.GetAppletConfigString("二号汽车衡_端口号3");
            //txtqch2_Channel3.Text = commonDAO.GetAppletConfigString("二号汽车衡_通道号3");
            ////三号汽车衡
            //txtqch3_ip1.Value = commonDAO.GetAppletConfigString("三号汽车衡_IP地址1");
            //txtqch3_UserName1.Text = commonDAO.GetAppletConfigString("三号汽车衡_用户名1");
            //txtqch3_Password1.Text = commonDAO.GetAppletConfigString("三号汽车衡_密码1");
            //txtqch3_Port1.Text = commonDAO.GetAppletConfigString("三号汽车衡_端口号1");
            //txtqch3_Channel1.Text = commonDAO.GetAppletConfigString("三号汽车衡_通道号1");

            //txtqch3_ip2.Value = commonDAO.GetAppletConfigString("三号汽车衡_IP地址2");
            //txtqch3_UserName2.Text = commonDAO.GetAppletConfigString("三号汽车衡_用户名2");
            //txtqch3_Password2.Text = commonDAO.GetAppletConfigString("三号汽车衡_密码2");
            //txtqch3_Port2.Text = commonDAO.GetAppletConfigString("三号汽车衡_端口号2");
            //txtqch3_Channel2.Text = commonDAO.GetAppletConfigString("三号汽车衡_通道号2");

            //txtqch3_ip3.Value = commonDAO.GetAppletConfigString("三号汽车衡_IP地址3");
            //txtqch3_UserName3.Text = commonDAO.GetAppletConfigString("三号汽车衡_用户名3");
            //txtqch3_Password3.Text = commonDAO.GetAppletConfigString("三号汽车衡_密码3");
            //txtqch3_Port3.Text = commonDAO.GetAppletConfigString("三号汽车衡_端口号3");
            //txtqch3_Channel3.Text = commonDAO.GetAppletConfigString("三号汽车衡_通道号3");
            ////四号汽车衡
            //txtqch4_ip1.Value = commonDAO.GetAppletConfigString("四号汽车衡_IP地址1");
            //txtqch4_UserName1.Text = commonDAO.GetAppletConfigString("四号汽车衡_用户名1");
            //txtqch4_Password1.Text = commonDAO.GetAppletConfigString("四号汽车衡_密码1");
            //txtqch4_Port1.Text = commonDAO.GetAppletConfigString("四号汽车衡_端口号1");
            //txtqch4_Channel1.Text = commonDAO.GetAppletConfigString("四号汽车衡_通道号1");

            //txtqch4_ip2.Value = commonDAO.GetAppletConfigString("四号汽车衡_IP地址2");
            //txtqch4_UserName2.Text = commonDAO.GetAppletConfigString("四号汽车衡_用户名2");
            //txtqch4_Password2.Text = commonDAO.GetAppletConfigString("四号汽车衡_密码2");
            //txtqch4_Port2.Text = commonDAO.GetAppletConfigString("四号汽车衡_端口号2");
            //txtqch4_Channel2.Text = commonDAO.GetAppletConfigString("四号汽车衡_通道号2");

            //txtqch4_ip3.Value = commonDAO.GetAppletConfigString("四号汽车衡_IP地址3");
            //txtqch4_UserName3.Text = commonDAO.GetAppletConfigString("四号汽车衡_用户名3");
            //txtqch4_Password3.Text = commonDAO.GetAppletConfigString("四号汽车衡_密码3");
            //txtqch4_Port3.Text = commonDAO.GetAppletConfigString("四号汽车衡_端口号3");
            //txtqch4_Channel3.Text = commonDAO.GetAppletConfigString("四号汽车衡_通道号3"); 
            #endregion
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        bool SaveAppConfig()
        {
            if (!TestDBConnect()) return false;
            if (!ValidateInput()) return false;
            commonAppConfig.AppIdentifier = txtAppIdentifier.Text.Trim();
            commonAppConfig.SelfConnStr = txtSelfConnStr.Text;
            commonAppConfig.Save();
            commonDAO.SetAppletConfig("开机启动", Convert.ToInt16(chbStartup.Checked).ToString());

            if (strEditMode == "add")
            {
                CmcsCamare video = new CmcsCamare()
                {
                    Code = videoDAO.GetCameraNewChildCode(Output.Code),
                    Name = txt_Name.Text,
                    Ip = txtip.Text,
                    Port = Convert.ToInt32(dbPort.Value),
                    Channel = Convert.ToInt32(dbChannel.Value),
                    UserName = txtUserName.Text,
                    Password = txtPassWord.Text,
                    Type = cmbType.Text,
                    Sequence = Convert.ToInt32(dbSequence.Value),
                    EquipmentCode = txtEquipmentCode.Text,
                    Remark = txt_ReMark.Text,
                    ParentId = Output.Id,
                };
                videoDAO.InsertVideo(video);
            }
            else
            {
                Output.Name = txt_Name.Text;
                Output.Ip = txtip.Text;
                Output.Port = Convert.ToInt32(dbPort.Value);
                Output.Channel = Convert.ToInt32(dbChannel.Value);
                Output.UserName = txtUserName.Text;
                Output.Password = txtPassWord.Text;
                Output.Type = cmbType.Text;
                Output.Sequence = Convert.ToInt32(dbSequence.Value);
                Output.EquipmentCode = txtEquipmentCode.Text;
                Output.Remark = txt_ReMark.Text;
                videoDAO.InsertVideo(Output);
            }

            try
            {
#if DEBUG

#else
               // 添加、取消开机启动
                if (chbStartup.Checked)
                    StartUpUtil.InsertStartUp(Application.ProductName, Application.ExecutablePath);
                else
                    StartUpUtil.DeleteStartUp(Application.ProductName);
#endif
            }
            catch { }
            #region 保存配置

            ////一号汽车衡
            //if (string.IsNullOrEmpty(txtqch1_Code1.Text))
            //{
            //    MessageBoxEx.Show("请填写一号汽车衡的一号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch1_Code1.Text, txtqch1_ip1.Value, txtqch1_UserName1.Text, txtqch1_Password1.Text, txtqch1_Port1.Text, txtqch1_Channel1.Text);

            //if (string.IsNullOrEmpty(txtqch1_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写一号汽车衡的二号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch1_Code2.Text, txtqch1_ip2.Value, txtqch1_UserName2.Text, txtqch1_Password2.Text, txtqch1_Port2.Text, txtqch1_Channel2.Text);

            //if (string.IsNullOrEmpty(txtqch1_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写一号汽车衡的三号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch1_Code2.Text, txtqch1_ip2.Value, txtqch1_UserName2.Text, txtqch1_Password2.Text, txtqch1_Port2.Text, txtqch1_Channel2.Text);

            ////二号汽车衡
            //if (string.IsNullOrEmpty(txtqch2_Code1.Text))
            //{
            //    MessageBoxEx.Show("请填写二号汽车衡的一号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch2_Code1.Text, txtqch2_ip1.Value, txtqch2_UserName1.Text, txtqch2_Password1.Text, txtqch2_Port1.Text, txtqch2_Channel1.Text);

            //if (string.IsNullOrEmpty(txtqch2_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写二号汽车衡的二号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch2_Code2.Text, txtqch2_ip2.Value, txtqch2_UserName2.Text, txtqch2_Password2.Text, txtqch2_Port2.Text, txtqch2_Channel2.Text);

            //if (string.IsNullOrEmpty(txtqch2_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写二号汽车衡的三号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch2_Code2.Text, txtqch2_ip2.Value, txtqch2_UserName2.Text, txtqch2_Password2.Text, txtqch2_Port2.Text, txtqch2_Channel2.Text);

            ////三号汽车衡
            //if (string.IsNullOrEmpty(txtqch3_Code1.Text))
            //{
            //    MessageBoxEx.Show("请填写三号汽车衡的一号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch3_Code1.Text, txtqch3_ip1.Value, txtqch3_UserName1.Text, txtqch3_Password1.Text, txtqch3_Port1.Text, txtqch3_Channel1.Text);

            //if (string.IsNullOrEmpty(txtqch3_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写三号汽车衡的二号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch3_Code2.Text, txtqch3_ip2.Value, txtqch3_UserName2.Text, txtqch3_Password2.Text, txtqch3_Port2.Text, txtqch3_Channel2.Text);

            //if (string.IsNullOrEmpty(txtqch3_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写三号汽车衡的三号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch3_Code2.Text, txtqch3_ip2.Value, txtqch3_UserName2.Text, txtqch3_Password2.Text, txtqch3_Port2.Text, txtqch3_Channel2.Text);

            ////四号汽车衡
            //if (string.IsNullOrEmpty(txtqch4_Code1.Text))
            //{
            //    MessageBoxEx.Show("请填写四号汽车衡的一号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch4_Code1.Text, txtqch4_ip1.Value, txtqch4_UserName1.Text, txtqch4_Password1.Text, txtqch4_Port1.Text, txtqch4_Channel1.Text);

            //if (string.IsNullOrEmpty(txtqch4_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写四号汽车衡的二号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch4_Code2.Text, txtqch4_ip2.Value, txtqch4_UserName2.Text, txtqch4_Password2.Text, txtqch4_Port2.Text, txtqch4_Channel2.Text);

            //if (string.IsNullOrEmpty(txtqch4_Code2.Text))
            //{
            //    MessageBoxEx.Show("请填写四号汽车衡的三号摄像机编码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}
            //videoDAO.InsertVideo(txtqch4_Code2.Text, txtqch4_ip2.Value, txtqch4_UserName2.Text, txtqch4_Password2.Text, txtqch4_Port2.Text, txtqch4_Channel2.Text);



            ////一号汽车衡
            //commonDAO.SetAppletConfig("一号汽车衡_IP地址1", txtqch1_ip1.Value);
            //commonDAO.SetAppletConfig("一号汽车衡_用户名1", txtqch1_UserName1.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP密码1", txtqch1_Password1.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP端口号1", txtqch1_Port1.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP通道号1", txtqch1_Channel1.Text);

            //commonDAO.SetAppletConfig("一号汽车衡_IP地址2", txtqch1_ip2.Value);
            //commonDAO.SetAppletConfig("一号汽车衡_用户名2", txtqch1_UserName2.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP密码2", txtqch1_Password2.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP端口号2", txtqch1_Port2.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP通道号2", txtqch1_Channel2.Text);

            //commonDAO.SetAppletConfig("一号汽车衡_IP地址3", txtqch1_ip3.Value);
            //commonDAO.SetAppletConfig("一号汽车衡_用户名3", txtqch1_UserName3.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP密码3", txtqch1_Password3.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP端口号3", txtqch1_Port3.Text);
            //commonDAO.SetAppletConfig("一号汽车衡_IP通道号3", txtqch1_Channel3.Text);
            ////二号汽车衡
            //commonDAO.SetAppletConfig("二号汽车衡_IP地址1", txtqch2_ip1.Value);
            //commonDAO.SetAppletConfig("二号汽车衡_用户名1", txtqch2_UserName1.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP密码1", txtqch2_Password1.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP端口号1", txtqch2_Port1.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP通道号1", txtqch2_Channel1.Text);

            //commonDAO.SetAppletConfig("二号汽车衡_IP地址2", txtqch2_ip2.Value);
            //commonDAO.SetAppletConfig("二号汽车衡_用户名2", txtqch2_UserName2.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP密码2", txtqch2_Password2.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP端口号2", txtqch2_Port2.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP通道号2", txtqch2_Channel2.Text);

            //commonDAO.SetAppletConfig("二号汽车衡_IP地址3", txtqch2_ip3.Value);
            //commonDAO.SetAppletConfig("二号汽车衡_用户名3", txtqch2_UserName3.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP密码3", txtqch2_Password3.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP端口号3", txtqch2_Port3.Text);
            //commonDAO.SetAppletConfig("二号汽车衡_IP通道号3", txtqch2_Channel3.Text);
            ////三号汽车衡
            //commonDAO.SetAppletConfig("三号汽车衡_IP地址1", txtqch3_ip1.Value);
            //commonDAO.SetAppletConfig("三号汽车衡_用户名1", txtqch3_UserName1.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP密码1", txtqch3_Password1.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP端口号1", txtqch3_Port1.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP通道号1", txtqch3_Channel1.Text);

            //commonDAO.SetAppletConfig("三号汽车衡_IP地址2", txtqch3_ip2.Value);
            //commonDAO.SetAppletConfig("三号汽车衡_用户名2", txtqch3_UserName2.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP密码2", txtqch3_Password2.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP端口号2", txtqch3_Port2.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP通道号2", txtqch3_Channel2.Text);

            //commonDAO.SetAppletConfig("三号汽车衡_IP地址3", txtqch3_ip3.Value);
            //commonDAO.SetAppletConfig("三号汽车衡_用户名3", txtqch3_UserName3.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP密码3", txtqch3_Password3.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP端口号3", txtqch3_Port3.Text);
            //commonDAO.SetAppletConfig("三号汽车衡_IP通道号3", txtqch3_Channel3.Text);
            ////四号汽车衡
            //commonDAO.SetAppletConfig("四号汽车衡_IP地址1", txtqch4_ip1.Value);
            //commonDAO.SetAppletConfig("四号汽车衡_用户名1", txtqch4_UserName1.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP密码1", txtqch4_Password1.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP端口号1", txtqch4_Port1.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP通道号1", txtqch4_Channel1.Text);

            //commonDAO.SetAppletConfig("四号汽车衡_IP地址2", txtqch4_ip2.Value);
            //commonDAO.SetAppletConfig("四号汽车衡_用户名2", txtqch4_UserName2.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP密码2", txtqch4_Password2.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP端口号2", txtqch4_Port2.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP通道号2", txtqch4_Channel2.Text);

            //commonDAO.SetAppletConfig("四号汽车衡_IP地址3", txtqch4_ip3.Value);
            //commonDAO.SetAppletConfig("四号汽车衡_用户名3", txtqch4_UserName3.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP密码3", txtqch4_Password3.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP端口号3", txtqch4_Port3.Text);
            //commonDAO.SetAppletConfig("四号汽车衡_IP通道号3", txtqch4_Channel3.Text);

            #endregion
            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidateInputEmpty(new List<string> { "程序唯一标识符", "数据库连接字符串" }, new List<Control> { txtAppIdentifier, txtSelfConnStr })) return;

            if (!SaveAppConfig()) return;
            FrmSetting_Load(null, null);
            if (MessageBoxEx.Show("更改的配置需要重启程序才能生效，是否立刻重启？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Restart();
            else
                this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 其他函数

        /// <summary>
        /// 验证批量控件为空，并提示
        /// </summary>
        /// <param name="tipsNames"></param>
        /// <param name="controls"></param>
        /// <returns></returns>
        public static bool ValidateInputEmpty(List<string> tipsNames, List<Control> controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Control control = controls[i];

                if (control is TextBoxX && string.IsNullOrEmpty(((TextBoxX)control).Text))
                {
                    control.Focus();
                    MessageBoxEx.Show("请输入" + tipsNames[i] + "！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 选中下拉框选项
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="text"></param>
        private void SelectedComboBoxItem(ComboBoxEx cmb, string value)
        {
            foreach (DataItem dataItem in cmb.Items)
            {
                if (dataItem.Value == value) cmb.SelectedItem = dataItem;
            }
        }

        /// <summary>
        /// 初始化串口下拉框
        /// </summary>
        /// <param name="cmb"></param>
        void InitComPortComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            for (int i = 1; i < 20; i++)
            {
                cmb.Items.Add(new DataItem("COM" + i.ToString(), i.ToString()));
            }

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化串口下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitComPortComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitComPortComboBox(cmb);
            }
        }

        /// <summary>
        /// 初始化波特率下拉框
        /// </summary>
        /// <param name="cmb"></param>
        private void InitBandrateComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            cmb.Items.Add(new DataItem("1200"));
            cmb.Items.Add(new DataItem("4800"));
            cmb.Items.Add(new DataItem("9600"));
            cmb.Items.Add(new DataItem("14400"));
            cmb.Items.Add(new DataItem("19200"));
            cmb.Items.Add(new DataItem("38400"));
            cmb.Items.Add(new DataItem("56000"));
            cmb.Items.Add(new DataItem("57600"));
            cmb.Items.Add(new DataItem("115200"));

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化波特率下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitBandrateComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitBandrateComboBox(cmb);
            }
        }

        /// <summary>
        /// 初始化数字下拉框
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void InitNumberAscComboBox(int start, int end, ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            for (int i = start; i <= end; i++)
            {
                cmb.Items.Add(new DataItem(i.ToString()));
            }

            if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化数字下拉框
        /// </summary>
        /// <param name="cmb"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        void InitNumberAscComboBoxs(int start, int end, params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitNumberAscComboBox(start, end, cmb);
            }
        }

        /// <summary>
        /// 初始化停止位下拉框
        /// </summary>
        /// <param name="cmb"></param>
        void InitStopBitsComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            cmb.Items.Add(new DataItem(StopBits.None.ToString(), ((int)StopBits.None).ToString()));
            cmb.Items.Add(new DataItem(StopBits.One.ToString(), ((int)StopBits.One).ToString()));
            cmb.Items.Add(new DataItem(StopBits.OnePointFive.ToString(), ((int)StopBits.OnePointFive).ToString()));
            cmb.Items.Add(new DataItem(StopBits.Two.ToString(), ((int)StopBits.Two).ToString()));

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化停止位下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitStopBitsComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitStopBitsComboBox(cmb);
            }
        }

        /// <summary>
        /// 初始化校验位下拉框
        /// </summary>
        /// <param name="cmb"></param>
        void InitParityComboBox(ComboBoxEx cmb)
        {
            cmb.Items.Clear();

            cmb.DisplayMember = "Text";
            cmb.ValueMember = "Value";

            cmb.Items.Add(new DataItem(Parity.None.ToString(), ((int)Parity.None).ToString()));
            cmb.Items.Add(new DataItem(Parity.Odd.ToString(), ((int)Parity.Odd).ToString()));
            cmb.Items.Add(new DataItem(Parity.Even.ToString(), ((int)Parity.Even).ToString()));
            cmb.Items.Add(new DataItem(Parity.Mark.ToString(), ((int)Parity.Mark).ToString()));
            cmb.Items.Add(new DataItem(Parity.Space.ToString(), ((int)Parity.Space).ToString()));

            cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化校验位下拉框
        /// </summary>
        /// <param name="cmbs"></param>
        void InitParityComboBoxs(params ComboBoxEx[] cmbs)
        {
            foreach (ComboBoxEx cmb in cmbs)
            {
                InitParityComboBox(cmb);
            }
        }

        #endregion

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Output == null)
            {
                MessageBoxEx.Show("请先选择一条记录！，", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.strEditMode = "add";
            Enable();
            Clear();
            txtCode.Text = videoDAO.GetCameraNewChildCode(this.Output.Code);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (Output == null)
            {
                MessageBoxEx.Show("请先选择一条记录！，", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.strEditMode = "edit";
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            if (Output == null)
            {
                MessageBoxEx.Show("请先选择一条记录！，", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Output.Id == "-1")
            {
                MessageBoxEx.Show("根节点不能删除！，", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBoxEx.Show("确定删除该条记录及其所有子节点？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                videoDAO.DelVideo(Output);
                FrmSetting_Load(null, null);
            }
        }
    }
}