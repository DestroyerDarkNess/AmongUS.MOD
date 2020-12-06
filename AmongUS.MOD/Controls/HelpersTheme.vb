Friend Module HelpersXylos

    Enum RoundingStyle As Byte
        All = 0
        Top = 1
        Bottom = 2
        Left = 3
        Right = 4
        TopRight = 5
        BottomRight = 6
    End Enum

    Public Sub CenterString(G As Graphics, T As String, F As Font, C As Color, R As Rectangle)
        Dim TS As SizeF = G.MeasureString(T, F)

        Using B As New SolidBrush(C)
            G.DrawString(T, F, B, New Point(R.Width / 2 - (TS.Width / 2), R.Height / 2 - (TS.Height / 2)))
        End Using
    End Sub

    Public Function ColorFromHex(Hex As String) As Color
        Return Color.FromArgb(Long.Parse(String.Format("FFFFFFFFFF{0}", Hex.Substring(1)), Globalization.NumberStyles.HexNumber))
    End Function

    Public Function FullRectangle(S As Size, Subtract As Boolean) As Rectangle

        If Subtract Then
            Return New Rectangle(0, 0, S.Width - 1, S.Height - 1)
        Else
            Return New Rectangle(0, 0, S.Width, S.Height)
        End If

    End Function

    Public Function RoundRect1(ByVal Rect As Rectangle, ByVal Rounding As Integer, Optional ByVal Style As RoundingStyle = RoundingStyle.All) As Drawing2D.GraphicsPath

        Dim GP As New Drawing2D.GraphicsPath()
        Dim AW As Integer = Rounding * 2

        GP.StartFigure()

        If Rounding = 0 Then
            GP.AddRectangle(Rect)
            GP.CloseAllFigures()
            Return GP
        End If

        Select Case Style
            Case RoundingStyle.All
                GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
            Case RoundingStyle.Top
                GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
            Case RoundingStyle.Bottom
                GP.AddLine(New Point(Rect.X, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
            Case RoundingStyle.Left
                GP.AddArc(New Rectangle(Rect.X, Rect.Y, AW, AW), -180, 90)
                GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height))
                GP.AddArc(New Rectangle(Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 90, 90)
            Case RoundingStyle.Right
                GP.AddLine(New Point(Rect.X, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
            Case RoundingStyle.TopRight
                GP.AddLine(New Point(Rect.X, Rect.Y + 1), New Point(Rect.X, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Y, AW, AW), -90, 90)
                GP.AddLine(New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height - 1), New Point(Rect.X + Rect.Width, Rect.Y + Rect.Height))
                GP.AddLine(New Point(Rect.X + 1, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
            Case RoundingStyle.BottomRight
                GP.AddLine(New Point(Rect.X, Rect.Y + 1), New Point(Rect.X, Rect.Y))
                GP.AddLine(New Point(Rect.X + Rect.Width - 1, Rect.Y), New Point(Rect.X + Rect.Width, Rect.Y))
                GP.AddArc(New Rectangle(Rect.Width - AW + Rect.X, Rect.Height - AW + Rect.Y, AW, AW), 0, 90)
                GP.AddLine(New Point(Rect.X + 1, Rect.Y + Rect.Height), New Point(Rect.X, Rect.Y + Rect.Height))
        End Select

        GP.CloseAllFigures()

        Return GP

    End Function

End Module

Public Class XylosTabControl
    Inherits TabControl

    Private G As Graphics
    Private Rect As Rectangle
    Private _OverIndex As Integer = -1

    Public Property FirstHeaderBorder As Boolean

    Private Property OverIndex As Integer
        Get
            Return _OverIndex
        End Get
        Set(value As Integer)
            _OverIndex = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Alignment = TabAlignment.Left
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(40, 180)
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint, True)
    End Sub

    Protected Overrides Sub OnControlAdded(e As ControlEventArgs)
        MyBase.OnControlAdded(e)
        e.Control.BackColor = Color.White
        e.Control.ForeColor = HelpersXylos.ColorFromHex("#7C858E")
        e.Control.Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(HelpersXylos.ColorFromHex("#FFFFFF"))

        For I As Integer = 0 To TabPages.Count - 1

            Rect = GetTabRect(I)

            If String.IsNullOrEmpty(TabPages(I).Tag) Then

                If SelectedIndex = I Then

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#3375ED")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#BECCD9")), TextFont As New Font("Segoe UI semibold", 9)
                        G.FillRectangle(Background, New Rectangle(Rect.X - 5, Rect.Y + 1, Rect.Width + 7, Rect.Height))
                        G.DrawString(TabPages(I).Text, TextFont, TextColor, New Point(Rect.X + 50 + (ItemSize.Height - 180), Rect.Y + 12))
                    End Using

                Else

                    Using TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#919BA6")), TextFont As New Font("Segoe UI semibold", 9)
                        G.DrawString(TabPages(I).Text, TextFont, TextColor, New Point(Rect.X + 50 + (ItemSize.Height - 180), Rect.Y + 12))
                    End Using

                End If

                If Not OverIndex = -1 And Not SelectedIndex = OverIndex Then

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#2F3338")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#919BA6")), TextFont As New Font("Segoe UI semibold", 9)
                        G.FillRectangle(Background, New Rectangle(GetTabRect(OverIndex).X - 5, GetTabRect(OverIndex).Y + 1, GetTabRect(OverIndex).Width + 7, GetTabRect(OverIndex).Height))
                        G.DrawString(TabPages(OverIndex).Text, TextFont, TextColor, New Point(GetTabRect(OverIndex).X + 50 + (ItemSize.Height - 180), GetTabRect(OverIndex).Y + 12))
                    End Using

                    If Not IsNothing(ImageList) Then
                        If Not TabPages(OverIndex).ImageIndex < 0 Then
                            G.DrawImage(ImageList.Images(TabPages(OverIndex).ImageIndex), New Rectangle(GetTabRect(OverIndex).X + 25 + (ItemSize.Height - 180), GetTabRect(OverIndex).Y + ((GetTabRect(OverIndex).Height / 2) - 9), 16, 16))
                        End If
                    End If

                End If


                If Not IsNothing(ImageList) Then
                    If Not TabPages(I).ImageIndex < 0 Then
                        G.DrawImage(ImageList.Images(TabPages(I).ImageIndex), New Rectangle(Rect.X + 25 + (ItemSize.Height - 180), Rect.Y + ((Rect.Height / 2) - 9), 16, 16))
                    End If
                End If

            Else

                Using TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#6A7279")), TextFont As New Font("Segoe UI", 7, FontStyle.Bold), Border As New Pen(HelpersXylos.ColorFromHex("#2B2F33"))

                    If FirstHeaderBorder Then
                        G.DrawLine(Border, New Point(Rect.X - 5, Rect.Y + 1), New Point(Rect.Width + 7, Rect.Y + 1))
                    Else
                        If Not I = 0 Then
                            G.DrawLine(Border, New Point(Rect.X - 5, Rect.Y + 1), New Point(Rect.Width + 7, Rect.Y + 1))
                        End If
                    End If

                    G.DrawString(TabPages(I).Text.ToUpper, TextFont, TextColor, New Point(Rect.X + 25 + (ItemSize.Height - 180), Rect.Y + 16))

                End Using

            End If

        Next

    End Sub

    Protected Overrides Sub OnSelecting(e As TabControlCancelEventArgs)
        MyBase.OnSelecting(e)

        If Not IsNothing(e.TabPage) Then
            If Not String.IsNullOrEmpty(e.TabPage.Tag) Then
                e.Cancel = True
            Else
                OverIndex = -1
            End If
        End If

    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        For I As Integer = 0 To TabPages.Count - 1
            If GetTabRect(I).Contains(e.Location) And Not SelectedIndex = I And String.IsNullOrEmpty(TabPages(I).Tag) Then
                OverIndex = I
                Exit For
            Else
                OverIndex = -1
            End If
        Next

    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        OverIndex = -1
    End Sub

End Class

Public Class XylosCheckBox
    Inherits Control

    Public Event CheckedChanged(sender As Object, e As EventArgs)

    Private _Checked As Boolean
    Private _EnabledCalc As Boolean
    Private G As Graphics

    Private B64Enabled As String = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA00lEQVQ4T6WTwQ2CMBSG30/07Ci6gY7gxZoIiYADuAIrsIDpQQ/cHMERZBOuXHimDSWALYL01EO/L//724JmLszk6S+BCOIExFsmL50sEH4kAZxVciYuJgnacD16Plpgg8tFtYMILntQdSXiZ3aXqa1UF/yUsoDw4wKglQaZZPa4RW3JEKzO4RjEbyJaN1BL8gvWgsMp3ADeq0lRJ2FimLZNYWpmFbudUJdolXTLyG2wTmDODUiccEfgSDIIfwmMxAMStS+XHPZn7l/z6Ifk+nSzBR8zi2d9JmVXSgAAAABJRU5ErkJggg=="
    Private B64Disabled As String = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA1UlEQVQ4T6WTzQ2CQBCF56EnLpaiXvUAJBRgB2oFtkALdEAJnoVEMIGzdEIFjNkFN4DLn+xpD/N9efMWQAsPFvL0lyBMUg8MiwzyZwuiJAuI6CyTMxezBC24EuSTBTp4xaaN6JWdqKQbge6udfB1pfbBjrMvEMZZAdCm3ilw7eO1KRmCxRyiOH0TsFUQs5KMwVLweKY7ALFKUZUTECD6qdquCxM7i9jNhLJEraQ5xZzrYJngO9crGYBbAm2SEfhHoCQGeeK+Ls1Ld+fuM0/+kPp+usWCD10idEOGa4QuAAAAAElFTkSuQmCC"

    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value

            If Enabled Then
                Cursor = Cursors.Hand
            Else
                Cursor = Cursors.Default
            End If

            Invalidate()
        End Set
    End Property


    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Enabled = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(BackColor)

        If Enabled Then

            Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F3F4F7")), Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E")), TextFont As New Font("Segoe UI", 9)
                G.FillPath(Background, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                G.DrawPath(Border, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
            End Using

            If Checked Then

                Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(B64Enabled)))
                    G.DrawImage(I, New Rectangle(3, 3, 11, 11))
                End Using

            End If

        Else

            Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F8")), Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7")), TextFont As New Font("Segoe UI", 9)
                G.FillPath(Background, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                G.DrawPath(Border, HelpersXylos.RoundRect1(New Rectangle(0, 0, 16, 16), 3))
                G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
            End Using

            If Checked Then

                Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(B64Disabled)))
                    G.DrawImage(I, New Rectangle(3, 3, 11, 11))
                End Using

            End If

        End If

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If Enabled Then
            Checked = Not Checked
            RaiseEvent CheckedChanged(Me, e)
        End If

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(Width, 18)
    End Sub

End Class

Public Class XylosRadioButton
    Inherits Control

    Public Event CheckedChanged(sender As Object, e As EventArgs)

    Private _Checked As Boolean
    Private _EnabledCalc As Boolean
    Private G As Graphics

    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value

            If Enabled Then
                Cursor = Cursors.Hand
            Else
                Cursor = Cursors.Default
            End If

            Invalidate()
        End Set
    End Property

    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Enabled = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Color.White)

        If Enabled Then

            Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F3F4F7")), Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E")), TextFont As New Font("Segoe UI", 9)
                G.FillEllipse(Background, New Rectangle(0, 0, 16, 16))
                G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
                G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
            End Using

            If Checked Then

                Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#575C62"))
                    G.FillEllipse(Background, New Rectangle(4, 4, 8, 8))
                End Using

            End If

        Else

            Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F8")), Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7")), TextFont As New Font("Segoe UI", 9)
                G.FillEllipse(Background, New Rectangle(0, 0, 16, 16))
                G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
                G.DrawString(Text, TextFont, TextColor, New Point(25, 0))
            End Using

            If Checked Then

                Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#BCC1C6"))
                    G.FillEllipse(Background, New Rectangle(4, 4, 8, 8))
                End Using

            End If

        End If

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If Enabled Then

            For Each C As Control In Parent.Controls
                If TypeOf C Is XylosRadioButton Then
                    DirectCast(C, XylosRadioButton).Checked = False
                End If
            Next

            Checked = Not Checked
            RaiseEvent CheckedChanged(Me, e)
        End If

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(Width, 18)
    End Sub

End Class

Public Class XylosNotice
    Inherits TextBox

    Private G As Graphics
    Private B64 As String = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAABL0lEQVQ4T5VT0VGDQBB9e2cBdGBSgTIDEr9MCw7pI0kFtgB9yFiC+KWMmREqMOnAAuDWOfAiudzhyA/svtvH7Xu7BOv5eH2atVKtwbwk0LWGGVyDqLzoRB7e3u/HJTQOdm+PGYjWNuk4ZkIW36RbkzsS7KqiBnB1Usw49DHh8oQEXMfJKhwgAM4/Mw7RIp0NeLG3ScCcR4vVhnTPnVCf9rUZeImTdKnz71VREnBnn5FKzMnX95jA2V6vLufkBQFESTq0WBXsEla7owmcoC6QJMKW2oCUePY5M0lAjK0iBAQ8TBGc2/d7+uvnM/AQNF4Rp4bpiGkRfTb2Gigx12+XzQb3D9JfBGaQzHWm7HS000RJ2i/av5fJjPDZMplErwl1GxDpMTbL1YC5lCwze52/AQFekh7wKBpGAAAAAElFTkSuQmCC"

    Sub New()
        DoubleBuffered = True
        Enabled = False
        [ReadOnly] = True
        BorderStyle = BorderStyle.None
        Multiline = True
        Cursor = Cursors.Default
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Color.White)

        Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#FFFDE8")), MainBorder As New Pen(HelpersXylos.ColorFromHex("#F2F3F7")), TextColor As New SolidBrush(HelpersXylos.ColorFromHex("#B9B595")), TextFont As New Font("Segoe UI", 9)
            G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
            G.DrawPath(MainBorder, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
            G.DrawString(Text, TextFont, TextColor, New Point(30, 6))
        End Using

        Using I As Image = Image.FromStream(New IO.MemoryStream(Convert.FromBase64String(B64)))
            G.DrawImage(I, New Rectangle(8, Height / 2 - 8, 16, 16))
        End Using

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

    End Sub

End Class


Public Class XylosTextBox
    Inherits Control

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
    End Enum

    Private WithEvents TB As New TextBox
    Private G As Graphics
    Private State As MouseState
    Private IsDown As Boolean

    Private _EnabledCalc As Boolean
    Private _allowpassword As Boolean = False
    Private _maxChars As Integer = 32767
    Private _textAlignment As HorizontalAlignment
    Private _multiLine As Boolean = False
    Private _readOnly As Boolean = False

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            TB.Enabled = value
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _allowpassword
        End Get
        Set(ByVal value As Boolean)
            TB.UseSystemPasswordChar = UseSystemPasswordChar
            _allowpassword = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property MaxLength() As Integer
        Get
            Return _maxChars
        End Get
        Set(ByVal value As Integer)
            _maxChars = value
            TB.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property

    Public Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _textAlignment
        End Get
        Set(ByVal value As HorizontalAlignment)
            _textAlignment = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property MultiLine() As Boolean
        Get
            Return _multiLine
        End Get
        Set(ByVal value As Boolean)
            _multiLine = value
            TB.Multiline = value
            OnResize(EventArgs.Empty)
            Invalidate()
        End Set
    End Property

    Public Shadows Property [ReadOnly]() As Boolean
        Get
            Return _readOnly
        End Get
        Set(ByVal value As Boolean)
            _readOnly = value
            If TB IsNot Nothing Then
                TB.ReadOnly = value
            End If
        End Set
    End Property

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnBackColorChanged(ByVal e As EventArgs)
        MyBase.OnBackColorChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnForeColorChanged(ByVal e As EventArgs)
        MyBase.OnForeColorChanged(e)
        TB.ForeColor = ForeColor
        Invalidate()
    End Sub

    Protected Overrides Sub OnFontChanged(ByVal e As EventArgs)
        MyBase.OnFontChanged(e)
        TB.Font = Font
    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As EventArgs)
        MyBase.OnGotFocus(e)
        TB.Focus()
    End Sub

    Private Sub TextChangeTb() Handles TB.TextChanged
        Text = TB.Text
    End Sub

    Private Sub TextChng() Handles MyBase.TextChanged
        TB.Text = Text
    End Sub

    Public Sub NewTextBox()
        With TB
            .Text = String.Empty
            .BackColor = Color.White
            .ForeColor = HelpersXylos.ColorFromHex("#7C858E")
            .TextAlign = HorizontalAlignment.Left
            .BorderStyle = BorderStyle.None
            .Location = New Point(3, 3)
            .Font = New Font("Segoe UI", 9)
            .Size = New Size(Width - 3, Height - 3)
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With
    End Sub

    Sub New()
        MyBase.New()
        NewTextBox()
        Controls.Add(TB)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        TextAlign = HorizontalAlignment.Left
        ForeColor = HelpersXylos.ColorFromHex("#7C858E")
        Font = New Font("Segoe UI", 9)
        Size = New Size(130, 29)
        Enabled = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Color.White)

        If Enabled Then

            TB.ForeColor = HelpersXylos.ColorFromHex("#7C858E")

            If State = MouseState.Down Then

                Using Border As New Pen(HelpersXylos.ColorFromHex("#78B7E6"))
                    G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 12))
                End Using

            Else

                Using Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9"))
                    G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 12))
                End Using

            End If

        Else

            TB.ForeColor = HelpersXylos.ColorFromHex("#7C858E")

            Using Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2"))
                G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 12))
            End Using

        End If

        TB.TextAlign = TextAlign
        TB.UseSystemPasswordChar = UseSystemPasswordChar

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If Not MultiLine Then
            Dim tbheight As Integer = TB.Height
            TB.Location = New Point(10, CType(((Height / 2) - (tbheight / 2) - 0), Integer))
            TB.Size = New Size(Width - 20, tbheight)
        Else
            TB.Location = New Point(10, 10)
            TB.Size = New Size(Width - 20, Height - 20)
        End If
    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        MyBase.OnEnter(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        MyBase.OnLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

End Class

Public Class XylosProgressBar
    Inherits Control

#Region " Drawing "

    Private _Val As Integer = 0
    Private _Min As Integer = 0
    Private _Max As Integer = 100

    Public Property Stripes As Color = Color.DarkGreen
    Public Property BackgroundColor As Color = Color.Green


    Public Property Value As Integer
        Get
            Return _Val
        End Get
        Set(value As Integer)
            _Val = value
            Invalidate()
        End Set
    End Property

    Public Property Minimum As Integer
        Get
            Return _Min
        End Get
        Set(value As Integer)
            _Min = value
            Invalidate()
        End Set
    End Property

    Public Property Maximum As Integer
        Get
            Return _Max
        End Get
        Set(value As Integer)
            _Max = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Maximum = 100
        Minimum = 0
        Value = 0
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Color.White)

        Using Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9"))
            G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 6))
        End Using

        If Not Value = 0 Then

            Using Background As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.LightUpwardDiagonal, Stripes, BackgroundColor)
                G.FillPath(Background, HelpersXylos.RoundRect1(New Rectangle(0, 0, Value / Maximum * Width - 1, Height - 1), 6))
            End Using

        End If


    End Sub

#End Region

End Class

Public Class XylosCombobox
    Inherits ComboBox

    Private G As Graphics
    Private Rect As Rectangle
    Private _EnabledCalc As Boolean

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            MyBase.Enabled = value
            Enabled = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        DropDownStyle = ComboBoxStyle.DropDownList
        Cursor = Cursors.Hand
        Enabled = True
        DrawMode = DrawMode.OwnerDrawFixed
        ItemHeight = 20
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        G.Clear(Color.White)

        If Enabled Then

            Using Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9")), TriangleColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E")), TriangleFont As New Font("Marlett", 13)
                G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 6))
                G.DrawString("6", TriangleFont, TriangleColor, New Point(Width - 22, 3))
            End Using

        Else

            Using Border As New Pen(HelpersXylos.ColorFromHex("#E1E1E2")), TriangleColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7")), TriangleFont As New Font("Marlett", 13)
                G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 6))
                G.DrawString("6", TriangleFont, TriangleColor, New Point(Width - 22, 3))
            End Using

        End If

        If Not IsNothing(Items) Then

            Using ItemsFont As New Font("Segoe UI", 9), ItemsColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E"))

                If Enabled Then

                    If Not SelectedIndex = -1 Then
                        G.DrawString(GetItemText(Items(SelectedIndex)), ItemsFont, ItemsColor, New Point(7, 4))
                    Else
                        Try
                            G.DrawString(GetItemText(Items(0)), ItemsFont, ItemsColor, New Point(7, 4))
                        Catch
                        End Try
                    End If

                Else

                    Using DisabledItemsColor As New SolidBrush(HelpersXylos.ColorFromHex("#D0D3D7"))

                        If Not SelectedIndex = -1 Then
                            G.DrawString(GetItemText(Items(SelectedIndex)), ItemsFont, DisabledItemsColor, New Point(7, 4))
                        Else
                            G.DrawString(GetItemText(Items(0)), ItemsFont, DisabledItemsColor, New Point(7, 4))
                        End If

                    End Using

                End If

            End Using

        End If

    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        MyBase.OnDrawItem(e)
        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        If Enabled Then
            e.DrawBackground()
            Rect = e.Bounds

            Try

                Using ItemsFont As New Font("Segoe UI", 9), Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9"))

                    If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then

                        Using ItemsColor As New SolidBrush(Color.White), Itembackground As New SolidBrush(HelpersXylos.ColorFromHex("#78B7E6"))
                            G.FillRectangle(Itembackground, Rect)
                            G.DrawString(GetItemText(Items(e.Index)), New Font("Segoe UI", 9), Brushes.White, New Point(Rect.X + 5, Rect.Y + 1))
                        End Using

                    Else
                        Using ItemsColor As New SolidBrush(HelpersXylos.ColorFromHex("#7C858E"))
                            G.FillRectangle(Brushes.White, Rect)
                            G.DrawString(GetItemText(Items(e.Index)), New Font("Segoe UI", 9), ItemsColor, New Point(Rect.X + 5, Rect.Y + 1))
                        End Using

                    End If

                End Using

            Catch
            End Try

        End If

    End Sub

    Protected Overrides Sub OnSelectedItemChanged(ByVal e As EventArgs)
        MyBase.OnSelectedItemChanged(e)
        Invalidate()
    End Sub

End Class

Public Class XylosSeparator
    Inherits Control

    Private G As Graphics

    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        Using C As New Pen(HelpersXylos.ColorFromHex("#EBEBEC"))
            G.DrawLine(C, New Point(0, 0), New Point(Width, 0))
        End Using

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(Width, 2)
    End Sub

End Class

Public Class XylosButton
    Inherits Control

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
    End Enum

    Private G As Graphics
    Private State As MouseState

    Private _EnabledCalc As Boolean

    Public Shadows Event Click(sender As Object, e As EventArgs)

    Sub New()
        DoubleBuffered = True
        Enabled = True
    End Sub

    Public Shadows Property Enabled As Boolean
        Get
            Return EnabledCalc
        End Get
        Set(value As Boolean)
            _EnabledCalc = value
            Invalidate()
        End Set
    End Property

    Public Property EnabledCalc As Boolean
        Get
            Return _EnabledCalc
        End Get
        Set(value As Boolean)
            Enabled = value
            Invalidate()
        End Set
    End Property

    Private _BackColorA As Color = Color.White
    Public Property BackColorA As Color
        Get
            Return _BackColorA
        End Get
        Set(value As Color)
            _BackColorA = value
            Invalidate()
        End Set
    End Property



    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        G = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        If Enabled Then

            Select Case State

                Case MouseState.Over

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#FDFDFD"))
                        G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                    End Using

                Case MouseState.Down

                    Using Background As New SolidBrush(HelpersXylos.ColorFromHex("#F0F0F0"))
                        G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                    End Using

                Case Else

                    Using Background As New SolidBrush(_BackColorA) 'HelpersXylos.ColorFromHex("#F6F6F6")
                        G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                    End Using

            End Select

            Using ButtonFont As New Font("Segoe UI", 9), Border As New Pen(HelpersXylos.ColorFromHex("#C3C3C3"))
                G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                HelpersXylos.CenterString(G, Text, ButtonFont, HelpersXylos.ColorFromHex("#7C858E"), HelpersXylos.FullRectangle(Size, False))
            End Using

        Else

            Using Background As New SolidBrush(_BackColorA), Border As New Pen(Color.White), ButtonFont As New Font("Segoe UI", 9)
                G.FillPath(Background, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                G.DrawPath(Border, HelpersXylos.RoundRect1(HelpersXylos.FullRectangle(Size, True), 3))
                HelpersXylos.CenterString(G, Text, ButtonFont, HelpersXylos.ColorFromHex("#D0D3D7"), HelpersXylos.FullRectangle(Size, False))
            End Using

        End If

    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If Enabled Then
            RaiseEvent Click(Me, e)
        End If

        State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub

End Class