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
        private string cardNum;
        public CreditCardService() { }
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
        { //Returns a specified integer based on parameter query
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand(query, Conn);
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
        [WebMethod]
        public bool TransactionSuccess(string cardNum, double total)
        { //Attempts to complete transaction, returns whether successful
            this.cardNum = cardNum;
            if (Denied("SUM(limit - " + total + ")") || Denied("DATEDIFF('day', DATE(), validityDate)")) return false;
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand("INSERT INTO Transactions(creditCard, total, dateOfPurchase, companyName) VALUES(@creditCard, @total, @date, 'Dungeon Maker')", Conn);
            command.Parameters.AddWithValue("@creditCard", cardNum);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            Conn.Open();
            command.ExecuteNonQuery();
            command = new OleDbCommand("UPDATE CreditCards SET balance = balance - " + total + " WHERE creditCardNumber = '" + cardNum + "'", Conn);
            command.ExecuteNonQuery();
            Conn.Close();
            return true;
        }
        private bool Denied(string field) { return GetIntByQuery("SELECT " + field + " FROM CreditCards WHERE creditCardNumber = '" + cardNum + "'") < 0; }
        [WebMethod]
        public string GetEmailByHolder(Holder holder) 
        { //Gets email through the rest of the mandatory credentials
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand("SELECT email FROM CardHolders WHERE firstName = @firstName AND lastName = @lastName AND phoneNumber = @phoneNumber AND billingAddress = @billingAddress", Conn);
            command.Parameters.AddWithValue("@firstName", holder.firstName);
            command.Parameters.AddWithValue("@lastName", holder.lastName);
            command.Parameters.AddWithValue("@phoneNumber", holder.phoneNumber);
            command.Parameters.AddWithValue("@billingAddress", holder.billingaddress);
            Conn.Open();
            try { return command.ExecuteScalar().ToString(); }
            catch { return null; }
            finally { Conn.Close(); }
        }
        private string GetConnectionString()
        {
            string FILE_NAME = "CreditCardDB.accdb", 
                Location = HttpContext.Current.Server.MapPath("~/CreditCardWebService/App_Data/" + FILE_NAME), 
                ConnectionString = @"provider=Microsoft.ACE.OLEDB.12.0; data source =" + Location;
            return ConnectionString;
        }
    }
}