Imports System.Data.OleDb
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Dim intUserID As Integer
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'check if user if logged in
        If (Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing) Then
            'setup buttons
            btnLogOut.Visible = True
            btnLogin.Visible = False
            btnMyAccount.Visible = True
            btnSignup.Visible = False

            ''get user id
            If CStr(Session("MemberID")) <> "" Then
                intUserID = Session("MemberID")
            Else
                intUserID = CInt(Server.HtmlEncode(Request.Cookies("MemberID").Value))
            End If
            'setup welcome message
            If Session("UserName") <> "" Then
                lblUserName.Text = "Hello <a href='User.aspx?UserID=" & intUserID & "'>" & Session("UserName") & "</a>!"
            Else
                lblUserName.Text = "Hello <a href='User.aspx?UserID=" & intUserID & "'>" & Server.HtmlEncode(Request.Cookies("UserName").Value) & "</a>!"
            End If
            'check if there is a won item without feedback for user to see
            If checkBidWon() Then
                btnBidWon.Visible = True
            Else
                btnBidWon.Visible = False
            End If
            'check if there is any new notification for user
            If checkNotification() Then
                btnNotif.Visible = True
            Else
                btnNotif.Visible = False
            End If

        Else ''user not logged, in setup screen
            btnLogOut.Visible = False
            btnLogin.Visible = True
            btnMyAccount.Visible = False
            btnSignup.Visible = True
            lblUserName.Text = "Hello stranger! "
        End If
    End Sub

    'sub to logout user
    Protected Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Session("UserName") = "" 'set session variables to empty strings
        Session("MemberID") = ""
        Response.Cookies("UserName").Expires = DateTime.Now.AddDays(-1D) 'expire cookies
        Response.Cookies("MemberID").Expires = DateTime.Now.AddDays(-1D)
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True) 'refresh page so "logged out" state of page will be shown
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Response.Redirect("Search.aspx?Search=" & txtTitle.Text) 'redirect user to search page with given search term
    End Sub

    Protected Sub btnBidWon_Click(sender As Object, e As EventArgs) Handles btnBidWon.Click
        Response.Redirect("MyAccount.aspx?Tab=Purchase&FeedbackLeft=False&NoFeedback=True&") 'redirect user to page where he can see won items
    End Sub

    Protected Sub btnSell_Click(sender As Object, e As EventArgs) Handles btnSell.Click
        Response.Redirect("Sell.aspx")
    End Sub

    Protected Sub btnNotif_Click(sender As Object, e As EventArgs) Handles btnNotif.Click
        Response.Redirect("MyAccount.aspx?Tab=Notification") 'redirect user to notifications page
    End Sub
    'db connection to check if user has won an item where he hasn't left feedback yet
    Function checkBidWon() As Boolean
        Dim returnVal As Boolean
        Dim strSQL As String
        strSQL = "SELECT ID From qryMasterPage WHERE BuyerID = " & intUserID
        Dim BidWonDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim BidWonCommand As New OleDbCommand(strSQL, BidWonDataConn)
        BidWonDataConn.Open()
        Dim BidWonDBReader As OleDbDataReader = BidWonCommand.ExecuteReader()
        If BidWonDBReader.Read() Then
            returnVal = True
        Else
            returnVal = False
        End If
        BidWonDBReader.Close()
        BidWonDataConn.Close()
        Return returnVal
    End Function
    'db connection to check if user has any unseen notification
    Function checkNotification() As Boolean
        Dim returnVal As Boolean
        Dim strSQL = "SELECT ID From tblNotification WHERE UserID = " & intUserID & " AND Seen = 0"
        Dim NotificationDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim NotificationCommand As New OleDbCommand(strSQL, NotificationDataConn)
        NotificationDataConn.Open()
        Dim NotificationDBReader As OleDbDataReader = NotificationCommand.ExecuteReader()
        If NotificationDBReader.Read() Then
            returnVal = True
        Else
            returnVal = False
        End If
        NotificationDBReader.Close()
        NotificationDataConn.Close()
        Return returnVal
    End Function

End Class

