Imports MySql.Data.MySqlClient
Public Class jsNomArcNominasMovimientos
    Private Const sModulo As String = "Movimiento de Trabajador Nomina"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodigoNomina As String
    Private CodigoTrabajador As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, CodNomina As String, CodTrabajador As String)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoNomina = CodNomina
        CodigoTrabajador = CodTrabajador

        ft.habilitarObjetos(False, True, txtCodigo, txtContable, txtNombre)
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal
            txtCodigo.Text = .Rows(nPosicion).Item("codtra").ToString
            txtNombre.Text = .Rows(nPosicion).Item("nombre").ToString
            txtContable.Text = .Rows(nPosicion).Item("codcon")
        End With
    End Sub

    Private Sub jsControlTablaSimpleMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'dsLocal = Nothing
        ft = Nothing
        'dtLocal = Nothing
    End Sub

    Private Sub jsControlTablaSimpleMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtContable.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique o seleccione la cuenta contable ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtContable.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una cuenta contable válida ...")
            ft.enfocarTexto(txtContable)
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
            InsertEditNOMINA_NOMINA_MOVIMIENTOS(MyConn, lblInfo, Insertar, _
                                           CodigoNomina, CodigoTrabajador, txtContable.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnCodigoContable_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoContable.Click
        txtContable.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, " select codcon codigo, descripcion " _
                                                & " from jscotcatcon where " _
                                                & " marca = 0 and " _
                                                & " id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                txtContable.Text)

    End Sub
End Class