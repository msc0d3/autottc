Imports System.IO
Imports Microsoft.Win32
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports Newtonsoft.Json.Linq
Public Class Helper
    Public Shared Function Logo()
        Return "
 █████╗ ██╗   ██╗████████╗ ██████╗     ████████╗████████╗ ██████╗
██╔══██╗██║   ██║╚══██╔══╝██╔═══██╗    ╚══██╔══╝╚══██╔══╝██╔════╝
███████║██║   ██║   ██║   ██║   ██║       ██║      ██║   ██║     
██╔══██║██║   ██║   ██║   ██║   ██║       ██║      ██║   ██║     
██║  ██║╚██████╔╝   ██║   ╚██████╔╝       ██║      ██║   ╚██████╗
╚═╝  ╚═╝ ╚═════╝    ╚═╝    ╚═════╝        ╚═╝      ╚═╝    ╚═════╝                                                     
"
    End Function
    Public Shared Sub Write_Line_Status(message As String, type As Integer)
        Try
            If type = 1 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(message)
            End If
            If type = 2 Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(message)
            End If
            If type = 3 Then
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine(message)
            End If
            Console.ResetColor()
        Catch ex As Exception

        End Try
    End Sub
End Class
