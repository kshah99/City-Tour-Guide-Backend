using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class clsUser
{
    private Int64 _UserId;
    public Int64 UserId
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

    private string _Email;
    public string Email
    {
        get
        {
            return _Email;
        }
        set
        {
            _Email = value;
        }
    }

    private string _Password;
    public string Password
    {
        get
        {
            return _Password;
        }
        set
        {
            _Password = value;
        }
    }

    public DataTable Login()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "spUser";

        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = _Email;
        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = _Password;
        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "Login";

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }

    public DataTable CheckEmail()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "spUser";

        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = _Email;
        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "CheckEmail";

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }

    public bool AddLoginData()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "spUser";

        cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = _UserId;
        cmd.Parameters.Add("@DateTime", SqlDbType.VarChar, 30).Value = clsConfiguration.Datetime();
        cmd.Parameters.Add("@IPAddress", SqlDbType.VarChar, 30).Value = clsConfiguration.IPAddress();
        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "AddLoginData";

        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();

        return true;
    }

    public DataTable Profile()
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "spUser";

        cmd.Parameters.Add("@UserId", SqlDbType.BigInt).Value = _UserId;
        cmd.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "Profile";

        DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);

        return dt;
    }
}