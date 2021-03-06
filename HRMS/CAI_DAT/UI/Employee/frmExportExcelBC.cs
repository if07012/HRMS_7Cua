using System;
using System.Data;
using System.Windows.Forms;
using EVSoft.HRMS.Common;
using XPTable.Models;

namespace EVSoft.HRMS.UI.Employee
{
    public partial class frmExportExcelBC : Form
    {
        public DataSet dsEmployee = null;
        private const string COMPANY_NM = "CÔNG TY TNHH ESTELLE VIỆT NAM";
        private const string CREDIT_CARD_NM = "THẺ RA VÀO";

        public frmExportExcelBC()
        {
            InitializeComponent();

            cboBarCodeType.SelectedIndex = 1;
        }

        private void btnExcelBarCode_Click(object sender, EventArgs e)
        {
            frmStatusMessage message = new frmStatusMessage();
            //message.Show("Xu?t danh sách ra file excel...");
            string str_xuat = WorkingContext.LangManager.GetString("frmListEmployee_Excel_frmMessa");
            message.Show(str_xuat);
            this.Cursor = Cursors.WaitCursor;

            PopulateEmployeeListView();

            if (!Utils.ExportExcelBC(lvwEmployeeBarcode, this.Text.ToUpper()))
            {
                string str = WorkingContext.LangManager.GetString("frmListEm_XuatExcel_Error");
                string str1 = WorkingContext.LangManager.GetString("frmListEm_XuatExcel_Error_Title");
                //MessageBox.Show("Có l?i x?y ra khi xu?t d? li?u ra file excel", "Xu?t excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(str, str1, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            message.Close();
            this.Cursor = Cursors.Default;
        }

        private void PopulateEmployeeListView()
        {

            ConvertFont convertFont = new ConvertFont();
            lvwEmployeeBarcode.BeginUpdate();
            lvwEmployeeBarcode.TableModel.Rows.Clear();

            string departmentNM, employeeNM, companyNM = string.Empty, creditCardNM = string.Empty;

            int STT = 0;
            string strBarCode;
            foreach (DataRow dr in dsEmployee.Tables[0].Rows)
            {
                departmentNM = dr["DepartmentName"].ToString();
                employeeNM = dr["EmployeeName"].ToString();
                companyNM = COMPANY_NM;
                creditCardNM = CREDIT_CARD_NM;
                
                //Tieng viet ko dau
                if(rdbTiengVietKD.Checked)
                {
                    departmentNM = Utils.RemoveSign4VietnameseString(departmentNM);
                    employeeNM = Utils.RemoveSign4VietnameseString(employeeNM);
                    companyNM = Utils.RemoveSign4VietnameseString(COMPANY_NM);
                    creditCardNM = Utils.RemoveSign4VietnameseString(CREDIT_CARD_NM);
                }
                //font: TCVN3(ABC)
                else if(rdbTCVN3.Checked)
                {
                    convertFont.Convert(ref departmentNM, FontIndex.iUNI, FontIndex.iTCV);
                    convertFont.Convert(ref employeeNM, FontIndex.iUNI, FontIndex.iTCV);
                    convertFont.Convert(ref companyNM, FontIndex.iUNI, FontIndex.iTCV);
                    convertFont.Convert(ref creditCardNM, FontIndex.iUNI, FontIndex.iTCV);
                }

                Cell DepartmentName = new Cell(departmentNM);
                Cell EmployeeName = new Cell(employeeNM);
                Cell CompanyName = new Cell(companyNM);
                Cell CreditCardName = new Cell(creditCardNM);

                Cell BarCodeNew = new Cell("");
                if (dr["BarCode"] != DBNull.Value)
                {
                    strBarCode = dr["BarCode"].ToString();
                    
                    if (strBarCode.Length > 0)
                    {
                        //Barcode 8 so
                        if (cboBarCodeType.SelectedIndex == 0)
                            strBarCode = strBarCode.Substring(5, 8);

                        //Ma BarCode bo so check code
                        if (!chkCheckCode.Checked)
                            strBarCode = strBarCode.Substring(0, strBarCode.Length - 1);

                        BarCodeNew = new Cell(strBarCode);
                    }
                }

                Cell CardID = new Cell(dr["CardID"].ToString());
                Cell StartDate = new Cell("");

                if (dr["StartDate"] != DBNull.Value)
                {
                    StartDate = new Cell(DateTime.Parse(dr["StartDate"].ToString()).ToString("dd/MM/yyyy"));
                }

                Cell ImageFilePath = new Cell("");
                if (dr["Picture"] != DBNull.Value)
                {
                    string PictureFileName = dr["Picture"].ToString();
                    if (PictureFileName.Equals(""))
                    {
                        ImageFilePath = new Cell(Application.StartupPath + @"\IMAGES\noimage3.jpg");
                    }
                    else
                    {
                        string PictureFilePath = WorkingContext.Setting.PicturePath + '\\' + dr["Picture"].ToString();
                        try
                        {
                            ImageFilePath = new Cell(PictureFilePath);
                        }
                        catch
                        {
                            ImageFilePath = new Cell(Application.StartupPath + @"\IMAGES\noimage3.jpg");
                        }
                    }
                }
                else
                {
                    ImageFilePath = new Cell(Application.StartupPath + "/IMAGES/noimage3.jpg");
                }

                Row rowBarcode = new Row(new Cell[] { new Cell(STT + 1), CompanyName, CreditCardName, CardID, BarCodeNew, EmployeeName, DepartmentName, StartDate, ImageFilePath });
                rowBarcode.Tag = STT;
                lvwEmployeeBarcode.TableModel.Rows.Add(rowBarcode);

                STT++;
            }
            lvwEmployeeBarcode.EndUpdate();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}