' // Booster Theme GDI+
' // 9 Controls
' // Made by AeroRev9 / Naywyn
' // 02/22.

Option Strict On

Imports System.Threading
Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Friend Module Helpers1

    Public G As Graphics
    Private TargetStringMeasure As SizeF

    Enum MouseState1 As Byte
        None = 0
        Over = 1
        Down = 2
    End Enum

    Enum RoundingStyle As Byte
        All = 0
        Top = 1
        Bottom = 2
        Left = 3
        Right = 4
        TopRight = 5
        BottomRight = 6
    End Enum

    Public Function ColorFromHex(Hex As String) As Color
        Return Color.FromArgb(CInt(Long.Parse(String.Format("FFFFFFFFFF{0}", Hex.Substring(1)), Globalization.NumberStyles.HexNumber)))
    End Function

    Public Function MiddlePoint1(TargetText As String, TargetFont As Font, Rect As Rectangle) As Point
        TargetStringMeasure = G.MeasureString(TargetText, TargetFont)
        Return New Point(CInt(Rect.Width / 2 - TargetStringMeasure.Width / 2), CInt(Rect.Height / 2 - TargetStringMeasure.Height / 2))
    End Function

    Public Function RoundRect1(Rect As Rectangle, Rounding As Integer, Optional Style As RoundingStyle = RoundingStyle.All) As GraphicsPath

        Dim GP As New GraphicsPath()
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

Public Class BoosterButton
    Inherits Button

    Private State As MouseState1
    Private Gradient As LinearGradientBrush

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
        ForeColor = HelpersXylos.ColorFromHex("#B6B6B6")
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        ThemeModule.G.Clear(Parent.BackColor)

        If Enabled Then

            Select Case State
                Case MouseState1.None
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), HelpersXylos.ColorFromHex("#606060"), HelpersXylos.ColorFromHex("#4E4E4E"), LinearGradientMode.Vertical)

                Case MouseState1.Over
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), HelpersXylos.ColorFromHex("#6A6A6A"), HelpersXylos.ColorFromHex("#585858"), LinearGradientMode.Vertical)

                Case MouseState1.Down
                    Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), HelpersXylos.ColorFromHex("#565656"), HelpersXylos.ColorFromHex("#444444"), LinearGradientMode.Vertical)

            End Select

            ThemeModule.G.FillPath(Gradient, HelpersXylos.RoundRect1(New Rectangle(0, 0, Width - 1, Height - 1), 3))

            Using Border As New Pen(HelpersXylos.ColorFromHex("#323232"))
                ThemeModule.G.DrawPath(Border, HelpersXylos.RoundRect1(New Rectangle(0, 0, Width - 1, Height - 1), 3))
            End Using

            '// Top Line
            Select Case State

                Case MouseState1.None

                    Using TopLine As New Pen(HelpersXylos.ColorFromHex("#737373"))
                        ThemeModule.G.DrawLine(TopLine, 4, 1, Width - 4, 1)
                    End Using

                Case MouseState1.Over

                    Using TopLine As New Pen(HelpersXylos.ColorFromHex("#7D7D7D"))
                        ThemeModule.G.DrawLine(TopLine, 4, 1, Width - 4, 1)
                    End Using

                Case MouseState1.Down

                    Using TopLine As New Pen(HelpersXylos.ColorFromHex("#696969"))
                        ThemeModule.G.DrawLine(TopLine, 4, 1, Width - 4, 1)
                    End Using

            End Select

            Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F5")), TextFont As New Font("Segoe UI", 9)
                ThemeModule.G.DrawString(Text, TextFont, TextBrush, MiddlePoint1(Text, TextFont, New Rectangle(0, 0, Width + 2, Height)))
            End Using

        Else

            Gradient = New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), HelpersXylos.ColorFromHex("#4C4C4C"), HelpersXylos.ColorFromHex("#3A3A3A"), LinearGradientMode.Vertical)

            ThemeModule.G.FillPath(Gradient, HelpersXylos.RoundRect1(New Rectangle(0, 0, Width - 1, Height - 1), 3))

            Using Border As New Pen(HelpersXylos.ColorFromHex("#323232"))
                ThemeModule.G.DrawPath(Border, HelpersXylos.RoundRect1(New Rectangle(0, 0, Width - 1, Height - 1), 3))
            End Using

            Using TopLine As New Pen(HelpersXylos.ColorFromHex("#5F5F5F"))
                ThemeModule.G.DrawLine(TopLine, 4, 1, Width - 4, 1)
            End Using

            Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#818181")), TextFont As New Font("Segoe UI", 9)
                ThemeModule.G.DrawString(Text, TextFont, TextBrush, MiddlePoint1(Text, TextFont, New Rectangle(0, 0, Width + 2, Height)))
            End Using

        End If

    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        State = MouseState1.Over : Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        State = MouseState1.None : Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        State = MouseState1.Down : Invalidate()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        State = MouseState1.Over : Invalidate()
        MyBase.OnMouseUp(e)
    End Sub

End Class

Public Class BoosterHeader
    Inherits Control

    Private TextMeasure As SizeF

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 10)
        ForeColor = HelpersXylos.ColorFromHex("#C0C0C0")
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        ThemeModule.G.Clear(Parent.BackColor)

        Using Line As New Pen(HelpersXylos.ColorFromHex("#5C5C5C"))
            ThemeModule.G.DrawLine(Line, 0, 6, Width - 1, 6)
        End Using

        Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#D4D4D4")), TextFont As New Font("Segoe UI", 10), ParentFill As New SolidBrush(Parent.BackColor)
            TextMeasure = ThemeModule.G.MeasureString(Text, TextFont)
            ThemeModule.G.FillRectangle(ParentFill, New Rectangle(14, -4, CInt(TextMeasure.Width + 8), CInt(TextMeasure.Height + 4)))
            ThemeModule.G.DrawString(Text, TextFont, TextBrush, New Point(20, -4))
        End Using

        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        Size = New Size(Width, 14)
        MyBase.OnResize(e)
    End Sub

End Class

Public Class BoosterToolTip
    Inherits ToolTip

    Public Sub New()
        OwnerDraw = True
        BackColor = HelpersXylos.ColorFromHex("#242424")
        AddHandler Draw, AddressOf OnDraw
    End Sub

    Private Sub OnDraw(sender As Object, e As DrawToolTipEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        ThemeModule.G.Clear(HelpersXylos.ColorFromHex("#242424"))

        Using Border As New Pen(HelpersXylos.ColorFromHex("#343434"))
            ThemeModule.G.DrawRectangle(Border, New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1))
        End Using

        If ToolTipIcon = ToolTipIcon.None Then

            Using TextFont As New Font("Segoe UI", 9), TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#B6B6B6"))
                ThemeModule.G.DrawString(e.ToolTipText, TextFont, TextBrush, New PointF(e.Bounds.X + 4, e.Bounds.Y + 1))
            End Using

        Else

            Select Case ToolTipIcon

                Case ToolTipIcon.Info

                    Using TextFont As New Font("Segoe UI", 9, FontStyle.Bold), TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#7FD88B"))
                        ThemeModule.G.DrawString("Information", TextFont, TextBrush, New PointF(e.Bounds.X + 4, e.Bounds.Y + 2))
                    End Using

                Case ToolTipIcon.Warning

                    Using TextFont As New Font("Segoe UI", 9, FontStyle.Bold), TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#D8C67F"))
                        ThemeModule.G.DrawString("Warning", TextFont, TextBrush, New PointF(e.Bounds.X + 4, e.Bounds.Y + 2))
                    End Using

                Case ToolTipIcon.Error

                    Using TextFont As New Font("Segoe UI", 9, FontStyle.Bold), TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#D87F7F"))
                        ThemeModule.G.DrawString("Error", TextFont, TextBrush, New PointF(e.Bounds.X + 4, e.Bounds.Y + 2))
                    End Using

            End Select

            Using TextFont As New Font("Segoe UI", 9), TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#B6B6B6"))
                ThemeModule.G.DrawString(e.ToolTipText, TextFont, TextBrush, New PointF(e.Bounds.X + 4, e.Bounds.Y + 15))
            End Using

        End If

    End Sub

End Class

<DefaultEvent("TextChanged")>
Public Class BoosterTextBox
    Inherits Control

    Private WithEvents T As TextBox
    Private State As MouseState1

    Public Shadows Property Text As String
        Get
            Return T.Text
        End Get
        Set(value As String)
            MyBase.Text = value
            T.Text = value
            Invalidate()
        End Set
    End Property

    Public Shadows Property Enabled As Boolean
        Get
            Return T.Enabled
        End Get
        Set(value As Boolean)
            T.Enabled = value
            Invalidate()
        End Set
    End Property

    Public Property UseSystemPasswordChar As Boolean
        Get
            Return T.UseSystemPasswordChar
        End Get
        Set(value As Boolean)
            T.UseSystemPasswordChar = value
            Invalidate()
        End Set
    End Property

    Public Property MultiLine() As Boolean
        Get
            Return T.Multiline
        End Get
        Set(ByVal value As Boolean)
            T.Multiline = value
            Size = New Size(T.Width + 2, T.Height + 2)
            Invalidate()
        End Set
    End Property

    Public Shadows Property [ReadOnly]() As Boolean
        Get
            Return T.ReadOnly
        End Get
        Set(ByVal value As Boolean)
            T.ReadOnly = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True

        T = New TextBox With {
            .BorderStyle = BorderStyle.None,
            .BackColor = HelpersXylos.ColorFromHex("#242424"),
            .ForeColor = HelpersXylos.ColorFromHex("#B6B6B6"),
            .Location = New Point(1, 1),
            .Multiline = True}

        Controls.Add(T)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        If Enabled Then

            T.BackColor = HelpersXylos.ColorFromHex("#242424")

            Select Case State

                Case MouseState1.Down

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#C8C8C8"))
                        ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
                    End Using

                Case Else

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#5C5C5C"))
                        ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
                    End Using

            End Select

        Else

            T.BackColor = HelpersXylos.ColorFromHex("#282828")

            Using Border As New Pen(HelpersXylos.ColorFromHex("#484848"))
                ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
            End Using

        End If

        MyBase.OnPaint(e)

    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        State = MouseState1.Down : Invalidate()
        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)
        State = MouseState1.None : Invalidate()
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        If MultiLine Then
            T.Size = New Size(Width - 2, Height - 2) : Invalidate()
        Else
            T.Size = New Size(Width - 2, T.Height)
            Size = New Size(Width, T.Height + 2)
        End If
        MyBase.OnResize(e)
    End Sub

    Private Sub TTextChanged() Handles T.TextChanged
        MyBase.OnTextChanged(EventArgs.Empty)
    End Sub

End Class

Public Class BoosterComboBox
    Inherits ComboBox

    Private State As MouseState1
    Private Rect As Rectangle

    Private ItemString As String = String.Empty
    Private FirstItem As String = String.Empty

    Sub New()
        ItemHeight = 20
        DoubleBuffered = True
        BackColor = Color.FromArgb(36, 36, 36)
        DropDownStyle = ComboBoxStyle.DropDownList
        DrawMode = DrawMode.OwnerDrawFixed
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        ThemeModule.G.Clear(Parent.BackColor)

        If Enabled Then

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#242424"))
                ThemeModule.G.FillRectangle(Fill, New Rectangle(0, 0, Width - 1, Height - 1))
            End Using

            Select Case State

                Case MouseState1.None

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#5C5C5C"))
                        ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
                    End Using

                Case MouseState1.Over

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#C8C8C8"))
                        ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
                    End Using

            End Select

            Using ArrowFont As New Font("Marlett", 12), ArrowBrush As New SolidBrush(HelpersXylos.ColorFromHex("#909090"))
                ThemeModule.G.DrawString("6", ArrowFont, ArrowBrush, New Point(Width - 20, 5))
            End Using

            If Not IsNothing(Items) Then

                Try : FirstItem = GetItemText(Items(0)) : Catch : End Try

                If Not SelectedIndex = -1 Then

                    Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#B6B6B6")), TextFont As New Font("Segoe UI", 9)
                        ThemeModule.G.DrawString(ItemString, TextFont, TextBrush, New Point(4, 4))
                    End Using

                Else

                    Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#B6B6B6")), TextFont As New Font("Segoe UI", 9)
                        ThemeModule.G.DrawString(FirstItem, TextFont, TextBrush, New Point(4, 4))
                    End Using

                End If


            End If

        Else

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#282828")), Border As New Pen(HelpersXylos.ColorFromHex("#484848"))
                ThemeModule.G.FillRectangle(Fill, New Rectangle(0, 0, Width - 1, Height - 1))
                ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
            End Using

            Using ArrowFont As New Font("Marlett", 12), ArrowBrush As New SolidBrush(HelpersXylos.ColorFromHex("#707070"))
                ThemeModule.G.DrawString("6", ArrowFont, ArrowBrush, New Point(Width - 20, 5))
            End Using

            If Not IsNothing(Items) Then

                Try : FirstItem = GetItemText(Items(0)) : Catch : End Try

                Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#818181")), TextFont As New Font("Segoe UI", 9)
                    ThemeModule.G.DrawString(FirstItem, TextFont, TextBrush, New Point(4, 4))
                End Using

            End If

        End If

    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        Rect = e.Bounds

        Using Back As New SolidBrush(HelpersXylos.ColorFromHex("#242424"))
            ThemeModule.G.FillRectangle(Back, New Rectangle(e.Bounds.X - 4, e.Bounds.Y - 1, e.Bounds.Width + 4, e.Bounds.Height - 1))
        End Using

        If Not e.Index = -1 Then
            ItemString = GetItemText(Items(e.Index))
        End If

        Using ItemsFont As New Font("Segoe UI", 9), Border As New Pen(HelpersXylos.ColorFromHex("#D0D5D9"))

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then

                Using HoverItemBrush As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F5"))
                    ThemeModule.G.DrawString(ItemString, New Font("Segoe UI", 9), HoverItemBrush, New Point(Rect.X + 5, Rect.Y + 1))
                End Using

            Else

                Using DefaultItemBrush As New SolidBrush(HelpersXylos.ColorFromHex("#C0C0C0"))
                    ThemeModule.G.DrawString(ItemString, New Font("Segoe UI", 9), DefaultItemBrush, New Point(Rect.X + 5, Rect.Y + 1))
                End Using

            End If

        End Using

        e.DrawFocusRectangle()

        MyBase.OnDrawItem(e)

    End Sub

    Protected Overrides Sub OnSelectedItemChanged(ByVal e As EventArgs)
        Invalidate()
        MyBase.OnSelectedItemChanged(e)
    End Sub

    Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
        State = MouseState1.None : Invalidate()
        MyBase.OnSelectedIndexChanged(e)
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        State = MouseState1.Over : Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        State = MouseState1.None : Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

End Class

Public Class BoosterCheckBox
    Inherits CheckBox

    Private State As MouseState1
    Private Block As Boolean

    Private CheckThread, UncheckThread As Thread
    Private OverFillRect As New Rectangle(1, 1, 14, 14)

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Private Sub CheckAnimation()

        Block = True

        Dim X As Integer = 1
        Dim Rectw As Integer = 15

        While Not OverFillRect.Width = 0
            X += 1
            Rectw -= 1
            OverFillRect = New Rectangle(X, OverFillRect.Y, Rectw, OverFillRect.Height)
            Invalidate()
            Thread.Sleep(30)
        End While

        Block = False

    End Sub

    Private Sub UncheckAnimation()

        Block = True

        Dim X As Integer = 15
        Dim Rectw As Integer = 0

        While Not OverFillRect.Width = 14
            X -= 1
            Rectw += 1
            OverFillRect = New Rectangle(X, OverFillRect.Y, Rectw, OverFillRect.Height)
            Invalidate()
            Thread.Sleep(30)
        End While

        Block = False

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        ThemeModule.G.Clear(Parent.BackColor)

        If Enabled Then

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#242424")), Border As New Pen(HelpersXylos.ColorFromHex("#5C5C5C"))
                ThemeModule.G.FillRectangle(Fill, New Rectangle(0, 0, 16, 16))
                ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Select Case State

                Case MouseState1.None

                    Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#B6B6B6")), TextFont As New Font("Segoe UI", 9)
                        ThemeModule.G.DrawString(Text, TextFont, TextBrush, New Point(25, -1))
                    End Using

                Case MouseState1.Over

                    Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F5")), TextFont As New Font("Segoe UI", 9)
                        ThemeModule.G.DrawString(Text, TextFont, TextBrush, New Point(25, -1))
                    End Using

            End Select

            Using CheckFont As New Font("Marlett", 12), CheckBrush As New SolidBrush(Color.FromArgb(144, 144, 144))
                ThemeModule.G.DrawString("b", CheckFont, CheckBrush, New Point(-2, 1))
            End Using

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#242424"))
                ThemeModule.G.SmoothingMode = SmoothingMode.None
                ThemeModule.G.FillRectangle(Fill, OverFillRect)
            End Using

        Else

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#282828")), Border As New Pen(HelpersXylos.ColorFromHex("#484848"))
                ThemeModule.G.FillRectangle(Fill, New Rectangle(0, 0, 16, 16))
                ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#818181")), TextFont As New Font("Segoe UI", 9)
                ThemeModule.G.DrawString(Text, TextFont, TextBrush, New Point(25, -1))
            End Using

            Using CheckFont As New Font("Marlett", 12), CheckBrush As New SolidBrush(HelpersXylos.ColorFromHex("#707070"))
                ThemeModule.G.DrawString("b", CheckFont, CheckBrush, New Point(-2, 1))
            End Using

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#282828"))
                ThemeModule.G.SmoothingMode = SmoothingMode.None
                ThemeModule.G.FillRectangle(Fill, OverFillRect)
            End Using

        End If

    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        State = MouseState1.Over : Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        State = MouseState1.None : Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnCheckedChanged(e As EventArgs)

        If Checked Then
            CheckThread = New Thread(AddressOf CheckAnimation) With {
                .IsBackground = True}
            CheckThread.Start()
        Else
            UncheckThread = New Thread(AddressOf UncheckAnimation) With {
             .IsBackground = True}
            UncheckThread.Start()
        End If

        If Not Block Then
            MyBase.OnCheckedChanged(e)
        End If

    End Sub

End Class

Public Class BoosterTabControl
    Inherits TabControl

    Private MainRect As Rectangle
    Private OverRect As Rectangle

    Private SubOverIndex As Integer = -1

    Private ReadOnly Property Hovering As Boolean
        Get
            Return Not OverIndex = -1
        End Get
    End Property

    Private Property OverIndex As Integer
        Get
            Return SubOverIndex
        End Get
        Set(value As Integer)
            SubOverIndex = value
            If Not SubOverIndex = -1 Then
                OverRect = GetTabRect(OverIndex)
            End If
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 10)
        ForeColor = HelpersXylos.ColorFromHex("#78797B")
        ItemSize = New Size(40, 170)
        SizeMode = TabSizeMode.Fixed
        Alignment = TabAlignment.Left
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Protected Overrides Sub CreateHandle()
        For Each Tab As TabPage In TabPages
            Tab.BackColor = HelpersXylos.ColorFromHex("#424242")
            Tab.ForeColor = HelpersXylos.ColorFromHex("#B6B6B6")
            Tab.Font = New Font("Segoe UI", 9)
        Next
        MyBase.CreateHandle()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        ThemeModule.G.Clear(HelpersXylos.ColorFromHex("#333333"))

        Using Border As New Pen(HelpersXylos.ColorFromHex("#292929"))
            ThemeModule.G.SmoothingMode = SmoothingMode.None
            ThemeModule.G.DrawLine(Border, ItemSize.Height + 3, 4, ItemSize.Height + 3, Height - 5)
        End Using

        For I As Integer = 0 To TabPages.Count - 1

            MainRect = GetTabRect(I)

            If SelectedIndex = I Then

                Using Selection As New SolidBrush(HelpersXylos.ColorFromHex("#424242"))
                    ThemeModule.G.FillRectangle(Selection, New Rectangle(MainRect.X - 6, MainRect.Y + 2, MainRect.Width + 8, MainRect.Height - 1))
                End Using

                Using SelectionLeft As New SolidBrush(HelpersXylos.ColorFromHex("#F63333"))
                    ThemeModule.G.FillRectangle(SelectionLeft, New Rectangle(MainRect.X - 2, MainRect.Y + 2, 3, MainRect.Height - 1))
                End Using

                Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F5")), TextFont As New Font("Segoe UI", 10)
                    ThemeModule.G.DrawString(TabPages(I).Text, TextFont, TextBrush, New Point(MainRect.X + 25, MainRect.Y + 11))
                End Using

            Else

                Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#C0C0C0")), TextFont As New Font("Segoe UI", 10)
                    ThemeModule.G.DrawString(TabPages(I).Text, TextFont, TextBrush, New Point(MainRect.X + 25, MainRect.Y + 11))
                End Using

            End If

            If Hovering Then

                Using Selection As New SolidBrush(HelpersXylos.ColorFromHex("#383838"))
                    ThemeModule.G.FillRectangle(Selection, New Rectangle(OverRect.X - 6, OverRect.Y + 2, OverRect.Width + 8, OverRect.Height - 1))
                End Using

                Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#C0C0C0")), TextFont As New Font("Segoe UI", 10)
                    ThemeModule.G.DrawString(TabPages(OverIndex).Text, TextFont, TextBrush, New Point(OverRect.X + 25, OverRect.Y + 11))
                End Using

            End If

        Next

        MyBase.OnPaint(e)

    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        For I As Integer = 0 To TabPages.Count - 1
            If GetTabRect(I).Contains(e.Location) And Not SelectedIndex = I Then
                OverIndex = I
                Exit For
            Else
                OverIndex = -1
            End If
        Next
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        OverIndex = -1
        MyBase.OnMouseLeave(e)
    End Sub

End Class

Public Class BoosterRadioButton
    Inherits RadioButton

    Private State As MouseState1

    Private CheckThread, UncheckThread As Thread
    Private EllipseRect As New Rectangle(5, 5, 6, 6)

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Private Sub CheckAnimation()

        Dim X As Integer = 1
        Dim Y As Integer = 1
        Dim EllipseW As Integer = 14
        Dim EllipseH As Integer = 14

        While Not EllipseH = 8

            If X < 4 Then
                X += 1
                Y += 1
            End If

            EllipseW -= 1
            EllipseH -= 1
            EllipseRect = New Rectangle(X, Y, EllipseW, EllipseH)
            Invalidate()
            Thread.Sleep(30)
        End While

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        MyBase.OnPaint(e)

        ThemeModule.G.Clear(Parent.BackColor)

        If Enabled Then

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#242424")), Border As New Pen(HelpersXylos.ColorFromHex("#5C5C5C"))
                ThemeModule.G.FillEllipse(Fill, New Rectangle(0, 0, 16, 16))
                ThemeModule.G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Select Case State

                Case MouseState1.None

                    Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#B6B6B6")), TextFont As New Font("Segoe UI", 9)
                        ThemeModule.G.DrawString(Text, TextFont, TextBrush, New Point(25, -1))
                    End Using

                Case MouseState1.Over

                    Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#F5F5F5")), TextFont As New Font("Segoe UI", 9)
                        ThemeModule.G.DrawString(Text, TextFont, TextBrush, New Point(25, -1))
                    End Using

            End Select

            If Checked Then

                Using CheckBrush As New SolidBrush(HelpersXylos.ColorFromHex("#909090"))
                    ThemeModule.G.FillEllipse(CheckBrush, EllipseRect)
                End Using

            End If

        Else

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#282828")), Border As New Pen(HelpersXylos.ColorFromHex("#484848"))
                ThemeModule.G.FillEllipse(Fill, New Rectangle(0, 0, 16, 16))
                ThemeModule.G.DrawEllipse(Border, New Rectangle(0, 0, 16, 16))
            End Using

            Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#818181")), TextFont As New Font("Segoe UI", 9)
                ThemeModule.G.DrawString(Text, TextFont, TextBrush, New Point(25, -1))
            End Using

            If Checked Then

                Using CheckBrush As New SolidBrush(HelpersXylos.ColorFromHex("#707070"))
                    ThemeModule.G.FillEllipse(CheckBrush, EllipseRect)
                End Using

            End If

        End If

    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        State = MouseState1.Over : Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        State = MouseState1.None : Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnCheckedChanged(e As EventArgs)

        If Checked Then
            CheckThread = New Thread(AddressOf CheckAnimation) With {
                .IsBackground = True}
            CheckThread.Start()
        End If

        MyBase.OnCheckedChanged(e)
    End Sub

End Class

Public Class BoosterNumericUpDown
    Inherits NumericUpDown

    Private State As MouseState1
    Public Property AfterValue As String

    Private ValueChangedThread As Thread
    Private TextPoint As New Point(2, 2)
    Private TextFont As New Font("Segoe UI", 10)

    Sub New()
        DoubleBuffered = True
        Font = New Font("Segoe UI", 10)
        Controls(0).Hide()
        Controls(1).Hide()
        ForeColor = HelpersXylos.ColorFromHex("#B6B6B6")
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.Opaque Or ControlStyles.OptimizedDoubleBuffer, True)
    End Sub

    Private Sub ValueChangedAnimation()

        Dim TextSize As Integer = 5

        While Not TextSize = 10
            TextSize += 1
            TextFont = New Font("Segoe UI", TextSize)
            Invalidate()
            Thread.Sleep(30)
        End While

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        ThemeModule.G = e.Graphics
        ThemeModule.G.SmoothingMode = SmoothingMode.HighQuality
        ThemeModule.G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit

        ThemeModule.G.Clear(Parent.BackColor)

        MyBase.OnPaint(e)

        If Enabled Then

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#242424"))
                ThemeModule.G.FillRectangle(Fill, New Rectangle(0, 0, Width - 1, Height - 1))
            End Using

            Select Case State

                Case MouseState1.None

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#5C5C5C"))
                        ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
                    End Using

                Case MouseState1.Over

                    Using Border As New Pen(HelpersXylos.ColorFromHex("#C8C8C8"))
                        ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
                    End Using

            End Select

            Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#B6B6B6"))
                ThemeModule.G.DrawString(Value & AfterValue, TextFont, TextBrush, TextPoint)
            End Using

            Using ArrowFont As New Font("Marlett", 10), ArrowBrush As New SolidBrush(HelpersXylos.ColorFromHex("#909090"))
                ThemeModule.G.DrawString("5", ArrowFont, ArrowBrush, New Point(Width - 18, 2))
                ThemeModule.G.DrawString("6", ArrowFont, ArrowBrush, New Point(Width - 18, 10))
            End Using

        Else

            Using Fill As New SolidBrush(HelpersXylos.ColorFromHex("#282828")), Border As New Pen(HelpersXylos.ColorFromHex("#484848"))
                ThemeModule.G.FillRectangle(Fill, New Rectangle(0, 0, Width - 1, Height - 1))
                ThemeModule.G.DrawRectangle(Border, New Rectangle(0, 0, Width - 1, Height - 1))
            End Using

            Using TextBrush As New SolidBrush(HelpersXylos.ColorFromHex("#818181"))
                ThemeModule.G.DrawString(Value & AfterValue, TextFont, TextBrush, TextPoint)
            End Using

            Using ArrowFont As New Font("Marlett", 10), ArrowBrush As New SolidBrush(HelpersXylos.ColorFromHex("#707070"))
                ThemeModule.G.DrawString("5", ArrowFont, ArrowBrush, New Point(Width - 18, 2))
                ThemeModule.G.DrawString("6", ArrowFont, ArrowBrush, New Point(Width - 18, 10))
            End Using

        End If

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)

        If e.X > Width - 16 AndAlso e.Y < 11 Then

            If Not Value + Increment > Maximum Then
                Value += Increment
            Else
                Value = Maximum
            End If

        ElseIf e.X > Width - 16 AndAlso e.Y > 13 Then
            If Not Value - Increment < Minimum Then
                Value -= Increment
            Else
                Value = Minimum
            End If
        End If

        Invalidate()

        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        State = MouseState1.Over : Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        State = MouseState1.None : Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnValueChanged(e As EventArgs)
        ValueChangedThread = New Thread(AddressOf ValueChangedAnimation) With {
            .IsBackground = True}
        ValueChangedThread.Start()
        MyBase.OnValueChanged(e)
    End Sub

End Class