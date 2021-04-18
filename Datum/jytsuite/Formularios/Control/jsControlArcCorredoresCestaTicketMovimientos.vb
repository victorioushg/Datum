Imports MySql.Data.MySqlClient
Public Class jsControlArcCorredoresCestaTicketMovimientos

    Private Const sModulo As String = "Movimiento de valor de cesta ticket"
    Private Const nTablaValores As String = "tblValoresCestaTicket"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private CodigoCorredor As String

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
                       ByVal Corredor As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCorredor = Corredor
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtEnBarra)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtValor)
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal Corredor As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCorredor = Corredor
        AsignarTXT(Apuntador)
        ft.habilitarObjetos(False, True, txtEnBarra)

        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtEnBarra.Text = .Item("enbarra")
            txtValor.Text = ft.FormatoNumero(.Item("valor"))
        End With
    End Sub

    Private Sub jsControlArcCorredoresCestaTicketMovimientosTipo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlArcCorredoresCestaTicketMovimientosTipoComision_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtEnBarra.Text)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorNumero(txtValor.Text) <= 0 Then
            ft.mensajeAdvertencia("Debe indicar un valor mayor a cero (0) ...")
            Return False
        End If

        If i_modo = movimiento.iAgregar Then
            If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvenvaltic where  " _
                                     & " codigo = '" & CodigoCorredor & "' and " _
                                     & " enbarra = '" & txtEnBarra.Text & "' and " _
                                     & " id_emp = '" & jytsistema.WorkID & "' ") > 0 Then

                ft.mensajeAdvertencia("Codigo en barra YA existe ...")
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
            InsertEditCONTROLCestaTicketMovimientoValor(MyConn, lblInfo, Insertar, CodigoCorredor, txtEnBarra.Text, ValorNumero(txtValor.Text))

            dsLocal = DataSetRequery(dsLocal, "select * from jsvenvaltic " _
                            & " where " _
                            & " codigo  = '" & CodigoCorredor & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " order by enbarra ", MyConn, nTablaValores, lblInfo)

            dtLocal = dsLocal.Tables(nTablaValores)

            Dim row As DataRow = dtLocal.Select(" ENBARRA = '" & txtEnBarra.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
            Apuntador = dtLocal.Rows.IndexOf(row)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtEnBarra.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComision_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub txtComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
         txtValor.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class