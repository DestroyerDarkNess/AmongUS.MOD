Imports HamsterCheese.AmongUsMemory
Imports System.Threading
Imports AmongUS.MOD.Core.Values

Public Class Form2

    Dim HWND As IntPtr = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(ProcessGame)).First.MainWindowHandle

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SetWindowState.SetWindowState(HWND, SetWindowState.WindowState.Normal)
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If Timer1.Enabled = False Then
            Timer1.Enabled = True
            Button2.Text = "ON"
        Else
            Timer1.Enabled = False
            Button2.Text = "OFF"
        End If
    End Sub

    Dim Two As Integer = 0

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Two += 1
        SetWindowState.SetWindowState(HWND, SetWindowState.WindowState.Maximize)
        SetWindowStyle.SetWindowStyle(HWND, SetWindowStyle.WindowStyles.WS_BORDER)
        If Two = 5 Then
            Timer1.Enabled = False
            Button2.Text = "OFF"
            End
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SetWindowState.SetWindowState(HWND, SetWindowState.WindowState.Maximize)
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


End Class