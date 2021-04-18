<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsVenArcRutasVisita
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsVenArcRutasVisita))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.grpEncab = New System.Windows.Forms.GroupBox()
        Me.cmbDia = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtNombreAsesor = New System.Windows.Forms.TextBox()
        Me.txtAsesor = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtNombreZona = New System.Windows.Forms.TextBox()
        Me.btnZona = New System.Windows.Forms.Button()
        Me.txtCodigoZona = New System.Windows.Forms.TextBox()
        Me.btnAsesor = New System.Windows.Forms.Button()
        Me.cmbTipo = New System.Windows.Forms.ComboBox()
        Me.txtNombreRuta = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtComentario = New System.Windows.Forms.TextBox()
        Me.txtCodigoRuta = New System.Windows.Forms.TextBox()
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
        Me.MenuBarraRenglon = New System.Windows.Forms.ToolStrip()
        Me.btnAgregarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnEditarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.btnEliminarMovimiento = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnReordenar = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tslblPeso = New System.Windows.Forms.ToolStripLabel()
        Me.tslblPesoT = New System.Windows.Forms.ToolStripLabel()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.btnReprocesarRutas = New System.Windows.Forms.ToolStripButton()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEncab.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.MenuBarra.SuspendLayout()
        Me.MenuBarraRenglon.SuspendLayout()
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
        Me.dg.Location = New System.Drawing.Point(0, 213)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(755, 194)
        Me.dg.TabIndex = 82
        '
        'grpEncab
        '
        Me.grpEncab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncab.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpEncab.Controls.Add(Me.cmbDia)
        Me.grpEncab.Controls.Add(Me.Label13)
        Me.grpEncab.Controls.Add(Me.txtNombreAsesor)
        Me.grpEncab.Controls.Add(Me.txtAsesor)
        Me.grpEncab.Controls.Add(Me.Label11)
        Me.grpEncab.Controls.Add(Me.txtNombreZona)
        Me.grpEncab.Controls.Add(Me.btnZona)
        Me.grpEncab.Controls.Add(Me.txtCodigoZona)
        Me.grpEncab.Controls.Add(Me.btnAsesor)
        Me.grpEncab.Controls.Add(Me.cmbTipo)
        Me.grpEncab.Controls.Add(Me.txtNombreRuta)
        Me.grpEncab.Controls.Add(Me.Label5)
        Me.grpEncab.Controls.Add(Me.txtComentario)
        Me.grpEncab.Controls.Add(Me.txtCodigoRuta)
        Me.grpEncab.Controls.Add(Me.Label3)
        Me.grpEncab.Controls.Add(Me.Label2)
        Me.grpEncab.Controls.Add(Me.Label1)
        Me.grpEncab.Location = New System.Drawing.Point(0, 42)
        Me.grpEncab.Name = "grpEncab"
        Me.grpEncab.Size = New System.Drawing.Size(754, 126)
        Me.grpEncab.TabIndex = 85
        Me.grpEncab.TabStop = False
        '
        'cmbDia
        '
        Me.cmbDia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDia.FormattingEnabled = True
        Me.cmbDia.Location = New System.Drawing.Point(651, 51)
        Me.cmbDia.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbDia.Name = "cmbDia"
        Me.cmbDia.Size = New System.Drawing.Size(99, 21)
        Me.cmbDia.TabIndex = 213
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(12, 55)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(108, 19)
        Me.Label13.TabIndex = 212
        Me.Label13.Text = "Asesor comercial"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNombreAsesor
        '
        Me.txtNombreAsesor.Enabled = False
        Me.txtNombreAsesor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombreAsesor.Location = New System.Drawing.Point(228, 54)
        Me.txtNombreAsesor.MaxLength = 19
        Me.txtNombreAsesor.Name = "txtNombreAsesor"
        Me.txtNombreAsesor.Size = New System.Drawing.Size(366, 20)
        Me.txtNombreAsesor.TabIndex = 209
        '
        'txtAsesor
        '
        Me.txtAsesor.Enabled = False
        Me.txtAsesor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsesor.Location = New System.Drawing.Point(120, 54)
        Me.txtAsesor.MaxLength = 19
        Me.txtAsesor.Name = "txtAsesor"
        Me.txtAsesor.Size = New System.Drawing.Size(74, 20)
        Me.txtAsesor.TabIndex = 208
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(22, 74)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(95, 19)
        Me.Label11.TabIndex = 207
        Me.Label11.Text = "Comentario"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNombreZona
        '
        Me.txtNombreZona.Enabled = False
        Me.txtNombreZona.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombreZona.Location = New System.Drawing.Point(228, 33)
        Me.txtNombreZona.MaxLength = 19
        Me.txtNombreZona.Name = "txtNombreZona"
        Me.txtNombreZona.Size = New System.Drawing.Size(366, 20)
        Me.txtNombreZona.TabIndex = 206
        '
        'btnZona
        '
        Me.btnZona.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnZona.Location = New System.Drawing.Point(200, 33)
        Me.btnZona.Name = "btnZona"
        Me.btnZona.Size = New System.Drawing.Size(25, 20)
        Me.btnZona.TabIndex = 205
        Me.btnZona.Text = "•••"
        Me.btnZona.UseVisualStyleBackColor = True
        '
        'txtCodigoZona
        '
        Me.txtCodigoZona.Enabled = False
        Me.txtCodigoZona.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigoZona.Location = New System.Drawing.Point(120, 33)
        Me.txtCodigoZona.MaxLength = 19
        Me.txtCodigoZona.Name = "txtCodigoZona"
        Me.txtCodigoZona.Size = New System.Drawing.Size(74, 20)
        Me.txtCodigoZona.TabIndex = 204
        '
        'btnAsesor
        '
        Me.btnAsesor.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAsesor.Location = New System.Drawing.Point(200, 54)
        Me.btnAsesor.Name = "btnAsesor"
        Me.btnAsesor.Size = New System.Drawing.Size(25, 20)
        Me.btnAsesor.TabIndex = 201
        Me.btnAsesor.Text = "•••"
        Me.btnAsesor.UseVisualStyleBackColor = True
        '
        'cmbTipo
        '
        Me.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipo.FormattingEnabled = True
        Me.cmbTipo.Location = New System.Drawing.Point(651, 11)
        Me.cmbTipo.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbTipo.Name = "cmbTipo"
        Me.cmbTipo.Size = New System.Drawing.Size(99, 21)
        Me.cmbTipo.TabIndex = 198
        '
        'txtNombreRuta
        '
        Me.txtNombreRuta.Enabled = False
        Me.txtNombreRuta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombreRuta.Location = New System.Drawing.Point(200, 12)
        Me.txtNombreRuta.MaxLength = 19
        Me.txtNombreRuta.Name = "txtNombreRuta"
        Me.txtNombreRuta.Size = New System.Drawing.Size(394, 20)
        Me.txtNombreRuta.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(600, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 19)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Tipo"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtComentario
        '
        Me.txtComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtComentario.Location = New System.Drawing.Point(120, 75)
        Me.txtComentario.MaxLength = 50
        Me.txtComentario.Multiline = True
        Me.txtComentario.Name = "txtComentario"
        Me.txtComentario.Size = New System.Drawing.Size(474, 45)
        Me.txtComentario.TabIndex = 4
        '
        'txtCodigoRuta
        '
        Me.txtCodigoRuta.Enabled = False
        Me.txtCodigoRuta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigoRuta.Location = New System.Drawing.Point(120, 12)
        Me.txtCodigoRuta.MaxLength = 2
        Me.txtCodigoRuta.Name = "txtCodigoRuta"
        Me.txtCodigoRuta.Size = New System.Drawing.Size(74, 20)
        Me.txtCodigoRuta.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(600, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Día"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(22, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Zona"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 26)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Ruta No."
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
        'MenuBarra
        '
        Me.MenuBarra.AutoSize = False
        Me.MenuBarra.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarra.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregar, Me.btnEditar, Me.btnEliminar, Me.btnBuscar, Me.ToolStripSeparator1, Me.btnPrimero, Me.btnAnterior, Me.ToolStripSeparator2, Me.Items, Me.lblItems, Me.ToolStripSeparator3, Me.btnSiguiente, Me.btnUltimo, Me.ToolStripSeparator4, Me.btnImprimir, Me.ToolStripSeparator5, Me.btnSalir, Me.ToolStripSeparator6, Me.btnReprocesarRutas})
        Me.MenuBarra.Location = New System.Drawing.Point(0, 0)
        Me.MenuBarra.Name = "MenuBarra"
        Me.MenuBarra.Size = New System.Drawing.Size(754, 39)
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
        Me.Items.Enabled = False
        Me.Items.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Items.Name = "Items"
        Me.Items.Size = New System.Drawing.Size(50, 39)
        Me.Items.Text = "0"
        Me.Items.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblItems
        '
        Me.lblItems.AutoSize = False
        Me.lblItems.Name = "lblItems"
        Me.lblItems.Size = New System.Drawing.Size(50, 22)
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
        'MenuBarraRenglon
        '
        Me.MenuBarraRenglon.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraRenglon.AutoSize = False
        Me.MenuBarraRenglon.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraRenglon.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarraRenglon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarMovimiento, Me.btnEditarMovimiento, Me.btnEliminarMovimiento, Me.ToolStripSeparator11, Me.btnReordenar, Me.ToolStripSeparator7, Me.tslblPeso, Me.tslblPesoT})
        Me.MenuBarraRenglon.Location = New System.Drawing.Point(0, 171)
        Me.MenuBarraRenglon.Name = "MenuBarraRenglon"
        Me.MenuBarraRenglon.Size = New System.Drawing.Size(1000, 39)
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
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(6, 39)
        '
        'btnReordenar
        '
        Me.btnReordenar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnReordenar.Image = Global.Datum.My.Resources.Resources.Probar
        Me.btnReordenar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnReordenar.Name = "btnReordenar"
        Me.btnReordenar.Size = New System.Drawing.Size(36, 36)
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 39)
        '
        'tslblPeso
        '
        Me.tslblPeso.AutoSize = False
        Me.tslblPeso.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslblPeso.Name = "tslblPeso"
        Me.tslblPeso.Size = New System.Drawing.Size(120, 36)
        Me.tslblPeso.Text = "N° clientes en ruta :"
        Me.tslblPeso.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tslblPesoT
        '
        Me.tslblPesoT.AutoSize = False
        Me.tslblPesoT.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslblPesoT.Name = "tslblPesoT"
        Me.tslblPesoT.Size = New System.Drawing.Size(50, 36)
        Me.tslblPesoT.Text = "0"
        Me.tslblPesoT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        Me.C1SuperTooltip1.IsBalloon = True
        Me.C1SuperTooltip1.ShowAlways = True
        '
        'btnReprocesarRutas
        '
        Me.btnReprocesarRutas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnReprocesarRutas.Image = Global.Datum.My.Resources.Resources.Probar
        Me.btnReprocesarRutas.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnReprocesarRutas.Name = "btnReprocesarRutas"
        Me.btnReprocesarRutas.Size = New System.Drawing.Size(36, 36)
        '
        'jsVenArcRutasVisita
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(754, 438)
        Me.ControlBox = False
        Me.Controls.Add(Me.MenuBarraRenglon)
        Me.Controls.Add(Me.MenuBarra)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpEncab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsVenArcRutasVisita"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "Rutas Visita"
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEncab.ResumeLayout(False)
        Me.grpEncab.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.MenuBarra.ResumeLayout(False)
        Me.MenuBarra.PerformLayout()
        Me.MenuBarraRenglon.ResumeLayout(False)
        Me.MenuBarraRenglon.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents grpEncab As System.Windows.Forms.GroupBox
    Friend WithEvents txtComentario As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigoRuta As System.Windows.Forms.TextBox
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
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents txtNombreRuta As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnAsesor As System.Windows.Forms.Button
    Friend WithEvents cmbTipo As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtNombreZona As System.Windows.Forms.TextBox
    Friend WithEvents btnZona As System.Windows.Forms.Button
    Friend WithEvents txtCodigoZona As System.Windows.Forms.TextBox
    Friend WithEvents txtNombreAsesor As System.Windows.Forms.TextBox
    Friend WithEvents txtAsesor As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents tslblPeso As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslblPesoT As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmbDia As System.Windows.Forms.ComboBox
    Friend WithEvents btnReordenar As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnReprocesarRutas As System.Windows.Forms.ToolStripButton
End Class
