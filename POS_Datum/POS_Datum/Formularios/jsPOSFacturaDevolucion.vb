Imports MySql.Data.MySqlClient
Imports FP_AclasBixolon
Public Class jsPOSFacturaDevolucion

    Private Const sModulo As String = "Devolución POS"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private ft As New Transportables
    Private IB As New AclasBixolon

    Private i_modo As Integer

    Private n_NumeroFactura As String
    Private n_DocumentoInterno As String
    Private n_NumeroSerie As String
    Private n_FechaFacturaAfectada As String
    Private n_HoraFacturaAfectada As String
    Private NumCaja As String = ""
    Public Property NumeroFactura() As String
        Get
            Return n_NumeroFactura
        End Get
        Set(ByVal value As String)
            n_NumeroFactura = value
        End Set
    End Property
    Public Property DocumentoInterno() As String
        Get
            Return n_DocumentoInterno
        End Get
        Set(ByVal value As String)
            n_DocumentoInterno = value
        End Set
    End Property
    Public Property NumeroSerie() As String
        Get
            Return n_NumeroSerie
        End Get
        Set(ByVal value As String)
            n_NumeroSerie = value
        End Set
    End Property
    Public Property FechaFacturaAfectada() As String
        Get
            Return n_FechaFacturaAfectada
        End Get
        Set(ByVal value As String)
            n_FechaFacturaAfectada = value
        End Set
    End Property
    Public Property HoraFacturaAfectada() As String
        Get
            Return n_HoraFacturaAfectada
        End Get
        Set(ByVal value As String)
            n_HoraFacturaAfectada = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal CodigoCaja As String)

        MyConn = MyCon
        dsLocal = ds
        NumCaja = CodigoCaja
        IniciarTXT()
        Me.ShowDialog()

    End Sub

    Private Sub IniciarTXT()

        Dim bRet As Boolean
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumeroFactura, txtFacturaInterna)

        bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
        If bRet Then
            If TipoImpresoraFiscal(MyConn, jytsistema.WorkBox) = 7 Then
                txtSerial.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NR_SRP812)
            ElseIf TipoImpresoraFiscal(MyConn, jytsistema.WorkBox) = 5 Then
                txtSerial.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NR_SRP350)
            Else
                txtSerial.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.numRegistro)
            End If
            IB.cerrarPuerto()
        End If

        txtFechaFacturaAfectada.Text = ft.muestraCampoFecha(jytsistema.sFechadeTrabajo)
        txtHoraFacturaAfectada.Text = ft.FormatoHoraCorta(Now())

    End Sub

    Private Sub jsPOSFacturaDevolucion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
    End Sub

    Private Sub jsPOSFacturaDevolucion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtNumeroFactura.GotFocus, txtFacturaInterna.GotFocus, txtSerial.GotFocus, txtFechaFacturaAfectada.GotFocus
        ft.enfocarTexto(sender)
        Select Case sender.name
            Case "txtNumeroFactura"
                lblInfo.Text = "Indique el Nº de Ticket Fiscal afectado ..."
            Case "txtFacturaInterna"
                lblInfo.Text = "Indique el Nº de Factura Interna Afectada ..."
            Case "txtSerial"
                lblInfo.Text = "Indique el Nº de Serial de la impresora Fiscal que emitió Factura de venta afectada ..."
            Case "txtFechaFacturaAfectada"
                lblInfo.Text = "Indique la FECHA de el Ticket Fiscal o Nº Factura de venta afectada ..."
            Case "txtHoraFacturaAfectada"
                lblInfo.Text = "Indique la HORA de el Ticket Fiscal o Nº Factura de venta afectada ..."
        End Select

    End Sub
    'Private Function DevolucionValida(MyConn As MySqlConnection, NumeroFactura As String, NumeroDocumentoInterno As String) As Boolean

    '    Dim dtRenglones As New DataTable

    '    dtRenglones = ft.AbrirDataTable(dsLocal, "tblRenglonesDevolucion", MyConn, " select * from jsvenrenpos where " _
    '                                    & " numfac = '" & NumeroDocumentoInterno & "' and id_emp = '" & jytsistema.WorkID & "' ")

    '    If dtRenglones.Rows.Count > 0 Then
    '        For Each iRenglon As DataRow In dtRenglones.Rows
    '            With iRenglon
    '                If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvenrenpos where " _
    '                                           & " numfac = '" & NumeroDocumentoInterno & "' and " _
    '                                           & " item = '" & .Item("ITEM") & "' and " _
    '                                           & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
    '                    Return False
    '                End If
    '            End With
    '        Next
    '    Else
    '        Return False
    '    End If

    '    Return True

    'End Function
    Private Function Validado() As Boolean

        If Trim(txtNumeroFactura.Text) = "" Then
            ft.mensajeCritico("Debe indicar un NUMERO DE FACTURA FISCAL VALIDA ...")
            txtNumeroFactura.Focus()
            Return False
        End If

        If Trim(txtFacturaInterna.Text) = "" Then
            ft.mensajeCritico("Debe indicar un NUMERO DE FACTURA INTERNA VALIDA ...")
            txtFacturaInterna.Focus()
            Return False
        Else
            If ft.DevuelveScalarCadena(MyConn, "SELECT numfac from jsvenencpos where numfac = '" & txtFacturaInterna.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "" Then
                ft.mensajeCritico(" NUMERO DE FACTURA INTERNA NO ES VALIDO. VERIFIQUE POR FAVOR ...")
                txtFacturaInterna.Focus()
                Return False
            End If
        End If


        If Trim(txtSerial.Text) = "" Or _
            ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(*) from jsconcatimpfis where maquinafiscal = '" & txtSerial.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
            ft.mensajeAdvertencia("Debe indicar un NUMERO DE SERIAL válido...")
            txtSerial.Focus()
            Return False
        End If

        If Not IsDate(txtFechaFacturaAfectada.Text) Then
            ft.mensajeAdvertencia("Debe indicar unA FECHA válida...")
            txtFechaFacturaAfectada.Focus()
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK

        If Validado() Then

            NumeroFactura = txtNumeroFactura.Text
            NumeroSerie = txtSerial.Text
            DocumentoInterno = txtFacturaInterna.Text
            FechaFacturaAfectada = Format(CDate(txtFechaFacturaAfectada.Text).Day, "00") & _
                Format(CDate(txtFechaFacturaAfectada.Text).Month, "00") & _
                Format(CDate(txtFechaFacturaAfectada.Text), "yy")

            HoraFacturaAfectada = Replace(txtHoraFacturaAfectada.Text, ":", "")
            Me.Close()
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        NumeroFactura = ""
        NumeroSerie = ""
        FechaFacturaAfectada = ""
        HoraFacturaAfectada = ""
        DocumentoInterno = ""
        Me.Close()
    End Sub

End Class