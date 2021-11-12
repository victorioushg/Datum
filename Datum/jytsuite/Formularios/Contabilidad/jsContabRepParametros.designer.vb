<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsContabRepParametros
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsContabRepParametros))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.grpImprimirSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.grpConstantes = New System.Windows.Forms.GroupBox()
        Me.cmbNivel = New System.Windows.Forms.ComboBox()
        Me.lblNivel = New System.Windows.Forms.Label()
        Me.lblConstante1 = New System.Windows.Forms.Label()
        Me.cmbConstante_TipoCuenta = New System.Windows.Forms.ComboBox()
        Me.grpGrupos = New System.Windows.Forms.GroupBox()
        Me.grpOrden = New System.Windows.Forms.GroupBox()
        Me.btnOrdenDesde = New System.Windows.Forms.Button()
        Me.btnOrdenHasta = New System.Windows.Forms.Button()
        Me.cmbOrdenadoPor = New System.Windows.Forms.ComboBox()
        Me.txtOrdenHasta = New System.Windows.Forms.TextBox()
        Me.txtOrdenDesde = New System.Windows.Forms.TextBox()
        Me.lblOrdenDesde = New System.Windows.Forms.Label()
        Me.lblOrdenHasta = New System.Windows.Forms.Label()
        Me.grpCriterios = New System.Windows.Forms.GroupBox()
        Me.lblAño = New System.Windows.Forms.Label()
        Me.lblMes = New System.Windows.Forms.Label()
        Me.cmbAño = New System.Windows.Forms.ComboBox()
        Me.cmbMes = New System.Windows.Forms.ComboBox()
        Me.lblMesAño = New System.Windows.Forms.Label()
        Me.chkList = New System.Windows.Forms.CheckedListBox()
        Me.lblTipodocumento = New System.Windows.Forms.Label()
        Me.txtTipDoc = New System.Windows.Forms.TextBox()
        Me.lblPeriodo = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblNombreReporte = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.txtPeriodoDesde = New Syncfusion.WinForms.Input.SfDateTimeEdit()
        Me.txtPeriodoHasta = New Syncfusion.WinForms.Input.SfDateTimeEdit()
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
        Me.grpConstantes.Controls.Add(Me.cmbNivel)
        Me.grpConstantes.Controls.Add(Me.lblNivel)
        Me.grpConstantes.Controls.Add(Me.lblConstante1)
        Me.grpConstantes.Controls.Add(Me.cmbConstante_TipoCuenta)
        Me.grpConstantes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpConstantes.Location = New System.Drawing.Point(6, 311)
        Me.grpConstantes.Name = "grpConstantes"
        Me.grpConstantes.Size = New System.Drawing.Size(870, 76)
        Me.grpConstantes.TabIndex = 3
        Me.grpConstantes.TabStop = False
        Me.grpConstantes.Text = "Constantes :"
        '
        'cmbNivel
        '
        Me.cmbNivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNivel.FormattingEnabled = True
        Me.cmbNivel.Location = New System.Drawing.Point(201, 40)
        Me.cmbNivel.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbNivel.Name = "cmbNivel"
        Me.cmbNivel.Size = New System.Drawing.Size(107, 21)
        Me.cmbNivel.TabIndex = 124
        '
        'lblNivel
        '
        Me.lblNivel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNivel.Location = New System.Drawing.Point(6, 43)
        Me.lblNivel.Name = "lblNivel"
        Me.lblNivel.Size = New System.Drawing.Size(191, 18)
        Me.lblNivel.TabIndex = 123
        Me.lblNivel.Text = "Nivel de cuenta contable :"
        Me.lblNivel.Visible = False
        '
        'lblConstante1
        '
        Me.lblConstante1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConstante1.Location = New System.Drawing.Point(6, 20)
        Me.lblConstante1.Name = "lblConstante1"
        Me.lblConstante1.Size = New System.Drawing.Size(191, 18)
        Me.lblConstante1.TabIndex = 122
        Me.lblConstante1.Text = "Tipo de cuenta contable :"
        Me.lblConstante1.Visible = False
        '
        'cmbConstante_TipoCuenta
        '
        Me.cmbConstante_TipoCuenta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbConstante_TipoCuenta.FormattingEnabled = True
        Me.cmbConstante_TipoCuenta.Location = New System.Drawing.Point(201, 17)
        Me.cmbConstante_TipoCuenta.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbConstante_TipoCuenta.Name = "cmbConstante_TipoCuenta"
        Me.cmbConstante_TipoCuenta.Size = New System.Drawing.Size(107, 21)
        Me.cmbConstante_TipoCuenta.TabIndex = 121
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
        'btnOrdenDesde
        '
        Me.btnOrdenDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrdenDesde.Location = New System.Drawing.Point(536, 17)
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
        Me.txtOrdenHasta.Location = New System.Drawing.Point(618, 17)
        Me.txtOrdenHasta.Name = "txtOrdenHasta"
        Me.txtOrdenHasta.Size = New System.Drawing.Size(160, 20)
        Me.txtOrdenHasta.TabIndex = 110
        '
        'txtOrdenDesde
        '
        Me.txtOrdenDesde.Location = New System.Drawing.Point(370, 17)
        Me.txtOrdenDesde.Name = "txtOrdenDesde"
        Me.txtOrdenDesde.Size = New System.Drawing.Size(160, 20)
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
        Me.grpCriterios.Controls.Add(Me.txtPeriodoHasta)
        Me.grpCriterios.Controls.Add(Me.txtPeriodoDesde)
        Me.grpCriterios.Controls.Add(Me.lblAño)
        Me.grpCriterios.Controls.Add(Me.lblMes)
        Me.grpCriterios.Controls.Add(Me.cmbAño)
        Me.grpCriterios.Controls.Add(Me.cmbMes)
        Me.grpCriterios.Controls.Add(Me.lblMesAño)
        Me.grpCriterios.Controls.Add(Me.chkList)
        Me.grpCriterios.Controls.Add(Me.lblTipodocumento)
        Me.grpCriterios.Controls.Add(Me.txtTipDoc)
        Me.grpCriterios.Controls.Add(Me.lblPeriodo)
        Me.grpCriterios.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpCriterios.Location = New System.Drawing.Point(3, 211)
        Me.grpCriterios.Name = "grpCriterios"
        Me.grpCriterios.Size = New System.Drawing.Size(870, 97)
        Me.grpCriterios.TabIndex = 2
        Me.grpCriterios.TabStop = False
        Me.grpCriterios.Text = " Criterios :"
        '
        'lblAño
        '
        Me.lblAño.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAño.Location = New System.Drawing.Point(337, 65)
        Me.lblAño.Name = "lblAño"
        Me.lblAño.Size = New System.Drawing.Size(60, 16)
        Me.lblAño.TabIndex = 123
        Me.lblAño.Text = "Año"
        Me.lblAño.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMes
        '
        Me.lblMes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMes.Location = New System.Drawing.Point(140, 65)
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
        Me.cmbAño.Location = New System.Drawing.Point(401, 64)
        Me.cmbAño.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbAño.Name = "cmbAño"
        Me.cmbAño.Size = New System.Drawing.Size(107, 21)
        Me.cmbAño.TabIndex = 121
        '
        'cmbMes
        '
        Me.cmbMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMes.FormattingEnabled = True
        Me.cmbMes.Location = New System.Drawing.Point(204, 64)
        Me.cmbMes.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbMes.Name = "cmbMes"
        Me.cmbMes.Size = New System.Drawing.Size(107, 21)
        Me.cmbMes.TabIndex = 120
        '
        'lblMesAño
        '
        Me.lblMesAño.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMesAño.Location = New System.Drawing.Point(9, 67)
        Me.lblMesAño.Name = "lblMesAño"
        Me.lblMesAño.Size = New System.Drawing.Size(131, 17)
        Me.lblMesAño.TabIndex = 119
        Me.lblMesAño.Text = "Mes y Año"
        Me.lblMesAño.Visible = False
        '
        'chkList
        '
        Me.chkList.ColumnWidth = 60
        Me.chkList.FormattingEnabled = True
        Me.chkList.Location = New System.Drawing.Point(204, 39)
        Me.chkList.MultiColumn = True
        Me.chkList.Name = "chkList"
        Me.chkList.Size = New System.Drawing.Size(335, 19)
        Me.chkList.TabIndex = 118
        '
        'lblTipodocumento
        '
        Me.lblTipodocumento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTipodocumento.Location = New System.Drawing.Point(9, 39)
        Me.lblTipodocumento.Name = "lblTipodocumento"
        Me.lblTipodocumento.Size = New System.Drawing.Size(131, 17)
        Me.lblTipodocumento.TabIndex = 117
        Me.lblTipodocumento.Text = "Tipos de documento :"
        Me.lblTipodocumento.Visible = False
        '
        'txtTipDoc
        '
        Me.txtTipDoc.Enabled = False
        Me.txtTipDoc.Location = New System.Drawing.Point(545, 39)
        Me.txtTipDoc.Name = "txtTipDoc"
        Me.txtTipDoc.Size = New System.Drawing.Size(191, 20)
        Me.txtTipDoc.TabIndex = 116
        Me.txtTipDoc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtTipDoc.Visible = False
        '
        'lblPeriodo
        '
        Me.lblPeriodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodo.Location = New System.Drawing.Point(9, 16)
        Me.lblPeriodo.Name = "lblPeriodo"
        Me.lblPeriodo.Size = New System.Drawing.Size(116, 16)
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
        Me.lblNombreReporte.Tag = "Reportes Contabilidad"
        Me.lblNombreReporte.Text = "REPORTES CONTABILIDAD"
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
        'txtPeriodoDesde
        '
        Me.txtPeriodoDesde.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtPeriodoDesde.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtPeriodoDesde.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtPeriodoDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeriodoDesde.Location = New System.Drawing.Point(204, 14)
        Me.txtPeriodoDesde.Name = "txtPeriodoDesde"
        Me.txtPeriodoDesde.Size = New System.Drawing.Size(114, 19)
        Me.txtPeriodoDesde.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtPeriodoDesde.TabIndex = 214
        '
        'txtPeriodoHasta
        '
        Me.txtPeriodoHasta.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtPeriodoHasta.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtPeriodoHasta.DateTimeEditingMode = Syncfusion.WinForms.Input.Enums.DateTimeEditingMode.Mask
        Me.txtPeriodoHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeriodoHasta.Location = New System.Drawing.Point(324, 14)
        Me.txtPeriodoHasta.Name = "txtPeriodoHasta"
        Me.txtPeriodoHasta.Size = New System.Drawing.Size(114, 19)
        Me.txtPeriodoHasta.Style.BackColor = System.Drawing.Color.AliceBlue
        Me.txtPeriodoHasta.TabIndex = 215
        '
        'jsContabRepParametros
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(875, 478)
        Me.ControlBox = False
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
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsContabRepParametros"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpImprimirSalir.ResumeLayout(False)
        Me.grpConstantes.ResumeLayout(False)
        Me.grpOrden.ResumeLayout(False)
        Me.grpOrden.PerformLayout()
        Me.grpCriterios.ResumeLayout(False)
        Me.grpCriterios.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
    Friend WithEvents lblPeriodo As System.Windows.Forms.Label
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
    Friend WithEvents lblConstante1 As System.Windows.Forms.Label
    Friend WithEvents cmbConstante_TipoCuenta As System.Windows.Forms.ComboBox
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents cmbNivel As System.Windows.Forms.ComboBox
    Friend WithEvents lblNivel As System.Windows.Forms.Label
    Friend WithEvents txtPeriodoHasta As Syncfusion.WinForms.Input.SfDateTimeEdit
    Friend WithEvents txtPeriodoDesde As Syncfusion.WinForms.Input.SfDateTimeEdit
End Class
