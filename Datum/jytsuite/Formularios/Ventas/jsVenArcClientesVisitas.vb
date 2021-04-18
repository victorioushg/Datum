Imports MySql.Data.MySqlClient
Public Class jsVenArcClientesVisitas
    Private Const sModulo As String = "Movimiento de Horario Visita/Despacho/Pago de cliente"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodigoCliente As String
    Private TipoHorario As Integer

    Private aHorario() As String = {"Visita", "Despacho", "Pago"}

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodigoCliente As String, _
                       ByVal iTipoHorario As Integer)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCliente = iCodigoCliente
        TipoHorario = iTipoHorario

        ft.RellenaCombo(aDias, cmbDia)

        Me.ShowDialog()

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodigoCliente As String, _
                      ByVal iTipoHorario As Integer)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt

        CodigoCliente = iCodigoCliente
        TipoHorario = iTipoHorario

        AsignarTXT(Apuntador)

        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            ft.RellenaCombo(aDias, cmbDia, .Item("dia"))
            mskMañanaDesde.Text = ft.FormatoHoraCorta(CDate(.Item("desde").ToString))
            mskMañanaHasta.Text = ft.FormatoHoraCorta(CDate(.Item("hasta").ToString))
            mskTardeDesde.Text = ft.FormatoHoraCorta(CDate(.Item("desdepm").ToString))
            mskTardeHasta.Text = ft.FormatoHoraCorta(CDate(.Item("hastapm").ToString))
        End With
    End Sub

    Private Sub jsVenArcClientesVisitas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsVenArcClientesVisitas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
        Me.Text = "Movimiento Horario de " & aHorario(TipoHorario)
    End Sub


    Private Function Validado() As Boolean

        If i_modo = movimiento.iAgregar AndAlso ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvencatvis where codcli = '" & CodigoCliente & "' and dia = '" & cmbDia.SelectedIndex & "' and tipo = '" & TipoHorario & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
            ft.mensajeEtiqueta(lblInfo, "Este día YA existe ...", Transportables.TipoMensaje.iInfo)
            cmbDia.Focus()
            Return False
        End If

        If Trim(mskMañanaDesde.Text) = ":" OrElse Trim(mskMañanaHasta.Text) = ":" OrElse _
            Trim(mskTardeDesde.Text) = ":" OrElse Trim(mskTardeHasta.Text) = ":" Then
            ft.mensajeAdvertencia("Debe indicar una hora válida ...")
            Return False
        End If

        If CDate(mskMañanaDesde.Text) >= CDate(mskMañanaHasta.Text) OrElse _
            CDate(mskTardeDesde.Text) >= CDate(mskTardeHasta.Text) Then
            ft.mensajeAdvertencia("Hora DESDE es mayor a la hora HASTA")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            InsertEditVENTASClientesVisitasDespachosPagos(MyConn, lblInfo, Insertar, CodigoCliente, cmbDia.SelectedIndex, _
                                                           mskMañanaDesde.Text, mskMañanaHasta.Text, mskTardeDesde.Text, _
                                                           mskTardeHasta.Text, TipoHorario, "")
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub mskMañanaDesde_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles mskMañanaDesde.GotFocus, _
        mskMañanaHasta.GotFocus, mskTardeDesde.GotFocus, mskTardeHasta.GotFocus

        ft.mensajeEtiqueta(lblInfo, "Indique una hora válida en formato de 24 horas/día ej. (22:47 = 10:47p.m.) ", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub mskMañanaDesde_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles mskMañanaDesde.KeyUp, _
        mskMañanaHasta.KeyUp, mskTardeDesde.KeyUp, mskTardeHasta.KeyUp

        Dim SenderSinEspacios As String = Replace(sender.text, " ", "")
        Dim EsSeparador As Boolean = IIf(SenderSinEspacios.Substring(SenderSinEspacios.Length() - 1, 1) = ":", True, False)
        Dim hora As String = Replace(sender.text, " ", "0").PadRight(5, "0"c)
        If ValidaHoraEnMask(hora) Then sender.text = Microsoft.VisualBasic.Left(SenderSinEspacios, SenderSinEspacios.Length() - IIf(EsSeparador, 2, 1))

    End Sub

End Class