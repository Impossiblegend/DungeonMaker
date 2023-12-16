using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace Mid_Project
{
    public partial class Create_Account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SignUp_Button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserName_TextBox.Text) || string.IsNullOrEmpty(Gmail_TextBox.Text) || string.IsNullOrEmpty(Password_TextBox.Text))
            {
                IsSignUp.Text = "Please fill in all fields.";
            }
            else
            {
                InsertUser(UserName_TextBox.Text, Gmail_TextBox.Text, Password_TextBox.Text);
                Response.Redirect("Login.aspx");
            }
        }

        public void InsertUser(string userName, string email, string password)
        {
            OleDbCommand command = new OleDbCommand();
            string connectionString = Connect.GetConnectionString();
            OleDbConnection Conn = new OleDbConnection(connectionString);
            command.Connection = Conn;
            command.CommandText = "INSERT INTO Users(email,userPassword,creationDate,elevation,username) VALUES(@email,@password,@date,1,@username)";
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@date", DateTime.Today);
            command.Parameters.AddWithValue("@username", userName);
            Conn.Open();
            command.ExecuteNonQuery();
            Conn.Close();
        }
    }
}