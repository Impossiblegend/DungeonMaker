using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace DungeonMaker.Classes.Services
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
        public static void SendComment(User user, string feedback, int rating)
        { //Uploads feedback and rating into database
            command.CommandText = "INSERT INTO Feedback(sender, feedbackBody, starRating, dateSent, isFeatured) VALUES(@sender, @feedback, @rating, @dateSent, false)";
            command.Parameters.AddWithValue("@sender", user.email);
            command.Parameters.AddWithValue("@feedback", feedback);
            command.Parameters.AddWithValue("@rating", rating);
            command.Parameters.AddWithValue("@dateSent", DateTime.Today);
            SafeExecute();
        }
        public static void ChangeFeatured(Comment comment) 
        { //Negates the featured state of a comment
            command.CommandText = "UPDATE Feedback SET isFeatured = @f WHERE feedbackID = " + comment.feedbackID;
            command.Parameters.AddWithValue("@f", comment.isFeatured);
            SafeExecute();
        }
        public static bool CanUpload(User user)
        { //Checks if a user has already sent feedback today
            command.CommandText = "SELECT Count(feedbackID) FROM Feedback WHERE sender = @sender AND MONTH(dateSent) = Month(DATE())";
            command.Parameters.AddWithValue("@sender", user.email);
            Conn.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            Conn.Close();
            return count == 0;
        }
    }
}