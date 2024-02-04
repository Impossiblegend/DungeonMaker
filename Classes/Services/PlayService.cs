using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using DungeonMaker.classes.Types;
using DungeonMaker.Classes.Types;

namespace DungeonMaker.classes.Services
{
    public class PlayService
    {
        public PlayService() { }
        public static void UploadGame(string player, int mapID, int time, int stars, int deaths, bool victory) 
        { //Uploads game stats into Games table
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "INSERT INTO Games(player, mapID, datePlayed, timeElapsed, starsCollected, deathCount, victory) " +
                "VALUES(@player, @mapID, @datePlayed, @timeElapsed, @starsCollected, @deathCount, @victory)";
            command.Parameters.AddWithValue("@player", player);
            command.Parameters.AddWithValue("@mapID", mapID);
            command.Parameters.AddWithValue("@datePlayed", DateTime.Today);
            command.Parameters.AddWithValue("@timeElapsed", time);
            command.Parameters.AddWithValue("@starsCollected", stars);
            command.Parameters.AddWithValue("@deathCount", deaths);
            command.Parameters.AddWithValue("@victory", victory);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static bool wasMapPlayed(int mapID) { return countGames(mapID) > 0; }
        public static int countGames(int mapID) 
        {
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "SELECT Count(gameID) FROM Games WHERE mapID = " + mapID;
            Conn.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            Conn.Close();
            return count;
        }
    }
}