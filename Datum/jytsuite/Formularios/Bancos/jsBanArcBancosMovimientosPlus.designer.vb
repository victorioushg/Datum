<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsBanArcBancosMovimientosPlus
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsBanArcBancosMovimientosPlus))
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.txtDocumento = New System.Windows.Forms.TextBox()
        Me.txtConcepto = New System.Windows.Forms.TextBox()
        Me.txtImporte = New System.Windows.Forms.TextBox()
        Me.txtBeneficiario = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.cmbTipoDeposito = New System.Windows.Forms.ComboBox()
        Me.dgAsiento = New System.Windows.Forms.DataGridView()
        Me.MenuComisiones = New System.Windows.Forms.ToolStrip()
        Me.btnAgregaCC = New System.Windows.Forms.ToolStripButton()
        Me.btnEditarCC = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaCC = New System.Windows.Forms.ToolStripButton()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtCodigoBeneficiario = New System.Windows.Forms.TextBox()
        Me.btnBeneficiario = New System.Windows.Forms.Button()
        Me.grpCheque = New System.Windows.Forms.GroupBox()
        Me.txtFecha = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.dgAsiento, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuComisiones.SuspendLayout()
        Me.grpCheque.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(668, 339)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 78
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
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 339)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(833, 30)
        Me.lblInfo.TabIndex = 79
        '
        'txtDocumento
        '
        Me.txtDocumento.Location = New System.Drawing.Point(127, 55)
        Me.txtDocumento.MaxLength = 15
        Me.txtDocumento.Name = "txtDocumento"
        Me.txtDocumento.Size = New System.Drawing.Size(155, 20)
        Me.txtDocumento.TabIndex = 81
        '
        'txtConcepto
        '
        Me.txtConcepto.Location = New System.Drawing.Point(127, 76)
        Me.txtConcepto.MaxLength = 250
        Me.txtConcepto.Multiline = True
        Me.txtConcepto.Name = "txtConcepto"
        Me.txtConcepto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtConcepto.Size = New System.Drawing.Size(523, 38)
        Me.txtConcepto.TabIndex = 82
        '
        'txtImporte
        '
        Me.txtImporte.Location = New System.Drawing.Point(127, 115)
        Me.txtImporte.MaxLength = 19
        Me.txtImporte.Name = "txtImporte"
        Me.txtImporte.Size = New System.Drawing.Size(155, 20)
        Me.txtImporte.TabIndex = 83
        Me.txtImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtBeneficiario
        '
        Me.txtBeneficiario.Location = New System.Drawing.Point(127, 36)
        Me.txtBeneficiario.MaxLength = 60
        Me.txtBeneficiario.Name = "txtBeneficiario"
        Me.txtBeneficiario.Size = New System.Drawing.Size(696, 20)
        Me.txtBeneficiario.TabIndex = 84
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 20)
        Me.Label1.TabIndex = 85
        Me.Label1.Text = "Fecha"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 20)
        Me.Label2.TabIndex = 86
        Me.Label2.Text = "Tipo Movimiento"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(109, 20)
        Me.Label3.TabIndex = 87
        Me.Label3.Text = "Nº Documento"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 76)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 20)
        Me.Label4.TabIndex = 88
        Me.Label4.Text = "Concepto"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 118)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 20)
        Me.Label5.TabIndex = 89
        Me.Label5.Text = "Importe"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(6, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(109, 20)
        Me.Label6.TabIndex = 90
        Me.Label6.Text = "Beneficiario"
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(127, 33)
        Me.cmbTipo.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(155, 21)
        Me.cmbTipo.TabIndex = 102
        '
        'cmbTipoDeposito
        '
        Me.cmbTipoDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoDeposito.FormattingEnabled = True
        Me.cmbTipoDeposito.Location = New System.Drawing.Point(303, 33)
        Me.cmbTipoDeposito.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbTipoDeposito.Name = "cmbTipoDeposito"
        Me.cmbTipoDeposito.Size = New System.Drawing.Size(94, 21)
        Me.cmbTipoDeposito.TabIndex = 104
        '
        'dgAsiento
        '
        Me.dgAsiento.AllowUserToAddRows = False
        Me.dgAsiento.AllowUserToDeleteRows = False
        Me.dgAsiento.AllowUserToOrderColumns = True
        Me.dgAsiento.AllowUserToResizeColumns = False
        Me.dgAsiento.AllowUserToResizeRows = False
        Me.dgAsiento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgAsiento.Location = New System.Drawing.Point(6, 84)
        Me.dgAsiento.Name = "dgAsiento"
        Me.dgAsiento.ReadOnly = True
        Me.dgAsiento.Size = New System.Drawing.Size(817, 111)
        Me.dgAsiento.TabIndex = 108
        '
        'MenuComisiones
        '
        Me.MenuComisiones.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuComisiones.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuComisiones.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuComisiones.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaCC, Me.btnEditarCC, Me.btnEliminaCC})
        Me.MenuComisiones.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuComisiones.Location = New System.Drawing.Point(9, 54)
        Me.MenuComisiones.Name = "MenuComisiones"
        Me.MenuComisiones.Size = New System.Drawing.Size(75, 27)
        Me.MenuComisiones.TabIndex = 110
        Me.MenuComisiones.Text = "ToolStrip1"
        '
        'btnAgregaCC
        '
        Me.btnAgregaCC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaCC.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregaCC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaCC.Name = "btnAgregaCC"
        Me.btnAgregaCC.Size = New System.Drawing.Size(24, 24)
        '
        'btnEditarCC
        '
        Me.btnEditarCC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarCC.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarCC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarCC.Name = "btnEditarCC"
        Me.btnEditarCC.Size = New System.Drawing.Size(24, 24)
        '
        'btnEliminaCC
        '
        Me.btnEliminaCC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaCC.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminaCC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminaCC.Name = "btnEliminaCC"
        Me.btnEliminaCC.Size = New System.Drawing.Size(24, 24)
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(124, 59)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(462, 22)
        Me.Label7.TabIndex = 111
        Me.Label7.Text = "Póliza contable"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCodigoBeneficiario
        '
        Me.txtCodigoBeneficiario.Location = New System.Drawing.Point(127, 15)
        Me.txtCodigoBeneficiario.MaxLength = 19
        Me.txtCodigoBeneficiario.Name = "txtCodigoBeneficiario"
        Me.txtCodigoBeneficiario.Size = New System.Drawing.Size(124, 20)
        Me.txtCodigoBeneficiario.TabIndex = 112
        '
        'btnBeneficiario
        '
        Me.btnBeneficiario.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBeneficiario.Location = New System.Drawing.Point(253, 15)
        Me.btnBeneficiario.Name = "btnBeneficiario"
        Me.btnBeneficiario.Size = New System.Drawing.Size(25, 20)
        Me.btnBeneficiario.TabIndex = 113
        Me.btnBeneficiario.Text = "•••"
        Me.btnBeneficiario.UseVisualStyleBackColor = True
        '
        'grpCheque
        '
        Me.grpCheque.Controls.Add(Me.txtCodigoBeneficiario)
        Me.grpCheque.Controls.Add(Me.dgAsiento)
        Me.grpCheque.Controls.Add(Me.Label7)
        Me.grpCheque.Controls.Add(Me.btnBeneficiario)
        Me.grpCheque.Controls.Add(Me.MenuComisiones)
        Me.grpCheque.Controls.Add(Me.Label6)
        Me.grpCheque.Controls.Add(Me.txtBeneficiario)
        Me.grpCheque.Location = New System.Drawing.Point(0, 135)
        Me.grpCheque.Name = "grpCheque"
        Me.grpCheque.Size = New System.Drawing.Size(829, 204)
        Me.grpCheque.TabIndex = 114
        Me.grpCheque.TabStop = False
        '
        'txtFecha
        '
        Me.txtFecha.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtFecha.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtFecha.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFecha.Location = New System.Drawing.Point(127, 12)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(124, 19)
        Me.txtFecha.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtFecha.TabIndex = 239
        '
        'jsBanArcBancosMovimientosPlus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(833, 369)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtFecha)
        Me.Controls.Add(Me.grpCheque)
        Me.Controls.Add(Me.cmbTipoDeposito)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.cmbTipo)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtImporte)
        Me.Controls.Add(Me.txtConcepto)
        Me.Controls.Add(Me.txtDocumento)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsBanArcBancosMovimientosPlus"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Movimiento bancario"
        Me.Text = "Movimiento bancario"
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.dgAsiento, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuComisiones.ResumeLayout(False)
        Me.MenuComisiones.PerformLayout()
        Me.grpCheque.ResumeLayout(False)
        Me.grpCheque.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents txtDocumento As System.Windows.Forms.TextBox
    Friend WithEvents txtConcepto As System.Windows.Forms.TextBox
    Friend WithEvents txtImporte As System.Windows.Forms.TextBox
    Friend WithEvents txtBeneficiario As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTipoDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents dgAsiento As System.Windows.Forms.DataGridView
    Friend WithEvents MenuComisiones As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregaCC As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminaCC As System.Windows.Forms.ToolStripButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtCodigoBeneficiario As System.Windows.Forms.TextBox
    Friend WithEvents btnBeneficiario As System.Windows.Forms.Button
    Friend WithEvents grpCheque As System.Windows.Forms.GroupBox
    Friend WithEvents btnEditarCC As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtFecha As Syncfusion.WinForms.Input.SfDateTimeEdit
End Class
