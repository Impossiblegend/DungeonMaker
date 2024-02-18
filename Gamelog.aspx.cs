using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DungeonMaker.Classes.Types;
using DungeonMaker.classes.Types;

namespace DungeonMaker
{
    public partial class Gamelog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query = "SELECT Games.*, Maps.mapName FROM Maps INNER JOIN Games ON Games.mapID = Maps.mapID WHERE Games.player = '" + ((User)Session["userPage"]).email + "'";
                GamesDataList.DataSource = GeneralService.GetDataSetByQuery(query, "Games");
                GamesDataList.DataBind();
                if (GamesDataList.Items.Count == 0)
                    EmptyLabel.Text = ((User)Session["user"]).elevation == 2 ?  "This user has not played any games yet." : "Play some games for them appear here!";
            }
        }

        protected void GamesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.CssClass = "maps-template animated-item";
                bool victory = (bool)DataBinder.Eval(e.Item.DataItem, "victory");
                ((Label)e.Item.FindControl("Victory")).Text = victory ? "VICTORY" : "DEFEAT";
                ((Label)e.Item.FindControl("DeathCounter")).Text = "x" + ((Label)e.Item.FindControl("DeathCounter")).Text;
                ((Label)e.Item.FindControl("StarCounter")).Text = "x" + ((Label)e.Item.FindControl("StarCounter")).Text;
                Label time = (Label)e.Item.FindControl("timeElapsed");
                time.Text = Connect.SecToMin(int.Parse(time.Text));
                Label date = (Label)e.Item.FindControl("datePlayed");
                date.Text = date.Text.Remove(date.Text.IndexOf(' '));
                Label title = (Label)e.Item.FindControl("Title");
                title.Text = title.Text.Remove(title.Text.Length - 1); //Remove thumbnail name handler (map count suffix)
            }
        }
    }
}