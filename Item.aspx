<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Item.aspx.vb" Inherits="Item" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        //run script when page loaded
        $(document).ready(function () {
            fixTable();
        });
        //this javascript function add's the £ sign to the price column in the bid history gridview
        function fixTable() {
            var i = 1;
            var temp = ""
            while (document.getElementById("ContentPlaceHolder1_GridView1").rows[i].cells[1]) {
                var temp = document.getElementById("ContentPlaceHolder1_GridView1").rows[i].cells[1].innerHTML;
                temp = "£" + temp
                document.getElementById("ContentPlaceHolder1_GridView1").rows[i].cells[1].innerHTML = temp;
                i++;
            }
        }
    </script>
    <!-- datalist conatining the main page for bidding on items. any time it only ever shows one item-->

    <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1">
        <ItemTemplate>
            <br />
            <b>
                <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></b>
            <br />
            <asp:Image ID="PictureLabel" class="categoryImage" Width="150px" runat="server" ImageUrl='<%# Eval("Picture") %>' />
            <br />
            <table>
                <tr>
                    <td>Seller:</td>
                    <td><a href="User.aspx?UserID=<%# Eval("SellerID")%>">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserName") %>' /></a></td>
                </tr>
                <tr>
                    <td>Bids:</td>
                    <td><a href="Item.aspx?ItemID=<%# Eval("ItemID")%>&History=True">
                        <asp:Label ID="CountOfBid" runat="server" Text='<%# Eval("CountOfBid")%>' /></a></td>
                </tr>
                <tr>
                    <td>Item condition:</td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Condition") %>' /></td>
                </tr>
                <tr>
                    <td>Date uploaded:</td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("DateUploaded") %>' /></td>
                </tr>
                <tr>
                    <td>Starting price:</td>
                    <td>£<asp:Label ID="StartingPriceLabel" runat="server" Text='<%# Eval("StartingPrice") %>' /></td>
                </tr>
                <tr>
                    <td>Min bid increment:</td>
                    <td>£<asp:Label ID="BidIncrementLabel" runat="server" Text='<%# Eval("BidIncrement") %>' /></td>
                </tr>
                <tr>
                    <td>Current price:</td>
                    <td><b>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></b></td>
                </tr>
                <tr>
                    <td>&nbsp</td>
                    <td><asp:Button class="button" ID="btnAddToWatchlist" CommandName="btnAddToWatchlist" runat="server" Text="Add to watchlist" CausesValidation="False" /><asp:Button class="button" ID="btnRemoveFromWatchlist" CommandName="btnRemoveFromWatchlist" runat="server" Text="Remove from watchlist" CausesValidation="False" /></td>
                </tr>
            </table>
            <br />
            <table class="tblBid">
                <tr>
                    <td></td>
                    <td><asp:Label ID="lblMaxBid" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td>Insert new maximum bid: </td>
                    <td>
                        <asp:Panel runat="server" DefaultButton="btnSendBid"><asp:TextBox ID="tbMaximumBid" placeholder="GBP" runat="server"></asp:TextBox></asp:Panel></td>
                    <td>
                        <asp:Button class="button" ID="btnSendBid" runat="server" CommandName="btnSendBid" Text="Send bid" CommandArgument="" OnClientClick="return confirm('Are you sure? You cannot change this later!')"/></td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvMaximumBid" runat="server" ErrorMessage="Please enter a bid" ControlToValidate="tbMaximumBid"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revMaximumBid" runat="server" ErrorMessage="Please enter a whole number" ControlToValidate="tbMaximumBid" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="2">
                        <asp:Label ID="lblSendBid" runat="server" Text=""></asp:Label></td>
                    <td></td>
                </tr>
            </table>
            <br />
            Description:
            <br />
            <div style="overflow: auto; border-style: solid; border-width: 1px; margin: 10px; padding: 10px;">
                <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>' />
                <br />
            </div>
        </ItemTemplate>
    </asp:DataList>
    <!-- The following datalist is used when the item is closed already -->
    <asp:DataList ID="DataList3" runat="server" DataSourceID="SqlDataSource4">
        <ItemTemplate>
            <br />
            <b>
                <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></b>
            <br />
            <asp:Image ID="PictureLabel" class="categoryImage" Width="150px" runat="server" ImageUrl='<%# Eval("Picture") %>' />
            <br />
            <table>
                <tr>
                    <td>Seller:</td>
                    <td><a href="User.aspx?UserID=<%# Eval("SellerID")%>">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("UserName") %>' /></a></td>
                </tr>
                <tr>
                    <td>Bids:</td>
                    <td><a href="Item.aspx?ItemID=<%# Eval("ItemID")%>&History=True">
                        <asp:Label ID="CountOfBid" runat="server" Text='<%# Eval("CountOfBid")%>' /></a></td>
                </tr>
                <tr>
                    <td>Item condition:</td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Condition") %>' /></td>
                </tr>
                <tr>
                    <td>Date uploaded:</td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("DateUploaded") %>' /></td>
                </tr>
                <tr>
                    <td>Date closed:</td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("DateClosed") %>' /></td>
                </tr>
                <tr>
                    <td>Starting price:</td>
                    <td>£<asp:Label ID="StartingPriceLabel" runat="server" Text='<%# Eval("StartingPrice") %>' /></td>
                </tr>
                <tr>
                    <td>Min bid increment:</td>
                    <td>£<asp:Label ID="BidIncrementLabel" runat="server" Text='<%# Eval("BidIncrement") %>' /></td>
                </tr>
                <tr>
                    <td>Final bid:</td>
                    <td><b>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></b></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnAddToWatchlist" CommandName="btnAddToWatchlist" runat="server" Text="Add to watchlist" CausesValidation="False" />
                        <asp:Button class="button" ID="btnRemoveFromWatchlist" CommandName="btnRemoveFromWatchlist" runat="server" Text="Remove from watchlist" CausesValidation="False" /></td>
                </tr>
            </table>
            <br />
            <p style="color: red; text-align: center; font-size: 30px;"><b>Item closed</b></p>
            Description:
            <br />
            <div style="overflow: auto; border-style: solid; border-width: 1px; margin: 10px; padding: 10px;">
                <asp:Label ID="DescriptionLabel" runat="server" Text='<%# Eval("Description") %>' />
                <br />
            </div>
        </ItemTemplate>
    </asp:DataList>
    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT * FROM [qryItem]"></asp:SqlDataSource>
    <!-- the next div is only ever showed, when the user wants to see the bid history of the current item -->
    <div name="history" id="divBidHistory" runat="server" visible="False">
        <p><b>Bid history</b></p>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource3" Visible="True" AutoGenerateColumns="False" CellPadding="1" CellSpacing="1" CssClass="tblBidHistory" GridLines="None" HorizontalAlign="Left">
            <Columns>
                <asp:BoundField HeaderText="User" DataField="UserName" />
                <asp:BoundField HeaderText="Bid" DataField="Bid" />
                <asp:BoundField HeaderText="Time of bid" DataField="TimeOfBid" />
            </Columns>
        </asp:GridView>
    </div>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [UserName], [Bid], [TimeOfBid], [UserID] FROM [qryBidHistory]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [Title], [Picture], [UserName], [Condition], [Description], [MaxOfBid], [BidIncrement], [StartingPrice], [DateUploaded], [SellerID], [CountOfBid], [ItemID] FROM [qryItemActive]"></asp:SqlDataSource>
</asp:Content>

