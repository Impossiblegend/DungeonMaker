using DungeonMaker.Classes.Services;
using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DungeonMaker
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Styles that differ from Login.aspx - whose stylesheet I reused here
                LastNameLabel.Style["margin-left"] = "75px";
                ExpDateLabel.Style["margin-left"] = "85px";
                foreach (Control control in form1.Controls)
                {
                    if (control is WebControl element)
                    {
                        element.Style["margin-top"] = "0";
                        element.Style["margin-bottom"] = "5px";
                    }
                }
                CardProviderIcon.Style["top"] = "54.5%";
                //IF USER EXISTS IN WEB SERVICE DB, AUTO FILL FIELDS
            }
        }
        protected void BackButton_Click(object sender, EventArgs e) { Response.Redirect("Store.aspx"); }

        protected void CreditcardTextBox_TextChanged(object sender, EventArgs e)
        {
            string card = CreditcardTextBox.Text;
            long num = -1;
            try 
            {
                num = long.Parse(card);
                num = long.Parse(card[0].ToString()); 
            }
            catch { EnableButtonState(true, "Credit card number is invalid."); return; }
            if (card.Length < 15) { EnableButtonState(true, "Credit card number length is incorrect."); return; }
            switch (num)
            {
                case 3: CardProviderIcon.ImageUrl = "assets/ui/AmericanExpress.png"; break;
                case 4: CardProviderIcon.ImageUrl = "assets/ui/Visa.png"; break;
                case 5: CardProviderIcon.ImageUrl = "assets/ui/Mastercard.png"; break;
                default: CardProviderIcon.ImageUrl = "assets/flags/default.png"; break;
            }
            EnableButtonState(false, "");
        }
        private void EnableButtonState(bool isDisabled, string error) 
        {
            if (isDisabled) ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('" + error + "');", true);
            string isDisabledString = isDisabled.ToString().ToLower();
            ScriptManager.RegisterStartupScript(this, GetType(), "State", "toggleButtonHoverEffect(" + isDisabledString + ");", true); 
        }
        protected void PhoneNumTextBox_TextChanged(object sender, EventArgs e)
        {
            ErrorLabel.Text = "";
            FlagIcon.ImageUrl = "assets/flags/default.png";
            string num = PhoneNumTextBox.Text;
            if (num.Length == 0) return;
            if (num[0] != '+') { EnableButtonState(true, "Please enter country dial code (e.g. +1 for the US and Canada)."); return; }
            CheckCountry(num.Substring(1, 5));
        }
        private void CheckCountry(string num) 
        { //Recursive method checking for a country based input dial code
            if (num == "") return;
            EnableButtonState(false, "");
            string[] imageFiles = Directory.GetFiles(Server.MapPath("assets/flags"), "*.png");
            foreach (string imagePath in imageFiles)
            {
                string imageName = Path.GetFileNameWithoutExtension(imagePath);
                if (imageName.Equals(num, StringComparison.OrdinalIgnoreCase)) { FlagIcon.ImageUrl = "assets/flags/" + imageName + ".png"; return; }
            }
            CheckCountry(num.Remove(num.Length - 1));
        }
        protected void PurchaseButton_Click(object sender, EventArgs e)
        {
            foreach (Control control in form1.Controls) 
                if (control is TextBox TB) 
                    if (string.IsNullOrEmpty(TB.Text)) 
                        { EnableButtonState(true, "A required field is empty."); return; }
            StoreService SS = new StoreService();
            //IF USER DOES NOT EXIST (FIELDS NOT AUTO-FILLED), ADD THEM NOW
            bool f = true; //CHECK IF OVER CREDIT CARD LIMIT FROM WEB SERVICE DB, IF TRANSACTION DENIED (ENOUGH BALANCE)
            if (f) StoreService.PurchaseCredits((User)Session["user"], 
                GeneralService.GetStringByQuery("SELECT bundleName FROM CreditBundles WHERE cost = " +
                Convert.ToInt32(Session["price"])), CreditcardTextBox.Text);
            //ADD TRANSACTION SUMMARY TO WEB SERVICE DB AS LOG
            Response.Redirect("Store.aspx");
        }
    }
}