using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using DungeonMaker.Classes.Types;

namespace DungeonMaker
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition { Path = "~/Scripts/jquery-3.7.1.min.js", DebugPath = "~/Scripts/jquery-3.7.1.js", CdnPath = "https://code.jquery.com/jquery-3.7.1.min.js", CdnDebugPath = "https://code.jquery.com/jquery-3.7.1.js"});
                UserService US = new UserService();
            }
        }

        protected void SignUp_Button_Click(object sender, EventArgs e)
        { //If inputs are valid, completes register
            int passLen = Password_TextBox.Text.Length;
            if (string.IsNullOrEmpty(UserName_TextBox.Text) || string.IsNullOrEmpty(Gmail_TextBox.Text) || string.IsNullOrEmpty(Password_TextBox.Text))
                IsSignUp.Text = "Please fill in all fields.";
            else if (passLen < 4 || passLen > 12)
                IsSignUp.Text = "Password length must be 4-12 characters. Yours is " + passLen + ".";
            else if (!UserService.FieldExists("email", Gmail_TextBox.Text) && !UserService.FieldExists("username", UserName_TextBox.Text))
            {
                UserService.InsertUser(UserName_TextBox.Text, Gmail_TextBox.Text, Password_TextBox.Text);
                User user = new User(UserName_TextBox.Text, Password_TextBox.Text);
                Session["user"] = user;
                Session["userPage"] = user;
                Response.Redirect("Explore.aspx");
            }
            else IsSignUp.Text = "User already exists.";
        }
    }
}