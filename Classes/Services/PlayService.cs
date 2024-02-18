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
        private static OleDbConnection Conn;
        private static OleDbCommand command;
        public PlayService()
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
        public static void UploadGame(string player, int mapID, int time, int stars, int deaths, bool victory) 
        { //Uploads game stats into Games table
            command.CommandText = "INSERT INTO Games(player, mapID, datePlayed, timeElapsed, starsCollected, deathCount, victory) " +
                "VALUES(@player, @mapID, @datePlayed, @timeElapsed, @starsCollected, @deathCount, @victory)";
            command.Parameters.AddWithValue("@player", player);
            command.Parameters.AddWithValue("@mapID", mapID);
            command.Parameters.AddWithValue("@datePlayed", DateTime.Today);
            command.Parameters.AddWithValue("@timeElapsed", time);
            command.Parameters.AddWithValue("@starsCollected", stars);
            command.Parameters.AddWithValue("@deathCount", deaths);
            command.Parameters.AddWithValue("@victory", victory);
            SafeExecute();
        }
        public static int countGames(int mapID) 
        {
            command.CommandText = "SELECT Count(gameID) FROM Games WHERE mapID = " + mapID;
            Conn.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            Conn.Close();
            return count;
        }
        public static List<Game> GetUserGames(User user) 
        {
            command.CommandText = "SELECT gameID FROM Games WHERE player = @email ORDER BY gameID DESC";
            command.Parameters.AddWithValue("@email", user.email);
            List<Game> games = new List<Game>();
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read()) games.Add(new Game(reader.GetInt32(0)));
            Conn.Close();
            return games;
        }
    }
}