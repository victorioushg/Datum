<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsComFormasPago
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsComFormasPago))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpContado = New System.Windows.Forms.GroupBox()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBeneficiario = New System.Windows.Forms.TextBox()
        Me.btnNombrePago = New System.Windows.Forms.Button()
        Me.txtImporte = New System.Windows.Forms.TextBox()
        Me.txtNombrePago = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.txtNumeroPago = New System.Windows.Forms.TextBox()
        Me.cmbFP = New System.Windows.Forms.ComboBox()
        Me.lblNumero = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNombrePagoX = New System.Windows.Forms.TextBox()
        Me.grpCredito = New System.Windows.Forms.GroupBox()
        Me.grpGiros = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.grpVencimiento = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbCredito = New System.Windows.Forms.ComboBox()
        Me.grpCondicion = New System.Windows.Forms.GroupBox()
        Me.lblCaja = New System.Windows.Forms.Label()
        Me.cmbCaja = New System.Windows.Forms.ComboBox()
        Me.cmbCondicion = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtVencimientoPago = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.txtVence = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.grpAceptarSalir.SuspendLayout()
        Me.grpContado.SuspendLayout()
        Me.grpTarjeta.SuspendLayout()
        Me.grpCredito.SuspendLayout()
        Me.grpGiros.SuspendLayout()
        Me.grpVencimiento.SuspendLayout()
        Me.grpCondicion.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Agregar.png")
        Me.ImageList1.Images.SetKeyName(1, "Eliminar.png")
        Me.ImageList1.Images.SetKeyName(2, "Primero.png")
        Me.ImageList1.Images.SetKeyName(3, "Anterior.png")
        Me.ImageList1.Images.SetKeyName(4, "Siguiente.png")
        Me.ImageList1.Images.SetKeyName(5, "Ultimo.png")
        Me.ImageList1.Images.SetKeyName(6, "Efectivo.png")
        Me.ImageList1.Images.SetKeyName(7, "button_ok.ico")
        Me.ImageList1.Images.SetKeyName(8, "button_cancel.ico")
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 359)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(561, 26)
        Me.lblInfo.TabIndex = 80
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(396, 359)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 89
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.ImageIndex = 8
        Me.btnCancel.ImageList = Me.ImageList1
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
        Me.btnOK.ImageIndex = 7
        Me.btnOK.ImageList = Me.ImageList1
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'grpContado
        '
        Me.grpContado.Controls.Add(Me.grpTarjeta)
        Me.grpContado.Location = New System.Drawing.Point(71, 108)
        Me.grpContado.Name = "grpContado"
        Me.grpContado.Size = New System.Drawing.Size(560, 238)
        Me.grpContado.TabIndex = 109
        Me.grpContado.TabStop = False
        Me.grpContado.Text = " Contado"
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.txtVencimientoPago)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Controls.Add(Me.txtBeneficiario)
        Me.grpTarjeta.Controls.Add(Me.btnNombrePago)
        Me.grpTarjeta.Controls.Add(Me.txtImporte)
        Me.grpTarjeta.Controls.Add(Me.txtNombrePago)
        Me.grpTarjeta.Controls.Add(Me.Label5)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.lblNombre)
        Me.grpTarjeta.Controls.Add(Me.txtNumeroPago)
        Me.grpTarjeta.Controls.Add(Me.cmbFP)
        Me.grpTarjeta.Controls.Add(Me.lblNumero)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Controls.Add(Me.txtNombrePagoX)
        Me.grpTarjeta.Location = New System.Drawing.Point(6, 19)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(555, 213)
        Me.grpTarjeta.TabIndex = 81
        Me.grpTarjeta.TabStop = False
        '
        'Label2
        '
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(6, 113)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(232, 31)
        Me.Label2.TabIndex = 124
        Me.Label2.Text = "Beneficiario  :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtBeneficiario
        '
        Me.txtBeneficiario.BackColor = System.Drawing.Color.White
        Me.txtBeneficiario.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBeneficiario.Location = New System.Drawing.Point(280, 113)
        Me.txtBeneficiario.MaxLength = 100
        Me.txtBeneficiario.Name = "txtBeneficiario"
        Me.txtBeneficiario.Size = New System.Drawing.Size(257, 30)
        Me.txtBeneficiario.TabIndex = 123
        '
        'btnNombrePago
        '
        Me.btnNombrePago.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNombrePago.Location = New System.Drawing.Point(240, 46)
        Me.btnNombrePago.Name = "btnNombrePago"
        Me.btnNombrePago.Size = New System.Drawing.Size(34, 23)
        Me.btnNombrePago.TabIndex = 121
        Me.btnNombrePago.Text = "•••"
        Me.btnNombrePago.UseVisualStyleBackColor = True
        '
        'txtImporte
        '
        Me.txtImporte.BackColor = System.Drawing.Color.White
        Me.txtImporte.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImporte.Location = New System.Drawing.Point(280, 146)
        Me.txtImporte.MaxLength = 19
        Me.txtImporte.Name = "txtImporte"
        Me.txtImporte.Size = New System.Drawing.Size(256, 30)
        Me.txtImporte.TabIndex = 21
        Me.txtImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNombrePago
        '
        Me.txtNombrePago.BackColor = System.Drawing.Color.White
        Me.txtNombrePago.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombrePago.Location = New System.Drawing.Point(280, 47)
        Me.txtNombrePago.MaxLength = 20
        Me.txtNombrePago.Name = "txtNombrePago"
        Me.txtNombrePago.Size = New System.Drawing.Size(256, 30)
        Me.txtNombrePago.TabIndex = 20
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(5, 179)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(233, 31)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Fecha vencimiento  :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(5, 147)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(233, 31)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Importe  :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblNombre
        '
        Me.lblNombre.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblNombre.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombre.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblNombre.Location = New System.Drawing.Point(6, 46)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(232, 28)
        Me.lblNombre.TabIndex = 17
        Me.lblNombre.Text = "Nombre de Pago  :"
        Me.lblNombre.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNumeroPago
        '
        Me.txtNumeroPago.BackColor = System.Drawing.Color.White
        Me.txtNumeroPago.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumeroPago.Location = New System.Drawing.Point(280, 80)
        Me.txtNumeroPago.MaxLength = 20
        Me.txtNumeroPago.Name = "txtNumeroPago"
        Me.txtNumeroPago.Size = New System.Drawing.Size(256, 30)
        Me.txtNumeroPago.TabIndex = 14
        '
        'cmbFP
        '
        Me.cmbFP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFP.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFP.FormattingEnabled = True
        Me.cmbFP.Location = New System.Drawing.Point(280, 16)
        Me.cmbFP.Name = "cmbFP"
        Me.cmbFP.Size = New System.Drawing.Size(257, 30)
        Me.cmbFP.TabIndex = 11
        '
        'lblNumero
        '
        Me.lblNumero.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblNumero.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumero.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblNumero.Location = New System.Drawing.Point(6, 77)
        Me.lblNumero.Name = "lblNumero"
        Me.lblNumero.Size = New System.Drawing.Size(232, 31)
        Me.lblNumero.TabIndex = 4
        Me.lblNumero.Text = "Número de Pago  :"
        Me.lblNumero.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(6, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(232, 27)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Forma de Pago  :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNombrePagoX
        '
        Me.txtNombrePagoX.BackColor = System.Drawing.Color.White
        Me.txtNombrePagoX.Font = New System.Drawing.Font("Arial Black", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombrePagoX.Location = New System.Drawing.Point(488, 49)
        Me.txtNombrePagoX.Name = "txtNombrePagoX"
        Me.txtNombrePagoX.Size = New System.Drawing.Size(37, 30)
        Me.txtNombrePagoX.TabIndex = 122
        Me.txtNombrePagoX.Visible = False
        '
        'grpCredito
        '
        Me.grpCredito.Controls.Add(Me.cmbCredito)
        Me.grpCredito.Controls.Add(Me.grpVencimiento)
        Me.grpCredito.Location = New System.Drawing.Point(1, 97)
        Me.grpCredito.Name = "grpCredito"
        Me.grpCredito.Size = New System.Drawing.Size(560, 216)
        Me.grpCredito.TabIndex = 110
        Me.grpCredito.TabStop = False
        Me.grpCredito.Text = "Crédito"
        '
        'grpGiros
        '
        Me.grpGiros.Controls.Add(Me.Label9)
        Me.grpGiros.Controls.Add(Me.TextBox1)
        Me.grpGiros.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGiros.Location = New System.Drawing.Point(56, 33)
        Me.grpGiros.Name = "grpGiros"
        Me.grpGiros.Size = New System.Drawing.Size(546, 159)
        Me.grpGiros.TabIndex = 113
        Me.grpGiros.TabStop = False
        Me.grpGiros.Text = " Giros "
        '
        'Label9
        '
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(6, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(260, 37)
        Me.Label9.TabIndex = 109
        Me.Label9.Text = "Saldo : "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.TextBox1.Enabled = False
        Me.TextBox1.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.Gray
        Me.TextBox1.Location = New System.Drawing.Point(272, 16)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(253, 32)
        Me.TextBox1.TabIndex = 108
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpVencimiento
        '
        Me.grpVencimiento.Controls.Add(Me.grpGiros)
        Me.grpVencimiento.Controls.Add(Me.txtVence)
        Me.grpVencimiento.Controls.Add(Me.Label8)
        Me.grpVencimiento.Location = New System.Drawing.Point(14, 57)
        Me.grpVencimiento.Name = "grpVencimiento"
        Me.grpVencimiento.Size = New System.Drawing.Size(546, 159)
        Me.grpVencimiento.TabIndex = 112
        Me.grpVencimiento.TabStop = False
        Me.grpVencimiento.Text = "Vencimiento"
        '
        'Label8
        '
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label8.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(6, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(260, 37)
        Me.Label8.TabIndex = 109
        Me.Label8.Text = "Con vencimiento al : "
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbCredito
        '
        Me.cmbCredito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCredito.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCredito.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmbCredito.FormattingEnabled = True
        Me.cmbCredito.Location = New System.Drawing.Point(6, 19)
        Me.cmbCredito.Name = "cmbCredito"
        Me.cmbCredito.Size = New System.Drawing.Size(204, 27)
        Me.cmbCredito.TabIndex = 111
        '
        'grpCondicion
        '
        Me.grpCondicion.Controls.Add(Me.lblCaja)
        Me.grpCondicion.Controls.Add(Me.cmbCaja)
        Me.grpCondicion.Controls.Add(Me.cmbCondicion)
        Me.grpCondicion.Controls.Add(Me.Label15)
        Me.grpCondicion.Location = New System.Drawing.Point(0, 2)
        Me.grpCondicion.Name = "grpCondicion"
        Me.grpCondicion.Size = New System.Drawing.Size(561, 100)
        Me.grpCondicion.TabIndex = 111
        Me.grpCondicion.TabStop = False
        Me.grpCondicion.Text = " Condición "
        '
        'lblCaja
        '
        Me.lblCaja.AutoSize = True
        Me.lblCaja.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaja.Location = New System.Drawing.Point(234, 66)
        Me.lblCaja.Name = "lblCaja"
        Me.lblCaja.Size = New System.Drawing.Size(45, 19)
        Me.lblCaja.TabIndex = 112
        Me.lblCaja.Text = "CAJA"
        '
        'cmbCaja
        '
        Me.cmbCaja.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCaja.Font = New System.Drawing.Font("Century Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCaja.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmbCaja.FormattingEnabled = True
        Me.cmbCaja.Location = New System.Drawing.Point(293, 62)
        Me.cmbCaja.Name = "cmbCaja"
        Me.cmbCaja.Size = New System.Drawing.Size(256, 27)
        Me.cmbCaja.TabIndex = 111
        '
        'cmbCondicion
        '
        Me.cmbCondicion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCondicion.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCondicion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmbCondicion.FormattingEnabled = True
        Me.cmbCondicion.Location = New System.Drawing.Point(6, 62)
        Me.cmbCondicion.Name = "cmbCondicion"
        Me.cmbCondicion.Size = New System.Drawing.Size(204, 27)
        Me.cmbCondicion.TabIndex = 110
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label15.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(5, 13)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(545, 46)
        Me.Label15.TabIndex = 109
        Me.Label15.Text = "CONDICION DE PAGO"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtVencimientoPago
        '
        Me.txtVencimientoPago.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtVencimientoPago.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtVencimientoPago.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtVencimientoPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVencimientoPago.Location = New System.Drawing.Point(280, 179)
        Me.txtVencimientoPago.Name = "txtVencimientoPago"
        Me.txtVencimientoPago.Size = New System.Drawing.Size(256, 27)
        Me.txtVencimientoPago.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtVencimientoPago.TabIndex = 214
        Me.txtVencimientoPago.Value = New Date(2021, 5, 2, 0, 0, 0, 0)
        '
        'txtVence
        '
        Me.txtVence.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtVence.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtVence.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtVence.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVence.Location = New System.Drawing.Point(272, 16)
        Me.txtVence.Name = "txtVence"
        Me.txtVence.Size = New System.Drawing.Size(262, 37)
        Me.txtVence.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtVence.TabIndex = 214
        Me.txtVence.Value = New Date(2021, 5, 2, 0, 0, 0, 0)
        '
        'jsComFormasPago
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(561, 385)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCredito)
        Me.Controls.Add(Me.grpContado)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpCondicion)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "jsComFormasPago"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Forma de pago"
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.grpContado.ResumeLayout(False)
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.grpCredito.ResumeLayout(False)
        Me.grpGiros.ResumeLayout(False)
        Me.grpGiros.PerformLayout()
        Me.grpVencimiento.ResumeLayout(False)
        Me.grpCondicion.ResumeLayout(False)
        Me.grpCondicion.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grpContado As System.Windows.Forms.GroupBox
    Friend WithEvents grpCredito As System.Windows.Forms.GroupBox
    Friend WithEvents grpCondicion As System.Windows.Forms.GroupBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cmbCondicion As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCredito As System.Windows.Forms.ComboBox
    Friend WithEvents grpVencimiento As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents grpGiros As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents lblCaja As System.Windows.Forms.Label
    Friend WithEvents cmbCaja As System.Windows.Forms.ComboBox
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents txtNombrePagoX As System.Windows.Forms.TextBox
    Friend WithEvents btnNombrePago As System.Windows.Forms.Button
    Friend WithEvents txtImporte As System.Windows.Forms.TextBox
    Friend WithEvents txtNombrePago As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblNombre As System.Windows.Forms.Label
    Friend WithEvents txtNumeroPago As System.Windows.Forms.TextBox
    Friend WithEvents cmbFP As System.Windows.Forms.ComboBox
    Friend WithEvents lblNumero As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBeneficiario As System.Windows.Forms.TextBox
    Friend WithEvents txtVencimientoPago As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents txtVence As Syncfusion.WinForms.Input.SfDateTimeEdit
End Class
