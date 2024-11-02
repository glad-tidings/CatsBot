Imports System.IO
Imports System.Text.Json
Imports System.Threading

Module Program

    Private proxies As Proxy()

    Sub Main()
        Console.WriteLine("---------------------------------  Cats Bot Starting  ---------------------------------")
        Console.WriteLine()

        Dim queries As New List(Of CatsQuery)
        Dim jsonConfig As String = ""
        Dim jsonProxy As String = ""
        Try
            jsonConfig = File.ReadAllText("Config.json")
        Catch ex As Exception
            Console.WriteLine("file 'Config.json' not found")
            GoTo Get_Error
        End Try
        Try
            jsonProxy = File.ReadAllText("Proxy.json")
        Catch ex As Exception
            Console.WriteLine("file 'Proxy.json' not found")
            GoTo Get_Error
        End Try
        Try
            queries = JsonSerializer.Deserialize(Of List(Of CatsQuery))(jsonConfig)
            proxies = JsonSerializer.Deserialize(Of Proxy())(jsonConfig)
        Catch ex As Exception
            Console.WriteLine("configuration is wrong")
            GoTo Get_Error
        End Try

        Dim Cats As New Thread(
            Sub()
                Dim counter As Integer = 0
                For Each Query In queries.Where(Function(x) x.Active)
                    Dim BotThread As New Thread(Sub() CatsThread(counter, Query))
                    BotThread.Start()

                    Thread.Sleep(1000)
                    counter += 1
                Next
            End Sub)
        Cats.Start()

Get_Error:
        Console.ReadLine()
    End Sub

    Public Async Sub CatsThread(Index As Integer, Query As CatsQuery)
        Thread.Sleep(120000 * Index)

        While True
            Dim RND As New Random()

            Try
                Dim Bot As New CatsBots(Query, proxies)
                If Not Bot.HasError Then
                    Log.Show("Cats", Query.Name, $"my ip '{Bot.IPAddress}'", ConsoleColor.White)
                    Log.Show("Cats", Query.Name, $"synced successfully. B<{Bot.UserDetail.CurrentRewards}>", ConsoleColor.Blue)
                    If Query.Task Then
                        Dim taskList = Await Bot.CatsTasksAsync()
                        If taskList IsNot Nothing Then
                            For Each task In taskList.Tasks.Where(Function(x) x.Completed = False And x.IsPending = False And x.AllowCheck = False And x.Type <> "YOUTUBE_WATCH" And x.Title.StartsWith("Invite") = False)
                                Dim claimTask = Await Bot.CatsTaskDoneAsync(task.Id, "")
                                If claimTask Then
                                    Log.Show("Cats", Query.Name, $"task '{task.Title}' completed", ConsoleColor.Green)
                                Else
                                    Log.Show("Cats", Query.Name, $"task '{task.Title}' failed", ConsoleColor.Red)
                                End If

                                Dim eachtaskRND As Integer = RND.Next(Query.TaskSleep(0), Query.TaskSleep(1))
                                Thread.Sleep(eachtaskRND * 1000)
                            Next

                            Dim taskAnswers = Await Bot.CatsAnswers()
                            If taskAnswers IsNot Nothing Then
                                For Each task In taskList.Tasks.Where(Function(x) x.Completed = False And x.IsPending = False And x.AllowCheck = False And x.Type = "YOUTUBE_WATCH")
                                    Dim taskAnswer As String = ""
                                    If taskAnswers.TryGetValue(task.Title, taskAnswer) Then
                                        Dim claimTask = Await Bot.CatsTaskDoneAsync(task.Id, taskAnswer)
                                        If claimTask Then
                                            Log.Show("Cats", Query.Name, $"task '{task.Title}' completed", ConsoleColor.Green)
                                        Else
                                            Log.Show("Cats", Query.Name, $"task '{task.Title}' failed", ConsoleColor.Red)
                                        End If

                                        Dim eachtaskRND As Integer = RND.Next(Query.TaskSleep(0), Query.TaskSleep(1))
                                        Thread.Sleep(eachtaskRND * 1000)
                                    End If
                                Next
                            End If
                        End If
                    End If
                Else
                    Log.Show("Cats", Query.Name, $"{Bot.ErrorMessage}", ConsoleColor.Red)
                End If
            Catch ex As Exception
                Log.Show("Cats", Query.Name, $"Error: {ex.Message}", ConsoleColor.Red)
            End Try

            Dim syncRND As Integer = 0
            If Date.Now.Hour < 8 Then
                syncRND = RND.Next(Query.NightSleep(0), Query.NightSleep(1))
            Else
                syncRND = RND.Next(Query.DaySleep(0), Query.DaySleep(1))
            End If
            Log.Show("Cats", Query.Name, $"sync sleep '{Int(syncRND / 3600)}h {Int((syncRND Mod 3600) / 60)}m {syncRND Mod 60}s'", ConsoleColor.Yellow)
            Thread.Sleep(syncRND * 1000)
        End While
    End Sub
End Module