﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace GISsys
{
    public partial class frmAttribute : Form
    {
        private ILayer layer;

        public frmAttribute(ILayer lyr)
        {
            InitializeComponent();
            layer = lyr;
            this.Text = "\"" + layer.Name + "\" 属性表";  
        }

        private void frmAttribute_Load(object sender, EventArgs e)
        {
            try
                //读取每一行，获取属性值

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection SelRows = this.dataGridView1.SelectedRows;
            DataGridViewRow row;
            MainForm form = (MainForm)Application.OpenForms[0];
            IMap m = form.getMapControl().Map;
            m.ClearSelection();
            for (int i = 0; i < SelRows.Count; i++)
            {
                row = SelRows[i];
                int ID = Convert.ToInt32(row.Cells["FID"].Value.ToString());
                IFeatureLayer flyr = (IFeatureLayer)layer;
                IFeatureClass featurecls = flyr.FeatureClass;
                IFeature feature = featurecls.GetFeature(ID);
                m.SelectFeature(layer, feature);  //获取属性表中选中行对应的图形要素
            }
            form.getMapControl().Refresh();
        }
    }
}