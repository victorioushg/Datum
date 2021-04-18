Imports MySql.Data.MySqlClient
Public Class jsNomArcConceptosMovimientos
    Private Const sModulo As String = "Movimientos conceptos de nómina"
    Private Const nTabla As String = "conceptos"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private aTipo() As String = {"Asignación", "Deducción", "Adicional", "Especial"}
    Private aEstatus() As String = {"Inactivo", "Activo"}
    Private nPosicion As Integer

    Private n_Apuntador As Long
    Public Property CodigoNomina As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsR As DataSet, ByVal dtR As DataTable)
        i_modo = movimiento.iAgregar

        ft.habilitarObjetos(False, True, txtConcepto, txtConjunto, txtConjuntoNombre, txtCuota, txtCuotaNombre, _
                         txtProveedor, txtNombreProveedor, txtConceptoPorcentaje)

        MyConn = MyCon
        ds = dsR
        dt = dtR
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtConcepto.Text = ft.autoCodigo(MyConn, "codcon", "jsnomcatcon", "codnom.id_emp", CodigoNomina + "." + jytsistema.WorkID, 5, True)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtDescripcion, txtComentario, txtCuota, txtConjunto, _
                            txtFormula, txtCondicion)
        ft.RellenaCombo(aTipo, cmbTipo)
        ft.RellenaCombo(aEstatus, cmbEstatus, 1)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsR As DataSet, ByVal dtR As DataTable)
        i_modo = movimiento.iEditar

        ft.habilitarObjetos(False, True, txtConcepto, txtConjunto, txtConjuntoNombre, txtCuota, txtCuotaNombre, _
                         txtProveedor, txtNombreProveedor, txtConceptoPorcentaje)

        MyConn = MyCon
        ds = dsR
        dt = dtR
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        With dt.Rows(nPosicion)

            txtConcepto.Text = .Item("codcon")
            txtDescripcion.Text = .Item("nomcon")
            txtComentario.Text = .Item("descripcion")
            ft.RellenaCombo(aTipo, cmbTipo, CInt(.Item("tipo")))
            txtCuota.Text = .Item("cuota")
            txtConjunto.Text = .Item("conjunto")
            txtFormula.Text = .Item("formula")
            txtCondicion.Text = .Item("condicion")
            ft.RellenaCombo(aEstatus, cmbEstatus, CInt(.Item("estatus")))
            txtProveedor.Text = .Item("codpro")
            txtConceptoPorcentaje.Text = .Item("concepto_por_asig")
          
        End With
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCamposF, "Agregar <B>campo</B> tabla ")
        C1SuperTooltip1.SetToolTip(btnFormulasF, "Agregar <B>fórmula</B> preestablecida ")
        C1SuperTooltip1.SetToolTip(btnConceptosF, "Agregar <B>concepto</B> especial")
        C1SuperTooltip1.SetToolTip(btnConstantesF, "<B>Agregar</B> constante")
        C1SuperTooltip1.SetToolTip(btnCamposC, "Agregar <B>campo</B> tabla ")
        C1SuperTooltip1.SetToolTip(btnFormulasC, "Agregar <B>fórmula</B> preestablecida ")
        C1SuperTooltip1.SetToolTip(btnConceptosC, "Agregar <B>concepto</B> especial")
        C1SuperTooltip1.SetToolTip(btnConstantesC, "<B>Agregar</B> constante")

        C1SuperTooltip1.SetToolTip(btnconjunto, " seleccione <B>conjunto</B> de datos para la formulación de este concepto...")
        C1SuperTooltip1.SetToolTip(btnCuota, " seleccione <B>Cuota/Préstamo</B> asociado a este concepto ...")
        C1SuperTooltip1.SetToolTip(btnProveedor, " seleccione <B>Proveedor</B> de gasto asociado a este concepto ...")

    End Sub
    Private Sub jsNomArcConceptosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        ds = Nothing
        dt = Nothing
    End Sub

    Private Sub jsNomArcConceptosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Text += " Nómina : " + CodigoNomina + " " + ft.DevuelveScalarCadena(MyConn, " SELECT DESCRIPCION FROM jsnomencnom WHERE CODNOM = '" & CodigoNomina & "' AND ID_EMP = '" & jytsistema.WorkID & "' ").ToString

        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtConcepto.Text)
        Dim aCam() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {"00001", jytsistema.WorkID}
        If Not qFound(MyConn, lblInfo, "jsconcojcat", aCam, aStr) Then
            ft.Ejecutar_strSQL(myconn, " insert into jsconcojcat values('00001','Trabajadores', '',''," & Gestion.iRecursosHumanos & ", '" & jytsistema.WorkID & "') ")
            ft.Ejecutar_strSQL(myconn, " insert into jsconcojtab values('00001', 'a','jsnomcattra', 0, ''," & Gestion.iRecursosHumanos & ",'" & jytsistema.WorkID & "' ) ")
        End If

        AsignarTooltips()
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus, _
        cmbTipo.GotFocus, txtDescripcion.GotFocus, txtCondicion.GotFocus, txtFormula.GotFocus, _
        txtConjunto.GotFocus, btnconjunto.GotFocus, cmbEstatus.GotFocus

        Select Case sender.name
            Case "txtConcepto"
                ft.mensajeEtiqueta(lblInfo, " Indique el código del concepto ... ", Transportables.TipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, " Indique la Nombre del concepto ...", Transportables.tipoMensaje.iInfo)
            Case "txtComentario"
                ft.mensajeEtiqueta(lblInfo, " Indique la descripción, comentario respecto del concepto ...", Transportables.tipoMensaje.iInfo)
            Case "txtConjunto", "btnconjunto"
                ft.mensajeEtiqueta(lblInfo, " Indique o seleccione el conjunto con el cual se desarrollará la fórmula y la condición del conepto ...", Transportables.TipoMensaje.iInfo)
            Case "txtCuota", "btnCuota"
                ft.mensajeEtiqueta(lblInfo, " Indique o seleccione cuota/préstamo asiciado a este concepto ...", Transportables.TipoMensaje.iInfo)
            Case "btnProveedor"
                ft.mensajeEtiqueta(lblInfo, " Indique o seleccione el proveedor de gasto asiciado a este concepto ...", Transportables.TipoMensaje.iInfo)
            Case "cmbTipo"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el tipo de concepto ...", Transportables.TipoMensaje.iInfo)
            Case "txtFormula"
                ft.mensajeEtiqueta(lblInfo, " Indique la fórmula con la cual se cálculará el concepto ...", Transportables.TipoMensaje.iInfo)
            Case "txtCondicion"
                ft.mensajeEtiqueta(lblInfo, " Indique la condición o condiciones para desarrollar la fórmula de este concepto ...", Transportables.TipoMensaje.iInfo)
            Case "cmbEstatus"
                ft.mensajeEtiqueta(lblInfo, " Indique el estatus del concepto ... ", Transportables.TipoMensaje.iInfo)
            Case "btnNomina"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el código de nómina para la cual aplica este concepto ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False
        If Trim(txtConcepto.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un codigo concepto válido...")
            txtConcepto.Focus()
            Exit Function
        Else

            If i_modo = movimiento.iAgregar Then
                Dim afld() As String = {"codcon", "id_emp"}
                Dim aFldN() As String = {txtConcepto.Text, jytsistema.WorkID}
                If qFound(MyConn, lblInfo, "jsnomcatcon", afld, aFldN) Then
                    ft.mensajeAdvertencia("Código de concepto YA se encuentra en archivo...")
                    txtConcepto.Focus()
                    Exit Function
                End If
            End If

        End If


        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un NOMBRE para el concepto ...")
            txtConcepto.Focus()
            Exit Function
        End If

        If txtConjunto.Text.Trim = "" Then
            ft.mensajeAdvertencia("Debe indicar un CONJUNTO válido para el concepto ...")
            btnconjunto.Focus()
            Exit Function
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dt.Rows.Count
            End If

            InsertEditNOMINAConcepto(MyConn, lblInfo, Insertar, txtConcepto.Text, txtDescripcion.Text, _
                                       cmbTipo.SelectedIndex, txtCuota.Text, txtConjunto.Text, txtComentario.Text, txtFormula.Text, _
                                       txtCondicion.Text, txtAgrupadoPor.Text, txtProveedor.Text, cmbEstatus.SelectedIndex, _
                                       txtConceptoPorcentaje.Text, txtCodigoContable.Text, CodigoNomina)

            If txtFormula.Text <> "" And txtCondicion.Text <> "" And txtConjunto.Text <> "" Then _
                Conceptos_A_Trabajadores(MyConn, lblInfo, ds, txtConcepto.Text, CodigoNomina, txtConjunto.Text, cmbEstatus.SelectedIndex, _
                    txtFormula.Text, txtCondicion.Text)

            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtConjunto_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConjunto.TextChanged
        If txtConjunto.Text <> "" Then
            Dim aFld() As String = {"codigo", "gestion", "id_emp"}
            Dim aFldN() As String = {txtConjunto.Text, Gestion.iRecursosHumanos, jytsistema.WorkID}
            txtConjuntoNombre.Text = qFoundAndSign(MyConn, lblInfo, "jsconcojcat", aFld, aFldN, "descrip")
        End If

    End Sub


    Private Function CamposTabla(ByVal MyConn As MySqlConnection, ByVal Conjunto As String, _
                                 ByVal nTableFields As String) As DataTable

        Dim tblfld As String = "tblfields" & ft.NumeroAleatorio(10000)
        Dim dtTables As DataTable
        Dim dtCampos As DataTable
        Dim nTableTabla As String = "tablas"
        Dim nTablaCampos As String = "campos"

        ft.Ejecutar_strSQL(myconn, "drop table if exists " & tblfld)
        Dim aCam() As String = {"nombrecampo.cadena.50.0", "descripcion.cadena.250.0"}
        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tblfld, aCam)

        ds = DataSetRequery(ds, "SELECT a.letra, a.tabla " _
                & " FROM jsconcojtab a " _
                & " LEFT JOIN jsconcojcat b ON (a.codigo = b.codigo AND a.id_emp = b.id_emp) " _
                & " WHERE " _
                & " a.codigo = '" + Conjunto + "' AND " _
                & " a.id_emp = '" + jytsistema.WorkID + "' ", MyConn, nTableTabla, lblInfo)

        dtTables = ds.Tables(nTableTabla)

        Dim iCont As Integer = 0, jCont As Integer = 0
        For jCont = 0 To dtTables.Rows.Count - 1
            With dtTables.Rows(jCont)
                Dim letra As String = .Item("letra")
                ds = DataSetRequery(ds, " show full columns from " + .Item("tabla") + " ", MyConn, nTablaCampos, lblInfo)

                dtCampos = ds.Tables(nTablaCampos)
                For iCont = 0 To dtCampos.Rows.Count - 1
                    With dtCampos.Rows(iCont)
                        ft.Ejecutar_strSQL(myconn, "insert into " + tblfld + " (nombrecampo, descripcion ) " _
                                        & " values('" + letra + "." + .Item("field") + "', '" + .Item("comment") + "') ")
                    End With
                Next
            End With
        Next

        ds = DataSetRequery(ds, "select nombrecampo codigo, descripcion from " & tblfld & " order by nombrecampo ", MyConn, nTableFields, lblInfo)
        CamposTabla = ds.Tables(nTableFields)

    End Function
    Private Sub btnCamposF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCamposF.Click

        Dim f As New jsControlArcTablaSimple
        Dim nTablaCampos As String = "tblcampos"
        If txtConjunto.Text <> "" Then
            Dim dtCampos As DataTable = CamposTabla(MyConn, txtConjunto.Text, nTablaCampos)
            f.Cargar("Campos del conjunto " + txtConjuntoNombre.Text, ds, dtCampos, nTablaCampos, TipoCargaFormulario.iShowDialog, False)
            txtFormula.Text += " " + f.Seleccion
            f = Nothing
            dtCampos.Dispose()
            dtCampos = Nothing

        End If

    End Sub
    Private Sub btnCamposC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCamposC.Click
        Dim f As New jsControlArcTablaSimple
        Dim nTablaCampos As String = "tblcampos"

        If txtConjunto.Text <> "" Then
            Dim dtCampos As DataTable = CamposTabla(MyConn, txtConjunto.Text, nTablaCampos)
            f.Cargar("Campos del conjunto " + txtConjuntoNombre.Text, ds, dtCampos, nTablaCampos, TipoCargaFormulario.iShowDialog, False)
            txtCondicion.Text += " " + f.Seleccion
            f = Nothing
            dtCampos.Dispose()
            dtCampos = Nothing
        End If

    End Sub

    Private Sub btnConstantesF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConstantesF.Click, _
        btnConstantesC.Click

        Dim Constante As String = CargarTablaSimple(MyConn, lblInfo, ds, " select constante codigo, valor descripcion " _
                                                    & " from jsnomcatcot " _
                                                    & " where id_emp = '" & jytsistema.WorkID & "' order by constante ", "Constantes", "")
        If sender.name = "btnConstantesF" Then
            txtFormula.Text += " [" + Constante + "]"
        Else
            txtCondicion.Text += " [" + Constante + "]"
        End If

    End Sub

    Private Sub btnconjunto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnconjunto.Click
        txtConjunto.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconcojcat where gestion = " & Gestion.iRecursosHumanos & " and id_emp = '" & jytsistema.WorkID & "' order by codigo ", "Conjunto de datos...", _
                                             txtConjunto.Text)
    End Sub

    Private Sub btnFormulasF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFormulasF.Click
        Dim Formula As String = CargarTablaSimple(MyConn, lblInfo, ds, " select concat(formula,'(', parametros,')') codigo, descripcion from jsnomformula order by 1 ", "Formulas...", "")
        txtFormula.Text += " " + Formula
    End Sub

    Private Sub btnFormulasC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFormulasC.Click

    End Sub
    Private Sub btnCamposGrupo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCamposGrupo.Click
        Dim f As New jsControlArcTablaSimple
        Dim nTablaCampos As String = "tblcampos"

        If txtConjunto.Text <> "" Then
            Dim dtCampos As DataTable = CamposTabla(MyConn, txtConjunto.Text, nTablaCampos)
            f.Cargar("Campos del conjunto " + txtConjuntoNombre.Text, ds, dtCampos, nTablaCampos, TipoCargaFormulario.iShowDialog, False)
            txtAgrupadoPor.Text += " " + f.Seleccion
            f = Nothing
            dtCampos.Dispose()
            dtCampos = Nothing
        End If

    End Sub

    Private Sub btnConceptosF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConceptosF.Click, _
        btnConceptosC.Click

        Dim Concepto As String = CargarTablaSimple(MyConn, lblInfo, ds, " select codcon codigo, nomcon descripcion " _
                                                   & " from jsnomcatcon " _
                                                   & " where " _
                                                   & " codcon <> '" & txtConcepto.Text & "' and " _
                                                   & " id_emp = '" & jytsistema.WorkID & "' order by codcon ", "Conceptos", "")

        If sender.name = "btnConceptosF" Then
            txtFormula.Text += " {" + Concepto + "}"
        Else
            txtCondicion.Text += " {" + Concepto + "}"
        End If

    End Sub

    Private Sub txtCuota_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCuota.TextChanged
        Dim aCAM() As String = {"codigo", "modulo", "id_emp"}
        Dim aSTR() As String = {txtCuota.Text, FormatoTablaSimple(Modulo.iPrestamoCuotaNomina), jytsistema.WorkID}
        txtCuotaNombre.Text = qFoundAndSign(MyConn, lblInfo, "jsconctatab", aCAM, aSTR, "descrip")
        If txtCuotaNombre.Text.Trim() <> "" Then
            txtFormula.Text = "CuotaPrestamo( '" & CodigoNomina & "', @Trabajador,  '" & txtCuota.Text & "' , @Empresa)"
            txtFormula.Enabled = False
            MenuFormula.Enabled = False
        Else
            txtFormula.Text = ""
            txtFormula.Enabled = True
            MenuFormula.Enabled = True
        End If
    End Sub

    Private Sub btnCuota_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCuota.Click
        txtCuota.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iPrestamoCuotaNomina) & "' and id_emp = '" & jytsistema.WorkID & "' order by codigo ", " Cuotas/Prestamos ...", txtCuota.Text)
    End Sub

    Private Sub btnProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click
        txtProveedor.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where estatus = 0 AND TIPO = 1 AND id_emp = '" & jytsistema.WorkID & "' order by codigo ", " Proveedores de gastos ...", txtProveedor.Text)
    End Sub

    Private Sub txtProveedor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProveedor.TextChanged
        Dim aCAM() As String = {"codpro", "tipo", "id_emp"}
        Dim aSTR() As String = {txtProveedor.Text, 1, jytsistema.WorkID}
        txtNombreProveedor.Text = qFoundAndSign(MyConn, lblInfo, "jsprocatpro", aCAM, aSTR, "nombre")
    End Sub

    Private Sub btnConceptoPorcentaje_Click(sender As System.Object, e As System.EventArgs) Handles btnConceptoPorcentaje.Click
        If cmbTipo.SelectedIndex = 1 Or cmbTipo.SelectedIndex = 2 Then
            txtConceptoPorcentaje.Text = CargarTablaSimple(MyConn, lblInfo, ds, " SELECT CODCON codigo, NOMCON descripcion from jsnomcatcon where CODNOM = '" & CodigoNomina & "' AND id_emp = '" & jytsistema.WorkID & "' order by codcon ", " CONCEPTOS DE NOMINA ", txtConceptoPorcentaje.Text)
        End If
    End Sub

    Private Sub btnCodigoContable_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by codcon  ", "CUENTAS CONTABLES", txtCodigoContable.Text)
    End Sub


End Class