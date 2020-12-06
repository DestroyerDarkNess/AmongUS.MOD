Imports System.Text
Imports System.Drawing
Imports System.Windows.Forms

Public Class WControlHooking

#Region " Properties "

    Private ignoreChildWindowsFlag As Boolean = False
    Public Property IgnoreChildWindowsFlagA As Boolean
        Get
            Return ignoreChildWindowsFlag
        End Get
        Set(value As Boolean)
            ignoreChildWindowsFlag = value
        End Set
    End Property

    Private _RefreshInterval As Integer = 100
    Public Property RefreshInterval As Integer
        Get
            Return _RefreshInterval
        End Get
        Set(value As Integer)
            _RefreshInterval = value
        End Set
    End Property

    '//////////////////////////////////////////////////////////////////

    Private _ProcName_Value As String = String.Empty
    Public ReadOnly Property ProcName As String
        Get
            Return _ProcName_Value
        End Get
    End Property

    Private _PID_Value As String = String.Empty
    Public ReadOnly Property ProcID As String
        Get
            Return _PID_Value
        End Get
    End Property

    Private _Hwnd_Value As String = String.Empty
    Public ReadOnly Property ProcHwnd As String
        Get
            Return _Hwnd_Value
        End Get
    End Property

    Private _Caption_Value As String = String.Empty
    Public ReadOnly Property ProcCaption As String
        Get
            Return _Caption_Value
        End Get
    End Property

    Private _CoordsRelative_Value As String = String.Empty
    Public ReadOnly Property CoordsRelative As String
        Get
            Return _CoordsRelative_Value
        End Get
    End Property

    Private _CoordsScreen_Value As String = String.Empty
    Public ReadOnly Property CoordsScreen As String
        Get
            Return _CoordsScreen_Value
        End Get
    End Property

#End Region

    Friend WithEvents RefreshTimer As New System.Windows.Forms.Timer

    Public Sub New(Optional ByVal AutoStart As Boolean = False)
        If AutoStart = True Then
            StartMonitor()
        End If
    End Sub

    Public Sub StartMonitor()
        With Me.RefreshTimer
            .Enabled = True
            .Start()
        End With
    End Sub

    Public Sub StopMonitor()
        With Me.RefreshTimer
            .Stop()
            .Enabled = False
            .Dispose()
        End With
    End Sub

    Private Sub WindowTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) _
    Handles RefreshTimer.Tick

        Dim hwnd As IntPtr = NativeMethods.WindowFromPoint(System.Windows.Forms.Control.MousePosition)

        If (hwnd <> IntPtr.Zero) Then

            If (Me.ignoreChildWindowsFlag) Then

                Dim parentHwnd As IntPtr

                Do While True

                    parentHwnd = NativeMethods.GetParent(hwnd)

                    If (parentHwnd <> IntPtr.Zero) Then
                        hwnd = parentHwnd

                    Else
                        Exit Do

                    End If

                Loop

            End If

            Me.ShowInfo(hwnd)

        End If

    End Sub

    Private Sub ShowInfo(ByVal hwnd As IntPtr)

        Dim pid As Integer
        NativeMethods.GetWindowThreadProcessId(hwnd, pid)

        Dim proc As Process = Process.GetProcessById(pid)

        Dim sb As New StringBuilder(256)
        NativeMethods.GetWindowText(hwnd, sb, sb.Capacity)

        Dim rc As NativeMethods.Rect
        NativeMethods.GetWindowRect(hwnd, rc)

        Dim pt As New Point(Control.MousePosition.X - rc.Left, Control.MousePosition.Y - rc.Top)

        _ProcName_Value = proc.ProcessName
        _PID_Value = pid.ToString
        _Hwnd_Value = hwnd.ToString
        _Caption_Value = sb.ToString
        _CoordsRelative_Value = String.Format("X={0}, Y={1}", pt.X, pt.Y)
        _CoordsScreen_Value = String.Format("X={0}, Y={1}", Control.MousePosition.X, Control.MousePosition.Y)

    End Sub


End Class
