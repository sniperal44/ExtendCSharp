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
        public event AppEvents_WorkbookOpenEventHandler WorkbookOpen;
        //TODO: implemento gli altri eventi

        protected String FileUri;
        protected Microsoft.Office.Interop.Excel.Application XApplication;
        protected Microsoft.Office.Interop.Excel._Workbook XWorkbook;
        //protected Microsoft.Office.Interop.Excel._Worksheet XSheet;
        //protected Microsoft.Office.Interop.Excel.Range XRng;

        public ExcelManager()
        {
        }

        public void OpenFile(string FileName)
        {
            XApplication = new Microsoft.Office.Interop.Excel.Application();
            XApplication.WorkbookOpen += (Workbook Wb) => { 
                WorkbookOpen?.Invoke(Wb); 
            };
            XApplication.Visible = true;
            XApplication.WorkbookBeforeClose += (Workbook Wb, ref bool Cancel) => { WorkbookBeforeClose?.Invoke(Wb, ref Cancel); };

            XWorkbook = XApplication.Workbooks.Open(FileName);

           
           
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

        public Worksheet GetWorksheet(String Name)
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

        public void Close()
        {
            XWorkbook.Close(0);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(XWorkbook);
            XApplication.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(XApplication);
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
