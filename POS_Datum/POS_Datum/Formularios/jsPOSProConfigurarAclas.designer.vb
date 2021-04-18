<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsPOSProConfigurarAclas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsPOSProConfigurarAclas))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpCaja = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCaja = New System.Windows.Forms.TextBox()
        Me.txtNombreCajero = New System.Windows.Forms.TextBox()
        Me.txtCodigoCajero = New System.Windows.Forms.TextBox()
        Me.lblcuenta = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblLeyenda = New System.Windows.Forms.Label()
        Me.grpLeyenda = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblImpresora = New System.Windows.Forms.Label()
        Me.btnDatosImpresora = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtUltimaFactura = New System.Windows.Forms.TextBox()
        Me.txtUltimaNC = New System.Windows.Forms.TextBox()
        Me.txtUltimaNF = New System.Windows.Forms.TextBox()
        Me.btnAnular = New System.Windows.Forms.Button()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.btnImprimirProgramacion = New System.Windows.Forms.Button()
        Me.btnProgramarPagos = New System.Windows.Forms.Button()
        Me.btnFinalizarFactura = New System.Windows.Forms.Button()
        Me.txtRegistroFiscal = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtUltimaND = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.grpCaja.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.grpLeyenda.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Location = New System.Drawing.Point(-3, 416)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(566, 21)
        Me.lblInfo.TabIndex = 80
        '
        'grpCaja
        '
        Me.grpCaja.BackColor = System.Drawing.Color.White
        Me.grpCaja.Controls.Add(Me.Label2)
        Me.grpCaja.Controls.Add(Me.txtCaja)
        Me.grpCaja.Controls.Add(Me.txtNombreCajero)
        Me.grpCaja.Controls.Add(Me.txtCodigoCajero)
        Me.grpCaja.Controls.Add(Me.lblcuenta)
        Me.grpCaja.Location = New System.Drawing.Point(1, 182)
        Me.grpCaja.Name = "grpCaja"
        Me.grpCaja.Size = New System.Drawing.Size(730, 48)
        Me.grpCaja.TabIndex = 82
        Me.grpCaja.TabStop = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(128, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 20)
        Me.Label2.TabIndex = 113
        Me.Label2.Text = "Cajero :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCaja
        '
        Me.txtCaja.Location = New System.Drawing.Point(72, 11)
        Me.txtCaja.Name = "txtCaja"
        Me.txtCaja.Size = New System.Drawing.Size(53, 20)
        Me.txtCaja.TabIndex = 112
        Me.txtCaja.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtNombreCajero
        '
        Me.txtNombreCajero.Location = New System.Drawing.Point(265, 11)
        Me.txtNombreCajero.Name = "txtNombreCajero"
        Me.txtNombreCajero.Size = New System.Drawing.Size(450, 20)
        Me.txtNombreCajero.TabIndex = 111
        '
        'txtCodigoCajero
        '
        Me.txtCodigoCajero.Location = New System.Drawing.Point(196, 11)
        Me.txtCodigoCajero.Name = "txtCodigoCajero"
        Me.txtCodigoCajero.Size = New System.Drawing.Size(53, 20)
        Me.txtCodigoCajero.TabIndex = 110
        Me.txtCodigoCajero.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblcuenta
        '
        Me.lblcuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcuenta.Location = New System.Drawing.Point(11, 13)
        Me.lblcuenta.Name = "lblcuenta"
        Me.lblcuenta.Size = New System.Drawing.Size(59, 20)
        Me.lblcuenta.TabIndex = 0
        Me.lblcuenta.Text = "Caja Nº :"
        Me.lblcuenta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(569, 410)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 85
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
        Me.ImageList1.Images.SetKeyName(2, "Imprimir.png")
        Me.ImageList1.Images.SetKeyName(3, "39.png")
        Me.ImageList1.Images.SetKeyName(4, "9.png")
        Me.ImageList1.Images.SetKeyName(5, "Configurar.png")
        Me.ImageList1.Images.SetKeyName(6, "Seleccionar.png")
        Me.ImageList1.Images.SetKeyName(7, "126.png")
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
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(1, 41)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(230, 21)
        Me.Label9.TabIndex = 87
        Me.Label9.Text = "POS : Configuración Aclas/Bixolon"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label10.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(1, 1)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(230, 40)
        Me.Label10.TabIndex = 88
        Me.Label10.Text = "Datum"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblLeyenda
        '
        Me.lblLeyenda.BackColor = System.Drawing.Color.White
        Me.lblLeyenda.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLeyenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeyenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.lblLeyenda.Location = New System.Drawing.Point(13, 16)
        Me.lblLeyenda.Name = "lblLeyenda"
        Me.lblLeyenda.Size = New System.Drawing.Size(701, 95)
        Me.lblLeyenda.TabIndex = 89
        Me.lblLeyenda.Text = "Configuracion impresora"
        '
        'grpLeyenda
        '
        Me.grpLeyenda.BackColor = System.Drawing.Color.White
        Me.grpLeyenda.Controls.Add(Me.lblLeyenda)
        Me.grpLeyenda.Location = New System.Drawing.Point(2, 60)
        Me.grpLeyenda.Name = "grpLeyenda"
        Me.grpLeyenda.Size = New System.Drawing.Size(728, 127)
        Me.grpLeyenda.TabIndex = 90
        Me.grpLeyenda.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Image = Global.POS_Datum.My.Resources.Resources.banda_amarilla
        Me.PictureBox1.Location = New System.Drawing.Point(231, 1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(500, 61)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 91
        Me.PictureBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 233)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 20)
        Me.Label4.TabIndex = 92
        Me.Label4.Text = "Impresora  :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblImpresora
        '
        Me.lblImpresora.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblImpresora.Location = New System.Drawing.Point(103, 233)
        Me.lblImpresora.Name = "lblImpresora"
        Me.lblImpresora.Size = New System.Drawing.Size(424, 20)
        Me.lblImpresora.TabIndex = 114
        Me.lblImpresora.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDatosImpresora
        '
        Me.btnDatosImpresora.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnDatosImpresora.ImageIndex = 6
        Me.btnDatosImpresora.ImageList = Me.ImageList1
        Me.btnDatosImpresora.Location = New System.Drawing.Point(15, 257)
        Me.btnDatosImpresora.Name = "btnDatosImpresora"
        Me.btnDatosImpresora.Size = New System.Drawing.Size(123, 63)
        Me.btnDatosImpresora.TabIndex = 115
        Me.btnDatosImpresora.Text = "Traer Datos Impresora"
        Me.btnDatosImpresora.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(155, 253)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(134, 20)
        Me.Label1.TabIndex = 116
        Me.Label1.Text = "N° Ultima Factura :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(155, 295)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(134, 20)
        Me.Label5.TabIndex = 117
        Me.Label5.Text = "N° Ultima NC :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(155, 317)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(134, 20)
        Me.Label6.TabIndex = 118
        Me.Label6.Text = "N° Ultima No Fiscal :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtUltimaFactura
        '
        Me.txtUltimaFactura.Location = New System.Drawing.Point(295, 254)
        Me.txtUltimaFactura.Name = "txtUltimaFactura"
        Me.txtUltimaFactura.Size = New System.Drawing.Size(109, 20)
        Me.txtUltimaFactura.TabIndex = 119
        Me.txtUltimaFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtUltimaNC
        '
        Me.txtUltimaNC.Location = New System.Drawing.Point(295, 296)
        Me.txtUltimaNC.Name = "txtUltimaNC"
        Me.txtUltimaNC.Size = New System.Drawing.Size(109, 20)
        Me.txtUltimaNC.TabIndex = 120
        Me.txtUltimaNC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtUltimaNF
        '
        Me.txtUltimaNF.Location = New System.Drawing.Point(295, 317)
        Me.txtUltimaNF.Name = "txtUltimaNF"
        Me.txtUltimaNF.Size = New System.Drawing.Size(109, 20)
        Me.txtUltimaNF.TabIndex = 121
        Me.txtUltimaNF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnAnular
        '
        Me.btnAnular.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnAnular.ImageIndex = 4
        Me.btnAnular.ImageList = Me.ImageList1
        Me.btnAnular.Location = New System.Drawing.Point(430, 326)
        Me.btnAnular.Name = "btnAnular"
        Me.btnAnular.Size = New System.Drawing.Size(123, 63)
        Me.btnAnular.TabIndex = 122
        Me.btnAnular.Text = "Anulación documento"
        Me.btnAnular.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnReset
        '
        Me.btnReset.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnReset.ImageIndex = 3
        Me.btnReset.ImageList = Me.ImageList1
        Me.btnReset.Location = New System.Drawing.Point(570, 257)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(123, 62)
        Me.btnReset.TabIndex = 123
        Me.btnReset.Text = "Reiniciar impresora"
        Me.btnReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnImprimirProgramacion
        '
        Me.btnImprimirProgramacion.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnImprimirProgramacion.ImageIndex = 2
        Me.btnImprimirProgramacion.ImageList = Me.ImageList1
        Me.btnImprimirProgramacion.Location = New System.Drawing.Point(569, 326)
        Me.btnImprimirProgramacion.Name = "btnImprimirProgramacion"
        Me.btnImprimirProgramacion.Size = New System.Drawing.Size(123, 63)
        Me.btnImprimirProgramacion.TabIndex = 124
        Me.btnImprimirProgramacion.Text = "Imprimir Programación"
        Me.btnImprimirProgramacion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnProgramarPagos
        '
        Me.btnProgramarPagos.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnProgramarPagos.ImageIndex = 5
        Me.btnProgramarPagos.ImageList = Me.ImageList1
        Me.btnProgramarPagos.Location = New System.Drawing.Point(15, 326)
        Me.btnProgramarPagos.Name = "btnProgramarPagos"
        Me.btnProgramarPagos.Size = New System.Drawing.Size(123, 63)
        Me.btnProgramarPagos.TabIndex = 125
        Me.btnProgramarPagos.Text = "Programar Formas de Pago"
        Me.btnProgramarPagos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnFinalizarFactura
        '
        Me.btnFinalizarFactura.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnFinalizarFactura.ImageIndex = 7
        Me.btnFinalizarFactura.ImageList = Me.ImageList1
        Me.btnFinalizarFactura.Location = New System.Drawing.Point(430, 256)
        Me.btnFinalizarFactura.Name = "btnFinalizarFactura"
        Me.btnFinalizarFactura.Size = New System.Drawing.Size(123, 62)
        Me.btnFinalizarFactura.TabIndex = 126
        Me.btnFinalizarFactura.Text = "Cierra Documento"
        Me.btnFinalizarFactura.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'txtRegistroFiscal
        '
        Me.txtRegistroFiscal.Location = New System.Drawing.Point(295, 347)
        Me.txtRegistroFiscal.Name = "txtRegistroFiscal"
        Me.txtRegistroFiscal.Size = New System.Drawing.Size(109, 20)
        Me.txtRegistroFiscal.TabIndex = 127
        Me.txtRegistroFiscal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(155, 347)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(134, 20)
        Me.Label3.TabIndex = 128
        Me.Label3.Text = "N° Registro Fiscal :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtUltimaND
        '
        Me.txtUltimaND.Location = New System.Drawing.Point(295, 275)
        Me.txtUltimaND.Name = "txtUltimaND"
        Me.txtUltimaND.Size = New System.Drawing.Size(109, 20)
        Me.txtUltimaND.TabIndex = 130
        Me.txtUltimaND.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(155, 275)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(134, 20)
        Me.Label7.TabIndex = 129
        Me.Label7.Text = "N° Ultima ND :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'jsPOSProConfigurarAclas
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(732, 438)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtUltimaND)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtRegistroFiscal)
        Me.Controls.Add(Me.btnFinalizarFactura)
        Me.Controls.Add(Me.btnProgramarPagos)
        Me.Controls.Add(Me.btnImprimirProgramacion)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.grpCaja)
        Me.Controls.Add(Me.btnAnular)
        Me.Controls.Add(Me.txtUltimaNF)
        Me.Controls.Add(Me.txtUltimaNC)
        Me.Controls.Add(Me.txtUltimaFactura)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnDatosImpresora)
        Me.Controls.Add(Me.lblImpresora)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.grpLeyenda)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsPOSProConfigurarAclas"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Configuración impresora Aclas"
        Me.grpCaja.ResumeLayout(False)
        Me.grpCaja.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.grpLeyenda.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpCaja As System.Windows.Forms.GroupBox
    Friend WithEvents lblcuenta As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda As System.Windows.Forms.Label
    Friend WithEvents grpLeyenda As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtCaja As System.Windows.Forms.TextBox
    Friend WithEvents txtNombreCajero As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigoCajero As System.Windows.Forms.TextBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblImpresora As System.Windows.Forms.Label
    Friend WithEvents btnDatosImpresora As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtUltimaFactura As System.Windows.Forms.TextBox
    Friend WithEvents txtUltimaNC As System.Windows.Forms.TextBox
    Friend WithEvents txtUltimaNF As System.Windows.Forms.TextBox
    Friend WithEvents btnAnular As System.Windows.Forms.Button
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents btnImprimirProgramacion As System.Windows.Forms.Button
    Friend WithEvents btnProgramarPagos As System.Windows.Forms.Button
    Friend WithEvents btnFinalizarFactura As System.Windows.Forms.Button
    Friend WithEvents txtRegistroFiscal As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtUltimaND As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
