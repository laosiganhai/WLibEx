﻿/*---------------------------------------------------------------- 
// auth： Windragon
// date： 2018
// desc： None
// mdfy:  None
//----------------------------------------------------------------*/

namespace WLib.ArcGis.Analysis.Topology
{
    /// <summary>
    /// 中文版的拓扑规则类型枚举，各枚举项对应的整型常量值与esriTopologyRuleType枚举项的常量值是一致的
    /// </summary>
    public enum ECnTopoRuleType
    {
        任何规则 = -1,
        要素大于最小容差 = 0,
        面要素之间无空隙 = 1,
        面要素间无重叠 = 3,
        第二个图层面要素必须被第一个图层任一面要素覆盖 = 4,
        两图层面要素必须互相覆盖 = 5,
        第一个图层面要素必须被第一个图层任一面要素包含 = 7,
        图层间面要素不能相互覆盖 = 8,
        线要素必须跟面图层边界的一部分或全部重叠 = 10,
        线要素必须在面内 = 11,
        点要素必须落在面要素边界上 = 13,
        点要素必须落在面要素内 = 15,
        面要素必须只包含一个点要素 = 16,
        线要素间不能有相互重叠部分 = 19,
        线要素之间不能相交 = 20,
        线要素不允许有悬挂点 = 21,
        线要素不允许有假节点 = 22,
        第一个图层线要素应被第二个线图层线要素覆盖 = 26,
        第一个图层线要素不被第二个线图层线要素覆盖 = 28,
        点要素应被线要素覆盖 = 29,
        点要素应在线要素的端点上 = 31,
        点要素之间不相交 = 34,
        点要素重合点要素 = 35,
        面要素边界必须被线要素覆盖 = 37,
        面要素的边界必须被另一面要素边界覆盖 = 38,
        线要素不能自重叠 = 39,
        线要素不能自相交 = 40,
        线要素间不能重叠和相交 = 41,
        线要素端点必须被点要素覆盖 = 42,
        面要素内必须包含至少一个点要素 = 43,
        线不能是多段 = 44,
        线要素必须不相交 = 45,
        线必须不相交或内部接触 = 46
    };
}
