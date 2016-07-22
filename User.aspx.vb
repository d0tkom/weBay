Imports System.Data.OleDb
Partial Class User

    Inherits System.Web.UI.Page
    Dim intUserID As Integer
    Dim intNegativeSeller As Integer
    Dim intNeutralSeller As Integer
    Dim intPostiveSeller As Integer
    Dim intNegativeBuyer As Integer
    Dim intNeutralBuyer As Integer
    Dim intPostiveBuyer As Integer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Check if user is logged in
        If Not ((Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing)) Then
            Response.Redirect("Login.aspx?ReturnUrl=" + HttpContext.Current.Request.Url.PathAndQuery.Substring(1))
        End If

        If Request.QueryString("UserID") Is Nothing Then ' clear screen if there is no userID in query string
            SqlDataSource1.SelectCommand = ""
            SqlDataSource2.SelectCommand = ""
            SqlDataSource3.SelectCommand = ""
            dlUser.Visible = False
            dlBuyer.Visible = False
            dlSeller.Visible = False
        Else
            intUserID = Request.QueryString("UserID")
            If Not Page.IsPostBack Then
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

                SqlDataSource3.SelectCommand = "SELECT [ID], [UserName], [DateJoined] FROM [tblUser] WHERE ID = " & intUserID
                setSelectCommand()
                createRatingTable()
            End If

        End If



    End Sub

    'create ratings table
    Function createRatingTable() As Single
        Dim strSQLNegativeSeller As String = "SELECT Count(SellerRating) FROM qryUserSellerRating WHERE SellerRating = 1 AND UserID = " & intUserID
        Dim strSQLNeutralSeller As String = "SELECT Count(SellerRating) FROM qryUserSellerRating WHERE SellerRating = 2 AND UserID = " & intUserID
        Dim strSQLPositiveSeller As String = "SELECT Count(SellerRating) FROM qryUserSellerRating WHERE SellerRating = 3 AND UserID = " & intUserID

        Dim intNegSell As Integer = getRating(strSQLNegativeSeller)
        Dim intNeuSell As Integer = getRating(strSQLNeutralSeller)
        Dim intPosSell As Integer = getRating(strSQLPositiveSeller)

        lblNegativeSeller.Text = intNegSell
        lblNeutralSeller.Text = intNeuSell
        lblPositiveSeller.Text = intPosSell

        Dim strSQLNegativeBuyer As String = "SELECT Count(BuyerRating) FROM qryUserBuyerRating WHERE BuyerRating = 1 AND UserID = " & intUserID
        Dim strSQLNeutralBuyer As String = "SELECT Count(BuyerRating) FROM qryUserBuyerRating WHERE BuyerRating = 2 AND UserID = " & intUserID
        Dim strSQLPositiveBuyer As String = "SELECT Count(BuyerRating) FROM qryUserBuyerRating WHERE BuyerRating = 3 AND UserID = " & intUserID

        Dim intNegBuy As Integer = getRating(strSQLNegativeBuyer)
        Dim intNeuBuy As Integer = getRating(strSQLNeutralBuyer)
        Dim intPosBuy As Integer = getRating(strSQLPositiveBuyer)

        lblNegativeBuyer.Text = intNegBuy
        lblNeutralBuyer.Text = intNeuBuy
        lblPositiveBuyer.Text = intPosBuy

        lblTotalNegative.Text = intNegSell + intNegBuy
        lblTotalNeutral.Text = intNeuSell + intNeuBuy
        lblTotalPositive.Text = intPosSell + intPosBuy

        lblSellerTotal.Text = intNegSell + intNeuSell + intPosSell
        lblBuyerTotal.Text = intNegBuy + intNeuBuy + intPosBuy
        lblTotal.Text = CInt(lblTotalNegative.Text) + CInt(lblTotalNeutral.Text) + CInt(lblTotalPositive.Text)

        lblSellerAverage.Text = Format((intNegSell * -1 + intPosSell) / CInt(lblSellerTotal.Text), "0.00")
        lblBuyerAverage.Text = Format((intNegBuy * -1 + intPosBuy) / CInt(lblBuyerTotal.Text), "0.00")
        lblTotalAverage.Text = Format((CInt(lblTotalNegative.Text) * -1 + CInt(lblTotalPositive.Text)) / CInt(lblTotal.Text), "0.00")
        Return 0
    End Function

    'get user's ratings
    Function getRating(ByVal strSQL As String) As Single
        Dim returnValue As Integer
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        MemberDataConn.Open()
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.Read() Then
            returnValue = MemberDBReader(0)
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()
        Return returnValue
    End Function

    'set select commands for feedback datalists
    Function setSelectCommand() As Single
        If rbBuying.Checked Then
            dlBuyer.Visible = True
            dlSeller.Visible = False
            SqlDataSource1.SelectCommand = ""
            If cbDisplayNegative.Checked And cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayNeutral.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND (BuyerRating = 1 OR BuyerRating = 2)" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayPositive.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND (BuyerRating = 1 OR BuyerRating = 3)" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND (BuyerRating = 2 OR BuyerRating = 3)" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNegative.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND BuyerRating = 1" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayNeutral.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND BuyerRating = 2" & " ORDER BY BuyerRating DESC"
            ElseIf cbDisplayPositive.Checked Then
                SqlDataSource2.SelectCommand = "SELECT [Rating], [UserName], [BidderUserID], [Title], [BuyerRating], [BuyerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackBuying] WHERE BidderUserID = " & intUserID & " AND BuyerRating = 3" & " ORDER BY BuyerRating DESC"
            Else
                SqlDataSource2.SelectCommand = ""
            End If

        ElseIf rbSelling.Checked = True Then
            dlBuyer.Visible = False
            dlSeller.Visible = True
            SqlDataSource2.SelectCommand = ""
            If cbDisplayNegative.Checked And cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayNeutral.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND (SellerRating = 1 OR SellerRating = 2)" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNegative.Checked And cbDisplayPositive.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND (SellerRating = 1 OR SellerRating = 3)" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNeutral.Checked And cbDisplayPositive.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND (SellerRating = 2 OR SellerRating = 3)" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNegative.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND SellerRating = 1" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayNeutral.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND SellerRating = 2" & " ORDER BY SellerRating DESC"
            ElseIf cbDisplayPositive.Checked Then
                SqlDataSource1.SelectCommand = "SELECT [Rating], [BidderUserName], [BidderUserID], [Title], [SellerRating], [SellerFeedback], [ItemID], [SellerID] FROM [qryMyAccountFeedbackSelling] WHERE SellerID = " & intUserID & " AND SellerRating = 3" & " ORDER BY SellerRating DESC"
            Else
                SqlDataSource1.SelectCommand = ""
            End If
        End If
        Return 0
    End Function


    Protected Sub rbBuying_CheckedChanged(sender As Object, e As EventArgs) Handles rbBuying.CheckedChanged
        setSelectCommand()

    End Sub

    Protected Sub rbSelling_CheckedChanged(sender As Object, e As EventArgs) Handles rbSelling.CheckedChanged
        setSelectCommand()
    End Sub

    Protected Sub cbDisplayPositive_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayPositive.CheckedChanged
        setSelectCommand()

    End Sub

    Protected Sub cbDisplayNeutral_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayNeutral.CheckedChanged
        setSelectCommand()
    End Sub

    Protected Sub cbDisplayNegative_CheckedChanged(sender As Object, e As EventArgs) Handles cbDisplayNegative.CheckedChanged
        setSelectCommand()
    End Sub
End Class
