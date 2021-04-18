<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsPOSRepParametros
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsPOSRepParametros))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.grpImprimirSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.grpConstantes = New System.Windows.Forms.GroupBox()
        Me.cmbTipoNomina = New System.Windows.Forms.ComboBox()
        Me.lblconTipoNomina = New System.Windows.Forms.Label()
        Me.lblConsResumen = New System.Windows.Forms.Label()
        Me.chkConsResumen = New System.Windows.Forms.CheckBox()
        Me.grpGrupos = New System.Windows.Forms.GroupBox()
        Me.lblG1H = New System.Windows.Forms.Label()
        Me.lblG1D = New System.Windows.Forms.Label()
        Me.cmbTNHasta = New System.Windows.Forms.ComboBox()
        Me.cmbTNDesde = New System.Windows.Forms.ComboBox()
        Me.cmbAgrupadorPor = New System.Windows.Forms.ComboBox()
        Me.grpOrden = New System.Windows.Forms.GroupBox()
        Me.btnOrdenDesde = New System.Windows.Forms.Button()
        Me.btnOrdenHasta = New System.Windows.Forms.Button()
        Me.cmbOrdenadoPor = New System.Windows.Forms.ComboBox()
        Me.txtOrdenHasta = New System.Windows.Forms.TextBox()
        Me.txtOrdenDesde = New System.Windows.Forms.TextBox()
        Me.lblOrdenDesde = New System.Windows.Forms.Label()
        Me.lblOrdenHasta = New System.Windows.Forms.Label()
        Me.grpCriterios = New System.Windows.Forms.GroupBox()
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
        Me.grpImprimirSalir.SuspendLayout()
        Me.grpConstantes.SuspendLayout()
        Me.grpGrupos.SuspendLayout()
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
        Me.btnLimpiar.Image = Global.POS_Datum.My.Resources.Resources.reiniciar
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
        Me.btnSalir.Image = Global.POS_Datum.My.Resources.Resources.Apagar
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
        Me.btnImprimir.Image = Global.POS_Datum.My.Resources.Resources.Imprimir
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
        Me.grpConstantes.Controls.Add(Me.cmbTipoNomina)
        Me.grpConstantes.Controls.Add(Me.lblconTipoNomina)
        Me.grpConstantes.Controls.Add(Me.lblConsResumen)
        Me.grpConstantes.Controls.Add(Me.chkConsResumen)
        Me.grpConstantes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpConstantes.Location = New System.Drawing.Point(6, 327)
        Me.grpConstantes.Name = "grpConstantes"
        Me.grpConstantes.Size = New System.Drawing.Size(870, 87)
        Me.grpConstantes.TabIndex = 3
        Me.grpConstantes.TabStop = False
        Me.grpConstantes.Text = "Constantes :"
        '
        'cmbTipoNomina
        '
        Me.cmbTipoNomina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoNomina.FormattingEnabled = True
        Me.cmbTipoNomina.Location = New System.Drawing.Point(201, 38)
        Me.cmbTipoNomina.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbTipoNomina.Name = "cmbTipoNomina"
        Me.cmbTipoNomina.Size = New System.Drawing.Size(160, 21)
        Me.cmbTipoNomina.TabIndex = 131
        '
        'lblconTipoNomina
        '
        Me.lblconTipoNomina.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblconTipoNomina.Location = New System.Drawing.Point(37, 41)
        Me.lblconTipoNomina.Name = "lblconTipoNomina"
        Me.lblconTipoNomina.Size = New System.Drawing.Size(116, 16)
        Me.lblconTipoNomina.TabIndex = 130
        Me.lblconTipoNomina.Text = "Tipo de nómina :"
        Me.lblconTipoNomina.Visible = False
        '
        'lblConsResumen
        '
        Me.lblConsResumen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConsResumen.Location = New System.Drawing.Point(37, 20)
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
        Me.grpGrupos.Controls.Add(Me.lblG1H)
        Me.grpGrupos.Controls.Add(Me.lblG1D)
        Me.grpGrupos.Controls.Add(Me.cmbTNHasta)
        Me.grpGrupos.Controls.Add(Me.cmbTNDesde)
        Me.grpGrupos.Controls.Add(Me.cmbAgrupadorPor)
        Me.grpGrupos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpGrupos.Location = New System.Drawing.Point(3, 132)
        Me.grpGrupos.Name = "grpGrupos"
        Me.grpGrupos.Size = New System.Drawing.Size(870, 76)
        Me.grpGrupos.TabIndex = 1
        Me.grpGrupos.TabStop = False
        Me.grpGrupos.Text = "Agrupado por :"
        '
        'lblG1H
        '
        Me.lblG1H.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblG1H.Location = New System.Drawing.Point(569, 21)
        Me.lblG1H.Name = "lblG1H"
        Me.lblG1H.Size = New System.Drawing.Size(45, 17)
        Me.lblG1H.TabIndex = 116
        Me.lblG1H.Text = "Hasta"
        Me.lblG1H.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblG1D
        '
        Me.lblG1D.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblG1D.Location = New System.Drawing.Point(309, 21)
        Me.lblG1D.Name = "lblG1D"
        Me.lblG1D.Size = New System.Drawing.Size(55, 17)
        Me.lblG1D.TabIndex = 115
        Me.lblG1D.Text = "Desde"
        Me.lblG1D.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbTNHasta
        '
        Me.cmbTNHasta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTNHasta.FormattingEnabled = True
        Me.cmbTNHasta.Location = New System.Drawing.Point(618, 17)
        Me.cmbTNHasta.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbTNHasta.Name = "cmbTNHasta"
        Me.cmbTNHasta.Size = New System.Drawing.Size(160, 21)
        Me.cmbTNHasta.TabIndex = 114
        '
        'cmbTNDesde
        '
        Me.cmbTNDesde.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTNDesde.FormattingEnabled = True
        Me.cmbTNDesde.Location = New System.Drawing.Point(370, 17)
        Me.cmbTNDesde.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbTNDesde.Name = "cmbTNDesde"
        Me.cmbTNDesde.Size = New System.Drawing.Size(160, 21)
        Me.cmbTNDesde.TabIndex = 113
        '
        'cmbAgrupadorPor
        '
        Me.cmbAgrupadorPor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAgrupadorPor.FormattingEnabled = True
        Me.cmbAgrupadorPor.Location = New System.Drawing.Point(21, 17)
        Me.cmbAgrupadorPor.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbAgrupadorPor.Name = "cmbAgrupadorPor"
        Me.cmbAgrupadorPor.Size = New System.Drawing.Size(216, 21)
        Me.cmbAgrupadorPor.TabIndex = 112
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
        Me.grpCriterios.Size = New System.Drawing.Size(870, 112)
        Me.grpCriterios.TabIndex = 2
        Me.grpCriterios.TabStop = False
        Me.grpCriterios.Text = " Criterios :"
        '
        'btnPeriodoHasta
        '
        Me.btnPeriodoHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPeriodoHasta.Location = New System.Drawing.Point(514, 13)
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
        Me.btnPeriodoDesde.Location = New System.Drawing.Point(317, 14)
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
        Me.txtPeriodoHasta.Location = New System.Drawing.Point(401, 14)
        Me.txtPeriodoHasta.Name = "txtPeriodoHasta"
        Me.txtPeriodoHasta.Size = New System.Drawing.Size(107, 20)
        Me.txtPeriodoHasta.TabIndex = 112
        Me.txtPeriodoHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtPeriodoHasta.Visible = False
        '
        'txtPeriodoDesde
        '
        Me.txtPeriodoDesde.Enabled = False
        Me.txtPeriodoDesde.Location = New System.Drawing.Point(204, 14)
        Me.txtPeriodoDesde.Name = "txtPeriodoDesde"
        Me.txtPeriodoDesde.Size = New System.Drawing.Size(107, 20)
        Me.txtPeriodoDesde.TabIndex = 111
        Me.txtPeriodoDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtPeriodoDesde.Visible = False
        '
        'lblPeriodoHasta
        '
        Me.lblPeriodoHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodoHasta.Location = New System.Drawing.Point(348, 16)
        Me.lblPeriodoHasta.Name = "lblPeriodoHasta"
        Me.lblPeriodoHasta.Size = New System.Drawing.Size(47, 17)
        Me.lblPeriodoHasta.TabIndex = 110
        Me.lblPeriodoHasta.Text = "Hasta"
        Me.lblPeriodoHasta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPeriodoDesde
        '
        Me.lblPeriodoDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodoDesde.Location = New System.Drawing.Point(141, 15)
        Me.lblPeriodoDesde.Name = "lblPeriodoDesde"
        Me.lblPeriodoDesde.Size = New System.Drawing.Size(60, 16)
        Me.lblPeriodoDesde.TabIndex = 109
        Me.lblPeriodoDesde.Text = "Desde"
        Me.lblPeriodoDesde.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPeriodo
        '
        Me.lblPeriodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeriodo.Location = New System.Drawing.Point(40, 16)
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
        Me.lblNombreReporte.Tag = "Reportes Recursos Humanos"
        Me.lblNombreReporte.Text = "REPORTES PUNTO DE VENTA"
        Me.lblNombreReporte.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox1.Image = Global.POS_Datum.My.Resources.Resources.banda_azul
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
        'jsPOSRepParametros
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
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
        Me.Name = "jsPOSRepParametros"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.grpImprimirSalir.ResumeLayout(False)
        Me.grpConstantes.ResumeLayout(False)
        Me.grpConstantes.PerformLayout()
        Me.grpGrupos.ResumeLayout(False)
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
    Friend WithEvents txtPeriodoHasta As System.Windows.Forms.TextBox
    Friend WithEvents txtPeriodoDesde As System.Windows.Forms.TextBox
    Friend WithEvents lblPeriodoHasta As System.Windows.Forms.Label
    Friend WithEvents lblPeriodoDesde As System.Windows.Forms.Label
    Friend WithEvents lblPeriodo As System.Windows.Forms.Label
    Friend WithEvents btnPeriodoHasta As System.Windows.Forms.Button
    Friend WithEvents btnPeriodoDesde As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblNombreReporte As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblConsResumen As System.Windows.Forms.Label
    Friend WithEvents chkConsResumen As System.Windows.Forms.CheckBox
    Friend WithEvents cmbAgrupadorPor As System.Windows.Forms.ComboBox
    Friend WithEvents lblG1H As System.Windows.Forms.Label
    Friend WithEvents lblG1D As System.Windows.Forms.Label
    Friend WithEvents cmbTNHasta As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTNDesde As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTipoNomina As System.Windows.Forms.ComboBox
    Friend WithEvents lblconTipoNomina As System.Windows.Forms.Label
    Friend WithEvents lblInfo As System.Windows.Forms.Label
End Class
