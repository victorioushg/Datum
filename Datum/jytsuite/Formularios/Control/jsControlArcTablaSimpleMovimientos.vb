Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsControlArcTablaSimpleMovimientos
    Private Const sModulo As String = "Movimiento "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private codModulo As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal CodigoModulo As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        codModulo = CodigoModulo
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        If codModulo = Modulo.iMER_UnidadesDeMedida Then
            txtCodigo.MaxLength = 3
            txtCodigo.Text = ""
            ft.habilitarObjetos(True, True, txtCodigo)
        Else
            txtCodigo.Text = ft.autoCodigo(MyConn, "codigo", "jsconctatab", "modulo.id_emp", codModulo + "." + jytsistema.WorkID, 5)
        End If

        txtDescripcion.Text = ""
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal CodigoModulo As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        codModulo = CodigoModulo
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal
            txtCodigo.Text = .Rows(nPosicion).Item("codigo")
            txtDescripcion.Text = .Rows(nPosicion).Item("descripcion")
        End With
    End Sub

    Private Sub jsControlTablaSimpleMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlTablaSimpleMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If i_modo = movimiento.iAgregar Then
            If txtCodigo.Text = "" Then
                ft.mensajeCritico("Código NO existe. por favor Verifiqe...")
                Return False
            End If
            If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsconctatab " _
                                       & " where " _
                                       & " codigo = '" & txtCodigo.Text & "' AND " _
                                       & " modulo = '" & codModulo & "' AND " _
                                       & " id_emp= '" & jytsistema.WorkID & "' ") > 0 Then
                ft.mensajeCritico("Este codigo YA existe. por favor Verifiqe...")
                Return False
            End If
        End If

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válida ...")
            ft.enfocarTexto(txtDescripcion)
            Exit Function
        Else
            If NumeroDeRegistrosEnTabla(MyConn, "jsconctatab", " modulo = '" & codModulo & "' AND descrip = '" & txtDescripcion.Text.Trim() & "' ") > 0 AndAlso _
               i_modo = movimiento.iAgregar Then
                ft.mensajeCritico("DESCRIPCION EXISTENTE. Indique descripción válida...")
                ft.enfocarTexto(txtDescripcion)
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
            InsertEditCONTROLTablaSimple(MyConn, lblInfo, Insertar, txtCodigo.Text, txtDescripcion.Text, codModulo)



            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class