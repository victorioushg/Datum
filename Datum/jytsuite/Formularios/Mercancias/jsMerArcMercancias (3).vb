Imports MySql.Data.MySqlClient
Imports C1.Win.C1Chart
Imports System.IO

Public Class jsMerArcMercancias
    Private Const sModulo As String = "Mercancías"
    Private Const lRegion As String = "RibbonButton114"
    Private Const nTabla As String = "mercancias"
    Private Const nTablaMovimientos As String = "movimientos"
    Private Const nTablaEquivalencias As String = "equivalencias"
    Private Const nTablaCompras As String = "compras"
    Private Const nTablaVentas As String = "ventas"
    Private Const nTablaIVA As String = "iva"

    Private strSQL As String = "select * from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart "
    Private strSQLMov As String
    Private strSQLEQu As String
    Private strSQLCompras As String
    Private strSQLVentas As String
    Private strSQLIVA As String = " select tipo from jsconctaiva group by tipo "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private da As New MySqlDataAdapter(myCom)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtEquivalencias As New DataTable

    Private aIVA() As String
    Private aTipo() As String = {"Venta", "Uso interno", "POP", "Alquiler", "Préstamo", "Materia prima", "Venta & Envase", "Otros"}
    Private aMix() As String = {"Económico", "Estandar", "Superior"}
    Private aCondicion() As String = {"Activo", "Inactivo"}

    Private aTipoMovimiento() As String = {"Todos", "Entradas", "Salidas", "Ajuste de Entrada", "Ajuste de Salida", "Ajuste de Costo"}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionEqu As Long
    Private CaminoFoto As String = ""

    Private Sub jsMerArcMercancias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")
            RellenaCombo(aTipo, cmbTipo)
            RellenaCombo(aMix, cmbMIX)
            RellenaCombo(aCondicion, cmbCondicion)
            RellenaCombo(aUnidad, cmbUnidad)
            RellenaCombo(aUnidad, cmbUnidadDetal)


            DesactivarMarco0()

            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarMercancia(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            btnAgregaEquivale.Enabled = btnAgregar.Enabled
            btnEditaEquivale.Enabled = btnEditar.Enabled
            btnEliminaEquivale.Enabled = btnEliminar.Enabled
            AsignarTooltips()


        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub

    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseado")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalcular existencias</B>")

        'Botones Adicionales
        C1SuperTooltip1.SetToolTip(btnCategoria, "Seleccione la <B>categoría</B> para esta mercancía ")
        C1SuperTooltip1.SetToolTip(btnMarca, "Seleccione la <B>marca</B> de la mercancía ")
        C1SuperTooltip1.SetToolTip(btnDivision, "Seleccione la <B>división</B> para esta mercancía ")
        C1SuperTooltip1.SetToolTip(btnTipJer, "Seleccione el <B>tipo de jerarquía</B> para esta mercancía ")
        C1SuperTooltip1.SetToolTip(btnIVA, "Agregue o modifique las diferentes <B>tasas de IVA</B> ")
        C1SuperTooltip1.SetToolTip(btnTallas, "Agregue o modifique las <B>tallas y colores</B> asociados a esta mercancía ")
        C1SuperTooltip1.SetToolTip(btnCombo, "Agregue o modifique las <B>mercancías asociadas</B> a esta mercancía-combo ")
        C1SuperTooltip1.SetToolTip(btnFoto, "Anexe o cambie <B>Foto</B> a esta mercancía ")
       

        C1Chart2.ToolTip.Enabled = True
        C1Chart2.ToolTip.SelectAction = SelectActionEnum.MouseOver
        C1Chart2.ToolTip.PlotElement = PlotElementEnum.Points
        C1Chart2.ToolTip.AutomaticDelay = 0
        C1Chart2.ToolTip.InitialDelay = 0

        c1Chart1.ToolTip.Enabled = True
        c1Chart1.ToolTip.SelectAction = SelectActionEnum.MouseOver
        c1Chart1.ToolTip.PlotElement = PlotElementEnum.Points
        c1Chart1.ToolTip.AutomaticDelay = 0
        c1Chart1.ToolTip.InitialDelay = 0


    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)

        If c >= 0 AndAlso dtMovimientos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarra)
    End Sub
    Private Sub AsignaEqu(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLEQu, myConn, nTablaEquivalencias, lblInfo)
        dtEquivalencias = ds.Tables(nTablaEquivalencias)
        If c >= 0 AndAlso dtEquivalencias.Rows.Count > 0 Then
            If c > dtEquivalencias.Rows.Count - 1 Then c = dtEquivalencias.Rows.Count - 1
            Me.BindingContext(ds, nTablaEquivalencias).Position = c
            dgEqu.Refresh()
            dgEqu.CurrentCell = dgEqu(0, c)

        End If
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If dt.Rows.Count > 0 Then

            With dt

                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

                With .Rows(nRow)
                    'Mercancías
                    nPosicionCat = nRow

                    txtCodigo.Text = MuestraCampoTexto(.Item("codart"))
                    txtNombre.Text = MuestraCampoTexto(.Item("nomart"))
                    txtAlterno.Text = MuestraCampoTexto(.Item("alterno"))
                    txtBarras.Text = MuestraCampoTexto(.Item("barras"))
                    txtCategoria.Text = MuestraCampoTexto(.Item("grupo"))
                    txtMarca.Text = MuestraCampoTexto(.Item("marca"))
                    txtDivision.Text = MuestraCampoTexto(.Item("division"))
                    txtSICA.Text = MuestraCampoTexto(.Item("CODJER"))

                    txtTipJer.Text = IIf(IsDBNull(.Item("tipjer")), "", .Item("tipjer"))
                    txtCodjer1.Text = IIf(IsDBNull(.Item("codjer1")), "", .Item("codjer1"))
                    txtCodjer2.Text = IIf(IsDBNull(.Item("codjer2")), "", .Item("codjer2"))
                    txtCodjer3.Text = IIf(IsDBNull(.Item("codjer3")), "", .Item("codjer3"))
                    txtCodjer4.Text = IIf(IsDBNull(.Item("codjer4")), "", .Item("codjer4"))
                    txtCodjer5.Text = IIf(IsDBNull(.Item("codjer5")), "", .Item("codjer5"))
                    txtCodjer6.Text = IIf(IsDBNull(.Item("codjer6")), "", .Item("codjer6"))

                    txtJerarquiaNombre.Text = ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer1.Text, 1) + IIf(txtCodjer2.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer2.Text, 2) + IIf(txtCodjer3.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer3.Text, 3) + IIf(txtCodjer4.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer4.Text, 4) + IIf(txtCodjer5.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer5.Text, 5) + IIf(txtCodjer6.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer6.Text, 6)

                    txtPresentacion.Text = IIf(IsDBNull(.Item("presentacion")), "", .Item("presentacion"))
                    txtSugerido.Text = IIf(IsDBNull(.Item("sugerido")), "", FormatoNumero(CDbl(.Item("sugerido"))))
                    chkRegulada.Checked = .Item("regulado")
                    chkDevoluciones.Checked = .Item("devolucion")
                    txtPorDevoluciones.Text = IIf(IsDBNull(.Item("por_acepta_dev")), FormatoNumero(0.0), FormatoNumero(.Item("por_acepta_dev")))
                    cmbUnidad.SelectedIndex = InArray(aUnidadAbreviada, .Item("unidad")) - 1
                    If ExisteCampo(myConn, lblInfo, jytsistema.WorkDataBase, "jsmerctainv", "unidaddetal") Then _
                        cmbUnidadDetal.SelectedIndex = InArray(aUnidadAbreviada, .Item("unidaddetal")) - 1
                    txtPesoUnidad.Text = IIf(IsDBNull(.Item("pesounidad")), FormatoCantidad(0.0), FormatoCantidad(.Item("pesounidad")))
                    txtExMin.Text = IIf(IsDBNull(.Item("exmin")), FormatoCantidad(0.0), FormatoCantidad(.Item("exmin")))
                    txtExMax.Text = IIf(IsDBNull(.Item("exmax")), FormatoCantidad(0.0), FormatoCantidad(.Item("exmax")))
                    Dim aUbica() As String = Split(.Item("ubicacion"), ".")

                    txtUbica1.Text = ""
                    txtUbica2.Text = ""
                    txtUbica3.Text = ""
                    If aUbica.Length = 1 Then txtUbica1.Text = aUbica(0)
                    If aUbica.Length = 2 Then txtUbica2.Text = aUbica(1)
                    If aUbica.Length = 3 Then txtUbica3.Text = aUbica(2)

                    txtAlto.Text = IIf(IsDBNull(.Item("altura")), FormatoNumero(0.0), FormatoNumero(.Item("altura")))
                    txtAncho.Text = IIf(IsDBNull(.Item("ancho")), FormatoNumero(0.0), FormatoNumero(.Item("ancho")))
                    txtProfun.Text = IIf(IsDBNull(.Item("profun")), FormatoNumero(0.0), FormatoNumero(.Item("profun")))

                    Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
                    RellenaCombo(aIVA, cmbIVA, InArray(aIVA, .Item("iva")) - 1)

                    chkCartera.Checked = .Item("cuota")
                    chkCuotaFija.Checked = .Item("cuotafija")
                    chkDescuentos.Checked = .Item("descuento")
                    txtPrecioA.Text = FormatoNumero(.Item("precio_a"))
                    txtPrecioB.Text = FormatoNumero(.Item("precio_b"))
                    txtPrecioC.Text = FormatoNumero(.Item("precio_c"))
                    txtPrecioD.Text = FormatoNumero(.Item("precio_d"))
                    txtPrecioE.Text = FormatoNumero(.Item("precio_e"))
                    txtPrecioF.Text = FormatoNumero(.Item("precio_f"))

                    txtDescA.Text = FormatoNumero(.Item("desc_a"))
                    txtDescB.Text = FormatoNumero(.Item("desc_b"))
                    txtDescC.Text = FormatoNumero(.Item("desc_c"))
                    txtDescD.Text = FormatoNumero(.Item("desc_d"))
                    txtDescE.Text = FormatoNumero(.Item("desc_e"))
                    txtDescF.Text = FormatoNumero(.Item("desc_f"))

                    txtGanA.Text = FormatoNumero(.Item("ganan_a"))
                    txtGanB.Text = FormatoNumero(.Item("ganan_b"))
                    txtGanC.Text = FormatoNumero(.Item("ganan_c"))
                    txtGanD.Text = FormatoNumero(.Item("ganan_d"))
                    txtGanE.Text = FormatoNumero(.Item("ganan_e"))
                    txtGanF.Text = FormatoNumero(.Item("ganan_f"))

                    txtOfertaA.Text = FormatoNumero(PrecioOferta(myConn, .Item("codart"), "A", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaB.Text = FormatoNumero(PrecioOferta(myConn, .Item("codart"), "B", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaC.Text = FormatoNumero(PrecioOferta(myConn, .Item("codart"), "C", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaD.Text = FormatoNumero(PrecioOferta(myConn, .Item("codart"), "D", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaE.Text = FormatoNumero(PrecioOferta(myConn, .Item("codart"), "E", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaF.Text = FormatoNumero(PrecioOferta(myConn, .Item("codart"), "F", jytsistema.sFechadeTrabajo, lblInfo))

                    ColorOfertas(txtOfertaA, Color.AliceBlue)
                    ColorOfertas(txtOfertaB, Color.AliceBlue)
                    ColorOfertas(txtOfertaC, Color.AliceBlue)
                    ColorOfertas(txtOfertaD, Color.AliceBlue)
                    ColorOfertas(txtOfertaE, Color.AliceBlue)
                    ColorOfertas(txtOfertaF, Color.AliceBlue)

                    If ExisteCampo(myConn, lblInfo, jytsistema.WorkDataBase, "jsmerctainv", "barra_a") Then
                        txtBarraA.Text = .Item("barra_a")
                        txtBarraB.Text = .Item("barra_b")
                        txtBarraC.Text = .Item("barra_c")
                        txtBarraD.Text = .Item("barra_d")
                        txtBarraE.Text = .Item("barra_e")
                        txtBarraF.Text = .Item("barra_f")
                    End If


                    cmbTipo.SelectedIndex = .Item("tipoart")
                    cmbMIX.SelectedIndex = .Item("mix")

                    Dim gCam() As String = {"codart", "id_emp"}
                    Dim gStr() As String = {.Item("codart"), jytsistema.WorkID}

                    chkCombo.Checked = False
                    If qFound(myConn, lblInfo, "jsmercatcom", gCam, gStr) Then chkCombo.Checked = True

                    txtIngreso.Text = FormatoFecha(CDate(.Item("creacion").ToString))
                    cmbCondicion.SelectedIndex = .Item("estatus")

                    If ExisteTabla(myConn, jytsistema.WorkDataBase, "jsmerctainvfot", lblInfo) Then
                        Dim dtFoto As DataTable
                        Dim nTablaFoto As String = "tblFoto"
                        ds = DataSetRequery(ds, " select Foto1 from jsmerctainvfot where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "'  ", myConn, nTablaFoto, lblInfo)
                        dtFoto = ds.Tables(nTablaFoto)
                        If dtFoto.Rows.Count > 0 Then
                            CaminoFoto = BaseDatosAImagen(dtFoto.Rows(0), "Foto1", .Item("codart"))
                        Else
                            CaminoFoto = ""
                        End If
                        If My.Computer.FileSystem.FileExists(CaminoFoto) Then
                            Dim fs As System.IO.FileStream
                            fs = New System.IO.FileStream(CaminoFoto, IO.FileMode.Open, IO.FileAccess.Read)
                            pctFoto.Image = System.Drawing.Image.FromStream(fs)
                            fs.Close()
                        Else
                            pctFoto.Image = Nothing
                        End If
                    End If


                    'Movimientos
                    txtCodigoMovimientos.Text = .Item("codart")
                    txtNombreMovimientos.Text = IIf(IsDBNull(.Item("nomart")), "", .Item("nomart"))

                    'Compras
                    txtCodigoCompras.Text = .Item("codart")
                    txtNombreCompras.Text = IIf(IsDBNull(.Item("nomart")), "", .Item("nomart"))

                    'Ventas
                    txtCodigoVentas.Text = .Item("codart")
                    txtNombreVentas.Text = IIf(IsDBNull(.Item("nomart")), "", .Item("nomart"))

                    'Existencias
                    txtCodigoExistencias.Text = .Item("codart")
                    txtNombreExistencias.Text = IIf(IsDBNull(.Item("nomart")), "", .Item("nomart"))

                    'Expedientes
                    txtCodigoExpediente.Text = .Item("codart")
                    txtNombreExpediente.Text = IIf(IsDBNull(.Item("nomart")), "", .Item("nomart"))

                    ' AbrirMovimientos(.Item("codart"))
                    AbrirEquivalencias(.Item("codart"), .Item("unidad"))

                End With
            End With
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub AbrirEquivalencias(ByVal CodigoArticulo As String, ByVal Unidad As String)

        strSQLEQu = " select codart, unidad, equivale, uvalencia, elt(divide + 1,'No','Si') divide, id_emp from jsmerequmer " _
                       & " where codart = '" & CodigoArticulo & "' and " _
                       & " unidad = '" & Unidad & "' and " _
                       & " id_emp = '" & jytsistema.WorkID & "' order by UVALENCIA "
        ds = DataSetRequery(ds, strSQLEQu, myConn, nTablaEquivalencias, lblInfo)
        dtEquivalencias = ds.Tables(nTablaEquivalencias)

        Dim aCamEqu() As String = {"equivale", "uvalencia", "divide"}
        Dim aNomEqu() As String = {"Equivale", "UND", "Divide"}
        Dim aAncEqu() As Long = {70, 40, 30}
        Dim aAliEqu() As Integer = {AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aForEqu() As String = {sFormatoCantidadLarga, "", ""}

        IniciarTabla(dgEqu, dtEquivalencias, aCamEqu, aNomEqu, aAncEqu, aAliEqu, aForEqu, , , , False)
        If dtEquivalencias.Rows.Count > 0 Then nPosicionEqu = 0
        AsignaEqu(nPosicionEqu, False)

    End Sub
    Private Sub AbrirMovimientos(ByVal CodigoArticulo As String)

        dg.DataSource = Nothing

        strSQLMov = " SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, a.almacen, a.cantidad, a.costotal, a.costotaldes," _
                              & " a.lote, a.peso, a.origen, a.prov_cli, IF(b.nombre IS NULL, IF( c.nombre IS NULL, '', c.nombre) , b.nombre) nomProv_Cli, a.asiento, a.fechasi, a.vendedor,  " _
                              & " CONCAT(d.nombres, ' ' , d.apellidos) nomVendedor, a.id_emp " _
                              & " FROM jsmertramer a " _
                              & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp AND a.origen IN ('FAC', 'NCV', 'NDV') ) " _
                              & " LEFT JOIN jsprocatpro c ON (a.prov_cli = c.codpro AND a.id_emp = c.id_emp AND a.origen IN ('COM', 'REP', 'NCC' 'NDC') ) " _
                              & " LEFT JOIN jsvencatven d ON (a.vendedor = d.codven AND a.id_emp = d.id_emp ) " _
                              & " WHERE " _
                              & " a.codart = '" & CodigoArticulo & "' AND " _
                              & " a.id_emp = '" & jytsistema.WorkID & "' " _
                              & " ORDER BY fechamov DESC "


        'strSQLMov = " ( SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a5.unidad, a.almacen, a.cantidad, a.costotal, a.costotaldes," _
        '                      & " a.lote, a.peso, a.origen, a.prov_cli, IF(b.nombre IS NULL, IF( c.nombre IS NULL, '', c.nombre) , b.nombre) nomProv_Cli, a.asiento, a.fechasi, a.vendedor,  " _
        '                      & " CONCAT(d.nombres, ' ' , d.apellidos) nomVendedor, a.id_emp " _
        '                      & " FROM jsmertramer a " _
        '                      & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp AND a.origen IN ('FAC', 'NCV', 'NDV') ) " _
        '                      & " LEFT JOIN jsprocatpro c ON (a.prov_cli = c.codpro AND a.id_emp = c.id_emp AND a.origen IN ('COM', 'REP', 'NCC' 'NDC') ) " _
        '                      & " LEFT JOIN jsvencatven d ON (a.vendedor = d.codven AND a.id_emp = d.id_emp and d.tipo = 0 ) " _
        '                      & " WHERE " _
        '                      & " a.proV_cli <> '00000000' AND" _
        '                      & " a.origen <> 'PVE' AND " _
        '                      & " a.codart = '" & CodigoArticulo & "' AND " _
        '                      & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
        '                      & " a.id_emp = '" & jytsistema.WorkID & "') " _
        '                      & " UNION (SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, a.almacen, a.cantidad, a.costotal, a.costotaldes," _
        '                      & " a.lote, a.peso, a.origen, a.prov_cli, IF(b.nombre IS NULL, IF( c.nombre IS NULL, '', c.nombre) , b.nombre) nomProv_Cli, a.asiento, a.fechasi, a.vendedor,  " _
        '                      & " CONCAT(d.nombres, ' ', d.apellidos) nomVendedor, a.id_emp " _
        '                      & " FROM jsmertramer a " _
        '                      & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp AND a.origen IN ('FAC', 'NCV', 'NDV') ) " _
        '                      & " LEFT JOIN jsprocatpro c ON (a.prov_cli = c.codpro AND a.id_emp = c.id_emp AND a.origen IN ('COM', 'REP', 'NCC' 'NDC') ) " _
        '                      & " LEFT JOIN jsvencatven d ON (a.vendedor = d.codven AND a.id_emp = d.id_emp and d.tipo = 1 ) " _
        '                      & " WHERE " _
        '                      & " a.proV_cli = '00000000' AND" _
        '                      & " a.origen = 'PVE' AND " _
        '                      & " a.codart = '" & CodigoArticulo & "' AND " _
        '                      & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
        '                      & " a.id_emp = '" & jytsistema.WorkID & "') " _
        '                      & " ORDER BY fechamov DESC, numdoc, tipomov "

        '                      & " DATEDIFF( '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', fechamov) / 30 <= " & ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM02") & " AND" _

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        Dim aCampos() As String = {"fechamov", "tipomov", "numdoc", "unidad", "almacen", "cantidad", "costotal", "costotaldes", "peso", "origen", "prov_cli", "nomProv_cli", "vendedor", "nomvendedor"}
        Dim aNombres() As String = {"Emisión", "TP", "Documento", "UND", "Alm.", "Cantidad", "Costo Total", _
                                    "Costo Total y Desc.", "Peso", "ORG", "Prov./Cli.", "Nombre o razón social", "Ases.", "Nombre"}
        Dim aAnchos() As Long = {80, 25, 100, 35, 50, 80, 90, 90, 80, 35, 80, 230, 50, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", "", "", "", "", "", "", "", "", "", ""}
        IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        RellenaCombo(aTipoMovimiento, cmbTipoMovimiento)

    End Sub
    Private Sub IniciarMercancia(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "arttmp" & NumeroAleatorio(100000)  ' AutoCodigo(8, ds, nTabla, "codart", IIf(dt.Rows.Count > 0, dt.Rows.Count - 1, 0))
        Else
            txtCodigo.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtAlterno, txtBarras, txtNombre, txtCategoria, txtMarca, txtDivision, txtTipJer, _
                       txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6, txtJerarquiaNombre, _
                       txtPresentacion, txtUbica1, txtUbica2, txtUbica3, txtOfertaA, txtOfertaB, txtOfertaC, _
                       txtOfertaD, txtOfertaE, txtOfertaF, txtCodigoMovimientos, txtNombreMovimientos, txtBarraA, txtBarraB, _
                       txtBarraC, txtBarraD, txtBarraE, txtBarraF, txtSICA)

        IniciarTextoObjetos(FormatoItemListView.iNumero, txtPrecioA, txtPrecioB, txtPrecioC, txtPrecioD, txtPrecioE, txtPrecioF, _
                    txtDescA, txtDescB, txtDescC, txtDescD, txtDescE, txtDescF, txtGanA, txtGanB, txtGanC, _
                    txtGanD, txtGanE, txtGanF, txtPorDevoluciones, txtSugerido)

        IniciarTextoObjetos(FormatoItemListView.iCantidad, txtExMin, txtExMax, txtAlto, txtAncho, txtProfun, txtPesoUnidad)
        IniciarTextoObjetos(FormatoItemListView.iFecha, txtIngreso)

        cmbUnidad.SelectedIndex = InArray(aUnidadAbreviada, "UND") - 1
        cmbUnidadDetal.SelectedIndex = InArray(aUnidadAbreviada, "UND") - 1

        cmbIVA.SelectedIndex = InArray(aIVA, "A") - 1

        cmbTipo.SelectedIndex = 0
        cmbCondicion.SelectedIndex = 0
        cmbMIX.SelectedIndex = 1

        chkCartera.Checked = True
        chkCuotaFija.Checked = False
        chkCombo.Checked = False
        chkDescuentos.Checked = True
        chkDevoluciones.Checked = False
        chkRegulada.Checked = False
        chkTallas.Checked = False

        dg.Columns.Clear()
        dgEqu.Columns.Clear()

        pctFoto.Image = Nothing


    End Sub
    Private Sub ActivarMarco0()
        grpAceptarSalir.Visible = True

        HabilitarObjetos(False, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5, C1DockingTabPage6)

        ActivarEnGrupodeControles(tbcMercas.TabPages(0).Controls, txtCodigo)
        ActivarEnGrupodeControles(grpPrecios.Controls, txtCodigo)

        HabilitarObjetos(False, False, txtCategoria, txtCategoriaNombre, txtMarca, txtMarcaNombre, txtDivision, _
                         txtDivisionNombre, txtTipJer, txtTipjerNombre, txtCodjer1, txtCodjer2, txtCodjer3, _
                         txtCodjer4, txtCodjer5, txtCodjer6, txtJerarquiaNombre, txtIVA, txtIngreso, _
                         txtOfertaA, txtOfertaB, txtOfertaC, txtOfertaD, txtOfertaE, txtOfertaF, chkTallas)

        HabilitarObjetos(True, False, btnFoto, cmbTipo, cmbMIX, cmbCondicion)

        If i_modo = movimiento.iAgregar Then HabilitarObjetos(True, False, btnCombo)
        If i_modo = movimiento.iEditar And chkCombo.Checked Then HabilitarObjetos(True, False, btnCombo)

        If i_modo = movimiento.iAgregar Then VisualizarObjetos(True, txtGanA, txtGanB, txtGanC, txtGanD, txtGanE, txtGanF)
        If i_modo = movimiento.iEditar Then HabilitarObjetos(False, False, txtCodigo)


        ColorOfertas(txtOfertaA, Color.White)
        ColorOfertas(txtOfertaB, Color.White)
        ColorOfertas(txtOfertaC, Color.White)
        ColorOfertas(txtOfertaD, Color.White)
        ColorOfertas(txtOfertaE, Color.White)
        ColorOfertas(txtOfertaF, Color.White)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(True, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5, C1DockingTabPage6)

        HabilitarObjetos(False, True, txtCodigo, txtCodigoMovimientos, txtAlterno, txtBarras, _
                         txtNombre, txtNombreMovimientos, txtCategoria, txtCategoriaNombre, _
                         txtMarca, txtMarcaNombre, txtTipJer, txtTipjerNombre, txtDivision, _
                         txtDivisionNombre, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, _
                         txtCodjer5, txtCodjer6, txtJerarquiaNombre, txtPresentacion, txtSugerido, _
                         chkRegulada, chkDevoluciones, txtPorDevoluciones, cmbUnidad, cmbUnidadDetal, txtPesoUnidad, _
                         txtExMin, txtExMax, txtUbica1, txtUbica2, txtUbica3, txtAlto, txtAncho, _
                         txtProfun, cmbIVA, txtIVA, btnIVA, btnCategoria, btnMarca, btnDivision, _
                         btnTipJer, chkCartera, chkCuotaFija, chkDescuentos, txtPrecioA, txtPrecioB, txtPrecioC, _
                         txtPrecioD, txtPrecioE, txtPrecioF, txtDescA, txtDescB, txtDescC, txtDescD, _
                         txtDescE, txtDescF, txtGanA, txtGanB, txtGanC, txtGanD, txtGanE, txtGanF, _
                         txtOfertaA, txtOfertaB, txtOfertaC, txtOfertaD, txtOfertaE, txtOfertaF, _
                         txtBarraA, txtBarraB, txtBarraC, txtBarraD, txtBarraE, txtBarraF, _
                         chkTallas, btnTallas, chkCombo, btnCombo, btnFoto, cmbTipo, cmbMIX, txtIngreso, btnIngreso, cmbCondicion, txtSICA)

        ColorOfertas(txtOfertaA, Color.AliceBlue)
        ColorOfertas(txtOfertaB, Color.AliceBlue)
        ColorOfertas(txtOfertaC, Color.AliceBlue)
        ColorOfertas(txtOfertaD, Color.AliceBlue)
        ColorOfertas(txtOfertaE, Color.AliceBlue)
        ColorOfertas(txtOfertaF, Color.AliceBlue)


        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) < 1 Then
            VisualizarObjetos(False, txtGanA, txtGanB, txtGanC, txtGanD, txtGanE, txtGanF)
            btnAgregaEquivale.Enabled = btnAgregar.Enabled
            btnEditaEquivale.Enabled = btnEditaEquivale.Enabled
            btnEliminaEquivale.Enabled = btnEliminaEquivale.Enabled
        End If
        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub ColorOfertas(ByVal txt As TextBox, ByVal Color1 As Color)
        If ValorNumero(txt.Text) > 0 Then
            txt.BackColor = Color.PaleGreen
        Else
            txt.BackColor = Color1
        End If
    End Sub


    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = 0
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarCompras(txtCodigo.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarVentas(txtCodigo.Text)
            Case "Existencias"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExistencias(txtCodigo.Text)
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select



    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarCompras(txtCodigo.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarVentas(txtCodigo.Text)
            Case "Existencias"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExistencias(txtCodigo.Text)
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarCompras(txtCodigo.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarVentas(txtCodigo.Text)
            Case "Existencias"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExistencias(txtCodigo.Text)
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarCompras(txtCodigo.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarVentas(txtCodigo.Text)
            Case "Existencias"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExistencias(txtCodigo.Text)
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                i_modo = movimiento.iAgregar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                IniciarMercancia(True)
            Case "Movimientos"
                Dim f As New jsMerArcMercanciasMovimientos
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                f.Agregar(myConn, ds, dtMovimientos, txtCodigoMovimientos.Text, txtNombreMovimientos.Text)
                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                dtMovimientos = ds.Tables(nTablaMovimientos)
                If f.Documento <> "" Then
                    Dim row As DataRow = dtMovimientos.Select(" codart = '" & txtCodigo.Text & "' AND numdoc = '" & f.Documento & "' AND origen = 'INV'  and id_emp = '" & jytsistema.WorkID & "' ")(0)
                    nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                End If
                AsignaMov(nPosicionMov, True)
                ActualizarExistenciasPlus(myConn, txtCodigo.Text)
                AsignaTXT(nPosicionCat)
                f = Nothing
            Case "Expediente"
                Dim g As New jsMERArcMercanciasExpediente
                g.Agregar(myConn, txtCodigo.Text, cmbCondicion.SelectedIndex)
                AsignarExpediente(txtCodigo.Text)
                g.Dispose()
                g = Nothing
        End Select


    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                i_modo = movimiento.iEditar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                cmbUnidad.Enabled = Not MercanciaPoseeMovimientos(myConn, lblInfo, txtCodigo.Text)
            Case "Movimientos"
                If dtMovimientos.Rows(nPosicionMov).Item("origen") = "INV" Then
                    Dim f As New jsMerArcMercanciasMovimientos
                    f.Apuntador = nPosicionMov
                    f.Editar(myConn, ds, dtMovimientos, txtCodigoMovimientos.Text, txtNombreMovimientos.Text)
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                    dtMovimientos = ds.Tables(nTablaMovimientos)
                    If f.Documento <> "" Then
                        Dim row As DataRow = dtMovimientos.Select(" codart = '" & txtCodigo.Text & "' AND numdoc = '" & f.Documento & "' AND origen = 'INV'  and id_emp = '" & jytsistema.WorkID & "' ")(0)
                        nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                    End If
                    AsignaMov(nPosicionMov, True)
                    f = Nothing
                End If
        End Select




    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                EliminaMercancia()
            Case "Movimientos"
                EliminarMovimiento()
        End Select
    End Sub
    Private Sub EliminaMercancia()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"codart", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerequmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerextalm where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerexpmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmerctainv", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            Else
                MensajeCritico(lblInfo, "EstA MERCANCIA posee movimientos. Verifique por favor ...")
            End If
        End If
    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult

        If nPosicionMov >= 0 Then
            With dtMovimientos.Rows(nPosicionMov)
                If .Item("origen").ToString = "INV" Then
                    sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                    If sRespuesta = MsgBoxResult.Yes Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))

                        Dim aCamposDel() As String = {"codart", "numdoc", "origen", "Ejercicio", "id_emp"}
                        Dim aFieldsDel() As String = {.Item("codart"), .Item("numdoc"), "INV", jytsistema.WorkExercise, jytsistema.WorkID}

                        nPosicionMov = EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jsmertramer", strSQLMov, aCamposDel, aFieldsDel, nPosicionMov)

                        ActualizarExistenciasPlus(myConn, txtCodigo.Text)

                        AsignaTXT(nPosicionCat)
                        AsignaMov(nPosicionMov, False)

                    End If
                End If
            End With
        Else
            MensajeCritico(lblInfo, "Movimiento proveniente de " & dtMovimientos.Rows(nPosicionMov).Item("origen") & ". ")
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                Dim Campos() As String = {"codart", "nomart", "alterno", "barras"}
                Dim Nombres() As String = {"Código", "Nombre", "Código alterno", "Código barras"}
                Dim Anchos() As Long = {120, 850, 100, 120}
                f.Text = "Mercancias"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Mercancías...")
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
            Case "Movimientos"
                Dim Campos() As String = {"fechamov", "numdoc", "prov_cli", "nomProv_Cli"}
                Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Código Prov./Cli.", "Cliente ó Proveedor"}
                Dim Anchos() As Long = {100, 120, 120, 500}
                f.Text = "Movimientos de mercancía"
                f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de mercancías...")
                nPosicionMov = f.Apuntador
                Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
                AsignaMov(nPosicionMov, False)
        End Select

        f = Nothing


    End Sub

    'Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
    '    'Dim f As New jsMerRepParametros
    '    'Select Case tbcMercas.SelectedTab.Text
    '    '    Case "Mercancías"
    '    '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cFichaMercancia, "Ficha Mercancía", txtCodigo.Text)
    '    '    Case "Movimientos"
    '    '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMovimientosMercancia, "Movimientos de mercancía", txtCodigoMovimientos.Text)
    '    'End Select
    '    'f.Dispose()
    '    'f = Nothing
    'End Sub


    Private Function Validado() As Boolean
        Validado = False

        '////// ESTA VALIDAR UNIDAD DE VENTA AL DETAL
        If cmbUnidadDetal.SelectedIndex >= 0 Then

            If EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT a.unidad from ( SELECT '" & aUnidadAbreviada(cmbUnidadDetal.SelectedIndex) & "' unidad  " _
                                     & " UNION " _
                                     & " SELECT uvalencia unidad FROM jsmerequmer " _
                                     & " WHERE " _
                                     & " codart = '" & txtCodigo.Text & "' AND " _
                                     & " id_emp = '" & jytsistema.WorkID & "') a WHERE '" & aUnidadAbreviada(cmbUnidadDetal.SelectedIndex) & "' = a.unidad").ToString = "0" Then

                MensajeCritico(lblInfo, "UNIDAD DE VENTA AL DETAL NO PERMITIDA. VERIFIQUE POR FAVOR...")
                Return False
            End If

        End If

        If i_modo = movimiento.iAgregar AndAlso Mid(txtCodigo.Text, 1, 6) <> "arttmp" Then
            If EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString <> "0" Then
                MensajeCritico(lblInfo, "CODIGO DE MERCANCIA YA EXISTE. VERIFIQUE POR FAVOR...")
                Return False
            End If
        End If

        If txtBarras.Text <> "" Then

            If i_modo = movimiento.iAgregar Then
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select COUNT(*) from jsmerctainv " _
                                         & " where " _
                                         & " BARRAS = '" & txtBarras.Text & "' and " _
                                         & " id_emp = '" & jytsistema.WorkID & "' ")) > 0 Then

                    MensajeCritico(lblInfo, "CODIGO DE BARRA  YA EXISTE. VERIFIQUE POR FAVOR...")
                    Return False
                End If
            End If


            If txtBarraA.Text = txtBarras.Text Then
                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS ES IGUAL AL CODIGO DE BARRAS 'A'. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If txtBarraB.Text = txtBarras.Text Then
                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS ES IGUAL AL CODIGO DE BARRAS 'B'. VERIFIQUE POR FAVOR...")
                    Return False
                Else
                    If txtBarraC.Text = txtBarras.Text Then
                        MensajeCritico(lblInfo, "EL CODIGO DE BARRAS ES IGUAL AL CODIGO DE BARRAS 'C'. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If txtBarraD.Text = txtBarras.Text Then
                            MensajeCritico(lblInfo, "EL CODIGO DE BARRAS ES IGUAL AL CODIGO DE BARRAS 'D'. VERIFIQUE POR FAVOR...")
                            Return False
                        Else
                            If txtBarraE.Text = txtBarras.Text Then
                                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS ES IGUAL AL CODIGO DE BARRAS 'E'. VERIFIQUE POR FAVOR...")
                                Return False
                            Else
                                If txtBarraF.Text = txtBarras.Text Then
                                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS ES IGUAL AL CODIGO DE BARRAS 'F'. VERIFIQUE POR FAVOR...")
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If


        End If

        If txtBarraA.Text <> "" Then

            If i_modo = movimiento.iAgregar Then
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select COUNT(*) from jsmerctainv " _
                                         & " WHERE " _
                                         & " BARRA_A = '" & txtBarraA.Text & "' and " _
                                         & " id_emp = '" & jytsistema.WorkID & "' ")) > 0 Then

                    MensajeCritico(lblInfo, "CODIGO DE BARRA *A* YA EXISTE. VERIFIQUE POR FAVOR...")
                    Return False
                End If
            End If

            If txtBarras.Text = txtBarraA.Text Then
                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'A' ES IGUAL AL CODIGO DE BARRAS. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If txtBarraB.Text = txtBarraA.Text Then
                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'A' ES IGUAL AL CODIGO DE BARRAS 'B'. VERIFIQUE POR FAVOR...")
                    Return False
                Else
                    If txtBarraC.Text = txtBarraA.Text Then
                        MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'A' ES IGUAL AL CODIGO DE BARRAS 'C'. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If txtBarraD.Text = txtBarraA.Text Then
                            MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'A' ES IGUAL AL CODIGO DE BARRAS 'D'. VERIFIQUE POR FAVOR...")
                            Return False
                        Else
                            If txtBarraE.Text = txtBarraA.Text Then
                                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'A' ES IGUAL AL CODIGO DE BARRAS 'E'. VERIFIQUE POR FAVOR...")
                                Return False
                            Else
                                If txtBarraF.Text = txtBarraA.Text Then
                                    MensajeCritico(lblInfo, "EL CODIGO DE  BARRAS 'A' ES IGUAL AL CODIGO DE BARRAS 'F'. VERIFIQUE POR FAVOR...")
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If txtBarraB.Text <> "" Then

            If i_modo = movimiento.iAgregar Then
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select COUNT(*) from jsmerctainv " _
                                         & " WHERE " _
                                         & " BARRA_B = '" & txtBarraB.Text & "' and " _
                                         & " ID_EMP = '" & jytsistema.WorkID & "' ")) > 0 Then

                    MensajeCritico(lblInfo, "CODIGO DE BARRA *B* YA EXISTE. VERIFIQUE POR FAVOR...")
                    Return False
                End If
            End If

            If txtBarras.Text = txtBarraB.Text Then
                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'B' ES IGUAL AL CODIGO DE BARRAS. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If txtBarraA.Text = txtBarraB.Text Then
                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'B' ES IGUAL AL CODIGO DE BARRAS 'A'. VERIFIQUE POR FAVOR...")
                    Return False
                Else
                    If txtBarraC.Text = txtBarraB.Text Then
                        MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'B' ES IGUAL AL CODIGO DE BARRAS 'C'. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If txtBarraD.Text = txtBarraB.Text Then
                            MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'B' ES IGUAL AL CODIGO DE BARRAS 'D'. VERIFIQUE POR FAVOR...")
                            Return False
                        Else
                            If txtBarraE.Text = txtBarraB.Text Then
                                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'B' ES IGUAL AL CODIGO DE BARRAS 'E'. VERIFIQUE POR FAVOR...")
                                Return False
                            Else
                                If txtBarraF.Text = txtBarraB.Text Then
                                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'B' ES IGUAL AL CODIGO DE BARRAS 'F'. VERIFIQUE POR FAVOR...")
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If txtBarraC.Text <> "" Then

            If i_modo = movimiento.iAgregar Then
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select COUNT(*) from jsmerctainv " _
                                         & " WHERE " _
                                         & " BARRA_C = '" & txtBarraC.Text & "' and " _
                                         & " ID_EMP = '" & jytsistema.WorkID & "' ")) > 0 Then

                    MensajeCritico(lblInfo, "CODIGO DE BARRA *C* YA EXISTE. VERIFIQUE POR FAVOR...")
                    Return False
                End If
            End If


            If txtBarras.Text = txtBarraC.Text Then
                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'C' ES IGUAL AL CODIGO DE BARRAS. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If txtBarraA.Text = txtBarraC.Text Then
                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'C' ES IGUAL AL CODIGO DE BARRAS 'A'. VERIFIQUE POR FAVOR...")
                    Return False
                Else
                    If txtBarraB.Text = txtBarraC.Text Then
                        MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'C' ES IGUAL AL CODIGO DE BARRAS 'B'. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If txtBarraD.Text = txtBarraC.Text Then
                            MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'C' ES IGUAL AL CODIGO DE BARRAS 'D'. VERIFIQUE POR FAVOR...")
                            Return False
                        Else
                            If txtBarraE.Text = txtBarraC.Text Then
                                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'C' ES IGUAL AL CODIGO DE BARRAS 'E'. VERIFIQUE POR FAVOR...")
                                Return False
                            Else
                                If txtBarraF.Text = txtBarraC.Text Then
                                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'C' ES IGUAL AL CODIGO DE BARRAS 'F'. VERIFIQUE POR FAVOR...")
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If


        If txtBarraD.Text <> "" Then

            If i_modo = movimiento.iAgregar Then
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select COUNT(*) from jsmerctainv " _
                                         & " WHERE " _
                                         & " BARRA_D = '" & txtBarraD.Text & "' and " _
                                         & " ID_EMP = '" & jytsistema.WorkID & "' ")) > 0 Then

                    MensajeCritico(lblInfo, "CODIGO DE BARRA *D* YA EXISTE. VERIFIQUE POR FAVOR...")
                    Return False
                End If
            End If

            If txtBarras.Text = txtBarraD.Text Then
                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'D' ES IGUAL AL CODIGO DE BARRAS. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If txtBarraA.Text = txtBarraD.Text Then
                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'D' ES IGUAL AL CODIGO DE BARRAS 'A'. VERIFIQUE POR FAVOR...")
                    Return False
                Else
                    If txtBarraB.Text = txtBarraD.Text Then
                        MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'D' ES IGUAL AL CODIGO DE BARRAS 'B'. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If txtBarraC.Text = txtBarraD.Text Then
                            MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'D' ES IGUAL AL CODIGO DE BARRAS 'C'. VERIFIQUE POR FAVOR...")
                            Return False
                        Else
                            If txtBarraE.Text = txtBarraD.Text Then
                                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'D' ES IGUAL AL CODIGO DE BARRAS 'D'. VERIFIQUE POR FAVOR...")
                                Return False
                            Else
                                If txtBarraF.Text = txtBarraD.Text Then
                                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'D' ES IGUAL AL CODIGO DE BARRAS 'F'. VERIFIQUE POR FAVOR...")
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If txtBarraE.Text <> "" Then

            If i_modo = movimiento.iAgregar Then
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select COUNT(*) from jsmerctainv " _
                                         & " WHERE " _
                                         & " BARRA_E = '" & txtBarraE.Text & "' and " _
                                         & " ID_EMP = '" & jytsistema.WorkID & "' ")) > 0 Then

                    MensajeCritico(lblInfo, "CODIGO DE BARRA *E* YA EXISTE. VERIFIQUE POR FAVOR...")
                    Return False
                End If
            End If



            If txtBarras.Text = txtBarraE.Text Then
                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'E' ES IGUAL AL CODIGO DE BARRAS. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If txtBarraA.Text = txtBarraE.Text Then
                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'E' ES IGUAL AL CODIGO DE BARRAS 'A'. VERIFIQUE POR FAVOR...")
                    Return False
                Else
                    If txtBarraB.Text = txtBarraE.Text Then
                        MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'E' ES IGUAL AL CODIGO DE BARRAS 'B'. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If txtBarraC.Text = txtBarraE.Text Then
                            MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'E' ES IGUAL AL CODIGO DE BARRAS 'C'. VERIFIQUE POR FAVOR...")
                            Return False
                        Else
                            If txtBarraD.Text = txtBarraE.Text Then
                                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'E' ES IGUAL AL CODIGO DE BARRAS 'D'. VERIFIQUE POR FAVOR...")
                                Return False
                            Else
                                If txtBarraF.Text = txtBarraE.Text Then
                                    MensajeCritico(lblInfo, "EL CODIGO DE  BARRAS 'E' ES IGUAL AL CODIGO DE BARRAS 'F'. VERIFIQUE POR FAVOR...")
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If txtBarraF.Text <> "" Then

            If i_modo = movimiento.iAgregar Then
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select COUNT(*) from jsmerctainv " _
                                         & " WHERE " _
                                         & " BARRA_F = '" & txtBarraF.Text & "' and " _
                                         & " ID_EMP = '" & jytsistema.WorkID & "' ")) > 0 Then

                    MensajeCritico(lblInfo, "CODIGO DE BARRA *F* YA EXISTE. VERIFIQUE POR FAVOR...")
                    Return False
                End If
            End If

            If txtBarras.Text = txtBarraF.Text Then
                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'F' ES IGUAL AL CODIGO DE BARRAS. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If txtBarraA.Text = txtBarraF.Text Then
                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'F' ES IGUAL AL CODIGO DE BARRAS 'A'. VERIFIQUE POR FAVOR...")
                    Return False
                Else
                    If txtBarraB.Text = txtBarraF.Text Then
                        MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'F' ES IGUAL AL CODIGO DE BARRAS 'B'. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If txtBarraC.Text = txtBarraF.Text Then
                            MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'F' ES IGUAL AL CODIGO DE BARRAS 'C'. VERIFIQUE POR FAVOR...")
                            Return False
                        Else
                            If txtBarraD.Text = txtBarraF.Text Then
                                MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'F' ES IGUAL AL CODIGO DE BARRAS 'D'. VERIFIQUE POR FAVOR...")
                                Return False
                            Else
                                If txtBarraE.Text = txtBarraF.Text Then
                                    MensajeCritico(lblInfo, "EL CODIGO DE BARRAS 'F' ES IGUAL AL CODIGO DE BARRAS 'E'. VERIFIQUE POR FAVOR...")
                                    Return False
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If


        If CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM01")) AndAlso txtTipjerNombre.Text = "" Then
            MensajeCritico(lblInfo, "  DEBE INDICAR UNA JERARQUIA VALIDA...")
            Return False
        End If

        If CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM03")) AndAlso txtMarcaNombre.Text = "" Then
            MensajeCritico(lblInfo, "  DEBE INDICAR UNA MARCA VALIDA...")
            Return False
        End If
        If CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM04")) AndAlso txtCategoriaNombre.Text = "" Then
            MensajeCritico(lblInfo, "  DEBE INDICAR UNA CATEGORIA VALIDA...")
            Return False
        End If
        If CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM05")) AndAlso txtDivisionNombre.Text = "" Then
            MensajeCritico(lblInfo, "  DEBE INDICAR UNA DIVISION VALIDA...")
            Return False
        End If

        If CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM06")) AndAlso ValorNumero(txtPesoUnidad.Text) = 0.0 Then
            MensajeCritico(lblInfo, "  DEBE INDICAR EL PESO DE LA UNIDAD DE VENTA VALIDO...")
            Return False
        End If

        If CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM08")) Then
            Dim equivalenciaAVerificar As String = CStr(ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM07"))


            If Not MercanciaPoseeEquivalencia(myConn, lblInfo, txtCodigo.Text, equivalenciaAVerificar) Then
                If aUnidadAbreviada(cmbUnidad.SelectedIndex) <> equivalenciaAVerificar Then
                    MensajeCritico(lblInfo, "  DEBE indicar la equivalencia de esta mercancías en " & equivalenciaAVerificar & " ...")
                    Return False
                End If
            End If


        End If


        If cmbUnidadDetal.SelectedIndex < 0 Then cmbUnidadDetal.SelectedIndex = cmbUnidad.SelectedIndex

        Validado = True

    End Function
    Private Sub GuardarTXT()

        Dim MyData() As Byte
        Dim Inserta As Boolean = False
        Dim CodigoArticulo As String = txtCodigo.Text
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            nPosicionCat = ds.Tables(nTabla).Rows.Count
            If Mid(txtCodigo.Text, 1, 6) = "arttmp" Then
                CodigoArticulo = AutoCodigo(8, ds, nTabla, "codart", IIf(dt.Rows.Count > 0, dt.Rows.Count - 1, 0))
            Else
                CodigoArticulo = txtCodigo.Text
            End If

            EjecutarSTRSQL(myConn, lblInfo, " update jsmerequmer set codart = '" & CodigoArticulo & "' where codart = '" & txtCodigo.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsmerexpmer set codart = '" & CodigoArticulo & "' where codart = '" & txtCodigo.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsmercatcom set codart = '" & CodigoArticulo & "' where codart = '" & txtCodigo.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")

        End If

        InsertEditMERCASMercancia(myConn, lblInfo, Inserta, CodigoArticulo, txtAlterno.Text, txtBarras.Text, txtNombre.Text, _
                                    txtCategoria.Text, txtMarca.Text, txtDivision.Text, txtTipJer.Text, _
                                    txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, _
                                    txtPresentacion.Text, CDbl(txtSugerido.Text), IIf(chkRegulada.Checked, 1, 0), IIf(chkDevoluciones.Checked, 1, 0), _
                                    CDbl(txtPorDevoluciones.Text), aUnidadAbreviada(cmbUnidad.SelectedIndex), CDbl(txtPesoUnidad.Text), IIf(dtEquivalencias.Rows.Count > 0, "1", IIf(aUnidadAbreviada(cmbUnidad.SelectedIndex) = "KGR", "1", "0")), aUnidadAbreviada(cmbUnidadDetal.SelectedIndex), _
                                    CDbl(txtExMin.Text), CDbl(txtExMax.Text), txtUbica1.Text, txtUbica2.Text, txtUbica3.Text, _
                                    CDbl(txtAlto.Text), CDbl(txtAncho.Text), CDbl(txtProfun.Text), cmbIVA.Text, _
                                    IIf(chkCartera.Checked, 1, 0), IIf(chkCuotaFija.Checked, 1, 0), If(chkDescuentos.Checked, 1, 0), CDbl(txtPrecioA.Text), _
                                    CDbl(txtPrecioB.Text), CDbl(txtPrecioC.Text), CDbl(txtPrecioD.Text), CDbl(txtPrecioE.Text), _
                                    CDbl(txtPrecioF.Text), CDbl(txtDescA.Text), CDbl(txtDescB.Text), CDbl(txtDescC.Text), _
                                    CDbl(txtDescD.Text), CDbl(txtDescE.Text), CDbl(txtDescF.Text), CDbl(txtGanA.Text), _
                                    CDbl(txtGanB.Text), CDbl(txtGanC.Text), CDbl(txtGanD.Text), CDbl(txtGanE.Text), CDbl(txtGanF.Text), _
                                    txtBarraA.Text, txtBarraB.Text, txtBarraC.Text, txtBarraD.Text, txtBarraE.Text, txtBarraF.Text, _
                                     cmbTipo.SelectedIndex, cmbMIX.SelectedIndex, CDate(txtIngreso.Text), cmbCondicion.SelectedIndex, _
                                    txtSICA.Text)

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, CodigoArticulo)

        If Not pctFoto.Image Is Nothing Then
            If CaminoFoto <> "" Then
                Dim fs As New FileStream(CaminoFoto, FileMode.OpenOrCreate, FileAccess.Read)
                MyData = New Byte(fs.Length) {}
                fs.Read(MyData, 0, fs.Length)
                fs.Close()
                GuardarFotoMercancia(myConn, lblInfo, CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT COUNT( DISTINCT codart ) FROM jsmerctainvfot WHERE codart = '" & CodigoArticulo & "' AND id_emp = '" & jytsistema.WorkID & "' ").ToString), CodigoArticulo, MyData)
            End If
        End If

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODART = '" & CodigoArticulo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat

        ActualizaExistenciasEnAlmacen(myConn, CodigoArticulo, lblInfo)
        AsignaTXT(nPosicionCat)


        DesactivarMarco0()
        tbcMercas.SelectedTab = C1DockingTabPage1
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtAlterno.GotFocus, txtAlterno.GotFocus, txtBarras.GotFocus, txtNombre.GotFocus, txtPresentacion.GotFocus, _
        txtPorDevoluciones.GotFocus, txtExMin.GotFocus, txtExMax.GotFocus, txtPesoUnidad.GotFocus, txtUbica1.GotFocus, _
        txtUbica2.GotFocus, txtUbica3.GotFocus, txtAlto.GotFocus, txtAncho.GotFocus, txtProfun.GotFocus, _
        txtPrecioA.GotFocus, txtPrecioB.GotFocus, txtPrecioC.GotFocus, txtPrecioD.GotFocus, _
        txtPrecioE.GotFocus, txtPrecioF.GotFocus, txtDescA.GotFocus, txtDescB.GotFocus, txtDescC.GotFocus, _
        txtDescD.GotFocus, txtDescE.GotFocus, txtDescF.GotFocus, txtGanA.GotFocus, txtGanB.GotFocus, txtGanC.GotFocus, _
        txtGanD.GotFocus, txtGanE.GotFocus, txtGanF.GotFocus, txtSugerido.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, " Indique el código de la mercancía ... ", TipoMensaje.iInfo)
            Case "txtAlterno"
                MensajeEtiqueta(lblInfo, " Indique el código alterno de la mercancía ... ", TipoMensaje.iInfo)
            Case "txtBarras"
                MensajeEtiqueta(lblInfo, " Indique el código de barras de la mercancía ... ", TipoMensaje.iInfo)
            Case "txtNombre"
                MensajeEtiqueta(lblInfo, " Indique el nombre o descripción de la mercancía ... ", TipoMensaje.iInfo)
            Case "txtPresentacion"
                MensajeEtiqueta(lblInfo, " Indique el presentación de la mercancías (ej. 12x185g = 12 unidades de 185 gramos) ... ", TipoMensaje.iInfo)
            Case "txtSugerido"
                MensajeEtiqueta(lblInfo, " Indique el precio de venta sugerido por proveedor ... ", TipoMensaje.iInfo)
            Case "txtPorDevoluciones"
                MensajeEtiqueta(lblInfo, " Indique el porcentaje para aceptación de devolución de mercancía (ej. 70% = se devuelve por 70 por ciento del precio con el que fue vendida ... ", TipoMensaje.iInfo)
            Case "txtExMin"
                MensajeEtiqueta(lblInfo, " Indique el existencia mínima de esta mercancía ... ", TipoMensaje.iInfo)
            Case "txtExMax"
                MensajeEtiqueta(lblInfo, " Indique el existencia máxima de esta mercancía ... ", TipoMensaje.iInfo)
            Case "txtPesoUnidad"
                MensajeEtiqueta(lblInfo, " Indique el peso de la unidad de venta ... ", TipoMensaje.iInfo)
            Case "txtUbica1", "txtUbica2", "txtUbica3"
                MensajeEtiqueta(lblInfo, " Indique la ubicación de la mercancia ... ", TipoMensaje.iInfo)
            Case "txtAlto"
                MensajeEtiqueta(lblInfo, " Indique el alto de la mercancía en metros (ej. 0.25 mtr ) ... ", TipoMensaje.iInfo)
            Case "txtAncho"
                MensajeEtiqueta(lblInfo, " Indique el ancho de la mercancía en metros (ej. 0.25 mtr ) ... ", TipoMensaje.iInfo)
            Case "txtProfun"
                MensajeEtiqueta(lblInfo, " Indique el profundidad de la mercancía en metros (ej. 0.25 mtr ) ... ", TipoMensaje.iInfo)
            Case "txtPrecioA", "txtPrecioB", "txtPrecioC", "txtPrecioD", "txtPrecioE", "txtPrecioF"
                MensajeEtiqueta(lblInfo, " Indique el precio de venta de la mercancía ... ", TipoMensaje.iInfo)
            Case "txtDescA", "txtDescB", "txtDescC", "txtDescD", "txtDescE", "txtDescF"
                MensajeEtiqueta(lblInfo, " Indique el descuento de venta por precio de la mercancía ... ", TipoMensaje.iInfo)
            Case "txtGanA", "txtGanB", "txtGanC", "txtGanD", "txtGanE", "txtGanF"
                MensajeEtiqueta(lblInfo, " Indique la ganancia por precio de la mercancía ... ", TipoMensaje.iInfo)
        End Select

    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        ' Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex

        Select Case dg.Columns(e.ColumnIndex).Name
            Case "cantidad", "peso"
                e.Value = FormatoCantidad(CDbl(e.Value))
            Case "fechamov"
                e.Value = FormatoFecha(CDate(e.Value.ToString))
            Case "costotal", "costotaldes"
                e.Value = FormatoNumero(CDbl(e.Value))
        End Select

    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcMercas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcMercas.SelectedIndexChanged

        Select Case tbcMercas.SelectedIndex
            Case 0 ' Mercancias
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B></B>")
                AsignaTXT(nPosicionCat)
            Case 1 ' Movimientos
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalcular costos de mercancías</B>")
                AbrirMovimientos(txtCodigo.Text)
                nPosicionMov = 0

                AsignaMov(nPosicionMov, True)
                Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
                dg.Enabled = True
            Case 2 ' Compras
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B></B>")
                AsignarCompras(txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 3 ' Ventas
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B></B>")
                AsignarVentas(txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 4 ' Existencias
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalcular existencias</B>")
                AsignarExistencias(txtCodigo.Text)
            Case 5 ' Expediente
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B></B>")
                AsignarExpediente(txtCodigo.Text)

        End Select
    End Sub
    Private Sub AsignarCompras(ByVal nArticulo As String)

        HabilitarObjetos(False, True, txtCodigoCompras, txtNombreCompras, txtOrdenes, txtRecepciones, txtBackorder, _
            txtFechaUltimaCompr, txtUltimoProveedor, txtEntradas, txtCostoAcum, txtCostoAcumDesc, txtUltimoCosto, _
            txtDevolucionesCompras, txtCostoAcumDev, txtCostoAcumDevDesc, txtCostoPromedio, txtNombreUltimoProveedor)

        If dt.Rows.Count > 0 Then
            With dt.Rows(nPosicionCat)

                txtOrdenes.Text = FormatoCantidad(.Item("ORDENES"))
                txtRecepciones.Text = FormatoCantidad(.Item("RECEPCIONES"))
                txtBackorder.Text = FormatoCantidad(.Item("BACKORDER"))
                txtFechaUltimaCompr.Text = FormatoFecha(CDate(.Item("FECULTCOSTO").ToString))
                txtUltimoProveedor.Text = IIf(IsDBNull(.Item("ULTIMOPROVEEDOR")), "", .Item("ultimoproveedor"))
                txtEntradas.Text = FormatoCantidad(.Item("ENTRADAS"))
                txtCostoAcum.Text = FormatoNumero(.Item("ACU_COS"))
                txtCostoAcumDesc.Text = FormatoNumero(.Item("ACU_COS_DES"))
                txtUltimoCosto.Text = FormatoNumero(.Item("montoultimacompra"))
                txtDevolucionesCompras.Text = FormatoCantidad(.Item("CREDITOSCOMPRAS"))
                txtCostoAcumDev.Text = FormatoNumero(.Item("ACU_COD"))
                txtCostoAcumDevDesc.Text = FormatoNumero(.Item("ACU_COD_DES"))
                txtCostoPromedio.Text = FormatoNumero(.Item("COSTO_PROM_DES"))

                txtUltimoCosto.Text = FormatoNumero(UltimoCostoAFecha(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))
                Dim xx As String = UltimoProveedor(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo)
                txtUltimoProveedor.Text = IIf(xx = "0", "", xx)
                txtFechaUltimaCompr.Text = FormatoFecha(UltimaFechaCompra(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))

            End With

            rBtn1.Checked = True
            VerHistograma(c1Chart1, 0, txtCodigo.Text, 0)

        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)


    End Sub
    Private Function ValoresMensuales(ByVal dtValores As DataTable) As Double()
        Dim aMes As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        If dtValores.Rows.Count > 0 Then
            With dtValores.Rows(0)
                aMes(0) = CDbl(.Item("ENE"))
                aMes(1) = CDbl(.Item("FEB"))
                aMes(2) = CDbl(.Item("MAR"))
                aMes(3) = CDbl(.Item("ABR"))
                aMes(4) = CDbl(.Item("MAY"))
                aMes(5) = CDbl(.Item("JUN"))
                aMes(6) = CDbl(.Item("JUL"))
                aMes(7) = CDbl(.Item("AGO"))
                aMes(8) = CDbl(.Item("SEP"))
                aMes(9) = CDbl(.Item("OCT"))
                aMes(10) = CDbl(.Item("NOV"))
                aMes(11) = CDbl(.Item("DIC"))
            End With
        End If
        ValoresMensuales = aMes

    End Function
    Private Sub VerHistograma(ByVal Histograma As C1Chart, ByVal Compras_o_Ventas As Integer, ByVal nArticulo As String, ByVal CantidadKilosMoney As Integer)

        Dim Area As Area = Histograma.ChartArea
        Dim ax As Axis = Area.AxisX
        Dim ay As Axis = Area.AxisY

        ax.ValueLabels.Clear()
        ax.ValueLabels.Add(1, "Ene")
        ax.ValueLabels.Add(2, "Feb")
        ax.ValueLabels.Add(3, "Mar")
        ax.ValueLabels.Add(4, "Abr")
        ax.ValueLabels.Add(5, "May")
        ax.ValueLabels.Add(6, "Jun")
        ax.ValueLabels.Add(7, "Jul")
        ax.ValueLabels.Add(8, "Ago")
        ax.ValueLabels.Add(9, "Sep")
        ax.ValueLabels.Add(10, "Oct")
        ax.ValueLabels.Add(11, "Nov")
        ax.ValueLabels.Add(12, "Dic")

        Dim aaY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim abY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Dim dtt As DataTable = IIf(Compras_o_Ventas = 0, TablaCompras(nArticulo, CantidadKilosMoney), _
                                   TablaVentas(nArticulo, CantidadKilosMoney))
        Dim dttAnteriores As DataTable = IIf(Compras_o_Ventas = 0, TablaCompras(nArticulo, CantidadKilosMoney, 1), _
                                             TablaVentas(nArticulo, CantidadKilosMoney, 1))

        aaY = ValoresMensuales(dtt)
        abY = ValoresMensuales(dttAnteriores)

        Dim aFFld() As String = {"id_emp"}
        Dim aSStr() As String = {jytsistema.WorkID}

        ay.Text = IIf(CantidadKilosMoney = 0, "Unidad de Venta", IIf(CantidadKilosMoney = 1, "Kilogramos", qFoundAndSign(myConn, lblInfo, "jsconctaemp", aFFld, aSStr, "moneda")))
        ax.Text = IIf(Compras_o_Ventas = 0, "Compras mes a mes", "Ventas mes a mes")

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Label = Year(jytsistema.sFechadeTrabajo)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Label = Year(jytsistema.sFechadeTrabajo) - 1

        dtt = Nothing
        dttAnteriores = Nothing


    End Sub

    Private Function TablaCompras(ByVal nArticulo As String, ByVal CantidadKilosMoney As Integer, Optional ByVal AñosAtras As Integer = 0) As DataTable
        Dim strCantidad As String, strKilos As String, strMoney As String

        strCantidad = "if( a.origen in ('COM','NDC'), if( a.tipomov <> 'AC', a.cantidad , 0 )  , if( a.tipomov <> 'AC', -1*a.cantidad ,0) ) "
        strKilos = "if( a.origen in ('COM','NDC'), if( a.tipomov <> 'AC', a.peso , 0 )  , if( a.tipomov <> 'AC', -1*a.peso ,0) ) "
        strMoney = "if( a.origen in ('COM','NDC'),  a.costotaldes   ,  -1*a.costotaldes ) "

        Dim nTablaCompras As String = "tablacompras" + AñosAtras.ToString
        Dim strSQLCompras As String
        Select Case CantidadKilosMoney
            Case 0 ' Cantidad
                strSQLCompras = " select a.CODART, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 1, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 1, " & strCantidad & "/EQUIVALE,0))) AS ENE, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 2, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 2, " & strCantidad & "/EQUIVALE,0))) AS FEB, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 3, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 3, " & strCantidad & "/EQUIVALE,0))) AS MAR, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 4, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 4, " & strCantidad & "/EQUIVALE,0))) AS ABR, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 5, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 5, " & strCantidad & "/EQUIVALE,0))) AS MAY, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 6, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 6, " & strCantidad & "/EQUIVALE,0))) AS JUN, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 7, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 7, " & strCantidad & "/EQUIVALE,0))) AS JUL, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 8, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 8, " & strCantidad & "/EQUIVALE,0))) AS AGO, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 9, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 9, " & strCantidad & "/EQUIVALE,0))) AS SEP, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 10, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 10, " & strCantidad & "/EQUIVALE,0))) AS OCT, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 11, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 11, " & strCantidad & "/EQUIVALE,0))) AS NOV, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 12, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 12, " & strCantidad & "/EQUIVALE,0))) AS DIC " _
                & " from " _
                & " jsmertramer a LEFT JOIN jsmerequmer b ON " _
                & " a.CODART = b.CODART AND a.UNIDAD = b.UVALENCIA " _
                & " WHERE " _
                & " a.CODART = '" & nArticulo & "' AND " _
                & " a.origen in ('COM','NDC','NCC')  AND " _
                & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " group by a.CODART "
            Case 1 ' Kilos
                strSQLCompras = "select a.CODART, " _
                                & "SUM(IF(MONTH(a.FECHAMOV) = 1, " & strKilos & ", 0)) AS ENE, SUM(IF(MONTH(a.FECHAMOV) = 2, " & strKilos & ", 0)) AS FEB, " _
                                & "SUM(IF(MONTH(a.FECHAMOV) = 3, " & strKilos & ", 0)) AS MAR, SUM(IF(MONTH(a.FECHAMOV) = 4, " & strKilos & ", 0)) AS ABR, " _
                                & "SUM(IF(MONTH(a.FECHAMOV) = 5, " & strKilos & ", 0)) AS MAY, SUM(IF(MONTH(a.FECHAMOV) = 6, " & strKilos & ", 0)) AS JUN, " _
                                & "SUM(IF(MONTH(a.FECHAMOV) = 7, " & strKilos & ", 0)) AS JUL, SUM(IF(MONTH(a.FECHAMOV) = 8, " & strKilos & ", 0)) AS AGO, " _
                                & "SUM(IF(MONTH(a.FECHAMOV) = 9, " & strKilos & ", 0)) AS SEP, SUM(IF(MONTH(a.FECHAMOV) = 10, " & strKilos & ", 0)) AS OCT, " _
                                & "SUM(IF(MONTH(a.FECHAMOV) = 11, " & strKilos & ", 0)) AS NOV, SUM(IF(MONTH(a.FECHAMOV) = 12, " & strKilos & ", 0)) AS DIC " _
                                & "from jsmertramer a where " _
                                & " a.origen in ('COM','NDC','NCC') AND " _
                                & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                                & "a.CODART = '" & nArticulo & "' AND a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                & "GROUP BY CODART "
            Case Else ' Money
                strSQLCompras = "select a.CODART, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 1, " & strMoney & ", 0)) AS ENE, SUM(IF(MONTH(a.FECHAMOV) = 2, " & strMoney & ", 0)) AS FEB, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 3, " & strMoney & ", 0)) AS MAR, SUM(IF(MONTH(a.FECHAMOV) = 4, " & strMoney & ", 0)) AS ABR, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 5, " & strMoney & ", 0)) AS MAY, SUM(IF(MONTH(a.FECHAMOV) = 6, " & strMoney & ", 0)) AS JUN, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 7, " & strMoney & ", 0)) AS JUL, SUM(IF(MONTH(a.FECHAMOV) = 8, " & strMoney & ", 0)) AS AGO, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 9, " & strMoney & ", 0)) AS SEP, SUM(IF(MONTH(a.FECHAMOV) = 10, " & strMoney & ", 0)) AS OCT, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 11, " & strMoney & ", 0)) AS NOV, SUM(IF(MONTH(a.FECHAMOV) = 12, " & strMoney & ", 0)) AS DIC " _
                                  & "from jsmertramer a where " _
                                  & " a.origen in ('COM','NDC','NCC') AND " _
                                  & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                                  & " a.CODART = '" & nArticulo & "' AND a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " GROUP BY CODART "

        End Select

        ds = DataSetRequery(ds, strSQLCompras, myConn, nTablaCompras, lblInfo)
        TablaCompras = ds.Tables(nTablaCompras)

    End Function
    Private Function TablaVentas(ByVal nArticulo As String, ByVal CantidadKilosMoney As Integer, Optional ByVal AñosAtras As Integer = 0) As DataTable
        Dim strCantidad As String, strKilos As String, strMoney As String

        strCantidad = "if( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', a.cantidad , 0 )  , if( a.tipomov <> 'AP', -1*a.cantidad ,0) ) "
        strKilos = "if( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', a.peso , 0 )  , if( a.tipomov <> 'AP', -1*a.peso ,0) ) "
        strMoney = "if( a.origen in ('FAC','PFC','PVE','NDV'),  a.ventotaldes   ,  -1*a.ventotaldes ) "

        Dim nTablaVentas As String = "tablaventas" + AñosAtras.ToString
        Dim strSQLVentas As String
        Select Case CantidadKilosMoney
            Case 0 ' Cantidad
                strSQLVentas = " select " _
                & " a.CODART, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 1, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 1, " & strCantidad & "/EQUIVALE,0))) AS ENE, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 2, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 2, " & strCantidad & "/EQUIVALE,0))) AS FEB, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 3, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 3, " & strCantidad & "/EQUIVALE,0))) AS MAR, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 4, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 4, " & strCantidad & "/EQUIVALE,0))) AS ABR, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 5, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 5, " & strCantidad & "/EQUIVALE,0))) AS MAY, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 6, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 6, " & strCantidad & "/EQUIVALE,0))) AS JUN, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 7, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 7, " & strCantidad & "/EQUIVALE,0))) AS JUL, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 8, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 8, " & strCantidad & "/EQUIVALE,0))) AS AGO, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 9, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 9, " & strCantidad & "/EQUIVALE,0))) AS SEP, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 10, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 10, " & strCantidad & "/EQUIVALE,0))) AS OCT, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 11, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 11, " & strCantidad & "/EQUIVALE,0))) AS NOV, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 12, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 12, " & strCantidad & "/EQUIVALE,0))) AS DIC " _
                & " from " _
                & " jsmertramer a LEFT JOIN jsmerequmer b ON " _
                & " a.CODART = b.CODART AND a.UNIDAD = b.UVALENCIA " _
                & " WHERE " _
                & " a.CODART = '" & nArticulo & "' AND " _
                & " a.ORIGEN in ('FAC','PFC','PVE','NDV','NCV') AND " _
                & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " group by a.CODART "

            Case 1 ' Kilos
                strSQLVentas = "select a.CODART, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 1, " & strKilos & ", 0)) AS ENE, SUM(IF(MONTH(a.FECHAMOV) = 2, " & strKilos & ", 0)) AS FEB, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 3, " & strKilos & ", 0)) AS MAR, SUM(IF(MONTH(a.FECHAMOV) = 4, " & strKilos & ", 0)) AS ABR, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 5, " & strKilos & ", 0)) AS MAY, SUM(IF(MONTH(a.FECHAMOV) = 6, " & strKilos & ", 0)) AS JUN, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 7, " & strKilos & ", 0)) AS JUL, SUM(IF(MONTH(a.FECHAMOV) = 8, " & strKilos & ", 0)) AS AGO, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 9, " & strKilos & ", 0)) AS SEP, SUM(IF(MONTH(a.FECHAMOV) = 10, " & strKilos & ", 0)) AS OCT, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 11, " & strKilos & ", 0)) AS NOV, SUM(IF(MONTH(a.FECHAMOV) = 12, " & strKilos & ", 0)) AS DIC " _
                & "from jsmertramer a where " _
                & "a.ORIGEN in ('FAC','PFC','PVE','NDV','NCV') aND " _
                & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                & "a.CODART = '" & nArticulo & "' AND a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & "GROUP BY CODART "
            Case Else ' Money
                strSQLVentas = "select a.CODART, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 1, " & strMoney & ", 0)) AS ENE, SUM(IF(MONTH(a.FECHAMOV) = 2, " & strMoney & ", 0)) AS FEB, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 3, " & strMoney & ", 0)) AS MAR, SUM(IF(MONTH(a.FECHAMOV) = 4, " & strMoney & ", 0)) AS ABR, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 5, " & strMoney & ", 0)) AS MAY, SUM(IF(MONTH(a.FECHAMOV) = 6, " & strMoney & ", 0)) AS JUN, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 7, " & strMoney & ", 0)) AS JUL, SUM(IF(MONTH(a.FECHAMOV) = 8, " & strMoney & ", 0)) AS AGO, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 9, " & strMoney & ", 0)) AS SEP, SUM(IF(MONTH(a.FECHAMOV) = 10, " & strMoney & ", 0)) AS OCT, " _
                & "SUM(IF(MONTH(a.FECHAMOV) = 11, " & strMoney & ", 0)) AS NOV, SUM(IF(MONTH(a.FECHAMOV) = 12, " & strMoney & ", 0)) AS DIC " _
                & "from jsmertramer a where " _
                & "a.ORIGEN in ('FAC','PFC','PVE','NDV','NCV') AND " _
                & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                & "a.CODART = '" & nArticulo & "' AND a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & "GROUP BY CODART "

        End Select

        ds = DataSetRequery(ds, strSQLVentas, myConn, nTablaVentas, lblInfo)
        TablaVentas = ds.Tables(nTablaVentas)

    End Function


    Private Sub AsignarVentas(ByVal nArticulo As String)

        HabilitarObjetos(False, True, txtCodigoVentas, txtNombreVentas, txtCotizacion, txtPedidos, txtEntregas, _
            txtFechaUltimaVenta, txtUltimoCliente, txtSalidas, txtVentasAcum, txtVentasAcumDesc, txtPrecioUltimo, _
            txtDevolucionesVentas, txtVentasAcumDev, txtVentasAcumDevDesc, txtPrecioPromedio, txtNombreUltimoCliente)

        If dt.Rows.Count > 0 Then
            With dt.Rows(nPosicionCat)
                txtCotizacion.Text = FormatoCantidad(.Item("cotizados"))
                txtPedidos.Text = FormatoCantidad(.Item("pedidos"))
                txtEntregas.Text = FormatoCantidad(.Item("entregas"))
                txtFechaUltimaVenta.Text = FormatoFecha(CDate(.Item("FECULTventa").ToString))
                txtUltimoCliente.Text = IIf(IsDBNull(.Item("ultimocliente")), "", .Item("ultimocliente"))
                txtSalidas.Text = FormatoCantidad(.Item("salidas"))
                txtVentasAcum.Text = FormatoNumero(.Item("ACU_PRE"))
                txtVentasAcumDesc.Text = FormatoNumero(.Item("ACU_PRE_DES"))
                txtPrecioUltimo.Text = FormatoNumero(.Item("montoultimaventa"))
                txtDevolucionesVentas.Text = FormatoCantidad(.Item("CREDITOSventas"))
                txtVentasAcumDev.Text = FormatoNumero(.Item("ACU_PRD"))
                txtVentasAcumDevDesc.Text = FormatoNumero(.Item("ACU_prd_DES"))
                txtPrecioPromedio.Text = FormatoNumero(.Item("Venta_PROM_DES"))
            End With

            rBtnV1.Checked = True
            VerHistograma(C1Chart2, 1, txtCodigo.Text, 0)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)


    End Sub
    Private Sub AsignarExistencias(ByVal nArticulo As String)

        HabilitarObjetos(False, True, txtCodigoExistencias, txtNombreExistencias, txtExisteUnidad, _
                         txtExistenciaKilos, txtPromedioUnidad, txtPromedioKilos, txtSugeridoUnidad, _
                         txtSugeridoKilos, txtUnidad1, txtUnidad2, txtUnidad4, txtKilos1, txtKilos2, _
                         txtKilos4, txtInventarioDias)

        Dim nMeses As Integer = 3
        Dim nDiasSugeridos As Integer = 10

        txtPromedioMeses.Text = FormatoEntero(nMeses)
        txtSugeridoDias.Text = FormatoEntero(nDiasSugeridos)

        PresentarExistencias(nArticulo, nMeses, nDiasSugeridos)

        Dim aExistencias() As String = {"Almacén", "Lote", "Color", "Talla", "Almacén y color ", "Almacen y Talla", "Almacen, color y talla", "Lote y color", "Lote y talla", "Lote, color y talla"}
        RellenaCombo(aExistencias, cmbExistencias)

        ExistenciasPor(nArticulo, cmbExistencias.SelectedIndex, nMeses)


    End Sub
    Private Sub ExistenciasPor(ByVal nArticulo As String, ByVal por As Integer, ByVal nMeses As Integer)

        Dim dtExPor As DataTable
        Dim nTablaExPor As String = "tablaexPor"

        Dim str1 As String = ""
        Dim str2 As String = ""
        Dim str3 As String = ""
        Dim str4 As String = ""
        Select Case por
            Case 0 ' Por almacen 
                str1 = " a.almacen codigo, e.desalm descripcion, "
                str2 = " b.almacen, "
                str3 = " left join jsmercatalm e ON (a.almacen = e.codalm AND a.id_emp = e.id_emp )  "
                str4 = " GROUP BY 1, 2 "
            Case 1 'Por LOTE
                str1 = " a.lote codigo, e.expiracion descripcion,  "
                str2 = " b.lote, "
                str3 = " left join jsmerlotmer e on (a.lote = e.lote and a.id_emp = e.id_emp ) "
                str4 = " group by 1,2 "
            Case Else
        End Select

        ds = DataSetRequery(ds, " SELECT a.codart, " & str1 & " c.unidad, SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) existencia , " _
            & " round(c.pesounidad*SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)),3) pesototal, " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)),3) ventasperiodo,  d.diashabiles, " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles, 3) promedioDiario,  " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles * c.pesounidad,3) promedioDiarioPeso , " _
            & " SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) / (  SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles ) Inventario " _
            & " FROM (SELECT  b.codart, " & str2 & " b.unidad,  " _
            & " SUM(IF( b.TIPOMOV IN( 'EN', 'AE', 'DV') , b.CANTIDAD, -1 * b.CANTIDAD )) existencia, " _
            & " SUM(IF( b.origen IN ('FAC', 'PVE', 'PFC') AND b.fechamov >= DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH) AND  b.fechamov <= DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL 1 DAY) ,  b.cantidad, 0.000 ) ) ventas, " _
            & " " _
            & " b.id_emp " _
            & " FROM jsmertramer b " _
            & " WHERE " _
            & "      b.tipomov <> 'AC' AND " _
            & "      b.codart = '" & nArticulo & "' AND " _
            & "      b.id_emp = '" & jytsistema.WorkID & "' AND  " _
            & "      b.ejercicio = '" & jytsistema.WorkExercise & "' AND  " _
            & "      date_format(b.fechamov, '%Y-%m-%d') <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " _
            & "      GROUP BY b.codart, " & str2 & " b.unidad ) a  " _
            & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp )  " _
            & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT '1' num, (CURRENT_DATE - DATE_ADD(CURRENT_DATE, INTERVAL -" & nMeses & " MONTH)) -  COUNT(*) DiasHabiles " _
            & " FROM (SELECT CONCAT(ano,'-',IF(LENGTH(mes)=1,CONCAT('0',mes),mes),'-',IF(LENGTH(dia)=1,CONCAT('0',dia),dia)) AS fecha FROM jsconcatper  " _
            & " HAVING  (fecha <CURRENT_DATE AND fecha>DATE_ADD(CURRENT_DATE,INTERVAL -" & nMeses & " MONTH)) )  a) d ON ( d.num = '1') " _
            & str3 _
            & str4, myConn, nTablaExPor, lblInfo)

        dtExPor = ds.Tables(nTablaExPor)

        Dim aCamEx() As String = {"codigo", "descripcion", "existencia", "pesototal", "inventario", ""}
        Dim aNomEx() As String = {"Código", "Descripción", "Existencia en Unidades Venta", "Existencia en Kilogramos", "Dias de inventario", ""}
        Dim aAncEx() As Long = {60, 140, 120, 120, 80, 100}
        Dim aAliEx() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                   AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aForEx() As String = {"", "", sFormatoCantidad, sFormatoCantidad, sFormatoEntero, ""}

        IniciarTabla(dgExistencias, dtExPor, aCamEx, aNomEx, aAncEx, aAliEx, aForEx)

    End Sub

    Private Sub PresentarExistencias(ByVal nArticulo As String, ByVal nMeses As Integer, ByVal nDiasSugeridos As Integer)

        Dim dtExistencias As DataTable
        Dim nTablaExiste As String = "tablaexiste"

        ds = DataSetRequery(ds, " SELECT a.codart, c.unidad, SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) existencia , " _
            & " round(c.pesounidad*SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)),3) pesototal, " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)),3) ventasperiodo,  d.diashabiles, " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles, 3) promedioDiario,  " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles * c.pesounidad,3) promedioDiarioPeso , " _
            & " SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) / (  SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles ) Inventario " _
            & " FROM (SELECT  b.codart, b.unidad,  " _
            & " SUM(IF( b.TIPOMOV IN( 'EN', 'AE', 'DV') , b.CANTIDAD, -1 * b.CANTIDAD )) existencia, " _
            & " SUM(IF( b.origen IN ('FAC', 'PVE', 'PFC') AND b.fechamov >= DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH) AND  b.fechamov <= DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL 1 DAY) ,  b.cantidad, 0.000 ) ) ventas, " _
            & " " _
            & " b.id_emp " _
            & " FROM jsmertramer b " _
            & " WHERE " _
            & "      b.tipomov <> 'AC' AND " _
            & "      b.codart = '" & nArticulo & "' AND " _
            & "      b.id_emp = '" & jytsistema.WorkID & "' AND  " _
            & "      b.ejercicio = '" & jytsistema.WorkExercise & "' AND  " _
            & "      date_format(b.fechamov, '%Y-%m-%d') <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " _
            & "      GROUP BY b.codart, b.unidad ) a  " _
            & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp )  " _
            & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT '1' num, (DATEDIFF('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL - " & nMeses & " MONTH))) -  COUNT(*) DiasHabiles " _
            & " FROM (SELECT CONCAT(ano,'-',IF(LENGTH(mes)=1,CONCAT('0',mes),mes),'-',IF(LENGTH(dia)=1,CONCAT('0',dia),dia)) AS fecha FROM jsconcatper  " _
            & " HAVING  (fecha <'" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND fecha>DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "',INTERVAL -" & nMeses & " MONTH)) )  a) d ON ( d.num = '1') " _
            & " group by 1 ", myConn, nTablaExiste, lblInfo)

        dtExistencias = ds.Tables(nTablaExiste)

        If dtExistencias.Rows.Count > 0 Then

            With dtExistencias.Rows(0)
                txtExisteUnidad.Text = FormatoCantidad(.Item("existencia"))
                txtExistenciaKilos.Text = FormatoCantidad(.Item("pesototal"))
                txtInventarioDias.Text = FormatoEntero(IIf(IsDBNull(.Item("inventario")), 0, .Item("inventario")))
                txtPromedioUnidad.Text = FormatoCantidad(IIf(IsDBNull(.Item("promediodiario")), 0, .Item("promediodiario")))
                txtPromedioKilos.Text = FormatoCantidad(IIf(IsDBNull(.Item("promediodiariopeso")), 0, .Item("promediodiariopeso")))
                txtSugeridoUnidad.Text = FormatoCantidad(nDiasSugeridos * IIf(IsDBNull(.Item("promediodiario")), 0, .Item("promediodiario")))
                txtSugeridoKilos.Text = FormatoCantidad(nDiasSugeridos * IIf(IsDBNull(.Item("promediodiariopeso")), 0, .Item("promediodiariopeso")))
                txtUnidad1.Text = .Item("unidad")
                txtUnidad2.Text = .Item("unidad")
                txtUnidad4.Text = .Item("unidad")
                txtKilos1.Text = "KGR"
                txtKilos2.Text = "KGR"
                txtKilos4.Text = "KGR"
            End With
        End If

    End Sub
    Private Sub AsignarExpediente(ByVal nArticulo As String)
        HabilitarObjetos(False, True, txtCodigoExpediente, txtNombreExpediente)

        Dim dtExp As DataTable
        Dim nTablaExp As String = "tblExpediente"
        Dim strSQLExp As String = " SELECT a.codart, a.fecha, a.comentario, a.causa, ELT(a.condicion+1,'Activo','Inactivo') condicion, " _
                        & " a.tipocondicion ,  " _
                        & " IF( IF( a.condicion = 0 , b.descrip, c.descrip )  IS NULL, 'NOTA DE USUARIO', IF( a.condicion = 0 , b.descrip, c.descrip )) Descripcion " _
                        & " FROM jsmerexpmer a  " _
                        & " LEFT JOIN jsconctatab b ON (a.causa = b.codigo AND a.id_emp = b.id_emp AND b.modulo = '00021') " _
                        & " LEFT JOIN jsconctatab c ON (a.causa = c.codigo AND a.id_emp = c.id_emp AND c.modulo = '00022') " _
                        & " WHERE " _
                        & " a.codart = '" & nArticulo & "' and " _
                        & " a.ID_EMP = '" & jytsistema.WorkID & "' order by fecha desc "

        ds = DataSetRequery(ds, strSQLExp, myConn, nTablaExp, lblInfo)
        dtExp = ds.Tables(nTablaExp)

        Dim aCamExp() As String = {"fecha", "descripcion", "condicion", "comentario"}
        Dim aNomExp() As String = {"Fecha", "Descripción causa", "Condición", "Comentario"}
        Dim aAncexp() As Long = {140, 240, 100, 350}
        Dim aAliExp() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aForExp() As String = {sFormatoFecha, "", "", ""}

        IniciarTabla(dgExpediente, dtExp, aCamExp, aNomExp, aAncexp, aAliExp, aForExp)

    End Sub
    Private Sub AgregaYCancela()

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsmercatcom where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerequmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        'OJOOJOJOJO. VERIFICAR TALLAS Y COLORES
        'EjecutarSTRSQL(myConn, lblInfo, " delete from jsprodescom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        'EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()
        If dt.Rows.Count = 0 Then
            IniciarMercancia(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
            AsignaTXT(nPosicionCat)
        End If
        DesactivarMarco0()

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub

    Private Sub btnAgregaEquivale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaEquivale.Click
        Dim g As New jsMerArcMercanciasEquivalencias
        g.Agregar(myConn, ds, dtEquivalencias, txtCodigo.Text, aUnidadAbreviada(cmbUnidad.SelectedIndex))

        AbrirEquivalencias(txtCodigo.Text, aUnidadAbreviada(cmbUnidad.SelectedIndex))

        If g.Apuntador >= 0 Then AsignaEqu(g.Apuntador, True)
        g = Nothing

    End Sub

    Private Sub btnEditaEquivale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditaEquivale.Click
        If dtEquivalencias.Rows.Count > 0 Then
            Dim g As New jsMerArcMercanciasEquivalencias
            nPosicionEqu = Me.BindingContext(ds, nTablaEquivalencias).Position
            g.Apuntador = nPosicionEqu
            g.Editar(myConn, ds, dtEquivalencias, txtCodigo.Text, aUnidadAbreviada(cmbUnidad.SelectedIndex))
            If g.Apuntador >= 0 Then AsignaEqu(g.Apuntador, True)
            g = Nothing
        End If
    End Sub

    Private Sub btnEliminaEquivale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaEquivale.Click
        With dtEquivalencias
            If .Rows.Count > 0 Then
                nPosicionEqu = Me.BindingContext(ds, nTablaEquivalencias).Position
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"codart", "unidad", "uvalencia", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, aUnidadAbreviada(cmbUnidad.SelectedIndex), _
                                           .Rows(nPosicionEqu).Item("uvalencia").ToString, _
                                           jytsistema.WorkID}

                If EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT COUNT(*) FROM jsmertramer WHERE " _
                                         & " codart = '" & txtCodigo.Text & "' AND " _
                                         & " unidad = '" & .Rows(nPosicionEqu).Item("uvalencia").ToString & "' AND " _
                                         & " id_emp = '" & jytsistema.WorkID & "'") = 0 Then

                    sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                    If sRespuesta = MsgBoxResult.Yes Then
                        AsignaEqu(EliminarRegistros(myConn, lblInfo, ds, nTablaEquivalencias, "jsmerequmer", _
                                                   strSQLEQu, aCamDel, aStrDel, nPosicionEqu), True)

                    End If
                Else
                    MensajeCritico(lblInfo, "ESTA UNIDAD O EQUIVALENCIA YA FUE UTILIZADA NO PUEDE SER ELIMINADA. VERIFIQUE POR FAVOR...")
                End If

            End If
        End With

    End Sub

    Private Sub dgEQU_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgEqu.RowHeaderMouseClick, _
        dgEqu.CellMouseClick, dgEqu.RegionChanged
        Me.BindingContext(ds, nTablaEquivalencias).Position = e.RowIndex
        nPosicionEqu = e.RowIndex
    End Sub


    Private Sub txtSugerido_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSugerido.KeyPress, _
        txtPorDevoluciones.KeyPress, txtPesoUnidad.KeyPress, txtExMax.KeyPress, txtExMin.KeyPress, txtAlto.KeyPress, _
        txtAncho.KeyPress, txtProfun.KeyPress, txtPrecioA.KeyPress, txtPrecioB.KeyPress, txtPrecioC.KeyPress, _
        txtPrecioD.KeyPress, txtPrecioE.KeyPress, txtPrecioF.KeyPress, txtDescA.KeyPress, txtDescB.KeyPress, _
        txtDescC.KeyPress, txtDescD.KeyPress, txtDescE.KeyPress, txtDescF.KeyPress, txtGanA.KeyPress, _
        txtGanB.KeyPress, txtGanC.KeyPress, txtGanD.KeyPress, txtGanE.KeyPress, txtGanF.KeyPress, _
        txtPromedioMeses.KeyPress, txtSugeridoDias.KeyPress

        e.Handled = ValidaNumeroEnTextbox(e)

    End Sub

    Private Sub cmbIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIVA.SelectedIndexChanged
        txtIVA.Text = FormatoNumero(PorcentajeIVA(myConn, lblInfo, jytsistema.sFechadeTrabajo, cmbIVA.Text)) & "%"
    End Sub

    Private Sub txtCategoria_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoria.TextChanged

        Dim aFld() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtCategoria.Text, "00002", jytsistema.WorkID}
        txtCategoriaNombre.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", aFld, aStr, "descrip")

    End Sub

    Private Sub txtMarca_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMarca.TextChanged

        Dim aFld() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtMarca.Text, "00003", jytsistema.WorkID}
        txtMarcaNombre.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", aFld, aStr, "descrip")

    End Sub

    Private Sub txtDivision_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDivision.TextChanged
        Dim aFld() As String = {"division", "id_emp"}
        Dim aStr() As String = {txtDivision.Text, jytsistema.WorkID}
        txtDivisionNombre.Text = qFoundAndSign(myConn, lblInfo, "jsmercatdiv", aFld, aStr, "descrip")
    End Sub

    Private Sub txtTipJer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTipJer.TextChanged
        Dim aFld() As String = {"tipjer", "id_emp"}
        Dim aStr() As String = {txtTipJer.Text, jytsistema.WorkID}
        txtTipjerNombre.Text = qFoundAndSign(myConn, lblInfo, "jsmerencjer", aFld, aStr, "descrip")
    End Sub

    Private Sub btnCategoria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoria.Click
        Dim fg As New jsControlArcTablaSimple
        fg.Cargar("Categorías", FormatoTablaSimple(Modulo.iCategoriaMerca), True, TipoCargaFormulario.iShowDialog)
        txtCategoria.Text = fg.Seleccion
        fg.Close()
        fg = Nothing
    End Sub
    Private Sub btnMarca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarca.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Marcas", FormatoTablaSimple(Modulo.iMarcaMerca), True, TipoCargaFormulario.iShowDialog)
        txtMarca.Text = f.Seleccion
        f.Close()
        f = Nothing
    End Sub

    Private Sub txtUltimoProveedor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUltimoProveedor.TextChanged
        Dim afld() As String = {"codpro", "id_emp"}
        Dim aCam() As String = {txtUltimoProveedor.Text, jytsistema.WorkID}
        txtNombreUltimoProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", afld, aCam, "nombre")
    End Sub

    Private Sub rBtn1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtn1.CheckedChanged, _
        rBtn2.CheckedChanged, rBtn3.CheckedChanged

        If rBtn1.Checked Then VerHistograma(c1Chart1, 0, txtCodigo.Text, 0)
        If rBtn2.Checked Then VerHistograma(c1Chart1, 0, txtCodigo.Text, 1)
        If rBtn3.Checked Then VerHistograma(c1Chart1, 0, txtCodigo.Text, 2)

    End Sub
    Private Sub rBtnV1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtnV1.CheckedChanged, _
        rBtnV2.CheckedChanged, rBtnV3.CheckedChanged

        If rBtnV1.Checked Then VerHistograma(C1Chart2, 1, txtCodigo.Text, 0)
        If rBtnV2.Checked Then VerHistograma(C1Chart2, 1, txtCodigo.Text, 1)
        If rBtnV3.Checked Then VerHistograma(C1Chart2, 1, txtCodigo.Text, 2)

    End Sub
    Private Sub txtUltimoCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUltimoCliente.TextChanged
        Dim afld() As String = {"codcli", "id_emp"}
        Dim aCam() As String = {txtUltimoCliente.Text, jytsistema.WorkID}
        txtNombreUltimoCliente.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", afld, aCam, "nombre")
    End Sub

    Private Sub txtPromedioMeses_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPromedioMeses.TextChanged, _
        txtSugeridoDias.TextChanged
        PresentarExistencias(txtCodigo.Text, ValorEntero(txtPromedioMeses.Text), ValorEntero(txtSugeridoDias.Text))
    End Sub

    Private Sub cmbExistencias_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbExistencias.SelectedIndexChanged
        ExistenciasPor(txtCodigo.Text, cmbExistencias.SelectedIndex, ValorEntero(txtPromedioMeses.Text))
    End Sub


    Private Sub btnTipJer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipJer.Click
        Dim f As New jsMerArcJerarquias
        f.TipoJerarquia = txtTipJer.Text
        f.Grupo1 = txtCodjer1.Text
        f.Grupo2 = txtCodjer2.Text
        f.Grupo3 = txtCodjer3.Text
        f.Grupo4 = txtCodjer4.Text
        f.Grupo5 = txtCodjer5.Text
        f.Grupo6 = txtCodjer6.Text
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)

        txtTipJer.Text = f.TipoJerarquia
        txtCodjer1.Text = f.Grupo1
        txtCodjer2.Text = f.Grupo2
        txtCodjer3.Text = f.Grupo3
        txtCodjer4.Text = f.Grupo4
        txtCodjer5.Text = f.Grupo5
        txtCodjer6.Text = f.Grupo6

        txtJerarquiaNombre.Text = ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer1.Text, 1) + IIf(txtCodjer2.Text <> "", "->", "") _
                    + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer2.Text, 2) + IIf(txtCodjer3.Text <> "", "->", "") _
                    + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer3.Text, 3) + IIf(txtCodjer4.Text <> "", "->", "") _
                    + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer4.Text, 4) + IIf(txtCodjer5.Text <> "", "->", "") _
                    + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer5.Text, 5) + IIf(txtCodjer6.Text <> "", "->", "") _
                    + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer6.Text, 6)


        f = Nothing
    End Sub

    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngreso.Click
        txtIngreso.Text = SeleccionaFecha(CDate(txtIngreso.Text), Me, sender)
    End Sub

    Private Sub btnIVA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIVA.Click
        Dim f As New jsControlArcIVA
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)

        aIVA = ArregloIVA(myConn, lblInfo)
        RellenaCombo(aIVA, cmbIVA, InArray(aIVA, f.Seleccionado) - 1)

        f = Nothing

    End Sub



    Private Sub btnDivision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivision.Click
        Dim f As New jsControlArcDivisiones
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtDivision.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub cmbTipoMovimiento_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoMovimiento.SelectedIndexChanged

        Dim bs As New BindingSource
        Dim aTTipo() As String = {"", "EN", "SA", "AE", "AS", "AC"}
        bs.DataSource = dtMovimientos
        If dtMovimientos.Columns("tipomov").DataType Is GetType(String) Then _
          bs.Filter = " tipomov like '%" & aTTipo(cmbTipoMovimiento.SelectedIndex) & "%'"
        dg.DataSource = bs
        dg.Refresh()

    End Sub

    Private Function ColocaJerarquiaNivel(ByVal MyConn As MySqlConnection, ByVal TipoJerarquia As String, ByVal CodigoJerarquia As String, ByVal Nivel As Integer) As String
        Dim aCam() As String = {"tipjer", "codjer", "nivel", "id_emp"}
        Dim aStr() As String = {TipoJerarquia, CodigoJerarquia, Nivel, jytsistema.WorkID}
        ColocaJerarquiaNivel = qFoundAndSign(MyConn, lblInfo, "jsmerrenjer", aCam, aStr, "desjer")
    End Function

    Private Sub btnCombo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCombo.Click
        If txtCodigo.Text <> "" Then
            Dim f As New jsMerArcComponentesCombo
            f.Cargar(myConn, txtCodigo.Text)
            EjecutarSTRSQL(myConn, lblInfo, " update jsmerctainv set montoultimacompra = " & f.CostoTotal & " , pesounidad = " & f.PesoTotal & " where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            txtPesoUnidad.Text = FormatoCantidad(f.PesoTotal)
            txtUltimoCosto.Text = FormatoNumero(f.CostoTotal)
            chkCombo.Checked = IIf(f.CostoTotal > 0, True, False)

            txtPrecioA.Text = FormatoNumero(f.precioTotalA)
            txtPrecioB.Text = FormatoNumero(f.precioTotalB)
            txtPrecioC.Text = FormatoNumero(f.precioTotalC)
            txtPrecioD.Text = FormatoNumero(f.precioTotalD)
            txtPrecioE.Text = FormatoNumero(f.precioTotalE)
            txtPrecioF.Text = FormatoNumero(f.precioTotalF)
            f.Dispose()
            f = Nothing
        End If
    End Sub

    Private Sub txtIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIVA.TextChanged

    End Sub

    Private Sub btnRecalcular_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecalcular.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                EjecutarSTRSQL(myConn, lblInfo, " update jsmerctainv set unidaddetal = unidad where unidaddetal = '' and id_emp = '" & jytsistema.WorkID & "' ")
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                AsignaTXT(nPosicionCat)
            Case "Movimientos"
                Dim fg As New jsMerProCostos
                fg.Cargar(myConn, txtCodigoMovimientos.Text, jytsistema.sFechadeTrabajo)
                MensajeInformativoPlus("Actualizado...")
                AsignaMov(0, True)
                fg.Dispose()
                fg = Nothing
            Case "Compras"
                txtUltimoCosto.Text = FormatoNumero(UltimoCostoAFecha(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))
                txtUltimoProveedor.Text = UltimoProveedor(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo)
                txtFechaUltimaCompr.Text = FormatoFecha(UltimaFechaCompra(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))
                MensajeInformativoPlus("Ultimo Costo actualizado...")
            Case "Existencias"
                ActualizaExistenciasEnAlmacen(myConn, txtCodigoExistencias.Text, lblInfo)
                AsignarExistencias(txtCodigoExistencias.Text)
                MensajeInformativoPlus("Actualizado...")
        End Select

    End Sub
    Private Sub ActualizarCostosEnMovmimientos()
        Dim dtMovSal As DataTable
        Dim nTablaaMovSal As String = "tblmovsalida"

        ds = DataSetRequery(ds, " select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " codart = '" & txtCodigoMovimientos.Text & "' AND " _
                            & " origen in ('FAC', 'PVE', 'NDV', 'TRF', 'INV') AND " _
                            & " tipomov in ('SA', 'AS' ) AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " UNION select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " codart = '" & txtCodigoMovimientos.Text & "' AND " _
                            & " origen in ('NCV') AND " _
                            & " tipomov in ('EN') AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " ORDER BY FECHAMOV ", myConn, nTablaaMovSal, lblInfo)

        dtMovSal = ds.Tables(nTablaaMovSal)

        For Each nRow As DataRow In dtMovSal.Rows
            With nRow

                Dim nCosto As Double = UltimoCostoAFecha(myConn, lblInfo, .Item("codart"), CDate(.Item("fechamov").ToString))
                Dim nEquivale As Double = Equivalencia(myConn, lblInfo, .Item("codart"), .Item("unidad"))
                Dim Descuento As Double = (1 - .Item("costotaldes") / .Item("costotal")) * 100


                Dim Costotal As Double = nCosto * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)
                Dim CostotalDescuento As Double = nCosto * (1 - Descuento / 100) * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)

                InsertEditMERCASMovimientoInventario(myConn, lblInfo, False, .Item("codart"), CDate(.Item("fechamov").ToString), _
                                                      .Item("tipomov"), .Item("numdoc"), .Item("unidad"), .Item("cantidad"), .Item("peso"), _
                                                      Costotal, CostotalDescuento, .Item("origen"), .Item("numorg"), _
                                                      .Item("lote"), .Item("prov_cli"), .Item("ventotal"), .Item("ventotaldes"),
                                                      .Item("impiva"), .Item("descuento"), .Item("vendedor"), .Item("almacen"), _
                                                      .Item("asiento"), CDate(.Item("fechasi").ToString))


            End With
        Next

        dtMovSal.Dispose()
        dtMovSal = Nothing

        AsignaMov(0, True)


    End Sub
    Private Sub c1Chart1_ShowTooltip(ByVal sender As Object, ByVal e As C1.Win.C1Chart.ShowTooltipEventArgs) Handles c1Chart1.ShowTooltip
        If TypeOf sender Is ChartDataSeries Then
            ' Create new tooltip text
            'If c1Chart1.ToolTip.PlotElement = PlotElementEnum.Series Then
            Dim ds As ChartDataSeries = CType(sender, ChartDataSeries)

            Dim p As Point = Control.MousePosition
            p = c1Chart1.PointToClient(p)

            Dim x As Double = 0
            Dim y As Double = 0

            ' Callculate data coordinates
            Dim aNom() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
            If ds.Group.CoordToDataCoord(p.X, p.Y, x, y) Then
                e.TooltipText = String.Format("{0}" + ControlChars.Lf + "Mes = " + aNom(Math.Round(x) - 1) + _
                                              ControlChars.Lf + "Valor = {2:#.##}", ds.Label, x, y)
            Else
                e.TooltipText = ""
            End If
            'End If
        End If
    End Sub
    Private Sub c1Chart2_ShowTooltip(ByVal sender As Object, ByVal e As C1.Win.C1Chart.ShowTooltipEventArgs) Handles C1Chart2.ShowTooltip
        If TypeOf sender Is ChartDataSeries Then
            ' Create new tooltip text
            'If c1Chart1.ToolTip.PlotElement = PlotElementEnum.Series Then
            Dim ds As ChartDataSeries = CType(sender, ChartDataSeries)

            Dim p As Point = Control.MousePosition
            p = C1Chart2.PointToClient(p)

            Dim x As Double = 0
            Dim y As Double = 0

            ' Callculate data coordinates
            Dim aNom() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
            If ds.Group.CoordToDataCoord(p.X, p.Y, x, y) Then
                e.TooltipText = String.Format("{0}" + ControlChars.Lf + "Mes = " + aNom(Math.Round(x) - 1) + _
                                              ControlChars.Lf + "Valor = {2:#.##}", ds.Label, x, y)
            Else
                e.TooltipText = ""
            End If
            'End If
        End If
    End Sub

    Private Sub cmbUnidad_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbUnidad.SelectedIndexChanged

        If cmbUnidad.SelectedIndex >= 0 Then _
            strSQLEQu = " select codart, unidad, equivale, uvalencia, elt(divide + 1,'No','Si') divide, id_emp from jsmerequmer " _
                        & " where codart = '" & txtCodigo.Text & "' and " _
                        & " unidad = '" & aUnidadAbreviada(cmbUnidad.SelectedIndex) & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' order by UVALENCIA "

        If i_modo = movimiento.iEditar Then
            EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerequmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            AsignaEqu(nPosicionEqu, True)
        End If

    End Sub

    Private Sub btnFoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFoto.Click
        Dim ofd As New OpenFileDialog()

        ofd.InitialDirectory = "c:\"
        ofd.Filter = "Archivos JPG |*.jpg"
        ofd.FilterIndex = 2
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            CaminoFoto = ofd.FileName
            pctFoto.ImageLocation = CaminoFoto
            pctFoto.Load()
        End If

        ofd = Nothing
    End Sub

    Private Sub dgEqu_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgEqu.CellContentClick

    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        If cmbUnidad.SelectedIndex >= 0 Then _
            strSQLEQu = " select codart, unidad, equivale, uvalencia, elt(divide + 1,'No','Si') divide, id_emp from jsmerequmer " _
                            & " where codart = '" & txtCodigo.Text & "' and " _
                            & " unidad = '" & aUnidadAbreviada(cmbUnidad.SelectedIndex) & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by UVALENCIA "
    End Sub

    Private Sub txtOfertaA_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOfertaA.MouseHover, _
        txtOfertaB.MouseHover, txtOfertaC.MouseHover, txtOfertaD.MouseHover, txtOfertaE.MouseHover, txtOfertaF.MouseHover

        Dim Mensaje As String = MensajeOferta(myConn, txtCodigo.Text, jytsistema.sFechadeTrabajo, lblInfo)
        If Mensaje <> "" Then
            MensajeInformativo(lblInfo, Mensaje)
            C1SuperTooltip1.SetToolTip(sender, Mensaje)
        End If

    End Sub

    Private Sub txtOfertaA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOfertaA.TextChanged

    End Sub

    Private Sub FichaMercanciaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FichaMercanciaToolStripMenuItem.Click
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cFichaMercancia, "FICHA DE MERCANCIA", txtCodigo.Text)
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub CódigosDeBarrasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CódigosDeBarrasToolStripMenuItem.Click
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cCodigoBarras, "CODIGO BARRAS DE MERCANCIA", txtCodigo.Text)
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub MovimientosDeMercanciaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MovimientosDeMercanciaToolStripMenuItem.Click
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMovimientosMercancia, "MOVIMIENTO DE MERCANCIA", txtCodigo.Text)
        f.Dispose()
        f = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
        End Select
    End Sub

    Private Sub btnSICA_Click(sender As System.Object, e As System.EventArgs) Handles btnSICA.Click
        Dim F As New jsControlArcRubrosSADA
        F.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtSICA.Text = F.Seleccionado
        F.Dispose()
        F = Nothing
    End Sub


End Class