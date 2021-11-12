Imports MySql.Data.MySqlClient
Imports System.IO
Imports Syncfusion.WinForms.Input

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

    Private dtNominas As New DataTable
    Private ft As New Transportables


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

    Private Sub jsNomArcTrabajadores_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsNomArcTrabajadores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

            Dim dates As SfDateTimeEdit() = {txtFechaDiaLibre, txtFechaNacimiento, txtIngreso, txtFechaTurno}
            SetSizeDateObjects(dates)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarTrabajador(False)
            End If

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

            tbcTrabajadores.SelectedTab = C1DockingTabPage1


        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir, btnAgregaTurno, btnEliminaTurno)

        'Botones Adicionales
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnBanco, btnG1, btnG2, btnG3, btnG4, btnG5, btnG6,
                         btnFoto, btnCargos)

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarra, dg, lRegion, _
                               jytsistema.sUsuario, nRow, Actualiza)

        CalculaTotalesConceptos()

    End Sub

    Private Sub AsignaExp(ByVal nRow As Long, ByVal Actualiza As Boolean)
        If strsqlExp <> "" Then _
        dtExpediente = ft.MostrarFilaEnTabla(myConn, ds, nTableExp, strsqlExp, Me.BindingContext, MenuBarra, dgExpediente, _
                                              lRegion, jytsistema.sUsuario, nRow, Actualiza)
    End Sub
    Private Sub AsignaAsi(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtAsistencia = ft.MostrarFilaEnTabla(myConn, ds, nTableAsi, strSQLAsi, Me.BindingContext, MenuBarra, dgAsistencias, lRegion, _
                                             jytsistema.sUsuario, nRow, Actualiza)
        CalculaTotalesAsistencia(dtAsistencia)
    End Sub
    Private Sub AsignaPre(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtPres = ft.MostrarFilaEnTabla(myConn, ds, nTablePre, strSQLPre, Me.BindingContext, MenuBarra, dgPrestamos, _
                                        lRegion, jytsistema.sUsuario, nRow, Actualiza)
    End Sub
    Private Sub AsignaTur(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtTurnos = ft.MostrarFilaEnTabla(myConn, ds, nTableTurnos, strSQLTur, Me.BindingContext, MenuBarra, dgTurnos, lRegion, _
                                         jytsistema.sUsuario, nRow, Actualiza, False)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            ft.MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            If .Rows.Count > 0 Then

                With .Rows(nRow)

                    'Trabajador 
                    txtCodigo.Text = ft.muestraCampoTexto(.Item("codtra"))
                    txtNacionalidad.Text = ft.muestraCampoTexto(.Item("nacional"))
                    txtID.Text = ft.muestraCampoTexto(.Item("cedula"))
                    txtBiometrico.Text = ft.muestraCampoTexto(.Item("codhp"))

                    ft.RellenaCombo(aCondicion, cmbCondicion, CInt(.Item("condicion").ToString))

                    txtNombre.Text = ft.muestraCampoTexto(.Item("nombres"))
                    txtApellido.Text = ft.muestraCampoTexto(.Item("apellidos"))
                    txtDireccion.Text = ft.muestraCampoTexto(.Item("direccion"))
                    txtTelef1.Text = ft.muestraCampoTexto(.Item("telef1"))
                    txtTelef2.Text = ft.muestraCampoTexto(.Item("telef2"))
                    txtemail.Text = ft.muestraCampoTexto(.Item("email"))
                    txtFechaNacimiento.Value = .Item("fnacimiento")
                    txtCiudad.Text = ft.muestraCampoTexto(.Item("lugarnac"))
                    txtEstado.Text = ft.muestraCampoTexto(.Item("edonac"))
                    txtPais.Text = ft.muestraCampoTexto(.Item("pais"))

                    ft.RellenaCombo(aEdoCivil, cmbEdoCivil, .Item("edocivil"))

                    txtAscendentes.Text = ft.muestraCampoEntero(.Item("ascendentes"))
                    txtDescendentes.Text = ft.muestraCampoEntero(.Item("descendentes"))

                    ft.RellenaCombo(aTipovivienda, cmbTipoVivienda, CInt(.Item("vivienda").ToString))

                    txtSSO.Text = ft.muestraCampoTexto(.Item("nosso"))
                    txtVehiculos.Text = ft.muestraCampoEntero(.Item("vehiculos"))

                    ft.RellenaCombo(aSexo, cmbSexo, CInt(.Item("sexo").ToString))

                    txtProfesion.Text = ft.muestraCampoTexto(.Item("profesion"))
                    txtIngreso.Value = .Item("ingreso")

                    If ft.DevuelveScalarEntero(myConn, " select count(*) from jsnomexptra where codtra = '" & txtCodigo.Text & "' and causa = '0' and id_emp = '" & jytsistema.WorkID & "'") = 0 Then
                        ft.Ejecutar_strSQL(myConn, " insert into jsnomexptra set codtra = '" & txtCodigo.Text & "', fecha = '" & ft.FormatoFechaHoraMySQL(CDate(txtIngreso.Text)) & "', comentario = 'FECHA INGRESO A LA EMPRESA', causa = 0, id_emp = '" & jytsistema.WorkID & "'    ")
                    End If
                    ft.RellenaCombo(aFP, cmbFormapago, CInt(.Item("formapago").ToString))

                    txtBanco.Text = ft.muestraCampoTexto(.Item("banco"))
                    txtCtaBanco.Text = ft.muestraCampoTexto(.Item("ctaban"))
                    txtBanco1.Text = ft.muestraCampoTexto(.Item("banco_1"))
                    txtNombreBanco1.Text = ft.muestraCampoTexto(.Item("ctaban_1"))

                    txtG1.Text = ft.muestraCampoTexto(.Item("subnivel1"))
                    txtG2.Text = ft.muestraCampoTexto(.Item("subnivel2"))
                    txtG3.Text = ft.muestraCampoTexto(.Item("subnivel3"))
                    txtG4.Text = ft.muestraCampoTexto(.Item("subnivel4"))
                    txtG5.Text = ft.muestraCampoTexto(.Item("subnivel5"))
                    txtG6.Text = ft.muestraCampoTexto(.Item("subnivel6"))

                    txtCodigoCargo.Text = ft.muestraCampoTexto(.Item("cargo"))
                    txtSueldo.Text = ft.muestraCampoTexto(ft.FormatoNumero(.Item("sueldo")))
                    txtISLR.Text = ft.muestraCampoTexto(ft.FormatoNumero(.Item("RETISLR")))

                    CaminoImagen = ft.ArchivoEnBaseDatos_A_ArchivoEnDisco(dt.Rows(nRow), "foto", .Item("codtra"), ".jpg")
                    If My.Computer.FileSystem.FileExists(CaminoImagen) Then
                        Dim fs As System.IO.FileStream
                        fs = New System.IO.FileStream(CaminoImagen, IO.FileMode.Open, IO.FileAccess.Read)
                        pctFoto.Image = System.Drawing.Image.FromStream(fs)
                        fs.Close()
                    Else
                        pctFoto.Image = Nothing
                    End If

                    txtFechaTurno.Value = .Item("turnodesde")
                    txtDiasLibres.Text = ft.muestraCampoEntero(.Item("freedays"))

                    ft.RellenaCombo(aTipoNomina, cmbPeriodoDiaLibre, CInt(.Item("periodo").ToString))

                    txtFechaDiaLibre.Value = .Item("datefreeday")
                    chkRotacion.Checked = .Item("rotatorio")

                    'NOMINAS
                    strSQLNominas = " select a.codnom, a.descripcion from jsnomencnom a " _
                        & " left join jsnomrennom b on (a.codnom = b.codnom and a.id_emp = b.id_emp) " _
                        & " where b.codtra = '" & .Item("codtra") & "' and " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' order by 1 "

                    dtNominas = ft.AbrirDataTable(ds, nTablaNominas, myConn, strSQLNominas)

                    Dim aCamCI() As String = {"codnom.Código.50.I.", "descripcion.Descripción.200.I."}
                    ft.IniciarTablaPlus(dgNominas, dtNominas, aCamCI, False, , New Font("Consolas", 8, FontStyle.Regular), False)
                    If dtNominas.Rows.Count > 0 Then
                        nPosicionNomina = 0
                        ft.RellenaComboConDatatable(cmbNominas, dtNominas, "DESCRIPCION", "CODNOM")
                    End If

                    'Turnos
                    strSQLTur = " select a.codtur, b.nombre " _
                                            & " from jsnomturtra a " _
                                            & " left join jsnomcattur b on (a.codtur = b.codtur and a.id_emp = b.id_emp) " _
                                            & " where " _
                                            & " a.codtra = '" & .Item("codtra") & "' and " _
                                            & " a.id_emp = '" & jytsistema.WorkID & "' "

                    dtTurnos = ft.AbrirDataTable(ds, nTableTurnos, myConn, strSQLTur)

                    Dim aCamTur() As String = {"codtur.Código.75.C.", "nombre.Nombre.200.I."}
                    ft.IniciarTablaPlus(dgTurnos, dtTurnos, aCamTur)
                    If dtTurnos.Rows.Count > 0 Then nPosicionTur = 0

                    If cmbCondicion.SelectedIndex <> 1 Then EliminarAsignacionesYDeducciones(myConn, lblInfo, txtCodigo.Text)


                    'Expediente 
                    strsqlExp = " select * from jsnomexptra where codtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "
                    ds = DataSetRequery(ds, strsqlExp, myConn, nTableExp, lblInfo)
                    dtExpediente = ds.Tables(nTableExp)

                    Dim aCamExp() As String = {"fecha.Inicio.90.C.fecha", _
                                               "fecha_fin.Termino.90.C.fecha", _
                                               "comentario.Comentario.350.I.", _
                                               "causa.Causa.200.I.", _
                                               "..100.I."}
                    ft.IniciarTablaPlus(dgExpediente, dtExpediente, aCamExp)

                    If dtExpediente.Rows.Count > 0 Then nPosicionExp = 0

                    'Asistencias
                    Dim FechaInicial As Date, FechaFinal As Date
                    Select Case CInt(.Item("tiponom").ToString)
                        Case 0 'Diaria
                            FechaInicial = jytsistema.sFechadeTrabajo
                            FechaFinal = jytsistema.sFechadeTrabajo
                        Case 1 'Semanal
                            Dim DiaPrimeroSemana As Integer = CInt(ParametroPlus(myConn, Gestion.iRecursosHumanos, "NOMPARAM01")) + 1
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
                        & " a.dia >= '" & ft.FormatoFechaMySQL(FechaInicial) & "' and  " _
                        & " a.dia <= '" & ft.FormatoFechaMySQL(FechaFinal) & "' and  " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' order by a.dia desc, a.tipo "

                    dtAsistencia = ft.AbrirDataTable(ds, nTableAsi, myConn, strSQLAsi)

                    Dim aCamAsi() As String = {"dia.Día.80.C.Fecha", _
                                               "entrada.Entrada.120.C.Hora", _
                                               "descanso.Descanso.120.C.Hora", _
                                               "retorno.Retorno.120.C.Hora", _
                                               "salida.Salida.120.C.Hora", _
                                               "horas.Total Horas.70.C.Entero", _
                                               "tipo.Tipo.200.I.", _
                                               "control..100.I."}

                    ft.IniciarTablaPlus(dgAsistencias, dtAsistencia, aCamAsi)

                    If dtAsistencia.Rows.Count > 0 Then nPosicionAsi = 0
                    CalculaTotalesAsistencia(dtAsistencia)

                    'PRESTAMOS
                    'Encabezado
                    strSQLPre = " SELECT a.*, b.num_cuota, b.fechafin, b.monto FROM jsnomencpre a " _
                        & " LEFT JOIN jsnomrenpre b ON (a.codtra = b.codtra AND a.codpre = b.codpre AND a.id_emp = b.id_emp and b.procesada = 1)  " _
                        & " WHERE " _
                        & " a.codtra = '" & .Item("codtra") & "' AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY a.codnom, a.codpre, b.fechafin DESC, b.num_cuota limit 1 "

                    dtPres = ft.AbrirDataTable(ds, nTablePre, myConn, strSQLPre)

                    Dim aCamPRE() As String = {"codnom.Código Nómina.100.C.", _
                                               "codpre.Código Préstamo.100.C.", _
                                               "descrip.Descripción.250.I.", _
                                               "fechaprestamo.Fecha Aprobación.100.C.fecha", _
                                               "montotal.Monto.120.D.Numero", _
                                               "fechainicio.Fecha Inicio Descuentos.100.C.fecha", _
                                               "saldo.Saldo Actual.120.D.Numero", _
                                               "fechafin.Fecha Ultimo Cobro.100.C.fecha", _
                                               "monto.Ultimo Cobro.120.D.Numero", _
                                               "sada..100.I."}

                    ft.IniciarTablaPlus(dgPrestamos, dtPres, aCamPRE)
                    If dtPres.Rows.Count > 0 Then nPosicionPre = 0


                End With
            End If
        End With

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub VerMovimientosEmpleado(CodigoNomina As String)

        strSQLMov = " select a.codcon, b.nomcon, b.tipo, a.importe,  " _
                            & " if(b.tipo = 0, a.importe, 0.00) asignaciones,  " _
                            & " if(b.tipo = 1, a.importe, 0.00) deducciones, " _
                            & " if(b.tipo = 2, a.importe, 0.00) adicionales,  " _
                            & " if(b.tipo = 3, a.importe, 0.00) especiales, porcentaje_asig, '' control  " _
                            & " from jsnomtrades a " _
                            & " left join jsnomcatcon b on ( a.codcon = b.codcon AND a.CODNOM = b.CODNOM AND a.id_emp = b.id_emp ) " _
                            & " where " _
                            & " a.importe > 0 and " _
                            & " a.codnom = '" & CodigoNomina & "' AND " _
                            & " a.codtra = '" & txtCodigo.Text & "' and  " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' order by b.tipo, a.codcon "

        dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

        Dim aCampos() As String = {"codcon.Código.70.C.", _
                                   "nomcon.Descripción.350.I.", _
                                   "asignaciones.Asignaciones.90.D.Numero", _
                                   "deducciones.Deducciones.90.D.Numero", _
                                   "adicionales.Adicionales.90.D.Numero", _
                                   "especiales.Especiales.90.D.Numero", _
                                   "porcentaje_asig.%  Asignación.90.D.Numero", _
                                   "control..90.I."}

        ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        CalculaTotalesConceptos()

    End Sub

    Private Sub CalculaTotalesConceptos()

        Dim dAsi As Double = 0.0, dDed As Double = 0.0, dAdi As Double = 0.0, dEsp As Double = 0.0
        For Each nRow As DataRow In dtMovimientos.Rows
            With nRow
                dAsi += ft.valorNumero(.Item("asignaciones"))
                dDed += ft.valorNumero(.Item("deducciones"))
                dAdi += ft.valorNumero(.Item("adicionales"))
                dEsp += ft.valorNumero(.Item("especiales"))
            End With
        Next

        txtAsignaciones.Text = ft.FormatoNumero(dAsi)
        txtDeducciones.Text = ft.FormatoNumero(dDed)
        txtAdicionales.Text = ft.FormatoNumero(dAdi)
        txtEspeciales.Text = ft.FormatoNumero(dEsp)
        txtTotal.Text = ft.FormatoNumero(dAsi - dDed)

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
            txtCodigo.Text = ft.autoCodigo(myConn, "codtra", "jsnomcattra", "id_emp", jytsistema.WorkID, 10, True)
        Else
            txtCodigo.Text = ""
        End If
        txtNacionalidad.Text = "V"
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtID, txtBiometrico, txtNombre, txtApellido, txtDireccion, _
                            txtTelef1, txtTelef2, txtemail, txtCiudad, txtEstado, txtPais, txtCtaBanco, _
                            txtNombreBanco1, txtBanco, txtBanco1, txtG1, txtG2, txtG3, txtG4, txtG5, txtG6, _
                            txtDescripcionCargo, txtProfesion, txtSSO)
        ft.RellenaCombo(aCondicion, cmbCondicion)
        txtFechaNacimiento.Value = jytsistema.sFechadeTrabajo
        ft.RellenaCombo(aEdoCivil, cmbEdoCivil)
        txtAscendentes.Text = ft.FormatoEntero(0)
        txtDescendentes.Text = ft.FormatoEntero(0)
        ft.RellenaCombo(aTipovivienda, cmbTipoVivienda)
        txtVehiculos.Text = ft.FormatoEntero(0)
        ft.RellenaCombo(aSexo, cmbSexo)
        txtIngreso.Value = jytsistema.sFechadeTrabajo
        ft.RellenaCombo(aFP, cmbFormapago)


        txtSueldo.Text = ft.FormatoNumero(0.0)
        txtISLR.Text = ft.FormatoNumero(0.0)

        ft.RellenaCombo(aTipoNomina, cmbTipoNomina)

        pctFoto.Image = Nothing
        txtFechaTurno.Value = jytsistema.sFechadeTrabajo
        txtDiasLibres.Text = ft.FormatoEntero(0)

        ft.RellenaCombo(aTipoNomina, cmbPeriodoDiaLibre)

        txtFechaDiaLibre.Value = jytsistema.sFechadeTrabajo
        chkRotacion.Checked = False

        dg.Columns.Clear()
        dgTurnos.Columns.Clear()
        dgNominas.Columns.Clear()



    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True
        ft.habilitarObjetos(False, True, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5)

        ft.habilitarObjetos(True, True, txtNacionalidad, txtID, txtBiometrico, cmbCondicion, txtNombre, txtApellido,
                         txtDireccion, txtTelef1, txtTelef2, txtemail, txtFechaNacimiento, txtCiudad, txtEstado, txtPais,
                         cmbEdoCivil, txtAscendentes, txtDescendentes, cmbTipoVivienda, txtSSO, txtVehiculos, cmbSexo,
                         txtProfesion, txtIngreso, cmbFormapago, txtCtaBanco, txtCtaBanco1,
                         btnG1, btnG2, btnG3, btnG4, btnG5, btnG6, btnCargos, txtSueldo, txtISLR, cmbTipoNomina, btnFoto,
                         txtFechaTurno, txtDiasLibres, cmbPeriodoDiaLibre, txtFechaDiaLibre, chkRotacion)
        MenuTurnos.Enabled = True

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False

        ft.habilitarObjetos(True, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5)

        ft.habilitarObjetos(False, True, txtCodigo, txtNacionalidad, txtID, txtBiometrico, cmbCondicion, txtNombre, txtApellido,
                         txtDireccion, txtTelef1, txtTelef2, txtemail, txtFechaNacimiento, txtCiudad, txtEstado, txtPais,
                         txtEdad, cmbEdoCivil, txtAscendentes, txtDescendentes, cmbTipoVivienda, txtSSO, txtVehiculos, cmbSexo,
                         txtProfesion, txtIngreso, txtAntiguedad, cmbFormapago, txtBanco, txtBanco1, btnBanco, btnBancoDeposito,
                         txtNombreBanco, txtNombreBanco1, txtCtaBanco, txtCtaBanco1, txtG1, txtG2, txtG3, txtG4, txtG5, txtG6,
                         btnG1, btnG2, btnG3, btnG4, btnG5, btnG6, txtDescripcionCargo, btnCargos, txtSueldo, cmbTipoNomina, btnFoto,
                         txtFechaTurno, txtDiasLibres, cmbPeriodoDiaLibre, txtFechaDiaLibre, chkRotacion, txtISLR)

        MenuTurnos.Enabled = False

        ft.habilitarObjetos(True, False, dgPrestamos, dgExpediente, dg, dgAsistencias)

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

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
            Case "Cuotas/Préstamos"
                nPosicionPre = 0
                Me.BindingContext(ds, nTablePre).Position = nPosicionPre
                AsignaPre(Me.BindingContext(ds, nTablePre).Position, False)
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
            Case "Cuotas/Préstamos"
                Me.BindingContext(ds, nTablePre).Position -= 1
                AsignaPre(Me.BindingContext(ds, nTablePre).Position, False)
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
            Case "Cuotas/Préstamos"
                Me.BindingContext(ds, nTablePre).Position += 1
                AsignaPre(Me.BindingContext(ds, nTablePre).Position, False)
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
            Case "Cuotas/Préstamos"
                Me.BindingContext(ds, nTablePre).Position = ds.Tables(nTablePre).Rows.Count - 1
                AsignaPre(Me.BindingContext(ds, nTablePre).Position, False)

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
                    f.Editar(myConn, ds, dtPres, txtCodigo.Text, dtPres.Rows(f.Apuntador).Item("codnom"))
                    AsignaPre(f.Apuntador, True)
                End If
            Case "Equipamiento"

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
                    ft.mensajeAdvertencia("Este trabajador está Inactivo(a) ....")
                End If

            Case "Expediente"
                EliminarExpediente()
            Case "Cuotas/Préstamos"
                EliminarPrestamo()
            Case "Equipamiento"
        End Select
    End Sub
    Private Sub EliminarPrestamo()

        If ft.PreguntaEliminarRegistro() = DialogResult.Yes Then
            If dtPres.Rows.Count > 0 Then
                With dtPres.Rows(Me.BindingContext(ds, nTablePre).Position)
                    If .Item("NUMCUOTAS") = ft.DevuelveScalarEntero(myConn, " SELECT COUNT(*) FROM jsnomrenpre where codtra = '" & .Item("codtra") & "' and codnom = '" & .Item("codnom") & "' and codpre = '" & .Item("codpre") & "' and procesada = 0 and id_emp = '" & jytsistema.WorkID & "' ") Then
                        Dim afld() As String = {"codtra", "codnom", "codpre", "id_emp"}
                        Dim aStr() As String = { .Item("codtra"), .Item("codnom"), .Item("codpre"), jytsistema.WorkID}

                        EliminarRegistros(myConn, lblInfo, ds, nTablePre, "jsnomrenpre", strSQLPre,
                                     afld, aStr, Me.BindingContext(ds, nTablePre).Position, True)
                        Me.BindingContext(ds, nTablePre).Position = EliminarRegistros(myConn, lblInfo, ds, nTablePre, "jsnomencpre", strSQLPre,
                                                                                              afld, aStr, Me.BindingContext(ds, nTablePre).Position, True)
                        nPosicionPre = Me.BindingContext(ds, nTablePre).Position
                        AsignaPre(nPosicionPre, True)
                    Else
                        ft.mensajeCritico("Este préstamo YA ha sido descontado en Nómina. Verifique por favor... ")
                    End If
                End With
            End If
        End If

    End Sub
    Private Sub EliminarExpediente()


        With dtExpediente.Rows(nPosicionExp)
            If ft.PreguntaEliminarRegistro() = DialogResult.Yes Then
                Dim afld() As String = {"codtra", "Fecha", "causa", "id_emp"}
                Dim aStr() As String = {txtCodigo.Text, ft.FormatoFechaMySQL(CDate(.Item("fecha").ToString)), .Item("causa"), jytsistema.WorkID}


                Me.BindingContext(ds, nTableExp).Position = EliminarRegistros(myConn, lblInfo, ds, nTableExp, "jsnomexptra", strsqlExp,
                                                                                      afld, aStr, Me.BindingContext(ds, nTableExp).Position, True)
                nPosicionExp = Me.BindingContext(ds, nTableExp).Position
                AsignaExp(nPosicionExp, True)
            End If

        End With
    End Sub
    Private Sub EliminaConcepto()
        Dim aFldEli() As String = {"codtra", "codcon", "id_emp"}
        Dim aStrEli() As String = {txtCodigo.Text, dtMovimientos.Rows(nPosicionMov).Item("codcon"), jytsistema.WorkID}
        If ft.PreguntaEliminarRegistro() = DialogResult.Yes Then
            Me.BindingContext(ds, nTablaMovimientos).Position = EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jsnomtrades", strSQLMov,
                                                                                  aFldEli, aStrEli, Me.BindingContext(ds, nTablaMovimientos).Position, True)
            nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
            AsignaMov(nPosicionMov, True)
        End If
    End Sub
    Private Sub EliminaTrabajador()

        Dim aCamposDel() As String = {"codtra", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = DialogResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then

                Me.BindingContext(ds, nTabla).Position = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsnomcattra", strSQL, aCamposDel, aStringsDel,
                                                              Me.BindingContext(ds, nTabla).Position, True)
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(nPosicionCat)
            Else
                ft.mensajeAdvertencia("Este trabajador posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar
        Select Case tbcTrabajadores.SelectedTab.Text
            Case "Trabajador"
                Dim Campos() As String = {"codtra", "apellidos", "nombres"}
                Dim Nombres() As String = {"Código", "Apellidos", "Nombres"}
                Dim Anchos() As Integer = {100, 200, 200}
                f.Text = "Trabajadores"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Trabajadores...")
                Me.BindingContext(ds, nTabla).Position = f.Apuntador
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(nPosicionCat)
                f = Nothing
            Case "Conceptos"
        End Select
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
                ft.mensajeAdvertencia("Código trabajador YA existe. Por favor verifique...")
                Return False
            End If

            If txtID.Text <> "" Then
                Dim aFldI() As String = {"cedula", "id_emp"}
                Dim aStrI() As String = {txtID.Text, jytsistema.WorkID}
                If qFound(myConn, lblInfo, "jsnomcattra", aFldI, aStrI) Then
                    ft.mensajeAdvertencia("Cédula de identidad YA existe. Por favor verifique...")
                    Return False
                End If
            End If
            If txtBiometrico.Text <> "" Then
                Dim aFldB() As String = {"codhp", "id_emp"}
                Dim aStrB() As String = {txtBiometrico.Text, jytsistema.WorkID}
                If qFound(myConn, lblInfo, "jsnomcattra", aFldB, aStrB) Then
                    ft.mensajeAdvertencia("Código Biométrico YA existe. Por favor verifique...")
                    Return False
                End If
            End If

        End If

        If cmbFormapago.SelectedIndex = 2 AndAlso txtBanco.Text <> "" _
            AndAlso txtCtaBanco.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de CUENTA BANCARIA 1 válida. Por favor verifique...")
            Return False
        End If

        If cmbFormapago.SelectedIndex = 2 AndAlso txtBanco1.Text <> "" _
            AndAlso txtCtaBanco1.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de CUENTA BANCARIA 2 válida. Por favor verifique...")
            Return False
        End If

        If Not ft.isNumeric(txtSueldo.Text) Then
            ft.mensajeCritico("Debe indicar un sueldo Válido para este trabajador...")
            ft.enfocarTexto(txtSueldo)
            Return False
        End If

        If Not ft.isNumeric(txtISLR.Text) Then
            ft.mensajeCritico("Debe indicar un porcentaje de Impuesto sobre la renta Válido para este trabajador...")
            ft.enfocarTexto(txtISLR)
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

        InsertEditNOMINATrabajador(myConn, lblInfo, Inserta, txtCodigo.Text, txtBiometrico.Text, txtApellido.Text, txtNombre.Text, txtProfesion.Text, CDate(txtIngreso.Text),
             CDate(txtFechaNacimiento.Text), txtCiudad.Text, txtPais.Text, "0", txtNacionalidad.Text, "", txtID.Text, cmbEdoCivil.SelectedIndex,
             cmbSexo.SelectedIndex, ValorEntero(txtAscendentes.Text), ValorEntero(txtDescendentes.Text), txtSSO.Text, "0", cmbTipoVivienda.SelectedIndex,
             ValorEntero(txtVehiculos.Text), txtDireccion.Text, txtTelef1.Text, txtTelef2.Text, txtemail.Text, cmbCondicion.SelectedIndex,
             cmbTipoNomina.SelectedIndex, cmbFormapago.SelectedIndex, txtBanco.Text, txtCtaBanco.Text, txtBanco1.Text, txtNombreBanco1.Text, "", "", "", "", "",
             "", CDate(txtIngreso.Text), "", "", "", txtG1.Text, txtG2.Text,
             txtG3.Text, txtG4.Text, txtG5.Text, txtG6.Text, txtCodigoCargo.Text, CDate(txtFechaTurno.Text),
             ValorNumero(txtSueldo.Text), ValorNumero(txtISLR.Text), ValorEntero(txtDiasLibres.Text), cmbPeriodoDiaLibre.SelectedIndex, CDate(txtFechaDiaLibre.Text),
             chkRotacion.Checked, CDate(txtIngreso.Text))

        If Not pctFoto.Image Is Nothing Then
            If CaminoImagen <> "" Then
                Dim fs As New FileStream(CaminoImagen, FileMode.OpenOrCreate, FileAccess.Read)
                MyData = New Byte(fs.Length) {}
                fs.Read(MyData, 0, fs.Length)
                fs.Close()
                GuardarFotoTrabajador(myConn, lblInfo, txtCodigo.Text, MyData)
                ft.GuardarArchivoEnBaseDeDatos(myConn, True, CaminoImagen, txtCodigo.Text, "00001", "NOM", "TRA", txtNombre.Text + " " + txtApellido.Text, "jpg", jytsistema.WorkID)
            End If
        End If

        If cmbCondicion.SelectedIndex <> 1 Then EliminarAsignacionesYDeducciones(myConn, lblInfo, txtCodigo.Text)

        'AsignaConceptosATrabajador(myConn, lblInfo, ds, txtCodigo.Text)
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODTRA = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcTrabajadores.SelectedTab = C1DockingTabPage1
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, txtNacionalidad.GotFocus,
        txtID.GotFocus, txtBiometrico.GotFocus, cmbCondicion.GotFocus, txtNombre.GotFocus, txtApellido.GotFocus,
        txtDireccion.GotFocus, txtTelef1.GotFocus, txtTelef2.GotFocus, txtemail.GotFocus, txtCiudad.GotFocus, txtEstado.GotFocus, txtPais.GotFocus, cmbEdoCivil.GotFocus,
        txtAscendentes.GotFocus, txtDescendentes.GotFocus, cmbTipoVivienda.GotFocus, txtSSO.GotFocus,
        txtVehiculos.GotFocus, cmbSexo.GotFocus, txtProfesion.GotFocus, cmbFormapago.GotFocus,
        btnBanco.GotFocus, txtCtaBanco.GotFocus, txtSueldo.GotFocus, cmbTipoNomina.GotFocus, btnFoto.GotFocus, txtDiasLibres.GotFocus, cmbPeriodoDiaLibre.GotFocus,
         btnG6.GotFocus, btnCargos.GotFocus, txtNombre.MouseHover


        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, " Indique el código del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtNacionalidad"
                ft.mensajeEtiqueta(lblInfo, " Indique nacionalidad (V,E,P) del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtID"
                ft.mensajeEtiqueta(lblInfo, " Indique el número de cédula de identidad trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtBiometrico"
                ft.mensajeEtiqueta(lblInfo, " Indique el código de identificación biométrica del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "cmbCondicion"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la condición o estatus del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, " Indique el nombre del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtApellido"
                ft.mensajeEtiqueta(lblInfo, " Indique el apellido del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtDireccion"
                ft.mensajeEtiqueta(lblInfo, " Indique la dirección o lugar de residencia del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtTelef1", "txtTelef2"
                ft.mensajeEtiqueta(lblInfo, " Indique telefonos del trabajador ... ", Transportables.tipoMensaje.iInfo)
            Case "txtemail"
                ft.mensajeEtiqueta(lblInfo, " Indique el correo eléctronico del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtFechaNacimiento", "btnNacimiento"
                ft.mensajeEtiqueta(lblInfo, " Seleccione fecha de nacimiento del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtCiudad"
                ft.mensajeEtiqueta(lblInfo, " Indique el ciudad de nacimiento del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtEstado"
                ft.mensajeEtiqueta(lblInfo, " Indique el estado donde nació el trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtPais"
                ft.mensajeEtiqueta(lblInfo, " Indique el país origen del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "cmbEdoCivil"
                ft.mensajeEtiqueta(lblInfo, " Seleccione estado civil del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtAscendentes "
                ft.mensajeEtiqueta(lblInfo, " Indique el número de parientes ascendentes (padres, abuelos, etc.) del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtDescendentes"
                ft.mensajeEtiqueta(lblInfo, " Indique el número de parientes descendentes (hijos, nietos, etc.) del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "cmbTipoVivienda"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el tipo de vivienda del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtSSO"
                ft.mensajeEtiqueta(lblInfo, " Indique el número de afiliación al Seguro Social Obligatorio del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtVehiculos"
                ft.mensajeEtiqueta(lblInfo, " Indique la cantidad de vehículos que posee el trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "cmbSexo"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el sexo del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtProfesion"
                ft.mensajeEtiqueta(lblInfo, " Indique la profesión del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "btnIngreso"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la fecha de ingreso a la empresa del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "cmbFormapago"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la forma de pago asignada al trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "btnBanco"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el banco en cual se depositará el pago del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtCtaBanco"
                ft.mensajeEtiqueta(lblInfo, " Indique el número de cuenta del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "btnGrupo"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el grupo principal del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "btnSubgrupos"
                ft.mensajeEtiqueta(lblInfo, " Seleccione e ó los grupos del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "btnCargos"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el cargo del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtSueldo"
                ft.mensajeEtiqueta(lblInfo, " Indique el sueldo base del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtISLR"
                ft.mensajeEtiqueta(lblInfo, " Indique el PORCENTAJE RETENCIÓN ISLR del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "cmbTipoNomina"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el tipo de nómina del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "btnFoto"
                ft.mensajeEtiqueta(lblInfo, " Seleccione una foto del trabajador (preferiblemente tipo carnet) ...", Transportables.tipoMensaje.iInfo)
            Case "btnTurnoFecha"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la fecha a partir de la cual se aplicarán los turnos del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "txtDiasLibres"
                ft.mensajeEtiqueta(lblInfo, " indique la cantidad de días libres en el período-nómina del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "btnDiaLibreFecha"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la fecha a partir de la cual se aplicará el ó los días libres del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "cmbPeriodoDiaLibre"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el período-nómina en el cual se aplicará el ó los días libres del trabajador ...", Transportables.tipoMensaje.iInfo)
        End Select

    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick,
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
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
            If Not qFound(myConn, lblInfo, "jsnomturtra", aFldH, aStrH) Then ft.Ejecutar_strSQL(myConn, "insert into jsnomturtra set codtra = '" & txtCodigo.Text & "', codtur = '" & g.Seleccion & "', id_emp = '" & jytsistema.WorkID & "' ")
        End If

        'AsignaTXT(nPosicionCat)
        If g.Apuntador >= 0 Then AsignaTur(g.Apuntador, True)

        g = Nothing
        dtTurH.Dispose()
        dtTurH = Nothing

    End Sub

    Private Sub btnEliminaTurno_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaTurno.Click


    End Sub

    Private Sub dgTar_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgTurnos.RowHeaderMouseClick,
        dgTurnos.CellMouseClick, dgTurnos.RegionChanged
        Me.BindingContext(ds, nTableTurnos).Position = e.RowIndex
    End Sub

    Private Sub txtFechaNacimiento_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFechaNacimiento.ValueChanged
        txtEdad.Text = CalculaDiferenciaFechas(CDate(txtFechaNacimiento.Text), jytsistema.sFechadeTrabajo, DiferenciaFechas.iAñosMesesDias)
    End Sub

    Private Sub txtIngreso_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIngreso.ValueChanged
        txtAntiguedad.Text = CalculaDiferenciaFechas(CDate(txtIngreso.Text), jytsistema.sFechadeTrabajo, DiferenciaFechas.iAñosMesesDias)
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        txtCodigo1.Text = txtCodigo.Text
        txtCodigo2.Text = txtCodigo.Text
        txtCodigo3.Text = txtCodigo.Text
        txtCodigo4.Text = txtCodigo.Text
        txtCodigoEquipamiento.Text = txtCodigo.Text
    End Sub

    Private Sub txtNombre_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        txtNombre.TextChanged, txtApellido.TextChanged
        txtNombre1.Text = txtNombre.Text + ", " + txtApellido.Text
        txtNombre2.Text = txtNombre.Text + ", " + txtApellido.Text
        txtNombre3.Text = txtNombre.Text + ", " + txtApellido.Text
        txtNombre4.Text = txtNombre.Text + ", " + txtApellido.Text
        txtNombreEquipamiento.Text = txtNombre.Text + ", " + txtApellido.Text
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


    Private Sub btnG1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnG1.Click,
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
        If txtCodigoCargo.Text <> "" Then txtDescripcionCargo.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsnomestcar where codigo = " & txtCodigoCargo.Text.Split(".")(UBound(txtCodigoCargo.Text.Split("."))) & " ")
    End Sub


    Private Sub txtBanco_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBanco.TextChanged
        If CBool(ParametroPlus(myConn, Gestion.iRecursosHumanos, "NOMPARAM02")) Then
            txtNombreBanco.Text = ft.DevuelveScalarCadena(myConn, " select NOMBAN from jsbancatban where codban = '" & txtBanco.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Else
            Dim afld() As String = {"codigo", "modulo", "id_emp"}
            Dim aStr() As String = {txtBanco.Text, FormatoTablaSimple(Modulo.iBancos), jytsistema.WorkID}
            txtNombreBanco.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", afld, aStr, "descrip")
        End If
    End Sub

    Private Sub btnBanco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBanco.Click
        If Not CBool(ParametroPlus(myConn, Gestion.iRecursosHumanos, "NOMPARAM02")) Then
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
                e.Value = ft.FormatoFecha(CDate(e.Value.ToString))
            Case "causa"
                e.Value = aCausaExpedienteNomina(e.Value)
        End Select
    End Sub

    Private Sub dgExpediente_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgExpediente.RowHeaderMouseClick,
        dgExpediente.CellMouseClick, dgExpediente.RegionChanged
        nPosicionExp = e.RowIndex
        Me.BindingContext(ds, nTableExp).Position = nPosicionExp
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTableExp).Rows.Count)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dgPrestramos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgPrestamos.RowHeaderMouseClick,
        dgPrestamos.CellMouseClick, dgPrestamos.RegionChanged

        nPosicionPre = e.RowIndex
        Me.BindingContext(ds, nTablePre).Position = nPosicionPre
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablePre).Rows.Count)
    End Sub

    Private Sub dgAsistencias_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgAsistencias.RowHeaderMouseClick,
          dgAsistencias.CellMouseClick, dgAsistencias.RegionChanged
        Me.BindingContext(ds, nTableAsi).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTableAsi).Rows.Count)
    End Sub

    Private Sub dgAsistencias_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgAsistencias.CellContentClick

    End Sub

    Private Sub dgAsistencias_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgAsistencias.CellFormatting
        Select Case dgAsistencias.Columns(e.ColumnIndex).Name
            Case "dia"
                e.Value = ft.FormatoFecha(CDate(e.Value.ToString))
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
        If CBool(ParametroPlus(myConn, Gestion.iRecursosHumanos, "NOMPARAM02")) Then
            Select Case cmbFormapago.SelectedIndex
                Case 2 'DEPOSITO
                    txtBanco.Text = ParametroPlus(myConn, Gestion.iRecursosHumanos, "NOMPARAM03")
                    txtBanco1.Text = ParametroPlus(myConn, Gestion.iRecursosHumanos, "NOMPARAM04")
                Case Else
                    txtBanco.Text = ""
                    txtBanco1.Text = ""
            End Select
        End If
    End Sub

    Private Sub txtFechaNacimiento_TextChanged(sender As Object, e As Events.DateTimeValueChangedEventArgs) Handles txtFechaNacimiento.ValueChanged

    End Sub

    Private Sub txtBanco1_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBanco1.TextChanged
        Dim afld() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtBanco1.Text, FormatoTablaSimple(Modulo.iBancos), jytsistema.WorkID}
        txtNombreBanco1.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", afld, aStr, "descrip")
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnBancoDeposito.Click
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
    Private Sub txtSueldo_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtSueldo.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
   
    Private Sub dgPrestamos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgPrestamos.RowHeaderMouseClick, _
        dgPrestamos.CellMouseClick, dgPrestamos.RegionChanged
        Me.BindingContext(ds, nTablePre).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablePre).Rows.Count)
    End Sub
End Class