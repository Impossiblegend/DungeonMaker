using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DungeonMaker
{
    public class Utility
    {
        public static string SecToMin(int sec) { return string.Format("{0:D2}:{1:D2}", sec / 60, sec % 60); } //e.g. 128 --> "02:08"

        public static int MinToSec(string time)
        { //e.g. "02:08" --> 128
            string[] parts = time.Split(':');
            return (int.Parse(parts[0]) * 60) + int.Parse(parts[1]);
        }

        public static string DecimalCommas(string st)
        { //e.g. "1234567890" --> "1,234,567,890"
            for (int i = 3; i < st.Length; i += 4) st = st.Insert(st.Length - i, ",");
            return st;
        }

        public static string GetConnectionString() { return ConfigurationManager.ConnectionStrings["DungeonMakerConnectionString"].ConnectionString; }
    }
}