using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
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
                //CHECK FOR ACHIEVEMENTS START
                //CHECK FOR ACHIEVEMENTS END
                string query = "SELECT * FROM Achievements INNER JOIN UserAchievements ON " +
                    "Achievements.achievementTitle = UserAchievements.achievement WHERE Achievements.isValid AND UserAchievements.awardee = '" + ((User)Session["userPage"]).email + "'";
                AchievementsDataList.DataSource = GeneralService.GetDataSetByQuery(query, "Achievements");
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
    }
}