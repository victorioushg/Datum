Imports MySql.Data.MySqlClient
Public Class jsControlProReconversionMonetaria
    Private Const sModulo As String = "Reconversión Monetaria"

    Private myConn As New MySqlConnection
    Private ft As New Transportables

    Private aReconversion() As String = {"jsconctaemp|ID_EMP.cadena.2.0|NOMBRE.cadena.100.0|DIRFISCAL.cadena.250.0|DIRORIGEN.cadena.250.0|CIUDAD.cadena.20.0|ESTADO.cadena.20.0|CODGEO.cadena.10.0|ZIP.cadena.10.0|TELEF1.cadena.15.0|TELEF2.cadena.15.0|FAX.cadena.15.0|EMAIL.cadena.50.0|ACTIVIDAD.cadena.100.0|RIF.cadena.15.0|NIT.cadena.15.0|CIIU.cadena.10.0|TIPOPERSONA.cadena.1.0|INICIO.fecha.0.0|CIERRE.fecha.0.0|TIPOSOC.cadena.1.0|LUCRO.cadena.1.0|NACIONAL.cadena.1.0|CI.cadena.10.0|PASAPORTE.cadena.10.0|CASADO.cadena.1.0|SEPARABIENES.cadena.1.0|RENTASEXENTAS.cadena.1.0|ESPOSADECLARA.cadena.1.0|REP_RIF.cadena.15.0|REP_NIT.cadena.15.0|REP_NACIONAL.cadena.1.0|REP_CI.cadena.15.0|REP_NOMBRE.cadena.150.0|REP_DIRECCION.cadena.250.0|REP_CIUDAD.cadena.15.0|REP_ESTADO.cadena.15.0|REP_TELEF.cadena.15.0|REP_FAX.cadena.15.0|REP_EMAIL.cadena.50.0|GER_NACIONAL.cadena.1.0|GER_CI.cadena.15.0|GER_NOMBRE.cadena.150.0|GER_DIRECCION.cadena.250.0|GER_TELEF.cadena.15.0|GER_CIUDAD.cadena.15.0|GER_ESTADO.cadena.15.0|GER_CEL.cadena.15.0|GER_EMAIL.cadena.50.0|LOGO.longblob.0.0|REPORTNAME.cadena.1.0|MONEDA.cadena.5.0|UNIDAD.cadena.3.0|fechatrabajo.fecha.0.0|RECONVERSION_20180604.entero.2.0|fechareconversion.fecha.0.0"}
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        '0 = Cierre, 1 = Reverso Cierre

        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Este proceso produce la RECONVERSION MONETARIA pautada para que se inicie el 04/06/2018 " & vbCr _
        & " Resolución N° 18-03-01 RM del Banco Central de Venezuela. Gaceta 41.387,  " & vbCr _
        & " " & vbCr _
        & " Este proceso no se puede revertir por lo tanto debe estar seguro que desea hacer la reconversión. " & vbCr _
        & " Además debe respaldar la Base de Datos.  " & vbCr _
        & vbCr _
        & " - Si no esta seguro POR FAVOR CONSULTE con el administrador " & vbCr _
        & " - SI NO ESTA SEGURO por favor consulte CON EL ADMINISTRADOR "

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.tipoMensaje.iAyuda)

        Me.Show()

    End Sub
    Private Sub ProcesarTablas(ByVal aTablas() As String, Titulo As String)

        Try

            Dim pb As New Progress_Bar
            pb.WindowTitle = "Tablas " & Titulo
            pb.TimeOut = 60
            pb.CallerThreadSet = Threading.Thread.CurrentThread


            Dim iCont As Integer
            For iCont = 0 To aTablas.Length - 1

                Dim ff() As String = Split(aTablas(iCont), "|")
                Dim aFld(0 To ff.Length - 1) As String
                Array.Copy(ff, 1, aFld, 0, ff.Length - 1)
                If ExisteTabla(myConn, jytsistema.WorkDataBase, ff(0)) Then

                    pb.OverallProgressText = ff(0) & " ..."
                    pb.OverallProgressValue = (iCont + 1) / aTablas.Length * 100

                    Dim kCont As Integer
                    Dim CampoAnterior As String = ""

                    For kCont = 0 To aFld.Length - 1
                        Dim gg() As String = Split(aFld(kCont), ".")

                        pb.PartialProgressText = "Procesando " & gg(0) & " ..."
                        pb.PartialProgressValue = (kCont + 1) / aFld.Length * 100

                        If gg(0) <> "" Then

                            If ExisteCampo(myConn, lblInfo, jytsistema.WorkDataBase, CStr(ff(0)), CStr(gg(0))) Then
                                'Modifica
                                ModificaCampoTabla(myConn, lblInfo, jytsistema.WorkDataBase, CStr(ff(0)), CStr(gg(0)), aFld(kCont))
                            Else
                                'Agrega
                                AgregarCampoTabla(myConn, lblInfo, jytsistema.WorkDataBase, CStr(ff(0)), aFld(kCont), CampoAnterior)
                            End If
                        End If
                        CampoAnterior = gg(0)
                    Next

                Else
                    CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, False, CStr(ff(0)), aFld)
                End If
            Next

            'pb.PartialProgressValue = 100
            pb.OverallProgressValue = 100
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProVerificarBD_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsControlProVerificarBD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ProcesarTablas(aReconversion, "Actualizando Fecha para reconversión")
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Not CBool(ft.Ejecutar_strSQL_DevuelveScalar(myConn, " select RECONVERSION_20180604 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")) Then
            Procesar()
        Else
            Dim strFecha As String = ft.muestraCampoFecha(ft.Ejecutar_strSQL_DevuelveScalar(myConn, " select fechareconversion from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' "))
            ft.mensajeCritico("La reconversión monetaria ya fué procesada con fecha " & _
                                strFecha)
        End If
    End Sub
    Private Sub Procesar()

        HabilitarCursorEnEspera()

        Dim i As Integer = 0
        For Each sItem As String In aTablasReconversion
            lblProgreso.Text = " Procesando ... " & sItem.Split("|")(0)
            ProgressBar1.Value = i / (aTablasReconversion.Count) * 100
            i += 1
            ReconversionTabla(myConn, sItem)
        Next

        ft.Ejecutar_strSQL(myConn, " UPDATE jsconctaemp SET RECONVERSION_20180604 = 1, " _
                           & " fechareconversion = '" & ft.FormatoFechaMySQL(Now) & "' " _
                           & " where id_emp = '" & jytsistema.WorkID & "' ")

        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        DeshabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")

    End Sub

    Private Sub ReconversionTabla(Myconn As MySqlConnection, TablaCampos As String)

        Dim str As String = ""
        For Each Campo As String In TablaCampos.Split("|")(1).Split(";")
            str = str & " " & Campo & " = round(" & Campo & "/" & ft.valorNumero(lblDivisor.Text) & ",2) , "
        Next

        ft.Ejecutar_strSQL(Myconn, " UPDATE " & TablaCampos.Split("|")(0) & " SET " & _
                           str.Substring(0, str.Length - 2) & _
                           " where ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub


    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        lblDivisor.Text = ft.FormatoEntero(10 ^ NumericUpDown1.Value)
    End Sub
End Class