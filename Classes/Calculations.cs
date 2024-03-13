﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DungeonMaker
{
    public class Calculations
    {
        public static string SecToMin(int sec)
        { //e.g. 128 --> "02:08"
            string min = "";
            if (sec < 600) min = "0";
            min += sec / 60 + ":";
            if (sec % 60 < 10) min += "0";
            min += sec % 60;
            return min;
        }

        public static string DecimalCommas(string st)
        { //e.g. "1234567890" --> "1,234,567,890"
            for (int i = 3; i < st.Length; i += 4) st = st.Insert(st.Length - i, ",");
            return st;
        }

        //DataSet ds = GeneralService.GetDataSetByQuery("SELECT email FROM Users", "Users");
        //foreach (DataRow row in ds.Tables[0].Rows) { StoreService SS = new StoreService(); StoreService.Purchase(new User(row[0].ToString()),"INSERT_NEW_PRODUCT_HERE"); }
    }
}