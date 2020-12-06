Imports AmongUS.MOD.Core.AmongCustomFuncs
Public Class PlayerInfoControl

#Region " Properties "

    Public Property PlayerType As String
        Get
            Return GunaLabel1.Text
        End Get
        Set(value As String)
            GunaLabel1.Text = value
        End Set
    End Property

    Public Property PlayerName As String
        Get
            Return GunaLabel18.Text
        End Get
        Set(value As String)
            GunaLabel18.Text = value
        End Set
    End Property

    Public Property Offset As String
        Get
            'Return GunaLabel2.Text
        End Get
        Set(value As String)
            ' GunaLabel2.Text = value
        End Set
    End Property

    Public WriteOnly Property ColorID As String
        Set(value As String)
            Panel2.BackColor = GetColorFromID(Integer.Parse(value))
        End Set
    End Property

#End Region

    Private Sub PlayerInfoControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        If LCase(PlayerType) = LCase("impostor") Then
            GunaLabel18.ForeColor = Color.Red
        End If
    End Sub

   
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
