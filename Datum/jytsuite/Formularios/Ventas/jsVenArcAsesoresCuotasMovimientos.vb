Imports MySql.Data.MySqlClient
Public Class jsVenArcAsesoresCuotasMovimientos
    Private Const sModulo As String = "Movimiento comisión cobranza por días "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

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

        IniciarTextoObjetos(FormatoItemListView.iEntero, txtDesde, txtHasta)
        txtporComision.Text = FormatoNumero(0.0)

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

        HabilitarObjetos(False, True, txtDesde, txtHasta)
        With dtLocal.Rows(nPosicion)
            txtDesde.Text = FormatoEntero(.Item("de"))
            txtHasta.Text = FormatoEntero(.Item("a"))
            txtporComision.Text = FormatoNumero(.Item("por_cobranza"))
        End With

    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtDesde_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDesde.GotFocus
        MensajeEtiqueta(lblInfo, "Indique dia desde en el período de cobranza ...", TipoMensaje.iInfo)
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHasta.GotFocus
        MensajeEtiqueta(lblInfo, "Indique día hasta en el período de cobranza ...", TipoMensaje.iInfo)
    End Sub
    Private Sub txtporComision_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtporComision.GotFocus
        MensajeEtiqueta(lblInfo, "Indique montgo de comisión para período de cobranza ...", TipoMensaje.iInfo)
    End Sub
    Private Function Validado() As Boolean

        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select tipjer from jsvencomvencob where codven = '" & CodigoVendedor & "' and de = " & ValorEntero(txtDesde.Text) & " AND tipjer = 'XX' and id_emp = '" & jytsistema.WorkID & "' ").ToString = "XX" And _
            i_modo = movimiento.iAgregar Then
            MensajeCritico(lblInfo, "Limite desde YA se encuentra en la tabla. Verifique por favor ...")
            Return False
        End If

        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select tipjer from jsvencomvencob where codven = '" & CodigoVendedor & "' and a = " & ValorEntero(txtHasta.Text) & " AND tipjer = 'XX' and id_emp = '" & jytsistema.WorkID & "' ").ToString = "XX" And _
            i_modo = movimiento.iAgregar Then
            MensajeCritico(lblInfo, "Limite hasta YA se encuentra en la tabla. Verifique por favor ...")
            Return False
        End If

        If ValorNumero(txtporComision.Text) < 0 Or _
            ValorNumero(txtporComision.Text) > 100 Then
            MensajeCritico(lblInfo, "Valor de comisión no es válido...")
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
            InsertEditVENTASComisionesCobranza(MyConn, lblInfo, Insertar, CodigoVendedor, ValorEntero(txtDesde.Text), ValorEntero(txtHasta.Text), ValorNumero(txtporComision.Text))
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtDesde_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtDesde.KeyPress, _
        txtHasta.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

End Class