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
                foreach (var star in map.stars) 
                { 
                    STARX.Text += ((GameObject)star).x + "_"; 
                    STARY.Text += ((GameObject)star).y + "_";
                }
                try { STARX.Text = STARX.Text.Remove(STARX.Text.Length - 1); }
                catch (ArgumentOutOfRangeException ex) { Console.WriteLine(ex.Message); }
                STARY.Text = STARY.Text.Remove(STARY.Text.Length - 1);
                foreach (var trap in map.traps)
                {
                    TRAPX.Text += ((Trap)trap).x + "_";
                    TRAPY.Text += ((Trap)trap).y + "_";
                    TRAPTYPE.Text += ((Trap)trap).type + "_";
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
            playPage.UploadGame(int.Parse(timeElapsed), int.Parse(starsCollected), int.Parse(deathCount), victory);
        }
        private void UploadGame(int time, int stars, int deaths, bool victory) 
        {
            string email = ((User)Session["user"]).email; //Don't record guest activity
            if(email != null) PlayService.UploadGame(email, ((Map)Session["map"]).mapID, time, stars, deaths, victory); 
        }
    }
}