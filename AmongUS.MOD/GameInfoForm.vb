Imports AmongUS.MOD.cMonitor
Imports System.IO
Imports AmongUS.MOD.Core.Values
Imports AmongUS.MOD.LogFuncs
Public Class GameInfoForm

#Region " Declare "

    Private AmongEvents As New List(Of String)
    Private AmongMonitorHook As cMonitor = Form1.AmongMonitorHook
    Private IntEx As Boolean = False

#End Region

    Private Sub GameInfoForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try : AddHandler Application.ThreadException, AddressOf Application_Exception_Handler _
            : Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False) _
                : Catch : End Try

        IntEx = True
        PlayerMonitor.Enabled = True
        Timer1.Enabled = True
    End Sub

    Private Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        WriteLog(ex.Message, InfoType.Exception)
    End Sub

    Private Sub PlayerMonitor_Tick(sender As Object, e As EventArgs) Handles PlayerMonitor.Tick
        On Error Resume Next
        Dim p As Process()
        p = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessGame))
        If p.Count = 0 Then
            AmongMonitorHook.Monitoring = False
        End If

        If AmongMonitorHook.Monitoring = True Then
           
            If AmongMonitorHook.GetPlayerList = True Then

                Dim Listar As Boolean = True

                Dim PlayerInfoV As List(Of PlayerCustomInfo) = AmongMonitorHook.GetPlayerInfoMonitor

                Dim PlayersCount As Integer = PlayerInfoV.Count
                Dim CurrentPlayersCount As Integer = Panel1.Controls.Count

                For Each CurrentC As Control In Panel1.Controls
                    Dim Controlname As String = CurrentC.Name
                    For Each PlayerInfoTi As PlayerCustomInfo In PlayerInfoV
                        If Not PlayerInfoTi.PlayerType = "Me" Then
                            If Controlname = PlayerInfoTi.Name Then
                                Listar = False
                            End If
                        End If
                    Next
                Next

                If Not CurrentPlayersCount = PlayersCount Then
                    Listar = True
                End If

                If Listar = True Then
                    ListPlayers(PlayerInfoV)
                End If

            End If
        End If
        AmongEvents = AmongMonitorHook.AmongEvents
        ListEvent()
    End Sub

    Private Sub ListEvent()
        GunaTextBox2.Text = ""
        For Each Events As String In AmongEvents
            GunaTextBox2.Text += Events & vbNewLine
        Next
    End Sub

    Private Sub ListPlayers(ByVal PlayerInfoV As List(Of PlayerCustomInfo))
        Panel1.Controls.Clear()
        On Error Resume Next
        Dim ItemsCount As Integer = 0
        Dim Local_X As Integer = 0
        Dim Local_Y As Integer = 2

        For Each PlayerInfo As PlayerCustomInfo In PlayerInfoV

            ' Colección modificada; puede que no se ejecute la operación de enumeración.

            ItemsCount += 1
            Dim PlayerControlItem As New PlayerInfoControl
            PlayerControlItem.PlayerType = PlayerInfo.PlayerType.ToString
            PlayerControlItem.PlayerName = PlayerInfo.Name.ToString
            PlayerControlItem.Name = PlayerInfo.Name.ToString
            PlayerControlItem.Offset = PlayerInfo.offset.ToString
            PlayerControlItem.ColorID = PlayerInfo.ColorId.ToString

            Panel1.Controls.Add(PlayerControlItem)

            PlayerControlItem.Location = New Point(Local_X, Local_Y)

            If ItemsCount = 1 Then
                Local_X = 0
                Local_Y += 30

                ItemsCount = 0

            End If

        Next
    End Sub

#Region " Attach to Client "

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        On Error Resume Next
        Call load_()
        Dim proc As Process() = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessGame))
        Dim MyHeight = Me.Height
        Dim windowname As String = proc(0).MainWindowTitle
        hWnd = FindWindow(vbNullString, windowname)
        GetWindowThreadProcessId(hWnd, processID)
        pHandle = OpenProcess(PROCESS_VM_ALL, 0, processID)
        GetWindowRect(Overlay.hWnd, window_loc)
        Me.Location = New Point((window_loc.Left + 10), (window_loc.Top + 35))
        Timer3.Enabled = False
    End Sub

#End Region

#Region " End Monitor "

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Form1.GunaGoogleSwitch17.Checked = True Then
            If IsFormVisible() = False Then
                Me.Show()
            End If
        Else
            Me.Hide()
        End If
    End Sub


    Private Function IsFormVisible() As Boolean
        For Each form In My.Application.OpenForms
            If (form.Name = Me.Name) Then
                If form.Visible Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

#End Region

#Region " Cheats "

    Private Sub GunaGoogleSwitch17_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch17.CheckedChanged
        If IntEx = True Then
            AmongMonitorHook.Monitoring = GunaGoogleSwitch17.Checked
        End If
    End Sub

#End Region


End Class