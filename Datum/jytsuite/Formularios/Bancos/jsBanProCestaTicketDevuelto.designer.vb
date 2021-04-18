<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsBanProCestaTicketDevuelto
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsBanProCestaTicketDevuelto))
        Me.lblInfo = New System.Windows.Forms.Label
        Me.grp = New System.Windows.Forms.GroupBox
        Me.btnImprimir = New System.Windows.Forms.Button
        Me.Label16 = New System.Windows.Forms.Label
        Me.txtEstatus = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtAsesor = New System.Windows.Forms.TextBox
        Me.txtFechaRemesa = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtFechaDep = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtCancelacion = New System.Windows.Forms.TextBox
        Me.txtCliente = New System.Windows.Forms.TextBox
        Me.txtDeposito = New System.Windows.Forms.TextBox
        Me.txtBanco = New System.Windows.Forms.TextBox
        Me.txtMonto = New System.Windows.Forms.TextBox
        Me.txtRemesa = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnCheque = New System.Windows.Forms.Button
        Me.txtCheque = New System.Windows.Forms.TextBox
        Me.btnFecha = New System.Windows.Forms.Button
        Me.txtFecha = New System.Windows.Forms.TextBox
        Me.cmbCorredor = New System.Windows.Forms.ComboBox
        Me.grpTotales = New System.Windows.Forms.GroupBox
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblProgreso = New System.Windows.Forms.Label
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.lblLeyenda = New System.Windows.Forms.Label
        Me.grpLeyenda = New System.Windows.Forms.GroupBox
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox
        Me.grp.SuspendLayout()
        Me.grpTotales.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.grpLeyenda.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Location = New System.Drawing.Point(2, 416)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(566, 21)
        Me.lblInfo.TabIndex = 80
        '
        'grp
        '
        Me.grp.BackColor = System.Drawing.SystemColors.Control
        Me.grp.Controls.Add(Me.btnImprimir)
        Me.grp.Controls.Add(Me.Label16)
        Me.grp.Controls.Add(Me.txtEstatus)
        Me.grp.Controls.Add(Me.Label12)
        Me.grp.Controls.Add(Me.txtAsesor)
        Me.grp.Controls.Add(Me.txtFechaRemesa)
        Me.grp.Controls.Add(Me.Label15)
        Me.grp.Controls.Add(Me.txtFechaDep)
        Me.grp.Controls.Add(Me.Label14)
        Me.grp.Controls.Add(Me.Label13)
        Me.grp.Controls.Add(Me.txtCancelacion)
        Me.grp.Controls.Add(Me.txtCliente)
        Me.grp.Controls.Add(Me.txtDeposito)
        Me.grp.Controls.Add(Me.txtBanco)
        Me.grp.Controls.Add(Me.txtMonto)
        Me.grp.Controls.Add(Me.txtRemesa)
        Me.grp.Controls.Add(Me.Label11)
        Me.grp.Controls.Add(Me.Label8)
        Me.grp.Controls.Add(Me.Label7)
        Me.grp.Controls.Add(Me.Label6)
        Me.grp.Controls.Add(Me.Label5)
        Me.grp.Controls.Add(Me.Label4)
        Me.grp.Controls.Add(Me.Label2)
        Me.grp.Controls.Add(Me.Label1)
        Me.grp.Controls.Add(Me.btnCheque)
        Me.grp.Controls.Add(Me.txtCheque)
        Me.grp.Controls.Add(Me.btnFecha)
        Me.grp.Controls.Add(Me.txtFecha)
        Me.grp.Controls.Add(Me.cmbCorredor)
        Me.grp.Location = New System.Drawing.Point(2, 152)
        Me.grp.Name = "grp"
        Me.grp.Size = New System.Drawing.Size(730, 206)
        Me.grp.TabIndex = 82
        Me.grp.TabStop = False
        '
        'btnImprimir
        '
        Me.btnImprimir.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnImprimir.Enabled = False
        Me.btnImprimir.Image = Global.Datum.My.Resources.Resources.Imprimir
        Me.btnImprimir.Location = New System.Drawing.Point(570, 163)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(158, 43)
        Me.btnImprimir.TabIndex = 147
        Me.btnImprimir.Text = "Imprimir comprobante"
        Me.btnImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(324, 59)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(118, 20)
        Me.Label16.TabIndex = 146
        Me.Label16.Text = "Estatus :"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtEstatus
        '
        Me.txtEstatus.BackColor = System.Drawing.Color.White
        Me.txtEstatus.Enabled = False
        Me.txtEstatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstatus.Location = New System.Drawing.Point(462, 59)
        Me.txtEstatus.Name = "txtEstatus"
        Me.txtEstatus.Size = New System.Drawing.Size(102, 20)
        Me.txtEstatus.TabIndex = 145
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(29, 184)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(136, 20)
        Me.Label12.TabIndex = 144
        Me.Label12.Text = "Asesor :"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAsesor
        '
        Me.txtAsesor.BackColor = System.Drawing.Color.White
        Me.txtAsesor.Enabled = False
        Me.txtAsesor.Location = New System.Drawing.Point(166, 185)
        Me.txtAsesor.Name = "txtAsesor"
        Me.txtAsesor.Size = New System.Drawing.Size(116, 20)
        Me.txtAsesor.TabIndex = 143
        '
        'txtFechaRemesa
        '
        Me.txtFechaRemesa.BackColor = System.Drawing.Color.White
        Me.txtFechaRemesa.Enabled = False
        Me.txtFechaRemesa.Location = New System.Drawing.Point(462, 122)
        Me.txtFechaRemesa.Name = "txtFechaRemesa"
        Me.txtFechaRemesa.Size = New System.Drawing.Size(102, 20)
        Me.txtFechaRemesa.TabIndex = 142
        Me.txtFechaRemesa.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(288, 122)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(154, 20)
        Me.Label15.TabIndex = 141
        Me.Label15.Text = "Fecha Remesa :"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFechaDep
        '
        Me.txtFechaDep.BackColor = System.Drawing.Color.White
        Me.txtFechaDep.Enabled = False
        Me.txtFechaDep.Location = New System.Drawing.Point(462, 101)
        Me.txtFechaDep.Name = "txtFechaDep"
        Me.txtFechaDep.Size = New System.Drawing.Size(102, 20)
        Me.txtFechaDep.TabIndex = 140
        Me.txtFechaDep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(288, 100)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(154, 20)
        Me.Label14.TabIndex = 139
        Me.Label14.Text = "Fecha depósito :"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(28, 122)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(136, 20)
        Me.Label13.TabIndex = 138
        Me.Label13.Text = "Nº Remesa :"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCancelacion
        '
        Me.txtCancelacion.BackColor = System.Drawing.Color.White
        Me.txtCancelacion.Enabled = False
        Me.txtCancelacion.Location = New System.Drawing.Point(166, 143)
        Me.txtCancelacion.Name = "txtCancelacion"
        Me.txtCancelacion.Size = New System.Drawing.Size(116, 20)
        Me.txtCancelacion.TabIndex = 135
        '
        'txtCliente
        '
        Me.txtCliente.BackColor = System.Drawing.Color.White
        Me.txtCliente.Enabled = False
        Me.txtCliente.Location = New System.Drawing.Point(166, 164)
        Me.txtCliente.Name = "txtCliente"
        Me.txtCliente.Size = New System.Drawing.Size(398, 20)
        Me.txtCliente.TabIndex = 134
        '
        'txtDeposito
        '
        Me.txtDeposito.BackColor = System.Drawing.Color.White
        Me.txtDeposito.Enabled = False
        Me.txtDeposito.Location = New System.Drawing.Point(166, 101)
        Me.txtDeposito.Name = "txtDeposito"
        Me.txtDeposito.Size = New System.Drawing.Size(116, 20)
        Me.txtDeposito.TabIndex = 133
        '
        'txtBanco
        '
        Me.txtBanco.BackColor = System.Drawing.Color.White
        Me.txtBanco.Enabled = False
        Me.txtBanco.Location = New System.Drawing.Point(166, 80)
        Me.txtBanco.Name = "txtBanco"
        Me.txtBanco.Size = New System.Drawing.Size(398, 20)
        Me.txtBanco.TabIndex = 132
        '
        'txtMonto
        '
        Me.txtMonto.BackColor = System.Drawing.Color.White
        Me.txtMonto.Enabled = False
        Me.txtMonto.Location = New System.Drawing.Point(166, 59)
        Me.txtMonto.Name = "txtMonto"
        Me.txtMonto.Size = New System.Drawing.Size(116, 20)
        Me.txtMonto.TabIndex = 131
        Me.txtMonto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRemesa
        '
        Me.txtRemesa.BackColor = System.Drawing.Color.White
        Me.txtRemesa.Enabled = False
        Me.txtRemesa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemesa.Location = New System.Drawing.Point(166, 122)
        Me.txtRemesa.Name = "txtRemesa"
        Me.txtRemesa.Size = New System.Drawing.Size(116, 20)
        Me.txtRemesa.TabIndex = 130
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(29, 142)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(135, 20)
        Me.Label11.TabIndex = 127
        Me.Label11.Text = "Nº Cancelación :"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(28, 163)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(136, 20)
        Me.Label8.TabIndex = 126
        Me.Label8.Text = "Cliente :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 100)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(154, 20)
        Me.Label7.TabIndex = 125
        Me.Label7.Text = "Nº depósito :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(26, 79)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(136, 20)
        Me.Label6.TabIndex = 124
        Me.Label6.Text = "Banco depósito :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(26, 58)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(136, 20)
        Me.Label5.TabIndex = 123
        Me.Label5.Text = "Monto cheque :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(468, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 20)
        Me.Label4.TabIndex = 122
        Me.Label4.Text = "Fecha devolución :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(156, 20)
        Me.Label2.TabIndex = 121
        Me.Label2.Text = "Nº cheque alimentación :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(158, 20)
        Me.Label1.TabIndex = 120
        Me.Label1.Text = "Corredor Cheque alim.:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnCheque
        '
        Me.btnCheque.AutoSize = True
        Me.btnCheque.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCheque.Image = CType(resources.GetObject("btnCheque.Image"), System.Drawing.Image)
        Me.btnCheque.Location = New System.Drawing.Point(401, 15)
        Me.btnCheque.Name = "btnCheque"
        Me.btnCheque.Size = New System.Drawing.Size(41, 38)
        Me.btnCheque.TabIndex = 119
        Me.btnCheque.UseVisualStyleBackColor = True
        '
        'txtCheque
        '
        Me.txtCheque.BackColor = System.Drawing.Color.White
        Me.txtCheque.Enabled = False
        Me.txtCheque.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheque.Location = New System.Drawing.Point(166, 33)
        Me.txtCheque.Name = "txtCheque"
        Me.txtCheque.Size = New System.Drawing.Size(230, 20)
        Me.txtCheque.TabIndex = 0
        '
        'btnFecha
        '
        Me.btnFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFecha.Location = New System.Drawing.Point(697, 9)
        Me.btnFecha.Name = "btnFecha"
        Me.btnFecha.Size = New System.Drawing.Size(25, 20)
        Me.btnFecha.TabIndex = 117
        Me.btnFecha.Text = "•••"
        Me.btnFecha.UseVisualStyleBackColor = True
        '
        'txtFecha
        '
        Me.txtFecha.BackColor = System.Drawing.Color.White
        Me.txtFecha.Enabled = False
        Me.txtFecha.Location = New System.Drawing.Point(592, 8)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(99, 20)
        Me.txtFecha.TabIndex = 115
        Me.txtFecha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbCorredor
        '
        Me.cmbCorredor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCorredor.FormattingEnabled = True
        Me.cmbCorredor.Location = New System.Drawing.Point(166, 8)
        Me.cmbCorredor.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbCorredor.Name = "cmbCorredor"
        Me.cmbCorredor.Size = New System.Drawing.Size(201, 21)
        Me.cmbCorredor.TabIndex = 105
        '
        'grpTotales
        '
        Me.grpTotales.BackColor = System.Drawing.SystemColors.Control
        Me.grpTotales.Controls.Add(Me.ProgressBar1)
        Me.grpTotales.Controls.Add(Me.Label3)
        Me.grpTotales.Controls.Add(Me.lblProgreso)
        Me.grpTotales.Enabled = False
        Me.grpTotales.Location = New System.Drawing.Point(2, 351)
        Me.grpTotales.Name = "grpTotales"
        Me.grpTotales.Size = New System.Drawing.Size(728, 62)
        Me.grpTotales.TabIndex = 83
        Me.grpTotales.TabStop = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(11, 33)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(711, 20)
        Me.ProgressBar1.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 20)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Progreso ..."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProgreso
        '
        Me.lblProgreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgreso.Location = New System.Drawing.Point(86, 10)
        Me.lblProgreso.Name = "lblProgreso"
        Me.lblProgreso.Size = New System.Drawing.Size(635, 20)
        Me.lblProgreso.TabIndex = 14
        Me.lblProgreso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(567, 410)
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
        Me.Label9.Text = "Bancos : Devolución Cesta Ticket "
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
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLeyenda
        '
        Me.lblLeyenda.BackColor = System.Drawing.Color.White
        Me.lblLeyenda.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLeyenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeyenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.lblLeyenda.Location = New System.Drawing.Point(11, 16)
        Me.lblLeyenda.Name = "lblLeyenda"
        Me.lblLeyenda.Size = New System.Drawing.Size(707, 73)
        Me.lblLeyenda.TabIndex = 89
        Me.lblLeyenda.Text = " "
        '
        'grpLeyenda
        '
        Me.grpLeyenda.BackColor = System.Drawing.SystemColors.Control
        Me.grpLeyenda.Controls.Add(Me.lblLeyenda)
        Me.grpLeyenda.Location = New System.Drawing.Point(2, 60)
        Me.grpLeyenda.Name = "grpLeyenda"
        Me.grpLeyenda.Size = New System.Drawing.Size(728, 97)
        Me.grpLeyenda.TabIndex = 90
        Me.grpLeyenda.TabStop = False
        '
        'C1PictureBox1
        '
        Me.C1PictureBox1.Image = Global.Datum.My.Resources.Resources.banda_amarilla
        Me.C1PictureBox1.Location = New System.Drawing.Point(92, 1)
        Me.C1PictureBox1.Name = "C1PictureBox1"
        Me.C1PictureBox1.Size = New System.Drawing.Size(639, 61)
        Me.C1PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.C1PictureBox1.TabIndex = 86
        Me.C1PictureBox1.TabStop = False
        '
        'jsBanProCestaTicketDevuelto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(732, 438)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpLeyenda)
        Me.Controls.Add(Me.grp)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpTotales)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsBanProCestaTicketDevuelto"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Cheques devueltos"
        Me.grp.ResumeLayout(False)
        Me.grp.PerformLayout()
        Me.grpTotales.ResumeLayout(False)
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.grpLeyenda.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grp As System.Windows.Forms.GroupBox
    Friend WithEvents grpTotales As System.Windows.Forms.GroupBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cmbCorredor As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblProgreso As System.Windows.Forms.Label
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblLeyenda As System.Windows.Forms.Label
    Friend WithEvents grpLeyenda As System.Windows.Forms.GroupBox
    Friend WithEvents txtFecha As System.Windows.Forms.TextBox
    Friend WithEvents btnCheque As System.Windows.Forms.Button
    Friend WithEvents txtCheque As System.Windows.Forms.TextBox
    Friend WithEvents btnFecha As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtCliente As System.Windows.Forms.TextBox
    Friend WithEvents txtDeposito As System.Windows.Forms.TextBox
    Friend WithEvents txtBanco As System.Windows.Forms.TextBox
    Friend WithEvents txtMonto As System.Windows.Forms.TextBox
    Friend WithEvents txtRemesa As System.Windows.Forms.TextBox
    Friend WithEvents txtCancelacion As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtFechaDep As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtFechaRemesa As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtAsesor As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtEstatus As System.Windows.Forms.TextBox
    Friend WithEvents btnImprimir As System.Windows.Forms.Button
End Class
