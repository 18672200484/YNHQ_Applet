using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using CMCS.Common;
using DevComponents.DotNetBar.SuperGrid;
using CMCS.CarTransport.DAO;
using CMCS.Common.DAO;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities;
using CMCS.Common.Entities.BaseInfo;

namespace CMCS.CarTransport.Weighter.Frms.BaseInfo.FuelKind
{
    public partial class FrmFuelKind_List : DevComponents.DotNetBar.Metro.MetroForm
    {
        /// <summary>
        /// 窗体唯一标识符
        /// </summary>
        public static string UniqueKey = "FrmFuelKind_List";
        /// <summary>
        /// 选中的实体
        /// </summary>
        public CmcsFuelKind Output;
        /// <summary>
        /// 新增 修改 标识
        /// </summary>
        string strEditMode = string.Empty;

        CarTransportDAO carTransportDao = CarTransportDAO.GetInstance();

        public FrmFuelKind_List()
        {
            InitializeComponent();
        }

        private void FrmFuelKind_List_Shown(object sender, EventArgs e)
        {
            advTree1.Nodes.Clear();

            CmcsFuelKind rootEntity = Dbers.GetInstance().SelfDber.Entity<CmcsFuelKind>("where ParentId is null");
            DevComponents.AdvTree.Node rootNode = CreateNode(rootEntity);

            LoadData(rootEntity, rootNode);

            advTree1.Nodes.Add(rootNode);
            addCmcsFuelKind(rootEntity);
            CMCS.CarTransport.Weighter.Utilities.Helper.ControlReadOnly(panelLeft);
            CMCS.CarTransport.Weighter.Utilities.Helper.ControlReadOnly(panelRight);
        }

        private void FrmFuelKind_List_KeyUp(object sender, KeyEventArgs e)
        {
        }

        void LoadData(CmcsFuelKind entity, DevComponents.AdvTree.Node node)
        {
            if (entity == null || node == null) return;

            foreach (CmcsFuelKind item in Dbers.GetInstance().SelfDber.Entities<CmcsFuelKind>("where ParentId=:ParentId order by Sequence asc", new { ParentId = entity.Id }))
            {
                DevComponents.AdvTree.Node newNode = CreateNode(item);
                node.Nodes.Add(newNode);
                LoadData(item, newNode);
            }
        }

        DevComponents.AdvTree.Node CreateNode(CmcsFuelKind entity)
        {
            DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node(entity.FuelName + ((entity.Valid == "有效") ? "" : "(无效)"));
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
            //if (advTree1.SelectedNode.Parent == null) return;
            this.Output = (advTree1.SelectedNode.Tag as CmcsFuelKind);
            addCmcsFuelKind(Output);
            strEditMode = "edit";
            EnableLeft();
        }

        void addCmcsFuelKind(CmcsFuelKind item)
        {
            txt_FuelName.Text = item.FuelName;
            txt_ReMark.Text = item.ReMark;
            dbi_Sequence.Text = item.Sequence.ToString();
            chb_IsUse.Checked = (item.Valid == "有效");
        }
        void EnableLeft()
        {
            CMCS.CarTransport.Weighter.Utilities.Helper.NoControlReadOnly(panelLeft);
        }
        void EnableRight()
        {
            CMCS.CarTransport.Weighter.Utilities.Helper.NoControlReadOnly(panelRight);
        }
        void Clear()
        {
            txt_FuelName.ResetText();
            dbi_Sequence.Value = 0;
            txt_ReMark.ResetText();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (Output == null)
            {
                MessageBoxEx.Show("请先选择一条记录！，", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Output.Id == "-1")
            {
                MessageBoxEx.Show("根节点不能修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.strEditMode = "edit";
            EnableRight();

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Output == null)
            {
                MessageBoxEx.Show("请先选择一条记录！，", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.strEditMode = "add";
            EnableRight();
            Clear();
        }

        private void btnDel_Click(object sender, EventArgs e)
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
                carTransportDao.DelFuelKind(Output);
                FrmFuelKind_List_Shown(null, null);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (strEditMode == "add")
            {
                CmcsFuelKind fuelkind = new CmcsFuelKind()
                {
                    FuelCode = carTransportDao.GetMineNewChildCode(Output.FuelCode),
                    FuelName = txt_FuelName.Text,
                    Valid = chb_IsUse.Checked ? "有效" : "无效",
                    Sequence = dbi_Sequence.Value,
                    ReMark = txt_ReMark.Text,
                    ParentId = Output.Id
                };
                carTransportDao.InsertFuelKind(fuelkind);
            }
            else
            {
                Output.FuelCode = carTransportDao.GetFuelKindNewChildCode(Output.FuelCode);
                Output.FuelName = txt_FuelName.Text;
                Output.Sequence = dbi_Sequence.Value;
                Output.Valid = chb_IsUse.Checked ? "有效" : "无效";
                Output.ReMark = txt_ReMark.Text;
                carTransportDao.InsertFuelKind(Output);
            }
            FrmFuelKind_List_Shown(null, null);
        }
    }
}