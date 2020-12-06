Imports AmongUS.MOD.Core.AmongCustomFuncs

Public Class RadarPlayerInfoControl

#Region " Properties "

    Public WriteOnly Property ColorID As String
        Set(value As String)
            Panel1.BackColor = GetColorFromID(Integer.Parse(value))
        End Set
    End Property

#End Region

   
End Class
