using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AMS.TextBox;
using EVSoft.HRMS.Common;
using EVSoft.HRMS.DO;

namespace EVSoft.HRMS.UI.Employee
{
    public partial class FrmUpdateSalaryHistory : Form
    {
        #region Danh sách các biến
        private EmployeeDO employeeDO = null;
        private DataSet dsSalaryHistory = null;
        private DataRow rowUpdate = null;

        private int SalaryHistoryID;
        private int EmployeeID;
        private decimal Salary;
        private string DecNumber;
        private string Note;
        private DateTime ModifiedDate;

        //Các biến trước khi cập nhật
        private decimal SalaryBeforeUpdate;
        private DateTime ModifiedDateBeforeUpdate;
        private string DecNumberBeforeUpdate;
        private string NoteBeforeUpdate;
        #endregion

        public FrmUpdateSalaryHistory()
        {
            InitializeComponent();
        }

        public FrmUpdateSalaryHistory(int iEmployeeID, decimal dbSalary, string strDecNumber, DateTime dtModefiedDate, string strNote, int iSelectedRow)
        {
            InitializeComponent();

            employeeDO = new EmployeeDO();
            dsSalaryHistory = employeeDO.GetSalaryHistory(iEmployeeID);
            rowUpdate = dsSalaryHistory.Tables[0].Rows[iSelectedRow];

            txtSalary.Text = dbSalary.ToString();
            txtDecNumber.Text = strDecNumber;
            dtpDate.Value = dtModefiedDate;
            txtNote.Text = strNote;
            //Lưu lại các thông tin trước khi update           
            SalaryBeforeUpdate = dbSalary;
            DecNumberBeforeUpdate = strDecNumber;
            ModifiedDateBeforeUpdate = dtModefiedDate.Date;
            NoteBeforeUpdate = strNote;
        }

        private void SetDataRowUpdate(ref DataRow dtRow)
        {
            dtRow.BeginEdit();
            dtRow["BasicSalary"] = txtSalary.Double;
            dtRow["DecisionNumber"] = txtDecNumber.Text;
            dtRow["Note"] = txtNote.Text;
            dtRow["ModifiedDate"] = dtpDate.Value.Date;
            dtRow.EndEdit();
        }

        private void FrmUpdateSalaryHistory_Load(object sender, EventArgs e)
        {
            txtSalary.Prefix = "";
            txtSalary.MaxDecimalPlaces = 0;
        }

        private void FrmUpdateSalaryHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOK_Click(null, null);
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void dtpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Kiểm tra nhập thông tin lương
            if (txtSalary.Text.Trim() == string.Empty || Convert.ToDouble(txtSalary.Text.Trim()) == 0)
            {
                MessageBox.Show(this, WorkingContext.LangManager.GetString("FrmUpdateSalaryHistory_Error_Messa"),
                    WorkingContext.LangManager.GetString("Loi"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.ActiveControl = txtSalary;
                return;
            }
            decimal dbSalaryPri = Math.Round(Convert.ToDecimal(txtSalary.Text.Trim()), 2);
            string strDecNumberPri = txtDecNumber.Text.Trim();
            string strNotePri = txtNote.Text.Trim();
            DateTime dtModifiedDatePri = dtpDate.Value.Date;
            if (dbSalaryPri == SalaryBeforeUpdate
                && strDecNumberPri == DecNumberBeforeUpdate
                && dtModifiedDatePri == ModifiedDateBeforeUpdate
                && strNotePri == NoteBeforeUpdate)
            {                
                this.Close();
                return;
            }
            DialogResult rs = MessageBox.Show(this, WorkingContext.LangManager.GetString("FrmUpdateSalaryHistory_Confirm_Messa"),
                WorkingContext.LangManager.GetString("Confirm"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.No)
            {
                return;
            }
            int ret = 0;
            SetDataRowUpdate(ref rowUpdate);
            ret = employeeDO.UpdateSalaryHistory(dsSalaryHistory);
            if (ret != 0)
            {
                MessageBox.Show(this, WorkingContext.LangManager.GetString("FrmUpdateSalaryHistory_OK_Messa"),
                    WorkingContext.LangManager.GetString("Message"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSalary_Leave(object sender, EventArgs e)
        {

        }
    }
}