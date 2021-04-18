Imports MySql.Data.MySqlClient
Module TablasCompras
    Public Sub InsertEditCOMProveedores(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                        ByVal CodigoProveedor As String, ByVal NombreProveedor As String, ByVal CATEGORIA As String, ByVal UNIDAD As String, _
                                        ByVal RIF As String, ByVal NIT As String, ByVal ASIGNADO As String, ByVal DireccionFiscal As String, ByVal DireccionAlterna As String, _
                                        ByVal EMAIL1 As String, ByVal EMAIL2 As String, ByVal EMAIL3 As String, ByVal EMAIL4 As String, ByVal EMAIL5 As String, _
                                        ByVal SITIOWEB As String, ByVal TELEF1 As String, ByVal TELEF2 As String, ByVal TELEF3 As String, ByVal FAX As String, _
                                        ByVal GERENTE As String, ByVal TELGER As String, ByVal CONTACTO As String, ByVal TELCON As String, ByVal LIMCREDITO As Double, _
                                        ByVal DISPONIBLE As Double, ByVal DESC1 As Double, ByVal DESC2 As Double, ByVal DESC3 As Double, ByVal DESC4 As Double, _
                                        ByVal DIAS2 As Integer, ByVal DIAS3 As Integer, ByVal OBSERVACION As String, ByVal ZONA As String, _
                                        ByVal COBRADOR As String, ByVal VENDEDOR As String, ByVal SALDO As Double, ByVal ULTPAGO As Double, _
                                        ByVal fecultpago As Date, ByVal FORULTPAGO As String, ByVal REGIMENIVA As String, ByVal FORMAPAGO As String, _
                                        ByVal BANCO As String, ByVal CTABANCO As String, ByVal BANCODEPOSITO1 As String, ByVal BANCODEPOSITO2 As String, _
                                        ByVal CTABANCODEPOSITO1 As String, ByVal CTABANCODEPOSITO2 As String, ByVal INGRESO As Date, ByVal CODCON As String, _
                                        ByVal ESTATUS As String, ByVal TIPO As Integer)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprocatpro SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprocatpro SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoProveedor, "codpro")
        strSQL += ModificarCadena(NombreProveedor, "nombre")
        strSQL += ModificarCadena(CATEGORIA, "categoria")
        strSQL += ModificarCadena(UNIDAD, "unidad")
        strSQL += ModificarCadena(RIF, "rif")
        strSQL += ModificarCadena(NIT, "nit")
        strSQL += ModificarCadena(ASIGNADO, "asignado")
        strSQL += ModificarCadena(DireccionFiscal, "dirfiscal")
        strSQL += ModificarCadena(DireccionAlterna, "dirprove")
        strSQL += ModificarCadena(EMAIL1, "email1")
        strSQL += ModificarCadena(EMAIL2, "email2")
        strSQL += ModificarCadena(EMAIL3, "email3")
        strSQL += ModificarCadena(EMAIL4, "sitioweb")
        strSQL += ModificarCadena(EMAIL5, "email5")
        strSQL += ModificarCadena(SITIOWEB, "email4")
        strSQL += ModificarCadena(TELEF1, "telef1")
        strSQL += ModificarCadena(TELEF2, "telef2")
        strSQL += ModificarCadena(TELEF3, "telef3")
        strSQL += ModificarCadena(FAX, "fax")
        strSQL += ModificarCadena(GERENTE, "gerente")
        strSQL += ModificarCadena(TELGER, "telger")
        strSQL += ModificarCadena(CONTACTO, "contacto")
        strSQL += ModificarCadena(TELCON, "telcon")
        strSQL += ModificarDoble(LIMCREDITO, "limcredito")
        strSQL += ModificarDoble(DISPONIBLE, "disponible")
        strSQL += ModificarDoble(DESC1, "desc1")
        strSQL += ModificarDoble(DESC2, "desc2")
        strSQL += ModificarDoble(DESC3, "desc3")
        strSQL += ModificarDoble(DESC4, "desc4")
        strSQL += ModificarEnteroLargo(DIAS2, "dias2")
        strSQL += ModificarEnteroLargo(DIAS3, "dias3")
        strSQL += ModificarCadena(OBSERVACION, "observacion")
        strSQL += ModificarCadena(ZONA, "zona")
        strSQL += ModificarCadena(COBRADOR, "cobrador")
        strSQL += ModificarCadena(VENDEDOR, "vendedor")
        strSQL += ModificarDoble(SALDO, "saldo")
        strSQL += ModificarDoble(ULTPAGO, "ultpago")
        strSQL += ModificarFecha(fecultpago, "fecultpago")
        If FORULTPAGO <> "" Then strSQL += ModificarCadena(FORULTPAGO, "forultpago")
        strSQL += ModificarCadena(REGIMENIVA, "regimeniva")
        strSQL += ModificarCadena(FORMAPAGO, "formapago")
        strSQL += ModificarCadena(BANCO, "banco")
        strSQL += ModificarCadena(CTABANCO, "ctabanco")
        strSQL += ModificarCadena(BANCODEPOSITO1, "bancodeposito1")
        strSQL += ModificarCadena(BANCODEPOSITO2, "bancodeposito2")
        strSQL += ModificarCadena(CTABANCODEPOSITO1, "ctabancodeposito1")
        strSQL += ModificarCadena(CTABANCODEPOSITO2, "ctabancodeposito2")
        strSQL += ModificarFecha(INGRESO, "ingreso")
        strSQL += ModificarCadena(CODCON, "codcon")
        strSQL += ModificarCadena(ESTATUS, "estatus")
        strSQL += ModificarEntero(TIPO, "tipo")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCOMPRASEncabezadoGasto(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroGastoAnterior As String, _
    ByVal NumeroGasto As String, ByVal NumeroSerieGasto As String, ByVal Emision As Date, ByVal EmisionIVA As Date, ByVal CodigoProveedorAnterior As String, _
    ByVal CodigoProveedor As String, ByVal NombreProveedor As String, _
    ByVal RIF As String, ByVal NIT As String, ByVal Comentario As String, ByVal Almacen As String, ByVal Referencia As String, ByVal CodigoContable As String, _
    ByVal Grupo As Integer, ByVal Subgrupo As Integer, ByVal TotalNeto As Double, ByVal PorcentajeDescuento As Double, _
    ByVal Descuento As Double, ByVal Cargos As Double, ByVal TipoIVA As String, ByVal PorcentajeIVA As Double, _
    ByVal BaseIVA As Double, ByVal ImpuestoIVA As Double, ByVal TotalGasto As Double, ByVal Vence As Date, _
    ByVal CondicionPago As Integer, ByVal TipoCredito As Integer, ByVal FormaPago As String, _
    ByVal NumeroPago As String, ByVal NombrePago As String, ByVal Beneficiario As String, ByVal Caja As String, _
    ByVal Abono As Double, ByVal Serie As String, ByVal NumeroGiros As Integer, ByVal PeriodoGiros As Integer, _
    ByVal Interes As Double, ByVal PorcentajeInteres As Double, ByVal Asiento As String, ByVal FechaAsiento As Date, _
    ByVal RetencionISLR As Double, ByVal NumeroCXP As String, ByVal OtraCXP As String, ByVal OtroProveedor As String, _
    ByVal Zona As String, ByVal Impresa As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsproencgas SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsproencgas SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numgas = '" & NumeroGastoAnterior & "' and " _
                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroGasto, "numgas")
        strSQL += ModificarCadena(NumeroSerieGasto, "serie_numgas")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarFecha(EmisionIVA, "EMISIONIVA")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(NombreProveedor, "NOMPRO")
        strSQL += ModificarCadena(RIF, "RIF")
        strSQL += ModificarCadena(NIT, "NIT")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarCadena(Almacen, "ALMACEN")
        strSQL += ModificarCadena(Referencia, "REFER")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarEnteroLargo(Grupo, "GRUPO")
        strSQL += ModificarEnteroLargo(Subgrupo, "SUBGRUPO")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(PorcentajeDescuento, "POR_DES")
        strSQL += ModificarDoble(Descuento, "DESCUEN")
        strSQL += ModificarDoble(Cargos, "CARGOS")
        strSQL += ModificarCadena(TipoIVA, "TIPOIVA")
        strSQL += ModificarDoble(PorcentajeIVA, "POR_IVA")
        strSQL += ModificarDoble(BaseIVA, "BASEIVA")
        strSQL += ModificarDoble(ImpuestoIVA, "IMP_IVA")
        strSQL += ModificarDoble(TotalGasto, "TOT_GAS")
        strSQL += ModificarFecha(Vence, "VENCE")
        strSQL += ModificarEntero(CondicionPago, "CONDPAG")
        strSQL += ModificarEntero(TipoCredito, "TIPOCREDITO")
        strSQL += ModificarCadena(FormaPago, "FORMAPAG")
        strSQL += ModificarCadena(NumeroPago, "NUMPAG")
        strSQL += ModificarCadena(NombrePago, "NOMPAG")
        strSQL += ModificarCadena(Beneficiario, "BENEFIC")
        strSQL += ModificarCadena(Caja, "CAJA")
        strSQL += ModificarDoble(Abono, "ABONO")
        strSQL += ModificarCadena(Serie, "SERIE")
        strSQL += ModificarEnteroLargo(NumeroGiros, "NUMGIRO")
        strSQL += ModificarEnteroLargo(PeriodoGiros, "PERGIRO")
        strSQL += ModificarDoble(Interes, "INTERES")
        strSQL += ModificarDoble(PorcentajeInteres, "PORINTERES")
        strSQL += ModificarCadena(Asiento, "ASIENTO")
        strSQL += ModificarFecha(FechaAsiento, "FECHASI")
        strSQL += ModificarDoble(RetencionISLR, "RET_ISLR")
        strSQL += ModificarCadena(NumeroCXP, "NUMCXP")
        strSQL += ModificarCadena(OtraCXP, "OTRA_CXP")
        strSQL += ModificarCadena(OtroProveedor, "OTRO_PRO")
        strSQL += ModificarCadena(Zona, "ZONA")
        strSQL += ModificarCadena(Impresa, "IMPRESA")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCOMPRASEncabezadoGastos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
    ByVal NumeroGasto As String, ByVal NumeroSerieGasto As String, ByVal Emision As Date, ByVal EmisionIVA As Date, _
    ByVal CodigoProveedor As String, ByVal NombreProveedor As String, _
    ByVal RIF As String, ByVal NIT As String, ByVal Comentario As String, ByVal Referencia As String, ByVal Almacen As String, _
    ByVal CodigoContable As String, ByVal Grupo As Integer, ByVal Subgrupo As Integer, ByVal TotalNeto As Double, ByVal PorcentajeDescuento As Double, _
    ByVal Descuento As Double, ByVal Cargos As Double, ByVal TipoIVA As String, ByVal PorcentajeIVA As Double, _
    ByVal BaseIVA As Double, ByVal ImpuestoIVA As Double, ByVal ImpuestoICS As Double, ByVal RetencionIVA As Double, _
    ByVal NumeroRetencionIVA As String, ByVal FEchaRetencionIVA As Date, ByVal TotalGasto As Double, ByVal Vence As Date, _
    ByVal CondicionPago As Integer, ByVal TipoCredito As Integer, ByVal FormaPago As String, _
    ByVal NumeroPago As String, ByVal NombrePago As String, ByVal Beneficiario As String, ByVal Caja As String, _
    ByVal Abono As Double, ByVal Serie As String, ByVal NumeroGiros As Integer, ByVal PeriodoGiros As Integer, _
    ByVal Interes As Double, ByVal PorcentajeInteres As Double, ByVal Asiento As String, ByVal FechaAsiento As Date, _
    ByVal RetencionISLR As Double, ByVal NumeroRetencionISLR As String, ByVal FechaRetencionISLR As Date, ByVal PorcentajeRetencionISLR As Double, _
    ByVal BaseRetencionISLR As Double, ByVal NumeroCXP As String, ByVal OtraCXP As String, ByVal OtroProveedor As String, _
    ByVal Zona As String, ByVal Impresa As String, ByVal NumeroGastoAnterior As String, ByVal CodigoProveedorAnterior As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsproencgas SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsproencgas SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numgas = '" & NumeroGastoAnterior & "' and " _
                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroGasto, "numgas")
        strSQL += ModificarCadena(NumeroSerieGasto, "serie_numgas")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarFecha(EmisionIVA, "EMISIONIVA")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(NombreProveedor, "NOMPRO")
        strSQL += ModificarCadena(RIF, "RIF")
        strSQL += ModificarCadena(NIT, "NIT")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarCadena(Almacen, "ALMACEN")
        strSQL += ModificarCadena(Referencia, "REFER")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarEnteroLargo(Grupo, "GRUPO")
        strSQL += ModificarEnteroLargo(Subgrupo, "SUBGRUPO")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(PorcentajeDescuento, "POR_DES")
        strSQL += ModificarDoble(Descuento, "DESCUEN")
        strSQL += ModificarDoble(Cargos, "CARGOS")
        strSQL += ModificarCadena(TipoIVA, "TIPOIVA")
        strSQL += ModificarDoble(PorcentajeIVA, "POR_IVA")
        strSQL += ModificarDoble(BaseIVA, "BASEIVA")
        strSQL += ModificarDoble(ImpuestoIVA, "IMP_IVA")
        strSQL += ModificarDoble(ImpuestoICS, "IMP_ICS")
        strSQL += ModificarDoble(RetencionIVA, "RET_IVA")
        strSQL += ModificarCadena(NumeroRetencionIVA, "NUM_RET_IVA")
        strSQL += ModificarFecha(FEchaRetencionIVA, "FECHA_RET_IVA")
        strSQL += ModificarDoble(TotalGasto, "TOT_GAS")
        strSQL += ModificarFecha(Vence, "VENCE")
        strSQL += ModificarEntero(CondicionPago, "CONDPAG")
        strSQL += ModificarEntero(TipoCredito, "TIPOCREDITO")
        strSQL += ModificarCadena(FormaPago, "FORMAPAG")
        strSQL += ModificarCadena(NumeroPago, "NUMPAG")
        strSQL += ModificarCadena(NombrePago, "NOMPAG")
        strSQL += ModificarCadena(Beneficiario, "BENEFIC")
        strSQL += ModificarCadena(Caja, "CAJA")
        strSQL += ModificarDoble(Abono, "ABONO")
        strSQL += ModificarCadena(Serie, "SERIE")
        strSQL += ModificarEnteroLargo(NumeroGiros, "NUMGIRO")
        strSQL += ModificarEnteroLargo(PeriodoGiros, "PERGIRO")
        strSQL += ModificarDoble(Interes, "INTERES")
        strSQL += ModificarDoble(PorcentajeInteres, "PORINTERES")
        strSQL += ModificarCadena(Asiento, "ASIENTO")
        strSQL += ModificarFecha(FechaAsiento, "FECHASI")
        strSQL += ModificarDoble(RetencionISLR, "RET_ISLR")
        strSQL += ModificarCadena(NumeroRetencionISLR, "NUM_RET_ISLR")
        strSQL += ModificarFecha(FechaRetencionISLR, "FECHA_RET_ISLR")
        strSQL += ModificarDoble(PorcentajeRetencionISLR, "POR_RET_ISLR")
        strSQL += ModificarDoble(BaseRetencionISLR, "BASE_RET_ISLR")
        strSQL += ModificarCadena(NumeroCXP, "NUMCXP")
        strSQL += ModificarCadena(OtraCXP, "OTRA_CXP")
        strSQL += ModificarCadena(OtroProveedor, "OTRO_PRO")
        strSQL += ModificarCadena(Zona, "ZONA")
        strSQL += ModificarCadena(Impresa, "IMPRESA")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCOMPRASEncabezadoOrdenes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                  ByVal NumeroDeOrden As String, ByVal NumeroSerieOrden As String, ByVal Emision As Date, ByVal Entrega As Date, _
                                                  ByVal CodigoProveedor As String, ByVal Comentario As String, ByVal TotalNeto As Double, _
                                                  ByVal ImporteIVA As Double, ByVal TotalOrden As Double, ByVal Estatus As String, _
                                                  ByVal Items As Integer, ByVal Impresa As String, ByVal CodigoProveedorAnterior As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsproencord SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsproencord SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numord = '" & NumeroDeOrden & "' and " _
                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroDeOrden, "NUMORD")
        strSQL += ModificarCadena(NumeroSerieOrden, "serie_numord")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarFecha(Entrega, "ENTREGA")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(ImporteIVA, "IMP_IVA")
        strSQL += ModificarDoble(TotalOrden, "TOT_ORD")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarEnterolargo(ITEMS, "ITEMS")
        strSQL += ModificarCadena(Impresa, "IMPRESA")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCOMPRASEncabezadoRecepciones(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                  ByVal NumeroRecepcion As String, ByVal NumeroSerieRecepcion As String, ByVal Emision As Date, _
                                                  ByVal CodigoProveedor As String, ByVal Comentario As String, ByVal Responsable As String, _
                                                  ByVal Almacen As String, ByVal TotalNeto As Double, ByVal ImporteIVA As Double, _
                                                  ByVal TotalRecepcion As Double, ByVal Estatus As String, ByVal NumeroDeCompra As String, _
                                                  ByVal Items As Integer, ByVal Cajas As Double, ByVal Kilos As Double, ByVal Impresa As String, ByVal CodigoProveedorAnterior As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsproencrep SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsproencrep SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numrec = '" & NumeroRecepcion & "' and " _
                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroRecepcion, "NUMREC")
        strSQL += ModificarCadena(NumeroSerieRecepcion, "serie_numrec")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarCadena(Responsable, "RESPONSABLE")
        strSQL += ModificarCadena(Almacen, "ALMACEN")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(ImporteIVA, "IMP_IVA")
        strSQL += ModificarDoble(TotalRecepcion, "TOT_REC")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarCadena(NumeroDeCompra, "NUMCOM")
        strSQL += ModificarEnterolargo(ITEMS, "ITEMS")
        strSQL += ModificarDoble(Cajas, "CAJAS")
        strSQL += ModificarDoble(Kilos, "KILOS")
        strSQL += ModificarCadena(Impresa, "IMPRESA")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "ID_EMP")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCOMPRASEncabezadoCompras(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                    ByVal NumeroCompra As String, ByVal NumeroSerieCompra As String, ByVal Emision As Date, ByVal EmisionIVA As Date, _
                                                    ByVal CodigoProveedor As String, ByVal Comentario As String, ByVal Almacen As String, _
                                                    ByVal Referencia As String, ByVal CodigoContable As String, ByVal Items As Integer, ByVal Cajas As Double, ByVal Kilos As Double, _
                                                    ByVal TotalNeto As Double, ByVal PorcentajeDescuento As Double, ByVal Descuento As Double, ByVal Cargos As Double, _
                                                    ByVal TotalCompra As Double, ByVal porDescuento1 As Double, ByVal porDescuento2 As Double, ByVal porDescuento3 As Double, ByVal porDescuento4 As Double, _
                                                    ByVal Descuento1 As Double, ByVal Descuento2 As Double, ByVal Descuento3 As Double, ByVal Descuento4 As Double, _
                                                    ByVal Vence1 As Date, ByVal Vence2 As Date, ByVal Vence3 As Date, ByVal Vence4 As Date, _
                                                    ByVal CondicionPago As Integer, ByVal TipoCredito As Integer, ByVal FormaPago As String, _
                                                    ByVal NumeroDePagp As String, ByVal NombreDePago As String, ByVal Beneficiario As String, ByVal Caja As String, _
                                                    ByVal Abono As Double, ByVal Serie As String, ByVal NumeroGiros As Integer, _
                                                    ByVal PeriodoEntreGiros As String, ByVal Interes As Double, ByVal porInteres As Double, _
                                                    ByVal Asiento As String, ByVal FechAsiento As Date, _
                                                    ByVal ImporteIVA As Double, _
                                                    ByVal ImporteICS As Double, ByVal RetencionIVA As Double, ByVal NumRetencionIVA As String, ByVal FechaRetencionIVA As Date, _
                                                    ByVal RetencionISLR As Double, ByVal NumRetencionISLR As String, ByVal FechaRetencionISLR As Date, ByVal porRetencionISLR As Double, _
                                                    ByVal BaseRetencionISLR As Double, ByVal NumeroPorPagar As String, ByVal OtraCuentaPorPagar As String, ByVal OtroProveedor As String, _
                                                    ByVal Impresa As String, ByVal CodigoProveedorAnterior As String, ByVal NumerodeCompraAnterior As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsproenccom SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsproenccom SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numcom = '" & NumerodeCompraAnterior & "' and " _
                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroCompra, "NUMCOM")
        strSQL += ModificarCadena(NumeroSerieCompra, "serie_numcom")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarFecha(EmisionIVA, "EMISIONIVA")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarCadena(Almacen, "ALMACEN")
        strSQL += ModificarCadena(Referencia, "REFER")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarEnterolargo(ITEMS, "ITEMS")
        strSQL += ModificarDoble(Cajas, "CAJAS")
        strSQL += ModificarDoble(Kilos, "KILOS")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(PorcentajeDescuento, "POR_DES")
        strSQL += ModificarDoble(Descuento, "DESCUEN")
        strSQL += ModificarDoble(Cargos, "CARGOS")
        strSQL += ModificarDoble(TotalCompra, "TOT_COM")
        strSQL += ModificarDoble(porDescuento1, "POR_DES1")
        strSQL += ModificarDoble(porDescuento2, "POR_DES2")
        strSQL += ModificarDoble(porDescuento3, "POR_DES3")
        strSQL += ModificarDoble(porDescuento4, "POR_DES4")
        strSQL += ModificarDoble(Descuento1, "DESCUEN1")
        strSQL += ModificarDoble(Descuento2, "DESCUEN2")
        strSQL += ModificarDoble(Descuento3, "DESCUEN3")
        strSQL += ModificarDoble(Descuento4, "DESCUEN4")
        strSQL += ModificarFecha(Vence1, "VENCE1")
        strSQL += ModificarFecha(Vence2, "VENCE2")
        strSQL += ModificarFecha(Vence3, "VENCE3")
        strSQL += ModificarFecha(Vence4, "VENCE4")
        strSQL += ModificarEntero(CondicionPago, "CONDPAG")
        strSQL += ModificarEntero(TipoCredito, "TIPOCREDITO")
        strSQL += ModificarCadena(FormaPago, "FORMAPAG")
        strSQL += ModificarCadena(NumeroDePagp, "NUMPAG")
        strSQL += ModificarCadena(NombreDePago, "NOMPAG")
        strSQL += ModificarCadena(Beneficiario, "BENEFIC")
        strSQL += ModificarCadena(Caja, "CAJA")
        strSQL += ModificarDoble(Abono, "ABONO")
        strSQL += ModificarCadena(Serie, "SERIE")
        strSQL += ModificarEnteroLargo(NumeroGiros, "NUMGIRO")
        strSQL += ModificarEnteroLargo(PeriodoEntreGiros, "PERGIRO")
        strSQL += ModificarDoble(Interes, "INTERES")
        strSQL += ModificarDoble(porInteres, "PORINTERES")
        strSQL += ModificarCadena(Asiento, "ASIENTO")
        strSQL += ModificarFecha(FechAsiento, "FECHASI")
        strSQL += ModificarDoble(ImporteIVA, "IMP_IVA")
        strSQL += ModificarDoble(ImporteICS, "IMP_ICS")
        strSQL += ModificarDoble(RetencionIVA, "RET_IVA")
        strSQL += ModificarCadena(NumRetencionIVA, "NUM_RET_IVA")
        strSQL += ModificarFecha(FechaRetencionIVA, "FECHA_RET_IVA")
        strSQL += ModificarDoble(RetencionISLR, "RET_ISLR")
        strSQL += ModificarCadena(NumRetencionISLR, "NUM_RET_ISLR")
        strSQL += ModificarFecha(FechaRetencionISLR, "FECHA_RET_ISLR")
        strSQL += ModificarDoble(porRetencionISLR, "POR_RET_ISLR")
        strSQL += ModificarDoble(BaseRetencionISLR, "BASE_RET_ISLR")
        strSQL += ModificarCadena(NumeroPorPagar, "NUMCXP")
        strSQL += ModificarCadena(OtraCuentaPorPagar, "OTRA_CXP")
        strSQL += ModificarCadena(OtroProveedor, "OTRO_PRO")
        strSQL += ModificarCadena(Impresa, "IMPRESA")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASEncabezadoNOTACREDITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                   ByVal NumeroNOTACREDITO As String, ByVal NumeroSerieNotaCredito As String, ByVal NumeroCompraAfectada As String, ByVal Emision As Date, ByVal EmisionIVA As Date, _
                                                   ByVal CodigoProveedor As String, ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Almacen As String, _
                                                   ByVal Transporte As String, ByVal Referencia As String, ByVal CodigoContable As String, ByVal Tarifa As String, _
                                                   ByVal Items As Integer, ByVal Cajas As Double, ByVal Kilos As Double, _
                                                   ByVal TotalNeto As Double, ByVal ImporteIVA As Double, _
                                                   ByVal TotalNOTACREDITO As Double, ByVal Vencimiento As Date, _
                                                   ByVal Estatus As String, ByVal Asiento As String, ByVal FechaAsiento As Date,
                                                   ByVal Impresa As String, ByVal CodigoProveedorAnterior As String, ByVal NumeroNOTACREDITOAnterior As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsproencncr SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsproencncr SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numncr = '" & NumeroNOTACREDITOAnterior & "' and " _
                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNOTACREDITO, "NUMNCR")
        strSQL += ModificarCadena(NumeroSerieNotaCredito, "serie_numncr")
        strSQL += ModificarCadena(NumeroCompraAfectada, "NUMCOM")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarFecha(EmisionIVA, "EMISIONIVA")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarCadena(CodigoVendedor, "CODVEN")
        strSQL += ModificarCadena(Almacen, "ALMACEN")
        strSQL += ModificarCadena(Transporte, "TRANSPORTE")
        strSQL += ModificarCadena(Referencia, "REFER")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarEnterolargo(ITEMS, "ITEMS")
        strSQL += ModificarDoble(Cajas, "CAJAS")
        strSQL += ModificarDoble(Kilos, "KILOS")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(ImporteIVA, "IMP_IVA")
        strSQL += ModificarDoble(TotalNOTACREDITO, "TOT_NCR")
        strSQL += ModificarFecha(Vencimiento, "VENCE")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarCadena(Asiento, "ASIENTO")
        strSQL += ModificarFecha(FechaAsiento, "FECHASI")
        strSQL += ModificarCadena(Impresa, "IMPRESA")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub


    Public Sub InsertEditCOMPRASEncabezadoNOTADEBITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                   ByVal NumeroNOTADEBITO As String, ByVal NumeroSerieNotaDebito As String, ByVal NumeroCompraAfectada As String, ByVal Emision As Date, ByVal EmisionIVA As Date, _
                                                   ByVal CodigoProveedor As String, ByVal Comentario As String, ByVal Almacen As String, _
                                                   ByVal Referencia As String, ByVal CodigoContable As String, _
                                                   ByVal Items As Integer, ByVal Cajas As Double, ByVal Kilos As Double, _
                                                   ByVal TotalNeto As Double, ByVal ImporteIVA As Double, _
                                                   ByVal TotalNOTADEBITO As Double, ByVal Vencimiento As Date, _
                                                   ByVal Asiento As String, ByVal FechaAsiento As Date,
                                                   ByVal CodigoProveedorAnterior As String, ByVal NumeroNOTADEBITOAnterior As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsproencndb SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsproencndb SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numndb = '" & NumeroNOTADEBITOAnterior & "' and " _
                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNOTADEBITO, "NUMNDB")
        strSQL += ModificarCadena(NumeroSerieNotaDebito, "serie_numndb")
        strSQL += ModificarCadena(NumeroCompraAfectada, "NUMCOM")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarFecha(EmisionIVA, "EMISIONIVA")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarCadena(Almacen, "ALMACEN")
        strSQL += ModificarCadena(Referencia, "REFER")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarEnterolargo(ITEMS, "ITEMS")
        strSQL += ModificarDoble(Cajas, "CAJAS")
        strSQL += ModificarDoble(Kilos, "KILOS")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(ImporteIVA, "IMP_IVA")
        strSQL += ModificarDoble(TotalNOTADEBITO, "TOT_NDB")
        strSQL += ModificarFecha(Vencimiento, "VENCE")
        strSQL += ModificarCadena(Asiento, "ASIENTO")
        strSQL += ModificarFecha(FechaAsiento, "FECHASI")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    '//////////////////////// RENGLONES
    Public Sub InsertEditCOMPRASRenglonOrdenes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                  ByVal NumeroDeOrden As String, ByVal Renglon As String, ByVal CodigoProveedor As String, _
                                                  ByVal Item As String, ByVal Descripcion As String, ByVal IVA As String, ByVal ICS As String, _
                                                  ByVal Unidad As String, ByVal Cantidad As Double, ByVal Peso As Double, ByVal CantidadTransito As Double, _
                                                  ByVal Estatus As String, ByVal CostoUnitario As Double, ByVal DescuentoArticulo As Double, _
                                                  ByVal DescuentoProveedor As Double, ByVal CostoTotal As Double, ByVal CostoTotalDescuento As Double, _
                                                  ByVal Lote As String, ByVal Aceptado As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprorenord SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprorenord SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numord = '" & NumeroDeOrden & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroDeOrden, "NUMORD")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Renglon, "RENGLON")
        strSQL += ModificarCadena(Item, "ITEM")
        strSQL += ModificarCadena(Descripcion, "DESCRIP")
        strSQL += ModificarCadena(IVA, "IVA")
        strSQL += ModificarCadena(ICS, "ICS")
        strSQL += ModificarCadena(Unidad, "UNIDAD")
        strSQL += ModificarDoble(Cantidad, "CANTIDAD")
        strSQL += ModificarDoble(Peso, "PESO")
        strSQL += ModificarDoble(CantidadTransito, "CANTRAN")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarDoble(CostoUnitario, "COSTOU")
        strSQL += ModificarDoble(DescuentoProveedor, "DES_PRO")
        strSQL += ModificarDoble(DescuentoArticulo, "DES_ART")
        strSQL += ModificarDoble(CostoTotal, "COSTOTOT")
        strSQL += ModificarDoble(CostoTotalDescuento, "COSTOTOTDES")
        strSQL += ModificarCadena(Lote, "LOTE")
        strSQL += ModificarCadena(Aceptado, "ACEPTADO")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASRenglonRECEPCIONES(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                  ByVal NumeroRECEPCION As String, ByVal Renglon As String, ByVal CodigoProveedor As String, _
                                                  ByVal Item As String, ByVal Descripcion As String, ByVal IVA As String, ByVal ICS As String, _
                                                  ByVal Unidad As String, ByVal Cantidad As Double, ByVal Peso As Double, ByVal CantidadTransito As Double, _
                                                  ByVal Estatus As String, ByVal CostoUnitario As Double, ByVal DescuentoArticulo As Double, _
                                                  ByVal DescuentoProveedor As Double, ByVal CostoTotal As Double, ByVal CostoTotalDescuento As Double, _
                                                  ByVal Lote As String, ByVal CodigoContable As String, ByVal NumeroOrden As String, ByVal RenglonOrden As String, _
                                                  ByVal Aceptado As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprorenrep SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprorenrep SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numrec = '" & NumeroRECEPCION & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroRECEPCION, "NUMREC")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Renglon, "RENGLON")
        strSQL += ModificarCadena(Item, "ITEM")
        strSQL += ModificarCadena(Descripcion, "DESCRIP")
        strSQL += ModificarCadena(IVA, "IVA")
        strSQL += ModificarCadena(ICS, "ICS")
        strSQL += ModificarCadena(Unidad, "UNIDAD")
        strSQL += ModificarDoble(Cantidad, "CANTIDAD")
        strSQL += ModificarDoble(Peso, "PESO")
        strSQL += ModificarDoble(CantidadTransito, "CANTRAN")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarDoble(CostoUnitario, "COSTOU")
        strSQL += ModificarDoble(DescuentoProveedor, "DES_PRO")
        strSQL += ModificarDoble(DescuentoArticulo, "DES_ART")
        strSQL += ModificarDoble(CostoTotal, "COSTOTOT")
        strSQL += ModificarDoble(CostoTotalDescuento, "COSTOTOTDES")
        strSQL += ModificarCadena(Lote, "LOTE")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarCadena(NumeroOrden, "NUMORD")
        strSQL += ModificarCadena(RenglonOrden, "RENORD")
        strSQL += ModificarCadena(Aceptado, "ACEPTADO")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASRenglonCOMPRAS(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                              ByVal NUMEROCOMPRA As String, ByVal Renglon As String, ByVal CodigoProveedor As String, _
                                              ByVal Item As String, ByVal Descripcion As String, ByVal IVA As String, ByVal ICS As String, _
                                              ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double, ByVal Peso As Double, _
                                              ByVal lote As String, ByVal Sabor As String, ByVal Color As String, _
                                              ByVal Estatus As String, ByVal CostoUnitario As Double, ByVal DescuentoArticulo As Double, _
                                              ByVal DescuentoProveedor As Double, ByVal CostoTotal As Double, ByVal CostoTotalDescuento As Double, _
                                              ByVal NumeroOrden As String, ByVal RenglonOrden As String, _
                                              ByVal NumeroRecepcion As String, ByVal RenglonRecepcion As String, _
                                              ByVal CodigoContable As String, ByVal Aceptado As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprorencom SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprorencom SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numcom = '" & NUMEROCOMPRA & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NUMEROCOMPRA, "NUMCOM")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Renglon, "RENGLON")
        strSQL += ModificarCadena(Item, "ITEM")
        strSQL += ModificarCadena(Descripcion, "DESCRIP")
        strSQL += ModificarCadena(IVA, "IVA")
        strSQL += ModificarCadena(ICS, "ICS")
        strSQL += ModificarCadena(Unidad, "UNIDAD")
        strSQL += ModificarDoble(Bultos, "BULTOS")
        strSQL += ModificarDoble(Cantidad, "CANTIDAD")
        strSQL += ModificarDoble(Peso, "PESO")
        strSQL += ModificarCadena(lote, "LOTE")
        strSQL += ModificarCadena(Color, "COLOR")
        strSQL += ModificarCadena(Sabor, "SABOR")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarDoble(CostoUnitario, "COSTOU")
        strSQL += ModificarDoble(DescuentoProveedor, "DES_PRO")
        strSQL += ModificarDoble(DescuentoArticulo, "DES_ART")
        strSQL += ModificarDoble(CostoTotal, "COSTOTOT")
        strSQL += ModificarDoble(CostoTotalDescuento, "COSTOTOTDES")
        strSQL += ModificarCadena(NumeroOrden, "NUMORD")
        strSQL += ModificarCadena(RenglonOrden, "RENORD")
        strSQL += ModificarCadena(NumeroRecepcion, "NUMREC")
        strSQL += ModificarCadena(RenglonRecepcion, "RENREC")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarCadena(Aceptado, "ACEPTADO")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASRenglonGASTOS(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                              ByVal NUMEROGASTO As String, ByVal Renglon As String, ByVal CodigoProveedor As String, _
                                              ByVal Item As String, ByVal Descripcion As String, ByVal IVA As String, ByVal ICS As String, _
                                              ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double, ByVal Peso As Double, _
                                              ByVal lote As String, _
                                              ByVal Estatus As String, ByVal CostoUnitario As Double, ByVal DescuentoArticulo As Double, _
                                              ByVal DescuentoProveedor As Double, ByVal CostoTotal As Double, ByVal CostoTotalDescuento As Double, _
                                              ByVal CAUSA As String, _
                                              ByVal CodigoContable As String, ByVal Aceptado As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprorengas SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprorengas SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numgas = '" & NUMEROGASTO & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NUMEROGASTO, "NUMGAS")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Renglon, "RENGLON")
        strSQL += ModificarCadena(Item, "ITEM")
        strSQL += ModificarCadena(Descripcion, "DESCRIP")
        strSQL += ModificarCadena(IVA, "IVA")
        strSQL += ModificarCadena(ICS, "ICS")
        strSQL += ModificarCadena(Unidad, "UNIDAD")
        strSQL += ModificarDoble(Bultos, "BULTOS")
        strSQL += ModificarDoble(Cantidad, "CANTIDAD")
        strSQL += ModificarDoble(Peso, "PESO")
        strSQL += ModificarCadena(lote, "LOTE")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarDoble(CostoUnitario, "COSTOU")
        strSQL += ModificarDoble(DescuentoProveedor, "DES_PRO")
        strSQL += ModificarDoble(DescuentoArticulo, "DES_ART")
        strSQL += ModificarDoble(CostoTotal, "COSTOTOT")
        strSQL += ModificarDoble(CostoTotalDescuento, "COSTOTOTDES")
        strSQL += ModificarCadena(CAUSA, "CAUSA")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarCadena(Aceptado, "ACEPTADO")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASRenglonNOTACREDITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                             ByVal NUMERONOTACREDITO As String, ByVal Renglon As String, ByVal CodigoProveedor As String, _
                                             ByVal Item As String, ByVal Descripcion As String, ByVal IVA As String, ByVal ICS As String, _
                                             ByVal Unidad As String, ByVal Cantidad As Double, ByVal Peso As Double, _
                                             ByVal lote As String, _
                                             ByVal Estatus As String, ByVal CostoUnitario As Double, ByVal DescuentoArticulo As Double, _
                                             ByVal DescuentoProveedor As Double, ByVal PorcentajeAceptacion As Double, ByVal CostoTotal As Double, ByVal CostoTotalDescuento As Double, _
                                             ByVal NumeroCompra As String, ByVal CAUSA As String, _
                                             ByVal CodigoContable As String, ByVal Editable As Integer, ByVal Aceptado As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprorenncr SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprorenncr SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numncr = '" & NUMERONOTACREDITO & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NUMERONOTACREDITO, "NUMNCR")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Renglon, "RENGLON")
        strSQL += ModificarCadena(Item, "ITEM")
        strSQL += ModificarCadena(Descripcion, "DESCRIP")
        strSQL += ModificarCadena(IVA, "IVA")
        strSQL += ModificarCadena(ICS, "ICS")
        strSQL += ModificarCadena(Unidad, "UNIDAD")
        strSQL += ModificarDoble(Cantidad, "CANTIDAD")
        strSQL += ModificarDoble(Peso, "PESO")
        strSQL += ModificarCadena(lote, "LOTE")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarDoble(CostoUnitario, "PRECIO")
        strSQL += ModificarDoble(DescuentoProveedor, "DES_PRO")
        strSQL += ModificarDoble(DescuentoArticulo, "DES_ART")
        strSQL += ModificarDoble(PorcentajeAceptacion, "POR_ACEPTA_DEV")
        strSQL += ModificarDoble(CostoTotal, "TOTREN")
        strSQL += ModificarDoble(CostoTotalDescuento, "TOTRENDES")
        strSQL += ModificarCadena(NumeroCompra, "NUMCOM")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarCadena(Editable, "EDITABLE")
        strSQL += ModificarCadena(CAUSA, "CAUSA")
        strSQL += ModificarCadena(Aceptado, "ACEPTADO")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASRenglonNOTADEBITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                             ByVal NUMERONOTADEBITO As String, ByVal Renglon As String, ByVal CodigoProveedor As String, _
                                             ByVal Item As String, ByVal Descripcion As String, ByVal IVA As String, ByVal ICS As String, _
                                             ByVal Unidad As String, ByVal Cantidad As Double, ByVal Peso As Double, _
                                             ByVal lote As String, _
                                             ByVal Estatus As String, ByVal CostoUnitario As Double, ByVal DescuentoArticulo As Double, _
                                             ByVal DescuentoProveedor As Double, ByVal CostoTotal As Double, ByVal CostoTotalDescuento As Double, _
                                             ByVal NumeroCompra As String, ByVal CAUSA As String, _
                                             ByVal CodigoContable As String, ByVal Editable As Integer, ByVal Aceptado As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprorenndb SET "
            strSQL = ""
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprorenndb SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numndb = '" & NUMERONOTADEBITO & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NUMERONOTADEBITO, "NUMNDB")
        strSQL += ModificarCadena(CodigoProveedor, "CODPRO")
        strSQL += ModificarCadena(Renglon, "RENGLON")
        strSQL += ModificarCadena(Item, "ITEM")
        strSQL += ModificarCadena(Descripcion, "DESCRIP")
        strSQL += ModificarCadena(IVA, "IVA")
        strSQL += ModificarCadena(ICS, "ICS")
        strSQL += ModificarCadena(Unidad, "UNIDAD")
        strSQL += ModificarDoble(Cantidad, "CANTIDAD")
        strSQL += ModificarDoble(Peso, "PESO")
        strSQL += ModificarCadena(lote, "LOTE")
        strSQL += ModificarCadena(Estatus, "ESTATUS")
        strSQL += ModificarDoble(CostoUnitario, "COSTO")
        strSQL += ModificarDoble(DescuentoProveedor, "DES_PRO")
        strSQL += ModificarDoble(DescuentoArticulo, "DES_ART")
        strSQL += ModificarDoble(CostoTotal, "TOTREN")
        strSQL += ModificarDoble(CostoTotalDescuento, "TOTRENDES")
        strSQL += ModificarCadena(NumeroCompra, "NUMCOM")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarCadena(Editable, "EDITABLE")
        strSQL += ModificarCadena(CAUSA, "CAUSA")
        strSQL += ModificarCadena(Aceptado, "ACEPTADO")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    '/////////////////////////////////////////////////////////// 
    Public Sub InsertEditCOMPRASCXP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoProveedor As String, _
        ByVal TipoMovimiento As String, ByVal NumeroMovimiento As String, ByVal FechaEmision As Date, ByVal Hora As String, _
        ByVal FechaVencimiento As Date, ByVal Referencia As String, ByVal Concepto As String, ByVal Importe As Double, _
        ByVal ImporteIVA As Double, ByVal FormaPago As String, ByVal NumeroPago As String, ByVal NombrePago As String, _
        ByVal Beneficiario As String, ByVal Origen As String, ByVal NumeroDeposito As String, ByVal CuentaDeposito As String, _
        ByVal BancoDeposito As String, ByVal CajaPago As String, ByVal NumeroOrigen As String, ByVal MultiCancelacion As String, _
        ByVal Asiento As String, ByVal FechaAsiento As Date, ByVal CodigoContable As String, ByVal Multidocumento As String, _
        ByVal TipoDocumentoCancelado As String, ByVal Intereses As Double, ByVal Capital As Double, ByVal NumeroComprobante As String, _
        ByVal Banco As String, ByVal CuentaBancaria As String, ByVal Remesa As String, ByVal CodigoVendedor As String, _
        ByVal CodigoCobrador As String, ByVal TipoProveedor As Integer, ByVal FoTipo As String, ByVal Historico As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsprotrapag SET "
            strSQLFin = " "
        Else
            strSQLInicio = " UPDATE jsprotrapag SET "
            strSQLFin = " WHERE " _
                & " CODPRO = '" & CodigoProveedor & "' AND " _
                & " TIPOMOV = '" & TipoMovimiento & "' AND " _
                & " NUMMOV = '" & NumeroMovimiento & "' AND " _
                & " EMISION = '" & FormatoFechaMySQL(FechaEmision) & "' AND " _
                & " TIPO = '" & TipoProveedor & "' AND " _
                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoProveedor, "codpro")
        strSQL += ModificarCadena(TipoMovimiento, "tipomov")
        strSQL += ModificarCadena(NumeroMovimiento, "nummov")
        strSQL += ModificarFecha(FechaEmision, "emision")
        strSQL += ModificarCadena(Hora, "hora")
        strSQL += ModificarFecha(FechaVencimiento, "vence")
        strSQL += ModificarCadena(Referencia, "REFER")
        strSQL += ModificarCadena(Concepto, "CONCEPTO")
        strSQL += ModificarDoble(Importe, "IMPORTE")
        strSQL += ModificarDoble(ImporteIVA, "PORIVA")
        strSQL += ModificarCadena(FormaPago, "FORMAPAG")
        strSQL += ModificarCadena(NumeroPago, "NUMPAG")
        strSQL += ModificarCadena(NombrePago, "NOMPAG")
        strSQL += ModificarCadena(Beneficiario, "BENEFIC")
        strSQL += ModificarCadena(Origen, "ORIGEN")
        strSQL += ModificarCadena(NumeroDeposito, "DEPOSITO")
        strSQL += ModificarCadena(CuentaDeposito, "CTADEP")
        strSQL += ModificarCadena(BancoDeposito, "BANCODEP")
        strSQL += ModificarCadena(CajaPago, "CAJAPAG")
        strSQL += ModificarCadena(NumeroOrigen, "NUMORG")
        strSQL += ModificarCadena(MultiCancelacion, "MULTICAN")
        strSQL += ModificarCadena(Asiento, "ASIENTO")
        strSQL += ModificarFecha(FechaAsiento, "FECHASI")
        strSQL += ModificarCadena(CodigoContable, "CODCON")
        strSQL += ModificarCadena(Multidocumento, "MULTIDOC")
        strSQL += ModificarCadena(TipoDocumentoCancelado, "TIPDOCCAN")
        strSQL += ModificarDoble(Intereses, "INTERES")
        strSQL += ModificarDoble(Capital, "CAPITAL")
        strSQL += ModificarCadena(NumeroComprobante, "COMPROBA")
        strSQL += ModificarCadena(Banco, "BANCO")
        strSQL += ModificarCadena(CuentaBancaria, "CTABANCO")
        strSQL += ModificarCadena(Remesa, "REMESA")
        strSQL += ModificarCadena(CodigoVendedor, "CODVEN")
        strSQL += ModificarCadena(CodigoCobrador, "CODCOB")
        strSQL += ModificarCadena(FoTipo, "FOTIPO")
        strSQL += ModificarCadena(Historico, "HISTORICO")
        strSQL += ModificarCadena(TipoProveedor, "TIPO")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "EJERCICIO")
        strSQL += ModificarCadena(jytsistema.WorkID, "ID_EMP")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASCancelacion(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
    ByVal CodigoProveedor As String, ByVal TipoMovimiento As String, ByVal NumeroMovimiento As String, _
    ByVal Emision As Date, ByVal Referencia As String, ByVal Concepto As String, ByVal Importe As Double, _
    ByVal Comprobante As String, ByVal CodigoVendedor As String)


        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsprotrapagcan SET "
        Else
            strSQLInicio = " UPDATE jsprotrapagcan set "
            strSQLFin = " WHERE " _
                & " codpro = '" & CodigoProveedor & "' and " _
                & " tipomov = '" & TipoMovimiento & "' and " _
                & " nummov = '" & NumeroMovimiento & "' and " _
                & " comproba = '" & Comprobante & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoProveedor, "codpro")
        strSQL += ModificarCadena(TipoMovimiento, "tipomov")
        strSQL += ModificarCadena(NumeroMovimiento, "nummov")
        strSQL += ModificarFecha(Emision, "emision")
        strSQL += ModificarCadena(Referencia, "refer")
        strSQL += ModificarCadena(Concepto, "concepto")
        strSQL += ModificarDoble(Importe, "importe")
        strSQL += ModificarCadena(Comprobante, "comproba")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCOMPRASGrupo(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                        ByVal CodigoNivelAnterior As Integer, ByVal CodigoGrupo As String, ByVal Descripcion As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsprogrugas SET codigo = null, "
        Else
            strSQLInicio = " UPDATE jsprogrugas SET "
            strSQLFin = " WHERE " _
                & " codigo = '" & CodigoGrupo & "' and " _
                & " antecesor = " & CodigoNivelAnterior & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoNivelAnterior, "antecesor")
        strSQL += ModificarCadena(Descripcion, "nombre")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(Myconn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCOMPRASDescuento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                       ByVal NombreTablaEnBD As String, ByVal NombreCampoDocumentoEnBD As String, _
                                       ByVal NumeroDocumento As String, ByVal numRenglon As String, ByVal DescripcionDescuento As String, _
                                       ByVal PorcentajeDescuento As Double, ByVal MontoDescuento As Double, _
                                       ByVal CodigoProveedor As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into " & NombreTablaEnBD & " SET "
        Else
            strSQLInicio = " UPDATE " & NombreTablaEnBD & " SET "
            strSQLFin = " WHERE " _
                & " " & NombreCampoDocumentoEnBD & " = '" & NumeroDocumento & "' and " _
                & " CODPRO = '" & CodigoProveedor & "' AND " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroDocumento, NombreCampoDocumentoEnBD)
        strSQL += ModificarCadena(CodigoProveedor, "codpro")
        strSQL += ModificarCadena(numRenglon, "renglon")
        strSQL += ModificarCadena(DescripcionDescuento, "descrip")
        strSQL += ModificarDoble(PorcentajeDescuento, "pordes")
        strSQL += ModificarDoble(MontoDescuento, "descuento")
        strSQL += ModificarDoble(0.0, "subtotal")
        strSQL += ModificarCadena("1", "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCOMPRASExpedienteProoveedor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                       ByVal CodigoPROVEEDOR As String, ByVal FechaMovimiento As Date, _
                       ByVal Comentario As String, ByVal Condicion As Integer, ByVal Causa As String, _
                       ByVal TipoCondicion As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsproexppro SET "
        Else
            strSQLInicio = " UPDATE jsproexppro SET "
            strSQLFin = " WHERE " _
                & " codpro = '" & CodigoPROVEEDOR & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoPROVEEDOR, "codpro")
        strSQL += ModificarFechaTiempo(FechaMovimiento, "fecha")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarEntero(Condicion, "condicion")
        strSQL += ModificarCadena(Causa, "causa")
        strSQL += ModificarEntero(TipoCondicion, "tipocondicion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

End Module
