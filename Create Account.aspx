<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create Account.aspx.cs" Inherits="Mid_Project.Create_Account" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="CreateAccount.css" />
</head>
<body>

    <form id="form1" runat="server">
        <asp:Label ID="Title_Label" class="container" runat="server" Text="Create Account"></asp:Label>
    
        <asp:Label ID="UserName_Label" class="container" runat="server" Text="Username"></asp:Label>
        <asp:TextBox ID="UserName_TextBox" class="container" runat="server" TabIndex="3" autocomplete="off" Height="45px" Width="558px"></asp:TextBox>

        <asp:Label ID="Gmail_Label" class="container" runat="server" Text="Gmail"></asp:Label>
        <asp:TextBox ID="Gmail_TextBox" class="container" runat="server" TabIndex="5" autocomplete="off" Height="38px" Width="557px"></asp:TextBox>

        <asp:Label ID="Password_Label" class="container" runat="server" Text="Password"></asp:Label>
        <asp:TextBox ID="Password_TextBox" class="container" runat="server" TabIndex="7" autocomplete="off" Height="50px" Width="556px"></asp:TextBox>
        <asp:Label ID="IsSignUp" class="container" runat="server" Text=""></asp:Label>

        <a href="Login.aspx">
            <asp:Label ID="Login_Label" runat="server" Text="Login"></asp:Label>
        </a>

        <asp:Button ID="SignUp_Button" class="container" runat="server" OnClick="SignUp_Button_Click" Text="Sign Up" />
    </form>

</body>
</html>
