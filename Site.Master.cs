using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DungeonMaker
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public string ImageUrl
        {
            get { return ProfilePic.ImageUrl; }
            set { ProfilePic.ImageUrl = value; }
        }
        private User user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null) Session["user"] = new User();
            user = (User)Session["user"];
            if (!IsPostBack)
            {
                if (user.elevation == 0)
                {
                    Create.Visible = false;
                    Achievements.Visible = false;
                    Gamelog.Visible = false;
                    Logout.Text = "Login";
                }
                ProfilePic.ImageUrl = user.profilePicture;
                if (user.IsAdmin()) Admin.Visible = true;
            }
        }
        protected void About_Click(object sender, EventArgs e) { Response.Redirect("About.aspx"); }
        protected void ProfilePic_Click(object sender, EventArgs e)
        {
            if (user.elevation == 0) Response.Redirect("Register.aspx");
            Session["userPage"] = user;
            Response.Redirect("Userpage.aspx");
        }
        protected void Explore_Click(object sender, EventArgs e) { Response.Redirect("Explore.aspx"); }
        protected void Gamelog_Click(object sender, EventArgs e) 
        {
            Session["userPage"] = user;
            Response.Redirect("Gamelog.aspx"); 
        }
        protected void Achievements_Click(object sender, EventArgs e) 
        {
            Session["userPage"] = user;
            Response.Redirect("Achievements.aspx"); 
        }
        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            //Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        protected void Create_Click(object sender, EventArgs e) { Response.Redirect("Create.aspx"); }
        protected void Admin_Click(object sender, EventArgs e) { Response.Redirect("Adminpage.aspx"); }
    }
}