using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class clsAttraction
{
    private Int64 _AttractionId;
    public Int64 AttractionId
    {
        get
        {
            return _AttractionId;
        }
        set
        {
            _AttractionId = value;
        }
    }

    private Int32 _UserId;
    public Int32 UserId
    {
        get
        {
            return _UserId;
        }
        set
        {
            _UserId = value;
        }
    }

    private string _Name;
    public string Name
    {
        get
        {
            return _Name;
        }
        set
        {
            _Name = value;
        }
    }

    private string _Description;
    public string Description
    {
        get
        {
            return _Description;
        }
        set
        {
            _Description = value;
        }
    }

    private string _MainPhoto;
    public string MainPhoto
    {
        get
        {
            return _MainPhoto;
        }
        set
        {
            _MainPhoto = value;
        }
    }

    private string _CoverPhoto;
    public string CoverPhoto
    {
        get
        {
            return _CoverPhoto;
        }
        set
        {
            _CoverPhoto = value;
        }
    }

    private string _Address;
    public string Address
    {
        get
        {
            return _Address;
        }
        set
        {
            _Address = value;
        }
    }

    private Int32 _Pincode;
    public Int32 Pincode
    {
        get
        {
            return _Pincode;
        }
        set
        {
            _Pincode = value;
        }
    }


    private string _ContactNumber;
    public string ContactNumber
    {
        get
        {
            return _ContactNumber;
        }
        set
        {
            _ContactNumber = value;
        }
    }

    private string _AlternateNumber;
    public string AlternateNumber
    {
        get
        {
            return _AlternateNumber;
        }
        set
        {
            _AlternateNumber = value;
        }
    }

    private string _EmailAddress;
    public string EmailAddress
    {
        get
        {
            return _EmailAddress;
        }
        set
        {
            _EmailAddress = value;
        }
    }

    private string _Website;
    public string Website
    {
        get
        {
            return _Website;
        }
        set
        {
            _Website = value;
        }
    }

    private bool _InVadodara;
    public bool InVadodara
    {
        get
        {
            return _InVadodara;
        }
        set
        {
            _InVadodara = value;
        }
    }

    private bool _Publish;
    public bool Publish
    {
        get
        {
            return _Publish;
        }
        set
        {
            _Publish = value;
        }
    }

    private string _Latitude;
    public string Latitude
    {
        get
        {
            return _Latitude;
        }
        set
        {
            _Latitude = value;
        }
    }

    private string _Longitutde;
    public string Longitutde
    {
        get
        {
            return _Longitutde;
        }
        set
        {
            _Longitutde = value;
        }
    }

    public bool AddAttraction()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "spAttraction";

        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = _Name;
        cmd.Parameters.Add("@MainPhoto", SqlDbType.NVarChar, 500).Value = _MainPhoto;
        cmd.Parameters.Add("@CoverPhoto", SqlDbType.NVarChar, 500).Value = _CoverPhoto;
        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = _Description;
        cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 300).Value = _Address;
        cmd.Parameters.Add("@Pincode", SqlDbType.BigInt).Value = _Pincode;
        cmd.Parameters.Add("@ContactNumber", SqlDbType.VarChar,20).Value = _ContactNumber;
        cmd.Parameters.Add("@AlternateNumber", SqlDbType.VarChar, 20).Value = _AlternateNumber;
        cmd.Parameters.Add("@EmailAddress", SqlDbType.NVarChar, 100).Value = _EmailAddress;
        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 100).Value = _Website;
        cmd.Parameters.Add("@InVadodara", SqlDbType.Bit).Value = _InVadodara;
        cmd.Parameters.Add("@Publish", SqlDbType.Bit).Value = _Publish;
        cmd.Parameters.Add("@Latitude", SqlDbType.NVarChar, 30).Value = _Latitude;
        cmd.Parameters.Add("@Longitude", SqlDbType.NVarChar, 30).Value = _Longitutde;
        cmd.Parameters.Add("@DateTime", SqlDbType.VarChar, 30).Value = clsConfiguration.Datetime();
        cmd.Parameters.Add("@IPAddress", SqlDbType.VarChar, 20).Value = clsConfiguration.IPAddress();
        cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = _UserId;
        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "AddAttraction";

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        return true;
    }

    public DataTable GetAttraction()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "spAttraction";

        cmd.Parameters.Add("@Publish", SqlDbType.Bit).Value = _Publish;
        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "GetAttraction";

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }

    public DataTable GetAttractionInfo()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "spAttraction";

        cmd.Parameters.Add("@AttractionId", SqlDbType.BigInt).Value = _AttractionId;
        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "GetAttractionInfo";

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }
}