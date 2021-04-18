<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsBanProConciliacion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsBanProConciliacion))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lv = New System.Windows.Forms.ListView()
        Me.grpCaja = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbEjercicio = New System.Windows.Forms.ComboBox()
        Me.cmbCuenta = New System.Windows.Forms.ComboBox()
        Me.cmbMes = New System.Windows.Forms.ComboBox()
        Me.lblejercicio = New System.Windows.Forms.Label()
        Me.lblcuenta = New System.Windows.Forms.Label()
        Me.txtSalC = New System.Windows.Forms.TextBox()
        Me.txtSalMesAct = New System.Windows.Forms.TextBox()
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtSaldoEnLibros = New System.Windows.Forms.TextBox()
        Me.txtSaldoEnBancos = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDBNC = New System.Windows.Forms.TextBox()
        Me.txtDBC = New System.Windows.Forms.TextBox()
        Me.txtDBMesAct = New System.Windows.Forms.TextBox()
        Me.txtDBMesAnt = New System.Windows.Forms.TextBox()
        Me.txtCRMesAct = New System.Windows.Forms.TextBox()
        Me.txtCRC = New System.Windows.Forms.TextBox()
        Me.txtSalMesAnt = New System.Windows.Forms.TextBox()
        Me.txtCRMesAnt = New System.Windows.Forms.TextBox()
        Me.txtSalNC = New System.Windows.Forms.TextBox()
        Me.txtCRNC = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.grpCaja.SuspendLayout()
        Me.grpTotales.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Location = New System.Drawing.Point(1, 467)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(566, 21)
        Me.lblInfo.TabIndex = 80
        '
        'lv
        '
        Me.lv.BackColor = System.Drawing.Color.MintCream
        Me.lv.CheckBoxes = True
        Me.lv.FullRowSelect = True
        Me.lv.GridLines = True
        Me.lv.HoverSelection = True
        Me.lv.Location = New System.Drawing.Point(-1, 127)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(730, 171)
        Me.lv.TabIndex = 81
        Me.lv.UseCompatibleStateImageBehavior = False
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'grpCaja
        '
        Me.grpCaja.BackColor = System.Drawing.SystemColors.Control
        Me.grpCaja.Controls.Add(Me.Label1)
        Me.grpCaja.Controls.Add(Me.cmbEjercicio)
        Me.grpCaja.Controls.Add(Me.cmbCuenta)
        Me.grpCaja.Controls.Add(Me.cmbMes)
        Me.grpCaja.Controls.Add(Me.lblejercicio)
        Me.grpCaja.Controls.Add(Me.lblcuenta)
        Me.grpCaja.Location = New System.Drawing.Point(1, 58)
        Me.grpCaja.Name = "grpCaja"
        Me.grpCaja.Size = New System.Drawing.Size(730, 63)
        Me.grpCaja.TabIndex = 82
        Me.grpCaja.TabStop = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(500, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 20)
        Me.Label1.TabIndex = 107
        Me.Label1.Text = "del mes"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbEjercicio
        '
        Me.cmbEjercicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEjercicio.FormattingEnabled = True
        Me.cmbEjercicio.Location = New System.Drawing.Point(183, 11)
        Me.cmbEjercicio.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbEjercicio.Name = "cmbEjercicio"
        Me.cmbEjercicio.Size = New System.Drawing.Size(313, 21)
        Me.cmbEjercicio.TabIndex = 106
        '
        'cmbCuenta
        '
        Me.cmbCuenta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCuenta.FormattingEnabled = True
        Me.cmbCuenta.Location = New System.Drawing.Point(183, 34)
        Me.cmbCuenta.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbCuenta.Name = "cmbCuenta"
        Me.cmbCuenta.Size = New System.Drawing.Size(313, 21)
        Me.cmbCuenta.TabIndex = 105
        '
        'cmbMes
        '
        Me.cmbMes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMes.FormattingEnabled = True
        Me.cmbMes.Location = New System.Drawing.Point(563, 34)
        Me.cmbMes.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbMes.Name = "cmbMes"
        Me.cmbMes.Size = New System.Drawing.Size(165, 21)
        Me.cmbMes.TabIndex = 104
        '
        'lblejercicio
        '
        Me.lblejercicio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblejercicio.Location = New System.Drawing.Point(6, 11)
        Me.lblejercicio.Name = "lblejercicio"
        Me.lblejercicio.Size = New System.Drawing.Size(163, 22)
        Me.lblejercicio.TabIndex = 0
        Me.lblejercicio.Text = "Ejercicio de conciliación"
        Me.lblejercicio.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblcuenta
        '
        Me.lblcuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcuenta.Location = New System.Drawing.Point(6, 33)
        Me.lblcuenta.Name = "lblcuenta"
        Me.lblcuenta.Size = New System.Drawing.Size(163, 20)
        Me.lblcuenta.TabIndex = 0
        Me.lblcuenta.Text = "Conciliación para la cuenta"
        Me.lblcuenta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSalC
        '
        Me.txtSalC.BackColor = System.Drawing.Color.MintCream
        Me.txtSalC.Location = New System.Drawing.Point(590, 55)
        Me.txtSalC.Name = "txtSalC"
        Me.txtSalC.Size = New System.Drawing.Size(134, 20)
        Me.txtSalC.TabIndex = 4
        Me.txtSalC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSalMesAct
        '
        Me.txtSalMesAct.BackColor = System.Drawing.Color.MintCream
        Me.txtSalMesAct.Location = New System.Drawing.Point(590, 99)
        Me.txtSalMesAct.Name = "txtSalMesAct"
        Me.txtSalMesAct.Size = New System.Drawing.Size(134, 20)
        Me.txtSalMesAct.TabIndex = 5
        Me.txtSalMesAct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'grpTotales
        '
        Me.grpTotales.BackColor = System.Drawing.SystemColors.Control
        Me.grpTotales.Controls.Add(Me.Label13)
        Me.grpTotales.Controls.Add(Me.txtSaldoEnLibros)
        Me.grpTotales.Controls.Add(Me.txtSaldoEnBancos)
        Me.grpTotales.Controls.Add(Me.Label11)
        Me.grpTotales.Controls.Add(Me.Label8)
        Me.grpTotales.Controls.Add(Me.Label7)
        Me.grpTotales.Controls.Add(Me.Label6)
        Me.grpTotales.Controls.Add(Me.Label5)
        Me.grpTotales.Controls.Add(Me.Label3)
        Me.grpTotales.Controls.Add(Me.Label2)
        Me.grpTotales.Controls.Add(Me.txtDBNC)
        Me.grpTotales.Controls.Add(Me.txtDBC)
        Me.grpTotales.Controls.Add(Me.txtDBMesAct)
        Me.grpTotales.Controls.Add(Me.txtDBMesAnt)
        Me.grpTotales.Controls.Add(Me.txtCRMesAct)
        Me.grpTotales.Controls.Add(Me.txtCRC)
        Me.grpTotales.Controls.Add(Me.txtSalMesAnt)
        Me.grpTotales.Controls.Add(Me.txtCRMesAnt)
        Me.grpTotales.Controls.Add(Me.txtSalC)
        Me.grpTotales.Controls.Add(Me.txtSalMesAct)
        Me.grpTotales.Controls.Add(Me.txtSalNC)
        Me.grpTotales.Controls.Add(Me.txtCRNC)
        Me.grpTotales.Controls.Add(Me.Label4)
        Me.grpTotales.Enabled = False
        Me.grpTotales.Location = New System.Drawing.Point(-1, 291)
        Me.grpTotales.Name = "grpTotales"
        Me.grpTotales.Size = New System.Drawing.Size(728, 173)
        Me.grpTotales.TabIndex = 83
        Me.grpTotales.TabStop = False
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(6, 120)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(298, 20)
        Me.Label13.TabIndex = 25
        Me.Label13.Text = "Saldo en estado de cuenta bancario debe ser"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtSaldoEnLibros
        '
        Me.txtSaldoEnLibros.BackColor = System.Drawing.Color.MintCream
        Me.txtSaldoEnLibros.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaldoEnLibros.Location = New System.Drawing.Point(590, 143)
        Me.txtSaldoEnLibros.Name = "txtSaldoEnLibros"
        Me.txtSaldoEnLibros.Size = New System.Drawing.Size(134, 20)
        Me.txtSaldoEnLibros.TabIndex = 23
        Me.txtSaldoEnLibros.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSaldoEnBancos
        '
        Me.txtSaldoEnBancos.BackColor = System.Drawing.Color.MintCream
        Me.txtSaldoEnBancos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaldoEnBancos.Location = New System.Drawing.Point(590, 121)
        Me.txtSaldoEnBancos.Name = "txtSaldoEnBancos"
        Me.txtSaldoEnBancos.Size = New System.Drawing.Size(134, 20)
        Me.txtSaldoEnBancos.TabIndex = 21
        Me.txtSaldoEnBancos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(6, 142)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(298, 20)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Saldo en libros este mes"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 98)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(298, 20)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Movimientos mes actual"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 77)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(298, 20)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "No conciliados de meses anteriores y mes actual"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(6, 55)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(298, 20)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Conciliados de meses anteriores y mes actual"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(298, 20)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Movimientos meses anteriores"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(310, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(134, 20)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Créditos"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(450, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(134, 20)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Debitos"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDBNC
        '
        Me.txtDBNC.BackColor = System.Drawing.Color.MintCream
        Me.txtDBNC.ForeColor = System.Drawing.Color.Navy
        Me.txtDBNC.Location = New System.Drawing.Point(450, 77)
        Me.txtDBNC.Name = "txtDBNC"
        Me.txtDBNC.Size = New System.Drawing.Size(134, 20)
        Me.txtDBNC.TabIndex = 13
        Me.txtDBNC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDBC
        '
        Me.txtDBC.BackColor = System.Drawing.Color.MintCream
        Me.txtDBC.ForeColor = System.Drawing.Color.Navy
        Me.txtDBC.Location = New System.Drawing.Point(450, 55)
        Me.txtDBC.Name = "txtDBC"
        Me.txtDBC.Size = New System.Drawing.Size(134, 20)
        Me.txtDBC.TabIndex = 12
        Me.txtDBC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDBMesAct
        '
        Me.txtDBMesAct.BackColor = System.Drawing.Color.MintCream
        Me.txtDBMesAct.ForeColor = System.Drawing.Color.Navy
        Me.txtDBMesAct.Location = New System.Drawing.Point(450, 99)
        Me.txtDBMesAct.Name = "txtDBMesAct"
        Me.txtDBMesAct.Size = New System.Drawing.Size(134, 20)
        Me.txtDBMesAct.TabIndex = 11
        Me.txtDBMesAct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDBMesAnt
        '
        Me.txtDBMesAnt.BackColor = System.Drawing.Color.MintCream
        Me.txtDBMesAnt.ForeColor = System.Drawing.Color.Navy
        Me.txtDBMesAnt.Location = New System.Drawing.Point(450, 33)
        Me.txtDBMesAnt.Name = "txtDBMesAnt"
        Me.txtDBMesAnt.Size = New System.Drawing.Size(134, 20)
        Me.txtDBMesAnt.TabIndex = 10
        Me.txtDBMesAnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCRMesAct
        '
        Me.txtCRMesAct.BackColor = System.Drawing.Color.MintCream
        Me.txtCRMesAct.ForeColor = System.Drawing.Color.Navy
        Me.txtCRMesAct.Location = New System.Drawing.Point(310, 99)
        Me.txtCRMesAct.Name = "txtCRMesAct"
        Me.txtCRMesAct.Size = New System.Drawing.Size(134, 20)
        Me.txtCRMesAct.TabIndex = 9
        Me.txtCRMesAct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCRC
        '
        Me.txtCRC.BackColor = System.Drawing.Color.MintCream
        Me.txtCRC.ForeColor = System.Drawing.Color.Navy
        Me.txtCRC.Location = New System.Drawing.Point(310, 55)
        Me.txtCRC.Name = "txtCRC"
        Me.txtCRC.Size = New System.Drawing.Size(134, 20)
        Me.txtCRC.TabIndex = 8
        Me.txtCRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSalMesAnt
        '
        Me.txtSalMesAnt.BackColor = System.Drawing.Color.MintCream
        Me.txtSalMesAnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSalMesAnt.ForeColor = System.Drawing.Color.Navy
        Me.txtSalMesAnt.Location = New System.Drawing.Point(590, 33)
        Me.txtSalMesAnt.Name = "txtSalMesAnt"
        Me.txtSalMesAnt.Size = New System.Drawing.Size(134, 20)
        Me.txtSalMesAnt.TabIndex = 8
        Me.txtSalMesAnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCRMesAnt
        '
        Me.txtCRMesAnt.BackColor = System.Drawing.Color.MintCream
        Me.txtCRMesAnt.Location = New System.Drawing.Point(310, 33)
        Me.txtCRMesAnt.Name = "txtCRMesAnt"
        Me.txtCRMesAnt.Size = New System.Drawing.Size(134, 20)
        Me.txtCRMesAnt.TabIndex = 4
        Me.txtCRMesAnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSalNC
        '
        Me.txtSalNC.BackColor = System.Drawing.Color.MintCream
        Me.txtSalNC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSalNC.ForeColor = System.Drawing.Color.Navy
        Me.txtSalNC.Location = New System.Drawing.Point(590, 77)
        Me.txtSalNC.Name = "txtSalNC"
        Me.txtSalNC.Size = New System.Drawing.Size(134, 20)
        Me.txtSalNC.TabIndex = 6
        Me.txtSalNC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCRNC
        '
        Me.txtCRNC.BackColor = System.Drawing.Color.MintCream
        Me.txtCRNC.ForeColor = System.Drawing.Color.Navy
        Me.txtCRNC.Location = New System.Drawing.Point(310, 77)
        Me.txtCRNC.Name = "txtCRNC"
        Me.txtCRNC.Size = New System.Drawing.Size(134, 20)
        Me.txtCRNC.TabIndex = 5
        Me.txtCRNC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(590, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(134, 20)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Saldo"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(566, 464)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 85
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
        'C1PictureBox1
        '
        Me.C1PictureBox1.Image = Global.Datum.My.Resources.Resources.banda_amarilla
        Me.C1PictureBox1.Location = New System.Drawing.Point(92, 1)
        Me.C1PictureBox1.Name = "C1PictureBox1"
        Me.C1PictureBox1.Size = New System.Drawing.Size(639, 61)
        Me.C1PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.C1PictureBox1.TabIndex = 86
        Me.C1PictureBox1.TabStop = False
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(1, 41)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(230, 21)
        Me.Label9.TabIndex = 87
        Me.Label9.Text = "Bancos: Conciliación Bancaria"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(216, Byte), Integer), CType(CType(22, Byte), Integer))
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label10.Font = New System.Drawing.Font("Consolas", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(1, 1)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(230, 40)
        Me.Label10.TabIndex = 88
        Me.Label10.Text = "Datum"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'jsBanProConciliacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(732, 491)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpCaja)
        Me.Controls.Add(Me.grpTotales)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsBanProConciliacion"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Conciliación bancaria"
        Me.grpCaja.ResumeLayout(False)
        Me.grpTotales.ResumeLayout(False)
        Me.grpTotales.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents lv As System.Windows.Forms.ListView
    Friend WithEvents grpCaja As System.Windows.Forms.GroupBox
    Friend WithEvents lblejercicio As System.Windows.Forms.Label
    Friend WithEvents grpTotales As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblcuenta As System.Windows.Forms.Label
    Friend WithEvents txtSalMesAct As System.Windows.Forms.TextBox
    Friend WithEvents txtSalC As System.Windows.Forms.TextBox
    Friend WithEvents txtSalNC As System.Windows.Forms.TextBox
    Friend WithEvents txtCRNC As System.Windows.Forms.TextBox
    Friend WithEvents txtSalMesAnt As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cmbEjercicio As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCuenta As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMes As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCRC As System.Windows.Forms.TextBox
    Friend WithEvents txtCRMesAnt As System.Windows.Forms.TextBox
    Friend WithEvents txtDBNC As System.Windows.Forms.TextBox
    Friend WithEvents txtDBC As System.Windows.Forms.TextBox
    Friend WithEvents txtDBMesAct As System.Windows.Forms.TextBox
    Friend WithEvents txtDBMesAnt As System.Windows.Forms.TextBox
    Friend WithEvents txtCRMesAct As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSaldoEnLibros As System.Windows.Forms.TextBox
    Friend WithEvents txtSaldoEnBancos As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
End Class
