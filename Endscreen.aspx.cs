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
                StarCounter.Text = game.stars + "/" + game.map.stars.Count;
                TimeElapsed.Text = Connect.SecToMin(game.time);
                BG.Style["Width"] = "100%";
                BG.Style["Height"] = "100%";
                BG.ImageUrl = game.map.thumbnail;
                if (game.player.elevation > 0)
                { //BEGIN CHECK FOR ACHIEVEMENTS
                    AchievementService AS = new AchievementService(); PlayService PS = new PlayService();
                    List<string> achievements = new List<string>(),
                        existing = AchievementService.GetAchievementsByUser(game.player);
                    if (game.deaths == 9 && AchievementService.IsValid("Cat God") && !existing.Contains("Cat God")) achievements.Add("Cat God");
                    if (game.time <= Math.Ceiling(0.5 * game.map.estTime) && game.victory && AchievementService.IsValid("Speedrun") && !existing.Contains("Speedrun")) achievements.Add("Speedrun");
                    if (PlayService.CountStars(game.player) >= 100 && AchievementService.IsValid("Tycoon") && !existing.Contains("Tycoon")) achievements.Add("Tycoon");
                    string jsArray = "[" + string.Join(",", achievements.Select(a => "'" + a + "'")) + "]";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowAchievements", "window.onload = function() { showAchievements(" + jsArray + "); };", true);
                }
            }
        }
        protected void Finish_Click(object sender, EventArgs e) { Response.Redirect("Explore.aspx"); }
    }
}