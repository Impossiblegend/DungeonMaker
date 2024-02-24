<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Achievements.aspx.cs" Inherits="DungeonMaker.Achievements" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="assets/styles/Gamelog.css" />
    <script src="scripts/jquery-3.7.1.js"></script>
    <div class="datalist-container">
        <asp:Label ID="EmptyLabel" runat="server" CssClass="label"></asp:Label>
        <asp:DataList ID="AchievementsDataList" runat="server" OnItemDataBound="AchievementsDataList_ItemDataBound" >
            <ItemTemplate>
                <div class="maps-template">
                    &nbsp; &nbsp;
                    <asp:Label ID="Title" runat="server" Text='<%# Bind("achievement") %>' Font-Bold="true" CssClass="other-labels" ></asp:Label> <br /> <br />
                    <asp:Literal ID="Bar" runat="server" Text="<b>|</b>&nbsp;"></asp:Literal>
                    <asp:Label ID="DescriptionBody" runat="server" Text='<%# Bind("description") %>' CssClass="other-labels"></asp:Label> <br /> <br />
                    <asp:Label ID="dateReceived" runat="server" Text='<%# Bind("dateReceived") %>' CssClass="other-labels"></asp:Label> <br />
                    <asp:Image ID="Coin" runat="server" ImageUrl="assets/ui/coin.png" Width="32px" Height="32px" CssClass="other-labels"/>
                    <asp:Label ID="Credits" runat="server" Text='<%# Bind("creditsWorth") %>' CssClass="other-labels"></asp:Label> <br />
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <script src="scripts/DataListAnimation.js"></script>
</asp:Content>