Public Class LogFuncs

#Region " Ignore Exceptions "

    Public Shared LogFile = Application.StartupPath & "\" & System.Reflection.Assembly.GetExecutingAssembly.GetName().Name & ".log"

    Public Enum InfoType
        Information
        Exception
        Critical
        None
    End Enum

  
    Public Shared Function WriteLog(ByVal Message As String, Optional ByVal InfoType As InfoType = LogFuncs.InfoType.None) As Boolean
        If VerifyLog(Message) = False Then
            Dim LocalDate As String = My.Computer.Clock.LocalTime.ToString.Split(" ").First
            Dim LocalTime As String = My.Computer.Clock.LocalTime.ToString.Split(" ").Last
            Dim LogDate As String = " [ " & LocalTime & " ]  "
            Dim MessageType As String = Nothing

            Select Case InfoType
                Case InfoType.Information : MessageType = "Information: "
                Case InfoType.Exception : MessageType = "Error: "
                Case InfoType.Critical : MessageType = "Critical: "
                Case InfoType.None : MessageType = ""
            End Select
            Try

                'My.Computer.FileSystem.WriteAllText(LogFile, vbNewLine & LogDate & MessageType & Message & vbNewLine, True)
                ' Dim LineNumber As Integer = Form1.GunaTextBox1.Text.ToArray.Count
                ' NewItem.ItemID = ItemID
                Form1.GunaTextBox1.Text += Message & vbNewLine
                ' Form1.GunaVScrollBar2.Value = Form1.GunaVScrollBar2.Maximum

                Return True
            Catch ex As Exception
                'Return False
                Throw New Exception(ex.Message)
            End Try
        End If
    End Function

    Public Shared Sub ClearLog()
        Form1.GunaTextBox1.Text = ""
    End Sub

    Private Shared Function VerifyLog(ByVal NewString As String) As Boolean
        Dim UltimateTextItem As String = String.Empty


        For Each LineText In Form1.GunaTextBox1.Text.ToList
            UltimateTextItem = LineText
        Next
        If UltimateTextItem = NewString Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

End Class