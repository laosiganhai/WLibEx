﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace WLib.UserCtrls.Dev.ArcGisCtrl.Map
{
    /// <summary>
    /// 表格(ArcGIS ITable)列表控件
    /// </summary>
    public partial class TableListBox : UserControl
    {
        /// <summary>
        /// 加载在表格列表中的表格
        /// </summary>
        public List<ITable> Tables { get; }
        /// <summary>
        /// 属性表窗口
        /// </summary>
        public AttributeForm AttributeForm { get; set; }

        
        /// <summary>
        /// 表格(ArcGIS ITable)列表控件
        /// </summary>
        public TableListBox()
        {
            InitializeComponent();
            Tables = new List<ITable>();
        }
        /// <summary>
        /// 表格(ArcGIS ITable)列表控件
        /// </summary>
        /// <param name="attributeForm">属性表窗口</param>
        public TableListBox(AttributeForm attributeForm)
        {
            InitializeComponent();
            AttributeForm = attributeForm;
            Tables = new List<ITable>();
        }


        /// <summary>
        /// 添加表格
        /// </summary>
        /// <param name="table"></param>
        public void AddTable(ITable table)
        {
            Tables.Add(table);
            string tableName;
            if (table is IObjectClass objectClass)
                tableName = objectClass.AliasName;
            else
                tableName = ((IDataset)table).Name;
            imagelistBoxTables.Items.Add(new ImageListBoxItem(tableName, 17));
        }
        /// <summary>
        /// 添加多个表格
        /// </summary>
        /// <param name="tables"></param>
        public void AddTables(IEnumerable<ITable> tables)
        {
            Tables.AddRange(tables);
            var tableNamesItems = tables.Select(v => ((IObjectClass)v).AliasName).Select(v => new ImageListBoxItem(v, 17)).ToArray();
            imagelistBoxTables.Items.AddRange(tableNamesItems);
        }
        /// <summary>
        /// 移除指定表格
        /// </summary>
        /// <param name="table"></param>
        public void RemoveTable(ITable table)
        {
            int index = Tables.IndexOf(table);
            Tables.Remove(table);
            imagelistBoxTables.Items.RemoveAt(index);
        }
        /// <summary>
        /// 移除指定表格
        /// </summary>
        /// <param name="tableName"></param>
        public void RemoveTable(string tableName)
        {
            var table = Tables.FirstOrDefault(v => ((IObjectClass)v).AliasName == tableName);
            if (table != null)
                RemoveTable(table);
        }
        /// <summary>
        /// 清空表格列表
        /// </summary>
        public void ClearTable()
        {
            Tables.Clear();
            imagelistBoxTables.Items.Clear();
        }


        private void 打开表格属性表TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imagelistBoxTables.SelectedIndex < 0)
                return;
            string tableName = imagelistBoxTables.SelectedItem.ToString();
            var table = Tables.FirstOrDefault(v => ((IObjectClass)v).AliasName == tableName);
            if (table != null)
            {
                if (AttributeForm == null || AttributeForm.IsDisposed)
                    AttributeForm = new AttributeForm();
                else
                    AttributeForm.Activate();//之前已打开，则给予焦点，置顶。
                if (!AttributeForm.Visible)
                    AttributeForm.Show(this);

                AttributeForm.LoadAttribute(table);
            }
        }
        private void 移除表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveTable(imagelistBoxTables.SelectedItem.ToString());
        }
        private void imagelistBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enable = imagelistBoxTables.SelectedIndex > -1;
            打开表格属性表TToolStripMenuItem.Enabled = enable;
            移除表格ToolStripMenuItem.Enabled = enable;
        }
        private void imagelistBoxTables_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                imagelistBoxTables.SelectedIndex = imagelistBoxTables.IndexFromPoint(e.Location);
        }
        private void imagelistBoxTables_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && imagelistBoxTables.SelectedIndex > -1)
                cMenuStripTableList.Show(imagelistBoxTables, e.Location);
        }
    }
}
