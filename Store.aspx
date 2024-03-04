<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Store.aspx.cs" Inherits="DungeonMaker.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="assets/styles/Store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h1>Game Credits</h1>
        <div class="bundles-container">
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="Heaplbl" runat="server" Text="500 CREDITS"></asp:Label> <br /> <br />
                    <asp:Image ID="HeapImg" runat="server" ImageUrl="assets/ui/heap.png" Width="110px" Height="80px" />
                </div>
                <br /> 
                <p><b>$</b><asp:Label ID="lblPrice1" runat="server" Text="4.99" Font-Bold="true"></asp:Label></p>
                <asp:Button ID="btnPurchase1" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="$" CommandArgument="4.99" CssClass="btnPurchase" />
            </div>
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="BundleLbl" runat="server" Text="1,000 CREDITS"></asp:Label>
                    <asp:Image ID="BundleImg" runat="server" ImageUrl="assets/ui/bundle.png" Width="130px" Height="100px" /> <br />
                    <asp:Label ID="SaleLbl10" runat="server" Text="10% OFF"></asp:Label>
                </div>
                <p><b>$</b><asp:Label ID="lblPrice2" runat="server" Text="8.99" Font-Bold="true"></asp:Label></p>
                <asp:Button ID="btnPurchase2" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="$" CommandArgument="8.99" CssClass="btnPurchase" />
            </div>
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="TroveLbl" runat="server" Text="5,000 CREDITS"></asp:Label>
                    <asp:Image ID="TroveImg" runat="server" ImageUrl="assets/ui/trove.png" Width="110px" Height="90px" /> <br />
                    <asp:Label ID="SaleLbl20" runat="server" Text="20% OFF"></asp:Label>
                </div>
                <p><b>$</b><asp:Label ID="lblPrice3" runat="server" Text="39.99" Font-Bold="true"></asp:Label></p>
                <asp:Button ID="btnPurchase3" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="$" CommandArgument="39.99" CssClass="btnPurchase" />
            </div>
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="ChestLbl" runat="server" Text="25,000 CREDITS"></asp:Label>
                    <asp:Image ID="ChestImg" runat="server" ImageUrl="assets/ui/chest.png" Width="130px" Height="100px" /> <br />
                    <asp:Label ID="SaleLbl40" runat="server" Text="40% OFF"></asp:Label>
                </div>
                <p><b>$</b><asp:Label ID="lblPrice4" runat="server" Text="149.99" Font-Bold="true"></asp:Label></p>
                <asp:Button ID="btnPurchase4" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="$" CommandArgument="149.99" CssClass="btnPurchase" />
            </div>
        </div>
        <br /> <hr />
        <h1>Map Types</h1>
        <div class="bundles-container">
            <div class="map-type">
                <div class="bundle-content">
                    <asp:Label ID="DefaultLbl" runat="server" Text="Default"></asp:Label> <br /> <br />
                    <asp:Image ID="DefaultImg" runat="server" ImageUrl="assets/sets/west.jpg" Width="220px" Height="145px" />
                </div>
                <p><asp:Label ID="Label6" runat="server" Text="FREE" Font-Bold="true"></asp:Label></p>
                <asp:Button ID="Button4" runat="server" Text="Owned" CssClass="Disabled" Enabled="false" />
            </div>
            <div class="map-type">
                <div class="bundle-content">
                    <asp:Label ID="SteampunkLbl" runat="server" Text="Steampunk"></asp:Label> <br /> <br />
                    <asp:Image ID="SteampunkImg" runat="server" ImageUrl="assets/sets/city.jpeg" Width="220px" Height="145px" />
                </div>
                <p><asp:Label ID="Label1" runat="server" Text="250" Font-Bold="true"></asp:Label> <b>CREDITS</b></p>
                <asp:Button ID="Button1" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandArgument="250" CssClass="btnPurchase" Enabled="false" />
            </div>
            <div class="map-type">
                <div class="bundle-content">
                    <asp:Label ID="CyberpunkLbl" runat="server" Text="Cyberpunk"></asp:Label> <br /> <br />
                    <asp:Image ID="CyberpunkImg" runat="server" ImageUrl="assets/sets/cyberpunk-street.png" Width="220px" Height="145px" />
                </div>
                <p><asp:Label ID="Label5" runat="server" Text="350" Font-Bold="true"></asp:Label> <b>CREDITS</b></p>
                <asp:Button ID="Button3" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandArgument="350" CssClass="btnPurchase" />
            </div>
        </div>
        <br /> <hr />
        <h1>Trap Types</h1>
        <div class="bundles-container">
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="SawLbl" runat="server" Text="Saw"></asp:Label> <br /> <br />
                    <asp:Image ID="SawImg" runat="server" ImageUrl="assets/sprites/saw.png" Width="100px" Height="100px" />
                </div>
                <p><asp:Label ID="Label3" runat="server" Text="FREE" Font-Bold="true"></asp:Label></p>
                <asp:Button ID="Button5" runat="server" Text="Owned" CssClass="Disabled" Enabled="false" />
            </div>
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="TurretLbl" runat="server" Text="Turret"></asp:Label> <br /> <br />
                    <asp:Image ID="TurretImg" runat="server" ImageUrl="assets/sprites/turret-freeze.png" Width="140px" Height="90px" />
                </div>
                <p><asp:Label ID="Label8" runat="server" Text="100" Font-Bold="true"></asp:Label> <b>CREDITS</b></p>
                <asp:Button ID="Button6" runat="server" Text="Purchase" OnClick="btnPurchase_Click"  CommandArgument="100" CssClass="btnPurchase" Enabled="false" />
            </div>
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="SpikesLbl" runat="server" Text="Spikes"></asp:Label> <br /> <br /> <br />
                    <asp:Image ID="SpikesImg" runat="server" ImageUrl="assets/sprites/spikes.png" Width="100px" Height="50px" />
                </div>
                <br />
                <p><asp:Label ID="Label10" runat="server" Text="25" Font-Bold="true"></asp:Label> <b>CREDITS</b></p>
                <asp:Button ID="Button7" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandArgument="25" CssClass="btnPurchase" />
            </div>
        </div>
        <br /> <hr />
        <h1>Player Sprites</h1>
        <div class="bundles-container">
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="SpriteLbl1" runat="server" Text="Default"></asp:Label> <br /> <br />
                    <asp:Image ID="SpriteImg1" runat="server" ImageUrl="assets/sprites/phaser-dude.png" Width="55px" Height="80px" />
                </div>
                <p><asp:Label ID="ownedLbl1" runat="server" Text="FREE" Font-Bold="true"></asp:Label></p>
                <asp:Button ID="ownedBtn1" runat="server" Text="Owned" CssClass="Disabled" Enabled="false" />
            </div>
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Label ID="SpriteLbl2" runat="server" Text="Strauey"></asp:Label> <br />
                    <asp:Image ID="SpriteImg2" runat="server" ImageUrl="assets/sprites/strauey-front.png" Width="80px" Height="100px" />
                </div>
                <p><asp:Label ID="Label4" runat="server" Text="75" Font-Bold="true"></asp:Label> <b>CREDITS</b></p>
                <asp:Button ID="Button2" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandArgument="75" CssClass="btnPurchase" />
            </div>
        </div>
    </div>
</asp:Content>