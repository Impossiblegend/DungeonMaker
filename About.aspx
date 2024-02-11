<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="DungeonMaker.About" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>About Dungeon Maker</title>
    <link rel="stylesheet" href="assets/styles/About.css" />
</head>
<body>
    <div>
        <button type="button" id="Back_Button" onclick="back()">BACK</button>
        <script> function back() { window.location.href = 'Explore.aspx'; } </script>
    </div>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="About_Title" runat="server" Text="ABOUT DUNGEON MAKER"></asp:Label> <br /> <br />
            <asp:Label ID="About_Body" runat="server" Text="Dungeon Maker is a dungeon map creation platform where you can create
                your own dungeons for you and your friends' enjoyment, or play others' maps as well.
                It's a solo project created for school by Ofer Bar - a HS senior - as his first big programming endeavour 
                over the 2023-24 school year. The website was mostly coded in C# and ASP.NET. Other languages used include SQL, XML, HTML, jQuery, AJAX, and styled with CSS.
                The game itself was made in Javascript with the Phaser 3 open source framework.">
            </asp:Label> <br /> <br /> <br />
            <asp:Label ID="Thanks_Title" runat="server" Text="SPECIAL THANKS"></asp:Label> <br /> <br />
            <asp:Label ID="Thanks_Body" runat="server" Text="I'd like to thank Orit, my software engineering and computer 
                science teacher. My friends, whose help got me through the project. My dad, who's a programmer, and often helped clear things up and debug.
                And my brother, who also did this project - from which I drew great inspiration for my own.">
            </asp:Label> <br /> <br /> <br />
            <asp:Label ID="Contact_Title" runat="server" Text="CONTACT & FEEDBACK"></asp:Label> <br />
            <asp:TextBox ID="Contact_Textbox" runat="server" placeholder="Or mail us at dungeonmakergame@gmail.com" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
            <asp:Label ID="Rating" runat="server" Text="Rate your overall experience"></asp:Label> <br />
            <asp:ImageButton ID="OneStar" runat="server" ImageUrl="assets/ui/emptyStar.png" Height="32px" Width="32px" OnClick="Rating_Click"/>
            <asp:ImageButton ID="TwoStars" runat="server" ImageUrl="assets/ui/emptyStar.png" Height="32px" Width="32px" OnClick="Rating_Click"/>
            <asp:ImageButton ID="ThreeStars" runat="server" ImageUrl="assets/ui/emptyStar.png" Height="32px" Width="32px" OnClick="Rating_Click"/>
            <asp:ImageButton ID="FourStars" runat="server" ImageUrl="assets/ui/emptyStar.png" Height="32px" Width="32px" OnClick="Rating_Click"/>
            <asp:ImageButton ID="FiveStars" runat="server" ImageUrl="assets/ui/emptyStar.png" Height="32px" Width="32px" OnClick="Rating_Click"/>
            <br />
            <asp:Button ID="SendButton" runat="server" Text="SEND" OnClick="SendButton_Click" Width="150px" />
        </div>
    </form>
</body>
</html>
