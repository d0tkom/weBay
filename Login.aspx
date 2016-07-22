<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="Styles/StyleSheet.css" rel="stylesheet" />
    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnLogin" Style="display: inline">
        <div id="wrapperCenter">
            <table style="margin: 5px auto; width: 585px; border-collapse: separate; border-spacing: 0.5em;">
                <tr>
                    <td class="auto-style1">
                        <asp:RequiredFieldValidator ID="rfvUsername0" runat="server" ControlToValidate="tbUsername" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        Username</td>
                    <td class="auto-style6">
                        <asp:TextBox ID="tbUsername" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <asp:RequiredFieldValidator ID="rfvPassword0" runat="server" ControlToValidate="tbPassword" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        Password</td>
                    <td class="auto-style8">
                        <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="auto-style6" style="text-align: center;">
                        <asp:Button class="button" ID="btnLogin" runat="server" Text="Login" />
                    </td>
                </tr>
                <tr runat="server" visible="false" id="trError">
                    <td colspan="2" style="text-align: center;">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                <tr>
                    <td class="auto-style1">Stay logged in</td>
                    <td class="auto-style6">
                        <asp:CheckBox ID="cbStayLoggedIn" runat="server" /><span>&nbsp(You need to enable cookies for this)</span>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Don't have an account?&nbsp</td>
                    <td class="auto-style6">
                        <a href="SignUp.aspx">Register</a>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>

