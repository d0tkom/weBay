Imports System.Data.OleDb
Partial Class Sell
    Inherits System.Web.UI.Page
    Dim strFileName As String
    Dim strImagePath As String
    Dim intUserID As Integer
    Dim intItemID As Integer
    Dim strSQL As String


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Check if user is logged in
        If Not ((Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing)) Then
            Response.Redirect("Login.aspx?ReturnUrl=" + HttpContext.Current.Request.Url.PathAndQuery.Substring(1))
        End If
        ''get ID of current user
        If CStr(Session("MemberID")) <> "" Then
            intUserID = Session("MemberID")
        Else
            intUserID = CInt(Server.HtmlEncode(Request.Cookies("MemberID").Value))
        End If

        If Not (Request.QueryString("ItemID") Is Nothing) Then ''case when user updates item values
            intItemID = CInt(Request.QueryString("ItemID"))
            If (Not checkItemActive(intItemID)) Then 'make sure item is active
                Response.Redirect("Sell.aspx")
            End If

            If Not Page.IsPostBack Then
                'set up screen for "ChangeDetails" page
                ddlCategories.AppendDataBoundItems = False
                ddlSubCategories.AppendDataBoundItems = False
                lblStartingPrice.Visible = True
                tbStartingPrice.Visible = False
                rfvStartingPrice.Enabled = False
                btnChange.Visible = True
                btnCancel.Visible = True
                btnSubmit.Visible = False
                rfvImage.Enabled = False

                intItemID = CInt(Request.QueryString("ItemID"))
                'get current values form db
                strSQL = "SELECT [Title], [Description], [StartingPrice], [ReservePrice], [BidIncrement], [CategoryID], [SubCategoryID], [ConditionID], [Picture] FROM [qrySellChangeDetails] WHERE ID = " & intItemID & " AND SellerID = " & intUserID
                Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
                Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
                MemberDataConn.Open()
                Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
                ''fill textboxes with current data
                If MemberDBReader.Read() Then
                    tbTitle.Text = MemberDBReader(0)
                    tbDescription.Text = MemberDBReader(1)
                    lblStartingPrice.Text = MemberDBReader(2)
                    tbReservePrice.Text = CInt(MemberDBReader(3))
                    tbBidIncrement.Text = MemberDBReader(4)
                    lblCategoryHidden.Text = MemberDBReader(5) 'fill hidden label for javascript to select correct dropdownlist element
                    lblSubCategoryHidden.Text = MemberDBReader(6)
                    lblConditionHidden.Text = MemberDBReader(7)
                    imgImage.ImageUrl = MemberDBReader(8)
                End If
                MemberDBReader.Close()
                MemberDataConn.Close()
                ''set dropdown list for subcategory
                SqlDataSource2.SelectCommand = "SELECT tblSubCategories.[subCategory], tblSubCategories.[ID] FROM [tblSubCategories] INNER JOIN tblCategories ON tblSubCategories.CategoryID = tblCategories.ID WHERE (((tblCategories.ID)=" & lblCategoryHidden.Text & "))"
                ''fill keywords textbox
                For Each i As String In getKeywords()
                    tbKeyWords.Text += i & " "
                Next
            End If
        End If

    End Sub

    'check if item is closed or not
    Function checkItemActive(ByVal x As Integer) As Boolean
        Dim active As Boolean
        strSQL = "SELECT [Title] FROM [qryActiveItems] WHERE ID = " & x
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.Read() Then
            active = True
        Else
            active = False
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()
        Return active
    End Function

    'get keywords form db
    Function getKeywords() As String()
        Dim keywords() As String
        Dim i As Integer = 0
        intItemID = CInt(Request.QueryString("ItemID"))
        strSQL = "SELECT [Keyword] FROM [tblKeywords] WHERE ItemID = " & intItemID
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        While MemberDBReader.Read()
            ReDim Preserve keywords(i)
            keywords(i) = MemberDBReader.GetString(0)
            i += 1
        End While
        MemberDBReader.Close()
        MemberDataConn.Close()
        Return keywords
    End Function


    Protected Sub ddlCategories_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCategories.SelectedIndexChanged
        ''clearing drop down list and adding "Select a sub Category" so it will stay at the top after every load
        If ddlSubCategories.AppendDataBoundItems = True Then
            ddlSubCategories.Items.Clear()
            ddlSubCategories.Items.Add("Select a Sub Category")
        End If
            SqlDataSource2.SelectCommand = "SELECT tblSubCategories.[subCategory], tblSubCategories.[ID] FROM [tblSubCategories] INNER JOIN tblCategories ON tblSubCategories.CategoryID = tblCategories.ID WHERE (((tblCategories.ID)=" & ddlCategories.SelectedValue & "))"
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        ''if image upload was succesfull, and all client side validators passed, add values to database
        If UploadImage() Then
            If CInt(tbBidIncrement.Text) >= 1 Then
                ''split string with multiple delimiters for keywords taken from: https://msdn.microsoft.com/en-us/library/6x627e5f(v=vs.90).aspx
                imgImage.ImageUrl = strImagePath
                Dim arrKeyWords() As String = Split(tbKeyWords.Text)
                Dim LastNonEmpty As Integer = -1
                For i As Integer = 0 To arrKeyWords.Length - 1
                    If arrKeyWords(i) <> "" Then
                        LastNonEmpty += 1
                        arrKeyWords(LastNonEmpty) = arrKeyWords(i)
                    End If
                Next
                ReDim Preserve arrKeyWords(LastNonEmpty)

                'submit item to db
                Dim strSQL As String
                ''different sql command if there is reserver price, and if there isn't
                If tbReservePrice.Text = "" Then
                    strSQL = "INSERT INTO [tblItems] ([Title], [Description], [Picture], [ConditionID], [StartingPrice], [BidIncrement], [SellerID], [SubCategoryID], [DateUploaded]) VALUES ('" & tbTitle.Text.Replace("'", "''") & "', '" & tbDescription.Text.Replace("'", "''") & "', '" & strImagePath.Replace("'", "''") & "', '" & ddlCondition.SelectedValue.Replace("'", "''") & "', '" & tbStartingPrice.Text.Replace("'", "''") & "', '" & tbBidIncrement.Text.Replace("'", "''") & "', '" & intUserID & "', '" & ddlSubCategories.SelectedValue & "', '" & Date.Now & "')"
                Else
                    strSQL = "INSERT INTO [tblItems] ([Title], [Description], [Picture], [ConditionID], [StartingPrice], [BidIncrement], [ReservePrice], [SellerID], [SubCategoryID], [DateUploaded]) VALUES ('" & tbTitle.Text.Replace("'", "''") & "', '" & tbDescription.Text.Replace("'", "''") & "', '" & strImagePath.Replace("'", "''") & "', '" & ddlCondition.SelectedValue.Replace("'", "''") & "', '" & tbStartingPrice.Text.Replace("'", "''") & "', '" & tbBidIncrement.Text.Replace("'", "''") & "', '" & tbReservePrice.Text.Replace("'", "''") & "', '" & intUserID & "', '" & ddlSubCategories.SelectedValue & "', '" & Date.Now & "')"
                End If
                Dim NewItemDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
                Dim NewItemCommand As New OleDbCommand(strSQL, NewItemDataConn)
                NewItemDataConn.Open()
                NewItemCommand.ExecuteNonQuery()
                NewItemCommand.CommandText = "Select @@Identity"
                ''store the ID of recently added item in a variable
                Dim intJustAddedItemID As Integer = CInt(NewItemCommand.ExecuteScalar())

                ''add keywords to the tblKeywords table
                For i As Integer = 0 To arrKeyWords.Length - 1
                    strSQL = "INSERT INTO [tblKeywords] ([ItemID], [Keyword]) VALUES ('" & intJustAddedItemID & "', '" & arrKeyWords(i).Replace("'", "''") & "')"
                    Dim NewKeyWordsCommand As New OleDbCommand(strSQL, NewItemDataConn)
                    NewKeyWordsCommand.ExecuteNonQuery()
                    NewKeyWordsCommand.CommandText = "Select @@Identity"
                Next
                NewItemDataConn.Close()
                Response.Redirect("MyAccount.aspx?Tab=Items&Page=Selling&Active=True") 'redirect to uploaded items page
                'Response.Redirect("Item.aspx?ItemID=" & intJustAddedItemID) 'redirect to newly added item's page
            Else
                lblError.Text = "Bid increment can't be 0"
            End If
        End If
    End Sub

    Function UploadImage() As Boolean 'from http://forums.asp.net/thread/1507740.aspx
        If fuImage.PostedFile.ContentLength < 4194304 Then
            If Not (fuImage.PostedFile Is Nothing) Then 'Check to make sure we actually have a file to upload, and that file size is less than limit
                Dim strLongFilePath As String = fuImage.PostedFile.FileName
                strFileName = DateTime.Now.ToString("yyyymmddMMss") & fuImage.PostedFile.FileName ''adding and date to the filename to make sure it is unique
                strImagePath = "~\Images\" & strFileName
                Select Case fuImage.PostedFile.ContentType
                    Case "image/pjpeg", "image/jpeg" 'Make sure we are getting a valid JPG image
                        fuImage.PostedFile.SaveAs(Server.MapPath("~\Images\") & strFileName)
                        Return True
                    Case Else
                        'Not a valid jpeg image
                        lblError.Text = "Image is not in jpg format"
                        Return False
                End Select
            Else
                lblError.Text = "No picture found"
                Return False
            End If
        Else
            Return False
        End If
    End Function


    Protected Sub btnChange_Click(sender As Object, e As EventArgs) Handles btnChange.Click
        If fuImage.HasFile Then 'check if user want's to change picture
            ''if image upload was succesfull, and all client side validators passed, add values to database
            If UploadImage() Then
                ''split string with multiple delimiters for keywords taken from: https://msdn.microsoft.com/en-us/library/6x627e5f(v=vs.90).aspx
                imgImage.ImageUrl = strImagePath
                Dim arrKeyWords() As String = Split(tbKeyWords.Text)
                Dim LastNonEmpty As Integer = -1
                For i As Integer = 0 To arrKeyWords.Length - 1
                    If arrKeyWords(i) <> "" Then
                        LastNonEmpty += 1
                        arrKeyWords(LastNonEmpty) = arrKeyWords(i)
                    End If
                Next
                ReDim Preserve arrKeyWords(LastNonEmpty)

                ''different sql command if there is reserver price, and if there isn't
                If tbReservePrice.Text = "" Then
                    strSQL = "UPDATE [tblItems] SET [Title]='" & tbTitle.Text.Replace("'", "''") & "', [Description]='" & tbDescription.Text.Replace("'", "''") & "', [ConditionID]= '" & ddlCondition.SelectedValue.Replace("'", "''") & "', [BidIncrement]='" & tbBidIncrement.Text.Replace("'", "''") & "', [SubCategoryID]='" & ddlSubCategories.SelectedValue & "', [Picture]='" & strImagePath.Replace("'", "''") & "',  [DateLastChanged] = '" & Date.Now & "' WHERE ID = " & intItemID
                Else
                    strSQL = "UPDATE [tblItems] SET [Title]='" & tbTitle.Text.Replace("'", "''") & "', [Description]='" & tbDescription.Text.Replace("'", "''") & "', [ConditionID]= '" & ddlCondition.SelectedValue.Replace("'", "''") & "', [BidIncrement]='" & tbBidIncrement.Text.Replace("'", "''") & "', [SubCategoryID]='" & ddlSubCategories.SelectedValue & "', [Picture]='" & strImagePath.Replace("'", "''") & "',  [DateLastChanged] = '" & Date.Now & "', [ReservePrice] = '" & tbReservePrice.Text.Replace("'", "''") & "' WHERE ID = " & intItemID
                End If
                Dim NewItemDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
                Dim NewItemCommand As New OleDbCommand(strSQL, NewItemDataConn)
                NewItemDataConn.Open()
                NewItemCommand.ExecuteNonQuery()
                NewItemCommand.CommandText = "Select @@Identity"
                NewItemDataConn.Close()
                lblError.Text = "Changed"
            End If
        Else 'dont change picture
            ''different sql command if there is reserver price, and if there isn't
            If tbReservePrice.Text = "" Then
                strSQL = "UPDATE [tblItems] SET [Title]='" & tbTitle.Text.Replace("'", "''") & "', [Description]='" & tbDescription.Text.Replace("'", "''") & "', [ConditionID]= '" & ddlCondition.SelectedValue.Replace("'", "''") & "', [BidIncrement]='" & tbBidIncrement.Text.Replace("'", "''") & "', [SubCategoryID]='" & ddlSubCategories.SelectedValue & "',  [DateLastChanged] = '" & Date.Now & "' WHERE ID = " & intItemID
            Else
                strSQL = "UPDATE [tblItems] SET [Title]='" & tbTitle.Text.Replace("'", "''") & "', [Description]='" & tbDescription.Text.Replace("'", "''") & "', [ConditionID]= '" & ddlCondition.SelectedValue.Replace("'", "''") & "', [BidIncrement]='" & tbBidIncrement.Text.Replace("'", "''") & "', [SubCategoryID]='" & ddlSubCategories.SelectedValue & "',  [DateLastChanged] = '" & Date.Now & "', [ReservePrice] = '" & tbReservePrice.Text.Replace("'", "''") & "' WHERE ID = " & intItemID
            End If
            Dim NewItemDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
            Dim NewItemCommand As New OleDbCommand(strSQL, NewItemDataConn)
            NewItemDataConn.Open()
            NewItemCommand.ExecuteNonQuery()
            NewItemCommand.CommandText = "Select @@Identity"
            NewItemDataConn.Close()
            lblError.Text = "Changed"
        End If

        'Response.Redirect("MyAccount.aspx?Tab=Items&Page=Selling&Active=True&Inactive=False")
        'Response.Redirect("Item.aspx?ItemID=" & intItemID, True)
    End Sub



    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("MyAccount.aspx?Tab=Items&Page=Selling&Active=True")
    End Sub
End Class
