<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsVenProPreCancelaciones
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
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.btnFecha = New System.Windows.Forms.Button()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.txtFecha = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lvCobranza = New System.Windows.Forms.ListView()
        Me.txtMCheques = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblProgreso = New System.Windows.Forms.Label()
        Me.cmbCaja = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbAsesores = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtCEfectivo = New System.Windows.Forms.TextBox()
        Me.txtMTarjetas = New System.Windows.Forms.TextBox()
        Me.txtMCupones = New System.Windows.Forms.TextBox()
        Me.txtMDepositos = New System.Windows.Forms.TextBox()
        Me.txtMTransfer = New System.Windows.Forms.TextBox()
        Me.txtMRetISLR = New System.Windows.Forms.TextBox()
        Me.txtMRetIVA = New System.Windows.Forms.TextBox()
        Me.txtCCheques = New System.Windows.Forms.TextBox()
        Me.txtCTarjetas = New System.Windows.Forms.TextBox()
        Me.txtCCupones = New System.Windows.Forms.TextBox()
        Me.txtCDepositos = New System.Windows.Forms.TextBox()
        Me.txtCTransfer = New System.Windows.Forms.TextBox()
        Me.txtCRetISLR = New System.Windows.Forms.TextBox()
        Me.txtCRetIVA = New System.Windows.Forms.TextBox()
        Me.txtCTotal = New System.Windows.Forms.TextBox()
        Me.txtMTotal = New System.Windows.Forms.TextBox()
        Me.btnCestaTicket = New System.Windows.Forms.Button()
        Me.chkEF = New System.Windows.Forms.CheckBox()
        Me.chkCH = New System.Windows.Forms.CheckBox()
        Me.chkTA = New System.Windows.Forms.CheckBox()
        Me.chkCT = New System.Windows.Forms.CheckBox()
        Me.chkDP = New System.Windows.Forms.CheckBox()
        Me.chkTR = New System.Windows.Forms.CheckBox()
        Me.chkISLR = New System.Windows.Forms.CheckBox()
        Me.chkIVA = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMEfectivo = New System.Windows.Forms.TextBox()
        Me.txtNumeroDeposito = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbBancos = New System.Windows.Forms.ComboBox()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTotales.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 486)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(1116, 26)
        Me.lblInfo.TabIndex = 80
        '
        'btnFecha
        '
        Me.btnFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFecha.Location = New System.Drawing.Point(215, 93)
        Me.btnFecha.Name = "btnFecha"
        Me.btnFecha.Size = New System.Drawing.Size(29, 20)
        Me.btnFecha.TabIndex = 105
        Me.btnFecha.Text = "•••"
        Me.btnFecha.UseVisualStyleBackColor = True
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(938, 481)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(175, 30)
        Me.grpAceptarSalir.TabIndex = 114
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = Global.Datum.My.Resources.Resources.button_cancel
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnCancel.Location = New System.Drawing.Point(90, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 24)
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
        Me.btnOK.Size = New System.Drawing.Size(81, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'txtFecha
        '
        Me.txtFecha.Enabled = False
        Me.txtFecha.Location = New System.Drawing.Point(124, 90)
        Me.txtFecha.MaxLength = 15
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(85, 20)
        Me.txtFecha.TabIndex = 124
        Me.txtFecha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(9, 93)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(111, 18)
        Me.Label9.TabIndex = 125
        Me.Label9.Text = "Fecha cobranza :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lvCobranza
        '
        Me.lvCobranza.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lvCobranza.CheckBoxes = True
        Me.lvCobranza.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvCobranza.FullRowSelect = True
        Me.lvCobranza.GridLines = True
        Me.lvCobranza.Location = New System.Drawing.Point(3, 159)
        Me.lvCobranza.Name = "lvCobranza"
        Me.lvCobranza.Size = New System.Drawing.Size(851, 260)
        Me.lvCobranza.TabIndex = 133
        Me.lvCobranza.UseCompatibleStateImageBehavior = False
        Me.lvCobranza.View = System.Windows.Forms.View.Details
        '
        'txtMCheques
        '
        Me.txtMCheques.Enabled = False
        Me.txtMCheques.Location = New System.Drawing.Point(991, 246)
        Me.txtMCheques.MaxLength = 15
        Me.txtMCheques.Name = "txtMCheques"
        Me.txtMCheques.Size = New System.Drawing.Size(87, 20)
        Me.txtMCheques.TabIndex = 136
        Me.txtMCheques.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(858, 391)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(130, 28)
        Me.Label11.TabIndex = 147
        Me.Label11.Text = "Totales conformados"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label12.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(0, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(495, 40)
        Me.Label12.TabIndex = 148
        Me.Label12.Text = "Datum"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'C1PictureBox1
        '
        Me.C1PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.C1PictureBox1.Image = Global.Datum.My.Resources.Resources.banda_amarilla
        Me.C1PictureBox1.Location = New System.Drawing.Point(489, 0)
        Me.C1PictureBox1.Name = "C1PictureBox1"
        Me.C1PictureBox1.Size = New System.Drawing.Size(627, 61)
        Me.C1PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.C1PictureBox1.TabIndex = 149
        Me.C1PictureBox1.TabStop = False
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(0, 40)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(495, 21)
        Me.Label13.TabIndex = 150
        Me.Label13.Tag = ""
        Me.Label13.Text = "Pase de pre-Cancelaciones a Cancelaciones"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpTotales
        '
        Me.grpTotales.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTotales.Controls.Add(Me.ProgressBar1)
        Me.grpTotales.Controls.Add(Me.Label14)
        Me.grpTotales.Controls.Add(Me.lblProgreso)
        Me.grpTotales.Location = New System.Drawing.Point(3, 420)
        Me.grpTotales.Name = "grpTotales"
        Me.grpTotales.Size = New System.Drawing.Size(1108, 63)
        Me.grpTotales.TabIndex = 151
        Me.grpTotales.TabStop = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(9, 30)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(1092, 20)
        Me.ProgressBar1.TabIndex = 16
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(8, 8)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(72, 20)
        Me.Label14.TabIndex = 15
        Me.Label14.Text = "Progreso ..."
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProgreso
        '
        Me.lblProgreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgreso.Location = New System.Drawing.Point(84, 8)
        Me.lblProgreso.Name = "lblProgreso"
        Me.lblProgreso.Size = New System.Drawing.Size(822, 20)
        Me.lblProgreso.TabIndex = 14
        Me.lblProgreso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCaja
        '
        Me.cmbCaja.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCaja.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCaja.FormattingEnabled = True
        Me.cmbCaja.Location = New System.Drawing.Point(351, 90)
        Me.cmbCaja.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbCaja.Name = "cmbCaja"
        Me.cmbCaja.Size = New System.Drawing.Size(332, 21)
        Me.cmbCaja.TabIndex = 157
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(250, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(95, 17)
        Me.Label6.TabIndex = 161
        Me.Label6.Text = "Caja :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbAsesores
        '
        Me.cmbAsesores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAsesores.Font = New System.Drawing.Font("Consolas", 8.25!)
        Me.cmbAsesores.FormattingEnabled = True
        Me.cmbAsesores.Location = New System.Drawing.Point(124, 67)
        Me.cmbAsesores.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbAsesores.Name = "cmbAsesores"
        Me.cmbAsesores.Size = New System.Drawing.Size(559, 21)
        Me.cmbAsesores.TabIndex = 163
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 18)
        Me.Label2.TabIndex = 164
        Me.Label2.Text = "Asesor comercial :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(860, 227)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 20)
        Me.Label3.TabIndex = 165
        Me.Label3.Text = "Efectivo"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(860, 247)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(129, 20)
        Me.Label4.TabIndex = 166
        Me.Label4.Text = "Cheques"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(860, 267)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(129, 20)
        Me.Label5.TabIndex = 167
        Me.Label5.Text = "Tarjetas"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(860, 287)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(129, 20)
        Me.Label7.TabIndex = 168
        Me.Label7.Text = "Cupones alimentación"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(860, 307)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(129, 20)
        Me.Label15.TabIndex = 169
        Me.Label15.Text = "Depósitos"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(855, 327)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(134, 20)
        Me.Label16.TabIndex = 170
        Me.Label16.Text = "Transferencias"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(858, 347)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(131, 20)
        Me.Label17.TabIndex = 171
        Me.Label17.Text = "Retenciones ISLR"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(858, 367)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(131, 20)
        Me.Label18.TabIndex = 172
        Me.Label18.Text = "Retenciones IVA"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCEfectivo
        '
        Me.txtCEfectivo.Enabled = False
        Me.txtCEfectivo.Location = New System.Drawing.Point(1080, 225)
        Me.txtCEfectivo.MaxLength = 15
        Me.txtCEfectivo.Name = "txtCEfectivo"
        Me.txtCEfectivo.Size = New System.Drawing.Size(31, 20)
        Me.txtCEfectivo.TabIndex = 173
        Me.txtCEfectivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMTarjetas
        '
        Me.txtMTarjetas.Enabled = False
        Me.txtMTarjetas.Location = New System.Drawing.Point(991, 267)
        Me.txtMTarjetas.MaxLength = 15
        Me.txtMTarjetas.Name = "txtMTarjetas"
        Me.txtMTarjetas.Size = New System.Drawing.Size(87, 20)
        Me.txtMTarjetas.TabIndex = 174
        Me.txtMTarjetas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMCupones
        '
        Me.txtMCupones.Enabled = False
        Me.txtMCupones.Location = New System.Drawing.Point(991, 288)
        Me.txtMCupones.MaxLength = 15
        Me.txtMCupones.Name = "txtMCupones"
        Me.txtMCupones.Size = New System.Drawing.Size(87, 20)
        Me.txtMCupones.TabIndex = 175
        Me.txtMCupones.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMDepositos
        '
        Me.txtMDepositos.Enabled = False
        Me.txtMDepositos.Location = New System.Drawing.Point(991, 309)
        Me.txtMDepositos.MaxLength = 15
        Me.txtMDepositos.Name = "txtMDepositos"
        Me.txtMDepositos.Size = New System.Drawing.Size(87, 20)
        Me.txtMDepositos.TabIndex = 176
        Me.txtMDepositos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMTransfer
        '
        Me.txtMTransfer.Enabled = False
        Me.txtMTransfer.Location = New System.Drawing.Point(991, 330)
        Me.txtMTransfer.MaxLength = 15
        Me.txtMTransfer.Name = "txtMTransfer"
        Me.txtMTransfer.Size = New System.Drawing.Size(87, 20)
        Me.txtMTransfer.TabIndex = 177
        Me.txtMTransfer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMRetISLR
        '
        Me.txtMRetISLR.Enabled = False
        Me.txtMRetISLR.Location = New System.Drawing.Point(991, 351)
        Me.txtMRetISLR.MaxLength = 15
        Me.txtMRetISLR.Name = "txtMRetISLR"
        Me.txtMRetISLR.Size = New System.Drawing.Size(87, 20)
        Me.txtMRetISLR.TabIndex = 178
        Me.txtMRetISLR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMRetIVA
        '
        Me.txtMRetIVA.Enabled = False
        Me.txtMRetIVA.Location = New System.Drawing.Point(991, 372)
        Me.txtMRetIVA.MaxLength = 15
        Me.txtMRetIVA.Name = "txtMRetIVA"
        Me.txtMRetIVA.Size = New System.Drawing.Size(87, 20)
        Me.txtMRetIVA.TabIndex = 179
        Me.txtMRetIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCCheques
        '
        Me.txtCCheques.Enabled = False
        Me.txtCCheques.Location = New System.Drawing.Point(1080, 246)
        Me.txtCCheques.MaxLength = 15
        Me.txtCCheques.Name = "txtCCheques"
        Me.txtCCheques.Size = New System.Drawing.Size(31, 20)
        Me.txtCCheques.TabIndex = 180
        Me.txtCCheques.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCTarjetas
        '
        Me.txtCTarjetas.Enabled = False
        Me.txtCTarjetas.Location = New System.Drawing.Point(1080, 267)
        Me.txtCTarjetas.MaxLength = 15
        Me.txtCTarjetas.Name = "txtCTarjetas"
        Me.txtCTarjetas.Size = New System.Drawing.Size(31, 20)
        Me.txtCTarjetas.TabIndex = 181
        Me.txtCTarjetas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCCupones
        '
        Me.txtCCupones.Enabled = False
        Me.txtCCupones.Location = New System.Drawing.Point(1080, 288)
        Me.txtCCupones.MaxLength = 15
        Me.txtCCupones.Name = "txtCCupones"
        Me.txtCCupones.Size = New System.Drawing.Size(31, 20)
        Me.txtCCupones.TabIndex = 182
        Me.txtCCupones.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCDepositos
        '
        Me.txtCDepositos.Enabled = False
        Me.txtCDepositos.Location = New System.Drawing.Point(1080, 309)
        Me.txtCDepositos.MaxLength = 15
        Me.txtCDepositos.Name = "txtCDepositos"
        Me.txtCDepositos.Size = New System.Drawing.Size(31, 20)
        Me.txtCDepositos.TabIndex = 183
        Me.txtCDepositos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCTransfer
        '
        Me.txtCTransfer.Enabled = False
        Me.txtCTransfer.Location = New System.Drawing.Point(1080, 330)
        Me.txtCTransfer.MaxLength = 15
        Me.txtCTransfer.Name = "txtCTransfer"
        Me.txtCTransfer.Size = New System.Drawing.Size(31, 20)
        Me.txtCTransfer.TabIndex = 184
        Me.txtCTransfer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCRetISLR
        '
        Me.txtCRetISLR.Enabled = False
        Me.txtCRetISLR.Location = New System.Drawing.Point(1080, 351)
        Me.txtCRetISLR.MaxLength = 15
        Me.txtCRetISLR.Name = "txtCRetISLR"
        Me.txtCRetISLR.Size = New System.Drawing.Size(31, 20)
        Me.txtCRetISLR.TabIndex = 185
        Me.txtCRetISLR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCRetIVA
        '
        Me.txtCRetIVA.Enabled = False
        Me.txtCRetIVA.Location = New System.Drawing.Point(1080, 372)
        Me.txtCRetIVA.MaxLength = 15
        Me.txtCRetIVA.Name = "txtCRetIVA"
        Me.txtCRetIVA.Size = New System.Drawing.Size(31, 20)
        Me.txtCRetIVA.TabIndex = 186
        Me.txtCRetIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCTotal
        '
        Me.txtCTotal.Enabled = False
        Me.txtCTotal.Location = New System.Drawing.Point(1080, 396)
        Me.txtCTotal.MaxLength = 15
        Me.txtCTotal.Name = "txtCTotal"
        Me.txtCTotal.Size = New System.Drawing.Size(31, 20)
        Me.txtCTotal.TabIndex = 187
        Me.txtCTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMTotal
        '
        Me.txtMTotal.Enabled = False
        Me.txtMTotal.Location = New System.Drawing.Point(991, 396)
        Me.txtMTotal.MaxLength = 15
        Me.txtMTotal.Name = "txtMTotal"
        Me.txtMTotal.Size = New System.Drawing.Size(87, 20)
        Me.txtMTotal.TabIndex = 188
        Me.txtMTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnCestaTicket
        '
        Me.btnCestaTicket.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCestaTicket.Location = New System.Drawing.Point(877, 159)
        Me.btnCestaTicket.Name = "btnCestaTicket"
        Me.btnCestaTicket.Size = New System.Drawing.Size(29, 20)
        Me.btnCestaTicket.TabIndex = 189
        Me.btnCestaTicket.Text = "•••"
        Me.btnCestaTicket.UseVisualStyleBackColor = True
        '
        'chkEF
        '
        Me.chkEF.AutoSize = True
        Me.chkEF.Location = New System.Drawing.Point(830, 69)
        Me.chkEF.Name = "chkEF"
        Me.chkEF.Size = New System.Drawing.Size(41, 17)
        Me.chkEF.TabIndex = 190
        Me.chkEF.Text = "EF"
        Me.chkEF.UseVisualStyleBackColor = True
        '
        'chkCH
        '
        Me.chkCH.AutoSize = True
        Me.chkCH.Location = New System.Drawing.Point(877, 68)
        Me.chkCH.Name = "chkCH"
        Me.chkCH.Size = New System.Drawing.Size(43, 17)
        Me.chkCH.TabIndex = 191
        Me.chkCH.Text = "CH"
        Me.chkCH.UseVisualStyleBackColor = True
        '
        'chkTA
        '
        Me.chkTA.AutoSize = True
        Me.chkTA.Location = New System.Drawing.Point(926, 68)
        Me.chkTA.Name = "chkTA"
        Me.chkTA.Size = New System.Drawing.Size(42, 17)
        Me.chkTA.TabIndex = 192
        Me.chkTA.Text = "TA"
        Me.chkTA.UseVisualStyleBackColor = True
        '
        'chkCT
        '
        Me.chkCT.AutoSize = True
        Me.chkCT.Location = New System.Drawing.Point(972, 68)
        Me.chkCT.Name = "chkCT"
        Me.chkCT.Size = New System.Drawing.Size(42, 17)
        Me.chkCT.TabIndex = 193
        Me.chkCT.Text = "CT"
        Me.chkCT.UseVisualStyleBackColor = True
        '
        'chkDP
        '
        Me.chkDP.AutoSize = True
        Me.chkDP.Location = New System.Drawing.Point(830, 91)
        Me.chkDP.Name = "chkDP"
        Me.chkDP.Size = New System.Drawing.Size(43, 17)
        Me.chkDP.TabIndex = 194
        Me.chkDP.Text = "DP"
        Me.chkDP.UseVisualStyleBackColor = True
        '
        'chkTR
        '
        Me.chkTR.AutoSize = True
        Me.chkTR.Location = New System.Drawing.Point(877, 90)
        Me.chkTR.Name = "chkTR"
        Me.chkTR.Size = New System.Drawing.Size(41, 17)
        Me.chkTR.TabIndex = 195
        Me.chkTR.Text = "EF"
        Me.chkTR.UseVisualStyleBackColor = True
        '
        'chkISLR
        '
        Me.chkISLR.AutoSize = True
        Me.chkISLR.Location = New System.Drawing.Point(972, 90)
        Me.chkISLR.Name = "chkISLR"
        Me.chkISLR.Size = New System.Drawing.Size(54, 17)
        Me.chkISLR.TabIndex = 196
        Me.chkISLR.Text = "ISLR"
        Me.chkISLR.UseVisualStyleBackColor = True
        '
        'chkIVA
        '
        Me.chkIVA.AutoSize = True
        Me.chkIVA.Location = New System.Drawing.Point(926, 91)
        Me.chkIVA.Name = "chkIVA"
        Me.chkIVA.Size = New System.Drawing.Size(46, 17)
        Me.chkIVA.TabIndex = 197
        Me.chkIVA.Text = "IVA"
        Me.chkIVA.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(905, 159)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(203, 41)
        Me.Label1.TabIndex = 198
        Me.Label1.Text = "para validar cancelación en Cupón de alimentación"
        '
        'txtMEfectivo
        '
        Me.txtMEfectivo.Enabled = False
        Me.txtMEfectivo.Location = New System.Drawing.Point(991, 225)
        Me.txtMEfectivo.MaxLength = 15
        Me.txtMEfectivo.Name = "txtMEfectivo"
        Me.txtMEfectivo.Size = New System.Drawing.Size(87, 20)
        Me.txtMEfectivo.TabIndex = 135
        Me.txtMEfectivo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNumeroDeposito
        '
        Me.txtNumeroDeposito.Enabled = False
        Me.txtNumeroDeposito.Location = New System.Drawing.Point(351, 136)
        Me.txtNumeroDeposito.MaxLength = 15
        Me.txtNumeroDeposito.Name = "txtNumeroDeposito"
        Me.txtNumeroDeposito.Size = New System.Drawing.Size(332, 20)
        Me.txtNumeroDeposito.TabIndex = 199
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(250, 133)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 17)
        Me.Label8.TabIndex = 200
        Me.Label8.Text = "N° Depósito :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(215, 113)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(130, 21)
        Me.Label10.TabIndex = 201
        Me.Label10.Text = "Banco Depósito :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbBancos
        '
        Me.cmbBancos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBancos.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBancos.FormattingEnabled = True
        Me.cmbBancos.Location = New System.Drawing.Point(351, 113)
        Me.cmbBancos.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbBancos.Name = "cmbBancos"
        Me.cmbBancos.Size = New System.Drawing.Size(332, 21)
        Me.cmbBancos.TabIndex = 202
        '
        'jsVenProPreCancelaciones
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1116, 512)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmbBancos)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtNumeroDeposito)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkIVA)
        Me.Controls.Add(Me.chkISLR)
        Me.Controls.Add(Me.chkTR)
        Me.Controls.Add(Me.chkDP)
        Me.Controls.Add(Me.chkCT)
        Me.Controls.Add(Me.chkTA)
        Me.Controls.Add(Me.chkCH)
        Me.Controls.Add(Me.chkEF)
        Me.Controls.Add(Me.btnCestaTicket)
        Me.Controls.Add(Me.txtMTotal)
        Me.Controls.Add(Me.txtCTotal)
        Me.Controls.Add(Me.txtCRetIVA)
        Me.Controls.Add(Me.txtCRetISLR)
        Me.Controls.Add(Me.txtCTransfer)
        Me.Controls.Add(Me.txtCDepositos)
        Me.Controls.Add(Me.txtCCupones)
        Me.Controls.Add(Me.txtCTarjetas)
        Me.Controls.Add(Me.txtCCheques)
        Me.Controls.Add(Me.txtMRetIVA)
        Me.Controls.Add(Me.txtMRetISLR)
        Me.Controls.Add(Me.txtMTransfer)
        Me.Controls.Add(Me.txtMDepositos)
        Me.Controls.Add(Me.txtMCupones)
        Me.Controls.Add(Me.txtMTarjetas)
        Me.Controls.Add(Me.txtCEfectivo)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbAsesores)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtMEfectivo)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbCaja)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.txtMCheques)
        Me.Controls.Add(Me.grpTotales)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.lvCobranza)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtFecha)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.btnFecha)
        Me.Controls.Add(Me.lblInfo)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "jsVenProPreCancelaciones"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Proceso Pre-Pedidos"
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTotales.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents btnFecha As System.Windows.Forms.Button
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtFecha As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lvCobranza As System.Windows.Forms.ListView
    Friend WithEvents txtMCheques As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents grpTotales As System.Windows.Forms.GroupBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblProgreso As System.Windows.Forms.Label
    Friend WithEvents cmbCaja As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbAsesores As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtCEfectivo As System.Windows.Forms.TextBox
    Friend WithEvents txtMTarjetas As System.Windows.Forms.TextBox
    Friend WithEvents txtMCupones As System.Windows.Forms.TextBox
    Friend WithEvents txtMDepositos As System.Windows.Forms.TextBox
    Friend WithEvents txtMTransfer As System.Windows.Forms.TextBox
    Friend WithEvents txtMRetISLR As System.Windows.Forms.TextBox
    Friend WithEvents txtMRetIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtCCheques As System.Windows.Forms.TextBox
    Friend WithEvents txtCTarjetas As System.Windows.Forms.TextBox
    Friend WithEvents txtCCupones As System.Windows.Forms.TextBox
    Friend WithEvents txtCDepositos As System.Windows.Forms.TextBox
    Friend WithEvents txtCTransfer As System.Windows.Forms.TextBox
    Friend WithEvents txtCRetISLR As System.Windows.Forms.TextBox
    Friend WithEvents txtCRetIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtCTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtMTotal As System.Windows.Forms.TextBox
    Friend WithEvents btnCestaTicket As System.Windows.Forms.Button
    Friend WithEvents chkEF As System.Windows.Forms.CheckBox
    Friend WithEvents chkCH As System.Windows.Forms.CheckBox
    Friend WithEvents chkTA As System.Windows.Forms.CheckBox
    Friend WithEvents chkCT As System.Windows.Forms.CheckBox
    Friend WithEvents chkDP As System.Windows.Forms.CheckBox
    Friend WithEvents chkTR As System.Windows.Forms.CheckBox
    Friend WithEvents chkISLR As System.Windows.Forms.CheckBox
    Friend WithEvents chkIVA As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMEfectivo As System.Windows.Forms.TextBox
    Friend WithEvents txtNumeroDeposito As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbBancos As System.Windows.Forms.ComboBox
End Class
