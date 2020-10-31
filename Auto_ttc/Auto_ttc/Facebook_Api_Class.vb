Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq
Public Class Facebook_Api_Class
    Public Shared Function CheckLiveCookie(cookie As String, Optional userAgent As String = "", Optional proxy As String = "") As String
        Dim output As String = "die"
        Dim uid As String = Regex.Match(cookie, "c_user=(.*?);").Groups(1).Value
        Dim request As Http_Helper = New Http_Helper(cookie, userAgent, "")
        Dim flag As Boolean = uid <> ""
        If flag Then
            Dim html As String = request.RequestGet("https://www.facebook.com/me").ToString()
            Dim flag2 As Boolean = html.Contains("id=""code_in_cliff""")
            If flag2 Then
                output = "die"
            Else
                Dim flag3 As Boolean = Regex.Match(html, """USER_ID"":""(.*?)""").Groups(1).Value.Trim() = uid.Trim() AndAlso Not html.Contains("checkpointSubmitButton")
                If flag3 Then
                    output = "live"
                End If
            End If
        End If
        Return output
    End Function
    Public Shared Function CheckLiveToken(ByVal token As String) As Boolean
        Dim output As Boolean = False
        Dim request As Http_Helper = New Http_Helper("", "", "")
        Try
            Dim html As String = request.RequestGet("https://graph.facebook.com/me/?access_token=" & token).ToString()
            Dim flag As Boolean = JObject.Parse(html)("id").ToString() <> ""
            If flag Then
                output = True
            End If
        Catch
        End Try
        Return output
    End Function
    Public Shared Function Get_Token(cookie As String)
        Dim result As String = ""
        Try
            Dim request As New Http_Helper(cookie, "", "")
            Dim data_response As String = request.RequestGet("https://m.facebook.com/composer/ocelot/async_loader/?publisher=feed").ToString()
            Dim token As String = ""
            token = data_response.Replace("\", "")
            token = Regex.Match(token, """accessToken"":""(.*?)""").Groups(1).Value
            result = token
        Catch ex As Exception
            result = "error"
        End Try
        Return result
    End Function
    Public Shared Function Follow(token As String, uid As String, cookie As String)
        Dim result As String = ""
        Try
            Dim request_follow As New Http_Helper(cookie, "", "")
            Dim data_follow As String = request_follow.RequestPost("https://graph.facebook.com/" & uid & "/subscribers", "access_token=" & token)
            If Not data_follow.Contains("error") Then
                Return "success"
            End If
            Dim is_block = Regex.Match(data_follow, "code(.*?),")
            If is_block.Success Then
                If is_block.Groups(1).Value.Contains("368") Then
                    Return "blocked"
                End If
                Return "fail"
            End If
            Return "fail"
        Catch ex As Exception
            result = "error"
        End Try
        Return result
    End Function
    Public Shared Function Like_Post(token As String, uid As String, cookie As String)
        Dim result As String = ""
        Try
            Dim request_like As New Http_Helper(cookie, "", "")
            Dim data_like As String = request_like.RequestPost("https://graph.facebook.com/" & uid & "/likes", "access_token=" & token)
            If Not data_like.Contains("error") Then
                Return "success"
            End If
            Dim is_block = Regex.Match(data_like, "code(.*?),")
            If is_block.Success Then
                If is_block.Groups(1).Value.Contains("368") Then
                    Return "blocked"
                End If
                Return "fail"
            End If
            Return "fail"
        Catch ex As Exception
            result = "error"
        End Try
        Return result
    End Function
    Public Shared Function Like_Page_Api(idpage As String, cookiefb As String)
        Try
            Dim linl_url As String
            Dim request As Http_Helper = New Http_Helper(cookiefb)
            Dim response As String = request.RequestGet("https://mbasic.facebook.com/" + idpage)
            linl_url = request.Response_Uri()
            Dim src As String
            'Dim linkld1 As String = linl_url.Replace("?_rdr", "")
            src = request.RequestGet(linl_url)
            Dim link As String = Regex.Match(src, "/a/profile.php(.*?)refid=(.*?)""").Groups(1).Value
            link = "https://mbasic.facebook.com/a/profile.php" & link.Replace("amp;", "") & "refid=17"
            Dim sc As String = request.RequestGet(link)
            Return "success"
        Catch ex As Exception
            Return "fail"
        End Try
    End Function
    Public Shared Function get_link_src(text As String, ID As String, num As String)
        Dim match = Regex.Match(text, "ft_ent_identifier=" & ID & "&amp;reaction_type=" & num & "(.*?)""")
        If match.Success = True Then
            Return match.Groups(1).Value
        Else
            Return Nothing
        End If
    End Function
    Public Shared Function React_API(cookie As String, id_react As String, type_react As String)
        Try
            Dim linl_url As String
            Dim request As Http_Helper = New Http_Helper(cookie)
            Dim response As String = request.RequestGet("https://mbasic.facebook.com/" + id_react)
            linl_url = request.Response_Uri()
            Dim src As String
            src = request.RequestGet(linl_url)
            Dim link As String = Regex.Match(src, "/reactions/picker/(.*?)""").Groups(1).Value
            If link.Length < 2 Then
                Return "not_found"
            End If
            link = "https://mbasic.facebook.com/reactions/picker/" & link.Replace("amp;", "") '& "refid=13"
            Dim sc As String = request.RequestGet(link)
            Dim get_react_link As String
            Dim loaicx As String
            Dim requset_react As String
            If type_react.Contains("SAD") Then
                Dim Lmao As String = get_link_src(sc, id_react, "7") 'Regex.Match(sc, "ft_ent_identifier=" & id_react & "&amp;reaction_type=7(.*?)""").Groups(1).Value
                get_react_link = "https://mbasic.facebook.com/ufi/reaction/?" & "ft_ent_identifier=" & id_react & "&amp;reaction_type=7" & Lmao
                get_react_link = get_react_link.Replace("amp;", "")
                'Nhan_Tien_React(cookie_tds, id_react, "SAD")
                requset_react = request.RequestGet(get_react_link)
                loaicx = "SAD"
            End If
            If type_react.Contains("ANGRY") Then
                Dim Lmao As String = get_link_src(sc, id_react, "8") 'Regex.Match(sc, "ft_ent_identifier=" & id_react & "&amp;reaction_type=8(.*?)""").Groups(1).Value
                get_react_link = "https://mbasic.facebook.com/ufi/reaction/?" & "ft_ent_identifier=" & id_react & "&amp;reaction_type=8" & Lmao
                get_react_link = get_react_link.Replace("amp;", "")
                'Nhan_Tien_React(cookie_tds, id_react, "ANGRY")
                requset_react = request.RequestGet(get_react_link)
                loaicx = "ANGRY"
            End If
            If type_react.Contains("LOVE") Then
                Dim Lmao As String = get_link_src(sc, id_react, "2")
                get_react_link = "https://mbasic.facebook.com/ufi/reaction/?" & "ft_ent_identifier=" & id_react & "&amp;reaction_type=2" & Lmao
                get_react_link = get_react_link.Replace("amp;", "")
                'Nhan_Tien_React(cookie_tds, id_react, "LOVE")
                requset_react = request.RequestGet(get_react_link)
                loaicx = "LOVE"
            End If
            If type_react.Contains("HAHA") Then
                Dim Lmao As String = get_link_src(sc, id_react, "4") 'Regex.Match(sc, "ft_ent_identifier=" & id_react & "&amp;reaction_type=4(.*?)""").Groups(1).Value
                get_react_link = "https://mbasic.facebook.com/ufi/reaction/?" & "ft_ent_identifier=" & id_react & "&amp;reaction_type=4" & Lmao
                get_react_link = get_react_link.Replace("amp;", "")
                'Nhan_Tien_React(cookie_tds, id_react, "HAHA")
                requset_react = request.RequestGet(get_react_link)
                loaicx = "HAHA"
            End If
            If type_react.Contains("WOW") Then
                Dim Lmao As String = get_link_src(sc, id_react, "3") 'Regex.Match(sc, "ft_ent_identifier=" & id_react & "&amp;reaction_type=3(.*?)""").Groups(1).Value
                get_react_link = "https://mbasic.facebook.com/ufi/reaction/?" & "ft_ent_identifier=" & id_react & "&amp;reaction_type=3" & Lmao
                get_react_link = get_react_link.Replace("amp;", "")
                'Nhan_Tien_React(cookie_tds, id_react, "WOW")
                requset_react = request.RequestGet(get_react_link)
                loaicx = "WOW"
            End If
            Return "success"
        Catch ex As Exception
            Return "error"
        End Try
    End Function
End Class
