Imports MySql.Data.MySqlClient
Public Class jsCambioEjercicio
    Private Const sModulo As String = "Cambio de ejercicio"
    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private ft As New Transportables

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon

        ft.mensajeEtiqueta(lblInfo, " haga click en aceptar para cambiar efectivamente el ejercicio de trabajo  ... ", Transportables.TipoMensaje.iAyuda)

        lblLeyenda.Text = " Si desea cambiar el ejercicio de trabajo debe seleccionar el ejercicio " + vbCr + _
                " deseado y luego hacer click en aceptar.  "

        EjercicioActual()
        IniciarEjercicios(myConn)
        Me.Show()

    End Sub
    Private Sub EjercicioActual()
        Dim nEjercicio As String = IIf(jytsistema.WorkExercise = "", "00000", jytsistema.WorkExercise)
        If nEjercicio = "00000" Then
            Dim aCampos() As String = {"id_emp"}
            Dim aStrings() As String = {jytsistema.WorkID}
            lblProgreso.Text = "00000 | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaemp", aCampos, aStrings, "inicio").ToString)) & _
            " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaemp", aCampos, aStrings, "cierre").ToString))
        Else
            Dim aFld() As String = {"ejercicio", "id_emp"}
            Dim aStr() As String = {nEjercicio, jytsistema.WorkID}
            lblProgreso.Text = nEjercicio & " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaeje", aFld, aStr, "inicio").ToString)) & _
                        " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaeje", aFld, aStr, "cierre").ToString))
        End If

    End Sub
    Private Sub IniciarEjercicios(ByVal MyCon As MySqlConnection)
        Dim eCont As Integer
        Dim nTablaE As String = "tEjercicio"
        ds = DataSetRequery(ds, " select * from jsconctaeje where id_emp = '" & jytsistema.WorkID & "' order by ejercicio desc ", myConn, nTablaE, lblInfo)
        Dim aEjercicio(ds.Tables(nTablaE).Rows.Count) As String
        Dim aCampos() As String = {"id_emp"}
        Dim aStrings() As String = {jytsistema.WorkID}
        aEjercicio(0) = "00000 | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaemp", aCampos, aStrings, "inicio").ToString)) & _
            " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaemp", aCampos, aStrings, "cierre").ToString))
        With ds.Tables(nTablaE)
            For eCont = 0 To .Rows.Count - 1
                aEjercicio(eCont + 1) = .Rows(eCont).Item("ejercicio") & " | " & _
                    ft.FormatoFecha(CDate(.Rows(eCont).Item("inicio").ToString)) & " | " & ft.FormatoFecha(CDate(.Rows(eCont).Item("cierre").ToString))
            Next
        End With

        ds.Tables(nTablaE).Dispose()
        ds.Dispose()

        ft.RellenaCombo(aEjercicio, cmbEjercicio)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsCambioEjercicio_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsCambioEjercicio_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        NuevoEjercicio()
        EjercicioActual()
        Me.Close()
    End Sub
    Private Sub NuevoEjercicio()
        jytsistema.WorkExercise = IIf(Mid(cmbEjercicio.Text, 1, 5) = "00000", "", Mid(cmbEjercicio.Text, 1, 5))
        Dim aeje() As String = Split(cmbEjercicio.Text, " | ")
        jytsistema.sFechadeTrabajo = IIf(jytsistema.WorkExercise = "", Now(), CDate(aeje(2).ToString))
    End Sub
End Class