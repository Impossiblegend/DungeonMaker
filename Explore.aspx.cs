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

namespace DungeonMaker
{
    public partial class Explore : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Game game = null;
            if (Session["game"] != null) game = (Game)Session["game"];
            else if (Session["user"] != null)
            {
                if (((User)Session["user"]).elevation > 0)
                {
                    game = PlayService.GetLastGame(((User)Session["user"]).email);
                    if (game != null)
                    {
                        string victory = game.victory ? "victory" : "defeat";
                        ((Literal)statsList.FindControl("prevGame")).Text = "Previous game summary:<br />" +
                            "<br />&nbsp;&nbsp;&nbsp;<b>Result</b> " + victory +
                            "<br />&nbsp;&nbsp;&nbsp;<b>Deaths</b> x" + game.deaths +
                            "<br />&nbsp;&nbsp;&nbsp;<b>Stars</b> x" + game.stars +
                            "<br />&nbsp;&nbsp;&nbsp;<b>Time</b> " + Connect.SecToMin(game.time) +
                            "<br />&nbsp;&nbsp;&nbsp;<b>Map</b> " + game.map.mapName.Remove(game.map.mapName.Length - 1);
                    }
                }
            }
            else Session["user"] = new User();
            //Default: newest
            if (!IsPostBack) 
            {
                DataListMultiView.ActiveViewIndex = 0;
                PlayService PS = new PlayService();
                string query = "SELECT Maps.mapID, Maps.mapName, Maps.thumbnail, COUNT(Games.mapID) AS playCount, Users.username AS creatorUsername " +
                    "FROM (Maps LEFT JOIN Games ON Maps.mapID = Games.mapID) LEFT JOIN Users ON Maps.creator = Users.email " +
                    "GROUP BY Maps.mapID, Maps.mapName, Maps.thumbnail, Users.username ORDER BY COUNT(Games.mapID) DESC";
                PopularMapsDataList.DataSource = ProductService.GetProductsByQuery(query, "Maps").Tables[0].AsEnumerable().Take(5).CopyToDataTable();
                PopularMapsDataList.DataBind();
                query = "SELECT TOP 5 mapID, mapName, username, thumbnail FROM Users INNER JOIN Maps " +
                    "ON Users.email = Maps.creator WHERE isPublic ORDER BY mapID DESC";
                NewestMapsDataList.DataSource = ProductService.GetProductsByQuery(query, "Maps");
                NewestMapsDataList.DataBind();
                query = "SELECT Feedback.*, Users.username, Users.profilePicture FROM Users INNER JOIN Feedback " +
                    "ON Feedback.sender = Users.email WHERE Feedback.isFeatured";
                FeedbackDataList.DataSource = ProductService.GetProductsByQuery(query, "Feedback");
                FeedbackDataList.DataBind();
            }
            if (((User)Session["user"]).elevation == 2) MapsDataList.ItemStyle.CssClass = "admin-maps-template";
            ((Literal)statsList.FindControl("totalGamesPlayed")).Text = "[X]";
            ((Literal)statsList.FindControl("totalMapsCreated")).Text = "[X]";
            ((Literal)statsList.FindControl("numberOfUsers")).Text = "[X]";
            ((Literal)statsList.FindControl("mostPlayedUserMaps")).Text = "[X]";
            ((Literal)statsList.FindControl("mostActiveUser")).Text = "[X]";
            Session["mapQuery"] = "SELECT mapID, mapName, username, thumbnail FROM Users INNER JOIN Maps ON " +
                "Users.email = Maps.creator WHERE mapName LIKE '%%' AND isPublic ORDER BY mapID DESC";
            Session["userQuery"] = "SELECT email, username, profilePicture FROM Users WHERE username " +
                "LIKE '%%' ORDER BY Users.creationDate";
            foreach (DataListItem item in FeedbackDataList.Items)
            {
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
            Session["mapQuery"] = Session["mapQuery"].ToString().Remove(Session["mapQuery"].ToString().IndexOf("BY") + 3);
            Session["userQuery"] = Session["userQuery"].ToString().Remove(Session["userQuery"].ToString().IndexOf("BY") + 3);
            if (TableSelect.SelectedValue == "Users")
            {
                switch (SortBy.SelectedValue)
                {
                    case "Newest": Session["userQuery"] += "Users.creationDate"; break;
                    case "Oldest": Session["userQuery"] += "Users.creationDate DESC"; break;
                    case "A-Z": Session["userQuery"] += "username"; break;
                    case "Z-A": Session["userQuery"] += "username DESC"; break;
                }
                Session["userQuery"] = Session["userQuery"].ToString().Insert(Session["userQuery"].ToString().IndexOf('%') + 1, SearchBar.Text);
                Session["ds"] = ProductService.GetProductsByQuery(Session["userQuery"].ToString(), "Users");
                UsersDataList.DataSource = (DataSet)Session["ds"];
                UsersDataList.DataBind();
                if (UsersDataList.Items.Count > 0) statisticsPanel.Style["Width"] = "13.5%";
                else statisticsPanel.Style["Width"] = "20%";
                Session["userQuery"] = Session["userQuery"].ToString().Remove(Session["userQuery"].ToString().IndexOf('%') + 1, SearchBar.Text.Length);
            }
            if (TableSelect.SelectedValue == "Maps")
            {
                switch (SortBy.SelectedValue)
                {
                    case "Newest": Session["mapQuery"] += "mapID DESC"; break;
                    case "Oldest": Session["mapQuery"] += "mapID"; break;
                    case "A-Z": Session["mapQuery"] += "mapName"; break;
                    case "Z-A": Session["mapQuery"] += "mapName DESC"; break;
                }
                Session["mapQuery"] = Session["mapQuery"].ToString().Insert(Session["mapQuery"].ToString().IndexOf('%') + 1, SearchBar.Text);
                Session["ds"] = ProductService.GetProductsByQuery(Session["mapQuery"].ToString(), "Maps");
                MapsDataList.DataSource = (DataSet)Session["ds"];
                MapsDataList.DataBind();
                if (MapsDataList.Items.Count > 0) statisticsPanel.Style["Width"] = "13.5%";
                else statisticsPanel.Style["Width"] = "20%";
                Session["mapQuery"] = Session["mapQuery"].ToString().Remove(Session["mapQuery"].ToString().IndexOf('%') + 1, SearchBar.Text.Length);
            }
        }
        //Difficulty definition: average death count and stars collected per map
        //protected void Selection_Change(object sender, EventArgs e) { }
        protected void TableSelect_SelectedIndexChanged(object sender, EventArgs e)
        { //Changes search objective
            switch (TableSelect.SelectedValue)
            {
                case "Maps":
                    DataListMultiView.ActiveViewIndex = 0;
                    SearchBar.Attributes["placeholder"] = "Search for maps...";
                    if (MapsDataList.Items.Count > 0) statisticsPanel.Style["Width"] = "13.5%";
                    else statisticsPanel.Style["Width"] = "20%";
                    break;
                case "Users":
                    DataListMultiView.ActiveViewIndex = 1;
                    SearchBar.Attributes["placeholder"] = "Search for users...";
                    if (UsersDataList.Items.Count > 0) statisticsPanel.Style["Width"] = "13.5%";
                    else statisticsPanel.Style["Width"] = "20%";
                    break;
            }
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
                    DataTable dt = ((DataSet)Session["ds"]).Tables[0];
                    DataRow rowToDelete = dt.Select("mapID = " + map.mapID).FirstOrDefault();
                    if (rowToDelete != null) dt.Rows.Remove(rowToDelete);
                    MapsDataList.DataSource = dt;
                    MapsDataList.DataBind();
                }
                else 
                {
                    Button bt = (Button)e.Item.FindControl("DeleteButton");
                    if (bt.Text == "Disable")
                    {
                        ((Button)e.Item.FindControl("PlayButton")).Enabled = false;
                        bt.Text = "Enable";
                    }
                    else 
                    {
                        ((Button)e.Item.FindControl("PlayButton")).Enabled = true;
                        bt.Text = "Disable";
                    }
                    MapService.ChangeValid(map.mapID);
                }
            }
        }
        protected void UsersDataList_ItemCommand(object source, DataListCommandEventArgs e)
        { 
            if (e.CommandName == "Visit_Click")
            { //Opens selected userpage
                Session["userPage"] = new User(((Label)UsersDataList.Items[e.Item.ItemIndex].FindControl("Email")).Text);
                Response.Redirect("Userpage.aspx");
            }
            if (e.CommandName == "Block_Click")
            { /* Un/blocks selected user */
                UserService.ChangeBlockState(((Label)UsersDataList.Items[e.Item.ItemIndex].FindControl("Email")).Text);
                string isBlocked = ((Button)UsersDataList.Items[e.Item.ItemIndex].FindControl("Block")).Text;
                ((Button)UsersDataList.Items[e.Item.ItemIndex].FindControl("Block")).Text = isBlocked == "Block" ? "Unblock" : "Block";
            }
        }
        protected void UsersDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (((User)Session["user"]).elevation == 2) ((Button)e.Item.FindControl("Block")).Visible = true;
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
                { //If map exists in Games table, enable/disable button instead of delete button
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