using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace DungeonMaker.Classes.Services
{
    public class AchievementService
    {
        private static OleDbConnection Conn;
        private static OleDbCommand command;
        public AchievementService()
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
        public static void Achieve(string title, User user) 
        {
            command.CommandText = "INSERT INTO UserAchievements(achievement, awardee, dateReceived) VALUES(@title, @user, @date)";
            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@user", user.email);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            SafeExecute();
        }
        public static int UserCreditsTotal(User user) 
        {
            command.CommandText = "SELECT SUM(A.creditsWorth) AS TotalCredits FROM Achievements A INNER JOIN UserAchievements UA " +
                "ON A.achievementTitle = UA.achievement WHERE UA.awardee = '" + user.email + "'";
            Conn.Open();
            try { return Convert.ToInt32(command.ExecuteScalar()); }
            catch { return 0; }
            finally { Conn.Close(); }
        }
        public static void ChangeValid(string title)
        { //Negates the validity of an achievement
            command.CommandText = "UPDATE Achievements SET isValid = NOT isValid WHERE achievementTitle = @title";
            command.Parameters.AddWithValue("@title", title);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static bool IsValid(string title) 
        { //Checks if an achievement is valid
            command.CommandText = "SELECT isValid FROM Achievements WHERE achievementTitle = @Title";
            command.Parameters.AddWithValue("@Title", title);
            Conn.Open();
            try { return (bool)command.ExecuteScalar(); }
            catch { throw new Exception("Achievement not found: " + title); }
            finally { Conn.Close(); }
        }
    }
}