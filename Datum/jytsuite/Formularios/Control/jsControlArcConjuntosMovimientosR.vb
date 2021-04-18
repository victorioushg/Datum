Imports MySql.Data.MySqlClient
Public Class jsControlArcConjuntosMovimientosR

    Private Const sModulo As String = "Movimiento renglon de conjunto "
    Private Const nTabla As String = "tblconjuntoR"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private aTipo() As String = {"", "LEFT JOIN", "INNER JOIN", "RIGHT JOIN"}
    Private CodigoConjunto As String
    Private NumGestion As Integer
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal CodConjunto As String, ByVal Gestion As Integer)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoConjunto = CodConjunto
        NumGestion = Gestion

        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtLetra.Text = ""
        txtTabla.Text = ""
        txtRelacion.Text = ""
        ft.RellenaCombo(aTipo, cmbTipo)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal CodConjunto As String, ByVal Gestion As Integer)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoConjunto = CodConjunto
        NumGestion = Gestion

        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtLetra.Text = .Item("letra")
            txtTabla.Text = .Item("tabla")
            txtRelacion.Text = .Item("relacion")
            ft.RellenaCombo(aTipo, cmbTipo, .Item("tipo"))
        End With
    End Sub
    Private Sub jsControlArcConjuntosMovimientosR_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlArcConjuntosMovimientosR_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtLetra.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLetra.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la letra identificadora ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtTabla.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre de tabla válido...")
            txtTabla.Focus()
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
            InsertEditCONTROLRenglonesConjunto(MyConn, lblInfo, Insertar, CodigoConjunto, txtLetra.Text, txtTabla.Text, cmbTipo.SelectedIndex, txtRelacion.Text, NumGestion)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtLetra.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

End Class