<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DungeonMaker.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="assets/styles/Login.css"/>
</head>
<body>

    <form id="form1" runat="server">
        <asp:Label ID="Title_Label" class="container" runat="server" Text="Sign In"></asp:Label>
        <asp:Label ID="UserName_Label" class="container" runat="server" Text="Username" ></asp:Label>
        <asp:TextBox ID="UserName_TextBox" class="container" runat="server"></asp:TextBox>
        <asp:Label ID="Password_Label" class="container" runat="server" Text="Password"></asp:Label>
        <asp:TextBox ID="Password_TextBox" class="container" runat="server" TextMode="Password"></asp:TextBox>
        <asp:Label ID="IsLogIn" class="container" runat="server" Text=""></asp:Label>
        <asp:Button ID="LogIn_Button" class="container" runat="server" OnClick="LogIn_Click" Text="Login" />
        <a href="ForgotPassword.aspx">
        <asp:Label ID="Forgot_Password_Label" runat="server" Text="Forgot Password"></asp:Label>
        </a>
        <asp:LinkButton ID="Guest_Login" runat="server" OnClick="Guest_Login_Click" CssClass="guest-link" Text="Guest Login"></asp:LinkButton>
        <a href="Register.aspx">
        <asp:Label ID="Create_account_label" runat="server" Width="70px">Sign Up</asp:Label>
        </a>

    </form>

</body>
</html>
