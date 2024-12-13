Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text.Json

Public Class CatsBots
    Public ReadOnly PubQuery As CatsQuery
    Private ReadOnly PubProxy As Proxy()
    Public ReadOnly UserDetail As CatsUserDetailResponse
    Public ReadOnly HasError As Boolean
    Public ReadOnly ErrorMessage As String
    Public ReadOnly IPAddress As String

    Public Sub New(Query As CatsQuery, Proxy As Proxy())
        PubQuery = Query
        PubProxy = Proxy
        IPAddress = GetIP().Result
        PubQuery.Auth = getSession().Result
        Dim GetUser = CatsLoginAsync().Result
        If GetUser IsNot Nothing Then
            UserDetail = GetUser
            HasError = False
            ErrorMessage = ""
        Else
            UserDetail = Nothing
            HasError = True
            ErrorMessage = "login failed"
        End If
    End Sub

    Private Async Function GetIP() As Task(Of String)
        Dim client As HttpClient
        Dim FProxy = PubProxy.Where(Function(x) x.Index = PubQuery.Index)
        If FProxy.Count <> 0 Then
            If FProxy(0).Proxy <> "" Then
                Dim handler = New HttpClientHandler With {.Proxy = New WebProxy With {.Address = New Uri(FProxy(0).Proxy)}}
                client = New HttpClient(handler) With {.Timeout = New TimeSpan(0, 0, 30)}
            Else
                client = New HttpClient With {.Timeout = New TimeSpan(0, 0, 30)}
            End If
        Else
            client = New HttpClient With {.Timeout = New TimeSpan(0, 0, 30)}
        End If
        Dim httpResponse As HttpResponseMessage = Nothing
        Try
            httpResponse = Await client.GetAsync($"https://httpbin.org/ip")
        Catch ex As Exception
        End Try
        If httpResponse IsNot Nothing Then
            If httpResponse.IsSuccessStatusCode Then
                Dim responseStream = Await httpResponse.Content.ReadAsStreamAsync()
                Dim responseJson = Await JsonSerializer.DeserializeAsync(Of httpbin)(responseStream)
                Return responseJson.Origin
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function

    Private Async Function getSession() As Task(Of String)
        Dim vw As TelegramMiniApp.WebView = New TelegramMiniApp.WebView(PubQuery.API_ID, PubQuery.API_HASH, PubQuery.Name, PubQuery.Phone, "catsgang_bot", "https://api.catshouse.club/")
        Dim url As String = Await vw.Get_URL()
        If url <> "" Then
            Return url.Split(New String() {"tgWebAppData="}, StringSplitOptions.None)(1).Split(New String() {"&tgWebAppVersion"}, StringSplitOptions.None)(0)
        Else
            Return ""
        End If
    End Function

    Private Async Function CatsLoginAsync() As Task(Of CatsUserDetailResponse)
        Dim CAPI As New CatsApi(PubQuery.Auth, PubQuery.Index, PubProxy)
        Dim httpResponse = Await CAPI.CAPIGet("https://api.catshouse.club/user")
        If httpResponse IsNot Nothing Then
            If httpResponse.IsSuccessStatusCode Then
                Dim responseStream = Await httpResponse.Content.ReadAsStreamAsync()
                Dim responseJson = Await JsonSerializer.DeserializeAsync(Of CatsUserDetailResponse)(responseStream)
                Return responseJson
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Async Function CatsTasksAsync() As Task(Of CatsTasksResponse)
        Dim CAPI As New CatsApi(PubQuery.Auth, PubQuery.Index, PubProxy)
        Dim httpResponse = Await CAPI.CAPIGet("https://api.catshouse.club/tasks/user?group=cats")
        If httpResponse IsNot Nothing Then
            If httpResponse.IsSuccessStatusCode Then
                Dim responseStream = Await httpResponse.Content.ReadAsStreamAsync()
                Dim responseJson = Await JsonSerializer.DeserializeAsync(Of CatsTasksResponse)(responseStream)
                Return responseJson
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Async Function CatsTaskDoneAsync(taskId As Integer, answer As String) As Task(Of Boolean)
        Dim CAPI As New CatsApi(PubQuery.Auth, PubQuery.Index, PubProxy)
        Dim httpResponse = Await CAPI.CAPIPost($"https://api.catshouse.club/tasks/{taskId}/complete{IIf(answer = "", "", "?answer=" & answer)}", Nothing)
        If httpResponse IsNot Nothing Then
            If httpResponse.IsSuccessStatusCode Then
                Dim responseStream = Await httpResponse.Content.ReadAsStreamAsync()
                Dim responseJson = Await JsonSerializer.DeserializeAsync(Of CatsTaskDoneResponse)(responseStream)
                Return responseJson.Success
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Async Function CatsAnswers() As Task(Of Dictionary(Of String, String))
        Dim client As New HttpClient With {
            .Timeout = New TimeSpan(0, 0, 30)
        }
        client.DefaultRequestHeaders.CacheControl = New CacheControlHeaderValue With {.NoCache = True, .NoStore = True, .MaxAge = TimeSpan.FromSeconds(0)}
        Dim httpResponse As HttpResponseMessage = Nothing
        Try
            httpResponse = Await client.GetAsync("https://raw.githubusercontent.com/glad-tidings/CatsBot/refs/heads/main/tasks.json")
        Catch ex As Exception
        End Try
        If httpResponse IsNot Nothing Then
            If httpResponse.IsSuccessStatusCode Then
                Dim responseStream = Await httpResponse.Content.ReadAsStreamAsync()
                Dim responseJson = Await JsonSerializer.DeserializeAsync(Of Dictionary(Of String, String))(responseStream)
                Return responseJson
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
End Class
