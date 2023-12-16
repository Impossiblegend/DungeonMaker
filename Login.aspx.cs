using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

namespace Mid_Project
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserName_TextBox.Text) || string.IsNullOrEmpty(Password_TextBox.Text))
            { IsLogIn.Text = "A field is empty."; }
            else
            {
                if (Login(UserName_TextBox.Text, Password_TextBox.Text))
                { Response.Redirect("index.html"); }
                else
                { IsLogIn.Text = "Username or password are incorrect"; }
            }
        }

        public static bool Login(string username, string password)
        {
            OleDbConnection Conn = new OleDbConnection();
            Conn.ConnectionString = Connect.GetConnectionString();
            OleDbCommand command = new OleDbCommand();
            command.Connection = Conn;
            Conn.Open();
            command.CommandText = "SELECT userPassword FROM Users WHERE Username = @username";
            command.Parameters.AddWithValue("@username", username);
            if ((string)command.ExecuteScalar() == password)
            {
                Conn.Close();
                return true;
            }
            Conn.Close();
            return false;
        }
    }
}