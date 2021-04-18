Imports MySql.Data.MySqlClient
Public Class jsControlArcUTMovimientos

    Private Const sModulo As String = "Movimiento Unidad tributaria"
    Private Const nTabla As String = "tblmovUT"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtMonto.Text = ft.FormatoNumero(0.0)
        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        ft.habilitarObjetos(False, True, txtFecha)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        With dtLocal.Rows(nPosicion)

            txtMonto.Text = ft.FormatoNumero(.Item("monto"))
            txtFecha.Text = ft.FormatoFecha(CDate(.Item("fecha").ToString))

        End With

        ft.habilitarObjetos(False, True, txtFecha, btnFecha)

    End Sub
    Private Sub jsControlArcIVAMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlArcIVAMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.habilitarObjetos(False, True, txtFecha)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Dim aFld() As String = {"fecha", "id_emp"}
        Dim aStr() As String = {ft.FormatoFechaMySQL(CDate(txtFecha.Text)), jytsistema.WorkID}

        If qFound(MyConn, lblInfo, "jsconctaunt", aFld, aStr) AndAlso i_modo = movimiento.iAgregar Then
            ft.mensajeAdvertencia("Fecha YA existe. Verifique...")
            Exit Function
        End If

        If ValorNumero(txtMonto.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar un monto válido ...")
            ft.enfocarTexto(txtMonto)
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            InsertEditCONTROLUT(MyConn, lblInfo, Insertar, ValorNumero(txtMonto.Text), CDate(txtFecha.Text))
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Sub txtPorcentaje_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMonto.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, sender)
    End Sub

End Class