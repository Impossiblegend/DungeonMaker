using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace DungeonMaker.classes.Services
{
    public class CommentService
    {
        public CommentService() { }
        public static void SendComment(string email, string feedback, int rating)
        { //Uploads feedback and rating into database
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.CommandText = "INSERT INTO Feedback(sender, feedbackBody, starRating, dateSent) " +
                "VALUES(@email, @feedback, @rating, @date)";
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@feedback", feedback);
            command.Parameters.AddWithValue("@rating", rating);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            command.Connection = Conn;
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
        public static void ChangeFeatured(int feedbackID) 
        {
            OleDbConnection Conn = new OleDbConnection(Connect.GetConnectionString());
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            command.CommandText = "UPDATE Feedback SET isFeatured = NOT isFeatured WHERE feedbackID = " + feedbackID;
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
    }
}