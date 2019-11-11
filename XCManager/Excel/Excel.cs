using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace XCManager.Excel
{
    public class ExcelDoc
    {
        string path = "";
        _Application excel = new _Excel.Application();
        Workbook workbook;
        Worksheet workSheet;

        public ExcelDoc(string path, int sheet)
        {
            this.path = path;
            workbook = excel.Workbooks.Open(path);
            workSheet = workbook.Worksheets[sheet];
        }

        public ExcelDoc()
        {

        }

        public void CreateNewFile()
        {
            workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            workSheet = workbook.Worksheets[1];
        }

        public void SaveAs(string path)
        {
            workbook.SaveAs(path);
        }

        public void Close()
        {
            workbook.Close();
        }

        public void Save()
        {
            workbook.Save();
        }

        public void WriteToCell(int i, int j, string s)
        {
            i++;
            j++;
            workSheet.Cells[i, j].Value = s;
        }
    }
}