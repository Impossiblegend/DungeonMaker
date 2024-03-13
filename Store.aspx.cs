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
                dlCredits.DataSource = GeneralService.GetDataSetByQuery("SELECT * FROM CreditBundles", "CreditBundles");
                dlCredits.DataBind();
                SetClass(dlMapTypes, "map");
                SetClass(dlTrapTypes, "trap");
                SetClass(dlSkins, "skin");
            }
        }

        private void SetClass(DataList DL, string className)
        {
            DL.DataSource = GeneralService.GetDataSetByQuery("SELECT * FROM Products WHERE class = '" + className + "' ORDER BY type", "Products");
            DL.DataBind();
        }

        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            if (!user.IsBanned() && sender is Button btn)
            {
                Session["price"] = btn.CommandArgument;
                if (btn.CommandName == "Credits") Response.Redirect("Payment.aspx");
                int credits = Convert.ToInt32(Session["price"]);
                if (user.GetCredits() < credits) ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('You have insufficient credits for this item.');", true);
                else
                {
                    SS = new StoreService();
                    StoreService.Purchase(user, GeneralService.GetStringByQuery("SELECT type FROM Products WHERE cost = " + credits));
                    ((Site)Master).UserCredits = Calculations.DecimalCommas(user.GetCredits().ToString());
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
            Label cost = (Label)e.Item.FindControl("costLabel");
            if (user.elevation > 0)
            {
                SS = new StoreService();
                if (StoreService.IsPurchased(user, ((Label)e.Item.FindControl("lblMapType")).Text)) 
                    DisableButton((Button)e.Item.FindControl("btnMapPurchase"));
            }
            if (cost.Text == "0") cost.Text = "FREE";
            else cost.Text += " CREDITS";
        }

        protected void dlTrapTypes_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label cost = (Label)e.Item.FindControl("costLabel");
            if (user.elevation > 0)
            {
                SS = new StoreService();
                if (StoreService.IsPurchased(user, ((Label)e.Item.FindControl("lblTrapType")).Text))
                    DisableButton((Button)e.Item.FindControl("btnTrapPurchase"));
            }
            if (cost.Text == "0") cost.Text = "FREE";
            else cost.Text += " CREDITS";
        }

        protected void dlSkins_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label cost = (Label)e.Item.FindControl("costLabel");
            if (user.elevation > 0)
            {
                SS = new StoreService();
                if (StoreService.IsPurchased(user, ((Label)e.Item.FindControl("lblSkin")).Text))
                    DisableButton((Button)e.Item.FindControl("btnSkinPurchase"));
            }
            if (cost.Text == "0") cost.Text = "FREE";
            else cost.Text += " CREDITS";
        }

        protected void dlCredits_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label cost = (Label)e.Item.FindControl("costLabel"), credits = (Label)e.Item.FindControl("creditAmount");
            cost.Text = "$" + (int.Parse(cost.Text) - 0.01);
            credits.Text = Calculations.DecimalCommas(credits.Text) + " CREDITS";
        }
    }
}