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
    Private Const nTablaCuotas As String = "tblCuotasAsesor"
    Private Const nTablaEnvases As String = "tblEnvases"

    Private strSQL As String = "select * from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart "
    Private strSQLMov As String
    Private strSQLEQu As String
    Private strSQLCompras As String
    Private strSQLVentas As String
    Private strSQLCuotas As String = ""
    Private strSQLIVA As String = " select tipo from jsconctaiva group by tipo "
    Private strSQLEnvases As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private da As New MySqlDataAdapter(myCom)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtEquivalencias As New DataTable
    Private dtCuotas As New DataTable
    Private dtEnvases As New DataTable

    Private ft As New Transportables

    Private aIVA() As String = ArregloIVA(myConn, lblInfo)
    Private aTipo() As String = {"Venta", "Uso interno", "POP", "Alquiler", "Préstamo", "Materia prima", "Venta & Envase", "Otros"}
    Private aMix() As String = {"Económico", "Estandar", "Superior"}
    Private aCondicion() As String = {"Activo", "Inactivo"}
    Private aTipoMovimiento() As String = {"Todos", "Entradas", "Salidas", "Ajuste de Entrada", "Ajuste de Salida", "Ajuste de Costo"}

    Private strUnidad As String = ""
    Private aUnidad() As String = {}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionEqu As Long
    Private nPosicionCuo As Long
    Private nPosicionEnv As Long
    Private CaminoFoto As String = ""

    Private Sub jsMerArcMercancias_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcMercancias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)


            ft.RellenaCombo(aTipo, cmbTipo)
            ft.RellenaCombo(aMix, cmbMIX)
            ft.RellenaCombo(aCondicion, cmbCondicion)
            ft.RellenaCombo(jytsistema.aUnidad, cmbUnidad)
            ft.RellenaCombo(jytsistema.aUnidad, cmbUnidadDetal)


            DesactivarMarco0()

            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarMercancia(False)
            End If
            tbcMercas.SelectedTab = C1DockingTabPage1
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            btnAgregaEquivale.Enabled = btnAgregar.Enabled
            btnEditaEquivale.Enabled = btnEditar.Enabled
            btnEliminaEquivale.Enabled = btnEliminar.Enabled
            AsignarTooltips()


        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub

    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, _
                          btnPrimero, btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir, btnRecalcular)
        'Botones Adicionales
        ft.colocaToolTip(C1SuperTooltip1, btnCategoria, btnMarca, btnDivision, btnTipJer, btnIVA, btnTallas, btnCombo, _
                         btnFoto, btnSACS, btnSICA, btnCEP, btnIngreso)


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

        dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarra, dg, lRegion, _
            jytsistema.sUsuario, nRow, Actualiza)
        
    End Sub
    Private Sub AsignaEqu(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dtEquivalencias = ft.MostrarFilaEnTabla(myConn, ds, nTablaEquivalencias, strSQLEQu, Me.BindingContext, MenuEquivalencia, dgEqu, lRegion, _
            jytsistema.sUsuario, nRow, Actualiza, False)
       
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If dt.Rows.Count > 0 Then

            With dt

                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)
                    'Mercancías
                    nPosicionCat = nRow

                    txtCodigo.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombre.Text = ft.muestraCampoTexto(.Item("nomart"))
                    txtAlterno.Text = ft.muestraCampoTexto(.Item("alterno"))
                    txtBarras.Text = ft.muestraCampoTexto(.Item("barras"))
                    txtCategoria.Text = ft.muestraCampoTexto(.Item("grupo"))
                    txtMarca.Text = ft.muestraCampoTexto(.Item("marca"))
                    txtDivision.Text = ft.muestraCampoTexto(.Item("division"))
                    txtSICA.Text = ft.muestraCampoTexto(.Item("CODJER"))

                    txtTipJer.Text = ft.muestraCampoTexto(.Item("tipjer"))
                    txtCodjer1.Text = ft.muestraCampoTexto(.Item("codjer1"))
                    txtCodjer2.Text = ft.muestraCampoTexto(.Item("codjer2"))
                    txtCodjer3.Text = ft.muestraCampoTexto(.Item("codjer3"))
                    txtCodjer4.Text = ft.muestraCampoTexto(.Item("codjer4"))
                    txtCodjer5.Text = ft.muestraCampoTexto(.Item("codjer5"))
                    txtCodjer6.Text = ft.muestraCampoTexto(.Item("codjer6"))

                    txtJerarquiaNombre.Text = ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer1.Text, 1) + IIf(txtCodjer2.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer2.Text, 2) + IIf(txtCodjer3.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer3.Text, 3) + IIf(txtCodjer4.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer4.Text, 4) + IIf(txtCodjer5.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer5.Text, 5) + IIf(txtCodjer6.Text <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, txtCodjer6.Text, 6)

                    txtSCAS.Text = ft.muestraCampoTexto(.Item("SACS"))
                    txtCEP.Text = ft.muestraCampoTexto(.Item("CEP"))

                    txtPresentacion.Text = ft.muestraCampoTexto(.Item("presentacion"))
                    txtSugerido.Text = ft.muestraCampoNumero(.Item("sugerido"))
                    chkRegulada.Checked = .Item("regulado")
                    chkDevoluciones.Checked = .Item("devolucion")
                    txtPorDevoluciones.Text = ft.muestraCampoNumero(.Item("por_acepta_dev"))

                    ft.RellenaCombo(aUnidadAbreviada, cmbUnidad, ft.InArray(aUnidadAbreviada, .Item("unidad")))
                    If ExisteCampo(myConn, lblInfo, jytsistema.WorkDataBase, "jsmerctainv", "unidaddetal") Then _
                         ft.RellenaCombo(aUnidadAbreviada, cmbUnidadDetal, ft.InArray(aUnidadAbreviada, .Item("unidaddetal")))

                    txtPesoUnidad.Text = ft.muestraCampoCantidad(.Item("pesounidad"))
                    txtExMin.Text = ft.muestraCampoCantidad(.Item("exmin"))
                    txtExMax.Text = ft.muestraCampoCantidad(.Item("exmax"))

                    'txtUbica1.Text = .Item("UBICACION").ToString.Split(".")(0)
                    '      txtUbica2.Text = .Item("UBICACION").ToString.Split(".")(1)
                    '        txtUbica3.Text = .Item("UBICACION").ToString.Split(".")(2)

                    txtAlto.Text = ft.muestraCampoNumero(.Item("altura"))
                    txtAncho.Text = ft.muestraCampoNumero(.Item("ancho"))
                    txtProfun.Text = ft.muestraCampoNumero(.Item("profun"))

                    ft.RellenaCombo(aIVA, cmbIVA, ft.InArray(aIVA, .Item("iva")))

                    chkCartera.Checked = .Item("cuota")
                    chkDescuentos.Checked = .Item("descuento")

                    txtPrecioA.Text = ft.muestraCampoNumero(.Item("precio_a"))
                    txtPrecioB.Text = ft.muestraCampoNumero(.Item("precio_b"))
                    txtPrecioC.Text = ft.muestraCampoNumero(.Item("precio_c"))
                    txtPrecioD.Text = ft.muestraCampoNumero(.Item("precio_d"))
                    txtPrecioE.Text = ft.muestraCampoNumero(.Item("precio_e"))
                    txtPrecioF.Text = ft.muestraCampoNumero(.Item("precio_f"))

                    txtDescA.Text = ft.muestraCampoNumero(.Item("desc_a"))
                    txtDescB.Text = ft.muestraCampoNumero(.Item("desc_b"))
                    txtDescC.Text = ft.muestraCampoNumero(.Item("desc_c"))
                    txtDescD.Text = ft.muestraCampoNumero(.Item("desc_d"))
                    txtDescE.Text = ft.muestraCampoNumero(.Item("desc_e"))
                    txtDescF.Text = ft.muestraCampoNumero(.Item("desc_f"))

                    txtGanA.Text = ft.muestraCampoNumero(.Item("ganan_a"))
                    txtGanB.Text = ft.muestraCampoNumero(.Item("ganan_b"))
                    txtGanC.Text = ft.muestraCampoNumero(.Item("ganan_c"))
                    txtGanD.Text = ft.muestraCampoNumero(.Item("ganan_d"))
                    txtGanE.Text = ft.muestraCampoNumero(.Item("ganan_e"))
                    txtGanF.Text = ft.muestraCampoNumero(.Item("ganan_f"))

                    txtOfertaA.Text = ft.FormatoNumero(PrecioOferta(myConn, .Item("codart"), "A", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaB.Text = ft.FormatoNumero(PrecioOferta(myConn, .Item("codart"), "B", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaC.Text = ft.FormatoNumero(PrecioOferta(myConn, .Item("codart"), "C", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaD.Text = ft.FormatoNumero(PrecioOferta(myConn, .Item("codart"), "D", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaE.Text = ft.FormatoNumero(PrecioOferta(myConn, .Item("codart"), "E", jytsistema.sFechadeTrabajo, lblInfo))
                    txtOfertaF.Text = ft.FormatoNumero(PrecioOferta(myConn, .Item("codart"), "F", jytsistema.sFechadeTrabajo, lblInfo))

                    ColorOfertas(txtOfertaA, Color.AliceBlue)
                    ColorOfertas(txtOfertaB, Color.AliceBlue)
                    ColorOfertas(txtOfertaC, Color.AliceBlue)
                    ColorOfertas(txtOfertaD, Color.AliceBlue)
                    ColorOfertas(txtOfertaE, Color.AliceBlue)
                    ColorOfertas(txtOfertaF, Color.AliceBlue)

                    txtBarraA.Text = ft.muestraCampoTexto(.Item("barra_a"))
                    txtBarraB.Text = ft.muestraCampoTexto(.Item("barra_b"))
                    txtBarraC.Text = ft.muestraCampoTexto(.Item("barra_c"))
                    txtBarraD.Text = ft.muestraCampoTexto(.Item("barra_d"))
                    txtBarraE.Text = ft.muestraCampoTexto(.Item("barra_e"))
                    txtBarraF.Text = ft.muestraCampoTexto(.Item("barra_f"))

                    cmbTipo.SelectedIndex = .Item("tipoart")
                    cmbMIX.SelectedIndex = .Item("mix")

                    chkCombo.Checked = CBool(ft.DevuelveScalarEntero(myConn, "Select IF( count(*) > 0, 1, 0) FROM jsmercatcom WHERE CODART = '" & .Item("CODART") & "' AND ID_EMP = '" & jytsistema.WorkID & "'  "))

                    txtIngreso.Text = ft.muestraCampoFecha(CDate(.Item("creacion").ToString))
                    cmbCondicion.SelectedIndex = .Item("estatus")

                    If ExisteTabla(myConn, jytsistema.WorkDataBase, "jsmerctainvfot") Then
                        Dim dtFoto As DataTable = ft.AbrirDataTable(ds, "tblFoto", myConn, " select Foto1 from jsmerctainvfot " _
                                                                    & " where codart = '" & .Item("codart") & "' and " _
                                                                    & " id_emp = '" & jytsistema.WorkID & "'  ")
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
                    txtCodigoMovimientos.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombreMovimientos.Text = ft.muestraCampoTexto(.Item("nomart"))

                    'Compras
                    txtCodigoCompras.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombreCompras.Text = ft.muestraCampoTexto(.Item("nomart"))

                    'Ventas
                    txtCodigoVentas.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombreVentas.Text = ft.muestraCampoTexto(.Item("nomart"))

                    'Existencias
                    txtCodigoExistencias.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombreExistencias.Text = ft.muestraCampoTexto(.Item("nomart"))

                    'Cuotas
                    txtCodigoCuotas.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombreCuotas.Text = ft.muestraCampoTexto(.Item("nomart"))

                    'Expedientes
                    txtCodigoExpediente.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombreExpediente.Text = ft.muestraCampoTexto(.Item("nomart"))

                    'Envases
                    txtCodigoEnvase.Text = ft.muestraCampoTexto(.Item("codart"))
                    txtNombreEnvase.Text = ft.muestraCampoTexto(.Item("nomart"))

                    'AbrirMovimientos(.Item("codart"))
                    AbrirEquivalencias(.Item("codart"), .Item("unidad"))

                End With
            End With
        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub AbrirEquivalencias(ByVal CodigoArticulo As String, ByVal Unidad As String)

        strSQLEQu = " select codart, unidad, equivale, uvalencia, elt(divide + 1,'No','Si') divide, DIVIDE nDIVIDE, " _
            & " ENVASE, CODIGO_ENVASE, ID_EMP from jsmerequmer " _
            & " where " _
            & " codart = '" & CodigoArticulo & "' and " _
            & " unidad = '" & Unidad & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' order by UVALENCIA "

        dtEquivalencias = ft.AbrirDataTable(ds, nTablaEquivalencias, myConn, strSQLEQu)

        Dim aCamEqu() As String = {"equivale.Equivale.70.D.Cantidad", "uvalencia.UND.40.C.", "divide.Divide.30.C."}

        ft.IniciarTablaPlus(dgEqu, dtEquivalencias, aCamEqu, , , , False)

        If dtEquivalencias.Rows.Count > 0 Then nPosicionEqu = 0
        AsignaEqu(nPosicionEqu, False)

    End Sub
    Private Sub AbrirMovimientos(ByVal CodigoArticulo As String)

        dg.DataSource = Nothing

        strSQLMov = " SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, a.almacen, a.cantidad, a.costotal, a.costotaldes," _
                              & " a.lote, a.peso, a.origen, a.prov_cli, IF(b.nombre IS NULL, IF( c.nombre IS NULL, '', c.nombre) , b.nombre) nomProv_Cli, a.asiento, a.fechasi, a.vendedor,  " _
                              & " CONCAT(d.nombres, ' ' , d.apellidos) nomVendedor, a.id_emp " _
                              & " FROM jsmertramer a " _
                              & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp AND a.origen IN ('FAC', 'PFC', 'NCV', 'NDV') ) " _
                              & " LEFT JOIN jsprocatpro c ON (a.prov_cli = c.codpro AND a.id_emp = c.id_emp AND a.origen IN ('COM', 'REP', 'NCC' 'NDC') ) " _
                              & " LEFT JOIN jsvencatven d ON (a.vendedor = d.codven AND a.id_emp = d.id_emp ) " _
                              & " WHERE " _
                              & " a.codart = '" & CodigoArticulo & "' AND " _
                              & " a.id_emp = '" & jytsistema.WorkID & "' " _
                              & " ORDER BY fechamov DESC "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        Dim aCampos() As String = {"fechamov.Emisión.80.C.fecha", _
                                   "tipomov.TP.35.C.", _
                                   "numdoc.Documento.100.I.", _
                                   "unidad.UND.35.C.", _
                                   "almacen.ALM.50.C.", _
                                   "cantidad.Cantidad.100.D.Cantidad", _
                                   "costotal.Costo Total.120.D.Numero", _
                                   "costotaldes.Costo Total y Desc.120.D.Numero", _
                                   "peso.Peso.100.D.Cantidad", _
                                   "origen.ORG.35.C.", _
                                   "prov_cli.Prov/Clie.80.C.", _
                                   "nomProv_cli.Nombre o razón social.230.I.", _
                                   "vendedor.Asesor.50.C.", _
                                   "nomvendedor.Nombre.90.I."}

        ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        ft.RellenaCombo(aTipoMovimiento, cmbTipoMovimiento)

    End Sub
    Private Sub IniciarMercancia(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "arttmp" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtAlterno, txtBarras, txtNombre, txtCategoria, txtMarca, txtDivision, txtTipJer, _
                           txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6, txtJerarquiaNombre, _
                           txtPresentacion, txtUbica1, txtUbica2, txtUbica3, txtOfertaA, txtOfertaB, txtOfertaC, _
                           txtOfertaD, txtOfertaE, txtOfertaF, txtCodigoMovimientos, txtNombreMovimientos, txtBarraA, txtBarraB, _
                           txtBarraC, txtBarraD, txtBarraE, txtBarraF, txtSICA, txtSCAS, txtCEP)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtPrecioA, txtPrecioB, txtPrecioC, txtPrecioD, txtPrecioE, txtPrecioF, _
                        txtDescA, txtDescB, txtDescC, txtDescD, txtDescE, txtDescF, txtGanA, txtGanB, txtGanC, _
                        txtGanD, txtGanE, txtGanF, txtPorDevoluciones, txtSugerido)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cantidad, txtExMin, txtExMax, txtAlto, txtAncho, txtProfun, txtPesoUnidad)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Fecha, txtIngreso)

        cmbUnidad.SelectedIndex = ft.InArray(aUnidadAbreviada, "UND")
        cmbUnidadDetal.SelectedIndex = ft.InArray(aUnidadAbreviada, "UND")

        ft.RellenaCombo(aIVA, cmbIVA, 1)
        cmbIVA.SelectedIndex = ft.InArray(aIVA, "A")

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

        ft.habilitarObjetos(False, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5, C1DockingTabPage6, _
                         C1DockingTabPage7)

        ActivarEnGrupodeControles(tbcMercas.TabPages(0).Controls, txtCodigo)
        ActivarEnGrupodeControles(grpPrecios.Controls, txtCodigo)

        ft.habilitarObjetos(False, False, txtCategoria, txtCategoriaNombre, txtMarca, txtMarcaNombre, txtDivision, _
                         txtDivisionNombre, txtTipJer, txtTipjerNombre, txtCodjer1, txtCodjer2, txtCodjer3, _
                         txtCodjer4, txtCodjer5, txtCodjer6, txtJerarquiaNombre, txtIVA, txtIngreso, _
                         txtOfertaA, txtOfertaB, txtOfertaC, txtOfertaD, txtOfertaE, txtOfertaF, chkTallas)

        ft.habilitarObjetos(True, False, btnFoto, cmbTipo, cmbMIX, cmbCondicion)

        ft.habilitarObjetos(True, False, btnAgregaEquivale, btnEditaEquivale, btnEliminaEquivale)

        If i_modo = movimiento.iAgregar Then ft.habilitarObjetos(True, False, btnCombo)
        If i_modo = movimiento.iEditar And chkCombo.Checked Then ft.habilitarObjetos(True, False, btnCombo)

        If i_modo = movimiento.iAgregar Then ft.visualizarObjetos(True, txtGanA, txtGanB, txtGanC, txtGanD, txtGanE, txtGanF)
        If i_modo = movimiento.iEditar Then ft.habilitarObjetos(False, False, txtCodigo)


        ColorOfertas(txtOfertaA, Color.White)
        ColorOfertas(txtOfertaB, Color.White)
        ColorOfertas(txtOfertaC, Color.White)
        ColorOfertas(txtOfertaD, Color.White)
        ColorOfertas(txtOfertaE, Color.White)
        ColorOfertas(txtOfertaF, Color.White)

        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) < 1 Then
            ft.visualizarObjetos(False, txtGanA, txtGanB, txtGanC, txtGanD, txtGanE, txtGanF)
            btnAgregaEquivale.Enabled = btnAgregar.Enabled
            btnEditaEquivale.Enabled = btnEditaEquivale.Enabled
            btnEliminaEquivale.Enabled = btnEliminaEquivale.Enabled
        End If

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(True, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5, C1DockingTabPage6, _
                         C1DockingTabPage7)

        ft.habilitarObjetos(False, True, txtCodigo, txtCodigoMovimientos, txtAlterno, txtBarras, _
                         txtNombre, txtNombreMovimientos, txtCategoria, txtCategoriaNombre, _
                         txtMarca, txtMarcaNombre, txtTipJer, txtTipjerNombre, txtDivision, _
                         txtDivisionNombre, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, _
                         txtCodjer5, txtCodjer6, txtJerarquiaNombre, txtSCAS, txtCEP, txtPresentacion, txtSugerido, _
                         chkRegulada, chkDevoluciones, txtPorDevoluciones, cmbUnidad, cmbUnidadDetal, txtPesoUnidad, _
                         txtExMin, txtExMax, txtUbica1, txtUbica2, txtUbica3, txtAlto, txtAncho, _
                         txtProfun, cmbIVA, txtIVA, btnIVA, btnCategoria, btnMarca, btnDivision, _
                         btnTipJer, chkCartera, chkDescuentos, txtPrecioA, txtPrecioB, txtPrecioC, _
                         txtPrecioD, txtPrecioE, txtPrecioF, txtDescA, txtDescB, txtDescC, txtDescD, _
                         txtDescE, txtDescF, txtGanA, txtGanB, txtGanC, txtGanD, txtGanE, txtGanF, _
                         txtOfertaA, txtOfertaB, txtOfertaC, txtOfertaD, txtOfertaE, txtOfertaF, _
                         txtBarraA, txtBarraB, txtBarraC, txtBarraD, txtBarraE, txtBarraF, _
                         chkTallas, btnTallas, chkCombo, btnCombo, btnFoto, cmbTipo, cmbMIX, txtIngreso, btnIngreso, cmbCondicion, txtSICA)

        ft.habilitarObjetos(False, False, btnAgregaEquivale, btnEditaEquivale, btnEliminaEquivale)

        ColorOfertas(txtOfertaA, Color.AliceBlue)
        ColorOfertas(txtOfertaB, Color.AliceBlue)
        ColorOfertas(txtOfertaC, Color.AliceBlue)
        ColorOfertas(txtOfertaD, Color.AliceBlue)
        ColorOfertas(txtOfertaE, Color.AliceBlue)
        ColorOfertas(txtOfertaF, Color.AliceBlue)

        MenuEquivalencia.Enabled = False
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) < 1 Then
            ft.visualizarObjetos(False, txtGanA, txtGanB, txtGanC, txtGanD, txtGanE, txtGanF)
        End If
        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
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

                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                With dtMovimientos.Rows(nPosicionMov)
                    Dim aCamposAdicionales() As String = {"CODART|'" & txtCodigo.Text & "'", _
                                                          "FECHAMOV|'" & ft.FormatoFechaHoraMySQL(Convert.ToDateTime(.Item("FECHAMOV"))) & "'", _
                                                          "NUMDOC|'" & .Item("numdoc") & "'", _
                                                          "TIPOMOV|'" & .Item("TIPOMOV") & "'", _
                                                          "ASIENTO|'" & .Item("ASIENTO") & "'"}

                    If DocumentoBloqueado(myConn, "jsmertramer", aCamposAdicionales) Then
                        ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                    Else

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
                    End If
                End With
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

        Dim aCamposDel() As String = {"codart", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then
                ft.Ejecutar_strSQL(myConn, " delete from jsmerequmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsmerextalm where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsmerexpmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmerctainv", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            Else
                ft.mensajeCritico("EstA MERCANCIA posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub EliminarMovimiento()
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If nPosicionMov >= 0 Then
            With dtMovimientos.Rows(nPosicionMov)
                Dim aCamposAdicionales() As String = {"CODART|'" & txtCodigo.Text & "'", _
                                                      "FECHAMOV|'" & ft.FormatoFechaHoraMySQL(Convert.ToDateTime(.Item("FECHAMOV"))) & "'", _
                                                      "NUMDOC|'" & .Item("numdoc") & "'", _
                                                      "TIPOMOV|'" & .Item("TIPOMOV") & "'", _
                                                      "ASIENTO|'" & .Item("ASIENTO") & "'"}

                If DocumentoBloqueado(myConn, "jsmertramer", aCamposAdicionales) Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else


                    If .Item("origen").ToString = "INV" Then
                        If Not (.Item("PROV_CLI").Equals("COMBO") Or .Item("PROV_CLI").Equals("COMBO_REV")) Then
                            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))

                                Dim aCamposDel() As String = {"codart", "numdoc", "origen", "Ejercicio", "id_emp"}
                                Dim aFieldsDel() As String = {.Item("codart"), .Item("numdoc"), "INV", jytsistema.WorkExercise, jytsistema.WorkID}

                                nPosicionMov = EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jsmertramer", strSQLMov, aCamposDel, aFieldsDel, nPosicionMov)

                                ActualizarExistenciasPlus(myConn, txtCodigo.Text)

                                AsignaTXT(nPosicionCat)
                                AsignaMov(nPosicionMov, False)

                            End If
                        End If
                    Else
                        ft.mensajeCritico("Movimiento proveniente de " & dtMovimientos.Rows(nPosicionMov).Item("origen") & ". ")
                    End If

                End If

            End With

        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                Dim Campos() As String = {"codart", "nomart", "alterno", "barras"}
                Dim Nombres() As String = {"Código", "Nombre", "Código alterno", "Código barras"}
                Dim Anchos() As Integer = {120, 850, 100, 120}
                f.Text = "Mercancias"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Mercancías...")
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
            Case "Movimientos"
                Dim Campos() As String = {"fechamov", "numdoc", "prov_cli", "nomProv_Cli"}
                Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Código Prov./Cli.", "Cliente ó Proveedor"}
                Dim Anchos() As Integer = {100, 120, 120, 500}
                f.Text = "Movimientos de mercancía"
                f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de mercancías...")
                nPosicionMov = f.Apuntador
                Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
                AsignaMov(nPosicionMov, False)
        End Select

        f = Nothing


    End Sub

    


    Private Function Validado() As Boolean
        Validado = False

        '////// ESTA VALIDAR UNIDAD DE VENTA AL DETAL
        If cmbUnidadDetal.SelectedIndex >= 0 Then

            If ft.DevuelveScalarCadena(myConn, "SELECT a.unidad from (SELECT '" & aUnidadAbreviada(cmbUnidadDetal.SelectedIndex) & "' unidad  " _
                                     & " UNION " _
                                     & " SELECT uvalencia unidad FROM jsmerequmer " _
                                     & " WHERE " _
                                     & " codart = '" & txtCodigo.Text & "' AND " _
                                     & " id_emp = '" & jytsistema.WorkID & "') a WHERE '" & aUnidadAbreviada(cmbUnidadDetal.SelectedIndex) & "' = a.unidad") = "" Then

                ft.mensajeCritico("UNIDAD DE VENTA AL DETAL NO PERMITIDA. VERIFIQUE POR FAVOR...")
                Return False
            End If

        End If

        If i_modo = movimiento.iAgregar AndAlso Mid(txtCodigo.Text, 1, 6) <> "arttmp" Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsmerctainv where codart = '" & txtCodigo.Text _
                                       & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
                ft.mensajeCritico("CODIGO DE MERCANCIA YA EXISTE. VERIFIQUE POR FAVOR...")
                Return False
            End If
        End If

        '//// VALIDACION CODIGOS DE BARRAS
        Dim aBarraA() As TextBox = {txtBarras, txtBarraA, txtBarraB, txtBarraC, txtBarraD, txtBarraE, txtBarraF}
        Dim aBarraB() As TextBox = {txtBarras, txtBarraA, txtBarraB, txtBarraC, txtBarraD, txtBarraE, txtBarraF}

        For Each barraA As TextBox In aBarraA
            For Each barraB As TextBox In aBarraB
                If barraA.Text <> "" AndAlso barraB.Text <> "" Then
                    If barraA.Name <> barraB.Name AndAlso barraA.Text = barraB.Text Then
                        ft.mensajeCritico(barraA.Tag & " = " & barraB.Tag & ". VERIFIQUE POR FAVOR...")
                        Return False
                    End If
                End If
            Next
            If barraA.Text <> "" AndAlso ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsmerctainv " _
                                         & " where " _
                                         & " codart <> '" & txtCodigo.Text & "' AND " _
                                         & " " & barraA.Tag.ToString.Split(" ")(1) & " = '" & barraA.Text & "' and " _
                                         & " id_emp = '" & jytsistema.WorkID & "' ") > 0 Then

                ft.mensajeCritico(barraA.Tag.ToString & " YA EXISTE. VERIFIQUE POR FAVOR...")
                Return False
            End If
        Next


        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM01")) AndAlso txtTipjerNombre.Text = "" Then
            ft.mensajeCritico("  DEBE INDICAR UNA JERARQUIA VALIDA...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM03")) AndAlso txtMarcaNombre.Text = "" Then
            ft.mensajeCritico("  DEBE INDICAR UNA MARCA VALIDA...")
            Return False
        End If
        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM04")) AndAlso txtCategoriaNombre.Text = "" Then
            ft.mensajeCritico("  DEBE INDICAR UNA CATEGORIA VALIDA...")
            Return False
        End If
        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM05")) AndAlso txtDivisionNombre.Text = "" Then
            ft.mensajeCritico("  DEBE INDICAR UNA DIVISION VALIDA...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM06")) AndAlso ValorNumero(txtPesoUnidad.Text) = 0.0 Then
            ft.mensajeCritico("  DEBE INDICAR EL PESO DE LA UNIDAD DE VENTA VALIDO...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM08")) Then
            Dim equivalenciaAVerificar As String = CStr(ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07"))


            If Not MercanciaPoseeEquivalencia(myConn, lblInfo, txtCodigo.Text, equivalenciaAVerificar) Then
                If aUnidadAbreviada(cmbUnidad.SelectedIndex) <> equivalenciaAVerificar Then
                    ft.mensajeCritico("  DEBE indicar la equivalencia de esta mercancías en " & equivalenciaAVerificar & " ...")
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
                CodigoArticulo = ft.autoCodigo(myConn, "codart", "jsmerctainv", "id_emp", jytsistema.WorkID, 8, True)
            Else
                CodigoArticulo = txtCodigo.Text
            End If

            ft.Ejecutar_strSQL(myconn, " update jsmerequmer set codart = '" & CodigoArticulo & "' where codart = '" & txtCodigo.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsmerexpmer set codart = '" & CodigoArticulo & "' where codart = '" & txtCodigo.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsmercatcom set codart = '" & CodigoArticulo & "' where codart = '" & txtCodigo.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")

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
                                    txtSICA.Text, txtSCAS.Text, txtCEP.Text)

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, CodigoArticulo)

        If Not pctFoto.Image Is Nothing Then
            If CaminoFoto <> "" Then
                Dim fs As New FileStream(CaminoFoto, FileMode.OpenOrCreate, FileAccess.Read)
                MyData = New Byte(fs.Length) {}
                fs.Read(MyData, 0, fs.Length)
                fs.Close()
                GuardarFotoMercancia(myConn, lblInfo, ft.DevuelveScalarBooleano(myConn, " SELECT COUNT( DISTINCT codart ) FROM jsmerctainvfot WHERE codart = '" & CodigoArticulo & "' AND id_emp = '" & jytsistema.WorkID & "' "), CodigoArticulo, MyData)
            End If
        End If

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODART = '" & CodigoArticulo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)
        Me.BindingContext(ds, nTabla).Position = nPosicionCat

        AsignaTXT(nPosicionCat)

        DesactivarMarco0()
        tbcMercas.SelectedTab = C1DockingTabPage1
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

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
                ft.mensajeEtiqueta(lblInfo, " Indique el código de la mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtAlterno"
                ft.mensajeEtiqueta(lblInfo, " Indique el código alterno de la mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtBarras"
                ft.mensajeEtiqueta(lblInfo, " Indique el código de barras de la mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, " Indique el nombre o descripción de la mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtPresentacion"
                ft.mensajeEtiqueta(lblInfo, " Indique el presentación de la mercancías (ej. 12x185g = 12 unidades de 185 gramos) ... ", Transportables.TipoMensaje.iInfo)
            Case "txtSugerido"
                ft.mensajeEtiqueta(lblInfo, " Indique el precio de venta sugerido por proveedor ... ", Transportables.TipoMensaje.iInfo)
            Case "txtPorDevoluciones"
                ft.mensajeEtiqueta(lblInfo, " Indique el porcentaje para aceptación de devolución de mercancía (ej. 70% = se devuelve por 70 por ciento del precio con el que fue vendida ... ", Transportables.TipoMensaje.iInfo)
            Case "txtExMin"
                ft.mensajeEtiqueta(lblInfo, " Indique el existencia mínima de esta mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtExMax"
                ft.mensajeEtiqueta(lblInfo, " Indique el existencia máxima de esta mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtPesoUnidad"
                ft.mensajeEtiqueta(lblInfo, " Indique el peso de la unidad de venta ... ", Transportables.TipoMensaje.iInfo)
            Case "txtUbica1", "txtUbica2", "txtUbica3"
                ft.mensajeEtiqueta(lblInfo, " Indique la ubicación de la mercancia ... ", Transportables.TipoMensaje.iInfo)
            Case "txtAlto"
                ft.mensajeEtiqueta(lblInfo, " Indique el alto de la mercancía en metros (ej. 0.25 mtr ) ... ", Transportables.TipoMensaje.iInfo)
            Case "txtAncho"
                ft.mensajeEtiqueta(lblInfo, " Indique el ancho de la mercancía en metros (ej. 0.25 mtr ) ... ", Transportables.TipoMensaje.iInfo)
            Case "txtProfun"
                ft.mensajeEtiqueta(lblInfo, " Indique el profundidad de la mercancía en metros (ej. 0.25 mtr ) ... ", Transportables.TipoMensaje.iInfo)
            Case "txtPrecioA", "txtPrecioB", "txtPrecioC", "txtPrecioD", "txtPrecioE", "txtPrecioF"
                ft.mensajeEtiqueta(lblInfo, " Indique el precio de venta de la mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtDescA", "txtDescB", "txtDescC", "txtDescD", "txtDescE", "txtDescF"
                ft.mensajeEtiqueta(lblInfo, " Indique el descuento de venta por precio de la mercancía ... ", Transportables.TipoMensaje.iInfo)
            Case "txtGanA", "txtGanB", "txtGanC", "txtGanD", "txtGanE", "txtGanF"
                ft.mensajeEtiqueta(lblInfo, " Indique la ganancia por precio de la mercancía ... ", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        e.Value = ft.dataGridViewCellFormating(dg, e)
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcMercas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcMercas.SelectedIndexChanged
        C1SuperTooltip1.SetToolTip(btnRecalcular, "<B></B>")
        Select Case tbcMercas.SelectedIndex
            Case 0 ' Mercancias
                AsignaTXT(nPosicionCat)
            Case 1 ' Movimientos
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalcular costos de mercancías</B>")
                AbrirMovimientos(txtCodigo.Text)
                nPosicionMov = 0

                AsignaMov(nPosicionMov, True)
                Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
                dg.Enabled = True
            Case 2 ' Compras
                AsignarCompras(txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 3 ' Ventas
                AsignarVentas(txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 4 ' Existencias
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalcular existencias</B>")
                AsignarExistencias(txtCodigo.Text)
            Case 5 'Cuotas

                AsignarCuotas(txtCodigo.Text)
                chkCuotaFija.Checked = ft.DevuelveScalarBooleano(myConn, " select cuotafija from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                strUnidad = ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")
                If strUnidad <> "" Then
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTAS", "MEDIDA SECUNDARIA (" & strUnidad & ")"}
                Else
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTA"}
                End If
                ft.RellenaCombo(aUnidad, cmbUnidadCuota)

                IniciarCuotas(txtCodigo.Text, cmbUnidadCuota.SelectedIndex)
            Case 6 ' Expediente
                AsignarExpediente(txtCodigo.Text)
            Case 7
                AbrirEnvases(txtCodigo.Text)
        End Select
    End Sub

    Private Sub AbrirEnvases(ByVal CodigoArticulo As String)

        dgEnvases.DataSource = Nothing

        strSQLEnvases = " SELECT a.*, IF(b.nombre IS NULL, IF( c.nombre IS NULL, '', c.nombre) , b.nombre) nomProv_Cli, " _
            & " CONCAT(d.nombres, ' ' , d.apellidos) nomVendedor, elt(a.estatus + 1, " _
            & " 'Tránsito', 'Cliente', 'Proveedor', 'Vacío/Depósito', 'Lleno/Depósito', 'Reparación', 'Desincorporado', 'Indeterminado') nomEstatus " _
            & " from jsmertraenv a " _
            & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp AND a.origen IN ('FAC', 'PFC', 'NCV', 'NDV') ) " _
            & " LEFT JOIN jsprocatpro c ON (a.prov_cli = c.codpro AND a.id_emp = c.id_emp AND a.origen IN ('COM', 'REP', 'NCC' 'NDC') ) " _
            & " LEFT JOIN jsvencatven d ON (a.vendedor = d.codven AND a.id_emp = d.id_emp ) " _
            & " WHERE " _
            & " a.item = '" & CodigoArticulo & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY fechamov DESC "

        dtEnvases = ft.AbrirDataTable(ds, nTablaEnvases, myConn, strSQLEnvases)

        Dim aCampos() As String = {"fechamov.Emisión.80.C.fecha", _
                                   "tipomov.TP.35.C.", _
                                   "numdoc.Documento.100.I.", _
                                   "almacen.ALM.50.C.", _
                                   "cantidad.Cantidad.70.D.Entero", _
                                   "origen.ORG.35.C.", _
                                   "prov_cli.Prov/Clie.80.C.", _
                                   "nomProv_cli.Nombre o razón social.300.I.", _
                                   "vendedor.Asesor.50.C.", _
                                   "nomvendedor.Nombre.300.I.", _
                                   "nomEstatus.Estatus.200.I.", _
                                   "sada..10.I."}

        ft.IniciarTablaPlus(dgEnvases, dtEnvases, aCampos)
        If dtEnvases.Rows.Count > 0 Then nPosicionEnv = 0

    End Sub
    Private Sub AsignarCompras(ByVal nArticulo As String)

        ft.habilitarObjetos(False, True, txtCodigoCompras, txtNombreCompras, txtOrdenes, txtRecepciones, txtBackorder, _
            txtFechaUltimaCompr, txtUltimoProveedor, txtEntradas, txtCostoAcum, txtCostoAcumDesc, txtUltimoCosto, _
            txtDevolucionesCompras, txtCostoAcumDev, txtCostoAcumDevDesc, txtCostoPromedio, txtNombreUltimoProveedor)

        If dt.Rows.Count > 0 Then
            With dt.Rows(nPosicionCat)

                txtOrdenes.Text = ft.FormatoCantidad(.Item("ORDENES"))
                txtRecepciones.Text = ft.FormatoCantidad(.Item("RECEPCIONES"))
                txtBackorder.Text = ft.FormatoCantidad(.Item("BACKORDER"))
                txtFechaUltimaCompr.Text = ft.FormatoFecha(CDate(.Item("FECULTCOSTO").ToString))
                txtUltimoProveedor.Text = IIf(IsDBNull(.Item("ULTIMOPROVEEDOR")), "", .Item("ultimoproveedor"))
                txtEntradas.Text = ft.FormatoCantidad(.Item("ENTRADAS"))
                txtCostoAcum.Text = ft.FormatoNumero(.Item("ACU_COS"))
                txtCostoAcumDesc.Text = ft.FormatoNumero(.Item("ACU_COS_DES"))
                txtUltimoCosto.Text = ft.FormatoNumero(.Item("montoultimacompra"))
                txtDevolucionesCompras.Text = ft.FormatoCantidad(.Item("CREDITOSCOMPRAS"))
                txtCostoAcumDev.Text = ft.FormatoNumero(.Item("ACU_COD"))
                txtCostoAcumDevDesc.Text = ft.FormatoNumero(.Item("ACU_COD_DES"))
                txtCostoPromedio.Text = ft.FormatoNumero(.Item("COSTO_PROM_DES"))

                txtUltimoCosto.Text = ft.FormatoNumero(UltimoCostoAFecha(myConn, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))
                Dim xx As String = UltimoProveedor(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo)
                txtUltimoProveedor.Text = IIf(xx = "0", "", xx)
                txtFechaUltimaCompr.Text = ft.FormatoFecha(UltimaFechaCompra(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))

            End With

            rBtn1.Checked = True
            VerHistograma(c1Chart1, 0, txtCodigo.Text, 0)

        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)


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

        ft.habilitarObjetos(False, True, txtCodigoVentas, txtNombreVentas, txtCotizacion, txtPedidos, txtEntregas, _
            txtFechaUltimaVenta, txtUltimoCliente, txtSalidas, txtVentasAcum, txtVentasAcumDesc, txtPrecioUltimo, _
            txtDevolucionesVentas, txtVentasAcumDev, txtVentasAcumDevDesc, txtPrecioPromedio, txtNombreUltimoCliente)

        If dt.Rows.Count > 0 Then
            With dt.Rows(nPosicionCat)
                txtCotizacion.Text = ft.FormatoCantidad(.Item("cotizados"))
                txtPedidos.Text = ft.FormatoCantidad(.Item("pedidos"))
                txtEntregas.Text = ft.FormatoCantidad(.Item("entregas"))
                txtFechaUltimaVenta.Text = ft.FormatoFecha(CDate(.Item("FECULTventa").ToString))
                txtUltimoCliente.Text = IIf(IsDBNull(.Item("ultimocliente")), "", .Item("ultimocliente"))
                txtSalidas.Text = ft.FormatoCantidad(.Item("salidas"))
                txtVentasAcum.Text = ft.FormatoNumero(.Item("ACU_PRE"))
                txtVentasAcumDesc.Text = ft.FormatoNumero(.Item("ACU_PRE_DES"))
                txtPrecioUltimo.Text = ft.FormatoNumero(.Item("montoultimaventa"))
                txtDevolucionesVentas.Text = ft.FormatoCantidad(.Item("CREDITOSventas"))
                txtVentasAcumDev.Text = ft.FormatoNumero(.Item("ACU_PRD"))
                txtVentasAcumDevDesc.Text = ft.FormatoNumero(.Item("ACU_prd_DES"))
                txtPrecioPromedio.Text = ft.FormatoNumero(.Item("Venta_PROM_DES"))
            End With

            rBtnV1.Checked = True
            VerHistograma(C1Chart2, 1, txtCodigo.Text, 0)
        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)


    End Sub
    Private Sub AsignarExistencias(ByVal nArticulo As String)

        ft.habilitarObjetos(False, True, txtCodigoExistencias, txtNombreExistencias, txtExisteUnidad, _
                         txtExistenciaKilos, txtPromedioUnidad, txtPromedioKilos, txtSugeridoUnidad, _
                         txtSugeridoKilos, txtUnidad1, txtUnidad2, txtUnidad4, txtKilos1, txtKilos2, _
                         txtKilos4, txtInventarioDias)

        Dim nMeses As Integer = 3
        Dim nDiasSugeridos As Integer = 10

        txtPromedioMeses.Text = ft.FormatoEntero(nMeses)
        txtSugeridoDias.Text = ft.FormatoEntero(nDiasSugeridos)

        PresentarExistencias(nArticulo, nMeses, nDiasSugeridos)

        Dim aExistencias() As String = {"Almacén", "Lote", "Color", "Talla", "Almacén y color ", "Almacen y Talla", "Almacen, color y talla", "Lote y color", "Lote y talla", "Lote, color y talla"}
        ft.RellenaCombo(aExistencias, cmbExistencias)

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
            & " SUM(IF( b.origen IN ('FAC', 'PVE', 'PFC') AND b.fechamov >= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH) AND  b.fechamov <= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL 1 DAY) ,  b.cantidad, 0.000 ) ) ventas, " _
            & " " _
            & " b.id_emp " _
            & " FROM jsmertramer b " _
            & " WHERE " _
            & "      b.tipomov <> 'AC' AND " _
            & "      b.codart = '" & nArticulo & "' AND " _
            & "      b.id_emp = '" & jytsistema.WorkID & "' AND  " _
            & "      b.ejercicio = '" & jytsistema.WorkExercise & "' AND  " _
            & "      date_format(b.fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " _
            & "      GROUP BY b.codart, " & str2 & " b.unidad ) a  " _
            & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp )  " _
            & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT '1' num, (CURRENT_DATE - DATE_ADD(CURRENT_DATE, INTERVAL -" & nMeses & " MONTH)) -  IFNULL(COUNT(*),0) DiasHabiles " _
            & " FROM (SELECT CONCAT(ano,'-',IF(LENGTH(mes)=1,CONCAT('0',mes),mes),'-',IF(LENGTH(dia)=1,CONCAT('0',dia),dia)) AS fecha FROM jsconcatper WHERE MODULO = 1 AND ID_EMP = '" & jytsistema.WorkID & "' " _
            & " HAVING  (fecha <CURRENT_DATE AND fecha>DATE_ADD(CURRENT_DATE,INTERVAL -" & nMeses & " MONTH)) )  a) d ON ( d.num = '1') " _
            & str3 _
            & str4, myConn, nTablaExPor, lblInfo)

        dtExPor = ds.Tables(nTablaExPor)

        Dim aCamEx() As String = {"codigo", "descripcion", "existencia", "pesototal", "inventario", ""}
        Dim aNomEx() As String = {"Código", "Descripción", "Existencia en Unidades Venta", "Existencia en Kilogramos", "Dias de inventario", ""}
        Dim aAncEx() As Integer = {60, 140, 120, 120, 80, 100}
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
            & " SUM(IF( b.origen IN ('FAC', 'PVE', 'PFC') AND b.fechamov >= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH) AND  b.fechamov <= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL 1 DAY) ,  b.cantidad, 0.000 ) ) ventas, " _
            & " " _
            & " b.id_emp " _
            & " FROM jsmertramer b " _
            & " WHERE " _
            & "      b.tipomov <> 'AC' AND " _
            & "      b.codart = '" & nArticulo & "' AND " _
            & "      b.id_emp = '" & jytsistema.WorkID & "' AND  " _
            & "      b.ejercicio = '" & jytsistema.WorkExercise & "' AND  " _
            & "      date_format(b.fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " _
            & "      GROUP BY b.codart, b.unidad ) a  " _
            & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp )  " _
            & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT '1' num, (DATEDIFF('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL - " & nMeses & " MONTH))) -  IFNULL(COUNT(*),0) DiasHabiles " _
            & " FROM (SELECT CONCAT(ano,'-',IF(LENGTH(mes)=1,CONCAT('0',mes),mes),'-',IF(LENGTH(dia)=1,CONCAT('0',dia),dia)) AS fecha FROM jsconcatper WHERE MODULO = 1 AND ID_EMP = '" & jytsistema.WorkID & "' " _
            & " HAVING  (fecha <'" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND fecha>DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "',INTERVAL -" & nMeses & " MONTH)) )  a) d ON ( d.num = '1') " _
            & " group by 1 ", myConn, nTablaExiste, lblInfo)

        dtExistencias = ds.Tables(nTablaExiste)

        If dtExistencias.Rows.Count > 0 Then

            With dtExistencias.Rows(0)
                txtExisteUnidad.Text = ft.FormatoCantidad(.Item("existencia"))
                txtExistenciaKilos.Text = ft.FormatoCantidad(.Item("pesototal"))
                txtInventarioDias.Text = ft.FormatoEntero(IIf(IsDBNull(.Item("inventario")), 0, .Item("inventario")))
                txtPromedioUnidad.Text = ft.FormatoCantidad(IIf(IsDBNull(.Item("promediodiario")), 0, .Item("promediodiario")))
                txtPromedioKilos.Text = ft.FormatoCantidad(IIf(IsDBNull(.Item("promediodiariopeso")), 0, .Item("promediodiariopeso")))
                txtSugeridoUnidad.Text = ft.FormatoCantidad(nDiasSugeridos * IIf(IsDBNull(.Item("promediodiario")), 0, .Item("promediodiario")))
                txtSugeridoKilos.Text = ft.FormatoCantidad(nDiasSugeridos * IIf(IsDBNull(.Item("promediodiariopeso")), 0, .Item("promediodiariopeso")))
                txtUnidad1.Text = .Item("unidad")
                txtUnidad2.Text = .Item("unidad")
                txtUnidad4.Text = .Item("unidad")
                txtKilos1.Text = "KGR"
                txtKilos2.Text = "KGR"
                txtKilos4.Text = "KGR"
            End With
        End If

    End Sub
    Private Sub AsignarCuotas(ByVal nArticulo As String)

        ft.habilitarObjetos(False, True, txtCodigoCuotas, txtNombreCuotas, txtUnidadCuota)

        Dim dtAse As New DataTable
        Dim nTablaAse As String = "tblAsesores"
        ds = DataSetRequery(ds, " select * from jsvencatven where tipo = 0 and clase = 0 and id_emp = '" & jytsistema.WorkID & "' ", myConn, _
                             nTablaAse, lblInfo)
        dtAse = ds.Tables(nTablaAse)

        For Each nRow As DataRow In dtAse.Rows
            With nRow
                If ft.DevuelveScalarEntero(myConn, " select count(*) from jsvencuoart where codven = '" & .Item("CODVEN") & "' and codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                    ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvencuoart VALUES( '" & .Item("CODVEN") & "','" & txtCodigo.Text & "', " _
                       & " 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,     0, '" & jytsistema.WorkExercise & "', '" & jytsistema.WorkID & "') ")

                End If
            End With
        Next

        dtAse.Dispose()
        dtAse = Nothing


    End Sub
    Private Sub AsignarExpediente(ByVal nArticulo As String)
        ft.habilitarObjetos(False, True, txtCodigoExpediente, txtNombreExpediente)

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
        Dim aAncexp() As Integer = {140, 240, 100, 350}
        Dim aAliExp() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aForExp() As String = {sFormatoFecha, "", "", ""}

        IniciarTabla(dgExpediente, dtExp, aCamExp, aNomExp, aAncexp, aAliExp, aForExp)

    End Sub
    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myconn, " delete from jsmercatcom where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsmerequmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        'OJOOJOJOJO. VERIFICAR TALLAS Y COLORES
        'ft.Ejecutar_strSQL ( myconn, " delete from jsprodescom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        'ft.Ejecutar_strSQL ( myconn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")

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
            If ft.DevuelveScalarEntero(myConn, " SELECT COUNT(*) FROM jsmertramer WHERE " _
                                        & " codart = '" & txtCodigo.Text & "' AND " _
                                        & " unidad = '" & dtEquivalencias.Rows(nPosicionEqu).Item("uvalencia").ToString & "' AND " _
                                        & " id_emp = '" & jytsistema.WorkID & "'") = 0 Then
                Dim g As New jsMerArcMercanciasEquivalencias
                nPosicionEqu = Me.BindingContext(ds, nTablaEquivalencias).Position
                g.Apuntador = nPosicionEqu
                g.Editar(myConn, ds, dtEquivalencias, txtCodigo.Text, aUnidadAbreviada(cmbUnidad.SelectedIndex))
                If g.Apuntador >= 0 Then AsignaEqu(g.Apuntador, True)
                g = Nothing
            Else
                ft.mensajeCritico("ESTA UNIDAD O EQUIVALENCIA YA FUE UTILIZADA NO PUEDE SER MODIFICADA. VERIFIQUE POR FAVOR...")
            End If

        End If
    End Sub

    Private Sub btnEliminaEquivale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaEquivale.Click
        With dtEquivalencias
            If .Rows.Count > 0 Then
                nPosicionEqu = Me.BindingContext(ds, nTablaEquivalencias).Position

                Dim aCamDel() As String = {"codart", "unidad", "uvalencia", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, aUnidadAbreviada(cmbUnidad.SelectedIndex), _
                                           .Rows(nPosicionEqu).Item("uvalencia").ToString, _
                                           jytsistema.WorkID}

                If ft.DevuelveScalarEntero(myConn, " SELECT COUNT(*) FROM jsmertramer WHERE " _
                                         & " codart = '" & txtCodigo.Text & "' AND " _
                                         & " unidad = '" & .Rows(nPosicionEqu).Item("uvalencia").ToString & "' AND " _
                                         & " id_emp = '" & jytsistema.WorkID & "'") = 0 Then

                    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                        AsignaEqu(EliminarRegistros(myConn, lblInfo, ds, nTablaEquivalencias, "jsmerequmer", _
                                                   strSQLEQu, aCamDel, aStrDel, nPosicionEqu), True)

                    End If
                Else
                    ft.mensajeCritico("ESTA UNIDAD O EQUIVALENCIA YA FUE UTILIZADA NO PUEDE SER ELIMINADA. VERIFIQUE POR FAVOR...")
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

        e.Handled = ft.validaNumeroEnTextbox(e)

    End Sub

    Private Sub cmbIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIVA.SelectedIndexChanged
        txtIVA.Text = ft.FormatoNumero(PorcentajeIVA(myConn, lblInfo, jytsistema.sFechadeTrabajo, cmbIVA.Text)) & "%"
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
        txtCategoria.Text = CargarTablaSimplePlusReal("Categorías", Modulo.iCategoriaMerca)
    End Sub
    Private Sub btnMarca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarca.Click
        txtMarca.Text = CargarTablaSimplePlusReal("Marcas", Modulo.iMarcaMerca)
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
        ft.RellenaCombo(aIVA, cmbIVA, ft.InArray(aIVA, f.Seleccionado))

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
            ft.Ejecutar_strSQL(myconn, " update jsmerctainv set montoultimacompra = " & f.CostoTotal & " , pesounidad = " & f.PesoTotal & " where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            txtPesoUnidad.Text = ft.FormatoCantidad(f.PesoTotal)
            txtUltimoCosto.Text = ft.FormatoNumero(f.CostoTotal)
            chkCombo.Checked = IIf(f.CostoTotal > 0, True, False)

            txtPrecioA.Text = ft.FormatoNumero(f.precioTotalA)
            txtPrecioB.Text = ft.FormatoNumero(f.precioTotalB)
            txtPrecioC.Text = ft.FormatoNumero(f.precioTotalC)
            txtPrecioD.Text = ft.FormatoNumero(f.precioTotalD)
            txtPrecioE.Text = ft.FormatoNumero(f.precioTotalE)
            txtPrecioF.Text = ft.FormatoNumero(f.precioTotalF)
            f.Dispose()
            f = Nothing
        End If
    End Sub

    Private Sub txtIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIVA.TextChanged

    End Sub

    Private Sub btnRecalcular_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecalcular.Click
        Select Case tbcMercas.SelectedTab.Text
            Case "Mercancías"
                ft.Ejecutar_strSQL(myconn, " update jsmerctainv set unidaddetal = unidad where unidaddetal = '' and id_emp = '" & jytsistema.WorkID & "' ")
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                AsignaTXT(nPosicionCat)
            Case "Movimientos"
                Dim fg As New jsMerProCostos
                fg.Cargar(myConn, txtCodigoMovimientos.Text, jytsistema.sFechadeTrabajo)
                ft.mensajeInformativo("Actualizado...")
                AsignaMov(0, True)
                fg.Dispose()
                fg = Nothing
            Case "Compras"
                txtUltimoCosto.Text = ft.FormatoNumero(UltimoCostoAFecha(myConn, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))
                txtUltimoProveedor.Text = UltimoProveedor(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo)
                txtFechaUltimaCompr.Text = ft.FormatoFecha(UltimaFechaCompra(myConn, lblInfo, txtCodigoCompras.Text, jytsistema.sFechadeTrabajo))
                ft.mensajeInformativo("Ultimo Costo actualizado...")
            Case "Existencias"
                For Each nrow As DataRow In dt.Rows
                    With nrow
                        ExistenciasEnAlmacenes(myConn, .Item("codart"), "")
                    End With
                Next

                AsignarExistencias(txtCodigoExistencias.Text)
                ft.mensajeInformativo("Actualizado...")
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

                Dim nCosto As Double = UltimoCostoAFecha(myConn, .Item("codart"), CDate(.Item("fechamov").ToString))
                Dim nEquivale As Double = Equivalencia(myConn,  .Item("codart"), .Item("unidad"))
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
            ft.Ejecutar_strSQL(myconn, " delete from jsmerequmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
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
            ft.mensajeEtiqueta(lblInfo, Mensaje, Transportables.tipoMensaje.iAyuda)
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


    Private Sub btnRecalculaPesos_Click(sender As System.Object, e As System.EventArgs) Handles btnRecalculaPesos.Click
        ft.Ejecutar_strSQL(myConn, " UPDATE jsmertramer a " _
                                  & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (" & SeleccionGENTablaEquivalencias() & ") c ON (a.codart = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp) " _
                                  & " SET a.peso = a.cantidad/IFNULL(c.equivale,1)*b.pesounidad " _
                                  & " WHERE " _
                                  & " a.codart = '" & txtCodigo.Text & "' AND " _
                                  & " a.id_emp = '" & jytsistema.WorkID & "'")

        ft.mensajeInformativo("Pesos en movimientos actualizados....")

    End Sub

    '///// CUOTAS
    Private Sub IniciarCuotas(nArticulo As String, KilosUnidadCajas As Integer)

        '0 = Kilos ; 1 = Unidad de Venta ; 2 = Cajas

        Dim strLeft As String = ""
        Dim strLeftD As String = " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp " _
                    & " FROM jsmerequmer a " _
                    & " WHERE " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & " SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                    & " FROM jsmerctainv a " _
                    & " WHERE " _
                    & " a.id_emp = '" & jytsistema.WorkID & "') d  ON (a.codart = d.codart AND d.uvalencia = '" & ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07") & "' AND a.id_emp = d.id_emp ) "

        Dim strWhere As String = ""
        Dim strXX As String = ""

        Select Case KilosUnidadCajas
            Case 1
                strXX = "/a.pesounidad "
            Case 2
                strXX = "*d.equivale/a.pesounidad"
        End Select

        strSQLCuotas = " select c.codven, CONCAT(c.apellidos,', ', c.nombres) VENDEDOR, c.factorcuota, if(" & KilosUnidadCajas & " = 0, 'KGR', if(" & KilosUnidadCajas & " = 1, a.unidad, d.uvalencia ) )  unidad, " _
            & " b.ESMES01" & strXX & " MES01, b.ESMES02" & strXX & " MES02, b.ESMES03" & strXX & " MES03, " _
            & " b.ESMES04" & strXX & " MES04, b.ESMES05" & strXX & " MES05, b.ESMES06" & strXX & " MES06, " _
            & " b.ESMES07" & strXX & " MES07, b.ESMES08" & strXX & " MES08, b.ESMES09" & strXX & " MES09, " _
            & " b.ESMES10" & strXX & " MES10, b.ESMES11" & strXX & " MES11, b.ESMES12" & strXX & " MES12,  " _
            & " (b.ESMES01 + b.ESMES02 + b.ESMES03 + " _
            & " b.ESMES04 + b.ESMES05 + b.ESMES06 + " _
            & " b.ESMES07 + b.ESMES08 + b.ESMES09 + " _
            & " b.ESMES10 + b.ESMES11 + b.ESMES12)" & strXX & "  total " _
            & " from jsmerctainv a " _
            & " left join jsvencuoart b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " left join jsvencatven c on (b.codven = c.codven and b.id_emp = c.id_emp AND c.tipo = 0 and c.estatus = 1) " _
            & strLeftD _
            & " where " _
            & strWhere _
            & " a.codart = '" & txtCodigo.Text & "' AND c.codven <> '' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' order by c.codven  "


        ds = DataSetRequery(ds, strSQLCuotas, myConn, nTablaCuotas, lblInfo)
        dtCuotas = ds.Tables(nTablaCuotas)

        IniciarGrilla()

        txtUnidadCuota.Text = ""
        If dtCuotas.Rows.Count > 0 Then
            nPosicionCuo = 0
            If IsDBNull(dtCuotas.Rows(nPosicionCuo).Item("unidad")) Then
                txtUnidadCuota.Text = ""
            Else
                txtUnidadCuota.Text = dtCuotas.Rows(nPosicionCuo).Item("unidad")
            End If

        End If


    End Sub
    Private Sub IniciarGrilla()

        Dim aCam() As String = {"codven.Código.60.I.", _
                                "vendedor.Apellidos/Nombres.250.I.", _
                                "factorcuota.%.50.D.Numero", _
                                "mes01.ENE.70.D.Cantidad", _
                                "mes02.FEB.70.D.Cantidad", _
                                "mes03.MAR.70.D.Cantidad", _
                                "mes04.ABR.70.D.Cantidad", _
                                "mes05.MAY.70.D.Cantidad", _
                                "mes06.JUN.70.D.Cantidad", _
                                "mes07.JUL.70.D.Cantidad", _
                                "mes08.AGO.70.D.Cantidad", _
                                "mes09.SEP.70.D.Cantidad", _
                                "mes10.OCT.70.D.Cantidad", _
                                "mes11.NOV.70.D.Cantidad", _
                                "mes12.DIC.70.D.Cantidad", _
                                "total.TOTAL.70.D.Cantidad"}
       
        ft.IniciarTablaPlus(dgCuotas, dtCuotas, aCam, , True, New Font("Consolas", 8, FontStyle.Regular))
        If dtCuotas.Rows.Count > 0 Then nPosicionCuo = 0

        Dim aEditar() As String = {"mes01", "mes02", "mes03", "mes04", "mes05", "mes06", "mes07", "mes08", "mes09", "mes10", "mes11", "mes12"}
        ft.EditarColumnasEnDataGridView(dgCuotas, aEditar)

    End Sub


    Private Sub cmbUnidadCuota_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbUnidadCuota.SelectedIndexChanged
        If cmbUnidadCuota.Items.Count > 0 Then IniciarCuotas(txtCodigo.Text, cmbUnidadCuota.SelectedIndex)
    End Sub

    Private Sub dgCuotas_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dgCuotas.CancelRowEdit
        ft.mensajeAdvertencia("Cancelando")
    End Sub
    Private Sub dgCuotas_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgCuotas.CellValidated
        Select Case dgCuotas.CurrentCell.ColumnIndex
            Case 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14

                If Not String.IsNullOrEmpty(dgCuotas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then

                    Dim Mes As String = "ESMES" & Format(dgCuotas.CurrentCell.ColumnIndex - 2, "00")
                    Dim nValorCuotas As Double = 0.0
                    Dim CodigoMercancia As String = CStr(dgCuotas.CurrentRow.Cells(0).Value)
                    Select Case cmbUnidadCuota.SelectedIndex
                        Case 0 'KILOGRAMOS
                            nValorCuotas = CDbl(dgCuotas.CurrentCell.Value)
                        Case 1 'UNIDAD VENTA
                            nValorCuotas = CDbl(dgCuotas.CurrentCell.Value) * ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Case 2 'UNIDAD MEDIDA SECUNDARIA
                            nValorCuotas = CDbl(dgCuotas.CurrentCell.Value) / Equivalencia(myConn,  CodigoMercancia, ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")) * ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    End Select
                    If ft.DevuelveScalarEntero(myConn, " SELECT count(*) from jsvencuoart where codart = '" & txtCodigo.Text & "' and codven = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                        ft.Ejecutar_strSQL(myConn, " insert into jsvencuoart values('" & CodigoMercancia & "','" & txtCodigo.Text & "', 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,0,'" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "' ) ")
                    End If
                    ft.Ejecutar_strSQL(myconn, " update jsvencuoart set " & Mes & " = " & nValorCuotas & " " _
                                            & " where codven = '" & CodigoMercancia & "' and codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                End If

        End Select

    End Sub

    Private Sub dgCuotas_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dgCuotas.CellValidating

        Dim headerText As String = _
            dgCuotas.Columns(e.ColumnIndex).HeaderText

        If Mid(headerText, 1, 5) = "ESMES" Then Return

        If Not String.IsNullOrEmpty(dgCuotas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
            If e.ColumnIndex >= 3 And e.ColumnIndex <= 14 Then

                If Not ft.isNumeric(e.FormattedValue.ToString()) Then
                    ft.mensajeAdvertencia("DEBE INDICAR UNA CANTIDAD VALIDA ...")
                    e.Cancel = True
                End If

            End If
        End If

    End Sub

    Private Sub chkCuotaFija_Click(sender As Object, e As System.EventArgs) Handles chkCuotaFija.Click

        If chkCuotaFija.Checked Then
            If ft.Pregunta("Al seleccionar cuota fija se limitará la venta de esta mercancía a la cuota asignada a cada vendedor." + vbLf + _
                           "Esta conforme con que esto suceda...") = Windows.Forms.DialogResult.No Then
                chkCuotaFija.Checked = Not chkCuotaFija.Checked
                Return
            End If

        Else
            If ft.Pregunta("Al deseleccionar cuota fija se podrá vender este producto sin limitaciones de cuota para cada vendedor." + vbLf + _
                           "Esta seguro(a) que desea que esto sea así...") = Windows.Forms.DialogResult.No Then
                chkCuotaFija.Checked = Not chkCuotaFija.Checked
                Return
            End If

        End If

        ft.Ejecutar_strSQL(myConn, " update jsmerctainv set cuotafija = " & IIf(chkCuotaFija.Checked, 1, 0) & " " _
                       & " where " _
                       & " codart = '" & txtCodigo.Text & "' and " _
                       & " id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub dgCuotas_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgCuotas.ColumnHeaderMouseClick
        If e.ColumnIndex >= 3 And e.ColumnIndex <= 14 Then
            If ft.Pregunta("Desea colocar cuotas a cero ", "CUOTAS ") = Windows.Forms.DialogResult.Yes Then
                Dim nMes As String = ft.RellenaConCaracter(e.ColumnIndex - 2, 2, "0", Transportables.lado.izquierdo)
                ft.Ejecutar_strSQL(myConn, " update jsvencuoart set REMES" & nMes & " = ESMES" & nMes & " where codart = '" & txtCodigoCuotas.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " update jsvencuoart set ESMES" & nMes & " = 0.000 where codart = '" & txtCodigoCuotas.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                IniciarCuotas(txtCodigoCuotas.Text, cmbUnidadCuota.SelectedIndex)
            End If
        End If
    End Sub

End Class