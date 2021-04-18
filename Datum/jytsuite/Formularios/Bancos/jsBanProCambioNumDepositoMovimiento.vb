Imports MySql.Data.MySqlClient
Public Class jsBanProCambioNumDepositoMovimiento
    Private Const sModulo As String = "Movimiento cambio número depósito "

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private n_Apuntador As Long

    Private numDeposito As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal NumeroDeposito As String)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        AsignarTXT(Apuntador)
        numDeposito = NumeroDeposito
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        ft.habilitarObjetos(False, True, txtFecha, txtcodban, txtConcepto, txtImporte)

        With dt
            txtFecha.Text = ft.FormatoFecha(CDate(.Rows(nPosicion).Item("fechamov").ToString))
            txtDocumento.Text = .Rows(nPosicion).Item("numdoc")
            txtConcepto.Text = IIf(IsDBNull(.Rows(nPosicion).Item("concepto")), "", .Rows(nPosicion).Item("concepto"))
            txtImporte.Text = ft.FormatoNumero(Math.Abs(.Rows(nPosicion).Item("importe")))
            txtcodban.Text = IIf(IsDBNull(.Rows(nPosicion).Item("codban")), "", .Rows(nPosicion).Item("codban"))
        End With
    End Sub

    Private Sub jsBanProCambioNumDepositoMovimiento_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)
        ft = Nothing
    End Sub

    Private Sub jsBanProCambioNumDepositoMovimiento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text)
        Me.Tag = sModulo
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtDocumento.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de documento válido ...")
            ft.enfocarTexto(txtConcepto)
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Insertar As Boolean = False
        If Validado() Then

            ft.Ejecutar_strSQL(myconn, " update jsbantraban set numdoc = '" & txtDocumento.Text & "' where numdoc = '" & numDeposito & "' and tipomov = 'DP' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsbantracaj set deposito = '" & txtDocumento.Text & "' where deposito = '" & numDeposito & "' and id_emp = '" & jytsistema.WorkID & "'  ")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtDocumento.Text)
            Me.Close()

        End If

    End Sub

    Private Sub txtDocumento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocumento.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de depósito generado por el banco ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtcodban_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtcodban.TextChanged
        Dim aFld() As String = {"codban", "id_emp"}
        Dim aStr() As String = {txtcodban.Text, jytsistema.WorkID}

        Label2.Text = qFoundAndSign(MyConn, lblInfo, "jsbancatban", aFld, aStr, "nomban") & "   " & _
        qFoundAndSign(MyConn, lblInfo, "jsbancatban", aFld, aStr, "ctaban")

    End Sub
End Class