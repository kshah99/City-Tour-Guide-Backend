using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Text;
using System.Text.RegularExpressions;

public partial class Login : System.Web.UI.Page
{
    static clsUser _user = new clsUser();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    public static string AdminLogin(string Email, string Password)
    {
        try
        {
            string response = "";

            // Check Login...
            _user.Email = Email;
            _user.Password = Password;

            DataTable dt = _user.Login();

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserType"].ToString() == "Admin")
                {
                    HttpContext.Current.Session["UserId"] = clsSecurity.EncryptText(dt.Rows[0]["UserId"].ToString());

                    _user.UserId = Convert.ToInt64(dt.Rows[0]["UserID"].ToString());
                    _user.AddLoginData();

                    response = "success";
                }
                else
                {
                    response = "unauthorize";
                }
            }
            else
            {
                DataTable dtCheckEmail = _user.CheckEmail();

                if (dtCheckEmail.Rows.Count > 0)
                {
                    response = "invalid";
                }
                else
                {
                    response = "not_exist";
                }
            }

            return response;
        }
        catch (Exception e)
        {
            return "error";
        }
    }
}