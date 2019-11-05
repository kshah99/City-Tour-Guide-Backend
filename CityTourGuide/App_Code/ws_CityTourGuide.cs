using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Text;

[WebService(Namespace = "http://citytourguide.mytestbuddy.net/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class ws_CityTourGuide : System.Web.Services.WebService
{
    clsAttraction _attraction = new clsAttraction();
    clsHotel _hotel = new clsHotel();
    clsRestaurant _restaurant = new clsRestaurant();


    public class ShowMessage
    {
        public string Message;
    }

    public class CommonListData
    {
        public string ListId;
        public string Name;
        public string Description;
        public string Photo;
    }

    public class CommonInfo
    {
        public string TypeId;
        public string Name;
        public string Description;
        public string Photo;
        public string Address;
        public string Pincode;
        public string ContactNumber;
        public string Website;
    }

    public class Review
    {
        public string UserId;
        public string Type;
        public string TypeId;
        public string Comment;
        public string Rating;
    }

    public class LocationData
    {
        public string Name;
        public string Address;
        public string Latitude;
        public string Longitude;
    }

    [WebMethod]
    public string Register(string FirstName, string LastName, string Email, string Password, string ContactNumber)
    {
        List<ShowMessage> lstMessage = new List<ShowMessage>();
        ShowMessage _message = new ShowMessage();

        JavaScriptSerializer jsMessage = new JavaScriptSerializer();

        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Connection.GetConnectionString();

            // Check Email...
            DataTable dtCE = clsCityTourGuide.CheckEmail(Email);

            if (dtCE.Rows.Count > 0)
            {
                // Already Registered...
                _message.Message = "already_registered";
            }
            else
            {
                Guid random = Guid.NewGuid();
                string PersonalId = random.ToString().Substring(0, 36);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spUser";

                cmd.Parameters.Add("@PersonalId", SqlDbType.NVarChar, 100).Value = PersonalId;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = LastName;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Password;
                cmd.Parameters.Add("@ContactNumber", SqlDbType.BigInt).Value = ContactNumber;
                cmd.Parameters.Add("@DateTime", SqlDbType.VarChar, 30).Value = clsConfiguration.Datetime();
                cmd.Parameters.Add("@IPAddress", SqlDbType.VarChar, 20).Value = clsConfiguration.IPAddress();
                cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "Register";

                con.Open();
                string UserId = cmd.ExecuteScalar().ToString();
                con.Close();

                // Send Activation Email...
                string key = UserId + "/" + PersonalId;

                StringBuilder sb = new StringBuilder();
                sb.Append("Hello " + FirstName + " " + LastName + ",");
                sb.Append("<br/><br/>");
                sb.Append("Greetings from City Tour Guide!");
                sb.Append("<br/><br/>");
                sb.Append("Thank you for registering with us. Please click on link below to activate your account :");
                sb.Append("<br/><br/>");
                sb.Append("http://citytourguide.mytestbuddy.net/activate.aspx?uid=" + key + "");
                sb.Append("<br/><br/><br/>");
                sb.Append("<b>Thanks & Regards,</b>");
                sb.Append("<br/>");
                sb.Append("City Tour Guide Team");

                //clsMail.SendMail(Email, sb.ToString(), "Activate your account on City Tour Guide");

                _message.Message = "success";
            }

            lstMessage.Insert(0, _message);
            return jsMessage.Serialize(lstMessage);
        }
        catch (Exception ex)
        {
            // Send Email to Admin regarding Exception...

            _message.Message = "error";

            lstMessage.Insert(0, _message);
            return jsMessage.Serialize(lstMessage);
        }
    }

    [WebMethod]
    public string Login(string Email, string Password)
    {
        List<ShowMessage> lstMessage = new List<ShowMessage>();
        ShowMessage _message = new ShowMessage();

        JavaScriptSerializer jsMessage = new JavaScriptSerializer();
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Connection.GetConnectionString();

            // Check Login...
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUser";

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;
            cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = Password;
            cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "Login";

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Active"].ToString() == "True")
                {
                    // Add Login Data...
                    SqlCommand cmdALD = new SqlCommand();
                    cmdALD.Connection = con;
                    cmdALD.CommandType = CommandType.StoredProcedure;
                    cmdALD.CommandText = "spUser";

                    cmdALD.Parameters.Add("@UserId", SqlDbType.BigInt).Value = dt.Rows[0]["UserId"].ToString();
                    cmdALD.Parameters.Add("@DateTime", SqlDbType.VarChar, 30).Value = clsConfiguration.Datetime();
                    cmdALD.Parameters.Add("@IPAddress", SqlDbType.VarChar, 20).Value = clsConfiguration.IPAddress();
                    cmdALD.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "AddLoginData";

                    con.Open();
                    cmdALD.ExecuteNonQuery();
                    con.Close();

                    _message.Message = dt.Rows[0]["UserId"].ToString();
                }
                else
                {
                    _message.Message = "not_active";
                }
            }
            else
            {
                DataTable dtCE = clsCityTourGuide.CheckEmail(Email);

                if (dtCE.Rows.Count > 0)
                {
                    _message.Message = "invalid";
                }
                else
                {
                    _message.Message = "not_registered";
                }
            }

            lstMessage.Insert(0, _message);
            return jsMessage.Serialize(lstMessage);
        }
        catch (Exception ex)
        {
            // Send Email to Admin regarding Exception...

            _message.Message = "error";

            lstMessage.Insert(0, _message);
            return jsMessage.Serialize(lstMessage);
        }
    }

    [WebMethod]
    public string ForgotPassword(string Email)
    {
        List<ShowMessage> lstMessage = new List<ShowMessage>();
        ShowMessage _message = new ShowMessage();

        JavaScriptSerializer jsMessage = new JavaScriptSerializer();

        try
        {
            DataTable dt = clsCityTourGuide.CheckEmail(Email);

            if (dt.Rows.Count > 0)
            {
                //Send Password on Email...
                StringBuilder sb = new StringBuilder();
                sb.Append("Hello " + dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString());
                sb.Append("<br/><br/>");
                sb.Append("Greetings from City Tour Guide!");
                sb.Append("<br/><br/>");
                sb.Append("Recently, You requested for Password. Please find your details as below :");
                sb.Append("<br/><br/>");
                sb.Append("<br>Email: </b>" + Email);
                sb.Append("<br/>");
                sb.Append("<b>Password: </b>" + dt.Rows[0]["Password"].ToString());
                sb.Append("<br/><br/>");
                sb.Append("<b>Thanks & Regards,</b>");
                sb.Append("<br/>");
                sb.Append("City Tour Guide Team");

                //clsMail.SendMail(Email, sb.ToString(), "Your account Password from City Tour Guide");

                _message.Message = "success";
            }
            else
            {
                _message.Message = "not_registered";
            }

            lstMessage.Insert(0, _message);
            return jsMessage.Serialize(lstMessage);
        }
        catch (Exception ex)
        {
            _message.Message = "error";

            lstMessage.Insert(0, _message);
            return jsMessage.Serialize(lstMessage);
        }
    }

    [WebMethod]
    public string GetCommonList(string Type)
    {
        List<CommonListData> lstCommonListData = new List<CommonListData>();

        JavaScriptSerializer jsCommonListData = new JavaScriptSerializer();

        try
        {
            if (Type == "Attraction")
            {
                _attraction.Publish = true;
                DataTable dt = _attraction.GetAttraction();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CommonListData _commonListData = new CommonListData();

                        _commonListData.ListId = dt.Rows[i]["AttractionId"].ToString();
                        _commonListData.Name = dt.Rows[i]["Name"].ToString();
                        if (dt.Rows[i]["Description"].ToString().Length > 50)
                        {
                            _commonListData.Description = dt.Rows[i]["Description"].ToString().Substring(0, 50) + "...";
                        }
                        else
                        {
                            _commonListData.Description = dt.Rows[i]["Description"].ToString();
                        }
                        if (dt.Rows[i]["MainPhoto"].ToString() == "")
                        {
                            _commonListData.Photo = "images/Attraction/main_photo/thumb/df__12-02-2015__11-34-37.jpg";
                        }
                        else
                        {
                            _commonListData.Photo = "images/Attraction/main_photo/thumb/" + dt.Rows[i]["MainPhoto"].ToString();
                        }

                        lstCommonListData.Insert(i, _commonListData);
                    }
                }
            }
            else if (Type == "Hotel")
            {
                _hotel.Publish = true;
                DataTable dt = _hotel.GetHotel();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CommonListData _commonListData = new CommonListData();


                        _commonListData.ListId = dt.Rows[i]["HotelId"].ToString();
                        _commonListData.Name = dt.Rows[i]["Name"].ToString();
                        if (dt.Rows[i]["Description"].ToString().Length > 50)
                        {
                            _commonListData.Description = dt.Rows[i]["Description"].ToString().Substring(0, 50) + "...";
                        }
                        else
                        {
                            _commonListData.Description = dt.Rows[i]["Description"].ToString();
                        }
                        if (dt.Rows[i]["MainPhoto"].ToString() == "")
                        {
                            _commonListData.Photo = "images/hotel/main_photo/thumb/d__12-02-2015__13-28-10.jpg";
                        }
                        else
                        {
                            _commonListData.Photo = "images/hotel/main_photo/thumb/" + dt.Rows[i]["MainPhoto"].ToString();
                        }

                        lstCommonListData.Insert(i, _commonListData);
                    }
                }

            }
            else if (Type == "Restaurant")
            {
                _restaurant.Publish = true;
                DataTable dt = _restaurant.GetRestaurant();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CommonListData _commonListData = new CommonListData();


                        _commonListData.ListId = dt.Rows[i]["RestaurantId"].ToString();
                        _commonListData.Name = dt.Rows[i]["Name"].ToString();
                        if (dt.Rows[i]["Description"].ToString().Length > 50)
                        {
                            _commonListData.Description = dt.Rows[i]["Description"].ToString().Substring(0, 50) + "...";
                        }
                        else
                        {
                            _commonListData.Description = dt.Rows[i]["Description"].ToString();
                        }
                        if (dt.Rows[i]["MainPhoto"].ToString() == "")
                        {
                            _commonListData.Photo = "images/Restaurant/main_photo/thumb/tygjhjlo__06-02-2015__16-20-50.jpg";
                        }
                        else
                        {
                            _commonListData.Photo = "images/Restaurant/main_photo/thumb/" + dt.Rows[i]["MainPhoto"].ToString();
                        }

                        lstCommonListData.Insert(i, _commonListData);
                    }
                }
            }
        }
        catch
        {
            CommonListData _commonListData = new CommonListData();
            _commonListData.ListId = "Server Error";
            _commonListData.Name = "Server Error";
            _commonListData.Description = "Server Error";
            _commonListData.Photo = "Server Error";

            lstCommonListData.Insert(0, _commonListData);

            return jsCommonListData.Serialize(lstCommonListData);
        }

        return jsCommonListData.Serialize(lstCommonListData);
    }

    [WebMethod]
    public string GetCommonInfo(string Type, string TypeId)
    {
        List<CommonInfo> lstCommonInfo = new List<CommonInfo>();
        CommonInfo _commonInfo = new CommonInfo();

        JavaScriptSerializer jsCommonInfo = new JavaScriptSerializer();

        try
        {
            if (Type == "Attraction")
            {
                _attraction.AttractionId = Convert.ToInt64(TypeId);
                DataTable dt = _attraction.GetAttractionInfo();

                if (dt.Rows.Count > 0)
                {
                    _commonInfo.TypeId = dt.Rows[0]["AttractionId"].ToString();
                    _commonInfo.Name = dt.Rows[0]["Name"].ToString();
                    _commonInfo.Description = dt.Rows[0]["Description"].ToString();
                    if (dt.Rows[0]["MainPhoto"].ToString() == "")
                    {
                        _commonInfo.Photo = "images/Attraction/main_photo/thumb/df__12-02-2015__11-34-37.jpg";
                    }
                    else
                    {
                        _commonInfo.Photo = "images/Attraction/main_photo/thumb/" + dt.Rows[0]["MainPhoto"].ToString();
                    }
                    _commonInfo.Address = dt.Rows[0]["Address"].ToString();
                    _commonInfo.Pincode = dt.Rows[0]["Pincode"].ToString();
                    _commonInfo.ContactNumber = dt.Rows[0]["ContactNumber"].ToString();
                    _commonInfo.Website = dt.Rows[0]["Website"].ToString();

                    lstCommonInfo.Insert(0, _commonInfo);
                }
            }
            else if (Type == "Hotel")
            {
                _hotel.HotelId = Convert.ToInt64(TypeId);
                DataTable dt = _hotel.GetHotelInfo();

                if (dt.Rows.Count > 0)
                {
                    _commonInfo.TypeId = dt.Rows[0]["HotelId"].ToString();
                    _commonInfo.Name = dt.Rows[0]["Name"].ToString();
                    _commonInfo.Description = dt.Rows[0]["Description"].ToString();
                    if (dt.Rows[0]["MainPhoto"].ToString() == "")
                    {
                        _commonInfo.Photo = "images/Attraction/main_photo/thumb/df__12-02-2015__11-34-37.jpg";
                    }
                    else
                    {
                        _commonInfo.Photo = "images/Attraction/main_photo/thumb/" + dt.Rows[0]["MainPhoto"].ToString();
                    }
                    _commonInfo.Address = dt.Rows[0]["Address"].ToString();
                    _commonInfo.Pincode = dt.Rows[0]["Pincode"].ToString();
                    _commonInfo.ContactNumber = dt.Rows[0]["ContactNumber"].ToString();
                    _commonInfo.Website = dt.Rows[0]["Website"].ToString();

                    lstCommonInfo.Insert(0, _commonInfo);
                }
            }
            else if (Type == "Restaurant")
            {
                _restaurant.RestaurantId = Convert.ToInt64(TypeId);
                DataTable dt = _restaurant.GetRestaurantInfo();

                if (dt.Rows.Count > 0)
                {
                    _commonInfo.TypeId = dt.Rows[0]["RestaurantId"].ToString();
                    _commonInfo.Name = dt.Rows[0]["Name"].ToString();
                    _commonInfo.Description = dt.Rows[0]["Description"].ToString();
                    if (dt.Rows[0]["MainPhoto"].ToString() == "")
                    {
                        _commonInfo.Photo = "images/Attraction/main_photo/thumb/df__12-02-2015__11-34-37.jpg";
                    }
                    else
                    {
                        _commonInfo.Photo = "images/Attraction/main_photo/thumb/" + dt.Rows[0]["MainPhoto"].ToString();
                    }
                    _commonInfo.Address = dt.Rows[0]["Address"].ToString();
                    _commonInfo.Pincode = dt.Rows[0]["Pincode"].ToString();
                    _commonInfo.ContactNumber = dt.Rows[0]["ContactNumber"].ToString();
                    _commonInfo.Website = dt.Rows[0]["Website"].ToString();

                    lstCommonInfo.Insert(0, _commonInfo);
                }
            }

            return jsCommonInfo.Serialize(lstCommonInfo);
        }
        catch
        {
            return "error";
        }
    }

    [WebMethod]
    public string AddUserReview(string UserId, string Type, string TypeId, string Comment, string Rating)
    {

        List<ShowMessage> lstMessage = new List<ShowMessage>();
        ShowMessage _message = new ShowMessage();

        JavaScriptSerializer jsMessage = new JavaScriptSerializer();

        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = Connection.GetConnectionString();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUserReviews";

            cmd.Parameters.Add("@UserId", SqlDbType.NVarChar, 50).Value = UserId;
            cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = Type;
            cmd.Parameters.Add("@TypeId", SqlDbType.BigInt).Value = TypeId;
            cmd.Parameters.Add("@Comment", SqlDbType.NVarChar, 500).Value = Comment;
            cmd.Parameters.Add("@Rating", SqlDbType.BigInt).Value = Convert.ToInt64(Rating).ToString("0");
            cmd.Parameters.Add("@DateTime", SqlDbType.VarChar, 30).Value = clsConfiguration.Datetime();
            cmd.Parameters.Add("@IPAddress", SqlDbType.VarChar, 20).Value = clsConfiguration.IPAddress();
            cmd.Parameters.Add("@Mode", SqlDbType.NVarChar, 50).Value = "AddUserReview";

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            _message.Message = "success";
            lstMessage.Insert(0, _message);

            return jsMessage.Serialize(lstMessage);
        }
        catch (Exception ex)
        {
            return "error";
        }
    }

    [WebMethod]
    public string GetLocationData(string Type)
    {
        List<LocationData> lstLocationData = new List<LocationData>();

        JavaScriptSerializer jsLocationData = new JavaScriptSerializer();

        try
        {
            if (Type == "Attraction")
            {
                _attraction.Publish = true;
                DataTable dt = _attraction.GetAttraction();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LocationData _LocationData = new LocationData();

                        _LocationData.Name = dt.Rows[i]["Name"].ToString();
                        _LocationData.Address = dt.Rows[i]["Address"].ToString();
                        _LocationData.Latitude = dt.Rows[i]["Latitude"].ToString();
                        _LocationData.Longitude = dt.Rows[i]["Longitude"].ToString();

                        lstLocationData.Insert(i, _LocationData);
                    }
                }
            }
            else if (Type == "Hotel")
            {
                _hotel.Publish = true;
                DataTable dt = _hotel.GetHotel();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LocationData _LocationData = new LocationData();

                        _LocationData.Name = dt.Rows[i]["Name"].ToString();
                        _LocationData.Address = dt.Rows[i]["Address"].ToString();
                        _LocationData.Latitude = dt.Rows[i]["Latitude"].ToString();
                        _LocationData.Longitude = dt.Rows[i]["Longitude"].ToString();

                        lstLocationData.Insert(i, _LocationData);
                    }
                }
            }
            else if (Type == "Restaurant")
            {
                _restaurant.Publish = true;
                DataTable dt = _restaurant.GetRestaurant();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        LocationData _LocationData = new LocationData();

                        _LocationData.Name = dt.Rows[i]["Name"].ToString();
                        _LocationData.Address = dt.Rows[i]["Address"].ToString();
                        _LocationData.Latitude = dt.Rows[i]["Latitude"].ToString();
                        _LocationData.Longitude = dt.Rows[i]["Longitude"].ToString();

                        lstLocationData.Insert(i, _LocationData);
                    }
                }
            }
        }
        catch
        {
            LocationData _LocationData = new LocationData();

            _LocationData.Name = "Server Error";
            _LocationData.Address = "Server Error";
            _LocationData.Latitude = "Server Error";
            _LocationData.Longitude = "Server Error";

            lstLocationData.Insert(0, _LocationData);
        }

        return jsLocationData.Serialize(lstLocationData);
    }

  /*  [WebMethod]
    public string GetCommonCategory(string Type) {

        List<CommonCategoryData> lstCommonCategoryData = new List<CommonCategoryData>();

        JavaScriptSerializer jsCommonCategoryData = new JavaScriptSerializer();

        try
        {
            if (Type == "Attraction")
            {
                _attraction.Publish = true;
                DataTable dt = _attraction.GetAttractionCategory();
            }
            if (Type == "Restaurant")
            {
                _restaurant.Publish = true;
                DataTable dt = _attraction.GetRestaurantCategory();
            }
            if (Type == "Hotel")
            {
                _hotel.Publish = true;
                DataTable dt = _attraction.GetHotelCategory();
            }

        }
        catch { 
        
        }
    }*/
}