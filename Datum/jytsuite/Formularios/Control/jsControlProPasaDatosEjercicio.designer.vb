<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsControlProPasaDatosEjercicio
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsControlProPasaDatosEjercicio))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpCaja = New System.Windows.Forms.GroupBox()
        Me.lblLeyenda = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkContabilidad = New System.Windows.Forms.CheckBox()
        Me.chkNomina = New System.Windows.Forms.CheckBox()
        Me.cmbEjercicio = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkBancos = New System.Windows.Forms.CheckBox()
        Me.chkCXP = New System.Windows.Forms.CheckBox()
        Me.chkCXC = New System.Windows.Forms.CheckBox()
        Me.chkPOS = New System.Windows.Forms.CheckBox()
        Me.chkMercancias = New System.Windows.Forms.CheckBox()
        Me.chkProduccion = New System.Windows.Forms.CheckBox()
        Me.grpCaja.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Location = New System.Drawing.Point(-1, 449)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(660, 31)
        Me.lblInfo.TabIndex = 80
        '
        'grpCaja
        '
        Me.grpCaja.BackColor = System.Drawing.SystemColors.Control
        Me.grpCaja.Controls.Add(Me.lblLeyenda)
        Me.grpCaja.Location = New System.Drawing.Point(1, 67)
        Me.grpCaja.Name = "grpCaja"
        Me.grpCaja.Size = New System.Drawing.Size(755, 218)
        Me.grpCaja.TabIndex = 82
        Me.grpCaja.TabStop = False
        '
        'lblLeyenda
        '
        Me.lblLeyenda.BackColor = System.Drawing.Color.White
        Me.lblLeyenda.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLeyenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeyenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.lblLeyenda.Location = New System.Drawing.Point(14, 17)
        Me.lblLeyenda.Name = "lblLeyenda"
        Me.lblLeyenda.Size = New System.Drawing.Size(741, 147)
        Me.lblLeyenda.TabIndex = 90
        Me.lblLeyenda.Text = " "
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(568, 449)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(192, 35)
        Me.grpAceptarSalir.TabIndex = 85
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = Global.Datum.My.Resources.Resources.button_cancel
        Me.btnCancel.Location = New System.Drawing.Point(99, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(89, 28)
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
        Me.btnOK.Size = New System.Drawing.Size(89, 28)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'C1PictureBox1
        '
        Me.C1PictureBox1.Image = Global.Datum.My.Resources.Resources.banda_amarilla
        Me.C1PictureBox1.Location = New System.Drawing.Point(8, 1)
        Me.C1PictureBox1.Name = "C1PictureBox1"
        Me.C1PictureBox1.Size = New System.Drawing.Size(745, 70)
        Me.C1PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.C1PictureBox1.TabIndex = 86
        Me.C1PictureBox1.TabStop = False
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(1, 47)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(286, 24)
        Me.Label9.TabIndex = 87
        Me.Label9.Text = "Control : Pase de datos a histórico"
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
        Me.Label10.Size = New System.Drawing.Size(286, 46)
        Me.Label10.TabIndex = 88
        Me.Label10.Text = "Datum"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 299)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(280, 23)
        Me.Label1.TabIndex = 89
        Me.Label1.Text = "procesar datos a histórico de"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkContabilidad
        '
        Me.chkContabilidad.AutoSize = True
        Me.chkContabilidad.Checked = True
        Me.chkContabilidad.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkContabilidad.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkContabilidad.Location = New System.Drawing.Point(103, 325)
        Me.chkContabilidad.Name = "chkContabilidad"
        Me.chkContabilidad.Size = New System.Drawing.Size(110, 19)
        Me.chkContabilidad.TabIndex = 90
        Me.chkContabilidad.Text = "Contabilidad"
        Me.chkContabilidad.UseVisualStyleBackColor = True
        '
        'chkNomina
        '
        Me.chkNomina.AutoSize = True
        Me.chkNomina.Checked = True
        Me.chkNomina.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNomina.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNomina.Location = New System.Drawing.Point(400, 325)
        Me.chkNomina.Name = "chkNomina"
        Me.chkNomina.Size = New System.Drawing.Size(138, 19)
        Me.chkNomina.TabIndex = 91
        Me.chkNomina.Text = "Recursos Humanos"
        Me.chkNomina.UseVisualStyleBackColor = True
        '
        'cmbEjercicio
        '
        Me.cmbEjercicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEjercicio.FormattingEnabled = True
        Me.cmbEjercicio.Location = New System.Drawing.Point(315, 396)
        Me.cmbEjercicio.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbEjercicio.Name = "cmbEjercicio"
        Me.cmbEjercicio.Size = New System.Drawing.Size(390, 23)
        Me.cmbEjercicio.TabIndex = 134
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 396)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(280, 23)
        Me.Label2.TabIndex = 135
        Me.Label2.Text = "cuyo período es"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkBancos
        '
        Me.chkBancos.AutoSize = True
        Me.chkBancos.Checked = True
        Me.chkBancos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBancos.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBancos.Location = New System.Drawing.Point(248, 325)
        Me.chkBancos.Name = "chkBancos"
        Me.chkBancos.Size = New System.Drawing.Size(124, 19)
        Me.chkBancos.TabIndex = 136
        Me.chkBancos.Text = "Bancos y Cajas"
        Me.chkBancos.UseVisualStyleBackColor = True
        '
        'chkCXP
        '
        Me.chkCXP.AutoSize = True
        Me.chkCXP.Checked = True
        Me.chkCXP.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCXP.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCXP.Location = New System.Drawing.Point(568, 325)
        Me.chkCXP.Name = "chkCXP"
        Me.chkCXP.Size = New System.Drawing.Size(117, 19)
        Me.chkCXP.TabIndex = 137
        Me.chkCXP.Text = "Compras y CxP"
        Me.chkCXP.UseVisualStyleBackColor = True
        '
        'chkCXC
        '
        Me.chkCXC.AutoSize = True
        Me.chkCXC.Checked = True
        Me.chkCXC.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCXC.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCXC.Location = New System.Drawing.Point(103, 352)
        Me.chkCXC.Name = "chkCXC"
        Me.chkCXC.Size = New System.Drawing.Size(110, 19)
        Me.chkCXC.TabIndex = 138
        Me.chkCXC.Text = "Ventas y CxC"
        Me.chkCXC.UseVisualStyleBackColor = True
        '
        'chkPOS
        '
        Me.chkPOS.AutoSize = True
        Me.chkPOS.Checked = True
        Me.chkPOS.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPOS.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPOS.Location = New System.Drawing.Point(248, 352)
        Me.chkPOS.Name = "chkPOS"
        Me.chkPOS.Size = New System.Drawing.Size(47, 19)
        Me.chkPOS.TabIndex = 139
        Me.chkPOS.Text = "POS"
        Me.chkPOS.UseVisualStyleBackColor = True
        '
        'chkMercancias
        '
        Me.chkMercancias.AutoSize = True
        Me.chkMercancias.Checked = True
        Me.chkMercancias.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMercancias.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMercancias.Location = New System.Drawing.Point(400, 354)
        Me.chkMercancias.Name = "chkMercancias"
        Me.chkMercancias.Size = New System.Drawing.Size(96, 19)
        Me.chkMercancias.TabIndex = 140
        Me.chkMercancias.Text = "Mercancías"
        Me.chkMercancias.UseVisualStyleBackColor = True
        '
        'chkProduccion
        '
        Me.chkProduccion.AutoSize = True
        Me.chkProduccion.Checked = True
        Me.chkProduccion.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkProduccion.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkProduccion.Location = New System.Drawing.Point(568, 354)
        Me.chkProduccion.Name = "chkProduccion"
        Me.chkProduccion.Size = New System.Drawing.Size(96, 19)
        Me.chkProduccion.TabIndex = 141
        Me.chkProduccion.Text = "Producción"
        Me.chkProduccion.UseVisualStyleBackColor = True
        '
        'jsControlProPasaDatosEjercicio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(758, 481)
        Me.ControlBox = False
        Me.Controls.Add(Me.chkProduccion)
        Me.Controls.Add(Me.chkMercancias)
        Me.Controls.Add(Me.chkPOS)
        Me.Controls.Add(Me.chkCXC)
        Me.Controls.Add(Me.chkCXP)
        Me.Controls.Add(Me.chkBancos)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmbEjercicio)
        Me.Controls.Add(Me.chkNomina)
        Me.Controls.Add(Me.chkContabilidad)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpCaja)
        Me.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsControlProPasaDatosEjercicio"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Conciliación bancaria"
        Me.grpCaja.ResumeLayout(False)
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpCaja As System.Windows.Forms.GroupBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblLeyenda As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkContabilidad As System.Windows.Forms.CheckBox
    Friend WithEvents chkNomina As System.Windows.Forms.CheckBox
    Friend WithEvents cmbEjercicio As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkBancos As System.Windows.Forms.CheckBox
    Friend WithEvents chkCXP As System.Windows.Forms.CheckBox
    Friend WithEvents chkCXC As System.Windows.Forms.CheckBox
    Friend WithEvents chkPOS As System.Windows.Forms.CheckBox
    Friend WithEvents chkMercancias As System.Windows.Forms.CheckBox
    Friend WithEvents chkProduccion As System.Windows.Forms.CheckBox
End Class
