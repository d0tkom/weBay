<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="User.aspx.vb" Inherits="User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <script type="text/javascript">
            //function to change url based on which page user was watching inside user.aspx
        function changeUrl() {
            var state = {};
            var title = "";
            var url = window.location.search.split("&",1)+"&";
            if (document.getElementById("ContentPlaceHolder1_rbBuying").checked) {
                url += "Page=Buying&"
            }
            else if (document.getElementById("ContentPlaceHolder1_rbSelling").checked) {
                url += "Page=Selling&"
            }
            if (document.getElementById("ContentPlaceHolder1_cbDisplayPositive").checked) {
                url += "Positive=True&"
            }
            else {
                url += "Positive=False&"
            }
            if (document.getElementById("ContentPlaceHolder1_cbDisplayNegative").checked) {
                url += "Negative=True&"
            }
            else {
                url += "Negative=False&"
            }
            if (document.getElementById("ContentPlaceHolder1_cbDisplayNeutral").checked) {
                url += "Neutral=True&"
            }
            else {
                url += "Neutral=False&"
            }
            history.pushState(state, title, url);
        }
    </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:DataList ID="dlUser" runat="server" DataKeyField="ID" DataSourceID="SqlDataSource3">
                        <ItemTemplate>
                            <b>User: 
                            <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>' />
                            <span>&nbsp</span>
                            Date joined: 
                            <asp:Label ID="DateJoinedLabel" runat="server" Text='<%# Eval("DateJoined") %>' /></b>
                        </ItemTemplate>
                    </asp:DataList>
                    <table class="tblUserRating">
                        <tr>
                            <td></td>
                            <td style="color: red;">Negative (-1)</td>
                            <td>Neutral (0)</td>
                            <td style="color: green;">Positive (+1)</td>
                            <td>Total</td>
                            <td>Average</td>
                        </tr>
                        <tr>
                            <td>Seller ratings</td>
                            <td style="color: red;">
                                <asp:Label ID="lblNegativeSeller" runat="server" Text=""></asp:Label></td>
                            <td>
                                <asp:Label ID="lblNeutralSeller" runat="server" Text=""></asp:Label></td>
                            <td style="color: green;">
                                <asp:Label ID="lblPositiveSeller" runat="server" Text=""></asp:Label></td>
                            <td>
                                <asp:Label ID="lblSellerTotal" runat="server" Text=""></asp:Label></td>
                            <td>
                                <asp:Label ID="lblSellerAverage" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Buyer ratings</td>
                            <td style="color: red;">
                                <asp:Label ID="lblNegativeBuyer" runat="server" Text=""></asp:Label></td>
                            <td>
                                <asp:Label ID="lblNeutralBuyer" runat="server" Text=""></asp:Label></td>
                            <td style="color: green;">
                                <asp:Label ID="lblPositiveBuyer" runat="server" Text=""></asp:Label></td>
                            <td>
                                <asp:Label ID="lblBuyerTotal" runat="server" Text=""></asp:Label></td>
                            <td>
                                <asp:Label ID="lblBuyerAverage" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Total</b></td>
                            <td style="color: red;">
                                <b><asp:Label ID="lblTotalNegative" runat="server" Text=""></asp:Label></b></td>
                            <td>
                                <b><asp:Label ID="lblTotalNeutral" runat="server" Text=""></asp:Label></b></td>
                            <td style="color: green;">
                                <b><asp:Label ID="lblTotalPositive" runat="server" Text=""></asp:Label></b></td>
                            <td>
                                <b><asp:Label ID="lblTotal" runat="server" Text=""></asp:Label></b></td>
                            <td>
                                <b><asp:Label ID="lblTotalAverage" runat="server" Text=""></asp:Label></b></td>
                        </tr>
                    </table>
                <br />
                <div style="float: left;">
                    <br />
                  <asp:CheckBox ID="cbDisplayNegative" runat="server" Text="Display negative" OnClick="changeUrl()" AutoPostBack="True" Checked="True" />
                <br />
                    <asp:CheckBox ID="cbDisplayNeutral" runat="server" Text="Display neutral" OnClick="changeUrl()" AutoPostBack="True" Checked="True" />
                <br />
                    <asp:CheckBox ID="cbDisplayPositive" runat="server" Text="Display positive" OnClick="changeUrl()" AutoPostBack="True" Checked="True" />
                    </div>
                &nbsp<asp:RadioButton Text="Buying" ID="rbBuying" GroupName="BuyingOrSelling" runat="server" OnClick="changeUrl()" AutoPostBack="True" />
                    <asp:RadioButton Text="Selling" ID="rbSelling" GroupName="BuyingOrSelling" runat="server" OnClick="changeUrl()" AutoPostBack="True" Checked="True" />
                <br />
                    <asp:DataList ID="dlSeller" runat="server" DataSourceID="SqlDataSource1">
                        <ItemTemplate>
                        <table class="tblFeedback">
                            <tr>
                                <td>Buyer:</td>
                                <td><a href="User.aspx?UserID=<%# Eval("BidderUserID")%>"><asp:Label ID="BidderUserNameLabel" runat="server" Text='<%# Eval("BidderUserName") %>' /></a></td>
                                <td rowspan="3" class="tdFeedback" ><b>Feedback:</b> <br /> <asp:Label ID="SellerFeedbackLabel" runat="server" Text='<%# Eval("SellerFeedback") %>' /></td>
                            </tr>
                            <tr>
                                <td>For item:</td>
                                <td class="tdFeedbackTitle"><a href="Item.aspx?ItemID=<%# Eval("ItemID")%>"><asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a></td>
                            </tr>
                            <tr>
                               <td>Rating:</td>
                                <td><asp:Label ID="SellerRatingLabel" runat="server" Text='<%# Eval("Rating")%>' /></td>
                                <td></td>
                            </tr>
                        </table>
                        <br />             
                        </ItemTemplate>
                    </asp:DataList>
                    <asp:DataList ID="dlBuyer" runat="server" DataSourceID="SqlDataSource2" Visible="False">
                        <ItemTemplate>
                        <table class="tblFeedback">
                            <tr>
                                <td>Seller:</td>
                                <td><a href="User.aspx?UserID=<%# Eval("SellerID")%>"><asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName")%>' /></a></td>
                                <td rowspan="3" class="tdFeedback" ><b>Feedback:</b> <br /> <asp:Label ID="BuyerFeedbackLabel" runat="server" Text='<%# Eval("BuyerFeedback")%>' /></td>
                            </tr>
                            <tr>
                                <td>For item:</td>
                                <td class="tdFeedbackTitle"><a href="Item.aspx?ItemID=<%# Eval("ItemID")%>"><asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a></td>
                            </tr>
                            <tr>
                               <td>Rating:</td>
                                <td><asp:Label ID="BuyerRatingLabel" runat="server" Text='<%# Eval("Rating")%>' /></td>
                                <td></td>
                            </tr>
                        </table>
                        <br />  
                        </ItemTemplate>
                    </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [ID], [UserName], [DateJoined] FROM [tblUser]"></asp:SqlDataSource>
</asp:Content>

