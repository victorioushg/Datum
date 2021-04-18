<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class jsGenRenglonesMovimientos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(jsGenRenglonesMovimientos))
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.grpTarjeta = New System.Windows.Forms.GroupBox()
        Me.lblRenglon = New System.Windows.Forms.Label()
        Me.btnDescProveedor = New System.Windows.Forms.Button()
        Me.cmbTipoRenglon = New System.Windows.Forms.ComboBox()
        Me.lblTipoRenglon = New System.Windows.Forms.Label()
        Me.btnPesoCaptura = New System.Windows.Forms.Button()
        Me.grpIVA = New System.Windows.Forms.GroupBox()
        Me.lbltotalIVA = New System.Windows.Forms.Label()
        Me.txtTotalIVA = New System.Windows.Forms.TextBox()
        Me.txtComentarioOferta = New System.Windows.Forms.TextBox()
        Me.txtPrecioIVA = New System.Windows.Forms.TextBox()
        Me.lblPrecioIVA = New System.Windows.Forms.Label()
        Me.lblCausaDEs = New System.Windows.Forms.Label()
        Me.lblPesoDes = New System.Windows.Forms.Label()
        Me.btnCodigoArticulo = New System.Windows.Forms.Button()
        Me.btnCausa = New System.Windows.Forms.Button()
        Me.btnCantidadTC = New System.Windows.Forms.Button()
        Me.btnLote = New System.Windows.Forms.Button()
        Me.btnFactura = New System.Windows.Forms.Button()
        Me.txtCausa = New System.Windows.Forms.TextBox()
        Me.txtPesoTotal = New System.Windows.Forms.TextBox()
        Me.txtCostoPrecioTotal = New System.Windows.Forms.TextBox()
        Me.txtPorAceptaDev = New System.Windows.Forms.TextBox()
        Me.txtDesc_ofe = New System.Windows.Forms.TextBox()
        Me.txtPorIVA = New System.Windows.Forms.TextBox()
        Me.txtFactura = New System.Windows.Forms.TextBox()
        Me.lblLote = New System.Windows.Forms.Label()
        Me.lblFactura = New System.Windows.Forms.Label()
        Me.lblCausa = New System.Windows.Forms.Label()
        Me.lblPesoTotal = New System.Windows.Forms.Label()
        Me.lblCostoTotal = New System.Windows.Forms.Label()
        Me.lblPorAcepta = New System.Windows.Forms.Label()
        Me.lblDsctoOferta = New System.Windows.Forms.Label()
        Me.lblDsctoCliente = New System.Windows.Forms.Label()
        Me.lblDsctoMercancia = New System.Windows.Forms.Label()
        Me.lblIVA = New System.Windows.Forms.Label()
        Me.txtDesc_cli = New System.Windows.Forms.TextBox()
        Me.txtDesc_art = New System.Windows.Forms.TextBox()
        Me.txtIVA = New System.Windows.Forms.TextBox()
        Me.txtCantidad = New System.Windows.Forms.TextBox()
        Me.btnComentarioAdicional = New System.Windows.Forms.Button()
        Me.lblCantidad = New System.Windows.Forms.Label()
        Me.txtLote = New System.Windows.Forms.TextBox()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.cmbUnidad = New System.Windows.Forms.ComboBox()
        Me.lblUnidad = New System.Windows.Forms.Label()
        Me.lblCostoPrecio = New System.Windows.Forms.Label()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.txtCostoPrecio = New System.Windows.Forms.TextBox()
        Me.cmbCostoPrecio = New System.Windows.Forms.ComboBox()
        Me.grpAceptarSalir = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.btnOK = New System.Windows.Forms.Button()
        Me.C1SuperTooltip1 = New C1.Win.C1SuperTooltip.C1SuperTooltip(Me.components)
        Me.grpTarjeta.SuspendLayout()
        Me.grpIVA.SuspendLayout()
        Me.grpAceptarSalir.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblInfo.Location = New System.Drawing.Point(0, 459)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(540, 26)
        Me.lblInfo.TabIndex = 79
        '
        'grpTarjeta
        '
        Me.grpTarjeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.grpTarjeta.Controls.Add(Me.lblRenglon)
        Me.grpTarjeta.Controls.Add(Me.btnDescProveedor)
        Me.grpTarjeta.Controls.Add(Me.cmbTipoRenglon)
        Me.grpTarjeta.Controls.Add(Me.lblTipoRenglon)
        Me.grpTarjeta.Controls.Add(Me.btnPesoCaptura)
        Me.grpTarjeta.Controls.Add(Me.grpIVA)
        Me.grpTarjeta.Controls.Add(Me.lblCausaDEs)
        Me.grpTarjeta.Controls.Add(Me.lblPesoDes)
        Me.grpTarjeta.Controls.Add(Me.btnCodigoArticulo)
        Me.grpTarjeta.Controls.Add(Me.btnCausa)
        Me.grpTarjeta.Controls.Add(Me.btnCantidadTC)
        Me.grpTarjeta.Controls.Add(Me.btnLote)
        Me.grpTarjeta.Controls.Add(Me.btnFactura)
        Me.grpTarjeta.Controls.Add(Me.txtCausa)
        Me.grpTarjeta.Controls.Add(Me.txtPesoTotal)
        Me.grpTarjeta.Controls.Add(Me.txtCostoPrecioTotal)
        Me.grpTarjeta.Controls.Add(Me.txtPorAceptaDev)
        Me.grpTarjeta.Controls.Add(Me.txtDesc_ofe)
        Me.grpTarjeta.Controls.Add(Me.txtPorIVA)
        Me.grpTarjeta.Controls.Add(Me.txtFactura)
        Me.grpTarjeta.Controls.Add(Me.lblLote)
        Me.grpTarjeta.Controls.Add(Me.lblFactura)
        Me.grpTarjeta.Controls.Add(Me.lblCausa)
        Me.grpTarjeta.Controls.Add(Me.lblPesoTotal)
        Me.grpTarjeta.Controls.Add(Me.lblCostoTotal)
        Me.grpTarjeta.Controls.Add(Me.lblPorAcepta)
        Me.grpTarjeta.Controls.Add(Me.lblDsctoOferta)
        Me.grpTarjeta.Controls.Add(Me.lblDsctoCliente)
        Me.grpTarjeta.Controls.Add(Me.lblDsctoMercancia)
        Me.grpTarjeta.Controls.Add(Me.lblIVA)
        Me.grpTarjeta.Controls.Add(Me.txtDesc_cli)
        Me.grpTarjeta.Controls.Add(Me.txtDesc_art)
        Me.grpTarjeta.Controls.Add(Me.txtIVA)
        Me.grpTarjeta.Controls.Add(Me.txtCantidad)
        Me.grpTarjeta.Controls.Add(Me.btnComentarioAdicional)
        Me.grpTarjeta.Controls.Add(Me.lblCantidad)
        Me.grpTarjeta.Controls.Add(Me.txtLote)
        Me.grpTarjeta.Controls.Add(Me.txtDescripcion)
        Me.grpTarjeta.Controls.Add(Me.txtCodigo)
        Me.grpTarjeta.Controls.Add(Me.cmbUnidad)
        Me.grpTarjeta.Controls.Add(Me.lblUnidad)
        Me.grpTarjeta.Controls.Add(Me.lblCostoPrecio)
        Me.grpTarjeta.Controls.Add(Me.lblDescripcion)
        Me.grpTarjeta.Controls.Add(Me.lblCodigo)
        Me.grpTarjeta.Controls.Add(Me.txtCostoPrecio)
        Me.grpTarjeta.Controls.Add(Me.cmbCostoPrecio)
        Me.grpTarjeta.Location = New System.Drawing.Point(0, 1)
        Me.grpTarjeta.Name = "grpTarjeta"
        Me.grpTarjeta.Size = New System.Drawing.Size(538, 455)
        Me.grpTarjeta.TabIndex = 80
        Me.grpTarjeta.TabStop = False
        '
        'lblRenglon
        '
        Me.lblRenglon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRenglon.Location = New System.Drawing.Point(410, 17)
        Me.lblRenglon.Name = "lblRenglon"
        Me.lblRenglon.Size = New System.Drawing.Size(118, 19)
        Me.lblRenglon.TabIndex = 57
        Me.lblRenglon.Text = "Item  :"
        Me.lblRenglon.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnDescProveedor
        '
        Me.btnDescProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDescProveedor.Location = New System.Drawing.Point(279, 253)
        Me.btnDescProveedor.Name = "btnDescProveedor"
        Me.btnDescProveedor.Size = New System.Drawing.Size(27, 20)
        Me.btnDescProveedor.TabIndex = 56
        Me.btnDescProveedor.Text = "···"
        Me.btnDescProveedor.UseVisualStyleBackColor = True
        '
        'cmbTipoRenglon
        '
        Me.cmbTipoRenglon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoRenglon.FormattingEnabled = True
        Me.cmbTipoRenglon.Location = New System.Drawing.Point(137, 428)
        Me.cmbTipoRenglon.Name = "cmbTipoRenglon"
        Me.cmbTipoRenglon.Size = New System.Drawing.Size(136, 21)
        Me.cmbTipoRenglon.TabIndex = 55
        '
        'lblTipoRenglon
        '
        Me.lblTipoRenglon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTipoRenglon.Location = New System.Drawing.Point(12, 428)
        Me.lblTipoRenglon.Name = "lblTipoRenglon"
        Me.lblTipoRenglon.Size = New System.Drawing.Size(118, 19)
        Me.lblTipoRenglon.TabIndex = 54
        Me.lblTipoRenglon.Text = "Tipo renglón :"
        Me.lblTipoRenglon.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnPesoCaptura
        '
        Me.btnPesoCaptura.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPesoCaptura.Location = New System.Drawing.Point(312, 158)
        Me.btnPesoCaptura.Name = "btnPesoCaptura"
        Me.btnPesoCaptura.Size = New System.Drawing.Size(27, 20)
        Me.btnPesoCaptura.TabIndex = 12
        Me.btnPesoCaptura.Text = "<"
        Me.btnPesoCaptura.UseVisualStyleBackColor = True
        '
        'grpIVA
        '
        Me.grpIVA.Controls.Add(Me.lbltotalIVA)
        Me.grpIVA.Controls.Add(Me.txtTotalIVA)
        Me.grpIVA.Controls.Add(Me.txtComentarioOferta)
        Me.grpIVA.Controls.Add(Me.txtPrecioIVA)
        Me.grpIVA.Controls.Add(Me.lblPrecioIVA)
        Me.grpIVA.Location = New System.Drawing.Point(312, 177)
        Me.grpIVA.Name = "grpIVA"
        Me.grpIVA.Size = New System.Drawing.Size(215, 178)
        Me.grpIVA.TabIndex = 53
        Me.grpIVA.TabStop = False
        '
        'lbltotalIVA
        '
        Me.lbltotalIVA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltotalIVA.Location = New System.Drawing.Point(6, 138)
        Me.lbltotalIVA.Name = "lbltotalIVA"
        Me.lbltotalIVA.Size = New System.Drawing.Size(89, 19)
        Me.lbltotalIVA.TabIndex = 28
        Me.lbltotalIVA.Text = "+ IVA :"
        Me.lbltotalIVA.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTotalIVA
        '
        Me.txtTotalIVA.Location = New System.Drawing.Point(100, 138)
        Me.txtTotalIVA.Name = "txtTotalIVA"
        Me.txtTotalIVA.Size = New System.Drawing.Size(109, 20)
        Me.txtTotalIVA.TabIndex = 27
        Me.txtTotalIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtComentarioOferta
        '
        Me.txtComentarioOferta.Location = New System.Drawing.Point(6, 97)
        Me.txtComentarioOferta.Name = "txtComentarioOferta"
        Me.txtComentarioOferta.Size = New System.Drawing.Size(203, 20)
        Me.txtComentarioOferta.TabIndex = 26
        Me.txtComentarioOferta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPrecioIVA
        '
        Me.txtPrecioIVA.Location = New System.Drawing.Point(101, 11)
        Me.txtPrecioIVA.Name = "txtPrecioIVA"
        Me.txtPrecioIVA.Size = New System.Drawing.Size(109, 20)
        Me.txtPrecioIVA.TabIndex = 25
        Me.txtPrecioIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblPrecioIVA
        '
        Me.lblPrecioIVA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrecioIVA.Location = New System.Drawing.Point(6, 12)
        Me.lblPrecioIVA.Name = "lblPrecioIVA"
        Me.lblPrecioIVA.Size = New System.Drawing.Size(89, 19)
        Me.lblPrecioIVA.TabIndex = 5
        Me.lblPrecioIVA.Text = "+ IVA :"
        Me.lblPrecioIVA.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCausaDEs
        '
        Me.lblCausaDEs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCausaDEs.Location = New System.Drawing.Point(137, 381)
        Me.lblCausaDEs.Name = "lblCausaDEs"
        Me.lblCausaDEs.Size = New System.Drawing.Size(391, 39)
        Me.lblCausaDEs.TabIndex = 51
        Me.lblCausaDEs.Text = "Kgr."
        '
        'lblPesoDes
        '
        Me.lblPesoDes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPesoDes.Location = New System.Drawing.Point(276, 336)
        Me.lblPesoDes.Name = "lblPesoDes"
        Me.lblPesoDes.Size = New System.Drawing.Size(30, 19)
        Me.lblPesoDes.TabIndex = 50
        Me.lblPesoDes.Text = "Kgr."
        Me.lblPesoDes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnCodigoArticulo
        '
        Me.btnCodigoArticulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCodigoArticulo.Location = New System.Drawing.Point(279, 19)
        Me.btnCodigoArticulo.Name = "btnCodigoArticulo"
        Me.btnCodigoArticulo.Size = New System.Drawing.Size(27, 20)
        Me.btnCodigoArticulo.TabIndex = 3
        Me.btnCodigoArticulo.Text = "···"
        Me.btnCodigoArticulo.UseVisualStyleBackColor = True
        '
        'btnCausa
        '
        Me.btnCausa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCausa.Location = New System.Drawing.Point(279, 358)
        Me.btnCausa.Name = "btnCausa"
        Me.btnCausa.Size = New System.Drawing.Size(27, 20)
        Me.btnCausa.TabIndex = 24
        Me.btnCausa.Text = "···"
        Me.btnCausa.UseVisualStyleBackColor = True
        '
        'btnCantidadTC
        '
        Me.btnCantidadTC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCantidadTC.Location = New System.Drawing.Point(279, 158)
        Me.btnCantidadTC.Name = "btnCantidadTC"
        Me.btnCantidadTC.Size = New System.Drawing.Size(27, 20)
        Me.btnCantidadTC.TabIndex = 11
        Me.btnCantidadTC.Text = "···"
        Me.btnCantidadTC.UseVisualStyleBackColor = True
        '
        'btnLote
        '
        Me.btnLote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLote.Location = New System.Drawing.Point(279, 137)
        Me.btnLote.Name = "btnLote"
        Me.btnLote.Size = New System.Drawing.Size(27, 20)
        Me.btnLote.TabIndex = 10
        Me.btnLote.Text = "···"
        Me.btnLote.UseVisualStyleBackColor = True
        '
        'btnFactura
        '
        Me.btnFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFactura.Location = New System.Drawing.Point(279, 116)
        Me.btnFactura.Name = "btnFactura"
        Me.btnFactura.Size = New System.Drawing.Size(27, 20)
        Me.btnFactura.TabIndex = 8
        Me.btnFactura.Text = "···"
        Me.btnFactura.UseVisualStyleBackColor = True
        '
        'txtCausa
        '
        Me.txtCausa.Location = New System.Drawing.Point(137, 358)
        Me.txtCausa.Name = "txtCausa"
        Me.txtCausa.Size = New System.Drawing.Size(136, 20)
        Me.txtCausa.TabIndex = 23
        Me.txtCausa.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPesoTotal
        '
        Me.txtPesoTotal.Location = New System.Drawing.Point(137, 337)
        Me.txtPesoTotal.Name = "txtPesoTotal"
        Me.txtPesoTotal.Size = New System.Drawing.Size(136, 20)
        Me.txtPesoTotal.TabIndex = 22
        Me.txtPesoTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCostoPrecioTotal
        '
        Me.txtCostoPrecioTotal.Location = New System.Drawing.Point(137, 316)
        Me.txtCostoPrecioTotal.Name = "txtCostoPrecioTotal"
        Me.txtCostoPrecioTotal.Size = New System.Drawing.Size(136, 20)
        Me.txtCostoPrecioTotal.TabIndex = 21
        Me.txtCostoPrecioTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPorAceptaDev
        '
        Me.txtPorAceptaDev.Location = New System.Drawing.Point(137, 295)
        Me.txtPorAceptaDev.Name = "txtPorAceptaDev"
        Me.txtPorAceptaDev.Size = New System.Drawing.Size(136, 20)
        Me.txtPorAceptaDev.TabIndex = 20
        Me.txtPorAceptaDev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDesc_ofe
        '
        Me.txtDesc_ofe.Location = New System.Drawing.Point(137, 274)
        Me.txtDesc_ofe.Name = "txtDesc_ofe"
        Me.txtDesc_ofe.Size = New System.Drawing.Size(136, 20)
        Me.txtDesc_ofe.TabIndex = 19
        Me.txtDesc_ofe.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPorIVA
        '
        Me.txtPorIVA.Location = New System.Drawing.Point(177, 211)
        Me.txtPorIVA.Name = "txtPorIVA"
        Me.txtPorIVA.Size = New System.Drawing.Size(96, 20)
        Me.txtPorIVA.TabIndex = 16
        Me.txtPorIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtFactura
        '
        Me.txtFactura.Location = New System.Drawing.Point(137, 116)
        Me.txtFactura.Name = "txtFactura"
        Me.txtFactura.Size = New System.Drawing.Size(136, 20)
        Me.txtFactura.TabIndex = 7
        '
        'lblLote
        '
        Me.lblLote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLote.Location = New System.Drawing.Point(13, 137)
        Me.lblLote.Name = "lblLote"
        Me.lblLote.Size = New System.Drawing.Size(118, 19)
        Me.lblLote.TabIndex = 37
        Me.lblLote.Text = "Nº Lote inventario :"
        Me.lblLote.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFactura
        '
        Me.lblFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFactura.Location = New System.Drawing.Point(0, 116)
        Me.lblFactura.Name = "lblFactura"
        Me.lblFactura.Size = New System.Drawing.Size(131, 20)
        Me.lblFactura.TabIndex = 36
        Me.lblFactura.Text = "Nº Factura/Débito :"
        Me.lblFactura.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCausa
        '
        Me.lblCausa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCausa.Location = New System.Drawing.Point(13, 360)
        Me.lblCausa.Name = "lblCausa"
        Me.lblCausa.Size = New System.Drawing.Size(118, 19)
        Me.lblCausa.TabIndex = 35
        Me.lblCausa.Text = "Causa devolución :"
        Me.lblCausa.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPesoTotal
        '
        Me.lblPesoTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPesoTotal.Location = New System.Drawing.Point(13, 337)
        Me.lblPesoTotal.Name = "lblPesoTotal"
        Me.lblPesoTotal.Size = New System.Drawing.Size(118, 19)
        Me.lblPesoTotal.TabIndex = 34
        Me.lblPesoTotal.Text = "Peso total :"
        Me.lblPesoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCostoTotal
        '
        Me.lblCostoTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCostoTotal.Location = New System.Drawing.Point(13, 316)
        Me.lblCostoTotal.Name = "lblCostoTotal"
        Me.lblCostoTotal.Size = New System.Drawing.Size(118, 19)
        Me.lblCostoTotal.TabIndex = 33
        Me.lblCostoTotal.Text = "Costo Total :"
        Me.lblCostoTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblPorAcepta
        '
        Me.lblPorAcepta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPorAcepta.Location = New System.Drawing.Point(3, 295)
        Me.lblPorAcepta.Name = "lblPorAcepta"
        Me.lblPorAcepta.Size = New System.Drawing.Size(128, 19)
        Me.lblPorAcepta.TabIndex = 32
        Me.lblPorAcepta.Text = "% para devolución :"
        Me.lblPorAcepta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDsctoOferta
        '
        Me.lblDsctoOferta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDsctoOferta.Location = New System.Drawing.Point(13, 274)
        Me.lblDsctoOferta.Name = "lblDsctoOferta"
        Me.lblDsctoOferta.Size = New System.Drawing.Size(118, 19)
        Me.lblDsctoOferta.TabIndex = 31
        Me.lblDsctoOferta.Text = "Dscto. oferta :"
        Me.lblDsctoOferta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDsctoCliente
        '
        Me.lblDsctoCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDsctoCliente.Location = New System.Drawing.Point(13, 253)
        Me.lblDsctoCliente.Name = "lblDsctoCliente"
        Me.lblDsctoCliente.Size = New System.Drawing.Size(118, 19)
        Me.lblDsctoCliente.TabIndex = 30
        Me.lblDsctoCliente.Text = "Dscto. cliente :"
        Me.lblDsctoCliente.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDsctoMercancia
        '
        Me.lblDsctoMercancia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDsctoMercancia.Location = New System.Drawing.Point(3, 232)
        Me.lblDsctoMercancia.Name = "lblDsctoMercancia"
        Me.lblDsctoMercancia.Size = New System.Drawing.Size(128, 19)
        Me.lblDsctoMercancia.TabIndex = 29
        Me.lblDsctoMercancia.Text = "Dscto. mercancía :"
        Me.lblDsctoMercancia.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblIVA
        '
        Me.lblIVA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIVA.Location = New System.Drawing.Point(13, 211)
        Me.lblIVA.Name = "lblIVA"
        Me.lblIVA.Size = New System.Drawing.Size(118, 19)
        Me.lblIVA.TabIndex = 28
        Me.lblIVA.Text = "IVA :"
        Me.lblIVA.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDesc_cli
        '
        Me.txtDesc_cli.Location = New System.Drawing.Point(137, 253)
        Me.txtDesc_cli.Name = "txtDesc_cli"
        Me.txtDesc_cli.Size = New System.Drawing.Size(136, 20)
        Me.txtDesc_cli.TabIndex = 18
        Me.txtDesc_cli.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtDesc_art
        '
        Me.txtDesc_art.Location = New System.Drawing.Point(137, 232)
        Me.txtDesc_art.Name = "txtDesc_art"
        Me.txtDesc_art.Size = New System.Drawing.Size(136, 20)
        Me.txtDesc_art.TabIndex = 17
        Me.txtDesc_art.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtIVA
        '
        Me.txtIVA.Location = New System.Drawing.Point(137, 211)
        Me.txtIVA.MaxLength = 1
        Me.txtIVA.Name = "txtIVA"
        Me.txtIVA.Size = New System.Drawing.Size(34, 20)
        Me.txtIVA.TabIndex = 15
        Me.txtIVA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtCantidad
        '
        Me.txtCantidad.Location = New System.Drawing.Point(137, 158)
        Me.txtCantidad.Name = "txtCantidad"
        Me.txtCantidad.Size = New System.Drawing.Size(136, 20)
        Me.txtCantidad.TabIndex = 2
        Me.txtCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnComentarioAdicional
        '
        Me.btnComentarioAdicional.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnComentarioAdicional.Location = New System.Drawing.Point(495, 75)
        Me.btnComentarioAdicional.Name = "btnComentarioAdicional"
        Me.btnComentarioAdicional.Size = New System.Drawing.Size(27, 20)
        Me.btnComentarioAdicional.TabIndex = 5
        Me.btnComentarioAdicional.Text = "···"
        Me.btnComentarioAdicional.UseVisualStyleBackColor = True
        '
        'lblCantidad
        '
        Me.lblCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCantidad.Location = New System.Drawing.Point(13, 157)
        Me.lblCantidad.Name = "lblCantidad"
        Me.lblCantidad.Size = New System.Drawing.Size(118, 20)
        Me.lblCantidad.TabIndex = 18
        Me.lblCantidad.Text = "Cantidad :"
        Me.lblCantidad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtLote
        '
        Me.txtLote.Location = New System.Drawing.Point(137, 137)
        Me.txtLote.Name = "txtLote"
        Me.txtLote.Size = New System.Drawing.Size(136, 20)
        Me.txtLote.TabIndex = 9
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Location = New System.Drawing.Point(137, 39)
        Me.txtDescripcion.Multiline = True
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
        Me.cmbUnidad.FormattingEnabled = True
        Me.cmbUnidad.Location = New System.Drawing.Point(137, 94)
        Me.cmbUnidad.Name = "cmbUnidad"
        Me.cmbUnidad.Size = New System.Drawing.Size(136, 21)
        Me.cmbUnidad.TabIndex = 6
        '
        'lblUnidad
        '
        Me.lblUnidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnidad.Location = New System.Drawing.Point(13, 97)
        Me.lblUnidad.Name = "lblUnidad"
        Me.lblUnidad.Size = New System.Drawing.Size(118, 19)
        Me.lblUnidad.TabIndex = 5
        Me.lblUnidad.Text = "Unidad :"
        Me.lblUnidad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCostoPrecio
        '
        Me.lblCostoPrecio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCostoPrecio.Location = New System.Drawing.Point(13, 189)
        Me.lblCostoPrecio.Name = "lblCostoPrecio"
        Me.lblCostoPrecio.Size = New System.Drawing.Size(118, 19)
        Me.lblCostoPrecio.TabIndex = 4
        Me.lblCostoPrecio.Text = "Costo unitario :"
        Me.lblCostoPrecio.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDescripcion
        '
        Me.lblDescripcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescripcion.Location = New System.Drawing.Point(13, 41)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(118, 19)
        Me.lblDescripcion.TabIndex = 3
        Me.lblDescripcion.Text = "Descripción  :"
        Me.lblDescripcion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCodigo
        '
        Me.lblCodigo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCodigo.Location = New System.Drawing.Point(12, 19)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(118, 19)
        Me.lblCodigo.TabIndex = 1
        Me.lblCodigo.Text = "Item  :"
        Me.lblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCostoPrecio
        '
        Me.txtCostoPrecio.Location = New System.Drawing.Point(137, 189)
        Me.txtCostoPrecio.Name = "txtCostoPrecio"
        Me.txtCostoPrecio.Size = New System.Drawing.Size(136, 20)
        Me.txtCostoPrecio.TabIndex = 14
        Me.txtCostoPrecio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbCostoPrecio
        '
        Me.cmbCostoPrecio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostoPrecio.Enabled = False
        Me.cmbCostoPrecio.FormattingEnabled = True
        Me.cmbCostoPrecio.Location = New System.Drawing.Point(137, 189)
        Me.cmbCostoPrecio.Name = "cmbCostoPrecio"
        Me.cmbCostoPrecio.Size = New System.Drawing.Size(136, 21)
        Me.cmbCostoPrecio.TabIndex = 13
        '
        'grpAceptarSalir
        '
        Me.grpAceptarSalir.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpAceptarSalir.ColumnCount = 2
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.grpAceptarSalir.Controls.Add(Me.btnCancel, 1, 0)
        Me.grpAceptarSalir.Controls.Add(Me.btnOK, 0, 0)
        Me.grpAceptarSalir.Location = New System.Drawing.Point(375, 456)
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
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
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
        Me.C1SuperTooltip1.Font = New System.Drawing.Font("Tahoma", 8.0!)
        '
        'jsGenRenglonesMovimientos
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(540, 485)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpAceptarSalir)
        Me.Controls.Add(Me.grpTarjeta)
        Me.Controls.Add(Me.lblInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "jsGenRenglonesMovimientos"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento mercancía"
        Me.grpTarjeta.ResumeLayout(False)
        Me.grpTarjeta.PerformLayout()
        Me.grpIVA.ResumeLayout(False)
        Me.grpIVA.PerformLayout()
        Me.grpAceptarSalir.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents grpTarjeta As System.Windows.Forms.GroupBox
    Friend WithEvents lblUnidad As System.Windows.Forms.Label
    Friend WithEvents lblCostoPrecio As System.Windows.Forms.Label
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents lblCodigo As System.Windows.Forms.Label
    Friend WithEvents cmbUnidad As System.Windows.Forms.ComboBox
    Friend WithEvents txtDescripcion As System.Windows.Forms.TextBox
    Friend WithEvents txtCodigo As System.Windows.Forms.TextBox
    Friend WithEvents grpAceptarSalir As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cmbCostoPrecio As System.Windows.Forms.ComboBox
    Friend WithEvents lblCantidad As System.Windows.Forms.Label
    Friend WithEvents txtLote As System.Windows.Forms.TextBox
    Friend WithEvents btnComentarioAdicional As System.Windows.Forms.Button
    Friend WithEvents txtCantidad As System.Windows.Forms.TextBox
    Friend WithEvents txtCostoPrecio As System.Windows.Forms.TextBox
    Friend WithEvents txtDesc_cli As System.Windows.Forms.TextBox
    Friend WithEvents txtDesc_art As System.Windows.Forms.TextBox
    Friend WithEvents txtIVA As System.Windows.Forms.TextBox
    Friend WithEvents lblPorAcepta As System.Windows.Forms.Label
    Friend WithEvents lblDsctoOferta As System.Windows.Forms.Label
    Friend WithEvents lblDsctoCliente As System.Windows.Forms.Label
    Friend WithEvents lblDsctoMercancia As System.Windows.Forms.Label
    Friend WithEvents lblIVA As System.Windows.Forms.Label
    Friend WithEvents txtFactura As System.Windows.Forms.TextBox
    Friend WithEvents lblLote As System.Windows.Forms.Label
    Friend WithEvents lblFactura As System.Windows.Forms.Label
    Friend WithEvents lblCausa As System.Windows.Forms.Label
    Friend WithEvents lblPesoTotal As System.Windows.Forms.Label
    Friend WithEvents lblCostoTotal As System.Windows.Forms.Label
    Friend WithEvents txtPorIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtDesc_ofe As System.Windows.Forms.TextBox
    Friend WithEvents txtPesoTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtCostoPrecioTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtPorAceptaDev As System.Windows.Forms.TextBox
    Friend WithEvents btnCausa As System.Windows.Forms.Button
    Friend WithEvents btnCantidadTC As System.Windows.Forms.Button
    Friend WithEvents btnLote As System.Windows.Forms.Button
    Friend WithEvents btnFactura As System.Windows.Forms.Button
    Friend WithEvents txtCausa As System.Windows.Forms.TextBox
    Friend WithEvents btnCodigoArticulo As System.Windows.Forms.Button
    Friend WithEvents lblPesoDes As System.Windows.Forms.Label
    Friend WithEvents lblCausaDEs As System.Windows.Forms.Label
    Friend WithEvents btnPesoCaptura As System.Windows.Forms.Button
    Friend WithEvents grpIVA As System.Windows.Forms.GroupBox
    Friend WithEvents txtPrecioIVA As System.Windows.Forms.TextBox
    Friend WithEvents lblPrecioIVA As System.Windows.Forms.Label
    Friend WithEvents lbltotalIVA As System.Windows.Forms.Label
    Friend WithEvents txtTotalIVA As System.Windows.Forms.TextBox
    Friend WithEvents txtComentarioOferta As System.Windows.Forms.TextBox
    Friend WithEvents C1SuperTooltip1 As C1.Win.C1SuperTooltip.C1SuperTooltip
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents cmbTipoRenglon As System.Windows.Forms.ComboBox
    Friend WithEvents lblTipoRenglon As System.Windows.Forms.Label
    Friend WithEvents btnDescProveedor As System.Windows.Forms.Button
    Friend WithEvents lblRenglon As System.Windows.Forms.Label
End Class
