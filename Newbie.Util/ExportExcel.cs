using System;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.IO;
using System.Web;

namespace Newbie.Util
{
    public class ExportExcel
    {
        const double cnt = 50000.0;
        HSSFWorkbook workbook = null;

        public ExportExcel()
        {
            this.workbook = new HSSFWorkbook();
        }

        public void Export(Dictionary<string, string> dic, DataTable dt)
        {
            int count = dt.Rows.Count;
            double sheetNum = count / cnt;
            int num = (int)Math.Ceiling(sheetNum);

            for (int i = 0; i < num; i++)
            {
                ISheet sheet = workbook.CreateSheet(string.Format("sheet{0}", i + 1));
                //生成excel的标题
                this.ExcelTitle(dic, sheet);
                //生成excel的内容
                DataTable dtClone = SplitDataTable(dt, i + 1, (int)cnt);
                this.ExcelData(dic, dtClone, sheet);
            }

            //写入内存
            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            //生成文件名
            string filename = string.Format("{0}.xls", DateTime.Now.ToString("yyyyMMddHHmmss"));
            ExportFromResponse(filename, file.GetBuffer());
        }

        //填充excel的列头
        private void ExcelTitle(Dictionary<string, string> dic, ISheet sheet)
        {
            IRow row = sheet.CreateRow(0);
            int nIndex = 0;
            foreach (string key in dic.Keys)
            {
                ICell cell = row.CreateCell(nIndex);
                cell.SetCellValue(key);
                nIndex++;
            }
        }
        //填充对应的列数据
        private void ExcelData(Dictionary<string, string> dic, DataTable dt, ISheet sheet)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int nIndex = 0;
                IRow row = sheet.CreateRow(i + 1);
                string tel = string.Empty;

                foreach (string val in dic.Values)
                {
                    ICell cell = row.CreateCell(nIndex);
                    string colVal = dt.Rows[i][val] == DBNull.Value ? "" : dt.Rows[i][val].ToString();
                    cell.SetCellValue(colVal);
                    nIndex++;
                }
            }
        }

        private void ExportFromResponse(string fileName, byte[] file)
        {
            string UserAgent = HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.ToLower().IndexOf("ie") > -1)
                fileName = HttpContext.Current.Server.UrlEncode(fileName);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            HttpContext.Current.Response.BinaryWrite(file);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 根据索引和pagesize返回记录
        /// </summary>
        /// <param name="dt">记录集 DataTable</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="pagesize">一页的记录数</param>
        /// <returns></returns>
        private DataTable SplitDataTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Clone();
            //newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
    }
}
