using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DungeonMaker.Classes.Types;
using System.Data;

namespace DungeonMaker
{
    public partial class GamelogButton : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query = "SELECT Games.*, Maps.mapName FROM Maps INNER JOIN Games ON Games.mapID = Maps.mapID WHERE Games.player = '" + ((User)Session["userPage"]).email + "'";
                Session["ds"] = GeneralService.GetDataSetByQuery(query, "Games");
                GamesDataList.DataSource = (DataSet)Session["ds"];
                GamesDataList.DataBind();
                int count = GamesDataList.Items.Count;
                if (count == 0) EmptyLabel.Text = ((User)Session["user"]).IsAdmin() ? "This user has not played any games yet." : "Play some games for them appear here!";
                if (count > 5) ScriptManager.RegisterStartupScript(this, GetType(), "Padding", "changePadding();", true);
            }
        }
        protected void GamesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.CssClass = "maps-template animated-item";
                bool victory = (bool)DataBinder.Eval(e.Item.DataItem, "victory");
                ((Label)e.Item.FindControl("Victory")).Text = (victory ? "VICTORY" : "DEFEAT") + "<b> | Dungeon</b>";
                Label title = (Label)e.Item.FindControl("Title");
                title.Text = title.Text.Remove(title.Text.Length - 1) + "<b> | Date</b>"; //Remove thumbnail name handler (map count suffix)
                Label date = (Label)e.Item.FindControl("datePlayed");
                date.Text = date.Text.Remove(date.Text.IndexOf(' ')) + "<b> | Deaths</b>"; //Remove 00:00
                ((Label)e.Item.FindControl("DeathCounter")).Text = "x" + ((Label)e.Item.FindControl("DeathCounter")).Text + "<b> | Stars</b>";
                ((Label)e.Item.FindControl("StarCounter")).Text = "x" + ((Label)e.Item.FindControl("StarCounter")).Text + "<b> | Time</b>";
                Label time = (Label)e.Item.FindControl("timeElapsed");
                time.Text = Connect.SecToMin(int.Parse(time.Text));
            }
        }
        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            DateTime fromDate = DateTime.Parse(FromDateTB.Text), toDate = DateTime.Parse(ToDateTB.Text);
            DataRow[] filteredRows = ((DataSet)Session["ds"]).Tables[0].Select(
                "datePlayed >= #" + fromDate.ToString("MM/dd/yyyy") + "# AND datePlayed <= #" + toDate.ToString("MM/dd/yyyy") + "#");
            if (string.IsNullOrEmpty(FromDateTB.Text) || string.IsNullOrEmpty(ToDateTB.Text)) ErrorMsg("Please provide both From and To dates.");
            else if (toDate < fromDate) ErrorMsg("To date should be after or equal to From date.");
            else if (filteredRows.Length == 0) ErrorMsg("No games found within the specified date range.");
            else
            {
                DataTable filteredTable = ((DataSet)Session["ds"]).Tables[0].Clone();
                foreach (DataRow row in filteredRows) filteredTable.ImportRow(row);
                GamesDataList.DataSource = filteredTable;
                GamesDataList.DataBind();
                EmptyLabel.Text = "";
            }
        }
        private void ErrorMsg(string msg) 
        {
            EmptyLabel.Text = msg;
            GamesDataList.DataSource = null;
            GamesDataList.DataBind();
        }
    }
}