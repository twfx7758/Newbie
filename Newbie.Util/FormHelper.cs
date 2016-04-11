using System;
using System.Globalization;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Text;
using System.Xml;

namespace Newbie.Util
{
    public class FormHelper
    {

        public DataRow GetDataRow1(SqlDataReader sqlDataReader)
        {
            DataTable dt = new DataTable();
            DataTable schemaTable = sqlDataReader.GetSchemaTable();
            foreach (DataRow dr in schemaTable.Rows)
            {
                DataColumn dc = new DataColumn();
                //设置列的数据类型
                dc.DataType = dr[0].GetType();
                //设置列的名称
                dc.ColumnName = dr[0].ToString();
                //将该列添加进构造的表中
                dt.Columns.Add(dc);
            }
            DataRow m_DataRow = dt.NewRow();
            DataSet pDataSet = new DataSet();
            DataRow pDataRow = m_DataRow.Table.NewRow();
            object[] Values = new object[m_DataRow.Table.Columns.Count];
            sqlDataReader.GetValues(Values);
            pDataRow.ItemArray = Values;
            return pDataRow;
        }

        public static FormHelper GetInstance()
        {
            return new FormHelper();
        }

        #region 数据基本操作
        private string _tableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            set
            {
                _tableName = value;
            }
            get
            {
                return _tableName;
            }
        }

        private string _primaryKey;
        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey
        {
            set
            {
                _primaryKey = value;
            }
            get
            {
                return _primaryKey;
            }
        }

        private string _filterExpression;
        /// <summary>
        /// 要用来筛选行的条件
        /// </summary>
        public string FilterExpression
        {
            set
            {
                _filterExpression = value;
            }
            get
            {
                return _filterExpression;
            }
        }

        private string _sort;
        /// <summary>
        /// 排序
        /// </summary>
        public string Sort
        {
            set
            {
                _sort = value;
            }
            get
            {
                return _sort;
            }
        }

        private string _connString;
        /// <summary>
        /// 数据连接字符
        /// </summary>
        public string ConnString
        {
            set
            {
                _connString = value;
            }
            get
            {
                return _connString;
            }
        }
        #endregion

        #region 绑定控件值到存储过程
        /// <summary>
        /// 绑定控件值到存储过程
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="prefix">控件前缀</param>
        /// <param name="sprocName">存储过程名</param>
        /// <param name="dataTable">数据表</param>
        public void BindControlToStoredProcedure(Page page, string prefix, string sprocName, DataTable dataTable)
        {
            string sql = string.Format("SELECT sys.sysobjects.name AS objectName, sys.syscolumns.name AS columnName, sys.systypes.name AS columnType FROM sys.sysobjects LEFT OUTER JOIN sys.syscolumns ON sys.sysobjects.id = sys.syscolumns.id LEFT OUTER JOIN sys.systypes ON sys.syscolumns.xtype = sys.systypes.xusertype WHERE (USER_NAME(sys.sysobjects.uid) = 'dbo') AND (sys.sysobjects.type = 'P') AND (sys.sysobjects.name = '{0}') ORDER BY objectName", sprocName);

            //取存储过程名及参数
            DataTable tblObjects = new DataTable();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, ConnString);
            sqlDataAdapter.Fill(tblObjects);

            if (tblObjects.Rows.Count > 0)
            {
                //返回结果集
                SqlCommand command = new SqlCommand(tblObjects.Rows[0]["objectName"].ToString(), new SqlConnection(ConnString));
                command.CommandType = CommandType.StoredProcedure;
                foreach (DataRow row in tblObjects.Rows)
                {
                    if (row["columnName"] != System.DBNull.Value)
                    {
                        Control control = page.FindControl(string.Concat(prefix, row["columnName"].ToString().Remove(0, 1)));
                        if (control == null)
                        {
                            command.Parameters.Add(row["columnName"].ToString(), ReType(row["columnType"].ToString()));
                            command.Parameters[row["columnName"].ToString()].Value = System.DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.Add(row["columnName"].ToString(), ReType(row["columnType"].ToString()));
                            command.Parameters[row["columnName"].ToString()].Value = GetControlValue(control);
                        }
                    }
                }
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(dataTable);
            }
        }

        /// <summary>
        /// 绑定控件值到存储过程
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="prefix">控件前缀</param>
        /// <param name="sprocName">存储过程名</param>
        /// <param name="dataTable">数据表</param>
        public void BindControlToStoredProcedure(Control parentControl, string prefix, string sprocName, DataTable dataTable)
        {
            string sql = string.Format("SELECT sys.sysobjects.name AS objectName, sys.syscolumns.name AS columnName, sys.systypes.name AS columnType FROM sys.sysobjects INNER JOIN sys.syscolumns ON sys.sysobjects.id = sys.syscolumns.id INNER JOIN sys.systypes ON sys.syscolumns.xtype = sys.systypes.xusertype WHERE (USER_NAME(sys.sysobjects.uid) = 'dbo') AND (sys.sysobjects.type = 'P') AND (sys.sysobjects.name = '{0}') ORDER BY objectName", sprocName);

            //取存储过程名及参数
            DataTable tblObjects = new DataTable();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, ConnString);
            sqlDataAdapter.Fill(tblObjects);

            if (tblObjects.Rows.Count > 0)
            {
                //返回结果集
                SqlCommand command = new SqlCommand(tblObjects.Rows[0]["objectName"].ToString(), new SqlConnection(ConnString));
                command.CommandType = CommandType.StoredProcedure;
                foreach (DataRow row in tblObjects.Rows)
                {
                    if (row["columnName"] != System.DBNull.Value)
                    {
                        Control control = parentControl.FindControl(string.Concat(prefix, row["columnName"].ToString().Remove(0, 1)));
                        if (control == null)
                        {
                            command.Parameters.Add(row["columnName"].ToString(), ReType(row["columnType"].ToString()));
                            command.Parameters[row["columnName"].ToString()].Value = System.DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.Add(row["columnName"].ToString(), ReType(row["columnType"].ToString()));
                            command.Parameters[row["columnName"].ToString()].Value = GetControlValue(control);
                        }
                    }
                }
                sqlDataAdapter.SelectCommand = command;
                sqlDataAdapter.Fill(dataTable);
            }
        }
        #endregion

        #region 绑定实体到DataRow
        /// <summary>
        /// 绑定实体到DataRow
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="dataRow">数据行</param>
        public void BindEntityToDataRow(object entity, DataRow dataRow)
        {
            if (entity == null) return;

            PropertyInfo[] objPropertiesArray = entity.GetType().GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                if (!dataRow.Table.Columns.Contains(objProperty.Name)) continue;

                dataRow[objProperty.Name] = objProperty.GetValue(entity, null);
            }
        }
        #endregion

        #region 绑定DataRow到实体
        /// <summary>
        /// 绑定DataRow到实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="dataRow">数据行</param>
        public void BindDataRowToEntity(object entity, DataRow dataRow)
        {
            if (entity == null) return;

            PropertyInfo[] objPropertiesArray = entity.GetType().GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                if (!dataRow.Table.Columns.Contains(objProperty.Name)) continue;

                if (dataRow[objProperty.Name] != System.DBNull.Value)
                {
                    #region 特殊类型
                    if (objProperty.PropertyType == typeof(XmlDocument))
                    {
                        XmlDocument xml = new XmlDocument();
                        var val = dataRow[objProperty.Name].ToString();
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            xml.LoadXml(val);
                            objProperty.SetValue(entity, (object)xml, null);
                        }
                        continue;
                    }
                    #endregion
                    objProperty.SetValue(entity, Convert.ChangeType(dataRow[objProperty.Name], objProperty.PropertyType), null);

                }
            }
        }
        #endregion

        #region 绑定控件值到DataRow
        /// <summary>
        /// 绑定控件值到DataRow
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="dataRow">数据行</param>
        public void BindControlToDataRow(Page page, DataRow dataRow)
        {
            BindControlToDataRow(page, "XX", dataRow);
        }

        /// <summary>
        /// 绑定控件值到DataRow
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="prefix">前缀</param>
        /// <param name="dataRow">数据行</param>
        public void BindControlToDataRow(Page page, string prefix, DataRow dataRow)
        {
            foreach (DataColumn col in dataRow.Table.Columns)
            {
                Control control = page.FindControl(string.Concat(prefix, col.ColumnName));
                if (control == null) continue;

                dataRow[col.ColumnName] = GetControlValue(control);
            }
        }

        /// <summary>
        /// 绑定控件值到DataRow
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="dataRow">数据行</param>
        public void BindControlToDataRow(Control parentControl, DataRow dataRow)
        {
            BindControlToDataRow(parentControl, "XX", dataRow);
        }

        /// <summary>
        /// 绑定控件值到DataRow
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="prefix">前缀</param>
        /// <param name="dataRow">数据行</param>
        public void BindControlToDataRow(Control parentControl, string prefix, DataRow dataRow)
        {
            foreach (DataColumn col in dataRow.Table.Columns)
            {
                Control control = parentControl.FindControl(string.Concat(prefix, col.ColumnName));
                if (control == null) continue;

                dataRow[col.ColumnName] = GetControlValue(control);
            }
        }
        #endregion

        #region 绑定DataRow到控件值
        /// <summary>
        /// 绑定DataRow到控件值
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="dataRow">数据行</param>
        public void BindDataRowToControl(Page page, DataRow dataRow)
        {
            BindDataRowToControl(page, "XX", dataRow);
        }

        /// <summary>
        /// 绑定DataRow到控件值
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="prefix">前缀</param>
        /// <param name="dataRow">数据行</param>
        public void BindDataRowToControl(Page page, string prefix, DataRow dataRow)
        {
            foreach (DataColumn col in dataRow.Table.Columns)
            {
                if (dataRow[col.ColumnName] != System.DBNull.Value)
                {
                    Control control = page.FindControl(string.Concat(prefix, col.ColumnName));
                    if (control == null) continue;

                    SetControlValue(control, dataRow[col.ColumnName]);
                }
            }
        }

        /// <summary>
        /// 绑定DataRow到控件值
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="dataRow">数据行</param>
        public void BindDataRowToControl(Control parentControl, DataRow dataRow)
        {
            BindDataRowToControl(parentControl, "XX", dataRow);
        }

        /// <summary>
        /// 绑定DataRow到控件值
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="prefix">前缀</param>
        /// <param name="dataRow">数据行</param>
        public void BindDataRowToControl(Control parentControl, string prefix, DataRow dataRow)
        {
            foreach (DataColumn col in dataRow.Table.Columns)
            {
                if (dataRow[col.ColumnName] != System.DBNull.Value)
                {
                    Control control = parentControl.FindControl(string.Concat(prefix, col.ColumnName));
                    if (control == null) continue;

                    SetControlValue(control, dataRow[col.ColumnName]);
                }
            }
        }
        #endregion

        #region 绑定控件值到实体
        /// <summary>
        /// 绑定控件值到实体
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="entity">实体</param>
        public void BindControlToEntity(Page page, object entity)
        {
            BindControlToEntity(page, "XX", entity);
        }

        /// <summary>
        /// 绑定控件值到实体
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="prefix">前缀</param>
        /// <param name="entity">实体</param>
        public void BindControlToEntity(Page page, string prefix, object entity)
        {
            if (entity == null) return;

            PropertyInfo[] objPropertiesArray = entity.GetType().GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                Control control = page.FindControl(string.Concat(prefix, objProperty.Name));
                if (control == null) continue;

                //objProperty.SetValue(entity, Convert.ChangeType(GetControlValue(control), objProperty.PropertyType), null);//原来的方法

                object value = null;
                object ctrlValue = null;
                ctrlValue = GetControlValue(control);
                try
                {
                    #region 数据类型判断
                    if (objProperty.PropertyType == typeof(DateTime))
                    {
                        if (ctrlValue == null || string.IsNullOrEmpty(ctrlValue.ToString()))
                        {
                            value = (object)Convert.ToDateTime("0001-01-01");
                        }
                    }
                    else if (objProperty.PropertyType == typeof(Guid))
                    {
                        if (ctrlValue != null && !string.IsNullOrEmpty(ctrlValue.ToString()))
                        {
                            value = (object)(new Guid(ctrlValue.ToString()));
                        }

                    }
                    else if (objProperty.PropertyType == typeof(XmlDocument))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(ctrlValue.ToString());
                        value = (object)xml;
                    }
                    else
                    {
                        //当遇到PropertyType为typeof(int)或typeof(DateTime)等数值型,且GetControlValue(control)为空时,此处将会有错误
                        value = Convert.ChangeType(ctrlValue, objProperty.PropertyType);
                    }
                    #endregion
                }
                catch
                {
                    #region 2011.12.07
                    //if (objProperty.PropertyType == typeof(DateTime))
                    //{
                    //    value = (object)Convert.ToDateTime("0001-01-01");
                    //}else if (objProperty.PropertyType == typeof(Guid))
                    //{
                    //    value = (object)(new Guid(GetControlValue(control).ToString()));
                    //}
                    //else if (objProperty.PropertyType == typeof(XmlDocument))
                    //{
                    //    XmlDocument xml = new XmlDocument();
                    //    xml.LoadXml(ctrlValue.ToString());
                    //    value = (object)xml;
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                    #endregion
                    continue;
                }

                //只设置能写的属性值
                //if (objProperty.CanWrite)
                if (value != null)
                {
                    objProperty.SetValue(entity, value, null);//原来的方法
                }

                //SetPropertyValue(entity, objProperty, control);//新的方法:增加了try{}catch{}--存在bug
            }
        }

        /// <summary>
        /// 绑定控件值到实体
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="entity">实体</param>
        public void BindControlToEntity(Control parentControl, object entity)
        {
            BindControlToEntity(parentControl, "XX", entity);
        }

        /// <summary>
        /// 绑定控件值到实体
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="prefix">前缀</param>
        /// <param name="entity">实体</param>
        public void BindControlToEntity(Control parentControl, string prefix, object entity)
        {
            if (entity == null) return;

            PropertyInfo[] objPropertiesArray = entity.GetType().GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                Control control = parentControl.FindControl(string.Concat(prefix, objProperty.Name));
                if (control == null) continue;

                //objProperty.SetValue(entity, Convert.ChangeType(GetControlValue(control), objProperty.PropertyType), null);//原来的方法

                object value = null;
                object ctrlValue = null;
                ctrlValue = GetControlValue(control);
                try
                {
                    #region 数据类型判断
                    if (objProperty.PropertyType == typeof(DateTime))
                    {
                        if (ctrlValue == null || string.IsNullOrEmpty(ctrlValue.ToString()))
                        {
                            value = (object)Convert.ToDateTime("0001-01-01");
                        }
                    }
                    else if (objProperty.PropertyType == typeof(Guid))
                    {
                        if (ctrlValue != null && !string.IsNullOrEmpty(ctrlValue.ToString()))
                        {
                            value = (object)(new Guid(ctrlValue.ToString()));
                        }

                    }
                    else if (objProperty.PropertyType == typeof(XmlDocument))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(ctrlValue.ToString());
                        value = (object)xml;
                    }
                    else
                    {
                        //当遇到PropertyType为typeof(int)或typeof(DateTime)等数值型,且GetControlValue(control)为空时,此处将会有错误
                        value = Convert.ChangeType(ctrlValue, objProperty.PropertyType);
                    }
                    #endregion
                }
                catch
                {
                    #region 2011.12.07
                    //if (objProperty.PropertyType == typeof(DateTime))
                    //{
                    //    value = (object)Convert.ToDateTime("0001-01-01");
                    //}else if (objProperty.PropertyType == typeof(Guid))
                    //{
                    //    value = (object)(new Guid(GetControlValue(control).ToString()));
                    //}
                    //else if (objProperty.PropertyType == typeof(XmlDocument))
                    //{
                    //    XmlDocument xml = new XmlDocument();
                    //    xml.LoadXml(ctrlValue.ToString());
                    //    value = (object)xml;
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                    #endregion
                    continue;
                }

                //只设置能写的属性值
                //if (objProperty.CanWrite)
                if (value != null)
                {
                    objProperty.SetValue(entity, value, null);//原来的方法
                }

                //SetPropertyValue(entity, objProperty, control);//新的方法:增加了try{}catch{}--存在bug
            }
        }
        #endregion

        #region 绑定实体到控件值
        /// <summary>
        /// 绑定控件值到实体
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="entity">实体</param>
        public void BindEntityToControl(Page page, object entity)
        {
            BindEntityToControl(page, "XX", entity);
        }

        /// <summary>
        /// 绑定实体到控件值
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="prefix">前缀</param>
        /// <param name="entity">实体</param>
        public void BindEntityToControl(Page page, string prefix, object entity)
        {
            if (entity == null) return;

            PropertyInfo[] objPropertiesArray = entity.GetType().GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                Control control = page.FindControl(string.Concat(prefix, objProperty.Name));
                if (control == null) continue;

                SetControlValue(control, objProperty.GetValue(entity, null));
            }
        }

        /// <summary>
        /// 绑定实体到控件值
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="entity">实体</param>
        public void BindEntityToControl(Control parentControl, object entity)
        {
            BindControlToEntity(parentControl, "XX", entity);
        }

        /// <summary>
        /// 绑定实体到控件值
        /// </summary>
        /// <param name="parentControl">父控件</param>
        /// <param name="prefix">前缀</param>
        /// <param name="entity">实体</param>
        public void BindEntityToControl(Control parentControl, string prefix, object entity)
        {
            if (entity == null) return;

            PropertyInfo[] objPropertiesArray = entity.GetType().GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                Control control = parentControl.FindControl(string.Concat(prefix, objProperty.Name));
                if (control == null) continue;

                SetControlValue(control, objProperty.GetValue(entity, null));
            }
        }
        #endregion

        #region 设置控件状态
        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="page">页</param>
        /// <param name="readOnly">是否只读</param>
        /// <param name="prefix">前缀</param>
        public void BindControlForStatus(Page page, bool readOnly, string prefix)
        {
            foreach (Control control in page.Controls)
            {
                BindControlForStatus(control, readOnly, prefix);
            }
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="readOnly">是否只读</param>
        /// <param name="prefix">前缀</param>
        public void BindControlForStatus(Control control, bool readOnly, string prefix)
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control c in control.Controls)
                {
                    if (c.ID != null)
                    {
                        if (c.ID.StartsWith(prefix))
                        {
                            SetControlStatus(c, readOnly);
                        }
                    }
                    BindControlForStatus(c, readOnly, prefix);
                }
            }
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <param name="readOnly">是否只读</param>
        private void SetControlStatus(Control inputControl, bool readOnly)
        {
            if (inputControl == null)
            {
                return;
            }

            switch (inputControl.GetType().Name)
            {
                case "TextBox":
                    TextBox txt = (TextBox)inputControl;
                    txt.ReadOnly = readOnly;
                    break;
                case "CheckBox":
                    CheckBox chk = (CheckBox)inputControl;
                    chk.Enabled = !readOnly;
                    break;
                case "RadioButton":
                    RadioButton rad = (RadioButton)inputControl;
                    rad.Enabled = !readOnly;
                    break;
                case "RadioButtonList":
                    RadioButtonList radl = (RadioButtonList)inputControl;
                    radl.Enabled = !readOnly;
                    break;
                case "CheckBoxList":
                    CheckBoxList chkl = (CheckBoxList)inputControl;
                    chkl.Enabled = !readOnly;
                    break;
                case "ListBox":
                    ListBox lst = (ListBox)inputControl;
                    lst.Enabled = !readOnly;
                    break;
                case "DropDownList":
                    DropDownList drop = (DropDownList)inputControl;
                    drop.Enabled = !readOnly;
                    break;
                default:
                    FindAndSetControlStatus(inputControl, readOnly);
                    break;
            }
            WebControl webctl = (WebControl)inputControl;
            if (readOnly)
            {

                if (!webctl.CssClass.Contains("readonly"))
                {
                    webctl.CssClass = webctl.CssClass.Trim() + " readonly";
                }
            }
            else
            {
                if (webctl.CssClass.Contains("readonly"))
                {
                    webctl.CssClass.Replace("readonly", "");
                }
            }
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <param name="readOnly">是否只读</param>
        private void FindAndSetControlStatus(Control inputControl, bool readOnly)
        {
            Type controlType = inputControl.GetType();
            PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

            bool success;
            success = FindAndSetControlValue(inputControl, controlPropertiesArray, "ReadOnly", typeof(bool), readOnly);

            if (!success)
                success = FindAndSetControlValue(inputControl, controlPropertiesArray, "Enabled", typeof(bool), !readOnly);
        }
        #endregion

        #region 设置控件属性
        /// <summary>
        /// 设置控件属性
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="controlsType">控件类型</param>
        /// <param name="controlsName">控件名称</param>
        /// <param name="controlsAttribute">控件属性</param>
        /// <param name="allowed">控件权限</param>
        public void SetControlAttribute(Control control, string controlsType, string controlsName, string controlsAttribute, object allowed)
        {
            try
            {
                string assemblyQualifiedName = typeof(Control).AssemblyQualifiedName;
                string assemblyInformation = assemblyQualifiedName.Substring(assemblyQualifiedName.IndexOf(","));
                Type type = Type.GetType(controlsType + assemblyInformation);
                PropertyInfo propertyInfo = type.GetProperty(controlsAttribute);
                Control controlFindControl = control.FindControl(controlsName);
                propertyInfo.SetValue(controlFindControl, allowed, null);
            }
            catch
            {
            }
        }
        #endregion

        #region 设置控件值
        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <param name="inputValue">输入值</param>
        private void SetControlValue(Control inputControl, object inputValue)
        {
            if (inputControl == null)
            {
                return;
            }

            if (inputValue == null)
            {
                return;
            }
            string value = inputValue != null ? inputValue.ToString().ToLower() : "";
            switch (inputControl.GetType().Name)
            {
                case "TextBox":
                    TextBox txt = (TextBox)inputControl;
                    txt.Text = inputValue.ToString();
                    break;
                case "CheckBox":
                    CheckBox chk = (CheckBox)inputControl;
                    //chk.Checked = (bool)inputValue;
                    if (value == "true" || value == "1")
                        chk.Checked = true;
                    else
                        chk.Checked = false;
                    break;
                case "RadioButton":
                    RadioButton rad = (RadioButton)inputControl;
                    //rad.Checked = (bool)inputValue;
                    if (value == "true" || value == "1")
                        rad.Checked = true;
                    else
                        rad.Checked = false;
                    break;
                case "RadioButtonList":
                    RadioButtonList radl = (RadioButtonList)inputControl;
                    radl.SelectedValue = inputValue.ToString();
                    break;
                case "CheckBoxList":
                    CheckBoxList chkl = (CheckBoxList)inputControl;
                    chkl.SelectedValue = inputValue.ToString();
                    break;
                case "ListBox":
                    ListBox lst = (ListBox)inputControl;
                    lst.SelectedValue = inputValue.ToString();
                    break;
                case "DropDownList":
                    DropDownList drop = (DropDownList)inputControl;
                    drop.SelectedValue = inputValue.ToString();
                    break;

                #region 自定义Web控件
                //case "OperationCheckBoxList":
                //    WebControls.OperationCheckBoxList ocbl = (WebControls.OperationCheckBoxList)inputControl;
                //    ocbl.Operation = inputValue.ToString();
                //   break;
                #endregion

                default:
                    FindAndSetControlValue(inputControl, inputValue);
                    break;
            }
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <param name="inputValue">输入值</param>
        private void FindAndSetControlValue(Control inputControl, object inputValue)
        {
            Type controlType = inputControl.GetType();
            PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

            bool success;
            success = FindAndSetControlValue(inputControl, controlPropertiesArray, "SelectedDate", typeof(DateTime), inputValue);

            if (!success)
                success = FindAndSetControlValue(inputControl, controlPropertiesArray, "SelectedValue", typeof(string), inputValue);

            if (!success)
                success = FindAndSetControlValue(inputControl, controlPropertiesArray, "Checked", typeof(bool), inputValue);

            if (!success)
                success = FindAndSetControlValue(inputControl, controlPropertiesArray, "Value", typeof(string), inputValue);

            if (!success)
                success = FindAndSetControlValue(inputControl, controlPropertiesArray, "Text", typeof(string), inputValue);
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="type">属性类型</param>
        /// <param name="inputValue">输入值</param>
        private void FindAndSetControlValue(Control inputControl, string propertyName, Type type, object inputValue)
        {
            PropertyInfo[] controlPropertiesArray = inputControl.GetType().GetProperties();

            FindAndSetControlValue(inputControl, controlPropertiesArray, propertyName, type, inputValue);
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <param name="controlPropertiesArray">控件属性集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="type">属性类型</param>
        /// <param name="inputValue">输入值</param>
        /// <returns>是否设置成功</returns>
        private bool FindAndSetControlValue(Control inputControl, PropertyInfo[] controlPropertiesArray, string propertyName, Type type, object inputValue)
        {
            foreach (PropertyInfo controlProperty in controlPropertiesArray)
            {
                if (controlProperty.Name.ToUpper() == propertyName.ToUpper() && controlProperty.PropertyType == type)
                {
                    controlProperty.SetValue(inputControl, Convert.ChangeType(inputValue, type), null);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 获取控件值
        /// <summary>
        /// 获取控件值
        /// </summary>
        /// <param name="inputControl">控件</param>
        private object GetControlValue(Control inputControl)
        {
            object ret;

            if (inputControl == null)
            {
                return null;
            }

            switch (inputControl.GetType().Name)
            {
                case "TextBox":
                    ret = ((TextBox)inputControl).Text;
                    break;
                case "CheckBox":
                    ret = ((CheckBox)inputControl).Checked;
                    break;
                case "RadioButton":
                    ret = ((RadioButton)inputControl).Checked;
                    break;
                case "RadioButtonList":
                    ret = ((RadioButtonList)inputControl).SelectedValue;
                    break;
                case "CheckBoxList":
                    ret = ((CheckBoxList)inputControl).SelectedValue;
                    break;
                case "ListBox":
                    ret = ((ListBox)inputControl).SelectedValue;
                    break;
                case "DropDownList":
                    ret = ((DropDownList)inputControl).SelectedValue;
                    break;

                #region 自定义Web控件
                //case "OperationCheckBoxList":
                //    ret = ((WebControls.OperationCheckBoxList)inputControl).Operation;
                //    break;
                #endregion

                default:
                    ret = FindAndGetControlValue(inputControl);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 获取控件值
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <returns>控件值</returns>
        private object FindAndGetControlValue(Control inputControl)
        {
            PropertyInfo[] controlPropertiesArray = inputControl.GetType().GetProperties();

            object ret;
            ret = FindAndGetControlValue(inputControl, controlPropertiesArray, "SelectedDate", typeof(DateTime));

            if (ret == null)
                ret = FindAndGetControlValue(inputControl, controlPropertiesArray, "SelectedValue", typeof(string));

            if (ret == null)
                ret = FindAndGetControlValue(inputControl, controlPropertiesArray, "Checked", typeof(bool));

            if (ret == null)
                ret = FindAndGetControlValue(inputControl, controlPropertiesArray, "Value", typeof(string));

            if (ret == null)
                ret = FindAndGetControlValue(inputControl, controlPropertiesArray, "Text", typeof(string));

            return ret;
        }

        /// <summary>
        /// 获取控件值
        /// </summary>
        /// <param name="inputControl">控件</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="type">属性类型</param>
        /// <returns>控件值</returns>
        private object FindAndGetControlValue(Control inputControl, string propertyName, Type type)
        {
            PropertyInfo[] controlPropertiesArray = inputControl.GetType().GetProperties();

            object ret;
            ret = FindAndGetControlValue(inputControl, controlPropertiesArray, propertyName, type);

            return ret;
        }

        /// <summary>
        /// 获取控件值
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="controlPropertiesArray">控件属性集合</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="type">属性类型</param>
        /// <returns>控件值</returns>
        private object FindAndGetControlValue(Control control, PropertyInfo[] controlPropertiesArray, string propertyName, Type type)
        {
            object ret = null;
            foreach (PropertyInfo controlProperty in controlPropertiesArray)
            {
                if (controlProperty.Name == propertyName && controlProperty.PropertyType == type)
                {
                    ret = controlProperty.GetValue(control, null);
                }
            }
            return ret;
        }
        #endregion

        #region 公共常量,方法,属性
        private string _errorMessage;
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage
        {
            set { _errorMessage = value; }
            get { return _errorMessage; }
        }

        /// <summary>
        /// 根据存储过程类型字符串得到SqlDbType类型
        /// </summary>
        /// <param name="inputType">类型字符串</param>
        /// <returns>SqlDbType类型</returns>
        private SqlDbType ReType(string inputType)
        {
            SqlDbType dbType = SqlDbType.VarChar;
            switch (inputType.ToLower())
            {
                case "bit":
                    {
                        dbType = SqlDbType.Bit;
                        break;
                    }
                case "char":
                    {
                        dbType = SqlDbType.Char;
                        break;
                    }
                case "datetime":
                    {
                        dbType = SqlDbType.DateTime;
                        break;
                    }
                case "float":
                    {
                        dbType = SqlDbType.Float;
                        break;
                    }
                case "int":
                    {
                        dbType = SqlDbType.Int;
                        break;
                    }
                case "text":
                    {
                        dbType = SqlDbType.Text;
                        break;
                    }
                case "tinyint":
                    {
                        dbType = SqlDbType.TinyInt;
                        break;
                    }
                case "varchar":
                    {
                        dbType = SqlDbType.VarChar;
                        break;
                    }
                case "nvarchar":
                    {
                        dbType = SqlDbType.NVarChar;
                        break;
                    }
                case "uniqueidentifier":
                    {
                        dbType = SqlDbType.UniqueIdentifier;
                        break;
                    }
                case "timestamp":
                    {
                        dbType = SqlDbType.Timestamp;
                        break;
                    }
                case "image":
                    {
                        dbType = SqlDbType.Image;
                        break;
                    }
            }
            return dbType;
        }
        #endregion




        #region Gin Zhang 增加的方法(2009-01-17)

        #region 设置实体类当前属性的值
        /// <summary>
        /// 设置实体类当前属性的值
        /// </summary>
        /// <param name="objProperty">当前entity实体的属性</param>
        /// <param name="value">从Control控件获取到的值</param>
        /// <param name="entity">当前entity实体</param>
        public void SetPropertyValue(PropertyInfo objProperty, object value, object entity)
        {
            if (entity == null) return;

            #region int
            if (objProperty.PropertyType == typeof(Int32))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    int i;
                    try
                    {
                        i = Convert.ToInt32(value);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region long
            else if (objProperty.PropertyType == typeof(Int64))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    long i;
                    try
                    {
                        i = Convert.ToInt64(value);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region float
            else if (objProperty.PropertyType == typeof(Single))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    float i;
                    try
                    {
                        i = Convert.ToSingle(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region double
            else if (objProperty.PropertyType == typeof(Double))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    double i;
                    try
                    {
                        i = Convert.ToDouble(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region decimal
            else if (objProperty.PropertyType == typeof(Decimal))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    decimal i;
                    try
                    {
                        i = Convert.ToDecimal(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region short
            else if (objProperty.PropertyType == typeof(Int16))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    short i;
                    try
                    {
                        i = Convert.ToInt16(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region bool
            else if (objProperty.PropertyType == typeof(Boolean))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    bool i;
                    try
                    {
                        i = Convert.ToBoolean(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region byte
            else if (objProperty.PropertyType == typeof(Byte))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    byte i;
                    try
                    {
                        i = Convert.ToByte(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region char
            else if (objProperty.PropertyType == typeof(Char))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    char i;
                    try
                    {
                        i = Convert.ToChar(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region DateTime
            else if (objProperty.PropertyType == typeof(DateTime))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    DateTime i;
                    try
                    {
                        i = Convert.ToDateTime(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region sbyte
            else if (objProperty.PropertyType == typeof(SByte))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    sbyte i;
                    try
                    {
                        i = Convert.ToSByte(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region ushort
            else if (objProperty.PropertyType == typeof(UInt16))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    ushort i;
                    try
                    {
                        i = Convert.ToUInt16(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region uint
            else if (objProperty.PropertyType == typeof(UInt32))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    uint i;
                    try
                    {
                        i = Convert.ToUInt32(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region ulong
            else if (objProperty.PropertyType == typeof(UInt64))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    ulong i;
                    try
                    {
                        i = Convert.ToUInt64(value);
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            #region Guid
            else if (objProperty.PropertyType == typeof(Guid))
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    Guid i;
                    try
                    {
                        i = new Guid(value.ToString());
                        objProperty.SetValue(entity, i, null);
                    }
                    catch { return; }
                    objProperty.SetValue(entity, i, null);
                }
            }
            #endregion

            else
            {
                object i;
                try
                {
                    i = Convert.ChangeType(value, objProperty.PropertyType);

                }
                catch { return; }
                objProperty.SetValue(entity, i, null);
            }



        }

        /// <summary>
        /// 设置实体类当前属性的值
        /// </summary>
        /// <param name="objProperty">当前entity实体的属性</param>
        /// <param name="control">当前实体属性相应的Control控件</param>
        /// <param name="entity">当前entity实体</param>
        public void SetPropertyValue(PropertyInfo objProperty, Control control, object entity)
        {
            object value = GetControlValue(control);
            SetPropertyValue(objProperty, value, entity);
        }

        /// <summary>
        /// 设置实体类当前属性的值
        /// </summary>
        /// <param name="entity">当前entity实体</param>
        /// <param name="objProperty">当前entity实体的属性</param>
        /// <param name="value">从Control控件获取到的值</param>
        public void SetPropertyValue(object entity, PropertyInfo objProperty, object value)
        {
            try
            {
                objProperty.SetValue(entity, Convert.ChangeType(value, objProperty.PropertyType), null);
            }
            catch { }
        }

        /// <summary>
        /// 设置实体类当前属性的值
        /// </summary>
        /// <param name="entity">当前entity实体</param>
        /// <param name="objProperty">当前entity实体的属性</param>
        /// <param name="control">当前实体属性相应的Control控件</param>
        public void SetPropertyValue(object entity, PropertyInfo objProperty, Control control)
        {
            try
            {
                object value = GetControlValue(control);
                objProperty.SetValue(entity, Convert.ChangeType(value, objProperty.PropertyType), null);
            }
            catch { }
        }
        #endregion

        #endregion

        #region 绑定Request的值到实体
        public void BindRequestParamsToEntity(string prefix, MethodType method, object entity)
        {
            if (entity == null) return;

            HttpRequest request = HttpContext.Current.Request;
            string[] list = null;//参数名称列表
            System.Collections.Specialized.NameValueCollection Params = null;
            if (method == MethodType.Post)
            {
                list = request.Form.AllKeys;
                Params = request.Form;
            }
            else if (method == MethodType.Get)
            {
                list = request.QueryString.AllKeys;
                Params = request.QueryString;
            }
            else
            {
                list = request.Params.AllKeys;
                Params = request.Params;
            }

            PropertyInfo[] objPropertiesArray = entity.GetType().GetProperties();
            foreach (PropertyInfo objProperty in objPropertiesArray)
            {
                string key = string.Concat(prefix, objProperty.Name);
                if (!IsContainsKey(list, key)) continue;

                string value = Params[key];
                if (value != null)
                {
                    //value = System.Web.HttpUtility.UrlDecode(value, Encoding.Default);
                    //对于Boolean类型的,要特殊判断
                    if (objProperty.PropertyType == typeof(Boolean))
                    {
                        value = value.ToLower();
                        if (value == "1" || value == "true")
                        {
                            value = "True";
                        }
                        else if (value == "0" || value == "false")
                        {
                            value = "False";
                        }
                        else
                        {
                            value = "False";
                        }
                    }

                    try
                    {
                        if (objProperty.PropertyType == typeof(Guid))
                        {
                            objProperty.SetValue(entity, (object)new Guid(value), null);
                            continue;
                        }
                        else if (objProperty.PropertyType == typeof(XmlDocument))
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                XmlDocument xml = new XmlDocument();
                                xml.LoadXml(value);
                                objProperty.SetValue(entity, (object)xml, null);
                            }
                            continue;
                        }
                        objProperty.SetValue(entity, Convert.ChangeType(value, objProperty.PropertyType), null);
                    }
                    catch (Exception ex)
                    {
                        //TKF.ConsoleInfo.showinfo.ShowInfo("FormHelper Error:" + ex.ToString(), true);
                    }

                }

            }
        }

        /// <summary>
        /// 检测str是否包含在list列表里
        /// </summary>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsContainsKey(string[] list, string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            foreach (string s in list)
            {
                if (!string.IsNullOrEmpty(s) && s.ToLower() == key.ToLower())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Post类型
        /// </summary>
        public enum MethodType
        {
            /// <summary>
            /// 全部
            /// </summary>
            All = 0,

            /// <summary>
            /// Post方法
            /// </summary>
            Post = 1,

            /// <summary>
            /// Get方法
            /// </summary>
            Get = 2
        }
        #endregion

    }
}
