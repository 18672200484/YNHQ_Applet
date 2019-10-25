using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CMCS.DumblyConcealer.Win.DumblyTasks;
using CMCS.DumblyConcealer.Win.Core;
using BasisPlatform.Util;

namespace CMCS.DumblyConcealer.Win
{
	public partial class MDIParent1 : Form
	{
		public MDIParent1()
		{
			InitializeComponent();
		}

		#region 窗口

		private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		#endregion

		private void MDIParent1_Load(object sender, EventArgs e)
		{

		}

		private void MDIParent1_Shown(object sender, EventArgs e)
		{
			BasisPlatformUtil.StartNewTask("开机延迟启动", () =>
			{
				int minute = 5, surplus = minute;

				while (minute > 0)
				{
					double d = minute - Environment.TickCount / 1000 / 60;
					if (Environment.TickCount < 0 || d <= 0) break;

					System.Threading.Thread.Sleep(60000);

					surplus--;
				}
#if DEBUG

#else
                    this.InvokeEx(() => { timer1.Enabled = true; });
#endif
			});
		}

		private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				if (MessageBox.Show("确认退出系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{

				}
				else
				{
					e.Cancel = true;
				}
			}
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

		/// <summary>
		/// 任务索引
		/// </summary>
		int taskFormIndex = 0;

		private void timer1_Tick(object sender, EventArgs e)
		{
			switch (taskFormIndex)
			{
				case 1:
					tsmiOpenFrmAssayDevice_Click(null, null);
					break;
			}

			if (taskFormIndex == 3)
			{
				TileHorizontalToolStripMenuItem_Click(null, null);
				timer1.Stop();
			}

			taskFormIndex++;
		}

		/// <summary>
		/// 05.化验设备数据读取
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tsmiOpenFrmAssayDevice_Click(object sender, EventArgs e)
		{
			new FrmAssayDevice
			{
				MdiParent = this
			}.Show();
		}

		private void tsmiOpenFrmCarSynchronous_Click(object sender, EventArgs e)
		{
			new FrmDataHandler
			{
				MdiParent = this
			}.Show();
		}
	}
}
