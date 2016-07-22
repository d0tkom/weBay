<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="MyAccount.aspx.vb" Inherits="MyAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        window.onpopstate = function (event) { //function that reloads page if user presses back button while still on MyAccount.aspx
            if (event && event.state) {        // taken from: http://stackoverflow.com/questions/14824766/history-pushstate-not-working
                location.reload();
            }
        }
        function pageLoad() { //call these functions everytime there is an ajax refresh on the page
            hideCloseButton();
            hideLeaveButtonHistory();
            hideLeaveButtonSeller();
            hideChangeDetailsButton();
            hideBuyerContact();
            hideDeleteButton();
            fixZeroReservationPrice()
        }
        function fixZeroReservationPrice() {
            var i = 0;
            while (document.getElementById("ContentPlaceHolder1_dlSelling_ReservePriceLabel_" + i)) {
                if ($("#ContentPlaceHolder1_dlSelling_ReservePriceLabel_" + i).text() == "0.0000") {
                    document.getElementById("ContentPlaceHolder1_dlSelling_ReservePriceLabel_" + i).innerHTML = 0;
                }
                i++
            }
        }
        function hideDeleteButton() { //hide close buttons where there is DateClosed  value
            var i = 0;
            while (document.getElementById("ContentPlaceHolder1_dlSelling_btnDelete_" + i)) {
                if ($("#ContentPlaceHolder1_dlSelling_DateClosed_" + i).text() != "") {
                    $("#ContentPlaceHolder1_dlSelling_btnDelete_" + i).hide();
                }
                i++
            }
        }
        function hideBuyerContact() { //hide close buttons where there is DateClosed  value
            var i = 0;
            while (document.getElementById("ContentPlaceHolder1_dlSelling_btnContact_" + i)) {
                if ($("#ContentPlaceHolder1_dlSelling_DateClosed_" + i).text() == "") {
                    $("#ContentPlaceHolder1_dlSelling_btnContact_" + i).hide();
                }
                i++
            }
        }
        function hideCloseButton() { //hide close buttons where there is DateClosed  value
            var i = 0;
            while (document.getElementById("ContentPlaceHolder1_dlSelling_btnCloseBid_" + i)) {
                if ($("#ContentPlaceHolder1_dlSelling_DateClosed_" + i).text() != "") {
                    $("#ContentPlaceHolder1_dlSelling_btnCloseBid_" + i).hide();
                }
                i++
            }
        }
        function hideLeaveButtonHistory() { //hide LeaveFeedback buttons where there is DateFeedback left value on purchase history page
            var i = 0;
            while (document.getElementById("ContentPlaceHolder1_dlHistory_btnFeedback_" + i)) {
                if ($("#ContentPlaceHolder1_dlHistory_DateSellerFeedbackLabel_" + i).text() != "") {
                    $("#ContentPlaceHolder1_dlHistory_btnFeedback_" + i).hide();
                }
                i++
            }
        }
        function hideLeaveButtonSeller() {  //hide LeaveFeedback buttons where there is DateFeedback left value on purchase history page
            var i = 0;
            while (document.getElementById("ContentPlaceHolder1_dlSelling_btnFeedback_" + i)) {
                if (($("#ContentPlaceHolder1_dlSelling_DateBuyerFeedbackLabel_" + i).text() != "") || $("#ContentPlaceHolder1_dlSelling_DateClosed_" + i).text() == "") { //hide button when item still open as well
                    $("#ContentPlaceHolder1_dlSelling_btnFeedback_" + i).hide();
                }
                i++
            }
        }
        function hideChangeDetailsButton() { //hide ChangeDetails button when item already closed
            var i = 0;
            while (document.getElementById("ContentPlaceHolder1_dlSelling_btnChangeDetails_" + i)) {
                if ($("#ContentPlaceHolder1_dlSelling_DateClosed_" + i).text() != "") {
                    $("#ContentPlaceHolder1_dlSelling_btnChangeDetails_" + i).hide();
                }
                i++
            }
        }
        function changeUrl(x) { //this function is responsible for maintaining the query strings in the browser history, so users will be taken back to the same page when they use back button
            var state = {};
            var title = "";
            var url = '?';

            if (x == "Customise") {
                url += "Tab=Customise&";
            }
            else if (x == "Details") {
                url += "Tab=Details&";
            }
            else if (x == "Purchase") {
                url += "Tab=Purchase&";
            }
            else if (x == "Notification") {
                url += "Tab=Notification&";
            }
            else if (x == "Feedback" || document.getElementById("ContentPlaceHolder1_btnMyFeedback").className == "buttonWhite") {
                url += "Tab=Feedback&";
            }
            else if (x == "Items" || document.getElementById("ContentPlaceHolder1_btnMyItems").className == "buttonWhite") {
                url += "Tab=Items&";
            }
            else if (x == "Purchase" || document.getElementById("ContentPlaceHolder1_btnPurchaseHistory").className == "buttonWhite") {
                url += "Tab=Purchase&";
            }

            if (document.getElementById("ContentPlaceHolder1_rbBuying") != null && document.getElementById("ContentPlaceHolder1_rbBuying").checked) {
                url += "Page=Buying&";
            }
            else if (document.getElementById("ContentPlaceHolder1_rbSelling") != null && document.getElementById("ContentPlaceHolder1_rbSelling").checked) {
                url += "Page=Selling&";
            }
            else if (document.getElementById("ContentPlaceHolder1_rbWatching") != null && document.getElementById("ContentPlaceHolder1_rbWatching").checked) {
                url += "Page=Watching&";
            }

            if (document.getElementById("ContentPlaceHolder1_cbDisplayActiveItems") != null && document.getElementById("ContentPlaceHolder1_cbDisplayActiveItems").checked) {
                url += "Active=True&";
            }
            else {
                url += "Active=False&";
            }

            if (document.getElementById("ContentPlaceHolder1_cbDisplayInactiveItems") != null && document.getElementById("ContentPlaceHolder1_cbDisplayInactiveItems").checked) {
                url += "Inactive=True&";
            }
            else {
                url += "Inactive=False&";
            }
            if (document.getElementById("ContentPlaceHolder1_cbDisplayPositive") != null && document.getElementById("ContentPlaceHolder1_cbDisplayPositive").checked) {
                url += "Positive=True&";
            }
            else {
                url += "Positive=False&";
            }
            if (document.getElementById("ContentPlaceHolder1_cbDisplayNegative") != null && document.getElementById("ContentPlaceHolder1_cbDisplayNegative").checked) {
                url += "Negative=True&";
            }
            else {
                url += "Negative=False&";
            }
            if (document.getElementById("ContentPlaceHolder1_cbDisplayNeutral") != null && document.getElementById("ContentPlaceHolder1_cbDisplayNeutral").checked) {
                url += "Neutral=True&";
            }
            else {
                url += "Neutral=False&";
            }
            if (document.getElementById("ContentPlaceHolder1_cbFeedbackLeft") != null && document.getElementById("ContentPlaceHolder1_cbFeedbackLeft").checked) {
                url += "FeedbackLeft=True&";
            }
            else {
                url += "FeedbackLeft=False&";
            }
            if (document.getElementById("ContentPlaceHolder1_cbNoFeedback") != null && document.getElementById("ContentPlaceHolder1_cbNoFeedback").checked) {
                url += "NoFeedback=True&";
            }
            else {
                url += "NoFeedback=False&";
            }
            history.pushState(state, title, url); //push history with correct query strings to browser
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="upItems" runat="server">
        <ContentTemplate>
            <table style="float: left; width: 168px; margin-top: 5px; border-collapse: separate; border-spacing: 0.125em;">
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnCustomise" runat="server" Text="Customise site" CausesValidation="False" Width="100%" OnClientClick="changeUrl('Customise');" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnChangeDetails" runat="server" Text="Change details" CausesValidation="False" Width="100%" OnClientClick="changeUrl('Details');" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnMyItems" runat="server" Text="My Items" CausesValidation="False" Width="100%" OnClientClick="changeUrl('Items');" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnPurchaseHistory" runat="server" Text="My purchase history" CausesValidation="False" Width="100%" OnClientClick="changeUrl('Purchase');" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnMyNotifications" runat="server" Text="My Notifications" CausesValidation="False" Width="100%" OnClientClick="changeUrl('Notification');" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnMyFeedback" runat="server" Text="My Feedback" CausesValidation="False" Width="100%" OnClientClick="changeUrl('Feedback');" />

                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Button class="button" ID="btnCloseAllBidding" runat="server" CausesValidation="False" Text="Close All Bidding" Width="100%" OnClientClick="return confirm('Are you sure? It will close all your items that reached the reservation limit. If you did not specify one it is £0 by default!')" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cbDisplayActiveItems" runat="server" Text="Display active items" OnClick="changeUrl()" AutoPostBack="True" Checked="True" Style="font-size: small" />
                        <asp:CheckBox ID="cbDisplayPositive" runat="server" Text="Display positive" OnClick="changeUrl()" AutoPostBack="True" Checked="True" Visible="False" Style="font-size: small" />
                        <asp:CheckBox ID="cbFeedbackLeft" runat="server" Text="Left feedback" OnClick="changeUrl()" AutoPostBack="True" Checked="True" Visible="False" Style="font-size: small" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cbDisplayInactiveItems" runat="server" Text="Display inactive items" OnClick="changeUrl()" AutoPostBack="True" Style="font-size: small" />
                        <asp:CheckBox ID="cbDisplayNeutral" runat="server" Text="Display neutral" OnClick="changeUrl()" AutoPostBack="True" Checked="True" Visible="False" Style="font-size: small" />
                        <asp:CheckBox ID="cbNoFeedback" runat="server" Text="Haven't left feedback" OnClick="changeUrl()" AutoPostBack="True" Checked="True" Visible="False" Style="font-size: small" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cbDisplayNegative" runat="server" Text="Display negative" OnClick="changeUrl()" AutoPostBack="True" Visible="false" Style="font-size: small" Checked="True" />
                    </td>
                </tr>
            </table>
            <div style="float: left; width: 561px;">
                <div style="margin-top: 5px; margin-left: 5px; text-align: center">
                    <asp:RadioButton ID="rbWatching" Text="Watching" GroupName="WatchingBuyingSelling" runat="server" OnClick="changeUrl()" AutoPostBack="True" />
                    <asp:RadioButton ID="rbBuying" runat="server" GroupName="WatchingBuyingSelling" Text="Buying" OnClick="changeUrl()" AutoPostBack="True" />
                    <asp:RadioButton ID="rbSelling" runat="server" GroupName="WatchingBuyingSelling" Text="Selling" Checked="True" OnClick="changeUrl()" AutoPostBack="True" />
                </div>
                <!-- Datalist for showing user's selling history-->
                <asp:DataList ID="dlSelling" runat="server" DataSourceID="SqlDataSource1" Width="724px">
                    <ItemTemplate>
                        <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                            <asp:Image class="categoryImage" ID="PictureLabel" runat="server" Width="100px" ImageUrl='<%# Eval("Picture") %>'></asp:Image></a>
                        <div style="overflow: auto; padding: 0px; margin: 10px;">
                            <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a>
                            <br />
                            <table>
                                <tr>
                                    <td>Last bid:</td>
                                    <td>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></td>
                                    <td>
                                        <asp:Button class="button myAccountButton" Width="100%" ID="btnChangeDetails" CommandName="ChangeDetails" CommandArgument='<%# Eval("ItemID")%>' runat="server" Text="Change details" CausesValidation="False" /></td>

                                </tr>
                                <tr>
                                    <td>Reservation price:</td>
                                    <td>£<asp:Label ID="ReservePriceLabel" runat="server" Text='<%# Eval("ReservePrice") %>' /></td>
                                    <td>
                                        <asp:Button Width="100%" class="button myAccountButton" ID="btnCloseBid" CommandName="CloseBid" CommandArgument='<%# Eval("ItemID")%>' runat="server" Text="Close bidding" CausesValidation="False" OnClientClick="return confirm('Are you sure?')"/></td>
                                    <td>
                                        <asp:Label ID="lblCloseBidCheck" runat="server" Text="" ForeColor="Red"></asp:Label></td>

                                </tr>
                                <tr>
                                    <td>Last bidder:</td>
                                    <td><a href="User.aspx?UserID=<%# Eval("BidderUserID")%>">
                                        <asp:Label ID="BidderUserNameLabel" runat="server" Text='<%# Eval("BidderUserName") %>' /></a></td>
                                    <td>
                                        <asp:Button class="button myAccountButton" ID="btnFeedback" CommandName="LeaveFeedback" CommandArgument='<%# Eval("ItemID")%>' runat="server" Text="Leave feedback" CausesValidation="False" /></td>
                                </tr>
                                <tr>
                                    <td>Time of last bid:</td>
                                    <td>
                                        <asp:Label ID="TimeOfBidLabel" runat="server" Text='<%# Eval("TimeOfBid") %>' /></td>
                                    <td><asp:Button Width="100%" CausesValidation="false" CommandName="ShowContact" CommandArgument='<%# Eval("ItemID")%>' class="button myAccountButton" ID="btnContact" runat="server" Text="Buyer Contact" /></td>
                                </tr>
                                <tr>
                                    <td>Date closed:</td>
                                    <td>
                                        <asp:Label ID="DateClosed" runat="server" Text='<%# Eval("DateClosed")%>' /></td>
                                    
                                </tr>
                                <tr>
                                    <td>Date uploaded:</td>
                                    <td>
                                        <asp:Label ID="DateUploaded" runat="server" Text='<%# Eval("DateUploaded")%>' /></td>
                                    <td> <asp:Button Width="100%" CausesValidation="false" CommandName="DeleteItem" CommandArgument='<%# Eval("ItemID")%>' class="buttonRed myAccountButton" ID="btnDelete" runat="server" Text="Delete Item" OnClientClick="return confirm('Are you sure? It will delete it permanently!')"/></td>
                                </tr>
                                <tr>
                                    <td>Date feedback left:</td>
                                    <td>
                                        <asp:Label ID="DateBuyerFeedbackLabel" runat="server" Text='<%# Eval("DateBuyerFeedback")%>' /></td>
                                </tr>
                            </table>
                        </div>
                        <div visible="false" id="divFeedback" style="background-color: #ECE0F8; text-align: center; padding: 10px; margin: 10px;" runat="server">
                            Rating: 
                                      <asp:RadioButtonList ID="rbtLstRating" runat="server"
                                          RepeatDirection="Horizontal" RepeatLayout="Table">
                                          <asp:ListItem runat="server" Text="Bad" Value="1"></asp:ListItem>
                                          <asp:ListItem runat="server" Text="Neutral" Value="2"></asp:ListItem>
                                          <asp:ListItem runat="server" Text="Good" Value="3"></asp:ListItem>
                                      </asp:RadioButtonList>
                            <br />
                            <asp:TextBox MaxLength="500" placeholder="Just a few words about the buyer" TextMode="MultiLine" Width="100%" Height="100%" ID="tbFeedback" runat="server"></asp:TextBox>
                            <br />
                            <asp:Button class="button" ID="btnCancel" runat="server" Text="Cancel" CommandName="CancelFeedback" CausesValidation="false" />
                            <asp:Button class="button" ID="btnSubmit" runat="server" Text="Submit" CommandName="SendFeedback" CommandArgument='<%# Eval("ItemID") %>' />
                            <asp:RequiredFieldValidator ControlToValidate="rbtLstRating" ControlID="RequiredFieldValidator1" runat="server" ErrorMessage="Select a rating"></asp:RequiredFieldValidator>

                            <br />
                        </div>
                        <div visible="false" id="divContact" style="background-color: #ECE0F8; text-align: center; padding: 10px; margin: 10px;" runat="server">
                            <table>
                                <tr>
                                    <td>Firstname:&nbsp</td>
                                    <td>
                                        <asp:Label ID="FirstNameLabel" runat="server" Text='<%# Eval("FirstName")%>' /></td>
                                </tr>
                                <tr>
                                    <td>Lastname:&nbsp</td>
                                    <td>
                                        <asp:Label ID="LastNameLabel" runat="server" Text='<%# Eval("LastName")%>' /></td>
                                </tr>
                                <tr>
                                    <td>Email:&nbsp</td>
                                    <td>
                                        <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email")%>' /></td>
                                </tr>
                                <tr>
                                    <td>Phone:&nbsp</td>
                                    <td>
                                        <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone")%>' /></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Button class="button" CausesValidation="false" ID="btnHide" runat="server" Text="Hide" CommandName="HideContact" />
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <!-- Datalist for showing user's buying history-->
                <asp:DataList ID="dlBuying" runat="server" DataSourceID="SqlDataSource2" Visible="False" Width="670px">
                    <ItemTemplate>
                        <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                            <asp:Image class="categoryImage" Width="100px" ID="PictureLabel" runat="server" ImageUrl='<%# Eval("Picture") %>' /></a>
                        <div style="overflow: auto; padding: 0px; margin: 10px;">
                            <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a>
                            <br />
                            <table>
                                <tr>
                                    <td>Last bid:</td>
                                    <td>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Condition:</td>
                                    <td>
                                        <asp:Label ID="ConditionLabel" runat="server" Text='<%# Eval("Condition") %>' /></td>
                                </tr>
                                <tr>
                                    <td>Seller:</td>
                                    <td><a href="User.aspx?UserID=<%# Eval("UserID")%>">
                                        <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>' /></a></td>
                                </tr>
                                <tr>
                                    <td>Time of last bid:</td>
                                    <td>
                                        <asp:Label ID="TimeOfBidLabel" runat="server" Text='<%# Eval("TimeOfBid") %>' /></td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <!-- Datalist for showing user's items on the wathcing list -->
                <asp:DataList ID="dlWatching" runat="server" DataSourceID="SqlDataSource3" Width="666px" Visible="False">
                    <ItemTemplate>
                        <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                            <asp:Image class="categoryImage" Width="100px" ID="PictureLabel" runat="server" ImageUrl='<%# Eval("Picture") %>' /></a>
                        <div style="overflow: auto; padding: 0px; margin: 10px;">
                            <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a>
                            <br />
                            <table>
                                <tr>
                                    <td>Last bid:</td>
                                    <td>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Condition:</td>
                                    <td>
                                        <asp:Label ID="ConditionLabel" runat="server" Text='<%# Eval("Condition") %>' /></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Seller:</td>
                                    <td><a href="User.aspx?UserID=<%# Eval("UserID")%>">
                                        <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>' /></a></td>
                                </tr>
                                <tr>
                                    <td>Time of last bid:</td>
                                    <td>
                                        <asp:Label ID="TimeOfBidLabel" runat="server" Text='<%# Eval("TimeOfBid") %>' /></td>
                                </tr>
                            </table>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
                <!-- Datalist for showing user's selling feedbacks -->
                <asp:DataList ID="dlFeedbackSelling" runat="server" DataSourceID="SqlDataSource4" Visible="False" Width="701px">
                    <ItemTemplate>
                        <table class="tblFeedback">
                            <tr>
                                <td>Buyer:</td>
                                <td><a href="User.aspx?UserID=<%# Eval("BidderUserID")%>">
                                    <asp:Label ID="BidderUserNameLabel" runat="server" Text='<%# Eval("BidderUserName") %>' /></a></td>
                                <td rowspan="3" class="tdFeedback"><b>Feedback:</b>
                                    <br />
                                    <asp:Label ID="SellerFeedbackLabel" runat="server" Text='<%# Eval("SellerFeedback") %>' /></td>
                            </tr>
                            <tr>
                                <td>For item:</td>
                                <td class="tdFeedbackTitle"><a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a></td>
                            </tr>
                            <tr>
                                <td>Rating:</td>
                                <td>
                                    <asp:Label ID="SellerRatingLabel" runat="server" Text='<%# Eval("Rating") %>' /></td>
                                <td></td>
                            </tr>
                        </table>
                        <br />
                    </ItemTemplate>
                </asp:DataList>
                <!-- Datalist for showing user's buying feedbacks-->
                <asp:DataList ID="dlFeedbackBuying" runat="server" DataSourceID="SqlDataSource5" Visible="False" Width="695px">
                    <ItemTemplate>
                        <table class="tblFeedback">
                            <tr>
                                <td>Seller:</td>
                                <td><a href="User.aspx?UserID=<%# Eval("SellerID")%>">
                                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName")%>' /></a></td>
                                <td rowspan="3" class="tdFeedback"><b>Feedback:</b>
                                    <br />
                                    <asp:Label ID="BuyerFeedbackLabel" runat="server" Text='<%# Eval("BuyerFeedback")%>' /></td>
                            </tr>
                            <tr>
                                <td>For item:</td>
                                <td class="tdFeedbackTitle"><a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a></td>
                            </tr>
                            <tr>
                                <td>Rating:</td>
                                <td>
                                    <asp:Label ID="BuyerRatingLabel" runat="server" Text='<%# Eval("Rating") %>' /></td>
                                <td></td>
                            </tr>
                        </table>
                        <br />
                    </ItemTemplate>
                </asp:DataList>
                <!-- div that includes the "page" for changing user details-->
                <div id="divChangeDetails" runat="server" visible="false">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSubmit">
                    <table id="tblChangeDetails" style="width: 121%; height: 250px; border-collapse: separate; border-spacing: 0.5em;">
                        <tr>
                            <td class="tdTitle">
                                <asp:RequiredFieldValidator ID="rfvEmail0" runat="server" ControlToValidate="tbEmail" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                E-mail</td>
                            <td class="auto-style7">
                                <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
                            </td>
                            <td class="auto-style5">
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="tbEmail" ErrorMessage="Please provide a real email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdTitle">Phone number</td>
                            <td class="auto-style7">
                                <asp:TextBox ID="tbPhone" runat="server"></asp:TextBox>
                            </td>
                            <td class="auto-style5">
                                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="tbPhone" ErrorMessage="Please provide a real UK phone number" ValidationExpression="^(((\+44\s?|0044\s?)?|(\(?0))((2[03489]\)?\s?\d{4}\s?\d{4})|(1[23456789]1\)?\s?\d{3}\s?\d{4})|(1[23456789][234578][0234679]\)?\s?\d{6})|(1[2579][0245][0467]\)?\s?\d{5})|(11[345678]\)?\s?\d{3}\s?\d{4})|(1[35679][234689]\s?[46789][234567]\)?\s?\d{4,5})|([389]\d{2}\s?\d{3}\s?\d{4})|([57][0-9]\s?\d{4}\s?\d{4})|(500\s?\d{6})|(7[456789]\d{2}\s?\d{6})))$" ForeColor="Red"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdTitle">New password</td>
                            <td class="auto-style7">
                                <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                            <td class="auto-style5">
                                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="tbPassword" ErrorMessage="Min 8, max 25, 1 num, 1 uppercase" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$" ForeColor="Red"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdTitle">Confirm new password</td>
                            <td class="auto-style7">
                                <asp:TextBox ID="tbPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                            <td class="auto-style6">
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tbPassword" ControlToValidate="tbPasswordConfirm" ErrorMessage="Passwords don't match" ForeColor="Red"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdTitle">
                                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="tbCurrentPassword" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                Current password</td>
                            <td class="auto-style7">
                                <asp:TextBox ID="tbCurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                            <td class="auto-style5"></td>
                        </tr>
                        <tr>
                            <td class="auto-style8">&nbsp;</td>
                            <td class="auto-style7" style="margin-top: 5px;">
                                <asp:Button class="button" ID="btnSubmit" runat="server" Text="Submit" />
                            </td>
                            <td class="auto-style6">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </asp:Panel>
                </div>
                <!-- div that includes the "page" where user can select the default image on the main page -->
                <div style="margin-top: 5px;" runat="server" id="divCustomiseSite" visible="false">
                    <table style="border-collapse: separate; border-spacing: 0.5em;">
                        <tr>
                            <td style="text-align: center;"><b>Select picture for main screen</b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image8" runat="server" Width="600px" ImageUrl="~/Pictures/WmgCompSciMath_1280.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label8" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image1" runat="server" Width="600px" ImageUrl="~/Pictures/accomodation.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label1" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image2" runat="server" Width="600px" ImageUrl="~/Pictures/aerial_shot.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label2" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image3" runat="server" Width="600px" ImageUrl="~/Pictures/conference_park.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label3" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image4" runat="server" Width="600px" ImageUrl="~/Pictures/koan.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label4" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image5" runat="server" Width="600px" ImageUrl="~/Pictures/rootes.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label5" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image6" runat="server" Width="600px" ImageUrl="~/Pictures/warwick_buildings.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label6" runat="server"></asp:Label></b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="Image7" runat="server" Width="600px" ImageUrl="~/Pictures/wmg.jpg" /></td>
                            <td><b>
                                <asp:Label ID="Label7" runat="server"></asp:Label></b></td>
                        </tr>
                    </table>
                </div>
                <!-- datalist that shows the user's purchase history -->
                <asp:DataList ID="dlHistory" runat="server" DataSourceID="SqlDataSource6" Width="654px" Visible="False" >
                    <ItemTemplate>
                        <div>
                            <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                <asp:Image class="categoryImage" Width="100px" ID="PictureLabel" runat="server" ImageUrl='<%# Eval("Picture") %>' /></a>
                            <div style="overflow: auto; padding: 0px; margin: 10px;">
                                <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a>
                                <br />
                                <table>
                                    <tr>
                                        <td>Final bid:</td>
                                        <td>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></td>
                                        <td>
                                            <asp:Button Width="100%" CausesValidation="false" CommandName="ShowContact" class="button" ID="btnContact" runat="server" Text="Seller contact" /></td>
                                    </tr>
                                    <tr>
                                        <td>Condition:</td>
                                        <td>
                                            <asp:Label ID="ConditionLabel" runat="server" Text='<%# Eval("Condition") %>' /></td>
                                        <td>
                                            <asp:Button CommandName="LeaveFeedback" class="button" ID="btnFeedback" runat="server" Text="Leave feedback" /></td>
                                    </tr>
                                    <tr>
                                        <td>Seller:</td>
                                        <td><a href="User.aspx?UserID=<%# Eval("SellerID")%>">
                                            <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>' /></a></td>
                                    </tr>
                                    <tr>
                                        <td>Time of purchase:</td>
                                        <td>
                                            <asp:Label ID="DateClosedLabel" runat="server" Text='<%# Eval("DateClosed")%>' /></td>
                                    </tr>
                                    <tr>
                                        <td>Date feedback left:</td>
                                        <td>
                                            <asp:Label ID="DateSellerFeedbackLabel" runat="server" Text='<%# Eval("DateSellerFeedback")%>' /></td>
                                    </tr>
                                </table>
                            </div>
                            <div visible="false" id="divFeedback" style="background-color: #ECE0F8; text-align: center; padding: 10px; margin: 10px;" runat="server">
                                Rating: 
                                      <asp:RadioButtonList ID="rbtLstRating" runat="server"
                                          RepeatDirection="Horizontal" RepeatLayout="Table">
                                          <asp:ListItem runat="server" Text="Bad" Value="1"></asp:ListItem>
                                          <asp:ListItem runat="server" Text="Neutral" Value="2"></asp:ListItem>
                                          <asp:ListItem runat="server" Text="Good" Value="3"></asp:ListItem>
                                      </asp:RadioButtonList>
                                <br />
                                <asp:TextBox MaxLength="500" placeholder="Just a few words about the seller" TextMode="MultiLine" Width="100%" Height="100%" ID="tbFeedback" runat="server"></asp:TextBox>
                                <br />
                                <asp:Button class="button" ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CommandName="CancelFeedback" />
                                <asp:Button class="button" ID="btnSubmit" runat="server" Text="Submit" CommandName="SendFeedback" CommandArgument='<%# Eval("ItemID") %>' />
                                <asp:RequiredFieldValidator ControlToValidate="rbtLstRating" ControlID="RequiredFieldValidator1" runat="server" ErrorMessage="Select a rating"></asp:RequiredFieldValidator>

                                <br />
                            </div>
                            <div visible="false" id="divContact" style="background-color: #ECE0F8; text-align: center; padding: 10px; margin: 10px;" runat="server">
                                <table>
                                    <tr>
                                        <td>Firstname:&nbsp</td>
                                        <td>
                                            <asp:Label ID="FirstNameLabel" runat="server" Text='<%# Eval("FirstName")%>' /></td>
                                    </tr>
                                    <tr>
                                        <td>Lastname:&nbsp</td>
                                        <td>
                                            <asp:Label ID="LastNameLabel" runat="server" Text='<%# Eval("LastName")%>' /></td>
                                    </tr>
                                    <tr>
                                        <td>Email:&nbsp</td>
                                        <td>
                                            <asp:Label ID="EmailLabel" runat="server" Text='<%# Eval("Email")%>' /></td>
                                    </tr>
                                    <tr>
                                        <td>Phone:&nbsp</td>
                                        <td>
                                            <asp:Label ID="PhoneLabel" runat="server" Text='<%# Eval("Phone")%>' /></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Button class="button" CausesValidation="false" ID="btnHide" runat="server" Text="Hide" CommandName="HideContact" />
                                        </td>
                                    </tr>

                                </table>
                            </div>
                    </ItemTemplate>
                </asp:DataList>
                <!-- datalist that shows the user's notifications -->
                <asp:DataList ID="dlNotification" runat="server" DataSourceID="SqlDataSource7" Visible="False" Width="701px">
                    <ItemTemplate>
                        <table class="tblFeedback">
                            <tr>
                                <td class="tdFeedback">Message:</td>
                                <td>
                                    <b>
                                        <asp:Label ID="MessageLabel" runat="server" Text='<%# Eval("Message") %>' /></b></td>
                                
                            </tr>
                            <tr>
                                <td>Item:</td>
                                <td><a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title")%>' /></a></td>
                            </tr>
                            <tr>
                                <td class="tdFeedback">Date:</td>
                                <td>
                                    <asp:Label ID="AddedDateLabel" runat="server" Text='<%# Eval("AddedDate")%>' /></td>
                            </tr>
                        </table>
                        <br />
                    </ItemTemplate>
                </asp:DataList>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT * FROM [qryMyAccountSelling]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountBuying]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountWatching]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [BidderUserName], [BidderUserID], [Title], [ItemID], [SellerID], [BuyerRating], [BuyerFeedback], [UserName] FROM [qryMyAccountFeedbackBuying]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT * FROM [qryMyAccountPurchaseFeedback]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT * FROM [qryNotification]"></asp:SqlDataSource>
</asp:Content>

