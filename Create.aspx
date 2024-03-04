<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Create.aspx.cs" Inherits="DungeonMaker.Create" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta http-equiv="X-UA-Compatible" content="ie=edge"/>
    <title></title>
    <link rel="stylesheet" href="assets/styles/Create.css" />
    <script src="scripts/phaser.js"></script>
    <script src="scripts/jquery-3.7.1.js"></script>
</head>
<body>
    <main></main>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TB1" runat="server" hidden="true" Text="blank"></asp:TextBox>
            <asp:TextBox ID="TB2" runat="server" hidden="true" Text="20"></asp:TextBox>
            <asp:TextBox ID="TB3" runat="server" hidden="true"></asp:TextBox>
            <asp:TextBox ID="TB4" runat="server" hidden="true"></asp:TextBox>
            <asp:TextBox ID="TB5" runat="server" hidden="true"></asp:TextBox>
            <asp:Button ID="Submit" class="container" runat="server" OnClientClick="saveMap()"  OnClick="Submit_Click" Text="SAVE & FINISH" BackColor="Green" />
            <asp:TextBox ID="mapName_TextBox" runat="server" placeholder="Name your map"></asp:TextBox>
            <asp:Button ID="Discard" runat="server" Text="QUIT & DISCARD" OnClientClick="confirmDiscard(); return false;" BackColor="Red"/>
        </div>
    </form>
    <script src="scripts/Create.js"></script>
</body>
</html>
