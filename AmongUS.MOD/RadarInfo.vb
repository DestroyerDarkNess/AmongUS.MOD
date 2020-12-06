Imports System.IO
Imports AmongUS.MOD.Core.Values
Imports AmongUS.MOD.LogFuncs
Imports AmongUS.MOD.cMonitor
Imports AmongUS.MOD.Core.AmongCustomFuncs

Public Class RadarInfo

#Region " Declare "

    Private AmongMonitorRadarHook As cMonitor = Form1.AmongMonitorHook

    Private RadarInfoList As New List(Of RadarPlayerInfo)

    Public Structure RadarPlayerInfo
        Public PlayerState As String
        Public Name As String
        Public ColorID As String
        Public Position As Vector2
    End Structure

#End Region

    Private Sub RadarInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PlayerCheats.Enabled = True
        Timer1.Enabled = True
    End Sub

    Private Sub PlayerCheats_Tick(sender As Object, e As EventArgs) Handles PlayerCheats.Tick
        On Error Resume Next
        Dim p As Process()
        p = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessGame))
        If p.Count = 0 Then
            AmongMonitorRadarHook.Monitoring = False
        End If
        '
        If AmongMonitorRadarHook.Monitoring = True Then

            RadarInfoList.Clear()

            Dim RefresbucleInfo As List(Of PlayerCustomInfo) = AmongMonitorRadarHook.GetPlayerInfoMonitor

            For Each PlayerInfo As PlayerCustomInfo In RefresbucleInfo
                Dim NewRadarInfo As New RadarPlayerInfo
                NewRadarInfo.PlayerState = PlayerInfo.PlayerType
                NewRadarInfo.Name = PlayerInfo.Name
                NewRadarInfo.ColorID = PlayerInfo.ColorId
                NewRadarInfo.Position = PlayerInfo.Position
                RadarInfoList.Add(NewRadarInfo)
            Next
            ListPlayers(RadarInfoList)
        End If


    End Sub

    Private Sub PanelClear()

        For Each ControlsC As Control In Panel1.Controls
            Dim ContinueTask As Boolean = True
            Dim Cname As String = ControlsC.Name

            If Cname = "GunaSeparator1" Then
                ContinueTask = False
            End If

            If Cname = "GunaVSeparator1" Then
                ContinueTask = False
            End If

            If ContinueTask = True Then
                Panel1.Controls.Remove(ControlsC)
            End If
        Next
    End Sub

    Private Sub ListPlayers(ByVal PlayerInfoR As List(Of RadarPlayerInfo))
        PanelClear()
        On Error Resume Next

        For Each PlayerInfo As RadarPlayerInfo In PlayerInfoR

            ' Colección modificada; puede que no se ejecute la operación de enumeración.

            Dim PlayerControlItem As New RadarPlayerInfoControl
            '  PlayerControlItem.PlayerType = PlayerInfo.PlayerState.ToString
            '  PlayerControlItem.PlayerName = PlayerInfo.Name.ToString
            PlayerControlItem.Name = PlayerInfo.Name.ToString
            PlayerControlItem.ColorID = PlayerInfo.ColorID.ToString
            PlayerControlItem.Visible = False
            Panel1.Controls.Add(PlayerControlItem)

            Dim PlayerCenterPoint As Point = New Point(Val((Panel1.Width / 2) - 6), Val((Panel1.Height / 2) - 6))

            Dim MaximusX As Integer = Panel1.Width
            Dim MaximusY As Integer = Panel1.Height

            Dim CalculateXPos As Integer = MapForPanelPosX(Val(PlayerInfo.Position.x.ToString))
            Dim CalculateYPos As Integer = MapForPanelPosY(Val(PlayerInfo.Position.y.ToString))

            If CalculateXPos > MaximusX Then : CalculateXPos = MaximusX : End If
            If CalculateYPos > MaximusY Then : CalculateYPos = MaximusY : End If

            Dim PlayerPosition As Point = New Point(CalculateXPos, CalculateYPos)
            PlayerControlItem.Location = New Point(PlayerPosition)

            If PlayerInfo.PlayerState = "Me" Then
                PlayerControlItem.Location = New Point(PlayerCenterPoint)
            Else
                PlayerControlItem.Location = PlayerPosition
            End If

            PlayerControlItem.Visible = True
        Next
    End Sub

    Private Function MapForPanelPosX(ByVal PlayerXpos As String) As Integer
        Dim CalculateXPos As String = PlayerXpos.ToString

        If CalculateXPos.Contains("-") = True Then '-X

            Dim ParseXPos As Integer = Val(CalculateXPos.Replace("-", "").ToString)

            'Panel -X Area | min = 0 , max = 110
            'Game Map -X Area| min= -20 , Max = 0 

            '  100% --- -20
            '  X ---Player(-x)
            Dim CalculatePorcentage As Integer = Val((ParseXPos * 100) / 20) '(-20 Maximun -X World map)

            '100% --- 110 '(110 Is -X Panel Area)
            '(CalculatePorcentage) ---  x
            Dim AdaptoPanel As Integer = 110 - Val((CalculatePorcentage * 100) / 110)

            Return AdaptoPanel

        Else

            Dim PlayXPos As Integer = Val(PlayerXpos)

            'Panel X Area | min = 110 , max = 220
            'Game Map X Area| min= 0 , Max = 20

            '  100% --- 20
            '  X ---Player(x)
            Dim CalculatePorcentage As Integer = Val((PlayXPos * 100) / 20) '(20 Maximun X World map)

            '100% --- 220 '(110 Is X Panel Area)
            '(CalculatePorcentage) ---  x
            Dim AdaptoPanel As Integer = 110 + Val((CalculatePorcentage * 100) / 110)

            Return AdaptoPanel

        End If
        Return 0
    End Function

    Private Function MapForPanelPosY(ByVal PlayerYpos As String) As Integer
        Dim CalculateYPos As String = PlayerYpos.ToString

        If CalculateYPos.Contains("-") = True Then '-Y

            Dim ParseYPos As Integer = Val(CalculateYPos.Replace("-", "").ToString)

            'Panel -Y Area | min = 95 , max = 190
            'Game Map -Y Area| min= 0 , Max = 20 

            '  100% --- -20
            '  X ---Player(-Y)
            Dim CalculatePorcentage As Integer = Val((ParseYPos * 100) / 17) '(-17 Maximun -X World map)

            '100% --- 190 '(190 Is -Y Panel Area)
            '(CalculatePorcentage) ---  x
            Dim AdaptoPanel As Integer = 95 + Val((CalculatePorcentage * 100) / 190)
         

            Return AdaptoPanel

        Else

            Dim PlayYPos As Integer = Val(PlayerYpos)

            'Panel Y Area | min = 0 , max = 95
            'Game Map Y Area| min= 0 , Max = 6


            '  100% --- 6
            '  X ---Player(y)
            Dim CalculatePorcentage As Integer = Val((PlayYPos * 100) / 6) '(6 Maximun Y World map)

            '100% --- 95 '(95 Is Y Panel Area)
            '(CalculatePorcentage) ---  x
            Dim AdaptoPanel As Integer = 95 - Val((CalculatePorcentage * 100) / 95)

           
            Return AdaptoPanel

        End If
        Return 0
    End Function

#Region " End Monitor "

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Form1.GunaGoogleSwitch1.Checked = True Then
            If IsFormVisible() = False Then
                Me.Show()
            End If
        Else
            Me.Hide()
        End If
    End Sub


    Private Function IsFormVisible() As Boolean
        For Each form In My.Application.OpenForms
            If (form.name = Me.Name) Then
                If form.Visible Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

#End Region



    'Mappos ist die Position des Spieler's als Punkt deklariert. Mit der WorldToRadar Funktion berechnen wir die Position der einzelnen Spieler.
    '  Mappos = New Point(WorldToRadar(New Point(meinspieler.x, meinspieler.y), New Point(gegner.x, gegner.y), meinspieler.yaw, Panel1.Width, Panel1.Height, 12))

End Class