<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsBanArcDepositarCaja
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsBanArcDepositarCaja))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lv = New System.Windows.Forms.ListView()
        Me.grpCaja = New System.Windows.Forms.GroupBox()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.btnBP = New System.Windows.Forms.Button()
        Me.txtRP = New System.Windows.Forms.TextBox()
        Me.cmbFP = New System.Windows.Forms.ComboBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.cmbSeleccion = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblCaja = New System.Windows.Forms.Label()
        Me.lblTituloCaja = New System.Windows.Forms.Label()
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.txtTickets = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtSaldoSel = New System.Windows.Forms.TextBox()
        Me.txtDocSel = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grptextos = New System.Windows.Forms.GroupBox()
        Me.txtTotalDeposito = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtIVA = New System.Windows.Forms.TextBox()
        Me.txtComision = New System.Windows.Forms.TextBox()
        Me.txtNumControl = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCargos = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtISRL = New System.Windows.Forms.TextBox()
        Me.txtAjustes = New System.Windows.Forms.TextBox()
        Me.txtConcepto = New System.Windows.Forms.TextBox()
        Me.txtDeposito = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.txtEmision = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.txtFechaSeleccion = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.grpCaja.SuspendLayout()
        Me.grpTotales.SuspendLayout()
        Me.grptextos.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 488)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(732, 27)
        Me.lblInfo.TabIndex = 80
        '
        'lv
        '
        Me.lv.BackColor = System.Drawing.Color.MintCream
        Me.lv.CheckBoxes = True
        Me.lv.FullRowSelect = True
        Me.lv.GridLines = True
        Me.lv.HideSelection = False
        Me.lv.Location = New System.Drawing.Point(-1, 103)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(730, 198)
        Me.lv.TabIndex = 81
        Me.lv.UseCompatibleStateImageBehavior = False
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'grpCaja
        '
        Me.grpCaja.Controls.Add(Me.txtFechaSeleccion)
        Me.grpCaja.Controls.Add(Me.txtBuscar)
        Me.grpCaja.Controls.Add(Me.Label15)
        Me.grpCaja.Controls.Add(Me.btnBP)
        Me.grpCaja.Controls.Add(Me.txtRP)
        Me.grpCaja.Controls.Add(Me.cmbFP)
        Me.grpCaja.Controls.Add(Me.btnGo)
        Me.grpCaja.Controls.Add(Me.cmbSeleccion)
        Me.grpCaja.Controls.Add(Me.Label1)
        Me.grpCaja.Controls.Add(Me.lblCaja)
        Me.grpCaja.Controls.Add(Me.lblTituloCaja)
        Me.grpCaja.Location = New System.Drawing.Point(-1, -2)
        Me.grpCaja.Name = "grpCaja"
        Me.grpCaja.Size = New System.Drawing.Size(730, 99)
        Me.grpCaja.TabIndex = 82
        Me.grpCaja.TabStop = False
        '
        'txtBuscar
        '
        Me.txtBuscar.Location = New System.Drawing.Point(517, 79)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(208, 20)
        Me.txtBuscar.TabIndex = 140
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(431, 81)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(79, 18)
        Me.Label15.TabIndex = 139
        Me.Label15.Text = "Buscar"
        '
        'btnBP
        '
        Me.btnBP.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBP.Location = New System.Drawing.Point(240, 33)
        Me.btnBP.Name = "btnBP"
        Me.btnBP.Size = New System.Drawing.Size(29, 20)
        Me.btnBP.TabIndex = 138
        Me.btnBP.Text = "•••"
        Me.btnBP.UseVisualStyleBackColor = True
        '
        'txtRP
        '
        Me.txtRP.Location = New System.Drawing.Point(180, 33)
        Me.txtRP.Name = "txtRP"
        Me.txtRP.Size = New System.Drawing.Size(54, 20)
        Me.txtRP.TabIndex = 137
        Me.txtRP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbFP
        '
        Me.cmbFP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFP.FormattingEnabled = True
        Me.cmbFP.Location = New System.Drawing.Point(111, 32)
        Me.cmbFP.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbFP.Name = "cmbFP"
        Me.cmbFP.Size = New System.Drawing.Size(51, 21)
        Me.cmbFP.TabIndex = 136
        '
        'btnGo
        '
        Me.btnGo.AutoSize = True
        Me.btnGo.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGo.Image = CType(resources.GetObject("btnGo.Image"), System.Drawing.Image)
        Me.btnGo.Location = New System.Drawing.Point(273, 8)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(46, 45)
        Me.btnGo.TabIndex = 135
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'cmbSeleccion
        '
        Me.cmbSeleccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSeleccion.FormattingEnabled = True
        Me.cmbSeleccion.Location = New System.Drawing.Point(111, 8)
        Me.cmbSeleccion.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbSeleccion.Name = "cmbSeleccion"
        Me.cmbSeleccion.Size = New System.Drawing.Size(158, 21)
        Me.cmbSeleccion.TabIndex = 132
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 18)
        Me.Label1.TabIndex = 131
        Me.Label1.Text = "Seleccionar"
        '
        'lblCaja
        '
        Me.lblCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaja.Location = New System.Drawing.Point(338, 30)
        Me.lblCaja.Name = "lblCaja"
        Me.lblCaja.Size = New System.Drawing.Size(356, 22)
        Me.lblCaja.TabIndex = 1
        Me.lblCaja.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTituloCaja
        '
        Me.lblTituloCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTituloCaja.Location = New System.Drawing.Point(338, 8)
        Me.lblTituloCaja.Name = "lblTituloCaja"
        Me.lblTituloCaja.Size = New System.Drawing.Size(356, 22)
        Me.lblTituloCaja.TabIndex = 0
        Me.lblTituloCaja.Text = "Depositar movimientos desde caja : "
        Me.lblTituloCaja.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpTotales
        '
        Me.grpTotales.Controls.Add(Me.txtTickets)
        Me.grpTotales.Controls.Add(Me.Label11)
        Me.grpTotales.Controls.Add(Me.txtSaldoSel)
        Me.grpTotales.Controls.Add(Me.txtDocSel)
        Me.grpTotales.Controls.Add(Me.Label4)
        Me.grpTotales.Controls.Add(Me.Label2)
        Me.grpTotales.Location = New System.Drawing.Point(2, 307)
        Me.grpTotales.Name = "grpTotales"
        Me.grpTotales.Size = New System.Drawing.Size(728, 43)
        Me.grpTotales.TabIndex = 83
        Me.grpTotales.TabStop = False
        '
        'txtTickets
        '
        Me.txtTickets.BackColor = System.Drawing.Color.MintCream
        Me.txtTickets.ForeColor = System.Drawing.Color.Navy
        Me.txtTickets.Location = New System.Drawing.Point(315, 16)
        Me.txtTickets.Name = "txtTickets"
        Me.txtTickets.Size = New System.Drawing.Size(42, 20)
        Me.txtTickets.TabIndex = 8
        Me.txtTickets.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(226, 15)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(78, 17)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Nº Tickets"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSaldoSel
        '
        Me.txtSaldoSel.BackColor = System.Drawing.Color.MintCream
        Me.txtSaldoSel.ForeColor = System.Drawing.Color.Navy
        Me.txtSaldoSel.Location = New System.Drawing.Point(588, 16)
        Me.txtSaldoSel.Name = "txtSaldoSel"
        Me.txtSaldoSel.Size = New System.Drawing.Size(134, 20)
        Me.txtSaldoSel.TabIndex = 6
        Me.txtSaldoSel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDocSel
        '
        Me.txtDocSel.BackColor = System.Drawing.Color.MintCream
        Me.txtDocSel.ForeColor = System.Drawing.Color.Navy
        Me.txtDocSel.Location = New System.Drawing.Point(178, 15)
        Me.txtDocSel.Name = "txtDocSel"
        Me.txtDocSel.Size = New System.Drawing.Size(42, 20)
        Me.txtDocSel.TabIndex = 5
        Me.txtDocSel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(450, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(132, 20)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Saldo seleccionado :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(166, 17)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Documentos seleccionados :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grptextos
        '
        Me.grptextos.Controls.Add(Me.txtEmision)
        Me.grptextos.Controls.Add(Me.txtTotalDeposito)
        Me.grptextos.Controls.Add(Me.Label14)
        Me.grptextos.Controls.Add(Me.Label13)
        Me.grptextos.Controls.Add(Me.Label12)
        Me.grptextos.Controls.Add(Me.txtIVA)
        Me.grptextos.Controls.Add(Me.txtComision)
        Me.grptextos.Controls.Add(Me.txtNumControl)
        Me.grptextos.Controls.Add(Me.Label10)
        Me.grptextos.Controls.Add(Me.txtCargos)
        Me.grptextos.Controls.Add(Me.Label9)
        Me.grptextos.Controls.Add(Me.Label8)
        Me.grptextos.Controls.Add(Me.txtISRL)
        Me.grptextos.Controls.Add(Me.txtAjustes)
        Me.grptextos.Controls.Add(Me.txtConcepto)
        Me.grptextos.Controls.Add(Me.txtDeposito)
        Me.grptextos.Controls.Add(Me.Label7)
        Me.grptextos.Controls.Add(Me.Label6)
        Me.grptextos.Controls.Add(Me.Label5)
        Me.grptextos.Controls.Add(Me.Label3)
        Me.grptextos.Location = New System.Drawing.Point(1, 345)
        Me.grptextos.Name = "grptextos"
        Me.grptextos.Size = New System.Drawing.Size(728, 140)
        Me.grptextos.TabIndex = 84
        Me.grptextos.TabStop = False
        '
        'txtTotalDeposito
        '
        Me.txtTotalDeposito.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtTotalDeposito.Enabled = False
        Me.txtTotalDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalDeposito.Location = New System.Drawing.Point(588, 116)
        Me.txtTotalDeposito.Name = "txtTotalDeposito"
        Me.txtTotalDeposito.Size = New System.Drawing.Size(134, 20)
        Me.txtTotalDeposito.TabIndex = 116
        Me.txtTotalDeposito.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(453, 116)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(129, 20)
        Me.Label14.TabIndex = 115
        Me.Label14.Text = "Total Depósito :"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(521, 13)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(61, 15)
        Me.Label13.TabIndex = 114
        Me.Label13.Text = "Ajustes :"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(521, 55)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 15)
        Me.Label12.TabIndex = 113
        Me.Label12.Text = "I.S.L.R. :"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtIVA
        '
        Me.txtIVA.Enabled = False
        Me.txtIVA.Location = New System.Drawing.Point(588, 95)
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(134, 20)
        Me.txtIVA.TabIndex = 112
        Me.txtIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtComision
        '
        Me.txtComision.Enabled = False
        Me.txtComision.Location = New System.Drawing.Point(588, 32)
        Me.txtComision.Name = "txtComision"
        Me.txtComision.Size = New System.Drawing.Size(134, 20)
        Me.txtComision.TabIndex = 111
        Me.txtComision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNumControl
        '
        Me.txtNumControl.Location = New System.Drawing.Point(72, 96)
        Me.txtNumControl.Name = "txtNumControl"
        Me.txtNumControl.Size = New System.Drawing.Size(148, 20)
        Me.txtNumControl.TabIndex = 110
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(11, 99)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(61, 15)
        Me.Label10.TabIndex = 109
        Me.Label10.Text = "Nª Control :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCargos
        '
        Me.txtCargos.Enabled = False
        Me.txtCargos.Location = New System.Drawing.Point(588, 74)
        Me.txtCargos.Name = "txtCargos"
        Me.txtCargos.Size = New System.Drawing.Size(134, 20)
        Me.txtCargos.TabIndex = 108
        Me.txtCargos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(521, 34)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(61, 15)
        Me.Label9.TabIndex = 107
        Me.Label9.Text = "Comisión :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(521, 76)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 15)
        Me.Label8.TabIndex = 106
        Me.Label8.Text = "Cargos :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtISRL
        '
        Me.txtISRL.Enabled = False
        Me.txtISRL.Location = New System.Drawing.Point(588, 53)
        Me.txtISRL.Name = "txtISRL"
        Me.txtISRL.Size = New System.Drawing.Size(134, 20)
        Me.txtISRL.TabIndex = 105
        Me.txtISRL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAjustes
        '
        Me.txtAjustes.Enabled = False
        Me.txtAjustes.Location = New System.Drawing.Point(588, 11)
        Me.txtAjustes.Name = "txtAjustes"
        Me.txtAjustes.Size = New System.Drawing.Size(134, 20)
        Me.txtAjustes.TabIndex = 7
        Me.txtAjustes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtConcepto
        '
        Me.txtConcepto.Location = New System.Drawing.Point(72, 53)
        Me.txtConcepto.Multiline = True
        Me.txtConcepto.Name = "txtConcepto"
        Me.txtConcepto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtConcepto.Size = New System.Drawing.Size(402, 42)
        Me.txtConcepto.TabIndex = 6
        '
        'txtDeposito
        '
        Me.txtDeposito.Location = New System.Drawing.Point(72, 11)
        Me.txtDeposito.Name = "txtDeposito"
        Me.txtDeposito.Size = New System.Drawing.Size(148, 20)
        Me.txtDeposito.TabIndex = 4
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(493, 94)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(89, 20)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "IVA :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(5, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 15)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Concepto :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(-2, 38)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 17)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Emisión :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(-3, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 17)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Nº depósito :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(573, 488)
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
        'txtEmision
        '
        Me.txtEmision.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtEmision.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtEmision.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtEmision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmision.Location = New System.Drawing.Point(72, 32)
        Me.txtEmision.Name = "txtEmision"
        Me.txtEmision.Size = New System.Drawing.Size(114, 19)
        Me.txtEmision.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtEmision.TabIndex = 214
        '
        'txtFechaSeleccion
        '
        Me.txtFechaSeleccion.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFechaSeleccion.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFechaSeleccion.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFechaSeleccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaSeleccion.Location = New System.Drawing.Point(111, 57)
        Me.txtFechaSeleccion.Name = "txtFechaSeleccion"
        Me.txtFechaSeleccion.Size = New System.Drawing.Size(123, 19)
        Me.txtFechaSeleccion.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFechaSeleccion.TabIndex = 214
        '
        'jsBanArcDepositarCaja
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(732, 515)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grptextos)
        Me.Controls.Add(Me.grpTotales)
        Me.Controls.Add(Me.grpCaja)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsBanArcDepositarCaja"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Depositar movimientos desde caja"
        Me.grpCaja.ResumeLayout(False)
        Me.grpCaja.PerformLayout()
        Me.grpTotales.ResumeLayout(False)
        Me.grpTotales.PerformLayout()
        Me.grptextos.ResumeLayout(False)
        Me.grptextos.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents lv As System.Windows.Forms.ListView
    Friend WithEvents grpCaja As System.Windows.Forms.GroupBox
    Friend WithEvents lblTituloCaja As System.Windows.Forms.Label
    Friend WithEvents grpTotales As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grptextos As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtAjustes As System.Windows.Forms.TextBox
    Friend WithEvents txtConcepto As System.Windows.Forms.TextBox
    Friend WithEvents txtDeposito As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSaldoSel As System.Windows.Forms.TextBox
    Friend WithEvents txtDocSel As System.Windows.Forms.TextBox
    Friend WithEvents txtISRL As System.Windows.Forms.TextBox
    Friend WithEvents txtCargos As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtTickets As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtComision As System.Windows.Forms.TextBox
    Friend WithEvents txtNumControl As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalDeposito As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblCaja As System.Windows.Forms.Label
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents cmbSeleccion As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbFP As System.Windows.Forms.ComboBox
    Friend WithEvents txtRP As System.Windows.Forms.TextBox
    Friend WithEvents btnBP As System.Windows.Forms.Button
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents txtEmision As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents txtFechaSeleccion As Syncfusion.WinForms.Input.SfDateTimeEdit
End Class
