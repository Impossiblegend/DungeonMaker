using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using DungeonMaker.Classes.Types;

namespace DungeonMaker.Classes.Services
{
    public class PlayService
    {
        private static OleDbConnection Conn;
        private static OleDbCommand command;
        public PlayService()
        {
            Conn = new OleDbConnection();
            Conn.ConnectionString = Utility.GetConnectionString();
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
        public static void UploadGame(Game game) 
        { //Uploads game stats into Games table
            command.CommandText = "INSERT INTO Games(player, mapID, datePlayed, timeElapsed, starsCollected, deathCount, victory) " +
                "VALUES(@player, @mapID, @datePlayed, @timeElapsed, @starsCollected, @deathCount, @victory)";
            if (game.player.email == null) game.player.email = "Guest";
            command.Parameters.AddWithValue("@player", game.player.email);
            command.Parameters.AddWithValue("@mapID", game.map.mapID);
            command.Parameters.AddWithValue("@datePlayed", DateTime.Today);
            command.Parameters.AddWithValue("@timeElapsed", game.time);
            command.Parameters.AddWithValue("@starsCollected", game.stars);
            command.Parameters.AddWithValue("@deathCount", game.deaths);
            command.Parameters.AddWithValue("@victory", game.victory);
            SafeExecute();
        }
        public static int countGames() 
        { //Count all games
            command.CommandText = "SELECT Max(gameID) FROM Games";
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
        public static int countGames(int mapID) 
        { //Count games of a specific map
            command.CommandText = "SELECT Count(gameID) FROM Games WHERE mapID = " + mapID;
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
        public static Game GetLastGame(User user) 
        { //Get user's last played game
            command.CommandText = "SELECT Max(gameID) FROM Games WHERE player = '" + user.email + "'";
            Conn.Open();
            try { return new Game(Convert.ToInt32(command.ExecuteScalar())); }
            catch { return null; }
            finally { Conn.Close(); }
        }
        public static List<Game> GetUserGames(User user) 
        { //Get all user's games
            command.CommandText = "SELECT gameID FROM Games WHERE player = @email ORDER BY gameID DESC";
            command.Parameters.AddWithValue("@email", user.email);
            List<Game> games = new List<Game>();
            Conn.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read()) games.Add(new Game(reader.GetInt32(0)));
            Conn.Close();
            return games;
        }
        public static int CountStars(User user) 
        {
            int stars = 0;
            foreach (Game game in GetUserGames(user)) stars += game.stars;
            return stars;
        }
        public static int CountUserVictories(User user)
        { //Count a user's victories
            command.CommandText = "SELECT Count(gameID) FROM Games WHERE player = @player AND victory";
            command.Parameters.AddWithValue("@player", user.email);
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            finally { Conn.Close(); }
        }
    }
}