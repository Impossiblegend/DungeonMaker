using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DungeonMaker.Classes.Types
{
    public class User
    {
        public string email { get; set; }
        public string username {get; set; }
        public string userPassword {get; set; }
        public DateTime creationDate { get; set; }
        public int elevation { get; set; }
        public string profilePicture { get; set; }
        public User(string email, string username, string userPassword, DateTime creationDate, int elevation, string profilePicture)
        {
            this.email = email;
            this.username = username;
            this.userPassword = userPassword;
            this.creationDate = creationDate;
            this.elevation = elevation;
            this.profilePicture = profilePicture;
        }
        public User(string username, string userPassword) 
        {
            this.username = username;
            this.userPassword = userPassword;
            DataSet ds = ProductService.GetDataSetByQuery("SELECT * FROM Users WHERE username = '" + username + "' AND userPassword = '" + userPassword + "'", "Users");
            DataRow user = ds.Tables[0].Rows[0];
            this.email = user["email"].ToString();
            this.creationDate = (DateTime)user["creationDate"];
            this.elevation = int.Parse(user["elevation"].ToString());
            this.profilePicture = user["profilePicture"].ToString();
        }
        public User(string email)
        {
            this.email = email;
            DataSet ds = ProductService.GetDataSetByQuery("SELECT * FROM Users WHERE email = '" + email + "'", "Users");
            DataRow user = ds.Tables[0].Rows[0];
            this.username = user["username"].ToString();
            this.userPassword = user["userPassword"].ToString();
            this.creationDate = (DateTime)user["creationDate"];
            this.elevation = Convert.ToInt32(user["elevation"]);
            this.profilePicture = user["profilePicture"].ToString();
        }
        public User()
        {
            this.elevation = 0;
            this.username = "guest";
            this.email = null;
            this.userPassword = null;
            this.creationDate = DateTime.Today;
            this.profilePicture = "assets/profiles/default.png";
        }
        public string GetRedactedPassword() { return new string('*', this.userPassword.Length); } // e.g. "redact" ---> "*****"
    }
}