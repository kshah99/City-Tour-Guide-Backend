using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class clsCityTourGuide
{
    public static DataTable CheckEmail(string Email)
    {
        SqlConnection con = new SqlConnection();
        con.ConnectionString = Connection.GetConnectionString();

        SqlCommand cmdCE = new SqlCommand();
        cmdCE.Connection = con;
        cmdCE.CommandType = CommandType.StoredProcedure;
        cmdCE.CommandText = "spUser";

        cmdCE.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;
        cmdCE.Parameters.Add("@Mode", SqlDbType.VarChar, 50).Value = "CheckEmail";

        DataTable dtCE = new DataTable();
        SqlDataAdapter daCE = new SqlDataAdapter(cmdCE);
        daCE.Fill(dtCE);

        return dtCE;
    }
}