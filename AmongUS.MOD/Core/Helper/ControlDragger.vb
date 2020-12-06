' ***********************************************************************
' Author           : Elektro
' Last Modified On : 11-10-2014
' ***********************************************************************
' <copyright file="ControlDragger.vb" company="Elektro Studios">
'     Copyright (c) Elektro Studios. All rights reserved.
' </copyright>
' ***********************************************************************

#Region " Usage Examples "

'Public Class Form1
'
'    Private Dragger As ControlDragger = ControlDragger.Empty
'
'    Private Sub InitializeDrag()
'        Me.Dragger = New ControlDragger(Button1)
'        Me.Dragger.Cursor = Cursors.SizeAll
'        Me.Dragger.Enabled = True
'    End Sub
'
'    Private Sub AlternateDrag()
'        Dragger.Enabled = Not Dragger.Enabled
'    End Sub
'
'    Private Sub FinishDrag()
'        Dragger.Dispose()
'    End Sub
'
'    Private Sub Test() Handles MyBase.Shown
'        Me.InitializeDrag()
'    End Sub
'
'End Class

#End Region

#Region " Imports "

Imports System.ComponentModel

#End Region

#Region " Control Dragger "

''' <summary>
''' Enable or disable drag at runtime on a <see cref="Control"/>.
''' </summary>
Friend NotInheritable Class ControlDragger : Implements IDisposable

#Region " Properties "

    ''' <summary>
    ''' Gets the associated <see cref="Control"/> used to perform draggable operations.
    ''' </summary>
    ''' <value>The control.</value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("The associated Control used to perform draggable operations.")>
    Friend ReadOnly Property Control As Control
        Get
            Return Me._ctrl
        End Get
    End Property
    ''' <summary>
    ''' The associated <see cref="Control"/> used to perform draggable operations.
    ''' </summary>
    Private WithEvents _ctrl As Control = Nothing
    Private WithEvents _ctrl2 As Control = Nothing

    ''' <summary>
    ''' Represents a <see cref="T:ControlDragger"/> instance that is <c>Nothing</c>.
    ''' </summary>
    ''' <value><c>Nothing</c></value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("Represents a ControlDragger instance that is Nothing.")>
    Public Shared ReadOnly Property Empty As ControlDragger
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets a value indicating whether drag is enabled on the associated <see cref="Control"/>.
    ''' </summary>
    ''' <value><c>true</c> if drag is enabled; otherwise, <c>false</c>.</value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("A value indicating whether drag is enabled on the associated control.")>
    Friend Property Enabled As Boolean = True

    ''' <summary>
    ''' Gets or sets the <see cref="Cursor"/> used to drag the associated <see cref="Control"/>.
    ''' </summary>
    ''' <value>The <see cref="Cursor"/>.</value>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Description("The Cursor used to drag the associated Control")>
    Friend Property Cursor As Cursor = Cursors.Default

    ''' <summary>
    ''' A <see cref="T:ControlDragger"/> instance instance containing the draggable information of the associated <see cref="Control"/>.
    ''' </summary>
    ''' <value>The draggable information.</value>
    <EditorBrowsable(EditorBrowsableState.Never)>
    <Description("A ControlDragger instance instance containing the draggable information of the associated Control.")>
    Private Property DragInfo As ControlDragger = ControlDragger.Empty

    ''' <summary>
    ''' Gets or sets the initial mouse coordinates, normally <see cref="Control.MousePosition"/>.
    ''' </summary>
    ''' <value>The initial mouse coordinates.</value>
    <EditorBrowsable(EditorBrowsableState.Never)>
    <Description("The initial mouse coordinates, normally 'Control.MousePosition'")>
    Private Property InitialMouseCoords As Point = Point.Empty

    ''' <summary>
    ''' Gets or sets the initial <see cref="Control"/> location, normally <see cref="Control.Location"/>.
    ''' </summary>
    ''' <value>The initial location.</value>
    <EditorBrowsable(EditorBrowsableState.Never)>
    <Description("The initial Control location, normally 'Control.Location'")>
    Private Property InitialLocation As Point = Point.Empty

    ''' <summary>
    ''' Gets or sets the old control's cursor to restore it after dragging.
    ''' </summary>
    ''' <value>The old control's cursor.</value>
    <EditorBrowsable(EditorBrowsableState.Never)>
    <Description("The old control's cursor to restore it after dragging.")>
    Private Property oldCursor As Cursor = Nothing

#End Region

#Region " Constructors "

    ''' <summary>
    ''' Prevents a default instance of the <see cref="ControlDragger"/> class from being created.
    ''' </summary>
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="ControlDragger"/> class.
    ''' </summary>
    ''' <param name="ctrl">The <see cref="Control"/> used to perform draggable operations.</param>
    Public Sub New(ByVal ctrl As Control, Optional ByVal ctrlParent As Control = Nothing)
        Me._ctrl = ctrl
        Me._ctrl2 = ctrlParent
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <see cref="ControlDragger"/> class.
    ''' </summary>
    ''' <param name="mouseCoordinates">The current mouse coordinates.</param>
    ''' <param name="location">The current location.</param>
    Private Sub New(ByVal mouseCoordinates As Point, ByVal location As Point)

        Me.InitialMouseCoords = mouseCoordinates
        Me.InitialLocation = location

    End Sub

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Return the new location.
    ''' </summary>
    ''' <param name="mouseCoordinates">The current mouse coordinates.</param>
    ''' <returns>The new location.</returns>
    Private Function GetNewLocation(ByVal mouseCoordinates As Point) As Point

        Return New Point(InitialLocation.X + (mouseCoordinates.X - InitialMouseCoords.X),
                         InitialLocation.Y + (mouseCoordinates.Y - InitialMouseCoords.Y))

    End Function

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

#Region " Event Handlers "

    ''' <summary>
    ''' Handles the MouseEnter event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) _
    Handles _ctrl.MouseEnter

        Me.oldCursor = Me._ctrl.Cursor

        If Me.Enabled Then

            Me._ctrl.Cursor = Me.Cursor
            ' Me._ctrl.BringToFront()

        End If

    End Sub

    ''' <summary>
    ''' Handles the MouseLeave event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) _
    Handles _ctrl.MouseLeave

        Me._ctrl.Cursor = Me.oldCursor

    End Sub

    ''' <summary>
    ''' Handles the MouseDown event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) _
    Handles _ctrl.MouseDown

        If Me.Enabled Then
            If _ctrl2 IsNot Nothing Then
                Me.DragInfo = New ControlDragger(Control.MousePosition, Me._ctrl2.Location)
            Else
                Me.DragInfo = New ControlDragger(Control.MousePosition, Me._ctrl.Location)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Handles the MouseMove event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) _
    Handles _ctrl.MouseMove

        If Me.Enabled AndAlso (Me.DragInfo IsNot ControlDragger.Empty) Then
            If _ctrl2 IsNot Nothing Then
                Me._ctrl2.Location = New Point(Me.DragInfo.GetNewLocation(Control.MousePosition))
            Else
                Me._ctrl.Location = New Point(Me.DragInfo.GetNewLocation(Control.MousePosition))
            End If
        End If

    End Sub

    ''' <summary>
    ''' Handles the MouseUp event of the control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
    Private Sub ctrl_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) _
    Handles _ctrl.MouseUp

        Me.DragInfo = ControlDragger.Empty

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
                    .DragInfo = ControlDragger.Empty
                    .InitialMouseCoords = Point.Empty
                    .InitialLocation = Point.Empty
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