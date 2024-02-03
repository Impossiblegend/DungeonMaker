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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null) Session["user"] = new User();
            if (!IsPostBack)
            {
                Session["starsRating"] = 0;
            }
        }
        protected void SendButton_Click(object sender, EventArgs e)
        { //Uploads feedback and rating to database if logged in
            Rating.ForeColor = Color.Black;
            int rating = Convert.ToInt32(Session["starsRating"]);
            if (rating == 0) 
            { 
                Rating.Text = "Rating must be between 1 and 5"; 
                Rating.ForeColor = Color.Red; 
            }
            else if (((User)Session["user"]).email != null)
            {
                CommentService.SendComment(((User)Session["user"]).email, Contact_Textbox.Text, rating);
                SendButton.Text = "SENT!";
                SendButton.Enabled = false;
            }
            else Rating.Text = "You must be signed in to send feedback";
        }
        protected void Rating_Click(object sender, ImageClickEventArgs e)
        { //Changes number of full stars shown
            if (sender is ImageButton star)
            {
                int numStars = 0;
                switch (star.ID)
                {
                    case "OneStar": numStars = 1; break;
                    case "TwoStars": numStars = 2; break;
                    case "ThreeStars": numStars = 3; break;
                    case "FourStars": numStars = 4; break;
                    case "FiveStars": numStars = 5; break;
                }
                if (numStars >= 1) OneStar.ImageUrl = "assets/ui/fullStar.png";
                if (numStars >= 2) TwoStars.ImageUrl = "assets/ui/fullStar.png";
                else TwoStars.ImageUrl = "assets/ui/emptyStar.png";
                if (numStars >= 3) ThreeStars.ImageUrl = "assets/ui/fullStar.png";
                else ThreeStars.ImageUrl = "assets/ui/emptyStar.png";
                if (numStars >= 4) FourStars.ImageUrl = "assets/ui/fullStar.png";
                else FourStars.ImageUrl = "assets/ui/emptyStar.png";
                if (numStars == 5) FiveStars.ImageUrl = "assets/ui/fullStar.png";
                else FiveStars.ImageUrl = "assets/ui/emptyStar.png";
                Session["starsRating"] = numStars;
            }
        }
    }
}