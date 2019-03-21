﻿/*---------------------------------------------------------------- 
// auth： Windragon
// date： 2018
// desc： None
// mdfy:  None
//----------------------------------------------------------------*/

using System.Collections.Generic;
using System.Data;
using WLib.Db.DbBase;

namespace WLib.Db.TableInfo
{
    /// <summary>
    /// 提供获取字典表的方法
    /// </summary>
    public class DictionaryTableHelper
    {
        /// <summary>
        /// 从数据库中获取字典表
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="tableName">字典表名称</param>
        /// <param name="eDbProviderTyp"></param>
        /// <returns></returns>
        public static DictionaryTable GetDictionaryTable(string connectString, string tableName, EDbProviderType eDbProviderTyp)
        {
            var dictTables = new Dictionary<string, DictionaryTable>();
            if (dictTables.ContainsKey(tableName))
                return dictTables[tableName];

            var tableFields = GetDictionaryTableFromMdb(connectString, tableName, eDbProviderTyp);
            dictTables.Add(tableName, tableFields);
            return tableFields;
        }

        /// <summary>
        /// 从数据库中获取字典表，注意字典表只有两个字段即“编码”、“名称”
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="tableName"></param>
        /// <param name="eDbProviderType"></param>
        /// <returns></returns>
        protected static DictionaryTable GetDictionaryTableFromMdb(string connectString, string tableName, EDbProviderType eDbProviderType)
        {
            var sql = "select 编码,名称 from " + tableName;
            var dbHelper = new DbHelper(connectString, eDbProviderType);
            var dataTable = dbHelper.GetDataTable(sql);

            var dictTable = new DictionaryTable(tableName);
            foreach (DataRow row in dataTable.Rows)
            {
                dictTable.CodeNameDict.Add(row[0].ToString(), row[1].ToString());
            }
            return dictTable;
        }
    }
}