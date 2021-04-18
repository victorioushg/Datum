<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsGenDescuentosComprasRenglon
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsGenDescuentosComprasRenglon))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMontoyDescuentos = New System.Windows.Forms.TextBox()
        Me.txtMontoInicial = New System.Windows.Forms.TextBox()
        Me.MenuDescuentos = New System.Windows.Forms.ToolStrip()
        Me.btnAgregaDescuento = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaDescuento = New System.Windows.Forms.ToolStripButton()
        Me.dgDescuentos = New System.Windows.Forms.DataGridView()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPorDescuentoTotal = New System.Windows.Forms.TextBox()
        Me.grpTarjeta.SuspendLayout()
        Me.MenuDescuentos.SuspendLayout()
        CType(Me.dgDescuentos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 246)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(315, 29)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.txtPorDescuentoTotal)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Controls.Add(Me.txtMontoyDescuentos)
        Me.grpTarjeta.Controls.Add(Me.txtMontoInicial)
        Me.grpTarjeta.Controls.Add(Me.MenuDescuentos)
        Me.grpTarjeta.Controls.Add(Me.dgDescuentos)
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(314, 242)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        Me.grpTarjeta.Text = "Descuentos"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 216)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(178, 19)
        Me.Label2.TabIndex = 223
        Me.Label2.Text = "Monto más descuentos  :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(63, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(121, 19)
        Me.Label1.TabIndex = 222
        Me.Label1.Text = "Monto Inicial :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMontoyDescuentos
        '
        Me.txtMontoyDescuentos.Location = New System.Drawing.Point(189, 216)
        Me.txtMontoyDescuentos.Name = "txtMontoyDescuentos"
        Me.txtMontoyDescuentos.Size = New System.Drawing.Size(119, 20)
        Me.txtMontoyDescuentos.TabIndex = 221
        Me.txtMontoyDescuentos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMontoInicial
        '
        Me.txtMontoInicial.Location = New System.Drawing.Point(190, 19)
        Me.txtMontoInicial.Name = "txtMontoInicial"
        Me.txtMontoInicial.Size = New System.Drawing.Size(118, 20)
        Me.txtMontoInicial.TabIndex = 220
        Me.txtMontoInicial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'MenuDescuentos
        '
        Me.MenuDescuentos.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuDescuentos.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuDescuentos.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuDescuentos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaDescuento, Me.btnEliminaDescuento})
        Me.MenuDescuentos.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuDescuentos.Location = New System.Drawing.Point(9, 31)
        Me.MenuDescuentos.Name = "MenuDescuentos"
        Me.MenuDescuentos.Size = New System.Drawing.Size(51, 27)
        Me.MenuDescuentos.TabIndex = 219
        Me.MenuDescuentos.Text = "ToolStrip1"
        '
        'btnAgregaDescuento
        '
        Me.btnAgregaDescuento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaDescuento.Image = CType(resources.GetObject("btnAgregaDescuento.Image"), System.Drawing.Image)
        Me.btnAgregaDescuento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaDescuento.Name = "btnAgregaDescuento"
        Me.btnAgregaDescuento.Size = New System.Drawing.Size(24, 24)
        '
        'btnEliminaDescuento
        '
        Me.btnEliminaDescuento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaDescuento.Image = CType(resources.GetObject("btnEliminaDescuento.Image"), System.Drawing.Image)
        Me.btnEliminaDescuento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminaDescuento.Name = "btnEliminaDescuento"
        Me.btnEliminaDescuento.Size = New System.Drawing.Size(24, 24)
        '
        'dgDescuentos
        '
        Me.dgDescuentos.AllowUserToAddRows = False
        Me.dgDescuentos.AllowUserToDeleteRows = False
        Me.dgDescuentos.AllowUserToOrderColumns = True
        Me.dgDescuentos.AllowUserToResizeColumns = False
        Me.dgDescuentos.AllowUserToResizeRows = False
        Me.dgDescuentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgDescuentos.Location = New System.Drawing.Point(6, 61)
        Me.dgDescuentos.Name = "dgDescuentos"
        Me.dgDescuentos.ReadOnly = True
        Me.dgDescuentos.Size = New System.Drawing.Size(303, 127)
        Me.dgDescuentos.TabIndex = 218
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(150, 246)
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
        Me.btnCancel.Image = My.Resources.Resources.button_cancel
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
        Me.btnOK.Image = My.Resources.Resources.button_ok
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(5, 191)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 19)
        Me.Label3.TabIndex = 225
        Me.Label3.Text = "Descuento total  :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtPorDescuentoTotal
        '
        Me.txtPorDescuentoTotal.Location = New System.Drawing.Point(124, 190)
        Me.txtPorDescuentoTotal.Name = "txtPorDescuentoTotal"
        Me.txtPorDescuentoTotal.Size = New System.Drawing.Size(67, 20)
        Me.txtPorDescuentoTotal.TabIndex = 224
        Me.txtPorDescuentoTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'jsGenDescuentosComprasRenglon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(315, 275)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsGenDescuentosComprasRenglon"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento descuentos"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.MenuDescuentos.ResumeLayout(False)
        Me.MenuDescuentos.PerformLayout()
        CType(Me.dgDescuentos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents MenuDescuentos As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregaDescuento As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminaDescuento As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgDescuentos As System.Windows.Forms.DataGridView
    Friend WithEvents txtMontoyDescuentos As System.Windows.Forms.TextBox
    Friend WithEvents txtMontoInicial As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPorDescuentoTotal As System.Windows.Forms.TextBox
End Class
