using DungeonMaker.Classes.Services;
using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DungeonMaker.localhost;
using CreditCardWebService;
using System.Data;

namespace DungeonMaker
{
    public partial class Payment : System.Web.UI.Page
    {
        private User user;
        private Card card;
        private CreditCardService CS;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (user == null) user = (User)Session["user"];
            if (!IsPostBack)
            {
                CS = new CreditCardService();
                Session["service"] = CS;
                card = CS.GetCardByEmail(user.email);
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
                if (card != null)
                { //If customer is known, autofill fields
                    FirstNameTextBox.Text = card.holder.firstName;
                    LastNameTextBox.Text = card.holder.lastName;
                    PhoneNumTextBox.Text = card.holder.phoneNumber;
                    AddressTextBox.Text = card.holder.billingaddress;
                    CreditcardTextBox.Text = card.number;
                    CVVTextBox.Text = card.CVV.ToString();
                    ExpDateTextBox.Text = card.validity.ToString("yyyy-MM");
                    CheckProvider(long.Parse(card.number[0].ToString()));
                    CheckCountry(card.holder.phoneNumber.Substring(1, 5));
                }
            }
            if (card == null) card = CS.GetCardByEmail(user.email);
            if (CS == null) CS = (CreditCardService)Session["service"];
        }
        private void CheckProvider(long num)
        { //Sets the credit card provider icon accordingly
            switch (num)
            {
                case 3: CardProviderIcon.ImageUrl = "assets/ui/AmericanExpress.png"; break;
                case 4: CardProviderIcon.ImageUrl = "assets/ui/Visa.png"; break;
                case 5: CardProviderIcon.ImageUrl = "assets/ui/Mastercard.png"; break;
            }
        }
        protected void BackButton_Click(object sender, EventArgs e) { Response.Redirect("Store.aspx"); }

        protected void CreditcardTextBox_TextChanged(object sender, EventArgs e)
        { //Performs checks on card number inputted
            CardProviderIcon.ImageUrl = "assets/flags/default.png";
            string card = CreditcardTextBox.Text;
            long num = -1;
            try 
            {
                num = long.Parse(card);
                num = long.Parse(card[0].ToString()); 
            }
            catch { EnableButtonState(true, "Credit card number is invalid."); return; }
            if (card.Length < 15) { EnableButtonState(true, "Credit card number length is incorrect."); return; }
            CheckProvider(num);
            EnableButtonState(false, "");
        }
        private void EnableButtonState(bool isDisabled, string error) 
        { //Toggles submit button hover style
            if (isDisabled) ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('" + error + "');", true);
            string isDisabledString = isDisabled.ToString().ToLower();
            ScriptManager.RegisterStartupScript(this, GetType(), "State", "toggleButtonHoverEffect(" + isDisabledString + ");", true); 
        }
        protected void PhoneNumTextBox_TextChanged(object sender, EventArgs e)
        { //Performs checks on phone inputted 
            FlagIcon.ImageUrl = "assets/flags/default.png";
            string num = PhoneNumTextBox.Text;
            if (num.Length == 0) return;
            if (num[0] != '+') { EnableButtonState(true, "Please enter country dial code (e.g. +1 for the US and Canada)."); return; }
            EnableButtonState(false, "");
            CheckCountry(num.Substring(1, 5));
        }
        private string[] imagePaths = null;
        private void CheckCountry(string num) 
        { //Recursive method checking for a country based input dial code
            if (imagePaths == null) imagePaths = Directory.GetFiles(Server.MapPath("assets/flags"), "*.png");
            foreach (string imagePath in imagePaths)
            {
                string imageName = Path.GetFileNameWithoutExtension(imagePath);
                if (imageName.Equals(num, StringComparison.OrdinalIgnoreCase)) 
                {
                    FlagIcon.ImageUrl = "assets/flags/" + imageName + ".png"; 
                    return;
                }
            }
            if (num.Length > 1) CheckCountry(num.Remove(num.Length - 1));
        }
        protected void PurchaseButton_Click(object sender, EventArgs e)
        { //Attempts to purchase selected product
            foreach (Control control in form1.Controls) 
                if (control is TextBox TB) 
                    if (string.IsNullOrEmpty(TB.Text)) 
                        { EnableButtonState(true, "A required field is empty."); return; }
            StoreService SS = new StoreService();
            //IF USER DOES NOT EXIST (FIELDS NOT AUTO-FILLED), ADD THEM NOW
            bool f = true; //CHECK IF OVER CREDIT CARD LIMIT FROM WEB SERVICE DB, IF TRANSACTION DENIED (ENOUGH BALANCE)
            if (f) StoreService.PurchaseCredits(user, GeneralService.GetStringByQuery(
                "SELECT bundleName FROM CreditBundles WHERE cost = " + Convert.ToInt32(Session["price"])), CreditcardTextBox.Text);
            //ADD TRANSACTION SUMMARY TO WEB SERVICE DB AS LOG
            Response.Redirect("Store.aspx");
        }
    }
}