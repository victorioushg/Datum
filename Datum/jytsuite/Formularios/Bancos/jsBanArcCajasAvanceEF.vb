Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsBanArcCajasAvanceEF
    Private Const sModulo As String = "Avance de Efectivo"
    Private Const nTabla As String = "tarjetas"
    Private strSQL As String = "select * from jsconctatar where id_emp = '" & jytsistema.WorkID & "' order by codtar"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private dtTar As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private aFormaPag() As String = {"Cheque", "Tarjeta", "Otra"}
    Private aFormaPagR() As String = {"CH", "TA", "OT"}
    Private CodigoCaja As String
    Private n_Apuntador As Long
    Private Renglon As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal CodCaja As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoCaja = CodCaja
        If dt.Rows.Count = 0 Then Apuntador = -1
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtFecha.Value = jytsistema.sFechadeTrabajo
        ft.RellenaCombo(aFormaPag, cmbFormaPago, 0)
        txtDocumento.Text = Contador(MyConn, lblInfo, Gestion.iBancos, "BANNUMAEF", "02")
        txtDocPago.Text = ""
        txtRefPago.Text = ""
        txtImporte.Text = ft.FormatoNumero(0.0)
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal codCaja As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoCaja = codCaja
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt
            txtFecha.Value = .Rows(nPosicion).Item("fecha")
            ft.RellenaCombo(aFormaPag, cmbFormaPago, Array.IndexOf(aFormaPagR, .Rows(nPosicion).Item("formpag")))
            txtDocumento.Text = .Rows(nPosicion).Item("nummov")
            txtDocPago.Text = IIf(IsDBNull(.Rows(nPosicion).Item("numpag")), "", .Rows(nPosicion).Item("numpag"))
            txtRefPago.Text = IIf(IsDBNull(.Rows(nPosicion).Item("refpag")), "", .Rows(nPosicion).Item("refpag"))
            txtImporte.Text = ft.FormatoNumero(Math.Abs(.Rows(nPosicion).Item("importe")))
            Renglon = .Rows(nPosicion).Item("renglon")
        End With
    End Sub

    Private Sub jsBanArcCajasAvanceEF_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dtTar = Nothing
        ft = Nothing
    End Sub

    Private Sub jsBanArcCajasAvanceEF_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, CodigoCaja)
        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        dtTar = ds.Tables(nTabla)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoCaja)
        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False
        If Trim(txtDocumento.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de documento válido ...")
            ft.enfocarTexto(txtDocumento)
            Exit Function
        Else
            Dim aCampos() As String = {"nummov", "id_emp"}
            Dim aValores() As String = {txtDocumento.Text, jytsistema.WorkID}
            If qFound(MyConn, lblInfo, "jsbantracaj", aCampos, aValores) AndAlso i_modo = movimiento.iAgregar Then
                ft.mensajeAdvertencia("Esté documento ya fué incluido, verifique ...")
                ft.enfocarTexto(txtDocumento)
                Exit Function
            End If
        End If

        If cmbFormaPago.SelectedIndex >= 1 AndAlso txtDocPago.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de pago válido ...")
            ft.enfocarTexto(txtDocPago)
            Exit Function
        End If

        If cmbFormaPago.SelectedIndex >= 1 AndAlso txtRefPago.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una referencia de pago válida ...")
            ft.enfocarTexto(txtRefPago)
            Exit Function
        End If

        If Not ft.isNumeric(txtImporte.Text) Then
            ft.mensajeAdvertencia("El importe  debe ser numérico ...")
            ft.enfocarTexto(txtImporte)
            Return False
        End If

        Validado = True
    End Function


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Insertar As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Insertar = True
            Apuntador = dt.Rows.Count
        End If
        InsertEditBANCOSRenglonCaja(MyConn, lblInfo, Insertar, CodigoCaja, Renglon, txtFecha.Value, "CAJ",
             "SA", txtDocumento.Text, "EF", "", "", -1 * ValorNumero(txtImporte.Text), "", "AVANCE EFECTIVO", "", jytsistema.MyDate, 1,
             "", "", "", MyDate, "", "", "0")

        InsertEditBANCOSRenglonCaja(MyConn, lblInfo, Insertar, CodigoCaja, Renglon, txtFecha.Value, "CAJ",
                     "EN", txtDocumento.Text, aFormaPagR(cmbFormaPago.SelectedIndex),
                     txtDocPago.Text, txtRefPago.Text, ValorNumero(txtImporte.Text), "", "AVANCE EFECTIVO", "", jytsistema.MyDate, 1,
                     "", "", "", MyDate, "", "", "0")


        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoCaja & " " & txtDocumento.Text)
        Me.Close()
    End Sub

    Private Sub txtDocumento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocumento.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de documento para este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtDocPago_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocPago.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de documento de pago para este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtRefPago_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRefPago.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la referencia de pago ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el monto ó importe para este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub cmbTipo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        ft.mensajeEtiqueta(lblInfo, "Seleccione el tipo de movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub cmbFormaPago_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFormaPago.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Seleccione la forma de pago para este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub cmbFormaPago_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
         cmbFormaPago.SelectionChangeCommitted
        If cmbFormaPago.SelectedIndex = 1 Then
            txtRefPago.Enabled = False
            btnTarjeta.Visible = True
        Else
            txtRefPago.Enabled = True
            btnTarjeta.Visible = False
        End If
    End Sub

    Private Sub btnTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTarjeta.Click
        Dim f As New jsPOSArcCajas
        f.Cargar(MyConn, 1)
        If dtTar.Rows.Count > 0 Then
            txtRefPago.Text = f.Seleccionado
        Else
            txtRefPago.Text = ""
        End If
        f = Nothing
    End Sub

    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporte.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class