<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DungeonMaker.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/assets/styles/Login.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server" class="form-container">
        <asp:MultiView ID="PagesMultiView" runat="server">
            <%--Login--%>
            <asp:View ID="LoginView" runat="server">
                <asp:Label ID="Title_Label" runat="server" Text="Sign In" CssClass="title-label"></asp:Label>
                <asp:Label ID="UserName_Label" runat="server" Text="Username" CssClass="input-label"></asp:Label>
                <asp:TextBox ID="UserName_TextBox" runat="server" CssClass="input-field"></asp:TextBox> <br />
                <asp:Label ID="Password_Label" runat="server" Text="Password" CssClass="input-label"></asp:Label>
                <asp:TextBox ID="Password_TextBox" runat="server" TextMode="Password" CssClass="input-field"></asp:TextBox> <br />
                <asp:Label ID="IsLogIn" runat="server" Text="" CssClass="error-message"></asp:Label> <br />
                <asp:Button ID="LogIn_Button" runat="server" OnClick="LogIn_Click" Text="Login" CssClass="submit-button" /> <br /> <br />
                <asp:LinkButton ID="ForgotPassword" runat="server"  OnClick="Nav_Click" CommandArgument="2" CssClass="link">Forgot Password</asp:LinkButton>
                <asp:LinkButton ID="Guest_Login" runat="server" OnClick="Guest_Login_Click" CssClass="link">Guest Login</asp:LinkButton>
                <asp:LinkButton ID="CreateAccount" runat="server" OnClick="Nav_Click" CommandArgument="1" CssClass="link">Sign Up</asp:LinkButton>
            </asp:View>
            <%--Register--%>
            <asp:View ID="RegisterView" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"  EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
                <asp:Label ID="SignUpTitleLabel" runat="server" Text="Sign Up" CssClass="title-label"></asp:Label>
                <asp:Label ID="UsernameLbl" runat="server" Text="Username" CssClass="input-label"></asp:Label>
                <asp:TextBox ID="UserNameTB" runat="server" TabIndex="3" autocomplete="off" Height="45px" Width="330px" CssClass="input-field"></asp:TextBox>
                <asp:RegularExpressionValidator ID="Username_Validator" runat="server" ControlToValidate="UserName_TextBox" ErrorMessage="Usernames contain only letters and numbers" 
                    ForeColor="Red" ValidationExpression="^[a-zA-Z0-9]*$" CssClass="hidden-validator">
                </asp:RegularExpressionValidator>
                <asp:Label ID="Gmail_Label" runat="server" Text="Email" CssClass="input-label"></asp:Label>
                <asp:TextBox ID="Gmail_TextBox" runat="server" TabIndex="5" autocomplete="off" Height="45px" Width="330px" CssClass="input-field" ></asp:TextBox>
                <asp:RegularExpressionValidator ID="Gmail_Validator" runat="server" ControlToValidate="Gmail_TextBox" ErrorMessage="Incorrect email format" ForeColor="Red" 
                    ValidationExpression="^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" CssClass="hidden-validator">
                </asp:RegularExpressionValidator>
                <asp:Label ID="PWLabel" runat="server" Text="Password" CssClass="input-label"></asp:Label>
                <asp:TextBox ID="PWTB" runat="server" TabIndex="7" autocomplete="off" Height="45px" Width="330px" TextMode="Password" CssClass="input-field"></asp:TextBox> <br />
                <asp:CheckBox ID="ToSCheckBox" runat="server" style="display: inline-block;"/>
                <asp:Label ID="TosLabel" runat="server" Text="I agree to the terms of service" CssClass="tos-label" ></asp:Label>
                <asp:Label ID="IsSignUp" runat="server" Text="" CssClass="error-message"></asp:Label>
                <asp:Button ID="SignUp_Button" runat="server" OnClick="SignUp_Button_Click" OnClientClick="attachValidationHandlers()" Text="Register" Enabled="false" CssClass="submit-button" />
                <asp:LinkButton ID="BackToLoginLabel" runat="server" OnClick="Nav_Click" CommandArgument="0" CssClass="link">Back to Login</asp:LinkButton>
            </asp:View>
            <%--Forgot/Change Password--%>
            <asp:View ID="ForgotPasswordView" runat="server">
                <asp:MultiView ID="FPMultiView" runat="server">
                    <%--Confirm Email--%>
                    <asp:View ID="EmailConfirmView" runat="server">
                        <asp:Label ID="ForgotPasswordTitleLabel" runat="server" Text="Forgot Password" CssClass="title-label"></asp:Label>
                        <asp:Label ID="UserNameLabel" runat="server" Text="Username" CssClass="input-label"></asp:Label>
                        <asp:TextBox ID="UserNameTextBox" runat="server" CssClass="input-field"></asp:TextBox>
                        <asp:Label ID="Email_Label" runat="server" Text="Email" CssClass="input-label"></asp:Label>
                        <asp:TextBox ID="Email_TextBox" runat="server" CssClass="input-field"></asp:TextBox>
                        <asp:Panel ID="BlankSpacePanel" runat="server" Height="80px"></asp:Panel>
                        <asp:Button ID="CheckAccount_Button" runat="server" OnClick="CheckAccount_Button_Click" Text="Check Account" CssClass="submit-button" />
                    </asp:View>
                    <%--Change Password--%>
                    <asp:View ID="ChangePasswordView" runat="server">
                        <asp:Label ID="ChangePassword_Label" runat="server" Text="Enter a new password" CssClass="input-label"></asp:Label>
                        <asp:TextBox ID="ChangePassword_TextBox" runat="server" CssClass="input-field"></asp:TextBox> <br /> <br />
                        <asp:Label ID="ConfirmPassword_Label" runat="server" Text="Confirm new password" CssClass="input-label"></asp:Label>
                        <asp:TextBox ID="ConfirmPassword_TextBox" runat="server" CssClass="input-field"></asp:TextBox>
                        <asp:Panel ID="Panel1" runat="server" Height="90px"></asp:Panel>
                        <asp:Button ID="ChangePassword_Button" runat="server" OnClick="ChangePassword_Button_Click" Text="Change Password" CssClass="submit-button" />
                    </asp:View>
                </asp:MultiView>
                <asp:Label ID="IsPasswordChanged" runat="server" Text="" CssClass="error-message"></asp:Label>
                <asp:LinkButton ID="BackToLogin" runat="server" OnClick="Nav_Click" CommandArgument="0" CssClass="link">Back to Login</asp:LinkButton>
            </asp:View>
        </asp:MultiView>
    </form>
    <script src="scripts/Register.js"></script>
</body>
</html>
