Imports MySql.Data.MySqlClient
Public Class jsVenArcClientesCIs
    Private Const sModulo As String = "Movimiento de Asociados de Cliente"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodigoCliente As String
    Private Expediente As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodCliente As String, ByVal iExpediente As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCliente = iCodCliente
        Expediente = iExpediente

        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()
        txtNacionalidad.Text = "V"
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtCI, txtNombreAsociado)
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodCliente As String, _
                      ByVal iExpediente As String)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt

        CodigoCliente = iCodCliente
        Expediente = iExpediente
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtNacionalidad.Text = ft.MuestraCampoTexto(.Item("nacional"))
            txtCI.Text = ft.MuestraCampoTexto(.Item("ci"))
            txtNombreAsociado.Text = ft.MuestraCampoTexto(.Item("nombre"))
        End With
    End Sub

    Private Sub jsVenArcClientesCIs_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsVenArcClientesCIss_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCI_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCI.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de cédula asociado ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtNombreAsociado_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreAsociado.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre asociado ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtNacionalidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNacionalidad.TextChanged
        ft.mensajeEtiqueta(lblInfo, "Indique la nacionalidad del asociado ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If i_modo = movimiento.iAgregar AndAlso InStr(txtNacionalidad.Text, "V.E.P.") > 0 Then
            ft.mensajeEtiqueta(lblInfo, "Indique nacionalidad válida para este asociado ...", Transportables.TipoMensaje.iInfo)
            ft.enfocarTexto(txtNacionalidad)
            Exit Function
        End If

        If Trim(txtCI.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de cédula válido")
            ft.enfocarTexto(txtCI)
            Exit Function
        End If

        If Trim(txtNombreAsociado.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar o escoger un nombre válido ...")
            ft.enfocarTexto(txtNombreAsociado)
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
            InsertEditVENTASAsociado(MyConn, lblInfo, Insertar, CodigoCliente, txtNacionalidad.Text, txtCI.Text, txtNombreAsociado.Text, "0")
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class