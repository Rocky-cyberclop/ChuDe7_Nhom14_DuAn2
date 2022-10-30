using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    //This class will store all of the information relate to the User has login likes cookie and session in web 
    public static class ObjectLoginValue
    {
        public static string strId, strIndex, strName, strPrivilege;
        public static string Login(string id, string index, string pass) {
            DataCommunicate dt = new DataCommunicate();
            string[] res = new string[4];
            res = dt.Login(id, index, pass);
            if (res[0] != "Bug") {
                strId = res[0];
                strIndex = res[1];
                strName = res[3];
                strPrivilege = res[2];
                return "Logged";
            }
            return res[1];
        }

        public static void Logout() {
            strId = "";
            strIndex = "";
            strName = "";
            strPrivilege = "";
        }

        public static string[] ChangePassword(string oldPass, string newPass) {
            string[] res = new string[1];
            string[] fields = new string[3];
            fields[0] = "MaHo";
            fields[1] = "SttThanhVien";
            fields[2] = "MatKhau";
            string[] values = new string[3];
            values[0] = strId;
            values[1] = strIndex;
            values[2] = EncodePass(oldPass);
            DataCommunicate dc = new DataCommunicate();
            if (dc.IsExisted(fields, values, "ThanhVien"))
            {
                dc.ChangePass(EncodePass(newPass));
                res[0] = "Yes";
            }
            else {
                res[0] = "No";
            }
            return res;
        }

        public static string EncodePass(string purePass)
        {
            string tmp1 = "", tmp2 = "";
            int i, n;
            n = purePass.Length;
            for (i = 0; i < n; i += 2)
            {
                tmp1 += purePass[i];
                if (n > i + 1)
                {
                    tmp2 += purePass[i + 1];
                }
            }

            return tmp1 + tmp2;

        }
    }
}
