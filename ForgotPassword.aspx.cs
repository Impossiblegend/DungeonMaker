using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Threading;
using DungeonMaker.Classes.Types;

namespace DungeonMaker
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private string token;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserService US = new UserService();
        }
        static string GenerateToken(int minutesValid)
        {
            // Generate a random token
            string token = Guid.NewGuid().ToString();
            DateTime expirationTime = DateTime.Now.AddMinutes(minutesValid);
            // Append the expiration time to the token (for verification)
            token += $"|{expirationTime:yyyy-MM-ddTHH:mm:ss}";
            return token;
        }
        protected void CheckAccount_Button_Click(object sender, EventArgs e)
        { //If valid, sends client an email with temp. code
            if (string.IsNullOrEmpty(UserName_TextBox.Text) || string.IsNullOrEmpty(Email_TextBox.Text))
                IsPasswordChanged.Text = "Please fill in both username and email.";
            else
            {
                User user = new User(Email_TextBox.Text);
                if (user.username == UserName_TextBox.Text)
                {
                    Title_Label.Visible = false; UserName_Label.Visible = false; UserName_TextBox.Visible = false; CheckAccount_Button.Visible = false;
                    ChangePassword_Label.Visible = true; ChangePassword_TextBox.Visible = true; ChangePassword_Button.Visible = true;
                    Session["user"] = user;
                    Email_Label.Text = "Enter email code"; Email_TextBox.Text = "";
                    IsPasswordChanged.Text = "Email sent! Check your inbox.";
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("Dungeon Maker", "dungeonmakergame@gmail.com"));
                    email.To.Add(new MailboxAddress("player", Email_TextBox.Text));
                    email.Subject = "Password Reset";
                    string token = GenerateToken(5);
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = "This email was sent because you requested a password change." +
                        "\nEnter the following code: " + token
                    };
                    using (var smtp = new SmtpClient())
                    {
                        smtp.Connect("smtp.gmail.com", 587, false);
                        //Only needed if the SMTP server requires authentication
                        try 
                        { 
                            smtp.Authenticate("dungeonmakergame@gmail.com", "Wnew4ug7");
                            smtp.Send(email);
                        }
                        catch 
                        {
                            IsPasswordChanged.Text = "Email failed, please try again later or contact us at dungeonmakergame@gmail.com";
                            ChangePassword_Button.Enabled = false;
                            return; //throw new Exception("Email failure");
                        }
                        finally { smtp.Disconnect(true); }
                    }
                    this.token = token;
                }
                else IsPasswordChanged.Text = "Invalid username or email.";
            }
        }
        protected void ChangePassword_Button_Click(object sender, EventArgs e)
        { //If valid, changes password
            if (string.IsNullOrEmpty(ChangePassword_TextBox.Text)) IsPasswordChanged.Text = "Please enter a new password.";
            else
            {
                if(Email_TextBox.Text != token) IsPasswordChanged.Text = "Incorrect code - token invalid.";
                else
                {
                    UserService.ChangePassword(ChangePassword_TextBox.Text, ((User)Session["user"]).username);
                    Response.Redirect("Login.aspx");
                }

            }
        }
    }
}