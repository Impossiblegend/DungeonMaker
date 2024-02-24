<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gamelog.aspx.cs" Inherits="DungeonMaker.Gamelog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="assets/styles/Gamelog.css" />
    <script src="scripts/jquery-3.7.1.js"></script>
    <div class="center-container">
        <asp:Label ID="From" runat="server" Text="From"></asp:Label> &nbsp;
        <asp:TextBox ID="FromDateTB" runat="server" TextMode="Date"></asp:TextBox> &nbsp;
        <asp:Label ID="To" runat="server" Text="to"></asp:Label> &nbsp;
        <asp:TextBox ID="ToDateTB" runat="server" TextMode="Date"></asp:TextBox> &nbsp;
        <asp:Button ID="ConfirmButton" runat="server" Text="Confirm" OnClick="ConfirmButton_Click" />
    </div>
    <div class="datalist-container">
        <asp:Label ID="EmptyLabel" runat="server" CssClass="label"></asp:Label>
        <asp:DataList ID="GamesDataList" runat="server" OnItemDataBound="GamesDataList_ItemDataBound" >
            <ItemTemplate>
                <div class="maps-template">
                    <br /> <br /> &nbsp; &nbsp;
                    <asp:Label ID="Victory" runat="server" Text='<%# Bind("victory") %>' Font-Bold="true" CssClass="other-labels" ></asp:Label>
                    <asp:Label ID="Title" runat="server" Text='<%# Bind("mapName") %>' CssClass="other-labels"></asp:Label>
                    <asp:Label ID="datePlayed" runat="server" Text='<%# Bind("datePlayed") %>' CssClass="other-labels"></asp:Label>
                    <asp:Image ID="Skull" runat="server" ImageUrl="assets/ui/skull.png" Width="32px" Height="32px" CssClass="other-labels"/>
                    <asp:Label ID="DeathCounter" runat="server" Text='<%# Bind("deathCount") %>' CssClass="other-labels"></asp:Label>
                    <asp:Image ID="Star" runat="server" ImageUrl="assets/ui/fullStar.png" Width="32px" Height="32px" CssClass="other-labels"/>
                    <asp:Label ID="StarCounter" runat="server" Text='<%# Bind("starsCollected") %>' CssClass="other-labels"></asp:Label>
                    <asp:Label ID="TimeElapsed" runat="server" Text='<%# Bind("timeElapsed") %>' CssClass="other-labels"></asp:Label>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <script src="scripts/DataListAnimation.js"></script>
</asp:Content>