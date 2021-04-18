Imports MySql.Data.MySqlClient
Imports MySql.Data.Types
Module TablasControl

    Public Sub InsertarAuditoria(ByVal MyConn As MySqlConnection, ByVal TipoMovimiento As Integer, ByVal Modulo As String, _
        ByVal Documento As String)

        Dim myComm As New MySqlCommand
        myComm.Connection = myconn
        myComm.CommandText = " insert into jsconregaud values( " _
        & "'" & jytsistema.sUsuario & "', '" & My.Computer.Name.ToString & "', '" _
        & FormatoFechaHoraMySQL(Now()) & "', " _
        & nGestion & ", '" & Modulo & "', " & TipoMovimiento & ", '" & Documento & "', " _
        & "'', '" & WorkID & "')"

        myComm.ExecuteNonQuery()

        myComm = Nothing

    End Sub
    Public Sub InsertEditCONTROLEmpresa(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Codigo As String, ByVal Nombre As String, _
    ByVal DireccionFiscal As String, ByVal DirDespacho As String, ByVal Ciudad As String, ByVal Estado As String, _
    ByVal CodGeografico As String, ByVal Zip As String, ByVal Telefono_1 As String, ByVal Telefono_2 As String, _
    ByVal Fax As String, ByVal email As String, ByVal Actividad As String, ByVal Rif As String, ByVal Nit As String, _
    ByVal Ciiu As String, ByVal TipoPersona As Integer, ByVal Inicio As Date, ByVal Cierre As Date, _
    ByVal TipoSociedad As Integer, ByVal Lucro As Integer, ByVal Nacional As String, ByVal CI As String, ByVal Pasaporte As String, _
    ByVal Casado As Integer, ByVal SeparaBienes As Integer, ByVal Rentasexentas As Integer, ByVal EsposaDeclara As Integer, _
    ByVal Rep_RIF As String, ByVal Rep_NIT As String, ByVal REP_Nacional As String, ByVal Rep_CI As String, _
    ByVal Rep_Nombre As String, ByVal Rep_Direccion As String, ByVal Rep_Ciudad As String, ByVal Rep_Estado As String, _
    ByVal Rep_Telef As String, ByVal Rep_Fax As String, ByVal Rep_Email As String, ByVal Ger_Nacional As String, _
    ByVal Ger_CI As String, ByVal Ger_Nombre As String, ByVal Ger_Direccion As String, ByVal Ger_Telef As String, _
    ByVal Ger_ciudad As String, ByVal Ger_Estado As String, ByVal Ger_cel As String, ByVal Ger_email As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsconctaemp SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsconctaemp SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " ID_EMP = '" & Codigo & "' "
        End If

        strSQL = strSQL & ModificarCadena(Codigo, "id_emp")
        strSQL = strSQL & ModificarCadena(Nombre, "nombre")
        strSQL = strSQL & ModificarCadena(DireccionFiscal, "dirfiscal")
        strSQL = strSQL & ModificarCadena(DirDespacho, "dirorigen")
        strSQL = strSQL & ModificarCadena(Ciudad, "ciudad")
        strSQL = strSQL & ModificarCadena(Estado, "estado")
        strSQL = strSQL & ModificarCadena(CodGeografico, "codgeo")
        strSQL = strSQL & ModificarCadena(Zip, "zip")
        strSQL = strSQL & ModificarCadena(Telefono_1, "telef1")
        strSQL = strSQL & ModificarCadena(Telefono_2, "telef2")
        strSQL = strSQL & ModificarCadena(Fax, "fax")
        strSQL = strSQL & ModificarCadena(email, "email")
        strSQL = strSQL & ModificarCadena(Actividad, "actividad")
        strSQL = strSQL & ModificarCadena(Rif, "rif")
        strSQL = strSQL & ModificarCadena(Nit, "nit")
        strSQL = strSQL & ModificarCadena(Ciiu, "ciiu")
        strSQL = strSQL & ModificarCadena(CStr(TipoPersona), "tipopersona")
        strSQL = strSQL & ModificarFecha(Inicio, "inicio")
        strSQL = strSQL & ModificarFecha(Cierre, "cierre")
        strSQL = strSQL & ModificarCadena(CStr(TipoSociedad), "tiposoc")
        strSQL = strSQL & ModificarCadena(CStr(Lucro), "lucro")
        strSQL = strSQL & ModificarCadena(Nacional, "nacional")
        strSQL = strSQL & ModificarCadena(CI, "CI")
        strSQL = strSQL & ModificarCadena(Pasaporte, "pasaporte")
        strSQL = strSQL & ModificarCadena(CStr(Casado), "casado")
        strSQL = strSQL & ModificarCadena(CStr(SeparaBienes), "separabienes")
        strSQL = strSQL & ModificarCadena(CStr(Rentasexentas), "rentasexentas")
        strSQL = strSQL & ModificarCadena(CStr(EsposaDeclara), "esposadeclara")
        strSQL = strSQL & ModificarCadena(Rep_RIF, "rep_rif")
        strSQL = strSQL & ModificarCadena(Rep_NIT, "rep_nit")
        strSQL = strSQL & ModificarCadena(REP_Nacional, "rep_nacional")
        strSQL = strSQL & ModificarCadena(Rep_CI, "rep_ci")
        strSQL = strSQL & ModificarCadena(Rep_Nombre, "rep_nombre")
        strSQL = strSQL & ModificarCadena(Rep_Direccion, "rep_direccion")
        strSQL = strSQL & ModificarCadena(Rep_Ciudad, "rep_ciudad")
        strSQL = strSQL & ModificarCadena(Rep_Estado, "rep_estado")
        strSQL = strSQL & ModificarCadena(Rep_Telef, "rep_telef")
        strSQL = strSQL & ModificarCadena(Rep_Fax, "rep_fax")
        strSQL = strSQL & ModificarCadena(Rep_Email, "rep_email")
        strSQL = strSQL & ModificarCadena(Ger_Nacional, "ger_nacional")
        strSQL = strSQL & ModificarCadena(Ger_CI, "ger_ci")
        strSQL = strSQL & ModificarCadena(Ger_Nombre, "ger_nombre")
        strSQL = strSQL & ModificarCadena(Ger_Direccion, "ger_direccion")
        strSQL = strSQL & ModificarCadena(Ger_Telef, "ger_telef")
        strSQL = strSQL & ModificarCadena(Ger_ciudad, "ger_ciudad")
        strSQL = strSQL & ModificarCadena(Ger_Estado, "ger_estado")
        strSQL = strSQL & ModificarCadena(Ger_cel, "ger_cel")
        strSQL = strSQL & ModificarCadena(Ger_email, "ger_email")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub GuardarLogo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Empresa As String, ByVal MyLogo() As Byte)
        Dim myComm As New MySqlCommand
        Try
            myComm.Connection = MyConn
            myComm.CommandText = "update jsconctaemp set logo = ?Logo where id_emp = '" & Empresa & "' "
            myComm.Parameters.AddWithValue("?logo", MyLogo)
            myComm.ExecuteNonQuery()
            myComm = Nothing
        Catch ex As MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try
    End Sub

    Public Sub GuardarFotoMercancia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                    ByVal CodigoMercancia As String, ByVal MyLogo() As Byte)
        Dim myComm As New MySqlCommand
        Try
            myComm.Connection = MyConn
            If Not Insertar Then
                myComm.CommandText = "insert into jsmerctainvfot set codart = '" & CodigoMercancia & "', Foto1 = ?Foto1, id_emp = '" & jytsistema.WorkID & "' "
            Else
                myComm.CommandText = "update jsmerctainvfot set Foto1 = ?Foto1 where codart = '" & CodigoMercancia & "' AND id_emp = '" & jytsistema.WorkID & "' "
            End If
            myComm.Parameters.AddWithValue("?Foto1", MyLogo)
            myComm.ExecuteNonQuery()
            myComm = Nothing
        Catch ex As MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try
    End Sub
    Public Sub InsertEditCONTROLEncabMapa(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                            ByVal Codigo As String, ByVal Nombre As String, _
                                            ByVal Incluye As Boolean, ByVal Modifica As Boolean, ByVal Elimina As Boolean)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsconencmap SET "
            InsertarRenglonesdeMapaIniciales(MyConn, lblInfo, Codigo)
        Else
            strSQLInicio = " UPDATE jsconencmap SET "
            strSQLFin = " WHERE " _
                & " mapa = '" & Codigo & "' "
        End If
        strSQL = strSQL & ModificarCadena(Codigo, "mapa")
        strSQL = strSQL & ModificarCadena(Nombre, "nombre")
        strSQL = strSQL & ModificarCadena(IIf(Incluye, "1", "0"), "incluye")
        strSQL = strSQL & ModificarCadena(IIf(Modifica, "1", "0"), "modifica")
        strSQL = strSQL & ModificarCadena(IIf(Elimina, "1", "0"), "elimina")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertarRenglonesdeMapaIniciales(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Codigo As String)
        Dim kCont As Integer
        For kCont = 1 To aGestion.Length
            EjecutarSTRSQL(Myconn, lblInfo, " INSERT INTO jsconrenglonesmapa " _
                                    & " SET mapa = '" & Codigo & "' , descripcion = '" & aGestion(kCont - 1) & "' , " _
                                    & " region = 'RibbonTab" & kCont & "' , gestion = " & kCont & ", nivel = 0, modulo = 0, acceso = 1, incluye = 1, modifica = 1, elimina = 1 ")
        Next
    End Sub
    Public Sub InsertEditCONTROLTarjeta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Codigo As String, ByVal Nombre As String, _
      ByVal Comision As Double, ByVal Impuesto As Double, ByVal Tipo As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconctatar SET "
        Else
            strSQLInicio = " UPDATE jsconctatar SET "
            strSQLFin = " WHERE " _
                & " codtar = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(Codigo, "codtar")
        strSQL = strSQL & ModificarCadena(Nombre, "nomtar")
        strSQL = strSQL & ModificarDoble(Comision, "com1")
        strSQL = strSQL & ModificarDoble(Impuesto, "com2")
        strSQL = strSQL & ModificarEntero(Tipo, "tipo")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLCestaTicket(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                               ByVal Corredor As String, ByVal Descripcion As String, ByVal PorcentajeComisionCorredor As Double, _
                                               ByVal PorcentajeComisionCliente As Double, ByVal LongitudCodigoBarras As Integer, _
                                               ByVal InicioPrecio As Integer, ByVal LongitudPrecio As Integer, ByVal InicioTipo As Integer, ByVal LongitudTipo As Integer, _
                                               ByVal Cargos As Double, ByVal TipoIva As String, ByVal CodigoContable As String, ByVal CodigoProveedor As String, _
                                               ByVal Grupo As Integer, ByVal SubGrupo As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsvencestic SET "
        Else
            strSQLInicio = " UPDATE jsvencestic SET "
            strSQLFin = " WHERE " _
                & " codigo = '" & Corredor & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(Corredor, "codigo")
        strSQL = strSQL & ModificarCadena(Descripcion, "descrip")
        strSQL = strSQL & ModificarCadena(Descripcion, "descrip")
        strSQL = strSQL & ModificarDoble(PorcentajeComisionCorredor, "porcom")
        strSQL = strSQL & ModificarDoble(PorcentajeComisionCliente, "porcomcli")
        strSQL = strSQL & ModificarEntero(LongitudCodigoBarras, "lencodbar")
        strSQL = strSQL & ModificarEntero(InicioPrecio, "inicioprecio")
        strSQL = strSQL & ModificarEntero(LongitudPrecio, "lenprecio")
        strSQL = strSQL & ModificarEntero(InicioTipo, "iniciotipo")
        strSQL = strSQL & ModificarEntero(LongitudTipo, "lentipo")
        strSQL = strSQL & ModificarDoble(Cargos, "cargos")
        strSQL = strSQL & ModificarCadena(TipoIva, "tipoiva")
        strSQL = strSQL & ModificarCadena(CodigoContable, "CODCON")
        strSQL = strSQL & ModificarCadena(CodigoProveedor, "codpro")
        strSQL = strSQL & ModificarEntero(Grupo, "grupo")
        strSQL = strSQL & ModificarEntero(SubGrupo, "subgrupo")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLCestaTicketTipo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                ByVal Corredor As String, ByVal Tipo As String, ByVal Descripcion As String, _
                                                ByVal ComisionCorredor As Double, ByVal ComisionCliente As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsvencestip SET "
        Else
            strSQLInicio = " UPDATE jsvencestip SET "
            strSQLFin = " WHERE " _
                & " corredor = '" & Corredor & "' and " _
                & " tipo = '" & Tipo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(Corredor, "corredor")
        strSQL = strSQL & ModificarCadena(Tipo, "tipo")
        strSQL = strSQL & ModificarCadena(Descripcion, "descrip")
        strSQL = strSQL & ModificarDoble(ComisionCorredor, "com_corredor")
        strSQL = strSQL & ModificarDoble(ComisionCliente, "com_cliente")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLCestaTicketTipoComisiones(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                            ByVal Corredor As String, ByVal Tipo As String, ByVal Desde As Integer, _
                                            ByVal Hasta As Integer, ByVal ComisionCliente As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsvencescom SET "
        Else
            strSQLInicio = " UPDATE jsvencescom SET "
            strSQLFin = " WHERE " _
                & " corredor = '" & Corredor & "' and " _
                & " tipo = '" & Tipo & "' and " _
                & " desde = " & Desde & " and " _
                & " hasta = " & Hasta & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(Corredor, "corredor")
        strSQL = strSQL & ModificarCadena(Tipo, "tipo")
        strSQL = strSQL & ModificarEntero(Desde, "desde")
        strSQL = strSQL & ModificarEntero(Hasta, "hasta")
        strSQL = strSQL & ModificarDoble(ComisionCliente, "comision")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLCestaTicketMovimientoValor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                        ByVal Corredor As String, ByVal EnBarra As String, ByVal Valor As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsvenvaltic SET "
        Else
            strSQLInicio = " UPDATE jsvenvaltic SET "
            strSQLFin = " WHERE " _
                & " codigo = '" & Corredor & "' and " _
                & " enbarra = '" & EnBarra & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(Corredor, "codigo")
        strSQL = strSQL & ModificarCadena(EnBarra, "enbarra")
        strSQL = strSQL & ModificarDoble(Valor, "VALOR")
        strSQL = strSQL & ModificarCadena("1", "ACEPTADO")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLFormadePago(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                            ByVal Codigo As String, ByVal Nombre As String, ByVal tipo As Integer, _
                                            ByVal Giros As Integer, ByVal Periodo As Integer, ByVal PeriodoEntreGiros As Integer, _
                                            ByVal Interes As Double, ByVal LimiteCredito As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconctafor SET "
        Else
            strSQLInicio = " UPDATE jsconctafor SET "
            strSQLFin = " WHERE " _
                & " codfor = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(Codigo, "codfor")
        strSQL = strSQL & ModificarCadena(Nombre, "nomfor")
        strSQL = strSQL & ModificarEntero(tipo, "tipodoc")
        strSQL = strSQL & ModificarEntero(Periodo, "periodo")
        strSQL = strSQL & ModificarEntero(Giros, "giros")
        strSQL = strSQL & ModificarEntero(PeriodoEntreGiros, "pergiros")
        strSQL = strSQL & ModificarDoble(Interes, "interes")
        strSQL = strSQL & ModificarDoble(LimiteCredito, "limite")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLTablaSimple(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Codigo As String, ByVal Descripcion As String, _
      ByVal Modulo As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconctatab SET "
        Else
            strSQLInicio = " UPDATE jsconctatab SET "
            strSQLFin = " WHERE " _
                & " codigo = '" & Codigo & "' and " _
                & " modulo = '" & Modulo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarCadena(Codigo, "codigo")
        strSQL = strSQL & ModificarCadena(Descripcion, "descrip")
        strSQL = strSQL & ModificarCadena(Modulo, "modulo")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCONTROLParametro(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Gestion As Integer, ByVal Modulo As Integer, _
      ByVal Codigo As String, ByVal Descripcion As String, ByVal Tipo As Integer, ByVal Valor As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconparametros SET "
        Else
            strSQLInicio = " UPDATE jsconparametros SET "
            strSQLFin = " WHERE " _
                & " gestion = " & Gestion & " and " _
                & " modulo = " & Modulo & " and " _
                & " codigo = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarEnteroLargo(Gestion, "gestion")
        strSQL = strSQL & ModificarEnteroLargo(Modulo, "modulo")
        strSQL = strSQL & ModificarCadena(Codigo, "codigo")
        strSQL = strSQL & ModificarCadena(Descripcion, "descripcion")
        strSQL = strSQL & ModificarEnteroLargo(Tipo, "tipo")
        strSQL = strSQL & ModificarCadena(Valor, "valor")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLContador(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Gestion As Integer, ByVal Modulo As Integer, _
      ByVal Codigo As String, ByVal Descripcion As String, ByVal Contador As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconcontadores SET "
        Else
            strSQLInicio = " UPDATE jsconcontadores SET "
            strSQLFin = " WHERE " _
                & " gestion = " & Gestion & " and " _
                & " codigo = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & ModificarEnteroLargo(Gestion, "gestion")
        strSQL = strSQL & ModificarCadena(Modulo, "modulo")
        strSQL = strSQL & ModificarCadena(Codigo, "codigo")
        strSQL = strSQL & ModificarCadena(Descripcion, "descripcion")
        strSQL = strSQL & ModificarCadena(Contador, "contador")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCONTROLUsuario(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal Login As String, ByVal PassWord As String, ByVal Nombre As String, ByVal CodigoUsuario As String, ByVal Mapa As String, _
        ByVal EmpresaInicio As String, ByVal GestionInicio As Integer, ByVal Nivel As Integer, ByVal Estatus As Integer, ByVal Moneda As String, _
        ByVal DivisionInicio As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconctausu SET " _
            & " password = aes_encrypt('" & PassWord & "', '" & Login & "') , "
        Else
            strSQLInicio = " UPDATE jsconctausu set " _
                & " password = aes_encrypt('" & PassWord & "', '" & Login & "') , "
            strSQLFin = " WHERE " _
                & " id_user = '" & CodigoUsuario & "' "
        End If

        strSQL = strSQL & ModificarCadena(CodigoUsuario, "id_user")
        strSQL = strSQL & ModificarCadena(Login, "USUARIO")
        strSQL = strSQL & ModificarCadena(Nombre, "NOMBRE")
        strSQL = strSQL & ModificarCadena(Mapa, "MAPA")
        strSQL = strSQL & ModificarCadena(Moneda, "Moneda")
        strSQL = strSQL & ModificarCadena(EmpresaInicio, "INI_EMP")
        strSQL = strSQL & ModificarEnteroLargo(GestionInicio, "INI_gestion")
        strSQL = strSQL & ModificarEnteroLargo(Nivel, "NIVEL")
        strSQL = strSQL & ModificarCadena(DivisionInicio, "DIVISION")
        strSQL = strSQL & ModificarEntero(Estatus, "ESTATUS")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCONTROLIVA(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal Tasa As String, ByVal porcentaje As Double, ByVal Fecha As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconctaiva SET "
        Else
            strSQLInicio = " UPDATE jsconctaiva set "
            strSQLFin = " WHERE " _
                & " tipo = '" & Tasa & "' and " _
                & " fecha = '" & FormatoFechaMySQL(Fecha) & "' "
        End If

        strSQL += ModificarCadena(Tasa, "tipo")
        strSQL += ModificarDoble(porcentaje, "monto")
        strSQL += ModificarFecha(Fecha, "fecha")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLNumeroControl(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                              ByVal NumeroDocumento As String, CodigoProveedorCliente As String, _
                                              ByVal NumeroDeControl As String, ByVal FechaEmision As Date, _
                                              ByVal OrigenGestion As String, ByVal OrigenModulo As String, _
                                              NumeroDocumentoAnterior As String, CodigoProveedorClienteAnterior As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        EjecutarSTRSQL(MyConn, lblInfo, " DELETE FROM jsconnumcon " _
                       & " where " _
                       & " numdoc = '" & NumeroDocumentoAnterior & "' and " _
                       & " prov_cli = '" & CodigoProveedorClienteAnterior & "' and  " _
                       & " org = '" & OrigenModulo & "' and " _
                       & " origen = '" & OrigenGestion & "' and " _
                       & " id_emp = '" & jytsistema.WorkID & "' ")

        strSQLInicio = " REPLACE into jsconnumcon SET "
        strSQL += ModificarCadena(NumeroDocumento, "numdoc")
        strSQL += ModificarCadena(NumeroDeControl, "num_control")
        If CodigoProveedorCliente <> "" Then strSQL += ModificarCadena(CodigoProveedorCliente, "prov_cli")
        strSQL += ModificarCadena(OrigenGestion, "origen")
        strSQL += ModificarCadena(OrigenModulo, "org")
        strSQL += ModificarFecha(FechaEmision, "emision")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLNumeroControlPorMaquina(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal ComputerName As String, ByVal NumeroDeControl As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconnumcontrol SET "
        Else
            strSQLInicio = " UPDATE jsconnumcontrol set "
            strSQLFin = " WHERE " _
                & " maquina = '" & ComputerName & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(ComputerName, "maquina")
        strSQL += ModificarCadena(NumeroDeControl, "numerocontrol")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLUT(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal monto As Double, ByVal Fecha As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconctaunt SET "
        Else
            strSQLInicio = " UPDATE jsconctaunt set "
            strSQLFin = " WHERE " _
                & " fecha = '" & FormatoFechaMySQL(Fecha) & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarDoble(monto, "monto")
        strSQL += ModificarFecha(Fecha, "fecha")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLRenglonesConjunto(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal Codigo As String, ByVal Letra As String, ByVal Tabla As String, ByVal Tipo As Integer, _
        ByVal Relacion As String, ByVal Gestion As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconcojtab SET "
        Else
            strSQLInicio = " UPDATE jsconcojtab set "
            strSQLFin = " WHERE " _
                & " codigo = '" & Codigo & "' and " _
                & " letra = '" & Letra & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codigo")
        strSQL += ModificarCadena(Letra, "letra")
        strSQL += ModificarCadena(Tabla, "tabla")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarCadena(Relacion, "relacion")
        strSQL += ModificarEnteroLargo(Gestion, "gestion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLCausaCreditoDebito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal Codigo As String, ByVal descripcion As String, ByVal CreditoDebito As Integer, ByVal MovimientoInventario As Integer, ByVal ValidaUnidad As Integer, _
       ByVal AjustePrecio As Integer, ByVal Estado As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsvencaudcr SET "
        Else
            strSQLInicio = " UPDATE jsvencaudcr set "
            strSQLFin = " WHERE " _
                & " codigo = '" & Codigo & "' and " _
                & " credito_debito = " & CreditoDebito & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codigo")
        strSQL += ModificarCadena(descripcion, "descrip")
        strSQL += ModificarEntero(MovimientoInventario, "inventario")
        strSQL += ModificarEntero(ValidaUnidad, "validaunidad")
        strSQL += ModificarEntero(AjustePrecio, "ajustaprecio")
        strSQL += ModificarEntero(Estado, "estado")
        strSQL += ModificarEntero(CreditoDebito, "credito_debito")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLEncabezadoConjunto(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal Codigo As String, ByVal descripcion As String, ByVal grupo As String, ByVal orden As String, _
        ByVal Gestion As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconcojcat SET "
        Else
            strSQLInicio = " UPDATE jsconcojcat set "
            strSQLFin = " WHERE " _
                & " codigo = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codigo")
        strSQL += ModificarCadena(descripcion, "descrip")
        strSQL += ModificarCadena(grupo, "grupo")
        strSQL += ModificarCadena(orden, "orden")
        strSQL += ModificarEnteroLargo(Gestion, "gestion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLTransportes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal Codigo As String, ByVal descripcion As String, ByVal Chofer As String, ByVal Acomnpañante As String, _
        ByVal Capacidad As Double, ByVal TipoVehiculo As Integer, ByVal Puestos As Integer, ByVal Marca As String, _
        ByVal Color As String, ByVal Placas As String, ByVal Serial1 As String, ByVal Serial2 As String, ByVal Serial3 As String, _
        ByVal Autonomia As Double, ByVal TipoCombustible As Integer, ByVal CapacidadTanque As Double, ByVal Modelo As String, _
        ByVal ValorInicial As Double, ByVal CodigoContable As String, ByVal ValorFinal As Double, ByVal FechaAdquisicion As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsconctatra SET "
        Else
            strSQLInicio = " UPDATE jsconctatra set "
            strSQLFin = " WHERE " _
                & " codtra = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codtra")
        strSQL += ModificarCadena(descripcion, "nomtra")
        strSQL += ModificarCadena(Chofer, "chofer")
        strSQL += ModificarCadena(Acomnpañante, "acompa")
        strSQL += ModificarDoble(Capacidad, "capacidad")
        strSQL += ModificarCadena(TipoVehiculo, "tipo")
        strSQL += ModificarEntero(Puestos, "puestos")
        strSQL += ModificarCadena(Marca, "marca")
        strSQL += ModificarCadena(Color, "color")
        strSQL += ModificarCadena(Placas, "placas")
        strSQL += ModificarCadena(Serial1, "serial1")
        strSQL += ModificarCadena(Serial2, "serial2")
        strSQL += ModificarCadena(Serial3, "serial3")
        strSQL += ModificarDoble(Autonomia, "autono")
        strSQL += ModificarCadena(TipoCombustible, "tipocon")
        strSQL += ModificarDoble(CapacidadTanque, "captanque")
        strSQL += ModificarCadena(Modelo, "modelo")
        strSQL += ModificarDoble(ValorInicial, "valorini")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")
        strSQL += ModificarDoble(ValorFinal, "valorfin")
        strSQL += ModificarFecha(FechaAdquisicion, "fechadq")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTROLRetencionISLR(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal Codigo As String, ByVal Nombre As String, ByVal TipoPersona As Integer, ByVal BaseImponible As Double, _
        ByVal Tarifa As Double, ByVal MontoMinimo As Double, ByVal Sustraendo As Double, ByVal Acumulativo As Integer, _
        ByVal Comentario As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jscontabret SET "
        Else
            strSQLInicio = " UPDATE jscontabret set "
            strSQLFin = " WHERE " _
                & " codret = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codret")
        strSQL += ModificarCadena(Nombre, "concepto")
        strSQL += ModificarEnteroLargo(TipoPersona, "tipo")
        strSQL += ModificarDoble(BaseImponible, "baseimp")
        strSQL += ModificarDoble(Tarifa, "tarifa")
        strSQL += ModificarDoble(MontoMinimo, "pagomin")
        strSQL += ModificarDoble(Sustraendo, "menos")
        strSQL += ModificarCadena(Acumulativo, "acumula")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCONTROLTerritorio(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal Codigo As Integer, ByVal Nombre As String, ByVal Antecesor As Integer, ByVal Nivel As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsconcatter SET codigo = null, "
        Else
            strSQLInicio = " UPDATE jsconcatter SET codigo = " & Codigo & ", "
            strSQLFin = " WHERE " _
                & " codigo = " & Codigo & " "
        End If
        strSQL += ModificarCadena(Nombre, "nombre")
        strSQL += ModificarEnteroLargo(Antecesor, "antecesor")
        strSQL += ModificarEnteroLargo(Nivel, "tipo")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(Myconn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditControlImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal Codigo As String, ByVal TipoImpresora As Integer, ByVal MaquinaFiscal As String, _
       ByVal UltimaFactura As String, ByVal UltimaNotaCredito As String, ByVal UltimoDocumentoNoFiscal As String, _
       ByVal EnUso As Integer, ByVal PuertoImpresoraFiscal As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsconcatimpfis SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsconcatimpfis SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codigo = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codigo")
        strSQL += ModificarEnteroLargo(TipoImpresora, "tipoimpresora")
        strSQL += ModificarCadena(MaquinaFiscal, "maquinafiscal")
        strSQL += ModificarCadena(UltimaFactura, "ultima_factura")
        strSQL += ModificarCadena(UltimaNotaCredito, "ultima_notacredito")
        strSQL += ModificarCadena(UltimoDocumentoNoFiscal, "ultimo_docnofiscal")
        strSQL += ModificarCadena(PuertoImpresoraFiscal, "puerto")
        strSQL += ModificarEntero(EnUso, "en_uso")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

End Module
