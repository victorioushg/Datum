Imports MySql.Data.MySqlClient
Public Class jsContabArcReglasMovimientos
    Private Const sModulo As String = "Movimientos de reglas de contabilización"
    Private Const nTabla As String = "tblReglas"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer

    Private n_Apuntador As Long
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

        ft.habilitarObjetos(False, True, txtConcepto, txtConjunto, txtConjuntoNombre)

        MyConn = MyCon
        ds = dsR
        dt = dtR
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        
        txtConcepto.Text = ft.autoCodigo(MyConn, "plantilla", "jscotcatreg", "id_emp", jytsistema.WorkID, 5)
        txtDescripcion.Text = ""
        txtConjunto.Text = ""
        txtFormula.Text = ""
        txtCondicion.Text = ""

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsR As DataSet, ByVal dtR As DataTable)
        i_modo = movimiento.iEditar

        ft.habilitarObjetos(False, True, txtConcepto, txtConjunto, txtConjuntoNombre)

        MyConn = MyCon
        ds = dsR
        dt = dtR
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        With dt.Rows(nPosicion)

            txtConcepto.Text = .Item("plantilla")
            txtDescripcion.Text = .Item("referencia")
            txtConjunto.Text = .Item("conjunto")
            txtFormula.Text = .Item("formula")
            txtCondicion.Text = .Item("condicion")
            txtAgrupadoPor.Text = .Item("agrupadopor")

        End With
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCamposF, "Agregar <B>campo</B> tabla ")
        C1SuperTooltip1.SetToolTip(btnFormulasF, "Agregar <B>fórmula</B> preestablecida ")
        C1SuperTooltip1.SetToolTip(btnConceptosF, "Agregar <B>regla</B> ")
        C1SuperTooltip1.SetToolTip(btnConstantesF, "<B>Agregar</B> constante")
        C1SuperTooltip1.SetToolTip(btnCamposC, "Agregar <B>campo</B> tabla ")
        C1SuperTooltip1.SetToolTip(btnFormulasC, "Agregar <B>fórmula</B> preestablecida ")
        C1SuperTooltip1.SetToolTip(btnConceptosC, "Agregar <B>regla</B> ")
        C1SuperTooltip1.SetToolTip(btnConstantesC, "<B>Agregar</B> constante")

        C1SuperTooltip1.SetToolTip(btnconjunto, " seleccione <B>conjunto</B> de datos para la formulación de esta regla...")

    End Sub
    Private Sub jsContabArcReglasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ds = Nothing
        dt = Nothing
    End Sub

    Private Sub jsContabArcReglasMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtConcepto.Text)

        Me.Tag = sModulo
        AsignarTooltips()

    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus, _
         txtDescripcion.GotFocus, txtCondicion.GotFocus, txtFormula.GotFocus, _
        txtConjunto.GotFocus, btnconjunto.GotFocus

        Select Case sender.name
            Case "txtConcepto"
                ft.mensajeEtiqueta(lblInfo, " Indique el código de la regla ... ", Transportables.TipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, " Indique la descripción de la regla ...", Transportables.TipoMensaje.iInfo)
            Case "txtConjunto", "btnconjunto"
                ft.mensajeEtiqueta(lblInfo, " Indique o seleccione el conjunto con el cual se desarrollará la fórmula y la condición de la regla ...", Transportables.TipoMensaje.iInfo)
            Case "cmbTipo"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el tipo de regla ...", Transportables.TipoMensaje.iInfo)
            Case "txtFormula"
                ft.mensajeEtiqueta(lblInfo, " Indique la fórmula con la cual se cálculará la regla ...", Transportables.TipoMensaje.iInfo)
            Case "txtCondicion"
                ft.mensajeEtiqueta(lblInfo, " Indique la condición o condiciones para desarrollar la fórmula de esta regla ...", Transportables.TipoMensaje.iInfo)
            Case "txtAgrupadoPor"
                ft.mensajeEtiqueta(lblInfo, " Indique la forma de agrupación para la fórmula de la regla ...", Transportables.TipoMensaje.iInfo)
            Case "cmbEstatus"
                ft.mensajeEtiqueta(lblInfo, " Indique el estatus de la regla ... ", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False
        If Trim(txtConcepto.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un codigo de regla válida ...")
            txtConcepto.Focus()
            Exit Function
        Else
            If i_modo = movimiento.iAgregar Then
                Dim afld() As String = {"plantilla", "id_emp"}
                Dim aFldN() As String = {txtConcepto.Text, jytsistema.WorkID}
                If qFound(MyConn, lblInfo, "jscotcatreg", afld, aFldN) Then
                    ft.mensajeAdvertencia("Código de regla YA se encuentra en archivo...")
                    txtConcepto.Focus()
                    Exit Function
                End If
            End If
        End If

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción para la regla ...")
            txtConcepto.Focus()
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
            InsertEditCONTABRegla(MyConn, lblInfo, Insertar, txtConcepto.Text, txtDescripcion.Text, _
                                      "", txtConjunto.Text, txtCondicion.Text, txtFormula.Text, txtAgrupadoPor.Text, "")

            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtConjunto_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConjunto.TextChanged
        If txtConjunto.Text <> "" Then
            Dim aFld() As String = {"codigo", "gestion", "id_emp"}
            Dim aFldN() As String = {txtConjunto.Text, Gestion.iContabilidad, jytsistema.WorkID}
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
                Dim nnTabla As String = .Item("tabla")
                ds = DataSetRequery(ds, " show full columns from " + nnTabla + " ", MyConn, nTablaCampos, lblInfo)

                dtCampos = ds.Tables(nTablaCampos)
                For Each nRow As DataRow In dtCampos.Rows
                    With nRow
                        ft.Ejecutar_strSQL(MyConn, "insert into " + tblfld + " (nombrecampo, descripcion ) " _
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
    Private Sub btnConstantesF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConstantesF.Click, _
        btnConstantesC.Click
        ' Dim f As New jsControlArcTablaSimple
        ' Dim nTableC As String = "tblConstantes"
        ' Dim dtTablaC As DataTable
        '
        '       ds = DataSetRequery(ds, " select constante codigo, valor descripcion from jsnomcatcot where id_emp = '" & jytsistema.WorkID & "' order by constante ", MyConn, nTableC)
        '       dtTablaC = ds.Tables(nTableC)
        '       f.Cargar(" Constantes ", ds, dtTablaC, nTableC, TipoCargaFormulario.iShowDialog, False)
        '       If sender.name = "btnConstantesF" Then
        ' txtFormula.Text += " [" + f.Seleccion + "]"
        ' Else
        ' txtCondicion.Text += " [" + f.Seleccion + "]"
        ' End If
        '
        '       f.Dispose()
        '       dtTablaC.Dispose()
        '       dtTablaC = Nothing
        '       f = Nothing

    End Sub

    Private Sub btnconjunto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnconjunto.Click
        txtConjunto.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconcojcat where gestion = " & Gestion.iContabilidad & " and id_emp = '" & jytsistema.WorkID & "' order by codigo ", "Conjunto de datos...", _
                                             txtConjunto.Text)
    End Sub

    Private Sub btnFormulasF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFormulasF.Click

    End Sub

    Private Sub btnFormulasC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFormulasC.Click

    End Sub


End Class