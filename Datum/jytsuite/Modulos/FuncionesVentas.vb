Imports MySql.Data.MySqlClient
Module FuncionesVentas

    Private ft As New Transportables

    Function SaldoCxC(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCliente As String) As Double

        SaldoCxC = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) from jsventracob " _
                & " where codcli = '" & CodigoCliente & "' " _
                & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                & " and ID_EMP = '" & jytsistema.WorkID & "' group by codcli ")

        ft.Ejecutar_strSQL(myconn, " UPDATE jsvencatcli SET " _
            & " DISPONIBLE = LIMITECREDITO - " & SaldoCxC & ", " _
            & " SALDO = " & SaldoCxC _
            & " where codcli = '" & CodigoCliente & "' " _
            & " and ID_EMP = '" & jytsistema.WorkID & "'")

    End Function

    Public Sub AjustarExistencias(MyConn As MySqlConnection, NumeroDocumento As String, lblInfo As Label, _
                                  Almacen As String, Campo As String, Tabla As String, Origen As String)

        'AJUSTAR POR EXISTENCIAS
        Dim ds As New DataSet
        Dim dtFac As DataTable
        Dim nTableFac As String = "tblfac" & NumeroDocumento
        ds = DataSetRequery(ds, " select * from " & Tabla & " where " & Campo & " = '" & NumeroDocumento & "' and id_emp = '" & jytsistema.WorkID & "' order by estatus, renglon ", MyConn, nTableFac, lblInfo)
        dtFac = ds.Tables(nTableFac)

        If dtFac.Rows.Count > 0 Then
            For Each nRow As DataRow In dtFac.Rows
                With nRow
                    Dim pesoItem As Double = IIf(.Item("cantidad") > 0, .Item("peso") / .Item("cantidad"), 0.0)
                    Dim Existencia As Double = ExistenciasEnAlmacenes(MyConn, .Item("item"), Almacen)
                    Dim Equivalencia As Double = CantidadEquivalente(MyConn, lblInfo, .Item("item"), .Item("unidad"), .Item("cantidad"))
                    If .Item("ESTATUS") <> "0" Then
                        Existencia -= ft.DevuelveScalarDoble(MyConn, " select " _
                                                            & " sum(if( isnull(b.uvalencia), a.cantidad, a.cantidad / b.equivale )) " _
                                                            & " from " & Tabla & " a " _
                                                            & " left join jsmerequmer b on (a.item = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
                                                            & " where " _
                                                            & " a." & Campo & " = '" & NumeroDocumento & "' and " _
                                                            & " a.item = '" & .Item("ITEM") & "' and " _
                                                            & " a.estatus = '0' and " _
                                                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.item ")
                    End If

                    If Existencia > 0 Then
                        If Equivalencia > Existencia Then Equivalencia = Existencia

                        Dim CantidadValida As Double = CantidadDesquivalente(MyConn, lblInfo, .Item("item"), .Item("unidad"), Equivalencia)
                        If MultiploValido(MyConn, .Item("item"), .Item("unidad"), CantidadValida, lblInfo) Then

                            Dim TotalNetoRenglon As Double = CantidadValida * .Item("precio")
                            Dim DescuentoArticulo As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, TotalNetoRenglon * .Item("des_art") / 100)
                            Dim DescuentoCliente As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, (TotalNetoRenglon - DescuentoArticulo) * .Item("DES_CLI") / 100)
                            Dim DescuentoOferta As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, (TotalNetoRenglon - DescuentoArticulo - DescuentoCliente) * .Item("DES_OFE") / 100)
                            Dim TotalRenglon As Double = TotalNetoRenglon - DescuentoArticulo - DescuentoCliente - DescuentoOferta

                            ft.Ejecutar_strSQL(myconn, " update " & Tabla & " set " _
                                                        & " CANTIDAD = " & CantidadValida & ", " _
                                                        & " TOTREN = " & TotalRenglon & ", " _
                                                        & " TOTRENDES = " & TotalRenglon & ", " _
                                                        & " PESO = " & CantidadValida * pesoItem & " " _
                                                        & " Where " _
                                                        & " " & Campo & " = '" & NumeroDocumento & "' and " _
                                                        & " item = '" & .Item("item") & "' and " _
                                                        & " renglon = '" & .Item("renglon") & "' and " _
                                                        & " estatus = '" & .Item("estatus") & "' and " _
                                                        & " id_emp = '" & jytsistema.WorkID & "' ")
                        Else
                            ft.Ejecutar_strSQL(myconn, " delete from " & Tabla & " where  " _
                                           & " " & Campo & " = '" & NumeroDocumento & "' and  " _
                                           & " item = '" & .Item("item") & "' and " _
                                           & " renglon = '" & .Item("renglon") & "' and " _
                                           & " estatus = '" & .Item("estatus") & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

                            ft.Ejecutar_strSQL(myconn, " delete from jsvenrencom where numdoc = '" & NumeroDocumento & "' and " _
                                           & " origen = '" & Origen & "' and " _
                                           & " renglon = '" & .Item("renglon") & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

                        End If
                    Else
                        ft.Ejecutar_strSQL(myconn, " delete from " & Tabla & " where  " _
                                           & " " & Campo & " = '" & NumeroDocumento & "' and  " _
                                           & " item = '" & .Item("item") & "' and " _
                                           & " renglon = '" & .Item("renglon") & "' and " _
                                           & " estatus = '" & .Item("estatus") & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

                        ft.Ejecutar_strSQL(myconn, " delete from jsvenrencom where numdoc = '" & NumeroDocumento & "' and " _
                                       & " origen = '" & Origen & "' and " _
                                       & " renglon = '" & .Item("renglon") & "' and " _
                                       & " id_emp = '" & jytsistema.WorkID & "' ")

                    End If
                    ActualizarExistenciasPlus(MyConn, .Item("item"), Almacen)

                End With
            Next
        End If

        ds.Dispose()
        dtFac.Dispose()
        dtFac = Nothing
        ds = Nothing



    End Sub
    Public Function tipoPersona(RIF_CI As String) As Integer
        '0 Persona Juridica ; 1 = persona natural

        Dim TipoRazon As String = RIF_CI.Replace("_", "").Replace(" ", "").Split("-")(0)
        Dim Documento As String = RIF_CI.Replace("_", "").Replace(" ", "").Split("-")(1)
        Dim Identificador As String = RIF_CI.Replace("_", "").Replace(" ", "").Split("-")(2)

        If InStr("V.E.P", TipoRazon) > 0 And Identificador = "" Then Return 1

        Return 0

    End Function

    Public Sub calculaIVAFacturasConCondicionEspecial(MyConn As MySqlConnection, nomTablaRenglones As String, nomTablaIVA As String, _
                                                       personaJuridica As Integer, _
                                                       NumeroFactura As String, OrigenFactura As String, NumeroSerialFiscal As String, _
                                                       MontoRestante As Double, formaDePago As String)

        Dim lblInfo As New Label
        If cumpleCondicionesIVAEspecial(MyConn, personaJuridica, NumeroFactura, OrigenFactura, MontoRestante, formaDePago) Then

            ActualizarIVARenglonAlbaranPlus(MyConn, lblInfo, nomTablaIVA, nomTablaRenglones, "numfac", _
                                   NumeroFactura, jytsistema.sFechadeTrabajo, "totrendes", _
                                   NumeroSerialFiscal)
        Else
            ActualizarIVARenglonAlbaran(MyConn, lblInfo, nomTablaIVA, nomTablaRenglones, "numfac", _
                                NumeroFactura, jytsistema.sFechadeTrabajo, "totrendes", _
                                NumeroSerialFiscal)
        End If

    End Sub

    Public Function montoResidualFactura(MyConn As MySqlConnection, nomTablaIVA As String, NumeroFactura As String, _
                                         OrigenFactura As String) As Double

        Dim totalAPagar As Double = ft.DevuelveScalarDoble(MyConn, " select sum(baseiva + impiva ) from " & nomTablaIVA & " " _
                                                               & " where " _
                                                               & " numfac = '" & NumeroFactura & "' and " _
                                                               & " id_emp = '" & jytsistema.WorkID & "' ")

        Dim subTotalPagos As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) from jsvenforpag " _
                            & " where " _
                            & " numfac = '" & NumeroFactura & "' and " _
                            & " origen = '" & OrigenFactura & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

        Return subTotalPagos - totalAPagar

    End Function

    Public Function cumpleCondicionesIVAEspecial(MyConn As MySqlConnection, personaJuridica As Integer, _
                                                 NumeroDocumento As String, nModulo As String, _
                                                 totalAPagar As Double, formaDePago As String) As Boolean

        ' 0 Persona Juridia ; 1 Persona Natural 
        '///If personaJuridica = 0 Then Return False

        '{"EF", "CH", "CT", "DP"}
        Dim cantidadFP As Integer = ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvenforpag " _
            & " where " _
            & " numfac = '" & NumeroDocumento & "' and " _
            & " origen = '" & nModulo & "' and " _
            & " formapag IN ('EF','CH','DP','CT') AND " _
            & " id_emp = '" & jytsistema.WorkID & "' group by formapag ")

        'Existen más de una forma de pago diferente de TA o TR
        If cantidadFP > 0 Then Return False

        If InStr("EF.CH.CT.DP", formaDePago) > 0 Then Return False

        Return True


    End Function

    Public Sub AjustarPorCuotaVendedor(MyConn As MySqlConnection, lblInfo As Label, NumeroDocumento As String, FechaDocumento As Date, _
                              Asesor As String, Campo As String, Tabla As String, Origen As String)

        'AJUSTAR POR EXISTENCIAS
        Dim ds As New DataSet
        Dim dtFac As DataTable
        Dim nTableFac As String = "tblfac" & NumeroDocumento
        ds = DataSetRequery(ds, " select * from " & Tabla & " where " & Campo & " = '" & NumeroDocumento & "' and id_emp = '" & jytsistema.WorkID & "' order by estatus, renglon ", MyConn, nTableFac, lblInfo)
        dtFac = ds.Tables(nTableFac)

        If dtFac.Rows.Count > 0 Then
            For Each nRow As DataRow In dtFac.Rows
                With nRow
                    'SI VENTA DE MERCANCIA DEBE MEDIRSE POR CUOTA FIJA
                    If ft.DevuelveScalarBooleano(MyConn, "SELECT CUOTAFIJA FROM jsmerctainv WHERE CODART = '" & .Item("ITEM") & "' AND ID_EMP = '" & jytsistema.WorkID & "' ") Then


                        Dim pesoItem As Double = IIf(.Item("cantidad") > 0, .Item("peso") / .Item("cantidad"), 0.0)
                        Dim Existencia As Double = CuotaAsesorMES_KGR(MyConn, Asesor, .Item("item"), FechaDocumento) - _
                            CantidadVentasEnElMes_KGR(MyConn, Asesor, .Item("item"), jytsistema.sFechadeTrabajo)
                        Dim Equivalencia As Double = .Item("peso")
                        If .Item("ESTATUS") <> "0" Then

                            Existencia -= ft.DevuelveScalarDoble(MyConn, " select " _
                                                                & " sum(a.peso) from " & Tabla & " a " _
                                                                & " where " _
                                                                & " a." & Campo & " = '" & NumeroDocumento & "' and " _
                                                                & " a.item = '" & .Item("ITEM") & "' and " _
                                                                & " a.estatus = '0' and " _
                                                                & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                                                & " a.id_emp = '" & jytsistema.WorkID & "' group by a.item ")
                        End If

                        If Existencia > 0 Then

                            If Equivalencia > Existencia Then Equivalencia = Existencia

                            Dim CantidadValida As Double = Equivalencia / pesoItem

                            If MultiploValido(MyConn, .Item("item"), .Item("unidad"), CantidadValida, lblInfo) Then

                                Dim TotalNetoRenglon As Double = CantidadValida * .Item("precio")
                                Dim DescuentoArticulo As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, TotalNetoRenglon * .Item("des_art") / 100)
                                Dim DescuentoCliente As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, (TotalNetoRenglon - DescuentoArticulo) * .Item("DES_CLI") / 100)
                                Dim DescuentoOferta As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, (TotalNetoRenglon - DescuentoArticulo - DescuentoCliente) * .Item("DES_OFE") / 100)
                                Dim TotalRenglon As Double = TotalNetoRenglon - DescuentoArticulo - DescuentoCliente - DescuentoOferta

                                ft.Ejecutar_strSQL(MyConn, " update " & Tabla & " set " _
                                                            & " CANTIDAD = " & CantidadValida & ", " _
                                                            & " TOTREN = " & TotalRenglon & ", " _
                                                            & " TOTRENDES = " & TotalRenglon & ", " _
                                                            & " PESO = " & CantidadValida * pesoItem & " " _
                                                            & " Where " _
                                                            & " " & Campo & " = '" & NumeroDocumento & "' and " _
                                                            & " item = '" & .Item("item") & "' and " _
                                                            & " renglon = '" & .Item("renglon") & "' and " _
                                                            & " estatus = '" & .Item("estatus") & "' and " _
                                                            & " id_emp = '" & jytsistema.WorkID & "' ")
                            Else
                                ft.Ejecutar_strSQL(MyConn, " delete from " & Tabla & " where  " _
                                               & " " & Campo & " = '" & NumeroDocumento & "' and  " _
                                               & " item = '" & .Item("item") & "' and " _
                                               & " renglon = '" & .Item("renglon") & "' and " _
                                               & " estatus = '" & .Item("estatus") & "' and " _
                                               & " id_emp = '" & jytsistema.WorkID & "' ")

                                ft.Ejecutar_strSQL(MyConn, " delete from jsvenrencom where numdoc = '" & NumeroDocumento & "' and " _
                                               & " origen = '" & Origen & "' and " _
                                               & " renglon = '" & .Item("renglon") & "' and " _
                                               & " id_emp = '" & jytsistema.WorkID & "' ")

                            End If
                        Else
                            ft.Ejecutar_strSQL(MyConn, " delete from " & Tabla & " where  " _
                                               & " " & Campo & " = '" & NumeroDocumento & "' and  " _
                                               & " item = '" & .Item("item") & "' and " _
                                               & " renglon = '" & .Item("renglon") & "' and " _
                                               & " estatus = '" & .Item("estatus") & "' and " _
                                               & " id_emp = '" & jytsistema.WorkID & "' ")

                            ft.Ejecutar_strSQL(MyConn, " delete from jsvenrencom where numdoc = '" & NumeroDocumento & "' and " _
                                           & " origen = '" & Origen & "' and " _
                                           & " renglon = '" & .Item("renglon") & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

                        End If
                        ActualizarExistenciasPlus(MyConn, .Item("item"))

                    End If

                End With
            Next
        End If

        ds.Dispose()
        dtFac.Dispose()
        dtFac = Nothing
        ds = Nothing



    End Sub


    Public Sub CalculaBonificaciones(MyConn As MySqlConnection, lblInfo As Label, nModulo As Integer, numDocumento As String, _
                                     dFechaBonificacion As Date, sTarifa As String)

        'nModulo --> 0 = "FAC", 1 = "PED", 2 = "PPE", 3 = "COT", 4 = "PFC"

        Dim nTabla() As String = {"jsvenrenfac", "jsvenrenped", "jsvenrenpedrgv", "jsvenrencot", "jsvenrennot"}
        Dim nCampo() As String = {"numfac", "numped", "numped", "numcot", "numfac"}

        Dim nTablaRenglones As String = "tblRenglones" & ft.NumeroAleatorio(100000)

        Dim fBonificacion As Bonificaciones
        Dim CantidadBonificacion As Double
        Dim ItemdeOferta As String
        Dim UnidadOferta As String
        Dim PrecioOferta As Double
        Dim PesoOferta As Double

        ft.Ejecutar_strSQL(MyConn, "DELETE from " & nTabla(nModulo) & " where " & nCampo(nModulo) & " = '" & numDocumento & "' and EDITABLE = 1 and " _
            & " ESTATUS = '2' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' ")

        Dim ds As New DataSet
        Dim dtRenglones As DataTable = ft.AbrirDataTable(ds, nTablaRenglones, MyConn, " select * from " & nTabla(nModulo) & " " _
                            & " where " _
                            & nCampo(nModulo) & " = '" & numDocumento & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " ORDER BY renglon ")

        If dtRenglones.Rows.Count > 0 Then
            For Each nRow As DataRow In dtRenglones.Rows
                With nRow

                    fBonificacion = BonificacionVigenteOferta(MyConn, lblInfo, dFechaBonificacion, .Item("ITEM"), _
                                                              .Item("RENGLON"), sTarifa, numDocumento, nModulo, 0)

                    CantidadBonificacion = fBonificacion.CantidadABonificar
                    ItemdeOferta = fBonificacion.ItemABonificar
                    UnidadOferta = fBonificacion.UnidadDeBonificacion
                    PrecioOferta = ft.DevuelveScalarDoble(MyConn, " select precio_" & sTarifa & " from jsmerctainv where codart = '" & ItemdeOferta & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    PrecioOferta = Math.Round(PrecioOferta / Equivalencia(MyConn, ItemdeOferta, UnidadOferta), 2)
                    PesoOferta = ft.DevuelveScalarDoble(MyConn, " select pesounidad from jsmerctainv where codart = '" & ItemdeOferta & "' and id_emp ='" & jytsistema.WorkID & "' ")
                    PesoOferta = CantidadBonificacion * PesoOferta / Equivalencia(MyConn, ItemdeOferta, UnidadOferta)

                    If CantidadBonificacion <= 0 Then
                    Else
                        ft.Ejecutar_strSQL(MyConn, " INSERT INTO " & nTabla(nModulo) _
                            & "(" & nCampo(nModulo) & ", RENGLON, ITEM, DESCRIP, IVA, UNIDAD, CANTIDAD," _
                            & " PRECIO, DES_ART, DES_CLI, TOTREN, PESO, " _
                            & " ESTATUS, ACEPTADO, EDITABLE, EJERCICIO, ID_EMP) VALUES (" _
                            & "'" & numDocumento & "', " _
                            & "'" & ft.autoCodigo(MyConn, "RENGLON", nTabla(nModulo), nCampo(nModulo) + ".id_emp", numDocumento + "." + jytsistema.WorkID, 5) & "', " _
                            & "'" & ItemdeOferta & "', " _
                            & "'" & ft.DevuelveScalarCadena(MyConn, " SELECT NOMART FROM jsmerctainv WHERE codart = '" & ItemdeOferta & "' AND  id_emp = '" & jytsistema.WorkID & "' ") & "', " _
                            & "'" & ft.DevuelveScalarCadena(MyConn, " SELECT IVA FROM jsmerctainv WHERE codart = '" & ItemdeOferta & "' AND  id_emp = '" & jytsistema.WorkID & "' ") & "', " _
                            & "'" & UnidadOferta & "', " _
                            & "" & CantidadBonificacion & ", " _
                            & "" & PrecioOferta & ", " _
                            & "" & 100 & ", " _
                            & "" & 0 & ", " _
                            & "" & 0 & ", " _
                            & "" & PesoOferta & ", " _
                            & "'2', '1', 1, " _
                            & "'" & jytsistema.WorkExercise & "', " _
                            & "'" & jytsistema.WorkID & "')")

                    End If

                End With
            Next

        End If

        dtRenglones.Dispose()
        ds.Dispose()

        dtRenglones = Nothing
        ds = Nothing


    End Sub


    Function ChequesDevueltosCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Cliente As String) As Integer
        Return ft.DevuelveScalarEntero(MyConn, "select count(*) " _
            & " from jsbanchedev where YEAR('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') = YEAR(FECHADEV) AND " _
            & " PROV_CLI = '" & Cliente & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' group by prov_cli ")

    End Function

    Public Function PoseeChequesDevueltosSinCancelar(MyConn As MySqlConnection, lblInfo As Label, CodigoCliente As String) As Boolean

        Dim SaldoPorCancelar As Double = ft.DevuelveScalarDoble(MyConn, " SELECT " _
            & " SUM(b.importe) " _
            & " FROM jsvenencndb a " _
            & " LEFT JOIN jsventracob b ON (a.codcli = b.codcli AND a.numndb = b.nummov AND a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.codcli = '" & CodigoCliente & "' AND " _
            & " a.comen = 'CHEQUE DEVUELTO, COMISION CHEQUE DEVUELTO y GASTOS ADM.' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.numndb " _
            & " Having SUM(b.importe) > 0 " _
            & " ORDER BY a.emision desc ")

        If SaldoPorCancelar <= 0.0 Then SaldoPorCancelar = ft.DevuelveScalarDoble(MyConn, " SELECT " _
            & " SUM(IMPORTE) FROM jsventracob " _
            & " WHERE " _
            & " SUBSTRING(nummov,1,2) = 'CD' AND " _
            & " codcli = '" & CodigoCliente & "' " _
            & " GROUP BY nummov " _
            & " HAVING SUM(IMPORTE) > 0 ")

        PoseeChequesDevueltosSinCancelar = False
        If SaldoPorCancelar > 0 Then PoseeChequesDevueltosSinCancelar = True

    End Function



    Public Function DocumentoPoseeCancelacionesAbonos(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String)
        Dim nCancela As String = ft.DevuelveScalarCadena(Myconn, " select comproba from jsventracob " _
                                                       & " where " _
                                                       & " nummov = '" & NumeroDocumento & "' AND " _
                                                       & " tipomov in ('AB', 'CA', 'NC', 'ND' ) AND " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' ")


        DocumentoPoseeCancelacionesAbonos = False
        If nCancela <> "" Then DocumentoPoseeCancelacionesAbonos = True
    End Function

    Public Function DocumentoPoseeCancelacionesAbonosDepositados(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String)

        Dim dsCan As New DataSet
        Dim dtCan As New DataTable
        Dim nTablaCan As String = "tblCan" & ft.NumeroAleatorio(10000)
        DocumentoPoseeCancelacionesAbonosDepositados = False

        dsCan = DataSetRequery(dsCan, " Select * from jsventracob  " _
                                                        & " where " _
                                                        & " nummov = '" & NumeroDocumento & "' AND " _
                                                        & " tipomov in ('AB', 'CA', 'NC', 'ND' ) AND " _
                                                        & " id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCan, lblInfo)

        dtCan = dsCan.Tables(nTablaCan)

        For Each nRow As DataRow In dtCan.Rows
            With nRow
                If DocumentoPoseeCancelacionesAbonos(MyConn, lblInfo, .Item("comproba")) Then DocumentoPoseeCancelacionesAbonosDepositados = True
            End With
        Next

        dtCan.Dispose()
        dsCan.Dispose()

        dtCan = Nothing
        dsCan = Nothing


    End Function


    Public Function CancelacionAbonoDepositado(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ComprobanteNumero As String) As Boolean

        Dim Deposito As String = ft.DevuelveScalarCadena(MyConn, " SELECT " _
            & " a.DEPOSITO " _
            & " FROM jsbantracaj a " _
            & " Where " _
            & " a.nummov = '" & ComprobanteNumero & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' ")


        If Deposito <> "" Then CancelacionAbonoDepositado = True

    End Function

    Public Sub IniciarDescuentosAsesorEnCombo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal cmbDescuentos As ComboBox, _
                                              ByVal CodigoAsesor As String, ByVal TipoAsesor As Integer, ByVal FechaDescuento As Date)

        Dim ds As New DataSet
        Dim eCont As Integer
        Dim nTabla As String = "tDescuentosA"
        Dim str As String = ""

        ds = DataSetRequery(ds, " select * from jsconcatdes " _
            & " where " _
            & " codven =  '" & CodigoAsesor & "' and " _
            & " inicio <= '" & ft.FormatoFechaMySQL(FechaDescuento) & "' AND " _
            & " fin >= '" & ft.FormatoFechaMySQL(FechaDescuento) & "' AND " _
            & " tipo = " & TipoAsesor & " and " _
            & " id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTabla, lblInfo)

        With ds.Tables(nTabla)
            Dim aDescuentos(.Rows.Count - 1) As String
            For eCont = 0 To .Rows.Count - 1
                aDescuentos(eCont) = .Rows(eCont).Item("coddes") & " | " & _
                    .Rows(eCont).Item("descrip")
            Next
            ft.RellenaCombo(aDescuentos, cmbDescuentos)
        End With

        ds.Tables(nTabla).Dispose()
        ds.Dispose()
        ds = Nothing

    End Sub

    Public Function DesbloqueoDeCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal CodigoCliente As String, _
                    ByVal Fecha As Date, ByVal DiasLimite As Integer, ByVal Causa As String, ByVal Comentario As String) As Boolean

        Dim dt As New DataTable
        Dim nTabla As String = "tbl" & ft.NumeroAleatorio(100000)
        Dim strSQL As String = ""

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM07")) Then
            strSQL = " Union " _
                & " SELECT a.codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, IFNULL(SUM(a.IMPORTE),0) as saldo, " _
                & " to_days('" & ft.FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence) as DV, " _
                & " if( to_days('" & ft.FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence)>= " & DiasLimite & ", 1, 0) as lapso " _
                & " from jsventracob a " _
                & " LEFT JOIN jsvencatcli b ON  (b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP) " _
                & " Where a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " and a.codcli = '" & CodigoCliente & "' " _
                & " and left(a.nummov,2) = 'CD' " _
                & " and historico = 0 " _
                & " GROUP BY a.codcli, a.nummov " _
                & " Having Saldo > 0.001 or Saldo < -0.001 "
        End If


        ds = DataSetRequery(ds, "SELECT a.CODCLI, b.NOMBRE, a.NUMMOV, a.TIPOMOV, a.REFER, a.EMISION, a.VENCE, a.IMPORTE, IFNULL(SUM(a.IMPORTE),0) AS SALDO, " _
            & " to_days('" & ft.FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence) as DV, " _
            & " if( to_days('" & ft.FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence)>= " & DiasLimite & ", 1, 0) as lapso " _
            & " from jsventracob a " _
            & " LEFT JOIN jsvencatcli b ON (b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP)  " _
            & " Where a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " and a.tipomov <> 'ND' " _
            & " and a.codcli = '" & CodigoCliente & "' " _
            & " GROUP BY a.codcli, a.nummov Having Saldo > 0.001 AND a.vence < '" & ft.FormatoFechaMySQL(Fecha) & "' and lapso = 1 " _
            & strSQL, MyConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        If dt.Rows.Count = 0 Then
            ft.Ejecutar_strSQL(MyConn, " UPDATE jsvencatcli set estatus = 0 where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "'  ")
            InsertEditVENTASExpedienteCliente(MyConn, lblInfo, True, CodigoCliente, Fecha, Comentario, 0, Causa, 0)
        End If

        dt.Dispose()
        dt = Nothing

    End Function






    Public Function DescuentoAsesorValido(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoDescuento As String, _
                                          ByVal CodigoAsesor As String, ByVal FechaDescuento As Date, _
                                          ByVal TipoAsesor As Integer, ByVal PorcentajeDescuento As Double) As Boolean

        Return ft.DevuelveScalarBooleano(MyConn, " select IF( pordes IS NULL, 0, IF( " & PorcentajeDescuento & " > pordes OR " & PorcentajeDescuento & " <= 0, 0, 1 ) ) from jsconcatdes " _
                                            & " where " _
                                            & " coddes = '" & CodigoDescuento & "' and " _
                                            & " codven =  '" & CodigoAsesor & "' and " _
                                            & " inicio <= '" & ft.FormatoFechaMySQL(FechaDescuento) & "' AND " _
                                            & " fin >= '" & ft.FormatoFechaMySQL(FechaDescuento) & "' AND " _
                                            & " tipo = " & TipoAsesor & " and " _
                                            & " id_emp = '" & jytsistema.WorkID & "' ")

    End Function

    Function ActualizarDescuentoVentas(ByVal MyConn As MySqlConnection, ByVal dt As DataTable, _
                                       ByVal nomTablaEnDB As String, ByVal nomCampoEnBD As String, _
                                       ByVal MontoInicial As Double, ByVal lblInfo As Label) As Double

        Dim Subtotal As Double = MontoInicial
        Dim Descuento As Double = 0.0
        Dim TotalDescuento As Double = 0.0
        Dim eCont As Integer

        If dt.Rows.Count > 0 Then
            For eCont = 0 To dt.Rows.Count - 1
                With dt.Rows(eCont)
                    Descuento = Subtotal * .Item("PORDES") / 100
                    ft.Ejecutar_strSQL(MyConn, " update " & nomTablaEnDB & " Set " _
                        & " descuento = " & Descuento & ", " _
                        & " subtotal = " & Subtotal - Descuento & " " _
                        & " where " _
                        & nomCampoEnBD & " = '" & .Item(nomCampoEnBD) & "' and " _
                        & " renglon = '" & .Item("renglon") & "' and " _
                        & " aceptado = '" & .Item("aceptado") & "' and " _
                        & " ejercicio = '" & .Item("ejercicio") & "' and " _
                        & " id_emp = '" & .Item("id_emp") & "' ")

                    Subtotal -= Descuento
                    TotalDescuento += Descuento

                End With
            Next
            Return TotalDescuento
        Else
            Return 0.0
        End If

    End Function

    Public Function CalculaTotalRenglonesVentas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                                ByVal nomTablaEnDB As String, ByVal nomCampoClaveEnBD As String, _
                                                ByVal nomCampoImporteEnBD As String, ByVal NumeroDocumento As String, _
                                                Optional ByVal TipoRenglon As Integer = 9, _
                                                Optional ByVal CodigoProveedor As String = "") As Double

        CalculaTotalRenglonesVentas = ft.DevuelveScalarDoble(MyConn, " select sum(" & nomCampoImporteEnBD & ") from " _
                                                            & nomTablaEnDB _
                                                            & " where " & _
                                                            nomCampoClaveEnBD & " = '" & NumeroDocumento & "' and " _
                                                            & IIf(CodigoProveedor <> "", " codpro = '" & CodigoProveedor & "' and ", "") _
                                                            & IIf(TipoRenglon <= 2, " estatus = '" & TipoRenglon & "' and ", "") _
                                                            & " id_emp = '" & jytsistema.WorkID & "'  ")


    End Function
    Public Function MontoParaDescuentoVentas(ByVal MyConn As MySqlConnection, ByVal discountTable As SimpleTableProperties)

        MontoParaDescuentoVentas = ft.DevuelveScalarDoble(MyConn, " SELECT SUM(a.totren) " _
                & "             FROM " & discountTable.NombreTabla & " a " _
                & "             LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                & "             Where " _
                & "             a.estatus = 0 and " _
                & "             b.descuento = 1 AND " _
                & "             a." & discountTable.DocumentKey & " = '" & discountTable.DocumentValue & "'  AND " _
                & "             a.id_emp = '" & jytsistema.WorkID & "' group by a." & discountTable.DocumentKey & " ")

    End Function
    Public Function CalculaTotalIVAVentas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nomTablaDescuentos As String,
                                                ByVal nomTablaIVA As String, ByVal nomTablaRenglones As String,
                                                ByVal nomCampoClave As String, ByVal NumeroDocumento As String,
                                                ByVal nomCampoImporteIVA As String, ByVal nomCampoImporteRenglon As String,
                                                ByVal FechaIVA As Date, Optional ByVal nombreCampoTotalRenglones As String = "totren",
                                                Optional ByVal numSerialPOS As String = "",
                                                Optional ByVal TipoMOV As Integer = 0) As Double

        ft.Ejecutar_strSQL(MyConn, " delete from " & nomTablaIVA & " where " & nomCampoClave & " = '" & NumeroDocumento & "' and " _
                       & IIf(nomTablaRenglones = "jsvenrenpos",
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " numserial = '" & numSerialPOS & "' AND ", ""), "") _
                       & IIf(nomTablaRenglones = "jsvenrenpos",
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), " tipo = '" & TipoMOV & "' AND ", ""), "") _
                       & " id_emp = '" & jytsistema.WorkID & "'  ")


        ft.Ejecutar_strSQL(MyConn, " insert into " & nomTablaIVA _
                & " SELECT a." & nomCampoClave & ", " _
                & IIf(nomTablaRenglones = "jsvenrenpos", IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " '" & numSerialPOS & "',", ""), "") _
                & IIf(nomTablaRenglones = "jsvenrenpos", IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), TipoMOV & ", ", ""), "") _
                & " a.iva, if( b.monto is null, 0.00, b.monto) , SUM(a." & nombreCampoTotalRenglones & "des) baseiva, " _
                & " ROUND(SUM(a." & nombreCampoTotalRenglones & "des*if( b.monto is null, 0.00, b.monto)/100),2) impiva, '' ejercicio, a.id_emp " _
                & " FROM " & nomTablaRenglones & " a " _
                & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ") b ON (a.iva = b.tipo) " _
                & " Where " _
                & " a." & nomCampoClave & " = '" & NumeroDocumento & "' AND " _
                & IIf(nomTablaRenglones = "jsvenrenpos",
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " a.numserial = '" & numSerialPOS & "' AND ", ""), "") _
                       & IIf(nomTablaRenglones = "jsvenrenpos",
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), " a.tipo = '" & TipoMOV & "' AND ", ""), "") _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.iva ")


        CalculaTotalIVAVentas = ft.DevuelveScalarDoble(MyConn, " select sum(" & nomCampoImporteIVA & ") from " _
                                                            & nomTablaIVA _
                                                            & " where " &
                                                            nomCampoClave & " = '" & NumeroDocumento & "' and " _
                                                            & IIf(nomTablaRenglones = "jsvenrenpos",
                                                                IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " numserial = '" & numSerialPOS & "' AND ", ""), "") _
                                                            & IIf(nomTablaRenglones = "jsvenrenpos",
                                                                IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), " tipo = '" & TipoMOV & "' AND ", ""), "") _
                                                            & " id_emp = '" & jytsistema.WorkID & "'  ")


    End Function
    Public Sub MostrarTotalesCambioMoneda(MyConn As MySqlConnection, Emision As DateTime, txtTotal As TextBox,
                                          txtTotalCambioEmision As TextBox, txtTotalActual As TextBox)

        Dim defaultChange = ft.DevuelveScalarEntero(MyConn, " select monedacambio from jsconctaemp where id_emp = '" + jytsistema.WorkID + "' ")
        Dim symbolChange = ft.DevuelveScalarCadena(MyConn, " select codigoISO from jsconcatmon where id = " + defaultChange.ToString() + " ")

        Dim releaseChange = CambioActual(MyConn, Emision)
        Dim actualChange = CambioActual(MyConn)

        Dim totalChange = Math.Round(1 / releaseChange * Convert.ToDecimal(txtTotal.Text), 2)
        txtTotalCambioEmision.Text = ft.FormatoNumero(totalChange) + " " + symbolChange
        txtTotalActual.Text = ft.FormatoNumero(totalChange * actualChange)

    End Sub
    Public Sub MostrarIVAAlbaran(Myconn As MySqlConnection, ByVal ivaTable As SimpleTableProperties, ByVal dgIVA As DataGridView,
                                 txtTotalIVA As TextBox)

        Dim strSQLIVA As String = "select * from " & ivaTable.NombreTabla & " " _
                            & " where " _
                            & " " & ivaTable.DocumentKey & "  = '" & ivaTable.DocumentValue & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

        Dim ds As New DataSet
        Dim lblinfo As New Label
        Dim dtIVA As New DataTable

        ds = DataSetRequery(ds, strSQLIVA, Myconn, "nTablaIVA", lblinfo)
        dtIVA = ds.Tables("nTablaIVA")

        Dim aCampos() As String = {"tipoiva", "poriva", "baseiva", "impiva"}
        Dim aNombres() As String = {"", "", "", ""}
        Dim aAnchos() As Integer = {15, 45, 90, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoPorcentajeSimbolo, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 7, False)

        txtTotalIVA.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(Myconn, " select SUM(IMPIVA) from " & ivaTable.NombreTabla _
                                                                   & " where " & ivaTable.DocumentKey & " = '" & ivaTable.DocumentValue _
                                                                   & "' and id_emp = '" & jytsistema.WorkID & "' group by " & ivaTable.DocumentKey & " "))

        dtIVA.Dispose()
        ds.Dispose()

    End Sub
    Public Sub MostrarDescuentosAlbaran(MyConn As MySqlConnection, ByVal descTable As SimpleTableProperties, ByVal dgDescuentos As DataGridView,
                                         txtTotalDescuentos As TextBox)

        Dim strSQLDescuentos As String = "select * from " & descTable.NombreTabla & " " _
                            & " where " _
                            & " " & descTable.DocumentKey & "  = '" & descTable.DocumentValue & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        Dim ds As New DataSet
        Dim dtDescuentos As New DataTable

        ds = DataSetRequery(ds, strSQLDescuentos, MyConn, "nTablaDescuentos")
        dtDescuentos = ds.Tables("nTablaDescuentos")

        Dim aCampos() As String = {"pordes", "descuento"}
        Dim aNombres() As String = {"", ""}
        Dim aAnchos() As Integer = {45, 120}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {sFormatoPorcentajeSimbolo, sFormatoNumero}
        IniciarTabla(dgDescuentos, dtDescuentos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        Dim tableDiscount As New SimpleTableProperties(descTable.NombreTabla.Replace("des", "ren"), descTable.DocumentKey, descTable.DocumentValue)
        Dim MontoParaDescuento As Double = MontoParaDescuentoVentas(MyConn, tableDiscount)

        txtTotalDescuentos.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, " select SUM(DESCUENTO) from " & descTable.NombreTabla _
                & " where " & descTable.DocumentKey & " = '" & descTable.DocumentValue & "' and id_emp = '" & jytsistema.WorkID & "' group by " & descTable.DocumentKey & " "))

        dtDescuentos.Dispose()
        ds.Dispose()

    End Sub

    Public Sub MostrarDisponibilidad(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCliente As String, ByVal RIF As String, ByVal ds As DataSet, ByVal dg As DataGridView)
        If RIF <> "" Then

            Dim dtDisp As DataTable = ft.AbrirDataTable(ds, "tblDisponible", MyConn, " select codcli, saldo, disponible, elt(estatus+1,'Activo','Bloqueado','Inactivo', 'Desincorporado') estatus, id_emp " _
                            & " from jsvencatcli where rif = '" & RIF & "' order by id_emp ")

            Dim aCampos() As String = {"saldo.Saldo.130.D.Numero",
                                       "disponible.Disponible.130.D.Numero",
                                       "estatus.Estatus.90.I.",
                                       "ID_EMP.ID.10.C."}

            ft.IniciarTablaPlus(dg, dtDisp, aCampos, True, , New Font("Consolas", 8, FontStyle.Regular), False)

            Dim SaldoCliente As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) " _
                                                                    & " from jsventracob " _
                                                                    & " where " _
                                                                    & " codcli = '" & CodigoCliente & "' and " _
                                                                    & " id_emp = '" & jytsistema.WorkID & "' group by codcli ")

            Dim Disponibilidad As Double = ft.DevuelveScalarDoble(MyConn, " select disponible from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Dim EstatusCliente As Integer = ft.DevuelveScalarEntero(MyConn, " select estatus from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

            dg.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            dg.RowsDefaultCellStyle.SelectionForeColor = dg.DefaultCellStyle.ForeColor
            If EstatusCliente = 0 Then
                If SaldoCliente = 0 Then 'Verde 
                    dg.RowsDefaultCellStyle.BackColor = Color.PaleGreen
                    dg.RowsDefaultCellStyle.SelectionBackColor = Color.PaleGreen
                    dg.AlternatingRowsDefaultCellStyle.BackColor = Color.ForestGreen
                Else
                    If SaldoCliente > 0 Then
                        If Disponibilidad > 0 Then 'Amarillo
                            dg.RowsDefaultCellStyle.BackColor = Color.LightYellow
                            dg.RowsDefaultCellStyle.SelectionBackColor = Color.LightYellow
                            dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Yellow
                        Else 'Rojo
                            dg.RowsDefaultCellStyle.BackColor = Color.LavenderBlush
                            dg.RowsDefaultCellStyle.SelectionBackColor = Color.LavenderBlush
                            dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Crimson
                        End If
                    Else 'Rojo
                        dg.RowsDefaultCellStyle.BackColor = Color.LavenderBlush
                        dg.RowsDefaultCellStyle.SelectionBackColor = Color.LavenderBlush
                        dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Crimson
                    End If
                End If
            Else
                dg.RowsDefaultCellStyle.BackColor = Color.LavenderBlush
                dg.RowsDefaultCellStyle.SelectionBackColor = Color.LavenderBlush
                dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Crimson
            End If

        End If

    End Sub
    Public Function ProveedorClienteEnItem(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Proveedor As String, _
                                           ByVal Cliente As String) As Boolean

        Return ft.DevuelveScalarBooleano(MyConn, " select count(*) from jsvenprocli where " _
            & " CODPRO = '" & Proveedor & "' AND " _
            & " CODCLI = '" & Cliente & "' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Function
    Function MercanciaAceptaDescuento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoArticulo As String) As Boolean
        MercanciaAceptaDescuento = ft.DevuelveScalarBooleano(MyConn, " select descuento from jsmerctainv where codart = '" & CodigoArticulo & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Function
    Public Function CondicionPagoProveedorCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal FormaDePago As String) As Integer

        Dim Periodo As Integer = ft.DevuelveScalarEntero(MyConn, " select periodo from jsconctafor where codfor = '" & FormaDePago & "' and id_emp = '" & jytsistema.WorkID & "' ")
        CondicionPagoProveedorCliente = 1
        If Periodo <> 0 Then CondicionPagoProveedorCliente = 0

    End Function
    Public Function DiasCreditoAlVencimiento(ByVal MyConn As MySqlConnection, ByVal FormaDePago As String) As Integer
        DiasCreditoAlVencimiento = ft.DevuelveScalarEntero(MyConn, " select periodo from jsconctafor where codfor = '" & FormaDePago & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Function

    Public Function FechaVencimientoFactura(MyConn As MySqlConnection, CodigoCliente As String, FechaEmision As Date) As Date
        Dim FormaDePagoCliente As String = ft.DevuelveScalarCadena(MyConn, " select formapago from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim FechaFinal As Date = DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(MyConn, FormaDePagoCliente), FechaEmision)
        Dim diasNoHabiles As Integer = DiasNoHabilesEnPeriodo(MyConn, FechaEmision, FechaFinal)

        FechaVencimientoFactura = DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(MyConn, FormaDePagoCliente) + _
                                          IIf(CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA26")), diasNoHabiles, 0), FechaEmision)
    End Function

    Function ClientePoseeChequesFuturos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCliente As String) As Boolean

        Return ft.DevuelveScalarBooleano(MyConn, " select count(*) from jsbantracaj " _
            & " where " _
            & " tipomov = 'EN' and " _
            & " formpag = 'CH' and " _
            & " fecha > '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " deposito = '' and " _
            & " prov_cli = '" & CodigoCliente & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' ")

    End Function

    Public Sub CargaListViewAsesores(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("Código", 60, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre asesor comercial", 150, HorizontalAlignment.Left)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(dt.Rows(iCont).Item("codven").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nombre").ToString)
        Next

        LV.EndUpdate()

    End Sub

    Public Sub CargarListaSeleccionAsesores(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                            Optional ByVal EncabezadoFila As Boolean = True)

        Dim aFld() As String = {"sel", "codven", "nombre"}
        Dim aNom() As String = {"", "Código", "Nombre"}
        Dim aAnc() As Integer = {20, 60, 150}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", ""}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = True
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargarListaSeleccionFacturasGuiaDespacho(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                            Optional ByVal EncabezadoFila As Boolean = True, Optional TipoRelacion As Integer = 0)

        'TIPO RELACION    0 = FACTURAS ; 1 = NCR ; 2 = PEDIDOS

        Dim aFld() As String = {"sel", "numfac", "emision", "codcli", "nombre", IIf(TipoRelacion = 0, "tot_fac", IIf(TipoRelacion = 1, "tot_ncr", "tot_ped")), "kilos", "codven", "comen"}
        Dim aNom() As String = {"", "N° Factura", "Emisión", "Cliente", "Nombre y/o Razón social", "Total Documento", "Peso (Kgr)", "Asesor", "Grupo"}
        Dim aAnc() As Integer = {20, 80, 60, 90, 240, 70, 70, 50, 50}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", sFormatoFechaCorta, "", "", sFormatoNumero, sFormatoCantidad, "", ""}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = True
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargarListaSeleccionRetencionesIVA(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                           Optional ByVal EncabezadoFila As Boolean = True)

        Dim aFld() As String = {"sel", "nummov", "tipomov", "num_control", "emision", "vence", "importe", "impiva", "retimpiva"}
        Dim aNom() As String = {"", "N° Documento", "TP", "N° Control", "Emisión", "Vence", "Importe Inicial", "Importe IVA", "Retención Importe IVA"}
        Dim aAnc() As Integer = {20, 90, 40, 90, 90, 90, 90, 90, 90}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                 AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
        Dim aFor() As String = {"", "", "", "", sFormatoFechaCorta, sFormatoFechaCorta, sFormatoNumero, sFormatoNumero, sFormatoNumero}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = False
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 And col.Index < 8 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargarListaSeleccionMercas(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                            Optional ByVal EncabezadoFila As Boolean = True)

        Dim aFld() As String = {"sel", "codart", "nomart", "alterno"}
        Dim aNom() As String = {"", "Código", "Nombre", "Alterno"}
        Dim aAnc() As Integer = {20, 100, 450, 100}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", "", ""}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = True
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargaListViewPreCobranza(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("Recibo N°", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Cliente", 60, HorizontalAlignment.Left)
        LV.Columns.Add("Documento N°", 80, HorizontalAlignment.Left)
        LV.Columns.Add("TP", 30, HorizontalAlignment.Center)
        LV.Columns.Add("Concepto", 190, HorizontalAlignment.Left)
        LV.Columns.Add("Importe", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Referencia", 90, HorizontalAlignment.Left)
        LV.Columns.Add("N° Pago", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre pago", 90, HorizontalAlignment.Left)
        LV.Columns.Add("FP", 30, HorizontalAlignment.Center)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(dt.Rows(iCont).Item("comproba").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("codcli").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nummov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("tipomov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("concepto").ToString)
            LV.Items(iCont).SubItems.Add(ft.FormatoNumero(CDbl(dt.Rows(iCont).Item("importe").ToString)))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("refer").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("numpag").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nompag").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("formapag").ToString)

        Next

        LV.EndUpdate()

    End Sub


    Public Sub CargaListViewPrepedidos(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Prepedido", 100, HorizontalAlignment.Left)
        LV.Columns.Add("Cliente", 80, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre ó Razón social", 240, HorizontalAlignment.Left)
        LV.Columns.Add("Emisión", 60, HorizontalAlignment.Center)
        LV.Columns.Add("Total", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Peso (Kgr.)", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Grupo", 70, HorizontalAlignment.Left)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(dt.Rows(iCont).Item("numped").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("codcli").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nombre").ToString)
            LV.Items(iCont).SubItems.Add(ft.FormatoFecha(CDate(dt.Rows(iCont).Item("emision").ToString)))
            LV.Items(iCont).SubItems.Add(ft.FormatoNumero(CDbl(dt.Rows(iCont).Item("tot_ped").ToString)))
            LV.Items(iCont).SubItems.Add(ft.FormatoCantidad(CDbl(dt.Rows(iCont).Item("kilos").ToString)))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("comen").ToString)
        Next

        LV.EndUpdate()

    End Sub


    Public Sub CargaListViewFacturasGuiaDespacho(ByVal LV As ListView, ByVal dt As DataTable, Optional ByVal TipoRelacion As Integer = 0)

        'TIPO RELACION    0 = FACTURAS ; 1 = NCR ; 2 = PEDIDOS

        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Factura", 80, HorizontalAlignment.Left)
        LV.Columns.Add("Emisión", 60, HorizontalAlignment.Center)
        LV.Columns.Add("Cliente", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre ó Razón social", 240, HorizontalAlignment.Left)
        LV.Columns.Add("Total", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Peso (Kgr.)", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Asesor", 40, HorizontalAlignment.Center)
        LV.Columns.Add("Grupo", 40, HorizontalAlignment.Center)

        For iCont = 0 To dt.Rows.Count - 1

            With dt.Rows(iCont)
                LV.Items.Add(.Item("numfac").ToString)
                LV.Items(iCont).SubItems.Add(ft.FormatoFecha(CDate(.Item("emision").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codcli").ToString)
                LV.Items(iCont).SubItems.Add(.Item("nombre").ToString)
                Select Case TipoRelacion
                    Case 0
                        LV.Items(iCont).SubItems.Add(ft.FormatoNumero(CDbl(.Item("tot_fac").ToString)))
                    Case 1
                        LV.Items(iCont).SubItems.Add(ft.FormatoNumero(CDbl(.Item("tot_ncr").ToString)))
                    Case 2
                        LV.Items(iCont).SubItems.Add(ft.FormatoNumero(CDbl(.Item("tot_PED").ToString)))
                End Select

                LV.Items(iCont).SubItems.Add(ft.FormatoCantidad(CDbl(.Item("kilos").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codven").ToString)
                LV.Items(iCont).SubItems.Add(.Item("comen").ToString)
            End With
        Next

        LV.EndUpdate()

    End Sub



    Public Sub CargaListViewImprimirFacturasGuiaDespacho(ByVal LV As ListView, ByVal dt As DataTable)

        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Factura", 80, HorizontalAlignment.Left)
        LV.Columns.Add("Emisión", 60, HorizontalAlignment.Center)
        LV.Columns.Add("Cliente", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre ó Razón social", 240, HorizontalAlignment.Left)
        LV.Columns.Add("Total", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Peso (Kgr.)", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Asesor", 40, HorizontalAlignment.Center)


        For iCont = 0 To dt.Rows.Count - 1

            With dt.Rows(iCont)
                LV.Items.Add(.Item("codigofac").ToString)
                LV.Items(iCont).SubItems.Add(ft.FormatoFecha(CDate(.Item("emision").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codcli").ToString)
                LV.Items(iCont).SubItems.Add(.Item("nomcli").ToString)
                LV.Items(iCont).SubItems.Add(ft.FormatoNumero(CDbl(.Item("totalfac").ToString)))
                LV.Items(iCont).SubItems.Add(ft.FormatoCantidad(CDbl(.Item("kilosfac").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codven").ToString)
            End With
        Next

        LV.EndUpdate()

    End Sub

    Public Sub CargarListaSaldosDocumentosCliente(ByVal dg As DataGridView, ByVal dtdepos As DataTable)

        Dim aFld() As String = {"sel", "nummov", "tipomov", "emision", "vence", "importe", "saldo", "codven", "nomVendedor"}
        Dim aNom() As String = {"", "Documento", "TP", "Emisión", "Vence", "Importe", "Saldo", "", "Asesor"}
        Dim aAnc() As Integer = {20, 110, 35, 90, 90, 110, 110, 45, 110}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                     AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                     AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", "", sFormatoFechaCorta, sFormatoFechaCorta, sFormatoNumero, sFormatoNumero, "", ""}


        IniciarTablaSeleccion(dg, dtdepos, aFld, aNom, aAnc, aAli, aFor, , True)


    End Sub



    Public Sub CargaLVSaldosDocumentoCliente(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Documento", 120, HorizontalAlignment.Left)
        LV.Columns.Add("Tipo", 50, HorizontalAlignment.Center)
        LV.Columns.Add("Emisión", 90, HorizontalAlignment.Center)
        LV.Columns.Add("Vence", 90, HorizontalAlignment.Center)
        LV.Columns.Add("Importe", 90, HorizontalAlignment.Right)
        LV.Columns.Add("Saldo", 90, HorizontalAlignment.Right)
        LV.Columns.Add("Asesor", 50, HorizontalAlignment.Center)
        LV.Columns.Add("Nombre Asesor", 200, HorizontalAlignment.Left)

        For iCont = 0 To dt.Rows.Count - 1
            With dt.Rows(iCont)
                LV.Items.Add(.Item("nummov").ToString)
                LV.Items(iCont).SubItems.Add(.Item("tipomov").ToString)
                LV.Items(iCont).SubItems.Add(ft.FormatoFecha(CDate(.Item("emision").ToString)))
                LV.Items(iCont).SubItems.Add(ft.FormatoFecha(CDate(.Item("vence").ToString)))
                LV.Items(iCont).SubItems.Add(ft.FormatoNumero(CDbl(.Item("importe").ToString)))
                LV.Items(iCont).SubItems.Add(ft.FormatoCantidad(CDbl(.Item("saldo").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codven").ToString)
                LV.Items(iCont).SubItems.Add(.Item("nomVendedor").ToString)
            End With

        Next

        LV.EndUpdate()

    End Sub
    Public Sub ActualizarRenglonesEnPresupuestos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal dtRenglon As DataTable, _
                                                 ByVal nombreTablaOrigen As String)

        Dim NumeroPresupuesto As String
        Dim sItem As String
        Dim RenglonPresupuesto As String
        Dim gCont As Integer

        For gCont = 0 To dtRenglon.Rows.Count - 1
            With dtRenglon.Rows(gCont)
                NumeroPresupuesto = IIf(Not IsDBNull(.Item("NUMCOT")), .Item("NUMCOT"), "")
                RenglonPresupuesto = IIf(Not IsDBNull(.Item("RENCOT")), .Item("RENCOT"), "")
                sItem = .Item("ITEM")
                If NumeroPresupuesto <> "" Then
                    If .Item("ESTATUS") < 2 Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrencot", "NUMCOT", NumeroPresupuesto, _
                                                           RenglonPresupuesto, sItem, .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenenccot", "jsvenrencot", "NUMCOT", NumeroPresupuesto)
                    End If
                End If
            End With
        Next

    End Sub
    Public Sub ActualizarRenglonesEnPrepedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                           ByVal dtRenglon As DataTable, ByVal sTablaOrigen As String)

        Dim NumeroPedido As String
        Dim RenglonPedido As String

        If dtRenglon.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtRenglon.Rows.Count - 1
                With dtRenglon.Rows(iCont)
                    NumeroPedido = IIf(Not IsDBNull(.Item("numped")), .Item("numped"), "")
                    RenglonPedido = IIf(Not IsDBNull(.Item("renped")), .Item("renped"), "")
                    If NumeroPedido <> "" Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrenpedrgv", "NUMPED", NumeroPedido, _
                                                           RenglonPedido, .Item("item"), .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenencpedrgv", "jsvenrenpedrgv", "NUMPED", NumeroPedido)
                    End If
                End With
            Next
        End If

    End Sub

    Public Sub ActualizarRenglonesEnPedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                            ByVal dtRenglon As DataTable, ByVal sTablaOrigen As String)

        Dim NumeroPedido As String
        Dim RenglonPedido As String

        If dtRenglon.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtRenglon.Rows.Count - 1
                With dtRenglon.Rows(iCont)
                    NumeroPedido = IIf(Not IsDBNull(.Item("numped")), .Item("numped"), "")
                    RenglonPedido = IIf(Not IsDBNull(.Item("renped")), .Item("renped"), "")
                    If NumeroPedido <> "" Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrenped", "NUMPED", NumeroPedido, _
                                                           RenglonPedido, .Item("item"), .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenencped", "jsvenrenped", "NUMPED", NumeroPedido)
                    End If
                End With
            Next
        End If

    End Sub

    Public Sub ActualizarRenglonesEnNotasDeEntrega(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                        ByVal dtRenglon As DataTable, ByVal sTablaOrigen As String)

        Dim NumeroNotaDeEntrega As String
        Dim RenglonNotaDeEntrega As String

        If dtRenglon.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtRenglon.Rows.Count - 1
                With dtRenglon.Rows(iCont)
                    NumeroNotaDeEntrega = IIf(Not IsDBNull(.Item("numnot")), .Item("numnot"), "")
                    RenglonNotaDeEntrega = IIf(Not IsDBNull(.Item("rennot")), .Item("rennot"), "")
                    If NumeroNotaDeEntrega <> "" Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrennot", "NUMFAC", NumeroNotaDeEntrega, _
                                                           RenglonNotaDeEntrega, .Item("item"), .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenencnot", "jsvenrennot", "NUMFAC", NumeroNotaDeEntrega)
                    End If
                End With
            Next
        End If

    End Sub


    Public Sub ActualizaCantidadTransitoEnRenglon(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                                  ByVal NombreTablaEnBD As String, ByVal CampoDocumentoEnBD As String, ByVal NumeroDocumento As String, _
                                                  ByVal NumeroRenglon As String, ByVal Item As String, ByVal Cantidad As Double, _
                                                  ByVal UnidadOrigen As String, ByVal Estatus As String)


        Dim dtAct As DataTable
        Dim nTablaAct As String = "tblAct"
        Dim CantidadReal As Double

        ds = DataSetRequery(ds, " select * from " & NombreTablaEnBD & " Where " _
                  & " " & CampoDocumentoEnBD & " = '" & NumeroDocumento & "' AND " _
                  & " RENGLON = '" & NumeroRenglon & "' AND " _
                  & " ITEM = '" & Item & "' AND " _
                  & " ESTATUS = '" & Estatus & "' AND " _
                  & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                  & " ID_EMP = '" & jytsistema.WorkID & "' ", MyConn, nTablaAct, lblInfo)

        dtAct = ds.Tables(nTablaAct)

        If dtAct.Rows.Count > 0 Then

            CantidadReal = ConversiondeUnidades(MyConn, lblInfo, ds, Item, UnidadOrigen, Cantidad, dtAct.Rows(0).Item("UNIDAD"))

            ft.Ejecutar_strSQL(MyConn, _
                " UPDATE " & NombreTablaEnBD & " set " _
                & " CANTRAN =  CANTIDAD - " & CantidadReal & " WHERE " _
                & " " & CampoDocumentoEnBD & " = '" & NumeroDocumento & "' AND " _
                & " RENGLON = '" & NumeroRenglon & "' AND " _
                & " ITEM = '" & Item & "' AND " _
                & " ESTATUS = '" & Estatus & "' AND " _
                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

        dtAct.Dispose()
        dtAct = Nothing

    End Sub

    Function CarteraMercancias(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoAsesor As String) As Integer
        CarteraMercancias = ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvencuoart where codven = '" & CodigoAsesor & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Function
    Function CarteraGrupo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoAsesor As String, _
                          ByVal Grupo As String) As Integer

        Return ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(DISTINCT b." & Grupo & ") " _
                                                      & " FROM jsvencuoart a " _
                                                      & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
                                                      & " WHERE " _
                                                      & " a.codven = '" & CodigoAsesor & "' AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "'")

    End Function
    Public Sub ActualizaEstatusDocumento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                       ByVal sTablaEncabezado As String, ByVal sTablaRenglones As String, ByVal Documento As String, ByVal NumeroDocumento As String)

        Dim dtEst As DataTable
        Dim nTablaEstatus As String = "tblEstatus"

        ds = DataSetRequery(ds, " SELECT SUM(IF(CANTRAN > 0, CANTRAN,0)) AS SUMA FROM " & sTablaRenglones & " WHERE" _
                    & " " & Documento & " = '" & NumeroDocumento & " ' AND " _
                    & " ACEPTADO = '1' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' " _
                    & " GROUP BY " & Documento & " ", MyConn, nTablaEstatus, lblInfo)
        dtEst = ds.Tables(nTablaEstatus)

        If dtEst.Rows.Count > 0 Then
            With dtEst.Rows(0)
                If .Item("SUMA") > 0 Then
                    ft.Ejecutar_strSQL(MyConn, " UPDATE " & sTablaEncabezado & " set ESTATUS = '0' where " _
                        & " " & Documento & " = '" & NumeroDocumento & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")
                Else
                    ft.Ejecutar_strSQL(MyConn, " UPDATE " & sTablaEncabezado & " set ESTATUS = '1' where " _
                        & " " & Documento & " = '" & NumeroDocumento & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")
                End If
            End With
        Else
            ft.Ejecutar_strSQL(MyConn, " UPDATE " & sTablaEncabezado & " set ESTATUS = '1' where " _
              & " " & Documento & " = '" & NumeroDocumento & "' AND " _
              & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
              & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

        dtEst.Dispose()
        dtEst = Nothing

    End Sub

    Function NumeroLineasEnNotaCredito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                  ByVal NumeroNotaCredito As String) As Integer
        'Lineas para comienzo de factura 
        NumeroLineasEnNotaCredito = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA18"))

        'Lineas de Encabezado de Factura
        NumeroLineasEnNotaCredito += 9
        'Número de Renglones
        NumeroLineasEnNotaCredito += 2 * ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenncr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ")

        'Subtotal
        NumeroLineasEnNotaCredito += 2

        'Número renglones de iva
        NumeroLineasEnNotaCredito += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenivancr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ")

        'Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnNotaCredito += 5

        'Comentarios x Factura
        NumeroLineasEnNotaCredito += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsconctacom where origen = 'NCR' and id_emp = '" & jytsistema.WorkID & "' ")

    End Function



    Function NumeroLineasEnFactura(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                   ByVal NumeroFactura As String) As Integer
        'Lineas para comienzo de factura 
        NumeroLineasEnFactura = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA18"))
        'Lineas de Encabezado de Factura
        NumeroLineasEnFactura += 9
        'Número de Renglones
        NumeroLineasEnFactura += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")

        'Comentarios de Renglones 
        NumeroLineasEnFactura += NumeroLineasPorComentarios(MyConn, lblInfo, NumeroFactura, "FAC")
        'Cantidad Mercancias de combo + 1 

        'Subtotal
        NumeroLineasEnFactura += 2
        'Descuentos (+2)
        NumeroLineasEnFactura += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvendesfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
        NumeroLineasEnFactura += 2
        'Número renglones de iva
        NumeroLineasEnFactura += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenivafac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")

        'Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnFactura += 5

        'Comentarios x Factura
        NumeroLineasEnFactura += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsconctacom where origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")

    End Function
    Function NumeroLineasEnFacturaPOS(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                  ByVal NumeroFactura As String) As Integer
        'Lineas para comienzo de factura 
        NumeroLineasEnFacturaPOS = CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM29"))
        'Lineas de Encabezado de Factura
        NumeroLineasEnFacturaPOS += 9
        'Número de Renglones
        NumeroLineasEnFacturaPOS += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")

        'Comentarios de Renglones 
        NumeroLineasEnFacturaPOS += NumeroLineasPorComentarios(MyConn, lblInfo, NumeroFactura, "PVE")
        'Cantidad Mercancias de combo + 1 

        'Subtotal
        NumeroLineasEnFacturaPOS += 2
        'Descuentos (+2)
        NumeroLineasEnFacturaPOS += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvendespos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
        NumeroLineasEnFacturaPOS += 2
        'Número renglones de iva
        NumeroLineasEnFacturaPOS += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenivapos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")

        'Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnFacturaPOS += 5

        'Comentarios x Factura
        NumeroLineasEnFacturaPOS += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsconctacom where origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ")

    End Function

    Function NumeroLineasEnNotasDebito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                   ByVal NumeroNotaDebito As String) As Integer
        'Lineas para comienzo de Nota Debito 
        NumeroLineasEnNotasDebito = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA20"))

        'Lineas de Encabezado de Nota Debito
        NumeroLineasEnNotasDebito += 9

        ''Número de Renglones
        NumeroLineasEnNotasDebito += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenndb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' ")

        ''Comentarios de Renglones 
        NumeroLineasEnNotasDebito += NumeroLineasPorComentarios(MyConn, lblInfo, NumeroNotaDebito, "NDB", 80)

        ''Subtotal
        NumeroLineasEnNotasDebito += 2

        ''Número renglones de iva
        NumeroLineasEnNotasDebito += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenivandb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' ")

        ''Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnNotasDebito += 5

        ''Comentarios x Nota Debito
        NumeroLineasEnNotasDebito += ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsconctacom where origen = 'NDB' and id_emp = '" & jytsistema.WorkID & "' ")

    End Function


    Function NumeroLineasPorComentarios(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                          ByVal NumeroDocumento As String, ByVal Origen As String, Optional ByVal numCaracteresEnLinea As Integer = 60) As Integer

        'Para imprimir comentarios de renglones se partiran los mismos en cadenas de 60 caracteres
        NumeroLineasPorComentarios = 0
        Dim dsCom As New DataSet
        Dim nTableCom As String = "tblComentario"
        Dim dtCom As DataTable

        dsCom = DataSetRequery(dsCom, " select * from jsvenrencom where origen = '" & Origen & "' and numdoc = '" & NumeroDocumento & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTableCom, lblInfo)
        dtCom = dsCom.Tables(nTableCom)

        Dim iCont As Integer
        For iCont = 0 To dtCom.Rows.Count - 1
            With dtCom.Rows(iCont)
                Dim nLineaAdicional As Double = CStr(.Item("comentario")).Length Mod numCaracteresEnLinea
                Dim nComens As Integer = Int(CStr(.Item("comentario")).Length / numCaracteresEnLinea) + IIf(nLineaAdicional = 0, 0, 1)
                NumeroLineasPorComentarios += nComens
            End With
        Next

        dsCom.Dispose()
        dsCom = Nothing

    End Function


    Function ClienteFacturaAPartirDeCostos(MyConn As MySqlConnection, lblInfo As Label, CodigoCliente As String) As Boolean
        Return ft.DevuelveScalarBooleano(MyConn, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Function


    Public Sub VerificaVencimientoCliente(MyConn As MySqlConnection, lblInfo As Label, ds As DataSet, CodigoCliente As String, Estatus As String, _
                                   DiasVencimiento As Integer, CausaBloqueo As String, NoFacturarAgain As Boolean)

        Dim dts As DataTable
        Dim nTable As String = "tblFActurasPendientes"
        ds = DataSetRequery(ds, "SELECT a.CODCLI, b.NOMBRE, a.NUMMOV, a.TIPOMOV, a.REFER, " _
            & " a.EMISION, a.VENCE, a.IMPORTE, IFNULL(SUM(a.IMPORTE),0) AS SALDO, " _
            & " to_days('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') - to_Days(a.vence) as DV, " _
            & " if( to_days('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') - to_Days(a.vence)>= " & DiasVencimiento & ", 1, 0) as lapso " _
            & " from jsventracob a LEFT JOIN jsvencatcli b ON  b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' Where a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " and a.tipomov <> 'ND' " _
            & " and a.codcli = '" & CodigoCliente & "' " _
            & " GROUP BY a.codcli, a.nummov Having Saldo > 1 AND a.vence < '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and lapso = 1 " _
            & " ORDER BY LAPSO, a.CODCLI, NUMMOV, EMISION ASC ", MyConn, nTable, lblInfo)


        dts = ds.Tables(nTable)

        If dts.Rows.Count > 0 Then
            If Estatus = "0" Then
                InsertEditVENTASExpedienteCliente(MyConn, lblInfo, True, CodigoCliente, CDate(ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & " " & ft.FormatoHora(Now())), _
                                                  "FACTURAS PENDIENTES", 1, CausaBloqueo, 0)

                ft.Ejecutar_strSQL(MyConn, " update jsvencatcli set estatus = '1' where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
            End If

        End If

        If NoFacturarAgain Then
            ft.Ejecutar_strSQL(MyConn, " update jsvencatcli set backorder = '0' where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        End If

        dts.Dispose()
        dts = Nothing

    End Sub
    Public Function ProcesoAutomaticoInicial(MyConn As MySqlConnection, lblInfo As Label, Proceso As Integer, Fecha As Date) As Boolean

        Return ft.DevuelveScalarBooleano(MyConn, " select 1 from  jscontrapro where  " _
                                           & " proceso = '" & Format(Proceso, "00") & "' and " _
                                           & " fecha = '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

    End Function

    Public Sub ActualizarProcesoAutomaticoInicial(MyConn As MySqlConnection, lblInfo As Label, proceso As ProcesoAutomatico, Fecha As Date)
        Dim proc As String = RellenaCadenaConCaracter(proceso, "D", 2, "0")
        ft.Ejecutar_strSQL(MyConn, " insert into jscontrapro set proceso = '" & proc & "', fecha = '" & ft.FormatoFechaMySQL(Fecha) & "', id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Public Function NumeroDeClientesAVisitarMes(MyConn As MySqlConnection, lblInfo As Label, ds As DataSet, _
                                                CodigoAsesor As String, FechaInicial As Date, FechaFinal As Date) As Integer

        Dim nClientes As Integer = 0
        Dim Fecha As Date = FechaInicial
        While Fecha <= FechaFinal
            If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsconcatper where DATE(CONCAT(ano,'-',mes,'-',dia)) = '" & ft.FormatoFechaMySQL(Fecha) & "' and modulo = 1 and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                nClientes += ft.DevuelveScalarEntero(MyConn, " SELECT items FROM jsvenencrut " _
                                                    & " WHERE " _
                                                    & " CODVEN = '" & CodigoAsesor & "' AND " _
                                                    & " TIPO = 0 AND " _
                                                    & " DIA = " & Fecha.DayOfWeek - 1 & " AND " _
                                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
            End If
            Fecha = Fecha.AddDays(1)
        End While
        Return nClientes


    End Function

    Public Function ClientesAVisitarSupervisor(MyConn As MySqlConnection, lblInfo As Label, CodigoAsesor As String, Fecha As Date) As Integer
        Dim nVendedores As New ArrayList
        nVendedores = ft.Ejecutar_strSQL_DevuelveLista(MyConn, "SELECT codven FROM jsvencatven WHERE supervisor = '" & CodigoAsesor & "' and id_emp = '" & jytsistema.WorkID & "' ")

        ClientesAVisitarSupervisor = 0
        For Each nVen As Object In nVendedores
            ClientesAVisitarSupervisor += ft.DevuelveScalarEntero(MyConn, " select avisitar from jsvenenccie where codven = '" & nVen.ToString & "' and YEAR(FECHA) = " & Fecha.Year & " AND month(fecha) = " & Fecha.Month & " and id_emp = '" & jytsistema.WorkID & "' ")
        Next

    End Function

    Public Function AsesoresDeSupervisor(MyConn As MySqlConnection, lblInfo As Label, CodigoSupervisor As String, Optional TipoSupervisor As Integer = 1) As String

        Dim nVendedores As New ArrayList
        Select Case TipoSupervisor
            Case 1
                nVendedores = ft.Ejecutar_strSQL_DevuelveLista(MyConn, "SELECT codven FROM jsvencatven WHERE supervisor = '" & CodigoSupervisor & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Case Else
                nVendedores = ft.Ejecutar_strSQL_DevuelveLista(MyConn, "SELECT codven FROM jsvencatven WHERE clase = 0 AND tipo = 0 AND id_emp = '" & jytsistema.WorkID & "' ")
        End Select

        AsesoresDeSupervisor = " ( "
        For Each nVen As Object In nVendedores
            AsesoresDeSupervisor += "'" & nVen.ToString & "', "
        Next
        If Right(AsesoresDeSupervisor, 2) = ", " Then AsesoresDeSupervisor = Mid(AsesoresDeSupervisor, 1, Len(AsesoresDeSupervisor) - 2)
        AsesoresDeSupervisor += " ) "

        If AsesoresDeSupervisor = " (  ) " Then AsesoresDeSupervisor = ""

    End Function

    Function montoFletesPorRegion(MyConn As MySqlConnection, Cliente As String) As Double

        montoFletesPorRegion = 0.0

        Dim fiscalPais As String = ft.DevuelveScalarCadena(MyConn, " select FPAIS from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalEstado As String = ft.DevuelveScalarCadena(MyConn, " select FESTADO from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalMunicipio As String = ft.DevuelveScalarCadena(MyConn, " select FMUNICIPIO from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalParroquia As String = ft.DevuelveScalarCadena(MyConn, " select FPARROQUIA from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalCiudad As String = ft.DevuelveScalarCadena(MyConn, " select FCIUDAD from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalBarrio As String = ft.DevuelveScalarCadena(MyConn, " select FBARRIO from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim monto As Double = ft.DevuelveScalarDoble(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = '" & fiscalPais & "' ")
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = ft.DevuelveScalarDoble(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = '" & fiscalEstado & "' ")
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = ft.DevuelveScalarDoble(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = '" & fiscalMunicipio & "' ")
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = ft.DevuelveScalarDoble(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = '" & fiscalParroquia & "' ")
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = ft.DevuelveScalarDoble(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = '" & fiscalCiudad & "' ")
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = ft.DevuelveScalarDoble(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = '" & fiscalBarrio & "' ")
        If monto <> 0.0 Then montoFletesPorRegion = monto

    End Function

    Function CuotaAsesorMES_KGR(MyConn As MySqlConnection, CodigoAsesor As String, CodigoMercancia As String, Fecha As Date) As Double
        Return ft.DevuelveScalarDoble(MyConn, " SELECT  ESMES" + Format(Fecha.Month, "00") + " " _
                                           & " FROM jsvencuoart " _
                                           & " WHERE " _
                                           & " codart = '" & CodigoMercancia & "' AND  " _
                                           & " codven = '" & CodigoAsesor & "' AND  " _
                                           & " id_emp = '" & jytsistema.WorkID & "'")

    End Function


    Public Function EstatusClienteFacturacion(MyConn As MySqlConnection, CodigoCliente As String, MontoPedido As Double) As Integer

        ' 0 = Verde ; 1 = Amarillo ; 2 = Rojo
        ' AMARILLO : 1. Disponibilidad Insuficiente
        '            2. COMISION CHEQUE DEVUELTO SIN CANCELAR
        ' ROJO     : 1. BLOQUEADO

        If ft.DevuelveScalarEntero(MyConn, " select estatus from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then Return 2
        If ft.DevuelveScalarDoble(MyConn, " select disponible from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ") < MontoPedido Then Return 1

        If ft.DevuelveScalarDoble(MyConn, " SELECT sum(b.saldo) " _
            & " 		   FROM jsventracob a " _
            & "        	   LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo , a.id_emp " _
            & "                 		FROM jsventracob a " _
            & "                         WHERE " _
            & "                         a.codcli = '" & CodigoCliente & "' AND " _
            & " 		                a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) b ON (a.codcli = b.codcli AND a.nummov = b.nummov AND a.id_emp = b.id_emp)  " _
            & "            WHERE " _
            & "            b.saldo <> 0 AND " _
            & "            a.codcli = '" & CodigoCliente & "' AND " _
            & "            a.concepto ='COMISION CHEQUE DEVUELTO y GASTOS ADM.' AND " _
            & "            a.id_emp = '" & jytsistema.WorkID & "' group by a.codcli ") > 0 Then Return 1

        Return 0

    End Function


End Module
