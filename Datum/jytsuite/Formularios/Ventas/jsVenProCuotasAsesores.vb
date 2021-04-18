Imports MySql.Data.MySqlClient
Public Class jsVenProCuotasAsesores

    Private Const sModulo As String = "Asignación de cuotas anuales a Asesores Comerciales"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtAsesores As New DataTable
    Private dtMercas As New DataTable
    Private ft As New Transportables

    Private n_Apuntador As Long

    Private strSQLAsesores As String = ""
    Private strSQLMercas As String = ""
    Private nTablaAsesores As String = "tblAsesores"
    Private nTablaMercas As String = "tblMercancias"

   
    Public Property Apuntador() As Long
        Get
4:          Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon

        lblLeyenda.Text = "Este proceso busca repartir entre la fuerza de venta, las cuotas anuales de la empresa que fueron calculadas y asignadas en " + _
            "CUOTAS DE MERCANCIA (MERCANCIAS/ARCHIVOS/CUOTAS ANUALES). Dichas cuotas se repartiran dependiendo del método de asignación seleccionado " + _
            "1. Cuota Histórica (según el peso del año anterior de cada asesor). 2. Cuota Fija (Divide igualmente la cuota entre todos los asesores)  " + _
            "3. Factor de asignación (porcentaje asignado a cada asesor en el módulo de Grupo de Ventas) "

        IniciarTXT()
        Me.ShowDialog()


    End Sub
    Private Sub IniciarTXT()

        strSQLAsesores = "select 1 sel, codven, concat(nombres,' ',apellidos) nombre from jsvencatven where estatus = 1 and tipo = " & TipoVendedor.iFuerzaventa & " and ID_EMP = '" & jytsistema.WorkID & "' order by codven "
        ds = DataSetRequery(ds, strSQLAsesores, MyConn, nTablaAsesores, lblInfo)
        dtAsesores = ds.Tables(nTablaAsesores)

        CargarListaSeleccionAsesores(dgAsesores, dtAsesores, 7, False)

        strSQLMercas = " select 1 sel, codart, nomart, alterno from jsmerctainv where estatus = 0 and id_emp = '" & jytsistema.WorkID & "' order by codart "
        ds = DataSetRequery(ds, strSQLMercas, MyConn, nTablaMercas, lblInfo)
        dtMercas = ds.Tables(nTablaMercas)

        CargarListaSeleccionMercas(dgMercas, dtMercas, 8, False)

        opt0.Checked = True

    End Sub

    Private Sub jsVenProCuotasAsesores_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProPrepedidosPedidos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim iCont As Integer = 1
            For Each nRowAsesor As DataRow In dtAsesores.Rows
                Dim nAsesor As String = nRowAsesor.Item("codven")

                If nRowAsesor.Item("sel") Then
                    For Each nRowMerca As DataRow In dtMercas.Rows
                        Dim nMerca As String = nRowMerca.Item("codart")
                        If nRowMerca.Item("sel") Then
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, iCont / dtAsesores.Rows.Count * 100, _
                                                  "Asesor : " & nAsesor & " --- Mercancía : " & nRowMerca.Item("codart") & " | " & nRowMerca.Item("nomart"))
                            IncluirItemEnCuota(nAsesor, nMerca)
                        End If
                    Next
                End If
                iCont += 1
            Next

            ProgressBar1.Value = 0
            lblProgreso.Text = "PROCESO TERMINADO..."

        End If

    End Sub
   
    Private Sub IncluirItemEnCuota(ByVal Asesor As String, ByVal CodigoArticulo As String)

        Dim AñoAnterior As Integer

        AñoAnterior = Year(JytSistema.sFechadeTrabajo) - 1

        If ft.DevuelveScalarCadena(MyConn, " select codart from jsvencuoart where codven = '" & Asesor & "' and codart = '" & CodigoArticulo & "' and id_emp = '" & jytsistema.WorkID & "'  ") = "0" Then
            ft.Ejecutar_strSQL(myconn, "insert into jsvencuoart (CODVEN, CODART, ESMES01, ESMES02, ESMES03, ESMES04, ESMES05, ESMES06, " _
                & " ESMES07, ESMES08, ESMES09, ESMES10, ESMES11, ESMES12, EJERCICIO, ID_EMP) VALUES(" _
                & "'" & Asesor & "', " _
                & "'" & CodigoArticulo & "', " _
                & CuotaMes(CodigoArticulo, 1) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 2) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 3) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 4) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 5) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 6) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 7) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 8) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 9) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 10) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 11) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & CuotaMes(CodigoArticulo, 12) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & "'" & jytsistema.WorkExercise & "', " _
                & "'" & jytsistema.WorkID & "')")
        Else
            ft.Ejecutar_strSQL(myconn, " update jsvencuoart set  " _
                & " esmes01 = " & CuotaMes(CodigoArticulo, 1) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes02 = " & CuotaMes(CodigoArticulo, 2) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes03 = " & CuotaMes(CodigoArticulo, 3) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes04 = " & CuotaMes(CodigoArticulo, 4) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes05 = " & CuotaMes(CodigoArticulo, 5) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes06 = " & CuotaMes(CodigoArticulo, 6) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes07 = " & CuotaMes(CodigoArticulo, 7) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes08 = " & CuotaMes(CodigoArticulo, 8) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes09 = " & CuotaMes(CodigoArticulo, 9) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes10 = " & CuotaMes(CodigoArticulo, 10) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes11 = " & CuotaMes(CodigoArticulo, 11) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & ", " _
                & " esmes12 = " & CuotaMes(CodigoArticulo, 12) * FactorX(CodigoArticulo, CodigoArticulo, AñoAnterior) & " " _
                & " where " _
                & " codven = '" & Asesor & "' and " _
                & " codart = '" & CodigoArticulo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' ")
        End If

    End Sub

    Function CuotaMes(ByVal CodigoArticulo As String, ByVal Mes As Integer) As Double
        Return ft.DevuelveScalarDoble(MyConn, " select " & "MES" & Format(Mes, "00") & " " _
                & " from jsmerctacuo a " _
                & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp ) " _
                & " WHERE " _
                & " a.codart = '" & CodigoArticulo & "' and " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a. ID_EMP = '" & jytsistema.WorkID & "' order by a.codart ")

    End Function
    Function FactorX(ByVal CodigoVendedor As String, ByVal CodigoArticulo As String, _
        ByVal Año As Integer) As Double
        Dim peso_anterior As Double
        Dim peso_anterior_vendedor As Double

        If opt0.Checked Then
            peso_anterior = PesoTotalAnterior(CodigoArticulo, Año)
            peso_anterior_vendedor = PesoTotalAnteriorVendedor(CodigoVendedor, CodigoArticulo, Año)
            If peso_anterior > 0 Then
                If peso_anterior_vendedor > 0 Then
                    FactorX = (peso_anterior_vendedor / peso_anterior) / FactoresSeleccionados(CodigoArticulo, Año)
                Else
                    FactorX = 0.0#
                End If
            Else
                FactorX = 0.0#
            End If

        ElseIf opt1.Checked Then
            FactorX = IIf(CantidadVendedores() > 0, 1 / CantidadVendedores(), 1)
        ElseIf opt2.Checked Then
            FactorX = FactorVendedor(CodigoVendedor)
        End If

    End Function

    Function FactoresSeleccionados(ByVal CodigoArticulo As String, ByVal Año As Integer) As Double

        Dim Factores As Double = 0.0
        Dim p_anterior As Double
        Dim p_anterior_vendedor As Double
        Dim FX As Double

        For Each nRow As DataRow In dtAsesores.Rows
            If nRow.Item("sel") Then
                p_anterior = PesoTotalAnterior(CodigoArticulo, Año)
                p_anterior_vendedor = PesoTotalAnteriorVendedor(nRow.Item("codven"), CodigoArticulo, Año)
                If p_anterior > 0 Then
                    If p_anterior_vendedor > 0 Then
                        FX = p_anterior_vendedor / p_anterior
                    Else
                        FX = 0.0#
                    End If
                Else
                    FX = 0.0#
                End If
                Factores += FX
            End If
        Next

        Return Factores

    End Function


    Function FactorVendedor(ByVal CodigoVendedor As String) As Double
        Return ft.DevuelveScalarDoble(MyConn, " select factorcuota from jsvencatven where codven = '" & CodigoVendedor & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Function

    Function PesoTotalAnteriorVendedor(ByVal CodigoVendedor As String, ByVal CodigoArticulo As String, ByVal Año As Integer) As Double

        Return ft.DevuelveScalarDoble(MyConn, "select sum( IF( origen in ('FAC','PFC','PVE','NDV'), peso,  -1*peso) ) from jsmertramer where " _
            & " codart = '" & CodigoArticulo & "' and " _
            & " vendedor = '" & CodigoVendedor & "' and " _
            & " origen in ('FAC','PFC','PVE','NCV', 'NDV') and " _
            & " year(fechamov) = " & Año & " and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " group by codart ")

    End Function


    Function PesoTotalAnterior(ByVal CodigoArticulo As String, ByVal Año As Integer) As Double
        Return ft.DevuelveScalarDoble(MyConn, "select sum( IF( origen in ('FAC','PFC','PVE','NDV'), peso,  -1*peso) ) from jsmertramer where " _
            & " codart = '" & CodigoArticulo & "' and " _
            & " origen in ('FAC','PFC','PVE','NCV', 'NDV') and " _
            & " year(fechamov) = " & Año & " and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " group by codart ")

    End Function

    Function CantidadVendedores() As Integer
        Return ft.DevuelveScalarEntero(MyConn, "select COUNT(*) from jsvencatven " _
            & " Where " _
            & " tipo = '0' and " _
            & " estatus = 1 and " _
            & " ID_EMP = '" & jytsistema.WorkID & "'")
    End Function


    Private Sub dgAsesores_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgAsesores.CellContentClick
        If e.ColumnIndex = 0 Then
            dtAsesores.Rows(e.RowIndex).Item(0) = Not CBool(dtAsesores.Rows(e.RowIndex).Item(0).ToString)
        End If
    End Sub

    Private Sub dgMercas_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgMercas.CellContentClick
        If e.ColumnIndex = 0 Then
            dtMercas.Rows(e.RowIndex).Item(0) = Not CBool(dtMercas.Rows(e.RowIndex).Item(0).ToString)
        End If
    End Sub
End Class