<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsMerArcOfertasMovimientosBono
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsMerArcOfertasMovimientosBono))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.cmbOtorgante = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbUnidadBono = New System.Windows.Forms.ComboBox()
        Me.txtDescripcionCada = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCodigoCada = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCantidadCada = New System.Windows.Forms.TextBox()
        Me.txtUnidadCada = New System.Windows.Forms.TextBox()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCantidad = New System.Windows.Forms.TextBox()
        Me.btnCodigoBono = New System.Windows.Forms.Button()
        Me.txtUnidad = New System.Windows.Forms.TextBox()
        Me.txtCantidadBono = New System.Windows.Forms.TextBox()
        Me.txtDescripcionBono = New System.Windows.Forms.TextBox()
        Me.txtCodigoBono = New System.Windows.Forms.TextBox()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.grpTarjeta.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 326)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(540, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.cmbOtorgante)
        Me.grpTarjeta.Controls.Add(Me.Label5)
        Me.grpTarjeta.Controls.Add(Me.cmbUnidadBono)
        Me.grpTarjeta.Controls.Add(Me.txtDescripcionCada)
        Me.grpTarjeta.Controls.Add(Me.Label3)
        Me.grpTarjeta.Controls.Add(Me.txtCodigoCada)
        Me.grpTarjeta.Controls.Add(Me.Label4)
        Me.grpTarjeta.Controls.Add(Me.txtCantidadCada)
        Me.grpTarjeta.Controls.Add(Me.txtUnidadCada)
        Me.grpTarjeta.Controls.Add(Me.txtDescripcion)
        Me.grpTarjeta.Controls.Add(Me.Label2)
        Me.grpTarjeta.Controls.Add(Me.txtCodigo)
        Me.grpTarjeta.Controls.Add(Me.Label1)
        Me.grpTarjeta.Controls.Add(Me.txtCantidad)
        Me.grpTarjeta.Controls.Add(Me.btnCodigoBono)
        Me.grpTarjeta.Controls.Add(Me.txtUnidad)
        Me.grpTarjeta.Controls.Add(Me.txtCantidadBono)
        Me.grpTarjeta.Controls.Add(Me.txtDescripcionBono)
        Me.grpTarjeta.Controls.Add(Me.txtCodigoBono)
        Me.grpTarjeta.Controls.Add(Me.lblDescripcion)
        Me.grpTarjeta.Controls.Add(Me.lblCodigo)
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(538, 322)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        '
        'cmbOtorgante
        '
        Me.cmbOtorgante.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOtorgante.FormattingEnabled = True
        Me.cmbOtorgante.Location = New System.Drawing.Point(136, 288)
        Me.cmbOtorgante.Name = "cmbOtorgante"
        Me.cmbOtorgante.Size = New System.Drawing.Size(86, 21)
        Me.cmbOtorgante.TabIndex = 60
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 281)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(118, 32)
        Me.Label5.TabIndex = 59
        Me.Label5.Text = "Esta bonificación será otorgada por"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmbUnidadBono
        '
        Me.cmbUnidadBono.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUnidadBono.FormattingEnabled = True
        Me.cmbUnidadBono.Location = New System.Drawing.Point(228, 162)
        Me.cmbUnidadBono.Name = "cmbUnidadBono"
        Me.cmbUnidadBono.Size = New System.Drawing.Size(44, 21)
        Me.cmbUnidadBono.TabIndex = 58
        '
        'txtDescripcionCada
        '
        Me.txtDescripcionCada.Location = New System.Drawing.Point(136, 233)
        Me.txtDescripcionCada.Multiline = True
        Me.txtDescripcionCada.Name = "txtDescripcionCada"
        Me.txtDescripcionCada.Size = New System.Drawing.Size(392, 41)
        Me.txtDescripcionCada.TabIndex = 57
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 212)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(118, 19)
        Me.Label3.TabIndex = 56
        Me.Label3.Text = "de "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCodigoCada
        '
        Me.txtCodigoCada.BackColor = System.Drawing.Color.White
        Me.txtCodigoCada.Location = New System.Drawing.Point(136, 212)
        Me.txtCodigoCada.Name = "txtCodigoCada"
        Me.txtCodigoCada.Size = New System.Drawing.Size(136, 20)
        Me.txtCodigoCada.TabIndex = 55
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 192)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 19)
        Me.Label4.TabIndex = 54
        Me.Label4.Text = "por cada "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCantidadCada
        '
        Me.txtCantidadCada.BackColor = System.Drawing.Color.White
        Me.txtCantidadCada.Location = New System.Drawing.Point(136, 191)
        Me.txtCantidadCada.Name = "txtCantidadCada"
        Me.txtCantidadCada.Size = New System.Drawing.Size(86, 20)
        Me.txtCantidadCada.TabIndex = 53
        Me.txtCantidadCada.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtUnidadCada
        '
        Me.txtUnidadCada.Location = New System.Drawing.Point(228, 191)
        Me.txtUnidadCada.Name = "txtUnidadCada"
        Me.txtUnidadCada.Size = New System.Drawing.Size(44, 20)
        Me.txtUnidadCada.TabIndex = 52
        Me.txtUnidadCada.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Location = New System.Drawing.Point(136, 53)
        Me.txtDescripcion.Multiline = True
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(392, 41)
        Me.txtDescripcion.TabIndex = 50
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 19)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = "de "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCodigo
        '
        Me.txtCodigo.BackColor = System.Drawing.Color.White
        Me.txtCodigo.Location = New System.Drawing.Point(136, 32)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(136, 20)
        Me.txtCodigo.TabIndex = 48
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 19)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = "A partir de "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCantidad
        '
        Me.txtCantidad.BackColor = System.Drawing.Color.White
        Me.txtCantidad.Location = New System.Drawing.Point(136, 11)
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(86, 20)
        Me.txtCantidad.TabIndex = 46
        Me.txtCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnCodigoBono
        '
        Me.btnCodigoBono.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCodigoBono.Location = New System.Drawing.Point(278, 99)
        Me.btnCodigoBono.Name = "btnCodigoBono"
        Me.btnCodigoBono.Size = New System.Drawing.Size(27, 20)
        Me.btnCodigoBono.TabIndex = 45
        Me.btnCodigoBono.Text = "···"
        Me.btnCodigoBono.UseVisualStyleBackColor = True
        '
        'txtUnidad
        '
        Me.txtUnidad.Location = New System.Drawing.Point(228, 11)
        Me.txtUnidad.Name = "txtUnidad"
        Me.txtUnidad.Size = New System.Drawing.Size(44, 20)
        Me.txtUnidad.TabIndex = 38
        Me.txtUnidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtCantidadBono
        '
        Me.txtCantidadBono.Location = New System.Drawing.Point(136, 162)
        Me.txtCantidadBono.Name = "txtCantidadBono"
        Me.txtCantidadBono.Size = New System.Drawing.Size(86, 20)
        Me.txtCantidadBono.TabIndex = 17
        Me.txtCantidadBono.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDescripcionBono
        '
        Me.txtDescripcionBono.Location = New System.Drawing.Point(136, 120)
        Me.txtDescripcionBono.Multiline = True
        Me.txtDescripcionBono.Name = "txtDescripcionBono"
        Me.txtDescripcionBono.Size = New System.Drawing.Size(392, 41)
        Me.txtDescripcionBono.TabIndex = 14
        '
        'txtCodigoBono
        '
        Me.txtCodigoBono.BackColor = System.Drawing.Color.White
        Me.txtCodigoBono.Location = New System.Drawing.Point(136, 100)
        Me.txtCodigoBono.Name = "txtCodigoBono"
        Me.txtCodigoBono.Size = New System.Drawing.Size(136, 20)
        Me.txtCodigoBono.TabIndex = 13
        '
        'lblDescripcion
        '
        Me.lblDescripcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion.Location = New System.Drawing.Point(12, 164)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(118, 19)
        Me.lblDescripcion.TabIndex = 3
        Me.lblDescripcion.Text = "la cantidad de "
        Me.lblDescripcion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCodigo
        '
        Me.lblCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCodigo.Location = New System.Drawing.Point(12, 100)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(118, 19)
        Me.lblCodigo.TabIndex = 1
        Me.lblCodigo.Text = "Se bonificacará de "
        Me.lblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(375, 323)
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
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        '
        'jsMerArcOfertasMovimientosBono
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(540, 352)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsMerArcOfertasMovimientosBono"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento precio especiales"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents lblCodigo As System.Windows.Forms.Label
    Friend WithEvents txtDescripcionBono As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigoBono As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtCantidadBono As System.Windows.Forms.TextBox
    Friend WithEvents txtUnidad As System.Windows.Forms.TextBox
    Friend WithEvents btnCodigoBono As System.Windows.Forms.Button
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCantidad As System.Windows.Forms.TextBox
    Friend WithEvents txtDescripcionCada As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCodigoCada As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtCantidadCada As System.Windows.Forms.TextBox
    Friend WithEvents txtUnidadCada As System.Windows.Forms.TextBox
    Friend WithEvents cmbUnidadBono As System.Windows.Forms.ComboBox
    Friend WithEvents cmbOtorgante As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
