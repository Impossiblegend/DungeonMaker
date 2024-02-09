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
        public static string SecToMin(int sec)
        { //e.g. 128 sec --> 02:08 min
            string min = "";
            if (sec < 600) min = "0";
            min += sec / 60 + ":";
            if (sec % 60 < 10) min += "0";
            min += sec % 60;
            return min;
        }
    }
}