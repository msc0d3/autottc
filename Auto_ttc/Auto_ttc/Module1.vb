Imports xNet
Imports System.Threading
Imports System.IO
Imports System.Text.RegularExpressions
Module Module1
    Private LIKE_PAGE As Boolean
    Private FOLLOW As Boolean
    Private Like_Post As Boolean
    Private REACT As Boolean
    Private LOOP_COUNT As Integer
    Private SLEEP_COUNT As Integer
    Private SLEEP_COUNT_1vong As Integer
    Private COUNT_MISSION As Integer
    Private user_ttc As String
    Private password_ttc As String
    Private VersionCli As Double = 2.2
    Private Linkupdate As String = ""
    ''' <summary>
    ''' @2020 Open Source Code : FAM Software, tienichmmo.net
    ''' Coder : Nguyen Dac Tai
    ''' Date : 31/10/2020
    ''' Note : Please don't remove this note !!! | xin đừng xoá dòng này !
    ''' </summary>
    Sub Main()
        Dim Http_Helper As New Http_Helper()
        Dim response As String = Http_Helper.RequestGet("https://tienichmmo.net/api/tool_autottc/version")
        If response = "error" Then
            MsgBox("bạn không có quyền truy cập vào tool này !", "access denied !", MsgBoxStyle.Critical)
            Environment.Exit(0)
        End If
        Dim version_server As String = Regex.Match(response, "ver = (.*?),").Groups(1).Value
        If VersionCli < Convert.ToDouble(version_server) Then
            MsgBox("phát hiện thấy phiên bản mới ! lập tức update...")
            Linkupdate = Regex.Match(response, "Link_update = ""(.*?)""").Groups(1).Value
            Try
                Process.Start("chrome.exe", Linkupdate)
            Catch ex As Exception
                Process.Start(Linkupdate)
            End Try
            Environment.Exit(0)
        End If
        Dim responseIsOnline = Http_Helper.RequestGet("https://tienichmmo.net/api/tool_autottc/server")
        If responseIsOnline = "error" Then
            MsgBox("bạn không có quyền truy cập vào tool này !", "access denied !", MsgBoxStyle.Critical)
            Environment.Exit(0)
        End If
        If responseIsOnline.Contains("OFF") Then
            'khoá
        End If
        Console.WriteLine(Helper.Logo())
        Helper.Write_Line_Status("By FAM Software, tienichmmo.net", 3)
        Console.WriteLine("______________________________________________________")
        Load_Setting()
        Start()
        Console.ReadLine()
    End Sub
    Private Sub Start()
        Dim count_loop As Integer = 0
        Dim is_done As Boolean = False
        Dim read_ck As String = File.ReadAllText("cookie.txt")
        Do
            'Start:
            'For Each cookies As String In read_ck
            Helper.Write_Line_Status("DANG CHECK LIVE FB...", 3)
            If count_loop >= LOOP_COUNT Then
                is_done = True
            End If
            Load_Setting()
            Dim cookie_fb As String = read_ck
            Dim token_fb As String = Facebook_Api_Class.Get_Token(cookie_fb)
            Dim is_live_fb As String = Facebook_Api_Class.CheckLiveCookie(cookie_fb)
            If is_live_fb = "die" Then
                Helper.Write_Line_Status("FACE BOOK DA DIE !", 1)
                Exit Sub
            End If
            Helper.Write_Line_Status("FB LIVE !", 2)
            Helper.Write_Line_Status("DANG LOGIN VÀO TTC...", 3)
            Dim cookie_tds As String = TTC_API_CLASS.Get_Cookie_TTC(user_ttc, password_ttc)
            If cookie_tds = "sai_tk" Or cookie_tds = "error" Then
                Helper.Write_Line_Status("TAI KHOAN TTC LOI !", 1)
                Exit Sub
            End If
            Helper.Write_Line_Status("LOGIN TTC OK !..", 2)
            If Like_Post = True Then
                TTC_API_CLASS.Like_Post_List(cookie_tds, token_fb, cookie_fb, SLEEP_COUNT)
            End If
            If FOLLOW = True Then
                TTC_API_CLASS.Follow_List(cookie_tds, token_fb, cookie_fb, SLEEP_COUNT)
            End If
            If LIKE_PAGE = True Then
                TTC_API_CLASS.Like_Page_List(cookie_tds, token_fb, cookie_fb, SLEEP_COUNT)
            End If
            If REACT = True Then
                TTC_API_CLASS.React_List(cookie_tds, cookie_fb, SLEEP_COUNT)
            End If
            count_loop += 1
            Helper.Write_Line_Status("XONG 1 VONG ! NGHI TRONG " & SLEEP_COUNT_1vong.ToString() & " GIAY", 3)
            Thread_Sleep(SLEEP_COUNT_1vong)
            'Next

        Loop While is_done = False
        Helper.Write_Line_Status("DA CHAY XONG " & LOOP_COUNT.ToString() & " VONG !", 3)
    End Sub
    Private Sub Thread_Sleep(time As Integer)
        Try
            Thread.Sleep(TimeSpan.FromSeconds(time))
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Load_Setting()
        Try
            Dim read_ttc_acc As String = File.ReadAllText("acc_ttc.txt")
            Dim read As String = File.ReadAllText("config.txt")
            Dim is_like As String = Regex.Match(read, "LIKE (.*?),").Groups(1).Value
            If is_like.Contains("ON") Then
                Like_Post = True
            Else
                Like_Post = False
            End If
            Dim is_like_page As String = Regex.Match(read, "LIKE_PAGE (.*?),").Groups(1).Value
            If is_like_page.Contains("ON") Then
                LIKE_PAGE = True
            Else
                LIKE_PAGE = False
            End If

            Dim is_follow As String = Regex.Match(read, "FOLLOW (.*?),").Groups(1).Value
            If is_follow.Contains("ON") Then
                FOLLOW = True
            Else
                FOLLOW = False
            End If

            Dim is_react As String = Regex.Match(read, "REACT (.*?),").Groups(1).Value
            If is_react.Contains("ON") Then
                REACT = True
            Else
                REACT = False
            End If
            Dim is_loop_count As Integer = Regex.Match(read, "SO_VONG_LAP (.*?),").Groups(1).Value
            LOOP_COUNT = is_loop_count
            Dim time_sleep As Integer = Regex.Match(read, "TIME_NGHI (.*?),").Groups(1).Value
            SLEEP_COUNT = time_sleep
            Dim time_sleep_1vong As Integer = Regex.Match(read, "TIME_NGHI_1vong (.*?),").Groups(1).Value
            SLEEP_COUNT_1vong = time_sleep_1vong
            Dim num_mission As Integer = Regex.Match(read, "SO_NV (.*?),").Groups(1).Value
            Setting_Class.SONV = num_mission
            COUNT_MISSION = num_mission
            Dim data_acc As String() = read_ttc_acc.Split("|")
            user_ttc = data_acc(0)
            password_ttc = data_acc(1)
            Console.Title = "Auto TTC v2.2 FREE VERSION By tienichmmo.net | account_ttc > " & user_ttc
        Catch ex As Exception
            Helper.Write_Line_Status("File acc_ttc.txt not found !", 1)
            Console.ReadLine()
        End Try
    End Sub

End Module
