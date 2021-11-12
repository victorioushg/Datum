Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsNomArcTrabajadoresMovimientosExpediente
    Private Const sModulo As String = "Movimientos de expediente de trabajador"
    Private Const nTabla As String = "tbl_movtraexp"

    Private MyConn As New MySqlConnection
    Private dsExp As DataSet
    Private dtExp As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private lEstatus As Integer = 0

    Private n_Apuntador As Long
    Private Codigo As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal CodigoTrabajador As String)
        i_modo = movimiento.iAgregar

        ft.habilitarObjetos(True, True, txtFecha, txtRetorno)

        MyConn = MyCon
        dsExp = ds
        dtExp = dt
        Codigo = CodigoTrabajador
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtFecha.Value = jytsistema.sFechadeTrabajo
        txtRetorno.Value = jytsistema.sFechadeTrabajo
        txtComentario.Text = ""
        ft.RellenaCombo(aCausaExpedienteNomina, cmbCausa)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal CodigoTrabajador As String)
        i_modo = movimiento.iEditar

        ft.habilitarObjetos(False, True, txtFecha, txtRetorno, cmbCausa)

        MyConn = MyCon
        dsExp = ds
        dtExp = dt
        Codigo = CodigoTrabajador

        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtExp.Rows(nPosicion)
            txtFecha.Value = .Item("fecha")
            txtRetorno.Value = .Item("fecha_fin")
            txtComentario.Text = ft.muestraCampoTexto(.Item("comentario"))
            ft.RellenaCombo(aCausaExpedienteNomina, cmbCausa, .Item("causa"))
            lEstatus = .Item("estatus")
        End With
    End Sub

    Private Sub jsNomArcTrabajadoresMovimientosExpediente_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsNomArcTrabajadoresMovimientosExpediente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text)
        Dim dates As SfDateTimeEdit() = {txtFecha, txtRetorno}
        SetSizeDateObjects(dates)
    End Sub

    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        cmbCausa.GotFocus, txtComentario.GotFocus
        Select Case sender.name
            Case "txtFecha", "btnFecha"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la fecha inicial para el movimiento en expediente ... ", Transportables.tipoMensaje.iInfo)
            Case "txtRetorno", "btnRetorno"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la fecha retorno para el movimiento en expediente ... ", Transportables.tipoMensaje.iInfo)
            Case "txtComentario"
                ft.mensajeEtiqueta(lblInfo, " Indique el comentario por el cual hace registro en el expediente del trabajador ...", Transportables.tipoMensaje.iInfo)
            Case "cmbCausa"
                ft.mensajeEtiqueta(lblInfo, " Seleccione la causa por la cual hace movimiento en expediente ...", Transportables.tipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean

        If Trim(txtComentario.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un comentario ...")
            txtComentario.Focus()
            Return False
        End If

        If CDate(txtRetorno.Text) < CDate(txtFecha.Text) Then
            ft.mensajeCritico("La fecha de retorno no puede ser menor que la fecha inicial...")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtExp.Rows.Count
            End If
            InsertEditNOMINAExpedienteTrabajador(MyConn, lblInfo, Insertar, Codigo, CDate(txtFecha.Text), CDate(txtRetorno.Text),
                                                 txtComentario.Text, cmbCausa.SelectedIndex, lEstatus)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub cmbCausa_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCausa.SelectedIndexChanged
        Select Case cmbCausa.SelectedIndex
            Case 0, 5, 6, 8, 9, 10, 11
                ft.visualizarObjetos(False, lblRetorno, txtRetorno)
            Case Else
                ft.visualizarObjetos(True, lblRetorno, txtRetorno)
        End Select
    End Sub

    Private Sub btnAdjuntos_Click(sender As Object, e As EventArgs) Handles btnAdjuntos.Click
        Dim f As New jsGenTablaArchivos
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog, Codigo + ft.FormatoFechaMySQL(CDate(txtFecha.Text)) + cmbCausa.SelectedIndex.ToString, "NOM", "EXP")
        f.Dispose()
        f = Nothing
    End Sub
End Class