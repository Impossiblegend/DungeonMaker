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
                        <asp:Label ID="lblMapType" runat="server" Text='<%# Eval("type") %>'></asp:Label>
                        <asp:Image ID="imgMapType" runat="server" ImageUrl='<%# Eval("asset") %>' Width="220px" Height="145px" />
                    </div>
                    <p><asp:Label ID="costLabel" runat="server" Text='<%# Eval("cost") %>' Font-Bold="true"></asp:Label></p>
                    <asp:Button ID="btnMapPurchase" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName="Product" CommandArgument='<%# Eval("cost") %>' CssClass="btnPurchase" />
                </div>
            </ItemTemplate>
        </asp:DataList>

        <!-- Trap Types -->
        <h1>Trap Types</h1>
        <asp:DataList ID="dlTrapTypes" runat="server" CssClass="bundles-container" RepeatDirection="Horizontal" OnItemDataBound="dlTrapTypes_ItemDataBound" >
            <ItemTemplate>
                <div class="bundle">
                    <div class="bundle-content">
                        <asp:Label ID="lblTrapType" runat="server" Text='<%# Eval("type") %>'></asp:Label> <br />
                        <asp:Image ID="imgTrapType" runat="server" ImageUrl='<%# Eval("asset") %>' Width="100px" Height="100px" />
                    </div>
                    <p><asp:Label ID="costLabel" runat="server" Text='<%# Eval("cost") %>' Font-Bold="true"></asp:Label></p>
                    <asp:Button ID="btnTrapPurchase" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName='<%# Eval("class") %>' CommandArgument='<%# Eval("cost") %>' CssClass="btnPurchase" />
                </div>
            </ItemTemplate>
        </asp:DataList>

        <!-- Player Sprites -->
        <h1>Player Sprites</h1>
        <asp:DataList ID="dlSkins" runat="server" CssClass="bundles-container" RepeatDirection="Horizontal" OnItemDataBound="dlSkins_ItemDataBound" >
            <ItemTemplate>
                <div class="bundle">
                    <div class="bundle-content">
                        <asp:Label ID="lblSkin" runat="server" Text='<%# Eval("type") %>'></asp:Label> <br />
                        <asp:Image ID="imgSkin" runat="server" ImageUrl='<%# Eval("asset") %>' Width="60px" Height="90px" />
                    </div>
                    <p><asp:Label ID="costLabel" runat="server" Text='<%# Eval("cost") %>' Font-Bold="true"></asp:Label></p>
                    <asp:Button ID="btnSkinPurchase" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandName='<%# Eval("class") %>' CommandArgument='<%# Eval("cost") %>' CssClass="btnPurchase" />
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>