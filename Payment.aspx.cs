﻿using DungeonMaker.Classes.Services;
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
                card = CS.GetCardByHolder(user.email);
                //Styles that differ from Login.aspx - whose stylesheet I reused here
                LastNameLabel.Style["margin-left"] = "75px";
                ExpDateLabel.Style["margin-left"] = "85px";
                foreach (Control control in form1.Controls)
                {
                    if (control is WebControl element)
                    {
                        element.Style["margin-top"] = "0";
                        element.Style["margin-bottom"] = "2px";
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
                    CardProviderIcon.ImageUrl = GetProvider(long.Parse(card.number[0].ToString()));
                    CheckCountry(card.holder.phoneNumber.Substring(1, 5));
                }
            }
            if (CS == null) CS = (CreditCardService)Session["service"];
            if (card == null) card = CS.GetCardByHolder(user.email);
        }
        private string GetProvider(long num)
        { //Sets the credit card provider icon accordingly
            switch (num)
            {
                case 3: return "assets/ui/AmericanExpress.png";
                case 4: return "assets/ui/Visa.png";
                case 5: return "assets/ui/Mastercard.png";
                default: return "assets/flags/default.png";
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
            CardProviderIcon.ImageUrl = GetProvider(num);
            EnableButtonState(false, "");
        }
        private void EnableButtonState(bool isDisabled, string error) 
        { //Toggles submit button hover style
            if (isDisabled) Alert(error);
            string isDisabledString = isDisabled.ToString().ToLower();
            ScriptManager.RegisterStartupScript(this, GetType(), "State", "toggleButtonHoverEffect(" + isDisabledString + ");", true); 
        }
        private void Alert(string alert) { ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('" + alert + "');", true); }
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
            Holder holder = new Holder(user.email, FirstNameTextBox.Text, LastNameTextBox.Text, default(DateTime), PhoneNumTextBox.Text, AddressTextBox.Text);
            if (this.card.holder != null) if (!holder.Equals(this.card.holder)) { EnableButtonState(true, "Mismatch in one or more required criteria."); return; }
            Card card;
            try { card = new Card(CreditcardTextBox.Text, holder, GetProvider(long.Parse(CreditcardTextBox.Text[0].ToString())), int.Parse(CVVTextBox.Text), DateTime.Parse(ExpDateTextBox.Text)); }
            catch { EnableButtonState(true, "Mismatch in one or more required criteria."); return; }
            if (!card.Equals(this.card)) CS.NewCard(card, this.card.holder != null, RememberMeCheckBox.Checked);
            bool f = true; //CHECK IF OVER CREDIT CARD LIMIT FROM WEB SERVICE DB, IF TRANSACTION DENIED (ENOUGH BALANCE)
            if (f) StoreService.PurchaseCredits(user, GeneralService.GetStringByQuery(
                "SELECT bundleName FROM CreditBundles WHERE cost = " + Convert.ToInt32(Session["price"])), CreditcardTextBox.Text);
            Alert(CS.TransactionSuccess(card.number, Convert.ToDouble(Session["price"])) ? "Purchase success!" : "Failure. Check card limit and balance.");
        }
    }
}