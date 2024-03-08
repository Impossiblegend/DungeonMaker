using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading;
using DungeonMaker.Classes.Types;

namespace DungeonMaker
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private User user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) FPMultiView.ActiveViewIndex = 0;
            UserService US = new UserService();
            if (Session["user"] != null && user == null) user = (User)Session["user"];
        }
        protected void CheckAccount_Button_Click(object sender, EventArgs e)
        { //Validates account credentials
            if (string.IsNullOrEmpty(UserName_TextBox.Text) || string.IsNullOrEmpty(Email_TextBox.Text)) IsPasswordChanged.Text = "Please fill in both your username and email.";
            else 
            {
                try { user = new User(Email_TextBox.Text); }
                catch
                {
                    IsPasswordChanged.Text = "Email not registered.";
                    return;
                }
                if (user.username != UserName_TextBox.Text) IsPasswordChanged.Text = "The provided username and email do not match.";
                else
                {
                    FPMultiView.ActiveViewIndex = 1;
                    IsPasswordChanged.Text = "";
                    Session["user"] = user;
                }
            }
        }
        protected void ChangePassword_Button_Click(object sender, EventArgs e)
        { //If valid, changes password
            string pass = ChangePassword_TextBox.Text;
            if (string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(ConfirmPassword_TextBox.Text)) IsPasswordChanged.Text = "A field is blank.";
            else if (pass.Length < 4 || pass.Length > 12) IsPasswordChanged.Text = "Password must be between 4 and 12 characters long. Yours is " + pass.Length + ".";
            else if (ChangePassword_TextBox.Text != ConfirmPassword_TextBox.Text) IsPasswordChanged.Text = "Password confirmation failed, no match.";
            else
            {
                UserService.ChangePassword(ChangePassword_TextBox.Text, user);
                Response.Redirect("Login.aspx");
            }
        }
    }
}