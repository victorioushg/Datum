<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmServidorValidacion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmServidorValidacion))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpLocal = New System.Windows.Forms.GroupBox()
        Me.btnHaciaRemoto = New System.Windows.Forms.Button()
        Me.cmbLicenciaLocal = New System.Windows.Forms.ComboBox()
        Me.cmbAplicacionLocal = New System.Windows.Forms.ComboBox()
        Me.btnExpiracionLocal = New System.Windows.Forms.Button()
        Me.txtExpiracionLocal = New System.Windows.Forms.TextBox()
        Me.txtMACLocal = New System.Windows.Forms.TextBox()
        Me.txtNumLicenciaLocal = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNombreLocal = New System.Windows.Forms.TextBox()
        Me.txtCodigoLocal = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpRemoto = New System.Windows.Forms.GroupBox()
        Me.cmbLicenciaRemoto = New System.Windows.Forms.ComboBox()
        Me.cmbaplicacionRemoto = New System.Windows.Forms.ComboBox()
        Me.txtExpiracionRemoto = New System.Windows.Forms.TextBox()
        Me.txtMACRemoto = New System.Windows.Forms.TextBox()
        Me.txtNumLicenciaRemoto = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtNombreRemoto = New System.Windows.Forms.TextBox()
        Me.txtCodigoRemoto = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.grpLocal.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.grpRemoto.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 214)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(915, 29)
        Me.lblInfo.TabIndex = 79
        '
        'grpLocal
        '
        Me.grpLocal.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpLocal.Controls.Add(Me.btnHaciaRemoto)
        Me.grpLocal.Controls.Add(Me.cmbLicenciaLocal)
        Me.grpLocal.Controls.Add(Me.cmbAplicacionLocal)
        Me.grpLocal.Controls.Add(Me.btnExpiracionLocal)
        Me.grpLocal.Controls.Add(Me.txtExpiracionLocal)
        Me.grpLocal.Controls.Add(Me.txtMACLocal)
        Me.grpLocal.Controls.Add(Me.txtNumLicenciaLocal)
        Me.grpLocal.Controls.Add(Me.Label7)
        Me.grpLocal.Controls.Add(Me.Label6)
        Me.grpLocal.Controls.Add(Me.Label5)
        Me.grpLocal.Controls.Add(Me.Label4)
        Me.grpLocal.Controls.Add(Me.Label3)
        Me.grpLocal.Controls.Add(Me.txtNombreLocal)
        Me.grpLocal.Controls.Add(Me.txtCodigoLocal)
        Me.grpLocal.Controls.Add(Me.Label2)
        Me.grpLocal.Controls.Add(Me.Label1)
        Me.grpLocal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpLocal.Location = New System.Drawing.Point(0, 1)
        Me.grpLocal.Name = "grpLocal"
        Me.grpLocal.Size = New System.Drawing.Size(451, 210)
        Me.grpLocal.TabIndex = 80
        Me.grpLocal.TabStop = False
        Me.grpLocal.Text = "Servidor Local"
        '
        'btnHaciaRemoto
        '
        Me.btnHaciaRemoto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHaciaRemoto.Location = New System.Drawing.Point(411, 92)
        Me.btnHaciaRemoto.Name = "btnHaciaRemoto"
        Me.btnHaciaRemoto.Size = New System.Drawing.Size(34, 26)
        Me.btnHaciaRemoto.TabIndex = 141
        Me.btnHaciaRemoto.Text = "--->"
        Me.btnHaciaRemoto.UseVisualStyleBackColor = True
        '
        'cmbLicenciaLocal
        '
        Me.cmbLicenciaLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLicenciaLocal.FormattingEnabled = True
        Me.cmbLicenciaLocal.Location = New System.Drawing.Point(137, 105)
        Me.cmbLicenciaLocal.Name = "cmbLicenciaLocal"
        Me.cmbLicenciaLocal.Size = New System.Drawing.Size(152, 21)
        Me.cmbLicenciaLocal.TabIndex = 144
        '
        'cmbAplicacionLocal
        '
        Me.cmbAplicacionLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAplicacionLocal.FormattingEnabled = True
        Me.cmbAplicacionLocal.Location = New System.Drawing.Point(137, 83)
        Me.cmbAplicacionLocal.Name = "cmbAplicacionLocal"
        Me.cmbAplicacionLocal.Size = New System.Drawing.Size(152, 21)
        Me.cmbAplicacionLocal.TabIndex = 143
        '
        'btnExpiracionLocal
        '
        Me.btnExpiracionLocal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExpiracionLocal.Location = New System.Drawing.Point(262, 169)
        Me.btnExpiracionLocal.Name = "btnExpiracionLocal"
        Me.btnExpiracionLocal.Size = New System.Drawing.Size(27, 20)
        Me.btnExpiracionLocal.TabIndex = 142
        Me.btnExpiracionLocal.Text = "···"
        Me.btnExpiracionLocal.UseVisualStyleBackColor = True
        '
        'txtExpiracionLocal
        '
        Me.txtExpiracionLocal.BackColor = System.Drawing.Color.White
        Me.txtExpiracionLocal.Enabled = False
        Me.txtExpiracionLocal.Location = New System.Drawing.Point(137, 169)
        Me.txtExpiracionLocal.Name = "txtExpiracionLocal"
        Me.txtExpiracionLocal.Size = New System.Drawing.Size(119, 20)
        Me.txtExpiracionLocal.TabIndex = 10
        '
        'txtMACLocal
        '
        Me.txtMACLocal.BackColor = System.Drawing.Color.White
        Me.txtMACLocal.Enabled = False
        Me.txtMACLocal.Location = New System.Drawing.Point(137, 148)
        Me.txtMACLocal.Name = "txtMACLocal"
        Me.txtMACLocal.Size = New System.Drawing.Size(275, 20)
        Me.txtMACLocal.TabIndex = 9
        '
        'txtNumLicenciaLocal
        '
        Me.txtNumLicenciaLocal.BackColor = System.Drawing.Color.White
        Me.txtNumLicenciaLocal.Location = New System.Drawing.Point(137, 127)
        Me.txtNumLicenciaLocal.Name = "txtNumLicenciaLocal"
        Me.txtNumLicenciaLocal.Size = New System.Drawing.Size(275, 20)
        Me.txtNumLicenciaLocal.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(13, 167)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(118, 22)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Fecha expiración :"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(13, 148)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(118, 19)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "MAC :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(13, 127)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(118, 19)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Licencia  :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(13, 105)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 19)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Tipo licencia  :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 19)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Tipo aplicación  :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNombreLocal
        '
        Me.txtNombreLocal.BackColor = System.Drawing.Color.White
        Me.txtNombreLocal.Location = New System.Drawing.Point(137, 36)
        Me.txtNombreLocal.Multiline = True
        Me.txtNombreLocal.Name = "txtNombreLocal"
        Me.txtNombreLocal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNombreLocal.Size = New System.Drawing.Size(308, 46)
        Me.txtNombreLocal.TabIndex = 1
        '
        'txtCodigoLocal
        '
        Me.txtCodigoLocal.BackColor = System.Drawing.Color.White
        Me.txtCodigoLocal.Location = New System.Drawing.Point(137, 15)
        Me.txtCodigoLocal.Name = "txtCodigoLocal"
        Me.txtCodigoLocal.Size = New System.Drawing.Size(152, 20)
        Me.txtCodigoLocal.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 19)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Empresa :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 19)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Código  :"
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
        Me.grpAceptarSalir.Location = New System.Drawing.Point(750, 214)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 88
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
        'grpRemoto
        '
        Me.grpRemoto.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpRemoto.Controls.Add(Me.cmbLicenciaRemoto)
        Me.grpRemoto.Controls.Add(Me.cmbaplicacionRemoto)
        Me.grpRemoto.Controls.Add(Me.txtExpiracionRemoto)
        Me.grpRemoto.Controls.Add(Me.txtMACRemoto)
        Me.grpRemoto.Controls.Add(Me.txtNumLicenciaRemoto)
        Me.grpRemoto.Controls.Add(Me.Label8)
        Me.grpRemoto.Controls.Add(Me.Label9)
        Me.grpRemoto.Controls.Add(Me.Label10)
        Me.grpRemoto.Controls.Add(Me.Label11)
        Me.grpRemoto.Controls.Add(Me.Label12)
        Me.grpRemoto.Controls.Add(Me.txtNombreRemoto)
        Me.grpRemoto.Controls.Add(Me.txtCodigoRemoto)
        Me.grpRemoto.Controls.Add(Me.Label13)
        Me.grpRemoto.Controls.Add(Me.Label14)
        Me.grpRemoto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpRemoto.Location = New System.Drawing.Point(452, 1)
        Me.grpRemoto.Name = "grpRemoto"
        Me.grpRemoto.Size = New System.Drawing.Size(451, 210)
        Me.grpRemoto.TabIndex = 89
        Me.grpRemoto.TabStop = False
        Me.grpRemoto.Text = "Servidor Remoto"
        '
        'cmbLicenciaRemoto
        '
        Me.cmbLicenciaRemoto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLicenciaRemoto.FormattingEnabled = True
        Me.cmbLicenciaRemoto.Location = New System.Drawing.Point(137, 105)
        Me.cmbLicenciaRemoto.Name = "cmbLicenciaRemoto"
        Me.cmbLicenciaRemoto.Size = New System.Drawing.Size(152, 21)
        Me.cmbLicenciaRemoto.TabIndex = 144
        '
        'cmbaplicacionRemoto
        '
        Me.cmbaplicacionRemoto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbaplicacionRemoto.FormattingEnabled = True
        Me.cmbaplicacionRemoto.Location = New System.Drawing.Point(137, 83)
        Me.cmbaplicacionRemoto.Name = "cmbaplicacionRemoto"
        Me.cmbaplicacionRemoto.Size = New System.Drawing.Size(152, 21)
        Me.cmbaplicacionRemoto.TabIndex = 143
        '
        'txtExpiracionRemoto
        '
        Me.txtExpiracionRemoto.BackColor = System.Drawing.Color.White
        Me.txtExpiracionRemoto.Enabled = False
        Me.txtExpiracionRemoto.Location = New System.Drawing.Point(137, 169)
        Me.txtExpiracionRemoto.Name = "txtExpiracionRemoto"
        Me.txtExpiracionRemoto.Size = New System.Drawing.Size(119, 20)
        Me.txtExpiracionRemoto.TabIndex = 10
        '
        'txtMACRemoto
        '
        Me.txtMACRemoto.BackColor = System.Drawing.Color.White
        Me.txtMACRemoto.Enabled = False
        Me.txtMACRemoto.Location = New System.Drawing.Point(137, 148)
        Me.txtMACRemoto.Name = "txtMACRemoto"
        Me.txtMACRemoto.Size = New System.Drawing.Size(275, 20)
        Me.txtMACRemoto.TabIndex = 9
        '
        'txtNumLicenciaRemoto
        '
        Me.txtNumLicenciaRemoto.BackColor = System.Drawing.Color.White
        Me.txtNumLicenciaRemoto.Enabled = False
        Me.txtNumLicenciaRemoto.Location = New System.Drawing.Point(137, 127)
        Me.txtNumLicenciaRemoto.Name = "txtNumLicenciaRemoto"
        Me.txtNumLicenciaRemoto.Size = New System.Drawing.Size(275, 20)
        Me.txtNumLicenciaRemoto.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(13, 167)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(118, 22)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "Fecha expiración :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(13, 148)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(118, 19)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "MAC :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(13, 127)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(118, 19)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Licencia  :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(13, 105)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(118, 19)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Tipo licencia  :"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(13, 83)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(118, 19)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Tipo aplicación  :"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtNombreRemoto
        '
        Me.txtNombreRemoto.BackColor = System.Drawing.Color.White
        Me.txtNombreRemoto.Location = New System.Drawing.Point(137, 36)
        Me.txtNombreRemoto.Multiline = True
        Me.txtNombreRemoto.Name = "txtNombreRemoto"
        Me.txtNombreRemoto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNombreRemoto.Size = New System.Drawing.Size(308, 46)
        Me.txtNombreRemoto.TabIndex = 1
        '
        'txtCodigoRemoto
        '
        Me.txtCodigoRemoto.BackColor = System.Drawing.Color.White
        Me.txtCodigoRemoto.Enabled = False
        Me.txtCodigoRemoto.Location = New System.Drawing.Point(137, 15)
        Me.txtCodigoRemoto.Name = "txtCodigoRemoto"
        Me.txtCodigoRemoto.Size = New System.Drawing.Size(152, 20)
        Me.txtCodigoRemoto.TabIndex = 0
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(13, 36)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(118, 19)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Empresa :"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(13, 15)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(118, 19)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "Código  :"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmServidorValidacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(915, 243)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpRemoto)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpLocal)
        Me.Controls.Add(Me.lblInfo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmServidorValidacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Validación servidor"
        Me.grpLocal.ResumeLayout(False)
        Me.grpLocal.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.grpRemoto.ResumeLayout(False)
        Me.grpRemoto.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpLocal As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNombreLocal As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigoLocal As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtExpiracionLocal As System.Windows.Forms.TextBox
    Friend WithEvents txtMACLocal As System.Windows.Forms.TextBox
    Friend WithEvents txtNumLicenciaLocal As System.Windows.Forms.TextBox
    Friend WithEvents btnExpiracionLocal As System.Windows.Forms.Button
    Friend WithEvents btnHaciaRemoto As System.Windows.Forms.Button
    Friend WithEvents cmbLicenciaLocal As System.Windows.Forms.ComboBox
    Friend WithEvents cmbAplicacionLocal As System.Windows.Forms.ComboBox
    Friend WithEvents grpRemoto As System.Windows.Forms.GroupBox
    Friend WithEvents cmbLicenciaRemoto As System.Windows.Forms.ComboBox
    Friend WithEvents cmbaplicacionRemoto As System.Windows.Forms.ComboBox
    Friend WithEvents txtExpiracionRemoto As System.Windows.Forms.TextBox
    Friend WithEvents txtMACRemoto As System.Windows.Forms.TextBox
    Friend WithEvents txtNumLicenciaRemoto As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtNombreRemoto As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigoRemoto As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
End Class
