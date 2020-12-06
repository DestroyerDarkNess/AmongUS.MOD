Imports System.IO
Imports AmongUS.MOD.Core.Values

Public Class Loader

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim p As Process()
        p = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(ProcessGame))
        If p.Count > 0 Then
            GunaLabel1.Text = "Status : " & vbNewLine & vbNewLine & "Counter Strike 1.6 Detected, starting hack . . ."
            Timer1.Stop()
            Form1.Show()
            Me.Hide()
            Form1.Hide()
            Timer1.Enabled = False
        Else
            GunaLabel1.Text = "Waiting for the Game.   |"
            GunaLabel1.Update()
            GunaLabel1.Text = "Waiting for the Game..  /"
            GunaLabel1.Update()
            GunaLabel1.Text = "Waiting for the Game...   -"
            GunaLabel1.Update()
            GunaLabel1.Text = "Waiting for the Game.... \"
            GunaLabel1.Update()
        End If
    End Sub

 
End Class