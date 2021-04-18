<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsProArcSeguimientoProduccion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsProArcSeguimientoProduccion))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.dgComponentes = New System.Windows.Forms.DataGridView()
        Me.grpEncab = New System.Windows.Forms.GroupBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbEstatus = New System.Windows.Forms.ComboBox()
        Me.btnEmision = New System.Windows.Forms.Button()
        Me.txtDescripProduccion = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuBarraRenglon = New System.Windows.Forms.ToolStrip()
        Me.btnAgregarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnEditarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnBuscarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnPrimerMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnAnteriorMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.itemsrenglon = New System.Windows.Forms.ToolStripTextBox()
        Me.lblitemsrenglon = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSiguienteMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnUltimoMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.tbcSeguimiento = New C1.Win.C1Command.C1DockingTab()
        Me.tabPageFormulacion = New C1.Win.C1Command.C1DockingTabPage()
        Me.tabPageEntregas = New C1.Win.C1Command.C1DockingTabPage()
        Me.dgResidual = New System.Windows.Forms.DataGridView()
        Me.tabPagePorEntregar = New C1.Win.C1Command.C1DockingTabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.tabPageAnotaciones = New C1.Win.C1Command.C1DockingTabPage()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.dgRenglones = New System.Windows.Forms.DataGridView()
        Me.btnImprimir = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSalir = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        CType(Me.dgComponentes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEncab.SuspendLayout()
        Me.MenuBarraRenglon.SuspendLayout()
        CType(Me.tbcSeguimiento, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbcSeguimiento.SuspendLayout()
        Me.tabPageFormulacion.SuspendLayout()
        Me.tabPageEntregas.SuspendLayout()
        CType(Me.dgResidual, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPagePorEntregar.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPageAnotaciones.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgRenglones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 498)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(1144, 28)
        Me.lblInfo.TabIndex = 80
        '
        'dgComponentes
        '
        Me.dgComponentes.AllowUserToAddRows = False
        Me.dgComponentes.AllowUserToDeleteRows = False
        Me.dgComponentes.AllowUserToOrderColumns = True
        Me.dgComponentes.AllowUserToResizeColumns = False
        Me.dgComponentes.AllowUserToResizeRows = False
        Me.dgComponentes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgComponentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgComponentes.Location = New System.Drawing.Point(0, 0)
        Me.dgComponentes.Name = "dgComponentes"
        Me.dgComponentes.ReadOnly = True
        Me.dgComponentes.Size = New System.Drawing.Size(1228, 234)
        Me.dgComponentes.TabIndex = 82
        '
        'grpEncab
        '
        Me.grpEncab.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpEncab.Controls.Add(Me.CheckBox1)
        Me.grpEncab.Controls.Add(Me.Label5)
        Me.grpEncab.Controls.Add(Me.cmbEstatus)
        Me.grpEncab.Controls.Add(Me.btnEmision)
        Me.grpEncab.Controls.Add(Me.txtDescripProduccion)
        Me.grpEncab.Controls.Add(Me.txtCodigo)
        Me.grpEncab.Controls.Add(Me.Label1)
        Me.grpEncab.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpEncab.Location = New System.Drawing.Point(0, 0)
        Me.grpEncab.Name = "grpEncab"
        Me.grpEncab.Size = New System.Drawing.Size(1144, 81)
        Me.grpEncab.TabIndex = 85
        Me.grpEncab.TabStop = False
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(212, 58)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(89, 17)
        Me.CheckBox1.TabIndex = 200
        Me.CheckBox1.Text = "Pendientes"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(194, 26)
        Me.Label5.TabIndex = 199
        Me.Label5.Text = "ORDENES DE PRODUCCION"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbEstatus
        '
        Me.cmbEstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEstatus.FormattingEnabled = True
        Me.cmbEstatus.Location = New System.Drawing.Point(792, 12)
        Me.cmbEstatus.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbEstatus.Name = "cmbEstatus"
        Me.cmbEstatus.Size = New System.Drawing.Size(70, 21)
        Me.cmbEstatus.TabIndex = 198
        '
        'btnEmision
        '
        Me.btnEmision.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmision.Location = New System.Drawing.Point(212, 13)
        Me.btnEmision.Name = "btnEmision"
        Me.btnEmision.Size = New System.Drawing.Size(25, 20)
        Me.btnEmision.TabIndex = 112
        Me.btnEmision.Text = "•••"
        Me.btnEmision.UseVisualStyleBackColor = True
        '
        'txtDescripProduccion
        '
        Me.txtDescripProduccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescripProduccion.Location = New System.Drawing.Point(243, 13)
        Me.txtDescripProduccion.MaxLength = 250
        Me.txtDescripProduccion.Multiline = True
        Me.txtDescripProduccion.Name = "txtDescripProduccion"
        Me.txtDescripProduccion.Size = New System.Drawing.Size(545, 20)
        Me.txtDescripProduccion.TabIndex = 4
        '
        'txtCodigo
        '
        Me.txtCodigo.Enabled = False
        Me.txtCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigo.Location = New System.Drawing.Point(101, 13)
        Me.txtCodigo.MaxLength = 15
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(105, 20)
        Me.txtCodigo.TabIndex = 3
        Me.txtCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 26)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Producto :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MenuBarraRenglon
        '
        Me.MenuBarraRenglon.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraRenglon.AutoSize = False
        Me.MenuBarraRenglon.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraRenglon.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarraRenglon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarMovimiento, Me.btnEditarMovimiento, Me.btnEliminarMovimiento, Me.btnBuscarMovimiento, Me.ToolStripSeparator8, Me.btnPrimerMovimiento, Me.btnAnteriorMovimiento, Me.ToolStripSeparator9, Me.itemsrenglon, Me.lblitemsrenglon, Me.ToolStripSeparator10, Me.btnSiguienteMovimiento, Me.btnUltimoMovimiento, Me.ToolStripSeparator11, Me.btnImprimir, Me.ToolStripSeparator1, Me.ToolStripSeparator2, Me.btnSalir})
        Me.MenuBarraRenglon.Location = New System.Drawing.Point(1, 84)
        Me.MenuBarraRenglon.Name = "MenuBarraRenglon"
        Me.MenuBarraRenglon.Size = New System.Drawing.Size(1390, 39)
        Me.MenuBarraRenglon.TabIndex = 89
        Me.MenuBarraRenglon.Text = "ToolStrip1"
        '
        'btnAgregarMovimiento
        '
        Me.btnAgregarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarMovimiento.Image = CType(resources.GetObject("btnAgregarMovimiento.Image"), System.Drawing.Image)
        Me.btnAgregarMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarMovimiento.Name = "btnAgregarMovimiento"
        Me.btnAgregarMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnEditarMovimiento
        '
        Me.btnEditarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarMovimiento.Image = CType(resources.GetObject("btnEditarMovimiento.Image"), System.Drawing.Image)
        Me.btnEditarMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarMovimiento.Name = "btnEditarMovimiento"
        Me.btnEditarMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnEliminarMovimiento
        '
        Me.btnEliminarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminarMovimiento.Image = CType(resources.GetObject("btnEliminarMovimiento.Image"), System.Drawing.Image)
        Me.btnEliminarMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarMovimiento.Name = "btnEliminarMovimiento"
        Me.btnEliminarMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnBuscarMovimiento
        '
        Me.btnBuscarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnBuscarMovimiento.Image = CType(resources.GetObject("btnBuscarMovimiento.Image"), System.Drawing.Image)
        Me.btnBuscarMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnBuscarMovimiento.Name = "btnBuscarMovimiento"
        Me.btnBuscarMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 39)
        '
        'btnPrimerMovimiento
        '
        Me.btnPrimerMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPrimerMovimiento.Image = CType(resources.GetObject("btnPrimerMovimiento.Image"), System.Drawing.Image)
        Me.btnPrimerMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrimerMovimiento.Name = "btnPrimerMovimiento"
        Me.btnPrimerMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnAnteriorMovimiento
        '
        Me.btnAnteriorMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAnteriorMovimiento.Image = CType(resources.GetObject("btnAnteriorMovimiento.Image"), System.Drawing.Image)
        Me.btnAnteriorMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAnteriorMovimiento.Name = "btnAnteriorMovimiento"
        Me.btnAnteriorMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(6, 39)
        '
        'itemsrenglon
        '
        Me.itemsrenglon.AutoSize = False
        Me.itemsrenglon.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.itemsrenglon.Enabled = False
        Me.itemsrenglon.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.itemsrenglon.Name = "itemsrenglon"
        Me.itemsrenglon.Size = New System.Drawing.Size(40, 39)
        Me.itemsrenglon.Text = "0"
        Me.itemsrenglon.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblitemsrenglon
        '
        Me.lblitemsrenglon.AutoSize = False
        Me.lblitemsrenglon.Name = "lblitemsrenglon"
        Me.lblitemsrenglon.Size = New System.Drawing.Size(40, 22)
        Me.lblitemsrenglon.Text = "de {0}"
        Me.lblitemsrenglon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblitemsrenglon.ToolTipText = "Número de ítems"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(6, 39)
        '
        'btnSiguienteMovimiento
        '
        Me.btnSiguienteMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSiguienteMovimiento.Image = CType(resources.GetObject("btnSiguienteMovimiento.Image"), System.Drawing.Image)
        Me.btnSiguienteMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSiguienteMovimiento.Name = "btnSiguienteMovimiento"
        Me.btnSiguienteMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnUltimoMovimiento
        '
        Me.btnUltimoMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnUltimoMovimiento.Image = CType(resources.GetObject("btnUltimoMovimiento.Image"), System.Drawing.Image)
        Me.btnUltimoMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUltimoMovimiento.Name = "btnUltimoMovimiento"
        Me.btnUltimoMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 39)
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.C1SuperTooltip1.IsBalloon = True
        Me.C1SuperTooltip1.ShowAlways = True
        '
        'tbcSeguimiento
        '
        Me.tbcSeguimiento.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbcSeguimiento.Controls.Add(Me.tabPageFormulacion)
        Me.tbcSeguimiento.Controls.Add(Me.tabPageEntregas)
        Me.tbcSeguimiento.Controls.Add(Me.tabPagePorEntregar)
        Me.tbcSeguimiento.Controls.Add(Me.tabPageAnotaciones)
        Me.tbcSeguimiento.Location = New System.Drawing.Point(0, 236)
        Me.tbcSeguimiento.Name = "tbcSeguimiento"
        Me.tbcSeguimiento.SelectedIndex = 3
        Me.tbcSeguimiento.Size = New System.Drawing.Size(1144, 259)
        Me.tbcSeguimiento.TabIndex = 149
        Me.tbcSeguimiento.TabsSpacing = 5
        Me.tbcSeguimiento.TabStyle = C1.Win.C1Command.TabStyleEnum.Office2007
        Me.tbcSeguimiento.VisualStyle = C1.Win.C1Command.VisualStyle.Office2007Blue
        Me.tbcSeguimiento.VisualStyleBase = C1.Win.C1Command.VisualStyle.Office2007Blue
        '
        'tabPageFormulacion
        '
        Me.tabPageFormulacion.Controls.Add(Me.dgComponentes)
        Me.tabPageFormulacion.Location = New System.Drawing.Point(1, 24)
        Me.tabPageFormulacion.Name = "tabPageFormulacion"
        Me.tabPageFormulacion.Size = New System.Drawing.Size(1142, 234)
        Me.tabPageFormulacion.TabIndex = 0
        Me.tabPageFormulacion.Text = "Formulacion"
        '
        'tabPageEntregas
        '
        Me.tabPageEntregas.Controls.Add(Me.dgResidual)
        Me.tabPageEntregas.Location = New System.Drawing.Point(1, 24)
        Me.tabPageEntregas.Name = "tabPageEntregas"
        Me.tabPageEntregas.Size = New System.Drawing.Size(1142, 234)
        Me.tabPageEntregas.TabIndex = 1
        Me.tabPageEntregas.Text = "Entregas"
        '
        'dgResidual
        '
        Me.dgResidual.AllowUserToAddRows = False
        Me.dgResidual.AllowUserToDeleteRows = False
        Me.dgResidual.AllowUserToOrderColumns = True
        Me.dgResidual.AllowUserToResizeColumns = False
        Me.dgResidual.AllowUserToResizeRows = False
        Me.dgResidual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgResidual.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgResidual.Location = New System.Drawing.Point(0, 0)
        Me.dgResidual.Name = "dgResidual"
        Me.dgResidual.ReadOnly = True
        Me.dgResidual.Size = New System.Drawing.Size(1142, 234)
        Me.dgResidual.TabIndex = 83
        '
        'tabPagePorEntregar
        '
        Me.tabPagePorEntregar.Controls.Add(Me.DataGridView1)
        Me.tabPagePorEntregar.Location = New System.Drawing.Point(1, 24)
        Me.tabPagePorEntregar.Name = "tabPagePorEntregar"
        Me.tabPagePorEntregar.Size = New System.Drawing.Size(1142, 234)
        Me.tabPagePorEntregar.TabIndex = 2
        Me.tabPagePorEntregar.Text = "Por Entregar"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(1142, 234)
        Me.DataGridView1.TabIndex = 84
        '
        'tabPageAnotaciones
        '
        Me.tabPageAnotaciones.Controls.Add(Me.DataGridView2)
        Me.tabPageAnotaciones.Location = New System.Drawing.Point(1, 24)
        Me.tabPageAnotaciones.Name = "tabPageAnotaciones"
        Me.tabPageAnotaciones.Size = New System.Drawing.Size(1142, 234)
        Me.tabPageAnotaciones.TabIndex = 3
        Me.tabPageAnotaciones.Text = "Anotaciones"
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.AllowUserToOrderColumns = True
        Me.DataGridView2.AllowUserToResizeColumns = False
        Me.DataGridView2.AllowUserToResizeRows = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView2.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.Size = New System.Drawing.Size(1142, 234)
        Me.DataGridView2.TabIndex = 84
        '
        'dgRenglones
        '
        Me.dgRenglones.AllowUserToAddRows = False
        Me.dgRenglones.AllowUserToDeleteRows = False
        Me.dgRenglones.AllowUserToOrderColumns = True
        Me.dgRenglones.AllowUserToResizeColumns = False
        Me.dgRenglones.AllowUserToResizeRows = False
        Me.dgRenglones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgRenglones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgRenglones.Location = New System.Drawing.Point(1, 126)
        Me.dgRenglones.Name = "dgRenglones"
        Me.dgRenglones.ReadOnly = True
        Me.dgRenglones.Size = New System.Drawing.Size(1149, 104)
        Me.dgRenglones.TabIndex = 150
        '
        'btnImprimir
        '
        Me.btnImprimir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnImprimir.Image = Global.Datum.My.Resources.Resources.Imprimir
        Me.btnImprimir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(36, 36)
        Me.btnImprimir.Text = "ToolStripButton1"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 39)
        '
        'btnSalir
        '
        Me.btnSalir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSalir.Image = Global.Datum.My.Resources.Resources.Apagar
        Me.btnSalir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(36, 36)
        Me.btnSalir.Text = "ToolStripButton2"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 39)
        '
        'jsProArcSeguimientoProduccion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1144, 526)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgRenglones)
        Me.Controls.Add(Me.tbcSeguimiento)
        Me.Controls.Add(Me.MenuBarraRenglon)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpEncab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsProArcSeguimientoProduccion"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "Ordenes de Producción"
        CType(Me.dgComponentes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEncab.ResumeLayout(False)
        Me.grpEncab.PerformLayout()
        Me.MenuBarraRenglon.ResumeLayout(False)
        Me.MenuBarraRenglon.PerformLayout()
        CType(Me.tbcSeguimiento, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbcSeguimiento.ResumeLayout(False)
        Me.tabPageFormulacion.ResumeLayout(False)
        Me.tabPageEntregas.ResumeLayout(False)
        CType(Me.dgResidual, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPagePorEntregar.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPageAnotaciones.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgRenglones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents dgComponentes As System.Windows.Forms.DataGridView
    Friend WithEvents grpEncab As System.Windows.Forms.GroupBox
    Friend WithEvents txtDescripProduccion As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuBarraRenglon As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregarMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminarMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnBuscarMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnPrimerMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAnteriorMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents itemsrenglon As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents lblitemsrenglon As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSiguienteMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnUltimoMovimiento As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents btnEmision As System.Windows.Forms.Button
    Friend WithEvents cmbEstatus As System.Windows.Forms.ComboBox
    Friend WithEvents tscmbUltimos As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tbcSeguimiento As C1.Win.C1Command.C1DockingTab
    Friend WithEvents tabPageFormulacion As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents tabPageEntregas As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents dgResidual As System.Windows.Forms.DataGridView
    Friend WithEvents dgRenglones As System.Windows.Forms.DataGridView
    Friend WithEvents tabPagePorEntregar As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents tabPageAnotaciones As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents btnImprimir As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSalir As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
End Class
