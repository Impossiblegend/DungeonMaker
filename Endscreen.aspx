<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Endscreen.aspx.cs" Inherits="DungeonMaker.Endscreen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="assets/styles/Endscreen.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <asp:Image ID="BG" runat="server" />
        <asp:Image ID="Border" runat="server" ImageUrl="assets/ui/border.png" CssClass="container"/>
        <div class="overlay">
            <asp:Label ID="Victory" runat="server"></asp:Label> <br /> <br />
            <asp:Image ID="Skull" runat="server" ImageUrl="assets/ui/skull.png" Width="32px" Height="32px"/>
            <asp:Label ID="DeathCounter" runat="server"></asp:Label> <br /> <br />
            <asp:Image ID="Star" runat="server" ImageUrl="assets/ui/fullStar.png" Width="32px" Height="32px"/>
            <asp:Label ID="StarCounter" runat="server"></asp:Label> <br /> <br />
            <asp:Label ID="TimeElapsed" runat="server"></asp:Label> <br /> <br />
            <asp:Button ID="Finish" runat="server" Text="FINISH" OnClick="Finish_Click" CssClass="custom-button"/>
        </div>
        </div>
    </form>
</body>
</html>
