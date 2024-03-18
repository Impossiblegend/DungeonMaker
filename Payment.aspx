<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="DungeonMaker.Payment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="assets/styles/Login.css" rel="stylesheet"/>
    <script src="scripts/jquery-3.7.1.js"></script>
</head>
<body>
    <div>
        <button type="button" id="Back_Button" onclick="back()">BACK</button>
    </div>
    <form id="form1" runat="server" class="form-container">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
            <asp:Label ID="TitleLabel" runat="server" CssClass="title-label">Payment</asp:Label>
            <asp:Label ID="FirstNameLabel" runat="server" CssClass="inline-label">First Name</asp:Label>
            <asp:Label ID="LastNameLabel" runat="server" CssClass="inline-label">Last Name</asp:Label> <br />
            <asp:TextBox ID="FirstNameTextBox" runat="server" CssClass="inline-input"></asp:TextBox>
            <asp:TextBox ID="LastNameTextBox" runat="server" CssClass="inline-input"></asp:TextBox> <br />
            <asp:Label ID="PhoneLabel" runat="server" CssClass="input-label">Phone Number</asp:Label>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Image ID="FlagIcon" runat="server" ImageUrl="assets/flags/default.png" CssClass="flag-icon" />
                    <asp:TextBox ID="PhoneNumTextBox" runat="server" TextMode="Phone" CssClass="input-field" OnTextChanged="PhoneNumTextBox_TextChanged" AutoPostBack="true" MaxLength="17"></asp:TextBox>
                    <asp:Label ID="CreditcardLabel" runat="server" CssClass="input-label">Credit Card</asp:Label>
                    <asp:Image ID="CardProviderIcon" runat="server" ImageUrl="assets/flags/default.png" CssClass="flag-icon" Width="28px" Height="20px" />
                    <asp:TextBox ID="CreditcardTextBox" runat="server" CssClass="input-field" OnTextChanged="CreditcardTextBox_TextChanged" TextMode="SingleLine" MaxLength="16" AutoPostBack="true"></asp:TextBox> <br />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="PhoneNumTextBox" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="CreditcardTextBox" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:Label ID="CVVLabel" runat="server" CssClass="inline-label">CVV/CVC</asp:Label>
            <asp:Label ID="ExpDateLabel" runat="server" CssClass="inline-label">Date of Exp.</asp:Label> <br />
            <asp:TextBox ID="CVVTextBox" runat="server" CssClass="inline-input" TextMode="SingleLine" MaxLength="3"></asp:TextBox>
            <asp:TextBox ID="ExpDateTextBox" runat="server" TextMode="Month" CssClass="inline-input"></asp:TextBox> <br />
            <asp:Label ID="AddressLabel" runat="server" CssClass="input-label">Billing Address</asp:Label>
            <asp:TextBox ID="AddressTextBox" runat="server" CssClass="input-field"></asp:TextBox> <br />
            <asp:Label ID="ErrorLabel" runat="server" CssClass="error-message"></asp:Label>
            <asp:Button ID="PurchaseButton" runat="server" OnClick="PurchaseButton_Click" Text="Purchase" CssClass="submit-button" />
        </div>
    </form>
    <script>
    function back() { window.location.href = 'Store.aspx'; }
    function toggleButtonHoverEffect(isDisabled) {
        var signUpButton = document.getElementById("PurchaseButton");
        if (isDisabled) signUpButton.classList.add('disabled-hover');
        else signUpButton.classList.remove('disabled-hover');
    }
    document.addEventListener("DOMContentLoaded", function () {
        var form = document.getElementById('form1');
        if (form) {
            form.style.height = "600px";
        }
    });
    </script>
</body>
</html>