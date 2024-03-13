using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Web;
using DungeonMaker.Classes.Types;

namespace DungeonMaker.Classes.Services
{
    public class StoreService
    {
        private static OleDbConnection Conn;
        private static OleDbCommand command;
        public StoreService()
        {
            Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            command = new OleDbCommand();
            command.Connection = Conn;
        }
        private static void SafeExecute()
        {
            try
            {
                Conn.Open();
                command.ExecuteNonQuery();
            }
            catch (OleDbException ex)
            {
                //Add extra stuff here
                throw new Exception("Error updating database: " + ex.Message);
            }
            finally { Conn.Close(); }
        }
        private static int SafeScalar() 
        {
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
        public static void Purchase(User user, string type) 
        {
            command.CommandText = "INSERT INTO Purchases(owner, type, dateOfPurchase) VALUES(@buyer, @type, @date)";
            command.Parameters.AddWithValue("@buyer", user.email);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            SafeExecute();
        }
        public static bool IsPurchased(User user, string type) 
        {
            command.CommandText = "SELECT Count(owner) FROM Purchases WHERE owner = @owner AND type = @type";
            command.Parameters.AddWithValue("@owner", user.email);
            command.Parameters.AddWithValue("@type", type);
            return SafeScalar() > 0;
        }
        public static int SumUserPurchases(User user)
        {
            command.CommandText = "SELECT SUM(cost) FROM Products INNER JOIN Purchases ON Products.type = Purchases.type WHERE owner = ?";
           command.Parameters.AddWithValue("@owner", user.email);
            try { return SafeScalar(); }
            catch { return 0; }
        }
        public static int SumCreditPurchases(User user) 
        {
            command.CommandText = "SELECT SUM(CreditBundles.creditAmount) FROM CreditBundles INNER JOIN CreditPurchases ON CreditPurchases.bundle = CreditBundles.bundleName WHERE CreditPurchases.customer = ?";
            command.Parameters.AddWithValue("@customer", user.email);
            try { return SafeScalar(); }
            catch { return 0; }
        }
        public static int GetTypeCost(string type) 
        {
            command.CommandText = "SELECT cost FROM Products WHERE type = ?";
            command.Parameters.AddWithValue("@type", type);
            return SafeScalar();
        }
    }
}