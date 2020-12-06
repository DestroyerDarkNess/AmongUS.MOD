Public Class FormNoFocus

    Private Const SW_SHOWNOACTIVATE As Integer = 4
    Private Const HWND_TOPMOST As Integer = -1
    Private Const SWP_NOACTIVATE As UInteger = &H10

    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowPos")>
    Private Shared Function SetWindowPos(ByVal hWnd As Integer, ByVal hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As UInteger) As Boolean
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function ShowWindow(ByVal hWnd As System.IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    Public Shared Sub ShowInactiveTopmost(ByVal frm As System.Windows.Forms.Form)
        Try
            ShowWindow(frm.Handle, SW_SHOWNOACTIVATE)
            SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, frm.Left, frm.Top, frm.Width, frm.Height, SWP_NOACTIVATE)
        Catch ex As System.Exception
        End Try
    End Sub

End Class
