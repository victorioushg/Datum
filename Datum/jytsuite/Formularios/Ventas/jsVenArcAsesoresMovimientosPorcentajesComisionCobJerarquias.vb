Imports MySql.Data.MySqlClient
Public Class jsVenArcAsesoresMovimientosPorcentajesComisionCobJerarquias
    Private Const sModulo As String = "Movimiento comisión cobranza por días y jerarquías "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables


    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodigoVendedor As String = ""

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       CodigoAsesor As String)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt

        CodigoVendedor = CodigoAsesor

        Me.Text = sModulo
        IniciarTXT()
        Me.ShowDialog()
    End Sub

    Private Sub IniciarTXT()

        ft.habilitarObjetos(False, True, txtJerarquia)
        ft.iniciarTextoObjetos(FormatoItemListView.iEntero, txtDesde, txtHasta)
        txtporComision.Text = ft.FormatoNumero(0.0)
        txtJerarquia.Text = ""

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      CodigoAsesor As String)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt

        CodigoVendedor = CodigoAsesor

        Me.Text = sModulo
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub

    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        ft.habilitarObjetos(False, True, txtDesde, txtHasta, txtJerarquia, btnJerarquia)
        With dtLocal.Rows(nPosicion)
            txtJerarquia.Text = .Item("tipjer")
            txtDesde.Text = ft.FormatoEntero(.Item("de"))
            txtHasta.Text = ft.FormatoEntero(.Item("a"))
            txtporComision.Text = ft.FormatoNumero(.Item("por_cobranza"))
        End With

    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtDesde_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDesde.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique dia desde en el período de cobranza ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHasta.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique día hasta en el período de cobranza ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtporComision_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtporComision.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique montgo de comisión para período de cobranza ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Function Validado() As Boolean

        If txtJerarquia.Text = "" AndAlso i_modo = movimiento.iAgregar Then
            ft.MensajeCritico("DEBE INDICAR UNA JERARQUIA VALIDA...")
            Return False
        End If

        If ft.DevuelveScalarCadena(MyConn, " select tipjer from jsvencomvencob where " _
                                   & " codven = '" & CodigoVendedor & "' and " _
                                   & " de = " & ValorEntero(txtDesde.Text) & " AND " _
                                   & " tipjer = '" & txtJerarquia.Text & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ").ToString <> "" And _
            i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Limite desde YA se encuentra en la tabla. Verifique por favor ...")
            Return False
        End If

        If ft.DevuelveScalarCadena(MyConn, " select tipjer from jsvencomvencob where " _
                                   & " codven = '" & CodigoVendedor & "' and " _
                                   & " a = " & ValorEntero(txtHasta.Text) & " AND " _
                                   & " tipjer = '" & txtJerarquia.Text & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ").ToString <> "" And _
            i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Limite hasta YA se encuentra en la tabla. Verifique por favor ...")
            Return False
        End If

        If ValorNumero(txtporComision.Text) < 0 Or _
            ValorNumero(txtporComision.Text) > 100 Then
            ft.mensajeCritico("Valor de comisión no es válido...")
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
            InsertEditVENTASComisionesCobranza(MyConn, lblInfo, Insertar, CodigoVendedor, ValorEntero(txtDesde.Text), _
                                               ValorEntero(txtHasta.Text), ValorNumero(txtporComision.Text), txtJerarquia.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtDesde_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtDesde.KeyPress, _
        txtHasta.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtJerarquia_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtJerarquia.TextChanged
        lblJerarquia.Text = ft.DevuelveScalarCadena(MyConn, " select descrip from jsmerencjer where tipjer = '" & txtJerarquia.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub btnJerarquia_Click(sender As System.Object, e As System.EventArgs) Handles btnJerarquia.Click
        txtJerarquia.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, " SELECT a.tipjer codigo, a.descrip descripcion from jsmerencjer a where a.id_emp = '" & jytsistema.WorkID & "' order by a.tipjer ", "LISTADO JERARQUIAS", txtJerarquia.Text)
    End Sub
End Class