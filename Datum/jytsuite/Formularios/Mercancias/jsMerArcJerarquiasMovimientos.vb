Imports MySql.Data.MySqlClient
Public Class jsMerArcJerarquiasMovimientos

    Private Const sModulo As String = "Movimiento renglón de jerarquía"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private TipoJerarquia As String
    Private Nivel As Integer
    Private Mascara As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal nTipoJerarquia As String, ByVal nNivel As Integer, ByVal nMascara As String)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        TipoJerarquia = nTipoJerarquia
        Nivel = nNivel
        Mascara = nMascara

        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal nTipoJerarquia As String, ByVal nNivel As String)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        TipoJerarquia = nTipoJerarquia
        Nivel = nNivel

        Habilitar()
        AsignarTXT(Apuntador)

        Me.ShowDialog()

    End Sub
    Private Sub Habilitar()
        ft.habilitarObjetos(True, True, txtCodigo, txtDescripcion)
        If i_modo = movimiento.iEditar Then _
            ft.habilitarObjetos(False, True, txtCodigo)
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ft.autoCodigo(MyConn, "codjer", "jsmerrenjer", "tipjer.id_emp", TipoJerarquia + "." + jytsistema.WorkID, Mascara.Length)
        txtDescripcion.Text = ""

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = ft.muestraCampoTexto(.Item("codjer"))
            txtDescripcion.Text = ft.muestraCampoTexto(.Item("desjer"))

        End With
    End Sub

    Private Sub jsMerArcJerarquiasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing '
    End Sub

    Private Sub jsMerArcJerarquiasMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el codigo de la jerarquía...", Transportables.TipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Indique una descripción para esta jerarquía... ", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If txtCodigo.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un código de jerarquía válido... ")
            ft.enfocarTexto(txtCodigo)
            Exit Function
        Else
            Dim aFld() As String = {"tipjer", "codjer", "nivel", "id_emp"}
            Dim aStr() As String = {TipoJerarquia, txtCodigo.Text, Nivel, jytsistema.WorkID}
            If i_modo = movimiento.iAgregar AndAlso qFound(MyConn, lblInfo, "jsmerrenjer", aFld, aStr) Then
                ft.mensajeAdvertencia("Código de jerarquía YA existe en el sistema...")
                ft.enfocarTexto(txtCodigo)
                Exit Function
            End If
        End If

        If txtDescripcion.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida para esta jerarquía...")
            ft.enfocarTexto(txtDescripcion)
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

            InsertEditMERCASRenglonJerarquia(MyConn, lblInfo, Insertar, TipoJerarquia, txtCodigo.Text, _
                        txtDescripcion.Text, Nivel, "1")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
        txtCodigo.KeyPress
        e.Handled = ValidaAlfaNumericoEnTextbox(e)
    End Sub

End Class