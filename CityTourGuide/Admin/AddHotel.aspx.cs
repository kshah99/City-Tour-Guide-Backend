using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_AddHotel : System.Web.UI.Page
{
    clsHotel _hotel = new clsHotel();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCategory();
        }
    }

    protected void btnAddHotel_Click(object sender, EventArgs e)
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
                    MainPhoto = txtHotelName.Text.Trim().Replace(" ", "-") + "__" + clsConfiguration.PhotoDateTime() + ".jpg";

                    clsGraphics.GenerateThumbnails(150, 150, hpf1.InputStream, Server.MapPath("~\\images\\hotel\\main_photo\\thumb\\") + MainPhoto);
                    clsGraphics.GenerateThumbnails(300, 300, hpf1.InputStream, Server.MapPath("~\\images\\hotel\\main_photo\\small\\") + MainPhoto);
                    clsGraphics.ResizeImage(500, 500, hpf1.InputStream, Server.MapPath("~\\images\\hotel\\main_photo\\big\\") + MainPhoto);
                }

                if (hpf2.ContentLength > 0)
                {
                    CoverPhoto = txtHotelName.Text.Trim().Replace(" ", "-") + "__" + clsConfiguration.PhotoDateTime() + ".jpg";

                    clsGraphics.ResizeImage(200, 100, hpf2.InputStream, Server.MapPath("~\\images\\hotel\\cover_photo\\thumb\\") + CoverPhoto);
                    clsGraphics.ResizeImage(500, 300, hpf2.InputStream, Server.MapPath("~\\images\\hotel\\cover_photo\\small\\") + CoverPhoto);
                    clsGraphics.ResizeImage(1000, 500, hpf2.InputStream, Server.MapPath("~\\images\\hotel\\cover_photo\\big\\") + CoverPhoto);
                }
            }

            _hotel.HotelCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            _hotel.Name = txtHotelName.Text.Trim();
            _hotel.MainPhoto = MainPhoto;
            _hotel.CoverPhoto = CoverPhoto;
            _hotel.Description = txtHotelDescription.Text.Trim();
            _hotel.Address = txtHotelAddress.Text.Trim();
            _hotel.Pincode = Convert.ToInt32(txtPincode.Text.Trim());
            _hotel.ContactNumber = txtContactNumber.Text.Trim();
            _hotel.AlternateNumber = txtAlternateNumber.Text.Trim();
            _hotel.EmailAddress = txtEmailAddress.Text.Trim();
            _hotel.Website = txtWebsite.Text.Trim();
            _hotel.Publish = chkPublish.Checked;
            _hotel.Latitude = hdnLatitude.Value;
            _hotel.Longitutde = hdnLongitude.Value;
            _hotel.UserId = Convert.ToInt32(clsSecurity.DecryptText(Session["UserID"].ToString()));

            _hotel.AddHotel();

            lblNote.Visible = true;
            lblNote.Text = "You have successfully added Hotel.";
            lblNote.CssClass = "SuccessMsg";

            ddlCategory.SelectedValue = "-999999";
            txtHotelName.Text = "";
            txtHotelDescription.Text = "";
            txtHotelAddress.Text = "";
            txtPincode.Text = "";
            txtContactNumber.Text = "";
            txtAlternateNumber.Text = "";
            txtEmailAddress.Text = "";
            txtWebsite.Text = "";

        }
        catch(Exception ex)
        {
            lblNote.Visible = true;
            lblNote.Text = ex.Message.ToString();
            //lblNote.Text = "Something is Wrong.";
            lblNote.CssClass = "ErrorMsg";
        }
    }

    private void BindCategory()
    {
        ddlCategory.Items.Clear();

        DataTable dt = _hotel.GetHotelCategory();

        if (dt.Rows.Count > 0)
        {
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "Category";
            ddlCategory.DataValueField = "HotelCategoryId";
            ddlCategory.DataBind();
        }

        ddlCategory.Items.Insert(0, new ListItem("Select Category", "-999999"));
    }
}