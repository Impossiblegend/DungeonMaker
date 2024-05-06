using DungeonMaker.Classes.Services;
using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DungeonMaker
{
    public partial class MapSelection : System.Web.UI.Page
    {
        private User user;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (user == null) user = (User)Session["user"];
            if (!IsPostBack) 
            {
                MapTypeDataList.DataSource = GeneralService.GetDataSetByQuery("SELECT type, asset, cost FROM Products WHERE class = 'map'", "Products");
                MapTypeDataList.DataBind();
            }
        }

        protected void MapTypeDataList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!Convert.ToBoolean(((ImageButton)e.Item.FindControl("Thumbnail")).CommandArgument)) Response.Redirect("Store.aspx");
            Label cost = (Label)e.Item.FindControl("Cost");
            if (user.GetCredits() >= int.Parse(cost.Text.Remove(cost.Text.IndexOf(' '))) || user.IsAdmin()) 
            {
                Session["mapType"] = ((Label)e.Item.FindControl("MapTypeName")).Text;
                Response.Redirect("Create.aspx");
            }
            else ScriptManager.RegisterStartupScript(this, GetType(), "animate", "animateCostLabel('" + MapTypeDataList.ClientID + "', '" + ((Label)e.Item.FindControl("Cost")).ClientID + "');", true);
        }
        protected void MapTypeDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                StoreService SS = new StoreService();
                bool isOwned = StoreService.IsPurchased(user, ((Label)e.Item.FindControl("MapTypeName")).Text);
                ((ImageButton)e.Item.FindControl("Thumbnail")).CommandArgument = isOwned.ToString();
                ((Image)e.Item.FindControl("LockImage")).Visible = !isOwned;
                Label cost = (Label)e.Item.FindControl("Cost");
                cost.Text = (int.Parse(cost.Text) / 100 + 15) + " CREDITS";
            }
        }
    }
}