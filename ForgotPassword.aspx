<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="DungeonMaker.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="assets/styles/ForgotPassword.css" />
</head>
<body>

    <form id="form1" runat="server">
        <asp:MultiView ID="FPMultiView" runat="server">
            <asp:View ID="EmailConfirmView" runat="server">
                <asp:Label ID="Title_Label" class="container" runat="server" Text="Forgot Password"></asp:Label>
                <asp:Label ID="UserName_Label" class="container" runat="server" Text="Username"></asp:Label>
                <asp:TextBox ID="UserName_TextBox" class="container" runat="server"></asp:TextBox>
                <asp:Label ID="Email_Label" class="container" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="Email_TextBox" class="container" runat="server"></asp:TextBox> <br />
                <asp:Button ID="CheckAccount_Button" class="container" runat="server" OnClick="CheckAccount_Button_Click" Text="Check Account" />
            </asp:View>
            <asp:View ID="ChangePasswordView" runat="server">
                <asp:Label ID="ChangePassword_Label" class="container" runat="server" Text="Enter a new password" ></asp:Label>
                <asp:TextBox ID="ChangePassword_TextBox" class="container" runat="server" ></asp:TextBox>
                <asp:Label ID="ConfirmPassword_Label" class="container" runat="server" Text="Confirm new password" ></asp:Label>
                <asp:TextBox ID="ConfirmPassword_TextBox" class="container" runat="server" ></asp:TextBox> <br /> <br />
                <asp:Button ID="ChangePassword_Button" class="container" runat="server" OnClick="ChangePassword_Button_Click" Text="Change Password" />
            </asp:View>
        </asp:MultiView>
        <asp:Label ID="IsPasswordChanged" class="container" runat="server" Text="" Visible="true"></asp:Label>
        <a href="Login.aspx"> <asp:Label ID="BackToLogin_Label" runat="server" Text="Back to Login"></asp:Label> </a>
    </form>

</body>
</html>
