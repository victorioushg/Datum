Imports MySql.Data.MySqlClient
Public Class jsControlArcImpresoraFiscalMovimientos

    Private Const sModulo As String = "Movimiento tipo de impresora para facturación "


    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private aPuerto() As String = {"COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", _
                                   "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16", "COM17", "COM18"}
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

        txtCodigo.Text = ft.autoCodigo(MyConn, "codigo", "jsconcatimpfis", "id_emp", jytsistema.WorkID, 5)
        txtMaquinaFiscal.Text = ""
        txtNumFactura.Text = ""
        txtnumNC.Text = ""
        txtNumNoFiscal.Text = ""
        ft.RellenaCombo(aFiscal, cmbTipo)
        ft.RellenaCombo(aPuerto, cmbPuerto)

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
            txtMaquinaFiscal.Text = .Item("maquinafiscal")
            ft.RellenaCombo(aFiscal, cmbTipo, .Item("tipoimpresora"))
            txtNumFactura.Text = .Item("ultima_factura")
            txtnumNC.Text = .Item("ultima_notacredito")
            txtNumNoFiscal.Text = .Item("ultimo_docnofiscal")
            Dim aa As Integer = IIf(ft.InArray(aPuerto, .Item("puerto")) < 0, 0, ft.InArray(aPuerto, .Item("puerto")))
            ft.RellenaCombo(aPuerto, cmbPuerto, aa)
        End With
    End Sub
    Private Sub jsPOSArcCajasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsPOSArcCajasMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.habilitarObjetos(False, True, txtCodigo, txtNumFactura, txtnumNC, txtNumNoFiscal)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
          txtMaquinaFiscal.GotFocus, txtNumFactura.GotFocus, _
        txtnumNC.GotFocus, txtNumNoFiscal.GotFocus
        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el código para tipo de impresión ...", Transportables.TipoMensaje.iInfo)
            Case "txtMaquinaFiscal"
                ft.mensajeEtiqueta(lblInfo, "Indique el número serial de la impresora fiscal ...", Transportables.TipoMensaje.iInfo)
            Case "btnAlmacen"
                ft.mensajeEtiqueta(lblInfo, "Seleccione almacén de salida para esta caja ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If cmbTipo.SelectedIndex > 1 Then
            If txtMaquinaFiscal.Text = "" Then
                ft.mensajeAdvertencia("Debe indicar un Nº Serial Caja válido ...")
                ft.enfocarTexto(txtMaquinaFiscal)
                Exit Function
            End If
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
            InsertEditControlImpresoraFiscal(MyConn, lblInfo, Insertar, txtCodigo.Text, _
                                       cmbTipo.SelectedIndex, txtMaquinaFiscal.Text, txtNumFactura.Text, _
                                       txtnumNC.Text, txtNumNoFiscal.Text, 0, cmbPuerto.Text)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

End Class