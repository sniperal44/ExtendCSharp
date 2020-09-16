using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendCSharp.Excel
{
    public class ExcelManager
    {
        public event AppEvents_WorkbookBeforeCloseEventHandler WorkbookBeforeClose;
        //TODO: implemento gli altri eventi

        protected String FileUri;
        protected Microsoft.Office.Interop.Excel.Application XApplication;
        protected Microsoft.Office.Interop.Excel._Workbook XWorkbook;
        //protected Microsoft.Office.Interop.Excel._Worksheet XSheet;
        //protected Microsoft.Office.Interop.Excel.Range XRng;

        public ExcelManager(String uri)
        {
            this.FileUri = uri;
            OpenFile(uri);
        }

        private void OpenFile(string FileName)
        {
            XApplication = new Microsoft.Office.Interop.Excel.Application();
            XWorkbook = XApplication.Workbooks.Open(FileName);

            XApplication.Visible = true;
            XApplication.WorkbookBeforeClose += (Workbook Wb, ref bool Cancel)=> { WorkbookBeforeClose?.Invoke(Wb,ref Cancel); };
        }



        public List<String> GetSheetsName()
        {
            List<String> sheetNames = new List<string>();
            foreach (Worksheet worksheet in XWorkbook.Worksheets)
            {
                sheetNames.Add(worksheet.Name);
            }


            return sheetNames;

        }

        public Worksheet GetWorksheets(String Name)
        {
            return (Worksheet)XWorkbook.Worksheets[Name];
        }

        public Worksheet CreateWorksheet(String Name)
        {
            if (!WorksheetExist(Name))
            {
                Worksheet ws = (Worksheet)XWorkbook.Worksheets.Add(Type.Missing, XWorkbook.Worksheets.GetEnumerable().Last(), Type.Missing, Type.Missing);
                ws.Name = Name;
                return ws;
            }
            else
                return (Worksheet)XWorkbook.Worksheets[Name];
        }
        public void DeleteWorksheet(String Name)
        {
            if (WorksheetExist(Name))
            {
                XApplication.DisplayAlerts = false;
                XWorkbook.Worksheets[Name].Delete();
                XApplication.DisplayAlerts = true;
            }
        }
        public bool WorksheetExist(String Name)
        {
            return XWorkbook.Worksheets.Keys().Contains(Name);
        }
    }

    public static class ExcelExtension
    {
        public static IEnumerable<String> Keys(this Sheets sheets)
        {
            return sheets.Cast<Worksheet>().Select(x => x.Name);
        }

        public static IEnumerable<dynamic> GetEnumerable(this Sheets sheets)
        {
            return sheets.Cast<dynamic>();
        }

        public static Range GetRange(this Worksheet sheet, int Row, int Column)
        {
            return (Range)sheet.Cells[Row, Column];
        }
    }
}
