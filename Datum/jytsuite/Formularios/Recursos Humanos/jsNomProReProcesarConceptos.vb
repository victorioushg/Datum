Imports MySql.Data.MySqlClient
Public Class jsNomProReProcesarConceptos
    Private Const sModulo As String = "Reprocesar conceptos de nómina"
    Private Const nTabla As String = "repronomina"

    Private strSQL As String = "" ' " select * from jsnomcatcon where id_emp = '" & jytsistema.WorkID & "' order by codnom, codcon  "

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables
    Public Property CodigoNomina As String
    Public Property CodigoConcepto As String
    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        ft.habilitarObjetos(False, True, txtNomina, txtConcepto)
        txtNomina.Text = CodigoNomina
        txtConcepto.Text = CodigoConcepto

        chkAsi.Checked = True
        chkDed.Checked = True
        chkAdi.Checked = True
        
        lblLeyenda.Text = " Mediante este proceso se reconstruyen los montos de los conceptos de nómina asignados a " + vbCr + _
                          " cada empleado, tomando en cuenta los nuevos valores asignados a las constantes y conceptos " + vbCr + _
                          " especiales. NO SE CAMBIARAN DE MODO ALGUNO LOS CONCEPTOS ESPECIALES. "

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.TipoMensaje.iAyuda)
        Me.ShowDialog()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsNomProProcesarNomina_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        PreparaCadenaSQL()
        ProcesarConceptos()
        Me.Close()
    End Sub
    Private Sub PreparaCadenaSQL()
        Dim str As String = IIf(txtNomina.Text.Trim <> "", " codnom = '" & txtNomina.Text & "' AND ", "")
        str += IIf(txtConcepto.Text.Trim <> "", " codcon = '" & txtConcepto.Text & "' AND ", "")

        strSQL += IIf(chkAsi.Checked, " select * " _
            & " from jsnomcatcon " _
            & " where " _
            & str _
            & " tipo = 0 AND " _
            & " id_emp = '" & jytsistema.WorkID & "' ", "")

        strSQL += IIf(strSQL.Trim <> "" And chkDed.Checked, " union  ", "")

        strSQL += IIf(chkDed.Checked, " select * " _
            & " from jsnomcatcon " _
            & " where " _
            & str _
            & " tipo = 1 AND " _
            & " id_emp = '" & jytsistema.WorkID & "' ", "")

        strSQL += IIf(strSQL.Trim <> "" And chkAdi.Checked, " union  ", "")

        strSQL += IIf(chkAdi.Checked, " select * " _
            & " from jsnomcatcon " _
            & " where " _
            & str _
            & " tipo = 2 AND " _
            & " id_emp = '" & jytsistema.WorkID & "' ", "")

        strSQL += IIf(strSQL.Trim <> "" And chkEsp.Checked, " union  ", "")

        strSQL += IIf(chkEsp.Checked, " select * " _
            & " from jsnomcatcon " _
            & " where " _
            & str _
            & " tipo = 3 AND " _
            & " id_emp = '" & jytsistema.WorkID & "' ", "")

        strSQL += IIf(strSQL.Trim <> "", " order by codnom, codcon  ", "")
        'ft.mensajeInformativo(strSQL)
    End Sub
    Private Sub ProcesarConceptos()
        Dim iCont As Integer = 0
        If strSQL.Trim <> "" Then
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            For iCont = 0 To dt.Rows.Count - 1
                With dt.Rows(iCont)
                    
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dt.Rows.Count * 100, "NOMINA : " + .Item("CODNOM") + " CONCEPTO : " + .Item("codcon") + " " + .Item("nomcon"))

                    Conceptos_A_Trabajadores(myConn, lblInfo, ds, .Item("CODCON"), .Item("CODNOM"), .Item("CONJUNTO"), .Item("ESTATUS"), _
                                             .Item("FORMULA"), .Item("CONDICION"))
                End With
            Next
        End If

        ft.mensajeInformativo("PROCESO CULMINADO ...")
        lblProgreso.Text = ""
        ProgressBar1.Value = 0

    End Sub
    
    Private Sub txtNomina_TextChanged(sender As Object, e As EventArgs) Handles txtNomina.TextChanged
        lblNomina.Text = ft.DevuelveScalarCadena(myConn, " SELECT DESCRIPCION FROM jsnomencnom where CODNOM = '" & txtNomina.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ").ToString
    End Sub

    Private Sub btnNomina_Click(sender As Object, e As EventArgs) Handles btnNomina.Click
        txtNomina.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codnom codigo, descripcion from jsnomencnom where id_emp = '" & jytsistema.WorkID & "' order by codnom ", "NOMINAS", txtNomina.Text)
        'ft.RellenaCombo(aTipoNomina, cmbipo, ft.DevuelveScalarEntero(myConn, " SELECT TIPONOM FROM jsnomencnom WHERE CODNOM = '" & txtNomina.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'  "))
    End Sub

    Private Sub btnConcepto_Click(sender As Object, e As EventArgs) Handles btnConcepto.Click
        txtConcepto.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, nomcon descripcion from jsnomcatcon " _
                                             & " where " _
                                             & IIf(txtNomina.Text <> "", " CODNOM = '" & txtNomina.Text & "' AND ", "") _
                                             & " id_emp = '" & jytsistema.WorkID & "' order by codnom ", "CONCEPTOS", txtConcepto.Text)

    End Sub

    Private Sub txtConcepto_TextChanged(sender As Object, e As EventArgs) Handles txtConcepto.TextChanged
        lblConcepto.Text = ft.DevuelveScalarCadena(myConn, " SELECT NOMCON FROM jsnomcatcon where CODCON = '" & txtConcepto.Text & "' AND CODNOM = '" & txtNomina.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ").ToString
    End Sub
End Class