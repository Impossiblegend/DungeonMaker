<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Adminpage.aspx.cs" Inherits="DungeonMaker.Adminpage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <link rel="stylesheet" href="assets/styles/Adminpage.css" />
        <asp:Panel ID="Feedback" runat="server" CssClass="datalist-container">
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
</asp:Content>
