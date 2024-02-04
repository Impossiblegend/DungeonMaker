using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using DungeonMaker.classes.Types;

namespace DungeonMaker
{
    public class MapService
    {
        private static int MapID { get; set; }
        
        public MapService() { }
        public static void InsertStars(int xpos, int ypos) 
        { //Uploads all stars on screen to the database
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "SELECT max(mapID) FROM Maps";
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            if(reader.Read()) 
                MapService.MapID = reader.GetInt32(0);
            OleDbCommand command2 = new OleDbCommand();
            command2.Connection = Conn;
            command2.CommandText = "INSERT INTO Stars(mapID,xpos,ypos) VALUES(@mapID,@xpos,@ypos)";
            command2.Parameters.AddWithValue("@mapID", MapService.MapID);
            command2.Parameters.AddWithValue("@xpos", xpos);
            command2.Parameters.AddWithValue("@ypos", ypos);
            command2.ExecuteNonQuery();
            Conn.Close();
        }
        public static void InsertTraps(int xpos, int ypos, string type)
        { //Uploads all traps on screen to the database
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.CommandText = "SELECT max(mapID) FROM Maps";
            command.Connection = Conn;
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
                MapService.MapID = reader.GetInt32(0);
            OleDbCommand command2 = new OleDbCommand();
            command2.Connection = Conn;
            command2.CommandText = "INSERT INTO Traps(type,mapID,xpos,ypos) VALUES(@type,@mapID,@xpos,@ypos)";
            command2.Parameters.AddWithValue("@type", type);
            command2.Parameters.AddWithValue("@mapID", MapService.MapID);
            command2.Parameters.AddWithValue("@xpos", xpos);
            command2.Parameters.AddWithValue("@ypos", ypos);
            command2.ExecuteNonQuery();
            Conn.Close();
        }
        public static void DeleteAllButNewestByTrapType(string type)
        {
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "DELETE FROM Traps WHERE mapID = @mapID AND type = @type " +
                "AND trapNum NOT IN (SELECT TOP 1 trapNum FROM Traps WHERE mapID = @mapID AND type = @type " +
                "ORDER BY trapNum DESC)";
            command.Parameters.AddWithValue("@mapID", MapService.MapID);
            command.Parameters.AddWithValue("@type", type);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static void UploadMap(string email, string mapName, string mapType) 
        { //Creates a new map in the database and saves its ID statically
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();            
            command.CommandText = "INSERT INTO Maps(creator, mapType, creationDate, mapName, isPublic, estTime, thumbnail, isValid) " +
                "VALUES(@creator, @mapType, @date, @mapName, False, 0, @thumbnail, True)";
            command.Parameters.AddWithValue("@creator", email);
            command.Parameters.AddWithValue("@mapType", mapType);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            command.Parameters.AddWithValue("@mapName", mapName);
            command.Parameters.AddWithValue("@thumbnail", "assets/screenshots/" + mapName + ".jpg");
            command.Connection = Conn;
            Conn.Open();
            command.ExecuteNonQuery();
            command.CommandText = "SELECT TOP 1 MapID FROM Maps ORDER BY Maps.creationDate";
            MapService.MapID = int.Parse(command.ExecuteScalar().ToString());
            Conn.Close();
        }
        public static void DeleteMap(int mapID) 
        {
            MapService.MapID = mapID;
            DeleteByMapID("Maps");
            DeleteByMapID("Traps");
            DeleteByMapID("Stars");
        }
        private static void DeleteByMapID(string table)
        {
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = $"DELETE FROM {table} WHERE mapID = @mapID";
            command.Parameters.AddWithValue("@mapID", MapService.MapID);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static int CountMapsWithName(string mapName) 
        {
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "SELECT Count(mapID) FROM Maps WHERE mapName = '" + mapName + "'";
            Conn.Open();
            int count =  (int)command.ExecuteScalar();
            Conn.Close();
            return count;
        }
        public static void ChangeValid(int mapID) 
        {
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "UPDATE Maps SET isValid = NOT isValid WHERE mapID = " + mapID;
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static void ChangeMapName(int mapID, string mapName) 
        {
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand(), command2 = new OleDbCommand();
            command.Connection = Conn; command2.Connection = Conn;
            command.CommandText = "UPDATE Maps SET mapName = @mapName WHERE mapID = " + mapID;
            command2.CommandText = "UPDATE Maps SET thumbnail = @thumbnail WHERE mapID = " + mapID;
            command.Parameters.AddWithValue("@mapName", mapName + MapService.CountMapsWithName(mapName));
            command2.Parameters.AddWithValue("@thumbnail", "assets/screenshots/" + mapName + MapService.CountMapsWithName(mapName));
            Conn.Open();
            command.ExecuteNonQuery();
            command2.ExecuteNonQuery();
            Conn.Close();
        }
        public static List<GameObject> GetStarsByMapID(int mapID)
        { //Returns all records in Stars table with foreign key mapID
            List<GameObject> stars = new List<GameObject>();
            string query = "SELECT xpos, ypos FROM Stars WHERE Stars.mapID = " + mapID;
            DataTable StarsTbl = ProductService.GetProductsByQuery(query, "Stars").Tables[0];
            foreach (DataRow dr in StarsTbl.Rows) stars.Add(new GameObject(Convert.ToInt32(dr["xpos"]), Convert.ToInt32(dr["ypos"])));
            return stars;
        }
        public static List<Trap> GetTrapsByMapID(int mapID)
        { //Returns all records in Traps table with foreign key mapID
            List<Trap> traps = new List<Trap>();
            string query = "SELECT xpos, ypos, type FROM Traps WHERE Traps.mapID = " + mapID;
            DataTable TrapsTbl = ProductService.GetProductsByQuery(query, "Traps").Tables[0];
            foreach (DataRow dr in TrapsTbl.Rows) traps.Add(new Trap(Convert.ToInt32(dr["xpos"]), Convert.ToInt32(dr["ypos"]), dr["type"].ToString()));
            return traps;
        }
    }
}