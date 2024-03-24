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
        public Card GetCardByHolder(string email) 
        {
            DataRow row = null;
            try { row = GetDataSetByQuery("SELECT * FROM CardHolders, CreditCards WHERE email = '" + email + "' AND rememberMe", "").Tables[0].Rows[0]; }
            catch { return null; }
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
        { //Returns a specified integer based on parameter query
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand(query, Conn);
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
        [WebMethod]
        public void NewHolder(Holder holder) 
        { //Adds new card holder
            string query = "INSERT INTO CardHolders(email, firstName, lastName, dateOfBirth, phoneNumber, billingAddress) VALUES(@email, @first, @last, @date, @phone, @address)";
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand(query, Conn);
            command.Parameters.AddWithValue("@email", holder.email);
            command.Parameters.AddWithValue("@first", holder.firstName);
            command.Parameters.AddWithValue("@last", holder.lastName);
            command.Parameters.AddWithValue("@date", holder.dateOfBirth);
            command.Parameters.AddWithValue("@phone", holder.phoneNumber);
            command.Parameters.AddWithValue("@address", holder.billingaddress);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        [WebMethod]
        public void NewCard(Card card, bool holderExists, bool remember) 
        { //Specify if holder exists and is adding another card, or this is the first
            if (!holderExists) NewHolder(card.holder);
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand("UPDATE CreditCards SET rememberMe = false WHERE cardHolder = '" + card.holder.email + "'", Conn);
            Conn.Open();
            command.ExecuteNonQuery();
            string query = "INSERT INTO CreditCards(creditCardNumber, cardHolder, cardProvider, CVV, validityDate, rememberMe) VALUES(@num, @holder, @provider, @cvv, @valid, @save)";
            command = new OleDbCommand(query, Conn);
            command.Parameters.AddWithValue("@num", card.number);
            command.Parameters.AddWithValue("@holder", card.holder.email);
            command.Parameters.AddWithValue("@provider", card.provider);
            command.Parameters.AddWithValue("@cvv", card.CVV);
            command.Parameters.AddWithValue("@valid", card.validity);
            command.Parameters.AddWithValue("@save", remember);
            command.ExecuteNonQuery();
            Conn.Close();
        }
        [WebMethod]
        public bool TransactionSuccess(string cardNum, double total) 
        {
            OleDbConnection Conn = new OleDbConnection(GetConnectionString());
            OleDbCommand command = new OleDbCommand("SELECT SUM(limit - " + total + ") FROM CreditCards WHERE creditCardNumber = '" + cardNum + "'", Conn);
            Conn.Open(); //limit always less than balance
            if (Convert.ToDouble(command.ExecuteScalar()) < 0) return false;
            command = new OleDbCommand("INSERT INTO Transactions(creditCard, total, dateOfPurchase, companyName) VALUES(@creditCard, @total, @date, 'Dungeon Maker')", Conn);
            command.Parameters.AddWithValue("@creditCard", cardNum);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            command.ExecuteNonQuery();
            command = new OleDbCommand("UPDATE CreditCards SET balance = balance - " + total " WHERE creditCardNumber = '" + cardNum + "'", Conn);
            command.ExecuteNonQuery();
            Conn.Close();
            return true;
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