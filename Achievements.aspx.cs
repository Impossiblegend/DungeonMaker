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
    public partial class Achievements : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query = "SELECT * FROM Achievements INNER JOIN UserAchievements ON Achievements.achievementTitle = UserAchievements.achievement " +
                    "WHERE Achievements.isValid AND UserAchievements.awardee = '" + ((User)Session["userPage"]).email + "' ORDER BY UserAchievements.dateReceived";
                DataSet ds = GeneralService.GetDataSetByQuery(query, "Achievements");
                AchievementsDataList.DataSource = ds;
                Session["ds"] = ds;
                AchievementsDataList.DataBind();
                if (AchievementsDataList.Items.Count == 0)
                    EmptyLabel.Text = ((User)Session["user"]).elevation == 2 ? "This user has not achieved anything yet." : "Play some games to get achievements!";
            }
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
        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FromDateTB.Text) && !string.IsNullOrEmpty(ToDateTB.Text))
            {
                DateTime fromDate = DateTime.Parse(FromDateTB.Text), toDate = DateTime.Parse(ToDateTB.Text);
                if (toDate >= fromDate)
                {
                    DataRow[] filteredRows = ((DataSet)Session["ds"]).Tables[0].Select("dateReceived >= #" + fromDate.ToString("MM/dd/yyyy") + "# AND dateReceived <= #" + toDate.ToString("MM/dd/yyyy") + "#");
                    if (filteredRows.Length > 0)
                    {
                        DataTable filteredTable = ((DataSet)Session["ds"]).Tables[0].Clone();
                        foreach (DataRow row in filteredRows) filteredTable.ImportRow(row);
                        AchievementsDataList.DataSource = filteredTable;
                        AchievementsDataList.DataBind();
                        EmptyLabel.Text = "";
                    }
                    else ErrorMsg("No games found within the specified date range.");
                }
                else ErrorMsg("To date should be after or equal to From date.");
            }
            else ErrorMsg("Please provide both From and To dates.");
        }
        private void ErrorMsg(string msg)
        {
            EmptyLabel.Text = msg;
            AchievementsDataList.DataSource = null;
            AchievementsDataList.DataBind();
        }
    }
}