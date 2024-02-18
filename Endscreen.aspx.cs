using DungeonMaker.classes.Services;
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
    public partial class Endscreen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Game game = (Game)Session["game"];
                Victory.Text = game.victory ? "VICTORY" : "DEFEAT";
                DeathCounter.Text = "x" + game.deaths;
                StarCounter.Text = "x" + game.stars;
                TimeElapsed.Text = Connect.SecToMin(game.time);
                BG.Style["Width"] = "100%";
                BG.Style["Height"] = "100%";
                BG.ImageUrl = game.map.thumbnail;
                //BEGIN CHECK FOR ACHIEVEMENTS
                List<string> achievements = new List<string>();
                AchievementService AS = new AchievementService();
                if (game.deaths == 9) achievements.Add("Cat God");
                if (game.time <= Math.Ceiling(0.5 * game.map.estTime)) achievements.Add("Speedrun");
                int countStars = 0;
                foreach (Game log in PlayService.GetUserGames(game.player)) countStars += log.stars;
                if (countStars >= 100) achievements.Add("Tycoon");
                foreach (string title in achievements)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Achieved: " + title + "');", true);
                    AchievementService.Achieve(title, game.player);
                }
            }
        }
        protected void Finish_Click(object sender, EventArgs e) { Response.Redirect("Explore.aspx"); }
    }
}