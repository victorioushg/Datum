Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Public Class jsNomArcTurnosHorarios
    Private Const sModulo As String = "Catálogos de turnos y horarios"
    Private Const nTabla As String = "tblTurnosHorarios"
    Private Const lRegion As String = "RibbonButton38"

    Private strSQL As String = " select * from jsnomcattur " _
            & " where id_emp = '" & jytsistema.WorkID & "' order by codtur "

    Private MyConn As MySqlConnection
    Private ds As New DataSet()
    Private dt As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Long
    Public Sub Cargar(ByVal MyConnection As MySqlConnection, ByVal TipoCarga As Integer)

        Me.Dock = DockStyle.Fill
        MyConn = MyConnection
        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        ft.RellenaCombo(aTipoNomina, cmbTipo)

        DesactivarMarco0()
        If dt.Rows.Count > 0 Then
            nPosicion = 0
            AsignaTXT(nPosicion)
        Else
            IniciarTurno(False)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
        AsignarTooltips()

        Me.Show()

    End Sub

    Private Sub jsNomArcTurnosHorarios_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsNomArcTurnosHorarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo turno/horario ")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> turno/horario actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> turno/horario actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> turno/horario deseada")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> turno/horario")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> turno/horario")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir <B>anterior</B> turno/horario")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último turno/horario</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> turno/horario")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)

                txtCodigo.Text = .Item("codtur")
                txtNombre.Text = IIf(IsDBNull(.Item("nombre")), "", .Item("nombre"))
                ft.RellenaCombo(aTipoNomina, cmbTipo, .Item("tipo"))
                mskHorasExDia.Text = ft.FormatoHoraCorta(CDate(.Item("horadiurna").ToString))
                mskHorasExNoche.Text = ft.FormatoHoraCorta(CDate(.Item("horanocturna").ToString))
                txtMarcasTurno.Text = ft.FormatoEntero(.Item("marcaturno"))
                txtMarcasDescanso.Text = ft.FormatoEntero(.Item("marcadescanso"))
                txtTolEntrada.Text = ft.FormatoEntero(.Item("tol_ent"))
                txtTolSalida.Text = ft.FormatoEntero(.Item("tol_sal"))
                txtTolDescanso.Text = ft.FormatoEntero(.Item("tol_ini_des"))
                txtTolRetorno.Text = ft.FormatoEntero(.Item("tol_fin_des"))

                chkL.Checked = CBool(.Item("L"))
                chkM.Checked = CBool(.Item("M"))
                chkI.Checked = CBool(.Item("I"))
                chkJ.Checked = CBool(.Item("J"))
                chkV.Checked = CBool(.Item("V"))
                chkS.Checked = CBool(.Item("S"))
                chkD.Checked = CBool(.Item("D"))

                mskEntradaL.Text = ft.FormatoHoraCorta(CDate(.Item("L_E").ToString))
                mskEntradaM.Text = ft.FormatoHoraCorta(CDate(.Item("M_E").ToString))
                mskEntradaI.Text = ft.FormatoHoraCorta(CDate(.Item("I_E").ToString))
                mskEntradaJ.Text = ft.FormatoHoraCorta(CDate(.Item("J_E").ToString))
                mskEntradaV.Text = ft.FormatoHoraCorta(CDate(.Item("V_E").ToString))
                mskEntradaS.Text = ft.FormatoHoraCorta(CDate(.Item("S_E").ToString))
                mskEntradaD.Text = ft.FormatoHoraCorta(CDate(.Item("D_E").ToString))

                mskSalidaL.Text = ft.FormatoHoraCorta(CDate(.Item("L_S").ToString))
                mskSalidaM.Text = ft.FormatoHoraCorta(CDate(.Item("M_S").ToString))
                mskSalidaI.Text = ft.FormatoHoraCorta(CDate(.Item("I_S").ToString))
                mskSalidaJ.Text = ft.FormatoHoraCorta(CDate(.Item("J_S").ToString))
                mskSalidaV.Text = ft.FormatoHoraCorta(CDate(.Item("V_S").ToString))
                mskSalidaS.Text = ft.FormatoHoraCorta(CDate(.Item("S_S").ToString))
                mskSalidaD.Text = ft.FormatoHoraCorta(CDate(.Item("D_S").ToString))

                mskDescansoL.Text = ft.FormatoHoraCorta(CDate(.Item("L_DE").ToString))
                mskDescansoM.Text = ft.FormatoHoraCorta(CDate(.Item("M_DE").ToString))
                mskDescansoI.Text = ft.FormatoHoraCorta(CDate(.Item("I_DE").ToString))
                mskDescansoJ.Text = ft.FormatoHoraCorta(CDate(.Item("J_DE").ToString))
                mskDescansoV.Text = ft.FormatoHoraCorta(CDate(.Item("V_DE").ToString))
                mskDescansoS.Text = ft.FormatoHoraCorta(CDate(.Item("S_DE").ToString))
                mskDescansoD.Text = ft.FormatoHoraCorta(CDate(.Item("D_DE").ToString))

                mskRetornoL.Text = ft.FormatoHoraCorta(CDate(.Item("L_DS").ToString))
                mskRetornoM.Text = ft.FormatoHoraCorta(CDate(.Item("M_DS").ToString))
                mskRetornoI.Text = ft.FormatoHoraCorta(CDate(.Item("I_DS").ToString))
                mskRetornoJ.Text = ft.FormatoHoraCorta(CDate(.Item("J_DS").ToString))
                mskRetornoV.Text = ft.FormatoHoraCorta(CDate(.Item("V_DS").ToString))
                mskRetornoS.Text = ft.FormatoHoraCorta(CDate(.Item("S_DS").ToString))
                mskRetornoD.Text = ft.FormatoHoraCorta(CDate(.Item("D_DS").ToString))

            End With
        End With
    End Sub
    Private Function CalculaTotalesDia(ByVal Entrada As String, ByVal Salida As String, _
            ByVal Descanso As String, ByVal Retorno As String, ByVal chk As CheckBox) As String

        CalculaTotalesDia = "00:00"
        Entrada = Replace(Entrada, " ", "0").PadRight(5, "0"c)
        Salida = Replace(Salida, " ", "0").PadRight(5, "0"c)
        Descanso = Replace(Descanso, " ", "0").PadRight(5, "0"c)
        Retorno = Replace(Retorno, " ", "0").PadRight(5, "0"c)

        Try

            Dim tE As Date = CType(Entrada, Date)
            Dim tS As Date = CType(Salida, Date)
            Dim tD As Date = CType(Descanso, Date)
            Dim tR As Date = CType(Retorno, Date)

            'MsgBox(t.ToString("HH:mm"))


            If chk.Checked Then
                Dim iMinutosTrabajo As Integer
                If Salida >= Entrada Then
                    iMinutosTrabajo = MinutosEntreFechas(MismaFecha(CDate(Entrada)), MismaFecha(CDate(Salida)))
                Else
                    iMinutosTrabajo = MinutosEntreFechas(MismaFecha(CDate(Entrada)), DateAdd("d", 1, MismaFecha(CDate(Salida))))
                End If

                Dim iMinutosDescanso As Integer
                If Retorno >= Descanso Then
                    iMinutosDescanso = MinutosEntreFechas(MismaFecha(CDate(Descanso)), MismaFecha(CDate(Retorno)))
                Else
                    iMinutosDescanso = MinutosEntreFechas(MismaFecha(CDate(Descanso)), DateAdd("d", 1, MismaFecha(CDate(Retorno))))
                End If

                Dim iMinutosTrabajoReal As Integer
                iMinutosTrabajoReal = iMinutosTrabajo - iMinutosDescanso
                CalculaTotalesDia = Minutos_A_Horas(iMinutosTrabajoReal)
            End If

        Catch ex As Exception

        End Try

    End Function
    Private Sub IniciarTurno(ByVal Inicio As Boolean)
        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(MyConn, "codtur", "jsnomcattur", "id_emp", jytsistema.WorkID, 5, True)
        Else
            txtCodigo.Text = ""
        End If

        txtNombre.Text = ""
        txtTolEntrada.Text = ft.FormatoEntero(0)
        txtTolSalida.Text = ft.FormatoEntero(0)
        txtTolDescanso.Text = ft.FormatoEntero(0)
        txtTolRetorno.Text = ft.FormatoEntero(0)

        txtMarcasTurno.Text = "0"
        txtMarcasDescanso.Text = "0"

        mskHorasExDia.Text = "00:00"
        mskHorasExNoche.Text = "00:00"

        mskEntradaL.Text = "00:00"
        mskEntradaM.Text = "00:00"
        mskEntradaI.Text = "00:00"
        mskEntradaJ.Text = "00:00"
        mskEntradaV.Text = "00:00"
        mskEntradaS.Text = "00:00"
        mskEntradaD.Text = "00:00"

        mskSalidaL.Text = "00:00"
        mskSalidaM.Text = "00:00"
        mskSalidaI.Text = "00:00"
        mskSalidaJ.Text = "00:00"
        mskSalidaV.Text = "00:00"
        mskSalidaS.Text = "00:00"
        mskSalidaD.Text = "00:00"

        mskDescansoL.Text = "00:00"
        mskDescansoM.Text = "00:00"
        mskDescansoI.Text = "00:00"
        mskDescansoJ.Text = "00:00"
        mskDescansoV.Text = "00:00"
        mskDescansoS.Text = "00:00"
        mskDescansoD.Text = "00:00"

        mskRetornoL.Text = "00:00"
        mskRetornoM.Text = "00:00"
        mskRetornoI.Text = "00:00"
        mskRetornoJ.Text = "00:00"
        mskRetornoV.Text = "00:00"
        mskRetornoS.Text = "00:00"
        mskRetornoD.Text = "00:00"



    End Sub
    Private Sub ActivarMarco0()

        ft.visualizarObjetos(True, grpAceptarSalir)
        ft.habilitarObjetos(True, True, txtNombre, cmbTipo, txtMarcasTurno, txtMarcasDescanso, txtTolEntrada, _
                         txtTolSalida, txtTolDescanso, txtTolRetorno, mskHorasExDia, mskHorasExNoche, mskEntradaL, _
                         mskEntradaM, mskEntradaI, mskEntradaJ, mskEntradaV, mskEntradaS, mskEntradaD, _
                         mskSalidaL, mskSalidaM, mskSalidaI, mskSalidaJ, mskSalidaV, mskSalidaS, mskSalidaD, _
                         mskDescansoL, mskDescansoM, mskDescansoI, mskDescansoJ, mskDescansoV, mskDescansoS, mskDescansoD, _
                         mskRetornoL, mskRetornoM, mskRetornoI, mskRetornoJ, mskRetornoV, mskRetornoS, mskRetornoD, _
                         chkL, chkM, chkI, chkJ, chkV, chkS, chkD)

        grpEncab.Enabled = True
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        ft.habilitarObjetos(False, True, txtCodigo, txtNombre, cmbTipo, txtMarcasTurno, txtMarcasDescanso, txtTolEntrada, _
                         txtTolSalida, txtTolDescanso, txtTolRetorno, mskHorasExDia, mskHorasExNoche, mskEntradaL, _
                         mskEntradaM, mskEntradaI, mskEntradaJ, mskEntradaV, mskEntradaS, mskEntradaD, _
                         mskSalidaL, mskSalidaM, mskSalidaI, mskSalidaJ, mskSalidaV, mskSalidaS, mskSalidaD, _
                         mskDescansoL, mskDescansoM, mskDescansoI, mskDescansoJ, mskDescansoV, mskDescansoS, mskDescansoD, _
                         mskRetornoL, mskRetornoM, mskRetornoI, mskRetornoJ, mskRetornoV, mskRetornoS, mskRetornoD, _
                         txtL, txtM, txtI, txtJ, txtV, txtS, txtD, txtT, chkL, chkM, chkI, chkJ, chkV, chkS, chkD)

        ft.habilitarObjetos(True, False, grpEncab, grpHorario)
        ft.visualizarObjetos(False, grpAceptarSalir)

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub GuardarTXT()
        '
        Dim Insertar As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Insertar = True
            nPosicion = dt.Rows.Count
        End If
        InsertEditNOMINATurnoHorario(MyConn, lblInfo, Insertar, _
                                      txtCodigo.Text, txtNombre.Text, cmbTipo.SelectedIndex, _
                                      CDate(mskHorasExDia.Text), CDate(mskHorasExNoche.Text), _
                                      CInt(txtMarcasTurno.Text), CInt(txtMarcasDescanso.Text), CInt(txtTolEntrada.Text), CInt(txtTolSalida.Text), _
                                      CInt(txtTolDescanso.Text), CInt(txtTolRetorno.Text), _
                                      IIf(chkL.Checked, 1, 0), CDate(mskEntradaL.Text), CDate(mskSalidaL.Text), CDate(mskDescansoL.Text), CDate(mskRetornoL.Text), _
                                      IIf(chkM.Checked, 1, 0), CDate(mskEntradaM.Text), CDate(mskSalidaM.Text), CDate(mskDescansoM.Text), CDate(mskRetornoM.Text), _
                                      IIf(chkI.Checked, 1, 0), CDate(mskEntradaI.Text), CDate(mskSalidaI.Text), CDate(mskDescansoI.Text), CDate(mskRetornoI.Text), _
                                      IIf(chkJ.Checked, 1, 0), CDate(mskEntradaJ.Text), CDate(mskSalidaJ.Text), CDate(mskDescansoJ.Text), CDate(mskRetornoJ.Text), _
                                      IIf(chkV.Checked, 1, 0), CDate(mskEntradaV.Text), CDate(mskSalidaV.Text), CDate(mskDescansoV.Text), CDate(mskRetornoV.Text), _
                                      IIf(chkS.Checked, 1, 0), CDate(mskEntradaS.Text), CDate(mskSalidaS.Text), CDate(mskDescansoS.Text), CDate(mskRetornoS.Text), _
                                      IIf(chkD.Checked, 1, 0), CDate(mskEntradaD.Text), CDate(mskSalidaD.Text), CDate(mskDescansoD.Text), CDate(mskRetornoD.Text))

        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicion
        AsignaTXT(nPosicion)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicion = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        Iniciarturno(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        nPosicion = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        '
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        '
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        'Dim f As New jsNomRepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptosXTrabajador, "Trabajadores, conceptos y variaciones", txtCodigo.Text, , , cmbTipo.SelectedIndex)
        'f = Nothing
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado()
        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre válido para este turno/horario...")
            Return False
        End If

        For Each msk As Control In grpEncab.Controls
            If msk.Name.Substring(0, 3) = "msk" Then
                If Not ValidaHoraEnMask(msk.Text) Then
                    ft.mensajeAdvertencia("Debe indicar una hora válida en formato militar (24 horas)...")
                    msk.Focus()
                    Return False
                End If
            End If
        Next

        For Each msk As Control In grpHorario.Controls
            If msk.Name.Substring(0, 3) = "msk" Then
                If Not ValidaHoraEnMask(msk.Text) Then
                    ft.mensajeAdvertencia("Debe indicar un hora válida en formato militar (24 horas)...")
                    msk.Focus()
                    Return False
                End If
            End If
        Next

        Return True

    End Function
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarTurno(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicion)
        End If
        DesactivarMarco0()
    End Sub
    Private Function TotalHoras() As String

        Dim horas As Integer = CInt(If(txtL.Text <> "", Split(txtL.Text, ":")(0), "0")) + _
                               CInt(If(txtM.Text <> "", Split(txtM.Text, ":")(0), "0")) + _
                               CInt(If(txtI.Text <> "", Split(txtI.Text, ":")(0), "0")) + _
                               CInt(If(txtJ.Text <> "", Split(txtJ.Text, ":")(0), "0")) + _
                               CInt(If(txtV.Text <> "", Split(txtV.Text, ":")(0), "0")) + _
                               CInt(If(txtS.Text <> "", Split(txtS.Text, ":")(0), "0")) + _
                               CInt(If(txtD.Text <> "", Split(txtD.Text, ":")(0), "0"))

        Dim Minutos As Integer = CInt(If(txtL.Text <> "", Split(txtL.Text, ":")(1), "0")) + _
                               CInt(If(txtM.Text <> "", Split(txtM.Text, ":")(1), "0")) + _
                               CInt(If(txtI.Text <> "", Split(txtI.Text, ":")(1), "0")) + _
                               CInt(If(txtJ.Text <> "", Split(txtJ.Text, ":")(1), "0")) + _
                               CInt(If(txtV.Text <> "", Split(txtV.Text, ":")(1), "0")) + _
                               CInt(If(txtS.Text <> "", Split(txtS.Text, ":")(1), "0")) + _
                               CInt(If(txtD.Text <> "", Split(txtD.Text, ":")(1), "0"))

        If Minutos > 59 Then
            horas = horas + Fix(Minutos / 60)
            Minutos = Minutos Mod 60
        End If
        TotalHoras = Format(horas, "00") + ":" + Format(Minutos, "00")

    End Function

    Private Sub mskHorasExDia_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        mskHorasExDia.GotFocus, mskHorasExNoche.GotFocus, _
        mskRetornoL.GotFocus, mskRetornoM.GotFocus, mskRetornoI.GotFocus, mskRetornoJ.GotFocus, mskRetornoV.GotFocus, mskRetornoS.GotFocus, mskRetornoD.GotFocus, _
        mskEntradaL.GotFocus, mskEntradaM.GotFocus, mskEntradaI.GotFocus, mskEntradaJ.GotFocus, mskEntradaV.GotFocus, mskEntradaS.GotFocus, mskEntradaD.GotFocus, _
        mskSalidaL.GotFocus, mskSalidaM.GotFocus, mskSalidaI.GotFocus, mskSalidaJ.GotFocus, mskSalidaV.GotFocus, mskSalidaS.GotFocus, mskSalidaD.GotFocus, _
        mskDescansoL.GotFocus, mskDescansoM.GotFocus, mskDescansoI.GotFocus, mskDescansoJ.GotFocus, mskDescansoV.GotFocus, mskDescansoS.GotFocus, mskDescansoD.GotFocus

        ft.mensajeEtiqueta(lblInfo, "Indique una hora válida en formato de 24 horas/día ej. (22:47 = 10:47p.m.) ", Transportables.tipoMensaje.iAyuda)

    End Sub


    Private Sub mskRetornoL_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mskRetornoL.TextChanged, _
        mskRetornoM.TextChanged, mskRetornoI.TextChanged, mskRetornoJ.TextChanged, mskRetornoV.TextChanged, mskRetornoS.TextChanged, _
        mskRetornoD.TextChanged, mskEntradaL.TextChanged, mskEntradaM.TextChanged, mskEntradaI.TextChanged, _
        mskEntradaJ.TextChanged, mskEntradaV.TextChanged, mskEntradaS.TextChanged, mskEntradaD.TextChanged, _
        mskSalidaL.TextChanged, mskSalidaM.TextChanged, mskSalidaI.TextChanged, _
        mskSalidaJ.TextChanged, mskSalidaV.TextChanged, mskSalidaS.TextChanged, mskSalidaD.TextChanged, _
        mskDescansoL.TextChanged, mskDescansoM.TextChanged, mskDescansoI.TextChanged, _
        mskDescansoJ.TextChanged, mskDescansoV.TextChanged, mskDescansoS.TextChanged, mskDescansoD.TextChanged, _
        chkL.CheckedChanged, chkM.CheckedChanged, chkI.CheckedChanged, chkJ.CheckedChanged, chkV.CheckedChanged, chkS.CheckedChanged, chkD.CheckedChanged

        Select Case Microsoft.VisualBasic.Right(sender.name, 1)
            Case "L"
                txtL.Text = CalculaTotalesDia(mskEntradaL.Text, mskSalidaL.Text, mskDescansoL.Text, mskRetornoL.Text, chkL)
            Case "M"
                txtM.Text = CalculaTotalesDia(mskEntradaM.Text, mskSalidaM.Text, mskDescansoM.Text, mskRetornoM.Text, chkM)
            Case "I"
                txtI.Text = CalculaTotalesDia(mskEntradaI.Text, mskSalidaI.Text, mskDescansoI.Text, mskRetornoI.Text, chkI)
            Case "J"
                txtJ.Text = CalculaTotalesDia(mskEntradaJ.Text, mskSalidaJ.Text, mskDescansoJ.Text, mskRetornoJ.Text, chkL)
            Case "V"
                txtV.Text = CalculaTotalesDia(mskEntradaV.Text, mskSalidaV.Text, mskDescansoV.Text, mskRetornoV.Text, chkV)
            Case "S"
                txtS.Text = CalculaTotalesDia(mskEntradaS.Text, mskSalidaS.Text, mskDescansoS.Text, mskRetornoS.Text, chkS)
            Case "D"
                txtD.Text = CalculaTotalesDia(mskEntradaD.Text, mskSalidaD.Text, mskDescansoD.Text, mskRetornoD.Text, chkD)
        End Select
        txtT.Text = TotalHoras()


    End Sub

    Private Sub txtL_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtL.TextChanged, _
        txtM.TextChanged, txtI.TextChanged, txtJ.TextChanged, txtV.TextChanged, txtS.TextChanged, txtD.TextChanged
        txtT.Text = TotalHoras()

    End Sub

    Private Sub txtMarcasTurno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtMarcasTurno.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de marcaciones que el trabajador realiza para entrar y salir del turno (ej. 2)", Transportables.tipoMensaje.iAyuda)
    End Sub
    Private Sub txtMarcasDescanso_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMarcasDescanso.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de marcaciones que el trabajador realiza para entrar y salir del descanso (ej. 2)", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub txtTolEntrada_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTolEntrada.GotFocus, _
        txtTolSalida.GotFocus, txtTolRetorno.GotFocus, txtTolDescanso.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la cantidad de minutos que el trabajador puede adelantar/atrasar antes/despues de la hora (ej. 10) ", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre odescripción del turno (ej. Administración)", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub cmbTipo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTipo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Seleccione el tipo de nómina para el cual aplica este turno...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub txtTolEntrada_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTolEntrada.KeyPress, _
        txtTolSalida.KeyPress, txtTolDescanso.KeyPress, txtTolRetorno.KeyPress, txtMarcasTurno.KeyPress, txtMarcasDescanso.KeyPress
        e.Handled = ValidaNumeroEnteroEnTextbox(e)
    End Sub


    Private Sub mskEntradaL_MaskInputRejected(sender As System.Object, e As System.Windows.Forms.MaskInputRejectedEventArgs) Handles mskEntradaL.MaskInputRejected

    End Sub

    Private Sub mskEntradaL_Leave(sender As Object, e As System.EventArgs) Handles mskEntradaL.Leave, _
        mskHorasExDia.Leave, mskHorasExNoche.Leave, _
        mskRetornoL.Leave, mskRetornoM.Leave, mskRetornoI.Leave, mskRetornoJ.Leave, mskRetornoV.Leave, mskRetornoS.Leave, mskRetornoD.Leave, _
        mskEntradaL.Leave, mskEntradaM.Leave, mskEntradaI.Leave, mskEntradaJ.Leave, mskEntradaV.Leave, mskEntradaS.Leave, mskEntradaD.Leave, _
        mskSalidaL.Leave, mskSalidaM.Leave, mskSalidaI.Leave, mskSalidaJ.Leave, mskSalidaV.Leave, mskSalidaS.Leave, mskSalidaD.Leave, _
        mskDescansoL.Leave, mskDescansoM.Leave, mskDescansoI.Leave, mskDescansoJ.Leave, mskDescansoV.Leave, mskDescansoS.Leave, mskDescansoD.Leave


        Try
            Dim t As Date = CType(sender.text, Date)
            'MsgBox(t.ToString("HH:mm"))

            Select Case Microsoft.VisualBasic.Right(sender.name, 1)
                Case "L"
                    txtL.Text = CalculaTotalesDia(mskEntradaL.Text, mskSalidaL.Text, mskDescansoL.Text, mskRetornoL.Text, chkL)
                Case "M"
                    txtM.Text = CalculaTotalesDia(mskEntradaM.Text, mskSalidaM.Text, mskDescansoM.Text, mskRetornoM.Text, chkM)
                Case "I"
                    txtI.Text = CalculaTotalesDia(mskEntradaI.Text, mskSalidaI.Text, mskDescansoI.Text, mskRetornoI.Text, chkI)
                Case "J"
                    txtJ.Text = CalculaTotalesDia(mskEntradaJ.Text, mskSalidaJ.Text, mskDescansoJ.Text, mskRetornoJ.Text, chkL)
                Case "V"
                    txtV.Text = CalculaTotalesDia(mskEntradaV.Text, mskSalidaV.Text, mskDescansoV.Text, mskRetornoV.Text, chkV)
                Case "S"
                    txtS.Text = CalculaTotalesDia(mskEntradaS.Text, mskSalidaS.Text, mskDescansoS.Text, mskRetornoS.Text, chkS)
                Case "D"
                    txtD.Text = CalculaTotalesDia(mskEntradaD.Text, mskSalidaD.Text, mskDescansoD.Text, mskRetornoD.Text, chkD)
            End Select
            txtT.Text = TotalHoras()

        Catch ex As Exception
            'Was not a proper time.
            ft.MensajeCritico("HORA NO VALIDA....")
            sender.Focus()
        End Try

    End Sub
End Class