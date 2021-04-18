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
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                     AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", sFormatoFechaCorta, "", "", "", sFormatoFechaCorta, sFormatoFechaCorta}


        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor)




    End Sub

End Module
