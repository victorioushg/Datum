Imports MySql.Data.MySqlClient
Public Class jsControlArcConjuntosMovimientos

    Private Const sModulo As String = "Movimiento conjunto"
    'Private Const nTabla As String = "tblconj"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private strSQLLocalR As String = ""
    Private nTablaR As String = "tblLocalRenglon"
    Private dtLocalR As DataTable

    Private i_modo As Integer
    Private nPosicion As Integer
    Private nPosicionR As Integer
    Private n_Apuntador As Long
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

        txtCodigo.Text = AutoCodigo(5, dsLocal, dtLocal.TableName, "codigo")
        txtNombre.Text = ""
        RellenaCombo(aGestion, cmbGestion)
        IniciarMovimiento(txtCodigo.Text)

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
            txtCodigo.Text = .Item("codigo")
            txtNombre.Text = .Item("descrip")
            RellenaCombo(aGestion, cmbGestion, .Item("Gestion") - 1)
            IniciarMovimiento(.Item("codigo"))
        End With
    End Sub
    Private Sub IniciarMovimiento(ByVal Codigo As String)

        strSQLLocalR = " select * from jsconcojtab " _
                           & " where codigo = '" & Codigo & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' order by letra "

        dsLocal = DataSetRequery(dsLocal, strSQLLocalR, MyConn, nTablaR, lblInfo)
        dtLocalR = dsLocal.Tables(nTablaR)

        Dim aCam() As String = {"letra", "tabla", "tipo", "relacion"}
        Dim aNom() As String = {"Letra", "Nombre de Tabla", "Tipo relación", "Relación"}
        Dim aAnc() As Long = {40, 150, 100, 150}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", "", ""}

        IniciarTabla(dg, dtLocalR, aCam, aNom, aAnc, aAli, aFor)
        If dtLocalR.Rows.Count > 0 Then nPosicionR = 0

    End Sub

    Private Sub jsControlArcConjuntosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlArcConjuntosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el código de conjunto ...", TipoMensaje.iInfo)
    End Sub
    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el descripción de conjunto ...", TipoMensaje.iInfo)
        txtNombre.MaxLength = 50
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
            InsertEditCONTROLEncabezadoConjunto(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, "", "", cmbGestion.SelectedIndex + 1)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dg.CellFormatting

        Select Case dg.Columns(e.ColumnIndex).Name
            Case "tipo"
                Dim aTipo() As String = {"", "LEFT JOIN", "INNER JOIN", "RIGHT JOIN"}
                e.Value = aTipo(e.Value)
        End Select
    End Sub

    Private Sub btnAgregaRenglon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaRenglon.Click
        Dim f As New jsControlArcConjuntosMovimientosR
        f.Agregar(MyConn, dsLocal, dtLocalR, txtCodigo.Text, cmbGestion.SelectedIndex + 1)
        f.Dispose()
        f = Nothing
        IniciarMovimiento(txtCodigo.Text)
    End Sub

    Private Sub btnEditarRenglon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarRenglon.Click
        Dim f As New jsControlArcConjuntosMovimientosR
        f.Apuntador = nPosicionR
        f.Editar(MyConn, dsLocal, dtLocalR, txtCodigo.Text, cmbGestion.SelectedIndex + 1)
        'IniciarMovimiento(txtC_.Text)
        AsignaRenglon(f.Apuntador, True)
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
         dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(dsLocal, nTablaR).Position = e.RowIndex
        nPosicionR = e.RowIndex
        AsignaRenglon(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        nPosicionR = Me.BindingContext(dsLocal, nTablaR).Position
    End Sub
    Private Sub AsignaRenglon(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then dsLocal = DataSetRequery(dsLocal, strSQLLocalR, MyConn, nTablaR, lblInfo)
        If c >= 0 AndAlso dsLocal.Tables(nTablaR).Rows.Count > 0 Then
            Me.BindingContext(dsLocal, nTablaR).Position = c
            dg.CurrentCell = dg(0, c)
        End If
    End Sub
End Class