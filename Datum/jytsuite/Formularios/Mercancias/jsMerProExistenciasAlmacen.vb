Imports MySql.Data.MySqlClient
Public Class jsMerProExistenciasAlmacen
    Private Const sModulo As String = "Existencias para almacenistas"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "tblExisteAlmacen"

    Private strSQL As String = "select * from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart "
    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Long
    Private aCondicion() As String = {"Activo", "Inactivo"}

    Private Sub jsMerProExistenciasAlmacen_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerProExistenciasAlmacen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            ft.RellenaCombo(aCondicion, cmbCondicion)
            ft.RellenaCombo(aUnidad, cmbUnidad)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicion = 0
                Me.BindingContext(ds, nTabla).Position = nPosicion
                AsignaTXT(nPosicion)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseada")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a registro <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
    End Sub

    
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicion = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = .Item("codart")
                txtAlterno.Text = ft.muestraCampoTexto(.Item("alterno"))
                txtBarras.Text = ft.muestraCampoTexto(.Item("barras"))
                txtDescripcion.Text = ft.muestraCampoTexto(.Item("nomart"))
                txtCategoria.Text = ft.muestraCampoTexto(.Item("grupo"))
                txtMarca.Text = ft.muestraCampoTexto(.Item("marca"))
                txtDivision.Text = ft.muestraCampoTexto(.Item("division"))
                txtJerarquia.Text = ft.muestraCampoTexto(.Item("tipjer"))
                txtPresentacion.Text = ft.muestraCampoTexto(.Item("presentacion"))

                Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
                ft.RellenaCombo(aIVA, cmbIVA, ft.InArray(aIVA, .Item("iva")))

                Dim aUbica() As String = Split(.Item("ubicacion"), ".")
                txtUbica1.Text = ""
                txtUbica2.Text = ""
                txtUbica3.Text = ""
                If aUbica.Length = 1 Then txtUbica1.Text = aUbica(0)
                If aUbica.Length = 2 Then txtUbica2.Text = aUbica(1)
                If aUbica.Length = 3 Then txtUbica3.Text = aUbica(2)

                cmbCondicion.SelectedIndex = .Item("estatus")

                cmbUnidad.SelectedIndex = ft.InArray(aUnidadAbreviada, .Item("unidad"))
                txtPesoUnidad.Text = IIf(IsDBNull(.Item("pesounidad")), ft.FormatoCantidad(0.0), ft.FormatoCantidad(.Item("pesounidad")))

                'Equivalencias
                Dim nTablaEquivalencias As String = "tblEquivale"
                Dim dtEquivalencias As DataTable
                Dim strSQLEQu As String = " select codart, unidad, equivale, uvalencia, elt(divide + 1,'No','Si') divide, id_emp from jsmerequmer " _
                        & " where codart = '" & .Item("codart") & "' and " _
                        & " unidad = '" & .Item("unidad") & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' order by UVALENCIA "
                ds = DataSetRequery(ds, strSQLEQu, myConn, nTablaEquivalencias, lblInfo)
                dtEquivalencias = ds.Tables(nTablaEquivalencias)

                Dim aCamEqu() As String = {"equivale", "uvalencia", "divide"}
                Dim aNomEqu() As String = {"Equivale", "UND", "Divide"}
                Dim aAncEqu() As Integer = {110, 30, 30}
                Dim aAliEqu() As Integer = {AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
                Dim aForEqu() As String = {sFormatoCantidadLarga, "", ""}

                IniciarTabla(dgEqu, dtEquivalencias, aCamEqu, aNomEqu, aAncEqu, aAliEqu, aForEqu)

                ExistenciasPor(.Item("codart"), 0, 3)

            End With
        End With
    End Sub

    Private Sub ExistenciasPor(ByVal nArticulo As String, ByVal por As Integer, ByVal nMeses As Integer)

        Dim dtExPor As DataTable
        Dim nTablaExPor As String = "tablaexPor"

        Dim str1 As String = ""
        Dim str2 As String = ""
        Dim str3 As String = ""
        Dim str4 As String = ""
        Select Case por
            Case 0 ' Por almacen 
                str1 = " a.almacen codigo, e.desalm descripcion, "
                str2 = " b.almacen, "
                str3 = " left join jsmercatalm e ON (a.almacen = e.codalm AND a.id_emp = e.id_emp )  "
                str4 = " GROUP BY 1,2 "
            Case 1 'Por LOTE
                str1 = " a.lote codigo, e.expiracion descripcion,  "
                str2 = " b.lote, "
                str3 = " left join jsmerlotmer e on (a.lote = e.lote and a.id_emp = e.id_emp ) "
                str4 = " group by 1,2 "
            Case Else
        End Select

        ds = DataSetRequery(ds, " SELECT a.codart, " & str1 & " c.unidad, SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) existencia , " _
            & " round(c.pesounidad*SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)),3) pesototal, " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)),3) ventasperiodo,  d.diashabiles, " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles, 3) promedioDiario,  " _
            & " ROUND(SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles * c.pesounidad,3) promedioDiarioPeso , " _
            & " SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) / (  SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles ) Inventario " _
            & " FROM (SELECT  b.codart, " & str2 & " b.unidad,  " _
            & " SUM(IF( b.TIPOMOV IN( 'EN', 'AE', 'DV') , b.CANTIDAD, -1 * b.CANTIDAD )) existencia, " _
            & " SUM(IF( b.origen IN ('FAC', 'PVE', 'PFC') AND b.fechamov >= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH) AND  b.fechamov <= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL 1 DAY) ,  b.cantidad, 0.000 ) ) ventas, " _
            & " " _
            & " b.id_emp " _
            & " FROM jsmertramer b " _
            & " WHERE " _
            & "      b.tipomov <> 'AC' AND " _
            & "      b.codart = '" & nArticulo & "' AND " _
            & "      b.id_emp = '" & jytsistema.WorkID & "' AND  " _
            & "      b.ejercicio = '" & jytsistema.WorkExercise & "' AND  " _
            & "      date_format(b.fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " _
            & "      GROUP BY b.codart, " & str2 & " b.unidad ) a  " _
            & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp )  " _
            & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT '1' num, (CURRENT_DATE - DATE_ADD(CURRENT_DATE, INTERVAL -" & nMeses & " MONTH)) -  IFNULL(COUNT(*),0) DiasHabiles " _
            & " FROM (SELECT CONCAT(ano,'-',IF(LENGTH(mes)=1,CONCAT('0',mes),mes),'-',IF(LENGTH(dia)=1,CONCAT('0',dia),dia)) AS fecha FROM jsconcatper WHERE MODULO = 1 AND ID_EMP = '" & jytsistema.WorkID & "'  " _
            & " HAVING  (fecha <CURRENT_DATE AND fecha>DATE_ADD(CURRENT_DATE,INTERVAL -" & nMeses & " MONTH)) )  a) d ON ( d.num = '1') " _
            & str3 _
            & str4, myConn, nTablaExPor, lblInfo)

        dtExPor = ds.Tables(nTablaExPor)

        Dim aCamEx() As String = {"codigo", "descripcion", "existencia", "pesototal", "inventario", ""}
        Dim aNomEx() As String = {"Código", "Descripción", "Existencia en Unidades Venta", "Existencia en Kilogramos", "Dias de inventario", ""}
        Dim aAncEx() As Integer = {60, 140, 120, 120, 80, 100}
        Dim aAliEx() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                   AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aForEx() As String = {"", "", sFormatoCantidad, sFormatoCantidad, sFormatoEntero, ""}

        IniciarTabla(dgExistencias, dtExPor, aCamEx, aNomEx, aAncEx, aAliEx, aForEx)

    End Sub


    Private Sub DesactivarMarco0()

        ft.habilitarObjetos(False, True, txtCodigo, txtDescripcion, txtAlterno, txtBarras, txtCategoria, txtMarca, txtDivision, _
                         txtJerarquia, txtIVA, txtUbica1, txtUbica2, txtUbica3, cmbIVA, cmbUnidad, cmbCondicion, txtPesoUnidad, _
                         txtPresentacion)

        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicion)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        ft.Ejecutar_strSQL(myconn, " delete from jsmerrentra where numtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Function Validado() As Boolean
        Validado = True
    End Function


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codart", "nomart", "alterno", "barras"}
        Dim Nombres() As String = {"Código", "Descripción", "Alterno", "Código Barras"}
        Dim Anchos() As Integer = {150, 650, 150, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Existencias en almacén...")
        AsignaTXT(f.Apuntador)
        f = Nothing
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        '     Dim f As New jsMerRepParametros
        '     f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cTransferencia, "Transferencia/Nota de Consumo de mercancías", txtCodigo.Text)
        '     '        f = Nothing
    End Sub
    Private Sub cmbIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIVA.SelectedIndexChanged
        txtIVA.Text = ft.FormatoNumero(PorcentajeIVA(myConn, lblInfo, jytsistema.sFechadeTrabajo, cmbIVA.Text)) & "%"
    End Sub

    Private Sub txtCategoria_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoria.TextChanged
        lblCategoria.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iCategoriaMerca) & "' and codigo = '" & txtCategoria.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtMarca_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMarca.TextChanged
        lblMarca.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iMarcaMerca) & "' and codigo = '" & txtMarca.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtDivision_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDivision.TextChanged
        lblDivision.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsmercatdiv where division = '" & txtDivision.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtJerarquia_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJerarquia.TextChanged
        lblJerarquia.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsmerencjer where  tipjer = '" & txtJerarquia.Text & "' and id_emp ='" & jytsistema.WorkID & "' ")
    End Sub

   
End Class