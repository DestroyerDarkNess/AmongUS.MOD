' ***********************************************************************
' Author           : Elektro
' Last Modified On : 11-10-2014
' ***********************************************************************
' <copyright file="ControlResizer.vb" company="Elektro Studios">
'     Copyright (c) Elektro Studios. All rights reserved.
' </copyright>
' ***********************************************************************

#Region " Usage Examples "

'Public Class Form1
'
'    Private Resizer As ControlResizer = ControlResizer.Empty
'
'    Private Sub InitializeResizer()
'        Me.Resizer = New ControlResizer(Button1)
'        Me.Resizer.Enabled = True
'        Me.Resizer.PixelMargin = 4
'    End Sub
'
'    Private Sub AlternateResizer()
'        Me.Resizer.Enabled = Not Resizer.Enabled
'    End Sub
'
'    Private Sub FinishResizer()
'        Me.Resizer.Dispose()
'    End Sub
'
'    Private Sub Test() Handles MyBase.Shown
'        Me.InitializeResizer()
'    End Sub
'
'End Class

#End Region

#Region " Imports "

Imports System.ComponentModel

#End Region

#Region " Control Resizer "

''' <summary>
''' Enable or disable resize at runtime on a <see cref="Control"/>.
''' </summary>
Public Class ControlResizer : Implements IDisposable

#Region " Properties "

#Region " Visible "

    ''' <summary>
    ''' Gets the associated <see cref="Control"/> used to perform resizable operations.
    ''' </summary>
    ''' <value>The control.</value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("The associated Control used to perform resizable operations.")>
    Friend ReadOnly Property Control As Control
        Get
            Return Me._ctrl
        End Get
    End Property
    ''' <summary>
    ''' The associated <see cref="Control"/> used to perform draggable operations.
    ''' </summary>
    Private WithEvents _ctrl As Control = Nothing

    ''' <summary>
    ''' Gets or sets the pixel margin required to activate resize indicators.
    ''' </summary>
    ''' <value>The pixel margin required activate resize indicators.</value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("The associated Control used to perform resizable operations.")>
    Friend Property PixelMargin As Integer = 4I

    ''' <summary>
    ''' Gets or sets a value indicating whether resize is enabled on the associated <see cref="Control"/>.
    ''' </summary>
    ''' <value><c>true</c> if resize is enabled; otherwise, <c>false</c>.</value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("A value indicating whether resize is enabled on the associated control.")>
    Friend Property Enabled As Boolean = True

    ''' <summary>
    ''' Represents a <see cref="T:ControlResizer"/> instance that is <c>Nothing</c>.
    ''' </summary>
    ''' <value><c>Nothing</c></value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("Represents a ControlResizer instance that is Nothing.")>
    Public Shared ReadOnly Property Empty As ControlResizer
        Get
            Return Nothing
        End Get
    End Property

#End Region

#Region " Hidden "

    ''' <summary>
    ''' Gets or sets a value indicating whether the left mouse button is down.
    ''' </summary>
    ''' <value><c>true</c> if left mouse button is down; otherwise, <c>false</c>.</value>
    Private Property IsLeftMouseButtonDown As Boolean = False

    ''' <summary>
    ''' Gets or sets the current active edge.
    ''' </summary>
    ''' <value>The current active edge.</value>
    Private Property ActiveEdge As Edges = Edges.None

    ''' <summary>
    ''' Gets or sets the old control's cursor to restore it after resizing.
    ''' </summary>
    ''' <value>The old control's cursor.</value>
    Private Property oldCursor As Cursor = Nothing

#End Region

#End Region

#Region " Enumerations "

    ''' <summary>
    ''' Contains the Edges.
    ''' </summary>
    Private Enum Edges As Integer

        ''' <summary>
        ''' Any edge.
        ''' </summary>
        None = 0I

        ''' <summary>
        ''' Left edge.
        ''' </summary>
        Left = 1I

        ''' <summary>
        ''' Right edge.
        ''' </summary>
        Right = 2I

        ''' <summary>
        ''' Top edge.
        ''' </summary>
        Top = 3I

        ''' <summary>
        ''' Bottom edge.
        ''' </summary>
        Bottom = 4I

        ''' <summary>
        ''' Top-Left edge.
        ''' </summary>
        TopLeft = 5I

        ''' <summary>
        ''' Top-Right edge.
        ''' </summary>
        TopRight = 6I

        ''' <summary>
        ''' Bottom-Left edge.
        ''' </summary>
        BottomLeft = 7I

        ''' <summary>
        ''' Bottom-Right edge.
        ''' </summary>
        BottomRight = 8I

    End Enum

#End Region

#Region " Constructors "

    ''' <summary>
    ''' Prevents a default instance of the <see cref="ControlResizer"/> class from being created.
    ''' </summary>
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="ControlResizer"/> class.
    ''' </summary>
    ''' <param name="ctrl">The control.</param>
    Public Sub New(ByVal ctrl As Control)

        Me._ctrl = ctrl

    End Sub

#End Region

#Region " Event Handlers "

    ''' <summary>
    ''' Handles the MouseEnter event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
    Handles _ctrl.MouseEnter

        Me.oldCursor = Me._ctrl.Cursor

    End Sub

    ''' <summary>
    ''' Handles the MouseLeave event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
    Handles _ctrl.MouseLeave

        Me.ActiveEdge = Edges.None
        Me._ctrl.Cursor = Me.oldCursor

    End Sub

    ''' <summary>
    ''' Handles the MouseDown event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) _
    Handles _ctrl.MouseDown

        Me.IsLeftMouseButtonDown = (e.Button = MouseButtons.Left)

    End Sub

    ''' <summary> 
    ''' Handles the MouseUp event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) _
    Handles _ctrl.MouseUp

        Me.IsLeftMouseButtonDown = False

    End Sub

    ''' <summary>
    ''' Handles the MouseMove event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) _
    Handles _ctrl.MouseMove

        If Not Me.Enabled Then
            Exit Sub

        ElseIf (Me.IsLeftMouseButtonDown) AndAlso Not (Me.ActiveEdge = Edges.None) Then
            Me.SetControlBounds(e)

        Else
            Me.SetActiveEdge(e)
            Me.SetSizeCursor()

        End If

    End Sub

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Sets the active edge.
    ''' </summary>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub SetActiveEdge(ByVal e As MouseEventArgs)

        Select Case True

            ' Top-Left Corner
            Case e.X <= (Me.PixelMargin * 2) AndAlso
                 e.Y <= (Me.PixelMargin * 2)

                Me.ActiveEdge = Edges.TopLeft

                ' TopRight Corner
            Case e.X > Me._ctrl.Width - (Me.PixelMargin * 2) AndAlso
                 e.Y <= (Me.PixelMargin * 2)

                Me.ActiveEdge = Edges.TopRight

                ' Bottom-Left Corner
            Case (e.X <= Me.PixelMargin * 2) AndAlso
                 (e.Y > Me._ctrl.Height - (Me.PixelMargin * 2))

                Me.ActiveEdge = Edges.BottomLeft

                ' Bottom-Right Corner
            Case (e.X > Me._ctrl.Width - (Me.PixelMargin * 2) - 1) AndAlso
                 (e.Y > Me._ctrl.Height - (Me.PixelMargin * 2))

                Me.ActiveEdge = Edges.BottomRight


                ' Left Edge
            Case e.X <= Me.PixelMargin
                Me.ActiveEdge = Edges.Left

                ' Right Edge
            Case e.X > Me._ctrl.Width - (Me.PixelMargin + 1)
                Me.ActiveEdge = Edges.Right

                ' Top Edge
            Case e.Y <= Me.PixelMargin
                Me.ActiveEdge = Edges.Top

                ' Bottom Edge
            Case e.Y > Me._ctrl.Height - (Me.PixelMargin + 1)
                Me.ActiveEdge = Edges.Bottom

            Case Else ' Any Edge
                Me.ActiveEdge = Edges.None

        End Select

    End Sub

    ''' <summary>
    ''' Sets the size cursor.
    ''' </summary>
    Private Sub SetSizeCursor()

        Select Case Me.ActiveEdge

            Case Edges.Left
                Me._ctrl.Cursor = Cursors.SizeWE

            Case Edges.Right
                Me._ctrl.Cursor = Cursors.SizeWE

            Case Edges.Top
                Me._ctrl.Cursor = Cursors.SizeNS

            Case Edges.Bottom
                Me._ctrl.Cursor = Cursors.SizeNS

            Case Edges.TopLeft
                Me._ctrl.Cursor = Cursors.SizeNWSE

            Case Edges.TopRight
                Me._ctrl.Cursor = Cursors.SizeNESW

            Case Edges.BottomLeft
                Me._ctrl.Cursor = Cursors.SizeNESW

            Case Edges.BottomRight
                Me._ctrl.Cursor = Cursors.SizeNWSE

            Case Edges.None
                If Me.oldCursor IsNot Nothing Then
                    Me._ctrl.Cursor = Me.oldCursor
                End If

        End Select

    End Sub

    ''' <summary>
    ''' Sets the control bounds.
    ''' </summary>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub SetControlBounds(ByVal e As MouseEventArgs)

        If Me._ctrl.Size.Width = Me._ctrl.MinimumSize.Width Then
            ' Exit Sub
        Else
            Debug.WriteLine(Me._ctrl.Size.ToString)
        End If

        Me._ctrl.SuspendLayout()

        Select Case Me.ActiveEdge

            Case Edges.Left
                If Not Me._ctrl.Width - e.X < (Me._ctrl.MinimumSize.Width) Then
                    Me._ctrl.SetBounds(x:=Me._ctrl.Left + e.X,
                                       y:=Me._ctrl.Top,
                                       width:=Me._ctrl.Width - e.X,
                                       height:=Me._ctrl.Height)
                End If

            Case Edges.Right
                Me._ctrl.SetBounds(x:=Me._ctrl.Left,
                                   y:=Me._ctrl.Top,
                                   width:=Me._ctrl.Width - (Me._ctrl.Width - e.X),
                                   height:=Me._ctrl.Height)

            Case Edges.Top
                If Not Me._ctrl.Height - e.Y < (Me._ctrl.MinimumSize.Height) Then
                    Me._ctrl.SetBounds(x:=Me._ctrl.Left,
                                       y:=Me._ctrl.Top + e.Y,
                                       width:=Me._ctrl.Width,
                                       height:=Me._ctrl.Height - e.Y)
                End If

            Case Edges.Bottom
                Me._ctrl.SetBounds(x:=Me._ctrl.Left,
                                   y:=Me._ctrl.Top,
                                   width:=Me._ctrl.Width,
                                   height:=Me._ctrl.Height - (Me._ctrl.Height - e.Y))

            Case Edges.TopLeft
                Me._ctrl.SetBounds(x:=If(Not Me._ctrl.Width - e.X < (Me._ctrl.MinimumSize.Width),
                                         Me._ctrl.Left + e.X,
                                         Me._ctrl.Left),
                                   y:=If(Not Me._ctrl.Height - e.Y < (Me._ctrl.MinimumSize.Height),
                                         Me._ctrl.Top + e.Y,
                                         Me._ctrl.Top),
                                   width:=If(Not Me._ctrl.Width - e.X < (Me._ctrl.MinimumSize.Width),
                                             Me._ctrl.Width - e.X,
                                             Me._ctrl.Width),
                                   height:=If(Not Me._ctrl.Height - e.Y < (Me._ctrl.MinimumSize.Height),
                                              Me._ctrl.Height - e.Y,
                                              Me._ctrl.Height))

            Case Edges.TopRight
                Me._ctrl.SetBounds(x:=Me._ctrl.Left,
                                   y:=If(Not Me._ctrl.Height - e.Y < (Me._ctrl.MinimumSize.Height),
                                         Me._ctrl.Top + e.Y,
                                         Me._ctrl.Top),
                                   width:=Me._ctrl.Width - (Me._ctrl.Width - e.X),
                                   height:=If(Not Me._ctrl.Height - e.Y < (Me._ctrl.MinimumSize.Height),
                                              Me._ctrl.Height - e.Y,
                                              Me._ctrl.Height))

            Case Edges.BottomLeft
                Me._ctrl.SetBounds(x:=If(Not Me._ctrl.Width - e.X < (Me._ctrl.MinimumSize.Width),
                                         Me._ctrl.Left + e.X,
                                         Me._ctrl.Left),
                                   y:=Me._ctrl.Top,
                                   width:=If(Not Me._ctrl.Width - e.X < (Me._ctrl.MinimumSize.Width),
                                             Me._ctrl.Width - e.X,
                                             Me._ctrl.Width),
                                   height:=Me._ctrl.Height - (Me._ctrl.Height - e.Y))

            Case Edges.BottomRight
                Me._ctrl.SetBounds(x:=Me._ctrl.Left,
                                   y:=Me._ctrl.Top,
                                   width:=Me._ctrl.Width - (Me._ctrl.Width - e.X),
                                   height:=Me._ctrl.Height - (Me._ctrl.Height - e.Y))

        End Select

        Me._ctrl.ResumeLayout()

    End Sub

#End Region

#Region " Hidden Methods "

    ''' <summary>
    ''' Serves as a hash function for a particular type.
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Sub GetHashCode()
    End Sub

    ''' <summary>
    ''' Gets the System.Type of the current instance.
    ''' </summary>
    ''' <returns>The exact runtime type of the current instance.</returns>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Function [GetType]()
        Return Me.GetType
    End Function

    ''' <summary>
    ''' Determines whether the specified System.Object instances are considered equal.
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Sub Equals()
    End Sub

    ''' <summary>
    ''' Determines whether the specified System.Object instances are the same instance.
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Private Shadows Sub ReferenceEquals()
    End Sub

    ''' <summary>
    ''' Returns a String that represents the current object.
    ''' </summary>
    <EditorBrowsable(EditorBrowsableState.Never)>
    Public Shadows Sub ToString()
    End Sub

#End Region

#Region " IDisposable "

    ''' <summary>
    ''' To detect redundant calls when disposing.
    ''' </summary>
    Private IsDisposed As Boolean = False

    ''' <summary>
    ''' Prevent calls to methods after disposing.
    ''' </summary>
    ''' <exception cref="System.ObjectDisposedException"></exception>
    Private Sub DisposedCheck()

        If Me.IsDisposed Then
            Throw New ObjectDisposedException(Me.GetType().FullName)
        End If

    End Sub

    ''' <summary>
    ''' Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    ''' <summary>
    ''' Releases unmanaged and - optionally - managed resources.
    ''' </summary>
    ''' <param name="IsDisposing">
    ''' <c>true</c> to release both managed and unmanaged resources; 
    ''' <c>false</c> to release only unmanaged resources.
    ''' </param>
    Protected Sub Dispose(ByVal IsDisposing As Boolean)

        If Not Me.IsDisposed Then

            If IsDisposing Then

                With Me._ctrl

                    If Not .IsDisposed AndAlso Not .Disposing Then

                        RemoveHandler .MouseEnter, AddressOf ctrl_MouseEnter
                        RemoveHandler .MouseLeave, AddressOf ctrl_MouseLeave
                        RemoveHandler .MouseDown, AddressOf ctrl_MouseDown
                        RemoveHandler .MouseMove, AddressOf ctrl_MouseMove
                        RemoveHandler .MouseUp, AddressOf ctrl_MouseUp

                    End If

                End With ' Me._ctrl

                With Me

                    .Enabled = False
                    .oldCursor = Nothing
                    ._ctrl = Nothing

                End With ' Me

            End If ' IsDisposing

        End If ' Not Me.IsDisposed

        Me.IsDisposed = True

    End Sub

#End Region

End Class

#End Region