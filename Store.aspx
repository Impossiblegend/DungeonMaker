<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Store.aspx.cs" Inherits="DungeonMaker.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="assets/styles/Store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- Game Credits -->
        <h1>Game Credits</h1>
        <asp:DataList ID="dlCredits" runat="server" CssClass="bundles-container" RepeatDirection="Horizontal" OnItemDataBound="dlCredits_ItemDataBound">
            <ItemTemplate>
                <div class="bundle">
                    <div class="bundle-content">
                        <asp:Label ID="creditAmount" runat="server" Text='<%# Eval("creditAmount") %>'></asp:Label> <br />
                        <asp:Image ID="imgBundle" runat="server" ImageUrl='<%# Eval("asset") %>' Width="120px" Height="100px" /> <br />
                        <asp:Label ID="extraText" runat="server" Text='<%# Eval("extraText") %>'></asp:Label>
                    </div>
                    <p><asp:Label ID="costLabel" runat="server" Text='<%# Eval("cost") %>' Font-Bold="true"></asp:Label></p>
                    <asp:Button ID="btnMapPurchase" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="Credits" CommandArgument='<%# Eval("cost") %>' CssClass="btnPurchase" />
                </div>
            </ItemTemplate>
        </asp:DataList>

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
