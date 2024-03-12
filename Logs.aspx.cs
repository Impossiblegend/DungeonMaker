using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DungeonMaker
{
    public partial class Logs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query = "SELECT * FROM Achievements INNER JOIN UserAchievements ON Achievements.achievementTitle = UserAchievements.achievement " +
                    "WHERE Achievements.isValid AND UserAchievements.awardee = '" + ((User)Session["userPage"]).email + "' ORDER BY UserAchievements.dateReceived";
                DataSet ds = GeneralService.GetDataSetByQuery(query, "Achievements");
                AchievementsDataList.DataSource = ds;
                Session["achievements"] = ds;
                AchievementsDataList.DataBind();
                if (AchievementsDataList.Items.Count == 0)
                    EmptyLabel.Text = ((User)Session["user"]).IsAdmin() ? "This user has not achieved anything yet." : "Play some games to get achievements!";
                query = "SELECT Games.*, Maps.mapName FROM Maps INNER JOIN Games ON Games.mapID = Maps.mapID WHERE Games.player = '" + ((User)Session["userPage"]).email + "'";
                ds = GeneralService.GetDataSetByQuery(query, "Games");
                Session["gamelogs"] = ds;
                GamesDataList.DataSource = ds;
                GamesDataList.DataBind();
                int count = GamesDataList.Items.Count;
                if (count == 0) EmptyLabel.Text = ((User)Session["user"]).IsAdmin() ? "This user has not played any games yet." : "Play some games for them appear here!";
                if (count > 5) ScriptManager.RegisterStartupScript(this, GetType(), "Padding", "changePadding();", true);
            }
        }
        protected void TableSelect_SelectedIndexChanged(object sender, EventArgs e) 
        {
            wrapper.Attributes["class"] = "content-wrapper no-padding-bottom";
            LogsMultiView.ActiveViewIndex = TableSelect.SelectedIndex; 
        }
        protected void AchievementsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.CssClass = "maps-template animated-item";
                ((Label)e.Item.FindControl("Credits")).Text = "x" + ((Label)e.Item.FindControl("Credits")).Text;
                Label body = (Label)e.Item.FindControl("DescriptionBody");
                if (body.Text.Length > 100) body.Text = body.Text.Remove(100).Insert(100, "...");
                body.Text += " <b>| Received</b>";
                Label date = (Label)e.Item.FindControl("dateReceived");
                date.Text = date.Text.Remove(date.Text.IndexOf(' ')) + " <b>| Credits</b>";
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
            DataList DL = (DataList)((Button)sender).NamingContainer;
            DateTime fromDate = DateTime.Parse(FromDateTB.Text), toDate = DateTime.Parse(ToDateTB.Text);
            string ds = DL.ID == "GamesDataList" ? "gamelogs" : (DL.ID == "AchievementsDataList" ? "achievements" : "purchases");
            DataRow[] filteredRows = ((DataSet)Session[ds]).Tables[0].Select(
                "dateReceived >= #" + fromDate.ToString("MM/dd/yyyy") + "# AND dateReceived <= #" + toDate.ToString("MM/dd/yyyy") + "#");
            if (string.IsNullOrEmpty(FromDateTB.Text) || string.IsNullOrEmpty(ToDateTB.Text)) ErrorMsg("Please provide both From and To dates.", DL);
            else if (toDate < fromDate) ErrorMsg("To date should be after or equal to From date.", DL);
            else if (filteredRows.Length == 0) ErrorMsg("No achievements received within the specified date range.", DL);
            else
            {
                DataTable filteredTable = ((DataSet)Session[ds]).Tables[0].Clone();
                foreach (DataRow row in filteredRows) filteredTable.ImportRow(row);
                DL.DataSource = filteredTable;
                DL.DataBind();
                EmptyLabel.Text = "";
            }
        }
        private void ErrorMsg(string msg, DataList DL)
        {
            EmptyLabel.Text = msg;
            DL.DataSource = null;
            DL.DataBind();
        }
    }
}