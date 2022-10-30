using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class DataAccessLayer
    {
        public static SqlConnection myConn;
        public static void Connect(string serverName, string databaseName, string UID, string PWD) {
            string connStr = "Server = " + serverName + "; Database = " + databaseName +
                "; Integrated Security = false; UID = " + UID + "; PWD = " + PWD;
            myConn = new SqlConnection();
            myConn.ConnectionString = connStr;

            try
            {
                myConn.Open();
            }
            catch (Exception) { 
            
            }

        }

        public static void OpenData(string strQuery, DataTable dtTable)
        {
            try
            {
                CheckConnectionOpen();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(strQuery, myConn);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Fill(dtTable);
                myConn.Close();
            }
            catch (Exception)
            {

            }
        }

        //When use this if you search for a name in vietnamese it will has error convert string
        public static bool IsExisted(string[] value, string[] field, string table) {
            bool res = false;
            try
            {
                string selectQuery = "select 1 from " + table + " where ";
                for (int i = 0; i < value.Length; i++) {
                    if (i > 0)
                    {
                        selectQuery += " and ";
                    }
                    selectQuery += field[i] + " = '" + value[i] + "'";
                }
                //Console.WriteLine(selectQuery);
                CheckConnectionOpen();
                SqlCommand cmd = new SqlCommand(selectQuery, myConn);
                SqlDataReader daReder = cmd.ExecuteReader();
                if (daReder.HasRows)
                {
                    res = true;
                }
                myConn.Close();
                daReder.Close();
            }
            catch (Exception) {
                res = false;
            }
            return res;
        }

        public static void CheckConnectionOpen() {
            if (myConn.State == ConnectionState.Closed)
            {
                myConn.Open();
            }
        }




        //This part for debugging

        public static void Disconnect() {
            if (myConn.State == ConnectionState.Open) {
                myConn.Close();
            }
        }
    }
}
