Imports MySql.Data.MySqlClient
Public Class jsMerArcServiciosMovimientos

    Private Const sModulo As String = "Movimiento de Servicio"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private aIVA() As String
    Private aTipoServicio() As String = {"ISLR", "NORMAL"}

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt

        AsignarTooltips()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt

        AsignarTooltips()
        Habilitar()
        AsignarTXT(Apuntador)

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnIVA, "<B>Seleccionar IVA</B> de este servicio ...")

    End Sub
    Private Sub Habilitar()
        ft.habilitarObjetos(False, True, txtCodigo)
        If i_modo = movimiento.iEditar Then ft.habilitarObjetos(False, True, btnIVA, cmbIVA, txtIVA)
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ft.autoCodigo(MyConn, "codser", "jsmercatser", "tipo.id_emp", "0." + jytsistema.WorkID, 8)
        txtDescripcion.Text = ""
        Dim aIVA() As String = ArregloIVA(MyConn, lblInfo)
        ft.RellenaCombo(aIVA, cmbIVA)
        txtPrecio.Text = ft.FormatoNumero(0.0)
        ft.RellenaCombo(aTipoServicio, cmbTipoServicio)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = .Item("codser")
            txtDescripcion.Text = ft.muestraCampoTexto(.Item("desser"))
            Dim aIVA() As String = ArregloIVA(MyConn, lblInfo)
            ft.RellenaCombo(aIVA, cmbIVA, ft.InArray(aIVA, .Item("tipoiva")))
            txtPrecio.Text = ft.FormatoNumero(.Item("precio"))
            txtCodigoContable.Text = .Item("codcon")

            ft.RellenaCombo(aTipoServicio, cmbTipoServicio, .Item("tiposervicio"))

        End With
    End Sub

    Private Sub jsMerArcServiciosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing '
    End Sub

    Private Sub jsMerArcServiciosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, txtPrecio.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el codigo del servicio...", Transportables.TipoMensaje.iInfo)
            Case "txtPrecio"
                ft.mensajeEtiqueta(lblInfo, "Indique el precio del servicio... ", Transportables.TipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Indique la descripción del servicio...", Transportables.TipoMensaje.iInfo)
            Case "cmbIVA"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la tasa de IVA deseada para este servicio... ", Transportables.TipoMensaje.iInfo)
            Case "cmbTipoServicio"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el tipo de servicio ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorNumero(txtPrecio.Text) < 0.0 Then
            ft.mensajeCritico("Precio debe ser mayor o igual a CERO (0.00). Veriqfique por favor ")
            ft.enfocarTexto(txtPrecio)
            Exit Function
        End If

        If txtDescripcion.Text = "" Then
            ft.mensajeCritico("Debe indicar una descripción válida para este servicio. Veriqfique por favor ")
            ft.enfocarTexto(txtDescripcion)
            Exit Function
        End If

        If CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERSERPA01")) Then
            If txtCodigoContable.Text = "" Then
                ft.mensajeCritico("Debe indicar un CODIGO CONTABLE válido. Veriqfique por favor...")
                ft.enfocarTexto(txtCodigoContable)
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

            InsertEditMERCASServicio(MyConn, lblInfo, Insertar, txtCodigo.Text, txtDescripcion.Text, _
                        0.0, CDbl(txtPrecio.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0, 0, _
                        cmbIVA.Text, cmbTipoServicio.SelectedIndex, "", 0, 0, txtCodigoContable.Text)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub


    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPrecio.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub


    Private Sub btnIVA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIVA.Click
        Dim f As New jsControlArcIVA
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)

        aIVA = ArregloIVA(MyConn, lblInfo)
        ft.RellenaCombo(aIVA, cmbIVA, ft.InArray(aIVA, f.Seleccionado))

        f = Nothing
    End Sub

    Private Sub cmbIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIVA.SelectedIndexChanged
        txtIVA.Text = ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, jytsistema.sFechadeTrabajo, cmbIVA.Text))
    End Sub

    Private Sub btnCodigoContable_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                    txtCodigoContable.Text)
    End Sub
End Class