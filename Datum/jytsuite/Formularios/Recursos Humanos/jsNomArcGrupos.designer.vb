<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsNomArcGrupos
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsNomArcGrupos))
        Me.dg = New System.Windows.Forms.DataGridView
        Me.MenuBarra = New System.Windows.Forms.ToolStrip
        Me.btnAgregar = New System.Windows.Forms.ToolStripButton
        Me.btnEditar = New System.Windows.Forms.ToolStripButton
        Me.btnEliminar = New System.Windows.Forms.ToolStripButton
        Me.btnSeleccionar = New System.Windows.Forms.ToolStripButton
        Me.btnImprimir = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.btnSalir = New System.Windows.Forms.ToolStripButton
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.MenuBarraS1 = New System.Windows.Forms.ToolStrip
        Me.btnAgregarS1 = New System.Windows.Forms.ToolStripButton
        Me.btnEditarS1 = New System.Windows.Forms.ToolStripButton
        Me.btnEliminarS1 = New System.Windows.Forms.ToolStripButton
        Me.btnSeleccionarS1 = New System.Windows.Forms.ToolStripButton
        Me.dg1 = New System.Windows.Forms.DataGridView
        Me.MenuBarraS2 = New System.Windows.Forms.ToolStrip
        Me.btnAgregarS2 = New System.Windows.Forms.ToolStripButton
        Me.btnEditarS2 = New System.Windows.Forms.ToolStripButton
        Me.btnEliminarS2 = New System.Windows.Forms.ToolStripButton
        Me.btnSeleccionarS2 = New System.Windows.Forms.ToolStripButton
        Me.dg2 = New System.Windows.Forms.DataGridView
        Me.dg3 = New System.Windows.Forms.DataGridView
        Me.dg4 = New System.Windows.Forms.DataGridView
        Me.dg5 = New System.Windows.Forms.DataGridView
        Me.MenuBarraS3 = New System.Windows.Forms.ToolStrip
        Me.btnAgregarS3 = New System.Windows.Forms.ToolStripButton
        Me.btnEditarS3 = New System.Windows.Forms.ToolStripButton
        Me.btnEliminarS3 = New System.Windows.Forms.ToolStripButton
        Me.btnSeleccionarS3 = New System.Windows.Forms.ToolStripButton
        Me.MenuBarraS4 = New System.Windows.Forms.ToolStrip
        Me.btnAgregarS4 = New System.Windows.Forms.ToolStripButton
        Me.btnEditarS4 = New System.Windows.Forms.ToolStripButton
        Me.btnEliminarS4 = New System.Windows.Forms.ToolStripButton
        Me.btnSeleccionarS4 = New System.Windows.Forms.ToolStripButton
        Me.MenuBarraS5 = New System.Windows.Forms.ToolStrip
        Me.btnAgregarS5 = New System.Windows.Forms.ToolStripButton
        Me.btnEditarS5 = New System.Windows.Forms.ToolStripButton
        Me.btnEliminarS5 = New System.Windows.Forms.ToolStripButton
        Me.btnSeleccionarS5 = New System.Windows.Forms.ToolStripButton
        Me.lblInfo = New System.Windows.Forms.Label
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuBarra.SuspendLayout()
        Me.MenuBarraS1.SuspendLayout()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuBarraS2.SuspendLayout()
        CType(Me.dg2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dg3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dg4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dg5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuBarraS3.SuspendLayout()
        Me.MenuBarraS4.SuspendLayout()
        Me.MenuBarraS5.SuspendLayout()
        Me.SuspendLayout()
        '
        'dg
        '
        Me.dg.AllowUserToAddRows = False
        Me.dg.AllowUserToDeleteRows = False
        Me.dg.AllowUserToOrderColumns = True
        Me.dg.AllowUserToResizeColumns = False
        Me.dg.AllowUserToResizeRows = False
        Me.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg.Location = New System.Drawing.Point(31, 0)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(643, 86)
        Me.dg.TabIndex = 29
        '
        'MenuBarra
        '
        Me.MenuBarra.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MenuBarra.AutoSize = False
        Me.MenuBarra.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarra.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuBarra.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregar, Me.btnEditar, Me.btnEliminar, Me.btnSeleccionar, Me.btnImprimir, Me.ToolStripSeparator5, Me.btnSalir})
        Me.MenuBarra.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuBarra.Location = New System.Drawing.Point(0, 0)
        Me.MenuBarra.Name = "MenuBarra"
        Me.MenuBarra.Size = New System.Drawing.Size(28, 592)
        Me.MenuBarra.TabIndex = 31
        '
        'btnAgregar
        '
        Me.btnAgregar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregar.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(26, 24)
        '
        'btnEditar
        '
        Me.btnEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditar.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(26, 24)
        '
        'btnEliminar
        '
        Me.btnEliminar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminar.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(26, 24)
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSeleccionar.Image = Global.Datum.My.Resources.Resources.Seleccionar
        Me.btnSeleccionar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(26, 24)
        '
        'btnImprimir
        '
        Me.btnImprimir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnImprimir.Image = Global.Datum.My.Resources.Resources.Imprimir
        Me.btnImprimir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(26, 24)
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(26, 6)
        '
        'btnSalir
        '
        Me.btnSalir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSalir.Image = Global.Datum.My.Resources.Resources.Apagar
        Me.btnSalir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(26, 24)
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        '
        'MenuBarraS1
        '
        Me.MenuBarraS1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraS1.AutoSize = False
        Me.MenuBarraS1.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraS1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuBarraS1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarS1, Me.btnEditarS1, Me.btnEliminarS1, Me.btnSeleccionarS1})
        Me.MenuBarraS1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuBarraS1.Location = New System.Drawing.Point(31, 89)
        Me.MenuBarraS1.Name = "MenuBarraS1"
        Me.MenuBarraS1.Size = New System.Drawing.Size(28, 503)
        Me.MenuBarraS1.TabIndex = 32
        '
        'btnAgregarS1
        '
        Me.btnAgregarS1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarS1.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregarS1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarS1.Name = "btnAgregarS1"
        Me.btnAgregarS1.Size = New System.Drawing.Size(26, 24)
        '
        'btnEditarS1
        '
        Me.btnEditarS1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarS1.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarS1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarS1.Name = "btnEditarS1"
        Me.btnEditarS1.Size = New System.Drawing.Size(26, 24)
        '
        'btnEliminarS1
        '
        Me.btnEliminarS1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminarS1.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminarS1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarS1.Name = "btnEliminarS1"
        Me.btnEliminarS1.Size = New System.Drawing.Size(26, 24)
        '
        'btnSeleccionarS1
        '
        Me.btnSeleccionarS1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSeleccionarS1.Image = Global.Datum.My.Resources.Resources.Seleccionar
        Me.btnSeleccionarS1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSeleccionarS1.Name = "btnSeleccionarS1"
        Me.btnSeleccionarS1.Size = New System.Drawing.Size(26, 24)
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToOrderColumns = True
        Me.dg1.AllowUserToResizeColumns = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg1.Location = New System.Drawing.Point(62, 89)
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.Size = New System.Drawing.Size(612, 86)
        Me.dg1.TabIndex = 33
        '
        'MenuBarraS2
        '
        Me.MenuBarraS2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraS2.AutoSize = False
        Me.MenuBarraS2.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraS2.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuBarraS2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarS2, Me.btnEditarS2, Me.btnEliminarS2, Me.btnSeleccionarS2})
        Me.MenuBarraS2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuBarraS2.Location = New System.Drawing.Point(62, 178)
        Me.MenuBarraS2.Name = "MenuBarraS2"
        Me.MenuBarraS2.Size = New System.Drawing.Size(28, 414)
        Me.MenuBarraS2.TabIndex = 34
        '
        'btnAgregarS2
        '
        Me.btnAgregarS2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarS2.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregarS2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarS2.Name = "btnAgregarS2"
        Me.btnAgregarS2.Size = New System.Drawing.Size(26, 24)
        '
        'btnEditarS2
        '
        Me.btnEditarS2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarS2.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarS2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarS2.Name = "btnEditarS2"
        Me.btnEditarS2.Size = New System.Drawing.Size(26, 24)
        '
        'btnEliminarS2
        '
        Me.btnEliminarS2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminarS2.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminarS2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarS2.Name = "btnEliminarS2"
        Me.btnEliminarS2.Size = New System.Drawing.Size(26, 24)
        '
        'btnSeleccionarS2
        '
        Me.btnSeleccionarS2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSeleccionarS2.Image = Global.Datum.My.Resources.Resources.Seleccionar
        Me.btnSeleccionarS2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSeleccionarS2.Name = "btnSeleccionarS2"
        Me.btnSeleccionarS2.Size = New System.Drawing.Size(26, 24)
        '
        'dg2
        '
        Me.dg2.AllowUserToAddRows = False
        Me.dg2.AllowUserToDeleteRows = False
        Me.dg2.AllowUserToOrderColumns = True
        Me.dg2.AllowUserToResizeColumns = False
        Me.dg2.AllowUserToResizeRows = False
        Me.dg2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg2.Location = New System.Drawing.Point(93, 178)
        Me.dg2.Name = "dg2"
        Me.dg2.ReadOnly = True
        Me.dg2.Size = New System.Drawing.Size(581, 86)
        Me.dg2.TabIndex = 35
        '
        'dg3
        '
        Me.dg3.AllowUserToAddRows = False
        Me.dg3.AllowUserToDeleteRows = False
        Me.dg3.AllowUserToOrderColumns = True
        Me.dg3.AllowUserToResizeColumns = False
        Me.dg3.AllowUserToResizeRows = False
        Me.dg3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg3.Location = New System.Drawing.Point(124, 267)
        Me.dg3.Name = "dg3"
        Me.dg3.ReadOnly = True
        Me.dg3.Size = New System.Drawing.Size(550, 86)
        Me.dg3.TabIndex = 36
        '
        'dg4
        '
        Me.dg4.AllowUserToAddRows = False
        Me.dg4.AllowUserToDeleteRows = False
        Me.dg4.AllowUserToOrderColumns = True
        Me.dg4.AllowUserToResizeColumns = False
        Me.dg4.AllowUserToResizeRows = False
        Me.dg4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg4.Location = New System.Drawing.Point(155, 356)
        Me.dg4.Name = "dg4"
        Me.dg4.ReadOnly = True
        Me.dg4.Size = New System.Drawing.Size(519, 86)
        Me.dg4.TabIndex = 37
        '
        'dg5
        '
        Me.dg5.AllowUserToAddRows = False
        Me.dg5.AllowUserToDeleteRows = False
        Me.dg5.AllowUserToOrderColumns = True
        Me.dg5.AllowUserToResizeColumns = False
        Me.dg5.AllowUserToResizeRows = False
        Me.dg5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg5.Location = New System.Drawing.Point(186, 445)
        Me.dg5.Name = "dg5"
        Me.dg5.ReadOnly = True
        Me.dg5.Size = New System.Drawing.Size(488, 114)
        Me.dg5.TabIndex = 38
        '
        'MenuBarraS3
        '
        Me.MenuBarraS3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraS3.AutoSize = False
        Me.MenuBarraS3.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraS3.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuBarraS3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarS3, Me.btnEditarS3, Me.btnEliminarS3, Me.btnSeleccionarS3})
        Me.MenuBarraS3.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuBarraS3.Location = New System.Drawing.Point(93, 267)
        Me.MenuBarraS3.Name = "MenuBarraS3"
        Me.MenuBarraS3.Size = New System.Drawing.Size(28, 325)
        Me.MenuBarraS3.TabIndex = 39
        '
        'btnAgregarS3
        '
        Me.btnAgregarS3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarS3.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregarS3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarS3.Name = "btnAgregarS3"
        Me.btnAgregarS3.Size = New System.Drawing.Size(26, 24)
        '
        'btnEditarS3
        '
        Me.btnEditarS3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarS3.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarS3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarS3.Name = "btnEditarS3"
        Me.btnEditarS3.Size = New System.Drawing.Size(26, 24)
        '
        'btnEliminarS3
        '
        Me.btnEliminarS3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminarS3.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminarS3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarS3.Name = "btnEliminarS3"
        Me.btnEliminarS3.Size = New System.Drawing.Size(26, 24)
        '
        'btnSeleccionarS3
        '
        Me.btnSeleccionarS3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSeleccionarS3.Image = Global.Datum.My.Resources.Resources.Seleccionar
        Me.btnSeleccionarS3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSeleccionarS3.Name = "btnSeleccionarS3"
        Me.btnSeleccionarS3.Size = New System.Drawing.Size(26, 24)
        '
        'MenuBarraS4
        '
        Me.MenuBarraS4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraS4.AutoSize = False
        Me.MenuBarraS4.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraS4.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuBarraS4.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarS4, Me.btnEditarS4, Me.btnEliminarS4, Me.btnSeleccionarS4})
        Me.MenuBarraS4.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuBarraS4.Location = New System.Drawing.Point(124, 356)
        Me.MenuBarraS4.Name = "MenuBarraS4"
        Me.MenuBarraS4.Size = New System.Drawing.Size(28, 236)
        Me.MenuBarraS4.TabIndex = 40
        '
        'btnAgregarS4
        '
        Me.btnAgregarS4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarS4.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregarS4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarS4.Name = "btnAgregarS4"
        Me.btnAgregarS4.Size = New System.Drawing.Size(26, 24)
        '
        'btnEditarS4
        '
        Me.btnEditarS4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarS4.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarS4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarS4.Name = "btnEditarS4"
        Me.btnEditarS4.Size = New System.Drawing.Size(26, 24)
        '
        'btnEliminarS4
        '
        Me.btnEliminarS4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminarS4.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminarS4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarS4.Name = "btnEliminarS4"
        Me.btnEliminarS4.Size = New System.Drawing.Size(26, 24)
        '
        'btnSeleccionarS4
        '
        Me.btnSeleccionarS4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSeleccionarS4.Image = Global.Datum.My.Resources.Resources.Seleccionar
        Me.btnSeleccionarS4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSeleccionarS4.Name = "btnSeleccionarS4"
        Me.btnSeleccionarS4.Size = New System.Drawing.Size(26, 24)
        '
        'MenuBarraS5
        '
        Me.MenuBarraS5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraS5.AutoSize = False
        Me.MenuBarraS5.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraS5.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuBarraS5.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarS5, Me.btnEditarS5, Me.btnEliminarS5, Me.btnSeleccionarS5})
        Me.MenuBarraS5.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.MenuBarraS5.Location = New System.Drawing.Point(155, 445)
        Me.MenuBarraS5.Name = "MenuBarraS5"
        Me.MenuBarraS5.Size = New System.Drawing.Size(28, 147)
        Me.MenuBarraS5.TabIndex = 41
        '
        'btnAgregarS5
        '
        Me.btnAgregarS5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarS5.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregarS5.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarS5.Name = "btnAgregarS5"
        Me.btnAgregarS5.Size = New System.Drawing.Size(26, 24)
        '
        'btnEditarS5
        '
        Me.btnEditarS5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarS5.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarS5.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarS5.Name = "btnEditarS5"
        Me.btnEditarS5.Size = New System.Drawing.Size(26, 24)
        '
        'btnEliminarS5
        '
        Me.btnEliminarS5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminarS5.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminarS5.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarS5.Name = "btnEliminarS5"
        Me.btnEliminarS5.Size = New System.Drawing.Size(26, 24)
        '
        'btnSeleccionarS5
        '
        Me.btnSeleccionarS5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSeleccionarS5.Image = Global.Datum.My.Resources.Resources.Seleccionar
        Me.btnSeleccionarS5.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSeleccionarS5.Name = "btnSeleccionarS5"
        Me.btnSeleccionarS5.Size = New System.Drawing.Size(26, 24)
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 562)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(676, 30)
        Me.lblInfo.TabIndex = 42
        '
        'jsNomArcGrupos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(676, 592)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.MenuBarraS5)
        Me.Controls.Add(Me.MenuBarraS4)
        Me.Controls.Add(Me.MenuBarraS3)
        Me.Controls.Add(Me.dg5)
        Me.Controls.Add(Me.dg4)
        Me.Controls.Add(Me.dg3)
        Me.Controls.Add(Me.dg2)
        Me.Controls.Add(Me.MenuBarraS2)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.MenuBarraS1)
        Me.Controls.Add(Me.MenuBarra)
        Me.Controls.Add(Me.dg)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsNomArcGrupos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Grupos y Subgrupos trabajadores"
        Me.Text = "Grupos y Subgrupos trabajadores"
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuBarra.ResumeLayout(False)
        Me.MenuBarra.PerformLayout()
        Me.MenuBarraS1.ResumeLayout(False)
        Me.MenuBarraS1.PerformLayout()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuBarraS2.ResumeLayout(False)
        Me.MenuBarraS2.PerformLayout()
        CType(Me.dg2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dg3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dg4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dg5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuBarraS3.ResumeLayout(False)
        Me.MenuBarraS3.PerformLayout()
        Me.MenuBarraS4.ResumeLayout(False)
        Me.MenuBarraS4.PerformLayout()
        Me.MenuBarraS5.ResumeLayout(False)
        Me.MenuBarraS5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents MenuBarra As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSeleccionar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnImprimir As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSalir As System.Windows.Forms.ToolStripButton
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents MenuBarraS1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregarS1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarS1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminarS1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnSeleccionarS1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuBarraS2 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregarS2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarS2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminarS2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSeleccionarS2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents dg2 As System.Windows.Forms.DataGridView
    Friend WithEvents dg3 As System.Windows.Forms.DataGridView
    Friend WithEvents dg4 As System.Windows.Forms.DataGridView
    Friend WithEvents dg5 As System.Windows.Forms.DataGridView
    Friend WithEvents MenuBarraS3 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregarS3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarS3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminarS3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSeleccionarS3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuBarraS4 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregarS4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarS4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminarS4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSeleccionarS4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuBarraS5 As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregarS5 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditarS5 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminarS5 As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSeleccionarS5 As System.Windows.Forms.ToolStripButton
    Friend WithEvents lblInfo As System.Windows.Forms.Label
End Class
