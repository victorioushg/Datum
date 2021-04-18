Imports MySql.Data.MySqlClient
Public Class jsComArcGrupoSubgrupoMovimientos
    Private Const sModulo As String = "Movimiento de grupo de gastos"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private cParentCod As Integer
    Private iLevel As Integer
    Private CodigoGrupo As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, _
                       ByVal dt As DataTable, _
                       ByVal cCodigoPadre As Integer)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        cParentCod = cCodigoPadre

        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()
        CodigoGrupo = ft.NumeroAleatorio(10000)
        Dim aFld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {CodigoGrupo, jytsistema.WorkID}
        While qFound(MyConn, lblInfo, "jsprogrugas", aFld, aStr)
            CodigoGrupo = ft.NumeroAleatorio(10000)
            aStr(0) = CodigoGrupo
        End While
        txtDescripcion.Text = ""
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, _
                      ByVal dt As DataTable, _
                      ByVal cCodigoPadre As Integer)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        cParentCod = cCodigoPadre
       
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal
            CodigoGrupo = .Rows(nPosicion).Item("codigo").ToString
            txtDescripcion.Text = .Rows(nPosicion).Item("nombre")
        End With
    End Sub

    Private Sub jsComArcGrupoSubgrupoMovimientos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsControlTablaSimpleMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válida ...")
            ft.enfocarTexto(txtDescripcion)
            Exit Function
        Else
            If NumeroDeRegistrosEnTabla(MyConn, "jsprogrugas", " antecesor = " & cParentCod & " and nombre = '" & txtDescripcion.Text.Trim() & "' ") > 0 AndAlso _
               i_modo = movimiento.iAgregar Then
                ft.MensajeCritico("DESCRIPCION EXISTENTE. Indique descripción válida...")
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
            InsertEditCOMPRASGrupo(MyConn, lblInfo, Insertar, _
                                          cParentCod, CodigoGrupo, txtDescripcion.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class