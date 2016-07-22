
Imports System.Data.OleDb
Partial Class Item
    Inherits System.Web.UI.Page
    Dim intItemID As Integer
    Dim intUserID As Integer
    Dim btnSendBid As Button
    Dim strSQL As String
    Dim CurrentPrice As String
    Dim StartingPrice As String
    Dim BidIncrement As String
    Dim AutoBidMax As String
    Dim AutoBidMaxBidder As String
    Dim Bid As String
    Dim Seller As String
    Dim CurrentWinner As Integer
    Dim ReservePrice As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Check if user is logged in
        If Not ((Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing)) Then
            ''if user is not logged in, redirect it to the login page, passing current page as a returnUrl query string
            Response.Redirect("Login.aspx?ReturnUrl=" + HttpContext.Current.Request.Url.PathAndQuery.Substring(1))
        End If

        ''get ID of current user
        If CStr(Session("MemberID")) <> "" Then
            intUserID = Session("MemberID")
        Else
            intUserID = CInt(Server.HtmlEncode(Request.Cookies("MemberID").Value))
        End If

        ''if there is no itemID in query string, clear screen
        If Request.QueryString("ItemID") Is Nothing Then
            clearScreen()
            ''if there is ItemID in query string, and history query string set to true, set up screen for showing bid history
        ElseIf Not Request.QueryString("ItemID") Is Nothing And Request.QueryString("History") = "True" Then
            clearScreen()
            intItemID = CInt(Request.QueryString("ItemID"))
            SqlDataSource3.SelectCommand = "SELECT [UserID], [UserName], [Bid], [TimeOfBid] FROM [qryBidHistory] WHERE ItemID = " & intItemID
            divBidHistory.Visible = True
            ''if there is itemID, no feedback=true, but item is closed, setup screen for "closed" screen
        ElseIf checkItemClosed(CInt(Request.QueryString("ItemID"))) Then
            clearScreen()
            intItemID = CInt(Request.QueryString("ItemID")) ''get item ID form query string
            DataList3.Visible = True
            SqlDataSource4.SelectCommand = "SELECT [DateClosed], [ItemID], [CountOfBid], [SellerID], [DateUploaded], [Title], [Picture], [UserName], [Condition], [Description], [MaxOfBid], [BidIncrement], [StartingPrice] FROM [qryItem] WHERE qryItem.ItemID = " & intItemID ''set select command for Item's datalist
            ''call function that sets up add to watchlist / remove from watchlist button
            setWatchButton(intItemID, True)
            ''if there is ItemID, no feedback=true, and item is not closed, show "normal" item screen
        Else
            clearScreen()
            intItemID = CInt(Request.QueryString("ItemID")) ''get item ID form query string
            DataList1.Visible = True
            SqlDataSource1.SelectCommand = "SELECT [ItemID], [CountOfBid], [SellerID], [DateUploaded], [Title], [Picture], [UserName], [Condition], [Description], [MaxOfBid], [BidIncrement], [StartingPrice] FROM [qryItem] WHERE qryItem.ItemID = " & intItemID ''set select command for Item's datalist
            ''call function that sets up add to watchlist / remove form watchlist button
            setWatchButton(intItemID, False)
            getPriceIncrementStartSeller(intItemID)
            Dim userMaxBid As Integer = getUserMaxBid(intItemID, intUserID)
            If userMaxBid <> 0 And userMaxBid > CurrentPrice Then
                CType(DataList1.Items(0).FindControl("lblMaxBid"), Label).Text = "Auto bidding is active. Your current maximum bid is: £" & userMaxBid
            End If
        End If
    End Sub

    ''This Sub handles the events of button presses inside DataList1, what shows the currently watched item
    Protected Sub DataList1_ItemCommand(ByVal source As Object, _
        ByVal e As DataListCommandEventArgs) Handles DataList1.ItemCommand
        If e.CommandName = "btnSendBid" Then ''the send bid button has been pressed
            ''check if input is a number (it is checked client side as well, but this takes care of users bypassing client side validation)
            If IsNumeric(CType(DataList1.Items(0).FindControl("tbMaximumBid"), TextBox).Text) Then
                ''this function gets current price, bid increment, starting price and seller for a given item and stores them in global variables
                getPriceIncrementStartSeller(intItemID)
                ''check if user tries to bid on his/her own item
                If Seller = intUserID Then
                    CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "You can't bid on your own item"
                Else
                    ''get values from maxbid table and store it in global variables
                    getAutoBidMaxAutoBiMaxdBidder(intItemID)
                    ''if there is no bid yet, make current price equal to the starting price - bidincrement
                    If CurrentPrice = "" Then
                        CurrentPrice = StartingPrice - CInt(BidIncrement)
                    End If
                    ''if value in max bid table is lower than or equal to current price, neglect it
                    If CInt(AutoBidMax) <= CInt(CurrentPrice) Then
                        AutoBidMax = ""
                    End If
                    ''get user's bid
                    Bid = CType(DataList1.Items(0).FindControl("tbMaximumBid"), TextBox).Text

                    ''This following code will do the bidding, with maximum bid feature included

                    ''check if bid high enough
                    If CInt(Bid) >= (CInt(CurrentPrice) + CInt(BidIncrement)) Then
                        ''check if there is a current maximum bid on the item
                        If AutoBidMax <> "" Then
                            ''check if user already has a maximum bid on the item
                            If AutoBidMaxBidder = intUserID And AutoBidMax >= CInt(Bid) Then
                                CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "You already have an autobid for this amount or higher!"
                                ''the user doesn't have a maximum bid on the item
                            Else
                                ''check if user's bid higher than the current maximum bid
                                If CInt(Bid) > CInt(AutoBidMax) Then
                                    ''if current winner is the logged in user only change maxium bid and don't actually bid on the item
                                    If CurrentWinner = intUserID Then
                                        insertMaxBidDB(intUserID, CInt(Bid))
                                        CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "We recorded a new maximum bid of £" & CInt(Bid) & " for you."
                                        CType(DataList1.Items(0).FindControl("lblMaxBid"), Label).Text = "Auto bidding is active. Your current maximum bid is: £" & CInt(Bid)
                                        ''bid not big enough to increment autobid by increment, so record only bid as new bid
                                    ElseIf CInt(AutoBidMax) + CInt(BidIncrement) > CInt(Bid) Then
                                        ''record autobid as a bid
                                        insertBidDB(AutoBidMaxBidder, CInt(AutoBidMax))
                                        ''notify past maximum bid holder about autobid
                                        notification(intItemID, AutoBidMaxBidder, 5)
                                        ''record user's bid
                                        insertBidDB(intUserID, CInt(Bid))
                                        ''notify past maximum bid holder that he has been outbid
                                        notification(intItemID, AutoBidMaxBidder, 1)
                                        CType(DataList1.Items(0).FindControl("MaxOfBidLabel"), Label).Text = Bid
                                        CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text = CInt(CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text) + 1
                                        CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "Thank you we recorded your bid."
                                        ''record bid as past maximumbid incremented by increment
                                    Else
                                        ''do autobidding for past maximum bidder
                                        insertBidDB(AutoBidMaxBidder, CInt(AutoBidMax))
                                        notification(intItemID, AutoBidMaxBidder, 5)
                                        ''insert bid into table
                                        insertBidDB(intUserID, CInt(AutoBidMax) + CInt(BidIncrement))
                                        notification(intItemID, AutoBidMaxBidder, 1)
                                        ''insert remainder of bid as maximum bid
                                        insertMaxBidDB(intUserID, CInt(Bid))
                                        CType(DataList1.Items(0).FindControl("MaxOfBidLabel"), Label).Text = CInt(AutoBidMax) + CInt(BidIncrement)
                                        CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text = CInt(CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text) + 1
                                        CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "You won the bid, and we recorded a new maximum bid of £" & CInt(Bid) & " for you."
                                        CType(DataList1.Items(0).FindControl("lblMaxBid"), Label).Text = "Auto bidding is active. Your current maximum bid is: £" & CInt(Bid)
                                    End If
                                    ''user's bid is less, or equal to current maximum bid
                                Else
                                    ''if bid equals to current maximum bid record only the maximum bid in the bids table 
                                    If CInt(Bid) = CInt(AutoBidMax) Then
                                        insertBidDB(AutoBidMaxBidder, CInt(AutoBidMax))
                                        notification(intItemID, AutoBidMaxBidder, 5)
                                        CType(DataList1.Items(0).FindControl("MaxOfBidLabel"), Label).Text = AutoBidMax
                                        CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text = CInt(CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text) + 1
                                        CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "You have been outbid by an AutoBid"
                                        ''if bid + bid increment would be bigger record the maximum bid as a new bid
                                    ElseIf CInt(Bid) + CInt(BidIncrement) >= CInt(AutoBidMax) Then
                                        ''record user's bid, even if it was outbid
                                        insertBidDB(intUserID, CInt(Bid))
                                        ''record autobid for owner of the maximum bid
                                        insertBidDB(AutoBidMaxBidder, CInt(AutoBidMax))
                                        notification(intItemID, AutoBidMaxBidder, 5)
                                        CType(DataList1.Items(0).FindControl("MaxOfBidLabel"), Label).Text = AutoBidMax
                                        CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text = CInt(CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text) + 1
                                        CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "You have been outbid by an AutoBid"
                                        ''record bid+bid increment as bid for user having a maximum bid on the item
                                    Else
                                        ''also record current user's bid attempt
                                        insertBidDB(intUserID, CInt(Bid))
                                        ''record autobid
                                        insertBidDB(AutoBidMaxBidder, CInt(Bid) + CInt(BidIncrement))
                                        notification(intItemID, AutoBidMaxBidder, 5)
                                        CType(DataList1.Items(0).FindControl("MaxOfBidLabel"), Label).Text = CInt(Bid) + CInt(BidIncrement)
                                        CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text = CInt(CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text) + 1
                                        CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "You have been outbid by an AutoBid"
                                    End If
                                End If
                            End If
                            ''there is no currnet maximum bid
                        Else
                            ''currnet winner is the logged in user, so only increment his/her maximum bid
                            If CurrentWinner = intUserID Then
                                insertMaxBidDB(intUserID, CInt(Bid))
                                CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "We recorded a new maximum bid of £" & CInt(Bid) & " for you."
                                CType(DataList1.Items(0).FindControl("lblMaxBid"), Label).Text = "Auto bidding is active. Your current maximum bid is: £" & CInt(Bid)
                                ''bid is not big enough for autobidding
                            ElseIf CInt(Bid) = (CInt(CurrentPrice) + CInt(BidIncrement)) Then
                                insertBidDB(intUserID, CInt(Bid))
                                CType(DataList1.Items(0).FindControl("MaxOfBidLabel"), Label).Text = CInt(Bid)
                                CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text = CInt(CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text) + 1
                                CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "Thank you we recorded your bid."
                                notification(intItemID, CurrentWinner, 1)
                                ''record bid and a new maximu bid as well
                            Else
                                ''nobody has current maximum bid on the item, and bid is bigger than currentprice + bidincrement so insert currentprice + bidincrement into bid table as user bid
                                insertBidDB(intUserID, CInt(CurrentPrice) + CInt(BidIncrement))
                                ''insert actual bid as a maximum bid into maxbid table
                                insertMaxBidDB(intUserID, CInt(Bid))
                                CType(DataList1.Items(0).FindControl("MaxOfBidLabel"), Label).Text = CInt(CurrentPrice) + CInt(BidIncrement)
                                CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text = CInt(CType(DataList1.Items(0).FindControl("CountOfBid"), Label).Text) + 1
                                CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "You won the bid, and we recorded a new maximum bid of £" & CInt(Bid) & " for you."
                                CType(DataList1.Items(0).FindControl("lblMaxBid"), Label).Text = "Auto bidding is active. Your current maximum bid is: £" & CInt(Bid)
                            End If
                        End If
                    Else
                        CType(DataList1.Items(0).FindControl("lblSendBid"), Label).Text = "Bid too low"
                    End If
                End If
            End If
            'Response.AddHeader("REFRESH", "0.5;URL=" & HttpContext.Current.Request.Url.ToString())
        ElseIf e.CommandName = "btnAddToWatchlist" Then
            addToWatchList()
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        ElseIf e.CommandName = "btnRemoveFromWatchlist" Then
            removeFromWatchList()
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        End If
    End Sub
    ''this sub handles button presses inside datalist3, what shows an item when it is already closed
    Protected Sub DataList3_ItemCommand(ByVal source As Object, _
   ByVal e As DataListCommandEventArgs) Handles DataList3.ItemCommand
        If e.CommandName = "btnAddToWatchlist" Then
            addToWatchList()
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        ElseIf e.CommandName = "btnRemoveFromWatchlist" Then
            removeFromWatchList()
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
        End If
    End Sub

    ''this function clears everything on the screen by setting select commands to empty strings, and hiding datalists
    Function clearScreen() As Integer
        SqlDataSource1.SelectCommand = ""
        SqlDataSource3.SelectCommand = ""
        SqlDataSource4.SelectCommand = ""
        DataList1.Visible = False
        DataList3.Visible = False
        divBidHistory.Visible = False
        Return 0
    End Function

    ''this function hide/shows the proper watchlist button. the second argument tells the funciton if item is closed or not
    Function setWatchButton(ByVal x As Integer, ByVal closed As Boolean) As Boolean
        ''the following database connection is to check if the item is already on watchlist for the logged in user
        Dim returnVal As Boolean
        strSQL = "SELECT [UserID] FROM [tblWatching] WHERE UserID = " & intUserID & " AND ItemID = " & x
        Dim CheckWatchlistConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim CheckWatchlistComm As New OleDbCommand(strSQL, CheckWatchlistConn)
        CheckWatchlistConn.Open()
        Dim CheckWatchlistReader As OleDbDataReader = CheckWatchlistComm.ExecuteReader()
        If CheckWatchlistReader.Read() Then           ''Match found, so user alredy watching this item, therefore make remove watchlist button visible
            If closed Then
                CType(DataList3.Items(0).FindControl("btnAddToWatchlist"), Button).Visible = False
                CType(DataList3.Items(0).FindControl("btnRemoveFromWatchlist"), Button).Visible = True
            Else
                CType(DataList1.Items(0).FindControl("btnAddToWatchlist"), Button).Visible = False
                CType(DataList1.Items(0).FindControl("btnRemoveFromWatchlist"), Button).Visible = True
            End If
        Else ''Match wasn't found, give user the option to add it to watchlist
            If closed Then
                CType(DataList3.Items(0).FindControl("btnAddToWatchlist"), Button).Visible = True
                CType(DataList3.Items(0).FindControl("btnRemoveFromWatchlist"), Button).Visible = False
            Else
                CType(DataList1.Items(0).FindControl("btnAddToWatchlist"), Button).Visible = True
                CType(DataList1.Items(0).FindControl("btnRemoveFromWatchlist"), Button).Visible = False
            End If
        End If
        CheckWatchlistReader.Close()
        CheckWatchlistConn.Close()
        Return returnVal
    End Function

    Function getUserMaxBid(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim returnVal As Integer
        strSQL = "SELECT [MaxOfMaxBid] FROM [qryMaxBid] WHERE ItemID = " & x & " AND BidderID = " & y
        Dim MaxDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MaxCommand As New OleDbCommand(strSQL, MaxDataConn)
        MaxDataConn.Open()
        Dim MaxDBReader As OleDbDataReader = MaxCommand.ExecuteReader()
        If MaxDBReader.Read() Then
            ''make sure current price exists
            If Not IsDBNull(MaxDBReader(0)) Then
                returnVal = MaxDBReader(0)
            End If
        End If
        MaxDBReader.Close()
        MaxDataConn.Close()
        Return returnVal
    End Function

    ''function to add item to watchlist
    Function addToWatchList() As Integer
        strSQL = "INSERT INTO [tblWatching] ([UserID], [ItemID]) VALUES ('" & intUserID & "', '" & intItemID & "')"
        Dim WatchlistDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim WatchlistComm As New OleDbCommand(strSQL, WatchlistDataConn)
        WatchlistDataConn.Open()
        WatchlistComm.ExecuteNonQuery()
        WatchlistComm.CommandText = "Select @@Identity"
        WatchlistDataConn.Close()
        Return 0
    End Function

    ''function to remove item from watchlist
    Function removeFromWatchList() As Integer
        strSQL = "DELETE FROM [tblWatching] WHERE UserID = " & intUserID & " AND ItemID = " & intItemID & ""
        Dim WatchlistDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim WatchlistComm As New OleDbCommand(strSQL, WatchlistDataConn)
        WatchlistDataConn.Open()
        WatchlistComm.ExecuteNonQuery()
        WatchlistComm.CommandText = "Select @@Identity"
        WatchlistDataConn.Close()
        Return 0
    End Function

    ''function to make a new bid
    Function insertBidDB(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim MakeBidConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        MakeBidConn.Open()
        strSQL = "INSERT INTO [tblBids] ([UserID], [ItemID], [Bid], [TimeOfBid]) VALUES (" & x & ", " & intItemID & ", " & y & ", '" & Date.Now & "')"
        Dim MakeBidComm As New OleDbCommand(strSQL, MakeBidConn)
        MakeBidComm.ExecuteNonQuery()
        MakeBidConn.Close()
        notification(intItemID, CInt(Seller), 2)
        Dim Watchers() As Integer = GetWatchers(intItemID)
        If Watchers IsNot Nothing Then
            For i = 0 To Watchers.Length - 1
                notification(intItemID, Watchers(i), 6)
            Next
        End If
        Return 0
    End Function

    ''function to put in a new maximum bid on an item
    Function insertMaxBidDB(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim MakeBidConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        MakeBidConn.Open()
        strSQL = "INSERT INTO [tblMaximumBid] ([BidderID], [ItemID], [MaxBid], [TimeOfMaxBid]) VALUES (" & x & ", " & intItemID & ", " & y & ", '" & Date.Now & "')"
        Dim MakeBidComm As New OleDbCommand(strSQL, MakeBidConn)
        MakeBidComm.ExecuteNonQuery()
        MakeBidConn.Close()
        If y >= ReservePrice Then
            notification(intItemID, Seller, 3)
        End If
        Return 0
    End Function

    ''this function gets current price, bid increment, starting price and seller for a given item
    Function getPriceIncrementStartSeller(ByVal x As Integer) As Integer
        strSQL = "SELECT [MaxOfBid], [BidIncrement], [StartingPrice], [SellerID], [BidderUserID], [ReservePrice] FROM [qryItemActive] WHERE qryItem.ItemID = " & x
        Dim ItemDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim ItemCommand As New OleDbCommand(strSQL, ItemDataConn)
        ItemDataConn.Open()
        Dim ItemDBReader As OleDbDataReader = ItemCommand.ExecuteReader()
        If ItemDBReader.Read() Then
            ''make sure current price exists
            If Not IsDBNull(ItemDBReader(0)) Then
                CurrentPrice = ItemDBReader(0)
                CurrentWinner = ItemDBReader(4)
            End If
            BidIncrement = ItemDBReader(1)
            StartingPrice = ItemDBReader(2)
            Seller = ItemDBReader(3)
            ReservePrice = ItemDBReader(5)
        End If
        ItemDBReader.Close()
        ItemDataConn.Close()
        Return 0
    End Function

    ''function to get the identity, and the maximum bid of the user who has a maximum bid on the item currently
    Function getAutoBidMaxAutoBiMaxdBidder(ByVal x As Integer) As Integer
        strSQL = "SELECT [MaxBid], [BidderID] FROM tblMaximumBid WHERE ItemID = " & x & " ORDER BY [MaxBid] DESC, TimeOfMaxBid"
        Dim ItemDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim ItemCommand As New OleDbCommand(strSQL, ItemDataConn)
        ItemDataConn.Open()
        Dim ItemDBReader As OleDbDataReader = ItemCommand.ExecuteReader()
        If ItemDBReader.Read() Then
            AutoBidMax = ItemDBReader(0)
            AutoBidMaxBidder = ItemDBReader(1)
        End If
        ItemDBReader.Close()
        ItemDataConn.Close()
        Return 0
    End Function

    ''function to send notification to users
    Sub notification(ByVal intItemID As Integer, ByVal intUserID As Integer, ByVal intMessageID As Integer)
        strSQL = "INSERT INTO [tblNotification] ([ItemID], [UserID], [MessageID], [AddedDate]) VALUES (" & intItemID & ", " & intUserID & ", " & intMessageID & ", '" & Date.Now & "')"
        Dim NotifDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim NotifCommand As New OleDbCommand(strSQL, NotifDataConn)
        NotifDataConn.Open()
        NotifCommand.ExecuteNonQuery()
        NotifDataConn.Close()
    End Sub

    ''the following function checks if item with given ID is closed
    Function checkItemClosed(ByVal x As Integer) As Boolean
        Dim returnVal As Boolean
        strSQL = "SELECT ID From tblItems WHERE DateClosed Is Not Null AND ID = " & x
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.Read() Then           'match found, so successful login
            returnVal = True
        Else
            returnVal = False
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()
        Return returnVal
    End Function

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
