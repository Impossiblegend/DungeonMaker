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
                TimeElapsed.Text = SecToMin(game.time);
                BG.Style["Width"] = "100%";
                BG.Style["Height"] = "100%";
                BG.ImageUrl = game.map.thumbnail;
            }
        }
        protected void Finish_Click(object sender, EventArgs e) { Response.Redirect("Explore.aspx"); }
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