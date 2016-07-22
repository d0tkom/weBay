<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        //The following javascript code will put the last 5 (or less in case if there is no 10 items in the db) elements in the horizontal datalist on the main page in a new row, so it won't go off the screen
        //run code when page loaded
        $(document).ready(function () {
            //get asp.net generated table
            var table = document.getElementById("ContentPlaceHolder1_DataList1");
            //get the index of the last element in the cells array
            var length = document.getElementById("ContentPlaceHolder1_DataList1").rows[0].cells.length - 1;
            //insert second row to fill
            var row = table.insertRow(1);
            //index used in new row
            var e = 0;
            //loop trough the cells of the first row, starting form the back
            for (var i = length; i > 4; i--) {
                //insert cell into new row
                row.insertCell(e).innerHTML = table.rows[0].cells[i].innerHTML;
                //delete old cell
                table.rows[0].deleteCell(i);
                e++;
            }
        })
    </script>
    <asp:Image ID="imgMain" runat="server" ImageUrl="~/Pictures/WmgCompSciMath_1280.jpg" Width="848px" style="border-style: solid; border-width: 1px; border-color: black;"/>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [Title], [MaxOfBid], [CountOfBid], [Condition], [DateUploaded], [Picture], [ItemID] FROM [qryRandomItems]"></asp:SqlDataSource>
    <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" RepeatDirection="Horizontal">
        <ItemTemplate>
            <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>"><asp:Image class="categoryImage" ID="PictureLabel" runat="server" width="150px" ImageUrl='<%# Eval("Picture") %>' /></a>
            <br />
            <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>"><asp:Label width="150px" ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a>
            <br />
            <b>£<asp:Label ID="MaxOfBid" runat="server" Text='<%# Eval("MaxOfBid")%>' /></b>
            <br />
            <asp:Label ID="CountOfBid" runat="server" Text='<%# Eval("CountOfBid")%>' />
            bids
            <br />
            Condition:
            <asp:Label ID="ConditionLabel" runat="server" Text='<%# Eval("Condition") %>' />
            <br />
        </ItemTemplate>
    </asp:DataList>
</asp:Content>

