Imports MySql.Data.MySqlClient

Public Class jsProArcOrdenProduccionRenglones

    Private Const sModulo As String = "Movimiento renglones orden de producción"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private Documento As String
    Private n_Apuntador As Long

    Private PesoActual As Double = 0.0


    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal NumeroOrdenProduccion As String)

        i_modo = movimiento.iAgregar

        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        Documento = NumeroOrdenProduccion

        AsignarTooltips()
        Iniciar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal NumeroOrdenProduccion As String)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Documento = NumeroOrdenProduccion

        AsignarTooltips()
        Iniciar()
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Seleccionar</B> formulación")

    End Sub
   
    Private Sub Iniciar()
        ft.habilitarObjetos(False, True, cmbUnidad, txtCostoUnitario, txtCostoTotal, txtPesoTotal)
        If i_modo = movimiento.iEditar Then ft.habilitarObjetos(False, True, txtCodigo, btnCodigo)
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        cmbUnidad.Items.Clear()
        txtCostoUnitario.Text = ft.FormatoNumero(0.0)
        txtCostoTotal.Text = ft.FormatoNumero(0.0)
        txtPesoTotal.Text = ft.FormatoCantidad(0.0)
        txtCantidad.Text = ft.FormatoCantidad(1.0)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = ft.muestraCampoTexto(.Item("item"))
            txtDescripcion.Text = ft.muestraCampoTexto(.Item("descrip"))

            ft.RellenaCombo({.Item("UNIDAD")}, cmbUnidad)
            txtCantidad.Text = ft.muestraCampoCantidad(.Item("cantidad"))

            txtCostoUnitario.Text = ft.muestraCampoNumero(.Item("costounitario"))
            txtCostoTotal.Text = ft.muestraCampoNumero(.Item("totalrenglon"))
            txtPesoTotal.Text = ft.muestraCampoCantidad(.Item("peso_renglon"))

        End With
    End Sub
    Private Sub jsGenRenglonesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsGenRenglonesMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        txtCodigo.Focus()
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, cmbUnidad.GotFocus, txtCantidad.GotFocus, _
        txtCostoUnitario.GotFocus, _
          btnCodigo.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el código formulación ...", Transportables.tipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Indique la descripción o nombre de formulación ...", Transportables.tipoMensaje.iInfo)
            Case "cmbUnidad"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la unidad para movimiento de orden de producción ...", Transportables.tipoMensaje.iInfo)
            Case "txtCantidad"
                ft.mensajeEtiqueta(lblInfo, "Indique la cantidad a producir de esta fórmula ", Transportables.tipoMensaje.iInfo)
            Case "txtCostoUnitario"
                ft.mensajeEtiqueta(lblInfo, "Indique el costo unitario de formulación... ", Transportables.tipoMensaje.iInfo)
            Case "txtCostoTotal"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el costo total de producto... ", Transportables.tipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida...")
            txtDescripcion.Focus()
            Return False
        End If

        'FORMULACION VALIDA
        If ft.DevuelveScalarEntero(MyConn, "select count(codfor) from jsfabencfor " _
                                   & " where " _
                                   & " codart = '" & txtCodigo.Text & "' and " _
                                   & " estatus = 1 and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

            ft.mensajeAdvertencia("Formulación NO VALIDA...")
            Return False

        End If

        If ValorNumero(txtCantidad.Text) <= 0 Then
            ft.mensajeCritico("DEBE INDICAR UNA CANTIDAD VALIDA")
            ft.enfocarTexto(txtCantidad)
            Return False
        End If

        If ValorNumero(txtCostoTotal.Text) <= 0 Then
            ft.mensajeCritico("FORMULACION SIN CANTIDAD O COSTO POR FAVOR VERIFIQUE...")
            ft.enfocarTexto(txtCantidad)
            Return False
        End If

        If ValorNumero(txtCostoUnitario.Text) = 0.0 Then
            ft.mensajeAdvertencia("Movimiento no puede tener costo CERO (0.00). Veriqfique por favor ")
            ft.enfocarTexto(txtCostoUnitario)
            Return False
        End If


        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Dim Insertar As Boolean = False
            Dim CodigoFormula As String = ft.DevuelveScalarCadena(MyConn, " select codfor from jsfabencfor where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If


            InsertEditPRODUCCIONRenglonOrdenProduccion(MyConn, lblInfo, Insertar, Documento, CodigoFormula, txtDescripcion.Text, _
                                               ValorCantidad(txtCantidad.Text), cmbUnidad.Text, ValorCantidad(txtPesoTotal.Text), _
                                               ValorNumero(txtCostoUnitario.Text), ValorNumero(txtCostoTotal.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click, _
        txtCantidad.Click, txtCostoUnitario.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

   

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress, _
        txtCostoUnitario.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCantidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCantidad.TextChanged, _
        txtCostoUnitario.TextChanged

        txtCostoTotal.Text = ft.muestraCampoNumero(ValorCantidad(txtCantidad.Text) * ValorNumero(txtCostoUnitario.Text))
        txtPesoTotal.Text = ft.FormatoCantidad(PesoActual * ValorCantidad(txtCantidad.Text))

    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged


        Dim dtFormula As New DataTable

        txtDescripcion.Text = ""
        cmbUnidad.Items.Clear()

        dtFormula = ft.AbrirDataTable(dsLocal, "tblFormulaEncontrada", MyConn, " select * from jsfabencfor where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "'  ")
        If dtFormula.Rows.Count > 0 Then
            With dtFormula.Rows(0)
                txtDescripcion.Text = .Item("DESCRIP_2")
                Dim aUnit() As String = {.Item("UNIDAD")}
                ft.RellenaCombo(aUnit, cmbUnidad)
                txtCostoUnitario.Text = ft.muestraCampoNumero(.Item("TOTAL"))
                PesoActual = .Item("PESO_TOTAL")
                txtPesoTotal.Text = ft.muestraCampoCantidad(PesoActual)
            End With
        End If


    End Sub

    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click

        ' //// OJO Actualizar el lISTADO para incluir formulaciones como subensambles
        Dim f As New jsMerArcListaCostosPreciosNormal
        f.Cargar(MyConn, TipoListaPrecios.CostosParaProduccion)
        txtCodigo.Text = f.Seleccionado
        f = Nothing

    End Sub

End Class