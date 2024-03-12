<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="DungeonMaker.Logs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="assets/styles/Logs.css" />
    <script src="scripts/jquery-3.7.1.js"></script>
    <div class="content-wrapper" runat="server" id="wrapper">
        <div class="radioButtonListContainer">
            <asp:RadioButtonList ID="TableSelect" runat="server" OnSelectedIndexChanged="TableSelect_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True"> Games </asp:ListItem>
                <asp:ListItem> Achievements </asp:ListItem>
                <asp:ListItem> Purchases </asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="center-container">
            <asp:Label ID="From" runat="server" Text="From"></asp:Label> &nbsp;
            <asp:TextBox ID="FromDateTB" runat="server" TextMode="Date"></asp:TextBox> &nbsp;
            <asp:Label ID="To" runat="server" Text="to"></asp:Label> &nbsp;
            <asp:TextBox ID="ToDateTB" runat="server" TextMode="Date"></asp:TextBox> &nbsp;
            <asp:Button ID="ConfirmButton" runat="server" Text="Confirm" OnClick="ConfirmButton_Click" /> <br /> <br />
            <asp:Label ID="EmptyLabel" runat="server" CssClass="label"></asp:Label>
        </div>
        <asp:MultiView ID="LogsMultiView" runat="server">
            <asp:View ID="GamesView" runat="server">
                <div class="datalist-container">
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
            </asp:View>
            <asp:View ID="AchievementsView" runat="server">
                <div class="datalist-container">
                    <asp:DataList ID="AchievementsDataList" runat="server" OnItemDataBound="AchievementsDataList_ItemDataBound" >
                        <ItemTemplate>
                            <div class="maps-template">
                                &nbsp; &nbsp;
                                <asp:Label ID="Title" runat="server" Text='<%# Bind("achievement") %>' Font-Bold="true" CssClass="other-labels" ></asp:Label>
                                <asp:Literal ID="Bar" runat="server" Text="<b>|</b>&nbsp;"></asp:Literal>
                                <asp:Label ID="DescriptionBody" runat="server" Text='<%# Bind("description") %>' CssClass="other-labels"></asp:Label>
                                <asp:Label ID="dateReceived" runat="server" Text='<%# Bind("dateReceived") %>' CssClass="other-labels"></asp:Label>
                                <asp:Image ID="Coin" runat="server" ImageUrl="assets/ui/coin.png" Width="32px" Height="32px" CssClass="other-labels"/>
                                <asp:Label ID="Credits" runat="server" Text='<%# Bind("creditsWorth") %>' CssClass="other-labels"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </asp:View>
            <asp:View ID="PurchasesView" runat="server">
                <div class="datalist-container">
                    <asp:DataList ID="PurchasesDataList" runat="server" OnItemDataBound="PurchasesDataList_ItemDataBound" >
                        <ItemTemplate>
                            <div class="maps-template">
                                &nbsp; &nbsp;
                                <asp:Label ID="ProductName" runat="server" Text='<%# Bind("productName") %>' Font-Bold="true" CssClass="other-labels" ></asp:Label>
                                <asp:Literal ID="Bar1" runat="server" Text="<b>|</b>&nbsp;"></asp:Literal>
                                <asp:Label ID="DateOfPurchase" runat="server" Text='<%# Bind("dateOfPurchase") %>' CssClass="other-labels"></asp:Label>
                                <asp:Literal ID="Bar2" runat="server" Text="<b>|</b>&nbsp;"></asp:Literal>
                                <asp:Label ID="Cost" runat="server" Text='<%# Bind("cost") %>' CssClass="other-labels"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
    <script src="scripts/DataListAnimation.js"></script>
</asp:Content>