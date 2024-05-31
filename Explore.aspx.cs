using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using DungeonMaker.Classes.Services;
using DungeonMaker.Classes.Types;
using System.IO;

namespace DungeonMaker
{
    public partial class Explore : System.Web.UI.Page
    {
        private DataSet ds;
        private string mapQuery, userQuery;
        private User user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null) Session["user"] = new User();
            user = (User)Session["user"];
            PlayService PS = new PlayService();
            //Default sort: newest. Queries must reset with each postback
            mapQuery = "SELECT Maps.mapID, Maps.mapName, Users.username, Maps.thumbnail, Maps.mapType, Maps.isValid, Maps.estTime FROM " +
                "(Users INNER JOIN Maps ON Users.email = Maps.creator) WHERE mapName LIKE '%%' AND isPublic ORDER BY mapID DESC";
            userQuery = "SELECT email, username, profilePicture FROM Users WHERE username LIKE '%%' ORDER BY Users.creationDate";
            if (!IsPostBack)
            {
                Game game = null;
                if (Session["game"] != null) game = (Game)Session["game"]; //Saves pulling from database
                else if (!user.IsBanned()) game = PlayService.GetLastGame(user);
                DataListMultiView.ActiveViewIndex = 0; //Users view index = 0, Dungeons view index = 1, more TBD
                DataTable mapsTbl = GeneralService.GetDataSetByQuery("SELECT mapID FROM Maps", "Maps").Tables[0];
                List<Map> playedMaps = new List<Map>();
                foreach (DataRow row in mapsTbl.Rows)
                    if (PlayService.countGames(Convert.ToInt32(row["mapID"])) > 0)
                        playedMaps.Add(new Map(Convert.ToInt32(row["mapID"])));
                Cache["playedMaps"] = playedMaps;
                if (game != null)
                { //Shows previous game results in stats panel
                    string nbsp = "<br />&nbsp;&nbsp;&nbsp;";
                    //<b>&#x2022;</b>
                    ((Literal)statsList.FindControl("prevGame")).Text = "Previous game summary:<br />" + nbsp +
                        "<b>Result</b> " + (game.victory ? "victory" : "defeat") + nbsp +
                        "<b>Deaths</b> x" + game.deaths + nbsp +
                        "<b>Stars</b> x" + game.stars + nbsp +
                        "<b>Time</b> " + Utility.SecToMin(game.time) + nbsp +
                        "<b>Map</b> " + game.map.mapName.Remove(game.map.mapName.Length - 1); //Remove map name handler suffix
                    ScriptManager.RegisterStartupScript(this, GetType(), "Display", "document.getElementById('prevList').style.display = 'block';", true);
                    if (!user.IsBanned())
                    {
                        string RecentMapsQuery = "SELECT TOP 5 Maps.mapID, Maps.mapName, Maps.thumbnail, Maps.isValid, Maps.estTime FROM " +
                            "((Games INNER JOIN Users ON Games.player = Users.email) INNER JOIN Maps ON Games.mapID = Maps.mapID) WHERE " +
                            "Games.player = '" + user.email + "' ORDER BY Games.gameID DESC";
                        DataTable table = GeneralService.GetDataSetByQuery(RecentMapsQuery, "Maps").Tables[0], distinctDataTable = table.Clone();
                        //DISTINCT does not work with ORDER BY in Access, so the following code selects distinct items programatically
                        HashSet<string> distinctItems = new HashSet<string>();
                        //HashSet<T> does not allow duplicate elements. If you try to add an element that already exists in it, it'll be ignored.
                        foreach (DataRow row in table.Rows)
                        {
                            string key = row["mapID"].ToString();
                            if (!distinctItems.Contains(key))
                            {
                                distinctItems.Add(key);
                                distinctDataTable.ImportRow(row);
                            }
                        }
                        RecentlyPlayedDataList.DataSource = distinctDataTable;
                        RecentlyPlayedDataList.DataBind();
                        RecentlyPlayedLabel.Visible = true;
                        RecentlyPlayedPanel.Visible = true;
                    }
                }
                string query = "SELECT Maps.mapID, Maps.mapName, Maps.thumbnail, Maps.isValid, Maps.estTime, COUNT(Games.mapID) AS playCount, Users.username " +
                    "AS creatorUsername FROM (Maps LEFT JOIN Games ON Maps.mapID = Games.mapID) LEFT JOIN Users ON Maps.creator = Users.email WHERE isPublic " +
                    "GROUP BY Maps.mapID, Maps.mapName, Maps.thumbnail, Maps.isValid, Maps.estTime, Users.username ORDER BY COUNT(Games.mapID) DESC";
                PopularMapsDataList.DataSource = GeneralService.GetDataSetByQuery(query, "Maps").Tables[0].AsEnumerable().Take(5).CopyToDataTable();
                // ↑ Equivalent to SQL "TOP 5" which didn't work for me in this query ↑
                PopularMapsDataList.DataBind();
                query = "SELECT TOP 5 mapID, mapName, username, thumbnail, isValid, estTime FROM Users INNER JOIN Maps ON Users.email = Maps.creator WHERE isPublic ORDER BY mapID DESC";
                NewestMapsDataList.DataSource = GeneralService.GetDataSetByQuery(query, "Maps");
                NewestMapsDataList.DataBind();
                query = "SELECT Feedback.*, Users.username, Users.profilePicture FROM Users INNER JOIN Feedback ON Feedback.sender = Users.email WHERE Feedback.isFeatured";
                FeedbackDataList.DataSource = GeneralService.GetDataSetByQuery(query, "Feedback");
                FeedbackDataList.DataBind();
                if (user.IsAdmin())
                { //Enlarges datalists' item styles to fit all admin buttons and shows banned user filtering
                    MapsDataList.ItemStyle.CssClass = "admin-maps-template";
                    UsersDataList.ItemStyle.CssClass = "admin-users-template";
                    BannedCBL.Visible = true;
                }
                //Stats panel calculations
                ((Literal)statsList.FindControl("totalGamesPlayed")).Text += GeneralService.LastMonthCompute("Games", "gameID", "datePlayed");
                ((Literal)statsList.FindControl("totalMapsCreated")).Text += GeneralService.LastMonthCompute("Maps", "mapID", "creationDate");
                ((Literal)statsList.FindControl("numberOfUsers")).Text += GeneralService.LastMonthCompute("Users", "email", "creationDate");
                //To find the user with the most popular maps, I sum all games with respect to the mapIDs of users' maps
                query = "SELECT TOP 1 Users.username, Users.email FROM (Users INNER JOIN Maps ON Users.email = Maps.creator) INNER JOIN Games " +
                    "ON Maps.mapID = Games.mapID GROUP BY Users.username, Users.email ORDER BY COUNT(Games.gameID) DESC";
                ((Literal)statsList.FindControl("mostPlayedUserMaps")).Text = GeneralService.GetStringByQuery(query);
                //To find the most active user, I sum games played and maps created for all users except Guest.
                query = "SELECT TOP 1 Users.username, Users.email FROM(SELECT creator AS email, COUNT(*) AS activity_count FROM Maps GROUP BY creator " +
                    "UNION ALL SELECT player AS email, COUNT(*) AS activity_count FROM Games GROUP BY player) AS CombinedActivities INNER JOIN Users " +
                    "ON Users.email = CombinedActivities.email WHERE Users.email <> 'Guest' GROUP BY Users.username, Users.email ORDER BY SUM(CombinedActivities.activity_count) DESC";
                ((Literal)statsList.FindControl("mostActiveUser")).Text = GeneralService.GetStringByQuery(query);
            }
            //Stars are generated in runtime so must be recreated with each postback after the first
            foreach (DataListItem item in FeedbackDataList.Items)
            {
                Comment comment = new Comment(int.Parse(((Label)item.FindControl("feedbackID")).Text));
                PlaceHolder starsPlaceHolder = (PlaceHolder)item.FindControl("starsPlaceHolder");
                for (int i = 0; i < comment.starRating; i++)
                {
                    Image imgStar = new Image();
                    imgStar.ImageUrl = "assets/ui/fullStar.png";
                    imgStar.Width = 20;
                    imgStar.Height = 20;
                    imgStar.Style["pointer-events"] = "none";
                    starsPlaceHolder.Controls.Add(imgStar);
                }
            }
            //DataSet ds = GeneralService.GetDataSetByQuery("SELECT email FROM Users", "Users");
            //foreach (DataRow row in ds.Tables[0].Rows) { StoreService SS = new StoreService(); StoreService.Purchase(new User(row[0].ToString()),"INSERT_NEW_PRODUCT_HERE"); }
        }

        protected void SearchButton_Click(object sender, ImageClickEventArgs e)
        { //Searches the database based on selected parameters
            SearchResultsLabel.Visible = true;
            int index = mapQuery.IndexOf("LEFT JOIN");
            if (index != -1) mapQuery = mapQuery.Remove(index, mapQuery.IndexOf("WHERE") - index); //Remove specific map ordering
            bool isDungeons = TableSelect.SelectedValue == "Dungeons";
            string query = isDungeons ? mapQuery : userQuery;
            query = query.Remove(query.LastIndexOf("BY") + 3); //Remove all ordering
            switch (SortBy.SelectedValue)
            { //Difficulty definition: average death count and stars collected per map
                case "Newest": query += (isDungeons ? "mapID DESC" : "Users.creationDate"); break;
                case "Oldest": query += (isDungeons ? "mapID" : "Users.creationDate DESC"); break;
                case "A-Z": query += (isDungeons ? "mapName" : "username"); break;
                case "Z-A": query += (isDungeons ? "mapName DESC" : "username DESC"); break;
                default: //Only possible if (isDungeons)
                    query += "IIF(ISNULL(MapPopularity.play_count), 0, MapPopularity.play_count) ";
                    if (SortBy.SelectedValue == "Most popular") query += "DESC";
                    query = query.Insert(query.IndexOf("WHERE") - 1,
                        " LEFT JOIN ( SELECT mapID, COUNT(*) AS play_count FROM Games GROUP BY mapID ) AS MapPopularity ON Maps.mapID = MapPopularity.mapID ");
                    break;
            }
            index = query.IndexOf('%') + 1;
            if (user.IsAdmin()) 
            {
                if (isDungeons) query = query.Remove(query.IndexOf("AND"), 13); //Show private maps by removing "AND isPublic"
                else
                { //Filter un/banned users based on selection
                    if (!BannedCBL.Items[0].Selected) query = query.Insert(index + 2, " AND elevation > 0 ");
                    if (!BannedCBL.Items[1].Selected) query = query.Insert(index + 2, " AND elevation < 0 ");
                }
            }
            if (isDungeons) 
            { //Filter map types based on selection
                int SumSelected = 0, newIndex = index + 2;
                foreach (ListItem item in MapTypesCBL.Items) if (item.Selected) SumSelected++;
                if (SumSelected > 0)
                {
                    query = query.Insert(newIndex, " AND "); newIndex += 5;
                    if (MapTypesCBL.Items[0].Selected)
                    {
                        query = query.Insert(newIndex, "Maps.mapType = 'blank' "); newIndex += 23;
                        if (SumSelected > 1) { query = query.Insert(newIndex, "OR "); newIndex += 3; }
                    }
                    if (MapTypesCBL.Items[1].Selected)
                    {
                        query = query.Insert(newIndex, "Maps.mapType = 'cyberpunk' "); newIndex += 27;
                        if (MapTypesCBL.Items[2].Selected) { query = query.Insert(newIndex, "OR "); newIndex += 3; }
                    }
                    if (MapTypesCBL.Items[2].Selected) query = query.Insert(newIndex, "Maps.mapType = 'steampunk' ");
                }
                else 
                {
                    MapsDataList.DataSource = null;
                    MapsDataList.DataBind();
                    BindDLCheck(MapsDataList, true);
                    return;
                }
            }
            query = query.Insert(index, SearchBar.Text); //Add user search input
            //SQL injection is not possible because the first '%' occurs before the search text and IndexOf() returns the index of the first occurrence
            ds = GeneralService.GetDataSetByQuery(query, TableSelect.SelectedValue);
            Session["ds"] = ds;
            query = query.Remove(index, SearchBar.Text.Length); //Remove user search input
            if (isDungeons) 
            {
                mapQuery = query;
                MapsDataList.DataSource = ds;
                MapsDataList.DataBind();
                BindDLCheck(MapsDataList, true);
            }
            else
            {
                userQuery = query;
                UsersDataList.DataSource = ds;
                UsersDataList.DataBind();
                BindDLCheck(UsersDataList, true);
            }
        }

        private void BindDLCheck(DataList dl, bool show)
        { //Visual technicalities, optimizes space on screen
            SearchResultsLabel.Visible = show;
            SearchResultsLabel.Text = (dl.Items.Count > 0 ? "SEARCH" : "NO") + " RESULTS";
            bool minimize = dl.Items.Count > 0 && dl.ID == "MapsDataList";
            statisticsPanel.Style["Width"] = minimize ? "13.5%" : "20%";
            patchNotesPanel.Style["Width"] = minimize ? "13.5%" : "20%";
            patchNotesPanel.Style["left"] = minimize ? "86.5%" : "80%";
        }

        protected void TableSelect_SelectedIndexChanged(object sender, EventArgs e)
        { //Changes search objective (users/dungeons)
            bool isDungeons = TableSelect.SelectedValue == "Dungeons";
            MapTypesCBL.Visible = isDungeons;
            if (user.IsAdmin()) BannedCBL.Visible = !isDungeons;
            SortBy.Items[4].Enabled = isDungeons; //most popular
            SortBy.Items[5].Enabled = isDungeons; //least popular
            SearchBar.Attributes["placeholder"] = "Search for " + (isDungeons ? "dungeons..." : "users...");
            DataList dl = isDungeons ? MapsDataList : UsersDataList;
            BindDLCheck(dl, dl.Items.Count > 0);
            DataListMultiView.ActiveViewIndex = Convert.ToInt32(isDungeons); //false -> 0 -> users, true -> 1 -> dungeons
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
                if (PlayService.countGames(map.mapID) == 0)
                { //Delete map
                    map.Delete();
                    FileInfo thumbnail = new FileInfo(Server.MapPath(map.thumbnail));
                    try { thumbnail.Delete(); }
                    catch { ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Thumbnail deletion error, likely due to physical path.');", true); }
                    DataTable dt = ((DataSet)Session["ds"]).Tables[0];
                    DataRow rowToDelete = dt.Select("mapID = " + map.mapID).FirstOrDefault();
                    if (rowToDelete != null) dt.Rows.Remove(rowToDelete);
                    MapsDataList.DataSource = dt;
                    MapsDataList.DataBind();
                }
                else 
                { /* Disable/enable map */
                    BindButtonChange(e.Item);
                    MapService.ChangeValid(map.mapID);
                    DataList[] dataLists = { MapsDataList, RecentlyPlayedDataList, PopularMapsDataList, NewestMapsDataList };
                    bool flag;
                    foreach (DataList dl in dataLists)
                    {
                        if (((DataList)source).ID != dl.ID)
                        { //No need to scan through source datalist, already changed using e.Item
                            flag = true;
                            for (int i = 0; flag && i < dl.Items.Count; i++)
                            {
                                if (int.Parse(((Label)dl.Items[i].FindControl("mapID")).Text) == map.mapID)
                                {
                                    BindButtonChange(dl.Items[i]);
                                    flag = false; //Once found no need to scan through rest of datalist
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BindButtonChange(DataListItem item) 
        { //UX mods
            Button bt = (Button)item.FindControl("DeleteButton");
            bool isEnabled = bt.Text == "Disable";
            ((Button)item.FindControl("PlayButton")).Enabled = !isEnabled;
            bt.Text = isEnabled ? "Enable" : "Disable";
        }

        protected void UsersDataList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            User userpage = new User(((Label)e.Item.FindControl("Email")).Text);
            switch (e.CommandName) 
            {
                case "Visit_Click": //Redirects to selected user's userpage
                    Session["userPage"] = userpage;
                    Response.Redirect("Userpage.aspx");
                    break;
                case "Ban_Click": /*Un/bans user*/
                    UserService.ChangeBlockState(userpage);
                    Button banBtn = (Button)e.Item.FindControl("BanButton");
                    banBtn.Text = banBtn.Text == "Ban" ? "Unban" : "Ban";
                    break;
                case "Logs_Click": //Redirects to selected user's logs page
                    Session["userPage"] = userpage;
                    Response.Redirect("Logs.aspx");
                    break;
            }
        }

        protected void UsersDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        { //predisplay necessary changes
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (this.user.IsAdmin())
                {
                    ((Button)e.Item.FindControl("BanButton")).Visible = true;
                    ((Button)e.Item.FindControl("LogsButton")).Visible = true;
                }
                User user = new User(((Label)e.Item.FindControl("Email")).Text);
                if (user.IsBanned()) ((Button)e.Item.FindControl("BanButton")).Text = "Unban";
                if (user.email == this.user.email) e.Item.Enabled = false;
            }
        }

        protected void MapsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label title = (Label)e.Item.FindControl("Title");
                title.Text = title.Text.Remove(title.Text.Length - 1); //Remove thumbnail name handler (map count suffix)
                Label time = (Label)e.Item.FindControl("EstimatedTime");
                time.Text = Utility.SecToMin(int.Parse(time.Text));
                Map map = null;
                if (user.IsAdmin() || (DataList)sender == RecentlyPlayedDataList) 
                { //No need to scan list if its result won't be used
                    foreach (Map played in (List<Map>)Cache["playedMaps"]) //Saves database trip for each map in datalist by saving them all once in Cache at !IsPostBack
                        if (played.mapID == int.Parse(((Label)e.Item.FindControl("mapID")).Text))
                            { map = played; break; } //Exits loop when map is found
                    if ((DataList)sender == RecentlyPlayedDataList) ((Label)e.Item.FindControl("Creator")).Text = map.creator.username;
                    if (user.IsAdmin())
                    {
                        Button bt = (Button)e.Item.FindControl("DeleteButton");
                        if (map != null) bt.Text = ((Button)e.Item.FindControl("PlayButton")).Enabled ? "Disable" : "Enable";
                        bt.Visible = true;
                    }
                }
            }
        }

        protected void FeedbackDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        { //Makes feedback datalist items' text expandable on click through js
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label body = (Label)e.Item.FindControl("Feedback");
                if (body.Text.Length > 100) body.Text = "<span class='expandable-text' onclick='expandText(this)'>" + body.Text.Remove(100) + "..." + "</span>" +
                    "<span class='full-text' style='display:none;'>" + body.Text + "</span>";
            }
        }
    }
}