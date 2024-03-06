﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Adminpage.aspx.cs" Inherits="DungeonMaker.Adminpage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="assets/styles/Adminpage.css" />
    <script src="scripts/jquery-3.7.1.js"></script>
    <div class="content-wrapper">
        <asp:Panel ID="FeedbackPanel" runat="server" CssClass="datalist-container">
            <asp:Label ID="FeedbackLabel" runat="server" Text="Select feedback to feature on explore page:" CssClass="title" Font-Bold="true" ></asp:Label>
            <asp:DataList ID="FeedbackDataList" runat="server" RepeatColumns="4" CellPadding="4" RepeatDirection="Horizontal" OnItemDataBound="FeedbackDataList_ItemDataBound" OnItemCommand="FeedbackDataList_ItemCommand" >
                <ItemTemplate>
                    <div class="item-template">
                        <asp:Label ID="Sender" runat="server" CssClass="item-label" Text='<%# Bind("username") %>'></asp:Label> <br />
                        <asp:Image ID="ProfilePicture" runat="server" Width="75px" Height="75px" CssClass="profilePicture" ImageUrl='<%# Bind("profilePicture") %>' /><br />
                        <asp:Label ID="Feedback" runat="server" CssClass="feedback" Text='<%# Bind("feedbackBody") %>'></asp:Label> <br />
                        <asp:Label ID="feedbackID" runat="server" Text='<%# Bind("feedbackID") %>' Visible="false"></asp:Label>
                        <asp:PlaceHolder ID="starsPlaceHolder" runat="server"></asp:PlaceHolder> <br />
                        <asp:ImageButton ID="Checkmark" runat="server" ImageUrl="assets/ui/check.png" Width="27px" Height="25px" CommandName="Checkmark_Click" />
                        <asp:ImageButton ID="Cross" runat="server" ImageUrl="assets/ui/cross.png" Width="25px" Height="25px" CommandName="Cross_Click"/>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
        <script src="scripts/ExpandText.js"></script>
        <asp:Panel ID="AchievementsPanel" runat="server" CssClass="achievements-panel">
            <asp:Label ID="AchievementsLabel" runat="server" Text="Achievements created:" CssClass="title" Font-Bold="true" ></asp:Label>
            <asp:DataList ID="AchievementsDataList" runat="server" OnItemDataBound="AchievementsDataList_ItemDataBound" OnItemCommand="AchievementsDataList_ItemCommand" >
                <ItemTemplate>
                    <div class="maps-template">
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="LockButton" runat="server" CssClass="lock-button" ImageUrl="assets/ui/" CommandName="LockButton_Click" Width="27px" Height="40px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="EditButton" runat="server" CssClass="lock-button" ImageUrl="assets/ui/pencil.png" CommandName="EditButton_Click" Width="33px" Height="40px" />
                        <asp:TextBox ID="EditTextBox" runat="server" Visible="false"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Title" runat="server" Text='<%# Bind("achievementTitle") %>' Font-Bold="true" CssClass="other-labels" ></asp:Label>
                        <asp:Label ID="Bar" runat="server" Text=" | " CssClass="other-labels"></asp:Label>
                        <asp:Label ID="DescriptionBody" runat="server" Text='<%# Bind("description") %>' CssClass="other-labels"></asp:Label>
                        <asp:Label ID="CreditsLabel" runat="server" Text=" | Credits" Font-Bold="true" CssClass="other-labels"></asp:Label>
                        <asp:Image ID="Coin" runat="server" ImageUrl="assets/ui/coin.png" Width="32px" Height="32px" CssClass="other-labels"/>
                        <asp:Label ID="Credits" runat="server" Text='<%# Bind("creditsWorth") %>' CssClass="other-labels"></asp:Label>
                        <asp:Label ID="isValid" runat="server" Text='<%# Bind("isValid") %>' Visible="false" ></asp:Label>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    <script src="scripts/DataListAnimation.js"></script>
</asp:Content>