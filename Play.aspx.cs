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
            Victory.Text = victory ? "VICTORY" : "DEFEAT";
            DeathCounter.Text = "x" + deaths;
            StarCounter.Text = "x" + stars;
            TimeElapsed.Text = time / 60 + ":" + time % 60;
            EndPanel.Visible = true;
            Session["game"] = new Game(PlayService.countGames(((Map)Session["map"]).mapID), (User)Session["user"], DateTime.Today, time, stars, deaths, victory);
        }
        protected void Finish_Click(object sender, EventArgs e)
        {
            Game game = (Game)Session["game"];
            string email = game.player.email;
            if (email == null) email = "Guest";
            PlayService.UploadGame(email, ((Map)Session["map"]).mapID, game.time, game.stars, game.deaths, game.victory);
        }
    }
}