using Newbie.Util.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Newbie.Util
{
    public partial class DataFormat
    {
        #region 根据sql语句及参数返回实体
        /// <summary>
        /// 根据sql语句及参数返回实体
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static E GetEntityBySql<E>(string connectionString, string sql, params SqlParameter[] parameters) where E : class, new()
        {
            DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, parameters);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return GetEntityByDataRow<E>(ds.Tables[0].Rows[0]);
            }
            else
            {
                ds.Dispose();
                return null;
            }
        }
        #endregion

        #region 根据DataRow返回Entity实体
        /// <summary>
        /// 根据DataRow返回Entity实体
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static E GetEntityByDataRow<E>(DataRow dr) where E : class, new()
        {
            E entity = null;
            if (dr != null)
            {
                entity = new E();
                FormHelper.GetInstance().BindDataRowToEntity(entity, dr);
            }
            return entity;
        }
        #endregion

        #region DataSet To List
        /// <summary>
        /// DataSet转换成List
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<E> DataSetToList<E>(DataSet ds) where E : class, new()
        {
            List<E> list = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<E>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    E entity = new E();
                    FormHelper.GetInstance().BindDataRowToEntity(entity, dr);
                    if (entity != null)
                    {
                        list.Add(entity);
                    }
                }
            }
            return list;
        }
        #endregion

        #region DataTable To List
        /// <summary>
        /// DataSet转换成List
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static List<E> DataTableToList<E>(DataTable dt) where E : class, new()
        {
            List<E> list = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                list = new List<E>();
                foreach (DataRow dr in dt.Rows)
                {
                    E entity = new E();
                    FormHelper.GetInstance().BindDataRowToEntity(entity, dr);
                    if (entity != null)
                    {
                        list.Add(entity);
                    }
                }
            }
            return list;
        }
        #endregion
    }
}
