<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsControlArcAlmacenesEstantes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsControlArcAlmacenesEstantes))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.dgUbica = New System.Windows.Forms.DataGridView()
        Me.MenuEquivalencia = New System.Windows.Forms.ToolStrip()
        Me.btnAgregaUbica = New System.Windows.Forms.ToolStripButton()
        Me.btnEditaUbica = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaUbica = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnImprimirUbica = New System.Windows.Forms.ToolStripButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.txtUbicacionAlmacen = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbTipoEstante = New System.Windows.Forms.ComboBox()
        Me.btnDescripcion = New System.Windows.Forms.Button()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.lblUbica = New System.Windows.Forms.Label()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.dgUbica, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuEquivalencia.SuspendLayout()
        Me.grpTarjeta.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 329)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(403, 29)
        Me.lblInfo.TabIndex = 79
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(238, 329)
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
        'dgUbica
        '
        Me.dgUbica.AllowUserToAddRows = False
        Me.dgUbica.AllowUserToDeleteRows = False
        Me.dgUbica.AllowUserToOrderColumns = True
        Me.dgUbica.AllowUserToResizeColumns = False
        Me.dgUbica.AllowUserToResizeRows = False
        Me.dgUbica.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgUbica.Location = New System.Drawing.Point(142, 120)
        Me.dgUbica.Name = "dgUbica"
        Me.dgUbica.ReadOnly = True
        Me.dgUbica.Size = New System.Drawing.Size(251, 197)
        Me.dgUbica.TabIndex = 114
        '
        'MenuEquivalencia
        '
        Me.MenuEquivalencia.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuEquivalencia.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuEquivalencia.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuEquivalencia.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaUbica, Me.btnEditaUbica, Me.btnEliminaUbica, Me.ToolStripSeparator1, Me.btnImprimirUbica})
        Me.MenuEquivalencia.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuEquivalencia.Location = New System.Drawing.Point(99, 125)
        Me.MenuEquivalencia.Name = "MenuEquivalencia"
        Me.MenuEquivalencia.Size = New System.Drawing.Size(25, 116)
        Me.MenuEquivalencia.TabIndex = 115
        Me.MenuEquivalencia.Text = "ToolStrip1"
        '
        'btnAgregaUbica
        '
        Me.btnAgregaUbica.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaUbica.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregaUbica.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaUbica.Name = "btnAgregaUbica"
        Me.btnAgregaUbica.Size = New System.Drawing.Size(23, 24)
        '
        'btnEditaUbica
        '
        Me.btnEditaUbica.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditaUbica.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditaUbica.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditaUbica.Name = "btnEditaUbica"
        Me.btnEditaUbica.Size = New System.Drawing.Size(23, 24)
        '
        'btnEliminaUbica
        '
        Me.btnEliminaUbica.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaUbica.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminaUbica.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminaUbica.Name = "btnEliminaUbica"
        Me.btnEliminaUbica.Size = New System.Drawing.Size(23, 24)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(23, 6)
        '
        'btnImprimirUbica
        '
        Me.btnImprimirUbica.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnImprimirUbica.Image = Global.Datum.My.Resources.Resources.Imprimir
        Me.btnImprimirUbica.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImprimirUbica.Name = "btnImprimirUbica"
        Me.btnImprimirUbica.Size = New System.Drawing.Size(23, 24)
        Me.btnImprimirUbica.Text = "ToolStripButton1"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(13, 106)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(118, 19)
        Me.Label5.TabIndex = 116
        Me.Label5.Text = "Ubicaciones :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 19)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Código  :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Descripción:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        'txtNombre
        '
        Me.txtNombre.BackColor = System.Drawing.Color.White
        Me.txtNombre.Location = New System.Drawing.Point(137, 36)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(251, 20)
        Me.txtNombre.TabIndex = 14
        '
        'txtUbicacionAlmacen
        '
        Me.txtUbicacionAlmacen.BackColor = System.Drawing.Color.White
        Me.txtUbicacionAlmacen.Location = New System.Drawing.Point(137, 57)
        Me.txtUbicacionAlmacen.Name = "txtUbicacionAlmacen"
        Me.txtUbicacionAlmacen.Size = New System.Drawing.Size(68, 20)
        Me.txtUbicacionAlmacen.TabIndex = 15
        Me.txtUbicacionAlmacen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(129, 19)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Ubicación almacén  :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(13, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 19)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Tipo Estante  :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbTipoEstante
        '
        Me.cmbTipoEstante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoEstante.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTipoEstante.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.cmbTipoEstante.FormattingEnabled = True
        Me.cmbTipoEstante.Location = New System.Drawing.Point(137, 79)
        Me.cmbTipoEstante.Name = "cmbTipoEstante"
        Me.cmbTipoEstante.Size = New System.Drawing.Size(251, 23)
        Me.cmbTipoEstante.TabIndex = 112
        '
        'btnDescripcion
        '
        Me.btnDescripcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDescripcion.Location = New System.Drawing.Point(211, 57)
        Me.btnDescripcion.Name = "btnDescripcion"
        Me.btnDescripcion.Size = New System.Drawing.Size(27, 20)
        Me.btnDescripcion.TabIndex = 113
        Me.btnDescripcion.Text = "···"
        Me.btnDescripcion.UseVisualStyleBackColor = True
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.MenuEquivalencia)
        Me.grpTarjeta.Controls.Add(Me.Label5)
        Me.grpTarjeta.Controls.Add(Me.lblUbica)
        Me.grpTarjeta.Controls.Add(Me.btnDescripcion)
        Me.grpTarjeta.Controls.Add(Me.cmbTipoEstante)
        Me.grpTarjeta.Controls.Add(Me.Label4)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.txtUbicacionAlmacen)
        Me.grpTarjeta.Controls.Add(Me.txtNombre)
        Me.grpTarjeta.Controls.Add(Me.txtCodigo)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Location = New System.Drawing.Point(5, 12)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(394, 311)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        Me.grpTarjeta.Text = "Estante"
        '
        'lblUbica
        '
        Me.lblUbica.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUbica.Location = New System.Drawing.Point(244, 58)
        Me.lblUbica.Name = "lblUbica"
        Me.lblUbica.Size = New System.Drawing.Size(144, 19)
        Me.lblUbica.TabIndex = 114
        Me.lblUbica.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'jsControlArcAlmacenesEstantes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(403, 358)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgUbica)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsControlArcAlmacenesEstantes"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento Estante"
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.dgUbica, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuEquivalencia.ResumeLayout(False)
        Me.MenuEquivalencia.PerformLayout()
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents dgUbica As System.Windows.Forms.DataGridView
    Friend WithEvents MenuEquivalencia As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregaUbica As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditaUbica As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminaUbica As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents txtUbicacionAlmacen As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbTipoEstante As System.Windows.Forms.ComboBox
    Friend WithEvents btnDescripcion As System.Windows.Forms.Button
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents lblUbica As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnImprimirUbica As System.Windows.Forms.ToolStripButton
End Class
