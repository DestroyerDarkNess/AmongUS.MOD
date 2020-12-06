<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RadarInfo
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PlayerCheats = New System.Windows.Forms.Timer(Me.components)
        Me.PanelFX1 = New AmongUS.[MOD].PanelFX()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GunaSeparator1 = New Guna.UI.WinForms.GunaSeparator()
        Me.GunaVSeparator1 = New Guna.UI.WinForms.GunaVSeparator()
        Me.GunaPanel2 = New Guna.UI.WinForms.GunaPanel()
        Me.GunaLabel18 = New Guna.UI.WinForms.GunaLabel()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GunaDragControl1 = New Guna.UI.WinForms.GunaDragControl(Me.components)
        Me.GunaDragControl2 = New Guna.UI.WinForms.GunaDragControl(Me.components)
        Me.GunaDragControl3 = New Guna.UI.WinForms.GunaDragControl(Me.components)
        Me.PanelFX1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GunaPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PlayerCheats
        '
        Me.PlayerCheats.Interval = 1
        '
        'PanelFX1
        '
        Me.PanelFX1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelFX1.Controls.Add(Me.Panel1)
        Me.PanelFX1.Controls.Add(Me.GunaPanel2)
        Me.PanelFX1.Location = New System.Drawing.Point(2, 8)
        Me.PanelFX1.Name = "PanelFX1"
        Me.PanelFX1.Size = New System.Drawing.Size(222, 211)
        Me.PanelFX1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(14, Byte), Integer), CType(CType(14, Byte), Integer), CType(CType(14, Byte), Integer))
        Me.Panel1.Controls.Add(Me.GunaSeparator1)
        Me.Panel1.Controls.Add(Me.GunaVSeparator1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 19)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(220, 190)
        Me.Panel1.TabIndex = 4
        '
        'GunaSeparator1
        '
        Me.GunaSeparator1.LineColor = System.Drawing.Color.Silver
        Me.GunaSeparator1.Location = New System.Drawing.Point(0, 95)
        Me.GunaSeparator1.Name = "GunaSeparator1"
        Me.GunaSeparator1.Size = New System.Drawing.Size(222, 1)
        Me.GunaSeparator1.TabIndex = 1
        '
        'GunaVSeparator1
        '
        Me.GunaVSeparator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.GunaVSeparator1.LineColor = System.Drawing.Color.Silver
        Me.GunaVSeparator1.Location = New System.Drawing.Point(110, 0)
        Me.GunaVSeparator1.Name = "GunaVSeparator1"
        Me.GunaVSeparator1.Size = New System.Drawing.Size(1, 189)
        Me.GunaVSeparator1.TabIndex = 0
        '
        'GunaPanel2
        '
        Me.GunaPanel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(170, Byte), Integer))
        Me.GunaPanel2.Controls.Add(Me.GunaLabel18)
        Me.GunaPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GunaPanel2.Location = New System.Drawing.Point(0, 0)
        Me.GunaPanel2.Name = "GunaPanel2"
        Me.GunaPanel2.Size = New System.Drawing.Size(220, 19)
        Me.GunaPanel2.TabIndex = 3
        '
        'GunaLabel18
        '
        Me.GunaLabel18.AutoSize = True
        Me.GunaLabel18.BackColor = System.Drawing.Color.Transparent
        Me.GunaLabel18.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GunaLabel18.ForeColor = System.Drawing.Color.White
        Me.GunaLabel18.Location = New System.Drawing.Point(2, 3)
        Me.GunaLabel18.Name = "GunaLabel18"
        Me.GunaLabel18.Size = New System.Drawing.Size(37, 13)
        Me.GunaLabel18.TabIndex = 1
        Me.GunaLabel18.Text = "Radar"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'GunaDragControl1
        '
        Me.GunaDragControl1.TargetControl = Me.Panel1
        '
        'GunaDragControl2
        '
        Me.GunaDragControl2.TargetControl = Me.GunaPanel2
        '
        'GunaDragControl3
        '
        Me.GunaDragControl3.TargetControl = Me.GunaLabel18
        '
        'RadarInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(225, 226)
        Me.Controls.Add(Me.PanelFX1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "RadarInfo"
        Me.Opacity = 0.9R
        Me.ShowInTaskbar = False
        Me.Text = "RadarInfo"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.Black
        Me.PanelFX1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.GunaPanel2.ResumeLayout(False)
        Me.GunaPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelFX1 As AmongUS.MOD.PanelFX
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GunaPanel2 As Guna.UI.WinForms.GunaPanel
    Friend WithEvents GunaLabel18 As Guna.UI.WinForms.GunaLabel
    Friend WithEvents PlayerCheats As System.Windows.Forms.Timer
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents GunaSeparator1 As Guna.UI.WinForms.GunaSeparator
    Friend WithEvents GunaVSeparator1 As Guna.UI.WinForms.GunaVSeparator
    Friend WithEvents GunaDragControl1 As Guna.UI.WinForms.GunaDragControl
    Friend WithEvents GunaDragControl2 As Guna.UI.WinForms.GunaDragControl
    Friend WithEvents GunaDragControl3 As Guna.UI.WinForms.GunaDragControl
End Class
