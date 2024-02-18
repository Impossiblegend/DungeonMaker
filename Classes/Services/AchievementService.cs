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
    }
}