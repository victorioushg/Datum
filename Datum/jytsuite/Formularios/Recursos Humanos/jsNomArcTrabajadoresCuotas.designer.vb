<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsNomArcTrabajadoresCuotas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsNomArcTrabajadoresCuotas))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grp = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbEstatus = New System.Windows.Forms.ComboBox()
        Me.txtSaldo = New System.Windows.Forms.TextBox()
        Me.txtCantidadCuotas = New System.Windows.Forms.TextBox()
        Me.txtPorcentajeInteres = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnInicio = New System.Windows.Forms.Button()
        Me.txtInicio = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnAprobacion = New System.Windows.Forms.Button()
        Me.txtAprobacion = New System.Windows.Forms.TextBox()
        Me.txtMonto = New System.Windows.Forms.TextBox()
        Me.btnPrestamo = New System.Windows.Forms.Button()
        Me.MenuRenglon = New System.Windows.Forms.ToolStrip()
        Me.btnAgregaRenglon = New System.Windows.Forms.ToolStripButton()
        Me.btnEditarRenglon = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaRenglon = New System.Windows.Forms.ToolStripButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.cmbTipoInteres = New System.Windows.Forms.ComboBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.cmbNomina = New System.Windows.Forms.ComboBox()
        Me.txtNomina = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.grp.SuspendLayout()
        Me.MenuRenglon.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 326)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(751, 29)
        Me.lblInfo.TabIndex = 79
        '
        'grp
        '
        Me.grp.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grp.Controls.Add(Me.cmbNomina)
        Me.grp.Controls.Add(Me.txtNomina)
        Me.grp.Controls.Add(Me.Label11)
        Me.grp.Controls.Add(Me.Label12)
        Me.grp.Controls.Add(Me.Label10)
        Me.grp.Controls.Add(Me.cmbEstatus)
        Me.grp.Controls.Add(Me.txtSaldo)
        Me.grp.Controls.Add(Me.txtCantidadCuotas)
        Me.grp.Controls.Add(Me.txtPorcentajeInteres)
        Me.grp.Controls.Add(Me.Label9)
        Me.grp.Controls.Add(Me.Label8)
        Me.grp.Controls.Add(Me.Label7)
        Me.grp.Controls.Add(Me.Label6)
        Me.grp.Controls.Add(Me.btnInicio)
        Me.grp.Controls.Add(Me.txtInicio)
        Me.grp.Controls.Add(Me.Label4)
        Me.grp.Controls.Add(Me.Label3)
        Me.grp.Controls.Add(Me.btnAprobacion)
        Me.grp.Controls.Add(Me.txtAprobacion)
        Me.grp.Controls.Add(Me.txtMonto)
        Me.grp.Controls.Add(Me.btnPrestamo)
        Me.grp.Controls.Add(Me.MenuRenglon)
        Me.grp.Controls.Add(Me.Label5)
        Me.grp.Controls.Add(Me.dg)
        Me.grp.Controls.Add(Me.cmbTipoInteres)
        Me.grp.Controls.Add(Me.txtNombre)
        Me.grp.Controls.Add(Me.txtCodigo)
        Me.grp.Controls.Add(Me.Label2)
        Me.grp.Controls.Add(Me.Label1)
        Me.grp.Location = New System.Drawing.Point(0, 1)
        Me.grp.Name = "grp"
        Me.grp.Size = New System.Drawing.Size(747, 322)
        Me.grp.TabIndex = 80
        Me.grp.TabStop = False
        Me.grp.Text = " Cuotas/Préstamo"
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(434, 44)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(118, 19)
        Me.Label10.TabIndex = 141
        Me.Label10.Text = "Estatus :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbEstatus
        '
        Me.cmbEstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEstatus.FormattingEnabled = True
        Me.cmbEstatus.Location = New System.Drawing.Point(560, 44)
        Me.cmbEstatus.Name = "cmbEstatus"
        Me.cmbEstatus.Size = New System.Drawing.Size(181, 21)
        Me.cmbEstatus.TabIndex = 140
        '
        'txtSaldo
        '
        Me.txtSaldo.BackColor = System.Drawing.Color.White
        Me.txtSaldo.Enabled = False
        Me.txtSaldo.Location = New System.Drawing.Point(560, 153)
        Me.txtSaldo.Name = "txtSaldo"
        Me.txtSaldo.Size = New System.Drawing.Size(181, 20)
        Me.txtSaldo.TabIndex = 139
        Me.txtSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCantidadCuotas
        '
        Me.txtCantidadCuotas.BackColor = System.Drawing.Color.White
        Me.txtCantidadCuotas.Location = New System.Drawing.Point(560, 131)
        Me.txtCantidadCuotas.Name = "txtCantidadCuotas"
        Me.txtCantidadCuotas.Size = New System.Drawing.Size(99, 20)
        Me.txtCantidadCuotas.TabIndex = 138
        Me.txtCantidadCuotas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPorcentajeInteres
        '
        Me.txtPorcentajeInteres.BackColor = System.Drawing.Color.White
        Me.txtPorcentajeInteres.Location = New System.Drawing.Point(560, 110)
        Me.txtPorcentajeInteres.Name = "txtPorcentajeInteres"
        Me.txtPorcentajeInteres.Size = New System.Drawing.Size(99, 20)
        Me.txtPorcentajeInteres.TabIndex = 137
        Me.txtPorcentajeInteres.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(434, 153)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(118, 19)
        Me.Label9.TabIndex = 136
        Me.Label9.Text = "Saldo :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(434, 130)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(118, 19)
        Me.Label8.TabIndex = 135
        Me.Label8.Text = "Número cuotas :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(436, 109)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(118, 19)
        Me.Label7.TabIndex = 134
        Me.Label7.Text = "Porcentaje interés :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(436, 90)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(118, 19)
        Me.Label6.TabIndex = 133
        Me.Label6.Text = "Tipo de interés :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnInicio
        '
        Me.btnInicio.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInicio.Location = New System.Drawing.Point(281, 130)
        Me.btnInicio.Name = "btnInicio"
        Me.btnInicio.Size = New System.Drawing.Size(25, 20)
        Me.btnInicio.TabIndex = 132
        Me.btnInicio.Text = "•••"
        Me.btnInicio.UseVisualStyleBackColor = True
        '
        'txtInicio
        '
        Me.txtInicio.BackColor = System.Drawing.Color.White
        Me.txtInicio.Location = New System.Drawing.Point(176, 130)
        Me.txtInicio.Name = "txtInicio"
        Me.txtInicio.Size = New System.Drawing.Size(99, 20)
        Me.txtInicio.TabIndex = 131
        Me.txtInicio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(15, 130)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(155, 19)
        Me.Label4.TabIndex = 130
        Me.Label4.Text = "Fecha inicio :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(52, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 19)
        Me.Label3.TabIndex = 129
        Me.Label3.Text = "Fecha aprobación :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnAprobacion
        '
        Me.btnAprobacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAprobacion.Location = New System.Drawing.Point(281, 108)
        Me.btnAprobacion.Name = "btnAprobacion"
        Me.btnAprobacion.Size = New System.Drawing.Size(25, 20)
        Me.btnAprobacion.TabIndex = 128
        Me.btnAprobacion.Text = "•••"
        Me.btnAprobacion.UseVisualStyleBackColor = True
        '
        'txtAprobacion
        '
        Me.txtAprobacion.BackColor = System.Drawing.Color.White
        Me.txtAprobacion.Location = New System.Drawing.Point(176, 109)
        Me.txtAprobacion.Name = "txtAprobacion"
        Me.txtAprobacion.Size = New System.Drawing.Size(99, 20)
        Me.txtAprobacion.TabIndex = 127
        Me.txtAprobacion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtMonto
        '
        Me.txtMonto.BackColor = System.Drawing.Color.White
        Me.txtMonto.Location = New System.Drawing.Point(176, 88)
        Me.txtMonto.Name = "txtMonto"
        Me.txtMonto.Size = New System.Drawing.Size(99, 20)
        Me.txtMonto.TabIndex = 126
        Me.txtMonto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnPrestamo
        '
        Me.btnPrestamo.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrestamo.Location = New System.Drawing.Point(250, 46)
        Me.btnPrestamo.Name = "btnPrestamo"
        Me.btnPrestamo.Size = New System.Drawing.Size(25, 20)
        Me.btnPrestamo.TabIndex = 125
        Me.btnPrestamo.Text = "•••"
        Me.btnPrestamo.UseVisualStyleBackColor = True
        '
        'MenuRenglon
        '
        Me.MenuRenglon.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuRenglon.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuRenglon.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuRenglon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaRenglon, Me.btnEditarRenglon, Me.btnEliminaRenglon})
        Me.MenuRenglon.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuRenglon.Location = New System.Drawing.Point(3, 182)
        Me.MenuRenglon.Name = "MenuRenglon"
        Me.MenuRenglon.Size = New System.Drawing.Size(25, 83)
        Me.MenuRenglon.TabIndex = 124
        Me.MenuRenglon.Text = "ToolStrip1"
        '
        'btnAgregaRenglon
        '
        Me.btnAgregaRenglon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaRenglon.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregaRenglon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaRenglon.Name = "btnAgregaRenglon"
        Me.btnAgregaRenglon.Size = New System.Drawing.Size(23, 24)
        '
        'btnEditarRenglon
        '
        Me.btnEditarRenglon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarRenglon.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarRenglon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarRenglon.Name = "btnEditarRenglon"
        Me.btnEditarRenglon.Size = New System.Drawing.Size(23, 24)
        '
        'btnEliminaRenglon
        '
        Me.btnEliminaRenglon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaRenglon.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminaRenglon.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminaRenglon.Name = "btnEliminaRenglon"
        Me.btnEliminaRenglon.Size = New System.Drawing.Size(23, 24)
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(52, 88)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(118, 19)
        Me.Label5.TabIndex = 123
        Me.Label5.Text = "Monto :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dg
        '
        Me.dg.AllowUserToAddRows = False
        Me.dg.AllowUserToDeleteRows = False
        Me.dg.AllowUserToOrderColumns = True
        Me.dg.AllowUserToResizeColumns = False
        Me.dg.AllowUserToResizeRows = False
        Me.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg.Location = New System.Drawing.Point(31, 179)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(710, 140)
        Me.dg.TabIndex = 122
        '
        'cmbTipoInteres
        '
        Me.cmbTipoInteres.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoInteres.FormattingEnabled = True
        Me.cmbTipoInteres.Location = New System.Drawing.Point(560, 88)
        Me.cmbTipoInteres.Name = "cmbTipoInteres"
        Me.cmbTipoInteres.Size = New System.Drawing.Size(181, 21)
        Me.cmbTipoInteres.TabIndex = 119
        '
        'txtNombre
        '
        Me.txtNombre.BackColor = System.Drawing.Color.White
        Me.txtNombre.Location = New System.Drawing.Point(176, 67)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(565, 20)
        Me.txtNombre.TabIndex = 14
        '
        'txtCodigo
        '
        Me.txtCodigo.BackColor = System.Drawing.Color.White
        Me.txtCodigo.Location = New System.Drawing.Point(176, 46)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(68, 20)
        Me.txtCodigo.TabIndex = 13
        Me.txtCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(52, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Descripción  :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(52, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 19)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Código  :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(586, 326)
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
        'cmbNomina
        '
        Me.cmbNomina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNomina.FormattingEnabled = True
        Me.cmbNomina.Location = New System.Drawing.Point(101, 15)
        Me.cmbNomina.Name = "cmbNomina"
        Me.cmbNomina.Size = New System.Drawing.Size(334, 21)
        Me.cmbNomina.TabIndex = 145
        '
        'txtNomina
        '
        Me.txtNomina.Enabled = False
        Me.txtNomina.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNomina.Location = New System.Drawing.Point(560, 16)
        Me.txtNomina.MaxLength = 2
        Me.txtNomina.Name = "txtNomina"
        Me.txtNomina.Size = New System.Drawing.Size(181, 20)
        Me.txtNomina.TabIndex = 144
        Me.txtNomina.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(31, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(64, 19)
        Me.Label11.TabIndex = 143
        Me.Label11.Text = "Nómina :"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(457, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(95, 19)
        Me.Label12.TabIndex = 142
        Me.Label12.Text = "Tipo nómina:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'jsNomArcTrabajadoresCuotas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(751, 355)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grp)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsNomArcTrabajadoresCuotas"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento Cuota/Prestamo Trabajador"
        Me.grp.ResumeLayout(False)
        Me.grp.PerformLayout()
        Me.MenuRenglon.ResumeLayout(False)
        Me.MenuRenglon.PerformLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grp As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cmbTipoInteres As System.Windows.Forms.ComboBox
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents MenuRenglon As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregaRenglon As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminaRenglon As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarRenglon As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnPrestamo As System.Windows.Forms.Button
    Friend WithEvents txtAprobacion As System.Windows.Forms.TextBox
    Friend WithEvents txtMonto As System.Windows.Forms.TextBox
    Friend WithEvents btnAprobacion As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnInicio As System.Windows.Forms.Button
    Friend WithEvents txtInicio As System.Windows.Forms.TextBox
    Friend WithEvents txtPorcentajeInteres As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtCantidadCuotas As System.Windows.Forms.TextBox
    Friend WithEvents txtSaldo As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbEstatus As System.Windows.Forms.ComboBox
    Friend WithEvents cmbNomina As System.Windows.Forms.ComboBox
    Friend WithEvents txtNomina As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class
