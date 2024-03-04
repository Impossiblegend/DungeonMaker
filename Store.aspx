<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Store.aspx.cs" Inherits="DungeonMaker.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="assets/styles/Store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- Game Credits -->
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

        <!-- Map Types -->
        <h1>Map Types</h1>
        <asp:DataList ID="dlMapTypes" runat="server" CssClass="bundles-container" RepeatDirection="Horizontal" OnItemDataBound="dlMapTypes_ItemDataBound">
            <ItemTemplate>
                <div class="map-type">
                    <div class="bundle-content">
                        <asp:Label ID="lblMapType" runat="server" Text='<%# Eval("mapType") %>'></asp:Label>
                        <asp:Image ID="imgMapType" runat="server" ImageUrl='<%# Eval("asset") %>' Width="220px" Height="145px" />
                    </div>
                    <p><b><%# Eval("cost") %> CREDITS</b></p>
                    <asp:Button ID="btnMapPurchase" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="Map" CommandArgument='<%# Eval("cost") %>' CssClass="btnPurchase" />
                </div>
            </ItemTemplate>
        </asp:DataList>

        <!-- Trap Types -->
        <h1>Trap Types</h1>
        <asp:DataList ID="dlTrapTypes" runat="server" CssClass="bundles-container" RepeatDirection="Horizontal" OnItemDataBound="dlTrapTypes_ItemDataBound" >
            <ItemTemplate>
                <div class="bundle">
                    <div class="bundle-content">
                        <asp:Label ID="lblTrapType" runat="server" Text='<%# Eval("trapType") %>'></asp:Label> <br />
                        <asp:Image ID="imgTrapType" runat="server" ImageUrl='<%# Eval("asset") %>' Width="100px" Height="100px" />
                    </div>
                    <p><b><%# Eval("cost") %> CREDITS</b></p>
                    <asp:Button ID="btnTrapPurchase" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="Trap" CommandArgument='<%# Eval("cost") %>' CssClass="btnPurchase" />
                </div>
            </ItemTemplate>
        </asp:DataList>

        <!-- Player Sprites -->
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
