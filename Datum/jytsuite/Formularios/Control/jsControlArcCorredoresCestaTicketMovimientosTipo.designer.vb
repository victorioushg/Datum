<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsControlArcCorredoresCestaTicketMovimientosTipo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsControlArcCorredoresCestaTicketMovimientosTipo))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.MenuComisiones = New System.Windows.Forms.ToolStrip()
        Me.dgCom = New System.Windows.Forms.DataGridView()
        Me.txtImpuesto = New System.Windows.Forms.TextBox()
        Me.txtComision = New System.Windows.Forms.TextBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAgregaTipo = New System.Windows.Forms.ToolStripButton()
        Me.btnEditaTipo = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaTipo = New System.Windows.Forms.ToolStripButton()
        Me.grpTarjeta.SuspendLayout()
        Me.MenuComisiones.SuspendLayout()
        CType(Me.dgCom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 241)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(411, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.MenuComisiones)
        Me.grpTarjeta.Controls.Add(Me.dgCom)
        Me.grpTarjeta.Controls.Add(Me.txtImpuesto)
        Me.grpTarjeta.Controls.Add(Me.txtComision)
        Me.grpTarjeta.Controls.Add(Me.txtNombre)
        Me.grpTarjeta.Controls.Add(Me.txtCodigo)
        Me.grpTarjeta.Controls.Add(Me.Label5)
        Me.grpTarjeta.Controls.Add(Me.Label4)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(411, 237)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        Me.grpTarjeta.Text = "Tipo Cesta Ticket"
        '
        'MenuComisiones
        '
        Me.MenuComisiones.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuComisiones.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuComisiones.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuComisiones.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaTipo, Me.btnEditaTipo, Me.btnEliminaTipo})
        Me.MenuComisiones.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuComisiones.Location = New System.Drawing.Point(331, 88)
        Me.MenuComisiones.Name = "MenuComisiones"
        Me.MenuComisiones.Size = New System.Drawing.Size(75, 27)
        Me.MenuComisiones.TabIndex = 109
        Me.MenuComisiones.Text = "ToolStrip1"
        '
        'dgCom
        '
        Me.dgCom.AllowUserToAddRows = False
        Me.dgCom.AllowUserToDeleteRows = False
        Me.dgCom.AllowUserToOrderColumns = True
        Me.dgCom.AllowUserToResizeColumns = False
        Me.dgCom.AllowUserToResizeRows = False
        Me.dgCom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgCom.Location = New System.Drawing.Point(3, 118)
        Me.dgCom.Name = "dgCom"
        Me.dgCom.ReadOnly = True
        Me.dgCom.Size = New System.Drawing.Size(402, 113)
        Me.dgCom.TabIndex = 107
        '
        'txtImpuesto
        '
        Me.txtImpuesto.Location = New System.Drawing.Point(153, 78)
        Me.txtImpuesto.Name = "txtImpuesto"
        Me.txtImpuesto.Size = New System.Drawing.Size(68, 20)
        Me.txtImpuesto.TabIndex = 16
        Me.txtImpuesto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtComision
        '
        Me.txtComision.Location = New System.Drawing.Point(153, 57)
        Me.txtComision.Name = "txtComision"
        Me.txtComision.Size = New System.Drawing.Size(68, 20)
        Me.txtComision.TabIndex = 15
        Me.txtComision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNombre
        '
        Me.txtNombre.BackColor = System.Drawing.Color.White
        Me.txtNombre.Location = New System.Drawing.Point(153, 36)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(252, 20)
        Me.txtNombre.TabIndex = 14
        '
        'txtCodigo
        '
        Me.txtCodigo.BackColor = System.Drawing.Color.White
        Me.txtCodigo.Enabled = False
        Me.txtCodigo.Location = New System.Drawing.Point(153, 15)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(68, 20)
        Me.txtCodigo.TabIndex = 13
        Me.txtCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(265, 19)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Comisiones tipo por días vencidos :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 78)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(144, 19)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "% Comisión Cliente  :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(-6, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(153, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "% Comisión proveedor  :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(147, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Nombre/Descripción  :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(134, 19)
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
        Me.grpAceptarSalir.Location = New System.Drawing.Point(246, 238)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 88
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "button_ok.ico")
        Me.ImageList1.Images.SetKeyName(1, "button_cancel.ico")
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
        'btnAgregaTipo
        '
        Me.btnAgregaTipo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaTipo.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregaTipo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaTipo.Name = "btnAgregaTipo"
        Me.btnAgregaTipo.Size = New System.Drawing.Size(24, 24)
        '
        'btnEditaTipo
        '
        Me.btnEditaTipo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditaTipo.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditaTipo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditaTipo.Name = "btnEditaTipo"
        Me.btnEditaTipo.Size = New System.Drawing.Size(24, 24)
        '
        'btnEliminaTipo
        '
        Me.btnEliminaTipo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaTipo.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminaTipo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminaTipo.Name = "btnEliminaTipo"
        Me.btnEliminaTipo.Size = New System.Drawing.Size(24, 24)
        '
        'jsControlArcCorredoresCestaTicketMovimientosTipo
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(411, 267)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsControlArcCorredoresCestaTicketMovimientosTipo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento tipo cesta ticket"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.MenuComisiones.ResumeLayout(False)
        Me.MenuComisiones.PerformLayout()
        CType(Me.dgCom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtImpuesto As System.Windows.Forms.TextBox
    Friend WithEvents txtComision As System.Windows.Forms.TextBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents dgCom As System.Windows.Forms.DataGridView
    Friend WithEvents MenuComisiones As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregaTipo As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditaTipo As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminaTipo As System.Windows.Forms.ToolStripButton
End Class
