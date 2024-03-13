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
using System.Web.UI.HtmlControls;

namespace DungeonMaker
{
    public partial class Login : System.Web.UI.Page
    {
        private User user;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
                {
                    Path = "~/Scripts/jquery-3.7.1.min.js",
                    DebugPath = "~/Scripts/jquery-3.7.1.js",
                    CdnPath = "https://code.jquery.com/jquery-3.7.1.min.js",
                    CdnDebugPath = "https://code.jquery.com/jquery-3.7.1.js"
                });
                FPMultiView.ActiveViewIndex = 0;
                PagesMultiView.ActiveViewIndex = 0;
            }
            UserService US = new UserService(); PlayService PS = new PlayService();
            if (Session["user"] != null && user == null) user = (User)Session["user"];
        }

        protected void LogIn_Click(object sender, EventArgs e)
        { //If inputs are valid, completes login
            if (string.IsNullOrEmpty(UserName_TextBox.Text) || string.IsNullOrEmpty(Password_TextBox.Text)) IsLogIn.Text = "A field is empty."; 
            else
            {
                User user = new User();
                try { user = new User(UserName_TextBox.Text, Password_TextBox.Text); }
                catch { IsLogIn.Text = "Username or password are incorrect"; return; }
                if (!user.IsBanned())
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

        protected void Nav_Click(object sender, EventArgs e)  { PagesMultiView.ActiveViewIndex = int.Parse(((LinkButton)sender).CommandArgument); }

        protected void SignUp_Button_Click(object sender, EventArgs e)
        { //If inputs are valid, completes register
            int passLen = PWTB.Text.Length;
            if (string.IsNullOrEmpty(UserNameTB.Text) || string.IsNullOrEmpty(Gmail_TextBox.Text) || string.IsNullOrEmpty(PWTB.Text)) IsSignUp.Text = "Please fill in all fields.";
            else if (passLen < 4 || passLen > 12) IsSignUp.Text = "Password length must be 4-12 characters. Yours is " + passLen + ".";
            else if (!UserService.FieldExists("email", Gmail_TextBox.Text) && !UserService.FieldExists("username", UserNameTB.Text))
            {
                UserService.InsertUser(UserNameTB.Text, Gmail_TextBox.Text, PWTB.Text);
                User user = new User(UserNameTB.Text, PWTB.Text);
                Session["user"] = user;
                Session["userPage"] = user;
                string[] freeProducts = new string[] { "saw", "portalFull", "portalEmpty", "blank" };
                StoreService SS = new StoreService();
                foreach (string product in freeProducts) { StoreService.Purchase(user, product); SS = new StoreService(); }
                Response.Redirect("Explore.aspx");
            }
            else IsSignUp.Text = "User already exists.";
        }

        protected void CheckAccount_Button_Click(object sender, EventArgs e)
        { //Validates account credentials
            if (string.IsNullOrEmpty(UserNameTextBox.Text) || string.IsNullOrEmpty(Email_TextBox.Text)) IsPasswordChanged.Text = "Please fill in both your username and email.";
            else
            {
                try { user = new User(Email_TextBox.Text); }
                catch
                {
                    IsPasswordChanged.Text = "Email not registered.";
                    return;
                }
                if (user.username != UserNameTextBox.Text) IsPasswordChanged.Text = "The provided username and email do not match.";
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
            else if (pass != ConfirmPassword_TextBox.Text) IsPasswordChanged.Text = "Password confirmation failed, no match.";
            else
            {
                UserService.ChangePassword(pass, user);
                PagesMultiView.ActiveViewIndex = 0;
            }
        }
    }
}