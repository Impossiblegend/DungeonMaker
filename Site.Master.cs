using DungeonMaker.Classes.Services;
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
        public bool CoinVisible { set { imgCredits.Style["display"] = value ? "block" : "none"; } }
        public int UserCredits
        {
            get { return int.Parse(litCredits.Text); }
            set { litCredits.Text = value.ToString(); }
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
                    CreateButton.Visible = false;
                    AchievementsButton.Visible = false;
                    GamelogButton.Visible = false;
                    LogoutButton.Text = "Login";
                }
                else
                {
                    AchievementService AS = new AchievementService();
                    StoreService SS = new StoreService();
                    litCredits.Text = user.IsAdmin() ? "<p style='font-size:35px;'><b>&#8734;</b></p>" : user.GetCredits().ToString();
                    imgCredits.Visible = true;
                }
                ProfilePic.ImageUrl = user.profilePicture;
                if (user.IsAdmin()) AdminButton.Visible = true;
            }
        }
        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            //Session.Abandon();
            Response.Redirect("Login.aspx");
        }
        protected void Menu_Click(object sender, EventArgs e) 
        {
            Session["userPage"] = user;
            imgCredits.Style["display"] = "block";
            IButtonControl btn = (IButtonControl)sender; //LinkButton and ImageButton both implement/inherit IButtonControl
            Response.Redirect(btn.CommandArgument + ".aspx");
        }
    }
}