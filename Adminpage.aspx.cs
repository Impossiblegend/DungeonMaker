using DungeonMaker.classes.Services;
using DungeonMaker.classes.Types;
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
                FeedbackDataList.DataSource = ProductService.GetDataSetByQuery(query, "Feedback");
                FeedbackDataList.DataBind();
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
                    starsPlaceHolder.Controls.Add(imgStar);
                }
                if (comment.isFeatured) item.BackColor = System.Drawing.Color.LightGreen;
                else item.BackColor = System.Drawing.Color.Transparent;
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
        protected void FeedbackDataList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            Comment comment = new Comment(int.Parse(((Label)e.Item.FindControl("feedbackID")).Text));
            if (e.CommandName == "Checkmark_Click") 
            {
                comment.isFeatured = true;
                e.Item.BackColor = System.Drawing.Color.LightGreen;
            }
            if (e.CommandName == "Cross_Click")
            {
                comment.isFeatured = false;
                e.Item.BackColor = System.Drawing.Color.Transparent;
            }
            CommentService.ChangeFeatured(comment);
            //ScriptManager.RegisterStartupScript(this, GetType(), "TriggerPostBack", "__doPostBack('', '');", true);
        }
    }
}