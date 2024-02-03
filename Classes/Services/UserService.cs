using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using DungeonMaker.Classes.Types;
using System.Data;

namespace DungeonMaker
{
    public class UserService
    {
        private static OleDbConnection Conn;
        private static OleDbCommand command;
        public UserService()
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
        public static bool FieldExists(string field, string value) 
        { // Returns true if a value of a field exists, otherwise false
            command.CommandText = "SELECT Count(" + field + ") FROM Users WHERE " + field + " = '" + value + "'";
            Conn.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            Conn.Close();
            return count > 0;
        }
        public static void InsertUser(string username, string email, string password)
        { //Register new user 
            command.CommandText = "INSERT INTO Users(email,username,userPassword,creationDate,elevation, profilePicture) " +
                "VALUES(@email,@username,@password,@date,1,@profile)";
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            command.Parameters.AddWithValue("@profile", "assets/profiles/default.png");
            SafeExecute();
        }
        public static void ChangePassword(string Password, string UserName)
        { //Changes a user's password
            command.CommandText = "UPDATE Users SET userPassword = '" + Password + "' WHERE username = '" + UserName + "'";
            SafeExecute();
        }
        public static void ChangePrivacy(int mapID)
        { //Switch the current privacy setting public <--> private
            command.CommandText = "UPDATE Maps SET isPublic = NOT isPublic WHERE mapID = " + mapID;
            SafeExecute();
        }
        public static void ChangeBlockState(string email)
        { //Switch the blocked state of a user un/block
            command.CommandText = "UPDATE Users SET elevation = -1 * elevation WHERE email = '" + email + "'";
            SafeExecute();
        }
        public static void ChangeProfilePic(string email, string image)
        {
            command.CommandText = "UPDATE Users SET profilePicture = '" + image + "' WHERE email = '" + email + "'";
            SafeExecute();
        }
        public static bool SignIn(string username, string password)
        { //Returns true if username and password match, otherwise returns false
            command.CommandText = "SELECT userPassword FROM Users WHERE Username = @username";
            command.Parameters.AddWithValue("@username", username);
            Conn.Open();
            if ((string)command.ExecuteScalar() == password)
            {
                Conn.Close();
                return true;
            }
            Conn.Close();
            return false;
        }
        public static void UpdateFieldByEmail(string field, string value,  string email) 
        { //Changes a Users field by email (PK) to a parameter value
            command.CommandText = "UPDATE Users SET " + field + " = '" + value + "' WHERE email = '" + email + "'";
            SafeExecute();
        }
    }
}