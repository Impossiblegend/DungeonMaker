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
        public static void PurchaseTrapType(User user, string type) 
        {
            command.CommandText = "INSERT INTO OwnedTrapTypes(owner, trapType, dateOfPurchase) VALUES(@buyer, @type, @date)";
            command.Parameters.AddWithValue("@buyer", user.email);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            SafeExecute();
        }
        public static void PurchaseMapType(User user, string type)
        {
            command.CommandText = "INSERT INTO OwnedMapTypes(owner, mapType, dateOfPurchase) VALUES(@buyer, @type, @date)";
            command.Parameters.AddWithValue("@buyer", user.email);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            SafeExecute();
        }
        public static bool IsTrapPurchased(User user, string type) 
        {
            command.CommandText = "SELECT Count(owner) FROM OwnedTrapTypes WHERE owner = @owner AND trapType = @type";
            command.Parameters.AddWithValue("@owner", user.email);
            command.Parameters.AddWithValue("@type", type);
            return SafeScalar() > 0;
        }
        public static bool IsMapPurchased(User user, string type)
        {
            command.CommandText = "SELECT Count(owner) FROM OwnedMapTypes WHERE owner = @owner AND mapType = @type";
            command.Parameters.AddWithValue("@owner", user.email);
            command.Parameters.AddWithValue("@type", type);
            return SafeScalar() > 0;
        }
        public static int SumUserPurchases(User user)
        {
            command.CommandText = "SELECT SUM(TT.cost) + SUM(MT.cost) AS TotalCost FROM ((OwnedTrapTypes AS OTT INNER JOIN TrapTypes AS TT ON OTT.trapType = TT.trapType) " +
                "LEFT JOIN OwnedMapTypes AS OMT ON OTT.owner = OMT.owner) LEFT JOIN MapTypes AS MT ON OMT.mapType = MT.mapType WHERE OTT.owner = ?";
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
        public static int GetMapTypeCost(string type) 
        {
            command.CommandText = "SELECT cost FROM MapTypes WHERE mapType = ?";
            command.Parameters.AddWithValue("@mapType", type);
            return SafeScalar();
        }
    }
}