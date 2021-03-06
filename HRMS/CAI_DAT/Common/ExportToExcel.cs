using System;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Application = Microsoft.Office.Interop.Excel.Application;
using System.Reflection;
using Aspose.Cells;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace EVSoft.HRMS.Common
{
    internal class ExportToExcel
    {
        public const string PATH_TEMPLATES = "\\Templates\\";


        public static void XuatDuLieuRaExcel(int iRowPara,
                                           int iColumnPara,
                                           string strSubHeaderPara,
                                           System.Data.DataTable tblBangDuLieuPara,
                                           string strTemplateNamePara)
        {
            //Đường dẫn file template
            string strSourceFilePri = string.Format("{0}{1}{2}",
              System.Windows.Forms.Application.StartupPath,
              PATH_TEMPLATES, strTemplateNamePara);

            SaveFileDialog saveFileDialogPri = new SaveFileDialog();
            saveFileDialogPri.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialogPri.FilterIndex = 1;

            if (saveFileDialogPri.ShowDialog() == DialogResult.OK)
            {
                FileStream streamTemp = new FileStream(strSourceFilePri, FileMode.Open);

                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                workbook.Open(streamTemp);
                workbook.Worksheets.Add();
                Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];

                //Set cell store subHeader
                Aspose.Cells.Cells cellHeader = worksheet.Cells;
                //cellHeader.Merge(2, 0, 1, tblBangDuLieuPara.Columns.Count);
                worksheet.Cells["A3"].PutValue(strSubHeaderPara);
                //worksheet.IsGridlinesVisible = false;
                worksheet.Cells.ImportDataTable(tblBangDuLieuPara, false, iRowPara, iColumnPara, tblBangDuLieuPara.Rows.Count, tblBangDuLieuPara.Columns.Count);

                //Formatting for cells store database
                for (int i = 0; i < tblBangDuLieuPara.Rows.Count; i++)
                {
                    for (int j = 0; j < tblBangDuLieuPara.Columns.Count; j++)
                    {
                        Aspose.Cells.Cell cell = worksheet.Cells[iRowPara + i, j];
                        workbook.Styles.Add();
                        Aspose.Cells.Style style = cell.GetStyle();
                        style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.BottomBorder].Color = Color.Silver;
                        style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.TopBorder].Color = Color.Silver;
                        style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.LeftBorder].Color = Color.Silver;
                        style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                        style.Borders[BorderType.RightBorder].Color = Color.Silver;
                        cell.SetStyle(style);
                    }
                }                
                //worksheet.AutoFitColumns();
                //Save excel file
                workbook.Save(saveFileDialogPri.FileName, FileFormatType.Default);

                MessageBox.Show(WorkingContext.LangManager.GetString("frmRestSheet_ExportExcel_Messa"), 
                    WorkingContext.LangManager.GetString("Message"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                streamTemp.Close();

                if (File.Exists(saveFileDialogPri.FileName))
                    Process.Start(saveFileDialogPri.FileName);
            }
        }
    }
}
