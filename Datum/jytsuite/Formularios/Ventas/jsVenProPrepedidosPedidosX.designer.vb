<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsVenProPrepedidosPedidos
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
        Me.txtHasta = New System.Windows.Forms.TextBox()
        Me.txtDesde = New System.Windows.Forms.TextBox()
        Me.btnFecha = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lvAsesores = New System.Windows.Forms.ListView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFecha = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnDesde = New System.Windows.Forms.Button()
        Me.btnHasta = New System.Windows.Forms.Button()
        Me.lvPrepedidos = New System.Windows.Forms.ListView()
        Me.txtPesoPrepedidos = New System.Windows.Forms.TextBox()
        Me.txtTotalPrepedidos = New System.Windows.Forms.TextBox()
        Me.txtItems = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblProgreso = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dgDescuentos = New System.Windows.Forms.DataGridView()
        Me.tbc = New System.Windows.Forms.TabControl()
        Me.tbcPesos = New System.Windows.Forms.TabPage()
        Me.tbcDescuentos = New System.Windows.Forms.TabPage()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtItemsFactura = New System.Windows.Forms.TextBox()
        Me.cmbGrupos = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dgGrupos = New System.Windows.Forms.DataGridView()
        Me.MenuEquivalencia = New System.Windows.Forms.ToolStrip()
        Me.btnAgregaGrupo = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaGrupo = New System.Windows.Forms.ToolStripButton()
        Me.dgPedidos = New System.Windows.Forms.DataGridView()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTotales.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgDescuentos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbc.SuspendLayout()
        Me.tbcPesos.SuspendLayout()
        Me.tbcDescuentos.SuspendLayout()
        CType(Me.dgGrupos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuEquivalencia.SuspendLayout()
        CType(Me.dgPedidos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 553)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(966, 26)
        Me.lblInfo.TabIndex = 80
        '
        'txtHasta
        '
        Me.txtHasta.Enabled = False
        Me.txtHasta.Location = New System.Drawing.Point(125, 112)
        Me.txtHasta.Name = "txtHasta"
        Me.txtHasta.Size = New System.Drawing.Size(85, 20)
        Me.txtHasta.TabIndex = 82
        Me.txtHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDesde
        '
        Me.txtDesde.Location = New System.Drawing.Point(125, 91)
        Me.txtDesde.MaxLength = 15
        Me.txtDesde.Name = "txtDesde"
        Me.txtDesde.Size = New System.Drawing.Size(85, 20)
        Me.txtDesde.TabIndex = 83
        Me.txtDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnFecha
        '
        Me.btnFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFecha.Location = New System.Drawing.Point(215, 70)
        Me.btnFecha.Name = "btnFecha"
        Me.btnFecha.Size = New System.Drawing.Size(29, 20)
        Me.btnFecha.TabIndex = 105
        Me.btnFecha.Text = "•••"
        Me.btnFecha.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(9, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 16)
        Me.Label3.TabIndex = 108
        Me.Label3.Text = "Pre-pedidos desde"
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(788, 548)
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
        'lvAsesores
        '
        Me.lvAsesores.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lvAsesores.CheckBoxes = True
        Me.lvAsesores.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvAsesores.FullRowSelect = True
        Me.lvAsesores.GridLines = True
        Me.lvAsesores.Location = New System.Drawing.Point(2, 138)
        Me.lvAsesores.Name = "lvAsesores"
        Me.lvAsesores.Size = New System.Drawing.Size(242, 354)
        Me.lvAsesores.TabIndex = 115
        Me.lvAsesores.UseCompatibleStateImageBehavior = False
        Me.lvAsesores.View = System.Windows.Forms.View.Details
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(80, 111)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 20)
        Me.Label2.TabIndex = 117
        Me.Label2.Text = "hasta"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFecha
        '
        Me.txtFecha.Enabled = False
        Me.txtFecha.Location = New System.Drawing.Point(125, 70)
        Me.txtFecha.MaxLength = 15
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(85, 20)
        Me.txtFecha.TabIndex = 124
        Me.txtFecha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 70)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(102, 20)
        Me.Label9.TabIndex = 125
        Me.Label9.Text = "Fecha proceso"
        '
        'btnDesde
        '
        Me.btnDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDesde.Location = New System.Drawing.Point(215, 91)
        Me.btnDesde.Name = "btnDesde"
        Me.btnDesde.Size = New System.Drawing.Size(29, 20)
        Me.btnDesde.TabIndex = 131
        Me.btnDesde.Text = "•••"
        Me.btnDesde.UseVisualStyleBackColor = True
        '
        'btnHasta
        '
        Me.btnHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHasta.Location = New System.Drawing.Point(215, 112)
        Me.btnHasta.Name = "btnHasta"
        Me.btnHasta.Size = New System.Drawing.Size(29, 20)
        Me.btnHasta.TabIndex = 132
        Me.btnHasta.Text = "•••"
        Me.btnHasta.UseVisualStyleBackColor = True
        '
        'lvPrepedidos
        '
        Me.lvPrepedidos.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lvPrepedidos.CheckBoxes = True
        Me.lvPrepedidos.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvPrepedidos.FullRowSelect = True
        Me.lvPrepedidos.GridLines = True
        Me.lvPrepedidos.Location = New System.Drawing.Point(250, 138)
        Me.lvPrepedidos.Name = "lvPrepedidos"
        Me.lvPrepedidos.Size = New System.Drawing.Size(713, 69)
        Me.lvPrepedidos.TabIndex = 133
        Me.lvPrepedidos.UseCompatibleStateImageBehavior = False
        Me.lvPrepedidos.View = System.Windows.Forms.View.Details
        '
        'txtPesoPrepedidos
        '
        Me.txtPesoPrepedidos.Enabled = False
        Me.txtPesoPrepedidos.Location = New System.Drawing.Point(392, 294)
        Me.txtPesoPrepedidos.MaxLength = 15
        Me.txtPesoPrepedidos.Name = "txtPesoPrepedidos"
        Me.txtPesoPrepedidos.Size = New System.Drawing.Size(98, 20)
        Me.txtPesoPrepedidos.TabIndex = 135
        Me.txtPesoPrepedidos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalPrepedidos
        '
        Me.txtTotalPrepedidos.Enabled = False
        Me.txtTotalPrepedidos.Location = New System.Drawing.Point(865, 291)
        Me.txtTotalPrepedidos.MaxLength = 15
        Me.txtTotalPrepedidos.Name = "txtTotalPrepedidos"
        Me.txtTotalPrepedidos.Size = New System.Drawing.Size(98, 20)
        Me.txtTotalPrepedidos.TabIndex = 136
        Me.txtTotalPrepedidos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtItems
        '
        Me.txtItems.Enabled = False
        Me.txtItems.Location = New System.Drawing.Point(597, 293)
        Me.txtItems.MaxLength = 15
        Me.txtItems.Name = "txtItems"
        Me.txtItems.Size = New System.Drawing.Size(98, 20)
        Me.txtItems.TabIndex = 137
        Me.txtItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(491, 293)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 21)
        Me.Label8.TabIndex = 145
        Me.Label8.Text = "No. pre-pedidos"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(247, 295)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(139, 16)
        Me.Label10.TabIndex = 146
        Me.Label10.Text = "Peso total pre-pedidos"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(715, 291)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(150, 20)
        Me.Label11.TabIndex = 147
        Me.Label11.Text = "Monto total pre-pedidos"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        Me.Label12.Size = New System.Drawing.Size(333, 40)
        Me.Label12.TabIndex = 148
        Me.Label12.Text = "Datum"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'C1PictureBox1
        '
        Me.C1PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.C1PictureBox1.Image = Global.Datum.My.Resources.Resources.banda_amarilla
        Me.C1PictureBox1.Location = New System.Drawing.Point(331, 0)
        Me.C1PictureBox1.Name = "C1PictureBox1"
        Me.C1PictureBox1.Size = New System.Drawing.Size(635, 61)
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
        Me.Label13.Size = New System.Drawing.Size(333, 21)
        Me.Label13.TabIndex = 150
        Me.Label13.Tag = ""
        Me.Label13.Text = "Pase de pre-pedidos a pedidos"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpTotales
        '
        Me.grpTotales.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTotales.Controls.Add(Me.ProgressBar1)
        Me.grpTotales.Controls.Add(Me.Label14)
        Me.grpTotales.Controls.Add(Me.lblProgreso)
        Me.grpTotales.Location = New System.Drawing.Point(0, 487)
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
        Me.dg.Location = New System.Drawing.Point(6, 26)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(693, 123)
        Me.dg.TabIndex = 152
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(435, 19)
        Me.Label1.TabIndex = 200
        Me.Label1.Text = "Por favor indique los pesos reales para pedidos"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dgDescuentos
        '
        Me.dgDescuentos.AllowUserToAddRows = False
        Me.dgDescuentos.AllowUserToDeleteRows = False
        Me.dgDescuentos.AllowUserToOrderColumns = True
        Me.dgDescuentos.AllowUserToResizeColumns = False
        Me.dgDescuentos.AllowUserToResizeRows = False
        Me.dgDescuentos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgDescuentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgDescuentos.Location = New System.Drawing.Point(6, 25)
        Me.dgDescuentos.Name = "dgDescuentos"
        Me.dgDescuentos.ReadOnly = True
        Me.dgDescuentos.Size = New System.Drawing.Size(696, 140)
        Me.dgDescuentos.TabIndex = 201
        '
        'tbc
        '
        Me.tbc.Controls.Add(Me.tbcPesos)
        Me.tbc.Controls.Add(Me.tbcDescuentos)
        Me.tbc.Location = New System.Drawing.Point(250, 314)
        Me.tbc.Name = "tbc"
        Me.tbc.SelectedIndex = 0
        Me.tbc.Size = New System.Drawing.Size(713, 178)
        Me.tbc.TabIndex = 202
        '
        'tbcPesos
        '
        Me.tbcPesos.Controls.Add(Me.dg)
        Me.tbcPesos.Controls.Add(Me.Label1)
        Me.tbcPesos.Location = New System.Drawing.Point(4, 22)
        Me.tbcPesos.Name = "tbcPesos"
        Me.tbcPesos.Padding = New System.Windows.Forms.Padding(3)
        Me.tbcPesos.Size = New System.Drawing.Size(705, 152)
        Me.tbcPesos.TabIndex = 0
        Me.tbcPesos.Text = "Pesos"
        Me.tbcPesos.UseVisualStyleBackColor = True
        '
        'tbcDescuentos
        '
        Me.tbcDescuentos.Controls.Add(Me.Label4)
        Me.tbcDescuentos.Controls.Add(Me.dgDescuentos)
        Me.tbcDescuentos.Location = New System.Drawing.Point(4, 22)
        Me.tbcDescuentos.Name = "tbcDescuentos"
        Me.tbcDescuentos.Padding = New System.Windows.Forms.Padding(3)
        Me.tbcDescuentos.Size = New System.Drawing.Size(705, 152)
        Me.tbcDescuentos.TabIndex = 1
        Me.tbcDescuentos.Text = "Descuentos"
        Me.tbcDescuentos.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(435, 19)
        Me.Label4.TabIndex = 202
        Me.Label4.Text = "Por favor indique los descuentos reales para pedidos"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(251, 67)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(139, 40)
        Me.Label5.TabIndex = 203
        Me.Label5.Text = "Número máximo de líneas en pedidos y/o facturas"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtItemsFactura
        '
        Me.txtItemsFactura.Location = New System.Drawing.Point(396, 67)
        Me.txtItemsFactura.MaxLength = 15
        Me.txtItemsFactura.Name = "txtItemsFactura"
        Me.txtItemsFactura.Size = New System.Drawing.Size(48, 20)
        Me.txtItemsFactura.TabIndex = 204
        Me.txtItemsFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbGrupos
        '
        Me.cmbGrupos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGrupos.FormattingEnabled = True
        Me.cmbGrupos.Location = New System.Drawing.Point(494, 105)
        Me.cmbGrupos.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbGrupos.Name = "cmbGrupos"
        Me.cmbGrupos.Size = New System.Drawing.Size(201, 21)
        Me.cmbGrupos.TabIndex = 205
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(491, 67)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 37)
        Me.Label6.TabIndex = 206
        Me.Label6.Text = "Pedidos serán agrupados por :"
        '
        'dgGrupos
        '
        Me.dgGrupos.AllowUserToAddRows = False
        Me.dgGrupos.AllowUserToDeleteRows = False
        Me.dgGrupos.AllowUserToOrderColumns = True
        Me.dgGrupos.AllowUserToResizeColumns = False
        Me.dgGrupos.AllowUserToResizeRows = False
        Me.dgGrupos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgGrupos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgGrupos.Location = New System.Drawing.Point(753, 67)
        Me.dgGrupos.Name = "dgGrupos"
        Me.dgGrupos.ReadOnly = True
        Me.dgGrupos.Size = New System.Drawing.Size(210, 65)
        Me.dgGrupos.TabIndex = 207
        '
        'MenuEquivalencia
        '
        Me.MenuEquivalencia.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuEquivalencia.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuEquivalencia.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuEquivalencia.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaGrupo, Me.btnEliminaGrupo})
        Me.MenuEquivalencia.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuEquivalencia.Location = New System.Drawing.Point(718, 67)
        Me.MenuEquivalencia.Name = "MenuEquivalencia"
        Me.MenuEquivalencia.Size = New System.Drawing.Size(25, 56)
        Me.MenuEquivalencia.TabIndex = 208
        Me.MenuEquivalencia.Text = "ToolStrip1"
        '
        'btnAgregaGrupo
        '
        Me.btnAgregaGrupo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaGrupo.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregaGrupo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaGrupo.Name = "btnAgregaGrupo"
        Me.btnAgregaGrupo.Size = New System.Drawing.Size(23, 24)
        '
        'btnEliminaGrupo
        '
        Me.btnEliminaGrupo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaGrupo.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminaGrupo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminaGrupo.Name = "btnEliminaGrupo"
        Me.btnEliminaGrupo.Size = New System.Drawing.Size(23, 24)
        '
        'dgPedidos
        '
        Me.dgPedidos.AllowUserToAddRows = False
        Me.dgPedidos.AllowUserToDeleteRows = False
        Me.dgPedidos.AllowUserToOrderColumns = True
        Me.dgPedidos.AllowUserToResizeColumns = False
        Me.dgPedidos.AllowUserToResizeRows = False
        Me.dgPedidos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgPedidos.Location = New System.Drawing.Point(250, 213)
        Me.dgPedidos.Name = "dgPedidos"
        Me.dgPedidos.ReadOnly = True
        Me.dgPedidos.Size = New System.Drawing.Size(713, 72)
        Me.dgPedidos.TabIndex = 209
        '
        'jsVenProPrepedidosPedidos
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(966, 579)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgPedidos)
        Me.Controls.Add(Me.MenuEquivalencia)
        Me.Controls.Add(Me.dgGrupos)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbGrupos)
        Me.Controls.Add(Me.txtItemsFactura)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.tbc)
        Me.Controls.Add(Me.lvAsesores)
        Me.Controls.Add(Me.txtTotalPrepedidos)
        Me.Controls.Add(Me.grpTotales)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtItems)
        Me.Controls.Add(Me.txtPesoPrepedidos)
        Me.Controls.Add(Me.lvPrepedidos)
        Me.Controls.Add(Me.btnHasta)
        Me.Controls.Add(Me.btnDesde)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtFecha)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnFecha)
        Me.Controls.Add(Me.txtDesde)
        Me.Controls.Add(Me.txtHasta)
        Me.Controls.Add(Me.lblInfo)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "jsVenProPrepedidosPedidos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Proceso Pre-Pedidos"
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTotales.ResumeLayout(False)
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgDescuentos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbc.ResumeLayout(False)
        Me.tbcPesos.ResumeLayout(False)
        Me.tbcDescuentos.ResumeLayout(False)
        CType(Me.dgGrupos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuEquivalencia.ResumeLayout(False)
        Me.MenuEquivalencia.PerformLayout()
        CType(Me.dgPedidos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents txtHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtDesde As System.Windows.Forms.TextBox
    Friend WithEvents btnFecha As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lvAsesores As System.Windows.Forms.ListView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtFecha As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnDesde As System.Windows.Forms.Button
    Friend WithEvents btnHasta As System.Windows.Forms.Button
    Friend WithEvents lvPrepedidos As System.Windows.Forms.ListView
    Friend WithEvents txtPesoPrepedidos As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalPrepedidos As System.Windows.Forms.TextBox
    Friend WithEvents txtItems As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents grpTotales As System.Windows.Forms.GroupBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents lblProgreso As System.Windows.Forms.Label
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgDescuentos As System.Windows.Forms.DataGridView
    Friend WithEvents tbc As System.Windows.Forms.TabControl
    Friend WithEvents tbcPesos As System.Windows.Forms.TabPage
    Friend WithEvents tbcDescuentos As System.Windows.Forms.TabPage
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtItemsFactura As System.Windows.Forms.TextBox
    Friend WithEvents cmbGrupos As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dgGrupos As System.Windows.Forms.DataGridView
    Friend WithEvents MenuEquivalencia As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregaGrupo As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminaGrupo As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgPedidos As System.Windows.Forms.DataGridView
End Class
