Imports MySql.Data.MySqlClient
Public Class jsBanArcBancosTarjetas
    Private Const sModulo As String = "Tarjetas del banco"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private CodigoBanco As String
    Private n_Apuntador As Long

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Codban As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoBanco = Codban
        If dt.Rows.Count = 0 Then Apuntador = -1
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtCodTar.Text = ""
        txtComision.Text = ft.FormatoNumero(0.0)
        txtISLR.Text = ft.FormatoNumero(0.0)
        lblNombreTarjeta.Text = ""
        lblTipo.Text = ""
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal codban As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoBanco = codban
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt
            txtCodTar.Text = .Rows(nPosicion).Item("codtar").ToString
            txtComision.Text = ft.FormatoNumero(CDbl(.Rows(nPosicion).Item("com1").ToString))
            txtISLR.Text = ft.FormatoNumero(CDbl(.Rows(nPosicion).Item("com2").ToString))
        End With
    End Sub

    Private Sub jsBanBancosTarjetas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoBanco & txtCodTar.Text)
        ft = Nothing
    End Sub

    Private Sub jsBanBancosTarjetas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, CodigoBanco & txtCodTar.Text)
        Me.Tag = sModulo
        Me.Text = Me.Text & " " & CodigoBanco
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False
        If Trim(txtCodTar.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de documento válido ...")
            ft.enfocarTexto(txtComision)
            Exit Function
        Else
            Dim aCampos() As String = {"codban", "codtar", "id_emp"}
            Dim aValores() As String = {CodigoBanco, txtCodTar.Text, jytsistema.WorkID}
            If qFound(MyConn, lblInfo, "jsbancatbantar", aCampos, aValores) AndAlso i_modo = movimiento.iAgregar Then
                ft.mensajeAdvertencia("Esté documento ya fué incluido, verifique ...")
                ft.enfocarTexto(txtCodTar)
                Exit Function
            End If
        End If

        If Not ft.isNumeric(txtComision.Text) Then
            ft.mensajeAdvertencia("Debe indicar una  comisión válida ...")
            ft.enfocarTexto(txtComision)
            Exit Function
        End If

        If Not ft.isNumeric(txtISLR.Text) Then
            ft.mensajeAdvertencia("Debe indicar un descuento por ISLR válido ...")
            ft.enfocarTexto(txtISLR)
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Insertar As Boolean = False
        If Validado() Then
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = 0
            End If
            InsertEditBANCOSTarjetaBanco(MyConn, lblInfo, Insertar, CodigoBanco, txtCodTar.Text, _
                                           ValorNumero(txtComision.Text), ValorNumero(txtISLR.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoBanco & " " & txtCodTar.Text)
            Me.Close()
        End If

    End Sub

    Private Sub txtCodTar_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodTar.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de tarjeta válido ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtISLR.Click, _
        txtComision.Click
        Dim objTXT As TextBox = sender
        ft.enfocarTexto(objTXT)
    End Sub

    Private Sub txtISLR_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtISLR.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el porcentaje por ISLR  ...", Transportables.TipoMensaje.iInfo)
        ft.enfocarTexto(txtISLR)
    End Sub

    Private Sub txtComision_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComision.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el porcentaje por comisión ...", Transportables.TipoMensaje.iInfo)
    End Sub


    Private Sub txtCodTar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodTar.TextChanged
        Dim aVar() As String = {"Crédito", "Débito"}
        Dim aCam() As String = {"codtar", "id_emp"}
        Dim aStr() As String = {txtCodTar.Text, jytsistema.WorkID}
        lblNombreTarjeta.Text = qFoundAndSign(MyConn, lblInfo, "jsconctatar", aCam, aStr, "nomtar")
        lblTipo.Text = aVar(CInt(qFoundAndSign(MyConn, lblInfo, "jsconctatar", aCam, aStr, "tipo")))
    End Sub

    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngreso.Click
        Dim f As New jsControlArcTarjetas
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        txtCodTar.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub txtComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtComision.KeyPress, _
    txtISLR.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class