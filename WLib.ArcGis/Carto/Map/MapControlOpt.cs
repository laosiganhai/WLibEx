﻿/*---------------------------------------------------------------- 
// auth： Windragon
// date： 2018
// desc： None
// mdfy:  None
//----------------------------------------------------------------*/

using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using WLib.ArcGis.GeoDb.FeatClass;
using WLib.ArcGis.GeoDb.WorkSpace;

namespace WLib.ArcGis.Carto.Map
{
    /// <summary>
    /// 向地图控件加载图层的操作
    /// </summary>
    public static class MapControlOpt
    {
        #region 获取图层
        /// <summary>
        /// 根据图层名称，在地图控件上查找对应矢量图层，找不到则返回Null
        /// </summary>
        /// <param name="mapControl"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static IFeatureLayer GetFeatureLayer(this AxMapControl mapControl, string layerName)
        {
            IFeatureLayer featureLayer = null;
            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                ILayer tmpLayer = mapControl.get_Layer(i);
                if (tmpLayer is IFeatureLayer layer && tmpLayer.Name.Equals(layerName))
                {
                    featureLayer = layer;
                    break;
                }
            }
            return featureLayer;
        }
        /// <summary>
        /// 得到地图控件上的所有矢量图层
        /// </summary>
        /// <param name="mapControl"></param>
        /// <returns></returns>
        public static List<IFeatureLayer> GetFeatureLayers(this AxMapControl mapControl)
        {
            List<IFeatureLayer> featureLayers = new List<IFeatureLayer>();
            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                ILayer tmpLayer = mapControl.get_Layer(i);
                if (tmpLayer is IFeatureLayer layer)
                    featureLayers.Add(layer);
            }
            return featureLayers;
        }
        #endregion


        #region 加载图层
        /// <summary>
        /// 将工作空间的所有要素类加载到地图控件中，图层名使用要素类别名
        /// </summary>
        /// <param name="mapControl">地图控件</param>
        /// <param name="strConOrPath">工作空间的路径，或者是sde的连接字符串</param>
        /// <param name="zoomToClass">指定要素类名称或别名，地图将缩放到此要素类的显示范围</param>
        public static void LoadAllFeatureLayers(this AxMapControl mapControl, string strConOrPath, string zoomToClass = null)
        {
            IWorkspace workspace = GetWorkspace.GetWorkSpace(strConOrPath);
            LoadAllFeatureLayers(mapControl, workspace, zoomToClass);
        }
        /// <summary>
        /// 将工作空间的所有要素类加载到地图控件中，图层名使用要素类别名
        /// </summary>
        /// <param name="mapControl">地图控件</param>
        /// <param name="workspace">工作空间</param>
        /// <param name="zoomToClass">指定要素类名称或别名，地图将缩放到此要素类的显示范围</param>
        public static void LoadAllFeatureLayers(this AxMapControl mapControl, IWorkspace workspace, string zoomToClass = null)
        {
            var featureClasses = workspace.GetFeatureClasses();

            featureClasses = featureClasses.SortByGeometryType();

            //将要素类作为图层加入地图控件中
            for (int i = featureClasses.Count - 1; i >= 0; i--)
            {
                var featureClass = featureClasses[i];
                IFeatureLayer layer = new FeatureLayerClass
                {
                    FeatureClass = featureClass,
                    Name = featureClass.AliasName
                };
                if (!string.IsNullOrEmpty(zoomToClass))
                {
                    if (featureClass.AliasName == zoomToClass || ((IDataset)featureClass).Name == zoomToClass)
                        mapControl.ActiveView.Extent = layer.AreaOfInterest;
                }
                mapControl.AddLayer(layer);
            }
        }
        /// <summary>
        /// 将工作空间的指定要素类加载到地图控件中，图层名使用要素类别名
        /// </summary>
        /// <param name="mapControl">地图控件</param>
        /// <param name="workspace">工作空间</param>
        /// <param name="zoomToClass">指定要素类名称或别名，地图将缩放到此要素类的显示范围（此值可空，若非空，则应包含在loadClasses中）</param>
        /// <param name="loadClasses">指定要加载的要素类集合</param>
        public static void LoadFeatureLayers(this AxMapControl mapControl, IWorkspace workspace, string zoomToClass, params string[] loadClasses)
        {
            var featureClasses = workspace.GetFeatureClasses();
            for (int i = 0; i < featureClasses.Count; i++)
            {
                var featureClass = featureClasses[i];
                if (loadClasses.Contains(featureClass.AliasName) || loadClasses.Contains(((IDataset)featureClass).Name))
                {
                    IFeatureLayer layer = new FeatureLayerClass
                    {
                        FeatureClass = featureClass,
                        Name = featureClass.AliasName
                    };
                    mapControl.AddLayer(layer);
                    if (!string.IsNullOrEmpty(zoomToClass))
                    {
                        if (featureClass.AliasName == zoomToClass || ((IDataset)featureClass).Name == zoomToClass)
                            mapControl.ActiveView.Extent = layer.AreaOfInterest;
                    }
                }
            }
        }
        #endregion
    }
}
