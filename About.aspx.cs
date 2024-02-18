using DungeonMaker.classes.Services;
using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DungeonMaker
{
    public partial class About : System.Web.UI.Page
    {
        private int rating;
        private User user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user"] == null) Session["user"] = new User();
                user = (User)Session["user"];
                rating = 0;
                CommentService CS = new CommentService();
            }
        }
        protected void SendButton_Click(object sender, EventArgs e)
        { //Uploads feedback and rating to database if logged in
            RatingLabel.ForeColor = Color.Black;
            if (rating == 0)
            {
                RatingLabel.Text = "Rating must be between 1 and 5";
                RatingLabel.ForeColor = Color.Red;
            }
            else if (user.email != null)
            {
                if (CommentService.CanUpload(user))
                {
                    CommentService.SendComment(user, Contact_Textbox.Text, rating);
                    SendButton.Text = "SENT!";
                    SendButton.Enabled = false;
                }
                else RatingLabel.Text = "You have already sent feedback this month.";
            }
            else RatingLabel.Text = "You must be signed in to send feedback";
        }
        protected void Rating_Click(object sender, ImageClickEventArgs e)
        { //Changes number of full stars shown
            if (sender is ImageButton star)
            {
                switch (star.ID)
                {
                    case "OneStar": rating = 1; break;
                    case "TwoStars": rating = 2; break;
                    case "ThreeStars": rating = 3; break;
                    case "FourStars": rating = 4; break;
                    case "FiveStars": rating = 5; break;
                }
                if (rating >= 1) OneStar.ImageUrl = "assets/ui/fullStar.png";
                TwoStars.ImageUrl = "assets/ui/" + (rating >= 2 ? "fullStar.png" : "emptyStar.png");
                ThreeStars.ImageUrl = "assets/ui/" + (rating >= 3 ? "fullStar.png" : "emptyStar.png");
                FourStars.ImageUrl = "assets/ui/" + (rating >= 4 ? "fullStar.png" : "emptyStar.png");
                FiveStars.ImageUrl = "assets/ui/" + (rating == 5 ? "fullStar.png" : "emptyStar.png");
            }
        }
    }
}