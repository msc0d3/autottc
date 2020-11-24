Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class TTC_API_CLASS
    Public Shared Function Get_Cookie_TTC(username As String, password As String)
        Dim try_count As Integer = 5
        Dim is_success As Boolean = False
        Do
            Try
                Dim request_cookie As New Http_Helper("", "", "")
                Dim data_respose As String = request_cookie.RequestPost("https://tuongtaccheo.com/login.php", "password=" & password & "&submit=ĐĂNG NHẬP" & "&username=" & username)
                If Not data_respose.Contains("Số dư") Then
                    Return "sai_tk"
                End If
                Return request_cookie.GetCookie()
            Catch ex As Exception
                try_count += 1
                If try_count >= 5 Then
                    is_success = True
                    Return "error"
                End If
            End Try
        Loop While is_success = False
    End Function
    Public Shared Function Dat_acc_chay(cookie_ttc As String, ID_FB As String)
        Try
            Dim request As Http_Helper = New Http_Helper(cookie_ttc)
            Dim response As String = request.RequestPost("https://tuongtaccheo.com/cauhinh/datnick.php", "iddat%5B%5D=" & ID_FB)
            If Not response = "1" Then
                Return "error"
            End If
            Return "success"
        Catch ex As Exception
            Return "error"
        End Try
    End Function
    Public Shared Sub Like_Post_List(cookie_ttc As String, token As String, cookiefb As String, timenghi As Integer)
        Try
            Dim request As Http_Helper = New Http_Helper(cookie_ttc)
            Dim response As String = request.RequestGet("https://tuongtaccheo.com/kiemtien/getpost.php")
            If response = "[]" Then
                Helper.Write_Line_Status("HET NHIEM VU LIKE !", 1)
                Exit Sub
            Else
                Dim res = Regex.Matches(response, "idpost"":""(\d+)"",""", RegexOptions.Singleline)
                Dim count As Integer = res.Count
                Console.WriteLine("TIM THAY : " & count.ToString() & " NHIEM VU LIKE !")
                Dim misssion_count As Integer = 0
                For i As Integer = 1 To count
                    If misssion_count >= Setting_Class.SONV Then
                        Helper.Write_Line_Status("XONG 1 VONG LIKE !", 3)
                        Exit Sub
                    End If
                    Dim id_posst As String = Regex.Match(res(i).ToString(), "idpost"":""(\d+)"",""").Groups(1).Value
                    Dim like_status As String = Facebook_Api_Class.Like_Post(token, id_posst, cookiefb)
                    If like_status = "success" Then
                        Dim is_nhantien_success As String = NHAN_TIEN(id_posst, cookie_ttc, "like", False, "")
                        If is_nhantien_success = "success" Then
                            Helper.Write_Line_Status("LIKE THANH CONG ID " & id_posst & " + 300 XU > " & Lay_so_du(cookie_ttc), 2)
                        Else
                            Helper.Write_Line_Status("NHAN TIEN THAT BAI " & id_posst, 1)
                        End If
                    End If
                    If like_status = "blocked" Then
                        Helper.Write_Line_Status("DA BI BLOCK TINH NANG !", 1)
                        Exit Sub
                    End If
                    TTC_API_CLASS.Thread_Sleep(timenghi)
                    misssion_count += 1
                Next
            End If
        Catch ex As Exception
            Helper.Write_Line_Status("CO LOI XAY RA !", 1)
            Exit Sub
        End Try
    End Sub
    Public Shared Sub Follow_List(cookie_ttc As String, token As String, cookiefb As String, timenghi As Integer)
        Try
            Dim request As Http_Helper = New Http_Helper(cookie_ttc)
            Dim response As String = request.RequestGet("https://tuongtaccheo.com/kiemtien/subcheo/getpost.php")
            If response = "[]" Then
                Helper.Write_Line_Status("HET NHIEM VU FOLLOW !", 1)
                Exit Sub
            Else
                Dim res = Regex.Matches(response, "idpost"":""(\d+)"",""", RegexOptions.Singleline)
                Dim count As Integer = res.Count
                Console.WriteLine("TIM THAY : " & count.ToString() & " NHIEM VU FOLLOW !")
                Dim misssion_count As Integer = 0
                For i As Integer = 1 To count
                    If misssion_count >= Setting_Class.SONV Then
                        Helper.Write_Line_Status("XONG 1 VONG FOLLOW !", 3)
                        Exit Sub
                    End If
                    Dim id_posst As String = Regex.Match(res(i).ToString(), "idpost"":""(\d+)"",""").Groups(1).Value
                    Dim like_status As String = Facebook_Api_Class.Follow(token, id_posst, cookiefb)
                    If like_status = "success" Then
                        Dim is_nhantien_success As String = NHAN_TIEN(id_posst, cookie_ttc, "follow", False, "")
                        If is_nhantien_success = "success" Then
                            Helper.Write_Line_Status("FOLLOW THANH CONG ID " & id_posst & " + 600 XU > " & Lay_so_du(cookie_ttc), 2)
                        Else
                            Helper.Write_Line_Status("NHAN TIEN THAT BAI " & id_posst, 1)
                        End If
                    End If
                    If like_status = "blocked" Then
                        Helper.Write_Line_Status("DA BI BLOCK TINH NANG !", 1)
                        Exit Sub
                    End If
                    TTC_API_CLASS.Thread_Sleep(timenghi)
                    misssion_count += 1
                Next
            End If
        Catch ex As Exception
            Helper.Write_Line_Status("CO LOI XAY RA", "fail")
            Exit Sub
        End Try
    End Sub
    Public Shared Sub Like_Page_List(cookie_ttc As String, token As String, cookiefb As String, timenghi As Integer)
        Try
            Dim request As Http_Helper = New Http_Helper(cookie_ttc)
            Dim response As String = request.RequestGet("https://tuongtaccheo.com/kiemtien/likepagecheo/getpost.php")
            If response = "[]" Then
                Helper.Write_Line_Status("HET NHIEM VU LIKE PAGE !", 1)
                Exit Sub
            Else
                Dim res = Regex.Matches(response, "idpost"":""(\d+)"",""", RegexOptions.Singleline)
                Dim count As Integer = res.Count
                Console.WriteLine("TIM THAY : " & count.ToString() & " NHIEM VU LIKE PAGE !")
                Dim misssion_count As Integer = 0
                For i As Integer = 1 To count
                    If misssion_count >= Setting_Class.SONV Then
                        Helper.Write_Line_Status("XONG 1 VONG LIKE PAGE !", 3)
                        Exit Sub
                    End If
                    Dim id_posst As String = Regex.Match(res(i).ToString(), "idpost"":""(\d+)"",""").Groups(1).Value
                    Dim like_status As String = Facebook_Api_Class.Like_Page_Api(id_posst, cookiefb)
                    If like_status = "success" Then
                        Dim is_nhantien_success As String = NHAN_TIEN(id_posst, cookie_ttc, "likepage", False, "")
                        If is_nhantien_success = "success" Then
                            Helper.Write_Line_Status("LIKE PAGE THANH CONG ID " & id_posst & " + 700 XU > " & Lay_so_du(cookie_ttc), 2)
                        Else
                            Helper.Write_Line_Status("NHAN TIEN THAT BAI " & id_posst, 1)
                        End If
                    End If
                    If like_status = "blocked" Then
                        Helper.Write_Line_Status("DA BI BLOCK TINH NANG !", 1)
                        Exit Sub
                    End If
                    TTC_API_CLASS.Thread_Sleep(timenghi)
                    misssion_count += 1
                Next
            End If
        Catch ex As Exception
            Helper.Write_Line_Status("CO LOI XAY RA", 1)
            Exit Sub
        End Try
    End Sub
    Public Shared Sub React_List(cookie_ttc As String, cookiefb As String, timenghi As Integer)
        Try
            Dim request As Http_Helper = New Http_Helper(cookie_ttc)
            Dim response As String = request.RequestGet("https://tuongtaccheo.com/kiemtien/camxuccheo/getpost.php")
            If response = "[]" Then
                Helper.Write_Line_Status("HET NHIEM VU REACT !", 1)
                Exit Sub
            Else
                Dim res = Regex.Matches(response, "idpost"":""(.*?)},", RegexOptions.Singleline)
                Dim count As Integer = res.Count
                Console.WriteLine("TIM THAY : " & count.ToString() & " NHIEM VU REACT !")
                Dim misssion_count As Integer = 0
                For i As Integer = 1 To count
                    If misssion_count >= Setting_Class.SONV Then
                        Helper.Write_Line_Status("XONG 1 VONG REACT !", 3)
                        Exit Sub
                    End If
                    Dim id_posst As String = Regex.Match(res(i).ToString(), "idpost"":""(\d+)"",""").Groups(1).Value
                    Dim type_react As String = Regex.Match(res(i).ToString(), "loaicx"":""(.*?)""").Groups(1).Value
                    Dim react_status As String = Facebook_Api_Class.React_API(cookiefb, id_posst, type_react)
                    If react_status = "success" Then
                        Dim is_nhantien_success As String = NHAN_TIEN(id_posst, cookie_ttc, "react", True, type_react)
                        If is_nhantien_success = "success" Then
                            Helper.Write_Line_Status("REACT THANH CONG ID " & id_posst & " + 400 XU > " & Lay_so_du(cookie_ttc), 2)
                        Else
                            Helper.Write_Line_Status("NHAN TIEN THAT BAI " & id_posst, 1)
                        End If
                    End If
                    If react_status = "not_found" Then
                        Helper.Write_Line_Status("ID REACT KHONG TON TAI : " & id_posst, 1)
                        Exit Sub
                    End If
                    TTC_API_CLASS.Thread_Sleep(timenghi)
                    misssion_count += 1
                Next
            End If
        Catch ex As Exception
            Helper.Write_Line_Status("CO LOI XAY RA", 1)
            Exit Sub
        End Try
    End Sub
    Public Shared Sub Thread_Sleep(time As Integer)
        Try
            Thread.Sleep(TimeSpan.FromSeconds(time))
        Catch ex As Exception

        End Try
    End Sub
    Public Shared Function Get_Url(type As String)
        If type = "like" Then
            Return "https://tuongtaccheo.com/kiemtien/nhantien.php"
        End If
        If type = "follow" Then
            Return "https://tuongtaccheo.com/kiemtien/subcheo/nhantien.php"
        End If
        If type = "likepage" Then
            Return "https://tuongtaccheo.com/kiemtien/likepagecheo/nhantien.php"
        End If
        If type = "react" Then
            Return "https://tuongtaccheo.com/kiemtien/camxuccheo/nhantien.php"
        End If
    End Function
    Public Shared Function NHAN_TIEN(id_nhan As String, cookie_ttc As String, type_get As String, is_react As Boolean, loaicx As String)
        Try
            Dim URL_GET_MONEY As String = TTC_API_CLASS.Get_Url(type_get)
            Dim request As Http_Helper = New Http_Helper(cookie_ttc)
            Dim data_post As String = ""
            If is_react = True Then
                data_post = "id=" & id_nhan & "&loaicx=" & loaicx
            Else
                data_post = "id=" & id_nhan
            End If
            Dim response As String = request.RequestPost(URL_GET_MONEY, data_post)
            If response.Contains("Thành công") Then
                Return "success"
            Else
                Return "fail"
            End If
        Catch ex As Exception
            Return "error"
        End Try
    End Function
    Public Shared Function Lay_so_du(cookie_ttc As String)
        Try
            Dim rq As Http_Helper = New Http_Helper(cookie_ttc)
            Dim response As String = rq.RequestGet("https://tuongtaccheo.com/cauhinh/")
            Dim sodu As String = Regex.Match(response, "soduchinh(.*?)<").Groups(1).Value
            sodu = Regex.Match(sodu, "(\d+)").Groups(1).Value
            Return sodu
        Catch ex As Exception
            Return "error"
        End Try
    End Function
End Class
