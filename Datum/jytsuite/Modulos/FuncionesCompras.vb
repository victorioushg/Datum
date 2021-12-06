Imports MySql.Data.MySqlClient
Module FuncionesCompras
    Private ft As New Transportables
    Function SaldoCxP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoProveedor As String) As Double

        SaldoCxP = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) from jsprotrapag " _
                & " where " _
                & " REMESA = '' AND " _
                & " CODPRO = '" & CodigoProveedor & "' " _
                & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                & " and ID_EMP = '" & jytsistema.WorkID & "' group by codpro ")

        ft.Ejecutar_strSQL(myconn, " UPDATE jsprocatpro SET " _
            & " DISPONIBLE = LIMCREDITO + " & SaldoCxP & ", " _
            & " SALDO = " & SaldoCxP _
            & " where CODPRO = '" & CodigoProveedor & "' " _
            & " and ID_EMP = '" & jytsistema.WorkID & "'")

    End Function
    Function SaldoExP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoProveedor As String) As Double

        SaldoExP = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) from jsprotrapag " _
                & " where " _
                & " REMESA = '1' AND " _
                & " CODPRO = '" & CodigoProveedor & "' " _
                & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                & " and ID_EMP = '" & jytsistema.WorkID & "' group by codpro ")

        'ft.Ejecutar_strSQL ( myconn, " UPDATE jsprocatpro SET " _
        '    & " DISPONIBLE = LIMCREDITO + " & SaldoCxPExP & ", " _
        '    & " SALDO = " & SaldoCxPExP _
        '    & " where CODPRO = '" & CodigoProveedor & "' " _
        '    & " and ID_EMP = '" & jytsistema.WorkID & "'")

    End Function
    Public Function DisponibilidadProveedor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoProveedor As String) As Double
        Dim limCredito As String = Math.Abs(ft.DevuelveScalarDoble(MyConn, " select limcredito from jsprocatpro where codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'"))

    End Function
    Public Sub ActualizarRenglonesEnOrdenesDeCompra(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal dtRenglon As DataTable, _
                                                 ByVal nombreTablaOrigen As String)

        Dim NumeroOrdenDeCompra As String
        Dim sItem As String
        Dim RenglonOrdenDeCompra As String
        Dim gCont As Integer

        For gCont = 0 To dtRenglon.Rows.Count - 1
            With dtRenglon.Rows(gCont)
                NumeroOrdenDeCompra = IIf(Not IsDBNull(.Item("numord")), .Item("numord"), "")
                RenglonOrdenDeCompra = IIf(Not IsDBNull(.Item("renord")), .Item("renord"), "")
                sItem = .Item("item")
                If NumeroOrdenDeCompra <> "" Then
                    Dim CantidadReal As Double = ft.DevuelveScalarEntero(MyConn, " select sum(a.cantidad/b.equivale) " _
                                                                         & " from  " & nombreTablaOrigen & " a " _
                                                                         & " left join (" & SeleccionGENTablaEquivalencias() & ") b on (a.item = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp)  " _
                                                                         & " where " _
                                                                         & " a.item = '" & sItem & "' and " _
                                                                         & " a.numord = '" & NumeroOrdenDeCompra & "' AND " _
                                                                         & " a.id_emp = '" & jytsistema.WorkID & "' ")

                    If .Item("ESTATUS") < 2 Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsprorenord", "numord", NumeroOrdenDeCompra, _
                                                           RenglonOrdenDeCompra, sItem, CantidadReal, _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsproencord", "jsprorenord", "NUMORD", NumeroOrdenDeCompra)
                    End If
                End If
            End With
        Next

    End Sub

    Public Sub ActualizarRenglonesEnRecepciones(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal dtRenglon As DataTable, _
                                                 ByVal nombreTablaOrigen As String)

        Dim NumeroRecepcion As String
        Dim sItem As String
        Dim RenglonRecepcion As String
        Dim gCont As Integer

        For gCont = 0 To dtRenglon.Rows.Count - 1
            With dtRenglon.Rows(gCont)
                NumeroRecepcion = IIf(Not IsDBNull(.Item("numrec")), .Item("numrec"), "")
                RenglonRecepcion = IIf(Not IsDBNull(.Item("renrec")), .Item("renrec"), "")
                sItem = .Item("item")
                If NumeroRecepcion <> "" Then
                    If .Item("ESTATUS") < 2 Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsprorenrep", "numrec", NumeroRecepcion, _
                                                           RenglonRecepcion, sItem, .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsproencrep", "jsprorenrep", "NUMREC", NumeroRecepcion)
                    End If
                End If
            End With
        Next

    End Sub

    Public Function CalculaTotalIVACompras(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                                ByVal CodigoProveedor As String, _
                                                ByVal nomTablaDescuentos As String, _
                                                ByVal nomTablaIVA As String, ByVal nomTablaRenglones As String, _
                                                ByVal nomCampoClave As String, ByVal NumeroDocumento As String, _
                                                ByVal nomCampoImporteIVA As String, ByVal nomCampoImporteRenglon As String, _
                                                ByVal FechaIVA As Date, Optional ByVal nombreCampoTotalRenglones As String = "totren") As Double

        Dim numRetentencion As String = ""
        Dim fechRetencion As String = ""
        Dim montoRetencion As Double = 0.0

        If DocumentoPoseeRetencionIVA(MyConn, lblInfo, NumeroDocumento, CodigoProveedor) Then

            numRetentencion = ft.DevuelveScalarCadena(MyConn, " select refer from jsprotrapag where " _
                                      & " SUBSTRING(concepto,1,13) = 'RETENCION IVA' AND " _
                                      & " TIPOMOV IN ('NC','ND') AND " _
                                      & " CODPRO = '" & CodigoProveedor & "' AND " _
                                      & " NUMMOV = '" & NumeroDocumento & "' AND " _
                                      & " ID_EMP = '" & jytsistema.WorkID & "' ")
            fechRetencion = ft.DevuelveScalarFecha(MyConn, " select emision from jsprotrapag where " _
                                      & " SUBSTRING(concepto,1,13) = 'RETENCION IVA' AND " _
                                      & " TIPOMOV IN ('NC','ND') AND " _
                                      & " CODPRO = '" & CodigoProveedor & "' AND " _
                                      & " NUMMOV = '" & NumeroDocumento & "' AND " _
                                      & " ID_EMP = '" & jytsistema.WorkID & "' ").ToString

            montoRetencion = Math.Abs(ft.DevuelveScalarDoble(MyConn, " select importe from jsprotrapag where " _
                                      & " SUBSTRING(concepto,1,13) = 'RETENCION IVA' AND " _
                                      & " TIPOMOV IN ('NC','ND') AND " _
                                      & " CODPRO = '" & CodigoProveedor & "' AND " _
                                      & " NUMMOV = '" & NumeroDocumento & "' AND " _
                                      & " ID_EMP = '" & jytsistema.WorkID & "' "))

        Else
            If InStr("jsvenrengas.jsvenrencom", nomTablaRenglones, ) > 0 Then

                numRetentencion = ft.DevuelveScalarCadena(MyConn, " select num_ret_iva from " & nomTablaRenglones.Replace("ren", "enc") & " " _
                                                                 & " where  " _
                                                                 & " " & nomCampoClave & " = '" & NumeroDocumento & "' and   " _
                                                                 & " codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                fechRetencion = ft.DevuelveScalarFecha(MyConn, " select fecha_ret_iva from " & nomTablaRenglones.Replace("ren", "enc") & " " _
                                                                 & " where  " _
                                                                 & " " & nomCampoClave & " = '" & NumeroDocumento & "' and   " _
                                                                 & " codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ").ToString

                montoRetencion = ft.DevuelveScalarDoble(MyConn, " select ret_iva from " & nomTablaRenglones.Replace("ren", "enc") & " " _
                                                                 & " where  " _
                                                                 & " " & nomCampoClave & " = '" & NumeroDocumento & "' and   " _
                                                                 & " codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")
            End If
        End If

        ft.Ejecutar_strSQL(myconn, " delete from " & nomTablaIVA & " where " & nomCampoClave & " = '" & NumeroDocumento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        If ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, nomTablaIVA, "retencion") Then

            ft.Ejecutar_strSQL(MyConn, " insert into " & nomTablaIVA _
                    & " SELECT a." & nomCampoClave & ", a.codpro, a.iva, if( b.monto is null, 0.00, b.monto) , SUM(a." & nombreCampoTotalRenglones & "des) baseiva, " _
                    & " ROUND(SUM(a." & nombreCampoTotalRenglones & "des*if( b.monto is null, 0.00, b.monto)/100),2) impiva, 0.00, '',  '' ejercicio, a.id_emp " _
                    & " FROM " & nomTablaRenglones & " a " _
                    & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ") b ON (a.iva = b.tipo) " _
                    & " Where " _
                    & " a." & nomCampoClave & " = '" & NumeroDocumento & "' AND " _
                    & " a.codpro = '" & CodigoProveedor & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " GROUP BY a.iva ")

        Else
            ft.Ejecutar_strSQL(MyConn, " insert into " & nomTablaIVA _
                   & " SELECT a." & nomCampoClave & ", a.codpro, a.iva, if( b.monto is null, 0.00, b.monto) , SUM(a." & nombreCampoTotalRenglones & "des) baseiva, " _
                   & " ROUND(SUM(a." & nombreCampoTotalRenglones & "des*if( b.monto is null, 0.00, b.monto)/100),2) impiva, '' ejercicio, a.id_emp " _
                   & " FROM " & nomTablaRenglones & " a " _
                   & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ") b ON (a.iva = b.tipo) " _
                   & " Where " _
                   & " a." & nomCampoClave & " = '" & NumeroDocumento & "' AND " _
                   & " a.codpro = '" & CodigoProveedor & "' and " _
                   & " a.id_emp = '" & jytsistema.WorkID & "' " _
                   & " GROUP BY a.iva ")
        End If


        ft.Ejecutar_strSQL(MyConn, " update " & nomTablaIVA & " set retencion = " & montoRetencion & ", numretencion = '" & numRetentencion & "' " _
                                & " WHERE " _
                                & " " & nomCampoClave & " = '" & NumeroDocumento & "' AND " _
                                & " codpro = '" & CodigoProveedor & "' and " _
                                & " id_emp = '" & jytsistema.WorkID & "' ")


        CalculaTotalIVACompras = ft.DevuelveScalarDoble(MyConn, " select  sum(" & nomCampoImporteIVA & ") from " _
                                                            & nomTablaIVA _
                                                            & " where " & _
                                                            nomCampoClave & " = '" & NumeroDocumento & "' and " _
                                                            & " codpro = '" & CodigoProveedor & "' and " _
                                                            & " id_emp = '" & jytsistema.WorkID & "'  ")


    End Function
    Public Function DocumentoCXPPoseeCancelacionesAbonos(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, _
                                                         ByVal NumeroDocumento As String, CodigoProveedor As String)
        Dim nCancela As String = ft.DevuelveScalarCadena(Myconn, " select comproba from jsprotrapag " _
                                                       & " where " _
                                                       & " codpro = '" & CodigoProveedor & "' and " _
                                                       & " nummov = '" & NumeroDocumento & "' AND " _
                                                       & " tipomov in ('AB', 'CA', 'NC', 'ND' ) AND " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' ")


        DocumentoCXPPoseeCancelacionesAbonos = False
        If nCancela <> "0" Then DocumentoCXPPoseeCancelacionesAbonos = True
    End Function

    Function DocumentoPoseeRetencionIVA(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String, ByVal CodigoProveedor As String) As Boolean


        If ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsprotrapag where " _
                                      & " SUBSTRING(concepto,1,13) = 'RETENCION IVA' AND " _
                                      & " TIPOMOV IN ('NC','ND') AND " _
                                      & " CODPRO = '" & CodigoProveedor & "' AND " _
                                      & " NUMMOV = '" & NumeroDocumento & "' AND " _
                                      & " ID_EMP = '" & jytsistema.WorkID & "' ") = 0 Then

            Return False
        Else
            Return True
        End If


    End Function

    Public Sub CargarListaDesdeCompras(ByVal dg As DataGridView, ByVal dt As DataTable)

        Dim aFld() As String = {"recibido", "emision", "numcom", "codpro", "nombre", "emisioniva", "fechasi"}
        Dim aNom() As String = {"", "Emisión", "N° Documento", "Código Proveedor", "Nombre Proveedor", "Fecha IVA", "Fecha Asiento"}
        Dim aAnc() As Integer = {20, 90, 120, 120, 350, 90, 90}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                     AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", sFormatoFechaCorta, "", "", "", sFormatoFechaCorta, sFormatoFechaCorta}


        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor)

    End Sub

    ''//// Listas de Datos 

    Public Function GetVendorList(MyConn As MySqlConnection) As List(Of Vendor)

        Dim strSQL = " select p.codpro Codigo, p.nombre, p.RIF, p.Zona, p.formapago FormaDePago  " &
            "  " &
            "  " &
            " from jsprocatpro p " &
            " where " &
            " p.id_emp='" & jytsistema.WorkID & "' order by p.nombre "

        Return Lista(Of Vendor)(MyConn, strSQL)

    End Function
    Public Function GetVendorZonesList(MyConn As MySqlConnection) As List(Of SimpleTable)

        Dim strSQL = " select codigo, descrip descripcion from jsconctatab where modulo = '" &
            FormatoTablaSimple(Modulo.iZonaProveedor) & "' and id_emp = '" & jytsistema.WorkID & "'  order by 1 "
        Return Lista(Of SimpleTable)(MyConn, strSQL)


    End Function

    Public Function GetVendorBalance(MyConn As MySqlConnection, CodigoProveedor As String, Remesa As String) As List(Of VendorTransaction)
        Dim strSQL As String = " SELECT a.codpro CodigoProveedor, a.nummov NumeroMovimiento, a.tipomov TipoMovimiento, " _
                                 & " a.refer Referencia, a.emision, a.vence Vencimiento, a.importe Importe, c.saldo,  " _
                                 & " IF(a.Currency = 0, " & jytsistema.WorkCurrency.Id & ", a.Currency) Currency " _
                                 & " FROM jsprotrapag a " _
                                 & " LEFT JOIN (SELECT a.codpro, a.nummov, a.tipomov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
                                 & "            FROM jsprotrapag a " _
                                 & "            WHERE " _
                                 & "            a.REMESA = '" & Remesa & "' AND " _
                                 & "            a.codpro = '" & CodigoProveedor & "' AND " _
                                 & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                 & "            GROUP BY a.nummov) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                 & " WHERE " _
                                 & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                                 & " a.codpro = '" & CodigoProveedor & "'  AND " _
                                 & " a.codpro <> '' AND " _
                                 & " (c.saldo > 0.001 OR c.saldo < -0.001) AND " _
                                 & " a.historico = '0' AND " _
                                 & " a.REMESA = '" & Remesa & "' AND " _
                                 & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                 & " ORDER BY a.nummov, a.emision "

        Return Lista(Of VendorTransaction)(MyConn, strSQL)

    End Function
    Public Function GetVendorBalanceISLR(MyConn As MySqlConnection, CodigoProveedor As String, Remesa As String) As List(Of VendorTransaction)
        Dim strSQL As String = " Select a.codpro CodigoProveedor, a.nummov NumeroMovimiento, a.tipomov TipoMovimiento, " _
                        & " c.num_control NumeroControl, a.emision, a.vence Vencimiento, a.importe, c.saldo " _
                        & " FROM jsprotrapag a " _
                        & " LEFT JOIN (Select a.codpro, a.numcom nummov, c.num_control, SUM(a.costototdes) saldo, a.id_emp " _
                        & " FROM jsprorencom a " _
                        & " LEFT JOIN jsproenccom b ON (a.numcom = b.numcom And a.codpro = b.codpro And a.id_emp = b.id_emp) " _
                        & " LEFT JOIN jsconnumcon c ON (a.numcom = c.numdoc And a.codpro = c.prov_cli And b.emisioniva = c.emision And c.org = 'COM' AND origen = 'COM' AND a.id_emp = c.id_emp)" _
                        & " LEFT JOIN jsmercatser d on (a.item = concat('$', d.codser) and d.tipo = '0' and a.id_emp = d.id_emp) " _
                        & " WHERE " _
                        & " d.tiposervicio = '0' and  d.tipo = '0' and " _
                        & " b.num_ret_islr = '' AND " _
                        & " a.codpro = '" & CodigoProveedor & "' and " _
                        & " MID(a.item,1,1) = '$' AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " GROUP BY a.codpro, a.numcom " _
                        & " UNION " _
                        & " SELECT a.codpro, a.numgas, c.num_control, SUM(a.costototdes) saldo, a.id_emp " _
                        & " FROM jsprorengas a " _
                        & " LEFT JOIN jsproencgas b ON (a.numgas = b.numgas AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                        & " LEFT JOIN jsconnumcon c ON (a.numgas = c.numdoc AND a.codpro = c.prov_cli AND b.emisioniva = c.emision AND c.org = 'GAS' AND origen = 'COM' AND a.id_emp = c.id_emp) " _
                        & " LEFT JOIN jsmercatser d on (a.item = concat('$', d.codser) and d.tipo = '0' and a.id_emp = d.id_emp) " _
                        & " WHERE " _
                        & " d.tiposervicio = '0' and d.tipo = '0' and " _
                        & " b.num_ret_islr = '' AND " _
                        & " a.codpro = '" & CodigoProveedor & "' and " _
                        & " MID(a.item,1,1) = '$' AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " GROUP BY a.codpro, a.numgas) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                        & " WHERE " _
                        & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                        & " a.codpro = '" & CodigoProveedor & "'  AND " _
                        & " a.codpro <> '' AND " _
                        & " (c.saldo > 0.001 OR c.saldo < -0.001) AND " _
                        & " a.REMESA = '" & Remesa & "' AND " _
                        & " a.historico = '0' AND " _
                        & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                        & " ORDER BY a.nummov, a.emision "
        Return Lista(Of VendorTransaction)(MyConn, strSQL)

    End Function

    Public Function GetVendorBalanceIVA(MyConn As MySqlConnection, CodigoProveedor As String, Remesa As String) As List(Of VendorTransaction)
        Dim strSQL As String = "SELECT a.codpro CodigoProveedor, a.nummov NumeroMovimiento, a.tipomov TipoMovimiento, " _
                          & " d.num_control NumeroControl, a.emision, a.vence Vencimiento, a.importe, c.impiva ImporteIVA " _
                          & " FROM jsprotrapag a " _
                          & " LEFT JOIN (SELECT a.codpro, a.numcom nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                          & "            FROM jsproivacom a " _
                          & "            WHERE " _
                          & "            a.tipoiva Not IN ('', 'E') AND " _
                          & "            a.codpro = '" & CodigoProveedor & "' AND " _
                          & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                          & "            GROUP BY a.numcom) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                          & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli and d.org = 'COM' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                          & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                          & " WHERE " _
                          & " d.num_control <> '' and " _
                          & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                          & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                          & " a.codpro = '" & CodigoProveedor & "'  AND " _
                          & " a.codpro <> '' AND " _
                          & " c.impiva <> 0.00 AND " _
                          & " e.saldo <> 0.00 AND " _
                          & " a.historico = '0' AND " _
                          & " a.REMESA = '" & Remesa & "' AND " _
                          & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                          & " UNION " _
                          & " SELECT a.codpro CodigoProveedor, a.nummov NumeroMovimiento, a.tipomov TipoMovimiento, " _
                          & " d.num_control NumeroControl, a.emision, a.vence Vencimiento, a.importe, c.impiva ImporteIVA " _
                          & " FROM jsprotrapag a " _
                          & " LEFT JOIN (SELECT a.codpro, a.numgas nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                          & "            FROM jsproivagas a " _
                          & "            WHERE " _
                          & "            a.tipoiva NOT IN ('', 'E') AND " _
                          & "            a.codpro = '" & CodigoProveedor & "' AND " _
                          & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                          & "            GROUP BY a.numgas) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                          & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli and d.org = 'GAS' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                          & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                          & " WHERE " _
                          & " d.num_control <> '' and " _
                          & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                          & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                          & " a.codpro = '" & CodigoProveedor & "'  AND " _
                          & " a.codpro <> '' AND " _
                          & " c.impiva <> 0.00 AND " _
                          & " e.saldo <> 0.00 AND " _
                          & " a.historico = '0' AND " _
                          & " a.REMESA = '" & Remesa & "' AND " _
                          & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                          & " UNION " _
                         & " SELECT a.codpro CodigoProveedor, a.nummov NumeroMovimiento, a.tipomov TipoMovimiento, " _
                          & " d.num_control NumeroControl, a.emision, a.vence Vencimiento, a.importe, c.impiva ImporteIVA " _
                          & " FROM jsprotrapag a " _
                          & " LEFT JOIN (SELECT a.codpro, a.numncr nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                          & "            FROM jsproivancr a " _
                          & "            WHERE " _
                          & "            a.tipoiva NOT IN ('', 'E') AND " _
                          & "            a.codpro = '" & CodigoProveedor & "' AND " _
                          & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                          & "            GROUP BY a.numncr) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                          & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli  and d.org = 'NCR' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                          & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                          & " WHERE " _
                          & " d.num_control <> '' and " _
                          & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                          & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                          & " a.codpro = '" & CodigoProveedor & "'  AND " _
                          & " a.codpro <> '' AND " _
                          & " c.impiva <> 0.00 AND " _
                          & " e.saldo <> 0.00 AND " _
                          & " a.REMESA = '" & Remesa & "' AND " _
                          & " a.historico = '0' AND " _
                          & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                          & " UNION " _
                          & " SELECT a.codpro CodigoProveedor, a.nummov NumeroMovimiento, a.tipomov TipoMovimiento, " _
                          & " d.num_control NumeroControl, a.emision, a.vence Vencimiento, a.importe, c.impiva ImporteIVA " _
                          & " FROM jsprotrapag a " _
                          & " LEFT JOIN (SELECT a.codpro, a.numndb nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                          & "            FROM jsproivandb a " _
                          & "            WHERE " _
                          & "            a.tipoiva NOT IN ('', 'E') AND " _
                          & "            a.codpro = '" & CodigoProveedor & "' AND " _
                          & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                          & "            GROUP BY a.numndb) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                          & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli and d.org = 'NDB' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                          & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                          & " WHERE " _
                          & " d.num_control <> '' and " _
                          & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                          & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                          & " a.codpro = '" & CodigoProveedor & "'  AND " _
                          & " a.codpro <> '' AND " _
                          & " c.impiva <> 0.00 AND " _
                          & " e.saldo <> 0.00 AND " _
                          & " a.REMESA = '" & Remesa & "' AND " _
                          & " a.historico = '0' AND " _
                          & " a.ID_EMP = '" & jytsistema.WorkID & "'" _
                          & " ORDER BY NumeroMovimiento, emision "
        Return Lista(Of VendorTransaction)(MyConn, strSQL)
    End Function

    Public Function GetCreditCauses(MyConn As MySqlConnection, Origen As String) As List(Of CreditCause)
        Dim strSQL As String = " select Codigo, Descripcion, CR Credit, Valida_documentos ValidaDocumentos, Origen, FormaPago, " _
            & " numpag NumeroPago, nompag NombrePago " _
            & " from jsconcausas_notascredito  where origen  = '" & Origen & "' order by codigo "
        Return Lista(Of CreditCause)(MyConn, strSQL)

    End Function

    Public Function GetVendorTransactions(MyConn As MySqlConnection, CodigoProveedor As String, Optional Remesa As String = "") As List(Of VendorTransaction)

        Dim strSQL As String = "select CODPRO CodigoProveedor, TIPOMOV TipoMovimiento, nummov NumeroMovimiento, " _
        & " Emision, HORA, VENCE Vencimiento, refer Referencia, Concepto, a.Importe, " _
        & " IFNULL ( a.Importe/m.Equivale , a.importe ) ImporteReal, " _
        & " IF(a.Currency = 0, " & jytsistema.WorkCurrency.Id & ", a.Currency) Currency, Currency_Date CurrencyDate, PORIVA PorcentajeIVA, FORMAPAG FormaDePago, NUMPAG NumeroDePago, " _
        & " NOMPAG NombreDePago, benefic Beneficiario, Origen, DEPOSITO NumeroDeposito, CTADEP CuentaDeposito, " _
        & " BANCODEP BancoDeposito, CAJAPAG CajaDePago, numorg NumeroOrigen, MULTICAN Multicancelacion, " _
        & " Asiento, FECHASI FechaAsiento, CODCON CodigoContable, MULTIDOC MultiDocumento, TIPDOCCAN TipoDocumentoCancelado, " _
        & " Interes, Capital, COMPROBA Comprobante, Banco, CTABANCO CuentaBanco, Remesa, CODVEN CodigoVendedor, " _
        & " CODCOB CodigoCobrador, FOTipo, Historico, Tipo, BLOCK_DATE FechaBloqueo " _
        & " From jsprotrapag a " _
        & " left join (" & SQLSelectCambiosYMonedas(jytsistema.sFechadeTrabajo) & " ) m on ( a.currency = m.moneda ) " _
        & " where " _
        & " a.remesa = '" & Remesa & "' and " _
        & " a.historico = '0' and " _
        & " a.codpro  = '" & CodigoProveedor & "' and " _
        & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " order by a.nummov, a.fotipo, a.emision, a.tipomov  "

        Return Lista(Of VendorTransaction)(MyConn, strSQL)

    End Function

    Public Function GetVendorTransactionsIVAWithholdings(MyConn As MySqlConnection, CodigoProveedor As String, PorcentajeRetencion As Double) As List(Of VendorTransaction)

        'Dim strSQL As String = " SELECT a.tipoiva, a.poriva, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, SUM(a.impiva)*" & PorcentajeRetencion / 100 & " retiva " _
        '                            & " FROM (SELECT a.tipoiva, a.poriva, -1*a.baseiva baseiva, -1*a.impiva impiva " _
        '                            & "       	FROM jsproivacom a " _
        '                            & "         WHERE " _
        '                            & "         a.numcom in ('" & strDocs & "') and " _
        '                            & "         codpro = '" & CodigoProveedor & "' AND " _
        '                            & "     	id_emp = '" & jytsistema.WorkID & "' " _
        '                            & " UNION ALL " _
        '                            & " 	  SELECT a.tipoiva, a.poriva, -1*a.baseiva baseiva, -1*a.impiva impiva " _
        '                            & "    	    FROM jsproivagas a " _
        '                            & "         WHERE " _
        '                            & "         a.numgas in ('" & strDocs & "') and " _
        '                            & "     	CODPRO = '" & CodigoProveedor & "' AND  " _
        '                            & "  	    id_emp = '" & jytsistema.WorkID & "' " _
        '                            & " UNION ALL " _
        '                            & " 	  SELECT a.tipoiva, a.poriva, a.baseiva, a.impiva " _
        '                            & "    	    FROM jsproivancr a " _
        '                            & "         WHERE " _
        '                             & "        a.numncr in ('" & strDocs & "') and " _
        '                            & "     	CODPRO = '" & CodigoProveedor & "' AND  " _
        '                            & "  	    id_emp = '" & jytsistema.WorkID & "'" _
        '                            & " UNION ALL " _
        '                            & " 	  SELECT a.tipoiva, a.poriva, -1*a.baseiva baseiva, -1*a.impiva impiva " _
        '                            & "    	    FROM jsproivandb a " _
        '                            & "         WHERE " _
        '                            & "         a.numndb in ('" & strDocs & "') and " _
        '                            & "     	CODPRO = '" & CodigoProveedor & "' AND  " _
        '                            & "  	    id_emp = '" & jytsistema.WorkID & "' ) a " _
        '                            & " GROUP BY a.tipoiva "

        'Return Lista(Of VendorTransaction)(MyConn, strSQL)

    End Function



End Module
