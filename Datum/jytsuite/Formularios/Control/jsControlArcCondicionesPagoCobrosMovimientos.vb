Imports MySql.Data.MySqlClient
Public Class jsControlArcCondicionesPagoCobrosMovimientos

    Private Const sModulo As String = "Movimiento Condiciones de pagos y/o cobros"
    Private Const nTabla As String = "tblcondicpag"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private aTipo() As String = {"FACTURA", "GIRO"}

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
        txtCodigo.Text = ft.autoCodigo(MyConn, "CODFOR", "jsconctafor", "id_emp", jytsistema.WorkID, 2)
        ft.RellenaCombo(aTipo, cmbTipo)
        ft.iniciarTextoObjetos(FormatoItemListView.iEntero, txtPeriodo, txtGiros)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtInteres, txtLimiteCredito)
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
            txtCodigo.Text = .Item("codfor")
            ft.RellenaCombo(aTipo, cmbTipo, .Item("tipodoc"))
            txtPeriodo.Text = ft.FormatoEntero(.Item("periodo"))
            txtGiros.Text = ft.FormatoEntero(.Item("giros"))
            txtInteres.Text = ft.FormatoNumero(.Item("interes"))
            txtLimiteCredito.Text = ft.FormatoNumero(.Item("limite"))
        End With
    End Sub
    Private Sub jsControlDivisionesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de forma de pago...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False


        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If

            Dim sDoc As String = ""
            If cmbTipo.Text = "FACTURA" Then
                sDoc = cmbTipo.Text & " A " & txtPeriodo.Text & " DIAS "
                If ValorNumero(txtPeriodo.Text) = 0 Then
                    sDoc = cmbTipo.Text & " CONTADO COD "
                End If
            Else
                sDoc = txtGiros.Text & " " & cmbTipo.Text & "S A " & txtPeriodo.Text & " DIAS Y " & txtInteres.Text & "% POR PERIODO"
            End If

            InsertEditCONTROLFormadePago(MyConn, lblInfo, Insertar, txtCodigo.Text, sDoc, cmbTipo.SelectedIndex, _
                                         ValorEntero(txtGiros.Text), ValorEntero(txtPeriodo.Text), 0, _
                                         ValorNumero(txtInteres.Text), ValorNumero(txtLimiteCredito.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPeriodo.Click, _
        txtPeriodo.GotFocus, txtGiros.GotFocus, txtInteres.GotFocus, txtLimiteCredito.GotFocus, _
        txtGiros.Click, txtInteres.Click, txtLimiteCredito.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub txtPeriodo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPeriodo.KeyPress, _
        txtGiros.KeyPress, txtInteres.KeyPress, txtLimiteCredito.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class