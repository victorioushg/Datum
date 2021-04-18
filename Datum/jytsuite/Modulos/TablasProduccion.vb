Imports MySql.Data.MySqlClient
Imports MySql.Data.Types
Module TablasProduccion

    Private ft As New Transportables
    Public Sub InsertEditPRODUCCIONRenglonesFormulas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                    ByVal CodigoFormula As String, ByVal Renglon As String, _
                                                    ByVal CodigoMercancia As String, ByVal DescripcionMercancia As String, ByVal Cantidad As Double, _
                                                    ByVal Unidad As String, ByVal PesoReglon As Double, ByVal CostoUnitario As Double, ByVal TotalRenglon As Double, _
                                                    ByVal Almacen As String, ByVal Residual As Integer, ByVal Subensamble As Integer, _
                                                    ByVal PorcentajeResidual As Double)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsfabrenfor SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsfabrenfor SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codfor = '" & CodigoFormula & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " item = '" & CodigoMercancia & "' and " _
                & " residual = " & Residual & " and " _
                & " ID_EMP = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(CodigoFormula, "codfor")
        strSQL = strSQL & ModificarCadena(Renglon, "renglon")
        strSQL = strSQL & ModificarCadena(CodigoMercancia, "item")
        strSQL = strSQL & ModificarCadena(DescripcionMercancia, "descrip")
        strSQL = strSQL & ModificarDoble(Cantidad, "Cantidad")
        strSQL = strSQL & ModificarCadena(Unidad, "unidad")
        strSQL = strSQL & ModificarDoble(PesoReglon, "peso_renglon")
        strSQL = strSQL & ModificarDoble(CostoUnitario, "costou")
        strSQL = strSQL & ModificarDoble(TotalRenglon, "totren")
        strSQL = strSQL & ModificarCadena(Almacen, "almacen_salida")
        strSQL = strSQL & ModificarEntero(Residual, "residual")
        strSQL = strSQL & ModificarEntero(Subensamble, "subensamble")
        strSQL = strSQL & ModificarDoble(PorcentajeResidual, "porcentaje")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub


    Public Sub InsertEditPRODUCCIONEncabezadoFormulas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                   ByVal CodigoFormula As String, ByVal CodigoMercancia As String, _
                                                   ByVal Descripcion_1 As String, ByVal Descripcion_2 As String, ByVal Cantidad As Double, _
                                                   ByVal Unidad As String, ByVal PesoTotal As Double, ByVal AlmacenDestino As String, _
                                                   ByVal TotalNeto As Double, ByVal TotalIndirectos As Double, ByVal Total As Double,
                                                   Comentarios As String, ByVal Fecha As Date, ByVal Estatus As Integer)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsfabencfor SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsfabencfor SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codfor = '" & CodigoFormula & "' and " _
                & " ID_EMP = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(CodigoFormula, "codfor")
        strSQL = strSQL & ModificarCadena(CodigoMercancia, "codart")
        strSQL = strSQL & ModificarCadena(Descripcion_1, "descrip_1")
        strSQL = strSQL & ModificarCadena(Descripcion_2, "descrip_2")
        strSQL = strSQL & ModificarDoble(Cantidad, "Cantidad")
        strSQL = strSQL & ModificarCadena(Unidad, "unidad")
        strSQL = strSQL & ModificarDoble(PesoTotal, "peso_total")
        strSQL = strSQL & ModificarCadena(AlmacenDestino, "almacen_destino")
        strSQL = strSQL & ModificarDoble(TotalNeto, "total_neto")
        strSQL = strSQL & ModificarDoble(TotalIndirectos, "total_indirectos")
        strSQL = strSQL & ModificarDoble(Total, "total")
        strSQL = strSQL & ModificarCadena(Comentarios, "comentarios")
        strSQL = strSQL & ModificarFecha(Fecha, "fecha")
        strSQL = strSQL & ModificarEntero(Estatus, "estatus")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditPRODUCCIONEncabezadoOrdenProduccion(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                   ByVal CodigoOrden As String, ByVal Descripcion As String, _
                                                   ByVal FechaEmision As Date, ByVal FechaEntregaEstimada As Date, _
                                                   ByVal PesoTotal As Double, _
                                                   ByVal Estatus As Integer, ByVal OrdenProduccionRelacionada As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsfabencord SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsfabencord SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codord = '" & CodigoOrden & "' and " _
                & " ID_EMP = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(CodigoOrden, "codord")
        strSQL = strSQL & ModificarCadena(Descripcion, "descrip")
        strSQL = strSQL & ModificarFecha(FechaEmision, "emision")
        strSQL = strSQL & ModificarFecha(FechaEntregaEstimada, "estimada")
        strSQL = strSQL & ModificarDoble(PesoTotal, "peso_total")
        strSQL = strSQL & ModificarEntero(Estatus, "estatus")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")
        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditPRODUCCIONRenglonOrdenProduccion(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                               ByVal CodigoOrden As String, ByVal CodigoFormula As String, ByVal Descripcion As String, _
                                               ByVal Cantidad As Double, ByVal Unidad As String, Peso As Double, _
                                               ByVal CostoUnitario As Double, TotalRenglon As Double)
        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsfabrenord SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsfabrenord SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codord = '" & CodigoOrden & "' and " _
                & " codfor = '" & CodigoFormula & "' and " _
                & " ID_EMP = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(CodigoOrden, "codord")
        strSQL = strSQL & ModificarCadena(CodigoFormula, "codfor")
        strSQL = strSQL & ModificarCadena(Descripcion, "descrip")
        strSQL = strSQL & ModificarDoble(Cantidad, "cantidad")
        strSQL = strSQL & ModificarDoble(Peso, "PESO")
        strSQL = strSQL & ModificarCadena(Unidad, "UNIDAD")
        strSQL = strSQL & ModificarDoble(CostoUnitario, "COSTOUNITARIO")
        strSQL = strSQL & ModificarDoble(TotalRenglon, "TOTALRENGLON")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditPRODUCCIONCostoFijo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                   ByVal CodigoCosto As String, ByVal DescripcionCosto As String, _
                                                   ByVal Porcentaje As Double, ByVal MontoFijo As Double)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsfabcatfij SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsfabcatfij SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codcosto = '" & CodigoCosto & "' and " _
                & " ID_EMP = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(CodigoCosto, "codcosto")
        strSQL = strSQL & ModificarCadena(DescripcionCosto, "titulo")
        strSQL = strSQL & ModificarDoble(Porcentaje, "porcentaje")
        strSQL = strSQL & ModificarDoble(MontoFijo, "montofijo")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub



End Module
