using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace DungeonMaker
{
    public class GeneralService
    {
        public GeneralService() { }
        public static DataSet GetDataSetByQuery(string query, string table) 
        { //Returns a dataset based on a given query. Table parameter is a dummy.
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = query;
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
        public static string GetStringByQuery(string query)
        { //Returns a string based on a given query
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = query;
            Conn.Open();
            try { return command.ExecuteScalar().ToString(); }
            catch (Exception ex) { return ex.Message; }
            finally { Conn.Close(); }
        }
        public static int LastMonthCompute(string table, string PK, string dateField) 
        { //Returns an integer based on a datefield in the past 30 days
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = $"SELECT Count({PK}) FROM {table} WHERE {dateField} >= DateAdd('d', -30, Date())";
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            catch { return int.MinValue; }
            finally { Conn.Close(); }
        }
    }
}