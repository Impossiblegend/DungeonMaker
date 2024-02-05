using DungeonMaker.classes.Services;
using DungeonMaker.classes.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DungeonMaker.Classes.Types;

namespace DungeonMaker
{
    public partial class Play : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                Map map = (Map)Session["map"];
                MAPTYPE.Text = map.mapType;
                foreach (GameObject star in map.stars) 
                { 
                    STARX.Text += star.x + "_"; 
                    STARY.Text += star.y + "_";
                }
                try 
                { 
                    STARX.Text = STARX.Text.Remove(STARX.Text.Length - 1);
                    STARY.Text = STARY.Text.Remove(STARY.Text.Length - 1);
                }
                catch (ArgumentOutOfRangeException ex) { Console.WriteLine(ex.Message); }
                foreach (Trap trap in map.traps)
                {
                    TRAPX.Text += trap.x + "_";
                    TRAPY.Text += trap.y + "_";
                    TRAPTYPE.Text += trap.type + "_";
                }
                try { TRAPX.Text = TRAPX.Text.Remove(TRAPX.Text.Length - 1); }
                catch (ArgumentOutOfRangeException ex) { Console.WriteLine(ex.Message); Response.Redirect("Error.html"); }
                TRAPY.Text = TRAPY.Text.Remove(TRAPY.Text.Length - 1);
                TRAPTYPE.Text = TRAPTYPE.Text.Remove(TRAPTYPE.Text.Length - 1);
            }
        }
        [System.Web.Services.WebMethod]
        public static void GameEnd(string timeElapsed, string starsCollected, string deathCount, bool victory)
        {
            Play playPage = new Play();
            playPage.ShowPanel(int.Parse(timeElapsed), int.Parse(starsCollected), int.Parse(deathCount), victory);
        }
        private void ShowPanel(int time, int stars, int deaths, bool victory) 
        {
            //Victory = (Label)EndPanel.FindControl("Victory");
            if (Victory == null) Victory = new Label(); Victory.CssClass = "overlay";
            if (DeathCounter == null) DeathCounter = new Label(); DeathCounter.CssClass = "overlay";
            if (StarCounter == null) StarCounter = new Label(); StarCounter.CssClass = "overlay";
            if (TimeElapsed == null) TimeElapsed = new Label(); TimeElapsed.CssClass = "overlay";
            if (EndPanel == null)
            {
                EndPanel = new Panel();
                EndPanel.CssClass = "container";
                EndPanel.Controls.Add(Victory); EndPanel.Controls.Add(DeathCounter); EndPanel.Controls.Add(StarCounter); EndPanel.Controls.Add(TimeElapsed);
            }
            Victory.Text = victory ? "VICTORY" : "DEFEAT";
            DeathCounter.Text = "x" + deaths;
            StarCounter.Text = "x" + stars;
            TimeElapsed.Text = SecToMin(time);
            EndPanel.Visible = true;
            Session["game"] = new Game(PlayService.countGames(((Map)Session["map"]).mapID), (User)Session["user"], DateTime.Today, time, stars, deaths, victory);
        }
        protected void Finish_Click(object sender, EventArgs e)
        {
            Game game = (Game)Session["game"];
            string email = game.player.email;
            if (email == null) email = "Guest";
            PlayService.UploadGame(email, ((Map)Session["map"]).mapID, game.time, game.stars, game.deaths, game.victory);
            Response.Redirect("Explore.aspx");
        }
        private string SecToMin(int sec) 
        { //e.g. 128 sec --> 02:08 min
            string min = "";
            if (sec < 600) min = "0"; 
            min += sec / 60 + ":";
            if (sec % 60 < 10) min += "0";
            min += sec % 60;
            return min;
        }
    }
}