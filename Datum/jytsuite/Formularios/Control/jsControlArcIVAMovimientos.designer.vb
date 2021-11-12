<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsControlArcIVAMovimientos
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsControlArcIVAMovimientos))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpEquivalencia = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtHasta_2 = New System.Windows.Forms.TextBox()
        Me.txtDesde_2 = New System.Windows.Forms.TextBox()
        Me.txtMonto_2 = New System.Windows.Forms.TextBox()
        Me.txtHasta_1 = New System.Windows.Forms.TextBox()
        Me.txtDesde_1 = New System.Windows.Forms.TextBox()
        Me.txtMonto_1 = New System.Windows.Forms.TextBox()
        Me.txtHasta = New System.Windows.Forms.TextBox()
        Me.txtDesde = New System.Windows.Forms.TextBox()
        Me.txtTasa = New System.Windows.Forms.TextBox()
        Me.txtMonto = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.txtFecha = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.grpEquivalencia.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 169)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(648, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpEquivalencia
        '
        Me.grpEquivalencia.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpEquivalencia.Controls.Add(Me.txtFecha)
        Me.grpEquivalencia.Controls.Add(Me.Label6)
        Me.grpEquivalencia.Controls.Add(Me.Label5)
        Me.grpEquivalencia.Controls.Add(Me.Label4)
        Me.grpEquivalencia.Controls.Add(Me.txtHasta_2)
        Me.grpEquivalencia.Controls.Add(Me.txtDesde_2)
        Me.grpEquivalencia.Controls.Add(Me.txtMonto_2)
        Me.grpEquivalencia.Controls.Add(Me.txtHasta_1)
        Me.grpEquivalencia.Controls.Add(Me.txtDesde_1)
        Me.grpEquivalencia.Controls.Add(Me.txtMonto_1)
        Me.grpEquivalencia.Controls.Add(Me.txtHasta)
        Me.grpEquivalencia.Controls.Add(Me.txtDesde)
        Me.grpEquivalencia.Controls.Add(Me.txtTasa)
        Me.grpEquivalencia.Controls.Add(Me.txtMonto)
        Me.grpEquivalencia.Controls.Add(Me.Label3)
        Me.grpEquivalencia.Controls.Add(Me.Label2)
        Me.grpEquivalencia.Controls.Add(Me.Label1)
        Me.grpEquivalencia.Location = New System.Drawing.Point(0, 1)
        Me.grpEquivalencia.Name = "grpEquivalencia"
        Me.grpEquivalencia.Size = New System.Drawing.Size(648, 163)
        Me.grpEquivalencia.TabIndex = 80
        Me.grpEquivalencia.TabStop = False
        Me.grpEquivalencia.Text = "Impuesto al Valor Agregado "
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(557, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(79, 19)
        Me.Label6.TabIndex = 148
        Me.Label6.Text = "Monto"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(409, 50)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(129, 19)
        Me.Label5.TabIndex = 147
        Me.Label5.Text = "Hasta"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(274, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(129, 19)
        Me.Label4.TabIndex = 146
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtHasta_2
        '
        Me.txtHasta_2.BackColor = System.Drawing.Color.White
        Me.txtHasta_2.Location = New System.Drawing.Point(409, 126)
        Me.txtHasta_2.Name = "txtHasta_2"
        Me.txtHasta_2.Size = New System.Drawing.Size(129, 20)
        Me.txtHasta_2.TabIndex = 145
        Me.txtHasta_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDesde_2
        '
        Me.txtDesde_2.BackColor = System.Drawing.Color.White
        Me.txtDesde_2.Location = New System.Drawing.Point(274, 126)
        Me.txtDesde_2.Name = "txtDesde_2"
        Me.txtDesde_2.Size = New System.Drawing.Size(129, 20)
        Me.txtDesde_2.TabIndex = 144
        Me.txtDesde_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMonto_2
        '
        Me.txtMonto_2.BackColor = System.Drawing.Color.White
        Me.txtMonto_2.Location = New System.Drawing.Point(557, 126)
        Me.txtMonto_2.Name = "txtMonto_2"
        Me.txtMonto_2.Size = New System.Drawing.Size(79, 20)
        Me.txtMonto_2.TabIndex = 143
        Me.txtMonto_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtHasta_1
        '
        Me.txtHasta_1.BackColor = System.Drawing.Color.White
        Me.txtHasta_1.Location = New System.Drawing.Point(409, 100)
        Me.txtHasta_1.Name = "txtHasta_1"
        Me.txtHasta_1.Size = New System.Drawing.Size(129, 20)
        Me.txtHasta_1.TabIndex = 142
        Me.txtHasta_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDesde_1
        '
        Me.txtDesde_1.BackColor = System.Drawing.Color.White
        Me.txtDesde_1.Location = New System.Drawing.Point(274, 100)
        Me.txtDesde_1.Name = "txtDesde_1"
        Me.txtDesde_1.Size = New System.Drawing.Size(129, 20)
        Me.txtDesde_1.TabIndex = 141
        Me.txtDesde_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMonto_1
        '
        Me.txtMonto_1.BackColor = System.Drawing.Color.White
        Me.txtMonto_1.Location = New System.Drawing.Point(557, 100)
        Me.txtMonto_1.Name = "txtMonto_1"
        Me.txtMonto_1.Size = New System.Drawing.Size(79, 20)
        Me.txtMonto_1.TabIndex = 140
        Me.txtMonto_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtHasta
        '
        Me.txtHasta.BackColor = System.Drawing.Color.White
        Me.txtHasta.Location = New System.Drawing.Point(409, 74)
        Me.txtHasta.Name = "txtHasta"
        Me.txtHasta.Size = New System.Drawing.Size(129, 20)
        Me.txtHasta.TabIndex = 139
        Me.txtHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDesde
        '
        Me.txtDesde.BackColor = System.Drawing.Color.White
        Me.txtDesde.Location = New System.Drawing.Point(274, 74)
        Me.txtDesde.Name = "txtDesde"
        Me.txtDesde.Size = New System.Drawing.Size(129, 20)
        Me.txtDesde.TabIndex = 138
        Me.txtDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTasa
        '
        Me.txtTasa.BackColor = System.Drawing.Color.White
        Me.txtTasa.Location = New System.Drawing.Point(150, 28)
        Me.txtTasa.Name = "txtTasa"
        Me.txtTasa.Size = New System.Drawing.Size(79, 20)
        Me.txtTasa.TabIndex = 15
        Me.txtTasa.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMonto
        '
        Me.txtMonto.BackColor = System.Drawing.Color.White
        Me.txtMonto.Location = New System.Drawing.Point(557, 74)
        Me.txtMonto.Name = "txtMonto"
        Me.txtMonto.Size = New System.Drawing.Size(79, 20)
        Me.txtMonto.TabIndex = 14
        Me.txtMonto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(39, 50)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(105, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Fecha :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(36, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(108, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Porcentaje  :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(42, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 19)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Tasa :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(483, 166)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 88
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = Global.Datum.My.Resources.Resources.button_cancel
        Me.btnCancel.Location = New System.Drawing.Point(85, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(76, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOK.Image = Global.Datum.My.Resources.Resources.button_ok
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'txtFecha
        '
        Me.txtFecha.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFecha.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFecha.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFecha.Location = New System.Drawing.Point(150, 50)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(114, 19)
        Me.txtFecha.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFecha.TabIndex = 214
        '
        'jsControlArcIVAMovimientos
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(648, 195)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpEquivalencia)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsControlArcIVAMovimientos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimientos IVA"
        Me.grpEquivalencia.ResumeLayout(False)
        Me.grpEquivalencia.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpEquivalencia As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMonto As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtTasa As System.Windows.Forms.TextBox
    Friend WithEvents txtHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtDesde As System.Windows.Forms.TextBox
    Friend WithEvents txtHasta_2 As System.Windows.Forms.TextBox
    Friend WithEvents txtDesde_2 As System.Windows.Forms.TextBox
    Friend WithEvents txtMonto_2 As System.Windows.Forms.TextBox
    Friend WithEvents txtHasta_1 As System.Windows.Forms.TextBox
    Friend WithEvents txtDesde_1 As System.Windows.Forms.TextBox
    Friend WithEvents txtMonto_1 As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtFecha As Syncfusion.WinForms.Input.SfDateTimeEdit
End Class
