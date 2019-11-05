using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Connection
{
    public static string GetConnectionString()
    {
        return System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    }
}