using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using DungeonMaker.classes.Services;
using DungeonMaker.Classes.Types;
using DungeonMaker.classes.Types;
using System.IO;

namespace DungeonMaker
{
    public partial class Explore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Game game = null;
            User user = (User)Session["user"];
            if (Session["game"] != null) game = (Game)Session["game"];
            else if (user != null)
            {
                if (user.elevation > 0)
                {
                    game = PlayService.GetLastGame(user.email);
                    if (game != null)
                    {
                        string nbsp = "<br />&nbsp;&nbsp;&nbsp;";
                        ((Literal)statsList.FindControl("prevGame")).Text = "Previous game summary:<br />" + nbsp + //<b>&#x2022;</b>
                            "<b>Result</b> " + (game.victory ? "victory" : "defeat") + nbsp +
                            "<b>Deaths</b> x" + game.deaths + nbsp +
                            "<b>Stars</b> x" + game.stars + nbsp +
                            "<b>Time</b> " + Connect.SecToMin(game.time) + nbsp +
                            "<b>Map</b> " + game.map.mapName.Remove(game.map.mapName.Length - 1);
                        ScriptManager.RegisterStartupScript(this, GetType(), "Display", "document.getElementById('prevList').style.display = 'block';", true);
                    }
                }
            }
            else
            {
                user = new User();
                Session["user"] = user;
            }
            if (!IsPostBack) 
            {
                DataListMultiView.ActiveViewIndex = 0; //Users view index = 0, Dungeons view index = 1, more TBD
                PlayService PS = new PlayService(); //Initialize private static properties
                string query = "SELECT Maps.mapID, Maps.mapName, Maps.thumbnail, COUNT(Games.mapID) AS playCount, Users.username AS creatorUsername " +
                    "FROM (Maps LEFT JOIN Games ON Maps.mapID = Games.mapID) LEFT JOIN Users ON Maps.creator = Users.email WHERE isPublic " +
                    "GROUP BY Maps.mapID, Maps.mapName, Maps.thumbnail, Users.username ORDER BY COUNT(Games.mapID) DESC";
                PopularMapsDataList.DataSource = ProductService.GetDataSetByQuery(query, "Maps").Tables[0].AsEnumerable().Take(5).CopyToDataTable();
                // ↑ Equivalent to SQL "TOP 5" which didn't work for me in this query ↑
                PopularMapsDataList.DataBind();
                query = "SELECT TOP 5 mapID, mapName, username, thumbnail FROM Users INNER JOIN Maps " +
                    "ON Users.email = Maps.creator WHERE isPublic ORDER BY mapID DESC";
                NewestMapsDataList.DataSource = ProductService.GetDataSetByQuery(query, "Maps");
                NewestMapsDataList.DataBind();
                query = "SELECT Feedback.*, Users.username, Users.profilePicture FROM Users INNER JOIN Feedback " +
                    "ON Feedback.sender = Users.email WHERE Feedback.isFeatured";
                FeedbackDataList.DataSource = ProductService.GetDataSetByQuery(query, "Feedback");
                FeedbackDataList.DataBind();
                if (user.elevation == 2)
                { //Enlarges datalist item styles to fit all admin buttons
                    MapsDataList.ItemStyle.CssClass = "admin-maps-template";
                    UsersDataList.ItemStyle.CssClass = "admin-users-template";
                }
                ((Literal)statsList.FindControl("totalGamesPlayed")).Text += ProductService.LastMonthCompute("Games", "gameID", "datePlayed");
                ((Literal)statsList.FindControl("totalMapsCreated")).Text += ProductService.LastMonthCompute("Maps", "mapID", "creationDate");
                ((Literal)statsList.FindControl("numberOfUsers")).Text += ProductService.LastMonthCompute("Users", "email", "creationDate");
                query = "SELECT TOP 1 Users.username, Users.email FROM (Users INNER JOIN Maps ON Users.email = Maps.creator) INNER JOIN Games " +
                    "ON Maps.mapID = Games.mapID GROUP BY Users.username, Users.email ORDER BY COUNT(Games.gameID) DESC";
                ((Literal)statsList.FindControl("mostPlayedUserMaps")).Text = ProductService.GetStringByQuery(query);
                query = "SELECT TOP 1 Users.username, Users.email FROM(SELECT creator AS email, COUNT(*) AS activity_count FROM Maps " +
                    "GROUP BY creator UNION ALL SELECT player AS email, COUNT(*) AS activity_count FROM Games GROUP BY player) AS CombinedActivities " +
                    "INNER JOIN Users ON Users.email = CombinedActivities.email GROUP BY Users.username, Users.email ORDER BY SUM(CombinedActivities.activity_count) DESC";
                ((Literal)statsList.FindControl("mostActiveUser")).Text = ProductService.GetStringByQuery(query); //Counts maps created and games played equally
            }
            //Default sort: newest
            Session["mapQuery"] = "SELECT Maps.mapID, Maps.mapName, Users.username, Maps.thumbnail FROM (Users INNER JOIN Maps ON " +
                "Users.email = Maps.creator) WHERE mapName LIKE '%%' AND isPublic ORDER BY mapID DESC";
            Session["userQuery"] = "SELECT email, username, profilePicture FROM Users WHERE username " +
                "LIKE '%%' ORDER BY Users.creationDate";
            foreach (DataListItem item in FeedbackDataList.Items)
            { //Stars are generated in runtime so must be recreated with each postback
                Label starRating = (Label)item.FindControl("starRating");
                PlaceHolder starsPlaceHolder = (PlaceHolder)item.FindControl("starsPlaceHolder");
                if (int.TryParse(starRating.Text, out int rating))
                {
                    for (int i = 0; i < rating; i++)
                    {
                        Image imgStar = new Image();
                        imgStar.ImageUrl = "assets/ui/fullStar.png";
                        imgStar.Width = 20;
                        imgStar.Height = 20;
                        starsPlaceHolder.Controls.Add(imgStar);
                    }
                }
            }
        }
        protected void SearchButton_Click(object sender, ImageClickEventArgs e)
        { //Searches the database based on selected parameters
            string tempQuery = Session["mapQuery"].ToString();
            int index = tempQuery.IndexOf("LEFT JOIN");
            if (index != -1) Session["mapQuery"] = tempQuery.Remove(index, tempQuery.IndexOf("WHERE") - index); //Remove specific map ordering
            bool isDungeons = TableSelect.SelectedValue == "Dungeons";
            string query = isDungeons ? "mapQuery" : "userQuery";
            Session[query] = Session[query].ToString().Remove(Session[query].ToString().LastIndexOf("BY") + 3); //Remove all ordering
            switch (SortBy.SelectedValue)
            { //Difficulty definition: average death count and stars collected per map
                case "Newest": Session[query] += (isDungeons ? "mapID DESC" : "Users.creationDate"); break;
                case "Oldest": Session[query] += (isDungeons ? "mapID" : "Users.creationDate DESC"); break;
                case "A-Z": Session[query] += (isDungeons ? "mapName" : "username"); break;
                case "Z-A": Session[query] += (isDungeons ? "mapName DESC" : "username DESC"); break;
                default: //Only possible if (isDungeons)
                    Session["mapQuery"] += "IIF(ISNULL(MapPopularity.play_count), 0, MapPopularity.play_count) ";
                    if (SortBy.SelectedValue == "Most popular") Session["mapQuery"] += "DESC";
                    Session["mapQuery"] = Session["mapQuery"].ToString().Insert(Session["mapQuery"].ToString().IndexOf("WHERE") - 1,
                        " LEFT JOIN ( SELECT mapID, COUNT(*) AS play_count FROM Games GROUP BY mapID ) AS MapPopularity ON Maps.mapID = MapPopularity.mapID ");
                    break;
            }
            Session[query] = Session[query].ToString().Insert(Session[query].ToString().IndexOf('%') + 1, SearchBar.Text); //Add user search input
            //SQL injection is not possible because the first '%' occurs before the search text and IndexOf() returns the index of the first occurrence
            Session["ds"] = ProductService.GetDataSetByQuery(Session[query].ToString(), TableSelect.SelectedValue);
            if (isDungeons) 
            {
                MapsDataList.DataSource = (DataSet)Session["ds"];
                MapsDataList.DataBind();
                BindDLCheck(MapsDataList);
            }
            else
            {
                UsersDataList.DataSource = (DataSet)Session["ds"];
                UsersDataList.DataBind();
                BindDLCheck(UsersDataList);
            }
            Session[query] = Session[query].ToString().Remove(Session[query].ToString().IndexOf('%') + 1, SearchBar.Text.Length); //Remove user search input
        }
        private void BindDLCheck(DataList dl)
        {
            SearchResultsLabel.Visible = dl.Items.Count > 0;
            bool flag = dl.Items.Count > 0 && dl.ID == "MapsDataList";
            statisticsPanel.Style["Width"] = flag ? "13.5%" : "20%";
            patchNotesPanel.Style["Width"] = flag ? "13.5%" : "20%";
            patchNotesPanel.Style["left"] = flag ? "86.5%" : "80%";
        }
        protected void TableSelect_SelectedIndexChanged(object sender, EventArgs e)
        { //Changes search objective
            bool flag = TableSelect.SelectedValue == "Dungeons";
            SortBy.Items[4].Enabled = flag; // most popular
            SortBy.Items[5].Enabled = flag;// least popular
            SearchBar.Attributes["placeholder"] = flag ? "Search for dungeons..." : "Search for users...";
            BindDLCheck(flag ? MapsDataList : UsersDataList);
            DataListMultiView.ActiveViewIndex = !flag ? 0 : 1;
        }
        protected void MapsDataList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            Map map = new Map(int.Parse(((Label)e.Item.FindControl("mapID")).Text));
            if (e.CommandName == "PlayButton")
            { //Opens selected map
                Session["map"] = map;
                Response.Redirect("Play.aspx");
            }
            if (e.CommandName == "DeleteButton")
            { //Deletes unplayed map from DB, and updates datalist accordingly
                if (!PlayService.wasMapPlayed(map.mapID))
                {
                    map.Delete();
                    FileInfo thumbnail = new FileInfo(Server.MapPath(map.thumbnail));
                    try { thumbnail.Delete(); }
                    catch { ScriptManager.RegisterStartupScript(this, GetType(), "Log", "console.log('Deletion error, likely due to physical path.');", true); }
                    DataTable dt = ((DataSet)Session["ds"]).Tables[0];
                    DataRow rowToDelete = dt.Select("mapID = " + map.mapID).FirstOrDefault();
                    if (rowToDelete != null) dt.Rows.Remove(rowToDelete);
                    MapsDataList.DataSource = dt;
                    MapsDataList.DataBind();
                }
                else 
                {
                    Button bt = (Button)e.Item.FindControl("DeleteButton");
                    bool isEnabled = bt.Text == "Disable";
                    ((Button)e.Item.FindControl("PlayButton")).Enabled = !isEnabled;
                    bt.Text = isEnabled ? "Enable" : "Disable";
                    MapService.ChangeValid(map.mapID);
                }
            }
        }
        protected void UsersDataList_ItemCommand(object source, DataListCommandEventArgs e)
        { 
            if (e.CommandName == "Visit_Click")
            { //Opens selected userpage
                Session["userPage"] = new User(((Label)e.Item.FindControl("Email")).Text);
                Response.Redirect("Userpage.aspx");
            }
            if (e.CommandName == "Block_Click")
            { /* Un/blocks selected user */
                UserService.ChangeBlockState(((Label)e.Item.FindControl("Email")).Text);
                string isBlocked = ((Button)e.Item.FindControl("Block")).Text;
                ((Button)e.Item.FindControl("Block")).Text = isBlocked == "Block" ? "Unblock" : "Block";
            }
            if (e.CommandName == "Logs_Click") 
            { //Opens selected user's logs
                Session["userPage"] = new User(((Label)e.Item.FindControl("Email")).Text);
                Response.Redirect("Gamelog.aspx");
            }
        }
        protected void UsersDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (((User)Session["user"]).elevation == 2)
                {
                    ((Button)e.Item.FindControl("Block")).Visible = true;
                    ((Button)e.Item.FindControl("Logs")).Visible = true;
                }
                User user = new User(((Label)e.Item.FindControl("Email")).Text);
                if (user.elevation == -1) ((Button)e.Item.FindControl("Block")).Text = "Unblock";
                if (user.email == ((User)Session["user"]).email) e.Item.Enabled = false;
            }
        }
        protected void MapsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Map map = new Map(int.Parse(((Label)e.Item.FindControl("mapID")).Text));
                Button bt = (Button)e.Item.FindControl("DeleteButton");
                if (PlayService.wasMapPlayed(map.mapID))
                { //If map exists in Games table, changes to enable/disable button instead of delete button
                    if (map.isValid)
                    {
                        bt.Text = "Disable";
                        ((Button)e.Item.FindControl("PlayButton")).Enabled = true;
                    }
                    else
                    {
                        bt.Text = "Enable";
                        ((Button)e.Item.FindControl("PlayButton")).Enabled = false;
                    }
                }
                if (((User)Session["user"]).elevation == 2) bt.Visible = true;
                string st = ((Label)e.Item.FindControl("Title")).Text;
                ((Label)e.Item.FindControl("Title")).Text = st.Remove(st.Length - 1); //Remove thumbnail name handler (map count suffix)
            }
        }
        protected void FeedbackDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label body = (Label)e.Item.FindControl("Feedback");
                if (body.Text.Length > 100) body.Text = body.Text.Remove(100).Insert(100, "...");
            }
        }
    }
}