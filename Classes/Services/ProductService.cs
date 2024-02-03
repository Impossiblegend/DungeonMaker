using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace DungeonMaker
{
    public class ProductService
    {
        public ProductService() { }
        public static DataSet GetProductsByQuery(string query, string table) 
        { //Returns a dataset based on a given query. The table parameter is a dummy.
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = query;
            Conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            DataTable dt1 = new DataTable(table);
            adapter.Fill(dt1); //Failing here means an incorrect SQL query
            DataSet ds = new DataSet();
            ds.Tables.Add(dt1);
            Conn.Close();
            return ds;
        }
    }
}