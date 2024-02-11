using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DungeonMaker.classes.Services;
using System.Web.Services;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using DungeonMaker.classes.Types;

namespace DungeonMaker
{
    public partial class Userpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User user = (User)Session["userPage"];
                //if (Session["userPage"] == null) user = new User(); user.email = "admin@gmail.com";
                string query = "SELECT mapID, mapName, Maps.creationDate, thumbnail, isPublic FROM Maps WHERE creator = '" + user.email + "' ORDER BY mapID DESC";
                Session["ds"] = ProductService.GetProductsByQuery(query, "Maps");
                MapsDataList.DataSource = (DataSet)Session["ds"];
                Session["edit"] = false;
                MapsDataList.DataBind();
                if (MapsDataList.Items.Count == 0)
                    EmptyLabel.Text = (User)Session["user"] == user ? "Create dungeons for them to appear here!" : "This user has not created any dungeons yet.";
                UserService US = new UserService();
                Avatar.ImageUrl = user.profilePicture;
                UsernameLabel.Text = user.username;
                AvatarUploader.Attributes.Add("accept", ".jpg,.png");
                PlayService PS = new PlayService();
                if ((User)Session["user"] == user || ((User)Session["user"]).elevation == 2)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Username");
                    dataTable.Columns.Add("Password");
                    dataTable.Columns.Add("Email");
                    dataTable.Columns.Add("Date");
                    DataRow row = dataTable.NewRow();
                    row["Username"] = user.username;
                    row["Password"] = user.GetRedactedPassword();
                    row["Email"] = user.email;
                    row["Date"] = user.creationDate.ToShortDateString();
                    dataTable.Rows.Add(row);
                    UserGridView.DataSource = dataTable;
                    Session["gridTable"] = dataTable;
                    UserGridView.DataBind();
                    UserGridView.Visible = true;
                }
            }
            if (((User)Session["user"]).email == null && ((User)Session["userPage"]).email == null)
                Response.Redirect("Register.aspx");
        }
        protected void MapsDataList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            Session["mapID"] = ((Label)e.Item.FindControl("mapID")).Text;
            DataTable dt = ((DataSet)Session["ds"]).Tables[0];
            Map map = new Map(Convert.ToInt32(Session["mapID"]));
            if (e.CommandName == "PlayButton")
            {
                Session["map"] = new Map(int.Parse(((Label)e.Item.FindControl("mapID")).Text));
                Response.Redirect("Play.aspx");
            }
            if (e.CommandName == "PrivacyButton")
            {
                Button btn = (Button)e.Item.FindControl("PrivacyButton");
                btn.Text = (btn.Text == "Public") ? "Private" : "Public";
                btn.BackColor = (btn.Text == "Public") ? ColorTranslator.FromHtml("#009900") : ColorTranslator.FromHtml("#990000");
                UserService.ChangePrivacy(map.mapID);
            }
            if (e.CommandName == "DeleteButton")
            {
                if (!PlayService.wasMapPlayed(map.mapID))
                {
                    map.Delete();
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
                Label date = (Label)e.Item.FindControl("CreationDate");
                date.Text = date.Text.Remove(date.Text.IndexOf(' '));
                bool isPublic = (bool)DataBinder.Eval(e.Item.DataItem, "isPublic");
                if (((User)Session["user"]).email == ((User)Session["userPage"]).email || ((User)Session["user"]).elevation == 2)
                { //Initializes privacyButton if this is the user's userpage
                    Button btn = (Button)e.Item.FindControl("PrivacyButton");
                    btn.Text = isPublic ? "Public" : "Private";
                    btn.BackColor = isPublic ? ColorTranslator.FromHtml("#009900") : ColorTranslator.FromHtml("#990000");
                    btn.Visible = true;
                    ((Button)e.Item.FindControl("RenameButton")).Visible = true;
                    Map map = new Map(int.Parse(((Label)e.Item.FindControl("mapID")).Text));
                    Button btn2 = (Button)e.Item.FindControl("DeleteButton");
                    if (PlayService.wasMapPlayed(map.mapID))
                    { //If map exists in Games table, enable/disable button instead of delete button
                        if (map.isValid)
                        {
                            btn2.Text = "Disable";
                            ((Button)e.Item.FindControl("PlayButton")).Enabled = true;
                        }
                        else
                        {
                            btn2.Text = "Enable";
                            ((Button)e.Item.FindControl("PlayButton")).Enabled = false;
                        }
                    }
                    btn2.Visible = true;
                }
                else if (!isPublic) e.Item.Enabled = false;
                string title = ((Label)e.Item.FindControl("Title")).Text;
                ((Label)e.Item.FindControl("Title")).Text = title.Remove(title.Length - 1);
            }
        }
        [WebMethod]
        public static void AvatarUpload() 
        { //AJAX call
            Userpage userpageInstance = new Userpage();
            userpageInstance.Avatar_Click();
        }
        private void Avatar_Click()
        { //Updates user avatar in database and current sessions
            User userpage = (User)Session["userPage"];
            FileInfo prev = new FileInfo(userpage.profilePicture);
            prev.Delete();
            string fileName = Path.GetFileName(AvatarUploader.PostedFile.FileName),
                fileType = AvatarUploader.PostedFile.ContentType,
                filePath = Server.MapPath("~/assets/profiles/") + fileName;
            if (AvatarUploader.PostedFile.ContentLength < 500000)
            {
                AvatarUploader.SaveAs(filePath);
                UserService.ChangeProfilePic(userpage.email, "assets/profiles/" + fileName);
            }
            Avatar.ImageUrl = filePath;
            if (Master is Site && ((User)Session["user"]).email == userpage.email) 
                ((Site)Master).ImageUrl = filePath;
        }
        [WebMethod]
        public static void TextChanged(string column, string newValue)
        { //AJAX call
            Userpage userpageInstance = new Userpage();
            userpageInstance.ChangeField(int.Parse(column), newValue);
        }
        private void ChangeField(int column, string value)
        { //Reflects GridView changes to database
            string email = ((User)Session["userPage"]).email;
            if (column == 0 && Regex.IsMatch(value, "^[a-zA-Z0-9]*$") && !UserService.FieldExists("username", value)) UserService.UpdateFieldByEmail("username", value, email);
            if (column == 1 && value.Length > 3 && value.Length < 13) UserService.UpdateFieldByEmail("userPassword", value, email);
        }
    }
}