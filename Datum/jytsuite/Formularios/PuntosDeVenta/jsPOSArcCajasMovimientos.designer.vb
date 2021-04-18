<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsPOSArcCajasMovimientos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsPOSArcCajasMovimientos))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.chkRegistrar = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnImpreFiscal = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtImpreFiscal = New System.Windows.Forms.TextBox()
        Me.txtAlmacen = New System.Windows.Forms.TextBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAlmacen = New System.Windows.Forms.Button()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.dgPer = New System.Windows.Forms.DataGridView()
        Me.MenuComisiones = New System.Windows.Forms.ToolStrip()
        Me.btnAgregaPerfil = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaPerfil = New System.Windows.Forms.ToolStripButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grpTarjeta.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.dgPer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuComisiones.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 295)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(477, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.chkRegistrar)
        Me.grpTarjeta.Controls.Add(Me.Label4)
        Me.grpTarjeta.Controls.Add(Me.btnImpreFiscal)
        Me.grpTarjeta.Controls.Add(Me.Label7)
        Me.grpTarjeta.Controls.Add(Me.txtImpreFiscal)
        Me.grpTarjeta.Controls.Add(Me.txtAlmacen)
        Me.grpTarjeta.Controls.Add(Me.txtNombre)
        Me.grpTarjeta.Controls.Add(Me.txtCodigo)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(473, 140)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        Me.grpTarjeta.Text = " POS "
        '
        'chkRegistrar
        '
        Me.chkRegistrar.AutoSize = True
        Me.chkRegistrar.Location = New System.Drawing.Point(397, 104)
        Me.chkRegistrar.Name = "chkRegistrar"
        Me.chkRegistrar.Size = New System.Drawing.Size(15, 14)
        Me.chkRegistrar.TabIndex = 98
        Me.chkRegistrar.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(385, 15)
        Me.Label4.TabIndex = 97
        Me.Label4.Text = "Registrar esta caja para facturación al mayor en este equipo"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnImpreFiscal
        '
        Me.btnImpreFiscal.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImpreFiscal.Location = New System.Drawing.Point(211, 62)
        Me.btnImpreFiscal.Name = "btnImpreFiscal"
        Me.btnImpreFiscal.Size = New System.Drawing.Size(25, 20)
        Me.btnImpreFiscal.TabIndex = 96
        Me.btnImpreFiscal.Text = "•••"
        Me.btnImpreFiscal.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(13, 63)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(118, 19)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Impresora Fiscal :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtImpreFiscal
        '
        Me.txtImpreFiscal.Location = New System.Drawing.Point(137, 62)
        Me.txtImpreFiscal.Name = "txtImpreFiscal"
        Me.txtImpreFiscal.Size = New System.Drawing.Size(68, 20)
        Me.txtImpreFiscal.TabIndex = 17
        Me.txtImpreFiscal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlmacen
        '
        Me.txtAlmacen.Location = New System.Drawing.Point(397, 63)
        Me.txtAlmacen.Name = "txtAlmacen"
        Me.txtAlmacen.Size = New System.Drawing.Size(68, 20)
        Me.txtAlmacen.TabIndex = 15
        Me.txtAlmacen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtAlmacen.Visible = False
        '
        'txtNombre
        '
        Me.txtNombre.BackColor = System.Drawing.Color.White
        Me.txtNombre.Location = New System.Drawing.Point(137, 36)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(328, 20)
        Me.txtNombre.TabIndex = 14
        '
        'txtCodigo
        '
        Me.txtCodigo.BackColor = System.Drawing.Color.White
        Me.txtCodigo.Enabled = False
        Me.txtCodigo.Location = New System.Drawing.Point(137, 15)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(68, 20)
        Me.txtCodigo.TabIndex = 13
        Me.txtCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(273, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Almacén :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label3.Visible = False
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
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 19)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Código  :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnAlmacen
        '
        Me.btnAlmacen.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAlmacen.Location = New System.Drawing.Point(382, 15)
        Me.btnAlmacen.Name = "btnAlmacen"
        Me.btnAlmacen.Size = New System.Drawing.Size(25, 20)
        Me.btnAlmacen.TabIndex = 95
        Me.btnAlmacen.Text = "•••"
        Me.btnAlmacen.UseVisualStyleBackColor = True
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(312, 292)
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
        'dgPer
        '
        Me.dgPer.AllowUserToAddRows = False
        Me.dgPer.AllowUserToDeleteRows = False
        Me.dgPer.AllowUserToOrderColumns = True
        Me.dgPer.AllowUserToResizeColumns = False
        Me.dgPer.AllowUserToResizeRows = False
        Me.dgPer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgPer.Location = New System.Drawing.Point(0, 178)
        Me.dgPer.Name = "dgPer"
        Me.dgPer.ReadOnly = True
        Me.dgPer.Size = New System.Drawing.Size(473, 111)
        Me.dgPer.TabIndex = 107
        '
        'MenuComisiones
        '
        Me.MenuComisiones.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuComisiones.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuComisiones.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuComisiones.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaPerfil, Me.btnEliminaPerfil})
        Me.MenuComisiones.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuComisiones.Location = New System.Drawing.Point(9, 144)
        Me.MenuComisiones.Name = "MenuComisiones"
        Me.MenuComisiones.Size = New System.Drawing.Size(51, 27)
        Me.MenuComisiones.TabIndex = 109
        Me.MenuComisiones.Text = "ToolStrip1"
        '
        'btnAgregaPerfil
        '
        Me.btnAgregaPerfil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaPerfil.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregaPerfil.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaPerfil.Name = "btnAgregaPerfil"
        Me.btnAgregaPerfil.Size = New System.Drawing.Size(24, 24)
        '
        'btnEliminaPerfil
        '
        Me.btnEliminaPerfil.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaPerfil.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminaPerfil.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminaPerfil.Name = "btnEliminaPerfil"
        Me.btnEliminaPerfil.Size = New System.Drawing.Size(24, 24)
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(84, 144)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(389, 27)
        Me.Label5.TabIndex = 110
        Me.Label5.Text = "Perfiles de venta asociados"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'jsPOSArcCajasMovimientos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(477, 321)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.MenuComisiones)
        Me.Controls.Add(Me.dgPer)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.btnAlmacen)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsPOSArcCajasMovimientos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimientos Cajas ó puntos de venta"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.dgPer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuComisiones.ResumeLayout(False)
        Me.MenuComisiones.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAlmacen As System.Windows.Forms.TextBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtImpreFiscal As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnAlmacen As System.Windows.Forms.Button
    Friend WithEvents btnImpreFiscal As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkRegistrar As System.Windows.Forms.CheckBox
    Friend WithEvents dgPer As System.Windows.Forms.DataGridView
    Friend WithEvents MenuComisiones As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregaPerfil As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminaPerfil As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
