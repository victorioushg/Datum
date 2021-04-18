<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsMerProCombos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsMerProCombos))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpCaja = New System.Windows.Forms.GroupBox()
        Me.lblCantidadMaxima = New System.Windows.Forms.Label()
        Me.txtCantidad = New System.Windows.Forms.TextBox()
        Me.lblCANTIDAD = New System.Windows.Forms.Label()
        Me.dg = New System.Windows.Forms.DataGridView()
        Me.lblALMACEN = New System.Windows.Forms.Label()
        Me.lblNOMBRE = New System.Windows.Forms.Label()
        Me.btnAlmacen = New System.Windows.Forms.Button()
        Me.txtAlmacen = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnCodigo = New System.Windows.Forms.Button()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.lblcuenta = New System.Windows.Forms.Label()
        Me.grpTotales = New System.Windows.Forms.GroupBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblProgreso = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1PictureBox1 = New C1.Win.C1Input.C1PictureBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblLeyenda = New System.Windows.Forms.Label()
        Me.grpLeyenda = New System.Windows.Forms.GroupBox()
        Me.lblUNIDAD = New System.Windows.Forms.Label()
        Me.grpCaja.SuspendLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTotales.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpLeyenda.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 411)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(833, 27)
        Me.lblInfo.TabIndex = 80
        '
        'grpCaja
        '
        Me.grpCaja.BackColor = System.Drawing.SystemColors.Control
        Me.grpCaja.Controls.Add(Me.lblUNIDAD)
        Me.grpCaja.Controls.Add(Me.lblCantidadMaxima)
        Me.grpCaja.Controls.Add(Me.txtCantidad)
        Me.grpCaja.Controls.Add(Me.lblCANTIDAD)
        Me.grpCaja.Controls.Add(Me.dg)
        Me.grpCaja.Controls.Add(Me.lblALMACEN)
        Me.grpCaja.Controls.Add(Me.lblNOMBRE)
        Me.grpCaja.Controls.Add(Me.btnAlmacen)
        Me.grpCaja.Controls.Add(Me.txtAlmacen)
        Me.grpCaja.Controls.Add(Me.Label2)
        Me.grpCaja.Controls.Add(Me.btnCodigo)
        Me.grpCaja.Controls.Add(Me.txtCodigo)
        Me.grpCaja.Controls.Add(Me.lblcuenta)
        Me.grpCaja.Location = New System.Drawing.Point(1, 125)
        Me.grpCaja.Name = "grpCaja"
        Me.grpCaja.Size = New System.Drawing.Size(832, 222)
        Me.grpCaja.TabIndex = 82
        Me.grpCaja.TabStop = False
        '
        'lblCantidadMaxima
        '
        Me.lblCantidadMaxima.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidadMaxima.Location = New System.Drawing.Point(360, 71)
        Me.lblCantidadMaxima.Name = "lblCantidadMaxima"
        Me.lblCantidadMaxima.Size = New System.Drawing.Size(466, 20)
        Me.lblCantidadMaxima.TabIndex = 138
        Me.lblCantidadMaxima.Text = "Cantidad maxima para construir "
        Me.lblCantidadMaxima.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCantidad
        '
        Me.txtCantidad.Location = New System.Drawing.Point(150, 57)
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(109, 20)
        Me.txtCantidad.TabIndex = 137
        Me.txtCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblCANTIDAD
        '
        Me.lblCANTIDAD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCANTIDAD.Location = New System.Drawing.Point(8, 55)
        Me.lblCANTIDAD.Name = "lblCANTIDAD"
        Me.lblCANTIDAD.Size = New System.Drawing.Size(136, 20)
        Me.lblCANTIDAD.TabIndex = 136
        Me.lblCANTIDAD.Text = "Cantidad a construir"
        Me.lblCANTIDAD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dg
        '
        Me.dg.AllowUserToAddRows = False
        Me.dg.AllowUserToDeleteRows = False
        Me.dg.AllowUserToOrderColumns = True
        Me.dg.AllowUserToResizeColumns = False
        Me.dg.AllowUserToResizeRows = False
        Me.dg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dg.Location = New System.Drawing.Point(1, 94)
        Me.dg.Name = "dg"
        Me.dg.ReadOnly = True
        Me.dg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg.Size = New System.Drawing.Size(829, 122)
        Me.dg.TabIndex = 135
        '
        'lblALMACEN
        '
        Me.lblALMACEN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblALMACEN.Location = New System.Drawing.Point(296, 39)
        Me.lblALMACEN.Name = "lblALMACEN"
        Me.lblALMACEN.Size = New System.Drawing.Size(423, 17)
        Me.lblALMACEN.TabIndex = 134
        '
        'lblNOMBRE
        '
        Me.lblNOMBRE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNOMBRE.Location = New System.Drawing.Point(296, 18)
        Me.lblNOMBRE.Name = "lblNOMBRE"
        Me.lblNOMBRE.Size = New System.Drawing.Size(423, 17)
        Me.lblNOMBRE.TabIndex = 132
        '
        'btnAlmacen
        '
        Me.btnAlmacen.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAlmacen.Location = New System.Drawing.Point(265, 35)
        Me.btnAlmacen.Name = "btnAlmacen"
        Me.btnAlmacen.Size = New System.Drawing.Size(25, 20)
        Me.btnAlmacen.TabIndex = 131
        Me.btnAlmacen.Text = "•••"
        Me.btnAlmacen.UseVisualStyleBackColor = True
        '
        'txtAlmacen
        '
        Me.txtAlmacen.Location = New System.Drawing.Point(150, 36)
        Me.txtAlmacen.Name = "txtAlmacen"
        Me.txtAlmacen.Size = New System.Drawing.Size(109, 20)
        Me.txtAlmacen.TabIndex = 122
        Me.txtAlmacen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(136, 20)
        Me.Label2.TabIndex = 118
        Me.Label2.Text = "Almacén"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnCodigo
        '
        Me.btnCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCodigo.Location = New System.Drawing.Point(265, 15)
        Me.btnCodigo.Name = "btnCodigo"
        Me.btnCodigo.Size = New System.Drawing.Size(25, 20)
        Me.btnCodigo.TabIndex = 116
        Me.btnCodigo.Text = "•••"
        Me.btnCodigo.UseVisualStyleBackColor = True
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(150, 15)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(109, 20)
        Me.txtCodigo.TabIndex = 115
        Me.txtCodigo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblcuenta
        '
        Me.lblcuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblcuenta.Location = New System.Drawing.Point(14, 15)
        Me.lblcuenta.Name = "lblcuenta"
        Me.lblcuenta.Size = New System.Drawing.Size(130, 20)
        Me.lblcuenta.TabIndex = 0
        Me.lblcuenta.Text = "Código combo"
        Me.lblcuenta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpTotales
        '
        Me.grpTotales.BackColor = System.Drawing.SystemColors.Control
        Me.grpTotales.Controls.Add(Me.ProgressBar1)
        Me.grpTotales.Controls.Add(Me.Label3)
        Me.grpTotales.Controls.Add(Me.lblProgreso)
        Me.grpTotales.Enabled = False
        Me.grpTotales.Location = New System.Drawing.Point(1, 334)
        Me.grpTotales.Name = "grpTotales"
        Me.grpTotales.Size = New System.Drawing.Size(830, 73)
        Me.grpTotales.TabIndex = 83
        Me.grpTotales.TabStop = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(9, 39)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(811, 20)
        Me.ProgressBar1.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 20)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Progreso ..."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblProgreso
        '
        Me.lblProgreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgreso.Location = New System.Drawing.Point(84, 16)
        Me.lblProgreso.Name = "lblProgreso"
        Me.lblProgreso.Size = New System.Drawing.Size(635, 20)
        Me.lblProgreso.TabIndex = 14
        Me.lblProgreso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(670, 410)
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
        Me.C1PictureBox1.Location = New System.Drawing.Point(192, 1)
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
        Me.Label9.Size = New System.Drawing.Size(259, 21)
        Me.Label9.TabIndex = 87
        Me.Label9.Tag = "Cargar conteo de mercancías en inventario"
        Me.Label9.Text = "Cargar conteo de mercancías en inventario"
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
        Me.Label10.Size = New System.Drawing.Size(259, 40)
        Me.Label10.TabIndex = 88
        Me.Label10.Text = "Datum"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLeyenda
        '
        Me.lblLeyenda.BackColor = System.Drawing.Color.White
        Me.lblLeyenda.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblLeyenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeyenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(71, Byte), Integer), CType(CType(182, Byte), Integer))
        Me.lblLeyenda.Location = New System.Drawing.Point(0, 16)
        Me.lblLeyenda.Name = "lblLeyenda"
        Me.lblLeyenda.Size = New System.Drawing.Size(829, 46)
        Me.lblLeyenda.TabIndex = 89
        Me.lblLeyenda.Text = "Este proceso construye los combos de mercancías a partir de los inventarios de un" & _
    "a mercancía escogida y de las existencias de sus componentes en el almacén desea" & _
    "do"
        '
        'grpLeyenda
        '
        Me.grpLeyenda.BackColor = System.Drawing.SystemColors.Control
        Me.grpLeyenda.Controls.Add(Me.lblLeyenda)
        Me.grpLeyenda.Location = New System.Drawing.Point(2, 60)
        Me.grpLeyenda.Name = "grpLeyenda"
        Me.grpLeyenda.Size = New System.Drawing.Size(829, 77)
        Me.grpLeyenda.TabIndex = 90
        Me.grpLeyenda.TabStop = False
        '
        'lblUNIDAD
        '
        Me.lblUNIDAD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUNIDAD.Location = New System.Drawing.Point(262, 58)
        Me.lblUNIDAD.Name = "lblUNIDAD"
        Me.lblUNIDAD.Size = New System.Drawing.Size(130, 19)
        Me.lblUNIDAD.TabIndex = 139
        Me.lblUNIDAD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'jsMerProCombos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(833, 438)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpLeyenda)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.C1PictureBox1)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.grpCaja)
        Me.Controls.Add(Me.grpTotales)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsMerProCombos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Construcción de combos de mercancías"
        Me.Text = "Construcción de combos de mercancías"
        Me.grpCaja.ResumeLayout(False)
        Me.grpCaja.PerformLayout()
        CType(Me.dg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTotales.ResumeLayout(False)
        Me.grpAceptarSalir.ResumeLayout(False)
        CType(Me.C1PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpLeyenda.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpCaja As System.Windows.Forms.GroupBox
    Friend WithEvents grpTotales As System.Windows.Forms.GroupBox
    Friend WithEvents lblcuenta As System.Windows.Forms.Label
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblProgreso As System.Windows.Forms.Label
    Friend WithEvents C1PictureBox1 As C1.Win.C1Input.C1PictureBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblLeyenda As System.Windows.Forms.Label
    Friend WithEvents grpLeyenda As System.Windows.Forms.GroupBox
    Friend WithEvents btnCodigo As System.Windows.Forms.Button
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtAlmacen As System.Windows.Forms.TextBox
    Friend WithEvents btnAlmacen As System.Windows.Forms.Button
    Friend WithEvents lblNOMBRE As System.Windows.Forms.Label
    Friend WithEvents lblALMACEN As System.Windows.Forms.Label
    Friend WithEvents lblCANTIDAD As System.Windows.Forms.Label
    Friend WithEvents dg As System.Windows.Forms.DataGridView
    Friend WithEvents txtCantidad As System.Windows.Forms.TextBox
    Friend WithEvents lblCantidadMaxima As System.Windows.Forms.Label
    Friend WithEvents lblUNIDAD As System.Windows.Forms.Label
End Class
