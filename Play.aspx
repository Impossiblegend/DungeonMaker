<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Play.aspx.cs" Inherits="DungeonMaker.Play" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .container {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 350px;
            height: 250px;
            background-color: rgba(255, 255, 255, 0.7);
        }
        .overlay {
            position: absolute;
            top: 5px;
            left: 125px;
            z-index: 1;
            padding: 10px;
        }
    </style>
    <script src="//cdn.jsdelivr.net/npm/phaser@3.70.0/dist/phaser.js"></script>
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
        </div>
        <center>
            <asp:Panel ID="EndPanel" runat="server" CssClass="container">
                <asp:Image ID="Border" runat="server" ImageUrl="assets/ui/border.png"/>
                <div class="overlay">
                    <asp:Label ID="Victory" runat="server"></asp:Label> <br /> <br />
                    <asp:Image ID="Skull" runat="server" ImageUrl="assets/ui/skull.png" Width="32px" Height="32px"/>
                    <asp:Label ID="DeathCounter" runat="server"></asp:Label> <br /> <br />
                    <asp:Image ID="Star" runat="server" ImageUrl="assets/ui/fullStar.png" Width="32px" Height="32px"/>
                    <asp:Label ID="StarCounter" runat="server"></asp:Label> <br /> <br />
                    <asp:Label ID="TimeElapsed" runat="server"></asp:Label> <br /> <br />
                    <asp:Button ID="Finish" runat="server" Text="FINISH" OnClick="Finish_Click"/>
                </div>
            </asp:Panel>
        </center>
    </form>
    <script src="scripts/Play.js"></script>
</body>
</html>