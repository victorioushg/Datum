Imports MySql.Data.MySqlClient
Public Class jsControlProVerificaBD
    Private Const sModulo As String = "Verificar Base de Datos"
    Private Const nTabla As String = "proVerifica"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se verifica la estructura de la base de datos para que sea funcional " + vbCr + _
                " con esta versión. " + vbCr + _
                " - Si no esta seguro POR FAVOR CONSULTE con el administrador " + vbCr + _
                " - Este proceso hace incompatible ciertas tablas con la versión anterior por lo tanto puede producir errores " + vbCr + _
                "   en dichas versiones. NO CORRA este proceso si todavía esta usando el sistema jytsuite " + vbCr + _
                " - Este proceso tarda un tiempo considerable y se recomienda que ningún usuario este conectado al sistema " + vbCr + _
                " - SI NO ESTA SEGURO por favor consulte CON EL ADMINISTRADOR "

        IniciarCHKs()

        MensajeEtiqueta(lblInfo, " ... ", TipoMensaje.iAyuda)

        Me.Show()

    End Sub
    Private Sub IniciarCHKs()
       
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProVerificarBD_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(myConn, MovAud.iSalir, sModulo, "")
        dt.Dispose()
    End Sub

    Private Sub jsControlProVerificarBD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()

    End Sub
    Private Sub Procesar()
        HabilitarCursorEnEspera()

        If chk1.Checked Then ActualizarContabilidad()
        If chk2.Checked Then ActualizarBancos()
        If chk3.Checked Then ActualizarNomina()
        If chk4.Checked Then ActualizarCompras()
        If chk5.Checked Then ActualizarVentas()
        If chk6.Checked Then ActualizarPuntosdeVenta()
        If chk7.Checked Then ActualizarMercancias()
        'If chk8.Checked Then ActualizarProduccion()
        'If chk9.Checked Then ActualizarMedicionGerencial()
        If chk10.Checked Then ActualizarControlDeGestiones()
        'If chk11.Checked Then ActualizarCilindros()
        'If chk12.Checked Then ActualizarServicios()


        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        IniciarCHKs()
        DeshabilitarCursorEnEspera()
        MensajeInformativoPlus(" Proceso culminado con éxito... ")

    End Sub
    Private Sub ProcesarTablas(ByVal aTablas() As String)
        Try
            Dim iCont As Integer
            For iCont = 0 To aTablas.Length - 1
                ProgressBar1.Value = (iCont + 1) / aTablas.Length * 100
                Application.DoEvents()
                Dim ff() As String = Split(aTablas(iCont), "|")
                Dim aFld(0 To ff.Length - 1) As String
                Array.Copy(ff, 1, aFld, 0, ff.Length - 1)
                If ExisteTabla(myConn, jytsistema.WorkDataBase, ff(0), lblInfo) Then
                    Dim kCont As Integer
                    Dim CampoAnterior As String = ""
                    For kCont = 0 To aFld.Length - 1
                        Dim gg() As String = Split(aFld(kCont), ".")
                        lblProgreso.Text = ff(0) & " " & gg(0)
                        ProgressBar1.Value = (kCont + 1) / aFld.Length * 100
                        Application.DoEvents()
                        If gg(0) <> "" Then

                            If ExisteCampo(myConn, lblInfo, jytsistema.WorkDataBase, CStr(ff(0)), CStr(gg(0))) Then
                                'Modifica
                                ModificaCampoTabla(myConn, lblInfo, jytsistema.WorkDataBase, CStr(ff(0)), CStr(gg(0)), aFld(kCont))
                            Else
                                'Agrega
                                AgregarCampoTabla(myConn, lblInfo, jytsistema.WorkDataBase, CStr(ff(0)), aFld(kCont), CampoAnterior)
                            End If
                        End If
                        CampoAnterior = gg(0)
                    Next
                Else
                    CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, False, CStr(ff(0)), aFld)
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub
    Private Sub ProcesarIndices(ByVal aIndices() As String)
        Dim iCont As Integer
        For iCont = 0 To aIndices.Length - 1
            Dim ff() As String = Split(aIndices(iCont), "|")

            If ff(0) <> "" Then
                ProgressBar1.Value = (iCont + 1) / aIndices.Length * 100
                lblProgreso.Text = ff(0) & " " & ff(1)
                Application.DoEvents()
                CrearIndice(myConn, lblInfo, jytsistema.WorkDataBase, ff(0), ff(1), ff(2), IIf(ff(1) <> "PRIMARY", 1, 0))
            End If

        Next

    End Sub
    Private Sub ActualizarContabilidad()
        Dim aTablas() As String = {"jscotcatcon|codcon.cadena30|descripcion.cadena50|nivel.entero2|marca.cadena|DEB00.doble19|CRE00.doble19|DEB01.doble19|CRE01.doble19|DEB02.doble19|CRE02.doble19|DEB03.doble19|CRE03.doble19|DEB04.doble19|CRE04.doble19|DEB05.doble19|CRE05.doble19|DEB06.doble19|CRE06.doble19|DEB07.doble19|CRE07.doble19|DEB08.doble19|CRE08.doble19|DEB09.doble19|CRE09.doble19|DEB10.doble19|CRE10.doble19|DEB11.doble19|CRE11.doble19|DEB12.doble19|CRE12.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jscotcatreg|plantilla.cadena5|REFERENCIA.cadena100|comen.memo|conjunto.cadena5|condicion.memo|formula.memo|agrupadopor.cadena50|CODCON.cadena30|ID_EMP.cadena2", _
                                   "jscotdaacon|codcon.cadena30|DEB00.doble19|CRE00.doble19|DEB01.doble19|CRE01.doble19|DEB02.doble19|CRE02.doble19|DEB03.doble19|CRE03.doble19|DEB04.doble19|CRE04.doble19|DEB05.doble19|CRE05.doble19|DEB06.doble19|CRE06.doble19|DEB07.doble19|CRE07.doble19|DEB08.doble19|CRE08.doble19|DEB09.doble19|CRE09.doble19|DEB10.doble19|CRE10.doble19|DEB11.doble19|CRE11.doble19|DEB12.doble19|CRE12.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jscotencasi|ASIENTO.cadena20|FECHASI.fecha|descripcion.cadena150|DEBITOS.doble19|CREDITOS.doble19|actual.cadena|PLANTILLA_ORIGEN.cadena5|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jscotencdef|ASIENTO.cadena20|descripcion.cadena150|FECHA_ULT_CON.fecha.FECHA ULTIMA CONTABILIZACION|INICIO_ULT_CON.fecha.INCIO ULTIMO PERIODO CONTABILIZADO|FIN_ULT_CON.fecha.FIN ULTIMO PERIODO CONTABILIZADO|ID_EMP.cadena2", _
                                   "jscotrenasi|ASIENTO.cadena20|RENGLON.cadena10|CODCON.cadena30|REFERENCIA.cadena100|CONCEPTO.memo|IMPORTE.doble19|LIBRO.cadena|DEBITO_CREDITO.cadena|actual.cadena|REGLA_ORIGEN.cadena5|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jscotrendef|ASIENTO.cadena20|RENGLON.cadena5|CODCON.cadena30|REFERENCIA.cadena100|concepto.memo|REGLA.cadena5|SIGNO.cadena|ACEPTADO.cadena|ID_EMP.cadena2"}

        ProcesarTablas(aTablas)

        Dim aIndices() As String = {"jscotcatcon|PRIMARY|`codcon`, `ejercicio`, `id_emp`", _
                                    "jscotdaacon|PRIMARY|`codcon`, `ejercicio`, `id_emp`", _
                                    "jscotencasi|PRIMARY|`asiento`,`actual`, `ejercicio`, `id_emp`", _
                                    "jscotrenasi|PRIMARY|`asiento`,`renglon`,`actual`, `ejercicio`, `id_emp`"}

        ProcesarIndices(aIndices)

    End Sub
    Private Sub ActualizarBancos()


        Dim aTablas() As String = {"jsbancatban|CODBAN.cadena5|NOMBAN.cadena50|CTABAN.cadena50|AGENCIA.cadena20|DIRECCION.cadena100|TELEF1.cadena20|TELEF2.cadena20|FAX.cadena20|titulo.cadena5|CONTACTO.cadena50|EMAIL.cadena50|WEBSITE.cadena50|COMISION.doble6|FECHACREA.fecha|SALDOACT.doble19|CODCON.cadena30|FORMATO.cadena2|ESTATUS.entero2|MONEDA.cadena5|ID_EMP.cadena2", _
                                   "jsbancatbantar|codban.cadena5|codtar.cadena5|com1.doble6|com2.doble6|id_emp.cadena2", _
                                   "jsbancatfor|formato.cadena2|DESCRIP.cadena100|MONTOTOP.entero6|MONTOLEFT.entero6|NOMBRETOP.entero6|NOMBRELEFT.entero6|MONTOLETRATOP.entero6|MONTOLETRALEFT.entero6|FECHATOP.entero6|FECHALEFT.entero6|NOENDOSABLETOP.entero6|NOENDOSABLELEFT.entero6|ID_EMP.cadena2", _
                                   "jsbanchedev|CODBAN.cadena5|NUMCHEQUE.cadena20|DEPOSITO.cadena20|PROV_CLI.cadena15|NUMCAN.cadena20|CAUSA.entero2|MONTO.doble19|FECHADEV.fecha|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsbandaaban|CODBAN.cadena5|DEB00.doble19|CRE00.doble19|DEB01.doble19|CRE01.doble19|DEB02.doble19|CRE02.doble19|DEB03.doble19|CRE03.doble19|DEB04.doble19|CRE04.doble19|DEB05.doble19|CRE05.doble19|DEB06.doble19|CRE06.doble19|DEB07.doble19|CRE07.doble19|DEB08.doble19|CRE08.doble19|DEB09.doble19|CRE09.doble19|DEB10.doble19|CRE10.doble19|DEB11.doble19|CRE11.doble19|DEB12.doble19|CRE12.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsbanenccaj|CAJA.cadena2|NOMCAJA.cadena50|SALDO.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsbantraban|FECHAMOV.fecha|NUMDOC.cadena30|TIPOMOV.cadena2|concepto.memo|IMPORTE.doble19|ORIGEN.cadena3|NUMORG.cadena20|BENEFIC.cadena150|comproba.cadena20|CONCILIADO.cadena|MESCONCILIA.fecha|FECCONCILIA.fecha|TIPORG.cadena2|ASIENTO.cadena15|FECHASI.fecha|MULTICAN.cadena|CODBAN.cadena5|CAJA.cadena2|PROV_CLI.cadena15|CODVEN.cadena5|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsbantracaj|CAJA.cadena5|RENGLON.cadena15|FECHA.fecha|ORIGEN.cadena3|TIPOMOV.cadena2|NUMMOV.cadena30|FORMPAG.cadena2|NUMPAG.cadena30|REFPAG.cadena50|IMPORTE.doble19|CODCON.cadena30|concepto.memo|DEPOSITO.cadena30|FECHA_DEP.fecha|CANTIDAD.entero4|CODBAN.cadena5|MULTICAN.cadena|ASIENTO.cadena15|FECHASI.fecha|PROV_CLI.cadena15|CODVEN.cadena5|ACEPTADO.cadena|ejercicio.cadena5|ID_EMP.cadena2", _
                                   "jsbantracon|FECHAMOV.fecha|NUMDOC.cadena30|TIPBAN.cadena2|concepto.memo|IMPORTE.doble19|CONCILIADO.cadena|ORIGEN.cadena3|CODBAN.cadena5|MESCONCILIA.fecha|FECCONCILIA.fecha|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsbanordpag|COMPROBA.cadena20|RENGLON.cadena5|CODCON.cadena30|REFER.cadena30|CONCEPTO.cadena250|IMPORTE.doble19|DEBITO_CREDITO.entero2|EJERCICIO.cadena5|ID_EMP.cadena2"}

        ProcesarTablas(aTablas)

        Dim aIndices() As String = {"jsbancatban|PRIMARY|`codban`, `id_emp`", _
                                    "jsbancatbantar|PRIMARY|`codban`, `codtar`,`id_emp`", _
                                    "jsbanchedev|PRIMARY|`codban`,`numcheque`, `numcan`, `fechadev`, `ejercicio`, `id_emp`", _
                                    "jsbandaaban|PRIMARY|`codban`, `ejercicio`, `id_emp`", _
                                    "jsbantraban|PRIMARY|`codban`, `fechamov`,`numdoc`,`tipomov`, `origen`,`numorg`,`prov_cli`,`ejercicio`,`id_emp`", _
                                    "jsbantracaj|PRIMARY|`caja`,`renglon`,`fecha`, `origen`,`tipomov`,`nummov`, `aceptado`, `ejercicio`,`id_emp`", _
                                    "jsbantracaj|SECOND|`numpag`, `id_emp`", _
                                    "jsbantracaj|THIRD|`deposito`, `id_emp`", _
                                    "jsbantracaj|NUMMOV|`nummov`,`tipomov`, `formpag`,`refpag`,`id_emp`", _
                                    "jsbanordpag|PRIMARY|`comproba`,`renglon`, `codcon`,`ejercicio`,`id_emp`"}

        ProcesarIndices(aIndices)

    End Sub
    Private Sub ActualizarNomina()

        Dim aTablas() As String = {"jsnomcatcam|CODIGO.cadena5|CAMPO.cadena20|DESCRIPCION.cadena50|TIPO.entero2|LONGITUD.entero4|DECIMALES.entero2|ID_EMP.cadena2", _
                                   "jsnomcatcon|CODCON.cadena5|NOMCON.cadena50|tipo.cadena|cuota.cadena5|CONJUNTO.cadena5|CONDICION.cadena250|FORMULA.cadena250|AGRUPADOPOR.cadena50|CODPRO.cadena15|CONCEPTO_POR_ASIG.cadena5|CODIGOCON.cadena30|ESTATUS.cadena|ID_EMP.cadena2", _
                                   "jsnomcatcot|CONSTANTE.cadena50|TIPO.entero2|VALOR.cadena50|ID_EMP.cadena2", _
                                   "jsnomcatdes|CODCON.cadena5|NOMCON.cadena50|CALCULO.cadena250|MENSAJE.cadena50|RESUMEN.cadena50|ID_EMP.cadena2", _
                                   "jsnomcattra|CODTRA.cadena10|CODHP.cadena15|APELLIDOS.cadena50|NOMBRES.cadena50|PROFESION.cadena50|INGRESO.fecha|FNACIMIENTO.fecha|LUGARNAC.cadena50|EDONAC.cadena30|PAIS.cadena50|NACIONALIZADO.cadena|NACIONAL.cadena|NOGACETA.cadena10|CEDULA.cadena10|edocivil.cadena|SEXO.cadena|ASCENDENTES.entero2|DESCENDENTES.entero2|NOSSO.cadena10|STATUSSSO.cadena|vivienda.cadena|VEHICULOS.entero2|DIRECCION.cadena250|TELEF1.cadena30|TELEF2.cadena30|EMAIL.cadena50|condicion.cadena|TIPONOM.entero2|FORMAPAGO.entero2|BANCO.cadena5|CTABAN.cadena30|BANCO_1.cadena5|CTABAN_1.cadena30|BANCO_2.cadena5|CTABAN_2.cadena30|CONYUGE.cadena100|CO_NACION.cadena|CO_CEDULA.cadena10|CO_PROFESION.cadena50|CO_FECNAC.fecha|CO_LUGNAC.cadena30|CO_EDONAC.cadena30|CO_PAIS.cadena30|GRUPO.cadena5|SUBNIVEL1.cadena30|SUBNIVEL2.cadena30|SUBNIVEL3.cadena30|SUBNIVEL4.cadena30|SUBNIVEL5.cadena30|SUBNIVEL6.cadena30|ESTRUCTURA.cadena150|CARGO.cadena50|SUELDO.doble19|RETISLR.doble6|FOTO.longblob|TURNODESDE.fecha|FREEDAYS.entero2|periodo.cadena|DATEFREEDAY.fecha|ROTATORIO.entero2|FECHARET.fecha|ID_EMP.cadena2", _
                                   "jsnomcattur|CODTUR.cadena5|NOMBRE.cadena50|tipo.entero2|HORADIURNA.tiempo|HORANOCTURNA.tiempo|MARCATURNO.entero2|MARCADESCANSO.entero2|TOL_ENT.entero2|TOL_SAL.entero2|TOL_INI_DES.entero2|TOL_FIN_DES.entero2|L.entero2|L_E.tiempo|L_S.tiempo|L_DS.tiempo|L_DE.tiempo|M.entero2|M_E.tiempo|M_S.tiempo|M_DS.tiempo|M_DE.tiempo|I.entero2|I_E.tiempo|I_S.tiempo|I_DS.tiempo|I_DE.tiempo|J.entero2|J_E.tiempo|J_S.tiempo|J_DS.tiempo|J_DE.tiempo|V.entero2|V_E.tiempo|V_S.tiempo|V_DS.tiempo|V_DE.tiempo|S.entero2|S_E.tiempo|S_S.tiempo|S_DS.tiempo|S_DE.tiempo|D.entero2|D_E.tiempo|D_S.tiempo|D_DS.tiempo|D_DE.tiempo|ID_EMP.cadena2", _
                                   "jsnomencgru|GRUPO.cadena5|DESCRIP.cadena50|MASK1.cadena15|MASK2.cadena15|MASK3.cadena15|MASK4.cadena15|MASK5.cadena15|MASK6.cadena15|DESCRIP1.cadena50|DESCRIP2.cadena50|DESCRIP3.cadena50|DESCRIP4.cadena50|DESCRIP5.cadena50|DESCRIP6.cadena50|ID_EMP.cadena2", _
                                   "jsnomencpre|CODTRA.cadena15|CODPRE.cadena5|descrip.cadena50|MONTOTAL.doble19|FECHAPRESTAMO.fecha|FECHAINICIO.fecha|TIPOINTERES.entero2|POR_INTERES.doble6|NUMCUOTAS.entero4|SALDO.doble19|estatus.entero2|ID_EMP.cadena2", _
                                   "jsnomestcar|CODIGO.enterolargo10A|NOMBRE.cadena50|CODIGOEMPRESA.cadena50|SUELDOBASE.doble19|ANTECESOR.enterolargo10|NIVEL.entero2|ID_EMP.cadena2", _
                                   "jsnomexptra|codtra.cadena30|fecha.fecha|COMENTARIO.memo|CAUSA.cadena5|ID_EMP.cadena2", _
                                   "jsnomfecnom|DESDE.fecha|HASTA.fecha|TIPONOM.cadena|ID_EMP.cadena2|", _
                                   "jsnomhisdes|CODTRA.cadena15|CODCON.cadena5|HASTA.fecha|IMPORTE.doble19|codpre.cadena5|num_cuota.entero2|ASIENTO.cadena10|FECHASI.fecha|GRUPO1.cadena5|GRUPO2.cadena5|GRUPO3.cadena5|PORCENTAJE_ASIG.doble6|CODIGOCON.cadena30|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsnomrengru|GRUPO.cadena5|COD_NIVEL_ANT.cadena15|COD_NIVEL.cadena30|DES_NIVEL.cadena50|NIVEL.entero2|ID_EMP.cadena2", _
                                   "jsnomrenpre|CODTRA.cadena15|CODPRE.cadena5|NUM_CUOTA.entero4|MONTO.doble19|CAPITAL.doble19|INTERES.doble19|PROCESADA.entero2|FECHAINICIO.fecha|FECHAFIN.fecha|ID_EMP.cadena2", _
                                   "jsnomtrades|CODTRA.cadena15|CODCON.cadena5|IMPORTE.doble19|PORCENTAJE_ASIG.doble6|ID_EMP.cadena2", _
                                   "jsnomtrahor|CODIGOHP.cadena15|FECHA.fecha|HORA.tiempo|TIPO.entero2|PROCESADA.entero2|ID_EMP.cadena2", _
                                   "jsnomtratur|CODTRA.cadena15|CODHP.cadena15|DIA.fecha|ENTRADA.fechahora|SALIDA.fechahora|DESCANSO.fechahora|RETORNO.fechahora|HORAS.tiempo|tipo.entero2|ID_EMP.cadena2", _
                                   "jsnomturtra|CODTRA.cadena15|CODTUR.cadena5|ID_EMP.cadena2", _
                                   "jsnomformula|formula.cadena50|parametros.cadena150|descripcion.cadena250|ID_EMP.cadena2"}

        ProcesarTablas(aTablas)

        '///////////// NOMINA
        EjecutarSTRSQL(myConn, lblInfo, "update jsnomcattra set sexo = '0' where sexo = 'M' ")
        EjecutarSTRSQL(myConn, lblInfo, "update jsnomcattra set sexo = '1' where sexo = 'F' ")
        EjecutarSTRSQL(myConn, lblInfo, "update jsnomcattra set sexo = '0' where sexo = '' ")

        Dim aIndices() As String = {"jsnomcatcam|PRIMARY|`codigo`, `campo`,  `id_emp`", _
                                 "jsnomcatcon|PRIMARY|`codcon`, `tipo`,`id_emp`", _
                                 "jsnomcatcot|PRIMARY|`CONSTANTE`,`ID_EMP`", _
                                 "jsnomcattra|PRIMARY|`codtra`,`id_emp`", _
                                 "jsnomcattur|PRIMARY|`codtur`,`id_emp`", _
                                 "jsnomencgru|PRIMARY|`grupo`,`id_emp`", _
                                 "jsnomencpre|PRIMARY|`codtra`,`codpre`,`id_emp`", _
                                 "jsnomexptra|PRIMARY|`codtra`,`fecha`, `causa`, `id_emp`", _
                                 "jsnomrengru|PRIMARY|`grupo`,`cod_nivel_ant`, `cod_nivel`, `nivel`, `id_emp`", _
                                 "jsnomrenpre|PRIMARY|`codtra`,`codpre`, `num_cuota`, `id_emp`", _
                                 "jsnomtrahor|PRIMARY|`codigohp`,`fecha`,`hora`, `id_emp`", _
                                 "jsnomtratur|PRIMARY|`codtra`,`codhp`,`dia`,`tipo`,`id_emp`", _
                                 "jsnomturtra|PRIMARY|`codtra`,`codtur`,`id_emp`", _
                                 "jsnomformula|PRIMARY|`formula`,`id_emp`"}

        ProcesarIndices(aIndices)


        '//////////////Funciones de Nomina
        Dim afld() As String = {"formula", "id_emp"}
        If Not ExisteFunction(myConn, lblInfo, jytsistema.WorkDataBase, "PrimerDiaMes") Then _
            EjecutarSTRSQL(myConn, lblInfo, " CREATE FUNCTION `PrimerDiaMes`(Fecha DATE) RETURNS date " _
                       & " COMMENT 'Devuelve primer día del mes a partir de fecha dada' " _
                       & " RETURN date_format(Fecha, '%Y-%m-01') ")

        Dim aStr() As String = {"PrimerDiaMes", jytsistema.WorkID}
        If qFound(myConn, lblInfo, "jsnomformula", afld, aStr) Then
            EjecutarSTRSQL(myConn, lblInfo, " update jsnomformula set parametros = '@Fecha', " _
                           & " descripcion = 'Devuelve el primer día del mes a partir de fecha dada' " _
                           & " where " _
                           & " Formula = 'PrimerDiaMes' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
        Else
            EjecutarSTRSQL(myConn, lblInfo, " insert into jsnomformula set Formula = 'PrimerDiaMes', parametros = '@Fecha', " _
                           & " descripcion = 'Devuelve el primer día del mes a partir de fecha dada', " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
        End If

        If Not ExisteFunction(myConn, lblInfo, jytsistema.WorkDataBase, "UltimoDiaMes") Then _
            EjecutarSTRSQL(myConn, lblInfo, " CREATE FUNCTION `UltimoDiaMes`(Fecha DATE) RETURNS date " _
                       & " COMMENT 'Devuelve último día del mes a partir de fecha dada' " _
                       & " RETURN DATE_SUB( DATE_ADD( date_format(Fecha, '%Y-%m-01'), INTERVAL 1 MONTH ), INTERVAL 1 DAY) ")

        Dim aStr1() As String = {"UltimoDiaMes", jytsistema.WorkID}
        If qFound(myConn, lblInfo, "jsnomformula", afld, aStr1) Then
            EjecutarSTRSQL(myConn, lblInfo, " update jsnomformula set parametros = '@Fecha', " _
                           & " descripcion = 'Devuelve el último día del mes a partir de fecha dada' " _
                           & " where " _
                           & " Formula = 'UltimoDiaMes' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
        Else
            EjecutarSTRSQL(myConn, lblInfo, " insert into jsnomformula set Formula = 'UltimoDiaMes', parametros = '@Fecha', " _
                           & " descripcion = 'Devuelve el último día del mes a partire de fecha dada', " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
        End If

        If Not ExisteFunction(myConn, lblInfo, jytsistema.WorkDataBase, "NumeroLunesEnMes") Then _
            EjecutarSTRSQL(myConn, lblInfo, " CREATE FUNCTION `NumeroLunesEnMes`(Fecha DATE) RETURNS int(11) " _
                        & " COMMENT 'Devuelve cantidad de lunes en el mes de fecha dada' " _
                        & " return WEEK ( DATE_SUB( UltimoDiaMes(Fecha), INTERVAL ( WEEKDAY(UltimoDiaMes(Fecha)) ) DAY)  ) -  " _
                        & " WEEK ( DATE_ADD( PrimerDiaMes(Fecha), INTERVAL (6  - IF ( WEEKDAY(PrimerDiaMes(Fecha)) > 0 ,  WEEKDAY(PrimerDiaMes(Fecha)) , 6 )  + IF ( WEEKDAY(PrimerDiaMes(Fecha)) > 0 , 1 , 0 )  ) DAY) )  +  1 ")

        Dim aStr2() As String = {"NumeroLunesEnMes", jytsistema.WorkID}
        If qFound(myConn, lblInfo, "jsnomformula", afld, aStr2) Then
            EjecutarSTRSQL(myConn, lblInfo, " update jsnomformula set parametros = '@Fecha', " _
                           & " descripcion = 'Devuelve la cantidad de lunes en el mes de fecha dada' " _
                           & " where " _
                           & " Formula = 'NumeroLunesEnMes' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
        Else
            EjecutarSTRSQL(myConn, lblInfo, " insert into jsnomformula set Formula = 'NumeroLunesEnMes', parametros = '@Fecha', " _
                           & " descripcion = 'Devuelve la cantidad de lunes en el mes de fecha dada', " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
        End If


        If Not ExisteFunction(myConn, lblInfo, jytsistema.WorkDataBase, "DiasHabilesRestantesMes") Then _
            EjecutarSTRSQL(myConn, lblInfo, " CREATE FUNCTION `DiasHabilesRestantesMes`(Fecha DATE, Empresa CHAR(2)) RETURNS INT(11) " _
                & " BEGIN " _
                & "     DECLARE DiasNoHabilesRestantes INT; " _
                & "     SELECT COUNT(*) INTO DiasNoHabilesRestantes " _
                & "     FROM jsconcatper " _
                & "     WHERE " _
                & " 	ano = YEAR(Fecha) AND " _
                & " 	mes = MONTH(Fecha) AND " _
                & "		dia > DAY(Fecha) AND " _
                & " 	id_emp = Empresa; " _
                & " 	RETURN (DAY(ultimodiames(Fecha)) - DAY(Fecha) + 1) - DiasNoHabilesRestantes; " _
                & " END ")



    End Sub

    Private Sub ActualizarCompras()

        Dim aTablas() As String = {"jsprotrapag|CODPRO.cadena15|TIPOMOV.cadena2|nummov.cadena30|EMISION.fecha|HORA.cadena10|VENCE.fecha|refer.cadena30|concepto.memo|IMPORTE.doble19|PORIVA.doble19|FORMAPAG.cadena2|NUMPAG.cadena20|NOMPAG.cadena20|benefic.cadena250|ORIGEN.cadena3|DEPOSITO.cadena20|CTADEP.cadena30|BANCODEP.cadena30|CAJAPAG.cadena2|numorg.cadena30|MULTICAN.cadena|ASIENTO.cadena15|FECHASI.fecha|CODCON.cadena30|MULTIDOC.cadena2|TIPDOCCAN.cadena2|INTERES.doble19|CAPITAL.doble19|COMPROBA.cadena20|BANCO.cadena5|CTABANCO.cadena30|REMESA.cadena20|CODVEN.cadena5|CODCOB.cadena5|FOTIPO.cadena|HISTORICO.cadena|TIPO.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprohispag|CODPRO.cadena15|TIPOMOV.cadena2|nummov.cadena30|EMISION.fecha|HORA.cadena10|VENCE.fecha|refer.cadena30|concepto.memo|IMPORTE.doble19|PORIVA.doble19|FORMAPAG.cadena2|NUMPAG.cadena20|NOMPAG.cadena20|benefic.cadena250|ORIGEN.cadena3|DEPOSITO.cadena20|CTADEP.cadena30|BANCODEP.cadena30|CAJAPAG.cadena2|numorg.cadena30|MULTICAN.cadena|ASIENTO.cadena15|FECHASI.fecha|CODCON.cadena30|MULTIDOC.cadena2|TIPDOCCAN.cadena2|INTERES.doble19|CAPITAL.doble19|COMPROBA.cadena20|BANCO.cadena5|CTABANCO.cadena30|REMESA.cadena20|CODVEN.cadena5|CODCOB.cadena5|FOTIPO.cadena|HISTORICO.cadena|TIPO.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprocatpro|CODPRO.cadena15|NOMBRE.cadena150|CATEGORIA.cadena10|UNIDAD.cadena5|RIF.cadena20|NIT.cadena20|ASIGNADO.cadena15|DIRFISCAL.cadena150|DIRPROVE.cadena150|EMAIL1.cadena50|EMAIL2.cadena50|EMAIL3.cadena50|EMAIL4.cadena50|EMAIL5.cadena50|SITIOWEB.cadena50|TELEF1.cadena50|TELEF2.cadena50|TELEF3.cadena50|FAX.cadena50|GERENTE.cadena50|TELGER.cadena50|CONTACTO.cadena50|TELCON.cadena50|LIMCREDITO.doble19|DISPONIBLE.doble19|DESC1.doble6|DESC2.doble6|DESC3.doble6|DESC4.doble6|DIAS2.entero2|DIAS3.entero2|OBSERVACION.cadena150|ZONA.cadena5|COBRADOR.cadena50|VENDEDOR.cadena50|SALDO.doble19|ULTPAGO.doble19|fecultpago.fecha|FORULTPAGO.cadena2|REGIMENIVA.cadena|FORMAPAGO.cadena2|BANCO.cadena5|CTABANCO.cadena50|BANCODEPOSITO1.cadena5|BANCODEPOSITO2.cadena5|CTABANCODEPOSITO1.cadena50|CTABANCODEPOSITO2.cadena50|INGRESO.fecha|CODCON.cadena30|ESTATUS.cadena|tipo.entero2|ID_EMP.cadena2", _
                                   "jsprodescom|NUMCOM.cadena30|CODPRO.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprodesgas|NUMGAS.cadena30|CODPRO.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproenccom|NUMCOM.cadena30|SERIE_NUMCOM.cadena5|EMISION.fecha|EMISIONIVA.fecha|CODPRO.cadena15|COMEN.cadena100|ALMACEN.cadena5|REFER.cadena15|CODCON.cadena30|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|POR_DES.doble6|DESCUEN.doble19|CARGOS.doble19|TOT_COM.doble19|POR_DES1.doble6|POR_DES2.doble6|POR_DES3.doble6|POR_DES4.doble6|DESCUEN1.doble19|DESCUEN2.doble19|DESCUEN3.doble19|DESCUEN4.doble19|VENCE1.fecha|VENCE2.fecha.FECHA DE CUOTA|CUOTA.entero.SI FUE PROCESADA PARA CUOTA|VENCE3.fecha.FECHA DE RECEPCION|RECIBIDO.entero.SI MERCANCIA FUE RECIBIDA|VENCE4.fecha.FECHA VENCIMIENTO|CONDPAG.entero2|TIPOCREDITO.entero2|FORMAPAG.cadena2|NUMPAG.cadena20|NOMPAG.cadena20|BENEFIC.cadena100|CAJA.cadena2|ABONO.doble19|SERIE.cadena3|NUMGIRO.entero4|PERGIRO.entero4|INTERES.doble19|PORINTERES.doble6|ASIENTO.cadena15|FECHASI.fecha|IMP_IVA.doble19|IMP_ICS.doble19|RET_IVA.doble19|NUM_RET_IVA.cadena20|FECHA_RET_IVA.fecha|RET_ISLR.doble19|NUM_RET_ISLR.cadena20|FECHA_RET_ISLR.fecha|POR_RET_ISLR.doble6|BASE_RET_ISLR.doble19|NUMCXP.cadena15|OTRA_CXP.cadena15|OTRO_PRO.cadena15|IMPRESA.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproencgas|NUMGAS.cadena30|SERIE_NUMGAS.cadena5|EMISION.fecha|EMISIONIVA.fecha|CODPRO.cadena15|NOMPRO.cadena100|RIF.cadena20|NIT.cadena20|COMEN.cadena100|REFER.cadena15|ALMACEN.cadena5|CODCON.cadena30|GRUPO.enterolargo10|SUBGRUPO.enterolargo10|TOT_NET.doble19|POR_DES.doble6|DESCUEN.doble19|CARGOS.doble19|TIPOIVA.cadena|POR_IVA.doble6|BASEIVA.doble19|IMP_IVA.doble19|imp_ics.doble19|RET_IVA.doble19|NUM_RET_IVA.cadena20|FECHA_RET_IVA.fecha|TOT_GAS.doble19|VENCE.fecha|CONDPAG.entero2|TIPOCREDITO.entero2|FORMAPAG.cadena2|NUMPAG.cadena20|NOMPAG.cadena20|BENEFIC.cadena100|CAJA.cadena2|ABONO.doble19|SERIE.cadena3|NUMGIRO.entero4|PERGIRO.entero4|INTERES.doble19|PORINTERES.doble6|ASIENTO.cadena15|FECHASI.fecha|RET_ISLR.doble19|NUM_RET_ISLR.cadena15|FECHA_RET_ISLR.fecha|POR_RET_ISLR.doble19|BASE_RET_ISLR.doble19|NUMCXP.cadena15|OTRA_CXP.cadena15|OTRO_PRO.cadena15|ZONA.cadena5|IMPRESA.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproencncr|NUMNCR.cadena30|SERIE_NUMNCR.cadena5|NUMCOM.cadena30|EMISION.fecha|EMISIONIVA.fecha|CODPRO.cadena15|COMEN.cadena100|CODVEN.cadena5|ALMACEN.cadena5|TRANSPORTE.cadena5|REFER.cadena15|CODCON.cadena30|TARIFA.cadena|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|IMP_IVA.doble19|TOT_NCR.doble19|VENCE.fecha|ESTATUS.entero2|ASIENTO.cadena15|FECHASI.fecha|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproencndb|NUMNDB.cadena30|SERIE_NUMNDB.cadena5|NUMCOM.cadena30|EMISION.fecha|emisioniva.fecha|CODPRO.cadena15|COMEN.cadena100|ALMACEN.cadena5|REFER.cadena15|CODCON.cadena30|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|IMP_IVA.doble19|TOT_NDB.doble19|VENCE.fecha|.ASIENTO.cadena15|FECHASI.fecha|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproencord|NUMORD.cadena30|SERIE_NUMORD.cadena5|EMISION.fecha|ENTREGA.fecha|CODPRO.cadena15|COMEN.cadena100|TOT_NET.doble19|IMP_IVA.doble19|TOT_ORD.doble19|ESTATUS.cadena|ITEMS.entero4|IMPRESA.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproencprg|NUMPRG.cadena15|FECHAPRGDESDE.fecha|FECHAPRGHASTA.fecha|ESTATUSPRG.entero2|BANCOPRG.cadena5|COMEN.cadena250|TOTALPRG.doble19|ID_EMP.cadena2", _
                                   "jsproencrep|NUMREC.cadena30|SERIE_NUMREC.cadena5|EMISION.fecha|CODPRO.cadena15|COMEN.cadena100|RESPONSABLE.cadena50|ALMACEN.cadena5|TOT_NET.doble19|IMP_IVA.doble19|TOT_REC.doble19|ESTATUS.cadena|NUMCOM.cadena30|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|IMPRESA.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprogrugas|CODIGO.enterolargo10A|NOMBRE.cadena50|ANTECESOR.enterolargo10|ID_EMP.cadena2", _
                                   "jsproicscom|NUMCOM.cadena30|codpro.cadena15|tipoics.cadena|porics.doble6|baseics.doble19|impics.doble19|retencion.doble19|numretencion.cadena20|ejercicio.cadena5|id_emp.cadena2", _
                                   "jsproivacom|NUMCOM.cadena30|CODPRO.cadena15|TIPOIVA.cadena|PORIVA.doble6|BASEIVA.doble19|IMPIVA.doble19|retencion.doble19|numretencion.cadena20|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproivagas|NUMGAS.cadena30|CODPRO.cadena15|TIPOIVA.cadena|PORIVA.doble6|BASEIVA.doble19|IMPIVA.doble19|retencion.doble19|numretencion.cadena20|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproivancr|NUMNCR.cadena30|CODPRO.cadena15|TIPOIVA.cadena|PORIVA.doble6|BASEIVA.doble19|IMPIVA.doble19|retencion.doble19|numretencion.cadena20|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproivandb|NUMNDB.cadena30|CODPRO.cadena15|TIPOIVA.cadena|PORIVA.doble6|BASEIVA.doble19|IMPIVA.doble19|retencion.doble19|numretencion.cadena20|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproivaord|NUMORD.cadena30|CODPRO.cadena15|TIPOIVA.cadena|PORIVA.doble6|BASEIVA.doble19|IMPIVA.doble19|retencion.doble19|numretencion.cadena20|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproivarec|NUMREC.cadena30|CODPRO.cadena15|TIPOIVA.cadena|PORIVA.doble6|BASEIVA.doble19|IMPIVA.doble19|retencion.doble19|numretencion.cadena20|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsproliscat|CODIGO.cadena10|DESCRIP.cadena50|ANTEC.cadena5|ID_EMP.cadena2", _
                                   "jsprolisuni|CODIGO.cadena5|DESCRIP.cadena50|ID_EMP.cadena50", _
                                   "jsprorencom|NUMCOM.cadena30|RENGLON.cadena5|CODPRO.cadena15|ITEM.cadena15|DESCRIP.cadena150|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|PESO.doble10|LOTE.cadena10|ESTATUS.cadena|COSTOU.doble19|DES_PRO.doble6|DES_ART.doble6|COSTOTOT.doble19|COSTOTOTDES.doble19|NUMORD.cadena30|RENORD.cadena5|NUMREC.cadena30|RENREC.cadena5|CODCON.cadena30|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprorendes|NUMDOC.cadena30|CODPRO.cadena15|ORIGEN.cadena3|ITEM.cadena15|RENGLON.cadena5|NUM_DESC.cadena5|PORDES.doble10|DESCUENTO.doble19|ID_EMP.cadena2", _
                                   "jsprorengas|NUMGAS.cadena30|RENGLON.cadena5|CODPRO.cadena15|ITEM.cadena15|DESCRIP.cadena150|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|PESO.doble10|LOTE.cadena10|ESTATUS.cadena|COSTOU.doble19|DES_PRO.doble6|DES_ART.doble6|COSTOTOT.doble19|COSTOTOTDES.doble19|CAUSA.cadena5|CODCON.cadena30|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprorenncr|NUMNCR.cadena30|RENGLON.cadena5|CODPRO.cadena15|ITEM.cadena15|DESCRIP.cadena150|IVA.cadena|ICS.cadena|UNIDAD.cadena3|CANTIDAD.doble10|PESO.doble10|LOTE.cadena10|ESTATUS.cadena|PRECIO.doble19|DES_ART.doble10|DES_PRO.doble10|POR_ACEPTA_DEV.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMCOM.cadena30|CODCON.cadena30|EDITABLE.entero2|CAUSA.cadena5|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprorenndb|NUMNDB.cadena30|RENGLON.cadena5|CODPRO.cadena15|ITEM.cadena15|DESCRIP.cadena150|IVA.cadena|ICS.cadena|UNIDAD.cadena3|CANTIDAD.doble10|PESO.doble10|LOTE.cadena10|ESTATUS.cadena|COSTO.doble19|DES_PRO.doble10|DES_ART.doble10|TOTREN.doble19|TOTRENDES.doble19|NUMCOM.cadena30|CODCON.cadena30|EDITABLE.entero2|CAUSA.cadena5|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprorenord|NUMORD.cadena30|RENGLON.cadena5|CODPRO.cadena15|ITEM.cadena15|DESCRIP.cadena150|IVA.cadena|ICS.cadena|UNIDAD.cadena3|CANTIDAD.doble10|PESO.doble10|CANTRAN.doble10|ESTATUS.cadena|COSTOU.doble19|DES_PRO.doble10|DES_ART.doble10|COSTOTOT.doble19|COSTOTOTDES.doble19|LOTE.cadena10|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprorenprg|NUMPRG.cadena15|CODPRO.cadena15|NUMMOV.cadena30|TIPOMOV.cadena2|EMISION.fecha|VENCE.fecha|IMPORTE.doble19|SALDO.doble19|EMISIONCH.fecha|BANCO.cadena5|A_CANCELAR.doble19|ID_EMP.cadena2", _
                                   "jsprorenrep|NUMREC.cadena30|RENGLON.cadena5|CODPRO.cadena15|ITEM.cadena15|DESCRIP.cadena150|IVA.cadena|ICS.cadena|UNIDAD.cadena3|CANTIDAD.doble10|CANTRAN.doble10|PESO.doble10|LOTE.cadena10|ESTATUS.cadena|COSTOU.doble19|DES_PRO.doble10|DES_ART.doble10|COSTOTOT.doble19|COSTOTOTDES.doble19|CODCON.cadena30|NUMORD.cadena30|RENORD.cadena15|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsprotrapagcan|codpro.cadena15|tipomov.cadena2|nummov.cadena30|emision.fecha|refer.cadena30|CONCEPTO.memo|importe.doble19|comproba.cadena15|id_emp.cadena2", _
                                   "jsproexppro|CODPRO.cadena15|FECHA.fechahora|COMENTARIO.memo|CONDICION.cadena|CAUSA.cadena5|TIPOCONDICION.cadena|ID_EMP.cadena2"}

        ProcesarTablas(aTablas)

        Dim aIndices() As String = {"jsprocatpro|PRIMARY|`CODPRO`, `TIPO`,  `ID_EMP`", _
                                    "jsprodescom|PRIMARY|`NUMCOM`,`CODPRO`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprodesgas|PRIMARY|`NUMGAS`,`CODPRO`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproenccom|PRIMARY|`NUMCOM`,`CODPRO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproencgas|PRIMARY|`NUMGAS`,`CODPRO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproencncr|PRIMARY|`NUMNCR`,`CODPRO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproencndb|PRIMARY|`NUMNDB`,`CODPRO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproencord|PRIMARY|`NUMORD`,`CODPRO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproencprg|PRIMARY|`NUMPRG`,`ID_EMP`", _
                                    "jsproencrep|PRIMARY|`NUMREC`,`CODPRO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproivacom|PRIMARY|`NUMCOM`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproivagas|PRIMARY|`NUMGAS`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproivancr|PRIMARY|`NUMNCR`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproivandb|PRIMARY|`NUMNDB`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproivaord|PRIMARY|`NUMORD`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsproivarec|PRIMARY|`NUMREC`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprorencom|PRIMARY|`NUMCOM`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprorendes|PRIMARY|`NUMDOC`,`CODPRO`,`ORIGEN`,`ITEM`,`RENGLON`,`NUM_DESC`,`ID_EMP`", _
                                    "jsprorengas|PRIMARY|`NUMGAS`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprorenncr|PRIMARY|`NUMNCR`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprorenndb|PRIMARY|`NUMNDB`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprorenord|PRIMARY|`NUMORD`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprorenprg|PRIMARY|`NUMPRG`,`CODPRO`,`NUMMOV`,`ID_EMP`", _
                                    "jsprorenrep|PRIMARY|`NUMREC`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprotrapag|PRIMARY|`CODPRO`,`TIPOMOV`,`nummov`,`EMISION`,`HORA`,`TIPO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprotrapag|SECOND|`CODPRO`,`nummov`,`ID_EMP`", _
                                    "jsprotrapag|THIRD|`CODPRO`,`EMISION`,`HISTORICO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprohispag|PRIMARY|`CODPRO`,`TIPOMOV`,`nummov`,`EMISION`,`HORA`,`TIPO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprohispag|SECOND|`CODPRO`,`nummov`,`ID_EMP`", _
                                    "jsprohispag|THIRD|`CODPRO`,`EMISION`,`HISTORICO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsprotrapagcan|PRIMARY|`CODPRO`,`TIPOMOV`,`nummov`,`EMISION`,`COMPROBA`,`ID_EMP`"}

        ProcesarIndices(aIndices)


    End Sub
    Private Sub ActualizarVentas()
        Dim aTablas() As String = {"jsvencatcli|CODCLI.cadena15|nombre.cadena250|CATEGORIA.cadena5|UNIDAD.cadena10|RIF.cadena15|NIT.cadena15|CI.cadena15|ALTERNO.cadena15|dirfiscal.cadena250|FPAIS.enterolargo10|FESTADO.enterolargo10|FMUNICIPIO.enterolargo10|FPARROQUIA.enterolargo10|FCIUDAD.enterolargo10|FBARRIO.enterolargo10|FZIP.cadena10|DPAIS.enterolargo10|DESTADO.enterolargo10|DMUNICIPIO.enterolargo10|DPARROQUIA.enterolargo10|DCIUDAD.enterolargo10|DBARRIO.enterolargo10|DZIP.cadena10|CODGEO.entero2|DIRDESPA.cadena250|EMAIL1.cadena50|EMAIL2.cadena50|EMAIL3.cadena50|EMAIL4.cadena50|TELEF1.cadena15|TELEF2.cadena15|TELEF3.cadena15|FAX.cadena15|GERENTE.cadena100|TELGER.cadena15|CONTACTO.cadena100|TELCON.cadena15|LIMITECREDITO.doble19|DISPONIBLE.doble19|CHEQDEV.entero4|CHEQMES.entero4|CHEQACU.entero4|DES_CLI.doble6|DESC_CLI_1.doble6|DESC_CLI_2.doble6|DESC_CLI_3.doble6|DESC_CLI_4.doble6|DESDE_1.entero2|HASTA_1.entero2|DESDE_2.entero2|HASTA_2.entero2|DESDE_3.entero2|HASTA_3.entero2|DESDE_4.entero2|HASTA_4.entero2|POR_CAR_DEV.doble6|ZONA.cadena5|RUTA_VISITA.cadena5|RUTA_DESPACHO.cadena5|NUM_DESPACHO.entero4|NUM_VISITA.entero4|COBRADOR.cadena5|VENDEDOR.cadena5|TRANSPORTE.cadena5|SALDO.doble19|ULTCOBRO.doble19|FECULTCOBRO.fecha|FORULTCOBRO.cadena2|REGIMENIVA.cadena|TARIFA.cadena|LISPRE.cadena5|FORMAPAGO.cadena2|BANCO.cadena5|CTABANCO.cadena50|INGRESO.fecha|CODCON.cadena30|CODCRE.entero2|ESTATUS.cadena|REQ_RIF.entero2|REQ_NIT.entero2|REQ_REC.entero2|REQ_CIS.entero2|REQ_REG.entero2|REQ_REA.entero2|REQ_BAN.entero2|REQ_COM.entero2|FECVISITA.entero2|INIVISITA.fecha|FECULTVISITA.fecha|MERCHANDISING.entero2|BACKORDER.entero2|DIAPAGO.entero2|DEPAGO.cadena5|APAGO.cadena5|RANKING.entero2|FECHARANK.fecha|COMENTARIO.cadena250|ESPECIAL.entero2|SHARE.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvencatclidiv|CODCLI.cadena15|DIVISION.cadena5|LIMITECREDITO.doble19|DISPONIBLE.doble19|SALDO.doble19|ASESOR.cadena5|RUTA_VISITA.cadena5|NUM_VISITA.entero4|FRECUENCIA_VISITA.entero2|FECHA_INICIO.fecha|FECHA_ULT_VISITA.fecha|RUTA_DESPACHO.cadena5|NUM_DESPACHO.entero4|TRANSPORTE.cadena5|ZONA.cadena5|DES_CLI.doble10|DESC_CLI_1.doble10|DESDE_1.entero4|HASTA_1.entero4|DESC_CLI_2.doble10|DESDE_2.entero4|HASTA_2.entero4|DESC_CLI_3.doble10|DESDE_3.entero4|HASTA_3.entero4|DESC_CLI_4.doble10|DESDE_4.entero4|HASTA_4.entero4|FORMAPAGO.cadena2|TARIFA.cadena2|LISTAPRECIOS.cadena5|ESTATUS.entero2|ID_EMP.cadena2", _
                                  "jsvencatcliSADA|CODCLI.cadena15|CODIGOSADA.cadena15|RAZON.cadena250|TIPO.cadena50|ESTADO.cadena50|MUNICIPIO.cadena50|CIUDAD.cadena50|ESTATUS.cadena30|ID_EMP.cadena2", _
                                  "jsvencatven|CODVEN.cadena5|DESCAR.cadena50|APELLIDOS.cadena50|NOMBRES.cadena50|DIRECCION.cadena100|TELEFONO.cadena15|CELULAR.cadena15|EMAIL.cadena50|FIANZA.doble19|TIPO.cadena|CLASE.entero2.VENDEDOR Ó SUPERVISOR Ó GERENTE|ZONA.cadena5|ESTRUCTURA.cadena50|CLAVE.cadena15|INGRESO.fecha|CARTERACLI.entero4|CARTERAART.entero4|CARTERAMAR.entero4|LISTA_A.entero2|LISTA_B.entero2|LISTA_C.entero2|LISTA_D.entero2|LISTA_E.entero2|LISTA_F.entero2|FACTORCUOTA.doble10|ESTATUS.entero2|DIVISION.cadena5|SUPERVISOR.cadena5|COM_VEN.doble10|COM_COB.doble10|ID_EMP.cadena2", _
                                  "jsvencatvendiv|codven.cadena5|division.cadena5|id_emp.cadena2", _
                                  "jsvencatvenjer|codven.cadena5|tipjer.cadena5|id_emp.cadena2", _
                                  "jsvencatvis|CODCLI.cadena15|DIA.entero2|DESDE.cadena5|HASTA.cadena5|DESDEPM.cadena5|HASTAPM.cadena5|TIPO.entero2|DIVISION.cadena5|ID_EMP.cadena2", _
                                  "jsvencaudcr|CODIGO.cadena5|DESCRIP.cadena100|INVENTARIO.entero2|VALIDAUNIDAD.entero2|AJUSTAPRECIO.entero2|ESTADO.entero2|CREDITO_DEBITO.entero2|ID_EMP.cadena2", _
                                  "jsvencedsoc|CODCLI.cadena15|NACIONAL.cadena|CI.cadena10|NOMBRE.cadena100|EXPEDIENTE.cadena|ID_EMP.cadena2", _
                                  "jsvencescom|CORREDOR.cadena5|TIPO.cadena2|DESDE.entero4|HASTA.entero4|COMISION.doble10|ID_EMP.cadena2", _
                                  "jsvencestic|CODIGO.cadena5|DESCRIP.cadena50|PORCOM.doble6|PORCOMCLI.doble6|LENCODBAR.entero4|INICIOPRECIO.entero4|LENPRECIO.entero4|INICIOTIPO.entero4|LENTIPO.entero4|CARGOS.doble19|TIPOIVA.cadena|CODCON.cadena30|CODPRO.cadena15|GRUPO.enterolargo10|SUBGRUPO.enterolargo10|ID_EMP.cadena2", _
                                  "jsvencestip|CORREDOR.cadena5|TIPO.cadena2|DESCRIP.cadena50|COM_CORREDOR.doble10|COM_CLIENTE.doble10|ID_EMP.cadena2", _
                                  "jsvenclirgv|CODVEN.cadena5|RUTA.cadena5|CODCLI.cadena15|NUMVISITA.entero2|VISITA.entero2|RAZON_NV.entero2|FECHA_PV.fecha|HORAENTRADA.cadena10|HORASALIDA.cadena10|FECHA.fecha|NUMPED.cadena15|NUMDEV.cadena15|DIA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvencobrgv|CODCLI.cadena15|TIPOMOV.cadena2|NUMMOV.cadena30|EMISION.fecha|HORA.cadena10|VENCE.fecha|REFER.cadena50|CONCEPTO.memo|IMPORTE.doble19|IMPIVA.doble19|FORMAPAG.cadena2|CAJAPAG.cadena5|NUMPAG.cadena30|NOMPAG.cadena30|BENEFIC.cadena250|ORIGEN.cadena3|NUMORG.cadena30|MULTICAN.cadena|ASIENTO.cadena15|FECHASI.fecha|CODCON.cadena30|MULTIDOC.cadena15|TIPDOCCAN.cadena2|INTERES.doble19|CAPITAL.doble19|COMPROBA.cadena15|BANCO.cadena5|CTABANCO.cadena50|REMESA.cadena15|CODVEN.cadena5|CODCOB.cadena5|HISTORICO.cadena|FOTIPO.cadena|EJERCICIO.cadena5|DIVISION.cadena5|ID_EMP.cadena2", _
                                  "jsvencomven|CODVEN.cadena5|TIPJER.cadena5|POR_VENTAS.doble10|TIPO.entero2|ID_EMP.cadena2", _
                                  "jsvencomvencob|CODVEN.cadena5|tipjer.cadena5|DE.entero2|A.entero2|POR_COBRANZA.doble6|ID_EMP.cadena2", _
                                  "jsvencuoart|CODVEN.cadena5|CODART.cadena15|ESMES01.doble10|ESMES02.doble10|ESMES03.doble10|ESMES04.doble10|ESMES05.doble10|ESMES06.doble10|ESMES07.doble10|ESMES08.doble10|ESMES09.doble10|ESMES10.doble10|ESMES11.doble10|ESMES12.doble10|REMES01.doble10|REMES02.doble10|REMES03.doble10|REMES04.doble10|REMES05.doble10|REMES06.doble10|REMES07.doble10|REMES08.doble10|REMES09.doble10|REMES10.doble10|REMES11.doble10|REMES12.doble10|ACT01.entero4|ACT02.entero4|ACT03.entero4|ACT04.entero4|ACT05.entero4|ACT06.entero4|ACT07.entero4|ACT08.entero4|ACT09.entero4|ACT10.entero4|ACT11.entero4|ACT12.entero4|TIPO.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvendesapt|NUMAPT.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvendescot|NUMCOT.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvendesfac|NUMFAC.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvendesnot|NUMFAC.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvendesped|NUMPED.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvendespedrgv|NUMPED.cadena15|RENGLON.cadena5|DESCRIP.cadena50|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvendivzon|CODCLI.cadena15|DIVISION.cadena5|NOMDIVISION.cadena50|ZONA.cadena5|RUTA.cadena5|ASESOR.cadena5|LIMITECREDITO.doble19|DISPONIBLE.doble19|SALDO.doble19|ID_EMP.cadena2", _
                                  "jsvenenccie|CODVEN.cadena5|FECHA.fecha|AVISITAR.entero2|VISITADOS.entero2|EFECTIVIDAD.doble6|ITEMS_CAJ.doble10|ITEMS_KGS.doble10|MONTO_CD.doble19|MONTO_COM_CD.doble19|CANTIDAD_CD.entero4|MARCAS_KGS.doble10|JERAR_CAJ.doble10|JERAR_KGS.doble10|PEDIDOS_CAJ.doble10|PEDIDOS_KGS.doble10|COSTOSVENTAS.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenenccot|NUMCOT.cadena15|EMISION.fecha|VENCE.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|TARIFA.cadena|TOT_NET.doble19|DESCUEN.doble19|CARGOS.doble19|IMP_IVA.doble19|TOT_COT.doble19|ESTATUS.cadena|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencnot|NUMFAC.cadena15|EMISION.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|ALMACEN.cadena5|TRANSPORTE.cadena5|VENCE.fecha|REFER.cadena15|CODCON.cadena30|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|PORDES.doble6|PORDES1.doble6|PORDES2.doble6|PORDES3.doble6|PORDES4.doble6|DESCUEN.doble19|DESCUEN1.doble19|DESCUEN2.doble19|DESCUEN3.doble19|DESCUEN4.doble19|CARGOS.doble19|CARGOS1.doble19|CARGOS2.doble19|CARGOS3.doble19|CARGOS4.doble19|TOT_FAC1.doble19|TOT_FAC2.doble19|TOT_FAC3.doble19|TOT_FAC4.doble19|VENCE1.fecha|VENCE2.fecha|VENCE3.fecha|VENCE4.fecha|CONDPAG.entero2|TIPOCRE.cadena|FORMAPAG.cadena2|NUMPAG.cadena50|NOMPAG.cadena50|CAJA.cadena5|IMPORTEEFECTIVO.doble19|NUMEROCHEQUE.cadena15|BANCOCHEQUE.cadena5|IMPORTECHEQUE.doble19|NUMEROTARJETA.cadena15|CODIGOTARJETA.cadena5|IMPORTETARJETA.doble19|NUMEROCESTATICKET.cadena15|NOMBRECESTATICKET.cadena15|IMPORTECESTATICKET.doble19|NUMERODEPOSITO.cadena15|BANCODEPOSITO.cadena5|IMPORTEDEPOSITO.doble19|ABONO.doble19|SERIE.cadena3|NUMGIRO.entero4|PERGIRO.entero4|INTERES.doble19|PORINT.doble6|ASIENTO.cadena15|FECHASI.fecha|BASEIVA.doble19|PORIVA.doble6|IMP_IVA.doble19|IMP_ICS.doble19|TIPOFAC.entero2|TIPO.entero2|TOT_FAC.doble19|ESTATUS.entero2|TARIFA.cadena|NUMCXC.cadena15|OTRA_CXC.cadena15|OTRO_CLI.cadena15|RELGUIA.cadena15|RELFACTURAS.cadena15|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencfac|NUMFAC.cadena15|EMISION.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|ALMACEN.cadena5|TRANSPORTE.cadena5|VENCE.fecha|REFER.cadena15|CODCON.cadena30|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|PORDES.doble6|PORDES1.doble6|PORDES2.doble6|PORDES3.doble6|PORDES4.doble6|DESCUEN.doble19|DESCUEN1.doble19|DESCUEN2.doble19|DESCUEN3.doble19|DESCUEN4.doble19|CARGOS.doble19|CARGOS1.doble19|CARGOS2.doble19|CARGOS3.doble19|CARGOS4.doble19|TOT_FAC1.doble19|TOT_FAC2.doble19|TOT_FAC3.doble19|TOT_FAC4.doble19|VENCE1.fecha|VENCE2.fecha|VENCE3.fecha|VENCE4.fecha|CONDPAG.entero2|TIPOCRE.cadena|FORMAPAG.cadena2|NUMPAG.cadena50|NOMPAG.cadena50|CAJA.cadena5|IMPORTEEFECTIVO.doble19|NUMEROCHEQUE.cadena15|BANCOCHEQUE.cadena5|IMPORTECHEQUE.doble19|NUMEROTARJETA.cadena15|CODIGOTARJETA.cadena5|IMPORTETARJETA.doble19|NUMEROCESTATICKET.cadena15|NOMBRECESTATICKET.cadena15|IMPORTECESTATICKET.doble19|NUMERODEPOSITO.cadena15|BANCODEPOSITO.cadena5|IMPORTEDEPOSITO.doble19|ABONO.doble19|SERIE.cadena3|NUMGIRO.entero4|PERGIRO.entero4|INTERES.doble19|PORINT.doble6|ASIENTO.cadena15|FECHASI.fecha|BASEIVA.doble19|PORIVA.doble6|IMP_IVA.doble19|IMP_ICS.doble19|TIPOFAC.entero2|TIPO.entero2|TOT_FAC.doble19|ESTATUS.entero2|TARIFA.cadena|NUMCXC.cadena15|OTRA_CXC.cadena15|OTRO_CLI.cadena15|RELGUIA.cadena15|RELFACTURAS.cadena15|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencgui|CODIGOGUIA.cadena15|DESCRIPCION.cadena100|ELABORADOR.cadena5|TRANSPORTE.cadena5|FECHAGUIA.fecha|EMISIONFAC.fecha|HASTAFAC.fecha|ITEMS.entero4|TOTALGUIA.doble19|TOTALKILOS.doble10|ESTATUS.entero2|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencguipedidos|CODIGOGUIA.cadena15|DESCRIPCION.cadena100|ELABORADOR.cadena5|TRANSPORTE.cadena5|FECHAGUIA.fecha|EMISIONFAC.fecha|HASTAFAC.fecha|ITEMS.entero4|TOTALGUIA.doble19|TOTALKILOS.doble10|ESTATUS.entero2|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencncr|NUMNCR.cadena30|NUMFAC.cadena15|EMISION.fecha|fechaiva.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|ALMACEN.cadena5|TRANSPORTE.cadena5|REFER.cadena15|CODCON.cadena30|TARIFA.cadena|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|IMP_IVA.doble19|imp_ics.doble19|TOT_NCR.doble19|VENCE.fecha|CONDPAG.entero2|TIPOCREDITO.entero2|FORMAPAG.cadena2|NUMPAG.cadena30|NOMPAG.cadena30|BENEFIC.cadena150|CAJA.cadena2|ORIGEN.cadena5|ESTATUS.entero2|ASIENTO.cadena15|FECHASI.fecha|IMPRESA.entero2|RELFACTURAS.cadena15|RELNCR.cadena15|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencncrrgv|NUMNCR.cadena30|NUMFAC.cadena15|EMISION.fecha|fechaiva.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|ALMACEN.cadena5|TRANSPORTE.cadena5|REFER.cadena15|CODCON.cadena30|TARIFA.cadena|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|IMP_IVA.doble19|imp_ics.doble19|TOT_NCR.doble19|VENCE.fecha|CONDPAG.entero2|TIPOCREDITO.entero2|FORMAPAG.cadena2|NUMPAG.cadena30|NOMPAG.cadena30|BENEFIC.cadena150|CAJA.cadena2|ORIGEN.cadena5|ESTATUS.entero2|ASIENTO.cadena15|FECHASI.fecha|IMPRESA.entero2|RELFACTURAS.cadena15|RELNCR.cadena15|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencndb|NUMNDB.cadena30|NUMFAC.cadena15|EMISION.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|ALMACEN.cadena5|TRANSPORTE.cadena5|REFER.cadena15|CODCON.cadena30|TARIFA.cadena|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|IMP_IVA.doble19|TOT_NDB.doble19|VENCE.fecha|ESTATUS.entero2|ASIENTO.cadena15|FECHASI.fecha|IMPRESA.entero2|RELFACTURAS.cadena15|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencped|NUMPED.cadena15|EMISION.fecha|ENTREGA.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|TARIFA.cadena|TOT_NET.doble19|PORDES.doble6|DESCUEN.doble19|CARGOS.doble19|IMP_IVA.doble19|TOT_PED.doble19|VENCE.fecha|PORDES1.doble6|PORDES2.doble6|PORDES3.doble6|PORDES4.doble6|DESCUEN1.doble19|DESCUEN2.doble19|DESCUEN3.doble19|DESCUEN4.doble19|VENCE1.fecha|VENCE2.fecha|VENCE3.fecha|VENCE4.fecha|ESTATUS.entero2|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|CONDPAG.entero2|TIPOCREDITO.entero2|FORMAPAG.cadena2|NOMPAG.cadena20|NUMPAG.cadena30|ABONO.doble19|SERIE.cadena3|NUMGIRO.entero4|PERGIRO.entero4|INTERES.doble19|PORINT.doble6|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencpedrgv|NUMPED.cadena15|EMISION.fecha|ENTREGA.fecha|CODCLI.cadena15|COMEN.cadena100|CODVEN.cadena5|TARIFA.cadena|TOT_NET.doble19|PORDES.doble6|DESCUEN.doble19|CARGOS.doble19|IMP_IVA.doble19|TOT_PED.doble19|VENCE.fecha|PORDES1.doble6|PORDES2.doble6|PORDES3.doble6|PORDES4.doble6|DESCUEN1.doble19|DESCUEN2.doble19|DESCUEN3.doble19|DESCUEN4.doble19|VENCE1.fecha|VENCE2.fecha|VENCE3.fecha|VENCE4.fecha|ESTATUS.entero2|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|CONDPAG.entero2|TIPOCREDITO.entero2|FORMAPAG.cadena2|NOMPAG.cadena20|NUMPAG.cadena30|ABONO.doble19|SERIE.cadena3|NUMGIRO.entero4|PERGIRO.entero4|INTERES.doble19|PORINT.doble6|IMPRESA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencrel|CODIGOGUIA.cadena15|DESCRIPCION.cadena100|ELABORADOR.cadena5|RESPONSABLE.cadena50|FECHAGUIA.fecha|EMISIONFAC.fecha|HASTAFAC.fecha|ITEMS.entero4|TOTALGUIA.doble19|TOTALKILOS.doble19|ESTATUS.entero2|IMPRESA.entero2|TIPO.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencrgv|CODVEN.cadena5|FECHA.fecha|CONDICION.entero2|DIA.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenencrut|CODRUT.cadena10|NOMRUT.cadena50|COMEN.cadena250|CODZON.cadena5|CODVEN.cadena5|CODCOB.cadena5|DIA.entero2|CONDICION.entero2|CODTRA.cadena5|TIPO.cadena|ITEMS.entero4|DIVISION.cadena5|ID_EMP.cadena2", _
                                  "jsvenexpcli|CODCLI.cadena15|FECHA.fechahora|COMENTARIO.memo|CONDICION.cadena|CAUSA.cadena5|TIPOCONDICION.cadena|ID_EMP.cadena2", _
                                  "jsvenforpag|numfac.cadena15|origen.cadena3|formapag.cadena2|numpag.cadena30|nompag.cadena30|importe.doble19|vence.fecha|id_emp.cadena2", _
                                  "jsvenforpagrgv|CODCLI.cadena15|NUMDOC.cadena30|ASESOR.cadena5|origen.cadena3|formapag.cadena2|numpag.cadena30|nompag.cadena30|importe.doble19|vence.fecha|id_emp.cadena2", _
                                  "jsvenicsfac|numfac.cadena15|tipoics.cadena|porics.doble6|baseics.doble19|impics.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenicsncr|NUMNCR.cadena30|tipoics.cadena|porics.doble6|baseics.doble19|impics.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenicsncrrgv|NUMNCR.cadena30|tipoics.cadena|porics.doble6|baseics.doble19|impics.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivacot|numcot.cadena15|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivafac|numfac.cadena15|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivancr|NUMNCR.cadena30|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivancrrgv|NUMNCR.cadena30|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivandb|NUMNDB.cadena30|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivanot|numfac.cadena15|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivaped|numped.cadena15|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenivapedrgv|numped.cadena15|tipoiva.cadena|poriva.doble6|baseiva.doble19|impiva.doble19|ejercicio.cadena5|id_emp.cadena2", _
                                  "jsvenliscan|CODIGO.cadena5|DESCRIP.cadena50|ID_EMP.cadena2", _
                                  "jsvenlistip|CODIGO.cadena10|DESCRIP.cadena50|ANTEC.cadena5|ID_EMP.cadena2", _
                                  "jsvenprocli|CODCLI.cadena15|CODPRO.cadena15|ID_EMP.cadena2", _
                                  "jsvenrencie|CODVEN.cadena5|FECHA.fecha|CLIENTE.cadena15|NOMBRE.cadena250|CAJAS.doble10|KILOS.doble10|COSTOS.doble19|VENTAS.doble19|CONDPAG.entero2|TOTALPEDIDO.doble19|TOTALDEV.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrencom|NUMDOC.cadena30|origen.cadena3|item.cadena15|renglon.cadena5|comentario.memo|id_emp.cadena2", _
                                  "jsvenrencot|NUMCOT.cadena15|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena250|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|CANTRAN.doble10|PESO.doble10|ESTATUS.cadena|PRECIO.doble19|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|TOTREN.doble19|TOTRENDES.doble19|ACEPTADO.cadena|EDITABLE.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrennot|NUMFAC.cadena15|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena250|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|INVENTARIO.doble10|SUGERIDO.doble10|REFUERZO.entero2|PRECIO.doble19|PESO.doble10|LOTE.cadena10|COLOR.cadena5|SABOR.cadena5|ESTATUS.cadena|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMCOT.cadena15|RENCOT.cadena5|NUMPED.cadena15|RENPED.cadena5|NUMNOT.cadena15|RENNOT.cadena5|CODCON.cadena30|FACDEV.cadena15|CAUSADEV.cadena5|ACEPTADO.cadena|EDITABLE.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenfac|NUMFAC.cadena15|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena250|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|INVENTARIO.doble10|SUGERIDO.doble10|REFUERZO.entero2|PRECIO.doble19|PESO.doble10|LOTE.cadena10|COLOR.cadena5|SABOR.cadena5|ESTATUS.cadena|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMCOT.cadena15|RENCOT.cadena5|NUMPED.cadena15|RENPED.cadena5|NUMNOT.cadena15|RENNOT.cadena5|CODCON.cadena30|FACDEV.cadena15|CAUSADEV.cadena5|ACEPTADO.cadena|EDITABLE.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrengui|CODIGOGUIA.cadena15|CODIGOFAC.cadena15|EMISION.fecha|CODCLI.cadena15|NOMCLI.cadena100|CODVEN.cadena5|KILOSFAC.doble10|TOTALFAC.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenguipedidos|CODIGOGUIA.cadena15|CODIGOFAC.cadena15|EMISION.fecha|CODCLI.cadena15|NOMCLI.cadena100|CODVEN.cadena5|KILOSFAC.doble10|TOTALFAC.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenncr|NUMNCR.cadena30|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena250|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|PESO.doble10|LOTE.cadena15|ESTATUS.cadena|PRECIO.doble19|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|POR_ACEPTA_DEV.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMFAC.cadena15|CODCON.cadena30|EDITABLE.entero2|CAUSA.cadena5|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenncrrgv|NUMNCR.cadena30|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena250|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|PESO.doble10|LOTE.cadena15|ESTATUS.cadena|PRECIO.doble19|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|POR_ACEPTA_DEV.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMFAC.cadena15|CODCON.cadena30|EDITABLE.entero2|CAUSA.cadena5|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenndb|NUMNDB.cadena30|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena250|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|PESO.doble10|LOTE.cadena15|ESTATUS.cadena|PRECIO.doble19|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|POR_ACEPTA_DEV.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMFAC.cadena15|CODCON.cadena30|EDITABLE.entero2|CAUSA.cadena5|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenped|NUMPED.cadena15|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena100|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|CANTRAN.doble10|INVENTARIO.doble10|SUGERIDO.doble10|REFUERZO.entero2|PESO.doble10|LOTE.cadena15|ESTATUS.cadena|PRECIO.doble19|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMCOT.cadena15|RENCOT.cadena5|CODCON.cadena30|ACEPTADO.cadena|EDITABLE.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenpedrgv|NUMPED.cadena15|RENGLON.cadena5|ITEM.cadena15|DESCRIP.cadena100|IVA.cadena|ICS.cadena|UNIDAD.cadena3|BULTOS.doble10|CANTIDAD.doble10|CANTRAN.doble10|INVENTARIO.doble10|SUGERIDO.doble10|REFUERZO.entero2|PESO.doble10|LOTE.cadena15|ESTATUS.cadena|PRECIO.doble19|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|TOTREN.doble19|TOTRENDES.doble19|NUMCOT.cadena15|RENCOT.cadena5|CODCON.cadena30|ACEPTADO.cadena|EDITABLE.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenrel|CODIGOGUIA.cadena15|CODIGOFAC.cadena15|EMISION.fecha|CODCLI.cadena15|NOMCLI.cadena100|CODVEN.cadena5|KILOSFAC.doble10|TOTALFAC.doble19|ACEPTADO.cadena|TIPO.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                  "jsvenrenrut|CODRUT.cadena10|NUMERO.entero4|CLIENTE.cadena15|NOMCLI.cadena150|TIPO.cadena|ACEPTADO.cadena|DIVISION.cadena5|CONDICION.entero2|ID_EMP.cadena2", _
                                  "jsvenrutcie|RUTACERRADA.cadena20", _
                                  "jsvenstaven|CODVEN.cadena5|FECHA.fecha|ITEM.cadena15|UNIDAD.cadena3|CUOTA.doble19|ACUMULADO.doble19|LOGRO.doble19|META.doble19|ACTIVADOS.entero4|ACTIVACION.doble6|CIERRE.doble19|TIPO.entero2|ID_EMP.cadena2", _
                                  "jsventabtic|TICKET.cadena30|CORREDOR.cadena5|MONTO.doble19|PORCOM.doble6|COMISION.doble19|NUMCAN.cadena15|CAJA.cadena5|NUMSOBRE.cadena15|FECHASOBRE.fecha|NUMDEP.cadena15|FECHADEP.fecha|BANCODEP.cadena5|CONDICION.entero2|FALSO.entero2|ID_EMP.cadena2", _
                                  "jsventracob|CODCLI.cadena15|TIPOMOV.cadena2|NUMMOV.cadena30|EMISION.fecha|HORA.cadena10|VENCE.fecha|REFER.cadena50|CONCEPTO.memo|IMPORTE.doble19|IMPIVA.doble19|FORMAPAG.cadena2|CAJAPAG.cadena5|NUMPAG.cadena30|NOMPAG.cadena30|benefic.cadena250|ORIGEN.cadena3|NUMORG.cadena30|MULTICAN.cadena|ASIENTO.cadena15|FECHASI.fecha|CODCON.cadena30|MULTIDOC.cadena15|TIPDOCCAN.cadena2|INTERES.doble19|CAPITAL.doble19|COMPROBA.cadena15|BANCO.cadena5|CTABANCO.cadena50|REMESA.cadena15|CODVEN.cadena5|CODCOB.cadena5|HISTORICO.cadena|FOTIPO.cadena|EJERCICIO.cadena5|DIVISION.cadena5|ID_EMP.cadena2", _
                                  "jsvenhiscob|CODCLI.cadena15|TIPOMOV.cadena2|NUMMOV.cadena30|EMISION.fecha|HORA.cadena10|VENCE.fecha|REFER.cadena50|CONCEPTO.memo|IMPORTE.doble19|IMPIVA.doble19|FORMAPAG.cadena2|CAJAPAG.cadena5|NUMPAG.cadena30|NOMPAG.cadena30|benefic.cadena250|ORIGEN.cadena3|NUMORG.cadena30|MULTICAN.cadena|ASIENTO.cadena15|FECHASI.fecha|CODCON.cadena30|MULTIDOC.cadena15|TIPDOCCAN.cadena2|INTERES.doble19|CAPITAL.doble19|COMPROBA.cadena15|BANCO.cadena5|CTABANCO.cadena50|REMESA.cadena15|CODVEN.cadena5|CODCOB.cadena5|HISTORICO.cadena|FOTIPO.cadena|EJERCICIO.cadena5|DIVISION.cadena5|ID_EMP.cadena2", _
                                  "jsventracobcan|codcli.cadena15|tipomov.cadena2|NUMMOV.cadena30|emision.fecha|refer.cadena15|concepto.memo|importe.doble19|comproba.cadena15|codven.cadena5|id_emp.cadena2", _
                                  "jsvenvaltic|CODIGO.cadena5|ENBARRA.cadena15|VALOR.doble19|ACEPTADO.cadena|ID_EMP.cadena2", _
                                  "jsvengruposfacturacion|agrupadopor.entero2|codigo.cadena15|ID_EMP.cadena2"}

        ProcesarTablas(aTablas)
        Dim aIndices() As String = {"jsvencatcli|PRIMARY|`codcli`, `ID_EMP`", _
                                    "jsvencatcliSADA|PRIMARY|`codcli`, `codigosada`, `ID_EMP`", _
                                    "jsvenencpedrgv|PRIMARY|`NUMPED`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencpedrgv|SECONDARY|`EMISION`,`ID_EMP`", _
                                    "jsvendespedrgv|PRIMARY|`NUMPED`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenivapedrgv|PRIMARY|`NUMPED`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenpedrgv|PRIMARY|`NUMPED`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenpedrgv|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencped|PRIMARY|`NUMPED`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencgui|PRIMARY|`CODIGOGUIA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencguipedidos|PRIMARY|`CODIGOGUIA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencped|SECONDARY|`EMISION`,`ID_EMP`", _
                                    "jsvendesped|PRIMARY|`NUMPED`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenivaped|PRIMARY|`NUMPED`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenped|PRIMARY|`NUMPED`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenped|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencncr|PRIMARY|`NUMNCR`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencncr|SECONDARY|`EMISION`,`ID_EMP`", _
                                    "jsvenicsncr|PRIMARY|`NUMNCR`,`TIPOICS`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenivancr|PRIMARY|`NUMNCR`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrengui|PRIMARY|`CODIGOGUIA`,`CODIGOFAC`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenguipedidos|PRIMARY|`CODIGOGUIA`,`CODIGOFAC`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenncr|PRIMARY|`NUMNCR`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenncr|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencncrrgv|PRIMARY|`NUMNCR`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenencncrrgv|SECONDARY|`EMISION`,`ID_EMP`", _
                                    "jsvenicsncrrgv|PRIMARY|`NUMNCR`,`TIPOICS`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenivancrrgv|PRIMARY|`NUMNCR`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenncrrgv|PRIMARY|`NUMNCR`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvenrenncrrgv|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`", _
                                    "jsvendescot|PRIMARY|`numcot`, `renglon`, `ejercicio`, `ID_EMP`", _
                                    "jsvencomven|PRIMARY|`codven`, `tipjer`, `tipo`, `ID_EMP`", _
                                    "jsvenenccot|PRIMARY|`numcot`, `ejercicio`, `ID_EMP`", _
                                    "jsvenivacot|PRIMARY|`numcot`, `tipoiva`, `ejercicio`, `ID_EMP`", _
                                    "jsvenrencot|PRIMARY|`numcot`, `renglon`, `item`, `estatus`,`aceptado`,`ejercicio`, `ID_EMP`", _
                                    "jsvenrencom|PRIMARY|`numdoc`, `origen`,`item`,`renglon`, `ID_EMP`", _
                                    "jsvengruposfacturacion|PRIMARY|`agrupadopor`, `codigo`, `ID_EMP`", _
                                    "jsvenstaven|PRIMARY|`CODVEN`, `ITEM`, `FECHA`, `TIPO`, `ID_EMP`", _
                                    "jsvenenccie|PRIMARY|`CODVEN`, `FECHA`, `EJERCICIO`, `ID_EMP`", _
                                    "jsvenrencie|PRIMARY|`CODVEN`, `FECHA`, `CLIENTE`, `EJERCICIO`, `ID_EMP`", _
                                    "jsventracob|PRIMARY|`CODCLI`, `TIPOMOV`, `NUMMOV`, `EMISION`, `HORA`, `EJERCICIO`, `ID_EMP`", _
                                    "jsventracob|SECOND|`NUMORG`, `EJERCICIO`, `ID_EMP`", _
                                    "jsventracob|TERCERO|`NUMMOV`, `EJERCICIO`, `ID_EMP`", _
                                    "jsventracob|CUARTO|`CODCLI`, `EMISION`, `HISTORICO`, `EJERCICIO`, `ID_EMP`", _
                                    "jsvenhiscob|PRIMARY|`CODCLI`, `TIPOMOV`, `NUMMOV`, `EMISION`, `HORA`, `EJERCICIO`, `ID_EMP`", _
                                    "jsvenhiscob|SECOND|`NUMORG`, `EJERCICIO`, `ID_EMP`", _
                                    "jsvenhiscob|TERCERO|`NUMMOV`, `EJERCICIO`, `ID_EMP`", _
                                    "jsvenhiscob|CUARTO|`CODCLI`, `EMISION`, `HISTORICO`, `EJERCICIO`, `ID_EMP`", _
                                    "jsventracobcan|PRIMARY|`CODCLI`, `TIPOMOV`, `NUMMOV`, `EMISION`, `ID_EMP`", _
                                    "jsventrapos|PRIMARY|`NUMMOV`, `CAJA`, `FECHA`, `TIPOMOV`, `FORMPAG`,`NUMPAG`, `NOMPAG`, `EJERCICIO`, `ID_EMP`", _
                                    "jsvenvaltic|PRIMARY|`CODIGO`, `ENBARRA`, `ID_EMP`", _
                                    "jsvenrutcie|PRIMARY|`RUTACERRADA`"}

        ProcesarIndices(aIndices)

    End Sub
    Private Sub ActualizarPuntosdeVenta()

        Dim aTablas() As String = {"jsvenaudpos|FECHA.fecha|CODVEN.cadena5|CODSUP.cadena5|TIPOMOV.cadena2|NUMDOC.cadena30|HORA.cadena10|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsvencatcaj|CODCAJ.cadena5|DESCRIP.cadena50|ALMACEN.cadena5|POS_FAC.entero|impre_fiscal.cadena5|ID_EMP.cadena2", _
                                   "jsvencatclipv|CODCLI.cadena15|nombre.cadena250|CATEGORIA.cadena5|UNIDAD.cadena10|RIF.cadena15|NIT.cadena15|CI.cadena10|ALTERNO.cadena10|dirfiscal.cadena250|TELEF1.cadena15|FAX.cadena15|INGRESO.fecha|ESTATUS.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsvencatsup|CODIGO.cadena5|DESCRIP.cadena50|NOMBRE.cadena100|CLAVE.cadena15|NIVEL.entero2|ID_EMP.cadena2", _
                                   "jsvendespos|NUMFAC.cadena15|NUMSERIAL.cadena15|tipo.entero2|RENGLON.cadena5|DESCRIP.cadena100|PORDES.doble6|DESCUENTO.doble19|SUBTOTAL.doble19|ACEPTADO.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsvenencpos|NUMFAC.cadena15|NUMSERIAL.cadena15|tipo.entero2|EMISION.fecha|CODCLI.cadena15|nomcli.cadena250|RIF.cadena15|NIT.cadena15|COMEN.cadena150|ALMACEN.cadena5|VENCE.fecha|REFER.cadena15|CODCON.cadena30|ITEMS.entero4|CAJAS.doble10|KILOS.doble10|TOT_NET.doble19|PORDES.doble6|DESCUEN.doble19|CARGOS.doble19|IMP_IVA.doble19|TOT_FAC.doble19|CONDPAG.entero2|TIPOCRE.cadena|ASIENTO.cadena15|FECHASI.fecha|ESTATUS.entero2|TARIFA.cadena|CODCAJ.cadena5|CODVEN.cadena5|IMPRESA.entero2|ejercicio.cadena5|ID_EMP.cadena2", _
                                   "jsveninipos|FECHA.fecha|CODVEN.cadena5|CODSUP.cadena5|MONTOINICIO.doble19|HORAINICIO.cadena10|HORACIERRE.cadena10|CODCAJ.cadena5|ESTATUS.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsvenivapos|NUMFAC.cadena15|NUMSERIAL.cadena15|tipo.entero2|TIPOIVA.cadena|PORIVA.doble6|BASEIVA.doble19|IMPIVA.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsvenrenpos|NUMFAC.cadena15|NUMSERIAL.cadena15|tipo.entero2|RENGLON.cadena5|ITEM.cadena15|BARRAS.cadena30|DESCRIP.cadena150|IVA.cadena|UNIDAD.cadena3|CANTIDAD.doble10|PRECIO.doble19|PESO.doble10|LOTE.cadena10|ESTATUS.cadena|DES_CLI.doble6|DES_ART.doble6|DES_OFE.doble6|TOTREN.doble19|TOTRENDES.doble19|CODCON.cadena30|ACEPTADO.cadena|EDITABLE.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsventrapos|CAJA.cadena5|FECHA.fecha|ORIGEN.cadena3|TIPOMOV.cadena2|NUMMOV.cadena30|FORMPAG.cadena2|NUMPAG.cadena30|NOMPAG.cadena30|IMPORTE.doble19|FECHACIERRE.fecha|CANTIDAD.entero4|CAJERO.cadena5|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsvenforpag|NUMFAC.cadena15|NUMSERIAL.cadena15|origen.cadena3|formapag.cadena2|numpag.cadena30|nompag.cadena30|importe.doble19|vence.fecha|id_emp.cadena2", _
                                   "jsvenperven|CODPER.cadena5|DESCRIP.cadena50|CR.entero2|CO.entero2|TARIFA_A.entero2|TARIFA_B.entero2|TARIFA_C.entero2|TARIFA_D.entero2|TARIFA_E.entero2|TARIFA_F.entero2|ALMACEN.cadena5|DESCUENTO.entero2|id_emp.cadena2", _
                                   "jsvenpervencaj|CODCAJ.cadena5|CODPER.cadena5|id_emp.cadena2", _
                                   "jsvenCADIPpos|TIPORAZON.cadena|DOCUMENTO.cadena15|IDENTIFICADOR.cadena|CANTIDAD.entero4|CODIGO.entero6|FECHAMOV.fecha|HORA.cadena10|FACTURA.cadena15|PROCESADO.entero|FECHAPROCESO.fechahora|ID_EMP.cadena2", _
                                   "jsvenCADIPposX|TIPORAZON.cadena|DOCUMENTO.cadena15|TIPOCOMERCIO.cadena|CODIGO_PRODUCTO.entero6|CANTIDAD_A_COMPRAR.entero4|CANTIDAD_COMPRADA.entero4|FECHAINICIOCOMPRA.fecha|FECHAULTIMACOMPRA.fecha|FECHAPROXIMACOMPRA.fecha|ESTATUS.entero2|ID_EMP.cadena2"}

        ProcesarTablas(aTablas)

        Dim aIndices() As String = {"jsvenencpos|PRIMARY|`numfac`, `numserial`, `tipo`, `codcli`, `ejercicio`, `ID_EMP`", _
                                    "jsvenrenpos|PRIMARY|`numfac`, `numserial`, `tipo`, `renglon`, `item`,`estatus`, `aceptado`, `ID_EMP`", _
                                    "jsvenivapos|PRIMARY|`numfac`, `numserial`, `tipo`, `tipoiva`, `ejercicio`, `ID_EMP`", _
                                    "jsvendespos|PRIMARY|`numfac`, `numserial`, `tipo`, `renglon`, `ejercicio`, `ID_EMP`", _
                                    "jsvenforpag|PRIMARY|`numfac`, `numserial`, `origen`, `formapag`, `numpag`, `nompag`, `ID_EMP`", _
                                    "jsventrapos|PRIMARY|`nummov`, `caja`,  `fecha`, `tipomov`, `formpag`, `numpag`, `nompag`,  `ejercicio`, `ID_EMP`", _
                                    "jsvencatclipv|PRIMARY|`CODCLI`,`RIF`,`ID_EMP`", _
                                    "jsvenperven|PRIMARY|`CODPER`,`ID_EMP`", _
                                    "jsvenpervencaj|PRIMARY|`CODCAJ`,`CODPER`,`ID_EMP`", _
                                    "jsvenCADIPpos|PRIMARY|`FACTURA`,`FECHAMOV`,`TIPORAZON`, `DOCUMENTO`, `IDENTIFICADOR`,`CODIGO`,`ID_EMP`", _
                                    "jsvenCADIPposX|PRIMARY|`TIPORAZON`,`DOCUMENTO`,`CODIGO_PRODUCTO`,`ID_EMP`"}

        ProcesarIndices(aIndices)

    End Sub

    Private Sub ActualizarMercancias()

        Dim aTablas() As String = {"jsmercatalm|CODALM.cadena5|DESALM.cadena50|RESPONSABLE.cadena100|TIPOALM.entero2|ID_EMP.cadena2", _
                                   "jsmercatcom|CODART.cadena15|CODARTCOM.cadena15|DESCRIP.cadena150|CANTIDAD.doble10|UNIDAD.cadena3|COSTO.doble19|PESO.doble10|ID_EMP.cadena2", _
                                   "jsmercatdiv|DIVISION.cadena5|DESCRIP.cadena150|color.cadena30|ID_EMP.cadena2", _
                                   "jsmercatser|CODSER.cadena15|DESSER.cadena150|PORCENTAJE.doble6|PRECIO.doble19|PRECIO_A.doble19|PRECIO_B.doble19|PRECIO_C.doble19|HORAS.doble10|HORAS_A.doble10|HORAS_B.doble10|HORAS_C.doble10|COL.entero2|CMS.entero2|TIPOIVA.cadena|tiposervicio.entero2|CODART_CODSER.cadena15|TIPO.entero2|CLASE.entero2|CODCON.cadena30|ID_EMP.cadena2", _
                                   "jsmerconmer|CONMER.cadena10|CODART.cadena15|NOMART.cadena150|UNIDAD.cadena3|CONTEO.doble10|EXISTENCIA.doble10|COSTOU.doble19|COSTO_TOT.doble19|EXIST1.doble10|CONT1.doble10|EXIST2.doble10|CONT2.doble10|EXIST3.doble10|CONT3.doble10|EXIST4.doble10|CONT4.doble10|EXIST5.doble10|CONT5.doble10|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerctacuo|CODART.cadena15|MES01.doble10|MES02.doble10|MES03.doble10|MES04.doble10|MES05.doble10|MES06.doble10|MES07.doble10|MES08.doble10|MES09.doble10|MES10.doble10|MES11.doble10|MES12.doble10|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerctainv|CODART.cadena15|NOMART.cadena150|ALTERNO.cadena15|BARRAS.cadena20|CREACION.fecha|GRUPO.cadena5|MARCA.cadena5|DIVISION.cadena5|IVA.cadena|ICS.cadena|PRESENTACION.cadena15|SUGERIDO.doble19|PRECIO_A.doble19|DESC_A.doble6|OFERTA_A.cadena10|GANAN_A.doble6|PRECIO_B.doble19|DESC_B.doble6|OFERTA_B.cadena10|GANAN_B.doble6|PRECIO_C.doble19|DESC_C.doble6|OFERTA_C.cadena10|GANAN_C.doble10|PRECIO_D.doble19|DESC_D.doble6|OFERTA_D.cadena10|GANAN_D.doble6|PRECIO_E.doble19|DESC_E.doble6|OFERTA_E.cadena10|GANAN_E.doble6|PRECIO_F.doble19|DESC_F.doble6|OFERTA_F.cadena10|GANAN_F.doble6|BARRA_A.cadena20|BARRA_B.cadena20|BARRA_C.cadena20|BARRA_D.cadena20|BARRA_E.cadena20|BARRA_F.cadena20|UNIDAD.cadena3|DIVIDEUV.cadena|UNIDADDETAL.cadena3|EXMAX.doble10|EXMIN.doble10|UBICACION.cadena15|PESOUNIDAD.doble10|CCUNIDAD.entero4|ALTURA.doble10|ANCHO.doble10|PROFUN.doble10|TIPOART.entero2|CUOTA.entero2|CUOTAFIJA.entero2|ORDENES.doble10|RECEPCIONES.doble10|BACKORDER.doble10|FECULTCOSTO.fecha|ULTIMOPROVEEDOR.cadena15|MONTOULTIMACOMPRA.doble19|COSTO_PROM.doble19|COSTO_PROM_DES.doble19|ENTRADAS.doble10|CREDITOSCOMPRAS.doble10|DEBITOSCOMPRAS.doble10|ACU_COS.doble19|ACU_COS_DES.doble19|ACU_COD.doble19|ACU_COD_DES.doble19|COTIZADOS.doble10|PEDIDOS.doble10|ENTREGAS.doble10|FECULTVENTA.fecha|ULTIMOCLIENTE.cadena15|MONTOULTIMAVENTA.doble19|VENTA_PROM.doble19|VENTA_PROM_DES.doble19|SALIDAS.doble10|CREDITOSVENTAS.doble10|DEBITOSVENTAS.doble10|ACU_PRE.doble19|ACU_PRE_DES.doble19|ACU_PRD.doble19|ACU_PRD_DES.doble19|EXISTE_ACT.doble10|CODCON.cadena30|ESTATUS.cadena|codjer1.cadena15|codjer2.cadena15|codjer3.cadena15|codjer4.cadena15|codjer5.cadena15|codjer6.cadena15|tipjer.cadena5|MIX.cadena|DEVOLUCION.cadena|DESCUENTO.entero2|POR_ACEPTA_DEV.doble6|REGULADO.entero2|ID_EMP.cadena2", _
                                   "jsmerctaser|CODART.cadena15|SERIAL1.cadena15|SERIAL2.cadena15|SERIAL3.cadena15|E_ORIGEN.cadena3|E_NUMDOC.cadena30|S_ORIGEN.cadena3|S_NUMDOC.cadena30|CONDICION.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerenccon|CONMER.cadena10|FECHACON.fecha|ALMACEN.cadena5|COMENTARIO.cadena150|PROCESADO.entero2|FECHAPRO.fecha|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerenccuo|CODCUO.cadena5|DESCRIPCION.cadena100|DESDE.fecha|HASTA.fecha|ID_EMP.cadena2", _
                                   "jsmerencjer|tipjer.cadena5|DESCRIP.cadena100|mascara1.cadena30|mascara2.cadena30|mascara3.cadena30|mascara4.cadena30|mascara5.cadena30|mascara6.cadena30|DESCRIP1.cadena30|DESCRIP2.cadena30|DESCRIP3.cadena30|DESCRIP4.cadena30|DESCRIP5.cadena30|DESCRIP6.cadena30|PROVEEDOR.cadena15|ID_EMP.cadena2", _
                                   "jsmerenclispre|CODLIS.cadena5|DESCRIP.cadena250|EMISION.fecha|VENCE.fecha|ID_EMP.cadena2", _
                                   "jsmerencofe|CODOFE.cadena15|DESCRIP.cadena150|DESDE.fecha|HASTA.fecha|TARIFA_A.cadena|TARIFA_B.cadena|TARIFA_C.cadena|TARIFA_D.cadena|TARIFA_E.cadena|TARIFA_F.cadena|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerenctra|NUMTRA.cadena15|EMISION.fecha|ALM_SALE.cadena5|ALM_ENTRA.cadena5|COMEN.cadena150|TOTALTRA.doble19|TOTALCAN.doble10|ITEMS.entero4|PESOTOTAL.doble10|tipo.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerequmer|CODART.cadena15|UNIDAD.cadena3|equivale.doble105|UVALENCIA.cadena3|DIVIDE.entero2|ID_EMP.cadena2", _
                                   "jsmerexpmer|CODART.cadena15|FECHA.fechahora|COMENTARIO.memo|CONDICION.cadena|CAUSA.cadena5|TIPOCONDICION.cadena5|ID_EMP.cadena2", _
                                   "jsmerextalm|CODART.cadena15|ALMACEN.cadena5|EXISTENCIA.doble10|UBICACION.cadena15|ID_EMP.cadena2", _
                                   "jsmerhismer|CODART.cadena15|FECHAMOV.fechahora|TIPOMOV.cadena2|NUMDOC.cadena30|UNIDAD.cadena3|CANTIDAD.doble10|PESO.doble10|COSTOTAL.doble19|COSTOTALDES.doble19|ORIGEN.cadena3|NUMORG.cadena20|LOTE.cadena15|PROV_CLI.cadena15|VENTOTAL.doble19|VENTOTALDES.doble19|IMPIVA.doble19|DESCUENTO.doble19|VENDEDOR.cadena5|ALMACEN.cadena5|ASIENTO.cadena15|FECHASI.fecha|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerlispre|FECHA.fecha|CODART.cadena15|TIPOPRECIO.cadena|MONTO.doble19|DES_ART.doble6|PROCESADO.entero2|ID_EMP.cadena2", _
                                   "jsmerlotmer|CODART.cadena15|LOTE.cadena15|EXPIRACION.fecha|ENTRADAS.doble10|SALIDAS.doble10|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerrenbon|CODOFE.cadena15|CODART.cadena15|RENGLON.cadena5|UNIDAD.cadena3|CANTIDAD.doble10|CANTIDADBON.doble10|CANTIDADINICIO.doble10|UNIDADBON.cadena3|ITEMBON.cadena15|NOMBREITEMBON.cadena150|OTORGACAN.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerrencuo|CODCUO.cadena5|alterno.cadena15|T1.cadena5|T2.cadena5|T3.cadena5|T4.cadena5|T5.cadena5|T6.cadena5|T7.cadena5|T8.cadena5|T9.cadena5|T10.cadena5|ID_EMP.cadena2", _
                                   "jsmerrenjer|tipjer.cadena5|CODJER.cadena30|DESJER.cadena50|NIVEL.entero2|ID_EMP.cadena2", _
                                   "jsmerrenlispre|CODLIS.cadena5|CODART.cadena15|PRECIO.doble19|ID_EMP.cadena2", _
                                   "jsmerrenofe|CODOFE.cadena15|RENGLON.cadena5|CODART.cadena15|DESCRIP.cadena150|UNIDAD.cadena3|LIMITEI.doble10|LIMITES.doble10|PORCENTAJE.doble6|OTORGAPOR.entero2|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerrentra|NUMTRA.cadena15|renglon.cadena5|ITEM.cadena15|DESCRIP.cadena150|UNIDAD.cadena3|CANTIDAD.doble10|PESO.doble10|LOTE.cadena15|COSTOU.doble19|TOTREN.doble19|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmertramer|CODART.cadena15|FECHAMOV.fechahora|TIPOMOV.cadena2|NUMDOC.cadena30|UNIDAD.cadena3|CANTIDAD.doble10|PESO.doble10|COSTOTAL.doble19|COSTOTALDES.doble19|ORIGEN.cadena3|NUMORG.cadena20|LOTE.cadena15|PROV_CLI.cadena15|VENTOTAL.doble19|VENTOTALDES.doble19|IMPIVA.doble19|DESCUENTO.doble19|VENDEDOR.cadena5|ALMACEN.cadena5|ASIENTO.cadena15|FECHASI.fecha|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsmerctainvfot|CODART.cadena15|FOTO1.blob|FOTO2.blob|FOTO3.blob|ID_EMP.cadena2",
                                   "jsmerCODSICA|CODIGO.cadena15|RUBRO.cadena30|CANTIDAD.entero2|FRECUENCIA.entero2|CATEGORIA.cadena5"}

        ProcesarTablas(aTablas)

        Dim aIndices() As String = {"jsmercatcom|PRIMARY|`codart`,`codartcom`, `id_emp`", _
                                    "jsmercatdiv|PRIMARY|`division`,`id_emp`", _
                                    "jsmercatser|PRIMARY|`codser`,`id_emp`", _
                                    "jsmerconmer|PRIMARY|`conmer`,`codart`,`ejercicio`, `id_emp`", _
                                    "jsmerctacuo|PRIMARY|`codart`,`ejercicio`, `id_emp`", _
                                    "jsmercatcom|PRIMARY|`codart`,`codartcom`, `id_emp`", _
                                    "jsmerctainv|PRIMARY|`codart`,`id_emp`", _
                                    "jsmerencconint|PRIMARY|`numcon`,`ejercicio`, `id_emp`", _
                                    "jsmerenccuo|PRIMARY|`codcuo`,`id_emp`", _
                                    "jsmerencjer|PRIMARY|`tipjer`,`id_emp`", _
                                    "jsmerenclispre|PRIMARY|`codlis`, `id_emp`", _
                                    "jsmerencofe|PRIMARY|`codofe`,`ejercicio`, `id_emp`", _
                                    "jsmerenctra|PRIMARY|`numtra`,`tipo`,`ejercicio`, `id_emp`", _
                                    "jsmerequmer|PRIMARY|`codart`,`unidad`, `uvalencia`, `id_emp`", _
                                    "jsmerequmer|PRIMARY|`codart`,`uvalencia`, `id_emp`", _
                                    "jsmerexpmer|PRIMARY|`codart`,`fecha`,`condicion`,`causa`, `id_emp`", _
                                    "jsmerextalm|PRIMARY|`codart`,`almacen`, `id_emp`", _
                                    "jsmerhismer|PRIMARY|`codart`,`fechamov`,`tipomov`,`numdoc`,`asiento`,`ejercicio`, `id_emp`", _
                                    "jsmerhismer|SECOND|`numdoc`,`tipomov`, `origen`,`numorg`, `ejercicio`, `id_emp`", _
                                    "jsmerhismer|THIRD|`numdoc`,`origen`, `numorg`, `ejercicio`, `id_emp`", _
                                    "jsmerhismer|FOURTH|`fechamov`,`tipomov`, `origen`, `prov_cli`, `id_emp`", _
                                    "jsmerhismer|FIFTH|`prov_cli`,`ejercicio`, `id_emp`", _
                                    "jsmerhismer|SIXTH|`fechamov`,`almacen`, `id_emp`", _
                                    "jsmerlispre|PRIMARY|`fecha`,`codart`,`tipoprecio`, `id_emp`", _
                                    "jsmerlotmer|PRIMARY|`codart`,`lote`,`ejercicio`, `id_emp`", _
                                    "jsmerrenconint|PRIMARY|`numcon`,`renglon`,`item`,`aceptado`, `ejercicio`, `id_emp`", _
                                    "jsmerrencuo|PRIMARY|`codcuo`,`alterno`, `id_emp`", _
                                    "jsmerrenlispre|PRIMARY|`codlis`,`codart`, `id_emp`", _
                                    "jsmerrenofe|PRIMARY|`codofe`,`renglon`, `codart`, `aceptado`,`ejercicio`, `id_emp`", _
                                    "jsmertramer|PRIMARY|`codart`,`fechamov`,`tipomov`,`numdoc`,`asiento`,`ejercicio`, `id_emp`", _
                                    "jsmertramer|SECOND|`numdoc`,`tipomov`, `origen`,`numorg`, `ejercicio`, `id_emp`", _
                                    "jsmertramer|THIRD|`numdoc`,`origen`, `numorg`, `ejercicio`, `id_emp`", _
                                    "jsmertramer|FOURTH|`fechamov`,`tipomov`, `origen`, `prov_cli`, `id_emp`", _
                                    "jsmertramer|FIFTH|`prov_cli`,`ejercicio`, `id_emp`", _
                                    "jsmertramer|SIXTH|`fechamov`,`almacen`, `id_emp`", _
                                    "jsmerCODSICA|PRIMARY|`codigo`"}

        ProcesarIndices(aIndices)


    End Sub

    Private Sub ActualizarControlDeGestiones()
        Dim aTablas() As String = {"jsconcatdes|CODDES.cadena5|DESCRIP.cadena50|PORDES.doble6|INICIO.fecha|FIN.fecha|CODVEN.cadena5|TIPO.cadena|ID_EMP.cadena2", _
                                   "jsconcatmon|codigo.cadena5|nombre.cadena50|abreviatura.cadena50|pais.cadena50", _
                                   "jsconcatper|MES.entero2|ANO.entero4|DIA.entero2|DESCRIPCION.cadena50|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsconcatter|CODIGO.enterolargo10A|NOMBRE.cadena50|MONTOFLETE.doble19|TIPO.cadena|ANTECESOR.cadena10|ZONA.cadena5|ID_EMP.cadena2", _
                                   "jsconcojcat|CODIGO.cadena5|DESCRIP.cadena50|GRUPO.cadena50|ORDEN.cadena50|GESTION.entero2|ID_EMP.cadena2", _
                                   "jsconcojtab|CODIGO.cadena5|LETRA.cadena|tabla.cadena250|tipo.entero2|relacion.cadena250|GESTION.entero2|ID_EMP.cadena2", _
                                   "jsconcontadores|gestion.cadena2|modulo.cadena2|codigo.cadena15|descripcion.cadena150|contador.cadena30|id_emp.cadena2", _
                                   "jsconctacam|fecha.fecha|moneda.cadena5|equivale.doble196|id_emp.cadena2", _
                                   "jsconctacom|CODIGO.cadena3|COMENTARIO.cadena150|ORIGEN.cadena3|ID_EMP.cadena2", _
                                   "jsconctaeje|EJERCICIO.cadena5|INICIO.fecha|CIERRE.fecha|ID_EMP.cadena2", _
                                   "jsconctaemp|ID_EMP.cadena2|NOMBRE.cadena100|DIRFISCAL.cadena250|DIRORIGEN.cadena250|CIUDAD.cadena20|ESTADO.cadena20|CODGEO.cadena10|ZIP.cadena10|TELEF1.cadena15|TELEF2.cadena15|FAX.cadena15|EMAIL.cadena50|ACTIVIDAD.cadena100|RIF.cadena15|NIT.cadena15|CIIU.cadena10|TIPOPERSONA.cadena|INICIO.fecha|CIERRE.fecha|TIPOSOC.cadena|LUCRO.cadena|NACIONAL.cadena|CI.cadena10|PASAPORTE.cadena10|CASADO.cadena|SEPARABIENES.cadena|RENTASEXENTAS.cadena|ESPOSADECLARA.cadena|REP_RIF.cadena15|REP_NIT.cadena15|REP_NACIONAL.cadena|REP_CI.cadena15|REP_NOMBRE.cadena150|REP_DIRECCION.cadena250|REP_CIUDAD.cadena15|REP_ESTADO.cadena15|REP_TELEF.cadena15|REP_FAX.cadena15|REP_EMAIL.cadena50|GER_NACIONAL.cadena|GER_CI.cadena15|GER_NOMBRE.cadena150|GER_DIRECCION.cadena250|GER_TELEF.cadena15|GER_CIUDAD.cadena15|GER_ESTADO.cadena15|GER_CEL.cadena15|GER_EMAIL.cadena50|LOGO.longblob|REPORTNAME.cadena|MONEDA.cadena5|UNIDAD.cadena3|fechatrabajo.fecha", _
                                   "jsconctaunt|FECHA.fecha|MONTO.doble196|ID_EMP.cadena2", _
                                   "jsconctafor|CODFOR.cadena2|NOMFOR.cadena50|TIPODOC.cadena2|PERIODO.entero4|GIROS.entero4|PERGIROS.entero4|INTERES.doble6|LIMITE.doble19|ID_EMP.cadena2", _
                                   "jsconctaics|fecha.fecha|tipo.cadena|monto.doble6", _
                                   "jsconctaiva|FECHA.fecha|TIPO.cadena|MONTO.doble6", _
                                   "jsconctamnu|mnuItem.cadena10|mnuItemParent.cadena20|mnuItemKey.cadena20|mnuItemName.cadena100|mnuItemKeyMask.entero2|mnuItemKeyCode.entero6|mnuItemLevel.entero2|mnuItemStyle.entero2|Gestion.entero2|mnuItemImage.cadena20", _
                                   "jsconctatab|CODIGO.cadena5|DESCRIP.cadena50|MODULO.cadena5|ID_EMP.cadena2", _
                                   "jsconctatar|CODTAR.cadena5|NOMTAR.cadena50|COM1.doble6|COM2.doble6|TIPO.cadena|ID_EMP.cadena2", _
                                   "jsconctatra|CODTRA.cadena5|NOMTRA.cadena50|CHOFER.cadena100|ACOMPA.cadena100|CAPACIDAD.doble10|TIPO.cadena|PUESTOS.entero2|MARCA.cadena15|COLOR.cadena15|PLACAS.cadena15|SERIAL1.cadena15|SERIAL2.cadena15|SERIAL3.cadena15|AUTONO.doble10|TIPOCON.cadena2|CAPTANQUE.doble10|MODELO.cadena5|VALORINI.doble19|CODCON.cadena30|EJERCICIO.cadena5|ID_EMP.cadena2|VALORFIN.doble19|FECHADQ.fecha", _
                                   "jsconctausu|USUARIO.cadena15|PASSWORD.memo|NOMBRE.cadena100|MAPA.cadena3|ID_USER.cadena5|INI_EMP.cadena2|ini_gestion.entero2|NIVEL.entero2|estatus.entero2|MONEDA.cadena5|DIVISION.cadena5", _
                                   "jsconencmap|MAPA.cadena3|NOMBRE.cadena100|INCLUYE.cadena|MODIFICA.cadena|ELIMINA.cadena", _
                                   "jsconnumcon|NUMDOC.cadena30|num_control.cadena30|prov_cli.cadena30|EMISION.fecha|ORG.cadena3|ORIGEN.cadena3|ID_EMP.cadena2", _
                                   "jsconnumcontrol|maquina.cadena50|numerocontrol.cadena15|id_emp.cadena2", _
                                   "jsconparametros|gestion.entero2|modulo.entero2|codigo.cadena15|descripcion.cadena150|tipo.entero2|valor.cadena100|id_emp.cadena2", _
                                   "jsconregaud|ID_USER.cadena5|MAQUINA.cadena100|FECHA.fechahora|GESTION.entero2|MODULO.cadena100|TIPOMOV.entero2|NUMDOC.cadena250|EJERCICIO.cadena5|ID_EMP.cadena2", _
                                   "jsconrenglonesmapa|mapa.cadena5|region.cadena50|descripcion.cadena150|gestion.entero2|nivel.entero2|modulo.entero2|orden.entero2|acceso.entero2|incluye.entero2|modifica.entero2|elimina.entero2", _
                                   "jscontabret|CODRET.cadena5|concepto.cadena250|BASEIMP.doble19|TARIFA.doble6|PAGOMIN.doble19|MENOS.doble19|PERSONA.cadena|ACUMULA.cadena|tipo.entero2|comentario.cadena100|CODCON.cadena20|ID_EMP.cadena2", _
                                   "jscontrapro|proceso.cadena2|fecha.fecha|id_emp.cadena2", _
                                   "jsconcatimpfis|codigo.cadena5|tipoimpresora.entero2|maquinafiscal.cadena30|ultima_factura.cadena15|ultima_notacredito.cadena15|ultimo_docnofiscal.cadena15|puerto.cadena5|EN_USO.entero|ID_EMP.cadena2"}

        ProcesarTablas(aTablas)

        Dim aIndices() As String = {"jsconcontadores|PRIMARY|`gestion`,`modulo`, `codigo`, `id_emp`", _
                                    "jsconctacom|PRIMARY|`codigo`,`origen`,`id_emp`", _
                                    "jsconctaeje|PRIMARY|`EJERCICIO`,`INICIO`,`CIERRE`,`ID_EMP`", _
                                    "jsconctaemp|PRIMARY|`ID_EMP`", _
                                    "jsconctaics|PRIMARY|`fecha`,`tipo`", _
                                    "jsconctaiva|PRIMARY|`fecha`,`tipo`", _
                                    "jsconctamnu|PRIMARY|`mnuItem`", _
                                    "jsconctatab|PRIMARY|`codigo`,`modulo`,`ID_EMP`", _
                                    "jsconnumcon|PRIMARY|`NUMDOC`,`PROV_CLI`,`EMISION`,`ORG`,`ORIGEN`,`ID_EMP`", _
                                    "jsconnumcontrol|PRIMARY|`maquina`,`ID_EMP`", _
                                    "jsconparametros|PRIMARY|`gestion`,`modulo`,`codigo`,`id_emp`", _
                                    "jsconregaud|PRIMARY|`ID_USER`,`MAQUINA`,`FECHA`,`GESTION`,`MODULO`,`TIPOMOV`,`NUMDOC`,`ID_EMP`", _
                                    "jscontabret|PRIMARY|`CODRET`,`PERSONA`,`ID_EMP`", _
                                    "jscontrapro|PRIMARY|`proceso`,`fecha`,`id_emp`", _
                                    "jsconcatimpfis|PRIMARY|`codigo`,`id_emp`"}

        ProcesarIndices(aIndices)

    End Sub

End Class