﻿/*---------------------------------------------------------------- 
// auth： Windragon
// date： 2019/2/23
// desc： None
// mdfy:  None
//----------------------------------------------------------------*/

using DevExpress.XtraEditors;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Linq;
using System.Windows.Forms;
using WLib.ArcGis.Control;
using WLib.ArcGis.Control.MapAssociation;
using WLib.ArcGis.Display;
using WLib.Attributes;

namespace WLib.UserCtrls.Dev.ArcGisCtrl
{
    /// <summary>
    /// 地图导航条
    /// </summary>
    public partial class MapNavigationTools : UserControl, IMapNavigationTools
    {
        /// <summary>
        /// 自定义测量工具对象
        /// </summary>
        private MapCtrlMeasure _measureTool;
        /// <summary>
        /// 地图导航工具条所绑定的地图控件
        /// </summary>
        private AxMapControl _mapCtrl;
        /// <summary>
        /// 用于卷帘工具
        /// </summary>
        private ILayerEffectProperties _effectLayer;
        /// <summary>
        /// 当前使用的地图导航工具
        /// </summary>
        public EMapTools CurrentTool { get; private set; }
        /// <summary>
        /// 地图导航工具条所绑定的地图控件
        /// </summary>
        public AxMapControl MapControl
        {
            get => _mapCtrl;
            set
            {
                _mapCtrl = value;
                _mapCtrl.OnMouseDown += mapControl_OnMouseDown;
                _mapCtrl.OnMouseMove += mapControl_OnMouseMove;
                _effectLayer = new CommandsEnvironmentClass();
                _measureTool = new MapCtrlMeasure(MapControl);
                CurrentTool = EMapTools.None;
            }
        }


        /// <summary>
        /// 地图导航条
        /// </summary>
        public MapNavigationTools()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 地图导航条
        /// </summary>
        /// <param name="mapCtrl">地图导航工具条所绑定的地图控件</param>
        public MapNavigationTools(AxMapControl mapCtrl = null)
        {
            InitializeComponent();
            this.MapControl = mapCtrl;
        }
        /// <summary>
        /// 启用或触发工具栏（地图导航条）上的工具
        /// </summary>
        /// <param name="mapTool"></param>
        public void ToolOnClick(EMapTools mapTool)
        {
            var text = mapTool.GetDescription();
            var button = grpMapNav.Controls.OfType<SimpleButton>().FirstOrDefault(v => v.Text == text);
            navigationButton_Click(button, null);
        }


        //鼠标左键点击测距离/面积，中键点击移动
        private void mapControl_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 4)
            {
                esriControlsMousePointer pointer = MapControl.MousePointer;
                MapControl.MousePointer = esriControlsMousePointer.esriPointerPan;
                MapControl.Pan();
                MapControl.MousePointer = pointer;
                return;
            }

            switch (CurrentTool)
            {
                case EMapTools.MeasureDistance: //测距离
                    MapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;//鼠标指针:十字状
                    lblMeasureTips.Visible = true;
                    if (e.button == 1)
                    {
                        var mapPoint = MapControl.ToMapPoint(e.x, e.y);
                        if (!_measureTool.IsSurveying)
                            _measureTool.LengthStart(mapPoint);
                        else
                            _measureTool.AddPoint(mapPoint);
                    }
                    else
                    {
                        if (_measureTool.IsSurveying)
                        {
                            object lineSymbolObj = SymbolCreate.GetSimpleLineSymbol("ff0000");
                            MapControl.DrawShape(_measureTool.SurveyEnd(MapControl.ToMapPoint(e.x, e.y)), ref lineSymbolObj);
                            lblMeasureInfo.Text = $@"总长度：{_measureTool.TotalLength:F2}米{Environment.NewLine}{Environment.NewLine}";
                            lblMeasureInfo.Text += $@"当前长度: {_measureTool.CurrentLength:F2}米";
                            lblMeasureInfo.Refresh();
                            _measureTool.SurveyEnd(MapControl.ToMapPoint(e.x, e.y));
                        }
                    }
                    break;
                case EMapTools.MeasureArea: //测面积
                    MapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;//鼠标指针:十字状
                    lblMeasureTips.Visible = true;
                    if (e.button == 1)
                    {
                        if (!_measureTool.IsSurveying)
                            _measureTool.AreaStart(MapControl.ToMapPoint(e.x, e.y));
                        else
                            _measureTool.AddPoint(MapControl.ToMapPoint(e.x, e.y));
                    }
                    else
                    {
                        if (_measureTool.IsSurveying && _measureTool.AreaPointCount > 1)
                        {
                            object fillSymbolObj = SymbolCreate.GetSimpleFillSymbol("99ccff", "ff0000");
                            MapControl.DrawShape(_measureTool.SurveyEnd(MapControl.ToMapPoint(e.x, e.y)), ref fillSymbolObj);
                            lblMeasureInfo.Text = $@"面积：{_measureTool.Area:#########.##}平方米";
                            lblMeasureInfo.Refresh();
                            _measureTool.SurveyEnd(MapControl.ToMapPoint(e.x, e.y));
                        }
                    }
                    break;
            }
        }

        //鼠标移动测距离/面积
        private void mapControl_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (!_measureTool.IsSurveying) return;

            if (CurrentTool == EMapTools.MeasureDistance)//测距离
            {
                _measureTool.MoveTo(MapControl.ToMapPoint(e.x, e.y));
                lblMeasureInfo.Text = $@"总长度：{_measureTool.TotalLength:#########.##}米{Environment.NewLine}{Environment.NewLine}";
                lblMeasureInfo.Text += $@"当前长度:{_measureTool.CurrentLength:#########.##}米";
                lblMeasureInfo.Refresh();
            }
            else if (CurrentTool == EMapTools.MeasureArea) //测距离
            {
                _measureTool.MoveTo(MapControl.ToMapPoint(e.x, e.y));
                lblMeasureInfo.Text = $@"面积：{_measureTool.Area:#########.##}平方米";
                lblMeasureInfo.Refresh();
            }
        }

        //清空、关闭测量结果
        private void btnMeasureClose_Click(object sender, EventArgs e)
        {
            ToolOnClick(EMapTools.Pan);
            lblMeasureInfo.Visible = false;
            lblMeasureTips.Text = "";
            MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        //点击地图导航条的工具
        private void navigationButton_Click(object sender, EventArgs e)
        {
            CurrentTool = ((SimpleButton)sender).Text.GetEnum<EMapTools>();
            this.lblMeasureTips.Visible = this.lblMeasureInfo.Visible = CurrentTool == EMapTools.MeasureDistance || CurrentTool == EMapTools.MeasureArea;
            this.lblSwipe.Visible = this.cmbLayers.Visible = CurrentTool == EMapTools.Swipe;
            if (CurrentTool == EMapTools.Swipe)
            {
                this.cmbLayers.Properties.Items.Clear();
                this.cmbLayers.Properties.Items.AddRange(this.MapControl.GetLayerNames());
            }

            ICommand command = null;
            switch (CurrentTool)
            {
                case EMapTools.FullExtent: command = new ControlsMapFullExtentCommand(); break;
                case EMapTools.ZoomIn: command = new ControlsMapZoomInTool(); break;
                case EMapTools.ZoomOut: command = new ControlsMapZoomOutTool(); break;
                case EMapTools.Pan: command = new ControlsMapPanTool(); break;
                case EMapTools.PreView: command = new ControlsMapZoomToLastExtentBackCommand(); break;
                case EMapTools.Identify: command = new ControlsMapIdentifyTool(); break;
                case EMapTools.Selection: command = new ControlsSelectFeaturesToolClass(); break;
                case EMapTools.Swipe: command = new ControlsMapSwipeToolClass(); break;
                default: MapControl.CurrentTool = null; break;
            }
            if (command != null)
            {
                command.OnCreate(MapControl.Object);
                if (command is ITool tool)
                    MapControl.CurrentTool = tool;
                else
                    command.OnClick();
            }
        }

        //选择卷帘图层
        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _effectLayer.SwipeLayer = _mapCtrl.get_Layer(this.cmbLayers.SelectedIndex);
        }
    }
}
