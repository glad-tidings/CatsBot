Imports System.Net
Imports System.Net.Http

Public Class CatsApi

    Private ReadOnly _client As HttpClient

    Public Sub New(queryID As String, queryIndex As Integer, Proxy As Proxy())
        Dim FProxy = Proxy.Where(Function(x) x.Index = queryIndex)
        If FProxy.Count <> 0 Then
            If FProxy(0).Proxy <> "" Then
                Dim handler = New HttpClientHandler With {.Proxy = New WebProxy With {.Address = New Uri(FProxy(0).Proxy)}}
                _client = New HttpClient(handler) With {.Timeout = New TimeSpan(0, 0, 30)}
            Else
                _client = New HttpClient With {.Timeout = New TimeSpan(0, 0, 30)}
            End If
        Else
            _client = New HttpClient With {.Timeout = New TimeSpan(0, 0, 30)}
        End If
        'client.DefaultRequestHeaders.CacheControl = New CacheControlHeaderValue With {.NoCache = True, .NoStore = True, .MaxAge = TimeSpan.FromSeconds(0)}
        _client.DefaultRequestHeaders.Add("Authorization", $"tma {queryID}")
        _client.DefaultRequestHeaders.Add("Accept-Language", "en-US")
        _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache")
        _client.DefaultRequestHeaders.Add("Pragma", "no-cache")
        _client.DefaultRequestHeaders.Add("Priority", "u=1, i")
        _client.DefaultRequestHeaders.Add("Origin", "https://cats-frontend.tgapps.store")
        _client.DefaultRequestHeaders.Add("Referer", "https://cats-frontend.tgapps.store/")
        _client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty")
        _client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors")
        _client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site")
        _client.DefaultRequestHeaders.Add("Sec-Ch-Ua", """Google Chrome"";v=""125"", ""Chromium"";v=""125"", ""Not.A/Brand"";v=""24""")
        _client.DefaultRequestHeaders.Add("User-Agent", Tools.getUserAgents(queryIndex))
        _client.DefaultRequestHeaders.Add("accept", "*/*")
        _client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0")
        _client.DefaultRequestHeaders.Add("sec-ch-ua-platform", $"""{Tools.getUserAgents(queryIndex, True)}""")
    End Sub

    Public Async Function CAPIGet(requestUri As String) As Task(Of HttpResponseMessage)
        Try
            Return Await _client.GetAsync(requestUri)
        Catch ex As Exception
            Return New HttpResponseMessage With {.StatusCode = HttpStatusCode.ExpectationFailed, .ReasonPhrase = ex.Message}
        End Try
    End Function

    Public Async Function CAPIPost(requestUri As String, content As HttpContent) As Task(Of HttpResponseMessage)
        Try
            Return Await _client.PostAsync(requestUri, content)
        Catch ex As Exception
            Return New HttpResponseMessage With {.StatusCode = HttpStatusCode.ExpectationFailed, .ReasonPhrase = ex.Message}
        End Try
    End Function
End Class
