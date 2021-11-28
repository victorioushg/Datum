<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsVenArcFacturas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsVenArcFacturas))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.grpEncab = New System.Windows.Forms.GroupBox()
        Me.cmbAlmacenes = New Syncfusion.WinForms.ListView.SfComboBox()
        Me.cmbTransportes = New Syncfusion.WinForms.ListView.SfComboBox()
        Me.cmbAsesores = New Syncfusion.WinForms.ListView.SfComboBox()
        Me.cmbCliente = New Syncfusion.WinForms.ListView.SfComboBox()
        Me.cmbMonedas = New Syncfusion.WinForms.ListView.SfComboBox()
        Me.txtEmision = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.grpDisponibilidad = New System.Windows.Forms.GroupBox()
        Me.lblDisponibilidad = New System.Windows.Forms.Label()
        Me.dgDisponibilidad = New System.Windows.Forms.DataGridView()
        Me.txtImpresion = New System.Windows.Forms.TextBox()
        Me.lblControl = New System.Windows.Forms.Label()
        Me.txtControl = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtReferencia = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCondicionPago = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtVencimiento = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbTarifa = New System.Windows.Forms.ComboBox()
        Me.txtEstatus = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtComentario = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
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
        Me.btnDuplicar = New System.Windows.Forms.ToolStripButton()
        Me.btnRecalcular = New System.Windows.Forms.ToolStripButton()
        Me.MenuBarraRenglon = New System.Windows.Forms.ToolStrip()
        Me.btnAgregarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnAgregarServicio = New System.Windows.Forms.ToolStripButton()
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
        Me.btnPresupuesto = New System.Windows.Forms.ToolStripButton()
        Me.btnPrepedido = New System.Windows.Forms.ToolStripButton()
        Me.btnPedido = New System.Windows.Forms.ToolStripButton()
        Me.btnNotaEntrega = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tslblPeso = New System.Windows.Forms.ToolStripLabel()
        Me.tslblPesoT = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnCortar = New System.Windows.Forms.ToolStripButton()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtCargos = New System.Windows.Forms.TextBox()
        Me.MenuDescuentos = New System.Windows.Forms.ToolStrip()
        Me.btnAgregaDescuento = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminaDescuento = New System.Windows.Forms.ToolStripButton()
        Me.dgDescuentos = New System.Windows.Forms.DataGridView()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtDescuentos = New System.Windows.Forms.TextBox()
        Me.dgIVA = New System.Windows.Forms.DataGridView()
        Me.txtSubTotal = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtTotalIVA = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.cmbCC = New Syncfusion.WinForms.ListView.SfComboBox()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEncab.SuspendLayout()
        CType(Me.cmbAlmacenes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbTransportes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbAsesores, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbCliente, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbMonedas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDisponibilidad.SuspendLayout()
        CType(Me.dgDisponibilidad, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAceptarSalir.SuspendLayout()
        Me.MenuBarra.SuspendLayout()
        Me.MenuBarraRenglon.SuspendLayout()
        Me.grpTotales.SuspendLayout()
        Me.MenuDescuentos.SuspendLayout()
        CType(Me.dgDescuentos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgIVA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cmbCC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 548)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(1166, 28)
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
        Me.dg.Location = New System.Drawing.Point(-5, 286)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.RowHeadersWidth = 62
        Me.dg.Size = New System.Drawing.Size(1167, 158)
        Me.dg.TabIndex = 82
        '
        'grpEncab
        '
        Me.grpEncab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncab.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpEncab.Controls.Add(Me.cmbCC)
        Me.grpEncab.Controls.Add(Me.cmbAlmacenes)
        Me.grpEncab.Controls.Add(Me.cmbTransportes)
        Me.grpEncab.Controls.Add(Me.cmbAsesores)
        Me.grpEncab.Controls.Add(Me.cmbCliente)
        Me.grpEncab.Controls.Add(Me.cmbMonedas)
        Me.grpEncab.Controls.Add(Me.txtEmision)
        Me.grpEncab.Controls.Add(Me.grpDisponibilidad)
        Me.grpEncab.Controls.Add(Me.txtImpresion)
        Me.grpEncab.Controls.Add(Me.lblControl)
        Me.grpEncab.Controls.Add(Me.txtControl)
        Me.grpEncab.Controls.Add(Me.Label17)
        Me.grpEncab.Controls.Add(Me.Label16)
        Me.grpEncab.Controls.Add(Me.txtReferencia)
        Me.grpEncab.Controls.Add(Me.Label14)
        Me.grpEncab.Controls.Add(Me.Label4)
        Me.grpEncab.Controls.Add(Me.txtCondicionPago)
        Me.grpEncab.Controls.Add(Me.Label12)
        Me.grpEncab.Controls.Add(Me.txtVencimiento)
        Me.grpEncab.Controls.Add(Me.Label13)
        Me.grpEncab.Controls.Add(Me.Label11)
        Me.grpEncab.Controls.Add(Me.Label6)
        Me.grpEncab.Controls.Add(Me.cmbTarifa)
        Me.grpEncab.Controls.Add(Me.txtEstatus)
        Me.grpEncab.Controls.Add(Me.Label5)
        Me.grpEncab.Controls.Add(Me.txtComentario)
        Me.grpEncab.Controls.Add(Me.txtCodigo)
        Me.grpEncab.Controls.Add(Me.Label3)
        Me.grpEncab.Controls.Add(Me.Label2)
        Me.grpEncab.Controls.Add(Me.Label1)
        Me.grpEncab.Location = New System.Drawing.Point(0, 42)
        Me.grpEncab.Name = "grpEncab"
        Me.grpEncab.Size = New System.Drawing.Size(1179, 203)
        Me.grpEncab.TabIndex = 85
        Me.grpEncab.TabStop = False
        '
        'cmbAlmacenes
        '
        Me.cmbAlmacenes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbAlmacenes.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains
        Me.cmbAlmacenes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cmbAlmacenes.DisplayMember = "nombre"
        Me.cmbAlmacenes.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center
        Me.cmbAlmacenes.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAlmacenes.Location = New System.Drawing.Point(120, 148)
        Me.cmbAlmacenes.Name = "cmbAlmacenes"
        Me.cmbAlmacenes.Size = New System.Drawing.Size(329, 23)
        Me.cmbAlmacenes.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.cmbAlmacenes.Style.EditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbAlmacenes.Style.EditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAlmacenes.Style.ReadOnlyEditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbAlmacenes.Style.ReadOnlyEditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAlmacenes.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbAlmacenes.Style.TokenStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAlmacenes.TabIndex = 240
        Me.cmbAlmacenes.ValueMember = "Codigo"
        '
        'cmbTransportes
        '
        Me.cmbTransportes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbTransportes.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains
        Me.cmbTransportes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cmbTransportes.DisplayMember = "nombre"
        Me.cmbTransportes.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center
        Me.cmbTransportes.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbTransportes.Location = New System.Drawing.Point(120, 120)
        Me.cmbTransportes.Name = "cmbTransportes"
        Me.cmbTransportes.Size = New System.Drawing.Size(329, 23)
        Me.cmbTransportes.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.cmbTransportes.Style.EditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbTransportes.Style.EditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbTransportes.Style.ReadOnlyEditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbTransportes.Style.ReadOnlyEditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbTransportes.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbTransportes.Style.TokenStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbTransportes.TabIndex = 239
        Me.cmbTransportes.ValueMember = "Codigo"
        '
        'cmbAsesores
        '
        Me.cmbAsesores.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbAsesores.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains
        Me.cmbAsesores.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cmbAsesores.DisplayMember = "nombre"
        Me.cmbAsesores.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center
        Me.cmbAsesores.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAsesores.Location = New System.Drawing.Point(120, 94)
        Me.cmbAsesores.Name = "cmbAsesores"
        Me.cmbAsesores.Size = New System.Drawing.Size(329, 23)
        Me.cmbAsesores.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.cmbAsesores.Style.EditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbAsesores.Style.EditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAsesores.Style.ReadOnlyEditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbAsesores.Style.ReadOnlyEditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAsesores.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbAsesores.Style.TokenStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAsesores.TabIndex = 238
        Me.cmbAsesores.ValueMember = "Codigo"
        '
        'cmbCliente
        '
        Me.cmbCliente.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbCliente.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains
        Me.cmbCliente.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cmbCliente.DisplayMember = "nombre"
        Me.cmbCliente.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center
        Me.cmbCliente.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCliente.Location = New System.Drawing.Point(120, 33)
        Me.cmbCliente.Name = "cmbCliente"
        Me.cmbCliente.Size = New System.Drawing.Size(630, 23)
        Me.cmbCliente.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.cmbCliente.Style.EditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbCliente.Style.EditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCliente.Style.ReadOnlyEditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbCliente.Style.ReadOnlyEditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCliente.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbCliente.Style.TokenStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCliente.TabIndex = 237
        Me.cmbCliente.ValueMember = "codcli"
        '
        'cmbMonedas
        '
        Me.cmbMonedas.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbMonedas.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains
        Me.cmbMonedas.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cmbMonedas.DisplayMember = "nombre"
        Me.cmbMonedas.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center
        Me.cmbMonedas.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbMonedas.Location = New System.Drawing.Point(760, 11)
        Me.cmbMonedas.Name = "cmbMonedas"
        Me.cmbMonedas.Size = New System.Drawing.Size(264, 23)
        Me.cmbMonedas.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.cmbMonedas.Style.EditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbMonedas.Style.EditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbMonedas.Style.ReadOnlyEditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbMonedas.Style.ReadOnlyEditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbMonedas.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbMonedas.Style.TokenStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbMonedas.TabIndex = 236
        Me.cmbMonedas.ValueMember = "codcli"
        '
        'txtEmision
        '
        Me.txtEmision.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtEmision.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtEmision.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtEmision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmision.Location = New System.Drawing.Point(278, 12)
        Me.txtEmision.Name = "txtEmision"
        Me.txtEmision.Size = New System.Drawing.Size(114, 19)
        Me.txtEmision.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtEmision.TabIndex = 235
        '
        'grpDisponibilidad
        '
        Me.grpDisponibilidad.Controls.Add(Me.lblDisponibilidad)
        Me.grpDisponibilidad.Controls.Add(Me.dgDisponibilidad)
        Me.grpDisponibilidad.Location = New System.Drawing.Point(754, 33)
        Me.grpDisponibilidad.Name = "grpDisponibilidad"
        Me.grpDisponibilidad.Size = New System.Drawing.Size(389, 124)
        Me.grpDisponibilidad.TabIndex = 213
        Me.grpDisponibilidad.TabStop = False
        '
        'lblDisponibilidad
        '
        Me.lblDisponibilidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDisponibilidad.Location = New System.Drawing.Point(13, 96)
        Me.lblDisponibilidad.Name = "lblDisponibilidad"
        Me.lblDisponibilidad.Size = New System.Drawing.Size(370, 19)
        Me.lblDisponibilidad.TabIndex = 218
        Me.lblDisponibilidad.Text = "Disponibilidad menos esta Factura :  0.00"
        Me.lblDisponibilidad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dgDisponibilidad
        '
        Me.dgDisponibilidad.AllowUserToAddRows = False
        Me.dgDisponibilidad.AllowUserToDeleteRows = False
        Me.dgDisponibilidad.AllowUserToResizeColumns = False
        Me.dgDisponibilidad.AllowUserToResizeRows = False
        Me.dgDisponibilidad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgDisponibilidad.Location = New System.Drawing.Point(6, 9)
        Me.dgDisponibilidad.Name = "dgDisponibilidad"
        Me.dgDisponibilidad.ReadOnly = True
        Me.dgDisponibilidad.RowHeadersWidth = 62
        Me.dgDisponibilidad.Size = New System.Drawing.Size(377, 84)
        Me.dgDisponibilidad.TabIndex = 217
        '
        'txtImpresion
        '
        Me.txtImpresion.Enabled = False
        Me.txtImpresion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtImpresion.Location = New System.Drawing.Point(732, 12)
        Me.txtImpresion.MaxLength = 19
        Me.txtImpresion.Name = "txtImpresion"
        Me.txtImpresion.Size = New System.Drawing.Size(18, 20)
        Me.txtImpresion.TabIndex = 234
        Me.txtImpresion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblControl
        '
        Me.lblControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControl.Location = New System.Drawing.Point(397, 12)
        Me.lblControl.Name = "lblControl"
        Me.lblControl.Size = New System.Drawing.Size(66, 19)
        Me.lblControl.TabIndex = 233
        Me.lblControl.Text = "N° Control"
        Me.lblControl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtControl
        '
        Me.txtControl.Enabled = False
        Me.txtControl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtControl.Location = New System.Drawing.Point(469, 12)
        Me.txtControl.MaxLength = 19
        Me.txtControl.Name = "txtControl"
        Me.txtControl.Size = New System.Drawing.Size(136, 20)
        Me.txtControl.TabIndex = 232
        Me.txtControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(455, 129)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(108, 20)
        Me.Label17.TabIndex = 228
        Me.Label17.Text = "Código contable"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(452, 92)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(128, 20)
        Me.Label16.TabIndex = 227
        Me.Label16.Text = "Referencia"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtReferencia
        '
        Me.txtReferencia.Enabled = False
        Me.txtReferencia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReferencia.Location = New System.Drawing.Point(589, 93)
        Me.txtReferencia.MaxLength = 19
        Me.txtReferencia.Name = "txtReferencia"
        Me.txtReferencia.Size = New System.Drawing.Size(158, 20)
        Me.txtReferencia.TabIndex = 225
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(9, 148)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(108, 17)
        Me.Label14.TabIndex = 224
        Me.Label14.Text = "Almacén"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 120)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(108, 20)
        Me.Label4.TabIndex = 220
        Me.Label4.Text = "Transporte"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCondicionPago
        '
        Me.txtCondicionPago.Enabled = False
        Me.txtCondicionPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCondicionPago.Location = New System.Drawing.Point(217, 177)
        Me.txtCondicionPago.MaxLength = 19
        Me.txtCondicionPago.Name = "txtCondicionPago"
        Me.txtCondicionPago.Size = New System.Drawing.Size(377, 20)
        Me.txtCondicionPago.TabIndex = 216
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(6, 177)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(108, 19)
        Me.Label12.TabIndex = 215
        Me.Label12.Text = "Vencimiento"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtVencimiento
        '
        Me.txtVencimiento.Enabled = False
        Me.txtVencimiento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVencimiento.Location = New System.Drawing.Point(120, 177)
        Me.txtVencimiento.MaxLength = 19
        Me.txtVencimiento.Name = "txtVencimiento"
        Me.txtVencimiento.Size = New System.Drawing.Size(91, 20)
        Me.txtVencimiento.TabIndex = 214
        Me.txtVencimiento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(9, 94)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(108, 19)
        Me.Label13.TabIndex = 212
        Me.Label13.Text = "Asesor comercial"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(22, 58)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 27)
        Me.Label11.TabIndex = 207
        Me.Label11.Text = "Comentario"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(225, 11)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 19)
        Me.Label6.TabIndex = 199
        Me.Label6.Text = "Emisión"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbTarifa
        '
        Me.cmbTarifa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTarifa.FormattingEnabled = True
        Me.cmbTarifa.Location = New System.Drawing.Point(705, 124)
        Me.cmbTarifa.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbTarifa.Name = "cmbTarifa"
        Me.cmbTarifa.Size = New System.Drawing.Size(42, 21)
        Me.cmbTarifa.TabIndex = 198
        '
        'txtEstatus
        '
        Me.txtEstatus.Enabled = False
        Me.txtEstatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstatus.Location = New System.Drawing.Point(661, 12)
        Me.txtEstatus.MaxLength = 19
        Me.txtEstatus.Name = "txtEstatus"
        Me.txtEstatus.Size = New System.Drawing.Size(70, 20)
        Me.txtEstatus.TabIndex = 7
        Me.txtEstatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(600, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 19)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Estatus"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtComentario
        '
        Me.txtComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComentario.Location = New System.Drawing.Point(120, 58)
        Me.txtComentario.MaxLength = 50
        Me.txtComentario.Multiline = True
        Me.txtComentario.Name = "txtComentario"
        Me.txtComentario.Size = New System.Drawing.Size(630, 32)
        Me.txtComentario.TabIndex = 4
        '
        'txtCodigo
        '
        Me.txtCodigo.Enabled = False
        Me.txtCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigo.Location = New System.Drawing.Point(120, 12)
        Me.txtCodigo.MaxLength = 2
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(105, 20)
        Me.txtCodigo.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(647, 124)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Tarifa"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(22, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Cliente"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 26)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Factura No."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(1001, 545)
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
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(85, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(76, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOK.Image = CType(resources.GetObject("btnOK.Image"), System.Drawing.Image)
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 23)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'MenuBarra
        '
        Me.MenuBarra.AutoSize = False
        Me.MenuBarra.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarra.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregar, Me.btnEditar, Me.btnEliminar, Me.btnBuscar, Me.ToolStripSeparator1, Me.btnPrimero, Me.btnAnterior, Me.ToolStripSeparator2, Me.Items, Me.lblItems, Me.ToolStripSeparator3, Me.btnSiguiente, Me.btnUltimo, Me.ToolStripSeparator4, Me.btnImprimir, Me.ToolStripSeparator5, Me.btnSalir, Me.ToolStripSeparator6, Me.btnDuplicar, Me.btnRecalcular})
        Me.MenuBarra.Location = New System.Drawing.Point(0, 0)
        Me.MenuBarra.Name = "MenuBarra"
        Me.MenuBarra.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.MenuBarra.Size = New System.Drawing.Size(1166, 39)
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
        Me.Items.AutoSize = False
        Me.Items.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.Items.Enabled = False
        Me.Items.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Items.Name = "Items"
        Me.Items.Size = New System.Drawing.Size(70, 21)
        Me.Items.Text = "0"
        Me.Items.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblItems
        '
        Me.lblItems.AutoSize = False
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(70, 22)
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
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 39)
        '
        'btnDuplicar
        '
        Me.btnDuplicar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnDuplicar.Image = Global.Datum.My.Resources.Resources.Duplicar1
        Me.btnDuplicar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnDuplicar.Name = "btnDuplicar"
        Me.btnDuplicar.Size = New System.Drawing.Size(36, 36)
        '
        'btnRecalcular
        '
        Me.btnRecalcular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRecalcular.Image = Global.Datum.My.Resources.Resources.Probar
        Me.btnRecalcular.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRecalcular.Name = "btnRecalcular"
        Me.btnRecalcular.Size = New System.Drawing.Size(36, 36)
        '
        'MenuBarraRenglon
        '
        Me.MenuBarraRenglon.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraRenglon.AutoSize = False
        Me.MenuBarraRenglon.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraRenglon.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarraRenglon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarMovimiento, Me.btnAgregarServicio, Me.btnEditarMovimiento, Me.btnEliminarMovimiento, Me.btnBuscarMovimiento, Me.ToolStripSeparator8, Me.btnPrimerMovimiento, Me.btnAnteriorMovimiento, Me.ToolStripSeparator9, Me.itemsrenglon, Me.lblitemsrenglon, Me.ToolStripSeparator10, Me.btnSiguienteMovimiento, Me.btnUltimoMovimiento, Me.ToolStripSeparator11, Me.btnPresupuesto, Me.btnPrepedido, Me.btnPedido, Me.btnNotaEntrega, Me.ToolStripSeparator7, Me.tslblPeso, Me.tslblPesoT, Me.ToolStripLabel1, Me.ToolStripSeparator12, Me.btnCortar})
        Me.MenuBarraRenglon.Location = New System.Drawing.Point(0, 248)
        Me.MenuBarraRenglon.Name = "MenuBarraRenglon"
        Me.MenuBarraRenglon.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.MenuBarraRenglon.Size = New System.Drawing.Size(1412, 39)
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
        'btnAgregarServicio
        '
        Me.btnAgregarServicio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregarServicio.Image = Global.Datum.My.Resources.Resources.AgregarServicio
        Me.btnAgregarServicio.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregarServicio.Name = "btnAgregarServicio"
        Me.btnAgregarServicio.Size = New System.Drawing.Size(36, 36)
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
        Me.itemsrenglon.AutoSize = False
        Me.itemsrenglon.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.itemsrenglon.Enabled = False
        Me.itemsrenglon.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.itemsrenglon.Name = "itemsrenglon"
        Me.itemsrenglon.Size = New System.Drawing.Size(40, 21)
        Me.itemsrenglon.Text = "0"
        Me.itemsrenglon.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblitemsrenglon
        '
        Me.lblitemsrenglon.AutoSize = False
        Me.lblitemsrenglon.Name = "lblitemsrenglon"
        Me.lblitemsrenglon.Size = New System.Drawing.Size(60, 22)
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
        'btnPresupuesto
        '
        Me.btnPresupuesto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPresupuesto.Image = Global.Datum.My.Resources.Resources.Presupuestos
        Me.btnPresupuesto.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPresupuesto.Name = "btnPresupuesto"
        Me.btnPresupuesto.Size = New System.Drawing.Size(36, 36)
        '
        'btnPrepedido
        '
        Me.btnPrepedido.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPrepedido.Image = Global.Datum.My.Resources.Resources.PrePedidos
        Me.btnPrepedido.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrepedido.Name = "btnPrepedido"
        Me.btnPrepedido.Size = New System.Drawing.Size(36, 36)
        '
        'btnPedido
        '
        Me.btnPedido.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnPedido.Image = Global.Datum.My.Resources.Resources.Pedidos
        Me.btnPedido.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPedido.Name = "btnPedido"
        Me.btnPedido.Size = New System.Drawing.Size(36, 36)
        '
        'btnNotaEntrega
        '
        Me.btnNotaEntrega.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnNotaEntrega.Image = Global.Datum.My.Resources.Resources.NotasEntrega
        Me.btnNotaEntrega.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnNotaEntrega.Name = "btnNotaEntrega"
        Me.btnNotaEntrega.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 39)
        '
        'tslblPeso
        '
        Me.tslblPeso.AutoSize = False
        Me.tslblPeso.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslblPeso.Name = "tslblPeso"
        Me.tslblPeso.Size = New System.Drawing.Size(85, 36)
        Me.tslblPeso.Text = "Peso total :"
        Me.tslblPeso.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tslblPesoT
        '
        Me.tslblPesoT.AutoSize = False
        Me.tslblPesoT.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslblPesoT.Name = "tslblPesoT"
        Me.tslblPesoT.Size = New System.Drawing.Size(70, 36)
        Me.tslblPesoT.Text = "0.000"
        Me.tslblPesoT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(26, 36)
        Me.ToolStripLabel1.Text = "Kgr."
        Me.ToolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(6, 39)
        '
        'btnCortar
        '
        Me.btnCortar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCortar.Image = Global.Datum.My.Resources.Resources.Cortar
        Me.btnCortar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCortar.Name = "btnCortar"
        Me.btnCortar.Size = New System.Drawing.Size(36, 36)
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.C1SuperTooltip1.IsBalloon = True
        Me.C1SuperTooltip1.ShowAlways = True
        '
        'grpTotales
        '
        Me.grpTotales.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpTotales.Controls.Add(Me.Label7)
        Me.grpTotales.Controls.Add(Me.txtCargos)
        Me.grpTotales.Controls.Add(Me.MenuDescuentos)
        Me.grpTotales.Controls.Add(Me.dgDescuentos)
        Me.grpTotales.Controls.Add(Me.Label15)
        Me.grpTotales.Controls.Add(Me.txtDescuentos)
        Me.grpTotales.Controls.Add(Me.dgIVA)
        Me.grpTotales.Controls.Add(Me.txtSubTotal)
        Me.grpTotales.Controls.Add(Me.Label10)
        Me.grpTotales.Controls.Add(Me.txtTotalIVA)
        Me.grpTotales.Controls.Add(Me.Label9)
        Me.grpTotales.Controls.Add(Me.txtTotal)
        Me.grpTotales.Controls.Add(Me.lblTotal)
        Me.grpTotales.Location = New System.Drawing.Point(0, 440)
        Me.grpTotales.Name = "grpTotales"
        Me.grpTotales.Size = New System.Drawing.Size(1406, 105)
        Me.grpTotales.TabIndex = 94
        Me.grpTotales.TabStop = False
        Me.grpTotales.Text = " Totales "
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(430, 44)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(153, 34)
        Me.Label7.TabIndex = 219
        Me.Label7.Text = "Total mercancías sin descuento"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCargos
        '
        Me.txtCargos.Enabled = False
        Me.txtCargos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCargos.Location = New System.Drawing.Point(433, 81)
        Me.txtCargos.MaxLength = 19
        Me.txtCargos.Name = "txtCargos"
        Me.txtCargos.Size = New System.Drawing.Size(150, 20)
        Me.txtCargos.TabIndex = 218
        Me.txtCargos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'MenuDescuentos
        '
        Me.MenuDescuentos.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuDescuentos.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.MenuDescuentos.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuDescuentos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregaDescuento, Me.btnEliminaDescuento})
        Me.MenuDescuentos.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuDescuentos.Location = New System.Drawing.Point(170, 79)
        Me.MenuDescuentos.Name = "MenuDescuentos"
        Me.MenuDescuentos.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.MenuDescuentos.Size = New System.Drawing.Size(52, 27)
        Me.MenuDescuentos.TabIndex = 217
        Me.MenuDescuentos.Text = "ToolStrip1"
        '
        'btnAgregaDescuento
        '
        Me.btnAgregaDescuento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregaDescuento.Image = Global.Datum.My.Resources.Resources.Agregar
        Me.btnAgregaDescuento.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregaDescuento.Name = "btnAgregaDescuento"
        Me.btnAgregaDescuento.Size = New System.Drawing.Size(24, 24)
        '
        'btnEliminaDescuento
        '
        Me.btnEliminaDescuento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminaDescuento.Image = Global.Datum.My.Resources.Resources.Menos
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
        Me.dgDescuentos.Location = New System.Drawing.Point(170, 10)
        Me.dgDescuentos.Name = "dgDescuentos"
        Me.dgDescuentos.ReadOnly = True
        Me.dgDescuentos.RowHeadersWidth = 62
        Me.dgDescuentos.Size = New System.Drawing.Size(250, 68)
        Me.dgDescuentos.TabIndex = 216
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(88, 10)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(76, 19)
        Me.Label15.TabIndex = 215
        Me.Label15.Text = "Descuentos"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDescuentos
        '
        Me.txtDescuentos.Enabled = False
        Me.txtDescuentos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescuentos.Location = New System.Drawing.Point(270, 81)
        Me.txtDescuentos.MaxLength = 19
        Me.txtDescuentos.Name = "txtDescuentos"
        Me.txtDescuentos.Size = New System.Drawing.Size(150, 20)
        Me.txtDescuentos.TabIndex = 214
        Me.txtDescuentos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'dgIVA
        '
        Me.dgIVA.AllowUserToAddRows = False
        Me.dgIVA.AllowUserToDeleteRows = False
        Me.dgIVA.AllowUserToOrderColumns = True
        Me.dgIVA.AllowUserToResizeColumns = False
        Me.dgIVA.AllowUserToResizeRows = False
        Me.dgIVA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgIVA.Location = New System.Drawing.Point(620, 8)
        Me.dgIVA.Name = "dgIVA"
        Me.dgIVA.ReadOnly = True
        Me.dgIVA.RowHeadersWidth = 62
        Me.dgIVA.Size = New System.Drawing.Size(250, 68)
        Me.dgIVA.TabIndex = 213
        '
        'txtSubTotal
        '
        Me.txtSubTotal.Enabled = False
        Me.txtSubTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSubTotal.Location = New System.Drawing.Point(14, 79)
        Me.txtSubTotal.MaxLength = 19
        Me.txtSubTotal.Name = "txtSubTotal"
        Me.txtSubTotal.Size = New System.Drawing.Size(150, 20)
        Me.txtSubTotal.TabIndex = 205
        Me.txtSubTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(12, 59)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(99, 19)
        Me.Label10.TabIndex = 207
        Me.Label10.Text = "Neto"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTotalIVA
        '
        Me.txtTotalIVA.Enabled = False
        Me.txtTotalIVA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalIVA.Location = New System.Drawing.Point(719, 79)
        Me.txtTotalIVA.MaxLength = 19
        Me.txtTotalIVA.Name = "txtTotalIVA"
        Me.txtTotalIVA.Size = New System.Drawing.Size(150, 20)
        Me.txtTotalIVA.TabIndex = 204
        Me.txtTotalIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(565, 10)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 19)
        Me.Label9.TabIndex = 206
        Me.Label9.Text = "IVA"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTotal
        '
        Me.txtTotal.Enabled = False
        Me.txtTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotal.Location = New System.Drawing.Point(874, 79)
        Me.txtTotal.MaxLength = 19
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(150, 20)
        Me.txtTotal.TabIndex = 203
        Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTotal
        '
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(874, 59)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(150, 19)
        Me.lblTotal.TabIndex = 12
        Me.lblTotal.Text = "Total"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbCC
        '
        Me.cmbCC.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbCC.AutoCompleteSuggestMode = Syncfusion.WinForms.ListView.Enums.AutoCompleteSuggestMode.Contains
        Me.cmbCC.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cmbCC.DisplayMember = "nombre"
        Me.cmbCC.DropDownPosition = Syncfusion.WinForms.Core.Enums.PopupRelativeAlignment.Center
        Me.cmbCC.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCC.Location = New System.Drawing.Point(455, 148)
        Me.cmbCC.Name = "cmbCC"
        Me.cmbCC.Size = New System.Drawing.Size(295, 23)
        Me.cmbCC.Style.DropDownStyle.BorderColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.cmbCC.Style.EditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbCC.Style.EditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCC.Style.ReadOnlyEditorStyle.DisabledBackColor = System.Drawing.Color.Azure
        Me.cmbCC.Style.ReadOnlyEditorStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCC.Style.TokenStyle.CloseButtonBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.cmbCC.Style.TokenStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCC.TabIndex = 241
        Me.cmbCC.ValueMember = "codcli"
        '
        'jsVenArcFacturas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1166, 576)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpTotales)
        Me.Controls.Add(Me.MenuBarraRenglon)
        Me.Controls.Add(Me.MenuBarra)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpEncab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsVenArcFacturas"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "Facturas"
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEncab.ResumeLayout(False)
        Me.grpEncab.PerformLayout()
        CType(Me.cmbAlmacenes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbTransportes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbAsesores, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbCliente, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbMonedas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDisponibilidad.ResumeLayout(False)
        CType(Me.dgDisponibilidad, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.MenuBarra.ResumeLayout(False)
        Me.MenuBarra.PerformLayout()
        Me.MenuBarraRenglon.ResumeLayout(False)
        Me.MenuBarraRenglon.PerformLayout()
        Me.grpTotales.ResumeLayout(False)
        Me.grpTotales.PerformLayout()
        Me.MenuDescuentos.ResumeLayout(False)
        Me.MenuDescuentos.PerformLayout()
        CType(Me.dgDescuentos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgIVA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cmbCC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents grpEncab As System.Windows.Forms.GroupBox
    Friend WithEvents txtComentario As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
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
    Friend WithEvents txtEstatus As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbTarifa As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents tslblPeso As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslblPesoT As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDuplicar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAgregarServicio As System.Windows.Forms.ToolStripButton
    Friend WithEvents grpDisponibilidad As System.Windows.Forms.GroupBox
    Friend WithEvents lblDisponibilidad As System.Windows.Forms.Label
    Friend WithEvents dgDisponibilidad As System.Windows.Forms.DataGridView
    Friend WithEvents txtCondicionPago As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtVencimiento As System.Windows.Forms.TextBox
    Friend WithEvents btnPresupuesto As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnPrepedido As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnPedido As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtReferencia As System.Windows.Forms.TextBox
    Friend WithEvents tscmbUltimos As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents lblControl As System.Windows.Forms.Label
    Friend WithEvents txtControl As System.Windows.Forms.TextBox
    Friend WithEvents txtImpresion As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnCortar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnRecalcular As System.Windows.Forms.ToolStripButton
    Friend WithEvents grpTotales As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtCargos As TextBox
    Friend WithEvents MenuDescuentos As ToolStrip
    Friend WithEvents btnAgregaDescuento As ToolStripButton
    Friend WithEvents btnEliminaDescuento As ToolStripButton
    Friend WithEvents dgDescuentos As DataGridView
    Friend WithEvents Label15 As Label
    Friend WithEvents txtDescuentos As TextBox
    Friend WithEvents dgIVA As DataGridView
    Friend WithEvents txtSubTotal As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtTotalIVA As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtTotal As TextBox
    Friend WithEvents lblTotal As Label
    Friend WithEvents txtEmision As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents btnNotaEntrega As ToolStripButton
    Friend WithEvents cmbMonedas As Syncfusion.WinForms.ListView.SfComboBox
    Friend WithEvents cmbCliente As Syncfusion.WinForms.ListView.SfComboBox
    Friend WithEvents cmbAsesores As Syncfusion.WinForms.ListView.SfComboBox
    Friend WithEvents cmbAlmacenes As Syncfusion.WinForms.ListView.SfComboBox
    Friend WithEvents cmbTransportes As Syncfusion.WinForms.ListView.SfComboBox
    Friend WithEvents cmbCC As Syncfusion.WinForms.ListView.SfComboBox
End Class
