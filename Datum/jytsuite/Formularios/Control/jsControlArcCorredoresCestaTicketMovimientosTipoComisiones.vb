Imports MySql.Data.MySqlClient
Public Class jsControlArcCorredoresCestaTicketMovimientosTipoComisiones

    Private Const sModulo As String = "Movimiento de Comisión por tipo de Cesta Ticket"
  
    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private CodigoCorredor As String
    Private TipoTickket As String
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
                       ByVal Corredor As String, ByVal TicketTipo As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCorredor = Corredor
        TipoTickket = TicketTipo
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtTipo.Text = TipoTickket
        ft.iniciarTextoObjetos(FormatoItemListView.iEntero, txtDesde, txtHasta)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtComision)
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal Corredor As String, ByVal ticketTipo As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCorredor = Corredor
        TipoTickket = ticketTipo
        AsignarTXT(Apuntador)
        ft.habilitarObjetos(False, True, txtTipo, txtDesde, txtHasta)

        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtTipo.Text = .Item("tipo")
            txtDesde.Text = ft.FormatoEntero(.Item("desde"))
            txtHasta.Text = ft.FormatoEntero(.Item("hasta"))
            txtComision.Text = ft.FormatoNumero(.Item("comision"))
        End With
    End Sub
   
    Private Sub jsControlArcCorredoresCestaTicketMovimientosTipo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlArcCorredoresCestaTicketMovimientosTipoComision_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtTipo.Text)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorEntero(txtDesde.Text) <= 0 Then
            ft.mensajeAdvertencia("Debe indicar un valor mayor a cero (0) ...")
            Return False
        End If

        If ValorEntero(txtHasta.Text) <= 0 Then
            ft.mensajeAdvertencia("Debe indicar un valor mayor a cero (0) ...")
            Return False
        End If

        If i_modo = movimiento.iAgregar Then
            If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvencescom where  " _
                                     & " corredor = '" & CodigoCorredor & "' and tipo = '" & TipoTickket & "' and " _
                                     & " desde = " & ValorEntero(txtDesde.Text) & " and " _
                                     & " hasta = " & ValorEntero(txtHasta.Text) & " and " _
                                     & " id_emp = '" & jytsistema.WorkID & "' ") > 0 Then

                ft.mensajeAdvertencia("Tipo de comisión y rango de días YA existe...")
                Return False
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
            InsertEditCONTROLCestaTicketTipoComisiones(MyConn, lblInfo, Insertar, CodigoCorredor, txtTipo.Text, CInt(txtDesde.Text), _
                                             ValorEntero(txtHasta.Text), ValorNumero(txtComision.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtTipo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDesde.Click, txtHasta.Click, _
        txtDesde.GotFocus, txtHasta.GotFocus
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub txtComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDesde.KeyPress, _
        txtHasta.KeyPress, txtComision.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class