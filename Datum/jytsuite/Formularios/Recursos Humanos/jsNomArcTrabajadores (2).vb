Imports MySql.Data.MySqlClient
Imports System.IO
Public Class jsNomArcTrabajadores
    Private Const sModulo As String = "Trabajadores"
    Private Const lRegion As String = "RibbonButton32"
    Private Const nTabla As String = "tblTrabajador"
    Private Const nTablaMovimientos As String = "movimientos"
    Private Const nTableTurnos As String = "turnos"
    Private Const nTableExp As String = "tblExpediente"
    Private Const nTableAsi As String = "tblAsistencias"
    Private Const nTablePre As String = "tblPrestamos"
    Private Const nTablaNominas As String = "tblNominas"

    Private strSQL As String = "select * from jsnomcattra where id_emp = '" & jytsistema.WorkID & "' order by codtra "
    Private strSQLMov As String
    Private strSQLTur As String
    Private strsqlExp As String
    Private strSQLAsi As String
    Private strSQLPre As String
    Private strSQLNominas As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtTurnos As New DataTable
    Private dtExpediente As New DataTable
    Private dtAsistencia As New DataTable
    Private dtPres As New DataTable
    Private dtPresR As New DataTable
    Private dtNominas As New DataTable


    Private aCondicion() As String = {"Inactivo", "Activo", "Desincorporado"}
    Private aEdoCivil() As String = {"SOLTERO(A)", "CASADO(A)", "DIVORCIADO(A)", "CONCUBINO(A)", "OTRO(A)"}
    Private aTipovivienda() As String = {"Propia", "Alquilada", "Préstamo", "Otra"}
    Private aSexo() As String = {"Masc.", "Fem."}
    Private aFP() As String = {"EFECTIVO", "CHEQUE", "DEPOSITO", "OTRO"}

    Private i_modo As Integer
    Private CaminoImagen As String = ""

    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionTur As Long, nPosicionExp As Long
    Private nPosicionAsi As Long, nPosicionPre As Long, nPosicionNomina As Long
    Private CredentialCheck As Boolean = True

    Private Sub jsNomArcTrabajadores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarTrabajador(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

            tbcTrabajadores.SelectedTab = C1DockingTabPage1


        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseado")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        'Menu Turnos
        C1SuperTooltip1.SetToolTip(btnAgregaTurno, "<B>Agrega</B> turno")
        C1SuperTooltip1.SetToolTip(btnEliminaTurno, "<B>Elimina</B> turno")

        'Botones Adicionales
        C1SuperTooltip1.SetToolTip(btnFechaNacimiento, "Selecciona la fecha de nacimiento del trabajador...")
        C1SuperTooltip1.SetToolTip(btnIngreso, "Selecciona la fecha de ingreso del trabajador en la empresa...")
        C1SuperTooltip1.SetToolTip(btnBanco, "Selecciona banco para depositar sueldo empleado...")

        C1SuperTooltip1.SetToolTip(btnG6, "Selecciona los subgrupos del trabajador...")
        C1SuperTooltip1.SetToolTip(btnFoto, "Selecciona la foto del trabajador...")
        C1SuperTooltip1.SetToolTip(btnFechaTurno, "Selecciona la fecha a partir de la cual se aplican los turnos...")
        C1SuperTooltip1.SetToolTip(btnFechaDiaLibre, "Selecciona la fecha a partir de la cual será efectivo el día libre...")

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

        If c >= 0 AndAlso dtMovimientos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            dg.Refresh()
            ' dg.CurrentCell = dg(0, c)
        End If
        CalculaTotalesConceptos()
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtMovimientos.Rows.Count)
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarra)

    End Sub

    'Public Sub AsignaConceptosATrabajador(ByVal CodigoTrabajador As String)
    '    Dim nTableConceptos As String = "tblconceptos"
    '    Dim dtConcepto As DataTable
    '    Dim hCont As Integer

    '    ds = DataSetRequery(ds, " select * from jsnomcatcon where  " _
    '                        & " codnom IN (SELECT codnom FROM jsnomrennom " _
    '                        & "             WHERE " _
    '                        & "             codtra = '" & CodigoTrabajador & "' AND " _
    '                        & "             id_emp = '" & jytsistema.WorkID & "') AND " _
    '                        & " estatus = 1 and " _
    '                        & " id_emp = '" & jytsistema.WorkID & "' ", myConn, nTableConceptos, lblInfo)

    '    dtConcepto = ds.Tables(nTableConceptos)

    '    For hCont = 0 To dtConcepto.Rows.Count - 1
    '        With dtConcepto.Rows(hCont)
    '            Conceptos_A_Trabajadores(myConn, lblInfo, ds, .Item("codcon"), IIf(IsDBNull(.Item("conjunto")), "", .Item("conjunto")), .Item("estatus"), _
    '                            IIf(IsDBNull(.Item("formula")), "", .Item("formula")), IIf(IsDBNull(.Item("condicion")), "", .Item("condicion")), CodigoTrabajador, CodigoTrabajador)
    '        End With
    '    Next
    '    dtConcepto.Dispose()
    '    dtConcepto = Nothing

    'End Sub


    Private Sub AsignaExp(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strsqlExp, myConn, nTableExp, lblInfo)

        If c >= 0 AndAlso dtExpediente.Rows.Count > 0 Then
            Me.BindingContext(ds, nTableExp).Position = c
            dgExpediente.Refresh()
            dgExpediente.CurrentCell = dgExpediente(0, c)
        End If
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtExpediente.Rows.Count)
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtExpediente, MenuBarra)

    End Sub
    Private Sub AsignaAsi(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLAsi, myConn, nTableAsi, lblInfo)
        dtAsistencia = ds.Tables(nTableAsi)
        If c >= 0 AndAlso dtAsistencia.Rows.Count > 0 Then
            Me.BindingContext(ds, nTableAsi).Position = c
            dgAsistencias.Refresh()
            dgAsistencias.CurrentCell = dgAsistencias(0, c)

        End If
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtAsistencia.Rows.Count)
        CalculaTotalesAsistencia(dtAsistencia)
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtAsistencia, MenuBarra)

    End Sub
    Private Sub AsignaPre(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLPre, myConn, nTablePre, lblInfo)

        If c >= 0 AndAlso dtPres.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablePre).Position = c
            dgPrestamos.Refresh()
            dgPrestamos.CurrentCell = dgPrestamos(0, c)
        End If
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtPres.Rows.Count)
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtPres, MenuBarra)

    End Sub
    Private Sub AsignaTur(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLTur, myConn, nTableTurnos, lblInfo)

        If c >= 0 AndAlso dtTurnos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTableTurnos).Position = c
            dgTurnos.Refresh()
            dgTurnos.CurrentCell = dgTurnos(0, c)
        End If

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)


        With dt

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)
            If .Rows.Count > 0 Then

                With .Rows(nRow)

                    'Trabajador 
                    txtCodigo.Text = MuestraCampoTexto(.Item("codtra"))
                    txtNacionalidad.Text = MuestraCampoTexto(.Item("nacional"))
                    txtID.Text = MuestraCampoTexto(.Item("cedula"))
                    txtBiometrico.Text = MuestraCampoTexto(.Item("codhp"))

                    RellenaCombo(aCondicion, cmbCondicion, CInt(.Item("condicion").ToString))
                    txtNombre.Text = MuestraCampoTexto(.Item("nombres"))
                    txtApellido.Text = MuestraCampoTexto(.Item("apellidos"))
                    txtDireccion.Text = MuestraCampoTexto(.Item("direccion"))
                    txtTelef1.Text = MuestraCampoTexto(.Item("telef1"))
                    txtTelef2.Text = MuestraCampoTexto(.Item("telef2"))
                    txtemail.Text = MuestraCampoTexto(.Item("email"))

                    txtFechaNacimiento.Text = MuestraCampoTexto(FormatoFecha(.Item("fnacimiento").ToString))

                    txtCiudad.Text = MuestraCampoTexto(.Item("lugarnac"))
                    txtEstado.Text = MuestraCampoTexto(.Item("edonac"))
                    txtPais.Text = MuestraCampoTexto(.Item("pais"))
                    RellenaCombo(aEdoCivil, cmbEdoCivil, .Item("edocivil"))
                    txtAscendentes.Text = FormatoEntero(CInt(.Item("ascendentes").ToString))
                    txtDescendentes.Text = FormatoEntero(CInt(.Item("descendentes").ToString))
                    RellenaCombo(aTipovivienda, cmbTipoVivienda, CInt(.Item("vivienda").ToString))
                    txtSSO.Text = .Item("nosso")
                    txtVehiculos.Text = FormatoEntero(CInt(.Item("vehiculos").ToString))
                    RellenaCombo(aSexo, cmbSexo, CInt(.Item("sexo").ToString))
                    txtProfesion.Text = MuestraCampoTexto(.Item("profesion"))
                    txtIngreso.Text = MuestraCampoTexto(FormatoFecha(.Item("ingreso").ToString))

                    Dim jCam() As String = {"codtra", "causa", "id_emp"}
                    Dim jstr() As String = {txtCodigo.Text, 0, jytsistema.WorkID}
                    If Not qFound(myConn, lblInfo, "jsnomexptra", jCam, jstr) Then
                        EjecutarSTRSQL(myConn, lblInfo, " insert into jsnomexptra set codtra = '" & txtCodigo.Text & "', fecha = '" & FormatoFechaHoraMySQL(CDate(txtIngreso.Text)) & "', comentario = 'FECHA INGRESO A LA EMPRESA', causa = 0, id_emp = '" & jytsistema.WorkID & "'    ")
                    End If
                    RellenaCombo(aFP, cmbFormapago, CInt(.Item("formapago").ToString))
                    txtBanco.Text = MuestraCampoTexto(.Item("banco"))
                    txtCtaBanco.Text = MuestraCampoTexto(.Item("ctaban"))
                    txtBanco1.Text = MuestraCampoTexto(.Item("banco_1"))
                    txtNombreBanco1.Text = MuestraCampoTexto(.Item("ctaban_1"))

                    txtG1.Text = MuestraCampoTexto(.Item("subnivel1"))
                    txtG2.Text = MuestraCampoTexto(.Item("subnivel2"))
                    txtG3.Text = MuestraCampoTexto(.Item("subnivel3"))
                    txtG4.Text = MuestraCampoTexto(.Item("subnivel4"))
                    txtG5.Text = MuestraCampoTexto(.Item("subnivel5"))
                    txtG6.Text = MuestraCampoTexto(.Item("subnivel6"))

                    txtCodigoCargo.Text = MuestraCampoTexto(.Item("cargo"))
                    txtSueldo.Text = MuestraCampoTexto(FormatoNumero(.Item("sueldo")))
                    txtISLR.Text = MuestraCampoTexto(FormatoNumero(.Item("RETISLR")))

                    'RellenaCombo(aTipoNomina, cmbTipoNomina, CInt(.Item("tiponom").ToString))


                    CaminoImagen = BaseDatosAImagen(dt.Rows(nRow), "foto", .Item("codtra"))
                    'CaminoImagen = My.Computer.FileSystem.CurrentDirectory & "\" & "foto" & .Item("codtra") & ".jpg"
                    If My.Computer.FileSystem.FileExists(CaminoImagen) Then
                        Dim fs As System.IO.FileStream
                        fs = New System.IO.FileStream(CaminoImagen, IO.FileMode.Open, IO.FileAccess.Read)
                        pctFoto.Image = System.Drawing.Image.FromStream(fs)
                        fs.Close()
                    Else
                        pctFoto.Image = Nothing
                    End If

                    txtFechaTurno.Text = MuestraCampoTexto(FormatoFecha(.Item("turnodesde").ToString))
                    txtDiasLibres.Text = FormatoEntero(CInt(.Item("freedays").ToString))
                    RellenaCombo(aTipoNomina, cmbPeriodoDiaLibre, CInt(.Item("periodo").ToString))

                    txtFechaDiaLibre.Text = MuestraCampoTexto(FormatoFecha(.Item("datefreeday").ToString))
                    chkRotacion.Checked = .Item("rotatorio")

                    'NOMINAS
                    strSQLNominas = " select a.codnom, a.descripcion from jsnomencnom a " _
                        & " left join jsnomrennom b on (a.codnom = b.codnom and a.id_emp = b.id_emp) " _
                        & " where b.codtra = '" & .Item("codtra") & "' and " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' order by 1 "

                    ds = DataSetRequery(ds, strSQLNominas, myConn, nTablaNominas, lblInfo)
                    dtNominas = ds.Tables(nTablaNominas)

                    Dim aCamCI() As String = {"codnom", "descripcion"}
                    Dim aNomCI() As String = {"Código", "Descripción"}
                    Dim aAncCI() As Long = {70, 200}
                    Dim aAliCI() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
                    Dim aForCI() As String = {"", ""}

                    IniciarTabla(dgNominas, dtNominas, aCamCI, aNomCI, aAncCI, aAliCI, aForCI, False, , 8, False)
                    If dtNominas.Rows.Count > 0 Then
                        nPosicionNomina = 0
                        RellenaComboConDatatable(cmbNominas, dtNominas, "DESCRIPCION", "CODNOM")
                    End If

                    'Turnos
                    strSQLTur = " select a.codtur, b.nombre " _
                                            & " from jsnomturtra a " _
                                            & " left join jsnomcattur b on (a.codtur = b.codtur and a.id_emp = b.id_emp) " _
                                            & " where " _
                                            & " a.codtra = '" & .Item("codtra") & "' and " _
                                            & " a.id_emp = '" & jytsistema.WorkID & "' "

                    ds = DataSetRequery(ds, strSQLTur, myConn, nTableTurnos, lblInfo)
                    dtTurnos = ds.Tables(nTableTurnos)
                    Dim aCamTur() As String = {"codtur", "nombre"}
                    Dim aNomTur() As String = {"Código", "Nombre"}
                    Dim aAncTur() As Long = {75, 200}
                    Dim aAliTur() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
                    Dim aForTur() As String = {"", ""}

                    IniciarTabla(dgTurnos, dtTurnos, aCamTur, aNomTur, aAncTur, aAliTur, aForTur)
                    If dtTurnos.Rows.Count > 0 Then nPosicionTur = 0


                    If cmbCondicion.SelectedIndex <> 1 Then EliminarAsignacionesYDeducciones(myConn, lblInfo, txtCodigo.Text)

                    'Movimientos
                    'strSQLMov = " select a.codcon, b.nomcon, b.tipo, a.importe,  " _
                    '        & " if(b.tipo = 0, a.importe, 0.00) asignaciones,  " _
                    '        & " if(b.tipo = 1, a.importe, 0.00) deducciones, " _
                    '        & " if(b.tipo = 2, a.importe, 0.00) adicionales,  " _
                    '        & " if(b.tipo = 3, a.importe, 0.00) especiales, porcentaje_asig, '' control  " _
                    '        & " from jsnomtrades a " _
                    '        & " left join jsnomcatcon b on (a.codcon = b.codcon and a.id_emp = b.id_emp) " _
                    '        & " where " _
                    '        & " a.importe > 0 and " _
                    '        & " a.codtra = '" & txtCodigo.Text & "' and  " _
                    '        & " a.id_emp = '" & jytsistema.WorkID & "' order by b.tipo, a.codcon "

                    'ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                    'dtMovimientos = ds.Tables(nTablaMovimientos)
                    'Dim aCampos() As String = {"codcon", "nomcon", "asignaciones", "deducciones", "adicionales", "especiales", "porcentaje_asig", "control"}
                    'Dim aNombres() As String = {"Código", "Descripción", "Asignaciones", "Deducciones", "Adicionales", "Especiales", "%/Asignación", ""}
                    'Dim aAnchos() As Long = {70, 350, 90, 90, 90, 90, 90, 90}
                    'Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                    '    AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                    '    AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
                    'Dim aFormatos() As String = {"", "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}
                    'IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
                    'If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0
                    'CalculaTotalesConceptos()

                    'Expediente 
                    strsqlExp = " select * from jsnomexptra where codtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "
                    ds = DataSetRequery(ds, strsqlExp, myConn, nTableExp, lblInfo)
                    dtExpediente = ds.Tables(nTableExp)
                    Dim aCamExp() As String = {"fecha", "comentario", "causa", ""}
                    Dim aNomExp() As String = {"Fecha", "Comentario", "Causa", ""}
                    Dim aAncExp() As Long = {120, 350, 350, 100}
                    Dim aAliExp() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
                    Dim aForExp() As String = {sFormatoFecha, "", "", ""}
                    IniciarTabla(dgExpediente, dtExpediente, aCamExp, aNomExp, aAncExp, aAliExp, aForExp)
                    If dtExpediente.Rows.Count > 0 Then nPosicionExp = 0

                    'Asistencias
                    Dim FechaInicial As Date, FechaFinal As Date
                    Select Case CInt(.Item("tiponom").ToString)
                        Case 0 'Diaria
                            FechaInicial = jytsistema.sFechadeTrabajo
                            FechaFinal = jytsistema.sFechadeTrabajo
                        Case 1 'Semanal
                            Dim DiaPrimeroSemana As Integer = CInt(ParametroPlus(MyConn, Gestion.iRecursosHumanos, "NOMPARAM01")) + 1
                            FechaInicial = PrimerDiaSemana(jytsistema.sFechadeTrabajo, DiaPrimeroSemana)
                            FechaFinal = UltimoDiaSemana(jytsistema.sFechadeTrabajo)
                        Case 2 'Quincenal
                            FechaInicial = PrimerDiaQuincena(jytsistema.sFechadeTrabajo)
                            FechaFinal = UltimoDiaQuincena(jytsistema.sFechadeTrabajo)
                        Case 3 'Mensual
                            FechaInicial = PrimerDiaMes(jytsistema.sFechadeTrabajo)
                            FechaFinal = UltimoDiaMes(jytsistema.sFechadeTrabajo)
                        Case Else  'Anual Y OTRAS 
                            FechaInicial = PrimerDiaAño(jytsistema.sFechadeTrabajo)
                            FechaFinal = UltimoDiaAño(jytsistema.sFechadeTrabajo)
                    End Select

                    strSQLAsi = " select a.codtra, a.codhp, a.dia, a.entrada, a.salida, a.descanso, a.retorno, time_format(a.horas,'%H:%i') horas, a.tipo, '' control " _
                        & " from jsnomtratur a " _
                        & " where " _
                        & " a.codtra = '" & txtCodigo.Text & "' and " _
                        & " a.dia >= '" & FormatoFechaMySQL(FechaInicial) & "' and  " _
                        & " a.dia <= '" & FormatoFechaMySQL(FechaFinal) & "' and  " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' order by a.dia desc, a.tipo "

                    ds = DataSetRequery(ds, strSQLAsi, myConn, nTableAsi, lblInfo)
                    dtAsistencia = ds.Tables(nTableAsi)

                    Dim aCamAsi() As String = {"dia", "entrada", "descanso", "retorno", "salida", "horas", "tipo", "control"}
                    Dim aNomAsi() As String = {"Día", "Entrada", "Descanso", "Retorno", "Salida", "Total Horas", "Tipo", ""}
                    Dim aAncAsi() As Long = {80, 120, 120, 120, 120, 70, 200, 100}
                    Dim aAliAsi() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                        AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
                    Dim aForAsi() As String = {sFormatoFechaCorta, sFormatoHoraCorta, sFormatoHoraCorta, sFormatoHoraCorta, "", "", "", ""}
                    IniciarTabla(dgAsistencias, dtAsistencia, aCamAsi, aNomAsi, aAncAsi, aAliAsi, aForAsi)
                    If dtAsistencia.Rows.Count > 0 Then nPosicionAsi = 0
                    CalculaTotalesAsistencia(dtAsistencia)

                    'PRESTAMOS
                    'Encabezado
                    strSQLPre = " select * from jsnomencpre a " _
                        & " where " _
                        & " a.codtra = '" & txtCodigo.Text & "' and " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codpre "

                    ds = DataSetRequery(ds, strSQLPre, myConn, nTablePre, lblInfo)
                    dtPres = ds.Tables(nTablePre)

                    Dim aCamPRE() As String = {"codpre", "descrip", "fechaprestamo", "montotal", "fechainicio", "saldo", "control"}
                    Dim aNomPRE() As String = {"Código", "Descripción", "Fecha Aprobación", "Monto", "Fecha Inicio Descuentos", "Saldo", ""}
                    Dim aAncPRE() As Long = {100, 250, 100, 100, 100, 100, 100}
                    Dim aAliPRE() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}

                    Dim aForPRE() As String = {"", "", sFormatoFecha, sFormatoNumero, sFormatoFecha, sFormatoNumero, ""}
                    IniciarTabla(dgPrestamos, dtPres, aCamPRE, aNomPRE, aAncPRE, aAliPRE, aForPRE)
                    If dtPres.Rows.Count > 0 Then nPosicionPre = 0


                End With
            End If
        End With

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub VerMovimientosEmpleado(CodigoNomina As String)

        strSQLMov = " select a.codcon, b.nomcon, b.tipo, a.importe,  " _
                            & " if(b.tipo = 0, a.importe, 0.00) asignaciones,  " _
                            & " if(b.tipo = 1, a.importe, 0.00) deducciones, " _
                            & " if(b.tipo = 2, a.importe, 0.00) adicionales,  " _
                            & " if(b.tipo = 3, a.importe, 0.00) especiales, porcentaje_asig, '' control  " _
                            & " from jsnomtrades a " _
                            & " left join jsnomcatcon b on ( a.codcon = b.codcon and a.id_emp = b.id_emp AND a.codnom = b.codnom ) " _
                            & " where " _
                            & " a.importe > 0 and " _
                            & " a.codnom = '" & CodigoNomina & "' AND " _
                            & " a.codtra = '" & txtCodigo.Text & "' and  " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' order by b.tipo, a.codcon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        Dim aCampos() As String = {"codcon", "nomcon", "asignaciones", "deducciones", "adicionales", "especiales", "porcentaje_asig", "control"}
        Dim aNombres() As String = {"Código", "Descripción", "Asignaciones", "Deducciones", "Adicionales", "Especiales", "%/Asignación", ""}
        Dim aAnchos() As Long = {70, 350, 90, 90, 90, 90, 90, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
            AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
        Dim aFormatos() As String = {"", "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        CalculaTotalesConceptos()

    End Sub

    Private Sub CalculaTotalesConceptos()
        Dim lCont As Integer
        Dim dAsi As Double = 0.0, dDed As Double = 0.0, dAdi As Double = 0.0, dEsp As Double = 0.0
        For lCont = 0 To dtMovimientos.Rows.Count - 1
            dAsi += CDbl(dtMovimientos.Rows(lCont).Item("asignaciones").ToString)
            dDed += CDbl(dtMovimientos.Rows(lCont).Item("deducciones").ToString)
            dAdi += CDbl(dtMovimientos.Rows(lCont).Item("adicionales").ToString)
            dEsp += CDbl(dtMovimientos.Rows(lCont).Item("especiales").ToString)
        Next

        txtAsignaciones.Text = FormatoNumero(dAsi)
        txtDeducciones.Text = FormatoNumero(dDed)
        txtAdicionales.Text = FormatoNumero(dAdi)
        txtEspeciales.Text = FormatoNumero(dEsp)
        txtTotal.Text = FormatoNumero(dAsi - dDed)

    End Sub

    Private Sub CalculaTotalesAsistencia(ByVal dtAsistencia As DataTable)
        Dim lCont As Integer
        Dim dHoras As Integer = 0.0, dMinutos As Integer
        Dim dCantidadJustificado As Integer = 0, dCantidadInjustificado As Integer = 0
        For lCont = 0 To dtAsistencia.Rows.Count - 1
            With dtAsistencia.Rows(lCont)
                Select Case .Item("tipo")
                    Case 0
                        If .Item("horas") <> "00:00" Then
                            dHoras += CInt(If(.Item("horas") <> "", Split(.Item("horas"), ":")(0), "0"))
                            dMinutos += CInt(If(.Item("horas") <> "", Split(.Item("horas"), ":")(1), "0"))
                            dCantidadJustificado += 1
                        Else
                            dCantidadInjustificado += 1
                        End If
                End Select

            End With
        Next

        txtHorasAsistencia_Just.Text = TotalHoras(dHoras, dMinutos)
        txtAsistencias_Just.Text = dCantidadJustificado

        txtInasistencias_Injust.Text = dCantidadInjustificado



    End Sub
    Private Function TotalHoras(ByVal Horas As Integer, ByVal Minutos As Integer) As String

        If Minutos > 59 Then
            Horas = Horas + Fix(Minutos / 60)
            Minutos = Minutos Mod 60
        End If
        TotalHoras = Format(horas, "00") + ":" + Format(Minutos, "00")

    End Function

    Private Sub IniciarTrabajador(ByVal Inicio As Boolean)

        If Inicio Then
            Dim aWhereFields() As String = {"id_emp"}
            Dim aWhereValues() As String = {jytsistema.WorkID}
            txtCodigo.Text = AutoCodigoXPlus(myConn, "codtra", "jsnomcattra", aWhereFields, aWhereValues, 10, True)    ' AutoCodigo(10, ds, nTabla, "codtra")
        Else
            txtCodigo.Text = ""
        End If
        txtNacionalidad.Text = "V"
        IniciarTextoObjetos(FormatoItemListView.iCadena, txtID, txtBiometrico, txtNombre, txtApellido, txtDireccion, _
                            txtTelef1, txtTelef2, txtemail, txtCiudad, txtEstado, txtPais, txtCtaBanco, _
                            txtNombreBanco1, txtBanco, txtBanco1, txtG1, txtG2, txtG3, txtG4, txtG5, txtG6, _
                            txtDescripcionCargo, txtProfesion, txtSSO)
        RellenaCombo(aCondicion, cmbCondicion)
        txtFechaNacimiento.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        RellenaCombo(aEdoCivil, cmbEdoCivil)
        txtAscendentes.Text = FormatoEntero(0)
        txtDescendentes.Text = FormatoEntero(0)
        RellenaCombo(aTipovivienda, cmbTipoVivienda)
        txtVehiculos.Text = FormatoEntero(0)
        RellenaCombo(aSexo, cmbSexo)
        txtIngreso.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        RellenaCombo(aFP, cmbFormapago)


        txtSueldo.Text = FormatoNumero(0.0)
        txtISLR.Text = FormatoNumero(0.0)

        RellenaCombo(aTipoNomina, cmbTipoNomina)

        pctFoto.Image = Nothing
        txtFechaTurno.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtDiasLibres.Text = FormatoEntero(0)

        RellenaCombo(aTipoNomina, cmbPeriodoDiaLibre)

        txtFechaDiaLibre.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        chkRotacion.Checked = False

        dg.Columns.Clear()
        dgTurnos.Columns.Clear()
        dgNominas.Columns.Clear()



    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True
        HabilitarObjetos(False, True, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5)

        HabilitarObjetos(True, True, txtNacionalidad, txtID, txtBiometrico, cmbCondicion, txtNombre, txtApellido, _
                         txtDireccion, txtTelef1, txtTelef2, txtemail, btnFechaNacimiento, txtCiudad, txtEstado, txtPais, _
                         cmbEdoCivil, txtAscendentes, txtDescendentes, cmbTipoVivienda, txtSSO, txtVehiculos, cmbSexo, _
                         txtProfesion, btnIngreso, cmbFormapago, txtCtaBanco, txtCtaBanco1, _
                         btnG1, btnG2, btnG3, btnG4, btnG5, btnG6, btnCargos, txtSueldo, txtISLR, cmbTipoNomina, btnFoto, _
                         btnFechaTurno, txtDiasLibres, cmbPeriodoDiaLibre, btnFechaDiaLibre, chkRotacion)
        MenuTurnos.Enabled = True

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False

        HabilitarObjetos(True, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5)

        HabilitarObjetos(False, True, txtCodigo, txtNacionalidad, txtID, txtBiometrico, cmbCondicion, txtNombre, txtApellido, _
                         txtDireccion, txtTelef1, txtTelef2, txtemail, txtFechaNacimiento, btnFechaNacimiento, txtCiudad, txtEstado, txtPais, _
                         txtEdad, cmbEdoCivil, txtAscendentes, txtDescendentes, cmbTipoVivienda, txtSSO, txtVehiculos, cmbSexo, _
                         txtProfesion, txtIngreso, btnIngreso, txtAntiguedad, cmbFormapago, txtBanco, txtBanco1, btnBanco, btnBanco1, _
                         txtNombreBanco, txtNombreBanco1, txtCtaBanco, txtCtaBanco1, txtG1, txtG2, txtG3, txtG4, txtG5, txtG6, _
                         btnG1, btnG2, btnG3, btnG4, btnG5, btnG6, txtDescripcionCargo, btnCargos, txtSueldo, cmbTipoNomina, btnFoto, _
                         btnFechaTurno, txtFechaTurno, txtDiasLibres, cmbPeriodoDiaLibre, btnFechaDiaLibre, txtFechaDiaLibre, chkRotacion, txtISLR)

        MenuTurnos.Enabled = False

        HabilitarObjetos(True, False, dgPrestamos, dgExpediente, dg, dgAsistencias)

        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub

    Private Sub btnFechaNacimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaNacimiento.Click, _
        btnIngreso.Click
        Dim nombreTXT As String = "txt" + Mid(sender.name, 4, Len(sender.name))
        Dim myTXT As TextBox = DirectCast(C1DockingTabPage1.Controls(nombreTXT), TextBox)
        myTXT.Text = SeleccionaFecha(CDate(myTXT.Text), Me, sender)

    End Sub
    Private Sub btnFechadiaLibre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        btnFechaDiaLibre.Click, btnFechaTurno.Click

        Dim nombreTXT As String = "txt" + Mid(sender.name, 4, Len(sender.name))
        Dim myTXT As TextBox = DirectCast(grpTurno.Controls(nombreTXT), TextBox)
        myTXT.Text = SeleccionaFecha(CDate(myTXT.Text), Me, sender)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                nPosicionCat = 0
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
            Case "Conceptos Regulares"
                nPosicionMov = 0
                Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select

    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                Me.BindingContext(ds, nTabla).Position -= 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(nPosicionCat)
            Case "Conceptos Regulares"
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                Me.BindingContext(ds, nTabla).Position += 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(nPosicionCat)
            Case "Conceptos Regulares"
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(nPosicionCat)
            Case "Conceptos Regulares"
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                i_modo = movimiento.iAgregar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                IniciarTrabajador(True)
            Case "Conceptos Regulares"
            Case "Expediente"
                If txtCodigo.Text <> "" Then
                    Dim f As New jsNomArcTrabajadoresMovimientosExpediente
                    f.Apuntador = Me.BindingContext(ds, nTableExp).Position
                    f.Agregar(myConn, ds, dtExpediente, txtCodigo.Text)
                    AsignaExp(f.Apuntador, True)
                    f = Nothing
                End If
            Case "Control de Asistencias"
                If txtCodigo.Text <> "" Then
                    Dim g As New jsNomArcTrabajadoresMovimientosAsistencia
                    g.Apuntador = Me.BindingContext(ds, nTableAsi).Position
                    g.Agregar(myConn, ds, dtAsistencia, txtCodigo.Text)
                    AsignaAsi(g.Apuntador, True)
                End If
            Case "Cuotas/Préstamos"
                If txtCodigo.Text <> "" Then
                    Dim f As New jsNomArcTrabajadoresCuotas
                    f.Apuntador = Me.BindingContext(ds, nTablePre).Position
                    f.Agregar(myConn, ds, dtPres, txtCodigo.Text)
                    AsignaPre(f.Apuntador, True)
                End If
        End Select

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                i_modo = movimiento.iEditar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
            Case "Conceptos Regulares"
                'Dim f As New jsBanArcBancosMovimientos
                'f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
                'f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
                'ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblinfo)
                'ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                'AsignaTXT(nPosicionCat)
                'AsignaMov(f.Apuntador, True)
                'f = Nothing
            Case "Expediente"
                Dim f As New jsNomArcTrabajadoresMovimientosExpediente
                f.Apuntador = Me.BindingContext(ds, nTableExp).Position
                f.Editar(myConn, ds, dtExpediente, txtCodigo.Text)
                AsignaExp(f.Apuntador, True)
                f = Nothing
            Case "Control de Asistencias"
                Dim g As New jsNomArcTrabajadoresMovimientosAsistencia
                g.Apuntador = Me.BindingContext(ds, nTableAsi).Position
                g.Editar(myConn, ds, dtAsistencia, txtCodigo.Text)
                AsignaAsi(g.Apuntador, True)
            Case "Cuotas/Préstamos"
                If txtCodigo.Text <> "" Then
                    Dim f As New jsNomArcTrabajadoresCuotas
                    f.Apuntador = Me.BindingContext(ds, nTablePre).Position
                    f.Editar(myConn, ds, dtPres, txtCodigo.Text)
                    AsignaPre(f.Apuntador, True)
                End If

        End Select

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                EliminaTrabajador()
            Case "Conceptos Regulares"
                If cmbCondicion.Text = "Activo" Or cmbCondicion.Text = "Desincorporado" Then
                    EliminaConcepto()
                Else
                    MensajeAdvertencia(lblInfo, "Este trabajador está Inactivo(a) ....")
                End If

            Case "Expediente"
                EliminarExpediente()
        End Select
    End Sub
    Private Sub EliminarExpediente()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim afld() As String = {"codtra", "Fecha", "causa", "id_emp"}
        With dtExpediente.Rows(nPosicionExp)
            Dim aStr() As String = {txtCodigo.Text, FormatoFechaMySQL(CDate(.Item("fecha").ToString)), .Item("causa"), jytsistema.WorkID}

            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                Me.BindingContext(ds, nTableExp).Position = EliminarRegistros(myConn, lblInfo, ds, nTableExp, "jsnomexptra", strsqlExp, _
                                                                                      afld, aStr, Me.BindingContext(ds, nTableExp).Position, True)
                nPosicionExp = Me.BindingContext(ds, nTableExp).Position
                AsignaExp(nPosicionExp, True)
            End If

        End With
    End Sub
    Private Sub EliminaConcepto()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aFldEli() As String = {"codtra", "codcon", "id_emp"}
        Dim aStrEli() As String = {txtCodigo.Text, dtMovimientos.Rows(nPosicionMov).Item("codcon"), jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")

        If sRespuesta = MsgBoxResult.Yes Then
            Me.BindingContext(ds, nTablaMovimientos).Position = EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jsnomtrades", strSQLMov, _
                                                                                  aFldEli, aStrEli, Me.BindingContext(ds, nTablaMovimientos).Position, True)
            nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
            AsignaMov(nPosicionMov, True)
        End If
    End Sub
    Private Sub EliminaTrabajador()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"codtra", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")

        If sRespuesta = MsgBoxResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then

                Me.BindingContext(ds, nTabla).Position = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsnomcattra", strSQL, aCamposDel, aStringsDel, _
                                                              Me.BindingContext(ds, nTabla).Position, True)
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(nPosicionCat)
            Else
                MensajeAdvertencia(lblInfo, "Este trabajador posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                Dim Campos() As String = {"codtra", "apellidos", "nombres"}
                Dim Nombres() As String = {"Código", "Apellidos", "Nombres"}
                Dim Anchos() As Long = {100, 200, 200}
                f.Text = "Trabajadores"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Trabajadores...")
                Me.BindingContext(ds, nTabla).Position = f.Apuntador
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(nPosicionCat)
                f = Nothing
            Case "Conceptos"
        End Select
    End Sub


    Private Sub btnColumnas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                Dim f As New jsNomRepParametros
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cFichaTrabajador, "Ficha de Trabajador", txtCodigo.Text)
                f = Nothing
            Case "Conceptos"
            Case "Expediente"
            Case "Control de Asistencias"
            Case "Cuotas/Préstamos"

        End Select

    End Sub
    Private Function Validado() As Boolean

        If i_modo = movimiento.iAgregar Then

            Dim aFld() As String = {"codtra", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

            If qFound(myConn, lblInfo, "jsnomcattra", aFld, aStr) Then
                MensajeAdvertencia(lblInfo, "Código trabajador YA existe. Por favor verifique...")
                Return False
            End If

            If txtID.Text <> "" Then
                Dim aFldI() As String = {"cedula", "id_emp"}
                Dim aStrI() As String = {txtID.Text, jytsistema.WorkID}
                If qFound(myConn, lblInfo, "jsnomcattra", aFldI, aStrI) Then
                    MensajeAdvertencia(lblInfo, "Cédula de identidad YA existe. Por favor verifique...")
                    Return False
                End If
            End If
            If txtBiometrico.Text <> "" Then
                Dim aFldB() As String = {"codhp", "id_emp"}
                Dim aStrB() As String = {txtBiometrico.Text, jytsistema.WorkID}
                If qFound(myConn, lblInfo, "jsnomcattra", aFldB, aStrB) Then
                    MensajeAdvertencia(lblInfo, "Código Biométrico YA existe. Por favor verifique...")
                    Return False
                End If
            End If

        End If

        If cmbFormapago.SelectedIndex = 2 AndAlso txtBanco.Text <> "" _
            AndAlso txtCtaBanco.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un número de CUENTA BANCARIA 1 válida. Por favor verifique...")
            Return False
        End If

        If cmbFormapago.SelectedIndex = 2 AndAlso txtBanco1.Text <> "" _
            AndAlso txtCtaBanco1.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un número de CUENTA BANCARIA 2 válida. Por favor verifique...")
            Return False
        End If

        Validado = True
    End Function
    Private Sub GuardarTXT()
        Dim MyData() As Byte
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionCat = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditNOMINATrabajador(myConn, lblInfo, Inserta, txtCodigo.Text, txtBiometrico.Text, txtApellido.Text, txtNombre.Text, txtProfesion.Text, CDate(txtIngreso.Text), _
             CDate(txtFechaNacimiento.Text), txtCiudad.Text, txtPais.Text, "0", txtNacionalidad.Text, "", txtID.Text, cmbEdoCivil.SelectedIndex, _
             cmbSexo.SelectedIndex, ValorEntero(txtAscendentes.Text), ValorEntero(txtDescendentes.Text), txtSSO.Text, "0", cmbTipoVivienda.SelectedIndex, _
             ValorEntero(txtVehiculos.Text), txtDireccion.Text, txtTelef1.Text, txtTelef2.Text, txtemail.Text, cmbCondicion.SelectedIndex, _
             cmbTipoNomina.SelectedIndex, cmbFormapago.SelectedIndex, txtBanco.Text, txtCtaBanco.Text, txtBanco1.Text, txtNombreBanco1.Text, "", "", "", "", "", _
             "", CDate(txtIngreso.Text), "", "", "", txtG1.Text, txtG2.Text, _
             txtG3.Text, txtG4.Text, txtG5.Text, txtG6.Text, txtCodigoCargo.Text, CDate(txtFechaTurno.Text), _
             ValorNumero(txtSueldo.Text), ValorNumero(txtISLR.Text), ValorEntero(txtDiasLibres.Text), cmbPeriodoDiaLibre.SelectedIndex, CDate(txtFechaDiaLibre.Text), _
             chkRotacion.Checked, CDate(txtIngreso.Text))

        If Not pctFoto.Image Is Nothing Then
            If CaminoImagen <> "" Then
                Dim fs As New FileStream(CaminoImagen, FileMode.OpenOrCreate, FileAccess.Read)
                MyData = New Byte(fs.Length) {}
                fs.Read(MyData, 0, fs.Length)
                fs.Close()
                GuardarFotoTrabajador(myConn, lblInfo, txtCodigo.Text, MyData)
            End If
        End If

        If cmbCondicion.SelectedIndex <> 1 Then EliminarAsignacionesYDeducciones(myConn, lblInfo, txtCodigo.Text)

        AsignaConceptosATrabajador(myConn, lblInfo, ds, txtCodigo.Text)
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODTRA = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcTrabajadores.SelectedTab = C1DockingTabPage1
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, txtNacionalidad.GotFocus, _
        txtID.GotFocus, txtBiometrico.GotFocus, cmbCondicion.GotFocus, txtNombre.GotFocus, txtApellido.GotFocus, _
        txtDireccion.GotFocus, txtTelef1.GotFocus, txtTelef2.GotFocus, txtemail.GotFocus, txtFechaNacimiento.GotFocus, _
        btnFechaNacimiento.GotFocus, txtCiudad.GotFocus, txtEstado.GotFocus, txtPais.GotFocus, cmbEdoCivil.GotFocus, _
        txtAscendentes.GotFocus, txtDescendentes.GotFocus, cmbTipoVivienda.GotFocus, txtSSO.GotFocus, _
        txtVehiculos.GotFocus, cmbSexo.GotFocus, txtProfesion.GotFocus, btnIngreso.GotFocus, cmbFormapago.GotFocus, _
        btnBanco.GotFocus, txtCtaBanco.GotFocus, txtSueldo.GotFocus, cmbTipoNomina.GotFocus, btnFoto.GotFocus, _
        btnFechaTurno.GotFocus, txtDiasLibres.GotFocus, btnFechaDiaLibre.GotFocus, cmbPeriodoDiaLibre.GotFocus, _
         btnG6.GotFocus, btnCargos.GotFocus, txtNombre.MouseHover


        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, " Indique el código del trabajador ... ", TipoMensaje.iInfo)
            Case "txtNacionalidad"
                MensajeEtiqueta(lblInfo, " Indique nacionalidad (V,E,P) del trabajador ... ", TipoMensaje.iInfo)
            Case "txtID"
                MensajeEtiqueta(lblInfo, " Indique el número de cédula de identidad trabajador ... ", TipoMensaje.iInfo)
            Case "txtBiometrico"
                MensajeEtiqueta(lblInfo, " Indique el código de identificación biométrica del trabajador ... ", TipoMensaje.iInfo)
            Case "cmbCondicion"
                MensajeEtiqueta(lblInfo, " Seleccione la condición o estatus del trabajador ... ", TipoMensaje.iInfo)
            Case "txtNombre"
                MensajeEtiqueta(lblInfo, " Indique el nombre del trabajador ... ", TipoMensaje.iInfo)
            Case "txtApellido"
                MensajeEtiqueta(lblInfo, " Indique el apellido del trabajador ... ", TipoMensaje.iInfo)
            Case "txtDireccion"
                MensajeEtiqueta(lblInfo, " Indique la dirección o lugar de residencia del trabajador ... ", TipoMensaje.iInfo)
            Case "txtTelef1", "txtTelef2"
                MensajeEtiqueta(lblInfo, " Indique telefonos del trabajador ... ", TipoMensaje.iInfo)
            Case "txtemail"
                MensajeEtiqueta(lblInfo, " Indique el correo eléctronico del trabajador ...", TipoMensaje.iInfo)
            Case "txtFechaNacimiento", "btnNacimiento"
                MensajeEtiqueta(lblInfo, " Seleccione fecha de nacimiento del trabajador ...", TipoMensaje.iInfo)
            Case "txtCiudad"
                MensajeEtiqueta(lblInfo, " Indique el ciudad de nacimiento del trabajador ...", TipoMensaje.iInfo)
            Case "txtEstado"
                MensajeEtiqueta(lblInfo, " Indique el estado donde nació el trabajador ...", TipoMensaje.iInfo)
            Case "txtPais"
                MensajeEtiqueta(lblInfo, " Indique el país origen del trabajador ...", TipoMensaje.iInfo)
            Case "cmbEdoCivil"
                MensajeEtiqueta(lblInfo, " Seleccione estado civil del trabajador ...", TipoMensaje.iInfo)
            Case "txtAscendentes "
                MensajeEtiqueta(lblInfo, " Indique el número de parientes ascendentes (padres, abuelos, etc.) del trabajador ...", TipoMensaje.iInfo)
            Case "txtDescendentes"
                MensajeEtiqueta(lblInfo, " Indique el número de parientes descendentes (hijos, nietos, etc.) del trabajador ...", TipoMensaje.iInfo)
            Case "cmbTipoVivienda"
                MensajeEtiqueta(lblInfo, " Seleccione el tipo de vivienda del trabajador ...", TipoMensaje.iInfo)
            Case "txtSSO"
                MensajeEtiqueta(lblInfo, " Indique el número de afiliación al Seguro Social Obligatorio del trabajador ...", TipoMensaje.iInfo)
            Case "txtVehiculos"
                MensajeEtiqueta(lblInfo, " Indique la cantidad de vehículos que posee el trabajador ...", TipoMensaje.iInfo)
            Case "cmbSexo"
                MensajeEtiqueta(lblInfo, " Seleccione el sexo del trabajador ...", TipoMensaje.iInfo)
            Case "txtProfesion"
                MensajeEtiqueta(lblInfo, " Indique la profesión del trabajador ...", TipoMensaje.iInfo)
            Case "btnIngreso"
                MensajeEtiqueta(lblInfo, " Seleccione la fecha de ingreso a la empresa del trabajador ...", TipoMensaje.iInfo)
            Case "cmbFormapago"
                MensajeEtiqueta(lblInfo, " Seleccione la forma de pago asignada al trabajador ...", TipoMensaje.iInfo)
            Case "btnBanco"
                MensajeEtiqueta(lblInfo, " Seleccione el banco en cual se depositará el pago del trabajador ...", TipoMensaje.iInfo)
            Case "txtCtaBanco"
                MensajeEtiqueta(lblInfo, " Indique el número de cuenta del trabajador ...", TipoMensaje.iInfo)
            Case "btnGrupo"
                MensajeEtiqueta(lblInfo, " Seleccione el grupo principal del trabajador ...", TipoMensaje.iInfo)
            Case "btnSubgrupos"
                MensajeEtiqueta(lblInfo, " Seleccione e ó los grupos del trabajador ...", TipoMensaje.iInfo)
            Case "btnCargos"
                MensajeEtiqueta(lblInfo, " Seleccione el cargo del trabajador ...", TipoMensaje.iInfo)
            Case "txtSueldo"
                MensajeEtiqueta(lblInfo, " Indique el sueldo base del trabajador ...", TipoMensaje.iInfo)
            Case "txtISLR"
                MensajeEtiqueta(lblInfo, " Indique el PORCENTAJE RETENCIÓN ISLR del trabajador ...", TipoMensaje.iInfo)
            Case "cmbTipoNomina"
                MensajeEtiqueta(lblInfo, " Seleccione el tipo de nómina del trabajador ...", TipoMensaje.iInfo)
            Case "btnFoto"
                MensajeEtiqueta(lblInfo, " Seleccione una foto del trabajador (preferiblemente tipo carnet) ...", TipoMensaje.iInfo)
            Case "btnTurnoFecha"
                MensajeEtiqueta(lblInfo, " Seleccione la fecha a partir de la cual se aplicarán los turnos del trabajador ...", TipoMensaje.iInfo)
            Case "txtDiasLibres"
                MensajeEtiqueta(lblInfo, " indique la cantidad de días libres en el período-nómina del trabajador ...", TipoMensaje.iInfo)
            Case "btnDiaLibreFecha"
                MensajeEtiqueta(lblInfo, " Seleccione la fecha a partir de la cual se aplicará el ó los días libres del trabajador ...", TipoMensaje.iInfo)
            Case "cmbPeriodoDiaLibre"
                MensajeEtiqueta(lblInfo, " Seleccione el período-nómina en el cual se aplicará el ó los días libres del trabajador ...", TipoMensaje.iInfo)
        End Select

    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcBancos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcTrabajadores.SelectedIndexChanged
        Select Case tbcTrabajadores.SelectedIndex
            Case 0 ' Trabajadores
                If nPosicionCat >= 0 AndAlso dt.Rows.Count > 0 Then
                    AsignaTXT(nPosicionCat)
                Else
                    IniciarTrabajador(False)
                End If
            Case 1 ' Conceptos
                VerMovimientosEmpleado(cmbNominas.SelectedValue)
                If dt.Rows.Count > 0 Then
                    dg.Enabled = True
                    AsignaMov(nPosicionMov, True)
                Else
                    AsignaMov(nPosicionMov, False)
                End If
            Case 2 'Expediente
                If dt.Rows.Count > 0 Then
                    AsignaExp(nPosicionExp, True)
                    dgExpediente.Enabled = True
                Else
                    AsignaExp(nPosicionExp, False)
                End If
        End Select


    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarTrabajador(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                nPosicionCat = Me.BindingContext(ds, nTabla).Position

            AsignaTXT(nPosicionCat)
        End If
        DesactivarMarco0()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub

    Private Sub btnAgregaTurno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaTurno.Click

        Dim g As New jsControlArcTablaSimple
        Dim strSQLTurH As String = " select codtur codigo, nombre descripcion from jsnomcattur where tipo = '" & cmbTipoNomina.SelectedIndex & "' and id_emp = '" & jytsistema.WorkID & "' order by codtur "
        Dim nTablaTurH As String = "tblturnospos"
        ds = DataSetRequery(ds, strSQLTurH, myConn, nTablaTurH, lblInfo)
        Dim dtTurH As DataTable
        dtTurH = ds.Tables(nTablaTurH)
        g.Cargar("Turnos / Horarios ", ds, dtTurH, nTablaTurH, TipoCargaFormulario.iShowDialog, False)

        If g.Seleccion <> "" Then
            Dim aFldH() As String = {"codtra", "codtur", "id_emp"}
            Dim aStrH() As String = {txtCodigo.Text, g.Seleccion, jytsistema.WorkID}
            If Not qFound(myConn, lblInfo, "jsnomturtra", aFldH, aStrH) Then EjecutarSTRSQL(myConn, lblInfo, "insert into jsnomturtra set codtra = '" & txtCodigo.Text & "', codtur = '" & g.Seleccion & "', id_emp = '" & jytsistema.WorkID & "' ")
        End If

        'AsignaTXT(nPosicionCat)
        If g.Apuntador >= 0 Then AsignaTur(g.Apuntador, True)

        g = Nothing
        dtTurH.Dispose()
        dtTurH = Nothing

    End Sub

    Private Sub btnEliminaTurno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaTurno.Click
        '        With dtTurnos
        ' If .Rows.Count > 0 Then
        ' nPosicionTur = Me.BindingContext(ds, nTableTurnos).Position
        ' Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        ' Dim aCamDel() As String = {"codtra", "codtur", "id_emp"}
        ' Dim aStrDel() As String = {txtCodigo.Text, .Rows(nPosicionTur).Item("codtur").ToString, jytsistema.WorkID}

        '        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        '        If sRespuesta = MsgBoxResult.Yes Then
        ' AsignaTur(EliminarRegistros(myConn,lblinfo, ds, nTableTurnos, "jsnomturtra", _
        '                   strSQLTur, aCamDel, aStrDel, nPosicionTur), True)

        'End If
        'End If
        'End With

    End Sub

    Private Sub dgTar_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgTurnos.RowHeaderMouseClick, _
        dgTurnos.CellMouseClick, dgTurnos.RegionChanged
        Me.BindingContext(ds, nTableTurnos).Position = e.RowIndex
    End Sub

    Private Sub txtFechaNacimiento_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFechaNacimiento.TextChanged
        txtEdad.Text = CalculaDiferenciaFechas(CDate(txtFechaNacimiento.Text), jytsistema.sFechadeTrabajo, DiferenciaFechas.iAñosMesesDias)
    End Sub

    Private Sub txtIngreso_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIngreso.TextChanged
        txtAntiguedad.Text = CalculaDiferenciaFechas(CDate(txtIngreso.Text), jytsistema.sFechadeTrabajo, DiferenciaFechas.iAñosMesesDias)
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        txtCodigo1.Text = txtCodigo.Text
        txtCodigo2.Text = txtCodigo.Text
        txtCodigo3.Text = txtCodigo.Text
        txtCodigo4.Text = txtCodigo.Text
    End Sub

    Private Sub txtNombre_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        txtNombre.TextChanged, txtApellido.TextChanged
        txtNombre1.Text = txtNombre.Text + ", " + txtApellido.Text
        txtNombre2.Text = txtNombre.Text + ", " + txtApellido.Text
        txtNombre3.Text = txtNombre.Text + ", " + txtApellido.Text
        txtNombre4.Text = txtNombre.Text + ", " + txtApellido.Text
    End Sub

    Private Sub btnFoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFoto.Click
        Dim ofd As New OpenFileDialog()

        ofd.InitialDirectory = "c:\"
        ofd.Filter = "Archivos JPG |*.jpg"
        ofd.FilterIndex = 2
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            CaminoImagen = ofd.FileName
            pctFoto.ImageLocation = CaminoImagen
            pctFoto.Load()
        End If

        ofd = Nothing
    End Sub


    Private Sub btnG1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnG1.Click, _
        btnG2.Click, btnG3.Click, btnG4.Click, btnG5.Click, btnG6.Click


        Dim fg As New jsControlArcTablaSimple
        fg.Cargar("Grupos de trabajadores", FormatoTablaSimple(Modulo.iGrupoNom), True, TipoCargaFormulario.iShowDialog)

        Dim c As Control
        For Each c In C1DockingTabPage1.Controls
            If c.Name = "txtG" + Microsoft.VisualBasic.Right(sender.name.ToString, 1) Then
                c.Text = fg.Seleccion
            End If
        Next
        c = Nothing
        fg.Close()
        fg.Dispose()
        fg = Nothing

    End Sub

    Private Sub txtCodigoCargo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoCargo.TextChanged
        If txtCodigoCargo.Text <> "" Then txtDescripcionCargo.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsnomestcar where codigo = " & txtCodigoCargo.Text.Split(".")(UBound(txtCodigoCargo.Text.Split("."))) & " ")
    End Sub


    Private Sub txtBanco_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBanco.TextChanged
        If CBool(ParametroPlus(MyConn, Gestion.iRecursosHumanos, "NOMPARAM02")) Then
            txtNombreBanco.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select NOMBAN from jsbancatban where codban = '" & txtBanco.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Else
            Dim afld() As String = {"codigo", "modulo", "id_emp"}
            Dim aStr() As String = {txtBanco.Text, FormatoTablaSimple(Modulo.iBancos), jytsistema.WorkID}
            txtNombreBanco.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", afld, aStr, "descrip")
        End If
    End Sub

    Private Sub btnBanco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBanco.Click
        If Not CBool(ParametroPlus(MyConn, Gestion.iRecursosHumanos, "NOMPARAM02")) Then
            Dim f As New jsControlArcTablaSimple
            f.Cargar("Bancos", FormatoTablaSimple(Modulo.iBancos), True, TipoCargaFormulario.iShowDialog)
            txtBanco.Text = f.Seleccion
            f = Nothing
        End If
    End Sub

    Private Sub btnCargos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargos.Click
        Dim f As New jsNomArcCargos
        f.CodigoCargo = txtCodigoCargo.Text
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtCodigoCargo.Text = f.CodigoCargo
        f = Nothing
    End Sub

    Private Sub dgExpediente_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgExpediente.CellFormatting

        Select Case dgExpediente.Columns(e.ColumnIndex).Name
            Case "fecha"
                e.Value = FormatoFecha(CDate(e.Value.ToString))
            Case "causa"
                e.Value = aCausaExpedienteNomina(e.Value)
        End Select
    End Sub

    Private Sub dgExpediente_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgExpediente.RowHeaderMouseClick, _
        dgExpediente.CellMouseClick, dgExpediente.RegionChanged
        nPosicionExp = e.RowIndex
        Me.BindingContext(ds, nTableExp).Position = nPosicionExp
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTableExp).Rows.Count)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dgPrestramos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgPrestamos.RowHeaderMouseClick, _
        dgPrestamos.CellMouseClick, dgPrestamos.RegionChanged

        nPosicionPre = e.RowIndex
        Me.BindingContext(ds, nTablePre).Position = nPosicionPre
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablePre).Rows.Count)
    End Sub

    Private Sub dgAsistencias_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgAsistencias.RowHeaderMouseClick, _
          dgAsistencias.CellMouseClick, dgAsistencias.RegionChanged
        Me.BindingContext(ds, nTableAsi).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTableAsi).Rows.Count)
    End Sub

    Private Sub dgAsistencias_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgAsistencias.CellContentClick

    End Sub

    Private Sub dgAsistencias_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgAsistencias.CellFormatting
        Select Case dgAsistencias.Columns(e.ColumnIndex).Name
            Case "dia"
                e.Value = FormatoFecha(CDate(e.Value.ToString))
            Case "entrada", "salida", "descanso", "retorno"
                e.Value = Format(CDate(e.Value.ToString), "dd/MM/yyyy HH:mm")
            Case "tipo"
                Select Case CInt(e.Value)
                    Case 0
                        e.Value = "Normal"
                    Case 1
                        e.Value = "Libre Contrato Colectivo"
                    Case 2
                        e.Value = "Libre Turno"
                    Case 3
                        e.Value = "Libre Feriado"
                    Case 4
                        e.Value = "Extras"
                End Select
        End Select
    End Sub


    Private Sub cmbFormapago_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbFormapago.SelectedIndexChanged
        If CBool(ParametroPlus(MyConn, Gestion.iRecursosHumanos, "NOMPARAM02")) Then
            Select Case cmbFormapago.SelectedIndex
                Case 2 'DEPOSITO
                    txtBanco.Text = ParametroPlus(MyConn, Gestion.iRecursosHumanos, "NOMPARAM03")
                    txtBanco1.Text = ParametroPlus(MyConn, Gestion.iRecursosHumanos, "NOMPARAM04")
                Case Else
                    txtBanco.Text = ""
                    txtBanco1.Text = ""
            End Select
        End If
    End Sub

    Private Sub txtBanco1_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBanco1.TextChanged
        Dim afld() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtBanco1.Text, FormatoTablaSimple(Modulo.iBancos), jytsistema.WorkID}
        txtNombreBanco1.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", afld, aStr, "descrip")
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnBanco1.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Bancos", FormatoTablaSimple(Modulo.iBancos), True, TipoCargaFormulario.iShowDialog)
        txtBanco1.Text = f.Seleccion
        f = Nothing
    End Sub

 
    Private Sub cmbNominas_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbNominas.SelectedIndexChanged
        If tbcTrabajadores.SelectedIndex = 1 Then
            VerMovimientosEmpleado(cmbNominas.SelectedValue)
        End If

    End Sub
End Class