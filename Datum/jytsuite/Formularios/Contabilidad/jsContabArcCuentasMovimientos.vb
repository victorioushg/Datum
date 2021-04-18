Imports MySql.Data.MySqlClient
Public Class jsContabArcCuentasMovimientos

    Private Const sModulo As String = "Movimiento cuenta contable"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private NivelCuenta As Integer
    Private MarcaCuenta As Integer
    Private num_Cuenta As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property numCuenta() As String
        Get
            Return num_Cuenta
        End Get
        Set(ByVal value As String)
            num_Cuenta = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal CuentaActual As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        IniciarTXT(CountPredictor(MyConn, lblInfo, CuentaActual))
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT(ByVal numCuenta As String)
        txtCodigo.Enabled = True
        txtCodigo.Text = numCuenta
        txtNombre.Text = ""
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        txtCodigo.Enabled = False
        With dtLocal
            txtCodigo.Text = .Rows(nPosicion).Item("codcon")
            txtNombre.Text = .Rows(nPosicion).Item("descripcion")
        End With
    End Sub
    Private Sub jsContabArcCuentasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsContabArcCuentasMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de cuenta contable ...", Transportables.TipoMensaje.iInfo)
        sender.maxlength = 30
    End Sub
    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción de la cuenta contable ...", Transportables.TipoMensaje.iInfo)
        sender.maxlength = 50
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If txtCodigo.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una cuenta contable válida...")
            Exit Function
        Else
            If i_modo = movimiento.iAgregar Then
                If CBool(ParametroPlus(MyConn, Gestion.iContabilidad, "CONPARAM02")) Then
                    If EstructuraValidaPlus(MyConn, txtCodigo.Text) Then '  ValidarEstructuraActual(MyConn, lblInfo, txtCodigo.Text, FuncionesContabilidad.EstructuraCodActual, FuncionesContabilidad.LongNivelesCodActual) Then
                        If SePuedeAgregarPlus(MyConn, txtCodigo.Text) Then
                        Else
                            ft.MensajeCritico("Esta cuenta NO puede ser agregada ...")
                            txtCodigo.Focus()
                            Exit Function
                        End If
                    Else
                        ft.MensajeCritico("Estructura no válida. Verifique...")
                        Exit Function
                    End If
                End If
                Dim aFld() As String = {"codcon", "id_emp"}
                Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}
                If qFound(MyConn, lblInfo, "jscotcatcon", aFld, aStr) Then
                    ft.mensajeAdvertencia("Esta cuenta YA existe...")
                    txtCodigo.Focus()
                    Exit Function
                Else
                    If Microsoft.VisualBasic.Right(txtCodigo.Text, 1) = "." Then
                        Dim aStrp() As String = {Mid(txtCodigo.Text, 1, Len(txtCodigo.Text) - 1), jytsistema.WorkID}
                        If qFound(MyConn, lblInfo, "jscotcatcon", aFld, aStrp) Then
                            ft.mensajeAdvertencia("Esta cuenta YA existe COMO CUENTA DE DETALLE ...")
                            txtCodigo.Focus()
                            Exit Function
                        End If
                    End If
                End If


            End If
        End If

        If txtNombre.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida...")
            txtNombre.Focus()
            Exit Function
        End If

        MarcaCuenta = IIf(Microsoft.VisualBasic.Right(txtCodigo.Text, 1) = ".", 1, 0)
        NivelCuenta = NivelCuentaContable(txtCodigo.Text)

        If NivelCuenta = 1 And Microsoft.VisualBasic.Right(txtCodigo.Text, 1) <> "." Then
            ft.mensajeAdvertencia(" Cuenta principal no válida...")
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
            InsertEditCONTABCuentaContable(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, NivelCuenta, MarcaCuenta)

            numCuenta = txtCodigo.Text

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
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
    Private Function SePuedeAgregarPlus(MyConn As MySqlConnection, CodigoCuenta As String) As Boolean

        Dim aCodigoCuenta() As String = CodigoCuenta.Split(".")
        If aCodigoCuenta.Length = 2 And aCodigoCuenta(1).Trim = "" Then Return True
        Dim aCadena As String = ""
        For iCont As Integer = 0 To aCodigoCuenta.Length - IIf(CodigoCuenta.EndsWith("."), 3, 2)
            aCadena += aCodigoCuenta(iCont) + "."
        Next

        If ft.DevuelveScalarEntero(MyConn, " select count(*) from jscotcatcon where codcon = '" & aCadena & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then Return True

        Return False

    End Function

   
End Class