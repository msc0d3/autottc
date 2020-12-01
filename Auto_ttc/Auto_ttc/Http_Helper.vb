Imports System
Imports System.Linq
Imports System.Text.RegularExpressions
Imports xNet
Public Class Http_Helper
    Private request As xNet.HttpRequest
    Public Sub New(Optional ByVal cookie As String = "", Optional ByVal userAgent As String = "", Optional ByVal proxy As String = "")
        Dim flag As Boolean = userAgent = ""
        If flag Then
            userAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36"
        End If
        Me.request = New xNet.HttpRequest With {
            .KeepAlive = True,
            .AllowAutoRedirect = True,
            .Cookies = New CookieDictionary(False),
            .userAgent = userAgent
        }
        Me.request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8")
        Me.request.AddHeader("Accept-Language", "en-US,en;q=0.9")
        Me.request.AddHeader("x-requested-with", "XMLHttpRequest")
        Dim flag2 As Boolean = cookie <> ""
        If flag2 Then
            Me.AddCookie(cookie)
        End If
        Dim flag3 As Boolean = proxy <> ""
        If flag3 Then
            Select Case proxy.Split(New Char() {":"c}).Count()
                Case 1
                    Me.request.Proxy = Socks5ProxyClient.Parse("127.0.0.1:" & proxy)
                Case 2
                    Me.request.Proxy = Socks5ProxyClient.Parse(proxy)
                Case 4
                    Me.request.Proxy = New HttpProxyClient(proxy.Split(New Char() {":"c})(0), Convert.ToInt32(proxy.Split(New Char() {":"c})(1)), proxy.Split(New Char() {":"c})(2), proxy.Split(New Char() {":"c})(3))
            End Select
        End If
    End Sub
    Public Function RequestGet(ByVal url As String) As String
        Return Me.request.Get(url, Nothing).ToString()
    End Function
    Public Function RequestPost(ByVal url As String, Optional ByVal data As String = "", Optional ByVal referer As String = "", Optional ByVal contentType As String = "application/x-www-form-urlencoded") As String
        Dim result As String
        Try
            Dim flag As Boolean = referer <> ""
            If flag Then
                Me.request.AddHeader("Referer", referer)
            End If
            Dim flag2 As Boolean = data = "" OrElse contentType = ""
            If flag2 Then
                result = Me.request.Post(url).ToString()
            Else
                result = Me.request.Post(url, data, contentType).ToString()
            End If
        Catch e1 As Exception
            result = "error"
        End Try
        Return result
    End Function
    Public Sub AddCookie(ByVal cookie As String)
        'Me.request.AddHeader("cookie", cookie)
        Dim temp() As String = cookie.Split(New Char() {";"c})
        For Each item As String In temp
            Dim temp2() As String = item.Split(New Char() {"="c})
            Dim flag As Boolean = temp2.Count() > 1
            If flag Then
                Me.request.Cookies.Add(temp2(0), temp2(1))
            End If
        Next item
    End Sub
    Public Function Response_Uri()
        Return Me.request.Response.Address.ToString()
    End Function
    Public Function GetCookie() As String
        Return Me.request.Cookies.ToString()
    End Function

End Class
