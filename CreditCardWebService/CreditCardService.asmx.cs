using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CreditCardWebService
{
    /// <summary>
    /// Summary description for CreditService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CreditCardService : System.Web.Services.WebService
    {
        public CreditCardService() { }
        [WebMethod]
        public Card GetCardByEmail(string email) 
        {
            DataRow row = GetDataSetByQuery("SELECT * FROM CardHolders, CreditCards WHERE email = '" + email + "'", "").Tables[0].Rows[0];
            string[] a = row.ItemArray.Select(col => col.ToString()).ToArray();
            try { return new Card(a[6], new Holder(a[0], a[1], a[2], DateTime.Parse(a[3]), a[4], a[5]) , a[8], int.Parse(a[9]), DateTime.Parse(a[10]), int.Parse(a[11]), int.Parse(a[12])); }
            catch { return null; }
        }
        [WebMethod]
        public DataSet GetDataSetByQuery(string query, string table)
        { //Returns a dataset based on a given query. Table parameter is for indexing.
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand(query, Conn);
            Conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dt1 = new DataTable(table);
            try { adapter.Fill(dt1); }
            catch (OleDbException ex) { throw new Exception("Incorrect SQL query: " + ex.Message); }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            Conn.Close();
            return ds;
        }
        [WebMethod]
        public int GetIntByQuery(string query)
        {
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand(query, Conn);
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
        private string GetConnectionString()
        {
            string FILE_NAME = "CreditCardDB.accdb";
            string Location = HttpContext.Current.Server.MapPath("~/CreditCardWebService/App_Data/" + FILE_NAME);
            string ConnectionString = @"provider=Microsoft.ACE.OLEDB.12.0; data source =" + Location;
            return ConnectionString;
        }
    }
}