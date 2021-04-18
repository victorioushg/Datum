Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Public Class jsPOSArcCajasMovimientos

    Private Const sModulo As String = "Movimiento cajas ó puntos de venta "
    Private Const nTabla As String = "cajaspos"
    Private Const nTablaPer As String = "tblPerfilesCajas"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private dtRenglon As DataTable

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private aPuerto() As String = {"COM1", "COM2", "COM3", "COM4", "COM5", "COM6"}
    Private strSQLPer As String = ""
    Private nPosRen As Integer = 0
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

        txtCodigo.Text = AutoCodigo(5, dsLocal, nTabla, "codcaj")
        txtNombre.Text = ""
        txtAlmacen.Text = ""
        txtImpreFiscal.Text = ""
        txtImpreFiscal.Text = ""

        AsignarMovimientos(txtCodigo.Text)

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

        If dtLocal.Rows.Count > 0 And nPosicion >= 0 Then
            With dtLocal.Rows(nPosicion)
                txtCodigo.Text = .Item("codcaj")
                txtNombre.Text = .Item("descrip")
                txtImpreFiscal.Text = .Item("impre_fiscal")

                AsignarMovimientos(.Item("codcaj"))

            End With
        End If

    End Sub

    Private Sub AsignarMovimientos(ByVal CodigoCaja As String)

        strSQLPer = " select a.codcaj, b.codper, b.descrip, b.cr, b.co, b.tarifa_a, b.tarifa_b, b.tarifa_c, " _
                                   & " b.tarifa_d, b.tarifa_e, b.tarifa_f, b.almacen, b.descuento from jsvenpervencaj a " _
                                   & " LEFT JOIN jsvenperven b on (a.codper = b.codper and a.id_emp = b.id_emp) " _
                                   & " where " _
                                   & " a.codcaj = '" & CodigoCaja & "' and " _
                                   & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codper "

        dsLocal = DataSetRequery(dsLocal, strSQLPer, MyConn, nTablaPer, lblInfo)
        dtRenglon = dsLocal.Tables(nTablaPer)

        Dim aCam() As String = {"codper", "descrip"}
        Dim aNom() As String = {"Perfil", "Descripción"}
        Dim aAnc() As Long = {70, 150}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", ""}

        IniciarTabla(dgPer, dtRenglon, aCam, aNom, aAnc, aAli, aFor, , , , False)
        If dtRenglon.Rows.Count > 0 Then
            nPosRen = 0
            AsignaMov(nPosRen, True)
        End If


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)
        If Actualiza Then dsLocal = DataSetRequery(dsLocal, strSQLPer, MyConn, nTablaPer, lblInfo)

        If c >= 0 Then
            Me.BindingContext(dsLocal, nTablaPer).Position = c
            dgPer.Refresh()
            dgPer.CurrentCell = dgPer(0, c)
        End If

    End Sub
    Private Sub jsPOSArcCajasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsPOSArcCajasMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HabilitarObjetos(False, True, txtCodigo, txtAlmacen, txtImpreFiscal)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtNombre.GotFocus, txtAlmacen.GotFocus, txtImpreFiscal.GotFocus, _
         btnAlmacen.GotFocus
        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, "Indique el código de caja ...", TipoMensaje.iInfo)
            Case "txtNombre"
                MensajeEtiqueta(lblInfo, "Indique el nombre o descripción caja ó punto de venta ...", TipoMensaje.iInfo)
            Case "txtAlmacen"
                MensajeEtiqueta(lblInfo, "Indique el código de almacén ...", TipoMensaje.iInfo)
            Case "txtMaquinaFiscal"
                MensajeEtiqueta(lblInfo, "Indique el número serial de la impresora fiscal ...", TipoMensaje.iInfo)
            Case "btnAlmacen"
                MensajeEtiqueta(lblInfo, "Seleccione almacén de salida para esta caja ...", TipoMensaje.iInfo)
        End Select

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
            InsertarModificarPOSCaja(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, txtAlmacen.Text, _
                                       0, txtImpreFiscal.text)

            Dim CajaFiscal As String = IIf(chkRegistrar.Checked, txtCodigo.Text, "")
            Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, CajaFiscal)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If i_modo = movimiento.iAgregar Then
            EjecutarSTRSQL(MyConn, lblInfo, " delete from jsvenpervencaj where codcaj = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "'")
        End If
        Me.Close()
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        txtAlmacen.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnImpreFiscal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImpreFiscal.Click
        Dim f As New jsControlArcImpresoraFiscal
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        txtImpreFiscal.Text = f.Seleccionado
        f = Nothing
    End Sub

  
    Private Sub btnAgregaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaPerfil.Click
        Dim f As New jsPOSArcPerfiles
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        If f.Seleccionado <> "" Then
            EjecutarSTRSQL_Scalar(MyConn, lblInfo, " replace into jsvenpervencaj set codcaj = '" & txtCodigo.Text & "', codper = '" & f.Seleccionado & "', id_emp = '" & jytsistema.WorkID & "' ")
            dsLocal = DataSetRequery(dsLocal, strSQLPer, MyConn, nTablaPer, lblInfo)
            dtRenglon = dsLocal.Tables(nTablaPer)
            AsignaMov(dtRenglon.Rows.Count - 1, True)
        End If
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub btnEliminaPerfil_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaPerfil.Click
        Dim sRespuesta As Integer
        nPosRen = Me.BindingContext(dsLocal, nTablaPer).Position
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", vbYesNo, "Eliminar registro en " & sModulo & " ...")
        If sRespuesta = vbYes Then
            Dim aCampos() As String = {"codcaj", "codper", "id_emp"}
            Dim aString() As String = {txtCodigo.Text, dtRenglon.Rows(nPosRen).Item("codper"), jytsistema.WorkID}
            nPosRen = EliminarRegistros(MyConn, lblInfo, dsLocal, nTablaPer, "jsvenpervencaj", strSQLPer, aCampos, aString, nPosRen)
        End If
        AsignarTXT(nPosRen)

    End Sub

    Private Sub dgPer_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgPer.RowHeaderMouseClick, _
     dgPer.CellMouseClick
        Me.BindingContext(dsLocal, nTablaPer).Position = e.RowIndex
        nPosRen = e.RowIndex
    End Sub

End Class