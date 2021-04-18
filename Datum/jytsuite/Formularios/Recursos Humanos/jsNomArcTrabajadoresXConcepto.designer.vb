<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsNomArcTrabajadoresXConcepto
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsNomArcTrabajadoresXConcepto))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.grpEncab = New System.Windows.Forms.GroupBox()
        Me.cmbNomina = New System.Windows.Forms.ComboBox()
        Me.txtNomina = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.txtTotalAño = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtTotalConcepto = New System.Windows.Forms.TextBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuBarra = New System.Windows.Forms.ToolStrip()
        Me.btnAgregar = New System.Windows.Forms.ToolStripButton()
        Me.btnEditar = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminar = New System.Windows.Forms.ToolStripButton()
        Me.btnBuscar = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnPrimero = New System.Windows.Forms.ToolStripButton()
        Me.btnAnterior = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.Items = New System.Windows.Forms.ToolStripTextBox()
        Me.lblItems = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSiguiente = New System.Windows.Forms.ToolStripButton()
        Me.btnUltimo = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnImprimir = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSalir = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnProcesarConceptos = New System.Windows.Forms.ToolStripButton()
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
        Me.btnReprocesarEmpleado = New System.Windows.Forms.ToolStripButton()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEncab.SuspendLayout()
        Me.MenuBarra.SuspendLayout()
        Me.MenuBarraRenglon.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 432)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(870, 28)
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
        Me.dg.Location = New System.Drawing.Point(-1, 207)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(871, 219)
        Me.dg.TabIndex = 82
        '
        'grpEncab
        '
        Me.grpEncab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncab.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpEncab.Controls.Add(Me.cmbNomina)
        Me.grpEncab.Controls.Add(Me.txtNomina)
        Me.grpEncab.Controls.Add(Me.Label6)
        Me.grpEncab.Controls.Add(Me.Label9)
        Me.grpEncab.Controls.Add(Me.txtTotal)
        Me.grpEncab.Controls.Add(Me.txtTotalAño)
        Me.grpEncab.Controls.Add(Me.Label5)
        Me.grpEncab.Controls.Add(Me.Label4)
        Me.grpEncab.Controls.Add(Me.txtTotalConcepto)
        Me.grpEncab.Controls.Add(Me.txtNombre)
        Me.grpEncab.Controls.Add(Me.txtCodigo)
        Me.grpEncab.Controls.Add(Me.Label3)
        Me.grpEncab.Controls.Add(Me.Label2)
        Me.grpEncab.Controls.Add(Me.Label1)
        Me.grpEncab.Location = New System.Drawing.Point(0, 42)
        Me.grpEncab.Name = "grpEncab"
        Me.grpEncab.Size = New System.Drawing.Size(870, 120)
        Me.grpEncab.TabIndex = 85
        Me.grpEncab.TabStop = False
        '
        'cmbNomina
        '
        Me.cmbNomina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNomina.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbNomina.FormattingEnabled = True
        Me.cmbNomina.Location = New System.Drawing.Point(113, 15)
        Me.cmbNomina.Name = "cmbNomina"
        Me.cmbNomina.Size = New System.Drawing.Size(334, 21)
        Me.cmbNomina.TabIndex = 31
        '
        'txtNomina
        '
        Me.txtNomina.Enabled = False
        Me.txtNomina.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNomina.Location = New System.Drawing.Point(592, 17)
        Me.txtNomina.MaxLength = 2
        Me.txtNomina.Name = "txtNomina"
        Me.txtNomina.Size = New System.Drawing.Size(156, 20)
        Me.txtNomina.TabIndex = 29
        Me.txtNomina.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(95, 19)
        Me.Label6.TabIndex = 28
        Me.Label6.Text = "Contrato :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(491, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 19)
        Me.Label9.TabIndex = 27
        Me.Label9.Text = "Tipo nómina:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTotal
        '
        Me.txtTotal.Enabled = False
        Me.txtTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotal.Location = New System.Drawing.Point(592, 80)
        Me.txtTotal.MaxLength = 19
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(156, 20)
        Me.txtTotal.TabIndex = 7
        Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotalAño
        '
        Me.txtTotalAño.Enabled = False
        Me.txtTotalAño.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalAño.Location = New System.Drawing.Point(592, 59)
        Me.txtTotalAño.MaxLength = 19
        Me.txtTotalAño.Name = "txtTotalAño"
        Me.txtTotalAño.Size = New System.Drawing.Size(156, 20)
        Me.txtTotalAño.TabIndex = 6
        Me.txtTotalAño.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(453, 81)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(133, 19)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Total acumulado :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(453, 59)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(133, 19)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Acumulado este año :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTotalConcepto
        '
        Me.txtTotalConcepto.Enabled = False
        Me.txtTotalConcepto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalConcepto.Location = New System.Drawing.Point(592, 38)
        Me.txtTotalConcepto.MaxLength = 19
        Me.txtTotalConcepto.Name = "txtTotalConcepto"
        Me.txtTotalConcepto.Size = New System.Drawing.Size(156, 20)
        Me.txtTotalConcepto.TabIndex = 5
        Me.txtTotalConcepto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNombre
        '
        Me.txtNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombre.Location = New System.Drawing.Point(113, 59)
        Me.txtNombre.MaxLength = 250
        Me.txtNombre.Multiline = True
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(334, 41)
        Me.txtNombre.TabIndex = 4
        '
        'txtCodigo
        '
        Me.txtCodigo.Enabled = False
        Me.txtCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigo.Location = New System.Drawing.Point(113, 38)
        Me.txtCodigo.MaxLength = 2
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(67, 20)
        Me.txtCodigo.TabIndex = 3
        Me.txtCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(465, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Total Concepto :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Descripción :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Concepto :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MenuBarra
        '
        Me.MenuBarra.AutoSize = False
        Me.MenuBarra.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarra.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregar, Me.btnEditar, Me.btnEliminar, Me.btnBuscar, Me.ToolStripSeparator1, Me.btnPrimero, Me.btnAnterior, Me.ToolStripSeparator2, Me.Items, Me.lblItems, Me.ToolStripSeparator3, Me.btnSiguiente, Me.btnUltimo, Me.ToolStripSeparator4, Me.btnImprimir, Me.ToolStripSeparator5, Me.btnSalir, Me.ToolStripSeparator6, Me.btnProcesarConceptos})
        Me.MenuBarra.Location = New System.Drawing.Point(0, 0)
        Me.MenuBarra.Name = "MenuBarra"
        Me.MenuBarra.Size = New System.Drawing.Size(870, 39)
        Me.MenuBarra.TabIndex = 88
        Me.MenuBarra.Text = "ToolStrip1"
        '
        'btnAgregar
        '
        Me.btnAgregar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregar.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(36, 36)
        '
        'btnEditar
        '
        Me.btnEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditar.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(36, 36)
        '
        'btnEliminar
        '
        Me.btnEliminar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminar.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(36, 36)
        '
        'btnBuscar
        '
        Me.btnBuscar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnBuscar.Image = Global.Datum.My.Resources.Resources.Buscar
        Me.btnBuscar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 39)
        '
        'btnPrimero
        '
        Me.btnPrimero.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPrimero.Image = Global.Datum.My.Resources.Resources.Primero
        Me.btnPrimero.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrimero.Name = "btnPrimero"
        Me.btnPrimero.Size = New System.Drawing.Size(36, 36)
        '
        'btnAnterior
        '
        Me.btnAnterior.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAnterior.Image = Global.Datum.My.Resources.Resources.Anterior
        Me.btnAnterior.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAnterior.Name = "btnAnterior"
        Me.btnAnterior.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 39)
        '
        'Items
        '
        Me.Items.Enabled = False
        Me.Items.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Items.Name = "Items"
        Me.Items.Size = New System.Drawing.Size(100, 39)
        Me.Items.Text = "0"
        Me.Items.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblItems
        '
        Me.lblItems.AutoSize = False
        Me.lblItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(80, 22)
        Me.lblItems.Text = "de {0}"
        Me.lblItems.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblItems.ToolTipText = "Número de ítems"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 39)
        '
        'btnSiguiente
        '
        Me.btnSiguiente.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSiguiente.Image = Global.Datum.My.Resources.Resources.Siguiente
        Me.btnSiguiente.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSiguiente.Name = "btnSiguiente"
        Me.btnSiguiente.Size = New System.Drawing.Size(36, 36)
        '
        'btnUltimo
        '
        Me.btnUltimo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnUltimo.Image = Global.Datum.My.Resources.Resources.Ultimo
        Me.btnUltimo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUltimo.Name = "btnUltimo"
        Me.btnUltimo.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 39)
        '
        'btnImprimir
        '
        Me.btnImprimir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnImprimir.Image = Global.Datum.My.Resources.Resources.Imprimir
        Me.btnImprimir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 39)
        '
        'btnSalir
        '
        Me.btnSalir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnSalir.Image = Global.Datum.My.Resources.Resources.Apagar
        Me.btnSalir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.AutoSize = False
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(48, 39)
        '
        'btnProcesarConceptos
        '
        Me.btnProcesarConceptos.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnProcesarConceptos.Image = Global.Datum.My.Resources.Resources.Probar
        Me.btnProcesarConceptos.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnProcesarConceptos.Name = "btnProcesarConceptos"
        Me.btnProcesarConceptos.Size = New System.Drawing.Size(36, 36)
        '
        'MenuBarraRenglon
        '
        Me.MenuBarraRenglon.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraRenglon.AutoSize = False
        Me.MenuBarraRenglon.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraRenglon.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarraRenglon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarMovimiento, Me.btnEditarMovimiento, Me.btnEliminarMovimiento, Me.btnBuscarMovimiento, Me.ToolStripSeparator8, Me.btnPrimerMovimiento, Me.btnAnteriorMovimiento, Me.ToolStripSeparator9, Me.itemsrenglon, Me.lblitemsrenglon, Me.ToolStripSeparator10, Me.btnSiguienteMovimiento, Me.btnUltimoMovimiento, Me.ToolStripSeparator11, Me.btnReprocesarEmpleado})
        Me.MenuBarraRenglon.Location = New System.Drawing.Point(0, 165)
        Me.MenuBarraRenglon.Name = "MenuBarraRenglon"
        Me.MenuBarraRenglon.Size = New System.Drawing.Size(1116, 39)
        Me.MenuBarraRenglon.TabIndex = 89
        Me.MenuBarraRenglon.Text = "ToolStrip1"
        '
        'btnAgregarMovimiento
        '
        Me.btnAgregarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarMovimiento.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregarMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarMovimiento.Name = "btnAgregarMovimiento"
        Me.btnAgregarMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnEditarMovimiento
        '
        Me.btnEditarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditarMovimiento.Image = Global.Datum.My.Resources.Resources.Modificar
        Me.btnEditarMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditarMovimiento.Name = "btnEditarMovimiento"
        Me.btnEditarMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnEliminarMovimiento
        '
        Me.btnEliminarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminarMovimiento.Image = Global.Datum.My.Resources.Resources.Menos
        Me.btnEliminarMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminarMovimiento.Name = "btnEliminarMovimiento"
        Me.btnEliminarMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnBuscarMovimiento
        '
        Me.btnBuscarMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnBuscarMovimiento.Image = Global.Datum.My.Resources.Resources.Buscar
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
        Me.btnPrimerMovimiento.Image = Global.Datum.My.Resources.Resources.Primero
        Me.btnPrimerMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrimerMovimiento.Name = "btnPrimerMovimiento"
        Me.btnPrimerMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnAnteriorMovimiento
        '
        Me.btnAnteriorMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAnteriorMovimiento.Image = Global.Datum.My.Resources.Resources.Anterior
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
        Me.itemsrenglon.Enabled = False
        Me.itemsrenglon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.itemsrenglon.Name = "itemsrenglon"
        Me.itemsrenglon.Size = New System.Drawing.Size(100, 39)
        Me.itemsrenglon.Text = "0"
        Me.itemsrenglon.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblitemsrenglon
        '
        Me.lblitemsrenglon.AutoSize = False
        Me.lblitemsrenglon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblitemsrenglon.Name = "lblitemsrenglon"
        Me.lblitemsrenglon.Size = New System.Drawing.Size(80, 22)
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
        Me.btnSiguienteMovimiento.Image = Global.Datum.My.Resources.Resources.Siguiente
        Me.btnSiguienteMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSiguienteMovimiento.Name = "btnSiguienteMovimiento"
        Me.btnSiguienteMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'btnUltimoMovimiento
        '
        Me.btnUltimoMovimiento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnUltimoMovimiento.Image = Global.Datum.My.Resources.Resources.Ultimo
        Me.btnUltimoMovimiento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnUltimoMovimiento.Name = "btnUltimoMovimiento"
        Me.btnUltimoMovimiento.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 39)
        '
        'btnReprocesarEmpleado
        '
        Me.btnReprocesarEmpleado.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnReprocesarEmpleado.Image = Global.Datum.My.Resources.Resources.Probar
        Me.btnReprocesarEmpleado.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnReprocesarEmpleado.Name = "btnReprocesarEmpleado"
        Me.btnReprocesarEmpleado.Size = New System.Drawing.Size(36, 36)
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.C1SuperTooltip1.IsBalloon = True
        Me.C1SuperTooltip1.ShowAlways = True
        '
        'jsNomArcTrabajadoresXConcepto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(870, 460)
        Me.ControlBox = False
        Me.Controls.Add(Me.MenuBarraRenglon)
        Me.Controls.Add(Me.MenuBarra)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpEncab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsNomArcTrabajadoresXConcepto"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "Trabajadores por concepto"
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEncab.ResumeLayout(False)
        Me.grpEncab.PerformLayout()
        Me.MenuBarra.ResumeLayout(False)
        Me.MenuBarra.PerformLayout()
        Me.MenuBarraRenglon.ResumeLayout(False)
        Me.MenuBarraRenglon.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents grpEncab As System.Windows.Forms.GroupBox
    Friend WithEvents txtTotalConcepto As System.Windows.Forms.TextBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MenuBarra As System.Windows.Forms.ToolStrip
    Friend WithEvents btnAgregar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEditar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnEliminar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnBuscar As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnPrimero As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAnterior As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Items As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents lblItems As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSiguiente As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnUltimo As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnImprimir As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnSalir As System.Windows.Forms.ToolStripButton
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
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtTotalAño As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnProcesarConceptos As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtNomina As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbNomina As System.Windows.Forms.ComboBox
    Friend WithEvents btnReprocesarEmpleado As System.Windows.Forms.ToolStripButton
End Class
