Imports System.Data.OleDb
Partial Class _Default
    Inherits System.Web.UI.Page
    ''declaring global variables for easier access between subs
    Dim intUserID As Integer
    Dim strBackground As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''check if user is logged in for setting picture on main page
        If (Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing) Then
            ''get member ID of user if logged in
            If CStr(Session("MemberID")) <> "" Then
                ''get UserID if user logged in using session variable
                intUserID = Session("MemberID")
            Else
                ''get userID if user logged inusing cookie
                intUserID = CInt(Server.HtmlEncode(Request.Cookies("MemberID").Value))
            End If

            ''the following database connection will try to get the custom main screen image for the logged in user
            Dim strSQL As String
            strSQL = "SELECT Background FROM tblUser WHERE ID = " & intUserID
            Dim BackgroundDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
            Dim BackgroundCommand As New OleDbCommand(strSQL, BackgroundDataConn)
            BackgroundDataConn.Open()
            Dim BackgroundDBReader As OleDbDataReader = BackgroundCommand.ExecuteReader()
            If BackgroundDBReader.Read() Then
                ''make sure url exists
                If Not IsDBNull(BackgroundDBReader(0)) Then
                    strBackground = BackgroundDBReader(0)
                End If
            End If
            BackgroundDBReader.Close()
            BackgroundDataConn.Close()
            ''if there is custom background, change to it
            If strBackground <> "" Then
                imgMain.ImageUrl = strBackground
            End If
        End If
    End Sub


End Class
