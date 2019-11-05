using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_AdminMaster : System.Web.UI.MasterPage
{
    clsUser _user = new clsUser();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _user.UserId = Convert.ToInt64(clsSecurity.DecryptText(Session["UserID"].ToString()));
            DataTable dt = _user.Profile();

            if (dt.Rows.Count > 0)
            {
                lblName.Text = dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString();
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!clsSecurity.DecryptText(Session["UserID"].ToString()).All(char.IsDigit))
                {
                    Response.Redirect("~/Login.aspx");
                }
            }
        }
    }

    protected void lnkbtnLogout_Click(object sender, EventArgs e)
    {
        Session["UserID"] = null;

        Response.Redirect("~/Login.aspx");
    }
}