<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Gamelog.aspx.cs" Inherits="DungeonMaker.Gamelog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="assets/styles/Gamelog.css" />
    <script src="scripts/jquery-3.7.1.js"></script>
    <div class="datalist-container">
        <asp:Label ID="EmptyLabel" runat="server" CssClass="label"></asp:Label>
        <asp:DataList ID="GamesDataList" runat="server" OnItemDataBound="GamesDataList_ItemDataBound" >
            <ItemTemplate>
                <div class="maps-template">
                    &nbsp; &nbsp;
                    <asp:Label ID="Victory" runat="server" Text='<%# Bind("victory") %>' Font-Bold="true" CssClass="other-labels" ></asp:Label> <br /> <br />
                    <asp:Label ID="MapLabel" runat="server" Text="| Dungeon" Font-Bold="true" CssClass="other-labels"></asp:Label>
                    <asp:Label ID="Title" runat="server" Text='<%# Bind("mapName") %>' CssClass="other-labels"></asp:Label> <br /> <br />
                    <asp:Label ID="DateLabel" runat="server" Text="| Date" Font-Bold="true" CssClass="other-labels"></asp:Label>
                    <asp:Label ID="datePlayed" runat="server" Text='<%# Bind("datePlayed") %>' CssClass="other-labels"></asp:Label> <br />
                    <asp:Label ID="DeathsLabel" runat="server" Text="| Deaths" Font-Bold="true" CssClass="other-labels"></asp:Label>
                    <asp:Image ID="Skull" runat="server" ImageUrl="assets/ui/skull.png" Width="32px" Height="32px" CssClass="other-labels"/>
                    <asp:Label ID="DeathCounter" runat="server" Text='<%# Bind("deathCount") %>' CssClass="other-labels"></asp:Label> <br />
                    <asp:Label ID="StarsLabel" runat="server" Text="| Stars" Font-Bold="true" CssClass="other-labels"></asp:Label>
                    <asp:Image ID="Star" runat="server" ImageUrl="assets/ui/fullStar.png" Width="32px" Height="32px" CssClass="other-labels"/>
                    <asp:Label ID="StarCounter" runat="server" Text='<%# Bind("starsCollected") %>' CssClass="other-labels"></asp:Label> <br /> <br />
                    <asp:Label ID="TimeLabel" runat="server" Text="| Time" Font-Bold="true" CssClass="other-labels"></asp:Label>
                    <asp:Label ID="TimeElapsed" runat="server" Text='<%# Bind("timeElapsed") %>' CssClass="other-labels"></asp:Label>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
    <script>
        $(document).ready(function () {
            $(".maps-template").css("opacity", "0");
            $(".maps-template").each(function (index) {
                $(this).css("animation", "slideIn 1s ease " + (index * 0.15) + "s forwards");
                /*$(this).delay(250 * index).animate({ opacity: 1 }, 1000);*/
            });
        });
    </script>
</asp:Content>