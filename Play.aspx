<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Play.aspx.cs" Inherits="DungeonMaker.Play" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="scripts/phaser.min.js"></script>
    <script src="scripts/jquery-3.7.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="QUIT" runat="server" Text="QUIT" OnClientClick="confirmDiscard(); return false;" BackColor="Red"/>
            <asp:TextBox ID="MAPTYPE" runat="server" width = "0px" hidden="true"></asp:TextBox>
            <asp:TextBox ID="STARX" runat="server" width = "0px" hidden="true"></asp:TextBox>
            <asp:TextBox ID="STARY" runat="server" width = "0px" hidden="true"></asp:TextBox>
            <asp:TextBox ID="TRAPX" runat="server" width = "0px" hidden="true"></asp:TextBox>
            <asp:TextBox ID="TRAPY" runat="server" width = "0px" hidden="true"></asp:TextBox>
            <asp:TextBox ID="TRAPTYPE" runat="server" width = "0px" hidden="true"></asp:TextBox>
            <asp:TextBox ID="TIME" runat="server" width = "0px" hidden="true"></asp:TextBox>
        </div>
    </form>
    <script src="scripts/Play.js"></script>
</body>
</html>