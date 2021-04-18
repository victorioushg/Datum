Imports MySql.Data.MySqlClient
Public Class jsVenProAnulacionDocumentos
    Private Const sModulo As String = "Anulación de Documentos Fiscales"

    Private Const nTabla As String = "tblREnglonesDocumento"

    Private strSQL As String = ""

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private aTipoDocumento() As String = {"Factura", "Nota Crédito", "Nota Débito", "Factura Punto de Venta"}

    Dim Fecha As Date = DateAdd("m", -6, jytsistema.sFechadeTrabajo)
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Este proceso anula documentos (FC,NC,ND). Elimina los renglones del documento deseado colocando los " + vbCr + _
                          "totales a cero, reestablece las mercancías al inventario y elimina la CXC en el cliente respectivo" + vbCr + _
                          " "

        ft.habilitarObjetos(True, True, cmbTipo, btnDocumento)
        ft.habilitarObjetos(False, True, txtDocumento, txtCodigo, txtCliente, txtEmision, txtTotal, txtItems, txtPeso, txtEstatus)

        ft.RellenaCombo(aTipoDocumento, cmbTipo)

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.TipoMensaje.iAyuda)

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        dt = Nothing
        ds = Nothing
        Me.Close()
    End Sub
    Private Sub jsVenProAnulacionDocumentos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub

    Private Sub jsVenProAnulacionDocumentos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If validado Then Procesar()
    End Sub

    Private Function Validado()

        If FechaUltimoBloqueo(myConn, "jsvenencfac") >= jytsistema.sFechadeTrabajo Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtDocumento.Text = "" Then
            ft.mensajeCritico(" Debe indicar un documento Válido...")
            Return False
        End If

        Return True

    End Function

    Private Sub Procesar()

        DeshabilitarCursorEnEspera()
        pb.Value = 0
        lblProgreso.Text = ""


        Select Case cmbTipo.SelectedIndex
            Case 0
                '1. Elimina Renglones txtDocumento.text
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 20, "Eliminando renglones documento...")

                ds = DataSetRequery(ds, " select * from jsvenrenfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)

                ft.Ejecutar_strSQL(myConn, "  UPDATE jsvenenccot a, jsvenrenfac b SET b.estatus = 0 " _
                               & " WHERE " _
                               & " a.numcot = b.numcot  AND " _
                               & " a.id_emp = b.id_emp AND " _
                               & " b.numfac = '" & txtDocumento.Text & "' AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(myconn, " UPDATE jsvenrencot a, jsvenrenfac b SET a.cantran = a.cantidad, b.numcot = '', b.rencot = '' " _
                                & " WHERE " _
                                & " a.numcot = b.numcot  AND " _
                                & " a.renglon = b.renglon AND " _
                                & " a.id_emp = b.id_emp AND " _
                                & " b.numfac = '" & txtDocumento.Text & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "'")

                ft.Ejecutar_strSQL(myconn, "  UPDATE jsvenencped a, jsvenrenfac b SET a.estatus = 0 " _
                               & " WHERE " _
                               & " a.numped = b.numped  AND " _
                               & " a.id_emp = b.id_emp AND " _
                               & " b.numfac = '" & txtDocumento.Text & "' AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(myconn, " UPDATE jsvenrenped a, jsvenrenfac b SET a.cantran = a.cantidad, b.numped = '', b.renped = '' " _
                                & " WHERE " _
                                & " a.numped = b.numped  AND " _
                                & " a.renglon = b.renglon AND " _
                                & " a.id_emp = b.id_emp AND " _
                                & " b.numfac = '" & txtDocumento.Text & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "'")

                ft.Ejecutar_strSQL(myconn, " delete from jsvenrenfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsvendesfac where numfac = '" & txtDocumento.text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsvenivafac where numfac = '" & txtDocumento.text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                '
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 40, "2. Elimina salidas en el inventario")
                ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where numdoc = '" & txtDocumento.text & "' and tipomov = 'SA' and origen = 'FAC' and numorg = '" & txtDocumento.text & "' and id_emp = '" & jytsistema.WorkID & "' ")


                '3. Elimina la cuenta por cobrar
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 60, "Eliminando CXC...")
                ft.Ejecutar_strSQL(myconn, " delete from jsventracob where nummov = '" & txtDocumento.text & "' and numorg = '" & txtDocumento.text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                '4. Coloca la txtDocumento.text con estatus de anulada
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 80, "Cambiando estatus documento...")
                ft.Ejecutar_strSQL(myconn, " UPDATE jsvenencfac set estatus = 2, comen = 'FACTURA ANULADA', " _
                & " tot_fac = 0.00, cargos = 0.00, descuen = 0.00, imp_iva = 0.00, tot_net = 0.00, " _
                & " items = 0, kilos = 0.000 where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                '5. Activa presupuestos y Pedidos
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 100, "Activando presupuestos y pedidos")

                For Each dtRow As DataRow In dt.Rows
                    With dtRow
                        If .Item("item").ToString.Substring(0, 1) <> "$" Then _
                                               ActualizarExistenciasPlus(myConn, .Item("item"))
                    End With
                Next

                ft.mensajeInformativo(cmbTipo.Text & "  " & txtDocumento.Text & " ANULADA CON EXITO...")
                InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, txtDocumento.Text)
            Case 1
                '1. Elimina Renglones NC
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 25, "Eliminando renglones Nota Crédito...")

                ds = DataSetRequery(ds, " select * from jsvenrenncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)

                ft.Ejecutar_strSQL(myconn, " delete from jsvenrenncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsvenivancr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                '2. Elimina en el inventarioç
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 50, "Eliminando movimientos inventario...")
                ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where numdoc = '" & txtDocumento.Text & "'  and origen = 'NCV' and id_emp = '" & jytsistema.WorkID & "' ")

                '3. Elimina la cuenta por cobrar
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 75, "Eliminando CxC...")
                ft.Ejecutar_strSQL(myconn, " delete from jsventracob where nummov = '" & txtDocumento.Text & "' and origen = 'NCR' and id_emp = '" & jytsistema.WorkID & "' ")

                '4. Coloca la txtDocumento.text con estatus de anulada
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 100, "Cambiando estatus documento...")
                ft.Ejecutar_strSQL(myconn, " UPDATE jsvenencncr set estatus = 2, comen = 'NOTA CREDITO ANULADA', " _
                & " tot_net = 0.00, imp_iva = 0.00, tot_ncr = 0.00, " _
                & " items = 0, kilos = 0.000 where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                For Each dtRow As DataRow In dt.Rows
                    With dtRow
                        If .Item("item").ToString.Substring(0, 1) <> "$" Then _
                                               ActualizarExistenciasPlus(myConn, .Item("item"))
                    End With
                Next

                ft.mensajeInformativo(cmbTipo.Text & "  " & txtDocumento.Text & " ANULADA CON EXITO...")
                InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, txtDocumento.Text)
            Case 2
                '1. Elimina Renglones ND
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 20, "Eliminando renglones Nota Débito...")

                ds = DataSetRequery(ds, " select * from jsvenrenndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)

                ft.Ejecutar_strSQL(myconn, " delete from jsvenrenndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsvenivandb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                '2. Elimina salidas en el inventarioç
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 40, "Eliminando Salidas de inventario...")
                ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where numdoc = '" & txtDocumento.Text & "'and origen = 'NDV' and numorg = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                '3. Elimina la cuenta por cobrar
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 60, "Eliminando CxC...")
                ft.Ejecutar_strSQL(myconn, " delete from jsventracob where nummov = '" & txtDocumento.Text & "' and origen in ('NDB','BAN') and id_emp = '" & jytsistema.WorkID & "' ")


                '4. Elimina Movimientos Generados en Banco
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 80, "Eliminando movimiento en el Banco...")
                Dim numChequeDevuelto As String = ft.DevuelveScalarCadena(myConn, " select refer from jsvenencndb where numndb = '" & txtDocumento.Text _
                                                                        & "' and id_emp = '" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(myconn, " delete from jsventracob where refer = '" & numChequeDevuelto & "' and origen in ('NDB','BAN') and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsbantraban where NUMDOC = '" & numChequeDevuelto & "' AND tipomov = 'ND' and concepto = 'CHEQUE DEVUELTO' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsbanchedev where prov_cli = '" & txtCodigo.Text & "' and numcheque = '" & numChequeDevuelto & "' and id_emp = '" & jytsistema.WorkID & "' ")

                '5. Coloca la txtDocumento.text con estatus de anulada
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 100, "Cambiando estatus de documento...")
                ft.Ejecutar_strSQL(myconn, " UPDATE jsvenencndb set estatus = 2, comen = 'NOTA DEBITO ANULADA', " _
                & " tot_ndb = 0.00, imp_iva = 0.00, tot_net = 0.00, " _
                & " items = 0, kilos = 0.000 where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                For Each dtRow As DataRow In dt.Rows
                    With dtRow
                        If .Item("item").ToString.Substring(0, 1) <> "$" Then _
                                               ActualizarExistenciasPlus(myConn, .Item("item"))
                    End With
                Next

                ft.mensajeInformativo(cmbTipo.Text & "  " & txtDocumento.Text & " ANULADA CON EXITO...")
                InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, txtDocumento.Text)

            Case 3
                '1. Elimina Renglones txtDocumento.text
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 25, "Eliminando renglones documento...")

                ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)

                ft.Ejecutar_strSQL(myconn, " delete from jsvenrenpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsvendespos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsvenivapos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsvenforpag where numfac = '" & txtDocumento.Text & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ")

                '2. Elimina salidas en el inventario
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 50, "Eliminando salidas de inventario")
                ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where numdoc = '" & txtDocumento.Text & "' and tipomov = 'SA' and origen = 'PVE' and numorg = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                '3. Elimina la cuenta por cobrar
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 75, "Eliminando cuentas por cobrar...")
                ft.Ejecutar_strSQL(myconn, " delete from jsventrapos where nummov = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsbantracaj where nummov = '" & txtDocumento.Text & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myconn, " delete from jsbantraban where numdoc = '" & txtDocumento.Text & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ")

                '4. Coloca la txtDocumento.text con estatus de anulada
                refrescaBarraprogresoEtiqueta(pb, lblProgreso, 100, "Cambiando estatus documento...")
                ft.Ejecutar_strSQL(myconn, " UPDATE jsvenencpos set estatus = 2, comen = 'FACTURA ANULADA', " _
                & " tot_fac = 0.00, cargos = 0.00, descuen = 0.00, imp_iva = 0.00, tot_net = 0.00, " _
                & " items = 0, kilos = 0.000 where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                For Each dtRow As DataRow In dt.Rows
                    With dtRow
                        If .Item("item").ToString.Substring(0, 1) <> "$" Then _
                                               ActualizarExistenciasPlus(myConn, .Item("item"))
                    End With
                Next

                ft.mensajeInformativo(cmbTipo.Text & "  " & txtDocumento.Text & " ANULADA CON EXITO...")
                InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, txtDocumento.Text)

        End Select

        SaldoCxC(myConn, lblInfo, txtCodigo.Text)

        HabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")


    End Sub

    Private Sub btnDocumento_Click(sender As System.Object, e As System.EventArgs) Handles btnDocumento.Click
        Select Case cmbTipo.SelectedIndex
            Case 0

                txtDocumento.Text = CargarTablaSimple(myConn, lblInfo, ds, " select a.numfac codigo, concat(a.codcli, ' - ', b.nombre) descripcion, " _
                                                      & " a.emision, a.tot_fac TOTAL , a.items,  a.estatus " _
                                                      & " from jsvenencfac a " _
                                                      & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                                                      & " where " _
                                                      & " a.emision >= '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numfac desc ", _
                                                      "LISTADO DE FACTURAS", _
                                                      txtDocumento.Text)

            Case 1

                txtDocumento.Text = CargarTablaSimple(myConn, lblInfo, ds, " select a.numncr codigo, concat(a.codcli, ' - ', b.nombre) descripcion, " _
                                                      & " a.emision, a.tot_ncr TOTAL, a.items, a.estatus " _
                                                      & " from jsvenencncr a " _
                                                      & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
                                                      & " where " _
                                                      & " a.emision >= '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numncr desc ", _
                                                      "LISTADO DE NOTAS DE CREDITO", _
                                                      txtDocumento.Text)
            Case 2

                txtDocumento.Text = CargarTablaSimple(myConn, lblInfo, ds, " select a.numndb codigo, concat(a.codcli, ' - ', b.nombre) descripcion, " _
                                                      & " a.emision, a.tot_ndb TOTAL, a.items, a.estatus " _
                                                      & " from jsvenencndb a " _
                                                      & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
                                                      & " where " _
                                                      & " a.emision >= '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numndb desc ", _
                                                      "LISTADO DE NOTAS DEBITO", _
                                                      txtDocumento.Text)

            Case 3

                txtDocumento.Text = CargarTablaSimple(myConn, lblInfo, ds, " select a.numfac codigo, concat(a.codcli, ' - ', b.nombre) descripcion, " _
                                                      & " a.emision, a.tot_fac TOTAL, a.items, a.estatus " _
                                                      & " from jsvenencpos a " _
                                                      & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
                                                      & " where " _
                                                      & " a.emision >= '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numfac desc ", _
                                                      "LISTADO DE FACTURAS PUNTO DE VENTA", _
                                                     txtDocumento.Text)
        End Select

    End Sub

    Private Sub txtDocumento_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDocumento.TextChanged

        Dim aEstatus() As String = {"NO CONFORMADA", "CONFORMADA", "ANULADA"}

        If txtDocumento.Text <> "" Then
            Select Case cmbTipo.SelectedIndex
                Case 0
                    txtCodigo.Text = ft.DevuelveScalarCadena(myConn, " SELECT codcli from jsvenencfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtCliente.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsvencatcli where codcli = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtEmision.Text = ft.muestraCampoFecha(ft.DevuelveScalarFecha(myConn, " SELECT emision from jsvenencfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtTotal.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " SELECT tot_fac from jsvenencfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtPeso.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(myConn, " SELECT kilos from jsvenencfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtItems.Text = ft.muestraCampoEntero(ft.DevuelveScalarEntero(myConn, " SELECT items from jsvenencfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtEstatus.Text = aEstatus(ft.DevuelveScalarEntero(myConn, " SELECT estatus from jsvenencfac where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Case 1
                    txtCodigo.Text = ft.DevuelveScalarCadena(myConn, " SELECT codcli from jsvenencncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtCliente.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsvencatcli where codcli = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtEmision.Text = ft.muestraCampoFecha(ft.DevuelveScalarFecha(myConn, " SELECT emision from jsvenencncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtTotal.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " SELECT tot_ncr from jsvenencncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtPeso.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(myConn, " SELECT kilos from jsvenencncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtItems.Text = ft.muestraCampoEntero(ft.DevuelveScalarEntero(myConn, " SELECT items from jsvenencncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtEstatus.Text = aEstatus(ft.DevuelveScalarEntero(myConn, " SELECT estatus from jsvenencncr where numncr = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Case 2
                    txtCodigo.Text = ft.DevuelveScalarCadena(myConn, " SELECT codcli from jsvenencndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtCliente.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsvencatcli where codcli = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtEmision.Text = ft.muestraCampoFecha(ft.DevuelveScalarFecha(myConn, " SELECT emision from jsvenencndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtTotal.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " SELECT tot_ndb from jsvenencndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtPeso.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(myConn, " SELECT kilos from jsvenencndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtItems.Text = ft.muestraCampoEntero(ft.DevuelveScalarEntero(myConn, " SELECT items from jsvenencndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtEstatus.Text = aEstatus(ft.DevuelveScalarEntero(myConn, " SELECT estatus from jsvenencndb where numndb = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Case 3
                    txtCodigo.Text = ft.DevuelveScalarCadena(myConn, " SELECT codcli from jsvenencpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtCliente.Text = ft.DevuelveScalarCadena(myConn, " SELECT nomcli from jsvenencpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtEmision.Text = ft.muestraCampoFecha(ft.DevuelveScalarFecha(myConn, " SELECT emision from jsvenencpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtTotal.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " SELECT tot_fac from jsvenencpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtPeso.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(myConn, " SELECT kilos from jsvenencpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtItems.Text = ft.muestraCampoEntero(ft.DevuelveScalarEntero(myConn, " SELECT items from jsvenencpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtEstatus.Text = aEstatus(ft.DevuelveScalarEntero(myConn, " SELECT estatus from jsvenencpos where numfac = '" & txtDocumento.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
            End Select

        End If
    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged

    End Sub
End Class