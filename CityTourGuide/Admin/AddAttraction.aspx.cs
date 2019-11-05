using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Admin_AddAttraction : System.Web.UI.Page
{
    clsAttraction _attraction = new clsAttraction();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnAddAttraction_Click(object sender, EventArgs e)
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
                    MainPhoto = txtAttractionName.Text.Trim().Replace(" ", "-") + "__" + clsConfiguration.PhotoDateTime() + ".jpg";

                    clsGraphics.GenerateThumbnails(150, 150, hpf1.InputStream, Server.MapPath("~\\images\\Attraction\\main_photo\\thumb\\") + MainPhoto);
                    clsGraphics.GenerateThumbnails(300, 300, hpf1.InputStream, Server.MapPath("~\\images\\Attraction\\main_photo\\small\\") + MainPhoto);
                    clsGraphics.ResizeImage(500, 500, hpf1.InputStream, Server.MapPath("~\\images\\Attraction\\main_photo\\big\\") + MainPhoto);
                }

                if (hpf2.ContentLength > 0)
                {
                    CoverPhoto = txtAttractionName.Text.Trim().Replace(" ", "-") + "__" + clsConfiguration.PhotoDateTime() + ".jpg";

                    clsGraphics.ResizeImage(100, 50, hpf2.InputStream, Server.MapPath("~\\images\\Attraction\\cover_photo\\thumb\\") + CoverPhoto);
                    clsGraphics.ResizeImage(500, 300, hpf2.InputStream, Server.MapPath("~\\images\\Attraction\\cover_photo\\small\\") + CoverPhoto);
                    clsGraphics.ResizeImage(1000, 500, hpf2.InputStream, Server.MapPath("~\\images\\Attraction\\cover_photo\\big\\") + CoverPhoto);
                }
            }

            _attraction.Name = txtAttractionName.Text.Trim();
            _attraction.MainPhoto = MainPhoto;
            _attraction.CoverPhoto = CoverPhoto;
            _attraction.Description = txtAttractionDescription.Text.Trim();
            _attraction.Address = txtAttractionAddress.Text.Trim();
            _attraction.Pincode = Convert.ToInt32(txtPincode.Text.Trim());
            _attraction.ContactNumber = txtContactNumber.Text.Trim();
            _attraction.AlternateNumber = txtAlternateNumber.Text.Trim();
            _attraction.EmailAddress = txtEmailAddress.Text.Trim();
            _attraction.Website = txtWebsite.Text.Trim();
            _attraction.InVadodara = chkInVadodara.Checked;
            _attraction.Publish = chkPublish.Checked;
            _attraction.Latitude = hdnLatitude.Value;
            _attraction.Longitutde = hdnLongitude.Value;
            _attraction.UserId = Convert.ToInt32(clsSecurity.DecryptText(Session["UserID"].ToString()));

            _attraction.AddAttraction();

            lblNote.Visible = true;
            lblNote.Text = "You have successfully added Attraction.";
            lblNote.CssClass = "SuccessMsg";

            //ddlCategory.SelectedValue = "-999999";
            txtAttractionName.Text = "";
            txtAttractionDescription.Text = "";
            txtAttractionAddress.Text = "";
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

   /* private void BindCategory()
    {
        ddlCategory.Items.Clear();

        DataTable dt = _attraction.GetAttractionCategory();

        if (dt.Rows.Count > 0)
        {
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "Category";
            ddlCategory.DataValueField = "AttrcationCategoryId";
            ddlCategory.DataBind();
        }

        ddlCategory.Items.Insert(0, new ListItem("Select Category", "-999999"));
    }*/

    }
