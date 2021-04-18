Imports MySql.Data.MySqlClient
Public Class jsPOSRecambioMercancia

    Private Const sModulo As String = "Recambio de mercancía POS"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private ft As New Transportables


    Private NumCaja As String = ""
    Private numCajero As String = ""
    Private AlmacenSalida As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal CodigoCaja As String, ByVal CodigoCajero As String, _
                      ByVal AlmacenDeSalida As String)

        MyConn = MyCon
        dsLocal = ds
        NumCaja = CodigoCaja
        numCajero = CodigoCajero
        AlmacenSalida = AlmacenDeSalida

        IniciarTXT()

        Me.ShowDialog()

    End Sub

    Private Sub IniciarTXT()

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumeroFactura, txtCodigoMercanciaADevolver, txtMercanciaADevolver, _
                              txtUnidadADevolver, txtCodigoMercanciaSalida, txtMercanciaSalida, txtUnidadSalida)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cantidad, txtCantidadADevolver, txtCantidadSalida)

        ft.habilitarObjetos(False, True, txtMercanciaADevolver, txtUnidadADevolver, txtMercanciaSalida, txtUnidadSalida)
        

    End Sub

    Private Sub jsPOSFacturaDevolucion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
    End Sub

    Private Sub jsPOSFacturaDevolucion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroFactura.GotFocus
        Select Case sender.name
            Case "txtNumeroFactura"
                lblInfo.Text = "Indique el número de Factura de venta..."
            Case "txtCodigoMercaniaADevolver"
                lblInfo.Text = "Indique el CODIGO de mercancía a cambiar  ..."
            Case "txtCantidadADevolver"
                lblInfo.Text = "Indique la CANTIDAD de la mercancía a cambiar ..."
            Case "txtCodigoMercanciaSalida"
                lblInfo.Text = "Indique el CODIGO de mercancía de salida..."
            Case "txtCantidadMercanciaSalida"
                lblInfo.Text = "Indique la CANTIDAD de la mercancía de salida ..."
        End Select

    End Sub

    Private Function Validado() As Boolean

        If txtNumeroFactura.Text.Trim = "" Then
            ft.mensajeCritico("DEBE INDICAR UNA FACTURA DE SISTEMA VALIDA...")
            Return False
        Else
            If ft.DevuelveScalarCadena(MyConn, " SELECT NUMFAC FROM jsvenencpos where numfac = '" & txtNumeroFactura.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "" Then
                ft.mensajeCritico("DEBE INDICAR UNA FACTURA DE SISTEMA VALIDA...")
                Return False
            End If
        End If

        If txtMercanciaADevolver.Text.Trim = "" Then
            ft.mensajeCritico("DEBE INDICAR UNA MERCANCIA A DEVOLVER VALIDA...")
            Return False
        Else
            If ft.DevuelveScalarCadena(MyConn, " select item from jsvenrenpos where numfac = '" & txtNumeroFactura.Text & "' and item = '" & txtCodigoMercanciaADevolver.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "" Then
                ft.mensajeCritico("DEBE INDICAR UNA MERCANCIA A DEVOLVER PERTENECIENTE A ESTA FACTURA VALIDA...")
                Return False

            End If
        End If

        If txtMercanciaSalida.Text.Trim = "" Then
            ft.mensajeCritico("DEBE INDICAR UNA MERCANCIA DE SALIDA VALIDA...")
            Return False
        Else
            If CBool(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM31")) Then
                If txtCodigoMercanciaADevolver.Text <> txtCodigoMercanciaSalida.Text Then
                    ft.mensajeCritico("LA MERCANCIA DE SALIDA DEBE SER EXACTAMENTE IGUAL A LA FACTURADA...")
                    Return False
                Else
                    If ValorNumero(txtCantidadADevolver.Text) <> ValorNumero(txtCantidadSalida.Text) Then
                        ft.mensajeCritico("LA CANTIDAD DE MERCANCIA DE SALIDA DE SER IGUAL A LA FACTURADA...")
                        Return False
                    End If
                End If
            End If
        End If

        If Not MultiploValido(MyConn, txtCodigoMercanciaSalida.Text, txtUnidadSalida.Text, ValorNumero(txtCantidadSalida.Text), lblInfo) Then
            ft.mensajeCritico("LA CANTIDAD DE SALIDA NO ES MULTIPLO VALIDO...")
            Return False
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then

            Dim NumeroDeRecambio As String = Contador(MyConn, lblInfo, Gestion.iPuntosdeVentas, "POSNUMREC", "04")
            Dim PesoADevolver As Double = ValorCantidad(txtCantidadADevolver.Text) * ft.DevuelveScalarDoble(MyConn, " select peso/cantidad from jsvenrenpos where numfac = '" & txtNumeroFactura.Text & "' and item = '" & txtCodigoMercanciaADevolver.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Dim CostoDevolucion As Double = ValorCantidad(txtCantidadADevolver.Text) * ft.DevuelveScalarDoble(MyConn, " select montoultimacompra from jsmerctainv where codart = '" & txtCodigoMercanciaADevolver.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / Equivalencia(MyConn, txtCodigoMercanciaADevolver.Text, txtUnidadADevolver.Text)

            'MERCANCIA DE ENTRADA
            InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, txtCodigoMercanciaADevolver.Text, jytsistema.sFechadeTrabajo, "EN", _
                                                  NumeroDeRecambio, txtUnidadADevolver.Text, ValorCantidad(txtCantidadADevolver.Text), PesoADevolver, CostoDevolucion, CostoDevolucion, "PVE", NumeroDeRecambio, "", "00000000", _
                                                  0.0, 0.0, 0.0, 0.0, numCajero, AlmacenSalida, "000010", jytsistema.sFechadeTrabajo)

            'MERCANCIA DE SALIDA
            Dim PesoSalida As Double = ValorCantidad(txtCantidadSalida.Text) * ft.DevuelveScalarDoble(MyConn, " select pesounidad from jsmerctainv where codart = '" & txtCodigoMercanciaSalida.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / Equivalencia(MyConn, txtCodigoMercanciaSalida.Text, txtUnidadSalida.Text)
            Dim CostoSalida As Double = ValorCantidad(txtCantidadSalida.Text) * ft.DevuelveScalarDoble(MyConn, " select montoultimacompra from jsmerctainv where codart = '" & txtCodigoMercanciaSalida.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / Equivalencia(MyConn, txtCodigoMercanciaSalida.Text, txtUnidadSalida.Text)

            InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, txtCodigoMercanciaSalida.Text, jytsistema.sFechadeTrabajo, "SA", _
                                                 NumeroDeRecambio, txtUnidadSalida.Text, ValorCantidad(txtCantidadSalida.Text), PesoSalida, CostoSalida, CostoSalida, "PVE", NumeroDeRecambio, "", "00000000", _
                                                 CostoSalida, CostoSalida, 0.0, 0.0, numCajero, AlmacenSalida, "000010", jytsistema.sFechadeTrabajo)


            ft.mensajeInformativo("RECAMBIO REALIZADO CON EXITO!!!!")


            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        
        Me.Close()
    End Sub

    Private Sub btnFacturas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFacturas.Click
        txtNumeroFactura.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, _
                                                               "select a.numfac codigo, a.nomcli descripcion, a.rif, a.emision from jsvenencpos a where a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -1, jytsistema.sFechadeTrabajo)) & "' and  estatus = " & EstatusFactura.eProcesada & " and id_emp = '" & jytsistema.WorkID & "' order by emision desc, numfac desc ", _
                                                               "FACTURAS PUNTO DE VENTA", txtNumeroFactura.Text)
    End Sub

    Private Sub btnMercanciaDevolucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaDevolucion.Click
        txtCodigoMercanciaADevolver.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, "  SELECT a.item codigo, a.descrip descripcion, a.barras from jsvenrenpos a " _
                                                              & " where a.numfac = '" & txtNumeroFactura.Text & "' and a.id_emp = '" & jytsistema.WorkID & "' order by renglon ", "MERCANCIAS DE FACTURA " & txtNumeroFactura.Text, txtCodigoMercanciaADevolver.Text)
    End Sub

    Private Sub btnMercanciaSalida_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaSalida.Click
        txtCodigoMercanciaSalida.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, "  SELECT a.codart codigo, a.nomart descripcion, a.barras from jsmerctainv a " _
                                                              & " where estatus = 0 and a.id_emp = '" & jytsistema.WorkID & "' order by a.codart ", "MERCANCIAS  ", txtCodigoMercanciaSalida.Text)
    End Sub

    Private Sub txtCodigoMercanciaADevolver_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoMercanciaADevolver.TextChanged
        txtMercanciaADevolver.Text = ft.DevuelveScalarCadena(MyConn, " SELECT DESCRIP FROM jsvenrenpos WHERE ITEM = '" & txtCodigoMercanciaADevolver.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
        txtUnidadADevolver.Text = ft.DevuelveScalarCadena(MyConn, " SELECT UNIDAD FROM jsvenrenpos WHERE ITEM = '" & txtCodigoMercanciaADevolver.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
        txtCantidadADevolver.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(MyConn, " SELECT CANTIDAD FROM jsvenrenpos WHERE ITEM = '" & txtCodigoMercanciaADevolver.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' "))

        txtCodigoMercanciaSalida.Text = txtCodigoMercanciaADevolver.Text

    End Sub



    Private Sub txtCodigoMercanciaSalida_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoMercanciaSalida.TextChanged
        txtMercanciaSalida.Text = ft.DevuelveScalarCadena(MyConn, " SELECT NOMART FROM jsmerctainv WHERE codart = '" & txtCodigoMercanciaSalida.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
        txtUnidadSalida.Text = ft.DevuelveScalarCadena(MyConn, " SELECT UNIDADDETAL FROM jsmerctainv WHERE codart = '" & txtCodigoMercanciaSalida.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtCantidadADevolver_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCantidadADevolver.TextChanged
        txtCantidadSalida.Text = txtCantidadADevolver.Text
    End Sub
End Class