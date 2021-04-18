<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsPOSRecambioMercancia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsPOSRecambioMercancia))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.txtUnidadADevolver = New System.Windows.Forms.TextBox()
        Me.txtMercanciaADevolver = New System.Windows.Forms.TextBox()
        Me.txtCantidadADevolver = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnMercanciaDevolucion = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCodigoMercanciaADevolver = New System.Windows.Forms.TextBox()
        Me.txtNumeroFactura = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnFacturas = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtUnidadSalida = New System.Windows.Forms.TextBox()
        Me.txtMercanciaSalida = New System.Windows.Forms.TextBox()
        Me.txtCantidadSalida = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnMercanciaSalida = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCodigoMercanciaSalida = New System.Windows.Forms.TextBox()
        Me.grpTarjeta.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 437)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(903, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.txtUnidadADevolver)
        Me.grpTarjeta.Controls.Add(Me.txtMercanciaADevolver)
        Me.grpTarjeta.Controls.Add(Me.txtCantidadADevolver)
        Me.grpTarjeta.Controls.Add(Me.Label6)
        Me.grpTarjeta.Controls.Add(Me.btnMercanciaDevolucion)
        Me.grpTarjeta.Controls.Add(Me.Label5)
        Me.grpTarjeta.Controls.Add(Me.Label4)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.txtCodigoMercanciaADevolver)
        Me.grpTarjeta.Controls.Add(Me.txtNumeroFactura)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Controls.Add(Me.btnFacturas)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(899, 209)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        Me.grpTarjeta.Text = "Mercancía Entrada"
        '
        'txtUnidadADevolver
        '
        Me.txtUnidadADevolver.BackColor = System.Drawing.Color.White
        Me.txtUnidadADevolver.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUnidadADevolver.Location = New System.Drawing.Point(403, 134)
        Me.txtUnidadADevolver.MaxLength = 25
        Me.txtUnidadADevolver.Name = "txtUnidadADevolver"
        Me.txtUnidadADevolver.Size = New System.Drawing.Size(67, 32)
        Me.txtUnidadADevolver.TabIndex = 109
        Me.txtUnidadADevolver.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMercanciaADevolver
        '
        Me.txtMercanciaADevolver.BackColor = System.Drawing.Color.White
        Me.txtMercanciaADevolver.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMercanciaADevolver.Location = New System.Drawing.Point(403, 95)
        Me.txtMercanciaADevolver.MaxLength = 25
        Me.txtMercanciaADevolver.Name = "txtMercanciaADevolver"
        Me.txtMercanciaADevolver.Size = New System.Drawing.Size(490, 32)
        Me.txtMercanciaADevolver.TabIndex = 108
        '
        'txtCantidadADevolver
        '
        Me.txtCantidadADevolver.BackColor = System.Drawing.Color.White
        Me.txtCantidadADevolver.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCantidadADevolver.Location = New System.Drawing.Point(403, 169)
        Me.txtCantidadADevolver.MaxLength = 25
        Me.txtCantidadADevolver.Name = "txtCantidadADevolver"
        Me.txtCantidadADevolver.Size = New System.Drawing.Size(208, 32)
        Me.txtCantidadADevolver.TabIndex = 107
        '
        'Label6
        '
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label6.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(6, 170)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(391, 31)
        Me.Label6.TabIndex = 106
        Me.Label6.Text = "Cantidad :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnMercanciaDevolucion
        '
        Me.btnMercanciaDevolucion.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMercanciaDevolucion.Location = New System.Drawing.Point(617, 73)
        Me.btnMercanciaDevolucion.Name = "btnMercanciaDevolucion"
        Me.btnMercanciaDevolucion.Size = New System.Drawing.Size(25, 20)
        Me.btnMercanciaDevolucion.TabIndex = 105
        Me.btnMercanciaDevolucion.Text = "•••"
        Me.btnMercanciaDevolucion.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(6, 134)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(391, 31)
        Me.Label5.TabIndex = 102
        Me.Label5.Text = "Unidad :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(6, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(391, 31)
        Me.Label4.TabIndex = 101
        Me.Label4.Text = "Descripción :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(6, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(391, 31)
        Me.Label3.TabIndex = 98
        Me.Label3.Text = "Código de barras mercancía :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtCodigoMercanciaADevolver
        '
        Me.txtCodigoMercanciaADevolver.BackColor = System.Drawing.Color.White
        Me.txtCodigoMercanciaADevolver.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigoMercanciaADevolver.Location = New System.Drawing.Point(403, 59)
        Me.txtCodigoMercanciaADevolver.MaxLength = 25
        Me.txtCodigoMercanciaADevolver.Name = "txtCodigoMercanciaADevolver"
        Me.txtCodigoMercanciaADevolver.Size = New System.Drawing.Size(208, 32)
        Me.txtCodigoMercanciaADevolver.TabIndex = 97
        '
        'txtNumeroFactura
        '
        Me.txtNumeroFactura.BackColor = System.Drawing.Color.White
        Me.txtNumeroFactura.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumeroFactura.Location = New System.Drawing.Point(403, 23)
        Me.txtNumeroFactura.MaxLength = 20
        Me.txtNumeroFactura.Name = "txtNumeroFactura"
        Me.txtNumeroFactura.Size = New System.Drawing.Size(208, 32)
        Me.txtNumeroFactura.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(6, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(391, 31)
        Me.Label1.TabIndex = 96
        Me.Label1.Text = "Nº Factura de sistema :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnFacturas
        '
        Me.btnFacturas.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFacturas.Location = New System.Drawing.Point(617, 35)
        Me.btnFacturas.Name = "btnFacturas"
        Me.btnFacturas.Size = New System.Drawing.Size(25, 20)
        Me.btnFacturas.TabIndex = 95
        Me.btnFacturas.Text = "•••"
        Me.btnFacturas.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Nombre  :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(738, 434)
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
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.GroupBox1.Controls.Add(Me.txtUnidadSalida)
        Me.GroupBox1.Controls.Add(Me.txtMercanciaSalida)
        Me.GroupBox1.Controls.Add(Me.txtCantidadSalida)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.btnMercanciaSalida)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtCodigoMercanciaSalida)
        Me.GroupBox1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(0, 227)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(899, 201)
        Me.GroupBox1.TabIndex = 89
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Mercancía Salida"
        '
        'txtUnidadSalida
        '
        Me.txtUnidadSalida.BackColor = System.Drawing.Color.White
        Me.txtUnidadSalida.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUnidadSalida.Location = New System.Drawing.Point(403, 97)
        Me.txtUnidadSalida.MaxLength = 25
        Me.txtUnidadSalida.Name = "txtUnidadSalida"
        Me.txtUnidadSalida.Size = New System.Drawing.Size(67, 32)
        Me.txtUnidadSalida.TabIndex = 109
        Me.txtUnidadSalida.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMercanciaSalida
        '
        Me.txtMercanciaSalida.BackColor = System.Drawing.Color.White
        Me.txtMercanciaSalida.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMercanciaSalida.Location = New System.Drawing.Point(403, 59)
        Me.txtMercanciaSalida.MaxLength = 25
        Me.txtMercanciaSalida.Name = "txtMercanciaSalida"
        Me.txtMercanciaSalida.Size = New System.Drawing.Size(490, 32)
        Me.txtMercanciaSalida.TabIndex = 108
        '
        'txtCantidadSalida
        '
        Me.txtCantidadSalida.BackColor = System.Drawing.Color.White
        Me.txtCantidadSalida.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCantidadSalida.Location = New System.Drawing.Point(403, 130)
        Me.txtCantidadSalida.MaxLength = 25
        Me.txtCantidadSalida.Name = "txtCantidadSalida"
        Me.txtCantidadSalida.Size = New System.Drawing.Size(208, 32)
        Me.txtCantidadSalida.TabIndex = 107
        '
        'Label7
        '
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label7.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(6, 130)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(391, 31)
        Me.Label7.TabIndex = 106
        Me.Label7.Text = "Cantidad :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'btnMercanciaSalida
        '
        Me.btnMercanciaSalida.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMercanciaSalida.Location = New System.Drawing.Point(617, 30)
        Me.btnMercanciaSalida.Name = "btnMercanciaSalida"
        Me.btnMercanciaSalida.Size = New System.Drawing.Size(25, 20)
        Me.btnMercanciaSalida.TabIndex = 105
        Me.btnMercanciaSalida.Text = "•••"
        Me.btnMercanciaSalida.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label8.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(6, 99)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(391, 31)
        Me.Label8.TabIndex = 102
        Me.Label8.Text = "Unidad :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label9
        '
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(6, 59)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(391, 31)
        Me.Label9.TabIndex = 101
        Me.Label9.Text = "Descripción :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label10
        '
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label10.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(6, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(391, 31)
        Me.Label10.TabIndex = 98
        Me.Label10.Text = "Código de barras mercancía :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtCodigoMercanciaSalida
        '
        Me.txtCodigoMercanciaSalida.BackColor = System.Drawing.Color.White
        Me.txtCodigoMercanciaSalida.Font = New System.Drawing.Font("Consolas", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigoMercanciaSalida.Location = New System.Drawing.Point(403, 18)
        Me.txtCodigoMercanciaSalida.MaxLength = 25
        Me.txtCodigoMercanciaSalida.Name = "txtCodigoMercanciaSalida"
        Me.txtCodigoMercanciaSalida.Size = New System.Drawing.Size(208, 32)
        Me.txtCodigoMercanciaSalida.TabIndex = 97
        '
        'jsPOSRecambioMercancia
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(903, 463)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsPOSRecambioMercancia"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Recambio de mercancías"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnFacturas As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNumeroFactura As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCodigoMercanciaADevolver As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnMercanciaDevolucion As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtUnidadADevolver As System.Windows.Forms.TextBox
    Friend WithEvents txtMercanciaADevolver As System.Windows.Forms.TextBox
    Friend WithEvents txtCantidadADevolver As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtUnidadSalida As System.Windows.Forms.TextBox
    Friend WithEvents txtMercanciaSalida As System.Windows.Forms.TextBox
    Friend WithEvents txtCantidadSalida As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnMercanciaSalida As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtCodigoMercanciaSalida As System.Windows.Forms.TextBox
End Class
