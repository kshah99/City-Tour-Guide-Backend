using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_AddRestaurant : System.Web.UI.Page
{
    clsRestaurant _restaurant = new clsRestaurant();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCategory();
        }
    }

    protected void btnAddRestaurant_Click(object sender, EventArgs e)
    {
        try
        {
            string MainPhoto = "";
            string CoverPhoto = "";

            HttpFileCollection hfc = Request.Files;
            if (hfc.Count > 0)
            {
                HttpPostedFile hpf1 = hfc[0];
                HttpPostedFile hpf2 = hfc[0];

                if (hpf1.ContentLength > 0)
                {
                    MainPhoto = txtRestaurantName.Text.Trim().Replace(" ", "-") + "__" + clsConfiguration.PhotoDateTime() + ".jpg";

                    clsGraphics.GenerateThumbnails(150, 150, hpf1.InputStream, Server.MapPath("~\\images\\Restaurant\\main_photo\\thumb\\") + MainPhoto);
                    clsGraphics.GenerateThumbnails(300, 300, hpf1.InputStream, Server.MapPath("~\\images\\Restaurant\\main_photo\\small\\") + MainPhoto);
                    clsGraphics.ResizeImage(500, 500, hpf1.InputStream, Server.MapPath("~\\images\\Restaurant\\main_photo\\big\\") + MainPhoto);
                }

                if (hpf2.ContentLength > 0)
                {
                    CoverPhoto = txtRestaurantName.Text.Trim().Replace(" ", "-") + "__" + clsConfiguration.PhotoDateTime() + ".jpg";

                    clsGraphics.ResizeImage(100, 50, hpf2.InputStream, Server.MapPath("~\\images\\Restaurant\\cover_photo\\thumb\\") + CoverPhoto);
                    clsGraphics.ResizeImage(500, 300, hpf2.InputStream, Server.MapPath("~\\images\\Restaurant\\cover_photo\\small\\") + CoverPhoto);
                    clsGraphics.ResizeImage(1000, 500, hpf2.InputStream, Server.MapPath("~\\images\\Restaurant\\cover_photo\\big\\") + CoverPhoto);
                }
            }

            _restaurant.Name = txtRestaurantName.Text.Trim();
            _restaurant.MainPhoto = MainPhoto;
            _restaurant.CoverPhoto = CoverPhoto;
            _restaurant.Description = txtRestaurantDescription.Text.Trim();
            _restaurant.Address = txtRestaurantAddress.Text.Trim();
            _restaurant.Pincode = Convert.ToInt32(txtPincode.Text.Trim());
            _restaurant.ContactNumber = txtContactNumber.Text.Trim();
            _restaurant.AlternateNumber = txtAlternateNumber.Text.Trim();
            _restaurant.EmailAddress = txtEmailAddress.Text.Trim();
            _restaurant.Website = txtWebsite.Text.Trim();
            _restaurant.Publish = chkPublish.Checked;
            _restaurant.Latitude = hdnLatitude.Value;
            _restaurant.Longitutde = hdnLongitude.Value;
            _restaurant.UserId = Convert.ToInt32(clsSecurity.DecryptText(Session["UserID"].ToString()));

            Int64 RestaurantId = _restaurant.AddRestaurant();

            for (int i = 0; i < chkCategory.Items.Count; i++)
            {
                if (chkCategory.Items[i].Selected)
                {
                    _restaurant.RestaurantId = RestaurantId;
                    _restaurant.RestaurantCategoryId = Convert.ToInt64(chkCategory.Items[i].Value.ToString());
                    _restaurant.AddRestaurantDish();
                }
            }

            lblNote.Visible = true;
            lblNote.Text = "You have successfully added Restaurant.";
            lblNote.CssClass = "SuccessMsg";

            txtRestaurantName.Text = "";
            txtRestaurantDescription.Text = "";
            txtRestaurantAddress.Text = "";
            txtPincode.Text = "";
            txtContactNumber.Text = "";
            txtAlternateNumber.Text = "";
            txtEmailAddress.Text = "";
            txtWebsite.Text = "";
            for (int i = 0; i < chkCategory.Items.Count; i++)
            {
                chkCategory.Items[i].Selected = false;
            }
        }
        catch (Exception ex)
        {
            lblNote.Visible = true;
            lblNote.Text = ex.Message.ToString();
            //lblNote.Text = "Something is Wrong.";
            lblNote.CssClass = "ErrorMsg";
        }
    }

    private void BindCategory()
    {
        chkCategory.Items.Clear();

        DataTable dt = _restaurant.GetRestaurantCategory();

        if (dt.Rows.Count > 0)
        {
            chkCategory.DataSource = dt;
            chkCategory.DataTextField = "Category";
            chkCategory.DataValueField = "RestaurantCategoryId";
            chkCategory.DataBind();
        }
    }
}