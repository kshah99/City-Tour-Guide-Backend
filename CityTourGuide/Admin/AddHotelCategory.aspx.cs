using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddHotelCategory : System.Web.UI.Page
{
    clsHotel _hotel = new clsHotel();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAddCategory_Click(object sender, EventArgs e)
    {
        try
        {
            _hotel.Category = txtCategoryName.Text.Trim();
            _hotel.UserId = Convert.ToInt32(clsSecurity.DecryptText(Session["UserID"].ToString()));
            _hotel.AddHotelCategory();

            txtCategoryName.Text = "";

            lblNote.Visible = true;
            lblNote.Text = "You have successfully added Hotel Category.";
            lblNote.CssClass = "SuccessMsg";
        }
        catch
        {
            lblNote.Visible = true;
            lblNote.Text = "Something is Wrong.";
            lblNote.CssClass = "ErrorMsg";
        }
    }
}