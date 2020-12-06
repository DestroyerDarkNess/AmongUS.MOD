Imports System.Runtime.InteropServices
Imports AmongUS.MOD.Core.Values
Imports Memory
Imports AmongUS.MOD.LogFuncs
Imports System.IO
Imports HamsterCheese.AmongUsMemory
Imports System.Threading

Public Class Form1

#Region " Pinvokes "

    <DllImport("user32.dll")> _
    Shared Function GetAsyncKeyState(ByVal vKey As System.Windows.Forms.Keys) As Short
    End Function

    Private Structure WINDOWPLACEMENT
        Public Length As Integer
        Public flags As Integer
        Public showCmd As Integer
        Public ptMinPosition As POINTAPI
        Public ptMaxPosition As POINTAPI
        Public rcNormalPosition As RECT
    End Structure

    Private Const SW_SHOWMAXIMIZED As Integer = 3
    Private Const SW_SHOWMINIMIZED As Integer = 2
    Private Const SW_SHOWNORMAL As Integer = 1


    Private Structure POINTAPI
        Public x As Integer
        Public y As Integer
    End Structure


    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Private Declare Function GetWindowPlacement Lib "user32" (ByVal hwnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As IntPtr

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function IsWindowVisible(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Private Function GetWindowStats(ByVal handle As IntPtr) As Boolean
        Dim wp As WINDOWPLACEMENT
        wp.Length = System.Runtime.InteropServices.Marshal.SizeOf(wp)
        GetWindowPlacement(handle, wp)
        Dim WindowsVisible As Boolean = IsWindowVisible(handle)
        If wp.showCmd = SW_SHOWMAXIMIZED Then
            If handle = GetForegroundWindow Then
                Return WindowsVisible
            End If
        ElseIf wp.showCmd = SW_SHOWNORMAL Then
            If handle = GetForegroundWindow Then
                Return WindowsVisible
            Else
            End If
        ElseIf wp.showCmd = SW_SHOWMINIMIZED Then
            Return False
        End If
        Return False
    End Function

#End Region

#Region " No Windows Focus "

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property

    Private Const WS_EX_TOPMOST As Integer = &H8

    Private Const WS_THICKFRAME As Integer = &H40000
    Private Const WS_CHILD As Integer = &H40000000
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000
    Private Const WS_EX_TOOLWINDOW As Integer = &H80

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim createParamsA As CreateParams = MyBase.CreateParams
            createParamsA.ExStyle = createParamsA.ExStyle Or WS_EX_TOPMOST Or WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW
            Return createParamsA
        End Get
    End Property

#End Region

#Region " Declare "

    Private WinMauseC As New WinMauseHelpersCore
    Public Memo As Mem = New Mem()

    Public AmongMonitorHook As New cMonitor

    Public GameWindowsVisible As Boolean = False
    Private MyAppName As String = Path.GetFileNameWithoutExtension(Application.ExecutablePath)
    Private MyAppProcess As String = Process.GetCurrentProcess().ProcessName
    Private WindowsExternalMonitor As New WControlHooking(True)

#End Region

    Dim VisibleMenu As Boolean = False

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Dim procIdFromName As Integer = Memo.GetProcIdFromName(Path.GetFileNameWithoutExtension(ProcessGame))
        Dim flag As Boolean = (procIdFromName > 0)
        If flag Then
            Memo.OpenProcess(procIdFromName)
            CheatTimer.Enabled = True
        Else
            End
        End If
        Me.InitializeDrag()
        Me.Hide()
        GameInfoForm.Show()
        RadarInfo.Show()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
         Try : AddHandler Application.ThreadException, AddressOf Application_Exception_Handler _
                  : Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, False) _
                      : Catch : End Try

        PosicionateForm()
        StartControls()
    End Sub

    Private Sub Application_Exception_Handler(ByVal sender As Object, ByVal e As System.Threading.ThreadExceptionEventArgs)
        Dim ex As Exception = CType(e.Exception, Exception)
        WriteLog(ex.Message, InfoType.Exception)
    End Sub

#Region " Controls "

    Dim vScrollHelperLog As Guna.UI.Lib.ScrollBar.PanelScrollHelper
    Dim vScrollHelperMain As Guna.UI.Lib.ScrollBar.PanelScrollHelper

    Public Sub StartControls()
        Panel7.AutoScroll = False
        Panel3.AutoScroll = False
        GunaRadioButton1.BackColor = Color.FromArgb(25, Color.Black)
        GunaRadioButton2.BackColor = Color.FromArgb(25, Color.Black)
        GunaRadioButton3.BackColor = Color.FromArgb(25, Color.Black)
        GunaRadioButton4.BackColor = Color.FromArgb(25, Color.Black)
        GunaRadioButton5.BackColor = Color.FromArgb(25, Color.Black)
        GunaMetroTrackBar1.BackColor = Color.FromArgb(25, Color.Black)
        GunaMetroTrackBar2.BackColor = Color.FromArgb(25, Color.Black)
        GunaMetroTrackBar3.BackColor = Color.FromArgb(25, Color.Black)

        vScrollHelperMain = New Guna.UI.Lib.ScrollBar.PanelScrollHelper(Panel7, GunaVScrollBar1, False)
        vScrollHelperMain.UpdateScrollBar()

        vScrollHelperLog = New Guna.UI.Lib.ScrollBar.PanelScrollHelper(Panel3, GunaVScrollBar2, True)
        vScrollHelperLog.UpdateScrollBar()

        WriteLog("Welcome " & Environment.UserName & " , I love Cheat.", InfoType.None)
        WriteLog("Enjoy Among US MOD by Destroyer...", InfoType.None)
        Dim LangSaveint As Integer = My.Settings.LangSave
        If LangSaveint = 0 Then
            SetLangControls(Lang.English)
        ElseIf LangSaveint = 1 Then
            SetLangControls(Lang.Spanish)
        ElseIf LangSaveint = 2 Then
            SetLangControls(Lang.Russian)
        ElseIf LangSaveint = 3 Then
            SetLangControls(Lang.Portuguese)
        ElseIf LangSaveint = 4 Then
            SetLangControls(Lang.Chinese)
        End If
        CheckHacks()
    End Sub

    Private Sub Panel7_Resize(sender As Object, e As EventArgs) Handles Panel7.Resize
        If vScrollHelperMain IsNot Nothing Then vScrollHelperMain.UpdateScrollBar()
    End Sub

    Private Sub GunaTextBox1_TextChanged(sender As Object, e As EventArgs) Handles GunaTextBox1.TextChanged
        Dim Lines As Integer = GunaTextBox1.Text.ToList.Count
        GunaTextBox1.Size = New Size(GunaTextBox1.Width, (Lines * 30)) '(GunaTextBox1.Height + 30)
        Panel3.Size = New Size(Panel3.Width, GunaTextBox1.Height)
    End Sub

    Private Sub Panel3_Resize(sender As Object, e As EventArgs) Handles Panel3.Resize
        If vScrollHelperLog IsNot Nothing Then vScrollHelperLog.UpdateScrollBar()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If GetAsyncKeyState(Keys.Insert) = -32767 Then
            If GameWindowsVisible = True Then
                If VisibleMenu = False Then
                    ShowMenu(True)
                    Exit Sub
                End If
                If VisibleMenu = True Then
                    ShowMenu(False)
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Dim HWND As IntPtr = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(ProcessGame)).First.MainWindowHandle

    Private Sub ShowMenu(ByVal ShowOrHide As Boolean)
        Me.TopMost = ShowOrHide
        VisibleMenu = ShowOrHide
        If ShowOrHide = True Then
            Timer3.Enabled = True


            ' SetWindowState.SetWindowState(HWND, SetWindowState.WindowState.Normal)

            ' WinMauseC.ShowCursorGame(True)
            Me.Show()
        Else
            Timer3.Enabled = False

            ' SetWindowState.SetWindowState(HWND, SetWindowState.WindowState.Restore)
            ' WinMauseC.ShowCursorGame(False)

            Me.Hide()
        End If
    End Sub

    Private Sub PosicionateForm()
        Dim GamePos As Point = WinMauseC.GetClientPosition(WinMauseC.GetProcessHandle(ProcessGame))
        Me.Location = GamePos
    End Sub

    Private Sub GunaAdvenceButton1_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton1.Click
        End
    End Sub

#End Region

#Region " Cheats "

    Private Sub GunaAdvenceButton6_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton6.Click
        CheckHacks()
    End Sub

    Private Sub GunaAdvenceButton2_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton2.Click
        CheckHacks()
    End Sub

    Private Sub GunaAdvenceButton5_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton5.Click
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch2_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch2.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch3_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch3.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch4_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch4.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaMetroTrackBar2_Scroll(sender As Object, e As ScrollEventArgs) Handles GunaMetroTrackBar2.Scroll
        CheckHacks()
    End Sub

    Private Sub GunaMetroTrackBar3_Scroll(sender As Object, e As ScrollEventArgs) Handles GunaMetroTrackBar3.Scroll
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch5_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch5.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch6_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch6.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch7_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch7.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch9_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch9.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch11_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch11.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch12_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch12.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch13_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch13.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch10_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch10.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch14_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch14.CheckedChanged
        CheckHacks()
    End Sub

    Private Sub GunaGoogleSwitch15_CheckedChanged(sender As Object, e As EventArgs) Handles GunaGoogleSwitch15.CheckedChanged
        CheckHacks()
    End Sub


    Private Sub CheckHacks()

        If GunaGoogleSwitch2.Checked = True Then
            'Rambow Colors
            Timer2.Enabled = True
        Else
            Timer2.Enabled = False
        End If

        If GunaGoogleSwitch4.Checked = True Then
            'Speed Override
            Memo.WriteMemory("GameAssembly.dll+00DA3C30,14,14,5C", "float", Double.Parse(GunaMetroTrackBar2.Value), "", Nothing)
            '     Memo.WriteMemory("GameAssembly.dll+00E23FA8,5C,14,14", "float", GunaMetroTrackBar2.Value, "", Nothing)
            '    Memo.WriteMemory("GameAssembly.dll+00D9BD28,48,198,54,C,5C,7C,14", "float", GunaMetroTrackBar2.Value, "", Nothing)
        End If

        If GunaGoogleSwitch3.Checked = True Then
            'Time Coolkilldown
            Memo.WriteMemory("GameAssembly.dll+00D2978C,1A8,6C,64,48,5C,24,20", "float", GunaMetroTrackBar3.Value, "", Nothing)
            Memo.WriteMemory("GameAssembly.dll+00DA050C,44,0,5C", "float", GunaMetroTrackBar3.Value, "", Nothing)
        End If

        If GunaGoogleSwitch6.Checked = True Then
            ' No Clip
            Memo.WriteMemory("UnityPlayer.dll+960CA5", "bytes", "0F 85", "", Nothing)
        End If

        If GunaGoogleSwitch7.Checked = True Then
            'Anti-Flood Bypass
            Memo.WriteMemory("GameAssembly.dll+2B1E33", "bytes", "0xC7 0x46 0x50 0x00 0x00 0x40 0x40", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+2B1E33", "bytes", "0xC7 0x46 0x50 0x00 0x00 0x00 0x00", "", Nothing)
        End If

        If GunaGoogleSwitch9.Checked = True Then
            ' Ban Bypass
            Memo.WriteMemory("GameAssembly.dll+A38BF0", "bytes", "B8 00 00 00 00 C3", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+A38BF0", "bytes", "55 8B EC 6A 00 FF", "", Nothing)
        End If

        If GunaGoogleSwitch11.Checked = True Then
            'Visible Ghost
            Memo.WriteMemory("GameAssembly.dll+20E7BB", "bytes", "80 7F 29 05 0F 85", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+20E7BB", "bytes", "80 7F 29 00 0F 84", "", Nothing)
        End If

        If GunaGoogleSwitch12.Checked = True Then
            'Visible Ghost Chat
            Memo.WriteMemory("GameAssembly.dll+2B1311", "bytes", "80 7E 29 05 75", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+2B1311", "bytes", "80 7E 29 00 74", "", Nothing)
        End If

        If GunaGoogleSwitch13.Checked = True Then
            ' Kill Other Impostor
            Memo.WriteMemory("GameAssembly.dll+20E5AF", "bytes", "0F 82", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+20E5AF", "bytes", "0F 85", "", Nothing)
        End If

        If GunaGoogleSwitch10.Checked = True Then
            'Enable Task
            Memo.WriteMemory("GameAssembly.dll+245E2C", "bytes", "80 78 28 05 0F 84", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+245E2C", "bytes", "80 78 28 00 0F 85", "", Nothing)
        End If

        If GunaGoogleSwitch14.Checked = True Then
            'Sabotage Doors Bypass
            Memo.WriteMemory("GameAssembly.dll+2A2BE4", "bytes", "C7 40 38 00 00 00 00", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+2A2BE4", "bytes", "C7 40 38 00 00 F0 41", "", Nothing)
        End If

        If GunaGoogleSwitch15.Checked = True Then
            'Invisible
            If GunaAdvenceButton6.Checked = True Then
                'crewmate  Aviable
                Memo.WriteMemory("UnityPlayer.dll+012A7A14,64,24,8,3C,C,34,28", "int", "256", "", Nothing)
            End If
            If GunaAdvenceButton2.Checked = True Then
                'Impostor  Aviable
                Memo.WriteMemory("UnityPlayer.dll+012A7A14,64,24,8,3C,C,34,28", "int", "-1", "", Nothing)
            End If
        Else
            'Desactivation
        End If

    End Sub

    Private Sub CheatTimer_Tick(sender As Object, e As EventArgs) Handles CheatTimer.Tick
        On Error Resume Next
        If GunaAdvenceButton6.Checked Then 'mode crewmate
            'GameAssembly.dll+00DA050C
            Memo.WriteMemory("GameAssembly.dll+DA5A84,28,34,0,5C", "bytes", "0", "", Nothing)


            ' Memo.WriteMemory("GameAssembly.dll+DA5A84,28,34,0,5C", "bytes", "0", "", Nothing) 'v2020.9.9s
            ' Memo.WriteMemory("UnityPlayer.dll+012A7A14,64,24,8,3C,C,34,28", "int", "0", "", Nothing)

        End If

        If GunaAdvenceButton2.Checked = True Then 'mode impostor
            Memo.WriteMemory("GameAssembly.dll+DA5A84,28,34,0,5C", "bytes", "1", "", Nothing)

            '  Memo.WriteMemory("GameAssembly.dll+DA5A84,28,34,0,5C", "bytes", "1", "", Nothing) 'v2020.9.9s
            ' Memo.WriteMemory("UnityPlayer.dll+012A7A14,64,24,8,3C,C,34,28", "int", "1", "", Nothing)
            ' Memo.WriteMemory("GameAssembly.dll+E22924,24,218,C,5C,0,34,28", "int", "1", "", Nothing) 'v2020.8.12s
        End If

        If GunaAdvenceButton5.Checked Then 'mode ghost
            Memo.WriteMemory("GameAssembly.dll+DA5A84,28,34,0,5C", "int", "257", "", Nothing) 'v2020.9.9s
            ' Memo.WriteMemory("UnityPlayer.dll+012A7A14,64,24,8,3C,C,34,28", "int", "257", "", Nothing)
            ' Memo.WriteMemory("GameAssembly.dll+E22924,24,218,C,5C,0,34,28", "int", "257", "", Nothing) 'v2020.8.12s
        End If

        If GunaGoogleSwitch5.Checked = True Then
            'detect impostor
            ' Memo.WriteMemory("GameAssembly.dll+00E37A7C,44,8,20,5C,0,34,28", "int", "1", "", Nothing)
        End If


        ''''''''''''''''''''''''''''''

        If GunaGoogleSwitch5.Checked = True Then
            'detect impostor
            Memo.WriteMemory("GameAssembly.dll+00E37A7C,44,8,20,5C,0,34,28", "int", "1", "", Nothing)
            Memo.WriteMemory("GameAssembly.dll+E2F7D4,5C,20,34,28", "int", "1", "", Nothing)
        Else
            Memo.WriteMemory("GameAssembly.dll+00E37A7C,44,8,20,5C,0,34,28", "int", "0", "", Nothing)
            Memo.WriteMemory("GameAssembly.dll+E2F7D4,5C,20,34,28", "int", "0", "", Nothing)
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        'Rambow Color
        Timer2.Interval = GunaMetroTrackBar1.Value
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "1", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "2", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "3", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "4", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "5", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "6", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "7", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "8", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "9", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00DA04E0,20,5C,0,24,8,10,10", "int", "10", "", Nothing)
    End Sub

    '---------------------------------------
    'Skin Changer
    '---------------------------------------
    Private Sub GunaAdvenceButton3_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton3.Click
        Memo.WriteMemory("GameAssembly.dll+00D93254,28,A0,A0,A0,A10,DE4", "int", "7", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+00D3920C,5C,8,0,5C,10,210,F70", "int", "70", "", Nothing)
    End Sub

    Private Sub GunaAdvenceButton7_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton7.Click
        Memo.WriteMemory("GameAssembly.dll+00D93254,28,A0,A0,A0,A10,DE4", "int", "3", "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+0x0D3920C,5C,8,0,5C,10,210,F70", "int", "21", "", Nothing)
    End Sub

    Dim random As New Random
    Private Sub GunaAdvenceButton4_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton4.Click
        Dim Random1 As Integer = random.Next(1, 10)
        Dim Random2 As Integer = random.Next(10, 70)
        Memo.WriteMemory("GameAssembly.dll+00D93254,28,A0,A0,A0,A10,DE4", "int", Random1, "", Nothing)
        Memo.WriteMemory("GameAssembly.dll+0x0D3920C,5C,8,0,5C,10,210,F70", "int", Random2, "", Nothing)
    End Sub
    '---------------------------------------

#End Region

#Region " LanguageRegion "

    Public Enum Lang
        English
        Spanish
        Chinese
        Russian
        Portuguese
    End Enum

    Private Sub GunaRadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles GunaRadioButton1.CheckedChanged
        SetLangControls(Lang.English)
    End Sub

    Private Sub GunaRadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles GunaRadioButton2.CheckedChanged
        SetLangControls(Lang.Spanish)
    End Sub

    Private Sub GunaRadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles GunaRadioButton3.CheckedChanged
        SetLangControls(Lang.Russian)
    End Sub

    Private Sub GunaRadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles GunaRadioButton4.CheckedChanged
        SetLangControls(Lang.Portuguese)
    End Sub

    Private Sub GunaRadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles GunaRadioButton5.CheckedChanged
        SetLangControls(Lang.Chinese)
    End Sub

    Private Sub SetLangControls(Optional ByVal SelectLang As Lang = Lang.English)
        Dim RadarDes As String = String.Empty
        Dim PlayerModeDes As String = String.Empty
        Dim RambowColorsDes As String = String.Empty
        Dim SpeedOverrideDes As String = String.Empty
        Dim TimeCoolkilldownDes As String = String.Empty
        Dim ImpostorDetectorDes As String = String.Empty
        Dim Walk_Throught_Walls As String = String.Empty
        Dim SkinChangerDes As String = String.Empty
        Dim Anti_FloodBypassDes As String = String.Empty
        Dim BanBypassDes As String = String.Empty
        Dim VisibleGhostDES As String = String.Empty
        Dim VisibleGhostChatDES As String = String.Empty
        Dim KillOtherImpostorDES As String = String.Empty
        Dim EnableTaskDES As String = String.Empty
        Dim SabotageDoorsBypassDes As String = String.Empty
        Dim InvisiblePlayerDes As String = String.Empty

        Select Case SelectLang
            Case Lang.English
                My.Settings.LangSave = 0
                RadarDes = "When activated, it shows a Radar that detects the Position of your Enemy."
                PlayerModeDes = "Change your player's Mode, between Crewmate / Imposter / Ghost." &
                    "To be a Deceiver, you must be the Host of the game." & _
"Why else, He does not let you be a True -Imposter-." & _
"(If you kill, only you can see it)."
                RambowColorsDes = "Change your skin color in loop, you can adjust the speed."
                SpeedOverrideDes = "You can run as fast as Flash, with the option to adjust the speed."
                TimeCoolkilldownDes = "Adjust the wait time to assassinate your next victim in Imposter mode."
                ImpostorDetectorDes = "Detect who the Imposter is, His name will appear in Red."
                Walk_Throught_Walls = "Allows you to walk / go through walls."
                SkinChangerDes = "Change your Skin to one that is by default." & vbNewLine & _
"Note:" & vbNewLine & _
"- You must first select the Laptop that is in the Lobby." & vbNewLine & _
"- When the dialog box that allows you to change your appearance appears, select a default skin from the Cheat."

                Anti_FloodBypassDes = "Remove the protection of the 3s to rewrite in the chat"
                BanBypassDes = "Allows you to re-enter the rooms where you were banned"
                VisibleGhostDES = "Allows you to see ghosts"
                VisibleGhostChatDES = "Allows you to view the ghost chat"
                KillOtherImpostorDES = "You can kill other Impostors"
                EnableTaskDES = "Activate the Tasks even if you are an Imposter or a Ghost"
                SabotageDoorsBypassDes = "Allows you to Sabotage Locked Doors"
                InvisiblePlayerDes = "Makes you Invisible to other players"

                Exit Select

            Case Lang.Spanish
                My.Settings.LangSave = 1
                RadarDes = "Cuando se activa, muestra un radar que detecta la posición de tu enemigo"
                PlayerModeDes = "Cambia el modo de tu jugador, entre Crewmate / Imposter / Ghost" & _
                     "Para ser un Impostor, debes ser el Anfitrión del juego" & _
"Por qué si no, Él no te deja ser un Verdadero -Impostor-" & _
"(Si matas, solo tú puedes verlo a no ser que seas el creador de la partida)"
                RambowColorsDes = "Cambia el color de tu piel en bucle, puedes ajustar la velocidad"
                SpeedOverrideDes = "Puedes correr tan rápido como Flash, con la opción de ajustar la velocidad"
                TimeCoolkilldownDes = "Ajusta el tiempo de espera para asesinar a tu próxima víctima en el modo Imposter"
                ImpostorDetectorDes = "Detecta quién es el impostor, su nombre aparecerá en rojo"
                Walk_Throught_Walls = "Le permite caminar / atravesar paredes"
                SkinChangerDes = "Cambie su máscara a una que sea la predeterminada" & vbNewLine & _
"Nota:" & vbNewLine & _
"- Primero debe seleccionar la computadora portátil que está en el vestíbulo" & vbNewLine & _
"- Cuando aparezca el cuadro de diálogo que te permite cambiar tu apariencia, selecciona una máscara predeterminada del Truco"

                Anti_FloodBypassDes = "Remueve La proteccion de los 3s para volver a escrivir en el chat"
                BanBypassDes = "Te permite volver a entrar a las salas donde fuiste baneado"
                VisibleGhostDES = "Te permite ver fantasmas"
                VisibleGhostChatDES = "Te permite Ver el Chat de los Fantasmas"
                KillOtherImpostorDES = "Puedes matar a otros Impostores"
                EnableTaskDES = "Activa las Tareas aunque seas Impostor o Fantasma"
                SabotageDoorsBypassDes = "Te permite Sabotear las Puertas Bloqueadas"
                InvisiblePlayerDes = "Te hace Invisible a otros jugadores"

                Exit Select

            Case Lang.Russian
                My.Settings.LangSave = 2
                RadarDes = "Pri aktivatsii pokazyvayet radar, kotoryy opredelyayet pozitsiyu vashego vraga."
                PlayerModeDes = "Измените режим вашего игрока между Crewmate / Imposter / Ghost." & _
                       "Чтобы быть Обманщиком, вы должны быть Хозяином игры" & _
  "Почему еще, Он не позволяет тебе быть Истинным Самозванцем-" & _
  "(Если вы убьете, это увидите только вы)"
                RambowColorsDes = "Tsiklicheski menyayte tsvet kozhi, vy mozhete regulirovat' skorost'."
                SpeedOverrideDes = "Vy mozhete begat' tak zhe bystro, kak Flash, s vozmozhnost'yu regulirovki skorosti."
                TimeCoolkilldownDes = "Nastroyte vremya ozhidaniya dlya ubiystva vashey sleduyushchey zhertvy v rezhime Samozvantsa."
                ImpostorDetectorDes = "Opredelite, kto samozvanets, yego imya budet krasnym."
                Walk_Throught_Walls = "Pozvolyayet vam prokhodit' / prokhodit' skvoz' steny."
                SkinChangerDes = "Izmenite skin na tot, kotoryy ustanovlen po umolchaniyu." & vbNewLine & _
"Primechaniye:" & vbNewLine & _
"- Snachala vy dolzhny vybrat' noutbuk, kotoryy nakhoditsya v kholle" & vbNewLine & _
"- Kogda poyavitsya dialogovoye okno, pozvolyayushcheye izmenit' vneshniy vid, vyberite skin po umolchaniyu iz Cheat"

                Anti_FloodBypassDes = "Снять защиту 3с для перезаписи в чате"
                BanBypassDes = "Позволяет повторно войти в комнаты, где вас забанили"
                VisibleGhostDES = "Позволяет видеть призраков"
                VisibleGhostChatDES = "Позволяет просматривать чат с привидениями"
                KillOtherImpostorDES = "Вы можете убивать других самозванцев"
                EnableTaskDES = "Активируйте задачи, даже если вы самозванец или призрак"
                SabotageDoorsBypassDes = "Позволяет саботировать запертые двери"
                InvisiblePlayerDes = "Делает вас невидимым для других игроков"

                Exit Select

            Case Lang.Portuguese
                My.Settings.LangSave = 3
                RadarDes = "Quando ativado, mostra um radar que detecta a posição do seu inimigo."
                PlayerModeDes = "Mude o modo do seu jogador, entre Crewmate / Imposter / Ghost." & _
                     "Para ser um Enganador, você deve ser o Host do jogo." & _
"Por que mais, Ele não permite que você seja um verdadeiro -Impostador-." & _
"(Se você matar, só você pode ver)."
                RambowColorsDes = "Altere a cor da pele em loop, você pode ajustar a velocidade."
                SpeedOverrideDes = "Você pode correr tão rápido quanto o Flash, com a opção de ajustar a velocidade."
                TimeCoolkilldownDes = "Ajuste o tempo de espera para assassinar sua próxima vítima no modo Impostor."
                ImpostorDetectorDes = "Detecte quem é o Impostor, o nome dele aparecerá em vermelho."
                Walk_Throught_Walls = "Permite que você caminhe / atravesse paredes."
                SkinChangerDes = "Mude o seu tema para um padrão." & vbNewLine & _
"Nota:" & vbNewLine & _
"- Você deve primeiro selecionar o Laptop que está no Lobby." & vbNewLine & _
"- Quando a caixa de diálogo que permite a você mudar sua aparência aparecer, selecione uma skin padrão do Cheat."

                Anti_FloodBypassDes = "Remova a proteção dos 3s para reescrever no chat"
                BanBypassDes = "Permite que você entre novamente nas salas onde foi banido"
                VisibleGhostDES = "Permite que você veja fantasmas"
                VisibleGhostChatDES = "Permite que você veja o chat fantasma"
                KillOtherImpostorDES = "Você pode matar outros Impostores"
                EnableTaskDES = "Ative as Tarefas mesmo que você seja um Impostor ou Fantasma"
                SabotageDoorsBypassDes = "Permite que você sabote portas trancadas"
                InvisiblePlayerDes = "Torna você invisível para outros jogadores"

                Exit Select

            Case Lang.Chinese
                My.Settings.LangSave = 4
                RadarDes = "激活后，它会显示可检测敌人位置的雷达。"

                PlayerModeDes = "在Crewmate / Imposter / Ghost之间更改玩家的模式。 和" & _
                     "要成为欺骗者，您必须是游戏的主持人。" & _
"为什么，他不让你成为一个真正的-Imposter-。" & _
"（如果杀死，只有你能看到它）。"
                RambowColorsDes = "循环更改皮肤颜色，可以调整速度。"
                SpeedOverrideDes = "您可以像调整Flash一样快地运行，并可以选择调整速度。"
                TimeCoolkilldownDes = "调整等待时间，以在冒名顶替者模式下暗杀您的下一个受害者。"
                ImpostorDetectorDes = "检测冒名顶替者，他的名字将显示为红色。"
                Walk_Throught_Walls = "允许您步行/穿过墙壁。"
                SkinChangerDes = "将您的皮肤更改为默认的皮肤。" & vbNewLine & _
"注意：" & vbNewLine & _
"-您必须首先选择大厅中的笔记本电脑。" & vbNewLine & _
"-当出现允许您更改外观的对话框时，请从 作弊 中选择默认外观。"

                Anti_FloodBypassDes = "取消对3s的保护以在聊天中重写"
                BanBypassDes = "允许您重新进入被禁止的房间"
                VisibleGhostDES = "允许您看到鬼影"
                VisibleGhostChatDES = "允许您查看幻像聊天"
                KillOtherImpostorDES = "您可以杀死其他冒名顶替者"
                EnableTaskDES = "即使是冒名顶替者也可以激活任务。"
                SabotageDoorsBypassDes = "允许您破坏锁门"
                InvisiblePlayerDes = "使您对其他玩家不可见"

                Exit Select
        End Select

        My.Settings.Save()

        BoosterToolTip1.SetToolTip(GunaLabel20, RadarDes)
        BoosterToolTip1.SetToolTip(GunaLabel21, PlayerModeDes)
        BoosterToolTip1.SetToolTip(GunaLabel14, RambowColorsDes)
        BoosterToolTip1.SetToolTip(GunaLabel15, SpeedOverrideDes)
        BoosterToolTip1.SetToolTip(GunaLabel16, TimeCoolkilldownDes)
        BoosterToolTip1.SetToolTip(GunaLabel17, ImpostorDetectorDes)
        BoosterToolTip1.SetToolTip(GunaLabel19, Walk_Throught_Walls)
        BoosterToolTip1.SetToolTip(GunaLabel7, SkinChangerDes)

        BoosterToolTip1.SetToolTip(GunaLabel23, Anti_FloodBypassDes)
        BoosterToolTip1.SetToolTip(GunaLabel27, BanBypassDes)
        BoosterToolTip1.SetToolTip(GunaLabel31, VisibleGhostDES)
        BoosterToolTip1.SetToolTip(GunaLabel33, VisibleGhostChatDES)
        BoosterToolTip1.SetToolTip(GunaLabel35, KillOtherImpostorDES)
        BoosterToolTip1.SetToolTip(GunaLabel29, EnableTaskDES)
        BoosterToolTip1.SetToolTip(GunaLabel37, SabotageDoorsBypassDes)
        BoosterToolTip1.SetToolTip(GunaLabel39, InvisiblePlayerDes)

    End Sub

#End Region

#Region " Attach to Client "

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        On Error Resume Next

        If VisibleMenu = True Then
            Me.TopMost = True
            Call load_()
            Dim proc As Process() = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessGame))
            Dim MyHeight = Me.Height
            Dim windowname As String = proc(0).MainWindowTitle
            hWnd = FindWindow(vbNullString, windowname)
            GetWindowThreadProcessId(hWnd, processID)
            pHandle = OpenProcess(PROCESS_VM_ALL, 0, processID)
            GetWindowRect(Overlay.hWnd, window_loc)
            Me.Location = New Point((window_loc.Left + 10), (window_loc.Top + 35))
            Me.Size = New Point(((window_loc.Right - window_loc.Left) - 25), ((window_loc.Bottom - window_loc.Top) - 45))
        End If
    End Sub

#End Region

#Region " Dragger - Minimize "

    Private Dragger As ControlDragger = ControlDragger.Empty

    Private Sub InitializeDrag()
        '----------------------------------------------------
        'Panel1 Moving | Main Cheats
        '----------------------------------------------------
        Me.Dragger = New ControlDragger(Panel7, Panel1)
        Me.Dragger = New ControlDragger(GunaLabel1, Panel1)
        Me.Dragger = New ControlDragger(GunaPanel1, Panel1)
        Me.Dragger = New ControlDragger(GunaSeparator1, Panel1)
        Me.Dragger = New ControlDragger(GunaSeparator2, Panel1)
        Me.Dragger = New ControlDragger(GunaSeparator3, Panel1)
        Me.Dragger = New ControlDragger(GunaSeparator4, Panel1)
        Me.Dragger = New ControlDragger(GunaSeparator5, Panel1)
        Me.Dragger = New ControlDragger(GunaSeparator6, Panel1)
        Me.Dragger = New ControlDragger(GunaSeparator7, Panel1)

        '----------------------------------------------------
        'PanelFX1 Moving | Log
        '----------------------------------------------------
        Me.Dragger = New ControlDragger(GunaLabel44, PanelFX1)
        Me.Dragger = New ControlDragger(GunaTextBox1, PanelFX1)
        Me.Dragger = New ControlDragger(Panel3, PanelFX1)
        Me.Dragger = New ControlDragger(GunaPanel6, PanelFX1)

        '----------------------------------------------------
        'PanelFX2 Moving | Misc
        '----------------------------------------------------
        Me.Dragger = New ControlDragger(GunaLabel43, PanelFX2)
        Me.Dragger = New ControlDragger(GunaPanel4, PanelFX2)
        Me.Dragger = New ControlDragger(GunaPanel3, PanelFX2)
        Me.Dragger = New ControlDragger(GunaGroupBox1, PanelFX2)
        Me.Dragger = New ControlDragger(Panel2, PanelFX2)

        '----------------------------------------------------
        'PanelFX3 Moving | GameInfo
        '----------------------------------------------------
        Me.Dragger = New ControlDragger(GunaLabel43, PanelFX2)

        Me.Dragger.Enabled = True
    End Sub

    Private Sub GunaPanel1_DoubleClick(sender As Object, e As EventArgs) Handles GunaPanel1.DoubleClick, _
       GunaLabel1.DoubleClick
        If Panel1.Height = 19 Then
            Panel1.BringToFront()
            Panel1.Height = 585
        Else
            Panel1.Height = 19
        End If
    End Sub

    Private Sub GunaPanel6_DoubleClick(sender As Object, e As EventArgs) Handles GunaPanel6.DoubleClick, _
      GunaLabel44.DoubleClick
        If PanelFX1.Height = 19 Then
            PanelFX1.BringToFront()
            PanelFX1.Height = 196
        Else
            PanelFX1.Height = 19
        End If
    End Sub

    Private Sub GunaPanel4_DoubleClick(sender As Object, e As EventArgs) Handles GunaPanel4.DoubleClick, _
     GunaLabel43.DoubleClick
        If PanelFX2.Height = 19 Then
            PanelFX2.BringToFront()
            PanelFX2.Height = 210
        Else
            PanelFX2.Height = 19
        End If
    End Sub

#End Region

#Region " End Cheat "

    Private Sub EndMonitor_Tick(sender As Object, e As EventArgs) Handles EndMonitor.Tick
        Dim p As Process()
        p = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessGame))
        If p.Count = 0 Then
            End
        End If

        Dim ProcName_Value As String = WindowsExternalMonitor.ProcName

        Dim CheatVisible As Boolean = GetWindowStats(Me.Handle)
        Dim GameVisible As Boolean = GetWindowStats(Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessGame))(0).MainWindowHandle)
        Dim CurrentProgram As Boolean = False

        If ProcName_Value = MyAppProcess Then
            CurrentProgram = True
        End If

        If LCase(ProcName_Value) = LCase(Path.GetFileNameWithoutExtension(ProcessGame)) Then
            CurrentProgram = True
        End If

        If CurrentProgram = True Then
            If GameVisible = False Then
                If CheatVisible = True Then
                    GameWindowsVisible = True
                Else
                    GameWindowsVisible = False
                End If
            Else
                If CheatVisible = False Then
                    GameWindowsVisible = True
                Else
                    ShowMenu(False)
                    GameWindowsVisible = True
                End If
            End If
        Else
            ShowMenu(False)
        End If

    End Sub

#End Region

    Private Sub GunaAdvenceButton8_Click(sender As Object, e As EventArgs) Handles GunaAdvenceButton8.Click
        Dim NameT As String = NameTextbox.Text

        Memo.WriteMemory("GameAssembly.dll+0DA5A84,C,C,34,0,5C", "string", NameT, "", Nothing)

    End Sub

End Class



