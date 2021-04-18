<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsGenFormasPagoMovimiento
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsGenFormasPagoMovimiento))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.txtNombrePagoX = New System.Windows.Forms.TextBox()
        Me.btnNombrePago = New System.Windows.Forms.Button()
        Me.btnFecha = New System.Windows.Forms.Button()
        Me.txtVence = New System.Windows.Forms.TextBox()
        Me.txtImporte = New System.Windows.Forms.TextBox()
        Me.txtNombrePago = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.txtNumeroPago = New System.Windows.Forms.TextBox()
        Me.cmbFP = New System.Windows.Forms.ComboBox()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpTarjeta.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 195)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(568, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.txtNombrePagoX)
        Me.grpTarjeta.Controls.Add(Me.btnNombrePago)
        Me.grpTarjeta.Controls.Add(Me.btnFecha)
        Me.grpTarjeta.Controls.Add(Me.txtVence)
        Me.grpTarjeta.Controls.Add(Me.txtImporte)
        Me.grpTarjeta.Controls.Add(Me.txtNombrePago)
        Me.grpTarjeta.Controls.Add(Me.Label5)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.lblNombre)
        Me.grpTarjeta.Controls.Add(Me.txtNumeroPago)
        Me.grpTarjeta.Controls.Add(Me.cmbFP)
        Me.grpTarjeta.Controls.Add(Me.lblNumero)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(564, 191)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        Me.grpTarjeta.Text = " Forma de Pago "
        '
        'txtNombrePagoX
        '
        Me.txtNombrePagoX.BackColor = System.Drawing.Color.White
        Me.txtNombrePagoX.Font = New System.Drawing.Font("Arial Black", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombrePagoX.Location = New System.Drawing.Point(519, 47)
        Me.txtNombrePagoX.Name = "txtNombrePagoX"
        Me.txtNombrePagoX.Size = New System.Drawing.Size(37, 30)
        Me.txtNombrePagoX.TabIndex = 122
        Me.txtNombrePagoX.Visible = False
        '
        'btnNombrePago
        '
        Me.btnNombrePago.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNombrePago.Location = New System.Drawing.Point(272, 47)
        Me.btnNombrePago.Name = "btnNombrePago"
        Me.btnNombrePago.Size = New System.Drawing.Size(28, 30)
        Me.btnNombrePago.TabIndex = 121
        Me.btnNombrePago.Text = "•••"
        Me.btnNombrePago.UseVisualStyleBackColor = True
        '
        'btnFecha
        '
        Me.btnFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFecha.Location = New System.Drawing.Point(272, 144)
        Me.btnFecha.Name = "btnFecha"
        Me.btnFecha.Size = New System.Drawing.Size(28, 30)
        Me.btnFecha.TabIndex = 120
        Me.btnFecha.Text = "•••"
        Me.btnFecha.UseVisualStyleBackColor = True
        '
        'txtVence
        '
        Me.txtVence.BackColor = System.Drawing.Color.White
        Me.txtVence.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVence.Location = New System.Drawing.Point(312, 144)
        Me.txtVence.Name = "txtVence"
        Me.txtVence.Size = New System.Drawing.Size(244, 26)
        Me.txtVence.TabIndex = 22
        Me.txtVence.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtImporte
        '
        Me.txtImporte.BackColor = System.Drawing.Color.White
        Me.txtImporte.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImporte.Location = New System.Drawing.Point(312, 112)
        Me.txtImporte.Name = "txtImporte"
        Me.txtImporte.Size = New System.Drawing.Size(244, 26)
        Me.txtImporte.TabIndex = 21
        Me.txtImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNombrePago
        '
        Me.txtNombrePago.BackColor = System.Drawing.Color.White
        Me.txtNombrePago.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombrePago.Location = New System.Drawing.Point(312, 47)
        Me.txtNombrePago.Name = "txtNombrePago"
        Me.txtNombrePago.Size = New System.Drawing.Size(244, 26)
        Me.txtNombrePago.TabIndex = 20
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(6, 142)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(260, 31)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Fecha vencimiento  :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'Label3
        '
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(6, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(260, 31)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Importe  :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblNombre
        '
        Me.lblNombre.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblNombre.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombre.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblNombre.Location = New System.Drawing.Point(6, 46)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(260, 31)
        Me.lblNombre.TabIndex = 17
        Me.lblNombre.Text = "Nombre de Pago  :"
        Me.lblNombre.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtNumeroPago
        '
        Me.txtNumeroPago.BackColor = System.Drawing.Color.White
        Me.txtNumeroPago.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumeroPago.Location = New System.Drawing.Point(312, 80)
        Me.txtNumeroPago.Name = "txtNumeroPago"
        Me.txtNumeroPago.Size = New System.Drawing.Size(244, 26)
        Me.txtNumeroPago.TabIndex = 14
        '
        'cmbFP
        '
        Me.cmbFP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFP.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFP.FormattingEnabled = True
        Me.cmbFP.Location = New System.Drawing.Point(312, 15)
        Me.cmbFP.Name = "cmbFP"
        Me.cmbFP.Size = New System.Drawing.Size(244, 27)
        Me.cmbFP.TabIndex = 11
        '
        'lblNumero
        '
        Me.lblNumero.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblNumero.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumero.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblNumero.Location = New System.Drawing.Point(6, 77)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(260, 31)
        Me.lblNumero.TabIndex = 4
        Me.lblNumero.Text = "Número de Pago  :"
        Me.lblNumero.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(6, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(260, 31)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Forma de Pago  :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(403, 192)
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
        Me.btnCancel.ImageIndex = 1
        Me.btnCancel.ImageList = Me.ImageList1
        Me.btnCancel.Location = New System.Drawing.Point(85, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(76, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "button_ok.ico")
        Me.ImageList1.Images.SetKeyName(1, "button_cancel.ico")
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOK.ImageIndex = 0
        Me.btnOK.ImageList = Me.ImageList1
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'jsGenFormasPagoMovimiento
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(568, 221)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsGenFormasPagoMovimiento"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento Forma de Pago"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbFP As System.Windows.Forms.ComboBox
    Friend WithEvents txtNumeroPago As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents txtVence As System.Windows.Forms.TextBox
    Friend WithEvents txtImporte As System.Windows.Forms.TextBox
    Friend WithEvents txtNombrePago As System.Windows.Forms.TextBox
    Friend WithEvents btnNombrePago As System.Windows.Forms.Button
    Friend WithEvents btnFecha As System.Windows.Forms.Button
    Friend WithEvents txtNombrePagoX As System.Windows.Forms.TextBox
End Class
