Imports MySql.Data.MySqlClient
Public Class jsContabArcAsientosMovimientos
    Private Const sModulo As String = "Movimiento asiento contable"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private CodigoAsiento As String
    Private n_Apuntador As Long
    Private numRenglon As String = ""
    Private Diferido_Actual As Integer
    Private Resto As Double
    Private CodigoReferencia As String = ""
    Private ReglaOrigen As String = ""
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Asiento As String, _
                       ByVal DiferidoActual As Integer, ByVal MontoResto As Double, ByVal CodReferencia As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoAsiento = Asiento
        Diferido_Actual = DiferidoActual
        CodigoReferencia = CodReferencia
        Resto = -1 * MontoResto
        If dt.Rows.Count = 0 Then Apuntador = -1
        IniciarTXT()
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, CodigoAsiento)
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        numRenglon = ft.autoCodigo(MyConn, "renglon", "jscotrenasi", "asiento.actual.ejercicio.id_emp", _
                                CodigoAsiento + "." + Convert.ToString(Diferido_Actual) + "." + jytsistema.WorkExercise + "." + jytsistema.WorkID, 5)
        txtCodigoCuenta.Enabled = True
        txtCodigoCuenta.Text = CodigoReferencia
        txtReferencia.Text = ""
        txtConcepto.Text = ""
        txtImporte.Text = ft.FormatoNumero(Resto)
        ReglaOrigen = ""

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Asiento As String, _
                      ByVal DiferidoActual As Integer)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoAsiento = Asiento
        Diferido_Actual = DiferidoActual
        AsignarTXT(Apuntador)
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoAsiento)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt
            txtCodigoCuenta.Text = .Rows(nPosicion).Item("codcon").ToString
            txtReferencia.Text = IIf(IsDBNull(.Rows(nPosicion).Item("Referencia")), "", .Rows(nPosicion).Item("Referencia"))
            txtConcepto.Text = IIf(IsDBNull(.Rows(nPosicion).Item("concepto")), "", .Rows(nPosicion).Item("concepto"))
            txtImporte.Text = ft.FormatoNumero(.Rows(nPosicion).Item("importe"))
            numRenglon = .Rows(nPosicion).Item("renglon")
            ReglaOrigen = .Rows(nPosicion).Item("regla_origen")
        End With
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then _
            ft.Ejecutar_strSQL(myconn, "delete from jscotrenasi where asiento = '" & txtCodigoCuenta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If txtCodigoCuenta .Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una cuenta válida...")
            Exit Function
        Else
        End If

        If txtConcepto.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un concepto válido...")
            txtConcepto.Focus()
            Exit Function
        End If

        If Not ft.isNumeric(txtImporte.Text) Then
            ft.mensajeAdvertencia("Debe indicar un importe válido...")
            txtImporte.Focus()
            Exit Function
        End If

        Dim aFLd() As String = {"codcon", "id_emp"}
        Dim aSFld() As String = {txtCodigoCuenta.Text, jytsistema.WorkID}
        If Not qFound(MyConn, lblInfo, "jscotcatcon", aFLd, aSFld) Then
            ft.mensajeAdvertencia("Cuenta contable no válida ...")
            btnCuentas.Focus()
            Exit Function
        Else
            If Microsoft.VisualBasic.Right(txtCodigoCuenta.Text, 1) = "." Then
                ft.mensajeAdvertencia("Cuenta contable no válida ...")
                Exit Function
            End If
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Insertar As Boolean = False
        If Validado() Then
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dt.Rows.Count
            End If
            InsertEditCONTABRenglonAsiento(MyConn, lblInfo, Insertar, CodigoAsiento, numRenglon, txtCodigoCuenta.Text, _
                                            txtReferencia.Text, txtConcepto.Text, ValorNumero(txtImporte.Text), _
                                            0, IIf(ValorNumero(txtImporte.Text) > 0, 0, 1), Diferido_Actual, ReglaOrigen)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoAsiento & " " & txtCodigoCuenta.Text)
            Me.Close()
        End If

    End Sub

    Private Sub btnCuentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCuentas.Click

        txtCodigoCuenta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by codigo ", "CUENTAS CONTABLES", txtCodigoCuenta.Text)

    End Sub
    Private Sub txtCodigoCuenta_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigoCuenta.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique o escoja una cuenta contable válida...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtReferencia_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReferencia.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la referencia ó Número de origen contable ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el concepto por el cual se realiza este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.Click, _
        txtReferencia.Click, txtConcepto.Click
        Dim objTXT As TextBox = sender
        ft.enfocarTexto(objTXT)
    End Sub

    Private Sub txtImporte_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el importe monetario de este movimiento ...", Transportables.TipoMensaje.iInfo)
        ft.enfocarTexto(txtImporte)
    End Sub

    Private Sub txtCodigoCuenta_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoCuenta.TextChanged
        Dim aFld() As String = {"codcon", "id_emp"}
        Dim aSFld() As String = {txtCodigoCuenta.Text, jytsistema.WorkID}
        lblCuenta.Text = qFoundAndSign(MyConn, lblInfo, "jscotcatcon", aFld, aSFld, "descripcion").ToString
    End Sub

    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporte.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

   
    Private Sub jsContabArcAsientosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
    End Sub

    Private Sub txtImporte_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtImporte.TextChanged

    End Sub
End Class