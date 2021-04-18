<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsPOSRetencionIVA
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsPOSRetencionIVA))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.txtSubTotalFactura = New System.Windows.Forms.TextBox()
        Me.txtIVA = New System.Windows.Forms.TextBox()
        Me.txtTotalFactura = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtFechaFactura = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtNombreCliente = New System.Windows.Forms.TextBox()
        Me.txtRIF = New System.Windows.Forms.TextBox()
        Me.txtDocumentoInterno = New System.Windows.Forms.TextBox()
        Me.txtNumeroSerial = New System.Windows.Forms.TextBox()
        Me.txtFacturaFiscal = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.grpRetencion = New System.Windows.Forms.GroupBox()
        Me.btnFechaRetencion = New System.Windows.Forms.Button()
        Me.txtFechaRecepcion = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtMontoRetencion = New System.Windows.Forms.TextBox()
        Me.txtPorcentajeRetencion = New System.Windows.Forms.TextBox()
        Me.txtNumeroRetencion = New System.Windows.Forms.TextBox()
        Me.txtFechaRetencion = New System.Windows.Forms.TextBox()
        Me.grpTarjeta.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.grpRetencion.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 459)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(993, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.txtSubTotalFactura)
        Me.grpTarjeta.Controls.Add(Me.txtIVA)
        Me.grpTarjeta.Controls.Add(Me.txtTotalFactura)
        Me.grpTarjeta.Controls.Add(Me.Label14)
        Me.grpTarjeta.Controls.Add(Me.Label13)
        Me.grpTarjeta.Controls.Add(Me.Label12)
        Me.grpTarjeta.Controls.Add(Me.txtFechaFactura)
        Me.grpTarjeta.Controls.Add(Me.Label11)
        Me.grpTarjeta.Controls.Add(Me.txtNombreCliente)
        Me.grpTarjeta.Controls.Add(Me.txtRIF)
        Me.grpTarjeta.Controls.Add(Me.txtDocumentoInterno)
        Me.grpTarjeta.Controls.Add(Me.txtNumeroSerial)
        Me.grpTarjeta.Controls.Add(Me.txtFacturaFiscal)
        Me.grpTarjeta.Controls.Add(Me.Label10)
        Me.grpTarjeta.Controls.Add(Me.Label9)
        Me.grpTarjeta.Controls.Add(Me.btnBuscar)
        Me.grpTarjeta.Controls.Add(Me.Label6)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpTarjeta.Location = New System.Drawing.Point(0, -2)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(989, 245)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        Me.grpTarjeta.Text = "Información Factura"
        '
        'txtSubTotalFactura
        '
        Me.txtSubTotalFactura.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubTotalFactura.Location = New System.Drawing.Point(117, 200)
        Me.txtSubTotalFactura.Multiline = True
        Me.txtSubTotalFactura.Name = "txtSubTotalFactura"
        Me.txtSubTotalFactura.Size = New System.Drawing.Size(202, 31)
        Me.txtSubTotalFactura.TabIndex = 124
        Me.txtSubTotalFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtIVA
        '
        Me.txtIVA.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIVA.Location = New System.Drawing.Point(440, 200)
        Me.txtIVA.Multiline = True
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(202, 31)
        Me.txtIVA.TabIndex = 123
        Me.txtIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalFactura
        '
        Me.txtTotalFactura.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalFactura.Location = New System.Drawing.Point(774, 200)
        Me.txtTotalFactura.Multiline = True
        Me.txtTotalFactura.Name = "txtTotalFactura"
        Me.txtTotalFactura.Size = New System.Drawing.Size(202, 31)
        Me.txtTotalFactura.TabIndex = 122
        Me.txtTotalFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label14.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(669, 200)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(99, 31)
        Me.Label14.TabIndex = 121
        Me.Label14.Text = "Total"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label13.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(335, 200)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(99, 31)
        Me.Label13.TabIndex = 120
        Me.Label13.Text = "IVA"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label12.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(12, 200)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(99, 31)
        Me.Label12.TabIndex = 119
        Me.Label12.Text = "SubTotal"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFechaFactura
        '
        Me.txtFechaFactura.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaFactura.Location = New System.Drawing.Point(774, 113)
        Me.txtFechaFactura.Multiline = True
        Me.txtFechaFactura.Name = "txtFechaFactura"
        Me.txtFechaFactura.Size = New System.Drawing.Size(202, 31)
        Me.txtFechaFactura.TabIndex = 118
        Me.txtFechaFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(542, 112)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(226, 31)
        Me.Label11.TabIndex = 117
        Me.Label11.Text = "Fecha Documento"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNombreCliente
        '
        Me.txtNombreCliente.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombreCliente.Location = New System.Drawing.Point(280, 146)
        Me.txtNombreCliente.Multiline = True
        Me.txtNombreCliente.Name = "txtNombreCliente"
        Me.txtNombreCliente.Size = New System.Drawing.Size(696, 31)
        Me.txtNombreCliente.TabIndex = 116
        '
        'txtRIF
        '
        Me.txtRIF.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRIF.Location = New System.Drawing.Point(280, 113)
        Me.txtRIF.Multiline = True
        Me.txtRIF.Name = "txtRIF"
        Me.txtRIF.Size = New System.Drawing.Size(203, 31)
        Me.txtRIF.TabIndex = 115
        '
        'txtDocumentoInterno
        '
        Me.txtDocumentoInterno.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocumentoInterno.Location = New System.Drawing.Point(774, 80)
        Me.txtDocumentoInterno.Multiline = True
        Me.txtDocumentoInterno.Name = "txtDocumentoInterno"
        Me.txtDocumentoInterno.Size = New System.Drawing.Size(202, 31)
        Me.txtDocumentoInterno.TabIndex = 114
        '
        'txtNumeroSerial
        '
        Me.txtNumeroSerial.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumeroSerial.Location = New System.Drawing.Point(774, 47)
        Me.txtNumeroSerial.Multiline = True
        Me.txtNumeroSerial.Name = "txtNumeroSerial"
        Me.txtNumeroSerial.Size = New System.Drawing.Size(202, 31)
        Me.txtNumeroSerial.TabIndex = 113
        '
        'txtFacturaFiscal
        '
        Me.txtFacturaFiscal.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFacturaFiscal.Location = New System.Drawing.Point(774, 14)
        Me.txtFacturaFiscal.Multiline = True
        Me.txtFacturaFiscal.Name = "txtFacturaFiscal"
        Me.txtFacturaFiscal.Size = New System.Drawing.Size(202, 31)
        Me.txtFacturaFiscal.TabIndex = 112
        '
        'Label10
        '
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label10.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(12, 146)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(262, 31)
        Me.Label10.TabIndex = 109
        Me.Label10.Text = "Nombre y/o Razón Social"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(12, 113)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(262, 31)
        Me.Label9.TabIndex = 108
        Me.Label9.Text = "RIF "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnBuscar
        '
        Me.btnBuscar.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnBuscar.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuscar.Image = Global.POS_Datum.My.Resources.Resources.Buscar
        Me.btnBuscar.Location = New System.Drawing.Point(280, 14)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(203, 64)
        Me.btnBuscar.TabIndex = 107
        Me.btnBuscar.Text = "Buscar Factura"
        Me.btnBuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'Label6
        '
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label6.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(542, 80)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(226, 31)
        Me.Label6.TabIndex = 106
        Me.Label6.Text = "Nº Doc. Interno"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(542, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(226, 31)
        Me.Label3.TabIndex = 98
        Me.Label3.Text = "Nº Serial"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(542, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(226, 31)
        Me.Label1.TabIndex = 96
        Me.Label1.Text = "Nº Factura Fiscal"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(549, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Nombre  :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(436, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(318, 31)
        Me.Label5.TabIndex = 102
        Me.Label5.Text = "Número Retención IVA"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(436, 75)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(318, 31)
        Me.Label4.TabIndex = 101
        Me.Label4.Text = "Fecha Retención IVA"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(828, 456)
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
        Me.btnCancel.Image = Global.POS_Datum.My.Resources.Resources.button_cancel
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
        Me.btnOK.Image = Global.POS_Datum.My.Resources.Resources.button_ok
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'Label7
        '
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(436, 141)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(318, 31)
        Me.Label7.TabIndex = 107
        Me.Label7.Text = "% Retención IVA"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label8.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(436, 174)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(318, 31)
        Me.Label8.TabIndex = 108
        Me.Label8.Text = "Monto total retención IVA"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpRetencion
        '
        Me.grpRetencion.Controls.Add(Me.btnFechaRetencion)
        Me.grpRetencion.Controls.Add(Me.txtFechaRecepcion)
        Me.grpRetencion.Controls.Add(Me.Label15)
        Me.grpRetencion.Controls.Add(Me.txtMontoRetencion)
        Me.grpRetencion.Controls.Add(Me.txtPorcentajeRetencion)
        Me.grpRetencion.Controls.Add(Me.txtNumeroRetencion)
        Me.grpRetencion.Controls.Add(Me.txtFechaRetencion)
        Me.grpRetencion.Controls.Add(Me.Label4)
        Me.grpRetencion.Controls.Add(Me.Label5)
        Me.grpRetencion.Controls.Add(Me.Label8)
        Me.grpRetencion.Controls.Add(Me.Label7)
        Me.grpRetencion.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRetencion.Location = New System.Drawing.Point(4, 242)
        Me.grpRetencion.Name = "grpRetencion"
        Me.grpRetencion.Size = New System.Drawing.Size(989, 211)
        Me.grpRetencion.TabIndex = 111
        Me.grpRetencion.TabStop = False
        Me.grpRetencion.Text = "Retención IVA"
        '
        'btnFechaRetencion
        '
        Me.btnFechaRetencion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechaRetencion.Location = New System.Drawing.Point(945, 54)
        Me.btnFechaRetencion.Name = "btnFechaRetencion"
        Me.btnFechaRetencion.Size = New System.Drawing.Size(27, 20)
        Me.btnFechaRetencion.TabIndex = 129
        Me.btnFechaRetencion.Text = "···"
        Me.btnFechaRetencion.UseVisualStyleBackColor = True
        '
        'txtFechaRecepcion
        '
        Me.txtFechaRecepcion.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaRecepcion.Location = New System.Drawing.Point(770, 22)
        Me.txtFechaRecepcion.Multiline = True
        Me.txtFechaRecepcion.Name = "txtFechaRecepcion"
        Me.txtFechaRecepcion.Size = New System.Drawing.Size(202, 31)
        Me.txtFechaRecepcion.TabIndex = 128
        Me.txtFechaRecepcion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label15
        '
        Me.Label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label15.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(436, 22)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(318, 31)
        Me.Label15.TabIndex = 127
        Me.Label15.Text = "Fecha RECEPCION documento"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMontoRetencion
        '
        Me.txtMontoRetencion.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMontoRetencion.Location = New System.Drawing.Point(770, 174)
        Me.txtMontoRetencion.Multiline = True
        Me.txtMontoRetencion.Name = "txtMontoRetencion"
        Me.txtMontoRetencion.Size = New System.Drawing.Size(202, 31)
        Me.txtMontoRetencion.TabIndex = 126
        Me.txtMontoRetencion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPorcentajeRetencion
        '
        Me.txtPorcentajeRetencion.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPorcentajeRetencion.Location = New System.Drawing.Point(770, 141)
        Me.txtPorcentajeRetencion.Multiline = True
        Me.txtPorcentajeRetencion.Name = "txtPorcentajeRetencion"
        Me.txtPorcentajeRetencion.Size = New System.Drawing.Size(202, 31)
        Me.txtPorcentajeRetencion.TabIndex = 125
        Me.txtPorcentajeRetencion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNumeroRetencion
        '
        Me.txtNumeroRetencion.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumeroRetencion.Location = New System.Drawing.Point(770, 108)
        Me.txtNumeroRetencion.Multiline = True
        Me.txtNumeroRetencion.Name = "txtNumeroRetencion"
        Me.txtNumeroRetencion.Size = New System.Drawing.Size(202, 31)
        Me.txtNumeroRetencion.TabIndex = 124
        '
        'txtFechaRetencion
        '
        Me.txtFechaRetencion.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaRetencion.Location = New System.Drawing.Point(770, 75)
        Me.txtFechaRetencion.Multiline = True
        Me.txtFechaRetencion.Name = "txtFechaRetencion"
        Me.txtFechaRetencion.Size = New System.Drawing.Size(202, 31)
        Me.txtFechaRetencion.TabIndex = 123
        Me.txtFechaRetencion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'jsPOSRetencionIVA
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(993, 485)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpRetencion)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsPOSRetencionIVA"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RETENCION IMPUESTO AL VALOR AGREGADO (IVA)"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.grpRetencion.ResumeLayout(False)
        Me.grpRetencion.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
    Friend WithEvents grpRetencion As System.Windows.Forms.GroupBox
    Friend WithEvents txtDocumentoInterno As System.Windows.Forms.TextBox
    Friend WithEvents txtNumeroSerial As System.Windows.Forms.TextBox
    Friend WithEvents txtFacturaFiscal As System.Windows.Forms.TextBox
    Friend WithEvents txtRIF As System.Windows.Forms.TextBox
    Friend WithEvents txtNombreCliente As System.Windows.Forms.TextBox
    Friend WithEvents txtFechaFactura As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtSubTotalFactura As System.Windows.Forms.TextBox
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalFactura As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtPorcentajeRetencion As System.Windows.Forms.TextBox
    Friend WithEvents txtNumeroRetencion As System.Windows.Forms.TextBox
    Friend WithEvents txtFechaRetencion As System.Windows.Forms.TextBox
    Friend WithEvents txtMontoRetencion As System.Windows.Forms.TextBox
    Friend WithEvents txtFechaRecepcion As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents btnFechaRetencion As System.Windows.Forms.Button
End Class
