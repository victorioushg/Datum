Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsVenArcCanalTipoNegocioMovimientos
    Private Const sModulo As String = "Movimiento "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

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
        Me.Text = sModulo & IIf(Antec = "", IIf(nomTabla.Substring(0, 5) = "jspro", " UNIDAD DE NEGOCIO", " CANAL DE DISTRIBUCION"), _
                                                 IIf(nomTabla.Substring(0, 5) = "jspro", " CATEGORIA DE NEGOCIO", " TIPO DE NEGOCIO"))
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        Dim aWhereFields As String = ""
        Dim aWhereValues As String = ""
        If Antecesor = "" Then
            aWhereFields = "id_emp"
            aWhereValues = jytsistema.WorkID
        Else
            aWhereFields = "antec.id_emp"
            aWhereValues = Antecesor + "." + jytsistema.WorkID
        End If
        txtCodigo.Text = ft.autoCodigo(MyConn, "CODIGO", nomTabla, aWhereFields, aWhereValues, 5)
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

        Me.Text = sModulo & IIf(Antec = "", IIf(nomTabla.Substring(0, 5) = "jspro", " UNIDAD DE NEGOCIO", " CANAL DE DISTRIBUCION"), _
                                                 IIf(nomTabla.Substring(0, 5) = "jspro", " CATEGORIA DE NEGOCIO", " TIPO DE NEGOCIO"))
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub

    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtCodigo.Text = ft.MuestraCampoTexto(.Item("codigo"))
            txtDescripcion.Text = ft.MuestraCampoTexto(.Item("descrip"))
        End With
    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsVenArcCanalTipoNegocioMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código ...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtDescripcion.Text) = "" Then
            ft.MensajeCritico("Debe indicar un nombre o descripción válida ...")
            ft.enfocarTexto(txtDescripcion)
            Exit Function
        Else

            If NumeroDeRegistrosEnTabla(MyConn, nomTabla, " descrip = '" & txtDescripcion.Text.Trim() & "' ") > 0 AndAlso _
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
            InsertEditVENTASCanaldistribucionTiponegocio(MyConn, lblInfo, Insertar, txtCodigo.Text, txtDescripcion.Text, Antecesor, nomTabla)
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class