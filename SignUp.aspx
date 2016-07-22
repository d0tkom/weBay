<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SignUp.aspx.vb" Inherits="SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <style type="text/css">
        
        .auto-style4 {
            width: 413px;
            text-align: right;
        }
        .auto-style5 {
            font-size: small;
        }
        .auto-style6 {
            width: 635px;
        }
        .auto-style7 {
            text-align: center;
            direction: ltr;
            font-size: small;
            margin-left: 0in;
            margin-top: 0pt;
            margin-bottom: 0pt;
        }
        .auto-style8 {
            width: 413px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- more complicated regular expressions were taken from http://regexlib.com/ -->
    <table style="margin-top: 5px; width: 846px; border-collapse:separate; border-spacing:0.5em;">
        <tr>
            <td class="auto-style4"><asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="tbFirstName" ErrorMessage="*" ForeColor="Red" CssClass="auto-style5"></asp:RequiredFieldValidator>
                First name&nbsp</td>
            <td class="auto-style6">
                <asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revFirstName" runat="server" ControlToValidate="tbFirstName" ErrorMessage="Only letters, max 35 characters" ForeColor="Red" ValidationExpression="^[a-zA-Z ]{0,35}$" CssClass="auto-style5"></asp:RegularExpressionValidator>
            </td>

        </tr>
        <tr>
            <td class="auto-style4"><asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="tbLastName" ErrorMessage="*" ForeColor="Red" CssClass="auto-style5"></asp:RequiredFieldValidator>
                Last name&nbsp</td>
            <td class="auto-style6">
                <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revLastName" runat="server" ControlToValidate="tbLastName" ErrorMessage="Only letters, max 35 characters" ForeColor="Red" ValidationExpression="^[a-zA-Z ]{0,35}$" CssClass="auto-style5"></asp:RegularExpressionValidator>
            </td>

        </tr>
        <tr>
            <td class="auto-style4"><asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbEmail" ErrorMessage="*" ForeColor="Red" CssClass="auto-style5"></asp:RequiredFieldValidator>
                E-mail&nbsp</td>
            <td class="auto-style6">
                <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="tbEmail" ErrorMessage="Please provide a real email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" CssClass="auto-style5"></asp:RegularExpressionValidator>
            </td>


        </tr>
                <tr>
            <td class="auto-style4">Phone number&nbsp</td>
            <td class="auto-style6">
                <asp:TextBox ID="tbPhone" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="tbPhone" ErrorMessage="Please provide a real UK phone number" ValidationExpression="^(((\+44\s?|0044\s?)?|(\(?0))((2[03489]\)?\s?\d{4}\s?\d{4})|(1[23456789]1\)?\s?\d{3}\s?\d{4})|(1[23456789][234578][0234679]\)?\s?\d{6})|(1[2579][0245][0467]\)?\s?\d{5})|(11[345678]\)?\s?\d{3}\s?\d{4})|(1[35679][234689]\s?[46789][234567]\)?\s?\d{4,5})|([389]\d{2}\s?\d{3}\s?\d{4})|([57][0-9]\s?\d{4}\s?\d{4})|(500\s?\d{6})|(7[456789]\d{2}\s?\d{6})))$" ForeColor="Red" CssClass="auto-style5"></asp:RegularExpressionValidator>
            </td>


        </tr>
        <tr>
            <td class="auto-style4"><asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="tbUserName" ErrorMessage="*" ForeColor="Red" CssClass="auto-style5"></asp:RequiredFieldValidator>
                Username&nbsp</td>
            <td class="auto-style6">
                <asp:TextBox ID="tbUserName" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revUserName" runat="server" ControlToValidate="tbUserName" ErrorMessage="Only letters and numbers, min 3 max 35" ForeColor="Red" ValidationExpression="^[a-zA-Z0-9]{3,35}$" CssClass="auto-style5"></asp:RegularExpressionValidator>
            </td>
     

        </tr>
        <tr>
            <td class="auto-style4"><asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="tbPassword" ErrorMessage="*" ForeColor="Red" CssClass="auto-style5"></asp:RequiredFieldValidator>
                Password&nbsp</td>
            <td class="auto-style6">
                <asp:TextBox ID="tbPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="tbPassword" ErrorMessage="Min 8 characters, at least 1 num and 1 uppercase" ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$" ForeColor="Red" CssClass="auto-style5"></asp:RegularExpressionValidator>
            </td>

        </tr>
        <tr>
            <td class="auto-style4"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbPassword" ErrorMessage="*" ForeColor="Red" CssClass="auto-style5"></asp:RequiredFieldValidator>
                Confirm Password&nbsp</td>
            <td class="auto-style6">
                <asp:TextBox ID="tbPasswordConfirm" runat="server" TextMode="Password"></asp:TextBox>

                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tbPassword" ControlToValidate="tbPasswordConfirm" ErrorMessage="Passwords don't match" ForeColor="Red" CssClass="auto-style5"></asp:CompareValidator>

            </td>

        </tr>


        <tr>
            <td class="auto-style4">
              
                <span style="font-size: medium; font-family: Calibri; mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; mso-bidi-font-family: +mn-cs; mso-ascii-theme-font: minor-latin; mso-fareast-theme-font: minor-fareast; mso-bidi-theme-font: minor-bidi; color: black; mso-color-index: 1; mso-font-kerning: 12.0pt; language: en-US; font-weight: bold; mso-style-textfill-type: solid; mso-style-textfill-fill-themecolor: text1; mso-style-textfill-fill-color: black; mso-style-textfill-fill-alpha: 100.0%">Terms and Conditions&nbsp</span></td>
            <td class="auto-style6">
                &nbsp;<br />
                <span style="font-size: small; font-family: Calibri; mso-ascii-font-family: Calibri; mso-fareast-font-family: +mn-ea; mso-bidi-font-family: +mn-cs; mso-ascii-theme-font: minor-latin; mso-fareast-theme-font: minor-fareast; mso-bidi-theme-font: minor-bidi; color: black; mso-color-index: 1; mso-font-kerning: 12.0pt; language: en-US; mso-style-textfill-type: solid; mso-style-textfill-fill-themecolor: text1; mso-style-textfill-fill-color: black; mso-style-textfill-fill-alpha: 100.0%">&quot;I declare that this work is being submitted on behalf of my group, in accordance with the University&#39;s Regulation 11 and the WBS guidelines on plagiarism and collusion. All external references and sources are clearly acknowledged and identified within the contents. No substantial part(s) of the work submitted here has also been submitted in other assessments for accredited courses of study and if this has been done it may result in us being reported for self-plagiarism and an appropriate reduction in marks may be made when marking this piece of work.&quot; </span>
                
            </td>

        </tr>

        <tr>
            <td class="auto-style4">  <p style="language: en-US; unicode-bidi: embed; mso-line-break-override: none; word-break: normal; punctuation-wrap: hanging" class="auto-style7">
            &nbsp;I acknowledge that I have read and agree to the Terms and Conditions</td>
            <td class="auto-style6">
              <asp:CheckBox ID="chkConfirm" runat="server" /> 
                <asp:Label ID="lblConfirm" runat="server" ForeColor="Red" CssClass="auto-style5"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style8"></td>
            <td class="auto-style6">  <asp:Button ID="btnSubmit" class="button" runat="server" Text="Submit" />
                <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="auto-style5"></asp:Label>
                </td>
        </tr>
    </table>
</asp:Content>

