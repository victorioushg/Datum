<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsBanRepParametros
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsBanRepParametros))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.grpImprimirSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.grpConstantes = New System.Windows.Forms.GroupBox()
        Me.lblConsRemesas = New System.Windows.Forms.Label()
        Me.rdbXDepositar = New System.Windows.Forms.RadioButton()
        Me.rdbDepositadas = New System.Windows.Forms.RadioButton()
        Me.chkTickets = New System.Windows.Forms.CheckBox()
        Me.lblconsTickets = New System.Windows.Forms.Label()
        Me.lblConsResumen = New System.Windows.Forms.Label()
        Me.chkConsResumen = New System.Windows.Forms.CheckBox()
        Me.grpGrupos = New System.Windows.Forms.GroupBox()
        Me.grpOrden = New System.Windows.Forms.GroupBox()
        Me.cmbOrdenHasta = New System.Windows.Forms.ComboBox()
        Me.cmbOrdenDesde = New System.Windows.Forms.ComboBox()
        Me.btnOrdenDesde = New System.Windows.Forms.Button()
        Me.btnOrdenHasta = New System.Windows.Forms.Button()
        Me.cmbOrdenadoPor = New System.Windows.Forms.ComboBox()
        Me.txtOrdenHasta = New System.Windows.Forms.TextBox()
        Me.txtOrdenDesde = New System.Windows.Forms.TextBox()
        Me.lblOrdenDesde = New System.Windows.Forms.Label()
        Me.lblOrdenHasta = New System.Windows.Forms.Label()
        Me.grpCriterios = New System.Windows.Forms.GroupBox()
        Me.cmbOrigen = New System.Windows.Forms.ComboBox()
        Me.lblOrigenSeleccion = New System.Windows.Forms.Label()
        Me.btnOrigen = New System.Windows.Forms.Button()
        Me.lblOrigen = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDepositoHasta = New System.Windows.Forms.TextBox()
        Me.txtDepositoDesde = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblDocumento = New System.Windows.Forms.Label()
        Me.txtDocumentoHasta = New System.Windows.Forms.TextBox()
        Me.txtDocumentoDesde = New System.Windows.Forms.TextBox()
        Me.lblDocHasta = New System.Windows.Forms.Label()
        Me.lblDocDesde = New System.Windows.Forms.Label()
        Me.lblAño = New System.Windows.Forms.Label()
        Me.lblMes = New System.Windows.Forms.Label()
        Me.cmbAño = New System.Windows.Forms.ComboBox()
        Me.cmbMes = New System.Windows.Forms.ComboBox()
        Me.lblMesAño = New System.Windows.Forms.Label()
        Me.chkList = New System.Windows.Forms.CheckedListBox()
        Me.lblTipodocumento = New System.Windows.Forms.Label()
        Me.txtTipDoc = New System.Windows.Forms.TextBox()
        Me.btnPeriodoHasta = New System.Windows.Forms.Button()
        Me.btnPeriodoDesde = New System.Windows.Forms.Button()
        Me.txtPeriodoHasta = New System.Windows.Forms.TextBox()
        Me.txtPeriodoDesde = New System.Windows.Forms.TextBox()
        Me.lblPeriodoHasta = New System.Windows.Forms.Label()
        Me.lblPeriodoDesde = New System.Windows.Forms.Label()
        Me.lblPeriodo = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblNombreReporte = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lblCaja = New System.Windows.Forms.Label()
        Me.txtCajaHasta = New System.Windows.Forms.TextBox()
        Me.txtCajaDesde = New System.Windows.Forms.TextBox()
        Me.lblCajaHasta = New System.Windows.Forms.Label()
        Me.lblCajaDesde = New System.Windows.Forms.Label()
        Me.grpImprimirSalir.SuspendLayout()
        Me.grpConstantes.SuspendLayout()
        Me.grpOrden.SuspendLayout()
        Me.grpCriterios.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Imprimir.ico")
        Me.ImageList1.Images.SetKeyName(1, "Salir.ico")
        Me.ImageList1.Images.SetKeyName(2, "NotaLimpia.ico")
        '
        'grpImprimirSalir
        '
        Me.grpImprimirSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpImprimirSalir.ColumnCount = 3
        Me.grpImprimirSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.grpImprimirSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.grpImprimirSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.grpImprimirSalir.Controls.Add(Me.btnLimpiar, 0, 0)
        Me.grpImprimirSalir.Controls.Add(Me.btnSalir, 2, 0)
        Me.grpImprimirSalir.Controls.Add(Me.btnImprimir, 1, 0)
        Me.grpImprimirSalir.Location = New System.Drawing.Point(555, 434)
        Me.grpImprimirSalir.Name = "grpImprimirSalir"
        Me.grpImprimirSalir.RowCount = 1
        Me.grpImprimirSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.grpImprimirSalir.Size = New System.Drawing.Size(321, 46)
        Me.grpImprimirSalir.TabIndex = 77
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnLimpiar.Image = Global.Datum.My.Resources.Resources.reiniciar
        Me.btnLimpiar.Location = New System.Drawing.Point(3, 3)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(101, 40)
        Me.btnLimpiar.TabIndex = 2
        Me.btnLimpiar.Text = "Reiniciar"
        Me.btnLimpiar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'btnSalir
        '
        Me.btnSalir.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSalir.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnSalir.Image = Global.Datum.My.Resources.Resources.Apagar
        Me.btnSalir.Location = New System.Drawing.Point(217, 3)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(101, 40)
        Me.btnSalir.TabIndex = 1
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'btnImprimir
        '
        Me.btnImprimir.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnImprimir.Image = Global.Datum.My.Resources.Resources.Imprimir
        Me.btnImprimir.Location = New System.Drawing.Point(113, 3)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(95, 40)
        Me.btnImprimir.TabIndex = 0
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'grpConstantes
        '
        Me.grpConstantes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpConstantes.Controls.Add(Me.lblConsRemesas)
        Me.grpConstantes.Controls.Add(Me.rdbXDepositar)
        Me.grpConstantes.Controls.Add(Me.rdbDepositadas)
        Me.grpConstantes.Controls.Add(Me.chkTickets)
        Me.grpConstantes.Controls.Add(Me.lblconsTickets)
        Me.grpConstantes.Controls.Add(Me.lblConsResumen)
        Me.grpConstantes.Controls.Add(Me.chkConsResumen)
        Me.grpConstantes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpConstantes.Location = New System.Drawing.Point(6, 378)
        Me.grpConstantes.Name = "grpConstantes"
        Me.grpConstantes.Size = New System.Drawing.Size(870, 50)
        Me.grpConstantes.TabIndex = 3
        Me.grpConstantes.TabStop = False
        Me.grpConstantes.Text = "Constantes :"
        '
        'lblConsRemesas
        '
        Me.lblConsRemesas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsRemesas.Location = New System.Drawing.Point(436, 25)
        Me.lblConsRemesas.Name = "lblConsRemesas"
        Me.lblConsRemesas.Size = New System.Drawing.Size(116, 17)
        Me.lblConsRemesas.TabIndex = 134
        Me.lblConsRemesas.Text = "Remesas :"
        Me.lblConsRemesas.Visible = False
        '
        'rdbXDepositar
        '
        Me.rdbXDepositar.AutoSize = True
        Me.rdbXDepositar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdbXDepositar.Location = New System.Drawing.Point(728, 23)
        Me.rdbXDepositar.Name = "rdbXDepositar"
        Me.rdbXDepositar.Size = New System.Drawing.Size(87, 17)
        Me.rdbXDepositar.TabIndex = 133
        Me.rdbXDepositar.TabStop = True
        Me.rdbXDepositar.Text = "Por depositar"
        Me.rdbXDepositar.UseVisualStyleBackColor = True
        Me.rdbXDepositar.Visible = False
        '
        'rdbDepositadas
        '
        Me.rdbDepositadas.AutoSize = True
        Me.rdbDepositadas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdbDepositadas.Location = New System.Drawing.Point(615, 23)
        Me.rdbDepositadas.Name = "rdbDepositadas"
        Me.rdbDepositadas.Size = New System.Drawing.Size(84, 17)
        Me.rdbDepositadas.TabIndex = 132
        Me.rdbDepositadas.TabStop = True
        Me.rdbDepositadas.Text = "Depositadas"
        Me.rdbDepositadas.UseVisualStyleBackColor = True
        Me.rdbDepositadas.Visible = False
        '
        'chkTickets
        '
        Me.chkTickets.AutoSize = True
        Me.chkTickets.Location = New System.Drawing.Point(367, 23)
        Me.chkTickets.Name = "chkTickets"
        Me.chkTickets.Size = New System.Drawing.Size(15, 14)
        Me.chkTickets.TabIndex = 131
        Me.chkTickets.UseVisualStyleBackColor = True
        Me.chkTickets.Visible = False
        '
        'lblconsTickets
        '
        Me.lblconsTickets.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblconsTickets.Location = New System.Drawing.Point(245, 20)
        Me.lblconsTickets.Name = "lblconsTickets"
        Me.lblconsTickets.Size = New System.Drawing.Size(116, 17)
        Me.lblconsTickets.TabIndex = 130
        Me.lblconsTickets.Text = "Mostrar tickets :"
        Me.lblconsTickets.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblconsTickets.Visible = False
        '
        'lblConsResumen
        '
        Me.lblConsResumen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsResumen.Location = New System.Drawing.Point(22, 20)
        Me.lblConsResumen.Name = "lblConsResumen"
        Me.lblConsResumen.Size = New System.Drawing.Size(116, 17)
        Me.lblConsResumen.TabIndex = 129
        Me.lblConsResumen.Text = "Resumido :"
        Me.lblConsResumen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblConsResumen.Visible = False
        '
        'chkConsResumen
        '
        Me.chkConsResumen.AutoSize = True
        Me.chkConsResumen.Location = New System.Drawing.Point(201, 20)
        Me.chkConsResumen.Name = "chkConsResumen"
        Me.chkConsResumen.Size = New System.Drawing.Size(15, 14)
        Me.chkConsResumen.TabIndex = 0
        Me.chkConsResumen.UseVisualStyleBackColor = True
        Me.chkConsResumen.Visible = False
        '
        'grpGrupos
        '
        Me.grpGrupos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpGrupos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGrupos.Location = New System.Drawing.Point(3, 132)
        Me.grpGrupos.Name = "grpGrupos"
        Me.grpGrupos.Size = New System.Drawing.Size(870, 76)
        Me.grpGrupos.TabIndex = 1
        Me.grpGrupos.TabStop = False
        Me.grpGrupos.Text = "Agrupado por :"
        '
        'grpOrden
        '
        Me.grpOrden.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpOrden.Controls.Add(Me.cmbOrdenHasta)
        Me.grpOrden.Controls.Add(Me.cmbOrdenDesde)
        Me.grpOrden.Controls.Add(Me.btnOrdenDesde)
        Me.grpOrden.Controls.Add(Me.btnOrdenHasta)
        Me.grpOrden.Controls.Add(Me.cmbOrdenadoPor)
        Me.grpOrden.Controls.Add(Me.txtOrdenHasta)
        Me.grpOrden.Controls.Add(Me.txtOrdenDesde)
        Me.grpOrden.Controls.Add(Me.lblOrdenDesde)
        Me.grpOrden.Controls.Add(Me.lblOrdenHasta)
        Me.grpOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpOrden.Location = New System.Drawing.Point(3, 75)
        Me.grpOrden.Name = "grpOrden"
        Me.grpOrden.Size = New System.Drawing.Size(870, 54)
        Me.grpOrden.TabIndex = 0
        Me.grpOrden.TabStop = False
        Me.grpOrden.Text = " Ordenado por : "
        '
        'cmbOrdenHasta
        '
        Me.cmbOrdenHasta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrdenHasta.FormattingEnabled = True
        Me.cmbOrdenHasta.Location = New System.Drawing.Point(552, 17)
        Me.cmbOrdenHasta.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbOrdenHasta.Name = "cmbOrdenHasta"
        Me.cmbOrdenHasta.Size = New System.Drawing.Size(85, 21)
        Me.cmbOrdenHasta.TabIndex = 122
        '
        'cmbOrdenDesde
        '
        Me.cmbOrdenDesde.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrdenDesde.FormattingEnabled = True
        Me.cmbOrdenDesde.Location = New System.Drawing.Point(281, 18)
        Me.cmbOrdenDesde.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbOrdenDesde.Name = "cmbOrdenDesde"
        Me.cmbOrdenDesde.Size = New System.Drawing.Size(85, 21)
        Me.cmbOrdenDesde.TabIndex = 121
        '
        'btnOrdenDesde
        '
        Me.btnOrdenDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrdenDesde.Location = New System.Drawing.Point(514, 18)
        Me.btnOrdenDesde.Name = "btnOrdenDesde"
        Me.btnOrdenDesde.Size = New System.Drawing.Size(25, 20)
        Me.btnOrdenDesde.TabIndex = 113
        Me.btnOrdenDesde.Text = "•••"
        Me.btnOrdenDesde.UseVisualStyleBackColor = True
        '
        'btnOrdenHasta
        '
        Me.btnOrdenHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrdenHasta.Location = New System.Drawing.Point(784, 17)
        Me.btnOrdenHasta.Name = "btnOrdenHasta"
        Me.btnOrdenHasta.Size = New System.Drawing.Size(25, 20)
        Me.btnOrdenHasta.TabIndex = 112
        Me.btnOrdenHasta.Text = "•••"
        Me.btnOrdenHasta.UseVisualStyleBackColor = True
        '
        'cmbOrdenadoPor
        '
        Me.cmbOrdenadoPor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrdenadoPor.FormattingEnabled = True
        Me.cmbOrdenadoPor.Location = New System.Drawing.Point(21, 17)
        Me.cmbOrdenadoPor.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbOrdenadoPor.Name = "cmbOrdenadoPor"
        Me.cmbOrdenadoPor.Size = New System.Drawing.Size(216, 21)
        Me.cmbOrdenadoPor.TabIndex = 111
        '
        'txtOrdenHasta
        '
        Me.txtOrdenHasta.Location = New System.Drawing.Point(641, 17)
        Me.txtOrdenHasta.Name = "txtOrdenHasta"
        Me.txtOrdenHasta.Size = New System.Drawing.Size(138, 20)
        Me.txtOrdenHasta.TabIndex = 110
        '
        'txtOrdenDesde
        '
        Me.txtOrdenDesde.Location = New System.Drawing.Point(370, 17)
        Me.txtOrdenDesde.Name = "txtOrdenDesde"
        Me.txtOrdenDesde.Size = New System.Drawing.Size(138, 20)
        Me.txtOrdenDesde.TabIndex = 109
        '
        'lblOrdenDesde
        '
        Me.lblOrdenDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrdenDesde.Location = New System.Drawing.Point(309, 18)
        Me.lblOrdenDesde.Name = "lblOrdenDesde"
        Me.lblOrdenDesde.Size = New System.Drawing.Size(55, 17)
        Me.lblOrdenDesde.TabIndex = 108
        Me.lblOrdenDesde.Text = "Desde"
        Me.lblOrdenDesde.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblOrdenHasta
        '
        Me.lblOrdenHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrdenHasta.Location = New System.Drawing.Point(567, 18)
        Me.lblOrdenHasta.Name = "lblOrdenHasta"
        Me.lblOrdenHasta.Size = New System.Drawing.Size(45, 17)
        Me.lblOrdenHasta.TabIndex = 107
        Me.lblOrdenHasta.Text = "Hasta"
        Me.lblOrdenHasta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpCriterios
        '
        Me.grpCriterios.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpCriterios.Controls.Add(Me.lblCaja)
        Me.grpCriterios.Controls.Add(Me.txtCajaHasta)
        Me.grpCriterios.Controls.Add(Me.txtCajaDesde)
        Me.grpCriterios.Controls.Add(Me.lblCajaHasta)
        Me.grpCriterios.Controls.Add(Me.lblCajaDesde)
        Me.grpCriterios.Controls.Add(Me.cmbOrigen)
        Me.grpCriterios.Controls.Add(Me.lblOrigenSeleccion)
        Me.grpCriterios.Controls.Add(Me.btnOrigen)
        Me.grpCriterios.Controls.Add(Me.lblOrigen)
        Me.grpCriterios.Controls.Add(Me.Label1)
        Me.grpCriterios.Controls.Add(Me.txtDepositoHasta)
        Me.grpCriterios.Controls.Add(Me.txtDepositoDesde)
        Me.grpCriterios.Controls.Add(Me.Label2)
        Me.grpCriterios.Controls.Add(Me.Label3)
        Me.grpCriterios.Controls.Add(Me.lblDocumento)
        Me.grpCriterios.Controls.Add(Me.txtDocumentoHasta)
        Me.grpCriterios.Controls.Add(Me.txtDocumentoDesde)
        Me.grpCriterios.Controls.Add(Me.lblDocHasta)
        Me.grpCriterios.Controls.Add(Me.lblDocDesde)
        Me.grpCriterios.Controls.Add(Me.lblAño)
        Me.grpCriterios.Controls.Add(Me.lblMes)
        Me.grpCriterios.Controls.Add(Me.cmbAño)
        Me.grpCriterios.Controls.Add(Me.cmbMes)
        Me.grpCriterios.Controls.Add(Me.lblMesAño)
        Me.grpCriterios.Controls.Add(Me.chkList)
        Me.grpCriterios.Controls.Add(Me.lblTipodocumento)
        Me.grpCriterios.Controls.Add(Me.txtTipDoc)
        Me.grpCriterios.Controls.Add(Me.btnPeriodoHasta)
        Me.grpCriterios.Controls.Add(Me.btnPeriodoDesde)
        Me.grpCriterios.Controls.Add(Me.txtPeriodoHasta)
        Me.grpCriterios.Controls.Add(Me.txtPeriodoDesde)
        Me.grpCriterios.Controls.Add(Me.lblPeriodoHasta)
        Me.grpCriterios.Controls.Add(Me.lblPeriodoDesde)
        Me.grpCriterios.Controls.Add(Me.lblPeriodo)
        Me.grpCriterios.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCriterios.Location = New System.Drawing.Point(3, 211)
        Me.grpCriterios.Name = "grpCriterios"
        Me.grpCriterios.Size = New System.Drawing.Size(870, 169)
        Me.grpCriterios.TabIndex = 2
        Me.grpCriterios.TabStop = False
        Me.grpCriterios.Text = " Criterios :"
        '
        'cmbOrigen
        '
        Me.cmbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrigen.FormattingEnabled = True
        Me.cmbOrigen.Location = New System.Drawing.Point(717, 58)
        Me.cmbOrigen.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbOrigen.Name = "cmbOrigen"
        Me.cmbOrigen.Size = New System.Drawing.Size(72, 21)
        Me.cmbOrigen.TabIndex = 173
        '
        'lblOrigenSeleccion
        '
        Me.lblOrigenSeleccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrigenSeleccion.Location = New System.Drawing.Point(590, 82)
        Me.lblOrigenSeleccion.Name = "lblOrigenSeleccion"
        Me.lblOrigenSeleccion.Size = New System.Drawing.Size(274, 66)
        Me.lblOrigenSeleccion.TabIndex = 172
        Me.lblOrigenSeleccion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOrigen
        '
        Me.btnOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrigen.Location = New System.Drawing.Point(688, 58)
        Me.btnOrigen.Name = "btnOrigen"
        Me.btnOrigen.Size = New System.Drawing.Size(25, 21)
        Me.btnOrigen.TabIndex = 171
        Me.btnOrigen.Text = "•••"
        Me.btnOrigen.UseVisualStyleBackColor = True
        '
        'lblOrigen
        '
        Me.lblOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrigen.Location = New System.Drawing.Point(587, 58)
        Me.lblOrigen.Name = "lblOrigen"
        Me.lblOrigen.Size = New System.Drawing.Size(76, 21)
        Me.lblOrigen.TabIndex = 135
        Me.lblOrigen.Text = "Origen :"
        Me.lblOrigen.Visible = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(25, 104)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 18)
        Me.Label1.TabIndex = 133
        Me.Label1.Text = "N° Depósito:"
        Me.Label1.Visible = False
        '
        'txtDepositoHasta
        '
        Me.txtDepositoHasta.Enabled = False
        Me.txtDepositoHasta.Location = New System.Drawing.Point(474, 100)
        Me.txtDepositoHasta.Name = "txtDepositoHasta"
        Me.txtDepositoHasta.Size = New System.Drawing.Size(107, 20)
        Me.txtDepositoHasta.TabIndex = 132
        Me.txtDepositoHasta.Visible = False
        '
        'txtDepositoDesde
        '
        Me.txtDepositoDesde.Enabled = False
        Me.txtDepositoDesde.Location = New System.Drawing.Point(233, 100)
        Me.txtDepositoDesde.Name = "txtDepositoDesde"
        Me.txtDepositoDesde.Size = New System.Drawing.Size(107, 20)
        Me.txtDepositoDesde.TabIndex = 131
        Me.txtDepositoDesde.Visible = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(423, 104)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 17)
        Me.Label2.TabIndex = 130
        Me.Label2.Text = "Hasta"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(167, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 16)
        Me.Label3.TabIndex = 129
        Me.Label3.Text = "Desde"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDocumento
        '
        Me.lblDocumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocumento.Location = New System.Drawing.Point(25, 81)
        Me.lblDocumento.Name = "lblDocumento"
        Me.lblDocumento.Size = New System.Drawing.Size(136, 18)
        Me.lblDocumento.TabIndex = 128
        Me.lblDocumento.Text = "Documento :"
        Me.lblDocumento.Visible = False
        '
        'txtDocumentoHasta
        '
        Me.txtDocumentoHasta.Enabled = False
        Me.txtDocumentoHasta.Location = New System.Drawing.Point(474, 79)
        Me.txtDocumentoHasta.Name = "txtDocumentoHasta"
        Me.txtDocumentoHasta.Size = New System.Drawing.Size(107, 20)
        Me.txtDocumentoHasta.TabIndex = 127
        Me.txtDocumentoHasta.Visible = False
        '
        'txtDocumentoDesde
        '
        Me.txtDocumentoDesde.Enabled = False
        Me.txtDocumentoDesde.Location = New System.Drawing.Point(233, 79)
        Me.txtDocumentoDesde.Name = "txtDocumentoDesde"
        Me.txtDocumentoDesde.Size = New System.Drawing.Size(107, 20)
        Me.txtDocumentoDesde.TabIndex = 126
        Me.txtDocumentoDesde.Visible = False
        '
        'lblDocHasta
        '
        Me.lblDocHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocHasta.Location = New System.Drawing.Point(423, 82)
        Me.lblDocHasta.Name = "lblDocHasta"
        Me.lblDocHasta.Size = New System.Drawing.Size(47, 17)
        Me.lblDocHasta.TabIndex = 125
        Me.lblDocHasta.Text = "Hasta"
        Me.lblDocHasta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDocDesde
        '
        Me.lblDocDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocDesde.Location = New System.Drawing.Point(167, 81)
        Me.lblDocDesde.Name = "lblDocDesde"
        Me.lblDocDesde.Size = New System.Drawing.Size(60, 16)
        Me.lblDocDesde.TabIndex = 124
        Me.lblDocDesde.Text = "Desde"
        Me.lblDocDesde.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblAño
        '
        Me.lblAño.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAño.Location = New System.Drawing.Point(417, 62)
        Me.lblAño.Name = "lblAño"
        Me.lblAño.Size = New System.Drawing.Size(53, 16)
        Me.lblAño.TabIndex = 123
        Me.lblAño.Text = "Año"
        Me.lblAño.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMes
        '
        Me.lblMes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMes.Location = New System.Drawing.Point(167, 59)
        Me.lblMes.Name = "lblMes"
        Me.lblMes.Size = New System.Drawing.Size(60, 16)
        Me.lblMes.TabIndex = 122
        Me.lblMes.Text = "Mes"
        Me.lblMes.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbAño
        '
        Me.cmbAño.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAño.FormattingEnabled = True
        Me.cmbAño.Location = New System.Drawing.Point(474, 57)
        Me.cmbAño.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbAño.Name = "cmbAño"
        Me.cmbAño.Size = New System.Drawing.Size(107, 21)
        Me.cmbAño.TabIndex = 121
        '
        'cmbMes
        '
        Me.cmbMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMes.FormattingEnabled = True
        Me.cmbMes.Location = New System.Drawing.Point(233, 57)
        Me.cmbMes.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbMes.Name = "cmbMes"
        Me.cmbMes.Size = New System.Drawing.Size(107, 21)
        Me.cmbMes.TabIndex = 120
        '
        'lblMesAño
        '
        Me.lblMesAño.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMesAño.Location = New System.Drawing.Point(25, 61)
        Me.lblMesAño.Name = "lblMesAño"
        Me.lblMesAño.Size = New System.Drawing.Size(136, 20)
        Me.lblMesAño.TabIndex = 119
        Me.lblMesAño.Text = "Mes y Año"
        Me.lblMesAño.Visible = False
        '
        'chkList
        '
        Me.chkList.ColumnWidth = 60
        Me.chkList.FormattingEnabled = True
        Me.chkList.Location = New System.Drawing.Point(233, 36)
        Me.chkList.MultiColumn = True
        Me.chkList.Name = "chkList"
        Me.chkList.Size = New System.Drawing.Size(348, 19)
        Me.chkList.TabIndex = 118
        '
        'lblTipodocumento
        '
        Me.lblTipodocumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTipodocumento.Location = New System.Drawing.Point(25, 38)
        Me.lblTipodocumento.Name = "lblTipodocumento"
        Me.lblTipodocumento.Size = New System.Drawing.Size(136, 23)
        Me.lblTipodocumento.TabIndex = 117
        Me.lblTipodocumento.Text = "Tipos de documento :"
        Me.lblTipodocumento.Visible = False
        '
        'txtTipDoc
        '
        Me.txtTipDoc.Enabled = False
        Me.txtTipDoc.Location = New System.Drawing.Point(588, 35)
        Me.txtTipDoc.Name = "txtTipDoc"
        Me.txtTipDoc.Size = New System.Drawing.Size(191, 20)
        Me.txtTipDoc.TabIndex = 116
        Me.txtTipDoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtTipDoc.Visible = False
        '
        'btnPeriodoHasta
        '
        Me.btnPeriodoHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPeriodoHasta.Location = New System.Drawing.Point(587, 14)
        Me.btnPeriodoHasta.Name = "btnPeriodoHasta"
        Me.btnPeriodoHasta.Size = New System.Drawing.Size(25, 20)
        Me.btnPeriodoHasta.TabIndex = 115
        Me.btnPeriodoHasta.Text = "•••"
        Me.btnPeriodoHasta.UseVisualStyleBackColor = True
        Me.btnPeriodoHasta.Visible = False
        '
        'btnPeriodoDesde
        '
        Me.btnPeriodoDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPeriodoDesde.Location = New System.Drawing.Point(341, 13)
        Me.btnPeriodoDesde.Name = "btnPeriodoDesde"
        Me.btnPeriodoDesde.Size = New System.Drawing.Size(25, 20)
        Me.btnPeriodoDesde.TabIndex = 114
        Me.btnPeriodoDesde.Text = "•••"
        Me.btnPeriodoDesde.UseVisualStyleBackColor = True
        Me.btnPeriodoDesde.Visible = False
        '
        'txtPeriodoHasta
        '
        Me.txtPeriodoHasta.Enabled = False
        Me.txtPeriodoHasta.Location = New System.Drawing.Point(474, 15)
        Me.txtPeriodoHasta.Name = "txtPeriodoHasta"
        Me.txtPeriodoHasta.Size = New System.Drawing.Size(107, 20)
        Me.txtPeriodoHasta.TabIndex = 112
        Me.txtPeriodoHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtPeriodoHasta.Visible = False
        '
        'txtPeriodoDesde
        '
        Me.txtPeriodoDesde.Enabled = False
        Me.txtPeriodoDesde.Location = New System.Drawing.Point(233, 14)
        Me.txtPeriodoDesde.Name = "txtPeriodoDesde"
        Me.txtPeriodoDesde.Size = New System.Drawing.Size(107, 20)
        Me.txtPeriodoDesde.TabIndex = 111
        Me.txtPeriodoDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtPeriodoDesde.Visible = False
        '
        'lblPeriodoHasta
        '
        Me.lblPeriodoHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodoHasta.Location = New System.Drawing.Point(421, 17)
        Me.lblPeriodoHasta.Name = "lblPeriodoHasta"
        Me.lblPeriodoHasta.Size = New System.Drawing.Size(47, 17)
        Me.lblPeriodoHasta.TabIndex = 110
        Me.lblPeriodoHasta.Text = "Hasta"
        Me.lblPeriodoHasta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPeriodoDesde
        '
        Me.lblPeriodoDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodoDesde.Location = New System.Drawing.Point(167, 16)
        Me.lblPeriodoDesde.Name = "lblPeriodoDesde"
        Me.lblPeriodoDesde.Size = New System.Drawing.Size(60, 16)
        Me.lblPeriodoDesde.TabIndex = 109
        Me.lblPeriodoDesde.Text = "Desde"
        Me.lblPeriodoDesde.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPeriodo
        '
        Me.lblPeriodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodo.Location = New System.Drawing.Point(25, 19)
        Me.lblPeriodo.Name = "lblPeriodo"
        Me.lblPeriodo.Size = New System.Drawing.Size(136, 19)
        Me.lblPeriodo.TabIndex = 107
        Me.lblPeriodo.Text = "Periodo :"
        Me.lblPeriodo.Visible = False
        '
        'Label10
        '
        Me.Label10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label10.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(3, 1)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(230, 40)
        Me.Label10.TabIndex = 89
        Me.Label10.Text = "Datum"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblNombreReporte
        '
        Me.lblNombreReporte.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNombreReporte.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.lblNombreReporte.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblNombreReporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombreReporte.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.lblNombreReporte.Location = New System.Drawing.Point(3, 41)
        Me.lblNombreReporte.Name = "lblNombreReporte"
        Me.lblNombreReporte.Size = New System.Drawing.Size(230, 31)
        Me.lblNombreReporte.TabIndex = 90
        Me.lblNombreReporte.Tag = "Reportes Bancos"
        Me.lblNombreReporte.Text = "REPORTES BANCOS"
        Me.lblNombreReporte.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox1.Image = Global.Datum.My.Resources.Resources.bandaazul
        Me.PictureBox1.Location = New System.Drawing.Point(236, 1)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(640, 71)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 78
        Me.PictureBox1.TabStop = False
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 434)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(875, 44)
        Me.lblInfo.TabIndex = 91
        '
        'lblCaja
        '
        Me.lblCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaja.Location = New System.Drawing.Point(25, 125)
        Me.lblCaja.Name = "lblCaja"
        Me.lblCaja.Size = New System.Drawing.Size(136, 18)
        Me.lblCaja.TabIndex = 178
        Me.lblCaja.Text = "N° Caja :"
        Me.lblCaja.Visible = False
        '
        'txtCajaHasta
        '
        Me.txtCajaHasta.Enabled = False
        Me.txtCajaHasta.Location = New System.Drawing.Point(474, 121)
        Me.txtCajaHasta.Name = "txtCajaHasta"
        Me.txtCajaHasta.Size = New System.Drawing.Size(107, 20)
        Me.txtCajaHasta.TabIndex = 177
        Me.txtCajaHasta.Visible = False
        '
        'txtCajaDesde
        '
        Me.txtCajaDesde.Enabled = False
        Me.txtCajaDesde.Location = New System.Drawing.Point(233, 121)
        Me.txtCajaDesde.Name = "txtCajaDesde"
        Me.txtCajaDesde.Size = New System.Drawing.Size(107, 20)
        Me.txtCajaDesde.TabIndex = 176
        Me.txtCajaDesde.Visible = False
        '
        'lblCajaHasta
        '
        Me.lblCajaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCajaHasta.Location = New System.Drawing.Point(423, 125)
        Me.lblCajaHasta.Name = "lblCajaHasta"
        Me.lblCajaHasta.Size = New System.Drawing.Size(47, 17)
        Me.lblCajaHasta.TabIndex = 175
        Me.lblCajaHasta.Text = "Hasta"
        Me.lblCajaHasta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCajaDesde
        '
        Me.lblCajaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCajaDesde.Location = New System.Drawing.Point(167, 125)
        Me.lblCajaDesde.Name = "lblCajaDesde"
        Me.lblCajaDesde.Size = New System.Drawing.Size(60, 16)
        Me.lblCajaDesde.TabIndex = 174
        Me.lblCajaDesde.Text = "Desde"
        Me.lblCajaDesde.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'jsBanRepParametros
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234,Byte),Integer), CType(CType(241,Byte),Integer), CType(CType(250,Byte),Integer))
        Me.ClientSize = New System.Drawing.Size(875, 478)
        Me.ControlBox = false
        Me.Controls.Add(Me.grpCriterios)
        Me.Controls.Add(Me.grpConstantes)
        Me.Controls.Add(Me.grpOrden)
        Me.Controls.Add(Me.lblNombreReporte)
        Me.Controls.Add(Me.grpGrupos)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.grpImprimirSalir)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "jsBanRepParametros"
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpImprimirSalir.ResumeLayout(false)
        Me.grpConstantes.ResumeLayout(false)
        Me.grpConstantes.PerformLayout
        Me.grpOrden.ResumeLayout(false)
        Me.grpOrden.PerformLayout
        Me.grpCriterios.ResumeLayout(false)
        Me.grpCriterios.PerformLayout
        CType(Me.PictureBox1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents grpImprimirSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnLimpiar As System.Windows.Forms.Button
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents btnImprimir As System.Windows.Forms.Button
    Friend WithEvents grpConstantes As System.Windows.Forms.GroupBox
    Friend WithEvents grpGrupos As System.Windows.Forms.GroupBox
    Friend WithEvents grpOrden As System.Windows.Forms.GroupBox
    Friend WithEvents btnOrdenDesde As System.Windows.Forms.Button
    Friend WithEvents btnOrdenHasta As System.Windows.Forms.Button
    Friend WithEvents cmbOrdenadoPor As System.Windows.Forms.ComboBox
    Friend WithEvents txtOrdenHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdenDesde As System.Windows.Forms.TextBox
    Friend WithEvents lblOrdenDesde As System.Windows.Forms.Label
    Friend WithEvents lblOrdenHasta As System.Windows.Forms.Label
    Friend WithEvents grpCriterios As System.Windows.Forms.GroupBox
    Friend WithEvents txtPeriodoHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtPeriodoDesde As System.Windows.Forms.TextBox
    Friend WithEvents lblPeriodoHasta As System.Windows.Forms.Label
    Friend WithEvents lblPeriodoDesde As System.Windows.Forms.Label
    Friend WithEvents lblPeriodo As System.Windows.Forms.Label
    Friend WithEvents btnPeriodoHasta As System.Windows.Forms.Button
    Friend WithEvents btnPeriodoDesde As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblNombreReporte As System.Windows.Forms.Label
    Friend WithEvents lblTipodocumento As System.Windows.Forms.Label
    Friend WithEvents txtTipDoc As System.Windows.Forms.TextBox
    Friend WithEvents chkList As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblMesAño As System.Windows.Forms.Label
    Friend WithEvents cmbAño As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMes As System.Windows.Forms.ComboBox
    Friend WithEvents lblAño As System.Windows.Forms.Label
    Friend WithEvents lblMes As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblDocumento As System.Windows.Forms.Label
    Friend WithEvents txtDocumentoHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtDocumentoDesde As System.Windows.Forms.TextBox
    Friend WithEvents lblDocHasta As System.Windows.Forms.Label
    Friend WithEvents lblDocDesde As System.Windows.Forms.Label
    Friend WithEvents lblConsResumen As System.Windows.Forms.Label
    Friend WithEvents chkConsResumen As System.Windows.Forms.CheckBox
    Friend WithEvents chkTickets As System.Windows.Forms.CheckBox
    Friend WithEvents lblconsTickets As System.Windows.Forms.Label
    Friend WithEvents rdbXDepositar As System.Windows.Forms.RadioButton
    Friend WithEvents rdbDepositadas As System.Windows.Forms.RadioButton
    Friend WithEvents lblConsRemesas As System.Windows.Forms.Label
    Friend WithEvents cmbOrdenHasta As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOrdenDesde As System.Windows.Forms.ComboBox
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDepositoHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtDepositoDesde As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblOrigen As System.Windows.Forms.Label
    Friend WithEvents cmbOrigen As System.Windows.Forms.ComboBox
    Friend WithEvents lblOrigenSeleccion As System.Windows.Forms.Label
    Friend WithEvents btnOrigen As System.Windows.Forms.Button
    Friend WithEvents lblCaja As System.Windows.Forms.Label
    Friend WithEvents txtCajaHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtCajaDesde As System.Windows.Forms.TextBox
    Friend WithEvents lblCajaHasta As System.Windows.Forms.Label
    Friend WithEvents lblCajaDesde As System.Windows.Forms.Label
End Class
