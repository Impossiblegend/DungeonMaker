using DungeonMaker.Classes.Services;
using DungeonMaker.Classes.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DungeonMaker
{
    public partial class Adminpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string query = "SELECT Feedback.*, Users.username, Users.profilePicture FROM Users INNER JOIN Feedback ON Feedback.sender = Users.email";
                FeedbackDataList.DataSource = GeneralService.GetDataSetByQuery(query, "Feedback");
                FeedbackDataList.DataBind();
                AchievementsDataList.DataSource = GeneralService.GetDataSetByQuery("SELECT * FROM Achievements", "Achievements");
                AchievementsDataList.DataBind();
                CommentService CS = new CommentService();
                //Cache["featured"] = new List<Comment>();
            }
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
        }
        protected void FeedbackDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label body = (Label)e.Item.FindControl("Feedback");
                if (body.Text.Length > 100) body.Text = "<span class='expandable-text' onclick='expandText(this)'>" +  body.Text.Remove(100) + "..." + "</span>" +
                    "<span class='full-text' style='display:none;'>" + body.Text + "</span>";
            }
        }
        protected void AchievementsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.CssClass = "maps-template animated-item";
                ImageButton btn = (ImageButton)e.Item.FindControl("LockButton");
                if ((bool)DataBinder.Eval(e.Item.DataItem, "isValid")) btn.ImageUrl += "un";
                btn.ImageUrl += "lock.png";
                ((Label)e.Item.FindControl("Credits")).Text = "x" + ((Label)e.Item.FindControl("Credits")).Text;
            }
        }
        protected void AchievementsDataList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            switch (e.CommandName) 
            {
                case "LockButton_Click":
                    AchievementService.ChangeValid(((Label)e.Item.FindControl("Title")).Text);
                    ImageButton btn = (ImageButton)e.Item.FindControl("LockButton");
                    btn.ImageUrl = "assets/ui/" + (btn.ImageUrl == "assets/ui/lock.png" ? "unlock.png" : "lock.png");
                    break;
                case "EditButton_Click":
                    Label lbl = (Label)e.Item.FindControl("DescriptionBody"); 
                    TextBox tb = (TextBox)e.Item.FindControl("EditTextBox");
                    AchievementService AS = new AchievementService();
                    if (tb.Visible)
                    {
                        AchievementService.ChangeDescription(((Label)e.Item.FindControl("Title")).Text, tb.Text);
                        lbl.Text = tb.Text;
                    }
                    lbl.Visible = !lbl.Visible;
                    tb.Visible = !tb.Visible;
                    break;
            }
        }

        protected void IsFeaturedCB_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox CB = (CheckBox)sender;
            DataListItem item = (DataListItem)CB.NamingContainer;
            Label feedbackID = (Label)item.FindControl("feedbackID");
            CommentService.ChangeFeatured(int.Parse(feedbackID.Text));
        }
        //CommentService.ChangeFeatured(int.Parse(((Label)((DataListItem)((CheckBox)sender).NamingContainer).FindControl("feedbackID")).Text));
    }
}