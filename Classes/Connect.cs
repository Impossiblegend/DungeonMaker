using System;
using System.Web;

namespace DungeonMaker
{
    public class Connect
    {
        public static string GetConnectionString()
        {
            string FILE_NAME = "DungeonMaker.accdb";
            string Location = HttpContext.Current.Server.MapPath("~/App_Data/" + FILE_NAME);
            string ConnectionString = @"provider=Microsoft.ACE.OLEDB.12.0; data source =" + Location;
            return ConnectionString;
        }
    }
}