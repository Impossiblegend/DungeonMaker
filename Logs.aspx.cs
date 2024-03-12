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
        private DataList DL;
        private string ds, dateField;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LogsMultiView.ActiveViewIndex = 0;
                string userpage = ((User)Session["userPage"]).email,
                    query = "SELECT CreditPurchases.bundle AS productName, CreditBundles.cost, CreditPurchases.dateOfPurchase FROM CreditPurchases INNER JOIN CreditBundles " +
                    "ON CreditPurchases.bundle = CreditBundles.bundleName WHERE CreditPurchases.customer = '" + userpage + "' AND CreditBundles.cost > 0 " +
                    "UNION ALL SELECT MapTypes.mapType AS productName, MapTypes.cost, OwnedMapTypes.dateOfPurchase FROM MapTypes INNER JOIN OwnedMapTypes " +
                    "ON MapTypes.mapType = OwnedMapTypes.mapType WHERE OwnedMapTypes.owner = '" + userpage + "' AND MapTypes.cost > 0 " +
                    "UNION ALL SELECT TrapTypes.trapType AS productName, TrapTypes.cost, OwnedTrapTypes.dateOfPurchase FROM TrapTypes INNER JOIN OwnedTrapTypes " +
                    "ON TrapTypes.trapType = OwnedTrapTypes.trapType WHERE OwnedTrapTypes.owner = '" + userpage + "' AND TrapTypes.cost > 0";
                DataSet ds = GeneralService.GetDataSetByQuery(query, "Purchases");
                PurchasesDataList.DataSource = ds;
                Session["purchases"] = ds;
                PurchasesDataList.DataBind();
                query = "SELECT * FROM Achievements INNER JOIN UserAchievements ON Achievements.achievementTitle = UserAchievements.achievement " +
                    "WHERE Achievements.isValid AND UserAchievements.awardee = '" + userpage + "' ORDER BY UserAchievements.dateReceived DESC";
                ds = GeneralService.GetDataSetByQuery(query, "Achievements");
                AchievementsDataList.DataSource = ds;
                Session["achievements"] = ds;
                AchievementsDataList.DataBind();
                query = "SELECT Games.*, Maps.mapName FROM Maps INNER JOIN Games ON Games.mapID = Maps.mapID WHERE Games.player = '" + userpage + "' ORDER BY datePlayed DESC";
                ds = GeneralService.GetDataSetByQuery(query, "Games");
                Session["gamelogs"] = ds;
                GamesDataList.DataSource = ds;
                GamesDataList.DataBind();
                if (GamesDataList.Items.Count == 0) EmptyLabel.Text = "No results.";
                if (GamesDataList.Items.Count > 5) wrapper.Attributes["class"] += " no-padding-bottom";
            }
        }

        protected void TableSelect_SelectedIndexChanged(object sender, EventArgs e)  
        {
            EmptyLabel.Text = "";
            LogsMultiView.ActiveViewIndex = TableSelect.SelectedIndex;
        }

        private string RemoveTime(Label date) { return date.Text.Remove(date.Text.IndexOf(' ')); } //Remove 00:00

        protected void PurchasesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        { //PurchasesDataList preface textual changes
            Label product = (Label)e.Item.FindControl("ProductName"), 
                date = (Label)e.Item.FindControl("dateOfPurchase"), 
                cost = (Label)e.Item.FindControl("Cost");
            if (product.Text == "Heap" || product.Text == "Bundle" || product.Text == "Trove" || product.Text == "Chest")
            {
                product.Text = "Credits " + product.Text;
                cost.Text = "$" + (int.Parse(cost.Text) - 0.01);
            }
            else cost.Text += " CREDITS";
            date.Text = RemoveTime(date);
        }

        protected void AchievementsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        { //AchievementsDataList preface textual changes
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.CssClass = "maps-template animated-item";
                Label body = (Label)e.Item.FindControl("DescriptionBody"), 
                    date = (Label)e.Item.FindControl("dateReceived"), 
                    credits = (Label)e.Item.FindControl("Credits");
                if (body.Text.Length > 100) body.Text = body.Text.Remove(100).Insert(100, "...");
                body.Text += " <b>| Received</b>";
                date.Text = RemoveTime(date) + " <b>| Credits</b>";
                credits.Text = "x" + credits.Text;
            }
        }

        protected void GamesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        { //GamesDataList preface textual changes
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.CssClass = "maps-template animated-item";
                bool victory = (bool)DataBinder.Eval(e.Item.DataItem, "victory");
                ((Label)e.Item.FindControl("Victory")).Text = (victory ? "VICTORY" : "DEFEAT") + "<b> | Dungeon</b>";
                Label title = (Label)e.Item.FindControl("Title"),
                    date = (Label)e.Item.FindControl("datePlayed"),
                    deaths = (Label)e.Item.FindControl("DeathCounter"),
                    stars = (Label)e.Item.FindControl("StarCounter"),
                    time = (Label)e.Item.FindControl("timeElapsed");
                title.Text = title.Text.Remove(title.Text.Length - 1) + "<b> | Date</b>"; //Remove thumbnail name handler (map count suffix)
                date.Text = RemoveTime(date) + "<b> | Deaths</b>";
                deaths.Text = "x" + deaths.Text + "<b> | Stars</b>";
                stars.Text = "x" + stars.Text + "<b> | Time</b>";
                time.Text = Calculations.SecToMin(int.Parse(time.Text));
            }
        }

        protected void ConfirmButton_Click(object sender, EventArgs e)
        { //Confirm datalist date filter
            if (string.IsNullOrEmpty(FromDateTB.Text) || string.IsNullOrEmpty(ToDateTB.Text)) { ErrorMsg("Please provide both From and To dates."); return; }
            DateTime fromDate = DateTime.Parse(FromDateTB.Text), toDate = DateTime.Parse(ToDateTB.Text);
            switch (TableSelect.SelectedIndex)
            {
                case 0: DL = GamesDataList; ds = "gamelogs"; dateField = "datePlayed"; break;
                case 1: DL = AchievementsDataList; ds = "achievements"; dateField = "dateReceived"; break;
                case 2: DL = PurchasesDataList; ds = "purchases"; dateField = "dateOfPurchase"; break;
            }
            wrapper.Attributes["class"] = "content-wrapper";
            if (DL.Items.Count == 0) EmptyLabel.Text = "No results.";
            if (DL.Items.Count > 5) wrapper.Attributes["class"] += " no-padding-bottom";
            DataRow[] filteredRows = ((DataSet)Session[ds]).Tables[0].Select(dateField + " >= #" + fromDate.ToString("MM/dd/yyyy") + "# AND " + dateField + " <= #" + toDate.ToString("MM/dd/yyyy") + "#");
            if (toDate < fromDate) ErrorMsg("To date should be after or equal to From date.");
            else if (filteredRows.Length == 0) ErrorMsg("No results found within the specified date range.");
            else
            {
                DataTable filteredTable = ((DataSet)Session[ds]).Tables[0].Clone();
                foreach (DataRow row in filteredRows) filteredTable.ImportRow(row);
                DL.DataSource = filteredTable;
                DL.DataBind();
            }
        }

        private void ErrorMsg(string msg)
        { //Nullify datalist and display error message
            EmptyLabel.Text = msg;
            if (DL != null)
            {
                DL.DataSource = null;
                DL.DataBind();
            }
        }
    }
}