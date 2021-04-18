<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsBanArcCajas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsBanArcCajas))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.grpEncab = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnCodigoContable = New System.Windows.Forms.Button()
        Me.txtCodigoContable = New System.Windows.Forms.TextBox()
        Me.txtOT = New System.Windows.Forms.TextBox()
        Me.txtCT = New System.Windows.Forms.TextBox()
        Me.txtTA = New System.Windows.Forms.TextBox()
        Me.txtCH = New System.Windows.Forms.TextBox()
        Me.txtEF = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSaldo = New System.Windows.Forms.TextBox()
        Me.txtNombre = New System.Windows.Forms.TextBox()
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
        Me.btnRemesas = New System.Windows.Forms.ToolStripDropDownButton()
        Me.btnAdelantoEfectivo = New System.Windows.Forms.ToolStripButton()
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
        Me.lblInfo.Location = New System.Drawing.Point(0, 432)
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
        Me.dg.Location = New System.Drawing.Point(-1, 231)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.Size = New System.Drawing.Size(755, 195)
        Me.dg.TabIndex = 82
        '
        'grpEncab
        '
        Me.grpEncab.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpEncab.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpEncab.Controls.Add(Me.Label9)
        Me.grpEncab.Controls.Add(Me.btnCodigoContable)
        Me.grpEncab.Controls.Add(Me.txtCodigoContable)
        Me.grpEncab.Controls.Add(Me.txtOT)
        Me.grpEncab.Controls.Add(Me.txtCT)
        Me.grpEncab.Controls.Add(Me.txtTA)
        Me.grpEncab.Controls.Add(Me.txtCH)
        Me.grpEncab.Controls.Add(Me.txtEF)
        Me.grpEncab.Controls.Add(Me.Label8)
        Me.grpEncab.Controls.Add(Me.Label7)
        Me.grpEncab.Controls.Add(Me.Label6)
        Me.grpEncab.Controls.Add(Me.Label5)
        Me.grpEncab.Controls.Add(Me.Label4)
        Me.grpEncab.Controls.Add(Me.txtSaldo)
        Me.grpEncab.Controls.Add(Me.txtNombre)
        Me.grpEncab.Controls.Add(Me.txtCodigo)
        Me.grpEncab.Controls.Add(Me.Label3)
        Me.grpEncab.Controls.Add(Me.Label2)
        Me.grpEncab.Controls.Add(Me.Label1)
        Me.grpEncab.Location = New System.Drawing.Point(0, 42)
        Me.grpEncab.Name = "grpEncab"
        Me.grpEncab.Size = New System.Drawing.Size(754, 144)
        Me.grpEncab.TabIndex = 85
        Me.grpEncab.TabStop = False
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(12, 58)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(109, 19)
        Me.Label9.TabIndex = 105
        Me.Label9.Text = "Código Contable :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnCodigoContable
        '
        Me.btnCodigoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCodigoContable.Location = New System.Drawing.Point(422, 55)
        Me.btnCodigoContable.Name = "btnCodigoContable"
        Me.btnCodigoContable.Size = New System.Drawing.Size(25, 20)
        Me.btnCodigoContable.TabIndex = 104
        Me.btnCodigoContable.Text = "•••"
        Me.btnCodigoContable.UseVisualStyleBackColor = True
        '
        'txtCodigoContable
        '
        Me.txtCodigoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigoContable.Location = New System.Drawing.Point(127, 55)
        Me.txtCodigoContable.MaxLength = 50
        Me.txtCodigoContable.Name = "txtCodigoContable"
        Me.txtCodigoContable.Size = New System.Drawing.Size(289, 20)
        Me.txtCodigoContable.TabIndex = 16
        '
        'txtOT
        '
        Me.txtOT.Enabled = False
        Me.txtOT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOT.Location = New System.Drawing.Point(592, 118)
        Me.txtOT.MaxLength = 19
        Me.txtOT.Name = "txtOT"
        Me.txtOT.Size = New System.Drawing.Size(156, 20)
        Me.txtOT.TabIndex = 14
        Me.txtOT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCT
        '
        Me.txtCT.Enabled = False
        Me.txtCT.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCT.Location = New System.Drawing.Point(592, 97)
        Me.txtCT.MaxLength = 19
        Me.txtCT.Name = "txtCT"
        Me.txtCT.Size = New System.Drawing.Size(156, 20)
        Me.txtCT.TabIndex = 9
        Me.txtCT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTA
        '
        Me.txtTA.Enabled = False
        Me.txtTA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTA.Location = New System.Drawing.Point(592, 76)
        Me.txtTA.MaxLength = 19
        Me.txtTA.Name = "txtTA"
        Me.txtTA.Size = New System.Drawing.Size(156, 20)
        Me.txtTA.TabIndex = 8
        Me.txtTA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCH
        '
        Me.txtCH.Enabled = False
        Me.txtCH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCH.Location = New System.Drawing.Point(592, 55)
        Me.txtCH.MaxLength = 19
        Me.txtCH.Name = "txtCH"
        Me.txtCH.Size = New System.Drawing.Size(156, 20)
        Me.txtCH.TabIndex = 7
        Me.txtCH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtEF
        '
        Me.txtEF.Enabled = False
        Me.txtEF.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEF.Location = New System.Drawing.Point(592, 34)
        Me.txtEF.MaxLength = 19
        Me.txtEF.Name = "txtEF"
        Me.txtEF.Size = New System.Drawing.Size(156, 20)
        Me.txtEF.TabIndex = 6
        Me.txtEF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(493, 118)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(93, 19)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Otros:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(493, 97)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(93, 19)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Cheques Alim.:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(493, 77)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(93, 19)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Tarjetas:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(493, 56)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(93, 19)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Cheques:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(493, 34)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(93, 19)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Efectivo:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSaldo
        '
        Me.txtSaldo.Enabled = False
        Me.txtSaldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaldo.Location = New System.Drawing.Point(592, 13)
        Me.txtSaldo.MaxLength = 19
        Me.txtSaldo.Name = "txtSaldo"
        Me.txtSaldo.Size = New System.Drawing.Size(156, 20)
        Me.txtSaldo.TabIndex = 5
        Me.txtSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNombre
        '
        Me.txtNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombre.Location = New System.Drawing.Point(127, 34)
        Me.txtNombre.MaxLength = 50
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(320, 20)
        Me.txtNombre.TabIndex = 4
        '
        'txtCodigo
        '
        Me.txtCodigo.Enabled = False
        Me.txtCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodigo.Location = New System.Drawing.Point(127, 13)
        Me.txtCodigo.MaxLength = 2
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(41, 20)
        Me.txtCodigo.TabIndex = 3
        Me.txtCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(465, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "SALDO CAJA :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Nombre :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Código Caja :"
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
        Me.grpAceptarSalir.Location = New System.Drawing.Point(589, 429)
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
        Me.MenuBarra.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregar, Me.btnEditar, Me.btnEliminar, Me.btnBuscar, Me.ToolStripSeparator1, Me.btnPrimero, Me.btnAnterior, Me.ToolStripSeparator2, Me.Items, Me.lblItems, Me.ToolStripSeparator3, Me.btnSiguiente, Me.btnUltimo, Me.ToolStripSeparator4, Me.btnImprimir, Me.ToolStripSeparator5, Me.btnSalir, Me.ToolStripSeparator6, Me.btnRemesas, Me.btnAdelantoEfectivo})
        Me.MenuBarra.Location = New System.Drawing.Point(0, 0)
        Me.MenuBarra.Name = "MenuBarra"
        Me.MenuBarra.Size = New System.Drawing.Size(754, 39)
        Me.MenuBarra.TabIndex = 88
        Me.MenuBarra.Text = "ToolStrip1"
        '
        'btnAgregar
        '
        Me.btnAgregar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAgregar.Image = CType(resources.GetObject("btnAgregar.Image"), System.Drawing.Image)
        Me.btnAgregar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(36, 36)
        '
        'btnEditar
        '
        Me.btnEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEditar.Image = CType(resources.GetObject("btnEditar.Image"), System.Drawing.Image)
        Me.btnEditar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(36, 36)
        '
        'btnEliminar
        '
        Me.btnEliminar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnEliminar.Image = CType(resources.GetObject("btnEliminar.Image"), System.Drawing.Image)
        Me.btnEliminar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(36, 36)
        '
        'btnBuscar
        '
        Me.btnBuscar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnBuscar.Image = CType(resources.GetObject("btnBuscar.Image"), System.Drawing.Image)
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
        Me.btnPrimero.Image = CType(resources.GetObject("btnPrimero.Image"), System.Drawing.Image)
        Me.btnPrimero.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPrimero.Name = "btnPrimero"
        Me.btnPrimero.Size = New System.Drawing.Size(36, 36)
        '
        'btnAnterior
        '
        Me.btnAnterior.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAnterior.Image = CType(resources.GetObject("btnAnterior.Image"), System.Drawing.Image)
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
        Me.Items.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Items.Name = "Items"
        Me.Items.Size = New System.Drawing.Size(100, 39)
        Me.Items.Text = "0"
        Me.Items.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblItems
        '
        Me.lblItems.AutoSize = False
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
        Me.btnSiguiente.Image = CType(resources.GetObject("btnSiguiente.Image"), System.Drawing.Image)
        Me.btnSiguiente.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSiguiente.Name = "btnSiguiente"
        Me.btnSiguiente.Size = New System.Drawing.Size(36, 36)
        '
        'btnUltimo
        '
        Me.btnUltimo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnUltimo.Image = CType(resources.GetObject("btnUltimo.Image"), System.Drawing.Image)
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
        Me.btnImprimir.Image = CType(resources.GetObject("btnImprimir.Image"), System.Drawing.Image)
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
        Me.btnSalir.Image = CType(resources.GetObject("btnSalir.Image"), System.Drawing.Image)
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
        'btnRemesas
        '
        Me.btnRemesas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRemesas.Image = Global.Datum.My.Resources.Resources.RemesasCestaTicket
        Me.btnRemesas.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRemesas.Name = "btnRemesas"
        Me.btnRemesas.Size = New System.Drawing.Size(45, 36)
        '
        'btnAdelantoEfectivo
        '
        Me.btnAdelantoEfectivo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnAdelantoEfectivo.Image = CType(resources.GetObject("btnAdelantoEfectivo.Image"), System.Drawing.Image)
        Me.btnAdelantoEfectivo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnAdelantoEfectivo.Name = "btnAdelantoEfectivo"
        Me.btnAdelantoEfectivo.Size = New System.Drawing.Size(36, 36)
        '
        'MenuBarraRenglon
        '
        Me.MenuBarraRenglon.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuBarraRenglon.AutoSize = False
        Me.MenuBarraRenglon.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuBarraRenglon.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.MenuBarraRenglon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnAgregarMovimiento, Me.btnEditarMovimiento, Me.btnEliminarMovimiento, Me.btnBuscarMovimiento, Me.ToolStripSeparator8, Me.btnPrimerMovimiento, Me.btnAnteriorMovimiento, Me.ToolStripSeparator9, Me.itemsrenglon, Me.lblitemsrenglon, Me.ToolStripSeparator10, Me.btnSiguienteMovimiento, Me.btnUltimoMovimiento, Me.ToolStripSeparator11})
        Me.MenuBarraRenglon.Location = New System.Drawing.Point(0, 189)
        Me.MenuBarraRenglon.Name = "MenuBarraRenglon"
        Me.MenuBarraRenglon.Size = New System.Drawing.Size(754, 39)
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
        Me.itemsrenglon.Enabled = False
        Me.itemsrenglon.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.itemsrenglon.Name = "itemsrenglon"
        Me.itemsrenglon.Size = New System.Drawing.Size(100, 39)
        Me.itemsrenglon.Text = "0"
        Me.itemsrenglon.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblitemsrenglon
        '
        Me.lblitemsrenglon.AutoSize = False
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
        'jsBanArcCajas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(754, 460)
        Me.ControlBox = False
        Me.Controls.Add(Me.MenuBarraRenglon)
        Me.Controls.Add(Me.MenuBarra)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.dg)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpEncab)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsBanArcCajas"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "Cajas y movimientos"
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
    Friend WithEvents txtSaldo As System.Windows.Forms.TextBox
    Friend WithEvents txtNombre As System.Windows.Forms.TextBox
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
    Friend WithEvents txtTA As System.Windows.Forms.TextBox
    Friend WithEvents txtCH As System.Windows.Forms.TextBox
    Friend WithEvents txtEF As System.Windows.Forms.TextBox
    Friend WithEvents txtOT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCT As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnRemesas As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents btnAdelantoEfectivo As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtCodigoContable As System.Windows.Forms.TextBox
    Friend WithEvents btnCodigoContable As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
