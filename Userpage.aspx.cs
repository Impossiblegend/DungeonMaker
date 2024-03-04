using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DungeonMaker.Classes.Services;
using System.Drawing;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace DungeonMaker
{
    public partial class Userpage : System.Web.UI.Page
    {
        private DataSet ds;
        private User userpage, user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (userpage == null) userpage = (User)Session["userPage"];
            if (user == null) user = (User)Session["user"];
            if (user.elevation == 0) Response.Redirect("Register.aspx");
            UserService US = new UserService();
            PlayService PS = new PlayService();
            AchievementService AS = new AchievementService();
            if (!IsPostBack)
            {
                string query = "SELECT mapID, mapName, Maps.creationDate, thumbnail, isPublic FROM Maps WHERE creator = '" + userpage.email + "' ORDER BY mapID DESC";
                ds = GeneralService.GetDataSetByQuery(query, "Maps");
                MapsDataList.DataSource = ds;
                Session["ds"] = ds;
                MapsDataList.DataBind();
                if (MapsDataList.Items.Count == 0) 
                    EmptyLabel.Text = user == userpage ? "Create dungeons for them to appear here!" : "This user has not created any dungeons yet.";
                Avatar.ImageUrl = userpage.profilePicture;
                UsernameLabel.Text = userpage.username;
                AvatarUploader.Attributes.Add("accept", ".jpg,.png");
                DataTable dataTable;
                DataRow row;
                if (user == userpage || user.IsAdmin())
                {
                    dataTable = new DataTable();
                    dataTable.Columns.Add("Username");
                    dataTable.Columns.Add("Password");
                    dataTable.Columns.Add("Email");
                    dataTable.Columns.Add("Date");
                    dataTable.Columns.Add("CreditsText");
                    row = dataTable.NewRow();
                    row["Username"] = userpage.username;
                    row["Password"] = userpage.GetRedactedPassword();
                    row["Email"] = userpage.email;
                    row["Date"] = userpage.creationDate.ToShortDateString();
                    row["CreditsText"] = AchievementService.UserCreditsTotal(userpage).ToString();
                    dataTable.Rows.Add(row);
                    UserGridView.DataSource = dataTable;
                    UserGridView.DataBind();
                    UserGridView.Visible = true;
                    ((Site)Master).CoinVisible = false;
                }
                else StatsGridView.Style["bottom"] = "40%";
                if (user.email == null && userpage.email == null) Response.Redirect("Register.aspx");
                dataTable = new DataTable();
                dataTable.Columns.Add("Maps Created");
                dataTable.Columns.Add("Games Played");
                dataTable.Columns.Add("Achievements");
                dataTable.Columns.Add("Stars Collected");
                dataTable.Columns.Add("Deaths");
                dataTable.Columns.Add("Total Time Played");
                dataTable.Columns.Add("Since Joined");
                row = dataTable.NewRow();
                row["Maps Created"] = "x" + GeneralService.GetStringByQuery("SELECT COUNT(mapID) FROM Maps WHERE creator = '" + userpage.email + "'");
                row["Games Played"] = "x" + GeneralService.GetStringByQuery("SELECT COUNT(gameID) FROM Games WHERE player = '" + userpage.email + "'");
                row["Achievements"] = "x" + GeneralService.GetStringByQuery("SELECT COUNT(achievement) FROM UserAchievements WHERE awardee = '" + userpage.email + "'");
                row["Stars Collected"] = "x" + SumGamesField("starsCollected");
                row["Deaths"] = "x" + SumGamesField("deathCount");
                row["Total Time Played"] = Connect.SecToMin(SumGamesField("timeElapsed"));
                row["Since Joined"] = GeneralService.GetStringByQuery("SELECT DATEDIFF('d', creationDate, Date()) FROM Users WHERE email = '" + userpage.email + "'") + " days";
                dataTable.Rows.Add(row);
                StatsGridView.DataSource = dataTable;
                StatsGridView.DataBind();
            }
            if (user == userpage || user.IsAdmin())
            { //Rewritten with each postback for AJAX dynamic text changes
                UserGridView.Rows[0].Cells[0].Text = userpage.username;
                UserGridView.Rows[0].Cells[1].Text = userpage.GetRedactedPassword();
                UsernameLabel.Text = userpage.username;
            }
        }
        private int SumGamesField(string field) 
        {
            try { return int.Parse(GeneralService.GetStringByQuery("SELECT SUM(" + field + ") FROM Games WHERE player = '" + userpage.email + "'")); }
            catch { return 0; }
        }
        protected void MapsDataList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            Map map = new Map(int.Parse(((Label)e.Item.FindControl("mapID")).Text));
            if (e.CommandName == "PlayButton")
            {
                Session["map"] = map;
                Response.Redirect("Play.aspx");
            }
            if (e.CommandName == "PrivacyButton")
            {
                Button btn = (Button)e.Item.FindControl("PrivacyButton");
                btn.Text = (btn.Text == "Public") ? "Private" : "Public";
                btn.BackColor = (btn.Text == "Public") ? ColorTranslator.FromHtml("#009900") : ColorTranslator.FromHtml("#990000");
                UserService.ChangePrivacy(map);
            }
            if (e.CommandName == "DeleteButton")
            {
                if (PlayService.countGames(map.mapID) == 0)
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
            if(e.CommandName == "RenameButton") 
            {
                TextBox tb = (TextBox)e.Item.FindControl("RenameTextBox");
                Label title = (Label)e.Item.FindControl("Title");
                if (!tb.Visible)
                {
                    tb.Visible = true;
                    title.Visible = false;
                }
                else 
                {
                    tb.Visible = false;
                    title.Text = tb.Text;
                    int countMaps = MapService.CountMapsWithName(tb.Text);
                    MapService.ChangeMapName(map.mapID, tb.Text + countMaps);
                    File.Move(Server.MapPath(map.thumbnail), Server.MapPath("assets/screenshots/" + tb.Text + countMaps + ".jpg"));
                    title.Visible = true;
                }
            }
        }
        protected void MapsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (user == null) user = (User)Session["user"];
                if (userpage == null) userpage = (User)Session["userPage"];
                Label date = (Label)e.Item.FindControl("CreationDate");
                date.Text = date.Text.Remove(date.Text.IndexOf(' '));
                bool isPublic = (bool)DataBinder.Eval(e.Item.DataItem, "isPublic");
                if (user.email == userpage.email || user.IsAdmin())
                { //Initializes privacyButton if this is the user's userpage
                    Button btn = (Button)e.Item.FindControl("PrivacyButton");
                    btn.Text = isPublic ? "Public" : "Private";
                    btn.BackColor = isPublic ? ColorTranslator.FromHtml("#009900") : ColorTranslator.FromHtml("#990000");
                    btn.Visible = true;
                    ((Button)e.Item.FindControl("RenameButton")).Visible = true;
                    int mapID = int.Parse(((Label)e.Item.FindControl("mapID")).Text);
                    Button btn2 = (Button)e.Item.FindControl("DeleteButton");
                    if (IsExist(mapID))
                    { //If map exists in Games table, delete button repurposes to an enable/disable button
                        Map map = new Map(mapID);
                        btn2.Text = map.isValid ? "Disable" : "Enable";
                        ((Button)e.Item.FindControl("PlayButton")).Enabled = map.isValid;
                    }
                    btn2.Visible = true;
                }
                else if (!isPublic) e.Item.Enabled = false;
                string title = ((Label)e.Item.FindControl("Title")).Text;
                ((Label)e.Item.FindControl("Title")).Text = title.Remove(title.Length - 1);
            }
        }
        private bool IsExist(int mapID)
        {
            foreach (Map map in (List<Map>)Cache["playedMaps"])
                if (map.mapID == mapID)
                    return true;
            return false;
        }
        [WebMethod]
        public static void AvatarUpload() 
        { //AJAX call
            Userpage userpageInstance = new Userpage();
            userpageInstance.Avatar_Click();
        }
        private void Avatar_Click()
        { //Updates user avatar in database, current userpage and masterpage (if applicable)
            FileInfo prev = new FileInfo(Server.MapPath(userpage.profilePicture));
            try { prev.Delete(); }
            catch { ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Deletion error, likely due to physical path.');", true); }
            string fileName = Path.GetFileName(AvatarUploader.PostedFile.FileName),
                fileType = AvatarUploader.PostedFile.ContentType,
                filePath = Server.MapPath("~/assets/profiles/") + fileName;
            if (AvatarUploader.PostedFile.ContentLength < 500000)
            {
                AvatarUploader.SaveAs(filePath);
                UserService.ChangeProfilePic("assets/profiles/" + fileName, userpage);
            }
            Avatar.ImageUrl = filePath;
            if (Master is Site && user.email == userpage.email) 
                ((Site)Master).ImageUrl = filePath;
        }
        [WebMethod]
        public static void TextChanged(string column, string newValue)
        { //AJAX call
            Userpage userpageInstance = new Userpage();
            userpageInstance.ChangeField(int.Parse(column), newValue);
        }
        private void ChangeField(int column, string value)
        { //Reflects GridView changes to database and Session
            User userpage = (User)Session["userPage"];
            if (column == 0 && Regex.IsMatch(value, "^[a-zA-Z0-9]*$") && !UserService.FieldExists("username", value))
            {
                UserService.UpdateFieldByEmail("username", value, userpage);
                ((User)Session["userPage"]).username = value;
            }
            else ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Username change failed. It must only consist of (latin) letters and numbers. If not applicable, this username may already exist.');", true);
            if (column == 1 && value.Length > 3 && value.Length < 13) 
            { 
                UserService.UpdateFieldByEmail("userPassword", value, userpage); 
                ((User)Session["userPage"]).userPassword = value; 
            }
            else ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Password change failed. It must be between 3 and 13 characters.');", true);
        }
    }
}