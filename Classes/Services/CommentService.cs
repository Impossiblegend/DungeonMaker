using DungeonMaker.classes.Types;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace DungeonMaker.classes.Services
{
    public class CommentService
    {
        private static OleDbConnection Conn;
        private static OleDbCommand command;
        public CommentService()
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
        public static void SendComment(string email, string feedback, int rating)
        { //Uploads feedback and rating into database
            command.CommandText = "INSERT INTO Feedback(sender, feedbackBody, starRating, dateSent) " +
                "VALUES(@email, @feedback, @rating, @date)";
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@feedback", feedback);
            command.Parameters.AddWithValue("@rating", rating);
            SafeExecute();
        }
        public static void ChangeFeatured(Comment comment) 
        {
            command.CommandText = "UPDATE Feedback SET isFeatured = @f WHERE feedbackID = " + comment.feedbackID;
            command.Parameters.AddWithValue("@f", comment.isFeatured);
            SafeExecute();
        }
    }
}