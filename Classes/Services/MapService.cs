using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using DungeonMaker.Classes.Types;
using System.IO;

namespace DungeonMaker
{
    public class MapService
    {
        public static int MapID { get; set; }
        
        public MapService() { }
        public static void InsertStars(int xpos, int ypos) 
        { //Uploads all stars on screen to the database
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand("SELECT max(mapID) FROM Maps", Conn);
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            if(reader.Read()) MapService.MapID = reader.GetInt32(0);
            command = new OleDbCommand("INSERT INTO Stars(mapID,xpos,ypos) VALUES(@mapID,@xpos,@ypos)", Conn);
            command.Parameters.AddWithValue("@mapID", MapService.MapID);
            command.Parameters.AddWithValue("@xpos", xpos);
            command.Parameters.AddWithValue("@ypos", ypos);
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static void InsertTraps(int xpos, int ypos, string type)
        { //Uploads all traps on screen to the database
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand("SELECT max(mapID) FROM Maps", Conn);
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read()) MapService.MapID = reader.GetInt32(0);
            command = new OleDbCommand("INSERT INTO Traps(type,mapID,xpos,ypos) VALUES(@type,@mapID,@xpos,@ypos)", Conn);
            command.Parameters.AddWithValue("@type", type);
            command.Parameters.AddWithValue("@mapID", MapService.MapID);
            command.Parameters.AddWithValue("@xpos", xpos);
            command.Parameters.AddWithValue("@ypos", ypos);
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static void DeleteAllButNewestByTrapType(string type)
        { //Deletes all but the nrewest trap, by type and map. Useful for edge cases.
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "DELETE FROM Traps WHERE mapID = @mapID AND type = @type " +
                "AND trapNum NOT IN (SELECT TOP 1 trapNum FROM Traps WHERE mapID = @mapID AND type = @type " +
                "ORDER BY trapNum DESC)";
            command.Parameters.AddWithValue("@mapID", MapService.MapID);
            command.Parameters.AddWithValue("@type", type);
            GeneralService.ExecuteNonQuery(command, Conn);
        }
        public static void UploadMap(string email, string mapName, string mapType) 
        { //Creates a new map in the database and saves its ID statically
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand();            
            command.CommandText = "INSERT INTO Maps(creator, mapType, creationDate, mapName, isPublic, estTime, thumbnail, isValid) " +
                "VALUES(@creator, @mapType, @date, @mapName, True, 90, @thumbnail, True)";
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
        public static void DeleteByMapID(string table)
        { //Deletes all records related to the map by table
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand($"DELETE FROM {table} WHERE mapID = @mapID", Conn);
            command.Parameters.AddWithValue("@mapID", MapService.MapID);
            GeneralService.ExecuteNonQuery(command, Conn);
        }
        public static int CountMapsWithName(string mapName) 
        { //Returns number of maps with a given name
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "SELECT COUNT(mapID) FROM Maps WHERE mapName = '" + mapName + "'";
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
        public static int SumMapCostsByUser(User user)
        { //Returns number of maps with a given name
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "SELECT SUM((cost / 100) + 15) FROM Maps INNER JOIN Products ON Maps.mapType = Products.type WHERE creator = '" + user.email + "'";
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            catch { return 0; }
            finally { Conn.Close(); }
        }
        public static void ChangeValid(int mapID) 
        { //Negates the validity of a map
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "UPDATE Maps SET isValid = NOT isValid WHERE mapID = " + mapID;
            GeneralService.ExecuteNonQuery(command, Conn);
        }
        public static void ChangeMapName(int mapID, string mapName) //mapName must include serial autonumber --- CountMapsWithName()
        { //Changes map name and thumbnail accordingly
            ChangeThumbnail(mapID, "assets/screenshots/" + mapName + ".jpg");
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand("UPDATE Maps SET mapName = @mapName WHERE mapID = " + mapID, Conn);
            command.Parameters.AddWithValue("@mapName", mapName);
            GeneralService.ExecuteNonQuery(command, Conn);
        }
        public static void ChangeThumbnail(int mapID, string path) 
        {
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand("UPDATE Maps SET thumbnail = @thumbnail WHERE mapID = " + mapID, Conn);
            command.Parameters.AddWithValue("@thumbnail", path);
            GeneralService.ExecuteNonQuery(command, Conn);
        }

        public static void ChangeTime(int mapID, int sec) 
        {
            OleDbConnection Conn = new OleDbConnection(Utility.GetConnectionString());
            OleDbCommand command = new OleDbCommand("UPDATE Maps SET estTime = @sec WHERE mapID = " + mapID, Conn);
            command.Parameters.AddWithValue("@sec", sec);
            GeneralService.ExecuteNonQuery(command, Conn);
        }
    }
}