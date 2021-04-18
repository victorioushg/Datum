Imports MySql.Data.MySqlClient
Public Class jsGenProNumerosControlMovimientos
    Private Const sModulo As String = "Número de control anulado"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private numControl As String
    Private Origen As String = ""
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property num_Control() As String
        Get
            Return numControl
        End Get
        Set(ByVal value As String)
            numControl = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, nOrigen As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Origen = nOrigen
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        ft.habilitarObjetos(True, True, txtNumeroControl)
        ft.habilitarObjetos(False, True, txtFecha)
        txtNumeroControl.Text = ""
        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
    End Sub

    Private Sub jsGenProNumerosControlMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsGenProNumerosControlMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroControl.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el Número de Control Anulado...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False
        If RTrim(txtNumeroControl.Text) = "" Then
            ft.enfocarTexto(txtNumeroControl)
            Exit Function
        Else
            Dim aCam() As String = {"num_control", "id_emp", "org", "origen"}
            Dim aStr() As String = {txtNumeroControl.Text, jytsistema.WorkID, "CON", Origen}
            If qFound(MyConn, lblInfo, "jsconnumcon", aCam, aStr) Then
                ft.MensajeCritico("Número de Control YA está ANULADO ")
                ft.enfocarTexto(txtNumeroControl)
                Exit Function
            End If
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then

                ft.Ejecutar_strSQL(myconn, "INSERT INTO jsconnumcon " _
                         & " set " _
                         & " numdoc = '" & txtNumeroControl.Text & "',   " _
                         & " num_control = '" & txtNumeroControl.Text & "',  " _
                         & " emision = '" & ft.FormatoFechaMySQL(CDate(txtFecha.Text)) & "',  " _
                         & " org = 'CON',  " _
                         & " origen = '" & Origen & "', " _
                         & " id_emp = '" & jytsistema.WorkID & "'")

                num_Control = txtNumeroControl.Text

            End If

            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnDescripcion_Click(sender As System.Object, e As System.EventArgs) Handles btnDescripcion.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, sender)
    End Sub
End Class