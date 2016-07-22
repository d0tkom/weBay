
Imports System.Data.OleDb

Partial Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'check if user is logged in, and redirect to my account page if true
        If (Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing) Then
            Response.Redirect("MyAccount.aspx")
        End If
    End Sub
    'handle event when login button clicked
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim strSQL As String
        'all "'" signs are replaced with "''" in sql command, making sql injection impossible
        strSQL = "SELECT ID From tblUser WHERE tblUser.UserName='" & tbUsername.Text.Replace("'", "''") & "' AND tblUser.Password = '" & tbPassword.Text.Replace("'", "''") & "'"
        Dim MemberDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim MemberCommand As New OleDbCommand(strSQL, MemberDataConn)
        MemberDataConn.Open()
        Dim MemberDBReader As OleDbDataReader = MemberCommand.ExecuteReader()
        If MemberDBReader.Read() Then           'match found, so successful login
            If cbStayLoggedIn.Checked Then  'if stay logged in checked, use cookies for login
                Response.Cookies("UserName").Value = tbUsername.Text
                Response.Cookies("UserName").Expires = "May 7, 2026" ' set cookie to expire in 10 years time
                Response.Cookies("MemberID").Value = MemberDBReader("ID")
                Response.Cookies("MemberID").Expires = "May 7, 2026"
            Else 'if stay logged in not checked use session variables
                Session("UserName") = tbUsername.Text
                Session("MemberID") = MemberDBReader("ID")
            End If
        Else 'throw error message if login was unsuccesful
            lblError.Text = "Username and password do not match"
            trError.Visible = True
        End If
        MemberDBReader.Close()
        MemberDataConn.Close()

        'redirect user if login was successfull
        If (Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing) Then
            If Request.QueryString("ReturnUrl") Is Nothing Then 'redirect to MyAccounts page if there wasn't a ReturnUrl in query string
                Response.Redirect("MyAccount.aspx")
            Else
                Response.Redirect(Request.QueryString("ReturnUrl")) 'redirect user back to the page that he tried to access before logging in
            End If
        End If
    End Sub
End Class
