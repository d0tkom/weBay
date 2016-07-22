<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Search.aspx.vb" Inherits="Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Styles/StyleSheet.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtTitle').focus(); //put cursor into searchbox right after page loads
            $("#txtTitle").val($("#txtTitle").val()); //push cursor at the end of the word
        });
        function txtTitleChanged() {
            $.ajax({
                type: "POST",
                url: "Search.aspx/txtTitleChanged",
                data: '{Title: "' + txtTitle.value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onSuccess,
                failure: function (AjaxResponse) {
                    alert("Failed: " + AjaxResponse.d);
                }
            });
        }
        function onSuccess(AjaxResponse) {
            $('#ContentPlaceHolder1_DataList1').hide() //hide previous results
            var availableTags = [];
            var listItems = "";
            var jsonList = AjaxResponse.d;
            for (var i = 0; i < jsonList.length; i++) {
                //add results to a string as html
                listItems += "<a href='Item.aspx?ItemID=" + jsonList[i].ItemID + "'><img height='100px' class='categoryImage' src='" + jsonList[i].ItemPicture.substring(2) + "'/></a><br/><a href='Item.aspx?ItemID=" + jsonList[i].ItemID + "'>" + jsonList[i].ItemTitle + "</a><br/><b>£" + jsonList[i].ItemMaxOfBid + "</b><br/>" + jsonList[i].ItemCountOfBid + " bids<br/>Condition: " + jsonList[i].ItemCondition + "<br/><br/><br/>";
                //add search result to an array as well for autocomplete
                availableTags.push(jsonList[i].ItemTitle);
            }
            $("#mySpan").html(listItems) //populate span with results
            $("#txtTitle").autocomplete({ //populate autocomplete
                source: availableTags,
            });
        }
    </script>
    <asp:Panel runat="server" ID="panel3">
        <span id="mySpan"></span> <!-- this span holds the ajax search results-->
    </asp:Panel>
    <!-- this datalist shows the search results from normal search, by using query strings-->
    <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" Width="845px"> 
        <ItemTemplate>
            <a href="Item.aspx?ItemID=<%# Eval("ID")%>"><asp:Image class="categoryImage" ID="PictureLabel" runat="server" height="100px" ImageUrl='<%# Eval("Picture") %>' /></a>
            <br />
            <a href="Item.aspx?ItemID=<%# Eval("ID")%>">
            <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a>
            <br />
            <b>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></b>
            <br />
            <asp:Label ID="CountOfBidLabel" runat="server" Text='<%# Eval("CountOfBid") %>' /> bids
            <br />
            Condition:
            <asp:Label ID="ConditionLabel" runat="server" Text='<%# Eval("Condition")%>' />
        </ItemTemplate>
    </asp:DataList>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [Title] FROM [qryCategoryActive]"></asp:SqlDataSource>
</asp:Content>

