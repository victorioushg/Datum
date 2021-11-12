Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsMerArcLotesMercanciaMovimientos

    Private Const sModulo As String = "Movimiento de lotes de mercancía"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodigoMercancia As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodigoMercancia As String)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        CodigoMercancia = iCodigoMercancia

        AsignarTooltips()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodigoMercancia As String)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        CodigoMercancia = iCodigoMercancia
        AsignarTooltips()
        Habilitar()
        AsignarTXT(Apuntador)

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(txtVencimiento, "<B>Selecciona fehca</B> vencimiento para el lote... ")
    End Sub
    Private Sub Habilitar()

        If i_modo = movimiento.iEditar Then _
            ft.habilitarObjetos(False, True, txtVencimiento)
    End Sub
    Private Sub IniciarTXT()

        txtLote.Text = ""
        txtVencimiento.Value = jytsistema.sFechadeTrabajo

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtLote.Text = .Item("lote")
            txtVencimiento.Text = ft.FormatoFecha(CDate(.Item("expiracion").ToString))

        End With
    End Sub

    Private Sub jsMerArcLotesMercanciaMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing '
    End Sub

    Private Sub jsMerArcLotesMercanciaMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtLote.Text)
        Dim dates As SfDateTimeEdit() = {txtVencimiento}
        SetSizeDateObjects(dates)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLote.GotFocus

        Select Case sender.name
            Case "txtLote"
                ft.mensajeEtiqueta(lblInfo, "Indique un Número de LOTE válido...", Transportables.tipoMensaje.iAyuda)
            Case "btnVence"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la fecha de vencimiento para este número de lote ...", Transportables.tipoMensaje.iAyuda)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If i_modo = movimiento.iAgregar AndAlso ft.DevuelveScalarEntero(MyConn, "select COUNT(*) from jsmerlotmer where codart = '" & CodigoMercancia & "' and lote = '" & txtLote.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
            ft.mensajeAdvertencia("Número de lote YA se encuentra. Verifique por favor...")
            ft.enfocarTexto(txtLote)
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

            InsertEditMERCASLoteMercancia(MyConn, lblInfo, Insertar, CodigoMercancia, txtLote.Text,
                        CDate(txtVencimiento.Text), 0, 0)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtLote.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLote.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

End Class