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
        Me.txtFechaEntrada = New System.Windows.Forms.TextBox()
        Me.grpConstante = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnFecha = New System.Windows.Forms.Button()
        Me.txtFecha = New System.Windows.Forms.TextBox()
        Me.mskTotal = New System.Windows.Forms.MaskedTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpAsiste = New System.Windows.Forms.GroupBox()
        Me.btnFechaSalida = New System.Windows.Forms.Button()
        Me.btnFechaRetorno = New System.Windows.Forms.Button()
        Me.btnFechaDescanso = New System.Windows.Forms.Button()
        Me.txtFechaSalida = New System.Windows.Forms.TextBox()
        Me.txtFechaRetorno = New System.Windows.Forms.TextBox()
        Me.txtFechaDescanso = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.btnFechaEntrada = New System.Windows.Forms.Button()
        Me.mskRetorno = New System.Windows.Forms.MaskedTextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.mskDescanso = New System.Windows.Forms.MaskedTextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.mskSalida = New System.Windows.Forms.MaskedTextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.mskEntrada = New System.Windows.Forms.MaskedTextBox()
        Me.cmbTipoDia = New System.Windows.Forms.ComboBox()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
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
        Me.Label1.Location = New System.Drawing.Point(47, 26)
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
        'txtFechaEntrada
        '
        Me.txtFechaEntrada.Location = New System.Drawing.Point(47, 51)
        Me.txtFechaEntrada.Name = "txtFechaEntrada"
        Me.txtFechaEntrada.Size = New System.Drawing.Size(94, 20)
        Me.txtFechaEntrada.TabIndex = 12
        Me.txtFechaEntrada.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpConstante
        '
        Me.grpConstante.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpConstante.Controls.Add(Me.Label3)
        Me.grpConstante.Controls.Add(Me.btnFecha)
        Me.grpConstante.Controls.Add(Me.txtFecha)
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
        'btnFecha
        '
        Me.btnFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFecha.Location = New System.Drawing.Point(212, 32)
        Me.btnFecha.Name = "btnFecha"
        Me.btnFecha.Size = New System.Drawing.Size(25, 20)
        Me.btnFecha.TabIndex = 142
        Me.btnFecha.Text = "•••"
        Me.btnFecha.UseVisualStyleBackColor = True
        '
        'txtFecha
        '
        Me.txtFecha.Location = New System.Drawing.Point(112, 32)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(94, 20)
        Me.txtFecha.TabIndex = 141
        Me.txtFecha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'mskTotal
        '
        Me.mskTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskTotal.Location = New System.Drawing.Point(358, 251)
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
        Me.Label2.Location = New System.Drawing.Point(251, 244)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 32)
        Me.Label2.TabIndex = 139
        Me.Label2.Text = "Total"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpAsiste
        '
        Me.grpAsiste.Controls.Add(Me.btnFechaSalida)
        Me.grpAsiste.Controls.Add(Me.btnFechaRetorno)
        Me.grpAsiste.Controls.Add(Me.btnFechaDescanso)
        Me.grpAsiste.Controls.Add(Me.txtFechaSalida)
        Me.grpAsiste.Controls.Add(Me.txtFechaRetorno)
        Me.grpAsiste.Controls.Add(Me.txtFechaDescanso)
        Me.grpAsiste.Controls.Add(Me.Label21)
        Me.grpAsiste.Controls.Add(Me.btnFechaEntrada)
        Me.grpAsiste.Controls.Add(Me.mskRetorno)
        Me.grpAsiste.Controls.Add(Me.Label20)
        Me.grpAsiste.Controls.Add(Me.txtFechaEntrada)
        Me.grpAsiste.Controls.Add(Me.Label1)
        Me.grpAsiste.Controls.Add(Me.mskDescanso)
        Me.grpAsiste.Controls.Add(Me.Label19)
        Me.grpAsiste.Controls.Add(Me.mskSalida)
        Me.grpAsiste.Controls.Add(Me.Label18)
        Me.grpAsiste.Controls.Add(Me.mskEntrada)
        Me.grpAsiste.Location = New System.Drawing.Point(66, 58)
        Me.grpAsiste.Name = "grpAsiste"
        Me.grpAsiste.Size = New System.Drawing.Size(398, 183)
        Me.grpAsiste.TabIndex = 138
        Me.grpAsiste.TabStop = False
        '
        'btnFechaSalida
        '
        Me.btnFechaSalida.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechaSalida.Location = New System.Drawing.Point(147, 147)
        Me.btnFechaSalida.Name = "btnFechaSalida"
        Me.btnFechaSalida.Size = New System.Drawing.Size(25, 20)
        Me.btnFechaSalida.TabIndex = 143
        Me.btnFechaSalida.Text = "•••"
        Me.btnFechaSalida.UseVisualStyleBackColor = True
        '
        'btnFechaRetorno
        '
        Me.btnFechaRetorno.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechaRetorno.Location = New System.Drawing.Point(147, 115)
        Me.btnFechaRetorno.Name = "btnFechaRetorno"
        Me.btnFechaRetorno.Size = New System.Drawing.Size(25, 20)
        Me.btnFechaRetorno.TabIndex = 142
        Me.btnFechaRetorno.Text = "•••"
        Me.btnFechaRetorno.UseVisualStyleBackColor = True
        '
        'btnFechaDescanso
        '
        Me.btnFechaDescanso.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechaDescanso.Location = New System.Drawing.Point(147, 83)
        Me.btnFechaDescanso.Name = "btnFechaDescanso"
        Me.btnFechaDescanso.Size = New System.Drawing.Size(25, 20)
        Me.btnFechaDescanso.TabIndex = 141
        Me.btnFechaDescanso.Text = "•••"
        Me.btnFechaDescanso.UseVisualStyleBackColor = True
        '
        'txtFechaSalida
        '
        Me.txtFechaSalida.Location = New System.Drawing.Point(47, 147)
        Me.txtFechaSalida.Name = "txtFechaSalida"
        Me.txtFechaSalida.Size = New System.Drawing.Size(94, 20)
        Me.txtFechaSalida.TabIndex = 140
        Me.txtFechaSalida.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtFechaRetorno
        '
        Me.txtFechaRetorno.Location = New System.Drawing.Point(47, 115)
        Me.txtFechaRetorno.Name = "txtFechaRetorno"
        Me.txtFechaRetorno.Size = New System.Drawing.Size(94, 20)
        Me.txtFechaRetorno.TabIndex = 139
        Me.txtFechaRetorno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtFechaDescanso
        '
        Me.txtFechaDescanso.Location = New System.Drawing.Point(47, 83)
        Me.txtFechaDescanso.Name = "txtFechaDescanso"
        Me.txtFechaDescanso.Size = New System.Drawing.Size(94, 20)
        Me.txtFechaDescanso.TabIndex = 138
        Me.txtFechaDescanso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(185, 108)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(85, 32)
        Me.Label21.TabIndex = 137
        Me.Label21.Text = "Horas fin de descanso"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnFechaEntrada
        '
        Me.btnFechaEntrada.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechaEntrada.Location = New System.Drawing.Point(147, 51)
        Me.btnFechaEntrada.Name = "btnFechaEntrada"
        Me.btnFechaEntrada.Size = New System.Drawing.Size(25, 20)
        Me.btnFechaEntrada.TabIndex = 129
        Me.btnFechaEntrada.Text = "•••"
        Me.btnFechaEntrada.UseVisualStyleBackColor = True
        '
        'mskRetorno
        '
        Me.mskRetorno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskRetorno.Location = New System.Drawing.Point(292, 115)
        Me.mskRetorno.Mask = "00:00"
        Me.mskRetorno.Name = "mskRetorno"
        Me.mskRetorno.Size = New System.Drawing.Size(42, 20)
        Me.mskRetorno.TabIndex = 136
        Me.mskRetorno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mskRetorno.ValidatingType = GetType(Date)
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(185, 76)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(85, 32)
        Me.Label20.TabIndex = 135
        Me.Label20.Text = "Horas inicio descanso"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'mskDescanso
        '
        Me.mskDescanso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskDescanso.Location = New System.Drawing.Point(292, 83)
        Me.mskDescanso.Mask = "00:00"
        Me.mskDescanso.Name = "mskDescanso"
        Me.mskDescanso.Size = New System.Drawing.Size(42, 20)
        Me.mskDescanso.TabIndex = 134
        Me.mskDescanso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mskDescanso.ValidatingType = GetType(Date)
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(185, 140)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(85, 32)
        Me.Label19.TabIndex = 133
        Me.Label19.Text = "Horas de Salida"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'mskSalida
        '
        Me.mskSalida.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskSalida.Location = New System.Drawing.Point(292, 147)
        Me.mskSalida.Mask = "00:00"
        Me.mskSalida.Name = "mskSalida"
        Me.mskSalida.Size = New System.Drawing.Size(42, 20)
        Me.mskSalida.TabIndex = 132
        Me.mskSalida.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mskSalida.ValidatingType = GetType(Date)
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(185, 44)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(85, 32)
        Me.Label18.TabIndex = 131
        Me.Label18.Text = "Horas de Entrada"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'mskEntrada
        '
        Me.mskEntrada.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskEntrada.Location = New System.Drawing.Point(292, 51)
        Me.mskEntrada.Mask = "00:00"
        Me.mskEntrada.Name = "mskEntrada"
        Me.mskEntrada.Size = New System.Drawing.Size(42, 20)
        Me.mskEntrada.TabIndex = 130
        Me.mskEntrada.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mskEntrada.ValidatingType = GetType(Date)
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
        Me.grpAsiste.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtFechaEntrada As System.Windows.Forms.TextBox
    Friend WithEvents grpConstante As System.Windows.Forms.GroupBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cmbTipoDia As System.Windows.Forms.ComboBox
    Friend WithEvents btnFechaEntrada As System.Windows.Forms.Button
    Friend WithEvents grpAsiste As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents mskRetorno As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents mskDescanso As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents mskSalida As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents mskEntrada As System.Windows.Forms.MaskedTextBox
    Friend WithEvents btnFechaSalida As System.Windows.Forms.Button
    Friend WithEvents btnFechaRetorno As System.Windows.Forms.Button
    Friend WithEvents btnFechaDescanso As System.Windows.Forms.Button
    Friend WithEvents txtFechaSalida As System.Windows.Forms.TextBox
    Friend WithEvents txtFechaRetorno As System.Windows.Forms.TextBox
    Friend WithEvents txtFechaDescanso As System.Windows.Forms.TextBox
    Friend WithEvents mskTotal As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnFecha As System.Windows.Forms.Button
    Friend WithEvents txtFecha As System.Windows.Forms.TextBox
End Class
