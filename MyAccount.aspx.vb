Imports System.Data.OleDb
Partial Class MyAccount

    Inherits System.Web.UI.Page
    'declare global variables that will be shared between functions
    Dim intUserID As Integer
    Dim strSQL As String
    Dim strEmail As String
    Dim strPhone As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Check if user is logged in
        If Not ((Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing)) Then
            Response.Redirect("Login.aspx?ReturnUrl=" + HttpContext.Current.Request.Url.PathAndQuery.Substring(1))
        End If
        ''get user ID
        If CStr(Session("MemberID")) <> "" Then
            intUserID = Session("MemberID")
        Else
            intUserID = CInt(Server.HtmlEncode(Request.Cookies("MemberID").Value))
        End If

        If Not Page.IsPostBack Then
            handleQueryString() 'set up page if user tries to access a given "page" inside myaccount by url, or by using back button
        End If
    End Sub

    Protected Sub dlSelling_ItemCommand(ByVal source As Object, _
ByVal e As DataListCommandEventArgs) Handles dlSelling.ItemCommand
        Dim index As Integer = e.Item.ItemIndex
        If e.CommandName = "CloseBid" Then
            If ItemClosed(e.CommandArgument) Then 'check if item is closed already
                CType(dlSelling.Items(index).FindControl("btnCloseBid"), Button).Text = "Already closed"
            ElseIf ReservePriceMet(e.CommandArgument) Then 'check if reserve price has been met
                closeBid(e.CommandArgument)
                CType(dlSelling.Items(index).FindControl("btnCloseBid"), Button).Text = "Closed"
                CType(dlSelling.Items(index).FindControl("DateClosed"), Label).Text = Date.Now()
            Else
                CType(dlSelling.Items(index).FindControl("lblCloseBidCheck"), Label).Text = "&nbspReserve price not met"
            End If
        ElseIf e.CommandName = "LeaveFeedback" Then
            If ItemClosed(e.CommandArgument) Then
                dlSelling.Items(index).FindControl("divFeedback").Visible = True
            End If
        ElseIf e.CommandName = "SendFeedback" Then
            If ItemClosed(e.CommandArgument) Then
                Dim intRating As Integer = CType(dlSelling.Items(index).FindControl("rbtLstRating"), RadioButtonList).SelectedValue
                Dim strFeedback As String = CType(dlSelling.Items(index).FindControl("tbFeedback"), TextBox).Text
                SendFeedback(intRating, strFeedback, e.CommandArgument)
                dlSelling.Items(index).FindControl("divFeedback").Visible = False
                CType(dlSelling.Items(index).FindControl("DateBuyerFeedbackLabel"), Label).Text = Date.Now()
            End If
        ElseIf e.CommandName = "CancelFeedback" Then
            dlSelling.Items(index).FindControl("divFeedback").Visible = False
        ElseIf e.CommandName = "ChangeDetails" Then
            If Not ItemClosed(e.CommandArgument) Then 'don't even redirect if item is still open
                Response.Redirect("Sell.aspx?ItemID=" & e.CommandArgument)
            End If
        ElseIf e.CommandName = "ShowContact" Then
            If ItemClosed(e.CommandArgument) Then 'make sure item is closed in case user disables client side validation
                dlSelling.Items(index).FindControl("divContact").Visible = True
            End If
        ElseIf e.CommandName = "HideContact" Then
            dlSelling.Items(index).FindControl("divContact").Visible = False
        ElseIf e.CommandName = "DeleteItem" Then
            If Not ItemClosed(e.CommandArgument) Then 'make sure delete command won't run when item is closed
                DeleteItem(e.CommandArgument)
            End If
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        End If
    End Sub

    Protected Sub cbNoFeedback_CheckedChanged(sender As Object, e As EventArgs) Handles cbNoFeedback.CheckedChanged
        setSelectCommandPurchase()
    End Sub

    Protected Sub btnMyNotifications_Click(sender As Object, e As EventArgs) Handles btnMyNotifications.Click
        hideEveryting()
        dlNotification.Visible = True
        btnMyNotifications.CssClass = "buttonWhite"
        SqlDataSource7.SelectCommand = "SELECT * FROM qryNotification WHERE UserID = " & intUserID
        updateNotificationStatus(intUserID)
    End Sub

    Protected Sub cbDisplayActiveItems_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayActiveItems.CheckedChanged
        setSelectCommand() 'call setSelectCommand after every change in radio buttons or checkboxes
    End Sub

    Protected Sub cbDisplayInactiveItems_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayInactiveItems.CheckedChanged
        setSelectCommand()
    End Sub

    Protected Sub rbBuying_CheckedChanged(sender As Object, e As EventArgs) Handles rbBuying.CheckedChanged
        'on the event of radio button change, check which page the user currently looking at, by checking the buttons current css class and run appropiate setSelect function
        If btnMyFeedback.CssClass = "buttonWhite" Then
            setSelectCommandFeedback()
        Else
            setSelectCommand()
        End If
    End Sub

    Protected Sub rbSelling_CheckedChanged(sender As Object, e As EventArgs) Handles rbSelling.CheckedChanged
        If btnMyFeedback.CssClass = "buttonWhite" Then
            setSelectCommandFeedback()
        Else
            setSelectCommand()
        End If
    End Sub

    Protected Sub rbWatching_CheckedChanged(sender As Object, e As EventArgs) Handles rbWatching.CheckedChanged
        setSelectCommand()
    End Sub

    Protected Sub Image1_Click(sender As Object, e As ImageClickEventArgs) Handles Image1.Click
        hideLabels()
        changeBackground(Image1.ImageUrl)
        Label1.Text = "Changed"
    End Sub

    Protected Sub Image2_Click(sender As Object, e As ImageClickEventArgs) Handles Image2.Click
        hideLabels()
        changeBackground(Image2.ImageUrl)
        Label2.Text = "Changed"
    End Sub

    Protected Sub Image3_Click(sender As Object, e As ImageClickEventArgs) Handles Image3.Click
        hideLabels()
        changeBackground(Image3.ImageUrl)
        Label3.Text = "Changed"
    End Sub

    Protected Sub Image4_Click(sender As Object, e As ImageClickEventArgs) Handles Image4.Click
        hideLabels()
        changeBackground(Image4.ImageUrl)
        Label4.Text = "Changed"
    End Sub

    Protected Sub Image5_Click(sender As Object, e As ImageClickEventArgs) Handles Image5.Click
        hideLabels()
        changeBackground(Image5.ImageUrl)
        Label5.Text = "Changed"
    End Sub

    Protected Sub Image6_Click(sender As Object, e As ImageClickEventArgs) Handles Image6.Click
        hideLabels()
        changeBackground(Image6.ImageUrl)
        Label6.Text = "Changed"
    End Sub

    Protected Sub Image7_Click(sender As Object, e As ImageClickEventArgs) Handles Image7.Click
        hideLabels()
        changeBackground(Image7.ImageUrl)
        Label7.Text = "Changed"
    End Sub

    Protected Sub Image8_Click(sender As Object, e As ImageClickEventArgs) Handles Image8.Click
        hideLabels()
        changeBackground(Image8.ImageUrl)
        Label8.Text = "Changed"
    End Sub

    Protected Sub btnCustomise_Click(sender As Object, e As EventArgs) Handles btnCustomise.Click
        hideEveryting()
        divCustomiseSite.Visible = True
        btnCustomise.CssClass = "buttonWhite"
    End Sub


    Protected Sub btnPurchaseHistory_Click(sender As Object, e As EventArgs) Handles btnPurchaseHistory.Click
        hideEveryting()
        dlHistory.Visible = True
        cbNoFeedback.Visible = True
        cbFeedbackLeft.Visible = True
        btnPurchaseHistory.CssClass = "buttonWhite"
        setSelectCommandPurchase()
    End Sub

    Protected Sub cbFeedbackLeft_CheckedChanged(sender As Object, e As EventArgs) Handles cbFeedbackLeft.CheckedChanged
        setSelectCommandPurchase()
    End Sub

    Protected Sub btnChangeDetails_Click(sender As Object, e As EventArgs) Handles btnChangeDetails.Click
        hideEveryting()
        divChangeDetails.Visible = True
        btnChangeDetails.CssClass = "buttonWhite"
        getDataChangeDetails()
    End Sub

    'this sub handles the button click evnet when user changes his/her details
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        If PasswordCorrect(intUserID, tbCurrentPassword.Text) Then 'if password is correct update details
            Dim strSQL As String
            If tbPassword.Text = "" Then
                strSQL = "UPDATE [tblUser] SET [Phone]='" & tbPhone.Text.Replace("'", "''") & "', [Email]='" & tbEmail.Text.Replace("'", "''") & "', [DateLastChanged]='" & Date.Now() & "' WHERE ID = " & intUserID
            Else
                strSQL = "UPDATE [tblUser] SET [Password]='" & tbPassword.Text.Replace("'", "''") & "', [Phone]='" & tbPhone.Text.Replace("'", "''") & "', [Email]='" & tbEmail.Text.Replace("'", "''") & "', [DateLastChanged]='" & Date.Now() & "' WHERE ID = " & intUserID
            End If
            Dim SignUpDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
            Dim SignUpCommand As New OleDbCommand(strSQL, SignUpDataConn)
            SignUpDataConn.Open()
            SignUpCommand.ExecuteNonQuery()
            SignUpCommand.CommandText = "Select @@Identity"
            SignUpDataConn.Close()
            lblError.Text = "All info updated"
        Else
            lblError.Text = "Wrong password"
        End If

    End Sub

    Function PasswordCorrect(ByVal intUserID As Integer, strPassword As String) As Boolean
        Dim returnVal As Boolean
        Dim strSQL As String
        strSQL = "SELECT ID From tblUser WHERE tblUser.ID=" & intUserID & " AND tblUser.Password = '" & strPassword.Replace("'", "''") & "'"
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.Read() Then          'check if user's paassword is correct
            returnVal = True
        Else
            returnVal = False
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()
        Return returnVal
    End Function

    Protected Sub btnMyFeedback_Click(sender As Object, e As EventArgs) Handles btnMyFeedback.Click
        hideEveryting()

        cbDisplayNegative.Visible = True
        cbDisplayNeutral.Visible = True
        cbDisplayPositive.Visible = True

        rbBuying.Visible = True
        rbSelling.Visible = True

        If rbWatching.Checked Then
            rbWatching.Checked = False
            rbBuying.Checked = True
        End If

        btnMyFeedback.CssClass = "buttonWhite"

        setSelectCommandFeedback()
    End Sub

    Protected Sub btnMyItems_Click(sender As Object, e As EventArgs) Handles btnMyItems.Click
        hideEveryting()

        cbDisplayActiveItems.Visible = True
        cbDisplayInactiveItems.Visible = True

        rbBuying.Visible = True
        rbSelling.Visible = True
        rbWatching.Visible = True

        btnMyItems.CssClass = "buttonWhite"

        setSelectCommand()
    End Sub

    Protected Sub cbDisplayPositive_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayPositive.CheckedChanged
        setSelectCommandFeedback()
    End Sub

    Protected Sub cbDisplayNeutral_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayNeutral.CheckedChanged
        setSelectCommandFeedback()
    End Sub

    Protected Sub cbDisplayNegative_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayNegative.CheckedChanged
        setSelectCommandFeedback()
    End Sub

    Protected Sub btnCloseAllBidding_Click(sender As Object, e As EventArgs) Handles btnCloseAllBidding.Click
        ''the following database read populates an Items array with the IDs of items that met reservation price for a given seller
        Dim Items(), i As Integer
        Dim strSQL As String
        strSQL = "SELECT qryCategoryActive.ItemID FROM qryCategoryActive WHERE (((qryCategoryActive.MaxOfBid)>=[qryCategoryActive].[ReservePrice])) AND SellerID = " & intUserID
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.HasRows Then
            i = 0
            While (MemberDBReader.Read())
                ReDim Preserve Items(i)
                Items(i) = MemberDBReader.GetInt32(0)
                i += 1
            End While
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()
        For i = 0 To Items.Length - 1
            closeBid(Items(i))
        Next
        Response.Redirect("MyAccount.aspx?Tab=Items&Page=Selling&Active=False&Inactive=True")

    End Sub

    Protected Sub dlHistory_ItemCommand(ByVal source As Object, _
ByVal e As DataListCommandEventArgs) Handles dlHistory.ItemCommand
        Dim index As Integer = e.Item.ItemIndex
        If e.CommandName = "LeaveFeedback" Then
            dlHistory.Items(index).FindControl("divFeedback").Visible = True
        ElseIf e.CommandName = "SendFeedback" Then
            Dim intRating As Integer
            Dim strFeedback As String
            intRating = CType(dlHistory.Items(index).FindControl("rbtLstRating"), RadioButtonList).SelectedValue
            strFeedback = CType(dlHistory.Items(index).FindControl("tbFeedback"), TextBox).Text
            SendFeedbackSeller(intRating, strFeedback, e.CommandArgument)
            dlHistory.Items(index).FindControl("divFeedback").Visible = False
            CType(dlHistory.Items(index).FindControl("DateSellerFeedbackLabel"), Label).Text = Date.Now()
            'Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        ElseIf e.CommandName = "CancelFeedback" Then
            dlHistory.Items(index).FindControl("divFeedback").Visible = False
        ElseIf e.CommandName = "ShowContact" Then
            dlHistory.Items(index).FindControl("divContact").Visible = True
        ElseIf e.CommandName = "HideContact" Then
            dlHistory.Items(index).FindControl("divContact").Visible = False
        End If

    End Sub

    'sub to fill in necessary data on ChangeDetails "tab"
    Sub getDataChangeDetails()
        Dim strSQL As String
        strSQL = "SELECT [Email], [Phone] FROM [tblUser] WHERE ID = " & intUserID
        Dim ChangeDetailsDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim ChangeDetailsCommand As New OleDbCommand(strSQL, ChangeDetailsDataConn)
        ChangeDetailsDataConn.Open()
        Dim ChangeDetailsDBReader As OleDbDataReader = ChangeDetailsCommand.ExecuteReader()
        If ChangeDetailsDBReader.Read() Then
            ''make email exists
            If Not IsDBNull(ChangeDetailsDBReader(0)) Then
                strEmail = ChangeDetailsDBReader(0)
                tbEmail.Text = strEmail
            End If
            ''make sure phone number exists
            If Not IsDBNull(ChangeDetailsDBReader(1)) Then
                strPhone = ChangeDetailsDBReader(1)
                tbPhone.Text = strPhone
            End If
        End If
        ChangeDetailsDBReader.Close()
        ChangeDetailsDataConn.Close()
    End Sub

    'this sub handles all the possible query strings that can be generated from the javascript inside the myaccount page
    'it sets up the page accordingly
    Sub handleQueryString()
        'the main if statements look for a specified "Tab" query string, it specifies which sidebar button needs to be pressed
        If Request.QueryString("Tab") = "Customise" Then
            hideEveryting() 'this function hides every dynamic element on the page, so only elements that need to be on the screen has to be specified
            divCustomiseSite.Visible = True
            btnCustomise.CssClass = "buttonWhite" 'change button to "selected" css class
        ElseIf Request.QueryString("Tab") = "Details" Then
            hideEveryting()
            divChangeDetails.Visible = True
            btnChangeDetails.CssClass = "buttonWhite"
            getDataChangeDetails()
        ElseIf Request.QueryString("Tab") = "Items" Then
            hideEveryting()

            cbDisplayActiveItems.Visible = True
            cbDisplayInactiveItems.Visible = True

            rbBuying.Visible = True
            rbSelling.Visible = True
            rbWatching.Visible = True

            btnMyItems.CssClass = "buttonWhite"
            'here we check which radio buttons, and checkboxes needs to be checked on the Items "Tab"
            If Request.QueryString("Active") = "True" Then
                cbDisplayActiveItems.Checked = True
            ElseIf Request.QueryString("Active") = "False" Then
                cbDisplayActiveItems.Checked = False
            End If

            If Request.QueryString("Inactive") = "True" Then
                cbDisplayInactiveItems.Checked = True
            ElseIf Request.QueryString("Inactive") = "False" Then
                cbDisplayInactiveItems.Checked = False
            End If

            If Request.QueryString("Page") = "Buying" Then
                rbBuying.Checked = True
                rbSelling.Checked = False
                rbWatching.Checked = False
            ElseIf Request.QueryString("Page") = "Selling" Then
                rbSelling.Checked = True
                rbBuying.Checked = False
                rbWatching.Checked = False
            ElseIf Request.QueryString("Page") = "Watching" Then
                rbSelling.Checked = False
                rbBuying.Checked = False
                rbWatching.Checked = True
            End If

            setSelectCommand() 'run setSelectCommand() that sets SelectCommands for the datalists based on the radio button and checkbox settings

        ElseIf Request.QueryString("Tab") = "Feedback" Then
            hideEveryting()

            cbDisplayNegative.Visible = True
            cbDisplayNeutral.Visible = True
            cbDisplayPositive.Visible = True

            rbBuying.Visible = True
            rbSelling.Visible = True

            'make sure there is a selected radio button if the (now hidden) rbWatching was the selected one, and there is no radio button specified in following query strings
            If rbWatching.Checked Then
                rbWatching.Checked = False
                rbBuying.Checked = True 'the default radio button will be rbBuying in this case
            End If

            btnMyFeedback.CssClass = "buttonWhite"

            If Request.QueryString("Positive") = "True" Then
                cbDisplayPositive.Checked = True
            ElseIf Request.QueryString("Positive") = "False" Then
                cbDisplayPositive.Checked = False
            End If

            If Request.QueryString("Neutral") = "True" Then
                cbDisplayNeutral.Checked = True
            ElseIf Request.QueryString("Neutral") = "False" Then
                cbDisplayNeutral.Checked = False
            End If

            If Request.QueryString("Negative") = "True" Then
                cbDisplayNegative.Checked = True
            ElseIf Request.QueryString("Negative") = "False" Then
                cbDisplayNegative.Checked = False
            End If

            If Request.QueryString("Page") = "Buying" Then
                rbBuying.Checked = True
                rbSelling.Checked = False
            ElseIf Request.QueryString("Page") = "Selling" Then
                rbSelling.Checked = True
                rbBuying.Checked = False
            End If

            setSelectCommandFeedback()

        ElseIf Request.QueryString("Tab") = "Purchase" Then
            hideEveryting()
            dlHistory.Visible = True
            cbFeedbackLeft.Visible = True
            cbNoFeedback.Visible = True
            btnPurchaseHistory.CssClass = "buttonWhite"

            If Request.QueryString("FeedbackLeft") = "True" Then
                cbFeedbackLeft.Checked = True
            ElseIf Request.QueryString("FeedbackLeft") = "False" Then
                cbFeedbackLeft.Checked = False
            End If

            If Request.QueryString("NoFeedback") = "True" Then
                cbNoFeedback.Checked = True
            ElseIf Request.QueryString("NoFeedback") = "False" Then
                cbNoFeedback.Checked = False
            End If

            setSelectCommandPurchase()

        ElseIf Request.QueryString("Tab") = "Notification" Then
            hideEveryting()
            dlNotification.Visible = True
            btnMyNotifications.CssClass = "buttonWhite"
            SqlDataSource7.SelectCommand = "SELECT * FROM qryNotification WHERE UserID = " & intUserID
            updateNotificationStatus(intUserID) 'set user's notification to "seen"
        Else  'set SelectCommand for default screen, in case there wasn't any (meaningful) query string
            SqlDataSource1.SelectCommand = "SELECT [FirstName], [LastName], [Email], [Phone], [DateBuyerFeedback], [DateUploaded], [DateClosed], [ItemID], [Title], [Picture], [ReservePrice], [MaxOfBid], [BidderUserID], [BidderUserName], [TimeOfBid] FROM [qryMyAccountSellingActive] WHERE SellerID = " & intUserID
        End If
    End Sub

    Sub hideEveryting()
        SqlDataSource1.SelectCommand = ""
        SqlDataSource2.SelectCommand = ""
        SqlDataSource3.SelectCommand = ""
        SqlDataSource4.SelectCommand = ""
        SqlDataSource5.SelectCommand = ""
        SqlDataSource6.SelectCommand = ""
        SqlDataSource7.SelectCommand = ""

        dlSelling.Visible = False
        dlBuying.Visible = False
        dlWatching.Visible = False
        dlHistory.Visible = False
        dlNotification.Visible = False

        rbBuying.Visible = False
        rbSelling.Visible = False
        rbWatching.Visible = False

        cbDisplayActiveItems.Visible = False
        cbDisplayInactiveItems.Visible = False

        cbDisplayNegative.Visible = False
        cbDisplayNeutral.Visible = False
        cbDisplayPositive.Visible = False

        cbFeedbackLeft.Visible = False
        cbNoFeedback.Visible = False

        divChangeDetails.Visible = False
        divCustomiseSite.Visible = False

        'btnMyFeedback.Visible = False
        'btnMyItems.Visible = False

        btnChangeDetails.CssClass = "button"
        btnCustomise.CssClass = "button"
        btnCloseAllBidding.CssClass = "button"
        btnMyItems.CssClass = "button"
        btnMyFeedback.CssClass = "button"
        btnPurchaseHistory.CssClass = "button"
        btnMyNotifications.CssClass = "button"
    End Sub

    'this function sets the appropiate select command for the My Items tab based on the selected radio buttons and checkboxes
    Sub setSelectCommand()

        If rbSelling.Checked Then
            dlBuying.Visible = False
            dlSelling.Visible = True
            dlWatching.Visible = False
            SqlDataSource2.SelectCommand = ""
            SqlDataSource3.SelectCommand = ""
            If cbDisplayActiveItems.Checked And cbDisplayInactiveItems.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [FirstName], [LastName], [Email], [Phone], [DateBuyerFeedback], [DateUploaded], [DateClosed], [ItemID], [Title], [Picture], [ReservePrice], [MaxOfBid], [BidderUserID], [BidderUserName], [TimeOfBid] FROM [qryMyAccountSelling] WHERE SellerID = " & intUserID
            ElseIf cbDisplayActiveItems.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [FirstName], [LastName], [Email], [Phone], [DateBuyerFeedback], [DateUploaded], [DateClosed], [ItemID], [Title], [Picture], [ReservePrice], [MaxOfBid], [BidderUserID], [BidderUserName], [TimeOfBid] FROM [qryMyAccountSellingActive] WHERE SellerID = " & intUserID
            ElseIf cbDisplayInactiveItems.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [FirstName], [LastName], [Email], [Phone], [DateBuyerFeedback], [DateUploaded], [DateClosed], [ItemID], [Title], [Picture], [ReservePrice], [MaxOfBid], [BidderUserID], [BidderUserName], [TimeOfBid] FROM [qryMyAccountSellingInactive] WHERE SellerID = " & intUserID
            Else
                SqlDataSource1.SelectCommand = ""
            End If
        ElseIf rbBuying.Checked Then
            dlBuying.Visible = True
            dlSelling.Visible = False
            dlWatching.Visible = False
            SqlDataSource1.SelectCommand = ""
            SqlDataSource3.SelectCommand = ""
            If cbDisplayActiveItems.Checked And cbDisplayInactiveItems.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountBuying] WHERE UserID = " & intUserID
            ElseIf cbDisplayActiveItems.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountBuyingActive] WHERE UserID = " & intUserID
            ElseIf cbDisplayInactiveItems.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountBuyingInactive] WHERE UserID = " & intUserID
            Else
                SqlDataSource2.SelectCommand = ""
            End If
        ElseIf rbWatching.Checked Then
            dlBuying.Visible = False
            dlSelling.Visible = False
            dlWatching.Visible = True
            SqlDataSource1.SelectCommand = ""
            SqlDataSource2.SelectCommand = ""
            If cbDisplayActiveItems.Checked And cbDisplayInactiveItems.Checked Then
                SqlDataSource3.SelectCommand = "SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountWatching] WHERE UserID = " & intUserID
            ElseIf cbDisplayActiveItems.Checked Then
                SqlDataSource3.SelectCommand = "SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountWatchingActive] WHERE UserID = " & intUserID
            ElseIf cbDisplayInactiveItems.Checked Then
                SqlDataSource3.SelectCommand = "SELECT [Picture], [Title], [MaxOfBid], [UserName], [Condition], [ItemID], [TimeOfBid], [UserID] FROM [qryMyAccountWatchingInactive] WHERE UserID = " & intUserID
            Else
                SqlDataSource3.SelectCommand = ""
            End If
        End If
        '' this was an attempt to hide close buttons on server side where item is already closed
        'hideCloseBtn()
    End Sub

    ''function to hide close buttons on server side, but it only worked after a page refresh
    'Function hideCloseBtn() As Integer
    '    For Each itm In dlSelling.Items
    '        If CType(itm.FindControl("DateClosed"), Label).Text <> "" Then
    '            CType(itm.FindControl("btnCloseBid"), Button).Visible = False
    '        End If
    '    Next
    '    Return 0
    'End Function

    Function ItemClosed(ByVal id As Integer) As Boolean
        Dim returnVal As Boolean
        Dim strSQL As String
        strSQL = "SELECT DateClosed From tblItems WHERE ID = " & id & " AND DateClosed Is Not Null"
        Dim BidCheckDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim BidCheckCommand As New OleDbCommand(strSQL, BidCheckDataConn)
        BidCheckDataConn.Open()
        Dim BidCheckDBReader As OleDbDataReader = BidCheckCommand.ExecuteReader()
        If BidCheckDBReader.Read() Then
            returnVal = True
        Else
            returnVal = False
        End If
        BidCheckDBReader.Close()
        BidCheckDataConn.Close()
        Return returnVal
    End Function

    Function ReservePriceMet(ByVal id As Integer) As Boolean
        Dim returnVal As Boolean
        Dim strSQL As String
        strSQL = "SELECT ItemID From qryMyAccountSelling WHERE MaxOfBid >= ReservePrice AND ItemID = " & id
        Dim BidCheckDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim BidCheckCommand As New OleDbCommand(strSQL, BidCheckDataConn)
        BidCheckDataConn.Open()
        Dim BidCheckDBReader As OleDbDataReader = BidCheckCommand.ExecuteReader()
        If BidCheckDBReader.Read() Then
            returnVal = True
        Else
            returnVal = False
        End If
        BidCheckDBReader.Close()
        BidCheckDataConn.Close()
        Return returnVal
    End Function

    Sub DeleteItem(ByVal id As Integer)
        'first copy item to tblDeleted for book keeping
        Dim strSQL As String = "INSERT INTO tblDeleted SELECT * FROM tblItems WHERE ID = " & id
        Dim DeleteDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim CopyComm As New OleDbCommand(strSQL, DeleteDataConn)
        DeleteDataConn.Open()
        CopyComm.ExecuteNonQuery()
        CopyComm.CommandText = "Select @@Identity"
        'delete item from tblItems
        strSQL = "DELETE FROM tblItems WHERE ID = " & id
        Dim DeleteComm As New OleDbCommand(strSQL, DeleteDataConn)
        DeleteComm.ExecuteNonQuery()
        DeleteComm.CommandText = "Select @@Identity"
        DeleteDataConn.Close()
    End Sub


    Sub SendFeedback(ByVal intRating As Integer, ByVal strFeedback As String, ByVal id As Integer)
        Dim strSQL As String = "UPDATE [tblItems] SET [BuyerRating] = " & intRating & ", [BuyerFeedback] = '" & strFeedback & "', [DateBuyerFeedback] = '" & Date.Now & "' WHERE ID = " & id
        Dim RatingDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim RatingComm As New OleDbCommand(strSQL, RatingDataConn)
        RatingDataConn.Open()
        RatingComm.ExecuteNonQuery()
        RatingComm.CommandText = "Select @@Identity"
        RatingDataConn.Close()
    End Sub

    Sub SendFeedbackSeller(ByVal intRating As Integer, ByVal strFeedback As String, ByVal id As Integer)
        Dim strSQL As String = "UPDATE [tblItems] SET [SellerRating] = " & intRating & ", [SellerFeedback] = '" & strFeedback & "', [DateSellerFeedback] = '" & Date.Now & "' WHERE ID = " & id
        Dim RatingDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim RatingComm As New OleDbCommand(strSQL, RatingDataConn)
        RatingDataConn.Open()
        RatingComm.ExecuteNonQuery()
        RatingComm.CommandText = "Select @@Identity"
        RatingDataConn.Close()
    End Sub

    ''function to close items that met reservation price
    Sub closeBid(ByVal id As Integer)
        Dim strSQL As String
        strSQL = "UPDATE [tblItems] SET [DateClosed] = '" & Date.Now & "' WHERE ID = " & id
        Dim CloseBidConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim CloseBidCommand As New OleDbCommand(strSQL, CloseBidConn)
        CloseBidConn.Open()
        CloseBidCommand.ExecuteNonQuery()
        CloseBidCommand.CommandText = "Select @@Identity"
        CloseBidConn.Close()
        notification(id, getWinner(id), 4) 'notify user winner
        Dim Watchers() As Integer = GetWatchers(id) 'notify wathcers
        If Watchers IsNot Nothing Then
            For i = 0 To Watchers.Length - 1
                notification(id, Watchers(i), 7)
            Next
        End If
    End Sub

    Function getWinner(ByVal intItemID As Integer) As Integer
        Dim returnVal As Integer
        Dim strSQL As String
        strSQL = "SELECT BidderUserID FROM qryMyAccountSelling WHERE ItemID = " & intItemID
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.Read() Then
            returnVal = MemberDBReader(0)
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()
        Return returnVal
    End Function

    Sub setSelectCommandFeedback()
        If rbBuying.Checked Then
            dlFeedbackBuying.Visible = True
            dlFeedbackSelling.Visible = False
            SqlDataSource4.SelectCommand = ""
            If cbDisplayNegative.Checked And cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource5.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayNeutral.Checked Then
                SqlDataSource5.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND (BuyerRating = 1 OR BuyerRating = 2)" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayPositive.Checked Then
                SqlDataSource5.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND (BuyerRating = 1 OR BuyerRating = 3)" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource5.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND (BuyerRating = 2 OR BuyerRating = 3)" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNegative.Checked Then
                SqlDataSource5.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND BuyerRating = 1" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNeutral.Checked Then
                SqlDataSource5.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND BuyerRating = 2" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayPositive.Checked Then
                SqlDataSource5.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND BuyerRating = 3" & " ORDER BY BuyerRating DESC"
            Else
                SqlDataSource5.SelectCommand = ""
            End If

        ElseIf rbSelling.Checked Then
            dlFeedbackBuying.Visible = False
            dlFeedbackSelling.Visible = True
            SqlDataSource5.SelectCommand = ""
            If cbDisplayNegative.Checked And cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource4.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayNeutral.Checked Then
                SqlDataSource4.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND (SellerRating = 1 OR SellerRating = 2)" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayPositive.Checked Then
                SqlDataSource4.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND (SellerRating = 1 OR SellerRating = 3)" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource4.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND (SellerRating = 2 OR SellerRating = 3)" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNegative.Checked Then
                SqlDataSource4.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND SellerRating = 1" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNeutral.Checked Then
                SqlDataSource4.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND SellerRating = 2" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayPositive.Checked Then
                SqlDataSource4.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND SellerRating = 3" & " ORDER BY SellerRating DESC"
            Else
                SqlDataSource4.SelectCommand = ""
            End If

        End If
    End Sub

    Sub changeBackground(ByVal x As String)
        strSQL = "UPDATE [tblUser] SET [BackGround] = '" & x & "' WHERE ID = " & intUserID
        Dim CloseBidConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim CloseBidCommand As New OleDbCommand(strSQL, CloseBidConn)
        CloseBidConn.Open()
        CloseBidCommand.ExecuteNonQuery()
        CloseBidCommand.CommandText = "Select @@Identity"
        CloseBidConn.Close()
    End Sub

    Sub hideLabels()
        Label1.Text = ""
        Label2.Text = ""
        Label3.Text = ""
        Label4.Text = ""
        Label5.Text = ""
        Label6.Text = ""
        Label7.Text = ""
        Label8.Text = ""
    End Sub

    Sub setSelectCommandPurchase()

        If (cbFeedbackLeft.Checked And cbNoFeedback.Checked) Then
            SqlDataSource6.SelectCommand = "SELECT [FirstName], [LastName], [Email], [Phone], [DateSellerFeedback], [Picture], [Title], [MaxOfBid], [UserName], [SellerID], [BidderUserID], [DateClosed], [ItemID], [Condition], [ID], [TimeOfBid] FROM [qryMyAccountPurchase] WHERE BidderUserID = " & intUserID
        ElseIf cbFeedbackLeft.Checked Then
            SqlDataSource6.SelectCommand = "SELECT [FirstName], [LastName], [Email], [Phone], [DateSellerFeedback], [Picture], [Title], [MaxOfBid], [UserName], [SellerID], [BidderUserID], [DateClosed], [ItemID], [Condition], [ID], [TimeOfBid] FROM [qryMyAccountPurchaseFeedback] WHERE BidderUserID = " & intUserID
        ElseIf cbNoFeedback.Checked Then
            SqlDataSource6.SelectCommand = "SELECT [FirstName], [LastName], [Email], [Phone], [DateSellerFeedback], [Picture], [Title], [MaxOfBid], [UserName], [SellerID], [BidderUserID], [DateClosed], [ItemID], [Condition], [ID], [TimeOfBid] FROM [qryMyAccountPurchaseNoFeedback] WHERE BidderUserID = " & intUserID
        Else
            SqlDataSource6.SelectCommand = ""
        End If
    End Sub

    'set notification status to seen
    Sub updateNotificationStatus(ByVal intUserID As Integer)
        strSQL = "UPDATE [tblNotification] SET [Seen] = -1, [SeenDate] = '" & Date.Now() & "' WHERE UserID = " & intUserID
        Dim CloseBidConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim CloseBidCommand As New OleDbCommand(strSQL, CloseBidConn)
        CloseBidConn.Open()
        CloseBidCommand.ExecuteNonQuery()
        CloseBidCommand.CommandText = "Select @@Identity"
        CloseBidConn.Close()
    End Sub

    ''function to send notification to users
    Sub notification(ByVal intItemID As Integer, ByVal intUserID As Integer, ByVal intMessageID As Integer)
        strSQL = "INSERT INTO [tblNotification] ([ItemID], [UserID], [MessageID], [AddedDate]) VALUES (" & intItemID & ", " & intUserID & ", " & intMessageID & ", '" & Date.Now & "')"
        Dim NotifDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim NotifCommand As New OleDbCommand(strSQL, NotifDataConn)
        NotifDataConn.Open()
        NotifCommand.ExecuteNonQuery()
        NotifDataConn.Close()
    End Sub

    Function GetWatchers(ByVal intItemID As Integer) As Integer()
        Dim intWatchers() As Integer
        Dim strSQL As String
        Dim i As Integer
        strSQL = "SELECT UserID FROM tblWatching WHERE ItemID = " & intItemID
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.HasRows Then
            i = 0
            While (MemberDBReader.Read())
                ReDim Preserve intWatchers(i)
                intWatchers(i) = MemberDBReader.GetInt32(0)
                i += 1
            End While
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()
        Return intWatchers
    End Function

End Class
