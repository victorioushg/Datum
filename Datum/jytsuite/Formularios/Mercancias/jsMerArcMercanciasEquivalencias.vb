Imports MySql.Data.MySqlClient
Public Class jsMerArcMercanciasEquivalencias

    Private Const sModulo As String = "Movimiento equivalencias de mercancías"
    Private Const nTabla As String = "tblequivalencias"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private CodigoMercancia As String
    Private UnidadDeVenta As String
    Private i_modo As Integer
    Private nPosicion As Integer
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
                       ByVal CodigoArticulo As String, ByVal Unidad As String)

        CodigoMercancia = CodigoArticulo
        UnidadDeVenta = Unidad

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        ft.RellenaCombo(aUnidadAbreviada, cmbUnidad)
        txtEquivale.Text = ft.muestraCampoCantidadLarga(0.0)
        chkDivide.Checked = True
        chkEnvase.Checked = False
        txtEnvase.Text = ""

        ft.habilitarObjetos(False, True, txtEnvase)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal CodigoArticulo As String, ByVal Unidad As String)

        CodigoMercancia = CodigoArticulo
        UnidadDeVenta = Unidad

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        With dtLocal.Rows(nPosicion)

            ft.RellenaCombo(aUnidadAbreviada, cmbUnidad, ft.InArray(aUnidadAbreviada, .Item("uvalencia")))
            txtEquivale.Text = ft.muestraCampoCantidadLarga(.Item("equivale"))
            chkDivide.Checked = Convert.ToBoolean(.Item("nDIVIDE"))
            chkEnvase.Checked = Convert.ToBoolean(.Item("ENVASE"))
            txtEnvase.Text = ft.muestraCampoTexto(.Item("CODIGO_ENVASE"))

        End With

        'ft.habilitarObjetos(False, True, cmbUnidad, chkEnvase, txtEnvase, btnEnvase)

        ft.habilitarObjetos(False, True, cmbUnidad, txtEnvase)

    End Sub
    Private Sub jsMerArcMercanciasEquivalencias_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing

    End Sub

    Private Sub jsMerArcMercanciasEquivalencias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, CodigoMercancia + UnidadDeVenta)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorCantidad(txtEquivale.Text) = 0.0 Then
            ft.mensajeAdvertencia("Debe indicar una cantidad válida...")
            ft.enfocarTexto(txtEquivale)
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar AndAlso ft.DevuelveScalarEntero(MyConn, " SELECT count(*) FROM jsmerequmer where codart = '" & CodigoMercancia & "' and unidad = '" & UnidadDeVenta & "' and uvalencia = '" & cmbUnidad.SelectedItem & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            Else
                Insertar = False
                dsLocal = DataSetRequery(dsLocal, " SELECT * FROM jsmerequmer where codart = '" & CodigoMercancia & "' and unidad = '" & UnidadDeVenta & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTabla, lblInfo)
                dtLocal = dsLocal.Tables(nTabla)
                Dim row As DataRow = dtLocal.Select("codart = '" & CodigoMercancia & "' and unidad = '" & UnidadDeVenta & "' and uvalencia = '" & cmbUnidad.SelectedItem & "' and id_emp = '" & jytsistema.WorkID & "' ")(0)
                Apuntador = dtLocal.Rows.IndexOf(row)
            End If

            InsertEditMERCASEquivalencia(MyConn, lblInfo, Insertar, CodigoMercancia, UnidadDeVenta, _
                                         ValorCantidadLarga(txtEquivale.Text), cmbUnidad.SelectedItem, _
                                         IIf(chkDivide.Checked, 1, 0), IIf(chkEnvase.Checked, 1, 0), txtEnvase.Text)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoMercancia + UnidadDeVenta + cmbUnidad.SelectedItem)


            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Sub txtEquivale_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEquivale.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub btnEnvase_Click(sender As Object, e As EventArgs) Handles btnEnvase.Click
        txtEnvase.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, " select codenv codigo, descripcion from jsmercatenv where id_emp = '" & jytsistema.WorkID & "' order by codenv ", "ENVASES", txtEnvase.Text)
    End Sub
End Class