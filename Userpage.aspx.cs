﻿using DungeonMaker.Classes.Types;
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
            if (!IsPostBack)
            {
                if (userpage.elevation == 0) Response.Redirect("Login.aspx");
                string query = "SELECT mapID, mapName, Maps.creationDate, thumbnail, isPublic, estTime FROM Maps WHERE creator = '" + userpage.email + "' ORDER BY mapID DESC";
                ds = GeneralService.GetDataSetByQuery(query, "Maps");
                MapsDataList.DataSource = ds;
                Session["ds"] = ds;
                MapsDataList.DataBind();
                if (MapsDataList.Items.Count == 0) 
                    EmptyLabel.Text = user == userpage ? "Create dungeons for them to appear here!" : "This user has not created any dungeons yet.";
                AvatarImg.ImageUrl = userpage.profilePicture;
                UsernameLabel.Text = userpage.username;
                AvatarUploader.Attributes.Add("accept", ".jpg,.png");
                DataTable dataTable; DataRow row;
                if (user == userpage || user.IsAdmin())
                {
                    dataTable = new DataTable(); dataTable.Columns.Add("Username"); dataTable.Columns.Add("Password");
                    dataTable.Columns.Add("Email"); dataTable.Columns.Add("Date"); dataTable.Columns.Add("CreditsText");
                    row = dataTable.NewRow();
                    row["Username"] = userpage.username;
                    row["Password"] = userpage.GetRedactedPassword();
                    row["Email"] = userpage.email;
                    row["Date"] = userpage.creationDate.ToShortDateString();
                    row["CreditsText"] = userpage.IsAdmin() ? "<p style='font-size:30px;'>&#8734;</p>" : Utility.DecimalCommas(userpage.GetCredits().ToString());
                    dataTable.Rows.Add(row);
                    UserGridView.DataSource = dataTable;
                    UserGridView.DataBind();
                    UserGridView.Visible = true;
                    ((Site)Master).CoinVisible = false;
                }
                else
                {
                    StatsGridView.Style["position"] = "absolute";
                    StatsGridView.Style["bottom"] = "50%";
                    ((Site)Master).yPosition = -43;
                    AvatarUploader.Visible = false;
                    AvatarSubmitButton.Visible = false;
                }
                dataTable = new DataTable(); dataTable.Columns.Add("Maps Created"); dataTable.Columns.Add("Games Played"); dataTable.Columns.Add("Achievements"); 
                dataTable.Columns.Add("Stars Collected"); dataTable.Columns.Add("Deaths"); dataTable.Columns.Add("Total Time Played"); dataTable.Columns.Add("Since Joined");
                row = dataTable.NewRow();
                row["Maps Created"] = "x" + GeneralService.GetStringByQuery("SELECT COUNT(mapID) FROM Maps WHERE creator = '" + userpage.email + "'");
                row["Games Played"] = "x" + GeneralService.GetStringByQuery("SELECT COUNT(gameID) FROM Games WHERE player = '" + userpage.email + "'");
                row["Achievements"] = "x" + GeneralService.GetStringByQuery("SELECT COUNT(achievement) FROM UserAchievements WHERE awardee = '" + userpage.email + "'");
                row["Stars Collected"] = "x" + SumGamesField("starsCollected");
                row["Deaths"] = "x" + SumGamesField("deathCount");
                row["Total Time Played"] = Utility.SecToMin(SumGamesField("timeElapsed"));
                row["Since Joined"] = GeneralService.GetStringByQuery("SELECT DATEDIFF('d', creationDate, Date()) FROM Users WHERE email = '" + userpage.email + "'") + " days";
                dataTable.Rows.Add(row);
                StatsGridView.DataSource = dataTable;
                StatsGridView.DataBind();
            }
            UserService US = new UserService(); PlayService PS = new PlayService(); AchievementService AS = new AchievementService();
            if (user == userpage || user.IsAdmin())
            { //Rewritten with each postback for AJAX dynamic text changes
                UserGridView.Rows[0].Cells[0].Text = userpage.username;
                UserGridView.Rows[0].Cells[1].Text = userpage.GetRedactedPassword();
                UsernameLabel.Text = userpage.username;
            }
        }
        private int SumGamesField(string field) 
        { //Specfic query fromat
            try { return int.Parse(GeneralService.GetStringByQuery("SELECT SUM(" + field + ") FROM Games WHERE player = '" + userpage.email + "'")); }
            catch { return 0; }
        }
        private void Alert(string alert) { ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('" + alert + "');", true); }
        protected void MapsDataList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            Map map = new Map(int.Parse(((Label)e.Item.FindControl("mapID")).Text));
            if (ds == null) ds = (DataSet)Session["ds"];
            DataTable dt = ds.Tables[0];
            switch (e.CommandName) 
            {
                case "PlayButton": //Redirects to play selected map
                    Session["map"] = map;
                    Response.Redirect("Play.aspx");
                    break;
                case "EditButton": //Switches selected map to edit template
                    if (MapsDataList.EditItemIndex == e.Item.ItemIndex) MapService.ChangeTime(map.mapID, Utility.MinToSec(((TextBox)e.Item.FindControl("EstimatedTimeTextBox")).Text));
                    MapsDataList.EditItemIndex = Convert.ToBoolean(e.CommandArgument) ? e.Item.ItemIndex : -1;
                    string query = "SELECT mapID, mapName, Maps.creationDate, thumbnail, isPublic, estTime FROM Maps WHERE creator = '" + userpage.email + "' ORDER BY mapID DESC";
                    MapsDataList.DataSource = GeneralService.GetDataSetByQuery(query, "Maps");
                    MapsDataList.DataBind();
                    break;
                case "PrivacyButton": //Changes privacy state of a map
                    Button btn = (Button)e.Item.FindControl("PrivacyButton");
                    btn.Text = (btn.Text == "Public") ? "Private" : "Public";
                    btn.BackColor = (btn.Text == "Public") ? ColorTranslator.FromHtml("#009900") : ColorTranslator.FromHtml("#990000");
                    UserService.ChangePrivacy(map);
                    break;
                case "DeleteButton":
                    if (PlayService.countGames(map.mapID) == 0)
                    { //Deletes map
                        map.Delete();
                        DataRow rowToDelete = dt.Select("mapID = " + map.mapID).FirstOrDefault();
                        if (rowToDelete != null) dt.Rows.Remove(rowToDelete);
                        MapsDataList.DataSource = dt;
                        MapsDataList.DataBind();
                    }
                    else
                    { /*Disables/enables map*/
                        Button bt = (Button)e.Item.FindControl("DeleteButton");
                        bool isDisabled = bt.Text == "Disable";
                        bt.Text = isDisabled ? "Enable" : "Disable";
                        MapService.ChangeValid(map.mapID);
                    }
                    break;
                case "RenameButton": //Switches rename textbox with map name label
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
                    break;
                case "SubmitButton": //Changes map thumbnail
                    FileUpload fu = (FileUpload)e.Item.FindControl("ThumbnailUploader");
                    if (!fu.HasFile) Alert("Please choose a new thumbnail before submitting it.");
                    else
                    {
                        FileInfo prev = new FileInfo(Server.MapPath(map.thumbnail));
                        try { prev.Delete(); }
                        catch { Alert("Deletion error, likely due to physical path."); }
                        string fileName = Path.GetFileName(fu.PostedFile.FileName),
                            fileType = fu.PostedFile.ContentType,
                            filePath = Server.MapPath("~/assets/screenshots/") + fileName;
                        if (fu.PostedFile.ContentLength < 500000)
                        {
                            try { fu.SaveAs(filePath); }
                            catch { Alert("Upload error."); }
                            MapService.ChangeThumbnail(map.mapID, "assets/screenshots/" + fileName);
                        }
                        else Alert("File size too large.");
                    }
                    break;
            }
        }
        protected void MapsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        { //Predisplay changes needed for maps
            if (user == null) user = (User)Session["user"];
            if (userpage == null) userpage = (User)Session["userPage"];
            bool isPublic = (bool)DataBinder.Eval(e.Item.DataItem, "isPublic");
            if (!isPublic && user.email != userpage.email && !user.IsAdmin()) e.Item.Enabled = false;
            Map map = new Map(int.Parse(((Label)e.Item.FindControl("mapID")).Text));
            Label title = (Label)e.Item.FindControl("Title");
            title.Text = title.Text.Remove(title.Text.Length - 1);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label time = (Label)e.Item.FindControl("EstimatedTime");
                time.Text = Utility.SecToMin(int.Parse(time.Text));
                ((Button)e.Item.FindControl("EditButton")).Visible = user.IsAdmin() || user.email == userpage.email;
                Label date = (Label)e.Item.FindControl("CreationDate");
                date.Text = date.Text.Remove(date.Text.IndexOf(' '));
                ((Button)e.Item.FindControl("PlayButton")).Enabled = map.isValid;
            }
            if (e.Item.ItemType == ListItemType.EditItem) 
            {
                Button btn = (Button)e.Item.FindControl("PrivacyButton");
                btn.Text = isPublic ? "Public" : "Private";
                ((TextBox)e.Item.FindControl("EstimatedTimeTextBox")).Text = Utility.SecToMin(int.Parse(btn.CommandArgument));
                btn.BackColor = isPublic ? ColorTranslator.FromHtml("#009900") : ColorTranslator.FromHtml("#990000");
                if (IsExist(map.mapID))
                {
                    btn = (Button)e.Item.FindControl("DeleteButton");
                    btn.Text = map.isValid ? "Disable" : "Enable";
                    btn.ToolTip = "Disabling still shows your map publicly, but prevents it from being played";
                }
            }
        }
        private bool IsExist(int mapID)
        { //Checks if a given map was played, ever
            foreach (Map map in (List<Map>)Cache["playedMaps"]) 
                if (map.mapID == mapID) 
                    return true;
            return false;
        }

        protected void AvatarSubmitButton_OnClick(object sender, EventArgs e)
        { //Updates user avatar in database, current userpage and masterpage (if applicable)
            if (!AvatarUploader.HasFile) Alert("File not selected - profile picture not updated.");
            FileInfo prev = new FileInfo(Server.MapPath(userpage.profilePicture));
            try { prev.Delete(); }
            catch { Alert("Deletion error, likely due to physical path."); }
            string fileName = Path.GetFileName(AvatarUploader.PostedFile.FileName),
                fileType = AvatarUploader.PostedFile.ContentType,
                filePath = Server.MapPath("~/assets/profiles/") + fileName;
            if (AvatarUploader.PostedFile.ContentLength < 500000)
            {
                AvatarUploader.SaveAs(filePath);
                UserService.ChangeProfilePic("assets/profiles/" + fileName, userpage);
            }
            else Alert("File size too large.");
            AvatarImg.ImageUrl = filePath;
            if (Master is Site master && user.email == userpage.email) master.ImageUrl = filePath;
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
            else Alert("Username change failed. It must only consist of (latin) letters and numbers. If not applicable, this username may already exist.");
            if (column == 1 && value.Length > 3 && value.Length < 13)
            {
                UserService.UpdateFieldByEmail("userPassword", value, userpage);
                ((User)Session["userPage"]).userPassword = value;
            }
            else Alert("Password change failed. It must be between 3 and 13 characters.");
        }
    }
}