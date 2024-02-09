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
            }
        }
        protected void Finish_Click(object sender, EventArgs e) { Response.Redirect("Explore.aspx"); }
    }
}