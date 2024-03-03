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
            if (user == null) user = (User)Session["user"];
        }
        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            if (!user.IsBanned() && sender is Button btn)
            {
                int price = Convert.ToInt32(btn.CommandArgument);
                Session["price"] = price;
                Response.Redirect("Payment.aspx");
            }
            else Response.Redirect("Login.aspx");
        }
    }
}