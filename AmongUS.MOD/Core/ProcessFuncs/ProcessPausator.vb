Public Class ProcessPausator

#Region " Pinvoke "

    Public Declare Function OpenThread Lib "kernel32.dll" (ByVal dwDesiredAccess As ThreadAccess, ByVal bInheritHandle As Boolean, ByVal dwThreadId As UInteger) As IntPtr
    Public Declare Function SuspendThread Lib "kernel32.dll" (ByVal hThread As IntPtr) As UInteger
    Public Declare Function ResumeThread Lib "kernel32.dll" (ByVal hThread As IntPtr) As UInteger
    Public Declare Function CloseHandle Lib "kernel32.dll" (ByVal hHandle As IntPtr) As Boolean

#End Region

#Region " Enum "

    Public Enum ThreadAccess As Integer
        TERMINATE = (&H1)
        SUSPEND_RESUME = (&H2)
        GET_CONTEXT = (&H8)
        SET_CONTEXT = (&H10)
        SET_INFORMATION = (&H20)
        QUERY_INFORMATION = (&H40)
        SET_THREAD_TOKEN = (&H80)
        IMPERSONATE = (&H100)
        DIRECT_IMPERSONATION = (&H200)
    End Enum

#End Region

    Public Shared Function PauseThread(ByRef Process_Name As String) As Boolean

        If Process_Name.ToLower.EndsWith(".exe") Then Process_Name = Process_Name.Substring(0, Process_Name.Length - 4)

        Dim proc() As Process = Process.GetProcessesByName(Process_Name)

        If Not proc.Length = 0 Then
            SuspendProcess(proc(0))
            Return True
        End If
        Return False
    End Function

    Public Shared Function ResumeThread(ByRef Process_Name As String) As Boolean

        If Process_Name.ToLower.EndsWith(".exe") Then Process_Name = Process_Name.Substring(0, Process_Name.Length - 4)

        Dim proc() As Process = Process.GetProcessesByName(Process_Name)

        If Not proc.Length = 0 Then
            ResumeProcess(proc(0))
            Return True
        End If
        Return False
    End Function

    Private Shared Sub SuspendProcess(ByVal process As System.Diagnostics.Process)
        For Each t As ProcessThread In process.Threads
            Dim th As IntPtr = OpenThread(ThreadAccess.SUSPEND_RESUME, False, t.Id)
            If Not th = IntPtr.Zero Then
                SuspendThread(th)
                CloseHandle(th)
            End If
        Next
    End Sub

    Private Shared Sub ResumeProcess(ByVal process As System.Diagnostics.Process)
        For Each t As ProcessThread In process.Threads
            Dim th As IntPtr = OpenThread(ThreadAccess.SUSPEND_RESUME, False, t.Id)
            If Not th = IntPtr.Zero Then
                ResumeThread(th)
                CloseHandle(th)
            End If
        Next
    End Sub

End Class
