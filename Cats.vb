﻿Imports System.Text.Json.Serialization

Public Class CatsQuery
    <JsonPropertyName("Index")>
    Public Property Index As Integer
    <JsonPropertyName("Name")>
    Public Property Name As String
    <JsonPropertyName("Auth")>
    Public Property Auth As String
    <JsonPropertyName("Active")>
    Public Property Active As Boolean
    <JsonPropertyName("Task")>
    Public Property Task As Boolean
    <JsonPropertyName("TaskSleep")>
    Public Property TaskSleep As Integer()
    <JsonPropertyName("DaySleep")>
    Public Property DaySleep As Integer()
    <JsonPropertyName("NightSleep")>
    Public Property NightSleep As Integer()
End Class

Public Class Proxy
    <JsonPropertyName("Index")>
    Public Property Index As Integer
    <JsonPropertyName("Proxy")>
    Public Property Proxy As String
End Class

Public Class httpbin
    <JsonPropertyName("origin")>
    Public Property Origin As String
End Class

Public Class CatsUserDetailResponse
    <JsonPropertyName("id")>
    Public Property Id As Long
    <JsonPropertyName("firstName")>
    Public Property FirstName As String
    <JsonPropertyName("lastName")>
    Public Property LastName As String
    <JsonPropertyName("username")>
    Public Property Username As String
    <JsonPropertyName("telegramAge")>
    Public Property TelegramAge As Integer
    <JsonPropertyName("referrerReward")>
    Public Property ReferrerReward As Integer
    <JsonPropertyName("telegramAgeReward")>
    Public Property TelegramAgeReward As Integer
    <JsonPropertyName("tasksReward")>
    Public Property TasksReward As Integer
    <JsonPropertyName("referentReward")>
    Public Property ReferentReward As Integer
    <JsonPropertyName("avatarReward")>
    Public Property AvatarReward As Integer
    <JsonPropertyName("premiumRewards")>
    Public Property PremiumRewards As Integer
    <JsonPropertyName("externalRewards")>
    Public Property ExternalRewards As Integer
    <JsonPropertyName("totalRewards")>
    Public Property TotalRewards As Integer
    <JsonPropertyName("rewardsSpent")>
    Public Property RewardsSpent As Integer
    <JsonPropertyName("currentRewards")>
    Public Property CurrentRewards As Integer
End Class

Public Class CatsTasksResponse
    <JsonPropertyName("tasks")>
    Public Property Tasks As List(Of CatsTask)
End Class

Public Class CatsTask
    <JsonPropertyName("id")>
    Public Property Id As Long
    <JsonPropertyName("title")>
    Public Property Title As String
    <JsonPropertyName("type")>
    Public Property Type As String
    <JsonPropertyName("rewardPoints")>
    Public Property RewardPoints As Integer
    <JsonPropertyName("group")>
    Public Property Group As String
    <JsonPropertyName("completed")>
    Public Property Completed As Boolean
    <JsonPropertyName("isPending")>
    Public Property IsPending As Boolean
    <JsonPropertyName("allowCheck")>
    Public Property AllowCheck As Boolean
End Class

Public Class CatsTaskDoneResponse
    <JsonPropertyName("success")>
    Public Property Success As Boolean
End Class