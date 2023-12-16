<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Mid_Project.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Login.css"/>
</head>
<body>

    <form id="form1" runat="server">
        <asp:Label ID="Title_Label" class="container" runat="server" Text="Login"></asp:Label>
        <asp:Label ID="UserName_Label" class="container" runat="server" Text="Username   "></asp:Label>
        <asp:TextBox ID="UserName_TextBox" class="container" runat="server"></asp:TextBox>
        <asp:Label ID="Password_Label" class="container" runat="server" Text="Password   "></asp:Label>
        <asp:TextBox ID="Password_TextBox" class="container" runat="server"></asp:TextBox>
        <asp:Label ID="IsLogIn" class="container" runat="server" Text=""></asp:Label>
        <asp:Button ID="LogIn_Button" class="container" runat="server" OnClick="Button1_Click" Text="Login" />
        <a href="ForgotPassword.aspx">
        <asp:Label ID="Forgot_Password_Label" runat="server" Text="Forgot Password"></asp:Label>
        </a>

        <a href="Create Account.aspx">
        <asp:Label ID="Create_account_label" runat="server" Text="Create Account" Width="138px"></asp:Label>
        </a>

    </form>

</body>
</html>
