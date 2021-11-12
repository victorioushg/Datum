<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsBanArcRemesasCestaTicket
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsBanArcRemesasCestaTicket))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lv = New System.Windows.Forms.ListView()
        Me.grpCaja = New System.Windows.Forms.GroupBox()
        Me.MenuComisiones = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.btnDocs = New System.Windows.Forms.ToolStripButton()
        Me.lblCaja = New System.Windows.Forms.Label()
        Me.lblTituloCaja = New System.Windows.Forms.Label()
        Me.txtRemesa = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.txtTickets = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtSaldoSel = New System.Windows.Forms.TextBox()
        Me.txtDocSel = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.txtEmision = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.grpCaja.SuspendLayout()
        Me.MenuComisiones.SuspendLayout()
        Me.grpTotales.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 411)
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
        Me.lv.Location = New System.Drawing.Point(-1, 91)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(730, 264)
        Me.lv.TabIndex = 81
        Me.lv.UseCompatibleStateImageBehavior = False
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'grpCaja
        '
        Me.grpCaja.Controls.Add(Me.txtEmision)
        Me.grpCaja.Controls.Add(Me.MenuComisiones)
        Me.grpCaja.Controls.Add(Me.lblCaja)
        Me.grpCaja.Controls.Add(Me.lblTituloCaja)
        Me.grpCaja.Controls.Add(Me.txtRemesa)
        Me.grpCaja.Controls.Add(Me.Label3)
        Me.grpCaja.Controls.Add(Me.Label5)
        Me.grpCaja.Location = New System.Drawing.Point(-1, -2)
        Me.grpCaja.Name = "grpCaja"
        Me.grpCaja.Size = New System.Drawing.Size(730, 87)
        Me.grpCaja.TabIndex = 82
        Me.grpCaja.TabStop = False
        '
        'MenuComisiones
        '
        Me.MenuComisiones.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuComisiones.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuComisiones.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuComisiones.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.btnDocs})
        Me.MenuComisiones.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuComisiones.Location = New System.Drawing.Point(672, 57)
        Me.MenuComisiones.Name = "MenuComisiones"
        Me.MenuComisiones.Size = New System.Drawing.Size(51, 27)
        Me.MenuComisiones.TabIndex = 109
        Me.MenuComisiones.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.Datum.My.Resources.Resources.Buscar
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(24, 24)
        '
        'btnDocs
        '
        Me.btnDocs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnDocs.Image = Global.Datum.My.Resources.Resources.Columnas
        Me.btnDocs.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDocs.Name = "btnDocs"
        Me.btnDocs.Size = New System.Drawing.Size(24, 24)
        '
        'lblCaja
        '
        Me.lblCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaja.Location = New System.Drawing.Point(365, 11)
        Me.lblCaja.Name = "lblCaja"
        Me.lblCaja.Size = New System.Drawing.Size(356, 22)
        Me.lblCaja.TabIndex = 1
        Me.lblCaja.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTituloCaja
        '
        Me.lblTituloCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTituloCaja.Location = New System.Drawing.Point(11, 11)
        Me.lblTituloCaja.Name = "lblTituloCaja"
        Me.lblTituloCaja.Size = New System.Drawing.Size(348, 22)
        Me.lblTituloCaja.TabIndex = 0
        Me.lblTituloCaja.Text = "Construcción de remesas de cheques de alimentación del corredor : "
        Me.lblTituloCaja.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtRemesa
        '
        Me.txtRemesa.Location = New System.Drawing.Point(180, 36)
        Me.txtRemesa.Name = "txtRemesa"
        Me.txtRemesa.Size = New System.Drawing.Size(148, 20)
        Me.txtRemesa.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(101, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 17)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Nº remesa :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(353, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 17)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Emisión :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpTotales
        '
        Me.grpTotales.Controls.Add(Me.txtTickets)
        Me.grpTotales.Controls.Add(Me.Label11)
        Me.grpTotales.Controls.Add(Me.txtSaldoSel)
        Me.grpTotales.Controls.Add(Me.txtDocSel)
        Me.grpTotales.Controls.Add(Me.Label4)
        Me.grpTotales.Controls.Add(Me.Label2)
        Me.grpTotales.Enabled = False
        Me.grpTotales.Location = New System.Drawing.Point(1, 361)
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
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(567, 411)
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
        Me.txtEmision.Location = New System.Drawing.Point(431, 37)
        Me.txtEmision.Name = "txtEmision"
        Me.txtEmision.Size = New System.Drawing.Size(114, 19)
        Me.txtEmision.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtEmision.TabIndex = 214
        '
        'jsBanArcRemesasCestaTicket
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(732, 438)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTotales)
        Me.Controls.Add(Me.grpCaja)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsBanArcRemesasCestaTicket"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Construcción de remesa de cheques de alimentación"
        Me.grpCaja.ResumeLayout(False)
        Me.grpCaja.PerformLayout()
        Me.MenuComisiones.ResumeLayout(False)
        Me.MenuComisiones.PerformLayout()
        Me.grpTotales.ResumeLayout(False)
        Me.grpTotales.PerformLayout()
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
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtRemesa As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSaldoSel As System.Windows.Forms.TextBox
    Friend WithEvents txtDocSel As System.Windows.Forms.TextBox
    Friend WithEvents txtTickets As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblCaja As System.Windows.Forms.Label
    Friend WithEvents MenuComisiones As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnDocs As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtEmision As Syncfusion.WinForms.Input.SfDateTimeEdit
End Class
