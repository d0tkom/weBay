Imports System.Data.OleDb
Partial Class Category
    Inherits System.Web.UI.Page
    ''declare global variables for easier access between subs
    Dim intCategoryID As Integer
    Dim intSubCategoryID As Integer
    Dim strSQL As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''don't show anything if there is no id for category nor subcategory in query string
        If Request.QueryString("CategoryID") Is Nothing And Request.QueryString("SubCategoryID") Is Nothing Then
            SqlDataSource1.SelectCommand = ""
            ''if there is subcategoryID in query string it will take precedence, and items in that subcategory will be shown
        ElseIf Not Request.QueryString("SubCategoryID") Is Nothing Then
            ''get subcategoryID
            intSubCategoryID = CInt(Request.QueryString("SubCategoryID"))
            ''specify sql select command for datalist
            SqlDataSource1.SelectCommand = "SELECT [Title], [UserName], [Picture], [MaxOfBid], [CountOfBid], [Condition], [ItemID] FROM [qryCategoryActive] WHERE SubCategoryID = " & intSubCategoryID
            ''edit location labels, to help user navigate
            lblLocation.Text = "<a href='Category.aspx?CategoryID=" & getSubCategory(intSubCategoryID)(1) & "'>" & getSubCategory(intSubCategoryID)(0) & "</a>"
            lblLocation.Text += " > <a href='Category.aspx?SubCategoryID=" & intSubCategoryID & "'>" & getSubCategory(intSubCategoryID)(2) & "</a>"
            ''if there is categoryID in query string but no subcategory, than items in given category will be shown
        ElseIf Not Request.QueryString("CategoryID") Is Nothing Then
            intCategoryID = CInt(Request.QueryString("CategoryID"))
            SqlDataSource1.SelectCommand = "SELECT [Title], [UserName], [Picture], [MaxOfBid], [CountOfBid], [Condition], [ItemID] FROM [qryCategoryActive] WHERE tblCategories.ID = " & intCategoryID
            lblLocation.Text = "<a href='Category.aspx?CategoryID=" & intCategoryID & "'>" & getCategory(intCategoryID) & "</a> > "
        End If
    End Sub

    ''this is a function to get the name of a category for a given CategoryID
    Function getCategory(ByVal x As Integer) As String
        Dim strCategory As String
        strSQL = "SELECT Category FROM qryLocation WHERE CategoryID = " & x
        Dim CategoryDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim CategoryCommand As New OleDbCommand(strSQL, CategoryDataConn)
        CategoryDataConn.Open()
        Dim CategoryDBReader As OleDbDataReader = CategoryCommand.ExecuteReader()
        If CategoryDBReader.Read() Then
            strCategory = CategoryDBReader(0)
        End If
        CategoryDBReader.Close()
        CategoryDataConn.Close()
        Return strCategory
    End Function

    ''this is a function to get the CategoryName, the CategoryID, and the SubCategoryName for a given subcategoryID
    ''It returns these values in an array
    Function getSubCategory(ByVal x As Integer) As String()
        Dim strSubCategory(3) As String
        strSQL = "SELECT Category, CategoryID, subCategory FROM qryLocation WHERE subCategoryID = " & x
        Dim subCategoryDataConn As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; data source=" & Server.MapPath("App_Data/weBay.mdb"))
        Dim subCategoryCommand As New OleDbCommand(strSQL, subCategoryDataConn)
        subCategoryDataConn.Open()
        Dim subCategoryDBReader As OleDbDataReader = subCategoryCommand.ExecuteReader()
        If subCategoryDBReader.Read() Then
            strSubCategory(0) = subCategoryDBReader(0)
            strSubCategory(1) = subCategoryDBReader(1)
            strSubCategory(2) = subCategoryDBReader(2)
        End If
        subCategoryDBReader.Close()
        subCategoryDataConn.Close()
        Return strSubCategory
    End Function

End Class
