﻿/*---------------------------------------------------------------- 
// auth： Windragon
// date： 2019/3
// desc： None
// mdfy:  None
//----------------------------------------------------------------*/

using WLib.Attributes;

namespace WLib.ArcGis.Control.MapAssociation
{
    /// <summary>
    /// 测量类型（0-空操作；1-测距离； 2-测面积； 3-测角度）
    /// </summary>
    public enum EMeasureType
    {
        /// <summary>
        /// 0-空操作
        /// </summary>
        [Description("空操作")]
        None = 0,
        /// <summary>
        /// 1-测距离
        /// </summary>
        [Description("测距离")]
        Distance = 1,
        /// <summary>
        /// 2-测面积
        /// </summary>
        [Description("测面积")]
        Area = 2,
        /// <summary>
        /// 3-测角度
        /// </summary>
        [Description("测角度")]
        Angle = 3
    }
}
