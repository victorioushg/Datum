Imports MySql.Data.MySqlClient
Module FuncionesCompras
    Function SaldoCxP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoProveedor As String) As Double

        SaldoCxP = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select SUM(IMPORTE) sSaldo from jsprotrapag " _
                & " where " _
                & " REMESA = '' AND " _
                & " CODPRO = '" & CodigoProveedor & "' " _
                & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                & " and ID_EMP = '" & jytsistema.WorkID & "' group by codpro ")

        EjecutarSTRSQL(MyConn, lblInfo, " UPDATE jsprocatpro SET " _
            & " DISPONIBLE = LIMCREDITO + " & SaldoCxP & ", " _
            & " SALDO = " & SaldoCxP _
            & " where CODPRO = '" & CodigoProveedor & "' " _
            & " and ID_EMP = '" & jytsistema.WorkID & "'")

    End Function
    Function SaldoExP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoProveedor As String) As Double

        SaldoExP = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select SUM(IMPORTE) sSaldo from jsprotrapag " _
                & " where " _
                & " REMESA = '1' AND " _
                & " CODPRO = '" & CodigoProveedor & "' " _
                & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                & " and ID_EMP = '" & jytsistema.WorkID & "' group by codpro ")

        'EjecutarSTRSQL(MyConn, lblInfo, " UPDATE jsprocatpro SET " _
        '    & " DISPONIBLE = LIMCREDITO + " & SaldoCxPExP & ", " _
        '    & " SALDO = " & SaldoCxPExP _
        '    & " where CODPRO = '" & CodigoProveedor & "' " _
        '    & " and ID_EMP = '" & jytsistema.WorkID & "'")

    End Function
    Public Function DisponibilidadProveedor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoProveedor As String) As Double
        Dim limCredito As String = Math.Abs(CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select limcredito from jsprocatpro where codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'")))

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
                    If .Item("ESTATUS") < 2 Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsprorenord", "numord", NumeroOrdenDeCompra, _
                                                           RenglonOrdenDeCompra, sItem, .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsproencord", "jsprorenord", "NUMORD", NumeroOrdenDeCompra)
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

        If Not DocumentoPoseeRetencionIVA(MyConn, lblInfo, NumeroDocumento, CodigoProveedor) Then

            EjecutarSTRSQL(MyConn, lblInfo, " delete from " & nomTablaIVA & " where " & nomCampoClave & " = '" & NumeroDocumento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

            If ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, nomTablaIVA, "retencion") Then

                EjecutarSTRSQL(MyConn, lblInfo, " insert into " & nomTablaIVA _
                        & " SELECT a." & nomCampoClave & ", a.codpro, a.iva, if( b.monto is null, 0.00, b.monto) , SUM(a." & nombreCampoTotalRenglones & "des) baseiva, " _
                        & " ROUND(SUM(a." & nombreCampoTotalRenglones & "des*if( b.monto is null, 0.00, b.monto)/100),2) impiva, 0.00, '',  '' ejercicio, a.id_emp " _
                        & " FROM " & nomTablaRenglones & " a " _
                        & " LEFT JOIN (SELECT fecha, tipo, monto " _
                        & "            From JSCONCTAIVA " _
                        & "            Where " _
                        & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
                        & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "') b ON (a.iva = b.tipo) " _
                        & " Where " _
                        & " a." & nomCampoClave & " = '" & NumeroDocumento & "' AND " _
                        & " a.codpro = '" & CodigoProveedor & "' and " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " GROUP BY a.iva ")

            Else
                EjecutarSTRSQL(MyConn, lblInfo, " insert into " & nomTablaIVA _
                       & " SELECT a." & nomCampoClave & ", a.codpro, a.iva, if( b.monto is null, 0.00, b.monto) , SUM(a." & nombreCampoTotalRenglones & "des) baseiva, " _
                       & " ROUND(SUM(a." & nombreCampoTotalRenglones & "des*if( b.monto is null, 0.00, b.monto)/100),2) impiva, '' ejercicio, a.id_emp " _
                       & " FROM " & nomTablaRenglones & " a " _
                       & " LEFT JOIN (SELECT fecha, tipo, monto " _
                       & "            From JSCONCTAIVA " _
                       & "            Where " _
                       & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
                       & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "') b ON (a.iva = b.tipo) " _
                       & " Where " _
                       & " a." & nomCampoClave & " = '" & NumeroDocumento & "' AND " _
                       & " a.codpro = '" & CodigoProveedor & "' and " _
                       & " a.id_emp = '" & jytsistema.WorkID & "' " _
                       & " GROUP BY a.iva ")
            End If

        End If

        CalculaTotalIVACompras = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select if(  sum(" & nomCampoImporteIVA & ") is null, 0.00, sum(" & nomCampoImporteIVA & ") ) from " _
                                                            & nomTablaIVA _
                                                            & " where " & _
                                                            nomCampoClave & " = '" & NumeroDocumento & "' and " _
                                                            & " codpro = '" & CodigoProveedor & "' and " _
                                                            & " id_emp = '" & jytsistema.WorkID & "'  "))


    End Function
    Public Function DocumentoCXPPoseeCancelacionesAbonos(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, _
                                                         ByVal NumeroDocumento As String, CodigoProveedor As String)
        Dim nCancela As String = EjecutarSTRSQL_Scalar(Myconn, lblInfo, " select comproba from jsprotrapag " _
                                                       & " where " _
                                                       & " codpro = '" & CodigoProveedor & "' and " _
                                                       & " nummov = '" & NumeroDocumento & "' AND " _
                                                       & " tipomov in ('AB', 'CA', 'NC', 'ND' ) AND " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' ")


        DocumentoCXPPoseeCancelacionesAbonos = False
        If nCancela <> "0" Then DocumentoCXPPoseeCancelacionesAbonos = True
    End Function

    Function DocumentoPoseeRetencionIVA(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String, ByVal CodigoProveedor As String) As Boolean


        If CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsprotrapag where " _
                                      & " SUBSTRING(concepto,1,13) = 'RETENCION IVA' AND " _
                                      & " TIPOMOV IN ('NC','ND') AND " _
                                      & " CODPRO = '" & CodigoProveedor & "' AND " _
                                      & " NUMMOV = '" & NumeroDocumento & "' AND " _
                                      & " ID_EMP = '" & jytsistema.WorkID & "' ").ToString) = 0 Then

            Return False
        Else
            Return True
        End If


    End Function

    Function DocumentoPoseeRetencionIVA_XXXXX_PARAELIMINAR(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nomTablaIVA As String, _
                                        ByVal nomCampoClave As String, ByVal NumeroDocumento As String, ByVal CodigoProveedor As String) As Boolean


        If CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from " & nomTablaIVA & " where " _
                              & nomCampoClave & " = '" & NumeroDocumento & "' and codpro = '" & CodigoProveedor _
                                  & "' and retencion <> '' and id_emp = '" & jytsistema.WorkID & "' group by " & nomCampoClave & "  ").ToString()) = 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Sub CargarListaDesdeCompras(ByVal dg As DataGridView, ByVal dt As DataTable)

        Dim aFld() As String = {"recibido", "emision", "numcom", "codpro", "nombre", "emisioniva", "fechasi"}
        Dim aNom() As String = {"", "Emisión", "N° Documento", "Código Proveedor", "Nombre Proveedor", "Fecha IVA", "Fecha Asiento"}
        Dim aAnc() As Long = {20, 90, 120, 120, 350, 90, 90}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                     AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", sFormatoFechaCorta, "", "", "", sFormatoFechaCorta, sFormatoFechaCorta}


        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor)




    End Sub

End Module
