<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsProArcFormulacionesRenglones
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsProArcFormulacionesRenglones))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.cmbSubEnsamble = New System.Windows.Forms.ComboBox()
        Me.lblTipoRenglon = New System.Windows.Forms.Label()
        Me.lblAlmacenDescripcion = New System.Windows.Forms.Label()
        Me.lblPesoDes = New System.Windows.Forms.Label()
        Me.btnCodigo = New System.Windows.Forms.Button()
        Me.btnAlmacen = New System.Windows.Forms.Button()
        Me.txtAlmacen = New System.Windows.Forms.TextBox()
        Me.txtPesoTotal = New System.Windows.Forms.TextBox()
        Me.txtCostoTotal = New System.Windows.Forms.TextBox()
        Me.txtPorResidual = New System.Windows.Forms.TextBox()
        Me.lblAlmacen = New System.Windows.Forms.Label()
        Me.lblPesoTotal = New System.Windows.Forms.Label()
        Me.lblCostoTotal = New System.Windows.Forms.Label()
        Me.lblPorResidual = New System.Windows.Forms.Label()
        Me.txtCantidad = New System.Windows.Forms.TextBox()
        Me.btnDescripcion = New System.Windows.Forms.Button()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.cmbUnidad = New System.Windows.Forms.ComboBox()
        Me.lblUnidad = New System.Windows.Forms.Label()
        Me.lblCostoUnitario = New System.Windows.Forms.Label()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.txtCostoUnitario = New System.Windows.Forms.TextBox()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.grpTarjeta.SuspendLayout
        Me.grpAceptarSalir.SuspendLayout
        Me.SuspendLayout
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252,Byte),Integer), CType(CType(255,Byte),Integer), CType(CType(217,Byte),Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 349)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(540, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234,Byte),Integer), CType(CType(241,Byte),Integer), CType(CType(250,Byte),Integer))
        Me.grpTarjeta.Controls.Add(Me.cmbSubEnsamble)
        Me.grpTarjeta.Controls.Add(Me.lblTipoRenglon)
        Me.grpTarjeta.Controls.Add(Me.lblAlmacenDescripcion)
        Me.grpTarjeta.Controls.Add(Me.lblPesoDes)
        Me.grpTarjeta.Controls.Add(Me.btnCodigo)
        Me.grpTarjeta.Controls.Add(Me.btnAlmacen)
        Me.grpTarjeta.Controls.Add(Me.txtAlmacen)
        Me.grpTarjeta.Controls.Add(Me.txtPesoTotal)
        Me.grpTarjeta.Controls.Add(Me.txtCostoTotal)
        Me.grpTarjeta.Controls.Add(Me.txtPorResidual)
        Me.grpTarjeta.Controls.Add(Me.lblAlmacen)
        Me.grpTarjeta.Controls.Add(Me.lblPesoTotal)
        Me.grpTarjeta.Controls.Add(Me.lblCostoTotal)
        Me.grpTarjeta.Controls.Add(Me.lblPorResidual)
        Me.grpTarjeta.Controls.Add(Me.txtCantidad)
        Me.grpTarjeta.Controls.Add(Me.btnDescripcion)
        Me.grpTarjeta.Controls.Add(Me.lblCantidad)
        Me.grpTarjeta.Controls.Add(Me.txtDescripcion)
        Me.grpTarjeta.Controls.Add(Me.txtCodigo)
        Me.grpTarjeta.Controls.Add(Me.cmbUnidad)
        Me.grpTarjeta.Controls.Add(Me.lblUnidad)
        Me.grpTarjeta.Controls.Add(Me.lblCostoUnitario)
        Me.grpTarjeta.Controls.Add(Me.lblDescripcion)
        Me.grpTarjeta.Controls.Add(Me.lblCodigo)
        Me.grpTarjeta.Controls.Add(Me.txtCostoUnitario)
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(538, 346)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = false
        '
        'cmbSubEnsamble
        '
        Me.cmbSubEnsamble.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubEnsamble.FormattingEnabled = true
        Me.cmbSubEnsamble.Location = New System.Drawing.Point(137, 286)
        Me.cmbSubEnsamble.Name = "cmbSubEnsamble"
        Me.cmbSubEnsamble.Size = New System.Drawing.Size(136, 21)
        Me.cmbSubEnsamble.TabIndex = 55
        '
        'lblTipoRenglon
        '
        Me.lblTipoRenglon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblTipoRenglon.Location = New System.Drawing.Point(13, 288)
        Me.lblTipoRenglon.Name = "lblTipoRenglon"
        Me.lblTipoRenglon.Size = New System.Drawing.Size(118, 19)
        Me.lblTipoRenglon.TabIndex = 54
        Me.lblTipoRenglon.Text = "Sub-Ensamble :"
        Me.lblTipoRenglon.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblAlmacenDescripcion
        '
        Me.lblAlmacenDescripcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblAlmacenDescripcion.Location = New System.Drawing.Point(137, 244)
        Me.lblAlmacenDescripcion.Name = "lblAlmacenDescripcion"
        Me.lblAlmacenDescripcion.Size = New System.Drawing.Size(391, 39)
        Me.lblAlmacenDescripcion.TabIndex = 51
        Me.lblAlmacenDescripcion.Text = "Kgr."
        '
        'lblPesoDes
        '
        Me.lblPesoDes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPesoDes.Location = New System.Drawing.Point(276, 201)
        Me.lblPesoDes.Name = "lblPesoDes"
        Me.lblPesoDes.Size = New System.Drawing.Size(43, 19)
        Me.lblPesoDes.TabIndex = 50
        Me.lblPesoDes.Text = "Kgr."
        Me.lblPesoDes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCodigo
        '
        Me.btnCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnCodigo.Location = New System.Drawing.Point(279, 19)
        Me.btnCodigo.Name = "btnCodigo"
        Me.btnCodigo.Size = New System.Drawing.Size(27, 20)
        Me.btnCodigo.TabIndex = 3
        Me.btnCodigo.Text = "···"
        Me.btnCodigo.UseVisualStyleBackColor = true
        '
        'btnAlmacen
        '
        Me.btnAlmacen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnAlmacen.Location = New System.Drawing.Point(279, 223)
        Me.btnAlmacen.Name = "btnAlmacen"
        Me.btnAlmacen.Size = New System.Drawing.Size(27, 20)
        Me.btnAlmacen.TabIndex = 24
        Me.btnAlmacen.Text = "···"
        Me.btnAlmacen.UseVisualStyleBackColor = true
        '
        'txtAlmacen
        '
        Me.txtAlmacen.Location = New System.Drawing.Point(137, 221)
        Me.txtAlmacen.Name = "txtAlmacen"
        Me.txtAlmacen.Size = New System.Drawing.Size(136, 20)
        Me.txtAlmacen.TabIndex = 23
        Me.txtAlmacen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPesoTotal
        '
        Me.txtPesoTotal.Location = New System.Drawing.Point(137, 200)
        Me.txtPesoTotal.Name = "txtPesoTotal"
        Me.txtPesoTotal.Size = New System.Drawing.Size(136, 20)
        Me.txtPesoTotal.TabIndex = 22
        Me.txtPesoTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCostoTotal
        '
        Me.txtCostoTotal.Location = New System.Drawing.Point(137, 179)
        Me.txtCostoTotal.Name = "txtCostoTotal"
        Me.txtCostoTotal.Size = New System.Drawing.Size(136, 20)
        Me.txtCostoTotal.TabIndex = 21
        Me.txtCostoTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPorResidual
        '
        Me.txtPorResidual.Location = New System.Drawing.Point(137, 158)
        Me.txtPorResidual.Name = "txtPorResidual"
        Me.txtPorResidual.Size = New System.Drawing.Size(136, 20)
        Me.txtPorResidual.TabIndex = 20
        Me.txtPorResidual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblAlmacen
        '
        Me.lblAlmacen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblAlmacen.Location = New System.Drawing.Point(13, 220)
        Me.lblAlmacen.Name = "lblAlmacen"
        Me.lblAlmacen.Size = New System.Drawing.Size(118, 19)
        Me.lblAlmacen.TabIndex = 35
        Me.lblAlmacen.Text = "Almacén Salida :"
        Me.lblAlmacen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPesoTotal
        '
        Me.lblPesoTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPesoTotal.Location = New System.Drawing.Point(13, 201)
        Me.lblPesoTotal.Name = "lblPesoTotal"
        Me.lblPesoTotal.Size = New System.Drawing.Size(118, 19)
        Me.lblPesoTotal.TabIndex = 34
        Me.lblPesoTotal.Text = "Peso total :"
        Me.lblPesoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCostoTotal
        '
        Me.lblCostoTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCostoTotal.Location = New System.Drawing.Point(13, 180)
        Me.lblCostoTotal.Name = "lblCostoTotal"
        Me.lblCostoTotal.Size = New System.Drawing.Size(118, 19)
        Me.lblCostoTotal.TabIndex = 33
        Me.lblCostoTotal.Text = "Costo Total :"
        Me.lblCostoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPorResidual
        '
        Me.lblPorResidual.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblPorResidual.Location = New System.Drawing.Point(2, 159)
        Me.lblPorResidual.Name = "lblPorResidual"
        Me.lblPorResidual.Size = New System.Drawing.Size(128, 19)
        Me.lblPorResidual.TabIndex = 32
        Me.lblPorResidual.Text = "% residual :"
        Me.lblPorResidual.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCantidad
        '
        Me.txtCantidad.Location = New System.Drawing.Point(137, 116)
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(136, 20)
        Me.txtCantidad.TabIndex = 2
        Me.txtCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnDescripcion
        '
        Me.btnDescripcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.btnDescripcion.Location = New System.Drawing.Point(495, 75)
        Me.btnDescripcion.Name = "btnDescripcion"
        Me.btnDescripcion.Size = New System.Drawing.Size(27, 20)
        Me.btnDescripcion.TabIndex = 5
        Me.btnDescripcion.Text = "···"
        Me.btnDescripcion.UseVisualStyleBackColor = true
        '
        'lblCantidad
        '
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCantidad.Location = New System.Drawing.Point(13, 116)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(118, 20)
        Me.lblCantidad.TabIndex = 18
        Me.lblCantidad.Text = "Cantidad :"
        Me.lblCantidad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Location = New System.Drawing.Point(137, 39)
        Me.txtDescripcion.Multiline = true
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(353, 54)
        Me.txtDescripcion.TabIndex = 4
        '
        'txtCodigo
        '
        Me.txtCodigo.BackColor = System.Drawing.Color.White
        Me.txtCodigo.Location = New System.Drawing.Point(137, 18)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(136, 20)
        Me.txtCodigo.TabIndex = 1
        '
        'cmbUnidad
        '
        Me.cmbUnidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUnidad.FormattingEnabled = true
        Me.cmbUnidad.Location = New System.Drawing.Point(137, 94)
        Me.cmbUnidad.Name = "cmbUnidad"
        Me.cmbUnidad.Size = New System.Drawing.Size(136, 21)
        Me.cmbUnidad.TabIndex = 6
        '
        'lblUnidad
        '
        Me.lblUnidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblUnidad.Location = New System.Drawing.Point(13, 97)
        Me.lblUnidad.Name = "lblUnidad"
        Me.lblUnidad.Size = New System.Drawing.Size(118, 19)
        Me.lblUnidad.TabIndex = 5
        Me.lblUnidad.Text = "Unidad :"
        Me.lblUnidad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCostoUnitario
        '
        Me.lblCostoUnitario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCostoUnitario.Location = New System.Drawing.Point(12, 137)
        Me.lblCostoUnitario.Name = "lblCostoUnitario"
        Me.lblCostoUnitario.Size = New System.Drawing.Size(118, 19)
        Me.lblCostoUnitario.TabIndex = 4
        Me.lblCostoUnitario.Text = "Costo unitario :"
        Me.lblCostoUnitario.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDescripcion
        '
        Me.lblDescripcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblDescripcion.Location = New System.Drawing.Point(13, 41)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(118, 19)
        Me.lblDescripcion.TabIndex = 3
        Me.lblDescripcion.Text = "Descripción  :"
        Me.lblDescripcion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCodigo
        '
        Me.lblCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.lblCodigo.Location = New System.Drawing.Point(12, 19)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(118, 19)
        Me.lblCodigo.TabIndex = 1
        Me.lblCodigo.Text = "Item  :"
        Me.lblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCostoUnitario
        '
        Me.txtCostoUnitario.Location = New System.Drawing.Point(137, 137)
        Me.txtCostoUnitario.Name = "txtCostoUnitario"
        Me.txtCostoUnitario.Size = New System.Drawing.Size(136, 20)
        Me.txtCostoUnitario.TabIndex = 14
        Me.txtCostoUnitario.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(375, 346)
        Me.grpAceptarSalir.Name = "grpAceptarSalir"
        Me.grpAceptarSalir.RowCount = 1
        Me.grpAceptarSalir.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
        Me.grpAceptarSalir.Size = New System.Drawing.Size(165, 30)
        Me.grpAceptarSalir.TabIndex = 88
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.ImageIndex = 1
        Me.btnCancel.ImageList = Me.ImageList1
        Me.btnCancel.Location = New System.Drawing.Point(85, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(76, 24)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"),System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "button_ok.ico")
        Me.ImageList1.Images.SetKeyName(1, "button_cancel.ico")
        '
        'btnOK
        '
        Me.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOK.ImageIndex = 0
        Me.btnOK.ImageList = Me.ImageList1
        Me.btnOK.Location = New System.Drawing.Point(3, 3)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(76, 24)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Aceptar"
        Me.btnOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        '
        'C1SuperTooltip1
        '
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8!)
        '
        'jsProArcFormulacionesRenglones
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234,Byte),Integer), CType(CType(241,Byte),Integer), CType(CType(250,Byte),Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(540, 375)
        Me.ControlBox = false
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
        Me.Name = "jsProArcFormulacionesRenglones"
        Me.ShowInTaskbar = false
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento formulación"
        Me.grpTarjeta.ResumeLayout(false)
        Me.grpTarjeta.PerformLayout
        Me.grpAceptarSalir.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents lblUnidad As System.Windows.Forms.Label
    Friend WithEvents lblCostoUnitario As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents lblCodigo As System.Windows.Forms.Label
    Friend WithEvents cmbUnidad As System.Windows.Forms.ComboBox
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents btnDescripcion As System.Windows.Forms.Button
    Friend WithEvents txtCantidad As System.Windows.Forms.TextBox
    Friend WithEvents txtCostoUnitario As System.Windows.Forms.TextBox
    Friend WithEvents lblPorResidual As System.Windows.Forms.Label
    Friend WithEvents lblAlmacen As System.Windows.Forms.Label
    Friend WithEvents lblPesoTotal As System.Windows.Forms.Label
    Friend WithEvents lblCostoTotal As System.Windows.Forms.Label
    Friend WithEvents txtPesoTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtCostoTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtPorResidual As System.Windows.Forms.TextBox
    Friend WithEvents btnAlmacen As System.Windows.Forms.Button
    Friend WithEvents txtAlmacen As System.Windows.Forms.TextBox
    Friend WithEvents btnCodigo As System.Windows.Forms.Button
    Friend WithEvents lblPesoDes As System.Windows.Forms.Label
    Friend WithEvents lblAlmacenDescripcion As System.Windows.Forms.Label
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents cmbSubEnsamble As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipoRenglon As System.Windows.Forms.Label
End Class
