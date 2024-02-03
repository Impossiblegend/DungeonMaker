<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Userpage.aspx.cs" Inherits="DungeonMaker.Userpage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="scripts/jquery-3.7.1.js"></script>
    <link rel="stylesheet" href="assets/styles/Userpage.css" />
    <div class="center-container">
        <asp:Panel ID="ProfilePanel" runat="server" CssClass="panel">
        <asp:ImageButton ID="Avatar" runat="server" Height="100px" Width="100px" CssClass="picture" OnClientClick="UploadFile()" />
        <asp:FileUpload ID="AvatarUploader" runat="server" Visible="false" onchange="return handleFileUpload();" />
            <asp:Label ID="UsernameLabel" runat="server" CssClass="item-label" Text=" "></asp:Label>
            <div class="gridview-container">
                <asp:GridView ID="UserGridView" runat="server" AutoGenerateColumns="False" CssClass="gridview-style">
                    <Columns>
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="Password" HeaderText="Password" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Date" HeaderText="Creation Date" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
        <hr />
    </div>
    <script src="scripts/Userpage.js"></script>
    <div class="datalist-container">
        <asp:DataList ID="MapsDataList" runat="server" RepeatColumns="4" CellPadding="4" OnItemCommand="MapsDataList_ItemCommand" RepeatDirection="Horizontal" OnItemDataBound="MapsDataList_ItemDataBound" CssClass="centered-datalist">
            <ItemTemplate>
            <div class="maps-template">
                <asp:Label ID="Title" runat="server" CssClass="item-label" Text='<%# Bind("mapName") %>' ></asp:Label> 
                <asp:TextBox ID="RenameTextBox" runat="server" Visible="false"></asp:TextBox> <br />
                <asp:Label ID="CreationDate" runat="server" CssClass="item-label" Text='<%# Bind("creationDate") %>' ></asp:Label> <br />
                <asp:Label ID="MapID" runat="server" Visible="false" Text='<%# Bind("mapID") %>' ></asp:Label>
                <asp:Image ID="Thumbnail" runat="server" Width="225px" CssClass="item-image" Height="110px" ImageUrl='<%# Bind("thumbnail") %>'/>
                <asp:Button ID="PlayButton" runat="server" Text="Play" Height="25px" Width="72px" CssClass="item-button" CommandName="PlayButton" BackColor="#c0c0c0"/>
                <asp:Button ID="PrivacyButton" runat="server" Text='<%# Bind("isPublic") %>' Height="25px" Width="72px" CssClass="item-button" CommandName="PrivacyButton" Visible="false"/> <br />
                <asp:Button ID="DeleteButton" runat="server" Text="Delete" Height="25px" Width="72px" CssClass="Delete-Button" CommandName="DeleteButton" Visible="false" BackColor="#000000" />
                <asp:Button ID="RenameButton" runat="server" Text="Rename" Height="25px" Width="72px" CssClass="item-button" CommandName="RenameButton" Visible="false" BackColor="#c0c0c0"/>
            </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
</asp:Content>