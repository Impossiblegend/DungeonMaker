<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MapSelection.aspx.cs" Inherits="DungeonMaker.MapSelection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="assets/styles/MapSelection.css" />
    <asp:Label ID="SelectLabel" runat="server" Text="CHOOSE A MAP TYPE TO USE" CssClass="title-label"></asp:Label>
    <div class="wrapper">
        <asp:DataList ID="MapTypeDataList" runat="server" RepeatColumns="3" OnItemDataBound="MapTypeDataList_ItemDataBound" OnItemCommand="MapTypeDataList_ItemCommand">
            <ItemTemplate>
                <div class="item">
                    <asp:ImageButton ID="Thumbnail" runat="server" ImageUrl='<%# Bind("asset") %>' Width="400px" Height="250px" />
                    <asp:Image ID="LockImage" runat="server" CssClass="overlay-image" ImageUrl="assets/ui/lock2.png" Width="54px" Height="80px" />
                    <asp:Label ID="MapTypeName" runat="server" Text='<%# Bind("mapType") %>' CssClass="name"></asp:Label>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>