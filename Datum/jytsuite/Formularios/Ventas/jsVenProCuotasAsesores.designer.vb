<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsVenProCuotasAsesores
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
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblProgreso = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.opt0 = New System.Windows.Forms.RadioButton()
        Me.opt1 = New System.Windows.Forms.RadioButton()
        Me.opt2 = New System.Windows.Forms.RadioButton()
        Me.dgMercas = New System.Windows.Forms.DataGridView()
        Me.dgAsesores = New System.Windows.Forms.DataGridView()
        Me.lblLeyenda = New System.Windows.Forms.Label()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTotales.SuspendLayout()
        CType(Me.dgMercas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgAsesores, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 540)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(964, 26)
        Me.lblInfo.TabIndex = 80
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(786, 535)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(175, 30)
        Me.grpAceptarSalir.TabIndex = 114
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Image = Global.Datum.My.Resources.Resources.button_cancel
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnCancel.Location = New System.Drawing.Point(90, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 24)
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
        Me.btnOK.Size = New System.Drawing.Size(81, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(10, 458)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(230, 21)
        Me.Label8.TabIndex = 145
        Me.Label8.Text = "Seleccione método de asignación"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(0, 155)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(139, 16)
        Me.Label10.TabIndex = 146
        Me.Label10.Text = "Asesores Comerciales"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label12.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(0, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(372, 40)
        Me.Label12.TabIndex = 148
        Me.Label12.Text = "Datum"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'C1PictureBox1
        '
        Me.C1PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.C1PictureBox1.Image = Global.Datum.My.Resources.Resources.banda_amarilla
        Me.C1PictureBox1.Location = New System.Drawing.Point(365, 0)
        Me.C1PictureBox1.Name = "C1PictureBox1"
        Me.C1PictureBox1.Size = New System.Drawing.Size(601, 61)
        Me.C1PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.C1PictureBox1.TabIndex = 149
        Me.C1PictureBox1.TabStop = False
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(0, 40)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(374, 21)
        Me.Label13.TabIndex = 150
        Me.Label13.Tag = ""
        Me.Label13.Text = "Cálculo de cuotas mensuales de mercancías para Asesores"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpTotales
        '
        Me.grpTotales.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTotales.Controls.Add(Me.ProgressBar1)
        Me.grpTotales.Controls.Add(Me.Label14)
        Me.grpTotales.Controls.Add(Me.lblProgreso)
        Me.grpTotales.Location = New System.Drawing.Point(3, 474)
        Me.grpTotales.Name = "grpTotales"
        Me.grpTotales.Size = New System.Drawing.Size(963, 63)
        Me.grpTotales.TabIndex = 151
        Me.grpTotales.TabStop = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(9, 30)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(951, 20)
        Me.ProgressBar1.TabIndex = 16
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(8, 8)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(72, 20)
        Me.Label14.TabIndex = 15
        Me.Label14.Text = "Progreso ..."
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProgreso
        '
        Me.lblProgreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgreso.Location = New System.Drawing.Point(84, 8)
        Me.lblProgreso.Name = "lblProgreso"
        Me.lblProgreso.Size = New System.Drawing.Size(822, 20)
        Me.lblProgreso.TabIndex = 14
        Me.lblProgreso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(241, 158)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(265, 13)
        Me.Label1.TabIndex = 152
        Me.Label1.Text = "Seleccione mercancías"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'opt0
        '
        Me.opt0.AutoSize = True
        Me.opt0.Location = New System.Drawing.Point(246, 462)
        Me.opt0.Name = "opt0"
        Me.opt0.Size = New System.Drawing.Size(112, 17)
        Me.opt0.TabIndex = 153
        Me.opt0.TabStop = True
        Me.opt0.Text = "Cuota Histórica"
        Me.opt0.UseVisualStyleBackColor = True
        '
        'opt1
        '
        Me.opt1.AutoSize = True
        Me.opt1.Location = New System.Drawing.Point(395, 462)
        Me.opt1.Name = "opt1"
        Me.opt1.Size = New System.Drawing.Size(94, 17)
        Me.opt1.TabIndex = 154
        Me.opt1.TabStop = True
        Me.opt1.Text = "Cuotas Fijas"
        Me.opt1.UseVisualStyleBackColor = True
        '
        'opt2
        '
        Me.opt2.AutoSize = True
        Me.opt2.Location = New System.Drawing.Point(515, 462)
        Me.opt2.Name = "opt2"
        Me.opt2.Size = New System.Drawing.Size(144, 17)
        Me.opt2.TabIndex = 155
        Me.opt2.TabStop = True
        Me.opt2.Text = "Factor de asignación"
        Me.opt2.UseVisualStyleBackColor = True
        '
        'dgMercas
        '
        Me.dgMercas.AllowUserToAddRows = False
        Me.dgMercas.AllowUserToDeleteRows = False
        Me.dgMercas.AllowUserToOrderColumns = True
        Me.dgMercas.AllowUserToResizeColumns = False
        Me.dgMercas.AllowUserToResizeRows = False
        Me.dgMercas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgMercas.Location = New System.Drawing.Point(244, 174)
        Me.dgMercas.Name = "dgMercas"
        Me.dgMercas.ReadOnly = True
        Me.dgMercas.Size = New System.Drawing.Size(719, 281)
        Me.dgMercas.TabIndex = 216
        '
        'dgAsesores
        '
        Me.dgAsesores.AllowUserToAddRows = False
        Me.dgAsesores.AllowUserToDeleteRows = False
        Me.dgAsesores.AllowUserToOrderColumns = True
        Me.dgAsesores.AllowUserToResizeColumns = False
        Me.dgAsesores.AllowUserToResizeRows = False
        Me.dgAsesores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgAsesores.Location = New System.Drawing.Point(3, 174)
        Me.dgAsesores.Name = "dgAsesores"
        Me.dgAsesores.ReadOnly = True
        Me.dgAsesores.Size = New System.Drawing.Size(238, 281)
        Me.dgAsesores.TabIndex = 217
        '
        'lblLeyenda
        '
        Me.lblLeyenda.BackColor = System.Drawing.Color.White
        Me.lblLeyenda.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLeyenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeyenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.lblLeyenda.Location = New System.Drawing.Point(3, 61)
        Me.lblLeyenda.Name = "lblLeyenda"
        Me.lblLeyenda.Size = New System.Drawing.Size(960, 83)
        Me.lblLeyenda.TabIndex = 218
        Me.lblLeyenda.Text = "Este proceso busca repartor las cuotas anuales de la empresa que están asignadas " & _
    "en CUOTAS DE MERCANCIA (MERCANCIAS/ARCHIVOS/CUOTAS ANUALES)"
        '
        'jsVenProCuotasAsesores
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(964, 566)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblLeyenda)
        Me.Controls.Add(Me.dgAsesores)
        Me.Controls.Add(Me.dgMercas)
        Me.Controls.Add(Me.opt2)
        Me.Controls.Add(Me.opt1)
        Me.Controls.Add(Me.opt0)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.grpTotales)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.lblInfo)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "jsVenProCuotasAsesores"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Proceso Pre-Pedidos"
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTotales.ResumeLayout(False)
        CType(Me.dgMercas, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgAsesores, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents grpTotales As System.Windows.Forms.GroupBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblProgreso As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents opt0 As System.Windows.Forms.RadioButton
    Friend WithEvents opt1 As System.Windows.Forms.RadioButton
    Friend WithEvents opt2 As System.Windows.Forms.RadioButton
    Friend WithEvents dgMercas As System.Windows.Forms.DataGridView
    Friend WithEvents dgAsesores As System.Windows.Forms.DataGridView
    Friend WithEvents lblLeyenda As System.Windows.Forms.Label
End Class
