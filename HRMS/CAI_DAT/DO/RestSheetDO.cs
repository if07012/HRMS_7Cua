using System;
using System.Data;
using System.Data.SqlClient;
using EVSoft.HRMS.Common;
using System.Windows.Forms;

namespace EVSoft.HRMS.DO
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class RestSheetDO
	{
		public RestSheetDO()
		{
			//
			// TODO: Add constructor logic here
			//
        }
        #region Các hàm lấy dữ liệu
        /// <summary>
		/// Lấy thông tin thanh toán tiền phép trong năm theo bộ phận làm việc
		/// </summary>
		/// <param name="DepartmentID">Mã phòng ban</param>
		/// <param name="Year">Năm tính phép</param>
		/// <returns></returns>
		public DataSet GetDepartmentRestSheet( int Year, int DepartmentID)
		{
			SqlConnection conn = WorkingContext.GetConnection();
			// Build the command
			SqlCommand sqlCommand = new SqlCommand("GetDepartmentRestSheet", conn);
			sqlCommand.CommandType = CommandType.StoredProcedure;
			
			sqlCommand.Parameters.Add(new SqlParameter("@Year", Year));
			sqlCommand.Parameters.Add(new SqlParameter("@DepartmentID", DepartmentID));

			SqlDataAdapter dataAdapter = new SqlDataAdapter();
			dataAdapter.SelectCommand = sqlCommand;

			DataSet dataSet = new DataSet();
			try
			{
				conn.Open();
				dataAdapter.Fill(dataSet, "GetDepartmentRestSheet");
				return dataSet;
			}
			catch(Exception oException)
			{
				MessageBox.Show(oException.ToString());
				return null;
			}
			finally
			{
				conn.Dispose();
				conn.Close();
			}
		}

        /// <summary>
        /// Lấy tổng số ngày nghỉ phép của 1 nhân viên trong 1 tháng
        /// </summary>
        /// <param name="iEmlpoyeeID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <returns></returns>
        public float GetTotalRestByMonth(int iEmlpoyeeID, int iYear, int iMonth)
        {
            SqlConnection conn = WorkingContext.GetConnection();
            SqlCommand cmd = new SqlCommand("GetTotalRestByMonth", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Clear();
            cmd.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.Int)).Value = iEmlpoyeeID;
            cmd.Parameters.Add(new SqlParameter("@Year",  SqlDbType.Int)).Value = iYear;
            cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int)).Value = iMonth;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet dsRestMonth = new DataSet();
            try
            {
                conn.Open();
                adapter.Fill(dsRestMonth, "GetTotalRestByMonth");
                if (dsRestMonth.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToSingle(dsRestMonth.Tables[0].Rows[0]["MonthRest"]);
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public DataSet GetFromRestDay()
        {
            SqlConnection conn = WorkingContext.GetConnection();
            // Build the command
            SqlCommand sqlCommand = new SqlCommand("GetFromRestDay", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = sqlCommand;

            DataSet dataSet = new DataSet();
            try
            {
                conn.Open();
                dataAdapter.Fill(dataSet, "GetFromRestDay");
                return dataSet;
            }
            catch (Exception oException)
            {
                MessageBox.Show(oException.ToString());
                return null;
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        /// <summary>
        /// Lấy về số ngày phép được hưởng của nhân viên trong 1 năm
        /// </summary>
        /// <param name="iEmployeeIDPara">Mã nhân viên</param>
        /// <param name="iYearPara">Năm tính phép</param>
        /// <param name="iMonthPara">Tháng hiện tại</param>
        /// <returns></returns>
        public DataSet GetRestAllowByEmployee(int iEmployeeIDPara, int iYearPara, int iMonthPara)
        {
            SqlConnection con = WorkingContext.GetConnection();
            
            SqlCommand com = new SqlCommand("GetRestAllowByEmployee", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.Clear();
            com.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.Int)).Value = iEmployeeIDPara;
            com.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int)).Value = iYearPara;
            com.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int)).Value = iMonthPara;

            SqlDataAdapter adapter = new SqlDataAdapter(com);
            DataSet dataSet = new DataSet();

            try
            {
                con.Open();
                adapter.Fill(dataSet, "GetRestAllowByEmployee");

                return dataSet;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Lấy thời gian đã nghỉ phép trong 1 tháng
        /// </summary>
        /// <param name="iEmployeeIDPara">Mã nhân viên</param>
        /// <param name="iYearPara">Năm tính phép</param>
        /// <param name="iMonthPara">Tháng tính phép</param>
        /// <returns></returns>
        public float GetRestInMonth(int iEmployeeIDPara, int iYearPara, int iMonthPara)
        {
            float fRestInMonthPri = 0;

            SqlConnection con = WorkingContext.GetConnection();

            SqlCommand com = new SqlCommand("GetRestInMonth", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.Clear();
            com.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.Int)).Value = iEmployeeIDPara;
            com.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int)).Value = iYearPara;
            com.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int)).Value = iMonthPara;            

            try
            {
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    fRestInMonthPri = Convert.ToSingle(reader["RestInMonth"]);
                }
                reader.Close();

                return fRestInMonthPri;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return -1;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }        
        #endregion

        #region Các hàm insert/update/delete dữ liệu
        /// <summary>
		/// Sinh b?ng Thanh toán ti?n phép
		/// </summary>
		///
		/// <param name="Year">Nam</param>
		/// <param name="EmployeeID">Mã nhân viên</param>
		/// <returns></returns>
		public int GenerateRestSheet( int Year, int EmployeeID)
		{
			SqlConnection conn = WorkingContext.GetConnection();
			// Build the command
			SqlCommand sqlCommand = new SqlCommand("GenerateRestSheet", conn);
			sqlCommand.CommandType = CommandType.StoredProcedure;
 
			sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", EmployeeID));
			sqlCommand.Parameters.Add(new SqlParameter("@Year", Year));
 
			try
			{
				conn.Open();
				return sqlCommand.ExecuteNonQuery();
			}
			catch(Exception oException)
			{
				MessageBox.Show(oException.ToString());
				return 0;
			}
			finally
			{
				conn.Dispose();
				conn.Close();
			}
		}
	
		/// <summary>
		/// Cập nhật thanh toán tiền phép
		/// </summary>
		/// <param name="dataSet"></param>
		/// <returns></returns>
		public int UpdateRestSheet(DataSet dataSet)
		{
			SqlConnection conn = WorkingContext.GetConnection();
			SqlCommand sqlCommand = new SqlCommand("UpdateRestSheet", conn);
			sqlCommand.CommandType = CommandType.StoredProcedure;
 
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Year", SqlDbType.Int, "Year"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@EmployeeID", SqlDbType.Int, "EmployeeID"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month1", SqlDbType.VarChar, "Month1"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month2", SqlDbType.VarChar, "Month2"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month3", SqlDbType.VarChar, "Month3"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month4", SqlDbType.VarChar, "Month4"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month5", SqlDbType.VarChar, "Month5"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month6", SqlDbType.VarChar, "Month6"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month7", SqlDbType.VarChar, "Month7"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month8", SqlDbType.VarChar, "Month8"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month9", SqlDbType.VarChar, "Month9"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month10", SqlDbType.VarChar, "Month10"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month11", SqlDbType.VarChar, "Month11"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month12", SqlDbType.VarChar, "Month12"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@TotalRest",SqlDbType.VarChar,"TotalRest"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@TotalRestAllow",SqlDbType.VarChar,"TotalRestAllow"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@TotalRestRemain",SqlDbType.VarChar,"TotalRestRemain"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@ToMoney",SqlDbType.Money,"ToMoney"));

			SqlDataAdapter dataAdapter = new SqlDataAdapter();
			dataAdapter.UpdateCommand = sqlCommand;
 
			try
			{
				conn.Open();
				return dataAdapter.Update(dataSet.Tables[0]);
			}
			catch(Exception oException)
			{
				MessageBox.Show(oException.ToString());
				return 0;
			}
			finally
			{
				conn.Dispose();
				conn.Close();
			}
		}

		public int DeleteRestSheet(DataSet dataSet)
		{
			SqlConnection conn = WorkingContext.GetConnection();
			// Build the command
			SqlCommand sqlCommand = new SqlCommand("DeleteRestSheet", conn);
			sqlCommand.CommandType = CommandType.StoredProcedure;
 
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@EmployeeID", SqlDbType.Int, "EmployeeID"));
			//sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Month", SqlDbType.Int, "Month"));
			sqlCommand.Parameters.Add(WorkingContext.CreateParam("@Year", SqlDbType.Int, "Year"));
 
			SqlDataAdapter dataAdapter = new SqlDataAdapter();
			dataAdapter.DeleteCommand = sqlCommand;
 
			try
			{
				conn.Open();
				return dataAdapter.Update(dataSet.Tables[0]);
			}
			catch(Exception oException)
			{
				MessageBox.Show(oException.ToString());
				return 0;
			}
			finally
			{
				conn.Dispose();
				conn.Close();
			}
		}

        public void DeleteFromRestDay(int Year)
        {
            SqlConnection conn = WorkingContext.GetConnection();
            // Build the command
            SqlCommand sqlCommand = new SqlCommand("DeleteFromRestDay", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.Add(new SqlParameter("@Year", Year));

            try
            {
                conn.Open();
                 sqlCommand.ExecuteNonQuery();
            }
            catch (Exception oException)
            {
                MessageBox.Show(oException.ToString());
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        public int UpdateRestDay(DateTime DayRest,decimal DayIndex,string Description)
        {
            SqlConnection conn = WorkingContext.GetConnection();
            // Build the command
            SqlCommand sqlCommand = new SqlCommand("UpdateRestDay", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.Add(new SqlParameter("@DayRest",DayRest));
            sqlCommand.Parameters.Add(new SqlParameter("@DayIndex",DayIndex));
            sqlCommand.Parameters.Add(new SqlParameter("@Description",Description));

            try
            {
                conn.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception oException)
            {
                MessageBox.Show(oException.ToString());
                return 0;
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        /// <summary>
        /// Xoa danh sach thanh toan tien phep trong nam
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public void DeleteRegRestInYear(int year)
        {
            SqlConnection conn = WorkingContext.GetConnection();
            // Build the command
            SqlCommand sqlCommand = new SqlCommand("DeleteRestSheetInYear", conn);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.Add(new SqlParameter("@Year", year));

            try
            {
                conn.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception oException)
            {
                MessageBox.Show(oException.ToString());
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }
        #endregion
    }
}
