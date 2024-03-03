<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Store.aspx.cs" Inherits="DungeonMaker.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="assets/styles/Store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h1>Credits Store</h1>
        <div class="bundles-container">
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Image ID="imgCredits1" runat="server" ImageUrl="assets/ui/coin.png" Width="32px" Height="32px" />
                    <asp:Label ID="lblCredits1" runat="server" Text="50"></asp:Label>
                </div>
                <p>$<asp:Label ID="lblPrice1" runat="server" Text="5"></asp:Label></p>
                <asp:Button ID="btnPurchase1" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandArgument="5" CssClass="btnPurchase" />
            </div>
            <div class="bundle">
                <div class="bundle-content">
                    <asp:Image ID="imgCredits2" runat="server" ImageUrl="assets/ui/coin.png" Width="32px" Height="32px" />
                    <asp:Label ID="lblCredits2" runat="server" Text="100"></asp:Label>
                </div>
                <p>$<asp:Label ID="lblPrice2" runat="server" Text="9"></asp:Label></p>
                <asp:Button ID="btnPurchase2" runat="server" Text="Purchase" OnClick="btnPurchase_Click" CommandArgument="9" CssClass="btnPurchase" />
            </div>
            <!-- Add more bundles as needed -->
        </div>
    </div>
</asp:Content>
