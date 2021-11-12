Imports MySql.Data.MySqlClient
Module FuncionesBancos
    Private ft As New Transportables
    Public Function CalculaSaldoCajaPorFP(ByVal Mycon As MySqlConnection, ByVal Caja As String, ByVal Tipo As String, ByVal lblInfo As Label) As Double

        Dim tbl As String = "tblcaja" & Tipo
        Dim ds As New DataSet
        Dim str As String

        CalculaSaldoCajaPorFP = 0.0
        str = "select a.caja, a.formpag, sum( if( a.tipomov = 'EN', a.importe, -1*ABS(a.importe) )  ) saldo from jsbantracaj a " _
                    & " where " _
                    & " a.deposito = '' and " _
                    & " a.caja = '" & Caja & "' and " _
                    & IIf(Tipo <> "", " a.formpag = '" & Tipo & "' and ", "") _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " group by a.caja "

        ds = DataSetRequery(ds, str, Mycon, tbl, lblInfo)

        If ds.Tables(0).Rows.Count > 0 Then _
            CalculaSaldoCajaPorFP = ds.Tables(0).Rows(0).Item("saldo")

        ds.Dispose()

    End Function
    Public Function CalculaSaldoBanco(ByVal Mycon As MySqlConnection, ByVal lblInfo As Label, ByVal Banco As String, _
                                      Optional ByVal AUnaFecha As Boolean = False, Optional ByVal Fecha As Date = jytsistema.MyDate, _
                                      Optional ByVal Debitos_Creditos_Todos As Integer = 2) As Double

        Dim tbl As String = "tblbanco"
        Dim ds As New DataSet
        Dim str As String

        CalculaSaldoBanco = 0.0
        str = "select a.codban, sum( a.importe ) saldo from jsbantraban a " _
                    & " where " _
                    & " a.codban = '" & Banco & "' and " _
                    & IIf(AUnaFecha, " a.fechamov <= '" & ft.FormatoFechaMySQL(Fecha) & "' and ", "") _
                    & IIf(Debitos_Creditos_Todos <> 2, IIf(Debitos_Creditos_Todos = 0, " a.importe < 0 and ", " a.importe >= 0 and "), "") _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " group by a.codban "

        ds = DataSetRequery(ds, str, Mycon, tbl, lblInfo)

        If ds.Tables(0).Rows.Count > 0 Then _
            CalculaSaldoBanco = ds.Tables(0).Rows(0).Item("saldo")

        ds.Dispose()

    End Function
    Public Sub CargaListViewDesdeCAJAT(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("Emisión", 100, HorizontalAlignment.Center)
        LV.Columns.Add("TP", 35, HorizontalAlignment.Center)
        LV.Columns.Add("Número", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Concepto", 200, HorizontalAlignment.Left)
        LV.Columns.Add("Importe", 90, HorizontalAlignment.Right)
        LV.Columns.Add("ORG", 45, HorizontalAlignment.Center)
        LV.Columns.Add("Código", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Proveedor", 220, HorizontalAlignment.Left)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(ft.FormatoFecha(CDate(dt.Rows(iCont).Item("fecha").ToString)))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("tipomov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nummov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("concepto").ToString)
            LV.Items(iCont).SubItems.Add(ft.FormatoNumero(dt.Rows(iCont).Item("importe")))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("origen").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("prov_cli").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nombre").ToString)
        Next

        LV.EndUpdate()

    End Sub
    Public Sub CargarListaDesdeCaja(ByVal dg As DataGridView, ByVal dtdepos As DataTable, ByVal CestaTicket As Integer)

        If CestaTicket = 0 Then
            Dim aFld() As String = {"sel", "fecha", "tipomov", "nummov", "formpag", "numpag", "refpag", "importe", "origen", "Cantidad", "codven"}
            Dim aNom() As String = {"", "Emisión", "TP", "Número", "FP", "Número Pago", "Ref. Pago", "Importe", "ORG", "Cant.", "Vendedor"}
            Dim aAnc() As Integer = {20, 90, 35, 110, 30, 110, 90, 110, 45, 50, 70}
            Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                     AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                     AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
            Dim aFor() As String = {"", sFormatoFechaCorta, "", "", "", "", "", sFormatoNumero, "", sFormatoEntero, ""}


            IniciarTablaSeleccion(dg, dtdepos, aFld, aNom, aAnc, aAli, aFor, , True)
        Else

            Dim aFld() As String = {"sel", "FECHASOBRE", "NUMSOBRE", "CORREDOR", "TICKETS", "MONTO"}
            Dim aNom() As String = {"", "Emisión", "Nº Sobre/Remesa", "Corredor", "Cantidad Tickets", "Importe"}
            Dim aAnc() As Integer = {20, 100, 150, 100, 100, 110}
            Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                     AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {"", sFormatoFechaCorta, "", "", sFormatoEntero, sFormatoNumero}


            IniciarTablaSeleccion(dg, dtdepos, aFld, aNom, aAnc, aAli, aFor)



        End If

    End Sub

    Public Sub CargaListViewDesdeCAJ(ByVal LV As ListView, ByVal dt As DataTable, ByVal CestaTicket As Integer)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        If CestaTicket = 0 Then

            LV.Columns.Add("Emisión", 100, HorizontalAlignment.Center)
            LV.Columns.Add("TP", 35, HorizontalAlignment.Center)
            LV.Columns.Add("Número", 110, HorizontalAlignment.Left)
            LV.Columns.Add("FP", 30, HorizontalAlignment.Center)
            LV.Columns.Add("Número Pago", 120, HorizontalAlignment.Left)
            LV.Columns.Add("Ref. Pago", 90, HorizontalAlignment.Left)
            LV.Columns.Add("Importe", 110, HorizontalAlignment.Right)
            LV.Columns.Add("ORG", 45, HorizontalAlignment.Center)
            LV.Columns.Add("Tickets", 60, HorizontalAlignment.Center)

            For iCont = 0 To dt.Rows.Count - 1
                LV.Items.Add(ft.FormatoFecha(CDate(dt.Rows(iCont).Item("fecha").ToString)))
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("tipomov").ToString)
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nummov").ToString)
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("formpag").ToString)
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("numpag").ToString)
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("refpag").ToString)
                LV.Items(iCont).SubItems.Add(ft.FormatoNumero(dt.Rows(iCont).Item("importe")))
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("origen").ToString)
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("cantidad").ToString)
            Next
        Else

            LV.Columns.Add("Emisión", 100, HorizontalAlignment.Center)
            LV.Columns.Add("Nº Sobre/Remesa", 150, HorizontalAlignment.Center)
            LV.Columns.Add("Corredor", 100, HorizontalAlignment.Center)
            LV.Columns.Add("Cantidad Tickets", 100, HorizontalAlignment.Right)
            LV.Columns.Add("Importe", 110, HorizontalAlignment.Right)

            For iCont = 0 To dt.Rows.Count - 1
                LV.Items.Add(ft.FormatoFecha(CDate(dt.Rows(iCont).Item("fechasobre").ToString)))
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("numsobre").ToString)
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("corredor").ToString)
                LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("tickets").ToString)
                LV.Items(iCont).SubItems.Add(ft.FormatoNumero(dt.Rows(iCont).Item("monto")))
            Next
        End If

        LV.EndUpdate()

    End Sub
    Public Sub CargaListViewRemesaCT(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()
        LV.BeginUpdate()

        LV.Columns.Add("Emisión", 100, HorizontalAlignment.Center)
        LV.Columns.Add("TP", 35, HorizontalAlignment.Center)
        LV.Columns.Add("Número", 110, HorizontalAlignment.Left)
        LV.Columns.Add("FP", 30, HorizontalAlignment.Center)
        LV.Columns.Add("Número Tickets", 100, HorizontalAlignment.Center)
        LV.Columns.Add("Número Pago", 120, HorizontalAlignment.Left)
        LV.Columns.Add("Importe", 110, HorizontalAlignment.Right)
        LV.Columns.Add("ORG", 40, HorizontalAlignment.Center)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(ft.FormatoFecha(CDate(dt.Rows(iCont).Item("fecha").ToString)))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("tipomov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nummov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("formpag").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("cantidad").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("numpag").ToString)
            LV.Items(iCont).SubItems.Add(ft.FormatoNumero(dt.Rows(iCont).Item("importe")))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("origen").ToString)
            If dt.Rows(iCont).Item("numsobre") <> "" Then LV.Items(iCont).Checked = True
        Next

        LV.EndUpdate()

    End Sub

    Public Sub CargaListViewDesdeTraBan(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("Emisión", 100, HorizontalAlignment.Center)
        LV.Columns.Add("TP", 35, HorizontalAlignment.Center)
        LV.Columns.Add("Número", 110, HorizontalAlignment.Left)
        LV.Columns.Add("Concepto", 260, HorizontalAlignment.Left)
        LV.Columns.Add("Importe", 100, HorizontalAlignment.Right)
        LV.Columns.Add("ORG", 40, HorizontalAlignment.Center)
        LV.Columns.Add("CON", 40, HorizontalAlignment.Center)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(ft.FormatoFecha(CDate(dt.Rows(iCont).Item("fechamov").ToString)))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("tipomov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("numdoc").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("concepto").ToString)
            LV.Items(iCont).SubItems.Add(ft.FormatoNumero(dt.Rows(iCont).Item("importe")))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("origen").ToString)
            LV.Items(iCont).SubItems.Add(IIf(dt.Rows(iCont).Item("conciliado").ToString = "0", "No", "Si"))
            If dt.Rows(iCont).Item("conciliado").ToString = "1" Then LV.Items(iCont).Checked = True
        Next

        LV.EndUpdate()

    End Sub
    Public Sub IniciarCuentasEnCombo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal cmbCuenta As ComboBox, _
                                Optional ByVal estatusBanco As Integer = 1, Optional ByVal montoenCombo As Integer = 0)

        Dim ds As New DataSet
        Dim eCont As Integer
        Dim nTablaB As String = "tBancos"
        Dim str As String = ""

        If estatusBanco <= 1 Then str = " estatus = 1 and "
        ds = DataSetRequery(ds, " select * from jsbancatban where " + str + " id_emp = '" & jytsistema.WorkID & "' order by codban ", MyConn, nTablaB, lblInfo)

        With ds.Tables(nTablaB)
            Dim aBancos(.Rows.Count - 1) As String
            For eCont = 0 To .Rows.Count - 1
                aBancos(eCont) = .Rows(eCont).Item("codban") & " | " & _
                    .Rows(eCont).Item("nomban") & " | " & .Rows(eCont).Item("ctaban") _
                    & IIf(montoenCombo = 1, " | " & ft.FormatoNumero(CDbl(.Rows(eCont).Item("saldoact"))), "")
            Next
            ft.RellenaCombo(aBancos, cmbCuenta)
        End With

        ds.Tables(nTablaB).Dispose()
        ds.Dispose()
        ds = Nothing

    End Sub
    Public Sub IniciarMesesEnCombo(ByVal cmbMes As ComboBox)
        Dim aMeses() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
        ft.RellenaCombo(aMeses, cmbMes, IIf(Month(jytsistema.sFechadeTrabajo) > 2, Month(jytsistema.sFechadeTrabajo) - 2, 0))
    End Sub
    Public Sub IniciarMesesEnComboPlus(MyConn As MySqlConnection, ByVal cmbMes As ComboBox, Ejercicio As String)

        Dim InicioEjercicio As Date
        Dim CierreEjercicio As Date

        If Ejercicio <> "00000" Then
            InicioEjercicio = ft.DevuelveScalarFecha(MyConn, " select inicio from jsconctaeje where ejercicio = '" & Ejercicio & "' and id_emp = '" & jytsistema.WorkID & "'   ")
            CierreEjercicio = ft.DevuelveScalarFecha(MyConn, " select cierre from jsconctaeje where ejercicio = '" & Ejercicio & "' and id_emp = '" & jytsistema.WorkID & "'   ")
        Else
            InicioEjercicio = ft.DevuelveScalarFecha(myConn, " select inicio from jsconctaemp where id_emp = '" & jytsistema.WorkID & "'   ")
            CierreEjercicio = ft.DevuelveScalarFecha(myConn, " select cierre from jsconctaemp where id_emp = '" & jytsistema.WorkID & "'   ")
        End If

        Dim EjercicioIni As Date
        Dim aMeses() As String = {}
        EjercicioIni = UltimoDiaMes(InicioEjercicio)
        Do While EjercicioIni <= CierreEjercicio
            ReDim Preserve aMeses(UBound(aMeses) + 1)
            aMeses(aMeses.Length - 1) = aMesesReal(EjercicioIni.Month - 1) + "-" + EjercicioIni.Year.ToString
            EjercicioIni = DateAdd("m", 1, EjercicioIni)
        Loop
        Dim hoy As String = aMesesReal(jytsistema.sFechadeTrabajo.Month - 1) + "-" + jytsistema.sFechadeTrabajo.Year.ToString
        Dim defaultItem As Integer = ft.InArray(aMeses, hoy)
        ft.RellenaCombo(aMeses, cmbMes, IIf(defaultItem < 0, aMeses.Length - 1, defaultItem))

    End Sub
    Public Sub EliminarImpuestoDebitoBancario(MyConn As MySqlConnection, lblinfo As Label, CodigoBanco As String, _
                                              NumeroDocumento As String, FechaDocumento As Date)

        Dim DescripcionIDB As String = Convert.ToString(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM04"))

        ft.Ejecutar_strSQL(MyConn, "DELETE from jsbantraban where " _
                            & " CODBAN = '" & CodigoBanco & "' and " _
                            & " TIPOMOV = 'ND' AND " _
                            & " FECHAMOV = '" & ft.FormatoFechaMySQL(FechaDocumento) & "' AND " _
                            & " SUBSTRING(CONCEPTO,1," & DescripcionIDB.Length & ") = '" & DescripcionIDB & "' AND " _
                            & " NUMDOC = '" & NumeroDocumento & "' and " _
                            & " ORIGEN = 'BAN' AND " _
                            & " substring(NUMORG,1,2) = 'DB' and " _
                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

    End Sub
    Public Sub IncluirImpuestoDebitoBancario(MyConn As MySqlConnection, lblInfo As Label, TipoIDB As String, _
                                              CodigoBanco As String, Numerodocumento As String, FechaDocumento As Date, _
                                              Importe As Double)

        'TipoIDB = "CH", "ND"
        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM10")) Then

            Dim MontoMinimoIDB As Double = Convert.ToDouble(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM03"))
            Dim TipoDocumentosIDB As String = Convert.ToString(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM01"))
            Dim PorcentajeIDB As Double = Convert.ToDouble(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM02"))
            Dim DescripcionIDB As String = Convert.ToString(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM04"))

            'ft.mensajeInformativo(InStr(TipoDocumentosIDB, TipoIDB))
            If InStr(TipoDocumentosIDB, TipoIDB) > 0 Then

                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult

                sRespuesta = MsgBox("Este documento GENERA una NOTA DEBITO en la cuenta por concepto de : " + vbLf _
                                    + DescripcionIDB + vbLf _
                                    + "por un monto de : " + ft.muestraCampoNumero(Importe * PorcentajeIDB / 100) + vbLf _
                                    + "¿ Desea proceder a realizar esta acción ?", MsgBoxStyle.YesNo, DescripcionIDB + " ... ")

                If sRespuesta = MsgBoxResult.Yes Then
                    Dim Insertar As Boolean = False
                    If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsbantraban where " _
                                               + " codban = '" + CodigoBanco + "' and " _
                                               + " numdoc = '" + Numerodocumento + "' and " _
                                               + " tipomov = 'ND' and " _
                                               + " origen = 'BAN' and " _
                                               + " numorg = '" + "DB" + ft.RellenaConCaracter(jytsistema.sFechadeTrabajo.Month.ToString, 2, "0", Transportables.lado.izquierdo) + jytsistema.sFechadeTrabajo.Year.ToString + "' and " _
                                               + " id_emp = '" + jytsistema.WorkID + "' ") = 0 Then Insertar = True

                    InsertEditBANCOSMovimientoBanco(MyConn, lblInfo, Insertar, FechaDocumento, Numerodocumento, "ND", CodigoBanco, "",
                                                     DescripcionIDB + " MES : " + ft.RellenaConCaracter(jytsistema.sFechadeTrabajo.Month.ToString, 2, "0", Transportables.lado.izquierdo) + "/" + jytsistema.sFechadeTrabajo.Year.ToString,
                                                    -1 * Math.Abs(Importe) * PorcentajeIDB / 100,
                                                    "BAN", "DB" + ft.RellenaConCaracter(jytsistema.sFechadeTrabajo.Month.ToString, 2, "0", Transportables.lado.izquierdo) + jytsistema.sFechadeTrabajo.Year.ToString,
                                                    "", "", "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "ND", "", jytsistema.sFechadeTrabajo,
                                                    "0", "", "", jytsistema.WorkCurrency.Id, DateTime.Now())

                End If

            End If

        End If

    End Sub




End Module
