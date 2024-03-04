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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
            }
            if (user == null) user = (User)Session["user"];
        }
        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            if (!user.IsBanned() && sender is Button btn)
            {
                Session["price"] = Convert.ToDouble(btn.CommandArgument);
                char type = Convert.ToChar(btn.CommandName);
                if (type == '$') Response.Redirect("Payment.aspx");
                int credits = Convert.ToInt32(Session["price"]);
                if (((Site)Master).UserCredits >= credits) 
                {
                    ((Site)Master).UserCredits -= credits;
                    btn.Enabled = false;
                    btn.CssClass = "Disabled";
                    btn.Text = "Owned";
                }
            }
            else Response.Redirect("Login.aspx");
        }
    }
}