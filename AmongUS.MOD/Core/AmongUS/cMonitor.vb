Imports HamsterCheese.AmongUsMemory

Public Class cMonitor

#Region " Properties "

    Private Shared Monitor As Boolean = True
    Public Property Monitoring As Boolean
        Get
            Return Monitor
        End Get
        Set(value As Boolean)
            Monitor = value
        End Set
    End Property

    Private Shared IsStartedDeclare As Boolean = False
    Public Property IsStarted As Boolean
        Get
            Return IsStartedDeclare
        End Get
        Set(value As Boolean)
            IsStartedDeclare = value
        End Set
    End Property

    Private Shared GetPlayerListBool As Boolean = False
    Public Property GetPlayerList As Boolean
        Get
            Return GetPlayerListBool
        End Get
        Set(value As Boolean)
            GetPlayerListBool = value
        End Set
    End Property

    Private Shared PlayerLister As List(Of PlayerCustomInfo) = New List(Of PlayerCustomInfo)()
    Public ReadOnly Property GetPlayerInfoMonitor As List(Of PlayerCustomInfo)
        Get
            Return PlayerLister
        End Get
    End Property

    Private Shared DiedInfo As List(Of String) = New List(Of String)()
    Public ReadOnly Property AmongEvents As List(Of String)
        Get
            Return DiedInfo
        End Get
    End Property

#End Region

#Region " Declare "

    Private Shared playerDatas As List(Of PlayerData) = New List(Of PlayerData)()


#End Region

#Region " Structure "

    Public Structure PlayerCustomInfo
        Public PlayerType As String
        Public offset As String
        Public Name As String
        Public OwnerId As String
        Public PlayerId As String
        Public spawnid As String
        Public spawnflag As String
        Public ColorId As String
        Public Position As Vector2
    End Structure

#End Region

#Region " Public Methods "

    Public Sub New()
        IsStartedDeclare = True
        Dim tsk As New Task(AddressOf CheatStart, TaskCreationOptions.LongRunning)
        tsk.Start()
    End Sub

#End Region

#Region " Private Methods "

    Private Shared Sub CheatStart()
        If Cheese.Init() Then
            Cheese.ObserveShipStatus(Sub(x As UInteger)
                                         If Monitor Then
                                             DiedInfo.Clear()
                                             For Each player As PlayerData In playerDatas
                                                 player.StopObserveState()
                                             Next
                                             playerDatas = Cheese.GetAllPlayers()
                                             For Each player2 As PlayerData In playerDatas
                                                 Dim playerData As PlayerData = player2
                                                 playerData.onDie = CType([Delegate].Combine(playerData.onDie, New Action(Of Vector2, Byte)(Sub(pos As Vector2, colorId As Byte)
                                                                                                                                                DiedInfo.Add("OnPlayerDied! Color ID : " & colorId.ToString)
                                                                                                                                            End Sub)), Action(Of Vector2, Byte))
                                                 player2.StartObserveState()
                                             Next
                                         End If
                                     End Sub)
            Dim cts As System.Threading.CancellationTokenSource = New System.Threading.CancellationTokenSource()
            Task.Factory.StartNew(AddressOf UpdateCheat, cts.Token)
        End If
        System.Threading.Thread.Sleep(1000000)
    End Sub

    Private Shared Sub UpdateCheat()
        On Error Resume Next
        While True
            GetPlayerListBool = False
            If Monitor Then
                PlayerLister.Clear()
                For Each data As PlayerData In playerDatas
                    Dim PlayerType As String = String.Empty
                    If data.IsLocalPlayer Then
                        PlayerType = "Me"
                    Else
                        PlayerType = "Crewmate"
                    End If
                    If data.PlayerInfo.Value.IsDead = 1 Then
                        PlayerType = "Ghost"
                    End If
                    If data.PlayerInfo.Value.IsImpostor = 1 Then
                        PlayerType = "Impostor"
                    End If

                    Dim ResultList As PlayerCustomInfo = Nothing

                    Dim Name As String = Utils.ReadString(data.PlayerInfo.Value.PlayerName).ToString()
                    Dim Positione As Vector2 = data.Position
                    ResultList.PlayerType = PlayerType.ToString()
                    ResultList.offset = data.offset_str.ToString()
                    ResultList.OwnerId = data.Instance.OwnerId.ToString()
                    ResultList.PlayerId = data.Instance.PlayerId.ToString()
                    ResultList.spawnid = data.Instance.SpawnId.ToString()
                    ResultList.spawnflag = data.Instance.SpawnFlags.ToString()
                    ResultList.Name = Name.ToString()
                    Dim colorId As Byte = data.PlayerInfo.Value.ColorId
                    ResultList.ColorId = colorId.ToString()
                    ResultList.Position = Positione
                    PlayerLister.Add(ResultList)

                Next
                GetPlayerListBool = True
                System.Threading.Thread.Sleep(100)
                GetPlayerListBool = False
            End If
        End While

    End Sub

#End Region

End Class
