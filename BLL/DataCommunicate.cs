using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    //This class as same as DTO
    public class DataCommunicate
    {
        private SqlDataAdapter da = new SqlDataAdapter();

        public DataCommunicate() {
            try
            {
                DataAccessLayer.Connect("ROCKYOPERATION", "QL_HoGiaDinh", "TN207User", "TN207User");
            }
            catch (Exception) {
            
            }
        }

        //Working with datatable
        public void GetDataSource(string table, DataTable dt) {
            string strSelect = "select * from " + table;
            this.OpenData(strSelect, dt);
        }

        public void InsertData(string[] data, string table) {
            string strInsert = "insert into " + table+" values(";
            for (int i = 0; i < data.Length; i++) {
                if (data[i] != "null")
                {
                    if (i > 0)
                    {
                        strInsert += ",";
                    }
                    strInsert += "@data" + i;
                }
                else {
                    if (i > 0)
                    {
                        strInsert += ",";
                    }
                    strInsert += data[i];
                }
            }
            strInsert += ")";
            //Console.WriteLine(strInsert);
            DataAccessLayer.CheckConnectionOpen();
            SqlCommand sqlCmd = new SqlCommand(strInsert, DataAccessLayer.myConn);
            for (int i = 0; i < data.Length; i++) {
                sqlCmd.Parameters.AddWithValue("@data"+i, data[i]);
            }
            sqlCmd.ExecuteNonQuery();
            this.TurnOff();
        }

        public void UpdateData(string[] fieldsData, string[] data, string[] fieldsPlace,
                string[] place,string table) {
            string strUpdate = "update " + table + " set ";
            for (int i = 0; i < fieldsData.Length; i++)
            {
                if (data[i] != "null")
                {
                    if (i > 0)
                    {
                        strUpdate += ",";
                    }
                    strUpdate += fieldsData[i] + "=" + "@data" + i;
                }
                else
                {
                    if (i > 0)
                    {
                        strUpdate += ",";
                    }
                    strUpdate += fieldsData[i] + "=" + data[i];
                }
            }
            strUpdate += " where ";
            for (int i = 0; i < fieldsPlace.Length; i++) {
                if (i > 0) {
                    strUpdate += " and ";
                }
                strUpdate += fieldsPlace[i] + "=" + "@place" + i;
            }
            DataAccessLayer.CheckConnectionOpen();
            SqlCommand sqlCmd = new SqlCommand(strUpdate, DataAccessLayer.myConn);
            for (int i = 0; i < fieldsData.Length; i++) {
                if (data[i] != "null")
                {
                    sqlCmd.Parameters.AddWithValue("@data" + i, data[i]);
                }
                else {
                    //Do nothing
                }
            }
            for (int i = 0; i < fieldsPlace.Length; i++)
            {
                sqlCmd.Parameters.AddWithValue("@place"+i, place[i]);
            }
            Console.WriteLine(strUpdate);
            sqlCmd.ExecuteNonQuery();
            this.TurnOff();
        }

        public void DeleteData(string[] fields, string[] data, string table) {
            string sqlDel = "delete "+table+" ";
            sqlDel += "where ";
            for (int i = 0; i < fields.Length; i++) {
                if (i > 0) {
                    sqlDel += " and ";
                }
                sqlDel += fields[i] + "=" + "@data" + i;
            }
            DataAccessLayer.CheckConnectionOpen();
            SqlCommand cmd = new SqlCommand(sqlDel, DataAccessLayer.myConn);
            for (int i = 0; i < fields.Length; i++) {
                cmd.Parameters.AddWithValue("@data" + i, data[i]);
            }
            //Console.WriteLine(sqlDel);
            cmd.ExecuteNonQuery();
            this.TurnOff();
        }

        //Open data
        public void OpenData(string strQuery, DataTable dtTable) {
            DataAccessLayer.OpenData(strQuery, dtTable);
        }

        //Special part for login
        //Login
        public string[] Login(string id, string index, string pass){
            string[] res = new string[4];
            SqlCommand cmdCommand;
            SqlDataReader drReader;
            string sqlTimKiem, strMK;
            try
            {
                DataAccessLayer.CheckConnectionOpen();
            }
            catch (Exception)
            {
                res[0] = "Bug";
                res[1] = "Lỗi khi thực hiện kết nối";
                return res;
            }
            if (DataAccessLayer.myConn.State == ConnectionState.Open)
            {
                strMK = ObjectLoginValue.EncodePass(pass);
                sqlTimKiem = "select MaHo, SttThanhVien, QuyenSD, HoTenThanhVien from ThanhVien where MaHo = @MaHo and " +
                "SttThanhVien = @STT and MatKhau = @MatKhau";
                cmdCommand = new SqlCommand(sqlTimKiem, DataAccessLayer.myConn);
                cmdCommand.Parameters.AddWithValue("@MaHo", id);
                cmdCommand.Parameters.AddWithValue("@STT", index);
                cmdCommand.Parameters.AddWithValue("@MatKhau", strMK);
                drReader = cmdCommand.ExecuteReader();
                if (drReader.HasRows)
                {
                    drReader.Read();
                    res[0] = drReader.GetInt32(0).ToString();
                    res[1] = drReader.GetInt16(1).ToString();
                    res[2] = drReader.GetString(2);
                    res[3] = drReader.GetString(3);
                }
                else
                {
                    res[0] = "Bug";
                    res[1] = "Mã hộ, số thứ tự hoặc mật khẩu sai!";
                }
                this.TurnOff();
            }
            else
            {
                res[0] = "Bug";
                res[1] = "Kết nối không thành công!";
            }
            return res;
        }

        //Change password
        public void ChangePass(string newPass) {
            string sql = "update ThanhVien set MatKhau=@Pass where MaHo=@Id and SttThanhVien=@Index";
            DataAccessLayer.CheckConnectionOpen();
            if (this.TestConnect()) {
                SqlCommand cmd = new SqlCommand(sql, DataAccessLayer.myConn);
                cmd.Parameters.AddWithValue("@Id", ObjectLoginValue.strId);
                cmd.Parameters.AddWithValue("@Index", ObjectLoginValue.strIndex);
                cmd.Parameters.AddWithValue("@Pass", newPass);
                cmd.ExecuteNonQuery();
                this.TurnOff();
            }
        }



        //This part for debugging

        //Return true when connected to database
        public bool TestConnect() {
            if (DataAccessLayer.myConn.State == ConnectionState.Open) {
                return true;
            }
            return false;
        }

        //Test select data
        public bool IsExisted(string[] fields, string[] values, string table) {
            if (DataAccessLayer.IsExisted(values, fields, table))
            {
                return true;
            }
            return false;

        }

        

        //Close the connection
        public void TurnOff() {
            DataAccessLayer.Disconnect();
        }
    }
}
