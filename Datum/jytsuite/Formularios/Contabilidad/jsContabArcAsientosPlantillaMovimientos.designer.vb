<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsContabArcAsientosPlantillaMovimientos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsContabArcAsientosPlantillaMovimientos))
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.txtCodigoCuenta = New System.Windows.Forms.TextBox()
        Me.txtReferencia = New System.Windows.Forms.TextBox()
        Me.txtConcepto = New System.Windows.Forms.TextBox()
        Me.txtPlantilla = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnCuentas = New System.Windows.Forms.Button()
        Me.lblCuenta = New System.Windows.Forms.Label()
        Me.btnPlantilla = New System.Windows.Forms.Button()
        Me.txtDescripcionPlantilla = New System.Windows.Forms.TextBox()
        Me.cmbSigno = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnReferencia = New System.Windows.Forms.Button()
        Me.btnConcepto = New System.Windows.Forms.Button()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(489, 201)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 78
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
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 203)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(654, 28)
        Me.lblInfo.TabIndex = 79
        '
        'txtCodigoCuenta
        '
        Me.txtCodigoCuenta.Location = New System.Drawing.Point(127, 12)
        Me.txtCodigoCuenta.Name = "txtCodigoCuenta"
        Me.txtCodigoCuenta.Size = New System.Drawing.Size(155, 20)
        Me.txtCodigoCuenta.TabIndex = 80
        '
        'txtReferencia
        '
        Me.txtReferencia.Location = New System.Drawing.Point(127, 82)
        Me.txtReferencia.MaxLength = 50
        Me.txtReferencia.Name = "txtReferencia"
        Me.txtReferencia.Size = New System.Drawing.Size(523, 20)
        Me.txtReferencia.TabIndex = 81
        '
        'txtConcepto
        '
        Me.txtConcepto.Location = New System.Drawing.Point(127, 103)
        Me.txtConcepto.MaxLength = 250
        Me.txtConcepto.Multiline = True
        Me.txtConcepto.Name = "txtConcepto"
        Me.txtConcepto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtConcepto.Size = New System.Drawing.Size(523, 56)
        Me.txtConcepto.TabIndex = 82
        '
        'txtPlantilla
        '
        Me.txtPlantilla.Location = New System.Drawing.Point(127, 61)
        Me.txtPlantilla.MaxLength = 19
        Me.txtPlantilla.Name = "txtPlantilla"
        Me.txtPlantilla.Size = New System.Drawing.Size(72, 20)
        Me.txtPlantilla.TabIndex = 83
        Me.txtPlantilla.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 17)
        Me.Label1.TabIndex = 85
        Me.Label1.Text = "Cuenta Contable"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 17)
        Me.Label2.TabIndex = 86
        Me.Label2.Text = "Referencia"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 20)
        Me.Label4.TabIndex = 88
        Me.Label4.Text = "Concepto"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(14, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 17)
        Me.Label5.TabIndex = 89
        Me.Label5.Text = "Plantilla"
        '
        'btnCuentas
        '
        Me.btnCuentas.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCuentas.Location = New System.Drawing.Point(292, 12)
        Me.btnCuentas.Name = "btnCuentas"
        Me.btnCuentas.Size = New System.Drawing.Size(25, 20)
        Me.btnCuentas.TabIndex = 103
        Me.btnCuentas.Text = "•••"
        Me.btnCuentas.UseVisualStyleBackColor = True
        '
        'lblCuenta
        '
        Me.lblCuenta.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCuenta.Location = New System.Drawing.Point(323, 12)
        Me.lblCuenta.Name = "lblCuenta"
        Me.lblCuenta.Size = New System.Drawing.Size(327, 39)
        Me.lblCuenta.TabIndex = 104
        '
        'btnPlantilla
        '
        Me.btnPlantilla.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlantilla.Location = New System.Drawing.Point(205, 61)
        Me.btnPlantilla.Name = "btnPlantilla"
        Me.btnPlantilla.Size = New System.Drawing.Size(25, 20)
        Me.btnPlantilla.TabIndex = 105
        Me.btnPlantilla.Text = "•••"
        Me.btnPlantilla.UseVisualStyleBackColor = True
        '
        'txtDescripcionPlantilla
        '
        Me.txtDescripcionPlantilla.Location = New System.Drawing.Point(236, 61)
        Me.txtDescripcionPlantilla.MaxLength = 19
        Me.txtDescripcionPlantilla.Name = "txtDescripcionPlantilla"
        Me.txtDescripcionPlantilla.Size = New System.Drawing.Size(414, 20)
        Me.txtDescripcionPlantilla.TabIndex = 106
        '
        'cmbSigno
        '
        Me.cmbSigno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSigno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSigno.FormattingEnabled = True
        Me.cmbSigno.Location = New System.Drawing.Point(127, 163)
        Me.cmbSigno.Margin = New System.Windows.Forms.Padding(1)
        Me.cmbSigno.Name = "cmbSigno"
        Me.cmbSigno.Size = New System.Drawing.Size(103, 21)
        Me.cmbSigno.TabIndex = 122
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 167)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(109, 17)
        Me.Label3.TabIndex = 123
        Me.Label3.Text = "Signo"
        '
        'btnReferencia
        '
        Me.btnReferencia.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReferencia.Location = New System.Drawing.Point(98, 81)
        Me.btnReferencia.Name = "btnReferencia"
        Me.btnReferencia.Size = New System.Drawing.Size(25, 20)
        Me.btnReferencia.TabIndex = 124
        Me.btnReferencia.Text = "•••"
        Me.btnReferencia.UseVisualStyleBackColor = True
        '
        'btnConcepto
        '
        Me.btnConcepto.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConcepto.Location = New System.Drawing.Point(98, 103)
        Me.btnConcepto.Name = "btnConcepto"
        Me.btnConcepto.Size = New System.Drawing.Size(25, 20)
        Me.btnConcepto.TabIndex = 125
        Me.btnConcepto.Text = "•••"
        Me.btnConcepto.UseVisualStyleBackColor = True
        '
        'jsContabArcAsientosPlantillaMovimientos
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(654, 231)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnConcepto)
        Me.Controls.Add(Me.btnReferencia)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmbSigno)
        Me.Controls.Add(Me.txtDescripcionPlantilla)
        Me.Controls.Add(Me.btnPlantilla)
        Me.Controls.Add(Me.lblCuenta)
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.btnCuentas)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtPlantilla)
        Me.Controls.Add(Me.txtConcepto)
        Me.Controls.Add(Me.txtReferencia)
        Me.Controls.Add(Me.txtCodigoCuenta)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsContabArcAsientosPlantillaMovimientos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Tag = "Movimiento plantilla contable"
        Me.Text = "Movimiento plantilla contable"
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents txtCodigoCuenta As System.Windows.Forms.TextBox
    Friend WithEvents txtReferencia As System.Windows.Forms.TextBox
    Friend WithEvents txtConcepto As System.Windows.Forms.TextBox
    Friend WithEvents txtPlantilla As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnCuentas As System.Windows.Forms.Button
    Friend WithEvents lblCuenta As System.Windows.Forms.Label
    Friend WithEvents btnPlantilla As System.Windows.Forms.Button
    Friend WithEvents txtDescripcionPlantilla As System.Windows.Forms.TextBox
    Friend WithEvents cmbSigno As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnReferencia As System.Windows.Forms.Button
    Friend WithEvents btnConcepto As System.Windows.Forms.Button
End Class
