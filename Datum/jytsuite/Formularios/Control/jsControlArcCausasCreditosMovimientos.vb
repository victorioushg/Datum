Imports MySql.Data.MySqlClient
Public Class jsControlArcCausasCreditosMovimientos

    Private sModulo As String = ""

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private CreditoDebito As Integer
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal Credito_Debito As Integer)
        i_modo = movimiento.iAgregar
        CreditoDebito = Credito_Debito
        sModulo = IIf(CreditoDebito = 0, "Movimiento causa de crédito", "Movimiento causa débito")
        Me.Text = sModulo
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        IniciarTXT()
        Me.ShowDialog()
    End Sub

    Private Sub IniciarTXT()
        txtCodigo.Text = ft.autoCodigo(MyConn, "codigo", "jsvencaudcr", "credito_debito.id_emp", Convert.ToString(CreditoDebito) + "." + jytsistema.WorkID, 5)
        txtNombre.Text = ""
        chk1.Checked = False
        chk2.Checked = False
        chk3.Checked = False
        chk4.Checked = False
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal Credito_Debito As Integer)
        i_modo = movimiento.iEditar
        CreditoDebito = Credito_Debito
        sModulo = IIf(CreditoDebito = 0, "Movimiento causa de crédito", "Movimiento causa débito")
        Me.Text = sModulo
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtCodigo.Text = .Item("codigo")
            txtNombre.Text = .Item("descrip")
            chk1.Checked = IIf(.Item("inventario") = 1, True, False)
            chk2.Checked = IIf(.Item("validaunidad") = 1, True, False)
            chk3.Checked = IIf(.Item("ajustaprecio") = 1, True, False)
            chk4.Checked = IIf(.Item("estado") = 1, True, False)
        End With
    End Sub
    Private Sub jsControlDivisionesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válido...")
            txtNombre.Focus()
            Exit Function
        Else

            If NumeroDeRegistrosEnTabla(MyConn, "jsvencaudcr", " credito_debito = " & CreditoDebito & " AND descrip = '" & txtNombre.Text.Trim() & "' ") > 0 AndAlso _
               i_modo = movimiento.iAgregar Then
                ft.mensajeCritico("DESCRIPCION EXISTENTE. Indique descripción válida...")
                ft.enfocarTexto(txtNombre)
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
            InsertEditCONTROLCausaCreditoDebito(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, CreditoDebito, _
                                                 IIf(chk1.Checked, 1, 0), IIf(chk2.Checked, 1, 0), IIf(chk3.Checked, 1, 0), _
                                                  IIf(chk4.Checked, 1, 0))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el descripción de la causa ...", Transportables.TipoMensaje.iInfo)
    End Sub

End Class