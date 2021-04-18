Imports MySql.Data.MySqlClient
Public Class jsControlArcRetencionesOSLRMovimiento
    Private Const sModulo As String = "Movimiento retención Impuesto Sobre La Renta"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private n_Apuntador As Long

    Dim aAcumula() As String = {"NO", "SI"}
    Dim aPersona() As String = {"PNR", "PNNR", "PJD", "PJND", "PJNCD"}
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable)
        i_modo = movimiento.iAgregar
        MyConn = Mycon
        ds = dsMov
        dt = dtMov

        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()
      
        txtCodigo.Text = ft.autoCodigo(MyConn, "codret", "jscontabret", "id_emp", jytsistema.WorkID, 3)
        txtNombre.Text = ""
        txtBase.Text = ft.FormatoNumero(100.0)
        txtTarifa.Text = ft.FormatoNumero(0.0)
        txtMinimo.Text = ft.FormatoNumero(0.0)
        txtMenos.Text = ft.FormatoNumero(0.0)
        ft.RellenaCombo(aPersona, cmbTipo)
        ft.RellenaCombo(aAcumula, cmbAcumula)
        txtComentario.Text = ""

    End Sub
    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        AsignarTXT(Apuntador)

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt.Rows(nPosicion)
            txtCodigo.Text = ft.muestraCampoTexto(.Item("codret"))
            txtNombre.Text = ft.muestraCampoTexto(.Item("concepto"))
            ft.RellenaCombo(aPersona, cmbTipo, .Item("tipo"))

            txtBase.Text = ft.FormatoNumero(.Item("baseimp"))
            txtTarifa.Text = ft.FormatoNumero(.Item("tarifa"))
            txtMinimo.Text = ft.FormatoNumero(.Item("pagomin"))
            txtMenos.Text = ft.FormatoNumero(.Item("menos"))

            ft.RellenaCombo(aAcumula, cmbAcumula, .Item("acumula"))
            txtComentario.Text = ft.muestraCampoTexto(.Item("comentario"))

        End With
    End Sub

    Private Sub jsControlArcRetencionesISLRMovimiento_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
    End Sub

    Private Sub jsControlArcRetencionesISLRMovimiento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        ft.habilitarObjetos(False, True, txtCodigo)
        Me.Tag = sModulo

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre para esta retención ...")
            ft.enfocarTexto(txtNombre)
            Exit Function
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
            InsertEditCONTROLRetencionISLR(MyConn, lblInfo, Insertar, _
                                           txtCodigo.Text, txtNombre.Text, cmbTipo.SelectedIndex, ValorNumero(txtBase.Text), _
                                           ValorNumero(txtTarifa.Text), ValorNumero(txtMinimo.Text), ValorNumero(txtMenos.Text), _
                                           cmbAcumula.SelectedIndex, txtComentario.Text)

            InsertarAuditoria(MyConn, IIf(i_modo = movimiento.iAgregar, MovAud.iIncluir, MovAud.imodificar), sModulo, txtCodigo.Text)
            Me.Close()

        End If

    End Sub

    Private Sub txtDocumento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        Select Case sender.name
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, "Indique nombre ó descripción de la retención ...", Transportables.TipoMensaje.iInfo)
            Case "txtBase"
                ft.mensajeEtiqueta(lblInfo, "Indique porcentaje sobre la base imponible sobre la que se calcula la retención ...", Transportables.TipoMensaje.iInfo)
            Case "txtTarifa"
                ft.mensajeEtiqueta(lblInfo, "Indique porcentaje de retención  ...", Transportables.TipoMensaje.iInfo)
            Case "txtMinimo"
                ft.mensajeEtiqueta(lblInfo, "Indique monto mínimo de la base sobre la cual se realiza la retención ...", Transportables.TipoMensaje.iInfo)
            Case "txtMenos"
                ft.mensajeEtiqueta(lblInfo, "Indique el sustraendo que se aplica al monto retenido ...", Transportables.TipoMensaje.iInfo)
            Case "txtComentario"
                ft.mensajeEtiqueta(lblInfo, "Indique comentario si desea para esta retención ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub
    Private Sub txtBase_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBase.KeyPress, _
        txtTarifa.KeyPress, txtMinimo.KeyPress, txtMenos.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtBase_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBase.TextChanged

    End Sub
End Class