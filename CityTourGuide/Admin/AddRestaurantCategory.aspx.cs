using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddRestaurantCategory : System.Web.UI.Page
{
    clsRestaurant _restaurant = new clsRestaurant();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAddCategory_Click(object sender, EventArgs e)
    {
        try
        {
            _restaurant.Category = txtCategoryName.Text.Trim();
            _restaurant.UserId = Convert.ToInt32(clsSecurity.DecryptText(Session["UserID"].ToString()));
            _restaurant.AddRestaurantCategory();

            txtCategoryName.Text = "";

            lblNote.Visible = true;
            lblNote.Text = "You have successfully added Restaurant Category.";
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