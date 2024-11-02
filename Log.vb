Imports System.Globalization
Imports System.IO

Public Class Log
    Public Shared Sub Show(Game As String, Account As String, Message As String, Color As ConsoleColor)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write($"[{Date.Now.ToString("yyyy-MM-dd HH:mm:ss")}] ")
        Console.ForegroundColor = ConsoleColor.DarkYellow
        Console.Write($"[{Game}] ")
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.Write($"[{Account}] ")
        Console.ForegroundColor = Color
        Console.WriteLine(Message)
        Console.ResetColor()

        Save(Game, Account, Message)
    End Sub

    Public Shared Sub Save(Game As String, Account As String, Message As String)
        Try
            Dim st As New StreamWriter($"logs\{Game}-{Date.Now.ToString("yyyy-MM-dd", New CultureInfo("en-US"))}.log", True)
            st.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss", New CultureInfo("en-US"))}] [{Account}] {Message}")
            st.Close()
        Catch ex As Exception
        End Try
    End Sub
End Class
