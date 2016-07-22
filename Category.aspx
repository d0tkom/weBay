<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Category.aspx.vb" Inherits="Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="wrapper">
        <div>
            <asp:Label ID="lblLocation" runat="server" Text=""></asp:Label></div>
        <asp:DataList class="break" ID="DataList1" runat="server" DataSourceID="SqlDataSource1" Width="845px">
            <ItemTemplate>
                <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                    <asp:Image class="categoryImage" ID="PictureLabel" runat="server" Width="100px" ImageUrl='<%# Eval("Picture") %>' /></a>
                <br />
                <a href="Item.aspx?ItemID=<%# Eval("ItemID")%>">
                    <asp:Label ID="TitleLabel" runat="server" Text='<%# Eval("Title") %>' /></a>
                <br />
                <b>£<asp:Label ID="MaxOfBidLabel" runat="server" Text='<%# Eval("MaxOfBid") %>' /></b>
                <br />
                <asp:Label ID="CountOfBidLabel" runat="server" Text='<%# Eval("CountOfBid") %>' />
                bids
            <br />
                Condition:
            <asp:Label ID="ConditionLabel" runat="server" Text='<%# Eval("Condition")%>' />
            </ItemTemplate>
        </asp:DataList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [Title], [MaxOfBid], [CountOfBid], [Picture], [Condition], [ItemID] FROM [qryCategoryActive]"></asp:SqlDataSource>
    </div>
</asp:Content>

