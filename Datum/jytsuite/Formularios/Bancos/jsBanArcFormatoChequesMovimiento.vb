Imports MySql.Data.MySqlClient
Public Class jsBanArcFormatoChequesMovimiento
    Private Const sModulo As String = "Movimiento de plantillas de cheques"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Public Const BUTTONDOWN = &HA1
    Public Const CAPTION = 2

    Private txt As String

    Private i_modo As Integer
    Private n_Apuntador As Long
    Private n_Y As Integer = 25
    Private n_X As Integer = 12
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        If dt.Rows.Count = 0 Then Apuntador = -1
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ft.autoCodigo(MyConn, "formato", "jsbancatfor", "id_emp", jytsistema.WorkID, 2, True)

        txtDescripcion.Text = ""
        txtMonto.Text = ft.FormatoEntero(440)
        txtNombre.Text = ft.FormatoEntero(1295)
        txtLetras.Text = ft.FormatoEntero(1595)
        txtFecha.Text = ft.FormatoEntero(2300)
        txtEndosable.Text = ft.FormatoEntero(850)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt
            txtCodigo.Text = .Rows(nPosicion).Item("formato")
            txtDescripcion.Text = .Rows(nPosicion).Item("descrip")
            txtMonto.Left = .Rows(nPosicion).Item("montoleft") / n_X
            txtMonto.Top = .Rows(nPosicion).Item("montotop") / n_Y
            txtNombre.Left = .Rows(nPosicion).Item("nombreleft") / n_X
            txtNombre.Top = .Rows(nPosicion).Item("nombretop") / n_Y
            txtLetras.Left = .Rows(nPosicion).Item("montoletraleft") / n_X
            txtLetras.Top = .Rows(nPosicion).Item("montoletratop") / n_Y
            txtFecha.Left = .Rows(nPosicion).Item("fechaleft") / n_X
            txtFecha.Top = .Rows(nPosicion).Item("fechatop") / n_Y
            txtEndosable.Left = .Rows(nPosicion).Item("noendosableleft") / n_X
            txtEndosable.Top = .Rows(nPosicion).Item("noendosabletop") / n_Y

        End With
    End Sub
    Private Sub AsignarTooltips()
        C1SuperTooltip1.SetToolTip(txtMonto, "***<B>Monto</B> en números ***")
        C1SuperTooltip1.SetToolTip(txtNombre, "<B>Nombre o beneficiario</B> del cheque")
        C1SuperTooltip1.SetToolTip(txtLetras, "<B>monto en letras</B> ")
        C1SuperTooltip1.SetToolTip(txtFecha, "<B>Fecha</B> de cheque")
        C1SuperTooltip1.SetToolTip(txtEndosable, "texto <B>NO ENDOSABLE</B>")

    End Sub
    Private Sub jsBanFormatoChequesMovimiento_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '
        ft = Nothing
    End Sub

    Private Sub jsBanFormatoChequesMovimiento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        AsignarTooltips()
        For Each c As Control In grpCheque.Controls
            c.Visible = True
        Next
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        If txtDescripcion.Text.Trim = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción para esta plantilla ...")
            Return False
        End If
        Return True
    End Function


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dt.Rows.Count
            End If
            InsertEditBANCOSPlantillaCheque(MyConn, lblInfo, Insertar, txtCodigo.Text, txtDescripcion.Text, _
                 ft.FormatoEntero(txtMonto.Top * n_Y), ft.FormatoEntero(txtMonto.Left * n_X), ft.FormatoEntero(txtNombre.Top * n_Y), _
                 ft.FormatoEntero(txtNombre.Left * n_X), ft.FormatoEntero(txtLetras.Top * n_Y), ft.FormatoEntero(txtLetras.Left * n_X), _
                 ft.FormatoEntero(txtFecha.Top * n_Y), ft.FormatoEntero(txtFecha.Left * n_X), ft.FormatoEntero(txtEndosable.Top * n_Y), _
                 ft.FormatoEntero(txtEndosable.Left * n_X))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique descripción para esta plantilla (sug. use los nombres de los bancos que la usan) ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtMontoTop_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMonto.GotFocus, _
         txtNombre.GotFocus, txtLetras.GotFocus, _
          txtEndosable.GotFocus
        ft.mensajeEtiqueta(lblInfo, "presione el botón izquierdo del ratón y presionado mueva a la el coordenada indicada ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtMontotop_MouseDown(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.MouseEventArgs) Handles txtMonto.MouseDown, txtNombre.MouseDown, _
        txtLetras.MouseDown, txtFecha.MouseDown, txtEndosable.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Left Then
            sender.Capture = False
            Dim msg As Message = _
            Message.Create(sender.Handle, BUTTONDOWN, _
            New IntPtr(CAPTION), IntPtr.Zero)
            Me.DefWndProc(msg)
        End If

    End Sub

    Private Sub txtMontoTop_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtMonto.MouseMove, _
        txtNombre.MouseMove, txtLetras.MouseMove, txtFecha.MouseMove, txtEndosable.MouseMove
        sender.Text = " (X, Y):(" & (sender.Left * n_X).ToString & "," & (sender.Top * n_Y).ToString & ")"
    End Sub

    
End Class