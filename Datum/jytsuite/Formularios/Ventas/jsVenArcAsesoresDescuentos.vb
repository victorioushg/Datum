Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsVenArcAsesoresDescuentos
    Private Const sModulo As String = "Movimientos Descuentos de Asesores Comerciales"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodVendedor As String = ""
    Private TipVendedor As Integer = 0
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, dss As DataSet, dtt As DataTable, _
                       CodigoVendedor As String, TipoVendedor As Integer)
        i_modo = movimiento.iAgregar

        MyConn = MyCon
        dsLocal = dss
        dtLocal = dtt
        CodVendedor = CodigoVendedor
        TipVendedor = TipoVendedor

        IniciarTXT()


        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ft.autoCodigo(MyConn, "coddes", "jsconcatdes", "codven.tipo.id_emp", CodVendedor + ".0." + jytsistema.WorkID, 5)
        txtDescripcion.Text = ""
        txtPorcentaje.Text = ft.FormatoNumero(0.0)
        txtFechaDesde.Value = jytsistema.sFechadeTrabajo
        txtFechaHasta.Value = UltimoDiaAño(jytsistema.sFechadeTrabajo)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, CodigoVendedor As String, _
                      ByVal TipoVendfedor As Integer)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt

        CodVendedor = CodigoVendedor
        TipVendedor = TipoVendfedor

        AsignarTXT(Apuntador)

        Me.ShowDialog()
    End Sub

    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtCodigo.Text = .Item("coddes")
            txtDescripcion.Text = .Item("descrip")
            txtPorcentaje.Text = ft.FormatoNumero(.Item("pordes"))
            txtFechaDesde.Value = CDate(.Item("inicio").ToString)
            txtFechaHasta.Value = CDate(.Item("FIN").ToString)
        End With
    End Sub

    Private Sub jsVenArcAsesoresDescuentos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsVenArcAsesoresDescuentos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)
        ft.habilitarObjetos(True, True, txtCodigo, txtFechaDesde, txtFechaHasta)
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtPorcentaje_GotFocus(sender As Object, e As System.EventArgs) Handles txtPorcentaje.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique porcentaje descuento a otorgar ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False



        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válida ...")
            ft.enfocarTexto(txtDescripcion)
            Exit Function
        End If

        If ValorNumero(txtPorcentaje.Text) <= 0 Or _
            ValorNumero(txtPorcentaje.Text) > 100 Then
            ft.mensajeAdvertencia("Debe indicar un porcentaje de descuento válido ...")
            ft.enfocarTexto(txtPorcentaje)
            Exit Function
        End If

        If CDate(txtFechaDesde.Text) > CDate(txtFechaHasta.Text) Then
            ft.mensajeAdvertencia("FECHA DE INICIO MAYOR A LA FECHA FINAL")
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
            InsertEditVENTASDescuentosAsesores(MyConn, lblInfo, Insertar, txtCodigo.Text, txtDescripcion.Text, CDbl(txtPorcentaje.Text), _
                                                CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text), CodVendedor, TipVendedor)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub txtPorcentaje_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtPorcentaje.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

End Class