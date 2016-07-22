Imports System.Data.OleDb
Imports System.Collections.Generic

Partial Class Search

    Inherits System.Web.UI.Page
    Dim strSearch As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("Search") Is Nothing Then
            SqlDataSource1.SelectCommand = ""
        ElseIf Not Page.IsPostBack Then
            strSearch = Request.QueryString("Search")
            'this select commands does the search. it lookes for matches inside keywords, and the titles, and matches the beginning, ending, or middle of any word
            SqlDataSource1.SelectCommand = "SELECT DISTINCT tblItems.Title, tblItems.ID, qryCategoryActive.MaxOfBid, qryCategoryActive.CountOfBid, qryCategoryActive.Condition, qryCategoryActive.Picture FROM qryCategoryActive INNER JOIN (tblItems INNER JOIN tblKeywords ON tblItems.ID = tblKeywords.ItemID) ON qryCategoryActive.ItemID = tblItems.ID WHERE (((tblKeywords.Keyword) Like '%" & strSearch.Replace("'", "''") & "%')) OR (((tblItems.Title) Like '%" & strSearch.Replace("'", "''") & "%'))"
            DirectCast(Master.FindControl("txtTitle"), TextBox).Text = strSearch 'fill searchbox with search term
        End If
    End Sub

    'web service that serves the ajax search
    <System.Web.Services.WebMethod()> _
    Public Shared Function txtTitleChanged(ByVal Title As String) As Object
        Dim strSQL As String
        Dim jsonList As New List(Of Item)()
        If Len(Title) > 0 Then
            strSQL = "SELECT DISTINCT tblItems.Title, tblItems.ID, qryCategoryActive.MaxOfBid, qryCategoryActive.CountOfBid, qryCategoryActive.Condition, qryCategoryActive.Picture FROM qryCategoryActive INNER JOIN (tblItems INNER JOIN tblKeywords ON tblItems.ID = tblKeywords.ItemID) ON qryCategoryActive.ItemID = tblItems.ID WHERE (((tblKeywords.Keyword) Like '%" & Title.Replace("'", "''") & "%')) OR (((tblItems.Title) Like '%" & Title.Replace("'", "''") & "%'))"
            Dim TitleDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & HttpContext.Current.Server.MapPath("App_Data/weBay.mdb"))
            Dim TitleCommand As New OleDbCommand(strSQL, TitleDataConn)
            TitleDataConn.Open()
            Dim TitleDBReader As OleDbDataReader = TitleCommand.ExecuteReader()
            While TitleDBReader.Read
                Dim MaxOfBid As String
                If IsDBNull(TitleDBReader("MaxOfBid")) Then 'make sure maxbid exists
                    MaxOfBid = ""
                Else
                    MaxOfBid = TitleDBReader("MaxOfBid")
                End If
                Dim thisItem As New Item(TitleDBReader("Title"), TitleDBReader("ID"), MaxOfBid, TitleDBReader("CountOfBid"), TitleDBReader("Condition"), TitleDBReader("Picture"))
                jsonList.Add(thisItem)
            End While
            TitleDBReader.Close()
            TitleDataConn.Close()
        End If
        Return jsonList
    End Function

    Public Class Item
        Public ItemTitle As String
        Public ItemID As String
        Public ItemMaxOfBid As String
        Public ItemCountOfBid As String
        Public ItemCondition As String
        Public ItemPicture As String
        Public Sub New(ByVal dbItemTitle As String, ByVal dbItemID As String, ByVal dbItemMaxOfBid As String, ByVal dbItemCountOfBid As String, ByVal dbItemCondition As String, ByVal dbItemPicture As String)
            ItemTitle = dbItemTitle
            ItemID = dbItemID
            ItemMaxOfBid = dbItemMaxOfBid
            ItemCountOfBid = dbItemCountOfBid
            ItemCondition = dbItemCondition
            ItemPicture = dbItemPicture
        End Sub
    End Class
End Class
