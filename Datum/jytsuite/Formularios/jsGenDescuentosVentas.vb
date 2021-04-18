Imports MySql.Data.MySqlClient
Public Class jsGenDescuentosVentas

    Private Const sModulo As String = "Descuentos ventas"
    Private Const nTabla As String = "tbldes"

    Private MyConn As New MySqlConnection
    Private dsLocal As New DataSet
    Private dtLocal As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private nomModulo As String = ""
    Private codAsesor As String = ""
    Private tipAsesor As Integer = 0
    Private FechaDescuento As Date = jytsistema.sFechadeTrabajo
    Private MontoNetoDocumento As Double = 0.0
    Private NombreTablaEnBD As String = ""
    Private NombreCampoEnBD As String = ""
    Private numDocumento As String = ""
    Private numRenglon As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal NombreTablaEnBaseDatos As String, ByVal NombreCampoClaveEnBD As String, ByVal NumeroDocumento As String, _
                       ByVal NombreModulo As String, ByVal CodigoAsesor As String, ByVal TipoAsesor As Integer, _
                       ByVal FechDescuento As Date, ByVal NetoDocumento As Double)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        nomModulo = NombreModulo
        codAsesor = CodigoAsesor
        tipAsesor = TipoAsesor
        FechaDescuento = FechDescuento
        MontoNetoDocumento = NetoDocumento
        NombreTablaEnBD = NombreTablaEnBaseDatos
        NombreCampoEnBD = NombreCampoClaveEnBD
        numDocumento = NumeroDocumento

        Me.Text += " " & NombreModulo

        IniciarTXT()

        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        numRenglon = ft.autoCodigo(MyConn, "renglon", NombreTablaEnBD, NombreCampoEnBD + ".id_emp", numDocumento + "." + jytsistema.WorkID, 5)

        IniciarDescuentosAsesorEnCombo(MyConn, lblInfo, cmbDescuentos, codAsesor, tipAsesor, FechaDescuento)
        If cmbDescuentos.Text <> "" Then txtNombre.Text = LTrim(cmbDescuentos.Text.Split("|")(1))

        ft.habilitarObjetos(False, True, txtMontoDescuento, txtDescuento)

    End Sub
    Private Sub jsGenArcDescuentosVentas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsGenArcDescuentosVentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, cmbDescuentos.Text)
    End Sub

    Private Function Validado() As Boolean

        If cmbDescuentos.Items.Count = 0 Then
            ft.mensajeCritico("Asesor NO posee descuentos ó han caducado...")
            Return False
        End If

        Dim aFld() As String = {NombreCampoEnBD, "renglon", "id_emp"}
        Dim aStr() As String = {numDocumento, numRenglon, jytsistema.WorkID}

        If qFound(MyConn, lblInfo, NombreTablaEnBD, aFld, aStr) Then
            ft.mensajeCritico("Este descuento YA ha sido otorgado...")
            Return False
        End If

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre válido...")
            txtNombre.Focus()
            Return False
        End If

        If ValorNumero(txtPorcentaje.Text) <= 0 Or ValorNumero(txtPorcentaje.Text) > 100 Then
            ft.mensajeAdvertencia("Debe indicar un valor de porcentaje válido...")
            ft.enfocarTexto(txtPorcentaje)
            Return False
        Else
            If Not DescuentoAsesorValido(MyConn, lblInfo, cmbDescuentos.Text.Substring(0, 5), codAsesor, FechaDescuento, _
                                         tipAsesor, ValorNumero(txtPorcentaje.Text)) Then
                ft.mensajeAdvertencia("Descuento no válido ó fecha descuento vencida... ")
                ft.enfocarTexto(txtPorcentaje)
                Return False
            End If
        End If

        'ESTIMACION DE PORCENTAJE SI EL MONTO DESCUENTO ES INCLUIDO MANUALMENTE 
        'Dim PorcentajeCalculado As Double = Math.Round(ValorNumero(txtDescuento.Text) / ValorNumero(txtMontoDescuento.Text) * 100, 2)
        ' txtPorcentaje.Text = ft.FormatoNumero(PorcentajeCalculado)
        'If Not DescuentoAsesorValido(MyConn, lblInfo, cmbDescuentos.Text.Substring(0, 5), codAsesor, FechaDescuento, _
        '                               tipAsesor, PorcentajeCalculado) Then
        'ft.mensajeAdvertencia( "Descuento no válido. Verifique por favor ... ")
        'Exit Function
        'End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Dim Insertar As Boolean = False

            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            InsertEditVENTASDescuento(MyConn, lblInfo, Insertar, NombreTablaEnBD, NombreCampoEnBD, numDocumento, _
                                      numRenglon, txtNombre.Text, ValorNumero(txtPorcentaje.Text), ValorNumero(txtDescuento.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, cmbDescuentos.Text.Substring(0, 5))
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtPorcentaje_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPorcentaje.Click, _
        txtPorcentaje.GotFocus
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
        ft.mensajeEtiqueta(lblInfo, "Indique un porcentaje válido para este descuento...", Transportables.tipoMensaje.iAyuda)
    End Sub
    Private Sub txtPorcentaje_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentaje.TextChanged
        txtMontoDescuento.Text = ft.FormatoNumero((1 - ValorNumero(txtPorcentaje.Text) / 100) * MontoNetoDocumento)
        txtDescuento.Text = ft.FormatoNumero(MontoNetoDocumento * ValorNumero(txtPorcentaje.Text) / 100)
    End Sub

    Private Sub txtMontoDescuento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMontoDescuento.GotFocus, _
        txtMontoDescuento.Click
        ft.enfocarTexto(txtMontoDescuento)
        ft.mensajeEtiqueta(lblInfo, "Indique un monto válido de descuento ...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub txtPorcentaje_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPorcentaje.KeyPress, _
        txtMontoDescuento.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub cmbDescuentos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDescuentos.SelectedIndexChanged

        Dim pordes As Double = ft.DevuelveScalarDoble(MyConn, " select pordes from jsconcatdes " _
                                            & " where " _
                                            & " coddes = '" & cmbDescuentos.Text.Substring(0, 4) & "' and " _
                                            & " codven =  '" & codAsesor & "' and " _
                                            & " inicio <= '" & ft.FormatoFechaMySQL(FechaDescuento) & "' AND " _
                                            & " fin >= '" & ft.FormatoFechaMySQL(FechaDescuento) & "' AND " _
                                            & " tipo = " & tipAsesor & " and " _
                                            & " id_emp = '" & jytsistema.WorkID & "' ")

        txtPorcentaje.Text = ft.FormatoPorcentajeLargo(pordes)
        txtNombre.Text = LTrim(cmbDescuentos.Text.Split("|")(1))

    End Sub
End Class