using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using DungeonMaker.Classes.Types;
using DungeonMaker.Classes.Services;

namespace DungeonMaker
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserService US = new UserService();
        }
        protected void LogIn_Click(object sender, EventArgs e)
        { //If inputs are valid, completes login
            if (string.IsNullOrEmpty(UserName_TextBox.Text) || string.IsNullOrEmpty(Password_TextBox.Text)) IsLogIn.Text = "A field is empty."; 
            else
            {
                User user = new User();
                try { user = new User(UserName_TextBox.Text, Password_TextBox.Text); }
                catch { IsLogIn.Text = "Username or password are incorrect"; return; }
                if (user.elevation > 0)
                {
                    Session["user"] = user;
                    Session["userPage"] = user;
                    Response.Redirect("Explore.aspx");
                }
                else IsLogIn.Text = "You are banned.";
            }
        }
        protected void Guest_Login_Click(object sender, EventArgs e)
        {
            Session["user"] = new User();
            Response.Redirect("Explore.aspx");
        }
    }
}