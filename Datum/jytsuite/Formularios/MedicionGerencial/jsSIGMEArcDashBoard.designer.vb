<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsSIGMEArcDashBoard
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
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim DataPoint1 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 10.0R)
        Dim DataPoint2 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 20.0R)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsSIGMEArcDashBoard))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.tbcSIGME = New C1.Win.C1Command.C1DockingTab()
        Me.C1DockingTabPage1 = New C1.Win.C1Command.C1DockingTabPage()
        Me.chrtSaldosBancos = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.C1DockingTabPage2 = New C1.Win.C1Command.C1DockingTabPage()
        Me.C1DockingTabPage3 = New C1.Win.C1Command.C1DockingTabPage()
        Me.C1DockingTabPage4 = New C1.Win.C1Command.C1DockingTabPage()
        Me.C1DockingTabPage5 = New C1.Win.C1Command.C1DockingTabPage()
        Me.C1DockingTabPage6 = New C1.Win.C1Command.C1DockingTabPage()
        Me.C1DockingTabPage7 = New C1.Win.C1Command.C1DockingTabPage()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.chrtSaldosBancosXMes = New System.Windows.Forms.DataVisualization.Charting.Chart()
        CType(Me.tbcSIGME, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbcSIGME.SuspendLayout()
        Me.C1DockingTabPage1.SuspendLayout()
        CType(Me.chrtSaldosBancos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chrtSaldosBancosXMes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 500)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(1081, 27)
        Me.lblInfo.TabIndex = 31
        '
        'tbcSIGME
        '
        Me.tbcSIGME.Controls.Add(Me.C1DockingTabPage1)
        Me.tbcSIGME.Controls.Add(Me.C1DockingTabPage2)
        Me.tbcSIGME.Controls.Add(Me.C1DockingTabPage3)
        Me.tbcSIGME.Controls.Add(Me.C1DockingTabPage4)
        Me.tbcSIGME.Controls.Add(Me.C1DockingTabPage5)
        Me.tbcSIGME.Controls.Add(Me.C1DockingTabPage6)
        Me.tbcSIGME.Controls.Add(Me.C1DockingTabPage7)
        Me.tbcSIGME.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbcSIGME.HotTrack = True
        Me.tbcSIGME.Location = New System.Drawing.Point(0, 0)
        Me.tbcSIGME.Name = "tbcSIGME"
        Me.tbcSIGME.SelectedIndex = 6
        Me.tbcSIGME.Size = New System.Drawing.Size(1081, 500)
        Me.tbcSIGME.TabIndex = 108
        Me.tbcSIGME.TabsSpacing = 5
        Me.tbcSIGME.TabStyle = C1.Win.C1Command.TabStyleEnum.Office2007
        Me.tbcSIGME.VisualStyle = C1.Win.C1Command.VisualStyle.Office2007Blue
        Me.tbcSIGME.VisualStyleBase = C1.Win.C1Command.VisualStyle.Office2007Blue
        '
        'C1DockingTabPage1
        '
        Me.C1DockingTabPage1.Controls.Add(Me.chrtSaldosBancosXMes)
        Me.C1DockingTabPage1.Controls.Add(Me.chrtSaldosBancos)
        Me.C1DockingTabPage1.Location = New System.Drawing.Point(1, 24)
        Me.C1DockingTabPage1.Name = "C1DockingTabPage1"
        Me.C1DockingTabPage1.Size = New System.Drawing.Size(1079, 475)
        Me.C1DockingTabPage1.TabIndex = 0
        Me.C1DockingTabPage1.Text = "Bancos y Cajas"
        '
        'chrtSaldosBancos
        '
        Me.chrtSaldosBancos.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        ChartArea2.Area3DStyle.Enable3D = True
        ChartArea2.Area3DStyle.Inclination = 60
        ChartArea2.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        ChartArea2.Name = "ChartArea1"
        Me.chrtSaldosBancos.ChartAreas.Add(ChartArea2)
        Legend2.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Legend2.Name = "Legend1"
        Me.chrtSaldosBancos.Legends.Add(Legend2)
        Me.chrtSaldosBancos.Location = New System.Drawing.Point(-1, 4)
        Me.chrtSaldosBancos.Name = "chrtSaldosBancos"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Me.chrtSaldosBancos.Series.Add(Series2)
        Me.chrtSaldosBancos.Size = New System.Drawing.Size(496, 469)
        Me.chrtSaldosBancos.TabIndex = 0
        '
        'C1DockingTabPage2
        '
        Me.C1DockingTabPage2.CaptionText = "Movimientos"
        Me.C1DockingTabPage2.Location = New System.Drawing.Point(1, 24)
        Me.C1DockingTabPage2.Name = "C1DockingTabPage2"
        Me.C1DockingTabPage2.Size = New System.Drawing.Size(1079, 475)
        Me.C1DockingTabPage2.TabIndex = 1
        Me.C1DockingTabPage2.Text = "Recursos Humanos"
        '
        'C1DockingTabPage3
        '
        Me.C1DockingTabPage3.Location = New System.Drawing.Point(1, 24)
        Me.C1DockingTabPage3.Name = "C1DockingTabPage3"
        Me.C1DockingTabPage3.Size = New System.Drawing.Size(1079, 475)
        Me.C1DockingTabPage3.TabIndex = 2
        Me.C1DockingTabPage3.Text = "Compras y Pagos"
        '
        'C1DockingTabPage4
        '
        Me.C1DockingTabPage4.Location = New System.Drawing.Point(1, 24)
        Me.C1DockingTabPage4.Name = "C1DockingTabPage4"
        Me.C1DockingTabPage4.Size = New System.Drawing.Size(1079, 475)
        Me.C1DockingTabPage4.TabIndex = 3
        Me.C1DockingTabPage4.Text = "Ventas y Cobros"
        '
        'C1DockingTabPage5
        '
        Me.C1DockingTabPage5.Location = New System.Drawing.Point(1, 24)
        Me.C1DockingTabPage5.Name = "C1DockingTabPage5"
        Me.C1DockingTabPage5.Size = New System.Drawing.Size(1079, 475)
        Me.C1DockingTabPage5.TabIndex = 4
        Me.C1DockingTabPage5.Text = "Puntos de Ventas"
        '
        'C1DockingTabPage6
        '
        Me.C1DockingTabPage6.CaptionText = "Expediente"
        Me.C1DockingTabPage6.Location = New System.Drawing.Point(1, 24)
        Me.C1DockingTabPage6.Name = "C1DockingTabPage6"
        Me.C1DockingTabPage6.Size = New System.Drawing.Size(1079, 475)
        Me.C1DockingTabPage6.TabIndex = 5
        Me.C1DockingTabPage6.Text = "Mercancías"
        '
        'C1DockingTabPage7
        '
        Me.C1DockingTabPage7.Location = New System.Drawing.Point(1, 24)
        Me.C1DockingTabPage7.Name = "C1DockingTabPage7"
        Me.C1DockingTabPage7.Size = New System.Drawing.Size(1079, 475)
        Me.C1DockingTabPage7.TabIndex = 6
        Me.C1DockingTabPage7.Text = "Producción"
        '
        'Label28
        '
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(203, 220)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(148, 19)
        Me.Label28.TabIndex = 126
        Me.Label28.Text = "Alto/Ancho/Profundidad"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label29
        '
        Me.Label29.BackColor = System.Drawing.Color.Transparent
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(0, 219)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(109, 19)
        Me.Label29.TabIndex = 125
        Me.Label29.Text = "Ubicación"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label30
        '
        Me.Label30.BackColor = System.Drawing.Color.Transparent
        Me.Label30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(219, 198)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(132, 19)
        Me.Label30.TabIndex = 124
        Me.Label30.Text = "Existencia máxima"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label31
        '
        Me.Label31.BackColor = System.Drawing.Color.Transparent
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(0, 197)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(109, 19)
        Me.Label31.TabIndex = 121
        Me.Label31.Text = "Existencia mínima"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label32
        '
        Me.Label32.BackColor = System.Drawing.Color.Transparent
        Me.Label32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(440, 177)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(44, 19)
        Me.Label32.TabIndex = 120
        Me.Label32.Text = "KGS."
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label33
        '
        Me.Label33.BackColor = System.Drawing.Color.Transparent
        Me.Label33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(216, 175)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(135, 19)
        Me.Label33.TabIndex = 119
        Me.Label33.Text = "Peso unidad de venta"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label34
        '
        Me.Label34.BackColor = System.Drawing.Color.Transparent
        Me.Label34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(490, 142)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(144, 19)
        Me.Label34.TabIndex = 117
        Me.Label34.Text = "¿Acepta devoluciones?"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label35
        '
        Me.Label35.BackColor = System.Drawing.Color.Transparent
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(533, 11)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(104, 19)
        Me.Label35.TabIndex = 110
        Me.Label35.Text = "Barras"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label36
        '
        Me.Label36.BackColor = System.Drawing.Color.Transparent
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(498, 180)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(104, 44)
        Me.Label36.TabIndex = 107
        Me.Label36.Text = "La unidad de venta es equivalente a"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label37
        '
        Me.Label37.BackColor = System.Drawing.Color.Transparent
        Me.Label37.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(5, 10)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(104, 19)
        Me.Label37.TabIndex = 27
        Me.Label37.Text = "Código"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label38
        '
        Me.Label38.BackColor = System.Drawing.Color.Transparent
        Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(8, 177)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(101, 19)
        Me.Label38.TabIndex = 42
        Me.Label38.Text = "Unidad de venta"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label39
        '
        Me.Label39.BackColor = System.Drawing.Color.Transparent
        Me.Label39.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(286, 11)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(104, 19)
        Me.Label39.TabIndex = 29
        Me.Label39.Text = "Alterno"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label40
        '
        Me.Label40.BackColor = System.Drawing.Color.Transparent
        Me.Label40.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(247, 257)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(104, 29)
        Me.Label40.TabIndex = 41
        Me.Label40.Text = "¿Cartera de asesores?"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label41
        '
        Me.Label41.BackColor = System.Drawing.Color.Transparent
        Me.Label41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(5, 31)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(104, 19)
        Me.Label41.TabIndex = 30
        Me.Label41.Text = "Nombre"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(655, 141)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(111, 20)
        Me.Label42.TabIndex = 40
        Me.Label42.Text = "% Devoluciones"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label43
        '
        Me.Label43.BackColor = System.Drawing.Color.Transparent
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(5, 64)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(104, 19)
        Me.Label43.TabIndex = 31
        Me.Label43.Text = "Categoría"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label44
        '
        Me.Label44.BackColor = System.Drawing.Color.Transparent
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(437, 64)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(104, 19)
        Me.Label44.TabIndex = 32
        Me.Label44.Text = "Marca"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label45
        '
        Me.Label45.BackColor = System.Drawing.Color.Transparent
        Me.Label45.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(5, 85)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(104, 19)
        Me.Label45.TabIndex = 33
        Me.Label45.Text = "División"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label46
        '
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.Location = New System.Drawing.Point(437, 86)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(104, 19)
        Me.Label46.TabIndex = 34
        Me.Label46.Text = "Tipo jerarquía"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label47
        '
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(5, 106)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(104, 19)
        Me.Label47.TabIndex = 35
        Me.Label47.Text = "Jerarquía"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label48
        '
        Me.Label48.BackColor = System.Drawing.Color.Transparent
        Me.Label48.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.Location = New System.Drawing.Point(5, 265)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(104, 19)
        Me.Label48.TabIndex = 36
        Me.Label48.Text = "IVA"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label49
        '
        Me.Label49.BackColor = System.Drawing.Color.Transparent
        Me.Label49.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(5, 142)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(104, 19)
        Me.Label49.TabIndex = 37
        Me.Label49.Text = "Presentación"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label50
        '
        Me.Label50.BackColor = System.Drawing.Color.Transparent
        Me.Label50.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.Location = New System.Drawing.Point(388, 143)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(82, 19)
        Me.Label50.TabIndex = 39
        Me.Label50.Text = "¿Regulada?"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.Button1.Image = Global.Datum.My.Resources.Resources.Apagar_2
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(996, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(81, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Salir"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'chrtSaldosBancosXMes
        '
        Me.chrtSaldosBancosXMes.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        ChartArea1.Area3DStyle.Inclination = 60
        ChartArea1.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        ChartArea1.Name = "ChartArea1"
        Me.chrtSaldosBancosXMes.ChartAreas.Add(ChartArea1)
        Legend1.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Legend1.Name = "Legend1"
        Me.chrtSaldosBancosXMes.Legends.Add(Legend1)
        Me.chrtSaldosBancosXMes.Location = New System.Drawing.Point(517, 3)
        Me.chrtSaldosBancosXMes.Name = "chrtSaldosBancosXMes"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Series1.Points.Add(DataPoint1)
        Series1.Points.Add(DataPoint2)
        Me.chrtSaldosBancosXMes.Series.Add(Series1)
        Me.chrtSaldosBancosXMes.Size = New System.Drawing.Size(496, 469)
        Me.chrtSaldosBancosXMes.TabIndex = 1
        '
        'jsSIGMEArcDashBoard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1081, 527)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.tbcSIGME)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsSIGMEArcDashBoard"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Tag = "DashBoard"
        CType(Me.tbcSIGME, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbcSIGME.ResumeLayout(False)
        Me.C1DockingTabPage1.ResumeLayout(False)
        CType(Me.chrtSaldosBancos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chrtSaldosBancosXMes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents tbcSIGME As C1.Win.C1Command.C1DockingTab
    Friend WithEvents C1DockingTabPage1 As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents C1DockingTabPage2 As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents C1DockingTabPage3 As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents C1DockingTabPage4 As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents C1DockingTabPage5 As C1.Win.C1Command.C1DockingTabPage



































    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label


    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label

    Friend WithEvents Label34 As System.Windows.Forms.Label



    Friend WithEvents Button18 As System.Windows.Forms.Button


    Friend WithEvents Label35 As System.Windows.Forms.Label



    Friend WithEvents Label36 As System.Windows.Forms.Label

    Friend WithEvents Label37 As System.Windows.Forms.Label









    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label



    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label



    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label

    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents C1DockingTabPage6 As C1.Win.C1Command.C1DockingTabPage

    Friend WithEvents C1DockingTabPage7 As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents chrtSaldosBancos As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents chrtSaldosBancosXMes As System.Windows.Forms.DataVisualization.Charting.Chart
End Class
