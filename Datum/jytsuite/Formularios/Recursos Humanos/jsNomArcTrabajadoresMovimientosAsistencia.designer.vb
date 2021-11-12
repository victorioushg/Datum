<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsNomArcTrabajadoresMovimientosAsistencia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsNomArcTrabajadoresMovimientosAsistencia))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.grpConstante = New System.Windows.Forms.GroupBox()
        Me.txtFecha = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.mskTotal = New System.Windows.Forms.MaskedTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpAsiste = New System.Windows.Forms.GroupBox()
        Me.txtFechaSalida = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.txtFechaRetorno = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.txtFechaDescanso = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.txtFechaEntrada = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.cmbTipoDia = New System.Windows.Forms.ComboBox()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.grpConstante.SuspendLayout()
        Me.grpAsiste.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 295)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(502, 30)
        Me.lblInfo.TabIndex = 28
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fecha "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(7, 249)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 19)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Día :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpConstante
        '
        Me.grpConstante.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpConstante.Controls.Add(Me.txtFecha)
        Me.grpConstante.Controls.Add(Me.Label3)
        Me.grpConstante.Controls.Add(Me.mskTotal)
        Me.grpConstante.Controls.Add(Me.Label2)
        Me.grpConstante.Controls.Add(Me.grpAsiste)
        Me.grpConstante.Controls.Add(Me.cmbTipoDia)
        Me.grpConstante.Controls.Add(Me.Label7)
        Me.grpConstante.Location = New System.Drawing.Point(0, 0)
        Me.grpConstante.Name = "grpConstante"
        Me.grpConstante.Size = New System.Drawing.Size(502, 292)
        Me.grpConstante.TabIndex = 31
        Me.grpConstante.TabStop = False
        Me.grpConstante.Text = "Registro de Asistencia"
        '
        'txtFecha
        '
        Me.txtFecha.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFecha.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFecha.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFecha.Location = New System.Drawing.Point(116, 32)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(114, 19)
        Me.txtFecha.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFecha.TabIndex = 214
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(46, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 19)
        Me.Label3.TabIndex = 143
        Me.Label3.Text = "Fecha "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mskTotal
        '
        Me.mskTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskTotal.Location = New System.Drawing.Point(404, 249)
        Me.mskTotal.Mask = "00:00"
        Me.mskTotal.Name = "mskTotal"
        Me.mskTotal.Size = New System.Drawing.Size(42, 20)
        Me.mskTotal.TabIndex = 140
        Me.mskTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mskTotal.ValidatingType = GetType(Date)
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(297, 242)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 32)
        Me.Label2.TabIndex = 139
        Me.Label2.Text = "Total"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpAsiste
        '
        Me.grpAsiste.Controls.Add(Me.Label6)
        Me.grpAsiste.Controls.Add(Me.Label5)
        Me.grpAsiste.Controls.Add(Me.Label4)
        Me.grpAsiste.Controls.Add(Me.txtFechaSalida)
        Me.grpAsiste.Controls.Add(Me.txtFechaRetorno)
        Me.grpAsiste.Controls.Add(Me.txtFechaDescanso)
        Me.grpAsiste.Controls.Add(Me.txtFechaEntrada)
        Me.grpAsiste.Controls.Add(Me.Label1)
        Me.grpAsiste.Controls.Add(Me.Label18)
        Me.grpAsiste.Location = New System.Drawing.Point(66, 58)
        Me.grpAsiste.Name = "grpAsiste"
        Me.grpAsiste.Size = New System.Drawing.Size(398, 183)
        Me.grpAsiste.TabIndex = 138
        Me.grpAsiste.TabStop = False
        '
        'txtFechaSalida
        '
        Me.txtFechaSalida.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFechaSalida.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFechaSalida.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFechaSalida.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaSalida.Format = "dd-MM-yyyy hh:mm"
        Me.txtFechaSalida.Location = New System.Drawing.Point(130, 129)
        Me.txtFechaSalida.Name = "txtFechaSalida"
        Me.txtFechaSalida.Size = New System.Drawing.Size(250, 19)
        Me.txtFechaSalida.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFechaSalida.TabIndex = 217
        '
        'txtFechaRetorno
        '
        Me.txtFechaRetorno.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFechaRetorno.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFechaRetorno.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFechaRetorno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaRetorno.Format = "dd-MM-yyyy hh:mm"
        Me.txtFechaRetorno.Location = New System.Drawing.Point(130, 104)
        Me.txtFechaRetorno.Name = "txtFechaRetorno"
        Me.txtFechaRetorno.Size = New System.Drawing.Size(250, 19)
        Me.txtFechaRetorno.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFechaRetorno.TabIndex = 216
        '
        'txtFechaDescanso
        '
        Me.txtFechaDescanso.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFechaDescanso.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFechaDescanso.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFechaDescanso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaDescanso.Format = "dd-MM-yyyy hh:mm"
        Me.txtFechaDescanso.Location = New System.Drawing.Point(130, 79)
        Me.txtFechaDescanso.Name = "txtFechaDescanso"
        Me.txtFechaDescanso.Size = New System.Drawing.Size(250, 19)
        Me.txtFechaDescanso.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFechaDescanso.TabIndex = 215
        '
        'txtFechaEntrada
        '
        Me.txtFechaEntrada.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFechaEntrada.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFechaEntrada.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFechaEntrada.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaEntrada.Format = "dd-MM-yyyy hh:mm"
        Me.txtFechaEntrada.Location = New System.Drawing.Point(130, 54)
        Me.txtFechaEntrada.Name = "txtFechaEntrada"
        Me.txtFechaEntrada.Size = New System.Drawing.Size(250, 19)
        Me.txtFechaEntrada.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFechaEntrada.TabIndex = 214
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(24, 54)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(100, 19)
        Me.Label18.TabIndex = 131
        Me.Label18.Text = "Entrada"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbTipoDia
        '
        Me.cmbTipoDia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoDia.FormattingEnabled = True
        Me.cmbTipoDia.Location = New System.Drawing.Point(66, 247)
        Me.cmbTipoDia.Name = "cmbTipoDia"
        Me.cmbTipoDia.Size = New System.Drawing.Size(172, 21)
        Me.cmbTipoDia.TabIndex = 25
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(336, 295)
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
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(24, 79)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 19)
        Me.Label4.TabIndex = 218
        Me.Label4.Text = "Descanso"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(24, 104)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 19)
        Me.Label5.TabIndex = 219
        Me.Label5.Text = "Retorno"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(24, 129)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 19)
        Me.Label6.TabIndex = 220
        Me.Label6.Text = "Salida"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'jsNomArcTrabajadoresMovimientosAsistencia
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(502, 325)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpConstante)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsNomArcTrabajadoresMovimientosAsistencia"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento Asistencia de trabajador"
        Me.grpConstante.ResumeLayout(False)
        Me.grpConstante.PerformLayout()
        Me.grpAsiste.ResumeLayout(False)
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents grpConstante As System.Windows.Forms.GroupBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cmbTipoDia As System.Windows.Forms.ComboBox
    Friend WithEvents grpAsiste As System.Windows.Forms.GroupBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents mskTotal As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFecha As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents txtFechaSalida As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents txtFechaRetorno As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents txtFechaDescanso As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents txtFechaEntrada As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
End Class
