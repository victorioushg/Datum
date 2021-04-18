Imports MySql.Data.MySqlClient
Public Class jsContabArcAsientosPlantillaMovimientos
    Private Const sModulo As String = "Movimiento plantilla asiento contable"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private CodigoAsiento As String
    Private n_Apuntador As Long
    Private numRenglon As String = ""
    Private aSigno() As String = {"+", "-"}
    Private strConjunto As String = ""
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Asiento As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoAsiento = Asiento
        If dt.Rows.Count = 0 Then Apuntador = -1
        IniciarTXT()
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, CodigoAsiento)
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        txtCodigoCuenta.Text = ""
        txtReferencia.Text = ""
        txtConcepto.Text = ""
        txtPlantilla.Text = ""
        ft.RellenaCombo(aSigno, cmbSigno)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Asiento As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoAsiento = Asiento
        AsignarTXT(Apuntador)
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoAsiento)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt.Rows(nPosicion)
            txtCodigoCuenta.Text = .Item("codcon").ToString
            txtReferencia.Text = IIf(IsDBNull(.Item("Referencia")), "", .Item("Referencia"))
            txtConcepto.Text = IIf(IsDBNull(.Item("concepto")), "", .Item("concepto"))
            txtPlantilla.Text = IIf(IsDBNull(.Item("regla")), "", .Item("regla"))
            numRenglon = .Item("renglon")
            ft.RellenaCombo(aSigno, cmbSigno, .Item("signo"))
        End With
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ' If i_modo = movimiento.iAgregar Then _
        'ft.Ejecutar_strSQL ( myconn, "delete from jscotrendef where asiento = '" & txtCodigoCuenta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If txtCodigoCuenta.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una cuenta válida...")
            Exit Function
        Else
        End If

        If txtConcepto.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un concepto válido...")
            txtConcepto.Focus()
            Exit Function
        End If

        'Dim aFLd() As String = {"codcon", "id_emp"}
        'Dim aSFld() As String = {txtCodigoCuenta.Text, jytsistema.WorkID}
        'If Not qFound(MyConn, lblInfo, "jscotcatcon", aFLd, aSFld) Then
        '    ft.mensajeAdvertencia( "Cuenta contable no válida ...")
        '    btnCuentas.Focus()
        '    Exit Function
        'Else
        '    If Microsoft.VisualBasic.Right(txtCodigoCuenta.Text, 1) = "." Then
        '        ft.mensajeAdvertencia( "Cuenta contable no válida ...")
        '        Exit Function
        '    End If
        'End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Insertar As Boolean = False
        If Validado() Then
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dt.Rows.Count
                numRenglon = ft.autoCodigo(MyConn, "RENGLON", "jscotrendef", "asiento.id_emp", CodigoAsiento + "." + jytsistema.WorkID, 5)

            End If
            InsertEditCONTABRenglonAsientoPlantilla(MyConn, lblInfo, Insertar, CodigoAsiento, numRenglon, txtCodigoCuenta.Text, _
                                            txtReferencia.Text, txtConcepto.Text, txtPlantilla.Text, _
                                            cmbSigno.SelectedIndex, "1")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoAsiento & " " & txtCodigoCuenta.Text)
            Me.Close()
        End If

    End Sub

    Private Sub btnCuentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCuentas.Click
        txtCodigoCuenta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", _
                                                  " Listado de Cuentas Contables", txtCodigoCuenta.Text)
    End Sub
    Private Sub txtCodigoCuenta_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigoCuenta.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique o escoja una cuenta contable válida...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtReferencia_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtReferencia.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la referencia ó Número de origen contable ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el concepto por el cual se realiza este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtCodigoCuenta_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoCuenta.TextChanged
        Dim aFld() As String = {"codcon", "id_emp"}
        Dim aSFld() As String = {txtCodigoCuenta.Text, jytsistema.WorkID}
        lblCuenta.Text = qFoundAndSign(MyConn, lblInfo, "jscotcatcon", aFld, aSFld, "descripcion").ToString
    End Sub

    Private Sub txtPlantilla_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPlantilla.TextChanged
        Dim aFld() As String = {"plantilla", "id_emp"}
        Dim aSFld() As String = {txtPlantilla.Text, jytsistema.WorkID}
        txtDescripcionPlantilla.Text = qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aFld, aSFld, "referencia").ToString
        If i_modo = MovAud.iIncluir Then txtConcepto.Text = qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aFld, aSFld, "referencia").ToString
        strConjunto = qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aFld, aSFld, "conjunto").ToString

    End Sub

    Private Sub jsContabArcAsientosPlantillaMovimientos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsContabArcAsientosPlantillaMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        ft.habilitarObjetos(False, True, txtPlantilla)
        ft.enfocarTexto(txtCodigoCuenta)
        txtCodigoCuenta.Focus()
    End Sub

    Private Sub btnPlantilla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlantilla.Click
        txtPlantilla.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select plantilla codigo, referencia descripcion from jscotcatreg where id_emp = '" & jytsistema.WorkID & "' order by 1 ", _
                                               " Reglas de contabilización  ", txtPlantilla.Text)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnReferencia.Click

        Dim f As New jsControlArcTablaSimple
        Dim nTablaCampos As String = "tblcampos"
        If strConjunto <> "" Then
            Dim dtCampos As DataTable = CamposTabla(MyConn, strConjunto, nTablaCampos)
            f.Cargar("Campos del conjunto ", ds, dtCampos, nTablaCampos, TipoCargaFormulario.iShowDialog, False)
            txtReferencia.Text += " {" + f.Seleccion + "}"
            f = Nothing
            dtCampos.Dispose()
            dtCampos = Nothing

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

    Private Sub btnConcepto_Click(sender As System.Object, e As System.EventArgs) Handles btnConcepto.Click
        Dim f As New jsControlArcTablaSimple
        Dim nTablaCampos As String = "tblcampos"
        If strConjunto <> "" Then
            Dim dtCampos As DataTable = CamposTabla(MyConn, strConjunto, nTablaCampos)
            f.Cargar("Campos del conjunto ", ds, dtCampos, nTablaCampos, TipoCargaFormulario.iShowDialog, False)
            txtConcepto.Text += " {" + f.Seleccion + "}"
            f = Nothing
            dtCampos.Dispose()
            dtCampos = Nothing

        End If
    End Sub

    Private Sub txtReferencia_TextChanged(sender As Object, e As EventArgs) Handles txtReferencia.TextChanged

    End Sub
End Class