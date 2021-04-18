Imports MySql.Data.MySqlClient
Public Class jsComArcUnidadCategoriaMovimientos
    Private Const sModulo As String = "Movimiento "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private Antecesor As String
    Private nomTabla As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal Antec As String, _
                       ByVal nombretabla As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Antecesor = Antec
        nomTabla = nombretabla
        Me.Text = sModulo & IIf(Antec = "", " Unidad de Negocio ", " Categoría proveedor ")
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtCodigo.Text = Antecesor & AutoCodigo(5, dsLocal, dtLocal.TableName, "codigo")
        txtDescripcion.Text = ""
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal Antec As String, _
                      ByVal nombreTabla As String)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Antecesor = Antec
        nomTabla = nombreTabla

        Me.Text = sModulo & IIf(Antec = "", " Unidad de Negocio ", " Categoría proveedor ")
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub

    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal
            txtCodigo.Text = .Rows(nPosicion).Item("codigo")
            txtDescripcion.Text = .Rows(nPosicion).Item("descripcion")
        End With
    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el código ...", TipoMensaje.iInfo)
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtDescripcion.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un nombre o descripción válida ...")
            EnfocarTexto(txtDescripcion)
            Exit Function
        Else

            If NumeroDeRegistrosEnTabla(MyConn, nomTabla, " descrip = '" & txtDescripcion.Text.Trim() & "' ") > 0 AndAlso _
                i_modo = movimiento.iAgregar Then
                MensajeAdvertencia(lblInfo, "DESCRIPCION EXISTENTE. Indique descripción válida...")
                EnfocarTexto(txtDescripcion)
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
            InsertEditVENTASCanaldistribucionTiponegocio(MyConn, lblInfo, Insertar, txtCodigo.Text, txtDescripcion.Text, Antecesor, nomTabla)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class