Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsNomArcTrabajadoresMovimientosAsistencia
    Private Const sModulo As String = "Movimientos de asistencia de trabajador"
    Private Const nTabla As String = "tbl_movtraasi"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer

    Private n_Apuntador As Long
    Private CodigoTrabajador As String
    Private aTipoDia() As String = {"Normal", "Libre X Contrato colectivo", "Libre x turno", "Libre x feriado", "Extras"}
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsA As DataSet, ByVal dtA As DataTable, ByVal CodigoWorker As String)
        i_modo = movimiento.iAgregar

        MyConn = MyCon
        ds = ds
        dt = dt
        CodigoTrabajador = CodigoWorker
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtFecha.Value = jytsistema.sFechadeTrabajo
        txtFechaEntrada.Value = jytsistema.sFechadeTrabajo
        txtFechaDescanso.Value = jytsistema.sFechadeTrabajo
        txtFechaRetorno.Value = jytsistema.sFechadeTrabajo
        txtFechaSalida.Value = jytsistema.sFechadeTrabajo

        ft.RellenaCombo(aTipoDia, cmbTipoDia)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsA As DataSet, ByVal dtA As DataTable, ByVal CodigoWorker As String)
        i_modo = movimiento.iEditar

        MyConn = MyCon
        ds = dsA
        dt = dtA
        CodigoTrabajador = CodigoWorker

        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt.Rows(nPosicion)

            txtFecha.Value = .Item("dia")
            txtFechaEntrada.Value = .Item("entrada")
            txtFechaDescanso.Value = .Item("descanso")
            txtFechaRetorno.Value = .Item("retorno")
            txtFechaSalida.Value = .Item("salida")
            ft.RellenaCombo(aTipoDia, cmbTipoDia, .Item("tipo"))

        End With
    End Sub

    Private Sub jsNomArcTrabajadoresMovimientosAsistencia_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsNomArcTrabajadoresMovimientosAsistencia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFechaEntrada.Text)
        Dim dates As SfDateTimeEdit() = {txtFecha, txtFechaEntrada, txtFechaDescanso, txtFechaRetorno, txtFechaSalida}
        SetSizeDateObjects(dates)
        txtFechaEntrada.DateTimePattern = Enums.DateTimePattern.Custom
        txtFechaDescanso.DateTimePattern = Enums.DateTimePattern.Custom
        txtFechaRetorno.DateTimePattern = Enums.DateTimePattern.Custom
        txtFechaSalida.DateTimePattern = Enums.DateTimePattern.Custom

        ft.habilitarObjetos(False, True, mskTotal)
    End Sub

    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        cmbTipoDia.GotFocus
        Select Case sender.name
            Case "txtFechaEntrada", "btnFechaEntrada", "btnFechaDescanso", "btnFechaRetorno", "btnFechaSalida",
                "txtFechaDescanso", "txtFechaRetorno", "txtFechaSalida", "btnFecha"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la fecha para el movimiento ... ", Transportables.tipoMensaje.iInfo)
            Case "cmbTipoDia"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la tipo de día de trabajo ...", Transportables.tipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        For Each msk As Control In grpAsiste.Controls
            If msk.Name.Substring(0, 3) = "msk" Then
                If Not ValidaHoraEnMask(msk.Text) Then
                    ft.mensajeAdvertencia("Debe indicar un hora válida en formato militar (24 horas)...")
                    msk.Focus()
                    Return False
                End If
            End If
        Next


        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dt.Rows.Count
            End If
            InsertEditNOMINAAsistenciaTrabajador(MyConn, lblInfo, Insertar, CodigoTrabajador, CDate(txtFecha.Text),
                                                 txtFechaEntrada.Value, txtFechaSalida.Value, txtFechaDescanso.Value, txtFechaRetorno.Value,
                                                 mskTotal.Text & ":00",
                                                 cmbTipoDia.SelectedIndex)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Function CalculaTotalesDia(ByVal Entrada As String, ByVal Salida As String,
            ByVal Descanso As String, ByVal Retorno As String, ByVal chk As CheckBox) As String

        'CalculaTotalesDia = "00:00"
        'If Entrada <> "" AndAlso Descanso <> "" AndAlso Retorno <> "" AndAlso Salida <> "" Then
        '    Entrada = Replace(Entrada, " ", "0").PadRight(5, "0"c)
        '    Salida = Replace(Salida, " ", "0").PadRight(5, "0"c)
        '    Descanso = Replace(Descanso, " ", "0").PadRight(5, "0"c)
        '    Retorno = Replace(Retorno, " ", "0").PadRight(5, "0"c)

        '    If chk.Checked Then
        '        Dim iMinutosTrabajo As Integer
        '        If Salida >= Entrada Then
        '            iMinutosTrabajo = MinutosEntreFechas(MismaFecha(CDate(Entrada)), MismaFecha(CDate(Salida)))
        '        Else
        '            iMinutosTrabajo = MinutosEntreFechas(MismaFecha(CDate(Entrada)), DateAdd("d", 1, MismaFecha(CDate(Salida))))
        '        End If

        '        Dim iMinutosDescanso As Integer
        '        If Retorno >= Descanso Then
        '            iMinutosDescanso = MinutosEntreFechas(MismaFecha(CDate(Descanso)), MismaFecha(CDate(Retorno)))
        '        Else
        '            iMinutosDescanso = MinutosEntreFechas(MismaFecha(CDate(Descanso)), DateAdd("d", 1, MismaFecha(CDate(Retorno))))
        '        End If

        '        Dim iMinutosTrabajoReal As Integer
        '        iMinutosTrabajoReal = iMinutosTrabajo - iMinutosDescanso
        '        CalculaTotalesDia = Minutos_A_Horas(iMinutosTrabajoReal)
        '    End If
        'End If

    End Function

    'Private Sub ValidarHora(Sender As Object)

    '    Dim SenderSinEspacios As String = Replace(Sender.text, " ", "")
    '    Dim EsSeparador As Boolean = IIf(SenderSinEspacios.Substring(SenderSinEspacios.Length() - 1, 1) = ":", True, False)
    '    Dim hora As String = Replace(Sender.text, " ", "0").PadRight(5, "0"c)
    '    If ValidaHoraEnMask(hora) Then
    '        If SenderSinEspacios.Length > 1 Then
    '            Sender.text = Microsoft.VisualBasic.Left(SenderSinEspacios, SenderSinEspacios.Length() - IIf(EsSeparador, 2, 1))
    '        End If
    '    End If


    'End Sub
    'Private Sub mskEntrada_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles 
    '    mskDescanso.TextChanged
    '    Dim chk As New CheckBox
    '    chk.Checked = True

    '    'ValidarHora(sender)

    '    If ValidaHoraEnMask(sender.text) Then _
    '        mskTotal.Text = CalculaTotalesDia(mskEntrada.Text, mskSalida.Text, mskDescanso.Text, mskRetorno.Text, chk)

    'End Sub

End Class