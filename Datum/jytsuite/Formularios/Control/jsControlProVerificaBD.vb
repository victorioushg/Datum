Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.ListView
Imports Syncfusion.WinForms.ListView.Enums

Public Class jsControlProVerificaBD
    Private Const sModulo As String = "Verificar Base de Datos"
    Private Const nTabla As String = "proVerifica"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private aApps As New List(Of String)()

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se verifica la estructura de la base de datos  "

        IniciarCHKs()
        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.tipoMensaje.iAyuda)
        Me.Show()

    End Sub
    Private Sub IniciarCHKs()

        aApps.Add("Contabilidad")
        aApps.Add("Bancos y Cajas")
        aApps.Add("Recursos Humanos")
        aApps.Add("Compras, Gastos y Cuentas por Pagar")
        aApps.Add("Ventas y Cuentas por Cobrar")
        aApps.Add("Puntos de Ventas")
        aApps.Add("Mercancías")
        aApps.Add("Producción")
        aApps.Add("Medición Gerencial")
        aApps.Add("Control de Gestiones")
        aApps.Add("Functiones y Procedimientos")
        aApps.Add("Servicios")

        lvApps.SelectionMode = SelectionMode.MultiSimple
        lvApps.CheckBoxSelectionMode = CheckBoxSelectionMode.CheckOnItemClick
        lvApps.ShowCheckBoxes = True
        lvApps.AllowSelectAll = True

        lvApps.DataSource = aApps

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProVerificarBD_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub
    Private Sub Procesar()
        HabilitarCursorEnEspera()

        Dim appsSelected = lvApps.CheckedItems.ToList()

        For Each aps As String In appsSelected
            Select Case aps
                Case "Contabilidad"
                    ActualizarContabilidad()
                Case "Bancos y Cajas"
                    ActualizarBancos()
                Case "Recursos Humanos"
                    ActualizarNomina()
                Case "Compras, Gastos y Cuentas por Pagar"
                    ActualizarCompras()
                Case "Ventas y Cuentas por Cobrar"
                    ActualizarVentas()
                Case "Puntos de Ventas"
                    ActualizarPuntosdeVenta()
                Case "Mercancías"
                    ActualizarMercancias()
                Case "Producción"
                    ActualizarProduccion()
                Case "Medición Gerencial"
                Case "Control de Gestiones"
                    ActualizarControlDeGestiones()
                Case "Functiones y Procedimientos"
                    ActualizarFunciones()
                Case "Servicios"
                    ActualizarMiscelaneos()
            End Select
        Next

        DeshabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")

    End Sub
    Private Sub ActualizarMiscelaneos()

        Dim nCont As Integer = 4
        Dim pb As New Progress_Bar
        pb.WindowTitle = "Misceláneos..."
        pb.TimeOut = 60


        pb.OverallProgressText = "Actualizando registro DATUM..."
        pb.OverallProgressValue = 1 / nCont * 100
        ActualizarRegistroDatum()

        '
        pb.OverallProgressText = "Verificando Unidades de Medida..."
        pb.OverallProgressValue = 2 / nCont * 100
        VerificaUnidadesDeMedida()

        ' 
        pb.OverallProgressText = "Actualizando Usuarios..."
        pb.OverallProgressValue = 3 / nCont * 100
        ActualizarUsuarios()

        ' 
        pb.OverallProgressText = "Actualizando nuevos valores..."
        pb.OverallProgressValue = 4 / nCont * 100
        '///////////// NOMINA
        ft.Ejecutar_strSQL(myConn, "update jsnomcattra set sexo = '0' where sexo = 'M' ")
        ft.Ejecutar_strSQL(myConn, "update jsnomcattra set sexo = '1' where sexo = 'F' ")
        ft.Ejecutar_strSQL(myConn, "update jsnomcattra set sexo = '0' where sexo = '' ")

        Dim aCausasNC() As String = {"00001.DESCUENTO Y/O EXONERACION.1.1.CXC.EX.EX00000001.EXONERACION",
                                     "00002.ANTICIPOS.0.0.CXC.AN.AN00000001.ANTICIPOS",
                                     "00001.DESCUENTO Y/O EXONERACION.1.1.CXP.EX.EX00000001.EXONERACION",
                                     "00002.ANTICIPOS.0.0.CXP.AN.AN00000001.ANTICIPOS",
                                     "00003.FALTANTE POR MERCANCIAS.1.1.CXP.FL.FL00000001.FALTANTE",
                                     "00004.SALDO A FAVOR.1.0.CXC.SA.SA00000001.SALDO",
                                     "00004.SALDO A FAVOR.1.0.CXP.SA.SA00000001.SALDO"}

        For Each aItem As String In aCausasNC
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsconcausas_notascredito where " _
                                       & " codigo = '" & aItem.Split(".")(0) & "' and " _
                                       & " origen = '" & aItem.Split(".")(4) & "' ") = 0 Then

                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsconcausas_notascredito SET " _
                                   & " CODIGO = '" & aItem.Split(".")(0) & "', " _
                                   & " DESCRIPCION = '" & aItem.Split(".")(1) & "', " _
                                   & " CR = '" & aItem.Split(".")(2) & "', " _
                                   & " VALIDA_DOCUMENTOS = '" & aItem.Split(".")(3) & "',  " _
                                   & " ORIGEN = '" & aItem.Split(".")(4) & "',  " _
                                   & " FORMAPAGO = '" & aItem.Split(".")(5) & "', " _
                                   & " NUMPAG = '" & aItem.Split(".")(6) & "', " _
                                   & " NOMPAG = '" & aItem.Split(".")(7) & "' ")

            End If
        Next

    End Sub
    Private Sub ActualizarFunciones()

        '//////////////Funciones de Nomina
        Dim nCont As Integer = 15
        Dim pb As New Progress_Bar
        pb.WindowTitle = "Misceláneos..."
        pb.TimeOut = 60

        Dim functionsList = New List(Of functionStructure) From
            {
                New functionStructure("PrimerDiaMes", "Fecha DATE", "Date", " RETURN date_format(Fecha, '%Y-%m-01');", "Devuelve primer día del mes a partir de fecha dada"),
                New functionStructure("PrimerDiaSemana", "Fecha DATE", "Date", " RETURN SUBDATE(Fecha,WEEKDAY(Fecha));", "Devuelve primer día de la semana a partir de fecha dada"),
                New functionStructure("UltimoDiaMes", "Fecha DATE", "Date", " RETURN DATE_SUB( DATE_ADD( date_format(Fecha, '%Y-%m-01'), INTERVAL 1 MONTH ), INTERVAL 1 DAY); ",
                                      "Devuelve último día del mes a partir de fecha dada"),
                New functionStructure("NumeroLunesEnMes", "Fecha DATE", "int(11) ", " RETURN WEEK (DATE_SUB( UltimoDiaMes(Fecha), INTERVAL ( WEEKDAY(UltimoDiaMes(Fecha)) ) DAY)  ) -  " _
                                & " WEEK (DATE_ADD( PrimerDiaMes(Fecha), INTERVAL (6 - IF ( WEEKDAY(PrimerDiaMes(Fecha)) > 0 ,  WEEKDAY(PrimerDiaMes(Fecha)) , 6 )  + IF ( WEEKDAY(PrimerDiaMes(Fecha)) > 0 , 1 , 0 )  ) DAY) )  +  1; ",
                                "Devuelve cantidad de lunes en el mes de fecha dada"),
                New functionStructure("NumeroLunesTrabajadorPeriodo", "FechaInicio DATE, FechaFinal DATE, Trabajador VARCHAR(15), Empresa CHAR(2) ", "int(11) ",
                                  "     DECLARE FechaInicial DATE DEFAULT FechaInicio; " _
                                & "     SELECT fecha_fin INTO FechaInicial " _
                                & "  	    FROM jsnomexptra " _
                                & "  	    WHERE " _
                                & "  	    causa IN (1,2,3,4) AND " _
                                & "  	    fecha_fin > FechaInicio AND " _
                                & "         fecha_fin <= FechaFinal AND " _
                                & "  	    codtra = Trabajador AND " _
                                & "  	    id_emp = Empresa " _
                                & "  	    ORDER BY fecha_fin DESC LIMIT 1; " _
                                & "     RETURN WEEK ( DATE_SUB( FechaFinal, INTERVAL ( WEEKDAY(FechaFinal) ) DAY)  ) -   " _
                                & "  		WEEK ( DATE_ADD( FechaInicial, INTERVAL (6  - IF ( WEEKDAY(FechaInicial) > 0 , " _
                                & "          WEEKDAY(FechaInicial) , 6 )  + IF ( WEEKDAY(FechaInicial) > 0 , 1 , 0 )  ) DAY) )  +  1; ",
                                "Devuelve la cantidad de lunes en un período de un Trabajador determinados"),
                New functionStructure("DiasHabilesRestantesMes", "Fecha DATE, Empresa CHAR(2)", " INT(11) ",
                                "     DECLARE DiasNoHabilesRestantes INT; " _
                                & "     SELECT IFNULL(COUNT(*),0) INTO DiasNoHabilesRestantes " _
                                & "     FROM jsconcatper " _
                                & "     WHERE " _
                                & "     modulo = 0 and " _
                                & " 	ano = YEAR(Fecha) AND " _
                                & " 	mes = MONTH(Fecha) AND " _
                                & "		dia > DAY(Fecha) AND " _
                                & " 	id_emp = Empresa; " _
                                & " 	RETURN (DAY(ultimodiames(Fecha)) - DAY(Fecha) + 1) - DiasNoHabilesRestantes; ",
                                "Devuelve los días restantes en el mes a partir de la fecha y la empresa dados"),
                New functionStructure("DiasEnPeriodo", "FechaInicial DATE, FechaFinal DATE", " INT(11) ", " RETURN ( TO_DAYS(FechaFinal)-  TO_DAYS(FechaInicial) + 1); ",
                                 "Devuelve los días dentro de un período desde la fecha inicial hasta la fecha final"),
                New functionStructure("DiasNoHabilesEnPeriodo", "FechaInicial DATE, FechaFinal DATE, Empresa CHAR(2)", " INT(11) ",
                                "     DECLARE DiasNoHabilesEnPeriodo INT; " _
                                & "     SELECT IFNULL(COUNT(*),0) INTO DiasNoHabilesEnPeriodo " _
                                & "     FROM jsconcatper " _
                                & "     WHERE " _
                                & "     MODULO = 0 AND " _
                                & "     DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia), '%Y-%m-%d') >=  FechaInicial AND " _
                                & "     DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia), '%Y-%m-%d') <=  FechaFinal AND " _
                                & "     TIPO = 0 AND " _
                                & " 	id_emp = Empresa; " _
                                & " 	RETURN DiasNoHabilesEnPeriodo; ",
                                "Devuelve los días NO HABILES dentro de un período desde la fecha inicial hasta la fecha final y para una empresa dada"),
                New functionStructure("DiasFeriadosEnPeriodo", "FechaInicial DATE, FechaFinal DATE, Empresa CHAR(2)", " INT(11) ",
                                "     DECLARE DiasFeriadosEnPeriodo INT; " _
                                & "     SELECT IFNULL(COUNT(*),0) INTO DiasFeriadosEnPeriodo " _
                                & "     FROM jsconcatper " _
                                & "     WHERE " _
                                & "     MODULO = 0 AND " _
                                & "     DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia), '%Y-%m-%d') >=  FechaInicial AND " _
                                & "     DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia), '%Y-%m-%d') <=  FechaFinal AND " _
                                & "     TIPO = 1 AND " _
                                & " 	id_emp = Empresa; " _
                                & " 	RETURN DiasFeriadosEnPeriodo; ",
                                "Devuelve los días FERIADOS dentro de un período desde la fecha inicial hasta la fecha final y para una empresa dada"),
                New functionStructure("DiasHabilesEnPeriodo", "FechaInicial DATE, FechaFinal DATE, Empresa CHAR(2)", " INT(11) ",
                                " 	RETURN ( DiasEnPeriodo(FechaInicial, FechaFinal) " _
                                & "             - DiasNoHabilesEnPeriodo(FechaInicial, FechaFinal, Empresa) " _
                                & "             - DiasFeriadosEnPeriodo(FechaInicial, FechaFinal, Empresa) " _
                                & "     ); ",
                                "Devuelve los días HABILES dentro de un período desde la fecha inicial hasta la fecha final y para una empresa dada"),
                New functionStructure("CuotaPrestamo", "CodigoNomina VARCHAR(5), CodigoTrabajador VARCHAR(15),  CodigoPrestamo VARCHAR(5), Empresa CHAR(2)", " DOUBLE(19,2)",
                                " 	RETURN ( SELECT b.monto FROM " _
                                & "             jsnomencpre a " _
                                & "             LEFT JOIN jsnomrenpre b ON (a.CODNOM = b.CODNOM AND a.CODTRA = b.CODTRA AND a.CODPRE = b.CODPRE AND a.ID_EMP = b.ID_EMP) " _
                                & "             LEFT JOIN jsnomcattra c ON (a.CODTRA = c.CODTRA AND a.ID_EMP = c.ID_EMP) " _
                                & "             WHERE " _
                                & "             b.procesada = 0 AND " _
                                & "             a.estatus = 1 AND " _
                                & "             a.codtra = CodigoTrabajador AND " _
                                & "             a.codnom = CodigoNomina AND " _
                                & "             a.codpre = CodigoPrestamo AND " _
                                & "             a.id_emp = Empresa " _
                                & "             ORDER BY a.fechainicio " _
                                & "             LIMIT 1 " _
                                & "     ); ",
                                "Devuelve el monto de la cuota pendiente del préstamo señalado del trabajador dados"),
                New functionStructure("tiempo_transcurrido", "FechaInicial DATE, FechaFinal DATE", " VARCHAR(20) ",
                                "     DECLARE anos INT; " _
                                & "     DECLARE meses INT; " _
                                & "     DECLARE dias INT; " _
                                & "     SET anos = YEAR(FechaFinal)  - YEAR(FechaInicial) -  IF(MONTH(FechaInicial)  > MONTH(FechaFinal), 1 , 0); " _
                                & "     SET meses =  MONTH(FechaFinal)  - MONTH(FechaInicial) -  IF(DAY(FechaInicial) > DAY(FechaFinal) , 1 , 0); " _
                                & "     SET dias = DAY(FechaFinal) - DAY(FechaInicial); " _
                                & "     IF dias < 0 THEN " _
                                & "         SET dias = dias + DAY(UltimoDiaMes(DATE_ADD(PrimerDiaMes(FechaFinal), INTERVAL -1 MONTH))); " _
                                & "         IF dias < 0 THEN " _
                                & "            IF dias = -1 Then SET dias = 30; " _
                                & "            ELSEIF dias = -2 THEN SET dias = 29; " _
                                & "            END IF; " _
                                & "         END IF; " _
                                & "     END IF; " _
                                & "     IF meses < 0 THEN " _
                                & "        IF meses >= -1 THEN SET anos = anos - 1; END IF; " _
                                & "        SET meses = meses + 12; " _
                                & "     END IF; " _
                                & " 	RETURN CONCAT(anos,'.', meses,'.', dias); ",
                                "Devuelve una cadena en la cual puedes verificar el tiempo transcurrido entre dos fecha de la forma años.meses.dias "),
                New functionStructure("esBisiesto", "Ano VARCHAR(4)", " INT ",
                                 " RETURN IF( DAY(UltimoDiaMes(CONCAT(Ano,'-02-01')) )=29, 1, 0);  " _
                                 , " Devuelve 1 si el año pasado en el parámetro es Bisiesto "),
                New functionStructure("split_STR", "x VARCHAR(255), delim VARCHAR(12), pos INT(3)", " VARCHAR(255)",
                                 " RETURN REPLACE(SUBSTRING(SUBSTRING_INDEX(x, delim, pos), " _
                                 & "       LENGTH(SUBSTRING_INDEX(x, delim, pos -1)) + 1), " _
                                 & "       delim, ''); ", "Devuelve un Array a partir de una cadena cuyos elementos estan delimitados por un caracter ")
        }

        For i As Integer = 0 To functionsList.Count - 1
            pb.OverallProgressText = functionsList(i).FunctionName
            pb.OverallProgressValue = (i + 1) / functionsList.Count * 100
            CrearFunctionEnBaseDatos(myConn, functionsList(i))
        Next

    End Sub

    Private Sub ActualizarUsuarios()

        Dim dtUsuarios As New DataTable
        dtUsuarios = ft.AbrirDataTable(ds, "tblUsers", myConn, " select * from jsconctausu order by id_user ")
        For Each nRow As DataRow In dtUsuarios.Rows
            With nRow
                If .Item("ini_emp").ToString.Trim <> "" Then

                    Dim Inserta As Boolean = Not CBool(ft.DevuelveScalarEntero(myConn, " select count(*) from " _
                                                                           & " jsconctausuemp where " _
                                                                           & " id_user = '" & .Item("id_user") & "' and " _
                                                                           & " id_emp ='" & .Item("ini_emp") & "' "))

                    If Not Inserta Then InsertEditCONTROLUsuarioEmpresas(myConn, lblInfo, Inserta, .Item("id_user"), .Item("ini_emp"), 1, 1)

                End If
            End With
        Next
        dtUsuarios.Dispose()
        dtUsuarios = Nothing

    End Sub
    Private Sub VerificaUnidadesDeMedida()

        For Each elem As String In aUnidadesIniciales
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsconctatab where modulo = '00035' and codigo = '" & elem.Split(".")(1) & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsconctatab SET " _
                                   & " CODIGO = '" & elem.Split(".")(1) & "', " _
                                   & " DESCRIP = '" & elem.Split(".")(0) & "', " _
                                   & " MODULO = '00035', " _
                                   & " ID_EMP = '" & jytsistema.WorkID & "' ")
            End If
        Next

    End Sub
    Private Sub ProcesarTablas(ByVal aTablas() As String, Titulo As String)

        Try

            Dim pb As New Progress_Bar
            pb.WindowTitle = "Tablas " & Titulo
            pb.TimeOut = 60
            pb.CallerThreadSet = Threading.Thread.CurrentThread


            Dim iCont As Integer
            For iCont = 0 To aTablas.Length - 1

                Dim ff() As String = Split(aTablas(iCont), "|")
                Dim aFld(0 To ff.Length - 1) As String
                Array.Copy(ff, 1, aFld, 0, ff.Length - 1)
                If ExisteTabla(myConn, jytsistema.WorkDataBase, ff(0)) Then

                    pb.OverallProgressText = ff(0) & " ..."
                    pb.OverallProgressValue = (iCont + 1) / aTablas.Length * 100

                    Dim kCont As Integer
                    Dim CampoAnterior As String = ""

                    For kCont = 0 To aFld.Length - 1
                        Dim gg() As String = Split(aFld(kCont), ".")

                        pb.PartialProgressText = "Procesando " & gg(0) & " ..."
                        pb.PartialProgressValue = (kCont + 1) / aFld.Length * 100

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

            'pb.PartialProgressValue = 100
            pb.OverallProgressValue = 100
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub
    Private Sub ProcesarIndices(ByVal aIndices() As String, Titulo As String)

        Dim pb As New Progress_Bar
        pb.WindowTitle = "Indices " & Titulo
        pb.TimeOut = 60

        Dim iCont As Integer
        For iCont = 0 To aIndices.Length - 1

            Dim ff() As String = Split(aIndices(iCont), "|")
            pb.OverallProgressText = "Procesando " & ff(0) & " / " & ff(1) & " ..."
            pb.OverallProgressValue = (iCont + 1) / aIndices.Length * 100
            If ff(0) <> "" Then
                CrearIndice(myConn, lblInfo, jytsistema.WorkDataBase, ff(0), ff(1), ff(2), IIf(ff(1) <> "PRIMARY", 1, 0))
            End If

        Next

    End Sub
    Private Sub ActualizarRegistroDatum()

        Dim Titulo As String = "Registro Sistema..."

        Dim aTablas() As String = {"datumreg|codigo.cadena.30.0.|empresa.cadena.250.0|aplicacion.entero.2.0|licencia.entero.2.0|numero_licencia.cadena.50.0|estacion.entero.2.0|mac_estacion.cadena.30.0|num_conexiones.entero.5.0|cadena_autenticacion.longblob.0.0|fecha_expiracion.fecha.0.0|version.cadena.20.0"}
        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"datumreg|PRIMARY|`codigo`, `estacion`, `mac_estacion`"}
        ProcesarIndices(aIndices, Titulo)

    End Sub

    Private Sub ActualizarContabilidad()

        Dim Titulo As String = "Contabilidad..."

        Dim aTablas() As String = {"jscotcatcon|codcon.cadena.30.0.CODIGO CONTABLE|descripcion.cadena.150.0.DESCRIPCION DE LA CUENTA CONTABLE|nivel.entero.2.0.NIVEL DE CUENTA CONTABLE QUE DEPENDE DE LA MASCARA|marca.cadena.1.0.SI CUENTA CONTABLE DE RESULTADO O NO|DEB00.doble.19.2.DEBITOS EJERCICIO ANTERIOR|CRE00.doble.19.2.CREDITOS EJERCICIO ANTERIOR|DEB01.doble.19.2.DEBITOS ENERO|CRE01.doble.19.2.CREDITOS ENERO|DEB02.doble.19.2.DEBITOS FEBRERO|CRE02.doble.19.2.CREDITOS FEBRERO|DEB03.doble.19.2.DEBITOS MARZO|CRE03.doble.19.2.CREDITOS MARZO|DEB04.doble.19.2.DEBITOS ABRIL|CRE04.doble.19.2.CREDITOS ABRIL|DEB05.doble.19.2.DEBITOS MAYO|CRE05.doble.19.2.CREDITOS MAYO|DEB06.doble.19.2.DEBITOS JUNIO|CRE06.doble.19.2.CREDITOS JUNIO|DEB07.doble.19.2.DEBITOS JULIO|CRE07.doble.19.2.CREDITOS JULIO|DEB08.doble.19.2.DEBITOS AGOSTO|CRE08.doble.19.2.CREDITOS AGOSTO|DEB09.doble.19.2.DEBITOS SEPTIEMBRE|CRE09.doble.19.2.CREDITOS SEPTIEMBRE|DEB10.doble.19.2.DEBITOS OCTUBRE|CRE10.doble.19.2.CREDITOS OCTUBRE|DEB11.doble.19.2.DEBITOS NOVIEMBRE|CRE11.doble.19.2.CREDITOS NOVIEMBRE|DEB12.doble.19.2.DEBITOS DICIEMBRE|CRE12.doble.19.2.CREDITOS DICIEMBRE|EJERCICIO.cadena.5.0.EJERCICIO CONTABLE|ID_EMP.cadena.2.0.CODIGO DE EMPRESA",
                                   "jscotcatreg|plantilla.cadena.5.0.CODIGO DE LA REGLA (Consulta SQL para Contabilidad)|REFERENCIA.cadena.100.0.DESCRIPCION DE LA REGLA|comen.memo.0.0.COMENTARIO PARA LA REGLA (NO USADO)|conjunto.cadena.5.0.CONJUNTO DE TABLAS Y RELACIONES QUE USA LA REGLA|condicion.memo.0.0.SENTENCIA `WHERE` DE LA CONSULTA|formula.memo.0.0.CAMPOS DE LA CONSULTA|agrupadopor.cadena.50.0.AGRUPADO POR (GROUP BY)|CODCON.cadena.30.0.CODIGO CONTABLE|ID_EMP.cadena.2.0.CODIGO DE EMPRESA",
                                   "jscotdaacon|codcon.cadena.30.0.CODIGO CONTABLE|DEB00.doble.19.2|CRE00.doble.19.2|DEB01.doble.19.2|CRE01.doble.19.2|DEB02.doble.19.2|CRE02.doble.19.2|DEB03.doble.19.2|CRE03.doble.19.2|DEB04.doble.19.2|CRE04.doble.19.2|DEB05.doble.19.2|CRE05.doble.19.2|DEB06.doble.19.2|CRE06.doble.19.2|DEB07.doble.19.2|CRE07.doble.19.2|DEB08.doble.19.2|CRE08.doble.19.2|DEB09.doble.19.2|CRE09.doble.19.2|DEB10.doble.19.2|CRE10.doble.19.2|DEB11.doble.19.2|CRE11.doble.19.2|DEB12.doble.19.2|CRE12.doble.19.2|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jscotencasi|ASIENTO.cadena.20.0|FECHASI.fecha.0.0|descripcion.cadena.150.0|DEBITOS.doble.19.2|CREDITOS.doble.19.2|actual.cadena.1.0|PLANTILLA_ORIGEN.cadena.5.0|BLOCK_DATE.fecha.0.0.ULTIMA FECHA DE BLOQUEO|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jscotencdef|ASIENTO.cadena.20.0|descripcion.cadena.150.0|FECHA_ULT_CON.fecha.0.0.fecha ULTIMA CONTABILIZACION|INICIO_ULT_CON.fecha.0.0.INICIO ULTIMO PERIODO CONTABILIZADO|FIN_ULT_CON.fecha.0.0.FIN ULTIMO PERIODO CONTABILIZADO|ID_EMP.cadena.2.0",
                                   "jscotrenasi|ASIENTO.cadena.20.0|RENGLON.cadena.10.0|CODCON.cadena.30.0|REFERENCIA.cadena.100.0|CONCEPTO.memo.0.0|IMPORTE.doble.19.2|LIBRO.cadena.1.0|DEBITO_CREDITO.cadena.1.0|actual.cadena.1.0|REGLA_ORIGEN.cadena.5.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jscotrendef|ASIENTO.cadena.20.0|RENGLON.cadena.5.0|CODCON.cadena.30.0|REFERENCIA.cadena.100.0|concepto.memo.0.0|REGLA.cadena.5.0|SIGNO.cadena.1.0|ACEPTADO.cadena.1.0|ID_EMP.cadena.2.0",
                                   "jscotactfij|CODIGO.cadena.20.0|DESCRIPCION.cadena.250.0|INGRESO.fecha.0.0|FECHA_ADQUISICION.fecha.0.0|VALOR_ADQUISICION.doble.19.2|ESTATUS.entero.2.0|GRUPO.cadena.5.0|SERIAL.cadena.50.0|MONEDA.cadena.5.0|TASA.doble.10.3|UBICACION.cadena.5.0|TIPO.entero.2.0|METODO.entero.2.0|VIDA_ANOS.entero.3.0|VIDA_MESES.entero.2.0|INICIO_VALUACION.fecha.0.0|CUENTA_ACTIVO_PASIVO.cadena.30.0|CUENTA_GASTOS_INGRESOS.cadena.30.0|CUENTA_REDEPRECIACION_ACUMULADA.cadena.30.0|ID_EMP.cadena.2.0",
                                   "jscottrafij|CODIGO.cadena.20.0|INICIO_PERIODO_VALUACION.fecha.0.0|MONTO_INICIAL.doble.19.2|FIN_PERIODO_VALUACION.fecha.0.0|ULTIMO_MONTO.doble.19.2|VALOR_SALVAMENTO.doble.19.2|VALOR_ACUMULADO.doble.19.2|VALOR_CONTABLE.doble.19.2|BLOCK_DATE.fecha.0.0.ULTIMA FECHA DE BLOQUEO|ID_EMP.cadena.2.0.CODIGO EMPRESA"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jscotcatcon|PRIMARY|`codcon`, `ejercicio`, `id_emp`",
                                    "jscotdaacon|PRIMARY|`codcon`, `ejercicio`, `id_emp`",
                                    "jscotencasi|PRIMARY|`asiento`,`actual`, `ejercicio`, `id_emp`",
                                    "jscotencasi|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jscotrenasi|PRIMARY|`asiento`,`renglon`,`actual`, `ejercicio`, `id_emp`",
                                    "jscotactfij|PRIMARY|`codigo`, `id_emp`",
                                    "jscottrafij|PRIMARY|`codigo`,`inicio_periodo_valuacion`,`id_emp`",
                                    "jscottrafij|SECONDARY|`BLOCK_DATE`,`ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)

    End Sub
    Private Sub ActualizarBancos()

        Dim Titulo As String = "Bancos y Cajas..."
        Dim aTablas() As String = {"jsbancatban|CODBAN.cadena.5.0|NOMBAN.cadena.50.0|CTABAN.cadena.50.0|CURRENCY.entero.4.0|AGENCIA.cadena.20.0|DIRECCION.cadena.100.0|TELEF1.cadena.20.0|TELEF2.cadena.20.0|FAX.cadena.20.0|titulo.cadena.5.0|CONTACTO.cadena.50.0|EMAIL.cadena.50.0|WEBSITE.cadena.50.0|COMISION.doble.6.2|FECHACREA.fecha.0.0|SALDOACT.doble.19.2|CODCON.cadena.30.0|FORMATO.cadena.2.0|ESTATUS.entero.2.0|MONEDA.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsbancatbantar|codban.cadena.5.0|codtar.cadena.5.0|com1.doble.6.2|com2.doble.6.2|id_emp.cadena.2.0",
                                   "jsbancatfor|formato.cadena.2.0|DESCRIP.cadena.100.0|MONTOTOP.entero.6.0|MONTOLEFT.entero.6.0|NOMBRETOP.entero.6.0|NOMBRELEFT.entero.6.0|MONTOLETRATOP.entero.6.0|MONTOLETRALEFT.entero.6.0|FECHATOP.entero.6.0|FECHALEFT.entero.6.0|NOENDOSABLETOP.entero.6.0|NOENDOSABLELEFT.entero.6.0|ID_EMP.cadena.2.0",
                                   "jsbanchedev|CODBAN.cadena.5.0|NUMCHEQUE.cadena.20.0|DEPOSITO.cadena.20.0|PROV_CLI.cadena.15.0|NUMCAN.cadena.20.0|CAUSA.entero.2.0|MONTO.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|FECHADEV.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsbandaaban|CODBAN.cadena.5.0|DEB00.doble.19.2|CRE00.doble.19.2|DEB01.doble.19.2|CRE01.doble.19.2|DEB02.doble.19.2|CRE02.doble.19.2|DEB03.doble.19.2|CRE03.doble.19.2|DEB04.doble.19.2|CRE04.doble.19.2|DEB05.doble.19.2|CRE05.doble.19.2|DEB06.doble.19.2|CRE06.doble.19.2|DEB07.doble.19.2|CRE07.doble.19.2|DEB08.doble.19.2|CRE08.doble.19.2|DEB09.doble.19.2|CRE09.doble.19.2|DEB10.doble.19.2|CRE10.doble.19.2|DEB11.doble.19.2|CRE11.doble.19.2|DEB12.doble.19.2|CRE12.doble.19.2|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsbanenccaj|CAJA.cadena.2.0|NOMCAJA.cadena.50.0|CODCON.cadena.50.0|SALDO.doble.19.2|CURRENCY.entero.4.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsbantraban|FECHAMOV.fecha.0.0|NUMDOC.cadena.30.0|TIPOMOV.cadena.2.0|concepto.memo.0.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ORIGEN.cadena.3.0|NUMORG.cadena.20.0|BENEFIC.cadena.150.0|comproba.cadena.20.0|CONCILIADO.cadena.1.0|MESCONCILIA.fecha.0.0|FECCONCILIA.fecha.0.0|TIPORG.cadena.2.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|MULTICAN.cadena.1.0|CODBAN.cadena.5.0|CAJA.cadena.2.0|PROV_CLI.cadena.15.0|CODVEN.cadena.5.0|BLOCK_DATE.fecha.0.0.FECHA ULTIMO BLOQUEO|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0|ID.bigintauto.20.0",
                                   "jsbantracaj|CAJA.cadena.5.0|RENGLON.cadena.15.0|FECHA.fecha.0.0|ORIGEN.cadena.3.0|TIPOMOV.cadena.2.0|NUMMOV.cadena.30.0|FORMPAG.cadena.2.0|NUMPAG.cadena.30.0|REFPAG.cadena.50.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|CODCON.cadena.30.0|concepto.memo.0.0|DEPOSITO.cadena.30.0|FECHA_DEP.fecha.0.0|CANTIDAD.entero.4.0|CODBAN.cadena.5.0|MULTICAN.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|PROV_CLI.cadena.15.0|CODVEN.cadena.5.0|ACEPTADO.cadena.1.0|BLOCK_DATE.fecha.0.0|ejercicio.cadena.5.0|ID_EMP.cadena.2.0|ID.bigintauto.20.0",
                                   "jsbantracon|FECHAMOV.fecha.0.0|NUMDOC.cadena.30.0|TIPBAN.cadena.2.0|concepto.memo.0.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|CONCILIADO.cadena.1.0|ORIGEN.cadena.3.0|CODBAN.cadena.5.0|MESCONCILIA.fecha.0.0|FECCONCILIA.fecha.0.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsbanordpag|COMPROBA.cadena.20.0|RENGLON.cadena.5.0|CODCON.cadena.30.0|REFER.cadena.30.0|CONCEPTO.cadena.250.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|DEBITO_CREDITO.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jsbancatban|PRIMARY|`codban`, `id_emp`",
                                    "jsbancatbantar|PRIMARY|`codban`, `codtar`,`id_emp`",
                                    "jsbanchedev|PRIMARY|`codban`,`numcheque`, `numcan`, `fechadev`, `ejercicio`, `id_emp`",
                                    "jsbandaaban|PRIMARY|`codban`, `ejercicio`, `id_emp`",
                                    "jsbantracon|PRIMARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsbantracon|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsbantraban|ID|`id`",
                                    "jsbantraban|PRIMARY|`codban`, `fechamov`,`numdoc`,`tipomov`, `origen`,`numorg`,`prov_cli`,`ejercicio`,`id_emp`, `ID` ",
                                    "jsbantraban|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsbantracaj|PRIMARY|`caja`,`renglon`,`fecha`, `origen`,`tipomov`,`nummov`, `aceptado`, `ejercicio`,`id_emp`, `id`",
                                    "jsbantracaj|SECOND|`numpag`, `id_emp`",
                                    "jsbantracaj|THIRD|`deposito`, `id_emp`",
                                    "jsbantracaj|NUMMOV|`nummov`,`tipomov`, `formpag`,`refpag`,`id_emp`",
                                    "jsbantracaj|ID|`id`",
                                    "jsbanordpag|PRIMARY|`comproba`,`renglon`, `codcon`,`ejercicio`,`id_emp`",
                                    "jsbanordpag|SECONDARY|`BLOCK_DATE`,`ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)

    End Sub
    Private Sub ActualizarNomina()

        Dim Titulo As String = "Recursos Humanos..."
        Dim aTablas() As String = {"jsnomcatcam|CODIGO.cadena.5.0|CAMPO.cadena.20.0|DESCRIPCION.cadena.50.0|TIPO.entero.2.0|LONGITUD.entero.4.0|DECIMALES.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsnomcatcon|CODNOM.cadena.5.0|CODCON.cadena.5.0|NOMCON.cadena.100.0|tipo.cadena.1.0|cuota.cadena.5.0|CONJUNTO.cadena.5.0|DESCRIPCION.memo.0.0|CONDICION.cadena.250.0|FORMULA.memo.0.0|AGRUPADOPOR.cadena.50.0|CODPRO.cadena.15.0|CONCEPTO_POR_ASIG.cadena.5.0|CODIGOCON.cadena.30.0|ESTATUS.cadena.1.0|ID_EMP.cadena.2.0",
                                   "jsnomcatcot|CONSTANTE.cadena.50.0|TIPO.entero.2.0|VALOR.cadena.50.0|ID_EMP.cadena.2.0",
                                   "jsnomcatdes|CODCON.cadena.5.0|NOMCON.cadena.50.0|CALCULO.cadena.250.0|MENSAJE.cadena.50.0|RESUMEN.cadena.50.0|ID_EMP.cadena.2.0",
                                   "jsnomcattra|CODTRA.cadena.10.0.CODIGO TRABAJADOR|CODHP.cadena.15.0.CODIGO BIOMETRICO|APELLIDOS.cadena.50.0.APELLIDOS TRABAJADOR|NOMBRES.cadena.50.0.NOMBRES TRABAJADOR|PROFESION.cadena.50.0.PROFESION TRABAJADOR|INGRESO.fecha.0.0.FECHA DE INGRESO A LA EMPRESA|FNACIMIENTO.fecha.0.0.FECHA NACIMIENTO TRABAJADOR|LUGARNAC.cadena.50.0.LUGAR NACIMIENTO TRABAJADOR|EDONAC.cadena.30.0.ESTADO O DEPARTAMENTO DE NACIMIENTO TRABAJADOR|PAIS.cadena.50.0.PAIS NACIMIENTO TRABAJADOR|NACIONALIZADO.cadena.1.0.NACIONALIZADO Ó NATURAL|NACIONAL.cadena.1.0.NACIONALIDAD V/E/P|NOGACETA.cadena.10.0.N° GACETA NACIONALIZACIÓN|CEDULA.cadena.10.0.NUMERO CEDULA DE IDENTIDAD|edocivil.cadena.1.0.ESTADO CIVIL|SEXO.cadena.1.0.SEXO TRABAJADOR|ASCENDENTES.entero.2.0.CANTIDAD PERSONAS ASCENDENTES A CARGO TRABAJADOR (ISLR)|DESCENDENTES.entero.2.0.CANTIDAD PERSONAS DESCENDENTES A CARGO TRABAJADOR (ISLR)|NOSSO.cadena.10.0.N° SEGURO SOCIAL OBLIGATORIO|STATUSSSO.cadena.1.0.ESTATUS SEGURO SOCIAL OBLIGATORIO|vivienda.cadena.1.0.TRABAJADOR POSEE VIVIENDA|VEHICULOS.entero.2.0.CANTIDAD VEHICULOS PROPIEDAD TRABAJADOR|DIRECCION.cadena.250.0.DIRECCION TRABAJADOR|TELEF1.cadena.30.0.TELEFONO TRABAJADOR|TELEF2.cadena.30.0.MOVIL TRABAJADOR|EMAIL.cadena.50.0.CORREO ELECTRONICO TRABAJADOR|condicion.cadena.1.0.CONDICION TRABAJADOR|TIPONOM.entero.2.0|FORMAPAGO.entero.2.0.FORMA DE PAGO A TRABAJADOR|BANCO.cadena.5.0.BANCO DONDE SE PAGA A TRABAJADOR|CTABAN.cadena.30.0.NUEMRO DE CUENTA TRABAJADOR|BANCO_1.cadena.5.0.BANCO ADICIONAL DE PAGO A TRABAJADOR|CTABAN_1.cadena.30.0.CUENTA BANCARIA ADICIONAL TRABAJADOR|BANCO_2.cadena.5.0.BANCO ADICIONAL TRABAJADOR|CTABAN_2.cadena.30.0.CUETNA ADICIONAL TRABAJADOR|CONYUGE.cadena.100.0.NOMBRE CONYUGUE TRABAJADOR|CO_NACION.cadena.1.0|CO_CEDULA.cadena.10.0.CEDULA IDENTIDAD CONYUGE|CO_PROFESION.cadena.50.0.PROFESION CONYUGE TRABAJADOR|CO_FECNAC.fecha.0.0.FECHA NACIMIENTO CONYUGE TRABAJADOR|CO_LUGNAC.cadena.30.0.LUGAR NACIMIENTO CONYUGE TRABAJADOR|CO_EDONAC.cadena.30.0.ESTADO NACIMIENTO CONYUGE TRABAJADOR|CO_PAIS.cadena.30.0.PAIS NACIMIENTO CONYUGE TRABAJADOR|GRUPO.cadena.5.0|SUBNIVEL1.cadena.30.0|SUBNIVEL2.cadena.30.0|SUBNIVEL3.cadena.30.0|SUBNIVEL4.cadena.30.0|SUBNIVEL5.cadena.30.0|SUBNIVEL6.cadena.30.0|ESTRUCTURA.cadena.150.0.ESTRUCTURA A LA CUAL PERTENECE TRABAJADOR|CARGO.cadena.50.0.CARGO ASIGNADO EN LA EMPRESA A TRASBAJADOR|SUELDO.doble.19.2.SUELDO MENSUAL TRABAJADOR|RETISLR.doble.6.2.PORCENTAJE RETENCION IMPUESTO SOBRE LA RENTA TRABAJADOR|FOTO.longblob.0.0.FOTO TRABAJADOR|TURNODESDE.fecha.0.0|FREEDAYS.entero.2.0|periodo.cadena.1.0|DATEFREEDAY.fecha.0.0|ROTATORIO.entero.2.0|FECHARET.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsnomcattur|CODTUR.cadena.5.0|NOMBRE.cadena.50.0|tipo.entero.2.0|HORADIURNA.tiempo.0.0|HORANOCTURNA.tiempo.0.0|MARCATURNO.entero.2.0|MARCADESCANSO.entero.2.0|TOL_ENT.entero.2.0|TOL_SAL.entero.2.0|TOL_INI_DES.entero.2.0|TOL_FIN_DES.entero.2.0|L.entero.2.0|L_E.tiempo.0.0|L_S.tiempo.0.0|L_DS.tiempo.0.0|L_DE.tiempo.0.0|M.entero.2.0|M_E.tiempo.0.0|M_S.tiempo.0.0|M_DS.tiempo.0.0|M_DE.tiempo.0.0|I.entero.2.0|I_E.tiempo.0.0|I_S.tiempo.0.0|I_DS.tiempo.0.0|I_DE.tiempo.0.0|J.entero.2.0|J_E.tiempo.0.0|J_S.tiempo.0.0|J_DS.tiempo.0.0|J_DE.tiempo.0.0|V.entero.2.0|V_E.tiempo.0.0|V_S.tiempo.0.0|V_DS.tiempo.0.0|V_DE.tiempo.0.0|S.entero.2.0|S_E.tiempo.0.0|S_S.tiempo.0.0|S_DS.tiempo.0.0|S_DE.tiempo.0.0|D.entero.2.0|D_E.tiempo.0.0|D_S.tiempo.0.0|D_DS.tiempo.0.0|D_DE.tiempo.0.0|ID_EMP.cadena.2.0",
                                   "jsnomencgru|GRUPO.cadena.5.0|DESCRIP.cadena.50.0|MASK1.cadena.15.0|MASK2.cadena.15.0|MASK3.cadena.15.0|MASK4.cadena.15.0|MASK5.cadena.15.0|MASK6.cadena.15.0|DESCRIP1.cadena.50.0|DESCRIP2.cadena.50.0|DESCRIP3.cadena.50.0|DESCRIP4.cadena.50.0|DESCRIP5.cadena.50.0|DESCRIP6.cadena.50.0|ID_EMP.cadena.2.0",
                                   "jsnomencpre|CODTRA.cadena.15.0|CODNOM.cadena.5.0|CODPRE.cadena.5.0|descrip.cadena.50.0|MONTOTAL.doble.19.2|FECHAPRESTAMO.fecha.0.0|FECHAINICIO.fecha.0.0|TIPOINTERES.entero.2.0|POR_INTERES.doble.6.2|NUMCUOTAS.entero.4.0|SALDO.doble.19.2|estatus.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsnomestcar|CODIGO.enterolargo10A.0.0|NOMBRE.cadena.50.0|CODIGOEMPRESA.cadena.50.0|SUELDOBASE.doble.19.2|ANTECESOR.entero.10.0|NIVEL.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsnomexptra|codtra.cadena.30.0|FECHA.fecha.0.0|FECHA_FIN.fecha.0.0|COMENTARIO.memo.0.0|CAUSA.cadena.5.0|ESTATUS.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsnomfecnom|CODNOM.cadena.5.0|DESDE.fecha.0.0|HASTA.fecha.0.0|TIPONOM.cadena.1.0|BLOCK_DATE.fecha.0.0|ID_EMP.cadena.2.0|",
                                   "jsnomhisdes|CODNOM.cadena.5.0|CODTRA.cadena.15.0|CODCON.cadena.5.0|HASTA.fecha.0.0|IMPORTE.doble.19.2|codpre.cadena.5.0|num_cuota.entero.2.0|ASIENTO.cadena.10.0|FECHASI.fecha.0.0|GRUPO1.cadena.5.0|GRUPO2.cadena.5.0|GRUPO3.cadena.5.0|PORCENTAJE_ASIG.doble.6.2|CODIGOCON.cadena.30.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsnomrengru|GRUPO.cadena.5.0|COD_NIVEL_ANT.cadena.15.0|COD_NIVEL.cadena.30.0|DES_NIVEL.cadena.50.0|NIVEL.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsnomrenpre|CODTRA.cadena.15.0|CODNOM.cadena.5.0|CODPRE.cadena.5.0|NUM_CUOTA.entero.4.0|MONTO.doble.19.2|CAPITAL.doble.19.2|INTERES.doble.19.2|PROCESADA.entero.2.0|FECHAINICIO.fecha.0.0|FECHAFIN.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsnomtrades|CODNOM.cadena.5.0|CODTRA.cadena.15.0|CODCON.cadena.5.0|IMPORTE.doble.19.2|PORCENTAJE_ASIG.doble.6.2|BLOCK_DATE.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsnomtrahor|CODIGOHP.cadena.15.0|FECHA.fecha.0.0|HORA.tiempo.0.0|TIPO.entero.2.0|PROCESADA.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsnomtratur|CODTRA.cadena.15.0|CODHP.cadena.15.0|DIA.fecha.0.0|ENTRADA.fechahora.0.0|SALIDA.fechahora.0.0|DESCANSO.fechahora.0.0|RETORNO.fechahora.0.0|HORAS.tiempo.0.0|tipo.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsnomturtra|CODTRA.cadena.15.0|CODTUR.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsnomformula|formula.cadena.50.0|parametros.cadena.150.0|descripcion.cadena.250.0",
                                   "jsnomencnom|CODNOM.cadena.5.0|DESCRIPCION.cadena.30.0|TIPONOM.entero.2.0|ULT_DESDE.fecha.0.0|ULT_HASTA.fecha.0.0|CODCON.cadena.30.0|ID_EMP.cadena.2.0",
                                   "jsnomrennom|CODNOM.cadena.5.0|CODTRA.cadena.10.0|CODCON.cadena.30.0|ID_EMP.cadena.2.0",
                                   "jsnomconpor|CODDES.cadena.10.0|CODASI.cadena.10.0|ID_EMP.cadena.2.0"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jsnomcatcam|PRIMARY|`codigo`, `campo`,  `id_emp`",
                                    "jsnomcatcon|PRIMARY|`codnom`,`codcon`, `tipo`,`id_emp`",
                                    "jsnomcatcot|PRIMARY|`CONSTANTE`,`ID_EMP`",
                                    "jsnomcattra|PRIMARY|`codtra`,`id_emp`",
                                    "jsnomcattur|PRIMARY|`codtur`,`id_emp`",
                                    "jsnomencgru|PRIMARY|`grupo`,`id_emp`",
                                    "jsnomencpre|PRIMARY|`codtra`,`codnom`,`codpre`,`id_emp`",
                                    "jsnomexptra|PRIMARY|`codtra`,`fecha`, `causa`, `id_emp`",
                                    "jsnomfecnom|PRIMARY|`codnom`,`desde`, `hasta`, `id_emp`",
                                    "jsnomfecnom|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsnomhisdes|PRIMARY|`codnom`,`codtra`, `codcon`, `desde`, `hasta`, `ejercicio`, `id_emp`",
                                    "jsnomhisdes|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsnomrengru|PRIMARY|`grupo`,`cod_nivel_ant`, `cod_nivel`, `nivel`, `id_emp`",
                                    "jsnomrenpre|PRIMARY|`codtra`, `codnom`,`codpre`, `num_cuota`, `id_emp`",
                                    "jsnomtrades|PRIMARY|`codnom`,`codtra`, `codcon`, `id_emp`",
                                    "jsnomtrades|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsnomtrahor|PRIMARY|`codigohp`,`fecha`,`hora`, `id_emp`",
                                    "jsnomtratur|PRIMARY|`codtra`,`codhp`,`dia`,`tipo`,`id_emp`",
                                    "jsnomturtra|PRIMARY|`codtra`,`codtur`,`id_emp`",
                                    "jsnomformula|PRIMARY|`formula`",
                                    "jsnomencnom|PRIMARY|`CODNOM`,`TIPONOM`,`ID_EMP`",
                                    "jsnomrennom|PRIMARY|`CODNOM`,`CODTRA`, `ID_EMP`",
                                    "jsnomconpor|PRIMARY|`CODDES`,`CODASI`, `ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)



    End Sub

    Private Sub ActualizarCompras()

        Dim Titulo As String = "Compras, Gastos y Cuentas por pagar..."
        Dim aTablas() As String = {"jsprotrapag|CODPRO.cadena.15.0|TIPOMOV.cadena.2.0|nummov.cadena.30.0|EMISION.fecha.0.0|HORA.cadena.10.0|VENCE.fecha.0.0|refer.cadena.30.0|concepto.memo.0.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|PORIVA.doble.19.2|FORMAPAG.cadena.2.0|NUMPAG.cadena.20.0|NOMPAG.cadena.20.0|benefic.cadena.250.0|ORIGEN.cadena.3.0|DEPOSITO.cadena.20.0|CTADEP.cadena.30.0|BANCODEP.cadena.30.0|CAJAPAG.cadena.2.0|numorg.cadena.30.0|MULTICAN.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|CODCON.cadena.30.0|MULTIDOC.cadena.2.0|TIPDOCCAN.cadena.2.0|INTERES.doble.19.2|CAPITAL.doble.19.2|COMPROBA.cadena.20.0|BANCO.cadena.5.0|CTABANCO.cadena.30.0|REMESA.cadena.20.0|CODVEN.cadena.5.0|CODCOB.cadena.5.0|FOTIPO.cadena.1.0|HISTORICO.cadena.1.0|TIPO.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprohispag|CODPRO.cadena.15.0|TIPOMOV.cadena.2.0|nummov.cadena.30.0|EMISION.fecha.0.0|HORA.cadena.10.0|VENCE.fecha.0.0|refer.cadena.30.0|concepto.memo.0.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|PORIVA.doble.19.2|FORMAPAG.cadena.2.0|NUMPAG.cadena.20.0|NOMPAG.cadena.20.0|benefic.cadena.250.0|ORIGEN.cadena.3.0|DEPOSITO.cadena.20.0|CTADEP.cadena.30.0|BANCODEP.cadena.30.0|CAJAPAG.cadena.2.0|numorg.cadena.30.0|MULTICAN.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|CODCON.cadena.30.0|MULTIDOC.cadena.2.0|TIPDOCCAN.cadena.2.0|INTERES.doble.19.2|CAPITAL.doble.19.2|COMPROBA.cadena.20.0|BANCO.cadena.5.0|CTABANCO.cadena.30.0|REMESA.cadena.20.0|CODVEN.cadena.5.0|CODCOB.cadena.5.0|FOTIPO.cadena.1.0|HISTORICO.cadena.1.0|TIPO.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprocatpro|CODPRO.cadena.15.0|NOMBRE.cadena.250.0|CATEGORIA.cadena.10.0|UNIDAD.cadena.5.0|RIF.cadena.20.0|NIT.cadena.20.0|ASIGNADO.cadena.15.0|DIRFISCAL.cadena.150.0|DIRPROVE.cadena.150.0|EMAIL1.cadena.50.0|EMAIL2.cadena.50.0|EMAIL3.cadena.50.0|EMAIL4.cadena.50.0|EMAIL5.cadena.50.0|SITIOWEB.cadena.50.0|TELEF1.cadena.50.0|TELEF2.cadena.50.0|TELEF3.cadena.50.0|FAX.cadena.50.0|GERENTE.cadena.50.0|TELGER.cadena.50.0|CONTACTO.cadena.50.0|TELCON.cadena.50.0|LIMCREDITO.doble.19.2|DISPONIBLE.doble.19.2|DESC1.doble.6.2|DESC2.doble.6.2|DESC3.doble.6.2|DESC4.doble.6.2|DIAS2.entero.2.0|DIAS3.entero.2.0|OBSERVACION.cadena.150.0|ZONA.cadena.5.0|COBRADOR.cadena.50.0|VENDEDOR.cadena.50.0|SALDO.doble.19.2|ULTPAGO.doble.19.2|fecultpago.fecha.0.0|FORULTPAGO.cadena.2.0|REGIMENIVA.cadena.1.0|FORMAPAGO.cadena.2.0|BANCO.cadena.5.0|CTABANCO.cadena.50.0|BANCODEPOSITO1.cadena.5.0|BANCODEPOSITO2.cadena.5.0|CTABANCODEPOSITO1.cadena.50.0|CTABANCODEPOSITO2.cadena.50.0|INGRESO.fecha.0.0|CODCON.cadena.30.0|ESTATUS.cadena.1.0|tipo.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsprodescom|NUMCOM.cadena.30.0|CODPRO.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprodesgas|NUMGAS.cadena.30.0|CODPRO.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproenccom|NUMCOM.cadena.30.0|SERIE_NUMCOM.cadena.10.0|EMISION.fecha.0.0|EMISIONIVA.fecha.0.0|CODPRO.cadena.15.0|COMEN.cadena.100.0|ALMACEN.cadena.5.0|REFER.cadena.15.0|CODCON.cadena.30.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|POR_DES.doble.6.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|TOT_COM.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|POR_DES1.doble.6.2|POR_DES2.doble.6.2|POR_DES3.doble.6.2|POR_DES4.doble.6.2|DESCUEN1.doble.19.2|DESCUEN2.doble.19.2|DESCUEN3.doble.19.2|DESCUEN4.doble.19.2|VENCE1.fecha.0.0|VENCE2.fecha.0.0.fecha DE CUOTA|CUOTA.entero.1.0.SI FUE PROCESADA PARA CUOTA|VENCE3.fecha.0.0.fecha DE RECEPCION|RECIBIDO.entero.1.0.SI MERCANCIA FUE RECIBIDA|VENCE4.fecha.0.0.fecha VENCIMIENTO|CONDPAG.entero.2.0|TIPOCREDITO.entero.2.0|FORMAPAG.cadena.2.0|NUMPAG.cadena.20.0|NOMPAG.cadena.20.0|BENEFIC.cadena.100.0|CAJA.cadena.2.0|ABONO.doble.19.2|SERIE.cadena.3.0|NUMGIRO.entero.4.0|PERGIRO.entero.4.0|INTERES.doble.19.2|PORINTERES.doble.6.2|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|IMP_IVA.doble.19.2|IMP_ICS.doble.19.2|RET_IVA.doble.19.2|NUM_RET_IVA.cadena.20.0|FECHA_RET_IVA.fecha.0.0|RET_ISLR.doble.19.2|NUM_RET_ISLR.cadena.20.0|FECHA_RET_ISLR.fecha.0.0|POR_RET_ISLR.doble.6.2|BASE_RET_ISLR.doble.19.2|NUMCXP.cadena.15.0|OTRA_CXP.cadena.15.0|OTRO_PRO.cadena.15.0|IMPRESA.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproencgas|NUMGAS.cadena.30.0|SERIE_NUMGAS.cadena.10.0|EMISION.fecha.0.0|EMISIONIVA.fecha.0.0|CODPRO.cadena.15.0|NOMPRO.cadena.100.0|RIF.cadena.20.0|NIT.cadena.20.0|COMEN.cadena.100.0|REFER.cadena.15.0|ALMACEN.cadena.5.0|CODCON.cadena.30.0|GRUPO.entero.10.0|SUBGRUPO.entero.10.0|TOT_NET.doble.19.2|POR_DES.doble.6.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|TIPOIVA.cadena.1.0|POR_IVA.doble.6.2|BASEIVA.doble.19.2|IMP_IVA.doble.19.2|imp_ics.doble.19.2|RET_IVA.doble.19.2|NUM_RET_IVA.cadena.20.0|FECHA_RET_IVA.fecha.0.0|TOT_GAS.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|CONDPAG.entero.2.0|TIPOCREDITO.entero.2.0|FORMAPAG.cadena.2.0|NUMPAG.cadena.20.0|NOMPAG.cadena.20.0|BENEFIC.cadena.100.0|CAJA.cadena.2.0|ABONO.doble.19.2|SERIE.cadena.3.0|NUMGIRO.entero.4.0|PERGIRO.entero.4.0|INTERES.doble.19.2|PORINTERES.doble.6.2|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|RET_ISLR.doble.19.2|NUM_RET_ISLR.cadena.15.0|FECHA_RET_ISLR.fecha.0.0|POR_RET_ISLR.doble.19.2|BASE_RET_ISLR.doble.19.2|NUMCXP.cadena.15.0|OTRA_CXP.cadena.15.0|OTRO_PRO.cadena.15.0|ZONA.cadena.5.0|TIPOGASTO.entero.1.0|IMPRESA.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproencncr|NUMNCR.cadena.30.0|SERIE_NUMNCR.cadena.10.0|NUMCOM.cadena.30.0|EMISION.fecha.0.0|EMISIONIVA.fecha.0.0|CODPRO.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|ALMACEN.cadena.5.0|TRANSPORTE.cadena.5.0|REFER.cadena.15.0|CODCON.cadena.30.0|TARIFA.cadena.1.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|TOT_NCR.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|ESTATUS.entero.2.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproencndb|NUMNDB.cadena.30.0|SERIE_NUMNDB.cadena.10.0|NUMCOM.cadena.30.0|EMISION.fecha.0.0|emisioniva.fecha.0.0|CODPRO.cadena.15.0|COMEN.cadena.100.0|ALMACEN.cadena.5.0|REFER.cadena.15.0|CODCON.cadena.30.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|TOT_NDB.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|.ASIENTO.cadena.15.0|FECHASI.fecha.0.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproencord|NUMORD.cadena.30.0|SERIE_NUMORD.cadena.10.0|EMISION.fecha.0.0|ENTREGA.fecha.0.0|CODPRO.cadena.15.0|COMEN.cadena.100.0|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|TOT_ORD.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ESTATUS.cadena.1.0|ITEMS.entero.4.0|IMPRESA.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproencprg|NUMPRG.cadena.15.0|FECHAPRG.fecha.0.0|FECHAPRGDESDE.fecha.0.0|FECHAPRGHASTA.fecha.0.0|ESTATUSPRG.entero.2.0|FECHAPROCESO.fecha.0.0|BANCOPRG.cadena.5.0|COMEN.cadena.250.0|TOTALPRG.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|BLOCK_DATE.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsproencrep|NUMREC.cadena.30.0|SERIE_NUMREC.cadena.5.0|EMISION.fecha.0.0|CODPRO.cadena.15.0|COMEN.cadena.100.0|RESPONSABLE.cadena.50.0|ALMACEN.cadena.5.0|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|TOT_REC.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ESTATUS.cadena.1.0|NUMCOM.cadena.30.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|IMPRESA.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprogrugas|CODIGO.enterolargo10A.0.0|NOMBRE.cadena.50.0|ANTECESOR.entero.10.0|ID_EMP.cadena.2.0",
                                   "jsproicscom|NUMCOM.cadena.30.0|codpro.cadena.15.0|tipoics.cadena.1.0|porics.doble.6.2|baseics.doble.19.2|impics.doble.19.2|retencion.doble.19.2|numretencion.cadena.20.0|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsproivacom|NUMCOM.cadena.30.0|CODPRO.cadena.15.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|retencion.doble.19.2|numretencion.cadena.20.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproivagas|NUMGAS.cadena.30.0|CODPRO.cadena.15.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|retencion.doble.19.2|numretencion.cadena.20.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproivancr|NUMNCR.cadena.30.0|CODPRO.cadena.15.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|retencion.doble.19.2|numretencion.cadena.20.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproivandb|NUMNDB.cadena.30.0|CODPRO.cadena.15.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|retencion.doble.19.2|numretencion.cadena.20.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproivaord|NUMORD.cadena.30.0|CODPRO.cadena.15.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|retencion.doble.19.2|numretencion.cadena.20.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproivarec|NUMREC.cadena.30.0|CODPRO.cadena.15.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|retencion.doble.19.2|numretencion.cadena.20.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsproliscat|CODIGO.cadena.10.0|DESCRIP.cadena.50.0|ANTEC.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprolisuni|CODIGO.cadena.5.0|DESCRIP.cadena.50.0|ID_EMP.cadena.50.0",
                                   "jsprorencom|NUMCOM.cadena.30.0|RENGLON.cadena.5.0|CODPRO.cadena.15.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.10.0|ESTATUS.cadena.1.0|COSTOU.doble.19.2|DES_PRO.doble.6.2|DES_ART.doble.6.2|COSTOTOT.doble.19.2|COSTOTOTDES.doble.19.2|NUMORD.cadena.30.0|RENORD.cadena.5.0|NUMREC.cadena.30.0|RENREC.cadena.5.0|CODCON.cadena.30.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprorendes|NUMDOC.cadena.30.0|CODPRO.cadena.15.0|ORIGEN.cadena.3.0|ITEM.cadena.15.0|RENGLON.cadena.5.0|NUM_DESC.cadena.5.0|PORDES.doble.10.3|DESCUENTO.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsprorengas|NUMGAS.cadena.30.0|RENGLON.cadena.5.0|CODPRO.cadena.15.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.10.0|ESTATUS.cadena.1.0|COSTOU.doble.19.2|DES_PRO.doble.6.2|DES_ART.doble.6.2|COSTOTOT.doble.19.2|COSTOTOTDES.doble.19.2|CAUSA.cadena.5.0|CODCON.cadena.30.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprorenncr|NUMNCR.cadena.30.0|RENGLON.cadena.5.0|CODPRO.cadena.15.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.10.0|ESTATUS.cadena.1.0|PRECIO.doble.19.2|DES_ART.doble.10.3|DES_PRO.doble.10.3|POR_ACEPTA_DEV.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMCOM.cadena.30.0|CODCON.cadena.30.0|EDITABLE.entero.2.0|CAUSA.cadena.5.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprorenndb|NUMNDB.cadena.30.0|RENGLON.cadena.5.0|CODPRO.cadena.15.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.10.0|ESTATUS.cadena.1.0|COSTO.doble.19.2|DES_PRO.doble.10.3|DES_ART.doble.10.3|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMCOM.cadena.30.0|CODCON.cadena.30.0|EDITABLE.entero.2.0|CAUSA.cadena.5.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprorenord|NUMORD.cadena.30.0|RENGLON.cadena.5.0|CODPRO.cadena.15.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PESO.doble.10.3|CANTRAN.doble.10.3|ESTATUS.cadena.1.0|COSTOU.doble.19.2|DES_PRO.doble.10.3|DES_ART.doble.10.3|COSTOTOT.doble.19.2|COSTOTOTDES.doble.19.2|LOTE.cadena.10.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprorenprg|NUMPRG.cadena.15.0|CODPRO.cadena.15.0|NUMMOV.cadena.30.0|TIPOMOV.cadena.2.0|EMISION.fecha.0.0|VENCE.fecha.0.0|IMPORTE.doble.19.2|SALDO.doble.19.2|EMISIONCH.fecha.0.0|BANCO.cadena.5.0|A_CANCELAR.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsprorenrep|NUMREC.cadena.30.0|RENGLON.cadena.5.0|CODPRO.cadena.15.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|CANTRAN.doble.10.3|PESO.doble.10.3|LOTE.cadena.10.0|ESTATUS.cadena.1.0|COSTOU.doble.19.2|DES_PRO.doble.10.3|DES_ART.doble.10.3|COSTOTOT.doble.19.2|COSTOTOTDES.doble.19.2|CODCON.cadena.30.0|NUMORD.cadena.30.0|RENORD.cadena.15.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsprotrapagcan|codpro.cadena.15.0|tipomov.cadena.2.0|nummov.cadena.30.0|emision.fecha.0.0|refer.cadena.30.0|CONCEPTO.memo.0.0|importe.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|comproba.cadena.15.0|id_emp.cadena.2.0",
                                   "jsproexppro|CODPRO.cadena.15.0|FECHA.fechahora.0.0|COMENTARIO.memo.0.0|CONDICION.cadena.1.0|CAUSA.cadena.5.0|TIPOCONDICION.cadena.1.0|BLOCK_DATE.fecha.0.0|ID_EMP.cadena.2.0"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jsprocatpro|PRIMARY|`CODPRO`, `TIPO`,  `ID_EMP`",
                                    "jsprodescom|PRIMARY|`NUMCOM`,`CODPRO`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprodesgas|PRIMARY|`NUMGAS`,`CODPRO`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsproenccom|PRIMARY|`NUMCOM`,`CODPRO`,`EJERCICIO`,`ID_EMP`",
                                    "jsproenccom|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsproencgas|PRIMARY|`NUMGAS`,`CODPRO`,`EJERCICIO`,`ID_EMP`",
                                    "jsproencgas|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsproencncr|PRIMARY|`NUMNCR`,`CODPRO`,`EJERCICIO`,`ID_EMP`",
                                    "jsproencncr|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsproencndb|PRIMARY|`NUMNDB`,`CODPRO`,`EJERCICIO`,`ID_EMP`",
                                    "jsproencndb|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsproencord|PRIMARY|`NUMORD`,`CODPRO`,`EJERCICIO`,`ID_EMP`",
                                    "jsproencord|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsproencprg|PRIMARY|`NUMPRG`,`ID_EMP`",
                                    "jsproencprg|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsproencrep|PRIMARY|`NUMREC`,`CODPRO`,`EJERCICIO`,`ID_EMP`",
                                    "jsproencrep|SECONDARY|`BLOCK_DATE`,`ID_EMP`",
                                    "jsproivacom|PRIMARY|`NUMCOM`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsproivagas|PRIMARY|`NUMGAS`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsproivancr|PRIMARY|`NUMNCR`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsproivandb|PRIMARY|`NUMNDB`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsproivaord|PRIMARY|`NUMORD`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsproivarec|PRIMARY|`NUMREC`,`CODPRO`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsprorencom|PRIMARY|`NUMCOM`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprorendes|PRIMARY|`NUMDOC`,`CODPRO`,`ORIGEN`,`ITEM`,`RENGLON`,`NUM_DESC`,`ID_EMP`",
                                    "jsprorengas|PRIMARY|`NUMGAS`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprorenncr|PRIMARY|`NUMNCR`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprorenndb|PRIMARY|`NUMNDB`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprorenord|PRIMARY|`NUMORD`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprorenprg|PRIMARY|`NUMPRG`,`CODPRO`,`NUMMOV`,`ID_EMP`",
                                    "jsprorenrep|PRIMARY|`NUMREC`,`CODPRO`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprotrapag|PRIMARY|`CODPRO`,`TIPOMOV`,`nummov`,`EMISION`,`HORA`,`TIPO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprotrapag|SECOND|`CODPRO`,`nummov`,`ID_EMP`",
                                    "jsprotrapag|THIRD|`CODPRO`,`EMISION`,`HISTORICO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprotrapag|FOURTH|`BLOCK_DATE`,`ID_EMP`",
                                    "jsprohispag|PRIMARY|`CODPRO`,`TIPOMOV`,`nummov`,`EMISION`,`HORA`,`TIPO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprohispag|SECOND|`CODPRO`,`nummov`,`ID_EMP`",
                                    "jsprohispag|THIRD|`CODPRO`,`EMISION`,`HISTORICO`,`EJERCICIO`,`ID_EMP`",
                                    "jsprohispag|FOURTH|`BLOCK_DATE`,`ID_EMP`",
                                    "jsprotrapagcan|PRIMARY|`CODPRO`,`TIPOMOV`,`nummov`,`EMISION`,`COMPROBA`,`ID_EMP`",
                                    "jsproexppro|SECONDARY|`BLOCK_DATE`,`ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)

    End Sub
    Private Sub ActualizarVentas()

        Dim Titulo As String = "Ventas y cuentas por cobrar..."

        Dim aTablas() As String = {"jsvencatcli|CODCLI.cadena.15.0|nombre.cadena.250.0|CATEGORIA.cadena.5.0|UNIDAD.cadena.10.0|RIF.cadena.15.0|NIT.cadena.15.0|CI.cadena.15.0|ALTERNO.cadena.15.0|dirfiscal.cadena.250.0|FPAIS.entero.10.0|FESTADO.entero.10.0|FMUNICIPIO.entero.10.0|FPARROQUIA.entero.10.0|FCIUDAD.entero.10.0|FBARRIO.entero.10.0|FZIP.cadena.10.0|DPAIS.entero.10.0|DESTADO.entero.10.0|DMUNICIPIO.entero.10.0|DPARROQUIA.entero.10.0|DCIUDAD.entero.10.0|DBARRIO.entero.10.0|DZIP.cadena.10.0|CODGEO.entero.2.0|DIRDESPA.cadena.250.0|EMAIL1.cadena.50.0|EMAIL2.cadena.50.0|EMAIL3.cadena.50.0|EMAIL4.cadena.50.0|TELEF1.cadena.15.0|TELEF2.cadena.15.0|TELEF3.cadena.15.0|FAX.cadena.15.0|GERENTE.cadena.100.0|TELGER.cadena.15.0|CONTACTO.cadena.100.0|TELCON.cadena.15.0|LIMITECREDITO.doble.19.2|DISPONIBLE.doble.19.2|CHEQDEV.entero.4.0|CHEQMES.entero.4.0|CHEQACU.entero.4.0|DES_CLI.doble.6.2|DESC_CLI_1.doble.6.2|DESC_CLI_2.doble.6.2|DESC_CLI_3.doble.6.2|DESC_CLI_4.doble.6.2|DESDE_1.entero.2.0|HASTA_1.entero.2.0|DESDE_2.entero.2.0|HASTA_2.entero.2.0|DESDE_3.entero.2.0|HASTA_3.entero.2.0|DESDE_4.entero.2.0|HASTA_4.entero.2.0|POR_CAR_DEV.doble.6.2|ZONA.cadena.5.0|RUTA_VISITA.cadena.5.0|RUTA_DESPACHO.cadena.5.0|NUM_DESPACHO.entero.4.0|NUM_VISITA.entero.4.0|COBRADOR.cadena.5.0|VENDEDOR.cadena.5.0|TRANSPORTE.cadena.5.0|SALDO.doble.19.2|ULTCOBRO.doble.19.2|FECULTCOBRO.fecha.0.0|FORULTCOBRO.cadena.2.0|REGIMENIVA.cadena.1.0|TARIFA.cadena.1.0|LISPRE.cadena.5.0|FORMAPAGO.cadena.2.0|BANCO.cadena.5.0|CTABANCO.cadena.50.0|INGRESO.fecha.0.0|CODCON.cadena.30.0|CODCRE.entero.2.0|ESTATUS.cadena.1.0|REQ_RIF.entero.2.0|REQ_NIT.entero.2.0|REQ_REC.entero.2.0|REQ_CIS.entero.2.0|REQ_REG.entero.2.0|REQ_REA.entero.2.0|REQ_BAN.entero.2.0|REQ_COM.entero.2.0|FECVISITA.entero.2.0|INIVISITA.fecha.0.0|FECULTVISITA.fecha.0.0|MERCHANDISING.entero.2.0|BACKORDER.entero.2.0|DIAPAGO.entero.2.0|DEPAGO.cadena.5.0|APAGO.cadena.5.0|RANKING.entero.2.0|FECHARANK.fecha.0.0|COMENTARIO.cadena.250.0|ESPECIAL.entero.2.0|SHARE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvencatclidiv|CODCLI.cadena.15.0|DIVISION.cadena.5.0|LIMITECREDITO.doble.19.2|DISPONIBLE.doble.19.2|SALDO.doble.19.2|ASESOR.cadena.5.0|RUTA_VISITA.cadena.5.0|NUM_VISITA.entero.4.0|FRECUENCIA_VISITA.entero.2.0|FECHA_INICIO.fecha.0.0|FECHA_ULT_VISITA.fecha.0.0|RUTA_DESPACHO.cadena.5.0|NUM_DESPACHO.entero.4.0|TRANSPORTE.cadena.5.0|ZONA.cadena.5.0|DES_CLI.doble.10.3|DESC_CLI_1.doble.10.3|DESDE_1.entero.4.0|HASTA_1.entero.4.0|DESC_CLI_2.doble.10.3|DESDE_2.entero.4.0|HASTA_2.entero.4.0|DESC_CLI_3.doble.10.3|DESDE_3.entero.4.0|HASTA_3.entero.4.0|DESC_CLI_4.doble.10.3|DESDE_4.entero.4.0|HASTA_4.entero.4.0|FORMAPAGO.cadena.2.0|TARIFA.cadena.2.0|LISTAPRECIOS.cadena.5.0|ESTATUS.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsvencatclisada|CODCLI.cadena.15.0|CODIGOSADA.cadena.15.0|RAZON.cadena.250.0|TIPO.cadena.50.0|ESTADO.cadena.50.0|MUNICIPIO.cadena.50.0|CIUDAD.cadena.50.0|ESTATUS.cadena.30.0|ID_EMP.cadena.2.0",
                                   "jsvencatven|CODVEN.cadena.5.0|DESCAR.cadena.50.0|APELLIDOS.cadena.50.0|NOMBRES.cadena.50.0|DIRECCION.cadena.100.0|TELEFONO.cadena.15.0|CELULAR.cadena.15.0|EMAIL.cadena.50.0|FIANZA.doble.19.2|TIPO.cadena.1.0|CLASE.entero.2.0.VENDEDOR Ó SUPERVISOR Ó GERENTE|ZONA.cadena.5.0|ESTRUCTURA.cadena.50.0|CLAVE.cadena.15.0|INGRESO.fecha.0.0|CARTERACLI.entero.4.0|CARTERAART.entero.4.0|CARTERAMAR.entero.4.0|LISTA_A.entero.2.0|LISTA_B.entero.2.0|LISTA_C.entero.2.0|LISTA_D.entero.2.0|LISTA_E.entero.2.0|LISTA_F.entero.2.0|FACTORCUOTA.doble.10.3|ESTATUS.entero.2.0|DIVISION.cadena.5.0|SUPERVISOR.cadena.5.0|COM_VEN.doble.10.3|COM_COB.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsvencatvendiv|codven.cadena.5.0|division.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvencatvenjer|codven.cadena.5.0|tipjer.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvencatvis|CODCLI.cadena.15.0|DIA.entero.2.0|DESDE.cadena.5.0|HASTA.cadena.5.0|DESDEPM.cadena.5.0|HASTAPM.cadena.5.0|TIPO.entero.2.0|DIVISION.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvencaudcr|CODIGO.cadena.5.0|DESCRIP.cadena.100.0|INVENTARIO.entero.2.0|VALIDAUNIDAD.entero.2.0|AJUSTAPRECIO.entero.2.0|ESTADO.entero.2.0|CREDITO_DEBITO.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsvencedsoc|CODCLI.cadena.15.0|NACIONAL.cadena.1.0|CI.cadena.10.0|NOMBRE.cadena.100.0|EXPEDIENTE.cadena.1.0|ID_EMP.cadena.2.0",
                                   "jsvencescom|CORREDOR.cadena.5.0|TIPO.cadena.2.0|DESDE.entero.4.0|HASTA.entero.4.0|COMISION.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsvencestic|CODIGO.cadena.5.0|DESCRIP.cadena.50.0|PORCOM.doble.6.2|PORCOMCLI.doble.6.2|LENCODBAR.entero.4.0|INICIOPRECIO.entero.4.0|LENPRECIO.entero.4.0|INICIOTIPO.entero.4.0|LENTIPO.entero.4.0|CARGOS.doble.19.2|TIPOIVA.cadena.1.0|CODCON.cadena.30.0|CODPRO.cadena.15.0|GRUPO.entero.10.0|SUBGRUPO.entero.10.0|ID_EMP.cadena.2.0",
                                   "jsvencestip|CORREDOR.cadena.5.0|TIPO.cadena.2.0|DESCRIP.cadena.50.0|COM_CORREDOR.doble.10.3|COM_CLIENTE.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsvenclirgv|CODVEN.cadena.5.0|RUTA.cadena.5.0|CODCLI.cadena.15.0|NUMVISITA.entero.2.0|VISITA.entero.2.0|RAZON_NV.entero.2.0|FECHA_PV.fecha.0.0|HORAENTRADA.cadena.10.0|HORASALIDA.cadena.10.0|FECHA.fecha.0.0|NUMPED.cadena.15.0|NUMDEV.cadena.15.0|DIA.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvencobrgv|CODCLI.cadena.15.0|TIPOMOV.cadena.2.0|NUMMOV.cadena.30.0|EMISION.fecha.0.0|HORA.cadena.10.0|VENCE.fecha.0.0|REFER.cadena.50.0|CONCEPTO.memo.0.0|IMPORTE.doble.19.2|IMPIVA.doble.19.2|FORMAPAG.cadena.2.0|CAJAPAG.cadena.5.0|NUMPAG.cadena.30.0|NOMPAG.cadena.30.0|BENEFIC.cadena.250.0|ORIGEN.cadena.3.0|NUMORG.cadena.30.0|MULTICAN.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|CODCON.cadena.30.0|MULTIDOC.cadena.15.0|TIPDOCCAN.cadena.2.0|INTERES.doble.19.2|CAPITAL.doble.19.2|COMPROBA.cadena.15.0|BANCO.cadena.5.0|CTABANCO.cadena.50.0|REMESA.cadena.15.0|CODVEN.cadena.5.0|CODCOB.cadena.5.0|HISTORICO.cadena.1.0|FOTIPO.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|DIVISION.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvencomven|CODVEN.cadena.5.0|TIPJER.cadena.5.0|POR_VENTAS.doble.10.3|TIPO.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsvencomvencob|CODVEN.cadena.5.0|tipjer.cadena.5.0|DE.entero.2.0|A.entero.2.0|POR_COBRANZA.doble.6.2|ID_EMP.cadena.2.0",
                                   "jsvencuoart|CODVEN.cadena.5.0|CODART.cadena.15.0|ESMES01.doble.10.3|ESMES02.doble.10.3|ESMES03.doble.10.3|ESMES04.doble.10.3|ESMES05.doble.10.3|ESMES06.doble.10.3|ESMES07.doble.10.3|ESMES08.doble.10.3|ESMES09.doble.10.3|ESMES10.doble.10.3|ESMES11.doble.10.3|ESMES12.doble.10.3|REMES01.doble.10.3|REMES02.doble.10.3|REMES03.doble.10.3|REMES04.doble.10.3|REMES05.doble.10.3|REMES06.doble.10.3|REMES07.doble.10.3|REMES08.doble.10.3|REMES09.doble.10.3|REMES10.doble.10.3|REMES11.doble.10.3|REMES12.doble.10.3|ACT01.entero.4.0|ACT02.entero.4.0|ACT03.entero.4.0|ACT04.entero.4.0|ACT05.entero.4.0|ACT06.entero.4.0|ACT07.entero.4.0|ACT08.entero.4.0|ACT09.entero.4.0|ACT10.entero.4.0|ACT11.entero.4.0|ACT12.entero.4.0|TIPO.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendesapt|NUMAPT.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendescot|NUMCOT.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendesfac|NUMFAC.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendesnot|NUMFAC.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendesped|NUMPED.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendespedrgv|NUMPED.cadena.15.0|RENGLON.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendivzon|CODCLI.cadena.15.0|DIVISION.cadena.5.0|NOMDIVISION.cadena.50.0|ZONA.cadena.5.0|RUTA.cadena.5.0|ASESOR.cadena.5.0|LIMITECREDITO.doble.19.2|DISPONIBLE.doble.19.2|SALDO.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsvenenccie|CODVEN.cadena.5.0|FECHA.fecha.0.0|AVISITAR.entero.2.0|VISITADOS.entero.2.0|EFECTIVIDAD.doble.6.2|ITEMS_CAJ.doble.10.3|ITEMS_KGS.doble.10.3|MONTO_CD.doble.19.2|MONTO_COM_CD.doble.19.2|CANTIDAD_CD.entero.4.0|MARCAS_KGS.doble.10.3|JERAR_CAJ.doble.10.3|JERAR_KGS.doble.10.3|PEDIDOS_CAJ.doble.10.3|PEDIDOS_KGS.doble.10.3|COSTOSVENTAS.doble.19.2|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenenccot|NUMCOT.cadena.15.0|EMISION.fecha.0.0|VENCE.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|TARIFA.cadena.1.0|TOT_NET.doble.19.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|IMP_IVA.doble.19.2|TOT_COT.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ESTATUS.cadena.1.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencnot|NUMFAC.cadena.15.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|ALMACEN.cadena.5.0|TRANSPORTE.cadena.5.0|VENCE.fecha.0.0|REFER.cadena.15.0|CODCON.cadena.30.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|CONDPAG.entero.2.0|TIPOCRE.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|TIPOFAC.entero.2.0|TIPO.entero.2.0|TOT_FAC.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ESTATUS.entero.2.0|TARIFA.cadena.1.0|NUMCXC.cadena.15.0|OTRA_CXC.cadena.15.0|OTRO_CLI.cadena.15.0|RELGUIA.cadena.15.0|RELFACTURAS.cadena.15.0|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencfac|NUMFAC.cadena.15.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|ALMACEN.cadena.5.0|TRANSPORTE.cadena.5.0|VENCE.fecha.0.0|REFER.cadena.15.0|CODCON.cadena.30.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|PORDES.doble.6.2|PORDES1.doble.6.2|PORDES2.doble.6.2|PORDES3.doble.6.2|PORDES4.doble.6.2|DESCUEN.doble.19.2|DESCUEN1.doble.19.2|DESCUEN2.doble.19.2|DESCUEN3.doble.19.2|DESCUEN4.doble.19.2|CARGOS.doble.19.2|CARGOS1.doble.19.2|CARGOS2.doble.19.2|CARGOS3.doble.19.2|CARGOS4.doble.19.2|TOT_FAC1.doble.19.2|TOT_FAC2.doble.19.2|TOT_FAC3.doble.19.2|TOT_FAC4.doble.19.2|VENCE1.fecha.0.0|VENCE2.fecha.0.0|VENCE3.fecha.0.0|VENCE4.fecha.0.0|CONDPAG.entero.2.0|TIPOCRE.cadena.1.0|FORMAPAG.cadena.2.0|NUMPAG.cadena.50.0|NOMPAG.cadena.50.0|CAJA.cadena.5.0|IMPORTEEFECTIVO.doble.19.2|NUMEROCHEQUE.cadena.15.0|BANCOCHEQUE.cadena.5.0|IMPORTECHEQUE.doble.19.2|NUMEROTARJETA.cadena.15.0|CODIGOTARJETA.cadena.5.0|IMPORTETARJETA.doble.19.2|NUMEROCESTATICKET.cadena.15.0|NOMBRECESTATICKET.cadena.15.0|IMPORTECESTATICKET.doble.19.2|NUMERODEPOSITO.cadena.15.0|BANCODEPOSITO.cadena.5.0|IMPORTEDEPOSITO.doble.19.2|ABONO.doble.19.2|SERIE.cadena.3.0|NUMGIRO.entero.4.0|PERGIRO.entero.4.0|INTERES.doble.19.2|PORINT.doble.6.2|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|BASEIVA.doble.19.2|PORIVA.doble.6.2|IMP_IVA.doble.19.2|IMP_ICS.doble.19.2|TIPOFAC.entero.2.0|TIPO.entero.2.0|TOT_FAC.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ESTATUS.entero.2.0|TARIFA.cadena.1.0|NUMCXC.cadena.15.0|OTRA_CXC.cadena.25.0|OTRO_CLI.cadena.15.0|RELGUIA.cadena.15.0|RELFACTURAS.cadena.15.0|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencgui|CODIGOGUIA.cadena.15.0|DESCRIPCION.cadena.100.0|ELABORADOR.cadena.5.0|TRANSPORTE.cadena.5.0|FECHAGUIA.fecha.0.0|EMISIONFAC.fecha.0.0|HASTAFAC.fecha.0.0|ITEMS.entero.4.0|TOTALGUIA.doble.19.2|TOTALKILOS.doble.10.3|ESTATUS.entero.2.0|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencguipedidos|CODIGOGUIA.cadena.15.0|DESCRIPCION.cadena.100.0|ELABORADOR.cadena.5.0|TRANSPORTE.cadena.5.0|FECHAGUIA.fecha.0.0|EMISIONFAC.fecha.0.0|HASTAFAC.fecha.0.0|ITEMS.entero.4.0|TOTALGUIA.doble.19.2|TOTALKILOS.doble.10.3|ESTATUS.entero.2.0|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencncr|NUMNCR.cadena.30.0|NUMFAC.cadena.15.0|EMISION.fecha.0.0|fechaiva.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|ALMACEN.cadena.5.0|TRANSPORTE.cadena.5.0|REFER.cadena.15.0|CODCON.cadena.30.0|TARIFA.cadena.1.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|imp_ics.doble.19.2|TOT_NCR.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|CONDPAG.entero.2.0|TIPOCREDITO.entero.2.0|FORMAPAG.cadena.2.0|NUMPAG.cadena.30.0|NOMPAG.cadena.30.0|BENEFIC.cadena.150.0|CAJA.cadena.2.0|ORIGEN.cadena.5.0|ESTATUS.entero.2.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|IMPRESA.entero.2.0|RELFACTURAS.cadena.15.0|RELNCR.cadena.15.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencncrrgv|NUMNCR.cadena.30.0|NUMFAC.cadena.15.0|EMISION.fecha.0.0|fechaiva.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|ALMACEN.cadena.5.0|TRANSPORTE.cadena.5.0|REFER.cadena.15.0|CODCON.cadena.30.0|TARIFA.cadena.1.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|imp_ics.doble.19.2|TOT_NCR.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|CONDPAG.entero.2.0|TIPOCREDITO.entero.2.0|FORMAPAG.cadena.2.0|NUMPAG.cadena.30.0|NOMPAG.cadena.30.0|BENEFIC.cadena.150.0|CAJA.cadena.2.0|ORIGEN.cadena.5.0|ESTATUS.entero.2.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|IMPRESA.entero.2.0|RELFACTURAS.cadena.15.0|RELNCR.cadena.15.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencndb|NUMNDB.cadena.30.0|NUMFAC.cadena.15.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|ALMACEN.cadena.5.0|TRANSPORTE.cadena.5.0|REFER.cadena.15.0|CODCON.cadena.30.0|TARIFA.cadena.1.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|TOT_NDB.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|ESTATUS.entero.2.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|IMPRESA.entero.2.0|RELFACTURAS.cadena.15.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencped|NUMPED.cadena.15.0|EMISION.fecha.0.0|ENTREGA.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|TARIFA.cadena.1.0|TOT_NET.doble.19.2|PORDES.doble.6.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|IMP_IVA.doble.19.2|TOT_PED.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|PORDES1.doble.6.2|PORDES2.doble.6.2|PORDES3.doble.6.2|PORDES4.doble.6.2|DESCUEN1.doble.19.2|DESCUEN2.doble.19.2|DESCUEN3.doble.19.2|DESCUEN4.doble.19.2|VENCE1.fecha.0.0|VENCE2.fecha.0.0|VENCE3.fecha.0.0|VENCE4.fecha.0.0|ESTATUS.entero.2.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|CONDPAG.entero.2.0|TIPOCREDITO.entero.2.0|FORMAPAG.cadena.2.0|NOMPAG.cadena.20.0|NUMPAG.cadena.30.0|ABONO.doble.19.2|SERIE.cadena.3.0|NUMGIRO.entero.4.0|PERGIRO.entero.4.0|INTERES.doble.19.2|PORINT.doble.6.2|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencpedrgv|NUMPED.cadena.15.0|EMISION.fecha.0.0|ENTREGA.fecha.0.0|CODCLI.cadena.15.0|COMEN.cadena.100.0|CODVEN.cadena.5.0|TARIFA.cadena.1.0|TOT_NET.doble.19.2|PORDES.doble.6.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|IMP_IVA.doble.19.2|TOT_PED.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|VENCE.fecha.0.0|PORDES1.doble.6.2|PORDES2.doble.6.2|PORDES3.doble.6.2|PORDES4.doble.6.2|DESCUEN1.doble.19.2|DESCUEN2.doble.19.2|DESCUEN3.doble.19.2|DESCUEN4.doble.19.2|VENCE1.fecha.0.0|VENCE2.fecha.0.0|VENCE3.fecha.0.0|VENCE4.fecha.0.0|ESTATUS.entero.2.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|CONDPAG.entero.2.0|TIPOCREDITO.entero.2.0|FORMAPAG.cadena.2.0|NOMPAG.cadena.20.0|NUMPAG.cadena.30.0|ABONO.doble.19.2|SERIE.cadena.3.0|NUMGIRO.entero.4.0|PERGIRO.entero.4.0|INTERES.doble.19.2|PORINT.doble.6.2|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencrel|CODIGOGUIA.cadena.15.0|DESCRIPCION.cadena.100.0|ELABORADOR.cadena.5.0|RESPONSABLE.cadena.50.0|FECHAGUIA.fecha.0.0|EMISIONFAC.fecha.0.0|HASTAFAC.fecha.0.0|ITEMS.entero.4.0|TOTALGUIA.doble.19.2|TOTALKILOS.doble.19.2|ESTATUS.entero.2.0|IMPRESA.entero.2.0|TIPO.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencrgv|CODVEN.cadena.5.0|FECHA.fecha.0.0|CONDICION.entero.2.0|DIA.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencrut|CODRUT.cadena.10.0|NOMRUT.cadena.50.0|COMEN.cadena.250.0|CODZON.cadena.5.0|CODVEN.cadena.5.0|CODCOB.cadena.5.0|DIA.entero.2.0|CONDICION.entero.2.0|CODTRA.cadena.5.0|TIPO.cadena.1.0|ITEMS.entero.4.0|DIVISION.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenexpcli|CODCLI.cadena.15.0|FECHA.fechahora.0.0|COMENTARIO.memo.0.0|CONDICION.cadena.1.0|CAUSA.cadena.5.0|TIPOCONDICION.cadena.1.0|BLOCK_DATE.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsvenforpag|numfac.cadena.15.0|origen.cadena.3.0|formapag.cadena.2.0|numpag.cadena.30.0|nompag.cadena.30.0|importe.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|vence.fecha.0.0|id_emp.cadena.2.0",
                                   "jsvenforpagrgv|CODCLI.cadena.15.0|NUMDOC.cadena.30.0|ASESOR.cadena.5.0|origen.cadena.3.0|formapag.cadena.2.0|numpag.cadena.30.0|nompag.cadena.30.0|importe.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|vence.fecha.0.0|id_emp.cadena.2.0",
                                   "jsvenicsfac|numfac.cadena.15.0|tipoics.cadena.1.0|porics.doble.6.2|baseics.doble.19.2|impics.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenicsncr|NUMNCR.cadena.30.0|tipoics.cadena.1.0|porics.doble.6.2|baseics.doble.19.2|impics.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenicsncrrgv|NUMNCR.cadena.30.0|tipoics.cadena.1.0|porics.doble.6.2|baseics.doble.19.2|impics.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivacot|numcot.cadena.15.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivafac|numfac.cadena.15.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivancr|NUMNCR.cadena.30.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivancrrgv|NUMNCR.cadena.30.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivandb|NUMNDB.cadena.30.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivanot|numfac.cadena.15.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivaped|numped.cadena.15.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenivapedrgv|numped.cadena.15.0|tipoiva.cadena.1.0|poriva.doble.6.2|baseiva.doble.19.2|impiva.doble.19.2|ejercicio.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenliscan|CODIGO.cadena.5.0|DESCRIP.cadena.50.0|ID_EMP.cadena.2.0",
                                   "jsvenlistip|CODIGO.cadena.10.0|DESCRIP.cadena.50.0|ANTEC.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenprocli|CODCLI.cadena.15.0|CODPRO.cadena.15.0|ID_EMP.cadena.2.0",
                                   "jsvenrencie|CODVEN.cadena.5.0|FECHA.fecha.0.0|CLIENTE.cadena.15.0|NOMBRE.cadena.250.0|CAJAS.doble.10.3|KILOS.doble.10.3|COSTOS.doble.19.2|VENTAS.doble.19.2|CONDPAG.entero.2.0|TOTALPEDIDO.doble.19.2|TOTALDEV.doble.19.2|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrencom|NUMDOC.cadena.30.0|origen.cadena.3.0|item.cadena.15.0|renglon.cadena.5.0|comentario.memo.0.0|id_emp.cadena.2.0",
                                   "jsvenrencot|NUMCOT.cadena.15.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.250.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|CANTRAN.doble.10.3|PESO.doble.10.3|ESTATUS.cadena.1.0|PRECIO.doble.19.2|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|ACEPTADO.cadena.1.0|EDITABLE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrennot|NUMFAC.cadena.15.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.250.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|INVENTARIO.doble.10.3|SUGERIDO.doble.10.3|REFUERZO.entero.2.0|PRECIO.doble.19.2|PESO.doble.10.3|LOTE.cadena.10.0|COLOR.cadena.5.0|SABOR.cadena.5.0|ESTATUS.cadena.1.0|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMCOT.cadena.15.0|RENCOT.cadena.5.0|NUMPED.cadena.15.0|RENPED.cadena.5.0|NUMNOT.cadena.15.0|RENNOT.cadena.5.0|CODCON.cadena.30.0|FACDEV.cadena.15.0|CAUSADEV.cadena.5.0|ACEPTADO.cadena.1.0|EDITABLE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenfac|NUMFAC.cadena.15.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.250.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|INVENTARIO.doble.10.3|SUGERIDO.doble.10.3|REFUERZO.entero.2.0|PRECIO.doble.19.2|PESO.doble.10.3|LOTE.cadena.10.0|COLOR.cadena.5.0|SABOR.cadena.5.0|ESTATUS.cadena.1.0|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMCOT.cadena.15.0|RENCOT.cadena.5.0|NUMPED.cadena.15.0|RENPED.cadena.5.0|NUMNOT.cadena.15.0|RENNOT.cadena.5.0|CODCON.cadena.30.0|FACDEV.cadena.15.0|CAUSADEV.cadena.5.0|ACEPTADO.cadena.1.0|EDITABLE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrengui|CODIGOGUIA.cadena.15.0|CODIGOFAC.cadena.15.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|NOMCLI.cadena.100.0|CODVEN.cadena.5.0|KILOSFAC.doble.10.3|TOTALFAC.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenguipedidos|CODIGOGUIA.cadena.15.0|CODIGOFAC.cadena.15.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|NOMCLI.cadena.100.0|CODVEN.cadena.5.0|KILOSFAC.doble.10.3|TOTALFAC.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenncr|NUMNCR.cadena.30.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.250.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.15.0|ESTATUS.cadena.1.0|PRECIO.doble.19.2|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|POR_ACEPTA_DEV.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMFAC.cadena.15.0|CODCON.cadena.30.0|EDITABLE.entero.2.0|CAUSA.cadena.5.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenncrrgv|NUMNCR.cadena.30.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.250.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.15.0|ESTATUS.cadena.1.0|PRECIO.doble.19.2|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|POR_ACEPTA_DEV.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMFAC.cadena.15.0|CODCON.cadena.30.0|EDITABLE.entero.2.0|CAUSA.cadena.5.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenndb|NUMNDB.cadena.30.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.250.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.15.0|ESTATUS.cadena.1.0|PRECIO.doble.19.2|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|POR_ACEPTA_DEV.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMFAC.cadena.15.0|CODCON.cadena.30.0|EDITABLE.entero.2.0|CAUSA.cadena.5.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenped|NUMPED.cadena.15.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.100.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|CANTRAN.doble.10.3|INVENTARIO.doble.10.3|SUGERIDO.doble.10.3|REFUERZO.entero.2.0|PESO.doble.10.3|LOTE.cadena.15.0|ESTATUS.cadena.1.0|PRECIO.doble.19.2|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMCOT.cadena.15.0|RENCOT.cadena.5.0|CODCON.cadena.30.0|ACEPTADO.cadena.1.0|EDITABLE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenpedrgv|NUMPED.cadena.15.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.100.0|IVA.cadena.1.0|ICS.cadena.1.0|UNIDAD.cadena.3.0|BULTOS.doble.10.3|CANTIDAD.doble.10.3|CANTRAN.doble.10.3|INVENTARIO.doble.10.3|SUGERIDO.doble.10.3|REFUERZO.entero.2.0|PESO.doble.10.3|LOTE.cadena.15.0|ESTATUS.cadena.1.0|PRECIO.doble.19.2|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|NUMCOT.cadena.15.0|RENCOT.cadena.5.0|CODCON.cadena.30.0|ACEPTADO.cadena.1.0|EDITABLE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenrel|CODIGOGUIA.cadena.15.0|CODIGOFAC.cadena.15.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|NOMCLI.cadena.100.0|CODVEN.cadena.5.0|KILOSFAC.doble.10.3|TOTALFAC.doble.19.2|ACEPTADO.cadena.1.0|TIPO.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenrut|CODRUT.cadena.10.0|NUMERO.entero.4.0|CLIENTE.cadena.15.0|NOMCLI.cadena.150.0|TIPO.cadena.1.0|ACEPTADO.cadena.1.0|DIVISION.cadena.5.0|CONDICION.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsvenrutcie|RUTACERRADA.cadena.20.0",
                                   "jsvenstaven|CODVEN.cadena.5.0|FECHA.fecha.0.0|ITEM.cadena.15.0|UNIDAD.cadena.3.0|CUOTA.doble.19.2|ACUMULADO.doble.19.2|LOGRO.doble.19.2|META.doble.19.2|ACTIVADOS.entero.4.0|ACTIVACION.doble.6.2|CIERRE.doble.19.2|TIPO.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsventabtic|TICKET.cadena.30.0|CORREDOR.cadena.5.0|MONTO.doble.19.2|PORCOM.doble.6.2|COMISION.doble.19.2|NUMCAN.cadena.15.0|CAJA.cadena.5.0|NUMSOBRE.cadena.15.0|FECHASOBRE.fecha.0.0|NUMDEP.cadena.15.0|FECHADEP.fecha.0.0|BANCODEP.cadena.5.0|CONDICION.entero.2.0|FALSO.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsventracob|CODCLI.cadena.15.0|TIPOMOV.cadena.2.0|NUMMOV.cadena.30.0|EMISION.fecha.0.0|HORA.cadena.10.0|VENCE.fecha.0.0|REFER.cadena.50.0|CONCEPTO.memo.0.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|IMPIVA.doble.19.2|FORMAPAG.cadena.2.0|CAJAPAG.cadena.5.0|NUMPAG.cadena.30.0|NOMPAG.cadena.30.0|benefic.cadena.250.0|ORIGEN.cadena.3.0|NUMORG.cadena.30.0|MULTICAN.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|CODCON.cadena.30.0|MULTIDOC.cadena.15.0|TIPDOCCAN.cadena.2.0|INTERES.doble.19.2|CAPITAL.doble.19.2|COMPROBA.cadena.25.0|BANCO.cadena.5.0|CTABANCO.cadena.50.0|REMESA.cadena.15.0|CODVEN.cadena.5.0|CODCOB.cadena.5.0|HISTORICO.cadena.1.0|FOTIPO.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|DIVISION.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenhiscob|CODCLI.cadena.15.0|TIPOMOV.cadena.2.0|NUMMOV.cadena.30.0|EMISION.fecha.0.0|HORA.cadena.10.0|VENCE.fecha.0.0|REFER.cadena.50.0|CONCEPTO.memo.0.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|IMPIVA.doble.19.2|FORMAPAG.cadena.2.0|CAJAPAG.cadena.5.0|NUMPAG.cadena.30.0|NOMPAG.cadena.30.0|benefic.cadena.250.0|ORIGEN.cadena.3.0|NUMORG.cadena.30.0|MULTICAN.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|CODCON.cadena.30.0|MULTIDOC.cadena.15.0|TIPDOCCAN.cadena.2.0|INTERES.doble.19.2|CAPITAL.doble.19.2|COMPROBA.cadena.25.0|BANCO.cadena.5.0|CTABANCO.cadena.50.0|REMESA.cadena.15.0|CODVEN.cadena.5.0|CODCOB.cadena.5.0|HISTORICO.cadena.1.0|FOTIPO.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|DIVISION.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsventracobcan|codcli.cadena.15.0|tipomov.cadena.2.0|NUMMOV.cadena.30.0|emision.fecha.0.0|refer.cadena.15.0|concepto.memo.0.0|importe.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|comproba.cadena.25.0|codven.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvenvaltic|CODIGO.cadena.5.0|ENBARRA.cadena.15.0|VALOR.doble.19.2|ACEPTADO.cadena.1.0|ID_EMP.cadena.2.0",
                                   "jsvengruposfacturacion|agrupadopor.entero.2.0|codigo.cadena.15.0|ID_EMP.cadena.2.0",
                                   "jsvennestlebpaid_customers|DistribuitorCode.cadena.10.0|CustomerShipTo.cadena.20.0|CustomerSalesTaxCode.cadena.16.0|CustomerName.cadena.35.0|CustomerAddress1.cadena.35.0|CustomerPostCode.cadena.10.0|CustomerCity.cadena.40.0|CustomerRegion.cadena.3.0|CustomerLocationCode.cadena.6.0|CustomerCountry.cadena.3.0|CommercialDenomination.cadena.35.0|StartValidityDate.fechahora.10.0|SalesArea.cadena.6.0|DeliverySpecification.cadena.1.0|BillingSpecification.cadena.1.0|CustomerSoldTo.cadena.20.0|CustomerCategoryCode.cadena.1.0|SuperiorGroupCode.cadena.10.0|DateChangeGroup.fechahora.10.0|PreviousCustomerCode.cadena.10.0|SuccessorCustomerCode.cadena.10.0|PreviousBrand.cadena.1.0|SuccessorBrand.cadena.1.0|StoreType.cadena.6.0|TradeIceFood.cadena.1.0|CustomerCreationDate.fechahora.10.0|CustomerStatus.cadena.1.0|CustomerTerminationReasonCode.cadena.2.0|CustomerTerminationDate.fechahora.10.0|UpdatingFlag.cadena.1.0|ReminderLevelCode.cadena.10.0|CustomerEmailAddress.cadena.30.0|CustomerType.cadena.2.0|Hierarchy6_Code.cadena.15.0|TerritoryCode.cadena.15.0|CreditLimit.cadena.20.0|CreditTermsCode.cadena.3.0|CustomerContactPerson.cadena.10.0|CustomerPhoneNumber.cadena.16.0|CustomerFaxNumber.cadena.31.0|CustomerAddress2.cadena.40.0|CustomerAddress3.cadena.40.0|CustomerAddress4.cadena.40.0|CustomerAddress5.cadena.40.0|CashDiscount.cadena.20.0|SalesManCode.cadena.10.0|SalesManDescription.cadena.30.0|SalesRouteCode.cadena.15.0|SalesRouteDescription.cadena.30.0|OtherDetail1.cadena.20.0|OtherDetail2.cadena.20.0|OtherDetail3.cadena.20.0|OtherDetail4.cadena.20.0|OtherDetail5.cadena.50.0|POCResponsibleGroup.cadena.10.0",
                                   "jsvennestleBpaid_sellout|Distributorcode.cadena.10.0|CustomerShipTo.cadena.20.0|CustomerSoldTo.cadena.20.0|PriceGroup.cadena.2.0|DocType.cadena.4.0|SalesManCode.cadena.10.0|DocNumber.cadena.15.0|DocDate.fechahora.10.0|MaterialCode.cadena.18.0|QuantityInSalesUnitsofMeasure.entero.5.0|UnitOfMeasure.cadena.2.0|RowTotal.entero.10.0|BasePrice.entero.10.0|AmountOfTheGpr.entero.10.0|AmountOfTheTts.entero.10.0|ArticleProgressiveow.entero.3.0|RowReason.cadena.2.0|Handwritten.cadena.1.0|VendorCustomerCode.cadena.10.0|SalesArea.cadena.6.0|SalesRouteCode.cadena.6.0|PurchaseOrderNumber.cadena.35.0|WarehouseCode.cadena.4.0",
                                   "jsvennestleBpaid_stockinventory|StockTakeDate.fechahora.10.0|DistributorCode.cadena.10.0|SalesArea.cadena.6.0|MaterialCode.cadena.18.0|StockConditionCode.cadena.30.0|WarehouseCode.cadena.4.0|SOHQty.entero.10.0|UnitofMeasure.cadena.3.0|SOHValue.entero.10.0|SaleValue.entero.10.0|BatchCode.cadena.10.0|ExpiryDate.fechahora.10.0"}

        ft.Ejecutar_strSQL(myConn, " update jsvencatcli set codcre = 0 ")

        ProcesarTablas(aTablas, Titulo)
        Dim aIndices() As String = {"jsvencatcli|PRIMARY|`codcli`, `ID_EMP`",
                                    "jsvencatcli|SECOND|`rif`, `codcli`, `ID_EMP`",
                                    "jsvencatclisada|PRIMARY|`codcli`, `codigosada`, `ID_EMP`",
                                    "jsvenencpedrgv|PRIMARY|`NUMPED`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencpedrgv|SECONDARY|`EMISION`,`ID_EMP`",
                                    "jsvendespedrgv|PRIMARY|`NUMPED`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenivapedrgv|PRIMARY|`NUMPED`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenpedrgv|PRIMARY|`NUMPED`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenpedrgv|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencped|PRIMARY|`NUMPED`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencgui|PRIMARY|`CODIGOGUIA`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencguipedidos|PRIMARY|`CODIGOGUIA`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencped|SECONDARY|`EMISION`,`ID_EMP`",
                                    "jsvendesped|PRIMARY|`NUMPED`,`RENGLON`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenivaped|PRIMARY|`NUMPED`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenped|PRIMARY|`NUMPED`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenped|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencncr|PRIMARY|`NUMNCR`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencncr|SECONDARY|`EMISION`,`ID_EMP`",
                                    "jsvenforpag|PRIMARY|`numfac`, `numserial`, `origen`, `formapag`, `numpag`, `nompag`, `ID_EMP`, `Currency`",
                                    "jsvenicsncr|PRIMARY|`NUMNCR`,`TIPOICS`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenivancr|PRIMARY|`NUMNCR`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrengui|PRIMARY|`CODIGOGUIA`,`CODIGOFAC`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenguipedidos|PRIMARY|`CODIGOGUIA`,`CODIGOFAC`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenncr|PRIMARY|`NUMNCR`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenncr|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencncrrgv|PRIMARY|`NUMNCR`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenencncrrgv|SECONDARY|`EMISION`,`ID_EMP`",
                                    "jsvenicsncrrgv|PRIMARY|`NUMNCR`,`TIPOICS`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenivancrrgv|PRIMARY|`NUMNCR`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenncrrgv|PRIMARY|`NUMNCR`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`",
                                    "jsvenrenncrrgv|SECONDARY|`ITEM`,`ESTATUS`,`EJERCICIO`,`ID_EMP`",
                                    "jsvendescot|PRIMARY|`numcot`, `renglon`, `ejercicio`, `ID_EMP`",
                                    "jsvencomven|PRIMARY|`codven`, `tipjer`, `tipo`, `ID_EMP`",
                                    "jsvenenccot|PRIMARY|`numcot`, `ejercicio`, `ID_EMP`",
                                    "jsvenivacot|PRIMARY|`numcot`, `tipoiva`, `ejercicio`, `ID_EMP`",
                                    "jsvenrencot|PRIMARY|`numcot`, `renglon`, `item`, `estatus`,`aceptado`,`ejercicio`, `ID_EMP`",
                                    "jsvenrencom|PRIMARY|`numdoc`, `origen`,`item`,`renglon`, `ID_EMP`",
                                    "jsvengruposfacturacion|PRIMARY|`agrupadopor`, `codigo`, `ID_EMP`",
                                    "jsvenstaven|PRIMARY|`CODVEN`, `ITEM`, `FECHA`, `TIPO`, `ID_EMP`",
                                    "jsvenenccie|PRIMARY|`CODVEN`, `FECHA`, `EJERCICIO`, `ID_EMP`",
                                    "jsvenrencie|PRIMARY|`CODVEN`, `FECHA`, `CLIENTE`, `EJERCICIO`, `ID_EMP`",
                                    "jsventracob|PRIMARY|`CODCLI`, `TIPOMOV`, `NUMMOV`, `EMISION`, `HORA`, `EJERCICIO`, `ID_EMP`",
                                    "jsventracob|SECOND|`NUMORG`, `EJERCICIO`, `ID_EMP`",
                                    "jsventracob|TERCERO|`NUMMOV`, `EJERCICIO`, `ID_EMP`",
                                    "jsventracob|CUARTO|`CODCLI`, `EMISION`, `HISTORICO`, `EJERCICIO`, `ID_EMP`",
                                    "jsvenhiscob|PRIMARY|`CODCLI`, `TIPOMOV`, `NUMMOV`, `EMISION`, `HORA`, `EJERCICIO`, `ID_EMP`",
                                    "jsvenhiscob|SECOND|`NUMORG`, `EJERCICIO`, `ID_EMP`",
                                    "jsvenhiscob|TERCERO|`NUMMOV`, `EJERCICIO`, `ID_EMP`",
                                    "jsvenhiscob|CUARTO|`CODCLI`, `EMISION`, `HISTORICO`, `EJERCICIO`, `ID_EMP`",
                                    "jsventracobcan|PRIMARY|`CODCLI`, `TIPOMOV`, `NUMMOV`, `EMISION`, `REFER`,`COMPROBA`, `ID_EMP`",
                                    "jsventrapos|PRIMARY|`NUMMOV`, `CAJA`, `FECHA`, `TIPOMOV`, `FORMPAG`,`NUMPAG`, `NOMPAG`, `EJERCICIO`, `ID_EMP`",
                                    "jsvenvaltic|PRIMARY|`CODIGO`, `ENBARRA`, `ID_EMP`",
                                    "jsvenrutcie|PRIMARY|`RUTACERRADA`"}

        ProcesarIndices(aIndices, Titulo)

    End Sub
    Private Sub ActualizarPuntosdeVenta()

        Dim Titulo As String = "Puntos de Venta..."
        Dim aTablas() As String = {"jsvenaudpos|FECHA.fecha.0.0|CODVEN.cadena.5.0|CODSUP.cadena.5.0|TIPOMOV.cadena.2.0|NUMDOC.cadena.30.0|HORA.cadena.10.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvencatcaj|CODCAJ.cadena.5.0|DESCRIP.cadena.50.0|ALMACEN.cadena.5.0|POS_FAC.entero.1.0|impre_fiscal.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvencatclipv|CODCLI.cadena.15.0|nombre.cadena.250.0|CATEGORIA.cadena.5.0|UNIDAD.cadena.10.0|RIF.cadena.15.0|NIT.cadena.15.0|CI.cadena.10.0|ALTERNO.cadena.10.0|dirfiscal.cadena.250.0|TELEF1.cadena.15.0|FAX.cadena.15.0|INGRESO.fecha.0.0|ESTATUS.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvencatsup|CODIGO.cadena.5.0|DESCRIP.cadena.50.0|NOMBRE.cadena.100.0|CLAVE.cadena.15.0|NIVEL.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsvendespos|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|RENGLON.cadena.5.0|DESCRIP.cadena.100.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvendesposhis|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|RENGLON.cadena.5.0|DESCRIP.cadena.100.0|PORDES.doble.6.2|DESCUENTO.doble.19.2|SUBTOTAL.doble.19.2|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencpos|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|nomcli.cadena.250.0|RIF.cadena.15.0|NIT.cadena.15.0|COMEN.cadena.150.0|ALMACEN.cadena.5.0|VENCE.fecha.0.0|REFER.cadena.15.0|CODCON.cadena.30.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|PORDES.doble.6.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|IMP_IVA.doble.19.2|TOT_FAC.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|NUMERO_RETENCION_IVA.cadena.15.0|NOMBRE_RETENCION_IVA.cadena.15.0|MONTO_RETENCION_IVA.doble.19.2|FECHA_RETENCION_IVA.fecha.0.0|CONDPAG.entero.2.0|TIPOCRE.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|ESTATUS.entero.2.0|TARIFA.cadena.1.0|CODCAJ.cadena.5.0|CODVEN.cadena.5.0|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|ejercicio.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenencposhis|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|EMISION.fecha.0.0|CODCLI.cadena.15.0|nomcli.cadena.250.0|RIF.cadena.15.0|NIT.cadena.15.0|COMEN.cadena.150.0|ALMACEN.cadena.5.0|VENCE.fecha.0.0|REFER.cadena.15.0|CODCON.cadena.30.0|ITEMS.entero.4.0|CAJAS.doble.10.3|KILOS.doble.10.3|TOT_NET.doble.19.2|PORDES.doble.6.2|DESCUEN.doble.19.2|CARGOS.doble.19.2|IMP_IVA.doble.19.2|TOT_FAC.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|NUMERO_RETENCION_IVA.cadena.15.0|NOMBRE_RETENCION_IVA.cadena.15.0|MONTO_RETENCION_IVA.doble.19.2|FECHA_RETENCION_IVA.fecha.0.0|CONDPAG.entero.2.0|TIPOCRE.cadena.1.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|ESTATUS.entero.2.0|TARIFA.cadena.1.0|CODCAJ.cadena.5.0|CODVEN.cadena.5.0|IMPRESA.entero.2.0|BLOCK_DATE.fecha.0.0|ejercicio.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsveninipos|FECHA.fecha.0.0|CODVEN.cadena.5.0|CODSUP.cadena.5.0|MONTOINICIO.doble.19.2|HORAINICIO.cadena.10.0|HORACIERRE.cadena.10.0|CODCAJ.cadena.5.0|ESTATUS.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenivapos|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenivaposhis|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenpos|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|BARRAS.cadena.30.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PRECIO.doble.19.2|PESO.doble.10.3|LOTE.cadena.10.0|ESTATUS.cadena.1.0|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|CODCON.cadena.30.0|ACEPTADO.cadena.1.0|EDITABLE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenrenposhis|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|tipo.entero.2.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|BARRAS.cadena.30.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PRECIO.doble.19.2|PESO.doble.10.3|LOTE.cadena.10.0|ESTATUS.cadena.1.0|DES_CLI.doble.6.2|DES_ART.doble.6.2|DES_OFE.doble.6.2|TOTREN.doble.19.2|TOTRENDES.doble.19.2|CODCON.cadena.30.0|ACEPTADO.cadena.1.0|EDITABLE.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsventrapos|CAJA.cadena.5.0|FECHA.fecha.0.0|ORIGEN.cadena.3.0|TIPOMOV.cadena.2.0|NUMMOV.cadena.30.0|FORMPAG.cadena.2.0|NUMPAG.cadena.30.0|NOMPAG.cadena.30.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|FECHACIERRE.fecha.0.0|CANTIDAD.entero.4.0|CAJERO.cadena.5.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsventraposhis|CAJA.cadena.5.0|FECHA.fecha.0.0|ORIGEN.cadena.3.0|TIPOMOV.cadena.2.0|NUMMOV.cadena.30.0|FORMPAG.cadena.2.0|NUMPAG.cadena.30.0|NOMPAG.cadena.30.0|IMPORTE.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|FECHACIERRE.fecha.0.0|CANTIDAD.entero.4.0|CAJERO.cadena.5.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsvenforpag|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|origen.cadena.3.0|formapag.cadena.2.0|numpag.cadena.30.0|nompag.cadena.30.0|importe.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|vence.fecha.0.0|id_emp.cadena.2.0",
                                   "jsvenforpaghis|NUMFAC.cadena.15.0|NUMSERIAL.cadena.15.0|origen.cadena.3.0|formapag.cadena.2.0|numpag.cadena.30.0|nompag.cadena.30.0|importe.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|vence.fecha.0.0|id_emp.cadena.2.0",
                                   "jsvenperven|CODPER.cadena.5.0|DESCRIP.cadena.50.0|CR.entero.2.0|CO.entero.2.0|TARIFA_A.entero.2.0|TARIFA_B.entero.2.0|TARIFA_C.entero.2.0|TARIFA_D.entero.2.0|TARIFA_E.entero.2.0|TARIFA_F.entero.2.0|ALMACEN.cadena.5.0|DESCUENTO.entero.2.0|id_emp.cadena.2.0",
                                   "jsvenpervencaj|CODCAJ.cadena.5.0|CODPER.cadena.5.0|id_emp.cadena.2.0",
                                   "jsvencadippos|TIPORAZON.cadena.1.0|DOCUMENTO.cadena.15.0|IDENTIFICADOR.cadena.1.0|CANTIDAD.entero.4.0|CODIGO.entero.6.0|FECHAMOV.fecha.0.0|HORA.cadena.10.0|FACTURA.cadena.15.0|PROCESADO.entero.1.0|FECHAPROCESO.fechahora.0.0|ID_EMP.cadena.2.0",
                                   "jsvencadipposX|TIPORAZON.cadena.1.0|DOCUMENTO.cadena.15.0|TIPOCOMERCIO.cadena.1.0|CODIGO_PRODUCTO.entero.6.0|CANTIDAD_A_COMPRAR.entero.4.0|CANTIDAD_COMPRADA.entero.4.0|FECHAINICIOCOMPRA.fecha.0.0|FECHAULTIMACOMPRA.fecha.0.0|FECHAPROXIMACOMPRA.fecha.0.0|ESTATUS.entero.2.0|ID_EMP.cadena.2.0"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jsvenencpos|PRIMARY|`numfac`, `numserial`, `tipo`, `codcli`, `ejercicio`, `ID_EMP`",
                                    "jsvenrenpos|PRIMARY|`numfac`, `numserial`, `tipo`, `renglon`, `item`,`estatus`, `aceptado`, `ID_EMP`",
                                    "jsvenivapos|PRIMARY|`numfac`, `numserial`, `tipo`, `tipoiva`, `ejercicio`, `ID_EMP`",
                                    "jsvendespos|PRIMARY|`numfac`, `numserial`, `tipo`, `renglon`, `ejercicio`, `ID_EMP`",
                                    "jsvenforpag|PRIMARY|`numfac`, `numserial`, `origen`, `formapag`, `numpag`, `nompag`, `ID_EMP`, `Currency`",
                                    "jsventrapos|PRIMARY|`nummov`, `caja`,  `fecha`, `tipomov`, `formpag`, `numpag`, `nompag`,  `ejercicio`, `ID_EMP`",
                                    "jsvencatclipv|PRIMARY|`RIF`, `CODCLI`, `ID_EMP`",
                                    "jsvenperven|PRIMARY|`CODPER`,`ID_EMP`",
                                    "jsvenpervencaj|PRIMARY|`CODCAJ`,`CODPER`,`ID_EMP`",
                                    "jsvencadippos|PRIMARY|`FACTURA`,`FECHAMOV`,`TIPORAZON`, `DOCUMENTO`, `IDENTIFICADOR`,`CODIGO`,`ID_EMP`",
                                    "jsvencadipposx|PRIMARY|`TIPORAZON`,`DOCUMENTO`,`CODIGO_PRODUCTO`,`ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)

    End Sub

    Private Sub ActualizarMercancias()

        Dim Titulo As String = "Mercancías..."

        Dim aTablas() As String = {"jsmercatalm|CODALM.cadena.5.0|DESALM.cadena.50.0|RESPONSABLE.cadena.100.0|TIPOALM.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsmercatest|CODEST.cadena.5.0|DESCRIP.cadena.50.0|TIPO.entero.2.0|CODALM.cadena.5.0|UBICA_ALM.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmercatubi|CODUBI.cadena.5.0|LAD.cadena.5.0|FIL.cadena.5.0|COL.cadena.5.0|CODEST.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmercatcom|CODART.cadena.15.0|CODARTCOM.cadena.15.0|DESCRIP.cadena.150.0|CANTIDAD.doble.10.3|UNIDAD.cadena.3.0|COSTO.doble.19.2|PESO.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsmercatdiv|DIVISION.cadena.5.0|DESCRIP.cadena.150.0|color.cadena.30.0|ID_EMP.cadena.2.0",
                                   "jsmercatser|CODSER.cadena.15.0|DESSER.cadena.150.0|PORCENTAJE.doble.6.2|PRECIO.doble.19.2|PRECIO_A.doble.19.2|PRECIO_B.doble.19.2|PRECIO_C.doble.19.2|HORAS.doble.10.3|HORAS_A.doble.10.3|HORAS_B.doble.10.3|HORAS_C.doble.10.3|COL.entero.2.0|CMS.entero.2.0|TIPOIVA.cadena.1.0|tiposervicio.entero.2.0|CODART_CODSER.cadena.15.0|TIPO.entero.2.0|CLASE.entero.2.0|CODCON.cadena.30.0|ID_EMP.cadena.2.0",
                                   "jsmerconmer|CONMER.cadena.10.0|CODEST.cadena.5.0|CODUBI.cadena.5.0|CODART.cadena.15.0|NOMART.cadena.150.0|UNIDAD.cadena.3.0|CONTEO.doble.10.3|EXISTENCIA.doble.10.3|COSTOU.doble.19.2|COSTO_TOT.doble.19.2|EXIST1.doble.10.3|CONT1.doble.10.3|EXIST2.doble.10.3|CONT2.doble.10.3|EXIST3.doble.10.3|CONT3.doble.10.3|EXIST4.doble.10.3|CONT4.doble.10.3|EXIST5.doble.10.3|CONT5.doble.10.3|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerconmerdinamica|CONMER.cadena.10.0|CODEST.cadena.5.0|CODUBI.cadena.5.0|CODART.cadena.15.0|NOMART.cadena.150.0|UNIDAD.cadena.3.0|CUENTA.entero.2.0|CANTIDAD.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsmerctacuo|CODART.cadena.15.0|MES01.doble.10.3|MES02.doble.10.3|MES03.doble.10.3|MES04.doble.10.3|MES05.doble.10.3|MES06.doble.10.3|MES07.doble.10.3|MES08.doble.10.3|MES09.doble.10.3|MES10.doble.10.3|MES11.doble.10.3|MES12.doble.10.3|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerctainv|CODART.cadena.15.0|NOMART.cadena.150.0|ALTERNO.cadena.15.0|BARRAS.cadena.20.0|CREACION.fecha.0.0|GRUPO.cadena.5.0|MARCA.cadena.5.0|DIVISION.cadena.5.0|IVA.cadena.1.0|ICS.cadena.1.0|SACS.cadena.20.0|CEP.cadena.20.0|PRESENTACION.cadena.15.0|SUGERIDO.doble.19.2|PRECIO_A.doble.19.2|DESC_A.doble.6.2|OFERTA_A.cadena.10.0|GANAN_A.doble.6.2|PRECIO_B.doble.19.2|DESC_B.doble.6.2|OFERTA_B.cadena.10.0|GANAN_B.doble.6.2|PRECIO_C.doble.19.2|DESC_C.doble.6.2|OFERTA_C.cadena.10.0|GANAN_C.doble.10.3|PRECIO_D.doble.19.2|DESC_D.doble.6.2|OFERTA_D.cadena.10.0|GANAN_D.doble.6.2|PRECIO_E.doble.19.2|DESC_E.doble.6.2|OFERTA_E.cadena.10.0|GANAN_E.doble.6.2|PRECIO_F.doble.19.2|DESC_F.doble.6.2|OFERTA_F.cadena.10.0|GANAN_F.doble.6.2|BARRA_A.cadena.20.0|BARRA_B.cadena.20.0|BARRA_C.cadena.20.0|BARRA_D.cadena.20.0|BARRA_E.cadena.20.0|BARRA_F.cadena.20.0|UNIDAD.cadena.3.0|DIVIDEUV.cadena.1.0|UNIDADDETAL.cadena.3.0|EXMAX.doble.10.3|EXMIN.doble.10.3|UBICACION.cadena.15.0|PESOUNIDAD.doble.10.3|CCUNIDAD.entero.4.0|ALTURA.doble.10.3|ANCHO.doble.10.3|PROFUN.doble.10.3|TIPOART.entero.2.0|CUOTA.entero.2.0|CUOTAFIJA.entero.2.0|ORDENES.doble.10.3|RECEPCIONES.doble.10.3|BACKORDER.doble.10.3|FECULTCOSTO.fecha.0.0|ULTIMOPROVEEDOR.cadena.15.0|MONTOULTIMACOMPRA.doble.19.2|COSTO_PROM.doble.19.2|COSTO_PROM_DES.doble.19.2|ENTRADAS.doble.10.3|CREDITOSCOMPRAS.doble.10.3|DEBITOSCOMPRAS.doble.10.3|ACU_COS.doble.19.2|ACU_COS_DES.doble.19.2|ACU_COD.doble.19.2|ACU_COD_DES.doble.19.2|COTIZADOS.doble.10.3|PEDIDOS.doble.10.3|ENTREGAS.doble.10.3|FECULTVENTA.fecha.0.0|ULTIMOCLIENTE.cadena.15.0|MONTOULTIMAVENTA.doble.19.2|VENTA_PROM.doble.19.2|VENTA_PROM_DES.doble.19.2|SALIDAS.doble.10.3|CREDITOSVENTAS.doble.10.3|DEBITOSVENTAS.doble.10.3|ACU_PRE.doble.19.2|ACU_PRE_DES.doble.19.2|ACU_PRD.doble.19.2|ACU_PRD_DES.doble.19.2|EXISTE_ACT.doble.10.3|CODCON.cadena.30.0|ESTATUS.cadena.1.0|codjer1.cadena.15.0|codjer2.cadena.15.0|codjer3.cadena.15.0|codjer4.cadena.15.0|codjer5.cadena.15.0|codjer6.cadena.15.0|tipjer.cadena.5.0|MIX.cadena.1.0|DEVOLUCION.cadena.1.0|DESCUENTO.entero.2.0|POR_ACEPTA_DEV.doble.6.2|REGULADO.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsmercatenv|CODENV.cadena.15.0|SERIAL_1.cadena.15.0|SERIAL_2.cadena.15.0|DESCRIPCION.cadena.100.0|CODIGO_CONTENIDO.cadena.15.0|CAPACIDAD.doble.10.3|UNIDAD.cadena.3.0|FECHA_REVISION.fecha.0.0|REVISIONES.entero.5.0|FABRICANTE.cadena.15.0|COMPRADOR.cadena.15.0|FECHA_ADQUISICION.fecha.0.0|NUM_LOTE.cadena.20.0|FECHA_FABRICACION.fecha.0.0|PRESION.doble.10.3|TIPO_PRESION.entero.2.0|TARA_COMPRA.doble.10.3|TARA_VENTA.doble.10.3|MATERIAL.cadena.5.0|TIPO.entero.2.0|ESTATUS.entero.2.0|PROPIETARIO.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsmercattar|CODENV.cadena.15.0|PROV_CLI.cadena.15.0|ORIGEN.cadena.3.0|TARA.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsmerctaser|CODART.cadena.15.0|SERIAL1.cadena.15.0|SERIAL2.cadena.15.0|SERIAL3.cadena.15.0|E_ORIGEN.cadena.3.0|E_NUMDOC.cadena.30.0|S_ORIGEN.cadena.3.0|S_NUMDOC.cadena.30.0|CONDICION.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerenccon|CONMER.cadena.10.0|FECHACON.fecha.0.0|ALMACEN.cadena.5.0|COMENTARIO.cadena.150.0|PROCESADO.entero.2.0|FECHAPRO.fecha.0.0|TIPOUNIDAD.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerenccuo|CODCUO.cadena.5.0|DESCRIPCION.cadena.100.0|DESDE.fecha.0.0|HASTA.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsmerencjer|tipjer.cadena.5.0|DESCRIP.cadena.100.0|mascara1.cadena.30.0|mascara2.cadena.30.0|mascara3.cadena.30.0|mascara4.cadena.30.0|mascara5.cadena.30.0|mascara6.cadena.30.0|DESCRIP1.cadena.30.0|DESCRIP2.cadena.30.0|DESCRIP3.cadena.30.0|DESCRIP4.cadena.30.0|DESCRIP5.cadena.30.0|DESCRIP6.cadena.30.0|PROVEEDOR.cadena.15.0|CODCON.cadena.30.0|ID_EMP.cadena.2.0",
                                   "jsmerenclispre|CODLIS.cadena.5.0|DESCRIP.cadena.250.0|EMISION.fecha.0.0|VENCE.fecha.0.0|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ID_EMP.cadena.2.0",
                                   "jsmerencofe|CODOFE.cadena.15.0|DESCRIP.cadena.150.0|DESDE.fecha.0.0|HASTA.fecha.0.0|TARIFA_A.cadena.1.0|TARIFA_B.cadena.1.0|TARIFA_C.cadena.1.0|TARIFA_D.cadena.1.0|TARIFA_E.cadena.1.0|TARIFA_F.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerenctra|NUMTRA.cadena.15.0|EMISION.fecha.0.0|ALM_SALE.cadena.5.0|ALM_ENTRA.cadena.5.0|COMEN.cadena.250.0|CAUSA.cadena.5.0|TOTALTRA.doble.19.2|TOTALCAN.doble.10.3|ITEMS.entero.4.0|PESOTOTAL.doble.10.3|tipo.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerequmer|CODART.cadena.15.0|UNIDAD.cadena.3.0|equivale.doble.10.5|UVALENCIA.cadena.3.0|DIVIDE.entero.2.0|ENVASE.entero.2.0|CODIGO_ENVASE.cadena.15.0|ID_EMP.cadena.2.0",
                                   "jsmerexpmer|CODART.cadena.15.0|FECHA.fechahora.0.0|COMENTARIO.memo.0.0|CONDICION.cadena.1.0|CAUSA.cadena.5.0|TIPOCONDICION.cadena.5.0|BLOCK_DATE.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsmerextalm|CODART.cadena.15.0|ALMACEN.cadena.5.0|EXISTENCIA.doble.10.3|UBICACION.cadena.15.0|ID_EMP.cadena.2.0",
                                   "jsmerlispre|FECHA.fecha.0.0|CODART.cadena.15.0|TIPOPRECIO.cadena.1.0|MONTO.doble.19.2|DES_ART.doble.6.2|PROCESADO.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsmerlotmer|CODART.cadena.15.0|LOTE.cadena.30.0|EXPIRACION.fecha.0.0|ENTRADAS.doble.10.3|SALIDAS.doble.10.3|COSTO.doble.19.2|PRECIO_A.doble.19.2|PRECIO_B.doble.19.2|PRECIO_C.doble.19.2|PRECIO_D.doble.19.2|PRECIO_E.doble.19.2|PRECIO_F.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsmerrenbon|CODOFE.cadena.15.0|CODART.cadena.15.0|RENGLON.cadena.5.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|CANTIDADBON.doble.10.3|CANTIDADINICIO.doble.10.3|UNIDADBON.cadena.3.0|ITEMBON.cadena.15.0|NOMBREITEMBON.cadena.150.0|OTORGACAN.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerrencuo|CODCUO.cadena.5.0|alterno.cadena.15.0|T1.cadena.5.0|T2.cadena.5.0|T3.cadena.5.0|T4.cadena.5.0|T5.cadena.5.0|T6.cadena.5.0|T7.cadena.5.0|T8.cadena.5.0|T9.cadena.5.0|T10.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerrenjer|tipjer.cadena.5.0|CODJER.cadena.30.0|DESJER.cadena.50.0|NIVEL.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsmerrenlispre|CODLIS.cadena.5.0|CODART.cadena.15.0|PRECIO.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsmerrenofe|CODOFE.cadena.15.0|RENGLON.cadena.5.0|CODART.cadena.15.0|DESCRIP.cadena.150.0|UNIDAD.cadena.3.0|LIMITEI.doble.10.3|LIMITES.doble.10.3|PORCENTAJE.doble.6.2|OTORGAPOR.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerrentra|NUMTRA.cadena.15.0|renglon.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PESO.doble.10.3|LOTE.cadena.15.0|COSTOU.doble.19.2|TOTREN.doble.19.2|NUMPED.cadena.30.0|RENPED.cadena.5.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmertramer|CODART.cadena.15.0|FECHAMOV.fechahora.0.0|TIPOMOV.cadena.2.0|NUMDOC.cadena.30.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PESO.doble.10.3|COSTOTAL.doble.19.2|COSTOTALDES.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ORIGEN.cadena.3.0|NUMORG.cadena.20.0|LOTE.cadena.15.0|PROV_CLI.cadena.15.0|VENTOTAL.doble.19.2|VENTOTALDES.doble.19.2|IMPIVA.doble.19.2|DESCUENTO.doble.19.2|VENDEDOR.cadena.5.0|ALMACEN.cadena.5.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerhismer|CODART.cadena.15.0|FECHAMOV.fechahora.0.0|TIPOMOV.cadena.2.0|NUMDOC.cadena.30.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PESO.doble.10.3|COSTOTAL.doble.19.2|COSTOTALDES.doble.19.2|CURRENCY.entero.4.0|CURRENCY_DATE.fechahora.0.0|ORIGEN.cadena.3.0|NUMORG.cadena.20.0|LOTE.cadena.15.0|PROV_CLI.cadena.15.0|VENTOTAL.doble.19.2|VENTOTALDES.doble.19.2|IMPIVA.doble.19.2|DESCUENTO.doble.19.2|VENDEDOR.cadena.5.0|ALMACEN.cadena.5.0|ASIENTO.cadena.15.0|FECHASI.fecha.0.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmertraenv|CODENV.cadena.15.0|SERIAL_1.cadena.20.0|SERIAL_2.cadena.20.0|ITEM.cadena.15.0|RENGLON.cadena.5.0|FECHAMOV.fechahora.0.0|TIPOMOV.cadena.2.0|NUMDOC.cadena.30.0|CANTIDAD.entero.10.0|ORIGEN.cadena.3.0|PROV_CLI.cadena.15.0|VENDEDOR.cadena.5.0|TRANSPORTE.cadena.5.0|ALMACEN.cadena.5.0|ESTATUS.entero.2.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerctainvfot|CODART.cadena.15.0|FOTO1.blob.0.0|FOTO2.blob.0.0|FOTO3.blob.0.0|ID_EMP.cadena.2.0",
                                   "jsmercodsica|CODIGO.cadena.15.0|RUBRO.cadena.30.0|CANTIDAD.entero.2.0|FRECUENCIA.entero.2.0|CATEGORIA.cadena.5.0",
                                   "jsmerencped|NUMPED.cadena.30.0|EMISION.fecha.0.0|ENTREGA.fecha.0.0|ALM_ORIGEN.cadena.5.0|ALM_DESTINO.cadena.5.0|COMEN.cadena.100.0|TOT_NET.doble.19.2|IMP_IVA.doble.19.2|TOT_PED.doble.19.2|ESTATUS.cadena.1.0|ITEMS.entero.4.0|IMPRESA.cadena.1.0|BLOCK_DATE.fecha.0.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerivaped|NUMPED.cadena.30.0|TIPOIVA.cadena.1.0|PORIVA.doble.6.2|BASEIVA.doble.19.2|IMPIVA.doble.19.2|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsmerrenped|NUMPED.cadena.30.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|IVA.cadena.1.0|UNIDAD.cadena.3.0|CANTIDAD.doble.10.3|PESO.doble.10.3|CANTRAN.doble.10.3|ESTATUS.cadena.1.0|COSTOU.doble.19.2|COSTOTOT.doble.19.2|LOTE.cadena.10.0|ACEPTADO.cadena.1.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jsmercatalm|PRIMARY|`codalm`,`id_emp`",
                                    "jsmercatest|PRIMARY|`codest`,`codalm`, `id_emp`",
                                    "jsmercatubi|PRIMARY|`codubi`,`codest`, `id_emp`",
                                    "jsmercatcom|PRIMARY|`codart`,`codartcom`, `id_emp`",
                                    "jsmercatdiv|PRIMARY|`division`,`id_emp`",
                                    "jsmercatser|PRIMARY|`codser`,`tipo`,`id_emp`",
                                    "jsmerconmer|PRIMARY|`conmer`,`codart`,`ejercicio`, `id_emp`",
                                    "jsmerconmerdinamica|PRIMARY|`conmer`, `codest`, `codubi`, `codart`,`cuenta`, `id_emp`",
                                    "jsmerctacuo|PRIMARY|`codart`,`ejercicio`, `id_emp`",
                                    "jsmercatcom|PRIMARY|`codart`,`codartcom`, `id_emp`",
                                    "jsmerctainv|PRIMARY|`codart`,`id_emp`",
                                    "jsmercatenv|PRIMARY|`codenv`,`serial_1`,`id_emp`",
                                    "jsmerencconint|PRIMARY|`numcon`,`ejercicio`, `id_emp`",
                                    "jsmerenccuo|PRIMARY|`codcuo`,`id_emp`",
                                    "jsmerencjer|PRIMARY|`tipjer`,`id_emp`",
                                    "jsmerenclispre|PRIMARY|`codlis`, `id_emp`",
                                    "jsmerencofe|PRIMARY|`codofe`,`ejercicio`, `id_emp`",
                                    "jsmerenctra|PRIMARY|`numtra`,`tipo`,`ejercicio`, `id_emp`",
                                    "jsmerequmer|PRIMARY|`codart`,`unidad`, `uvalencia`, `id_emp`",
                                    "jsmerequmer|PRIMARY|`codart`,`uvalencia`, `id_emp`",
                                    "jsmerexpmer|PRIMARY|`codart`,`fecha`,`condicion`,`causa`, `id_emp`",
                                    "jsmerextalm|PRIMARY|`codart`,`almacen`, `id_emp`",
                                    "jsmerhismer|PRIMARY|`codart`,`fechamov`,`tipomov`,`unidad`,`numdoc`, `almacen`,`asiento`,`ejercicio`, `id_emp`",
                                    "jsmerhismer|SECOND|`numdoc`,`tipomov`, `origen`,`numorg`, `ejercicio`, `id_emp`",
                                    "jsmerhismer|THIRD|`numdoc`,`origen`, `numorg`, `ejercicio`, `id_emp`",
                                    "jsmerhismer|FOURTH|`fechamov`,`tipomov`, `origen`, `prov_cli`, `id_emp`",
                                    "jsmerhismer|FIFTH|`prov_cli`,`ejercicio`, `id_emp`",
                                    "jsmerhismer|SIXTH|`fechamov`,`almacen`, `id_emp`",
                                    "jsmerlispre|PRIMARY|`fecha`,`codart`,`tipoprecio`, `id_emp`",
                                    "jsmerlotmer|PRIMARY|`codart`,`lote`, `id_emp`",
                                    "jsmerrenconint|PRIMARY|`numcon`,`renglon`,`item`,`aceptado`, `ejercicio`, `id_emp`",
                                    "jsmerrencuo|PRIMARY|`codcuo`,`alterno`, `id_emp`",
                                    "jsmerrenlispre|PRIMARY|`codlis`,`codart`, `id_emp`",
                                    "jsmerrenofe|PRIMARY|`codofe`,`renglon`, `codart`, `aceptado`,`ejercicio`, `id_emp`",
                                    "jsmertramer|PRIMARY|`codart`,`fechamov`,`tipomov`,`unidad`,`numdoc`,`almacen`,`asiento`,`ejercicio`, `id_emp`",
                                    "jsmertramer|SECOND|`numdoc`,`tipomov`, `origen`,`numorg`, `ejercicio`, `id_emp`",
                                    "jsmertramer|THIRD|`numdoc`,`origen`, `numorg`, `ejercicio`, `id_emp`",
                                    "jsmertramer|FOURTH|`fechamov`,`tipomov`, `origen`, `prov_cli`, `id_emp`",
                                    "jsmertramer|FIFTH|`prov_cli`,`ejercicio`, `id_emp`",
                                    "jsmertramer|SIXTH|`fechamov`,`almacen`, `id_emp`",
                                    "jsmertramer|SEVENTH|`numorg`,`tipomov`,`origen`,`id_emp`",
                                    "jsmertraenv|PRIMARY|`codenv`,`serial_1`, `fechamov`, `tipomov`,`numdoc`,`item`, `renglon`, `id_emp`",
                                    "jsmercodsica|PRIMARY|`codigo`",
                                    "jsmerencped|PRIMARY|`NUMPED`,`EJERCICIO`,`ID_EMP`",
                                    "jsmerivaped|PRIMARY|`NUMPED`,`TIPOIVA`,`EJERCICIO`,`ID_EMP`",
                                    "jsmerrenped|PRIMARY|`NUMPED`,`RENGLON`,`ITEM`,`ESTATUS`,`ACEPTADO`,`EJERCICIO`,`ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)

    End Sub

    Private Sub ActualizarProduccion()

        Dim Titulo As String = "Producción..."
        Dim aTablas() As String = {"jsfabencfor|CODFOR.cadena.15.0|CODART.cadena.15.0|DESCRIP_1.cadena.250.0|DESCRIP_2.cadena.250.0|CANTIDAD.doble.10.3|UNIDAD.cadena.3.0|PESO_TOTAL.doble.10.3|ALMACEN_DESTINO.cadena.5.0|TOTAL_NETO.doble.19.2|TOTAL_INDIRECTOS.doble.19.2|TOTAL.doble.19.2|COMENTARIOS.memo.0.0|FECHA.fecha.0.0|ESTATUS.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsfabrenfor|CODFOR.cadena.15.0|RENGLON.cadena.5.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|CANTIDAD.doble.10.3|UNIDAD.cadena.3.0|PESO_RENGLON.doble.10.3|COSTOU.doble.19.2|TOTREN.doble.19.2|ALMACEN_SALIDA.cadena.5.0|RESIDUAL.entero.2.0|SUBENSAMBLE.entero.2.0|PORCENTAJE.doble.6.2|ID_EMP.cadena.2.0",
                                   "jsfabencord|CODORD.cadena.15.0|DESCRIP.cadena.150.0|EMISION.fecha.0.0|ESTIMADA.fecha.0.0|PESO_TOTAL.doble.10.3.PESO TOTAL ORDEN DE PRODUCCION|COSTO_TOTAL.doble.19.2.COSTO TOTAL DE LA ORDEN DE PRODUCCION|ESTATUS.entero.2.0|ORDEN_RELACIONADA.cadena.15.0.NUMERO DE ORDEN PRINCIPAL|BLOCK_DATE.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsfabrenord|CODORD.cadena.15.0|CODFOR.cadena.15.0|DESCRIP.cadena.150.0|CANTIDAD.doble.10.3|PESO.doble.10.3|UNIDAD.cadena.3.0|COSTOUNITARIO.doble.19.2|TOTALRENGLON.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsfabencent|CODENT.cadena.15.0|CODORD.cadena.15.0|CODFOR.cadena.15.0|FECHA_ENTREGA.fecha.0.0|CANTIDAD_ENTREGA.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsfabrenent|CODENT.cadena.15.0|CODORD.cadena.15.0|CODFOR.cadena.15.0|ITEM.cadena.15.0|DESCRIP.cadena.150.0|CANTIDAD.doble.10.3|UNIDAD.cadena.3.0|PREVISTA.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsfabtraord|CODORD.cadena.15.0|CODFOR.cadena.15.0|ITEM.cadena.15.0|PENDIENTE.doble.10.3|APARTADA.doble.10.3|CONSUMIDA.doble.10.3|ID_EMP.cadena.2.0",
                                   "jsfabadjfor|CODFOR.cadena.15.0|RENGLON.cadena.5.0|CAMINO.cadena.250.0|ID_EMP.cadena.2.0",
                                   "jsfabfijfor|CODFOR.cadena.15.0|CODCOSTO.cadena.5.0|TITULO.cadena.150.0|PORCENTAJE.doble.6.2|MONTOFIJO.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsfabcatfij|CODCOSTO.cadena.5.0|TITULO.cadena.150.0|PORCENTAJE.doble.6.2|MONTOFIJO.doble.19.2|ID_EMP.cadena.2.0"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jsfabencfor|PRIMARY|`CODFOR`, `ID_EMP`",
                                    "jsfabrenfor|PRIMARY|`CODFOR`, `RENGLON`, `ITEM`,`RESIDUAL`,`ID_EMP`",
                                    "jsfabencord|PRIMARY|`CODORD`, `ID_EMP`",
                                    "jsfabrenord|PRIMARY|`CODORD`, `CODFOR`, `ID_EMP`",
                                    "jsfabencent|PRIMARY|`CODENT`, `CODORD`,`CODFOR`, `ID_EMP`",
                                    "jsfabrenent|PRIMARY|`CODENT`, `CODORD`, `CODFOR`,`ITEM`,`ID_EMP`",
                                    "jsfabadjfor|PRIMARY|`CODFOR`,`RENGLON`,`ID_EMP`",
                                    "jsfabfijfor|PRIMARY|`CODFOR`,`CODCOSTO`,`ID_EMP`",
                                    "jsfabcatfij|PRIMARY|`CODCOSTO`,`ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)


    End Sub

    Private Sub ActualizarControlDeGestiones()

        Dim Titulo As String = "Control de Gestiones..."
        Dim aTablas() As String = {"jsconcatdes|CODDES.cadena.5.0|DESCRIP.cadena.50.0|PORDES.doble.6.2|INICIO.fecha.0.0|FIN.fecha.0.0|CODVEN.cadena.5.0|TIPO.cadena.1.0|ID_EMP.cadena.2.0",
                                   "jsconcatmon|Id.entero.10.0|pais.cadena.250.0|UnidadMonetaria.cadena.250.0|simbolo.cadena.50.0|codigoISO.cadena.50.0|UnidadFraccionaria.cadena.250.0|Division.entero.10.0|Notas.cadena.250.0",
                                   "jsconcatper|MES.entero.2.0|ANO.entero.4.0|DIA.entero.2.0|DESCRIPCION.cadena.50.0|TIPO.entero.2.0|MODULO.entero.2.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsconcatter|CODIGO.enterolargo10A.0.0|NOMBRE.cadena.50.0|MONTOFLETE.doble.19.2|TIPO.cadena.1.0|ANTECESOR.cadena.10.0|ZONA.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsconcojcat|CODIGO.cadena.5.0|DESCRIP.cadena.50.0|GRUPO.cadena.50.0|ORDEN.cadena.50.0|GESTION.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsconcojtab|CODIGO.cadena.5.0|LETRA.cadena.1.0|tabla.cadena.250.0|tipo.entero.2.0|relacion.cadena.250.0|GESTION.entero.2.0|ID_EMP.cadena.2.0",
                                   "jsconcontadores|gestion.cadena.2.0|modulo.cadena.2.0|codigo.cadena.15.0|descripcion.cadena.150.0|contador.cadena.30.0|id_emp.cadena.2.0",
                                   "jsconctacam|FECHA.fechahora.0.0|moneda.cadena.5.0|equivale.doble.19.6|id_emp.cadena.2.0",
                                   "jsconctacom|CODIGO.cadena.3.0|COMENTARIO.cadena.250.0|ORIGEN.cadena.3.0|ID_EMP.cadena.2.0",
                                   "jsconctaeje|EJERCICIO.cadena.5.0|INICIO.fecha.0.0|CIERRE.fecha.0.0|ID_EMP.cadena.2.0",
                                   "jsconctaemp|ID_EMP.cadena.2.0|NOMBRE.cadena.100.0|DIRFISCAL.cadena.250.0|DIRORIGEN.cadena.250.0|CIUDAD.cadena.20.0|ESTADO.cadena.20.0|CODGEO.cadena.10.0|ZIP.cadena.10.0|TELEF1.cadena.15.0|TELEF2.cadena.15.0|FAX.cadena.15.0|EMAIL.cadena.50.0|ACTIVIDAD.cadena.100.0|RIF.cadena.15.0|NIT.cadena.15.0|CIIU.cadena.10.0|TIPOPERSONA.cadena.1.0|INICIO.fecha.0.0|CIERRE.fecha.0.0|TIPOSOC.cadena.1.0|LUCRO.cadena.1.0|NACIONAL.cadena.1.0|CI.cadena.10.0|PASAPORTE.cadena.10.0|CASADO.cadena.1.0|SEPARABIENES.cadena.1.0|RENTASEXENTAS.cadena.1.0|ESPOSADECLARA.cadena.1.0|REP_RIF.cadena.15.0|REP_NIT.cadena.15.0|REP_NACIONAL.cadena.1.0|REP_CI.cadena.15.0|REP_NOMBRE.cadena.150.0|REP_DIRECCION.cadena.250.0|REP_CIUDAD.cadena.15.0|REP_ESTADO.cadena.15.0|REP_TELEF.cadena.15.0|REP_FAX.cadena.15.0|REP_EMAIL.cadena.50.0|GER_NACIONAL.cadena.1.0|GER_CI.cadena.15.0|GER_NOMBRE.cadena.150.0|GER_DIRECCION.cadena.250.0|GER_TELEF.cadena.15.0|GER_CIUDAD.cadena.15.0|GER_ESTADO.cadena.15.0|GER_CEL.cadena.15.0|GER_EMAIL.cadena.50.0|LOGO.longblob.0.0|REPORTNAME.cadena.1.0|MONEDA.cadena.5.0|MONEDACAMBIO.cadena.5.0|UNIDAD.cadena.3.0|fechatrabajo.fecha.0.0|RECONVERSION_20180604.entero.2.0|fechareconversion.fecha.0.0",
                                   "jsconctaunt|FECHA.fecha.0.0|MONTO.doble.19.6|ID_EMP.cadena.2.0",
                                   "jsconctafor|CODFOR.cadena.2.0|NOMFOR.cadena.50.0|TIPODOC.cadena.2.0|PERIODO.entero.4.0|GIROS.entero.4.0|PERGIROS.entero.4.0|INTERES.doble.6.2|LIMITE.doble.19.2|ID_EMP.cadena.2.0",
                                   "jsconctaics|FECHA.fecha.0.0|tipo.cadena.1.0|monto.doble.6.2",
                                   "jsconctaiva|FECHA.fecha.0.0|TIPO.cadena.1.0|DESDE.doble.19.2|HASTA.doble.19.2|MONTO.doble.6.2|DESDE_1.doble.19.2|HASTA_1.doble.19.2|MONTO_1.doble.6.2|DESDE_2.doble.19.2|HASTA_2.doble.19.2|MONTO_2.doble.6.2",
                                   "jsconctamnu|mnuItem.cadena.10.0|mnuItemParent.cadena.20.0|mnuItemKey.cadena.20.0|mnuItemName.cadena.100.0|mnuItemKeyMask.entero.2.0|mnuItemKeyCode.entero.6.0|mnuItemLevel.entero.2.0|mnuItemStyle.entero.2.0|Gestion.entero.2.0|mnuItemImage.cadena.20.0",
                                   "jsconctatab|CODIGO.cadena.6.0|DESCRIP.cadena.250.0|MODULO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsconctatar|CODTAR.cadena.5.0|NOMTAR.cadena.50.0|COM1.doble.6.2|COM2.doble.6.2|TIPO.cadena.1.0|ID_EMP.cadena.2.0",
                                   "jsconctatra|CODTRA.cadena.5.0|NOMTRA.cadena.50.0|CHOFER.cadena.100.0|ACOMPA.cadena.100.0|CAPACIDAD.doble.10.3|TIPO.cadena.1.0|PUESTOS.entero.2.0|MARCA.cadena.15.0|COLOR.cadena.15.0|PLACAS.cadena.15.0|SERIAL1.cadena.15.0|SERIAL2.cadena.15.0|SERIAL3.cadena.15.0|AUTONO.doble.10.3|TIPOCON.cadena.2.0|CAPTANQUE.doble.10.3|MODELO.cadena.5.0|VALORINI.doble.19.2|CODCON.cadena.30.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0|VALORFIN.doble.19.2|FECHADQ.fecha.0.0",
                                   "jsconctausu|USUARIO.cadena.15.0|PASSWORD.memo.0.0|NOMBRE.cadena.100.0|MAPA.cadena.3.0|ID_USER.cadena.5.0|INI_EMP.cadena.2.0|ini_gestion.entero.2.0|NIVEL.entero.2.0|estatus.entero.2.0|MONEDA.cadena.5.0|DIVISION.cadena.5.0",
                                   "jsconctausuemp|ID_USER.cadena.5.0|ID_EMP.cadena.2.0|PERMITE_EMPRESA.cadena.1.0|EMPRESA_INICIAL.cadena.1.0",
                                   "jsconencmap|MAPA.cadena.3.0|NOMBRE.cadena.100.0|INCLUYE.cadena.1.0|MODIFICA.cadena.1.0|ELIMINA.cadena.1.0",
                                   "jsconnumcon|NUMDOC.cadena.30.0|num_control.cadena.30.0|prov_cli.cadena.30.0|EMISION.fecha.0.0|ORG.cadena.3.0|ORIGEN.cadena.3.0|ID_EMP.cadena.2.0",
                                   "jsconnumcontrol|maquina.cadena.50.0|numerocontrol.cadena.15.0|id_emp.cadena.2.0",
                                   "jsconparametros|gestion.entero.2.0|modulo.entero.2.0|codigo.cadena.15.0|descripcion.cadena.150.0|tipo.entero.2.0|valor.cadena.100.0|id_emp.cadena.2.0",
                                   "jsconregaud|ID_USER.cadena.5.0|MAQUINA.cadena.100.0|FECHA.fechahora.0.0|GESTION.entero.2.0|MODULO.cadena.250.0|TIPOMOV.entero.2.0|NUMDOC.cadena.250.0|EJERCICIO.cadena.5.0|ID_EMP.cadena.2.0",
                                   "jsconrenglonesmapa|mapa.cadena.5.0|region.cadena.50.0|descripcion.cadena.150.0|gestion.entero.2.0|nivel.entero.2.0|modulo.entero.2.0|orden.entero.2.0|acceso.entero.2.0|incluye.entero.2.0|modifica.entero.2.0|elimina.entero.2.0",
                                   "jscontabret|CODRET.cadena.5.0|concepto.cadena.250.0|BASEIMP.doble.19.2|TARIFA.doble.6.2|PAGOMIN.doble.19.2|MENOS.doble.19.2|PERSONA.cadena.1.0|ACUMULA.cadena.1.0|tipo.entero.2.0|comentario.cadena.100.0|CODCON.cadena.20.0|ID_EMP.cadena.2.0",
                                   "jscontrapro|proceso.cadena.2.0|FECHA.fecha.0.0|id_emp.cadena.2.0",
                                   "jsconcatimpfis|codigo.cadena.5.0|tipoimpresora.entero.2.0|maquinafiscal.cadena.30.0|ultima_factura.cadena.15.0|ultima_notacredito.cadena.15.0|ultimo_docnofiscal.cadena.15.0|puerto.cadena.5.0|EN_USO.entero.1.0|ID_EMP.cadena.2.0",
                                   "jsconencconsulta|CONSULTA_ID.cadena.15.0|CONSULTA_NOMBRE.cadena.250.0|USUARIO_ID.cadena.15.0|REPORTE_ID.entero.6.0|ID_EMP.cadena.2.0",
                                   "jsconrenconsulta|CONSULTA_ID.cadena.15.0|OBJETO_ID.cadena.30.0|OBJETO_TEXTO.cadena.250.0|ID_EMP.cadena.2.0",
                                   "jsconcausas_notascredito|CODIGO.cadena.5.0|DESCRIPCION.cadena.50.0|CR.entero.2.0|VALIDA_DOCUMENTOS.entero.2.0|ORIGEN.cadena.3.0|FORMAPAGO.cadena.2.0|NUMPAG.cadena.15.0|NOMPAG.cadena.30.0",
                                   "jsconregistroefectivo|CAJA.cadena.5.0|CAJERO.cadena.5.0|FECHA.fechahora.0.0|NUM_CONTROL_INTERNO.cadena.20.0|NUM_CONTROL_FISCAL.cadena.20.0|MONTO.cadena.10.0|CANTIDAD.entero.10.0|ID_EMP.cadena.2.0",
                                   "jscontablaarchivos|CODIGO.cadena.30.0|RENGLON.cadena.5.0|GESTION_ORIGEN.cadena.3.0|MODULO_ORIGEN.cadena.3.0|ARCHIVO.longblob.0.0|NOMBRE.cadena.50.0|EXTENSION.cadena.5.0|ID_EMP.cadena.2.0"}

        ProcesarTablas(aTablas, Titulo)

        Dim aIndices() As String = {"jsconcontadores|PRIMARY|`gestion`,`modulo`, `codigo`, `id_emp`",
                                    "jsconctacom|PRIMARY|`codigo`,`origen`,`id_emp`",
                                    "jsconctaeje|PRIMARY|`EJERCICIO`,`INICIO`,`CIERRE`,`ID_EMP`",
                                    "jsconctaemp|PRIMARY|`ID_EMP`",
                                    "jsconctaics|PRIMARY|`fecha`,`tipo`",
                                    "jsconctaiva|PRIMARY|`fecha`,`tipo`",
                                    "jsconctamnu|PRIMARY|`mnuItem`",
                                    "jsconctatab|PRIMARY|`codigo`,`modulo`,`ID_EMP`",
                                    "jsconnumcon|PRIMARY|`NUMDOC`,`PROV_CLI`,`EMISION`,`ORG`,`ORIGEN`,`ID_EMP`",
                                    "jsconnumcontrol|PRIMARY|`maquina`,`ID_EMP`",
                                    "jsconparametros|PRIMARY|`gestion`,`modulo`,`descripcion`,`codigo`,`id_emp`",
                                    "jsconregaud|PRIMARY|`ID_USER`,`MAQUINA`,`FECHA`,`GESTION`,`MODULO`,`TIPOMOV`,`NUMDOC`,`ID_EMP`",
                                    "jscontabret|PRIMARY|`CODRET`,`PERSONA`,`ID_EMP`",
                                    "jscontrapro|PRIMARY|`proceso`,`fecha`,`id_emp`",
                                    "jsconcatimpfis|PRIMARY|`codigo`,`id_emp`",
                                    "jsconencconsulta|PRIMARY|`CONSULTA_ID`,`ID_EMP`",
                                    "jsconrenconsulta|PRIMARY|`CONSULTA_ID`,`OBJETO_ID`,`ID_EMP`",
                                    "jsconcausas_notascredito|PRIMARY|`CODIGO`,`ORIGEN`",
                                    "jsconregistroefectivo|PRIMARY|`CAJA`,`CAJERO`, `NUM_CONTROL_INTERNO`,`NUM_CONTROL_FISCAL`,`MONTO`,`ID_EMP`"}

        ProcesarIndices(aIndices, Titulo)

    End Sub
End Class