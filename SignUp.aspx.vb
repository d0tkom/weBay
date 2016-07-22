Imports System.Data.OleDb

Partial Class SignUp
    Inherits System.Web.UI.Page

    ' check if user logged in already
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If (Session("UserName") <> "") Or (Not Request.Cookies("UserName") Is Nothing) Then
            Response.Redirect("MyAccount.aspx")
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        If chkConfirm.Checked Then 'check if terms and conditions are accepted
            If Not UserExist(tbUserName.Text) Then 'check if user exists
                'add user
                Dim strSQL As String
                strSQL = "INSERT INTO [tblUser] ([UserName], [Password], [Phone], [Email], [FirstName], [LastName], [DateJoined]) VALUES ('" & tbUserName.Text.Replace("'", "''") & "', '" & tbPassword.Text.Replace("'", "''") & "', '" & tbPhone.Text.Replace("'", "''") & "', '" & tbEmail.Text.Replace("'", "''") & "', '" & tbFirstName.Text.Replace("'", "''") & "', '" & tbLastName.Text.Replace("'", "''") & "', '" & Date.Now & "')"
                Dim SignUpDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
                Dim SignUpCommand As New OleDbCommand(strSQL, SignUpDataConn)
                SignUpDataConn.Open()
                SignUpCommand.ExecuteNonQuery()
                SignUpCommand.CommandText = "Select @@Identity"
                Session("UserName") = tbUserName.Text
                Session("MemberID") = SignUpCommand.ExecuteScalar()
                SignUpDataConn.Close()
                Response.Redirect("MyAccount.aspx")
            Else
                lblError.Text = "Sorry, user already exists"
            End If
        Else
            lblConfirm.Text = "Confirm acceptance of terms & conditions, or go no further!"
        End If
    End Sub

    Function UserExist(ByVal strUserName As String) As Boolean
        Dim returnVal As Boolean
        Dim strSQL As String
        strSQL = "SELECT ID From tblUser WHERE tblUser.UserName='" & strUserName & "'"
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

End Class
