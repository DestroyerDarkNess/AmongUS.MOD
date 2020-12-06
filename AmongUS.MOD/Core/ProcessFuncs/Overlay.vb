Imports System.Runtime.InteropServices
Imports System.Net.Mail
Imports System.Text
Imports AmongUS.MOD.Core.Values

Public Module Overlay

    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Public window_loc As RECT
    Public screencenter(1) As Integer

    <DllImport("user32.dll")> _
    Public Function GetWindowRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImportAttribute("kernel32.dll", EntryPoint:="ReadProcessMemory", SetLastError:=True)> _
    Private Function ReadProcessMemory(<InAttribute()> ByVal hProcess As System.IntPtr, <InAttribute()> ByVal lpBaseAddress As System.IntPtr, <Out()> ByVal lpBuffer As Byte(), ByVal nSize As UInteger, <OutAttribute()> ByRef lpNumberOfBytesRead As UInteger) As <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function

    Public Declare Function ReadProcessMemory2 Lib "kernel32" Alias "ReadProcessMemory" (ByVal hProcess As Integer, ByVal lpBaseAddress As Integer, ByRef lpBuffer As Long, ByVal nSize As Integer, ByRef lpNumberOfBytesWritten As Integer) As Integer
    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal Classname As String, ByVal WindowName As String) As IntPtr
    Public Declare Function GetWindowThreadProcessId Lib "user32" (ByVal hWnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
    Public Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Integer, ByVal bInheritHandle As Integer, ByVal dwProcessId As Integer) As IntPtr
    Public Declare Function CloseHandle Lib "kernel32" (ByVal hObject As IntPtr) As Integer

    Public Const PROCESS_VM_ALL As Integer = &H1F0FFF
    Public hWnd As IntPtr, pHandle As IntPtr, processID As Integer


    Function load_()
        Dim proc As Process() = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(ProcessGame))
        Dim windowname As String
        windowname = proc(0).MainWindowTitle
        hWnd = FindWindow(vbNullString, windowname)
        GetWindowThreadProcessId(hWnd, processID)
        pHandle = OpenProcess(PROCESS_VM_ALL, 0, processID)
        If hWnd = 0 Then Return 0 Else Return 1
    End Function

    Sub is_active(Optional ByVal exit_ As Boolean = True)
        Dim p As Process() = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(ProcessGame))
        If p.Length = 0 And exit_ Then
            Environment.Exit(0)
        End If
    End Sub

    Function readInt(ByVal address As Integer, ByVal bytes As Integer)
        Dim read_value As Long = 0
        ReadProcessMemory2(pHandle, address, read_value, bytes, 0)
        Return read_value
    End Function

    Function readFloat(ByVal address As Integer, ByVal bytes As Integer)
        Dim read_value(bytes) As Byte
        ReadProcessMemory(pHandle, address, read_value, bytes, 0)
        Return BitConverter.ToSingle(read_value, 0)
    End Function

    Function readString(ByVal address As Integer, ByVal bytes As Integer, ByVal terminator As Byte)
        Dim read_value As String = ""
        Dim buffer As Byte
        For i = 0 To bytes
            ReadProcessMemory2(pHandle, address + i, buffer, 1, 0)
            If Not buffer = terminator Then
                read_value &= Chr(buffer)
            Else
                Exit For
            End If
        Next
        Return read_value
    End Function

End Module
