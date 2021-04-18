Imports MySql.Data.MySqlClient
Public Class jsMerProCombos
    Private Const sModulo As String = "Construcción de COMBOS de mercancías"
    Private Const nTabla As String = "tblProcombos"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Private UnidadPPal As String = ""

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
      
        MensajeEtiqueta(lblInfo, "Seleccione el combo que desea construir ...", TipoMensaje.iAyuda)

        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        HabilitarObjetos(False, False, txtCodigo, txtAlmacen)
        txtCodigo.Text = ""
        txtAlmacen.Text = ""
        txtCantidad.Text = FormatoCantidad(1)
    End Sub
    Private Sub AbrirMovimientos()

        If txtAlmacen.Text <> "" AndAlso txtCodigo.Text <> "" Then

            dg.DataSource = Nothing

            strSQL = " SELECT  a.codart, a.codartcom, a.descrip, a.cantidad, a.unidad unidadcombo, a.peso*" & ValorCantidad(txtCantidad.Text) & " pesomayor, " _
                & " a.cantidad*" & ValorCantidad(txtCantidad.Text) & "/IF( c.uvalencia IS NULL, 1, c.equivale) CantidadMayor, " _
                & " if( d.unidad is null, 'UND', d.unidad ) unidadventa, " _
                & " b.existencia, d.unidad unidadexistencia, a.id_emp " _
                & " FROM jsmercatcom a " _
                & " LEFT JOIN jsmerextalm b ON ( a.codartcom = b.codart AND b.almacen = '" & txtAlmacen.Text & "'  AND a.id_emp = b.id_emp ) " _
                & " LEFT JOIN jsmerequmer c ON ( a.codartcom = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN jsmerctainv d ON ( a.codartcom = d.codart AND a.id_emp = d.id_emp) " _
                & " WHERE " _
                & " " _
                & " a.codart = '" & txtCodigo.Text & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "


            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            Dim aCampos() As String = {"codartcom", "descrip", "cantidad", "unidadcombo", "cantidadmayor", "unidadventa", "existencia", "unidadexistencia"}
            Dim aNombres() As String = {"Código", "Descripción", "Cantidad Combo", "UND", "Cantidad solicitada", "UND", "Existencia Almacén", "UND"}
            Dim aAnchos() As Long = {80, 300, 100, 35, 100, 35, 100, 35}
            Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                            AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                            AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                            AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro}
            Dim aFormatos() As String = {"", "", sFormatoCantidad, "", sFormatoCantidad, "", sFormatoCantidad, ""}
            IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)


            CalculaTotales()

        End If

    End Sub
    Private Sub CalculaTotales()
        If dt.Rows.Count > 0 Then
            Dim CodigoMaximo As String = "" ' dt.Rows(0).Item("codartcom")
            Dim CantidadMaxima As Double = 0 ' dt.Rows(0).Item("existencia")
            Dim unidadMaxima As String = "" ' dt.Rows(0).Item("unidadcombo")
            For Each nRow As DataRow In dt.Rows
                With nRow

                    If .Item("codartcom").ToString.Substring(0, 1) <> "$" Then

                        If .Item("existencia") > CantidadMaxima Then
                            CodigoMaximo = .Item("codartcom")
                            CantidadMaxima = .Item("existencia")
                            unidadMaxima = .Item("unidadcombo")
                        End If

                    End If

                End With
            Next
            lblCantidadMaxima.Text = "  Cantidad maxima para construir >>>> " & FormatoCantidad(CantidadDesquivalente(myConn, lblInfo, CodigoMaximo, unidadMaxima, CantidadMaxima)) & " " & unidadMaxima & " <<<<"
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsMerProProcesarConteo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub

    Private Sub jsMerProProcesarConteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        CrearCombos()
        MensajeInformativoPlus(" Proceso culminado ...")
        IniciarTXT()
        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub

    Private Sub CrearCombos()

        Dim NumeroMovimiento As String
        Dim PesoMercancia As Double, CostoMercancia As Double
        Dim CostoTotalCombo As Double, PesoTotalCombo As Double

        NumeroMovimiento = Contador(myConn, lblInfo, Gestion.iMercancías, "INVNUMMOV", "2")
        CostoTotalCombo = 0.0#
        If dt.Rows.Count > 0 Then
            For Each nRow As DataRow In dt.Rows
                With nRow

                    PesoMercancia = .Item("pesomayor")
                    CostoMercancia = CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " SELECT montoultimacompra from jsmerctainv where codart = '" & .Item("codartcom") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * .Item("cantidadmayor")
                    CostoTotalCombo += CostoMercancia
                    PesoTotalCombo += PesoMercancia

                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("codartcom"), jytsistema.sFechadeTrabajo, "SA", NumeroMovimiento, _
                                            .Item("unidadventa"), .Item("cantidadmayor"), .Item("pesomayor"), CostoMercancia, _
                                            CostoMercancia, "INV", NumeroMovimiento, "", "COMBO", 0.0#, 0.0#, 0.0#, 0.0#, "", _
                                            txtAlmacen.Text, "", jytsistema.sFechadeTrabajo)


                    ActualizarExistenciasPlus(myConn, .Item("codartcom"))

                End With

            Next
        End If


        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, txtCodigo.Text, jytsistema.sFechadeTrabajo, "EN", NumeroMovimiento, _
                                             lblUNIDAD.Text, ValorCantidad(txtCantidad.Text), PesoTotalCombo, CostoTotalCombo, _
                                             CostoTotalCombo, "INV", NumeroMovimiento, "", "COMBO", 0.0#, 0.0#, 0.0#, 0.0#, "", _
                                             txtAlmacen.Text, "", jytsistema.sFechadeTrabajo)

        ActualizarExistenciasPlus(myConn, txtCodigo.Text)



    End Sub


    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        txtCodigo.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT a.codart codigo, a.nomart descripcion, COUNT(b.codartcom) cuenta " _
                                           & " FROM jsmerctainv a " _
                                           & " LEFT JOIN jsmercatcom b ON (a.codart =  b.codart AND a.id_emp = b.id_emp) " _
                                           & " WHERE " _
                                           & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                                           & " GROUP BY a.codart " _
                                           & " HAVING cuenta > 0 ", _
                                                   " COMBOS DE MERCANCIAS ...", txtCodigo.Text)
    End Sub


    Private Sub btnJerarquiaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtAlmacen.Text = f.Seleccionado
    End Sub


    Private Sub txtCodigo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCodigo.TextChanged
        lblNOMBRE.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT NOMART FROM jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "'")
        UnidadPPal = EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT UNIDAD FROM jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "'")
        lblUNIDAD.Text = UnidadPPal
        AbrirMovimientos()

    End Sub

    Private Sub txtAlmacen_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAlmacen.TextChanged
        lblALMACEN.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        AbrirMovimientos()
    End Sub

    Private Sub txtCantidad_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCantidad_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCantidad.TextChanged
        AbrirMovimientos()
    End Sub
End Class