<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="DungeonMaker.Register" EnableViewState="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="assets/styles/Register.css" />
    <script src="scripts/jquery-3.7.1.js"></script>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
        <asp:Label ID="Title_Label" class="container" runat="server" Text="Sign Up"></asp:Label>
        <asp:Label ID="UserName_Label" class="container" runat="server" Text="Username"></asp:Label>
        <asp:TextBox ID="UserName_TextBox" class="container" runat="server" TabIndex="3" autocomplete="off" Height="45px" Width="330px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="Username_Validator" runat="server" ControlToValidate="UserName_TextBox" ErrorMessage="Usernames contain only letters and numbers" 
            ForeColor="Red" ValidationExpression="^[a-zA-Z0-9]*$" CssClass="hidden-validator">
        </asp:RegularExpressionValidator>
        <asp:Label ID="Gmail_Label" class="container" runat="server" Text="Email"></asp:Label>
        <asp:TextBox ID="Gmail_TextBox" class="container" runat="server" TabIndex="5" autocomplete="off" Height="45px" Width="330px"></asp:TextBox>
        <asp:RegularExpressionValidator ID="Gmail_Validator" runat="server" ControlToValidate="Gmail_TextBox" ErrorMessage="Incorrect email format" ForeColor="Red" 
            ValidationExpression="^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" CssClass="hidden-validator">
        </asp:RegularExpressionValidator>
        <asp:Label ID="Password_Label" class="container" runat="server" Text="Password"></asp:Label>
        <asp:TextBox ID="Password_TextBox" class="container" runat="server" TabIndex="7" autocomplete="off" Height="45px" Width="330px" TextMode="Password"></asp:TextBox>
        <asp:Label ID="IsSignUp" class="container" runat="server" Text=""></asp:Label>
        <a href="Login.aspx">
            <asp:Label ID="Login_Label" runat="server" Text="Login"></asp:Label>
        </a>
        <asp:Button ID="SignUp_Button" class="container" runat="server" OnClick="SignUp_Button_Click" OnClientClick="attachValidationHandlers()" Text="Register" />
    </form>
    <script>
        function attachValidationHandlers() {
            var Gmail_Validator = document.getElementById("Gmail_Validator");
            var Username_Validator = document.getElementById("Username_Validator");
            if (!Gmail_Validator.isValid) {
                Gmail_Validator.style.display = 'block';
                document.getElementById("Password_Label").style.marginTop = '18px';
            }
            if (!Username_Validator.isValid) {
                Username_Validator.style.display = 'block';
                document.getElementById("Gmail_Label").style.marginTop = '18px';
            }
        }
    </script>
</body>
</html>
