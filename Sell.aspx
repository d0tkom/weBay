<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Sell.aspx.vb" Inherits="Sell" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #divDetails {
            height: 22px;
            width: 1280px;
        }

        .auto-style1 {
            width: 334px;
        }

        .auto-style2 {
            width: 334px;
            height: 168px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <script type="text/javascript">
        $(document).ready(function () {
            var ddlCategories = document.getElementById("ContentPlaceHolder1_ddlCategories");
            var ddlSubCategories = document.getElementById("ContentPlaceHolder1_ddlSubCategories");
            var ddlCondition = document.getElementById("ContentPlaceHolder1_ddlCondition");
            var lblCategoryHidden = document.getElementById("ContentPlaceHolder1_lblCategoryHidden");
            var lblSubCategoryHidden = document.getElementById("ContentPlaceHolder1_lblSubCategoryHidden");
            var lblConditionHidden = document.getElementById("ContentPlaceHolder1_lblConditionHidden");
            if (lblCategoryHidden.innerHTML != "") { 
                ddlCategories.selectedIndex = parseInt(lblCategoryHidden.innerHTML)-1; //if there is hidden value, set selected item's index to the correct one
            }
            ddlCondition.selectedIndex = lblConditionHidden.innerHTML;
            var length = ddlSubCategories.options.length;
            for (var i = 1; i < length; i++) {
                if (ddlSubCategories.options[i].value == parseInt(lblSubCategoryHidden.innerHTML)) {
                    ddlSubCategories.selectedIndex = i; //loop through subcategories and select the one that matches the hidden value
                    return;
                }
            }
        })
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel DefaultButton="btnChange" runat="server">
    <div id="divCategories" style="margin-top: 5px;">
        <asp:UpdatePanel ID="upCategories" runat="server">
            <ContentTemplate>                
                <asp:DropDownList ID="ddlCategories" runat="server" DataSourceID="SqlDataSource1" DataTextField="Category" DataValueField="ID" AppendDataBoundItems="True" AutoPostBack="True">
                    <asp:ListItem Value="Select a Category">Select a Category</asp:ListItem>
                </asp:DropDownList>
                <asp:RegularExpressionValidator ID="revCategories" runat="server" ControlToValidate="ddlCategories" ErrorMessage="*" ForeColor="Red" ValidationExpression="^(?!.*Select).*$"></asp:RegularExpressionValidator>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT * FROM [tblCategories]"></asp:SqlDataSource>
                <asp:DropDownList ID="ddlSubCategories" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource2" DataTextField="subCategory" DataValueField="ID">
                    <asp:ListItem Value="Select a Sub Category">Select a Sub Category</asp:ListItem>
                </asp:DropDownList>
                <asp:RegularExpressionValidator ID="rfvSubCategories" runat="server" ControlToValidate="ddlSubCategories" ErrorMessage="*" ForeColor="Red" ValidationExpression="^(?!.*Select).*$"></asp:RegularExpressionValidator>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT [subCategory], [ID] FROM [tblSubCategories]"></asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divDetails">
        <table style="float: left; border-collapse:separate; border-spacing:0.5em;">
            <tr>
                <td>Title
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="*" ControlToValidate="tbTitle" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tbTitle" runat="server" MaxLength="250" placeholder="Try to be concise and precise" Width="100%"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Condition 
        <asp:RegularExpressionValidator ID="rfvCondition" runat="server" ControlToValidate="ddlCondition" ErrorMessage="*" ForeColor="Red" ValidationExpression="^(?!.*Select).*$"></asp:RegularExpressionValidator>
                </td>
                <td class="auto-style1">
                    <asp:DropDownList ID="ddlCondition" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource3" DataTextField="Condition" DataValueField="ID">
                        <asp:ListItem>Select Category</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectString1217790 %>" ProviderName="<%$ ConnectionStrings:ConnectString1217790.ProviderName %>" SelectCommand="SELECT * FROM [tblConditions]"></asp:SqlDataSource>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Keywords
                    <asp:RequiredFieldValidator ID="rfvKeyWords" runat="server" ErrorMessage="*" ControlToValidate="tbKeyWords" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="tbKeyWords" runat="server" MaxLength="250" placeholder="Keywords, separated with a single space" Width="100%"></asp:TextBox></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Description
                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="*" ControlToValidate="tbDescription" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="tbDescription" runat="server" MaxLength="500" placeholder="Just a short description of what you're selling" TextMode="MultiLine" Width="100%" Height="100%"></asp:TextBox></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>Image
                    <asp:RequiredFieldValidator ID="rfvImage" runat="server" ControlToValidate="fuImage" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:FileUpload ID="fuImage" runat="server" onclick="btnFileUpload_Click"/>
                </td>
                <td>&nbsp</td>
            </tr>
            <tr><td>

                </td>
                <td>
                    <span>(jpg, max 4MB)</span>
                </td>
            </tr>
            <tr><td></td>
                <td><asp:Image width="100px" ID="imgImage" runat="server" /></td></tr>
            
        </table>
        <table style="float: left; border-collapse:separate; border-spacing:0.5em;">
            <tr>
                <td>Starting price
                    <asp:RequiredFieldValidator ID="rfvStartingPrice" runat="server" ControlToValidate="tbStartingPrice" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="tbStartingPrice" runat="server" placeholder="(GBP)"></asp:TextBox>
                    <asp:Label ID="lblStartingPrice" runat="server" Text="" Visible="false"></asp:Label>
                </td>
                <td><asp:RegularExpressionValidator ID="revStartingPrice" runat="server" ErrorMessage="Only whole numbers" ControlToValidate="tbStartingPrice" ValidationExpression="^\d+$" ForeColor="Red"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td>Reserve price</td>
                <td>
                    <asp:TextBox ID="tbReservePrice" runat="server" placeholder="(optional)"></asp:TextBox>
                </td>
                 <td><asp:RegularExpressionValidator ID="revReservePrice" runat="server" ErrorMessage="Only whole numbers" ControlToValidate="tbReservePrice" ValidationExpression="^\d+$" ForeColor="Red"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td>Bid increment<asp:RequiredFieldValidator ID="rfvIncrement" runat="server" ControlToValidate="tbBidIncrement" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="tbBidIncrement" runat="server" placeholder="(GBP)"></asp:TextBox>
                </td>
                 <td><asp:RegularExpressionValidator ID="revBidIncrement" runat="server" ErrorMessage="Only whole numbers" ControlToValidate="tbBidIncrement" ValidationExpression="^\d+$" ForeColor="Red"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Button class="button" ID="btnSubmit" runat="server" Text="Submit" />
                    <asp:Button class="button" ID="btnCancel" runat="server" Text="Back" Visible="False" />
                    <asp:Button class="button" ID="btnChange" runat="server" Text="Change" Visible="False" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:Label style="color: white;" ID="lblCategoryHidden" runat="server" Text=""></asp:Label>
    <asp:Label style="color: white;" ID="lblSubCategoryHidden" runat="server" Text=""></asp:Label>
    <asp:Label style="color: white;" ID="lblConditionHidden" runat="server" Text=""></asp:Label>
        </asp:Panel>
</asp:Content>

