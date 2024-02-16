<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Explore.aspx.cs" Inherits="DungeonMaker.Explore" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="assets/styles/Explore.css" />
    <div class="search-container">
        <div class="search-input-container">
            <asp:ImageButton ID="SearchButton" runat="server" ImageUrl="assets/ui/search.png" Height="36px" Width="36px" OnClick="SearchButton_Click" />
            <asp:TextBox ID="SearchBar" placeholder="Search for users..." runat="server" CssClass="search-input" Text=""></asp:TextBox>
            <asp:DropDownList ID="SortBy" runat="server" Height="42px" Width="108px">
                <asp:ListItem Selected="True" Value="Newest"> Newest</asp:ListItem>
                <asp:ListItem Value="Oldest"> Oldest </asp:ListItem>
                <asp:ListItem Value="A-Z"> A-Z </asp:ListItem>
                <asp:ListItem Value="Z-A"> Z-A </asp:ListItem>
                <asp:ListItem Value="Most popular" Enabled="false"> Most popular </asp:ListItem>
                <asp:ListItem Value="Least popular" Enabled="false"> Least popular </asp:ListItem>
                <asp:ListItem Value="Hardest" Enabled="false"> Hardest </asp:ListItem>
                <asp:ListItem Value="Easiest" Enabled="false"> Easiest </asp:ListItem>
            </asp:DropDownList>
            <asp:RadioButtonList ID="TableSelect" runat="server" OnSelectedIndexChanged="TableSelect_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="Users" Selected="True"> Users </asp:ListItem>
                <asp:ListItem Value="Dungeons"> Dungeons </asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>
    <asp:Label ID="SearchResultsLabel" runat="server" Text="SEARCH RESULTS" CssClass="search-label" Visible="false"></asp:Label>
    <div class="datalist-container">
        <asp:MultiView ID="DataListMultiView" runat="server">
            <asp:View ID="Users" runat="server">
            <asp:DataList ID="UsersDataList" runat="server" RepeatColumns="5" CellPadding="4" OnItemCommand="UsersDataList_ItemCommand" RepeatDirection="Horizontal" OnItemDataBound="UsersDataList_ItemDataBound">
                <ItemTemplate>
                <div class="users-template">
                    <asp:Label ID="User" runat="server" CssClass="item-label" Text='<%# Bind("username") %>'></asp:Label> <br />
                    <asp:Label ID="Email" runat="server" Visible="false" Text='<%# Bind("email") %>'></asp:Label>
                    <asp:Image ID="ProfilePicture" runat="server" Width="110px" Height="110px" CssClass="profilePicture" ImageUrl='<%# Bind("profilePicture") %>' /><br />
                    <asp:Button ID="Visit" runat="server" Text="Visit Page" Height="25px" Width="110px" CssClass="item-button" CommandName="Visit_Click" />
                    <asp:Button ID="Block" runat="server" Text="Block" Height="25px" Width="110px" CssClass="item-button" Visible="false" CommandName="Block_Click"/>
                    <asp:Button ID="Logs" runat="server" Text="View Logs" Height="25px" Width="110px" CssClass="item-button" Visible="false" CommandName="Logs_Click"/>
                </div>
                </ItemTemplate>
            </asp:DataList>
            </asp:View>
            <asp:View ID="Dungeons" runat="server">
            <asp:DataList ID="MapsDataList" runat="server" RepeatColumns="4" CellPadding="4" OnItemCommand="MapsDataList_ItemCommand" RepeatDirection="Horizontal" OnItemDataBound="MapsDataList_ItemDataBound">
                <ItemTemplate>
                <div class="maps-template">
                    <asp:Label ID="Title" runat="server" CssClass="item-label" Text='<%# Bind("mapName") %>' ></asp:Label> <br />
                    <asp:Label ID="Creator" runat="server" CssClass="item-label" Text='<%# Bind("username") %>'></asp:Label> <br />
                    <asp:Label ID="mapID" runat="server" Text='<%# Bind("mapID") %>' Visible="false" Width="0px"></asp:Label>
                    <asp:Image ID="Thumbnail" runat="server" Width="200px" CssClass="item-image" Height="100px" ImageUrl='<%# Bind("thumbnail") %>'/>
                    <asp:Button ID="PlayButton" runat="server" Text="Play" Height="35px" Width="145px" CssClass="item-button"  CommandName="PlayButton"/>
                    <asp:Button ID="DeleteButton" runat="server" Text="Delete" Height="35px" Width="145px" CssClass="Delete-Button" CommandName="DeleteButton" Visible="false" BackColor="#000000" />
                </div>
                </ItemTemplate>
            </asp:DataList>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:Panel ID="BlankSpace1" runat="server" Height="30px"></asp:Panel>
        <div class="container">        
            <asp:Label ID="PopularMapsLabel" runat="server" Text="POPULAR DUNGEONS" CssClass="title"></asp:Label>
            <asp:Panel ID="PopularMapsPanel" runat="server" CssClass="newest-datalist-container">
                <asp:DataList ID="PopularMapsDataList" runat="server" CellPadding="4" RepeatDirection="Horizontal" OnItemDataBound="MapsDataList_ItemDataBound" OnItemCommand="MapsDataList_ItemCommand">
                    <ItemTemplate>
                    <div class="maps-template">
                        <asp:Label ID="title" runat="server" CssClass="item-label" Text='<%# Bind("mapName") %>' ></asp:Label> <br />
                        <asp:Label ID="Creator" runat="server" CssClass="item-label" Text='<%# Bind("creatorUsername") %>'></asp:Label> <br />
                        <asp:Label ID="mapID" runat="server" Text='<%# Bind("mapID") %>' Visible="false" Height="0px"></asp:Label>
                        <asp:Image ID="Thumbnail" runat="server" Width="150px" CssClass="item-image" Height="75px" ImageUrl='<%# Bind("thumbnail") %>'/>
                        <asp:Button ID="PlayButton" runat="server" Text="Play" Height="30px" Width="110px" CssClass="item-button"  CommandName="PlayButton"/> <br />
                        <asp:Button ID="DeleteButton" runat="server" Text="Delete" Height="30px" Width="110px" CssClass="Delete-Button" CommandName="DeleteButton" Visible="false" BackColor="#000000" />
                    </div>
                    </ItemTemplate>
                </asp:DataList>
            </asp:Panel>
        </div>
    <asp:Panel ID="BlankSpace2" runat="server" Height="30px"></asp:Panel>
    <div class="container">
        <asp:Label ID="NewestMapsLabel" runat="server" Text="NEWEST DUNGEONS" CssClass="title"></asp:Label>
        <asp:Panel ID="NewestMapsPanel" runat="server" CssClass="newest-datalist-container">
            <asp:DataList ID="NewestMapsDataList" runat="server" CellPadding="4" RepeatDirection="Horizontal" OnItemDataBound="MapsDataList_ItemDataBound" OnItemCommand="MapsDataList_ItemCommand">
                <ItemTemplate>
                <div class="maps-template">
                    <asp:Label ID="Title" runat="server" CssClass="item-label" Text='<%# Bind("mapName") %>' ></asp:Label> <br />
                    <asp:Label ID="Creator" runat="server" CssClass="item-label" Text='<%# Bind("username") %>'></asp:Label> <br />
                    <asp:Label ID="mapID" runat="server" Text='<%# Bind("mapID") %>' Visible="false" Height="0px"></asp:Label>
                    <asp:Image ID="Thumbnail" runat="server" Width="150px" CssClass="item-image" Height="75px" ImageUrl='<%# Bind("thumbnail") %>'/>
                    <asp:Button ID="PlayButton" runat="server" Text="Play" Height="30px" Width="110px" CssClass="item-button"  CommandName="PlayButton"/> <br />
                    <asp:Button ID="DeleteButton" runat="server" Text="Delete" Height="30px" Width="110px" CssClass="Delete-Button" CommandName="DeleteButton" Visible="false" BackColor="#000000" />
                </div>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
    <asp:Panel ID="statisticsPanel" runat="server" CssClass="statistics-panel" ScrollBars="Horizontal">
        <h2>Monthly Stats</h2>
        <ul id="statsList" runat="server">
            <li>Total games played: <asp:Literal ID="totalGamesPlayed" runat="server" Text=""></asp:Literal></li>
            <li>New dungeons created: <asp:Literal ID="totalMapsCreated" runat="server" Text=""></asp:Literal></li>
            <li>Newly registered users: <asp:Literal ID="numberOfUsers" runat="server" Text=""></asp:Literal></li>
            <li id="prevList" style="display:none;"><asp:Literal ID="prevGame" runat="server"></asp:Literal></li>
            <li>User with most popular dungeons: <asp:Literal ID="mostPlayedUserMaps" runat="server"></asp:Literal></li>
            <li>Most active user: <asp:Literal ID="mostActiveUser" runat="server"></asp:Literal></li>
            <!-- Add more statistics as needed -->
        </ul>
    </asp:Panel>
    <asp:Panel ID="patchNotesPanel" runat="server" CssClass="patch-notes-panel" ScrollBars="Horizontal">
        <h2>Patch Notes</h2>
        <ul id="notesList" runat="server">
            <li>Added gameslog</li>
            <li>Added this panel</li>
            <li>Fixed bugs</li>
        </ul>
    </asp:Panel>
    <asp:Panel ID="FeaturedFeedback" runat="server" CssClass="datalist-container">
        <asp:DataList ID="FeedbackDataList" runat="server" RepeatColumns="4" CellPadding="4" RepeatDirection="Horizontal" OnItemDataBound="FeedbackDataList_ItemDataBound">
            <ItemTemplate>
                <div class="item-template">
                    <asp:Label ID="Sender" runat="server" CssClass="item-label" Text='<%# Bind("username") %>'></asp:Label> <br />
                    <asp:Image ID="ProfilePicture" runat="server" Width="75px" Height="75px" CssClass="profilePicture" ImageUrl='<%# Bind("profilePicture") %>' /> <br />
                    <asp:Label ID="Feedback" runat="server" CssClass="feedback" Text='<%# Bind("feedbackBody") %>'></asp:Label> <br />
                    <asp:Label ID="starRating" runat="server" CssClass="item-label" Text='<%# Bind("starRating") %>' Visible="false"></asp:Label>
                    <asp:PlaceHolder ID="starsPlaceHolder" runat="server"></asp:PlaceHolder>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </asp:Panel>
</asp:Content>