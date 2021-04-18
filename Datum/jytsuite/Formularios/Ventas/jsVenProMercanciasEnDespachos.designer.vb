<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsVenProMercanciasEnDespachos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsVenProMercanciasEnDespachos))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.grpEncab = New System.Windows.Forms.GroupBox()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.btnAsesorHasta = New System.Windows.Forms.Button()
        Me.btnAsesorDesde = New System.Windows.Forms.Button()
        Me.btnMercanciaDesde = New System.Windows.Forms.Button()
        Me.btnFechaDesde = New System.Windows.Forms.Button()
        Me.btnMercanciaHasta = New System.Windows.Forms.Button()
        Me.txtAsesorHasta = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtMercanciaHasta = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtFechaDesde = New System.Windows.Forms.TextBox()
        Me.txtAsesorDesde = New System.Windows.Forms.TextBox()
        Me.btnFechaHasta = New System.Windows.Forms.Button()
        Me.txtMercanciaDesde = New System.Windows.Forms.TextBox()
        Me.txtFechaHasta = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEncab.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 410)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(754, 28)
        Me.lblInfo.TabIndex = 80
        '
        'dg
        '
        Me.dg.AllowUserToAddRows = False
        Me.dg.AllowUserToDeleteRows = False
        Me.dg.AllowUserToOrderColumns = True
        Me.dg.AllowUserToResizeColumns = False
        Me.dg.AllowUserToResizeRows = False
        Me.dg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg.Location = New System.Drawing.Point(0, 184)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(755, 217)
        Me.dg.TabIndex = 82
        '
        'grpEncab
        '
        Me.grpEncab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncab.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpEncab.Controls.Add(Me.btnGo)
        Me.grpEncab.Controls.Add(Me.btnAsesorHasta)
        Me.grpEncab.Controls.Add(Me.btnAsesorDesde)
        Me.grpEncab.Controls.Add(Me.btnMercanciaDesde)
        Me.grpEncab.Controls.Add(Me.btnFechaDesde)
        Me.grpEncab.Controls.Add(Me.btnMercanciaHasta)
        Me.grpEncab.Controls.Add(Me.txtAsesorHasta)
        Me.grpEncab.Controls.Add(Me.Label5)
        Me.grpEncab.Controls.Add(Me.Label4)
        Me.grpEncab.Controls.Add(Me.txtMercanciaHasta)
        Me.grpEncab.Controls.Add(Me.Label11)
        Me.grpEncab.Controls.Add(Me.txtFechaDesde)
        Me.grpEncab.Controls.Add(Me.txtAsesorDesde)
        Me.grpEncab.Controls.Add(Me.btnFechaHasta)
        Me.grpEncab.Controls.Add(Me.txtMercanciaDesde)
        Me.grpEncab.Controls.Add(Me.txtFechaHasta)
        Me.grpEncab.Controls.Add(Me.Label2)
        Me.grpEncab.Location = New System.Drawing.Point(1, 54)
        Me.grpEncab.Name = "grpEncab"
        Me.grpEncab.Size = New System.Drawing.Size(754, 124)
        Me.grpEncab.TabIndex = 85
        Me.grpEncab.TabStop = False
        '
        'btnGo
        '
        Me.btnGo.AutoSize = True
        Me.btnGo.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGo.Image = CType(resources.GetObject("btnGo.Image"), System.Drawing.Image)
        Me.btnGo.Location = New System.Drawing.Point(705, 86)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(43, 38)
        Me.btnGo.TabIndex = 231
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'btnAsesorHasta
        '
        Me.btnAsesorHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAsesorHasta.Location = New System.Drawing.Point(723, 62)
        Me.btnAsesorHasta.Name = "btnAsesorHasta"
        Me.btnAsesorHasta.Size = New System.Drawing.Size(25, 20)
        Me.btnAsesorHasta.TabIndex = 230
        Me.btnAsesorHasta.Text = "•••"
        Me.btnAsesorHasta.UseVisualStyleBackColor = True
        '
        'btnAsesorDesde
        '
        Me.btnAsesorDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAsesorDesde.Location = New System.Drawing.Point(578, 62)
        Me.btnAsesorDesde.Name = "btnAsesorDesde"
        Me.btnAsesorDesde.Size = New System.Drawing.Size(25, 20)
        Me.btnAsesorDesde.TabIndex = 229
        Me.btnAsesorDesde.Text = "•••"
        Me.btnAsesorDesde.UseVisualStyleBackColor = True
        '
        'btnMercanciaDesde
        '
        Me.btnMercanciaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMercanciaDesde.Location = New System.Drawing.Point(578, 39)
        Me.btnMercanciaDesde.Name = "btnMercanciaDesde"
        Me.btnMercanciaDesde.Size = New System.Drawing.Size(25, 20)
        Me.btnMercanciaDesde.TabIndex = 228
        Me.btnMercanciaDesde.Text = "•••"
        Me.btnMercanciaDesde.UseVisualStyleBackColor = True
        '
        'btnFechaDesde
        '
        Me.btnFechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechaDesde.Location = New System.Drawing.Point(578, 18)
        Me.btnFechaDesde.Name = "btnFechaDesde"
        Me.btnFechaDesde.Size = New System.Drawing.Size(25, 20)
        Me.btnFechaDesde.TabIndex = 227
        Me.btnFechaDesde.Text = "•••"
        Me.btnFechaDesde.UseVisualStyleBackColor = True
        '
        'btnMercanciaHasta
        '
        Me.btnMercanciaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMercanciaHasta.Location = New System.Drawing.Point(723, 39)
        Me.btnMercanciaHasta.Name = "btnMercanciaHasta"
        Me.btnMercanciaHasta.Size = New System.Drawing.Size(25, 20)
        Me.btnMercanciaHasta.TabIndex = 226
        Me.btnMercanciaHasta.Text = "•••"
        Me.btnMercanciaHasta.UseVisualStyleBackColor = True
        '
        'txtAsesorHasta
        '
        Me.txtAsesorHasta.Enabled = False
        Me.txtAsesorHasta.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsesorHasta.Location = New System.Drawing.Point(615, 62)
        Me.txtAsesorHasta.MaxLength = 19
        Me.txtAsesorHasta.Name = "txtAsesorHasta"
        Me.txtAsesorHasta.Size = New System.Drawing.Size(102, 22)
        Me.txtAsesorHasta.TabIndex = 223
        Me.txtAsesorHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(353, 19)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(108, 19)
        Me.Label5.TabIndex = 222
        Me.Label5.Text = "Período :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(353, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(108, 19)
        Me.Label4.TabIndex = 221
        Me.Label4.Text = "Mercancía :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtMercanciaHasta
        '
        Me.txtMercanciaHasta.Enabled = False
        Me.txtMercanciaHasta.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMercanciaHasta.Location = New System.Drawing.Point(615, 39)
        Me.txtMercanciaHasta.MaxLength = 19
        Me.txtMercanciaHasta.Name = "txtMercanciaHasta"
        Me.txtMercanciaHasta.Size = New System.Drawing.Size(102, 22)
        Me.txtMercanciaHasta.TabIndex = 214
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(353, 62)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(108, 19)
        Me.Label11.TabIndex = 207
        Me.Label11.Text = "Asesor :"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFechaDesde
        '
        Me.txtFechaDesde.Enabled = False
        Me.txtFechaDesde.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaDesde.Location = New System.Drawing.Point(467, 16)
        Me.txtFechaDesde.MaxLength = 19
        Me.txtFechaDesde.Name = "txtFechaDesde"
        Me.txtFechaDesde.Size = New System.Drawing.Size(105, 22)
        Me.txtFechaDesde.TabIndex = 206
        Me.txtFechaDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAsesorDesde
        '
        Me.txtAsesorDesde.Enabled = False
        Me.txtAsesorDesde.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsesorDesde.Location = New System.Drawing.Point(467, 62)
        Me.txtAsesorDesde.MaxLength = 19
        Me.txtAsesorDesde.Name = "txtAsesorDesde"
        Me.txtAsesorDesde.Size = New System.Drawing.Size(105, 22)
        Me.txtAsesorDesde.TabIndex = 204
        Me.txtAsesorDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnFechaHasta
        '
        Me.btnFechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechaHasta.Location = New System.Drawing.Point(723, 16)
        Me.btnFechaHasta.Name = "btnFechaHasta"
        Me.btnFechaHasta.Size = New System.Drawing.Size(25, 20)
        Me.btnFechaHasta.TabIndex = 112
        Me.btnFechaHasta.Text = "•••"
        Me.btnFechaHasta.UseVisualStyleBackColor = True
        '
        'txtMercanciaDesde
        '
        Me.txtMercanciaDesde.Enabled = False
        Me.txtMercanciaDesde.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMercanciaDesde.Location = New System.Drawing.Point(467, 39)
        Me.txtMercanciaDesde.MaxLength = 19
        Me.txtMercanciaDesde.Name = "txtMercanciaDesde"
        Me.txtMercanciaDesde.Size = New System.Drawing.Size(105, 22)
        Me.txtMercanciaDesde.TabIndex = 5
        '
        'txtFechaHasta
        '
        Me.txtFechaHasta.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFechaHasta.Location = New System.Drawing.Point(615, 15)
        Me.txtFechaHasta.MaxLength = 50
        Me.txtFechaHasta.Multiline = True
        Me.txtFechaHasta.Name = "txtFechaHasta"
        Me.txtFechaHasta.Size = New System.Drawing.Size(102, 21)
        Me.txtFechaHasta.TabIndex = 4
        Me.txtFechaHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.White
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label2.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(341, 63)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Proceso para cambiar las cantidades en pedidos . Por Período. Por Mercancia. Por " & _
            "Fecha "
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(589, 407)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 87
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
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.C1SuperTooltip1.IsBalloon = True
        Me.C1SuperTooltip1.ShowAlways = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, -3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 61)
        Me.Label3.TabIndex = 92
        Me.Label3.Text = "Mercancías en pedidos"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'C1PictureBox1
        '
        Me.C1PictureBox1.Image = Global.Datum.My.Resources.Resources.banda_amarilla
        Me.C1PictureBox1.Location = New System.Drawing.Point(115, -3)
        Me.C1PictureBox1.Name = "C1PictureBox1"
        Me.C1PictureBox1.Size = New System.Drawing.Size(639, 61)
        Me.C1PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.C1PictureBox1.TabIndex = 88
        Me.C1PictureBox1.TabStop = False
        '
        'jsVenProMercanciasEnDespachos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(754, 438)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpEncab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsVenProMercanciasEnDespachos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "Pedidos"
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEncab.ResumeLayout(False)
        Me.grpEncab.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents grpEncab As System.Windows.Forms.GroupBox
    Friend WithEvents txtMercanciaDesde As System.Windows.Forms.TextBox
    Friend WithEvents txtFechaHasta As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents btnFechaHasta As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtFechaDesde As System.Windows.Forms.TextBox
    Friend WithEvents txtAsesorDesde As System.Windows.Forms.TextBox
    Friend WithEvents txtMercanciaHasta As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtAsesorHasta As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnMercanciaHasta As System.Windows.Forms.Button
    Friend WithEvents btnAsesorHasta As System.Windows.Forms.Button
    Friend WithEvents btnAsesorDesde As System.Windows.Forms.Button
    Friend WithEvents btnMercanciaDesde As System.Windows.Forms.Button
    Friend WithEvents btnFechaDesde As System.Windows.Forms.Button
    Friend WithEvents btnGo As System.Windows.Forms.Button
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
End Class
