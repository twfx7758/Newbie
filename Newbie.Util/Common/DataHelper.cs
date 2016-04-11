using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace Newbie.Util.Common
{
    public sealed class DataHelper
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dt) where T : new()
        {
            using (dt)
            {
                if (Equals(null, dt))
                    return null;

                var prList = new List<PropertyInfo>();

                Array.ForEach(typeof (T).GetProperties(), p =>
                {
                    if (dt.Columns.IndexOf(p.Name) > -1)
                    {
                        prList.Add(p);
                    }
                });

                var list = new List<T>();
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var t = new T();
                    prList.ForEach(
                        p => p.SetValue(t, Equals(DBNull.Value, dt.Rows[i][p.Name]) ? null : dt.Rows[i][p.Name], null));
                    list.Add(t);
                }

                return list;
            }
        }

        /// <summary>
        /// DataReader转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(IDataReader reader) where T : new()
        {
            using (reader)
            {
                if (Equals(null, reader))
                    return null;

                var list = new List<T>();
                var columsStr = new StringBuilder(",");
                var prList = new List<PropertyInfo>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    columsStr.AppendFormat("{0},", reader.GetName(i));
                }
                Array.ForEach(typeof (T).GetProperties(), p =>
                {
                    if (columsStr.ToString().IndexOf(string.Format(",{0},", p.Name),
                        StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        prList.Add(p);
                    }
                });
                while (reader.Read())
                {
                    var t = new T();
                    foreach (var property in prList)
                    {
                        property.SetValue(t, Equals(DBNull.Value, reader[property.Name]) ? null : reader[property.Name],
                            null);
                    }

                    list.Add(t);
                }

                if (!reader.IsClosed)
                    reader.Close();

                return list;
            }
        }

        /// <summary>
        /// DataReader转 dynamic List
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<DynamicEntity> ToList(IDataReader reader)
        {
            using (reader)
            {
                if (Equals(null, reader))
                    return null;

                var list = new List<DynamicEntity>();
                while (reader.Read())
                {
                    var entity = new DynamicEntity();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var key = reader.GetName(i);
                        if (entity.ContainsKey(key))
                            continue;

                        var val = reader.GetValue(i);
                        entity[key] = val;
                    }

                    list.Add(entity);
                }

                if (!reader.IsClosed)
                    reader.Close();

                return list;
            }
        }
    }
}