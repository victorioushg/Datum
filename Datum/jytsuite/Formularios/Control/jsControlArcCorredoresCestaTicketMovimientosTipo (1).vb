Imports MySql.Data.MySqlClient
Public Class jsControlArcCorredoresCestaTicketMovimientosTipo

    Private Const sModulo As String = "Movimiento Tipo de Cesta Ticket"
    Private Const nTabla As String = "tblTipo"
    Private Const nTablaC As String = "tblTipoCom"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private strSQLCom As String = ""
    Private dtDiasCom As New DataTable

    Private i_modo As Integer
    Private CodigoCorredor As String
    Private nPosicion As Integer
    Private nPosicionCom As Integer
    Private n_Apuntador As Long
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal Corredor As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCorredor = Corredor
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        IniciarTextoObjetos(FormatoItemListView.iCadena, txtCodigo, txtNombre)
        IniciarTextoObjetos(FormatoItemListView.iNumero, txtComision, txtImpuesto)
        If dtDiasCom.Rows.Count > 0 Then dgCom.Columns.Clear()
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal Corredor As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCorredor = Corredor
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtCodigo.Text = .Item("tipo")
            txtNombre.Text = .Item("descrip")
            txtComision.Text = FormatoNumero(.Item("com_corredor"))
            txtImpuesto.Text = FormatoNumero(.Item("com_cliente"))
        End With
    End Sub
    Private Sub IniciarComisionesTipoTicket(ByVal Corredor As String, ByVal TipoTicket As String)
        strSQLCom = "  select * from jsvencescom where corredor = '" & Corredor & "' and tipo = '" & TipoTicket & "' and id_emp = '" & jytsistema.WorkID & "'  "
        dsLocal = DataSetRequery(dsLocal, strSQLCom, MyConn, nTablaC, lblInfo)
        dtDiasCom = dsLocal.Tables(nTablaC)

        Dim aCam() As String = {"desde", "hasta", "comision"}
        Dim aNom() As String = {"Desde", "Hasta", "Comisión por días"}
        Dim aAnc() As Long = {70, 70, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aFor() As String = {sFormatoEntero, sFormatoEntero, sFormatoNumero}

        IniciarTabla(dgCom, dtDiasCom, aCam, aNom, aAnc, aAli, aFor)
        If dtDiasCom.Rows.Count > 0 Then nPosicionCom = 0
    End Sub
    Private Sub jsControlArcCorredoresCestaTicketMovimientosTipo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlArcCorredoresCestaTicketMovimientosTipo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un nombre válido...")
            txtNombre.Focus()
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
            InsertEditCONTROLCestaTicketTipo(MyConn, lblInfo, Insertar, CodigoCorredor, txtCodigo.Text, txtNombre.Text, _
                                             ValorNumero(txtComision.Text), ValorNumero(txtImpuesto.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComision.Click, txtImpuesto.Click, _
        txtComision.GotFocus, txtImpuesto.GotFocus
        Dim txt As TextBox = sender
        EnfocarTexto(txt)
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        IniciarComisionesTipoTicket(CodigoCorredor, txtCodigo.Text)
    End Sub

    Private Sub txtComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtComision.KeyPress, _
        txtImpuesto.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub btnAgregaTipo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaTipo.Click
        Dim g As New jsControlArcCorredoresCestaTicketMovimientosTipoComisiones
        g.Agregar(MyConn, dsLocal, dtDiasCom, CodigoCorredor, txtCodigo.Text)
        If g.Apuntador >= 0 Then AsignaTipo(g.Apuntador, True)
        g = Nothing
    End Sub

    Private Sub btnEditaTipo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditaTipo.Click
        Dim g As New jsControlArcCorredoresCestaTicketMovimientosTipoComisiones
        g.Editar(MyConn, dsLocal, dtDiasCom, CodigoCorredor, txtCodigo.Text)
        If g.Apuntador >= 0 Then AsignaTipo(g.Apuntador, True)
        g = Nothing
    End Sub

    Private Sub btnEliminaTipo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaTipo.Click
        With dtDiasCom
            If .Rows.Count > 0 Then
                nPosicionCom = Me.BindingContext(dsLocal, nTablaC).Position

                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"corredor", "tipo", "desde", "hasta", "id_emp"}
                Dim aStrDel() As String = {CodigoCorredor, txtCodigo.Text, _
                                           .Rows(nPosicionCom).Item("desde").ToString, .Rows(nPosicionCom).Item("hasta").ToString, jytsistema.WorkID}
                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then

                    AsignaTipo(EliminarRegistros(MyConn, lblInfo, dsLocal, nTablaC, "jsvencescom", _
                                                strSQLCom, aCamDel, aStrDel, nPosicionCom), True)

                End If
            End If
        End With
    End Sub
    Private Sub AsignaTipo(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then dsLocal = DataSetRequery(dsLocal, strSQLCom, MyConn, nTablaC, lblInfo)

        If c >= 0 AndAlso dtDiasCom.Rows.Count > 0 Then
            Me.BindingContext(dsLocal, nTablaC).Position = c
            dgCom.Refresh()
            dgCom.CurrentCell = dgCom(0, c)
        End If

    End Sub
End Class