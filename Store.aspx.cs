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
    public partial class Store : System.Web.UI.Page
    {
        private User user;
        private StoreService SS;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (user == null) user = (User)Session["user"];
            if (!IsPostBack) 
            {
                dlMapTypes.DataSource = GeneralService.GetDataSetByQuery("SELECT * FROM MapTypes", "MapTypes");
                dlMapTypes.DataBind();
                dlTrapTypes.DataSource = GeneralService.GetDataSetByQuery("SELECT * FROM TrapTypes WHERE trapType <> 'portalFull' AND trapType <> 'portalEmpty'", "TrapTypes");
                dlTrapTypes.DataBind();
            }
        }
        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            if (!user.IsBanned() && sender is Button btn)
            {
                Session["price"] = btn.CommandArgument;
                if (btn.CommandName == "Credits") Response.Redirect("Payment.aspx");
                int credits = Convert.ToInt32(Session["price"]);
                if (((Site)Master).UserCredits < credits) ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('You have insufficient credits for this item.');", true);
                else
                {
                    SS = new StoreService();
                    if (btn.CommandName == "Map") StoreService.PurchaseMapType(user, GeneralService.GetStringByQuery("SELECT mapType FROM MapTypes WHERE cost = " + credits));
                    if (btn.CommandName == "Trap") StoreService.PurchaseTrapType(user, GeneralService.GetStringByQuery("SELECT trapType FROM TrapTypes WHERE cost = " + credits));
                    ((Site)Master).UserCredits -= credits;
                    DisableButton(btn);
                }
            }
            else Response.Redirect("Login.aspx");
        }

        private void DisableButton(Button btn)
        {
            btn.Enabled = false;
            btn.CssClass = "Disabled";
            btn.Text = "Owned";
        }

        protected void dlMapTypes_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (user.elevation > 0)
            {
                SS = new StoreService();
                if (StoreService.IsMapPurchased(user, ((Label)e.Item.FindControl("lblMapType")).Text))
                    DisableButton((Button)e.Item.FindControl("btnMapPurchase"));
            }
        }
        protected void dlTrapTypes_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (user.elevation > 0)
            {
                SS = new StoreService();
                if (StoreService.IsTrapPurchased(user, ((Label)e.Item.FindControl("lblTrapType")).Text))
                    DisableButton((Button)e.Item.FindControl("btnTrapPurchase"));
            }
        }
    }
}