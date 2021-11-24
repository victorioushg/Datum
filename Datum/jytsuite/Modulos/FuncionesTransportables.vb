Imports MySql.Data.Types
Imports MySql.Data.MySqlClient
Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Deployment.Application
Imports FP_AclasBixolon
Imports fTransport
Imports System.Net.NetworkInformation
Imports System.Management
Imports Syncfusion.WinForms.Input
Imports Syncfusion.WinForms.ListView
Imports C1.Win.C1SuperTooltip

Module FuncionesTransportables

    Public Const cFormatoEntero As String = "#,##0"
    Public Const cFormatoNumero As String = "#,##0.00"
    Public Const cFormatoCantidad As String = "#,##0.000"
    Public Const cFormatoFecha As String = "dd/MM/yyyy"

    Public Const NombreSistema As String = "DATUM"

    Public Enum Lado
        izquierdo = 0
        derecho = 1
    End Enum


    Private IB As New AclasBixolon
    Private ft As New Transportables

    'BASE DE DATOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#Region "Formatos"

    Function FormatoTablaSimple(ByVal iModulo As Modulo) As String
        Return ft.RellenaConCaracter(CInt(iModulo), 5, "0", Lado.izquierdo)
    End Function


#End Region
#Region "Base De Datos"
    Public Function DataSetRequeryPlus(rDataset As DataSet, nTabla As String, MyConn As MySqlConnection, strSQL As String) As DataSet
        rDataset.EnforceConstraints = False
        Using nDataAdapter As MySqlDataAdapter = New MySqlDataAdapter()

            If Not rDataset.Tables(nTabla) Is Nothing Then rDataset.Tables(nTabla).Clear()

            Try
                nDataAdapter.SelectCommand = New MySqlCommand(strSQL, MyConn)
                nDataAdapter.Fill(rDataset, nTabla)
            Catch ex As MySqlException
                ft.mensajeCritico(ex.Message + ". Error base de datos")
            End Try

        End Using

        Return rDataset

    End Function
    Public Function AbrirDataTable(rDataset As DataSet, nTabla As String, MyConn As MySqlConnection, strSQL As String) As DataTable
        Return DataSetRequeryPlus(rDataset, nTabla, MyConn, strSQL).Tables(nTabla)
    End Function

    Public Function MostrarFilaEnTabla(myConn As MySqlConnection, ds As DataSet, nTabla As String, strSQL As String,
                                       Contexto As BindingContext, MenuBarra As ToolStrip, dg As DataGridView, Region As String,
                                           Usuario As String, nRow As Long, ActualizaDataset As Boolean) As DataSet

        If (ActualizaDataset) Then ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
        If (nRow >= 0 And ds.Tables(nTabla).Rows.Count > 0) Then
            Contexto(ds, nTabla).Position = Convert.ToInt32(nRow)
            MostrarItemsEnMenuBarra(MenuBarra, Convert.ToInt32(nRow), ds.Tables(nTabla).Rows.Count)
            dg.CurrentCell = dg(0, Convert.ToInt32(nRow))
        End If
        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTabla), Region, MenuBarra, Usuario)
        Return ds

    End Function

    Public Sub CrearFunctionEnBaseDatos(MyConn As MySqlConnection, formula As functionStructure)
        Elimina_Funcion_En_BD(MyConn, formula.FunctionName)
        ft.Ejecutar_strSQL(MyConn, " CREATE FUNCTION `" & formula.FunctionName & "`(" & formula.FunctionParameters & ")  RETURNS " & formula.FunctionReturnType & " " _
            & " BEGIN " _
            & formula.FunctionFormula _
            & " END ")
        ft.Ejecutar_strSQL(MyConn, " INSERT into jsnomformula set Formula = '" & formula.FunctionName & "', parametros = '" & ParametrosDeFuncion(formula.FunctionParameters) & "' , " _
                                 & " descripcion = '" & formula.FunctionCommentary & "' ")

    End Sub
    Private Function ParametrosDeFuncion(str As String) As String

        Dim strDevuelta As String = ""
        For Each s As String In str.Split(",")
            strDevuelta += "@" + s.TrimStart().TrimEnd.Split(" ")(0) + ", "
        Next
        Return strDevuelta.TrimEnd(" ").TrimEnd(",")

    End Function
    Private Sub Elimina_Funcion_En_BD(MyConn As MySqlConnection, nombreFuncion As String)
        ft.Ejecutar_strSQL(MyConn, " DROP FUNCTION IF EXISTS `" & nombreFuncion & "`")
        ft.Ejecutar_strSQL(MyConn, " DELETE FROM jsnomformula WHERE FORMULA = '" & nombreFuncion & "' ")
    End Sub

    Public Function GetWorkCurrency(MyConn As MySqlConnection) As Moneda
        Dim ds As New DataSet
        Dim nTabla As String = "tblmoneda"
        ds = DataSetRequeryPlus(ds, nTabla, MyConn, " select m.* from jsconcatmon m inner join jsconctaemp e on  (e.moneda = m.id) where e.id_emp = '" & jytsistema.WorkID & "' ")
        Return ConvertDataTable(Of Moneda)(ds.Tables(nTabla)).FirstOrDefault()
    End Function





#End Region
#Region "Menus"
    Public Sub MostrarItemsEnMenuBarra(MenuBarra As ToolStrip, ItemPosicion As Integer, ItemCantidad As Integer)
        For Each item As ToolStripItem In MenuBarra.Items
            If (item.Name.ToUpper = "ITEMS" Or item.Name.ToUpper = "ITEMSRENGLON") Then
                item.Text = ft.FormatoEntero(ItemPosicion + 1)
            End If
            If (item.Name.ToUpper = "LBLITEMS" Or item.Name.ToUpper = "LBLITEMSRENGLON") Then
                item.Text = String.Format("de {0}", ft.FormatoEntero(ItemCantidad))
            End If
        Next
    End Sub
    Public Sub AsignarToolTipsMenuBarraToolStrip(menus As List(Of ToolStrip), Optional menuName As String = "")

        Dim tt As New C1SuperTooltip()
        For Each menu As ToolStrip In menus
            For Each btn As ToolStripItem In menu.Items
                Select Case btn.Name
                    Case "btnAgregar"
                        tt.SetToolTip(btn, "<B>Agregar</B> nuevo(a) " & menuName)
                    Case "btnEditar"
                        tt.SetToolTip(btn, "<B>Editar o mofificar</B> " & menuName)
                    Case "btnEliminar"
                        tt.SetToolTip(btn, "<B>Eliminar</B> " & menuName)
                    Case "btnBuscar"
                        tt.SetToolTip(btn, "<B>Buscar</B> " & menuName)
                    Case "btnPrimero"
                        tt.SetToolTip(btn, "ir a <B>primer(a)</B> " & menuName)
                    Case "btnSiguiente"
                        tt.SetToolTip(btn, "ir a <B>siguiente</B> " & menuName)
                    Case "btnAnterior"
                        tt.SetToolTip(btn, "ir a <B>anterior</B> " & menuName)
                    Case "btnUltimo"
                        tt.SetToolTip(btn, "ir a <B>último(a) </B> " & menuName)
                    Case "btnImprimir"
                        tt.SetToolTip(btn, "<B>Imprimir</B> " & menuName)
                    Case "btnSalir"
                        tt.SetToolTip(btn, "<B>Cerrar</B> esta ventana")
                    Case "btnDuplicar"
                        tt.SetToolTip(btn, "<B>Duplicar</B> este(a) " & menuName)
                    Case "btnRecalcular"
                        tt.SetToolTip(btn, "<B>Recalcular</B> este(a) " & menuName)
                    Case "btnRemesas"
                        tt.SetToolTip(btn, "Construir <B>Remesas de Cheques de Alimentacion</B> de esta " & menuName)
                    Case "btnAdelantoEfectivo"
                        tt.SetToolTip(btn, "<B>Adelantos de EFectivo</B> desde esta " & menuName)

                    Case "btnAgregarMovimiento"
                        'Menu barra renglón
                        tt.SetToolTip(btn, "<B>Agregar</B> renglón en " & menuName)
                    Case "btnAgregarServicio"
                        'Menu barra renglón
                        tt.SetToolTip(btn, "<B>Agregar</B> renglón de Servicios en " & menuName)
                    Case "btnEditarMovimiento"
                        tt.SetToolTip(btn, "<B>Editar</B> renglón en " & menuName)
                    Case "btnEliminarMovimiento"
                        tt.SetToolTip(btn, "<B>Eliminar</B> renglón en " & menuName)
                    Case "btnBuscarMovimiento"
                        tt.SetToolTip(btn, "<B>Buscar</B> un renglón en " & menuName)
                    Case "btnPrimerMovimiento"
                        tt.SetToolTip(btn, "ir a <B>primer</B> renglón en " & menuName)
                    Case "btnAnteriorMovimiento"
                        tt.SetToolTip(btn, "ir a <B>anterior</B> renglón en " & menuName)
                    Case "btnSiguienteMovimiento"
                        tt.SetToolTip(btn, "ir a renglón <B>siguiente </B> en " & menuName)
                    Case "btnUltimoMovimiento"
                        tt.SetToolTip(btn, "ir a <B>último</B> renglón de " & menuName)
                    Case "btnPresupuesto"
                        tt.SetToolTip(btn, "Traer <B>presupuesto</B> a " & menuName)
                    Case "btnPrepedido"
                        tt.SetToolTip(btn, "Traer <B>pre-pedido</B> a " & menuName)
                    Case "btnPedido"
                        tt.SetToolTip(btn, "Traer <B>pedido</B> a " & menuName)
                    Case "btnTraerFacturas"
                        tt.SetToolTip(btn, "Traer <B>Facturas</B> a " & menuName)
                    Case "btnCortar"
                        tt.SetToolTip(btn, "<B>Recortar precios</B> en " & menuName)

                    Case "btnDepositarEfectivo"
                        tt.SetToolTip(btn, "<B>Depositar efectivo</B> desde caja seleccionada en " & menuName)
                    Case "btnDepositarTarjetas"
                        tt.SetToolTip(btn, "<B>Depositar Tarjetas</B> desde caja seleccionada en " & menuName)
                    Case "btnDepositarCestaTicket"
                        tt.SetToolTip(btn, "<B>Depositar Cesta Ticket</B> desde la caja seleccionada en " & menuName)
                    Case "btnReposicion"
                        tt.SetToolTip(btn, "<B>Reposicion</B> de caja chica desde " & menuName)


                    Case "btnAgregaDescuento"
                        'Menu Barra Descuento 
                        tt.SetToolTip(btn, "<B>Agrega </B> descuento global a " & menuName)
                    Case "btnEliminaDescuento"
                        tt.SetToolTip(btn, "<B>Elimina</B> descuento global de " & menuName)

                        'btnDepositarEfectivo, btnDepositarTarjetas, btnDepositarCestaTicket, btnReposicion)

                End Select
            Next
        Next

    End Sub

#End Region
#Region "Controles"
    Public Sub SetComboCurrency(currencyId As Integer, cmbMonedas As SfComboBox, Optional lblTotal As Label = Nothing)
        currencyId = IIf(currencyId = 0, jytsistema.WorkCurrency.Id, currencyId)
        Dim monedas As List(Of CambioMonedaPlus) = cmbMonedas.DataSource
        Dim moneda = monedas.FirstOrDefault(Function(change) change.Moneda = currencyId)
        cmbMonedas.SelectedValue = currencyId

        If lblTotal IsNot Nothing Then lblTotal.Text = "TOTAL (" + moneda.CodigoIso + ") "

    End Sub
#End Region
#Region "Syncfusion"
    Public Function SetSizeDateObjects(ByVal dates() As SfDateTimeEdit)

        For Each fecha As SfDateTimeEdit In dates
            fecha.DropDownSize = New Size(220, 219)
            fecha.Style.DisabledBackColor = Color.Azure
        Next

    End Function
#End Region
#Region "Utilidades"
    Function getMacAddress()
        Dim nics() As NetworkInterface =
              NetworkInterface.GetAllNetworkInterfaces
        Return nics(0).GetPhysicalAddress.ToString
    End Function

    Function driveser(ByVal model As String) As String
        Dim devid As String = ""
        driveser = ""
        Try
            Dim searcher As New ManagementObjectSearcher(
                "root\CIMV2",
                "SELECT * FROM Win32_DiskDrive WHERE Model LIKE '%" + model + "%'")
            For Each queryObj As ManagementObject In searcher.Get()
                If queryObj("SerialNumber") <> "" Then driveser = queryObj("SerialNumber")
                'Debug.Print(queryObj("Model") + ":" + driveser)
            Next
        Catch err As ManagementException
            'Debug.Print("An error occurred while querying for WMI data: " & err.Message)
            ft.mensajeCritico("Un error ha ocurrido recopilando datos del WMI" & err.Message)
        End Try
    End Function


#End Region
#Region "Arrays"
    Public Function DeleteArrayItem(ByVal arr() As Object, ByVal Index As Integer) As Object
        'Elimina el elemento <Index> del arreglo <arr>
        For iCont As Integer = Index To arr.GetUpperBound(0) - 1
            arr(iCont) = arr(iCont + 1)
        Next
        ReDim Preserve arr(arr.GetUpperBound(0) - 2)
        Return arr
    End Function
    'Public Function ft.InArray(ByVal aArray() As Object, ByVal Elemento As Object) As Integer
    '    Return Array.IndexOf(aArray, Elemento)
    'End Function
    Public Function InsertArrayItem(ByVal arr() As Object, ByVal Index As Integer, ByVal newValue As Object) As Object
        ' Inserta un elemento <newValue> en el arreglo <arr>, en la posición <Index>
        ReDim Preserve arr(arr.Length + 1)
        If arr.Length > 1 Then
            Dim iCont As Integer
            For iCont = arr.Length - 2 To Index Step -1
                arr(iCont + 1) = arr(iCont)
            Next
        End If
        arr(Index) = newValue
        Return arr
    End Function
    Public Function InsertArrayItemString(ByVal arr() As String, ByVal Index As Integer, ByVal newValue As String) As String()
        ' Inserta un elemento <newValue> en el arreglo <arr>, en la posición <Index>
        ReDim Preserve arr(arr.Length + 1)
        If arr.Length > 1 Then
            Dim iCont As Integer
            For iCont = arr.Length - 2 To Index Step -1
                arr(iCont + 1) = arr(iCont)
            Next
        End If
        arr(Index) = newValue
        Return arr
    End Function
    Public Function InsertArrayItemStringPlus(ByVal arr() As String, ByVal Index As Integer, ByVal newValue As String) As String()
        ' Inserta un elemento <newValue> en el arreglo <arr>, en la posición <Index>
        ReDim Preserve arr(arr.Length)
        If arr.Length > 1 Then
            Dim iCont As Integer
            For iCont = arr.Length - 2 To Index Step -1
                arr(iCont + 1) = arr(iCont)
            Next
        End If
        arr(If(Index < 0, 0, Index)) = newValue
        Return arr
    End Function
    Public Function InsertArrayItemInteger(ByVal arr() As Integer, ByVal Index As Integer, ByVal newValue As Integer) As Integer()
        ' Inserta un elemento <newValue> en el arreglo <arr>, en la posición <Index>
        ReDim Preserve arr(arr.Length + 1)
        If arr.Length > 1 Then
            Dim iCont As Integer
            For iCont = arr.Length - 2 To Index Step -1
                arr(iCont + 1) = arr(iCont)
            Next
        End If
        arr(Index) = newValue
        Return arr
    End Function
    Public Function InsertArrayItemLong(ByVal arr() As Long, ByVal Index As Integer, ByVal newValue As Long) As Long()
        ' Inserta un elemento <newValue> en el arreglo <arr>, en la posición <Index>
        ReDim Preserve arr(arr.Length + 1)
        If arr.Length > 1 Then
            Dim iCont As Integer
            For iCont = arr.Length - 2 To Index Step -1
                arr(iCont + 1) = arr(iCont)
            Next
        End If
        arr(Index) = newValue
        Return arr
    End Function
    Public Function DeleteArrayValue(ByVal arr() As String, ByVal dValue As String) As String()
        ' Elimina <dValue> en el arreglo <arr> cuantas veces esté

        Dim iCont As Integer
        Dim aNewArray() As String = {}
        For iCont = 0 To arr.Length - 1
            If arr(iCont) = dValue Then
            Else
                ReDim Preserve aNewArray(UBound(aNewArray) + 1)
                aNewArray(UBound(aNewArray)) = arr(iCont)
            End If
        Next
        Return aNewArray

    End Function
    Public Function DeleteArrayValuePlus(ByVal arr() As String, ByVal dValue As String) As String()
        Dim AList As ArrayList = New ArrayList(arr)
        AList.Remove(dValue)
        Return CType(AList.ToArray(GetType(String)), String())
    End Function
    Public Function MaxValueInArray(ByVal arr() As String) As String
        Dim iCont As Integer
        MaxValueInArray = arr(0)
        For iCont = 1 To arr.GetUpperBound(0) - 1
            If arr(iCont) > MaxValueInArray Then MaxValueInArray = arr(iCont)
        Next
        Return MaxValueInArray

    End Function
    Public Function MinValueInArray(ByVal arr() As String) As String
        MinValueInArray = arr(0)
        For iCont As Integer = 1 To arr.GetUpperBound(0) - 1
            If arr(iCont) < MinValueInArray Then MinValueInArray = arr(iCont)
        Next
        Return MinValueInArray
    End Function
#End Region
#Region "Datum - DB"

    Function CadenaConexion(Optional lServidor As String = "", Optional lBaseDatos As String = "") As String

        Dim Servidor As String = IIf(lServidor = "", Microsoft.Win32.Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveServidor, ""), lServidor)
        Dim BaseDeDatos As String = IIf(lBaseDatos = "", Microsoft.Win32.Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveBaseDatos, ""), lBaseDatos)

        Dim aServidor() As String = Servidor.Split(":")

        Dim strServer As String = ""
        If aServidor.Length = 2 Then
            strServer = "Port = " + aServidor(1) + ";"
        Else
            strServer = "Port = 3307; "
        End If

        Return "server=" + aServidor(0) +
                "; database=" + BaseDeDatos + ";" _
                + strServer _
                + jytsistema.strCon

    End Function


    Function DocumentoBloqueado(MyConn As MySqlConnection, nTablaDB As String, aCampos_y_Valores() As String) As Boolean

        Dim str As String = ""

        If Not aCampos_y_Valores Is Nothing Then
            For Each aItem As String In aCampos_y_Valores
                str += aItem.Split("|")(0) & " = " & aItem.Split("|")(1) & " AND "
            Next
        End If

        If ft.DevuelveScalarEntero(MyConn, " select count(*) from " & nTablaDB & " WHERE " _
                                   & str _
                                   & " BLOCK_DATE = '2009-01-01' AND " _
                                   & " ID_EMP = '" & jytsistema.WorkID & "' ") > 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Function FechaUltimoBloqueo(Myconn As MySqlConnection, nTablaBD As String, Optional aAdicionales() As String = Nothing) As Date

        Dim str As String = ""

        If Not aAdicionales Is Nothing Then
            For Each aItem As String In aAdicionales
                str += aItem
            Next
        End If
        Return ft.DevuelveScalarFecha(Myconn, " SELECT DISTINCT(block_date) BLOCK_DATE FROM " & nTablaBD & " WHERE " _
                                      & str _
                                      & " ID_EMP =  '" & jytsistema.WorkID & "' " _
                                      & " ORDER BY 1 DESC " _
                                      & " LIMIT 1 ")
    End Function

#End Region

    Function DataSetRequery(ByVal rDataset As DataSet, ByVal strSQL As String,
         ByVal myConn As MySqlConnection, ByVal nTabla As String, Optional ByVal lblInfo As Label = Nothing) As DataSet

        Dim nDataAdapter As New MySqlDataAdapter

        rDataset.EnforceConstraints = False

        If Not IsNothing(rDataset.Tables(nTabla)) Then rDataset.Tables(nTabla).Clear()

        Try
            nDataAdapter.SelectCommand = New MySqlCommand(strSQL, myConn)
            nDataAdapter.Fill(rDataset, nTabla)

        Catch ex As MySqlException
            ft.mensajeCritico(ex.Message + ". Error base de datos")
        End Try
        DataSetRequery = rDataset
        nDataAdapter = Nothing

    End Function
    Function DataSetRequery(ByVal myConn As MySqlConnection, strStoreProcedure As String, list As List(Of KeyValuePair(Of String, Object)),
                            resDataSet As DataSet, ByVal nTabla As String) As DataSet
        Dim nDataAdapter As New MySqlDataAdapter
        resDataSet.EnforceConstraints = False
        If Not IsNothing(resDataSet.Tables(nTabla)) Then resDataSet.Tables(nTabla).Clear()
        Try
            nDataAdapter.SelectCommand = New MySqlCommand(strStoreProcedure, myConn)
            nDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
            For Each pair As KeyValuePair(Of String, Object) In list
                nDataAdapter.SelectCommand.Parameters.AddWithValue("@" + pair.Key.ToString(), pair.Value)
            Next
            nDataAdapter.Fill(resDataSet, nTabla)
            nDataAdapter = Nothing
            Return resDataSet
        Catch ex As Exception
            ft.mensajeCritico(ex.Message + ". Error base de datos")
        End Try
    End Function
    Function NivelUsuario(MyConn As MySqlConnection, lblInfo As Label, CodigoUsuario As String) As Integer
        Return ft.DevuelveScalarEntero(MyConn, "select nivel from jsconctausu where id_user = '" & CodigoUsuario & "' ")
    End Function
    Function UsuarioClaveAESPlus(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ClaveEncriptada As String) As String
        Return ft.DevuelveScalarCadena(MyConn, " SELECT id_user FROM jsconctausu WHERE AES_DECRYPT(PASSWORD, usuario)  = '" & ClaveEncriptada & "'  ")
    End Function
    Function UsuarioClaveAES(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Usuario As String, ByVal ClaveEncriptada As String) As Boolean

        jytsistema.sUsuario = ft.Ejecutar_strSQL_DevuelveScalar(MyConn, "select  if(id_user is null,'', id_user)  from jsconctausu " _
            & " Where aes_decrypt(password, usuario)  = '" & ClaveEncriptada & "' " _
            & " and usuario = '" & Usuario & "' " _
            & " and estatus = 1 ")
        jytsistema.sNombreUsuario = ft.Ejecutar_strSQL_DevuelveScalar(MyConn, "select IF(nombre is null, '', nombre) from jsconctausu " _
            & " Where aes_decrypt(password, usuario)  = '" & ClaveEncriptada & "' " _
            & " and usuario = '" & Usuario & "' " _
            & " and estatus = 1 ")

        If jytsistema.sUsuario <> "" Then UsuarioClaveAES = True

    End Function
    Function UsuarioClave(ByVal MyConn As MySqlConnection, ByVal Usuario As String, ByVal ClaveEncriptada As String) As Boolean
        Dim myReader As MySqlDataReader
        Dim myCommand = New MySqlCommand(
            " select id_user, usuario, aes_decrypt(password,usuario) password, nombre from jsconctausu where usuario = '" &
            LCase(Usuario) & "' and estatus = 1 ", MyConn)

        myReader = myCommand.ExecuteReader()

        Dim contraseña As String
        If myReader.HasRows Then
            While myReader.Read
                contraseña = myReader.GetString("password")
                If contraseña = ClaveEncriptada Then
                    jytsistema.sUsuario = CStr(myReader("id_user"))
                    jytsistema.sNombreUsuario = CStr(myReader("nombre"))
                    UsuarioClave = True
                End If
            End While
        End If

        myReader.Close()
        myReader = Nothing

    End Function
    Public Sub EsperateUnPoquito(ByVal MyConn As MySqlConnection, Optional Intervalo As Long = 0)
        If Intervalo = 0 Then
            While MyConn.State = ConnectionState.Executing
            End While
        Else
            For iCont As Long = 0 To Intervalo
                iCont += 1
            Next
        End If
    End Sub
    Function Licencia(ByVal MyConn As MySqlConnection, ByVal Rif As String) As String
        Dim myReader As MySqlDataReader
        Dim myCommand = New MySqlCommand(
            " select aes_decrypt(num_licencia,'capidoncella31051110') num_licencia from jsconlicencia where num_control = '" &
            Rif & "' ", MyConn)

        myReader = myCommand.ExecuteReader()
        Licencia = ""
        If myReader.HasRows Then
            While myReader.Read
                Licencia = myReader.GetString("num_licencia")
            End While
        End If

        myReader.Close()
        myReader = Nothing

    End Function

    Public Sub MuestraItemMenu(ByVal MyConn As MySqlConnection, ByVal Usuario As String,
                                        ByVal MenuItem As ToolStripMenuItem, ByVal SubMenuItem As ToolStripMenuItem)
        'Evalue si Usuario posee derecho del Subitem en el item 
        MenuItem.DropDownItems.Add(SubMenuItem)
    End Sub
    ' Arrays ////////////////////////////////////////////////



    Public Sub FechaString(ByVal sender As Object, ByVal e As ConvertEventArgs)
        '  e.Value = CType(e.Value, DateTime).ToString("d")
    End Sub

    Public Function ValidaNumeroEnteroEnTextbox(ByVal e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        If Not Char.IsNumber(e.KeyChar) And e.KeyChar <> vbBack Then ValidaNumeroEnteroEnTextbox = True
    End Function
    Public Function ValidaAlfaNumericoEnTextbox(ByVal e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        If Not Char.IsLetterOrDigit(e.KeyChar) And
            e.KeyChar <> "." And
                e.KeyChar <> "-" And
                    e.KeyChar <> vbBack Then ValidaAlfaNumericoEnTextbox = True
    End Function
    Public Function ValidaHoraEnMask(ByVal Hora As String) As Boolean
        Dim nHora As String = Split(Hora, ":")(0)
        Dim nMinutos As String = Split(Hora, ":")(1)

        If nHora.Trim() = "" Then Return False
        If nMinutos.Trim = "" Then Return False

        If (ValorEnteroLargo(nHora) >= 0 AndAlso ValorEnteroLargo(nHora) <= 23) Then
            If ValorEnteroLargo(nMinutos) >= 0 AndAlso ValorEnteroLargo(nMinutos) <= 59 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
    Public Sub StringFecha(ByVal sender As Object, ByVal e As ConvertEventArgs)
        Try
            e.Value = CDate(e.Value)
        Catch exp As MySqlException
            MsgBox(exp.Message, MsgBoxStyle.Critical, "Error formato ")
        End Try
    End Sub
    Function ItemMenuActivo(ByVal dt As DataTable, ByVal ItemMenu As String) As Boolean
        'itemmenu = "INCLUYE":"MODIFICA":"ELIMINA"
        If dt.Rows(0).Item(ItemMenu) = "1" Then ItemMenuActivo = True
    End Function
    Function ModificarCadena(ByVal Str As String, ByVal Campo As String) As String
        If Str <> ":-)" Then
            ModificarCadena = IIf(Str.Length > 0,
                                  " " & Campo & " = '" & Str.Replace("'", "''").Replace("\", " ") & "', ",
                                  " " & Campo & " = '', ")
        Else
            ModificarCadena = ""
        End If
    End Function
    Function ModificarEnteroLargo(ByVal Entero As Long, ByVal Campo As String) As String
        If Entero <> 9999 Then
            ModificarEnteroLargo = " " & Campo & " = " & Entero & ", "
        Else
            ModificarEnteroLargo = ""
        End If
    End Function
    Function ModificarEntero(ByVal Entero As Integer, ByVal Campo As String) As String
        If Entero <> 9 Then
            ModificarEntero = " " & Campo & " = " & Entero & ", "
        Else
            ModificarEntero = ""
        End If
    End Function
    Function ModificarDoble(ByVal Doble As Double, ByVal Campo As String) As String
        ' Doble = CDbl(Replace(Doble.ToString, ",", "."))
        If Doble <> 0.0001 Then
            ModificarDoble = " " & Campo & " = " & Doble & ", "
        Else
            ModificarDoble = ""
        End If
    End Function


    Function ModificarFecha(ByVal Fecha As Date, ByVal Campo As String) As String
        ModificarFecha = " " & Campo & " = '" & ft.FormatoFechaMySQL(Fecha) & "', "
    End Function
    Function ModificarFechaTiempo(ByVal Fecha As Date, ByVal Campo As String) As String
        ModificarFechaTiempo = " " & Campo & " = '" & ft.FormatoFechaHoraMySQL(Fecha) & "', "
    End Function
    Function ModificarFechaTiempoPlus(ByVal Fecha As Date, ByVal Campo As String) As String
        ModificarFechaTiempoPlus = " " & Campo & " = '" & ft.FormatoFechaHoraMySQL(Fecha) & "', "
    End Function
    Function ModificarHora(ByVal Fecha As Date, ByVal Campo As String) As String
        ModificarHora = " " & Campo & " = '" & ft.FormatoHoraCorta(Fecha) & "', "
    End Function
    Function ValorNumero(ByVal sNumero As String) As Double

        If sNumero.Trim() = "" Then sNumero = "0.00"
        If sNumero.Trim() = "-" Then sNumero = "0.00"
        If sNumero.Trim() = "." Then sNumero = "0.00"

        Return Convert.ToDouble(sNumero)

    End Function
    Function ValorPorcentajeLargo(ByVal sNumero As String) As Double

        If sNumero.Trim() = "" Then sNumero = "0.0000"
        If sNumero.Trim() = "-" Then sNumero = "0.0000"
        If sNumero.Trim() = "." Then sNumero = "0.0000"

        Return Convert.ToDouble(sNumero)

    End Function
    Function ValorEntero(ByVal sNumero As String) As Integer

        sNumero = Regex.Replace(sNumero, "[^0-9]", "")
        If Trim(sNumero) = "" Then sNumero = "0"
        ValorEntero = CInt(sNumero)

    End Function
    Function ValorEnteroLargo(ByVal sNumero As String) As Long

        sNumero = Regex.Replace(sNumero, "[^0-9]", "")
        If Trim(sNumero) = "" Then sNumero = "0"
        ValorEnteroLargo = CLng(sNumero)

    End Function
    Function ValorCantidad(ByVal sNumero As String) As Double
        If sNumero = "" Then sNumero = "0.00"
        ValorCantidad = CDbl(sNumero)
    End Function
    Function ValorCantidadLarga(ByVal sNumero As String) As Double
        If sNumero = "" Then sNumero = "0.00000"
        ValorCantidadLarga = CDbl(sNumero)
    End Function
    Function PrimerDiaSemana(ByVal sFecha As Date, Optional ByVal DiaPrimeroSemana As FirstDayOfWeek = FirstDayOfWeek.Monday) As Date
        PrimerDiaSemana = DateAdd("d", -Weekday(sFecha, DiaPrimeroSemana) + 1, sFecha)
    End Function
    Function PrimerDiaQuincena(ByVal sFecha As Date) As Date
        If sFecha.Day <= 15 Then
            PrimerDiaQuincena = CDate("01/" & Format(sFecha, "MM/yyyy"))
        Else
            PrimerDiaQuincena = CDate("16/" & Format(sFecha, "MM/yyyy"))
        End If
    End Function
    Function PrimerDiaMes(ByVal sFecha As Date) As Date
        Dim fecha As Date = CDate("01/" & Format(sFecha, "MM/yyyy"))
        Return fecha
    End Function
    Function PrimerDiaAño(ByVal sFecha As Date) As Date
        PrimerDiaAño = CDate("01/01/" & Format(sFecha, "yyyy"))
    End Function

    Function UltimoDiaSemana(ByVal sFecha As Date, Optional ByVal DiaPrimeroSemana As FirstDayOfWeek = FirstDayOfWeek.Monday) As Date
        UltimoDiaSemana = DateAdd("d", 7 - Weekday(sFecha, DiaPrimeroSemana), sFecha)
    End Function
    Function UltimoDiaQuincena(ByVal sFecha As Date) As Date
        If sFecha.Day <= 15 Then
            UltimoDiaQuincena = CDate(Format(sFecha, "15-MM-yyyy"))
        Else
            UltimoDiaQuincena = UltimoDiaMes(sFecha)
        End If
    End Function
    Function UltimoDiaMes(ByVal sFecha As Date) As Date
        UltimoDiaMes = DateAdd(DateInterval.Day, -1, CDate("01/" & Format(DateAdd(DateInterval.Month, 1, sFecha), "MM/yyyy")))
    End Function

    Function UltimoDiaAño(ByVal sFecha As Date) As Date
        UltimoDiaAño = CDate("31/12/" & Format(sFecha, "yyyy"))
    End Function
    Public Function Minutos_A_Horas(ByVal Minutos As Integer) As String
        Dim iHoras As Integer
        Dim iMinutos As Integer
        iHoras = Fix(Minutos / 60)
        iMinutos = (Minutos Mod 60)
        Minutos_A_Horas = Format(iHoras, "00") + ":" + Format(iMinutos, "00")
    End Function
    Function MinutosEntreFechas(ByVal FechaInicial As Date, ByVal FechaFinal As Date) As Integer
        MinutosEntreFechas = DateDiff(DateInterval.Minute, FechaInicial, FechaFinal)
    End Function
    Function MismaFecha(ByVal Fecha As Date) As Date
        MismaFecha = CDate("01/01/2011" & " " & Format(Fecha, "HH:mm:ss"))
    End Function
    Function Actualizar_strSQL(ByVal strSQLInicio As String, ByVal strSQLMedio As String, ByVal strSQLCierre As String) As String
        strSQLMedio = LTrim(RTrim(strSQLMedio))
        If strSQLMedio <> "" Then
            If Right(strSQLMedio, 1) = "," Then strSQLMedio = Mid(strSQLMedio, 1, Len(strSQLMedio) - 1)
            strSQLMedio = strSQLInicio & strSQLMedio & strSQLCierre
        End If
        Actualizar_strSQL = strSQLMedio
    End Function

    Function AutoCodigoMensual(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String,
                               ByVal Campo As String, ByVal Fecha As Date) As String
        Dim nW As Boolean = True

        Dim strFecha As String = Format(Fecha, "yyyyMM")
        Dim strValor As String = "00001"


        While nW
            Dim afld() As String = {Campo, "id_emp"}
            Dim aStr() As String = {strFecha & "-" & strValor, jytsistema.WorkID}
            If qFound(MyConn, lblInfo, nTabla, afld, aStr) Then
                strValor = IncrementarCadena(strValor)
            Else
                nW = False
            End If
        End While

        AutoCodigoMensual = strFecha & "-" & strValor

    End Function
    Function AutoCodigoDiario(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String,
                               ByVal Campo As String, ByVal Fecha As Date) As String
        Dim nW As Boolean = True

        Dim strFecha As String = Format(Fecha, "yyyyMMdd")
        Dim strValor As String = "00001"


        While nW
            Dim afld() As String = {Campo, "id_emp"}
            Dim aStr() As String = {strFecha & "-" & strValor, jytsistema.WorkID}
            If qFound(MyConn, lblInfo, nTabla, afld, aStr) Then
                strValor = IncrementarCadena(strValor)
            Else
                nW = False
            End If
        End While

        AutoCodigoDiario = strFecha & "-" & strValor

    End Function
    Function LicenciaValida(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label) As Integer()

        Dim sRIF As String = ft.DevuelveScalarCadena(Myconn, " select RIF from jsconctaemp where id_emp = '01' ")
        Dim strLic() As String = Split(Licencia(Myconn, sRIF), ".")


        Dim aLic() As Integer = {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0}
        If sRIF = strLic(0) Then
            aLic(0) = strLic(2).Substring(1, 1)
            aLic(1) = strLic(2).Substring(2, 1)
            aLic(2) = strLic(2).Substring(3, 1)
            aLic(3) = strLic(2).Substring(4, 1)
            aLic(4) = strLic(2).Substring(5, 1)
            aLic(5) = strLic(2).Substring(6, 1)
            aLic(6) = strLic(2).Substring(7, 1)
            aLic(7) = strLic(2).Substring(8, 1)
            aLic(8) = strLic(2).Substring(9, 1)
            aLic(9) = strLic(2).Substring(10, 1)
        Else
            ft.mensajeAdvertencia("Número de control NO Válido...")
        End If
        Return aLic

    End Function

    Function RellenaCadenaConCaracter(ByVal Cadena As String, ByVal Lado As String, ByVal LongitudTotalCadena As Integer,
                                      Optional ByVal Caracter As String = " ") As String
        Dim iCont As Integer

        If Len(Cadena) > LongitudTotalCadena Then
            RellenaCadenaConCaracter = Cadena
            Exit Function
        End If
        For iCont = 1 To LongitudTotalCadena - Len(Cadena)
            If Lado = "I" Then
                Cadena = Cadena & Caracter
            ElseIf Lado = "D" Then
                Cadena = Caracter & Cadena
            Else
                If (iCont Mod 2) <> 0 Then Cadena = Caracter & Cadena
            End If
        Next

        RellenaCadenaConCaracter = Cadena

    End Function
    Function IncrementarCadena(ByVal sCadena) As String
        Dim sSuma As Integer
        Dim sChar As String
        Dim sNum As Integer
        Dim iCont As Integer

        sSuma = 1
        IncrementarCadena = ""
        For iCont = Len(sCadena) To 1 Step -1
            sChar = Mid(sCadena, iCont, 1)
            If ft.isNumeric(sChar) Then
                sNum = Val(sChar) + sSuma
                If sNum > 9 Then
                    sSuma = 1
                    IncrementarCadena = "0" & IncrementarCadena
                Else
                    IncrementarCadena = CStr(sNum) & IncrementarCadena
                    sSuma = 0
                End If
            Else
                If Asc(sChar) >= 65 And Asc(sChar) <= 90 Then
                    sNum = Asc(sChar) + sSuma
                    If sNum > 90 Then
                        sSuma = 1
                        IncrementarCadena = "A" & IncrementarCadena
                    Else
                        sSuma = 0
                        IncrementarCadena = Chr(sNum) & IncrementarCadena
                    End If
                ElseIf sChar = "." Or sChar = "-" Then
                    IncrementarCadena = sChar & IncrementarCadena
                End If
            End If
        Next

    End Function
    Public Function qFound(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String,
                            ByVal nCampos() As String, ByVal nStrings() As String) As Boolean
        Dim tblBuscar As String = "Buscar"
        Dim dsBuscar As New DataSet
        Dim i As Integer
        Dim strBuscar As String = "", str As String

        For i = 0 To UBound(nCampos)
            strBuscar = strBuscar & " and " & nCampos(i) & " = '" & nStrings(i) & "' "
        Next
        qFound = False
        If strBuscar <> "" Then
            strBuscar = Mid(strBuscar, 5, Len(strBuscar))
            str = " select * from " & nTabla & " where " & strBuscar
            dsBuscar = ft.DataSetRequeryPlus(dsBuscar, tblBuscar, myConn, str)
            If dsBuscar.Tables(tblBuscar).Rows.Count > 0 Then
                qFound = True
            End If
        End If
        dsBuscar = Nothing

    End Function
    Public Function qFoundAndSign(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String,
        ByVal nCampos() As String, ByVal nStrings() As String, ByVal nCampoAsignable As String) As Object

        Dim tblBuscar As String = "Buscar"
        Dim dsBuscar As New DataSet
        Dim dtBusca As New DataTable
        Dim i As Integer
        Dim strBuscar As String = "", str As String

        For i = 0 To UBound(nCampos)
            strBuscar = strBuscar & " and " & nCampos(i) & " = '" & nStrings(i) & "' "
        Next
        qFoundAndSign = ""
        If strBuscar <> "" Then
            strBuscar = Mid(strBuscar, 5, Len(strBuscar))
            str = " select * from " & nTabla & " where " & strBuscar
            dtBusca = ft.AbrirDataTable(dsBuscar, tblBuscar, myConn, str)
            If dtBusca.Rows.Count > 0 Then
                qFoundAndSign = dtBusca.Rows(0).Item(nCampoAsignable)
            Else
                qFoundAndSign = ""
            End If
        End If
        dsBuscar = Nothing
        dtBusca = Nothing

    End Function

    Function DocumentoNumeroAleatorio(Base As Integer) As String
        Return "DT" & ft.RellenaConCaracter(ft.NumeroAleatorio(Base), Base.ToString.Length, "0", Transportables.lado.izquierdo)
    End Function
    '//////////////////////////////////////////////////////////////////////////////
    '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< PROCEDIMIENTOS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    Public Sub HabilitarObjetosEnGrupo(ByVal Habilitar As Boolean, ByVal CambiaColor As Boolean, ByVal oObjetos As Object)
        Dim oObjeto As Object
        For Each oObjeto In oObjetos
            oObjeto.Enabled = Habilitar
            If CambiaColor Then
                If Habilitar Then
                    oObjeto.Backcolor = ColorHabilitado
                Else
                    oObjeto.Backcolor = ColorDeshabilitado
                End If
            End If
        Next
    End Sub


    Public Function BaseDatosAImagen(ByVal MyRow As DataRow, ByVal Campo As String, ByVal Nombre As String) As String
        Dim Camino As String = ""
        If Not IsDBNull(MyRow(Campo)) Then
            Dim MyData() As Byte
            Camino = My.Computer.FileSystem.CurrentDirectory & "\" & Campo & Nombre & ".jpg"

            If My.Computer.FileSystem.FileExists(Camino) Then My.Computer.FileSystem.DeleteFile(Camino)
            MyData = MyRow(Campo)
            Dim K As Long
            K = MyData.Length
            Dim fs As New FileStream(Camino, FileMode.OpenOrCreate, FileAccess.Write)
            fs.Write(MyData, 0, K)
            fs.Close()
            fs.Dispose()
            fs = Nothing

        End If

        Return Camino

    End Function


    Public Sub DesactivarMenuBarra(ByVal BarraMenu As ToolStrip)
        Dim c As ToolStripItem
        For Each c In BarraMenu.Items
            c.Enabled = False
        Next
    End Sub

    Public Sub EnfocarTextoM(ByVal txtFoco As MaskedTextBox)
        If txtFoco.Text.Length > 0 Then txtFoco.Select(0, txtFoco.Text.Length)
        txtFoco.Focus()
    End Sub
    Public Sub EnfocarTextoTP(ByVal txtFoco As ToolStripTextBox)
        If txtFoco.Text.Length > 0 Then txtFoco.Select(0, txtFoco.Text.Length)
        txtFoco.Focus()
    End Sub

    Public Sub LimpiarTextos(ByVal ParamArray oObjetos() As Object)
        Dim oObjeto As Object
        For Each oObjeto In oObjetos
            oObjeto.text = ""
        Next
    End Sub

    Public Sub laCalculadora()

        Dim calcInfoPath As String
        On Error GoTo calculadoraErr

        If (Dir("C:\WINDOWS\CALC.EXE") <> "") Then
            calcInfoPath = "C:\WINDOWS\CALC.EXE"
            ' Error: no se puede encontrar el archivo...
        ElseIf (Dir("C:\WINDOWS\SYSTEM32\CALC.EXE") <> "") Then
            calcInfoPath = "C:\WINDOWS\SYSTEM32\CALC.EXE"
        Else
            GoTo calculadoraErr
        End If

        Call Shell(calcInfoPath, vbNormalFocus)

        Exit Sub
calculadoraErr:
        MsgBox("La calculadora no está disponible en este momento", vbOKOnly)
    End Sub

    Public Sub RellenaComboConDatatable(ByVal cmbListado As ComboBox,
                                        ByVal datTable As DataTable, ByVal DisplayMember As String, ByVal ValueMember As String,
                                        Optional ByVal ItemporDefecto As Integer = 0)

        cmbListado.DataSource = Nothing
        cmbListado.Items.Clear()

        cmbListado.DisplayMember = DisplayMember
        cmbListado.ValueMember = ValueMember
        cmbListado.DataSource = datTable


        If cmbListado.Items.Count > 0 Then cmbListado.SelectedIndex = ItemporDefecto

    End Sub
    Public Sub RellenaListaSeleccionable(ByVal CHKLista As CheckedListBox, ByVal Items As Object, ByVal ItemsChecked As Object)
        Dim i As Integer
        CHKLista.Items.Clear()
        i = 0
        For i = 0 To UBound(Items)
            CHKLista.Items.Add(Items(i), ItemsChecked(i))
        Next

    End Sub
    Public Sub IniciarTablaSeleccion(ByVal dg As DataGridView, ByVal dt As DataTable,
    ByVal aCampos() As String, ByVal aNombres() As String,
    ByVal aAnchos() As Integer, Optional ByVal aAlineacion() As Integer = Nothing,
    Optional ByVal aFormatos() As String = Nothing, Optional ByVal Encabezado As Boolean = True,
    Optional ByVal EditaCampos As Boolean = False, Optional ByVal FontSize As Single = 9,
    Optional ByVal EncabezadoDeFila As Boolean = True)
        Dim i As Integer

        dg.Columns.Clear()
        dg.AutoGenerateColumns = False
        dg.AlternatingRowsDefaultCellStyle.BackColor = Drawing.Color.AliceBlue
        dg.RowsDefaultCellStyle.SelectionBackColor = Drawing.Color.DodgerBlue
        dg.RowHeadersDefaultCellStyle.SelectionBackColor = Drawing.Color.DarkBlue
        dg.RowsDefaultCellStyle.Font = New Font("Consolas", FontSize, FontStyle.Regular)
        dg.ColumnHeadersVisible = Encabezado
        dg.RowHeadersVisible = EncabezadoDeFila

        dg.AllowUserToAddRows = False
        dg.RowTemplate.Height = 18
        dg.RowHeadersWidth = 25
        If EditaCampos Then
            dg.SelectionMode = DataGridViewSelectionMode.CellSelect
        Else
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End If
        dg.MultiSelect = False
        dg.EditMode = IIf(EditaCampos, DataGridViewEditMode.EditOnKeystrokeOrF2, DataGridViewEditMode.EditProgrammatically)

        Dim AnchoColumnas As Integer = 20

        Dim colCero As New DataGridViewCheckBoxColumn

        colCero.Name = aCampos(0)
        colCero.HeaderText = aNombres(0)
        colCero.DataPropertyName = aCampos(0)
        colCero.Width = 20

        dg.Columns.Add(colCero)


        For i = 1 To UBound(aCampos)
            If Not IsNothing(aCampos(i)) Then
                dg.Columns.Add(aCampos(i), aNombres(i))
                With dg.Columns(aCampos(i))
                    .DataPropertyName = aCampos(i)
                    .Width = aAnchos(i)
                    AnchoColumnas += aAnchos(i)
                    If Not aAlineacion Is Nothing Then .DefaultCellStyle.Alignment = aAlineacion(i)
                    .Resizable = DataGridViewTriState.False
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .HeaderCell.Style.Font = New Font("Consolas", 9, FontStyle.Bold)
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    If Not aFormatos Is Nothing Then .DefaultCellStyle.Format = aFormatos(i)
                    If i = UBound(aCampos) Then .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End With
            End If
        Next

        dg.DataSource = dt

    End Sub

    Public Sub IniciarTabla(ByVal dg As DataGridView, ByVal dt As DataTable,
        ByVal aCampos() As String, ByVal aNombres() As String,
        ByVal aAnchos() As Integer, Optional ByVal aAlineacion() As Integer = Nothing,
        Optional ByVal aFormatos() As String = Nothing, Optional ByVal Encabezado As Boolean = True,
        Optional ByVal EditaCampos As Boolean = False, Optional ByVal FontSize As Single = 9,
        Optional ByVal EncabezadoDeFila As Boolean = True, Optional AltoDeFila As Single = 18,
        Optional AsignaDataSource As Boolean = True)
        Dim i As Integer

        dg.Columns.Clear()
        dg.AutoGenerateColumns = False
        dg.AlternatingRowsDefaultCellStyle.BackColor = Drawing.Color.AliceBlue
        dg.RowsDefaultCellStyle.SelectionBackColor = Drawing.Color.DodgerBlue
        dg.RowHeadersDefaultCellStyle.SelectionBackColor = Drawing.Color.DarkBlue
        dg.RowsDefaultCellStyle.Font = New Font("Consolas", FontSize, FontStyle.Regular)
        dg.ColumnHeadersVisible = Encabezado
        dg.RowHeadersVisible = EncabezadoDeFila

        dg.AllowUserToAddRows = False
        dg.RowTemplate.Height = AltoDeFila
        dg.RowHeadersWidth = 25

        If EditaCampos Then
            dg.SelectionMode = DataGridViewSelectionMode.CellSelect
        Else
            dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End If
        dg.MultiSelect = False
        dg.EditMode = IIf(EditaCampos, DataGridViewEditMode.EditOnKeystrokeOrF2, DataGridViewEditMode.EditProgrammatically)

        Dim AnchoColumnas As Integer = 0.0
        For i = 0 To UBound(aCampos)
            If Not IsNothing(aCampos(i)) Then
                dg.Columns.Add(aCampos(i), aNombres(i))
                With dg.Columns(aCampos(i))
                    .DataPropertyName = aCampos(i)
                    .Width = aAnchos(i)
                    AnchoColumnas += aAnchos(i)
                    If Not aAlineacion Is Nothing Then .DefaultCellStyle.Alignment = aAlineacion(i)
                    .Resizable = DataGridViewTriState.False
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .HeaderCell.Style.Font = New Font("Consolas", FontSize, FontStyle.Bold)
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    If Not aFormatos Is Nothing Then .DefaultCellStyle.Format = aFormatos(i)
                    If i = UBound(aCampos) Then .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End With
            End If
        Next

        If AsignaDataSource Then dg.DataSource = dt

    End Sub

    Public Function ChildFormOpen(ByRef ParentForm As Form, ByRef Childform As Form) As Boolean
        Dim blnIsOpen As Boolean = False
        Dim frm As Form
        For Each frm In ParentForm.MdiChildren
            If Childform.Name = frm.Name Then
                Childform.Focus()
                blnIsOpen = True
                Childform = frm
                Exit For
            End If
        Next
        ChildFormOpen = blnIsOpen
        frm = Nothing
    End Function
    Public Function EliminarRegistros(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal nTabla As String, ByVal nTablaBD As String,
                                 ByVal strSQL As String, ByVal aCampos() As String, ByVal aValores() As String,
                                 ByVal nRow As Long,
                                 Optional ByVal Actualiza As Boolean = True) As Long

        Dim lCont As Integer
        Dim str As String = ""

        For lCont = 0 To UBound(aCampos)
            str = str & " " & aCampos(lCont) & " = '" & aValores(lCont) & "' and "
        Next

        ft.Ejecutar_strSQL(MyConn, " DELETE FROM " & nTablaBD & " where " _
            & Mid(str, 1, Len(str) - 4) _
            & " ")

        If Actualiza Then ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        Dim q As Integer = ds.Tables(nTabla).Rows.Count - 1
        If q < CInt(nRow) Then
            EliminarRegistros = CLng(q)
        Else
            EliminarRegistros = nRow
        End If

    End Function
    Public Function PorcentajeIVA(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label,
                          ByVal dFecha As Date, ByVal sTipo As String) As Double

        Return ft.DevuelveScalarDoble(Myconn, " select a.monto " _
                & " from (" & SeleccionGENTablaIVA(dFecha) & ") a " _
                & " where " _
                & " a.tipo = '" & sTipo & "' ")

    End Function

    Public Function ValorUnidadTributaria(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal dFEcha As Date) As Double

        ValorUnidadTributaria = ft.DevuelveScalarDoble(MyConn,
                                                      " SELECT monto FROM jsconctaunt WHERE fecha IN ( SELECT MAX(fecha) FROM " _
                                                      & " jsconctaunt WHERE fecha <= '" & ft.FormatoFechaMySQL(dFEcha) & "' AND " _
                                                      & " id_emp = '" & jytsistema.WorkID & "' ) AND " _
                                                      & " id_emp = '" & jytsistema.WorkID & "'")

    End Function

    Public Function FechaInicioEjercicio(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As Date
        If jytsistema.WorkExercise = "" Then
            Dim aFld() As String = {"id_emp"}
            Dim aSFld() As String = {jytsistema.WorkID}
            FechaInicioEjercicio = CDate(qFoundAndSign(MyConn, lblInfo, "jsconctaemp", aFld, aSFld, "inicio").ToString)
        Else
            Dim aFld() As String = {"Ejercicio", "id_emp"}
            Dim aSFld() As String = {jytsistema.WorkExercise, jytsistema.WorkID}
            FechaInicioEjercicio = CDate(qFoundAndSign(MyConn, lblInfo, "jsconctaeje", aFld, aSFld, "inicio").ToString)
        End If
    End Function
    Public Function FechaCierreEjercicio(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As Date
        If jytsistema.WorkExercise = "" Then
            Dim aFld() As String = {"id_emp"}
            Dim aSFld() As String = {jytsistema.WorkID}
            FechaCierreEjercicio = CDate(qFoundAndSign(MyConn, lblInfo, "jsconctaemp", aFld, aSFld, "cierre").ToString)
        Else
            Dim aFld() As String = {"Ejercicio", "id_emp"}
            Dim aSFld() As String = {jytsistema.WorkExercise, jytsistema.WorkID}
            FechaCierreEjercicio = CDate(qFoundAndSign(MyConn, lblInfo, "jsconctaeje", aFld, aSFld, "cierre").ToString)
        End If
    End Function
    Public Function CargarTablaSimple(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal strSQL As String, ByVal Titulo As String,
                                      ByVal textoInicial As String) As String


        CargarTablaSimple = IIf(textoInicial.Replace("-", "").Trim = "", "", textoInicial)

        Dim nTable As String = "tbl" & ft.NumeroAleatorio(10000)
        Dim dt As DataTable = ft.AbrirDataTable(ds, nTable, MyConn, strSQL)
        If dt.Rows.Count > 0 Then
            Dim f As New jsControlArcTablaSimple
            f.stringSQL = strSQL
            f.Cargar(Titulo, ds, dt, nTable, TipoCargaFormulario.iShowDialog, False, CargarTablaSimple)
            CargarTablaSimple = f.Seleccion
            f = Nothing
        End If
        dt.Dispose()
        dt = Nothing

    End Function
    Public Function CargarTablaSimplePlusReal(ByVal Titulo As String, Modulo As String) As String
        Dim f As New jsControlArcTablaSimple
        f.Cargar(Titulo, FormatoTablaSimple(Modulo), True, TipoCargaFormulario.iShowDialog)
        CargarTablaSimplePlusReal = f.Seleccion
        f = Nothing
    End Function
    Public Sub CargaListViewDesdeTabla(ByVal LV As ListView, ByVal dt As DataTable, ByVal aNombres() As String,
                                       ByVal aCampos() As String, ByVal aAnchos() As Integer, ByVal aAlineacion() As System.Windows.Forms.HorizontalAlignment,
                                       ByVal aFormato() As FormatoItemListView)
        Dim iCont As Integer
        Dim jCont As Integer

        LV.Clear()
        LV.BeginUpdate()

        For iCont = 0 To aNombres.Length - 1
            LV.Columns.Add(aNombres(iCont).ToString, aAnchos(iCont), aAlineacion(iCont))
        Next

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(FormatoCampoListView(dt.Rows(iCont).Item(aCampos(0).ToString).ToString, aFormato(0)))
            For jCont = 1 To aCampos.Length - 1
                LV.Items(iCont).SubItems.Add(FormatoCampoListView(dt.Rows(iCont).Item(aCampos(jCont).ToString).ToString, aFormato(jCont)))
            Next
        Next

        LV.EndUpdate()

    End Sub

    Public Function FormatoCampoListView(ByVal Campo As String, ByVal Formato As FormatoItemListView) As String
        Select Case Formato
            Case FormatoItemListView.iBoolean
                FormatoCampoListView = CBool(Campo)
            Case FormatoItemListView.iCadena
                FormatoCampoListView = Campo
            Case FormatoItemListView.iCantidad
                FormatoCampoListView = ft.FormatoCantidad(CDbl(Campo))
            Case FormatoItemListView.iEntero
                FormatoCampoListView = ft.FormatoEntero(CInt(Campo))
            Case FormatoItemListView.iFecha
                FormatoCampoListView = ft.FormatoFecha(CDate(Campo))
            Case FormatoItemListView.iFechaHora
                FormatoCampoListView = Campo
            Case FormatoItemListView.iNumero
                FormatoCampoListView = ft.FormatoNumero(CDbl(Campo))
            Case FormatoItemListView.iSino
                FormatoCampoListView = IIf(Campo = "0", "No", "Si")
            Case Else
                FormatoCampoListView = ""
        End Select
    End Function
    Public Function CalculaDiferenciaFechas(ByVal FechaInicial As Date, ByVal FechaFinal As Date, ByVal TipoDiferencia As DiferenciaFechas) As String

        Dim time() As Integer = ft.tiempo_transcurrido(FechaInicial, FechaFinal)
        Select Case TipoDiferencia
            Case DiferenciaFechas.iAños
                CalculaDiferenciaFechas = CStr(time(0)) & "a"
            Case DiferenciaFechas.iAñosMeses
                CalculaDiferenciaFechas = CStr(time(0)) & "a " & CStr(time(1)) & "m "
            Case DiferenciaFechas.iAñosMesesDias
                CalculaDiferenciaFechas = CStr(time(0)) & "a " & CStr(time(1)) & "m " & CStr(time(2)) & "d "
            Case Else
                CalculaDiferenciaFechas = ""
        End Select

    End Function

    Function tiempo_transcurrido(ByVal fecha_nacimiento As Date, ByVal fecha_control As Date) As Integer()
        Dim FechaActual As String = ft.FormatoFechaMySQL(fecha_control)
        Dim array_nacimiento() As String = Split(ft.FormatoFechaMySQL(fecha_nacimiento), "-")
        Dim array_actual() As String = Split(ft.FormatoFechaMySQL(fecha_control), "-")

        Dim años As Integer = CInt(array_actual(0)) - CInt(array_nacimiento(0)) - IIf(CInt(array_nacimiento(1)) > CInt(array_actual(1)), 1, 0)
        Dim meses As Integer = CInt(array_actual(1)) - CInt(array_nacimiento(1)) - IIf(CInt(array_nacimiento(2)) > CInt(array_actual(2)), 1, 0)
        Dim dias As Integer = CInt(array_actual(2)) - CInt(array_nacimiento(2))

        Dim dias_mes_anterior As Integer

        If dias < 0 Then
            Select Case CInt(array_actual(1))
                Case 1
                    dias_mes_anterior = 31
                Case 2
                    dias_mes_anterior = 31
                Case 3
                    If bisiesto(CInt(array_actual(0))) Then
                        dias_mes_anterior = 29
                    Else
                        dias_mes_anterior = 28
                    End If
                Case 4
                    dias_mes_anterior = 31
                Case 5
                    dias_mes_anterior = 30
                Case 6
                    dias_mes_anterior = 31
                Case 7
                    dias_mes_anterior = 30
                Case 8
                    dias_mes_anterior = 31
                Case 9
                    dias_mes_anterior = 31
                Case 10
                    dias_mes_anterior = 30
                Case 11
                    dias_mes_anterior = 31
                Case 12
                    dias_mes_anterior = 30
            End Select
            dias = dias + dias_mes_anterior
            If dias < 0 Then
                If dias = -1 Then
                    dias = 30
                ElseIf dias = -2 Then
                    dias = 29
                End If
            End If

        End If

        If meses < 0 Then
            If meses >= -1 Then años -= 1
            meses += 12
        End If

        Dim tiempo() As Integer = {años, meses, dias}
        tiempo_transcurrido = tiempo

    End Function

    Function bisiesto(ByVal año_actual As Integer) As Boolean
        If DateSerial(año_actual, 2, 29).Day = 29 Then bisiesto = True
    End Function

    Public Function GetNode(ByVal tag As String, ByVal parentCollection As TreeNodeCollection) As TreeNode

        Dim ret As New TreeNode
        Dim child As New TreeNode

        For Each child In parentCollection 'step through the parentcollection
            If child.Tag = tag Then
                ret = child
            ElseIf child.GetNodeCount(False) > 0 Then ' if there is child items then call this function recursively
                ret = GetNode(tag, child.Nodes)
            End If

            If Not ret Is Nothing Then Exit For 'if something was found, exit out of the for loop

        Next

        Return ret

    End Function
    Public Class clsListviewSorter ' Implements a comparer   
        Implements IComparer
        Private m_ColumnNumber As Integer
        Private m_SortOrder As SortOrder
        Public Sub New(ByVal column_number As Integer, ByVal sort_order As SortOrder)
            m_ColumnNumber = column_number
            m_SortOrder = sort_order
        End Sub
        ' Compare the items in the appropriate column  
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim item_x As ListViewItem = DirectCast(x, ListViewItem)
            Dim item_y As ListViewItem = DirectCast(y, ListViewItem)
            ' Get the sub-item values.  
            Dim string_x As String
            If item_x.SubItems.Count <= m_ColumnNumber Then
                string_x = ""
            Else
                string_x = item_x.SubItems(m_ColumnNumber).Text
            End If
            Dim string_y As String
            If item_y.SubItems.Count <= m_ColumnNumber Then
                string_y = ""
            Else
                string_y = item_y.SubItems(m_ColumnNumber).Text
            End If
            ' Compare them.  
            If m_SortOrder = SortOrder.Ascending Then
                If ft.isNumeric(string_x) And ft.isNumeric(string_y) Then
                    Return Val(string_x).CompareTo(Val(string_y))
                ElseIf IsDate(string_x) And IsDate(string_y) Then
                    Return DateTime.Parse(string_x).CompareTo(DateTime.Parse(string_y))
                Else
                    Return String.Compare(string_x, string_y)
                End If
            Else
                If ft.isNumeric(string_x) And ft.isNumeric(string_y) Then
                    Return Val(string_y).CompareTo(Val(string_x))
                ElseIf IsDate(string_x) And IsDate(string_y) Then
                    Return DateTime.Parse(string_y).CompareTo(DateTime.Parse(string_x))
                Else
                    Return String.Compare(string_y, string_x)
                End If
            End If
        End Function
    End Class
    Public Function NumerosATexto(ByVal Value As Double) As String
        Dim Resto As Double = Math.Round((Value - Int(Value)) * 100, 2)
        Value = Int(Value)
        NumerosATexto = Num2Text(Value) + " CON " + Format(Resto, "00") + "/100 CTS"
        'NumerosATexto = UCase(Letras(CStr(Value))) + " CON " + Format(Resto, "00") + "/100 CTS"
    End Function

    Public Function Num2Text(ByVal value As Double) As String

        Select Case value
            Case 0 : Num2Text = "CERO"
            Case 1 : Num2Text = "UNO"
            Case 2 : Num2Text = "DOS"
            Case 3 : Num2Text = "TRES"
            Case 4 : Num2Text = "CUATRO"
            Case 5 : Num2Text = "CINCO"
            Case 6 : Num2Text = "SEIS"
            Case 7 : Num2Text = "SIETE"
            Case 8 : Num2Text = "OCHO"
            Case 9 : Num2Text = "NUEVE"
            Case 10 : Num2Text = "DIEZ"
            Case 11 : Num2Text = "ONCE"
            Case 12 : Num2Text = "DOCE"
            Case 13 : Num2Text = "TRECE"
            Case 14 : Num2Text = "CATORCE"
            Case 15 : Num2Text = "QUINCE"
            Case Is < 20 : Num2Text = "DIECI" & Num2Text(value - 10)
            Case 20 : Num2Text = "VEINTE"
            Case Is < 30 : Num2Text = "VEINTI" & Num2Text(value - 20)
            Case 30 : Num2Text = "TREINTA"
            Case 40 : Num2Text = "CUARENTA"
            Case 50 : Num2Text = "CINCUENTA"
            Case 60 : Num2Text = "SESENTA"
            Case 70 : Num2Text = "SETENTA"
            Case 80 : Num2Text = "OCHENTA"
            Case 90 : Num2Text = "NOVENTA"
            Case Is < 100 : Num2Text = Num2Text(Int(value \ 10) * 10) & " Y " & Num2Text(value Mod 10)
            Case 100 : Num2Text = "CIEN"
            Case Is < 200 : Num2Text = "CIENTO " & Num2Text(value - 100)
            Case 200, 300, 400, 600, 800 : Num2Text = Num2Text(Int(value \ 100)) & "CIENTOS"
            Case 500 : Num2Text = "QUINIENTOS"
            Case 700 : Num2Text = "SETECIENTOS"
            Case 900 : Num2Text = "NOVECIENTOS"
            Case Is < 1000 : Num2Text = Num2Text(Int(value \ 100) * 100) & " " & Num2Text(value Mod 100)
            Case 1000 : Num2Text = "MIL"
            Case Is < 2000 : Num2Text = "MIL " & Num2Text(value Mod 1000)
            Case Is < 1000000 : Num2Text = Num2Text(Int(value \ 1000)) & " MIL"
                If value Mod 1000 Then Num2Text = Num2Text & " " & Num2Text(value Mod 1000)
            Case 1000000 : Num2Text = "UN MILLON"
            Case Is < 2000000 : Num2Text = "UN MILLON " & Num2Text(value Mod 1000000)
            Case Is < 1000000000000.0# : Num2Text = Num2Text(Int(value / 1000000)) & " MILLONES "
                If (value - Int(value / 1000000) * 1000000) Then Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000) * 1000000)
            Case 1000000000000.0# : Num2Text = "UN BILLON"
            Case Is < 2000000000000.0# : Num2Text = "UN BILLON " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
            Case Else : Num2Text = Num2Text(Int(value / 1000000000000.0#)) & " BILLONES"
                If (value - Int(value / 1000000000000.0#) * 1000000000000.0#) Then Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
        End Select

    End Function
    Public Function Letras(ByVal numero As String) As String
        '********Declara variables de tipo cadena************
        Dim palabras As String = ""
        Dim entero As String = ""
        Dim dec As String = ""
        Dim flag As String = "N"

        '********Declara variables de tipo entero***********
        Dim num, x, y As Integer


        '**********Número Negativo***********
        If Mid(numero, 1, 1) = "-" Then
            numero = Mid(numero, 2, numero.ToString.Length - 1).ToString
            'palabras = "menos "
        End If

        '**********Si tiene ceros a la izquierda*************
        For x = 1 To numero.ToString.Length
            If Mid(numero, 1, 1) = "0" Then
                numero = Trim(Mid(numero, 2, numero.ToString.Length).ToString)
                If Trim(numero.ToString.Length) = 0 Then palabras = ""
            Else
                Exit For
            End If
        Next

        '*********Dividir parte entera y decimal************
        For y = 1 To Len(numero)
            If Mid(numero, y, 1) = "." Then
                flag = "S"
            Else
                If flag = "N" Then
                    entero = entero + Mid(numero, y, 1)
                Else
                    dec = dec + Mid(numero, y, 1)
                End If
            End If
        Next y

        If Len(dec) = 1 Then dec = dec & "0"

        '**********proceso de conversión***********
        flag = "N"

        If Val(numero) <= 999999999 Then
            For y = Len(entero) To 1 Step -1
                num = Len(entero) - (y - 1)
                Select Case y
                    Case 3, 6, 9
                        '**********Asigna las palabras para las centenas***********
                        Select Case Mid(entero, num, 1)
                            Case "1"
                                If Mid(entero, num + 1, 1) = "0" And Mid(entero, num + 2, 1) = "0" Then
                                    palabras = palabras & "CIEN "
                                Else
                                    palabras = palabras & "CIENTO "
                                End If
                            Case "2"
                                palabras = palabras & "DOSCIENTOS "
                            Case "3"
                                palabras = palabras & "TRESCIENTOS "
                            Case "4"
                                palabras = palabras & "CUATROCIENTOS "
                            Case "5"
                                palabras = palabras & "QUINIENTOS "
                            Case "6"
                                palabras = palabras & "SEISCIENTOS "
                            Case "7"
                                palabras = palabras & "SETECIENTOS "
                            Case "8"
                                palabras = palabras & "OCHOCIENTOS "
                            Case "9"
                                palabras = palabras & "NOVECIENTOS "
                        End Select
                    Case 2, 5, 8
                        '*********Asigna las palabras para las decenas************
                        Select Case Mid(entero, num, 1)
                            Case "1"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    flag = "S"
                                    palabras = palabras & "DIEZ "
                                End If
                                If Mid(entero, num + 1, 1) = "1" Then
                                    flag = "S"
                                    palabras = palabras & "ONCE "
                                End If
                                If Mid(entero, num + 1, 1) = "2" Then
                                    flag = "S"
                                    palabras = palabras & "DOCE "
                                End If
                                If Mid(entero, num + 1, 1) = "3" Then
                                    flag = "S"
                                    palabras = palabras & "TRECE "
                                End If
                                If Mid(entero, num + 1, 1) = "4" Then
                                    flag = "S"
                                    palabras = palabras & "CATORCE "
                                End If
                                If Mid(entero, num + 1, 1) = "5" Then
                                    flag = "S"
                                    palabras = palabras & "QUINCE "
                                End If
                                If Mid(entero, num + 1, 1) > "5" Then
                                    flag = "N"
                                    palabras = palabras & "DIECI"
                                End If
                            Case "2"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "VEINTE "
                                    flag = "S"
                                Else
                                    palabras = palabras & "VEINTI"
                                    flag = "N"
                                End If
                            Case "3"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "TREINTA "
                                    flag = "S"
                                Else
                                    palabras = palabras & "TREINTA Y "
                                    flag = "N"
                                End If
                            Case "4"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "CUARENTA "
                                    flag = "S"
                                Else
                                    palabras = palabras & "CUARENTA Y "
                                    flag = "N"
                                End If
                            Case "5"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "CINCUENTA "
                                    flag = "S"
                                Else
                                    palabras = palabras & "CINCUENTA Y "
                                    flag = "N"
                                End If
                            Case "6"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "SESENTA "
                                    flag = "S"
                                Else
                                    palabras = palabras & "SESENTA Y "
                                    flag = "N"
                                End If
                            Case "7"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "SETENTA "
                                    flag = "S"
                                Else
                                    palabras = palabras & "SETENTA Y "
                                    flag = "N"
                                End If
                            Case "8"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "OCHENTA "
                                    flag = "S"
                                Else
                                    palabras = palabras & "OCHENTA Y "
                                    flag = "N"
                                End If
                            Case "9"
                                If Mid(entero, num + 1, 1) = "0" Then
                                    palabras = palabras & "NOVENTA "
                                    flag = "S"
                                Else
                                    palabras = palabras & "NOVENTA Y "
                                    flag = "N"
                                End If
                        End Select
                    Case 1, 4, 7
                        '*********Asigna las palabras para las unidades*********
                        Select Case Mid(entero, num, 1)
                            Case "1"
                                If flag = "N" Then
                                    If y = 1 Then
                                        palabras = palabras & "UNO "
                                    Else
                                        palabras = palabras & "UN "
                                    End If
                                End If
                            Case "2"
                                If flag = "N" Then palabras = palabras & "DOS "
                            Case "3"
                                If flag = "N" Then palabras = palabras & "TRES "
                            Case "4"
                                If flag = "N" Then palabras = palabras & "CUATRO "
                            Case "5"
                                If flag = "N" Then palabras = palabras & "CINCO "
                            Case "6"
                                If flag = "N" Then palabras = palabras & "SEIS "
                            Case "7"
                                If flag = "N" Then palabras = palabras & "SIETE "
                            Case "8"
                                If flag = "N" Then palabras = palabras & "OCHO "
                            Case "9"
                                If flag = "N" Then palabras = palabras & "NUEVE "
                        End Select
                End Select

                '***********Asigna la palabra mil***************
                If y = 4 Then
                    If Mid(entero, 6, 1) <> "0" Or Mid(entero, 5, 1) <> "0" Or Mid(entero, 4, 1) <> "0" Or
                    (Mid(entero, 6, 1) = "0" And Mid(entero, 5, 1) = "0" And Mid(entero, 4, 1) = "0" And
                    Len(entero) <= 6) Then palabras = palabras & "MIL "
                End If

                '**********Asigna la palabra millón*************
                If y = 7 Then
                    If Len(entero) = 7 And Mid(entero, 1, 1) = "1" Then
                        palabras = palabras & "MILLON "
                    Else
                        palabras = palabras & "MILLONES "
                    End If
                End If
            Next y

            '**********Une la parte entera y la parte decimal*************
            If dec <> "" Then
                Letras = palabras & "CON " & dec
            Else
                Letras = palabras
            End If
        Else
            Letras = ""
        End If
    End Function
    Public Sub HabilitarCursorEnEspera()
        Cursor.Show()
        Cursor.Current = Cursors.WaitCursor
    End Sub
    Public Sub DeshabilitarCursorEnEspera()
        Cursor.Hide()
        Cursor.Current = Cursors.Default
    End Sub
    Public Sub EsperaPorFavor()
        Dim frm As New frmEspere
        frm.ShowDialog()
        frm.Dispose()
        frm = Nothing
    End Sub
    Public Function ArregloUnidades(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String, ByVal Unidad As String) As String()
        Dim ds As New DataSet
        Dim dtEqu As DataTable
        Dim nTablEqu As String = "tblEqu"

        Dim aFld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {CodigoMercancia, jytsistema.WorkID}
        Dim kCont As Integer
        Dim aArregloX() As String = {}
        ArregloUnidades = aArregloX
        If qFound(MyConn, lblInfo, "jsmerctainv", aFld, aStr) Then
            ds = DataSetRequery(ds, " select uvalencia  from jsmerequmer where " _
                                & " codart = '" & CodigoMercancia & "' and " _
                                & " Unidad = '" & Unidad & "' AND " _
                                & " id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablEqu, lblInfo)
            dtEqu = ds.Tables(nTablEqu)

            Dim aArreglo(0 To dtEqu.Rows.Count) As String
            aArreglo(0) = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "UNIDAD")
            For kCont = 1 To dtEqu.Rows.Count
                aArreglo(kCont) = dtEqu.Rows(kCont - 1).Item("uvalencia")
            Next
            ArregloUnidades = aArreglo
        End If

    End Function



    Public Function ArregloPreciosPlus(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label,
                    ByVal CodigoItem As String, ByVal CodigoCliente As String, ByVal Equivalencia As Double,
                    Optional ByVal CodigoVendedor As String = "", Optional ByVal TipoVendedor As Integer = 0,
                    Optional FechaMovimiento As Date = MyDate) As String()

        Dim ListaPrecios() As String = {}
        Dim CodigoAsesor As String

        If CodigoCliente <> "" Then

            Dim TipoDeFacturacion As Integer = ft.DevuelveScalarEntero(MyConn, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

            If MercanciaRegulada(MyConn, lblInfo, CodigoItem) Then TipoDeFacturacion = 0

            If TipoDeFacturacion = 0 Then 'FACTURACION A PARTIR DE PRECIO

                Dim ListaEspecial As String = ft.DevuelveScalarCadena(MyConn, " select lispre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Dim PrecioEspecial As Double = ft.DevuelveScalarDoble(MyConn, " select a.precio " _
                    & " from jsmerrenlispre a " _
                    & " inner join jsmerenclispre b on (a.codlis = b.codlis and a.id_emp = b.id_emp) " _
                    & " Where " _
                    & " b.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and  " _
                    & " b.vence >= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " a.codlis = '" & ListaEspecial & "' and " _
                    & " a.codart = '" & CodigoItem & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "'")

                Dim iPosicion As Integer

                If PrecioEspecial = 0 Then

                    If CodigoVendedor = "" Then
                        CodigoAsesor = ft.DevuelveScalarCadena(MyConn, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Else
                        CodigoAsesor = CodigoVendedor
                    End If

                    Dim Lista As Integer
                    Dim Precio As Double

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_a from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select precio_a from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_B from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select precio_B from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_C from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select precio_C from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_D from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select precio_D from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_E from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select precio_E from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_F from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select precio_F from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If
                    If UBound(ListaPrecios) <= 0 Then
                        Dim Tarifa As String = ft.DevuelveScalarCadena(MyConn, " Select TARIFA from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Precio = ft.DevuelveScalarDoble(MyConn, " select precio_" & IIf(Tarifa = "", "A", Tarifa) & " from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                Else
                    iPosicion = 0
                    'ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                    ListaPrecios = {ft.FormatoNumero(PrecioEspecial)}
                    'ListaPrecios(UBound(ListaPrecios)) = 
                End If

                If UBound(ListaPrecios) = 0 Then ListaPrecios = {}

            Else ' A PARTIR DE COSTOS

                Dim ultimaCompra As Double = ft.DevuelveScalarDoble(MyConn, " select montoultimacompra from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim porcentageGanancia As Double = ft.DevuelveScalarDoble(MyConn, " select des_cli from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(Math.Round(ultimaCompra * (1 + porcentageGanancia / 100), 2))

            End If
        Else

            Dim ultimaCompra As Double = UltimoCostoAFecha(MyConn, CodigoItem, FechaMovimiento)

            ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
            ListaPrecios(UBound(ListaPrecios)) = ft.FormatoNumero(ultimaCompra)

        End If

        ArregloPreciosPlus = ListaPrecios

    End Function


    Public Function ArregloPrecios(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String) As String()

        Dim aFld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {CodigoMercancia, jytsistema.WorkID}
        Dim aArreglo() As String = {0, 0, 0, 0, 0, 0}
        aArreglo(0) = ft.FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_A"))
        aArreglo(1) = ft.FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_B"))
        aArreglo(2) = ft.FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_C"))
        aArreglo(3) = ft.FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_D"))
        aArreglo(4) = ft.FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_E"))
        aArreglo(5) = ft.FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_F"))
        ArregloPrecios = aArreglo

    End Function

    Public Function ArregloDescuentos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String) As Double()

        Dim aFld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {CodigoMercancia, jytsistema.WorkID}
        Dim aArreglo() As Double = {0, 0, 0, 0, 0, 0}
        aArreglo(0) = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "DESC_A")
        aArreglo(1) = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "DESC_B")
        aArreglo(2) = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "DESC_C")
        aArreglo(3) = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "DESC_D")
        aArreglo(4) = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "DESC_E")
        aArreglo(5) = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "DESC_F")
        ArregloDescuentos = aArreglo

    End Function

    Public Function ArregloDescuentosPlus(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label,
                ByVal CodigoItem As String, ByVal CodigoCliente As String, ByVal Equivalencia As Double,
                Optional ByVal CodigoVendedor As String = "", Optional ByVal TipoVendedor As Integer = 0) As Double()

        Dim ListaDescuentos() As Double = {}
        Dim CodigoAsesor As String

        If CodigoCliente <> "" Then

            Dim TipoDeFacturacion As Integer = ft.DevuelveScalarEntero(MyConn, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

            If MercanciaRegulada(MyConn, lblInfo, CodigoItem) Then TipoDeFacturacion = 0

            If TipoDeFacturacion = 0 Then

                Dim ListaEspecial As String = ft.DevuelveScalarCadena(MyConn, " select lispre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Dim PrecioEspecial As Double = ft.DevuelveScalarDoble(MyConn, " select a.precio " _
                    & " from jsmerrenlispre a " _
                    & " inner join jsmerenclispre b on (a.codlis = b.codlis and a.id_emp = b.id_emp) " _
                    & " Where " _
                    & " b.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and  " _
                    & " b.vence >= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " a.codlis = '" & ListaEspecial & "' and " _
                    & " a.codart = '" & CodigoItem & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "'")


                If PrecioEspecial = 0 Then

                    If CodigoVendedor = "" Then
                        CodigoAsesor = ft.DevuelveScalarCadena(MyConn, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Else
                        CodigoAsesor = CodigoVendedor
                    End If

                    Dim Lista As Integer
                    Dim Precio As Double
                    Dim Descuento As Double

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_a from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select PRECIO_A from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Descuento = ft.DevuelveScalarDoble(MyConn, " select DESC_A from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_B from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select PRECIO_B from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Descuento = ft.DevuelveScalarDoble(MyConn, " select DESC_B from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_C from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select PRECIO_C from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Descuento = ft.DevuelveScalarDoble(MyConn, " select DESC_C from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_D from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select PRECIO_D from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Descuento = ft.DevuelveScalarDoble(MyConn, " select DESC_D from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_E from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select PRECIO_E from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Descuento = ft.DevuelveScalarDoble(MyConn, " select DESC_E from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = ft.DevuelveScalarEntero(MyConn, " select lista_F from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' ")
                    Precio = ft.DevuelveScalarDoble(MyConn, " select PRECIO_F from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    Descuento = ft.DevuelveScalarDoble(MyConn, " select DESC_F from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If
                    If UBound(ListaDescuentos) <= 0 Then
                        Dim Tarifa As String = ft.DevuelveScalarCadena(MyConn, " Select TARIFA from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Descuento = ft.DevuelveScalarDoble(MyConn, " select DESC_" & IIf(Tarifa = "", "A", Tarifa) & " from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                Else
                    ListaDescuentos = {0.0}
                End If

                If UBound(ListaDescuentos) = 0 Then ListaDescuentos = {}

            Else

                ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                ListaDescuentos(UBound(ListaDescuentos)) = 0.0

            End If


        Else

            ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
            ListaDescuentos(UBound(ListaDescuentos)) = 0.0

        End If

        ArregloDescuentosPlus = ListaDescuentos

    End Function

    Public Function ArregloIVA(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As String()

        Dim ds As New DataSet
        Dim dtIVA As New DataTable
        Dim nTablaIVA As String = "tipoiva"


        Dim kCont As Integer

        ds = DataSetRequery(ds, " select tipo from jsconctaiva group by tipo ", MyConn, nTablaIVA, lblInfo)
        dtIVA = ds.Tables(nTablaIVA)

        Dim aArreglo(0 To dtIVA.Rows.Count) As String

        For kCont = 0 To dtIVA.Rows.Count - 1
            aArreglo(kCont) = dtIVA.Rows(kCont).Item("tipo")
        Next
        aArreglo(dtIVA.Rows.Count) = ""
        ArregloIVA = aArreglo

        dtIVA = Nothing
        ds = Nothing

    End Function

    Public Sub DesactivarEnGrupodeControles(ByVal GrupoDeControles As Control.ControlCollection,
                                 ByVal Control_como As Control)
        Dim c As Control
        For Each c In GrupoDeControles
            c.Enabled = False
            If c.GetType Is Control_como.GetType Then c.BackColor = Color.AliceBlue
        Next

    End Sub
    Public Sub ActivarEnGrupodeControles(ByVal GrupoDeControles As Control.ControlCollection,
                                 ByVal Control_como As Control)
        Dim c As Control
        For Each c In GrupoDeControles
            c.Enabled = True
            If c.GetType Is Control_como.GetType Then c.BackColor = Color.White
        Next

    End Sub
    Function AlmostEqual(ByVal x, ByVal y) As Boolean
        Return (Math.Abs(x - y) <= 0.001)
    End Function
    Function AlmostEqualPLUS(MyConn As MySqlConnection, ByVal x As Double, ByVal y As Double) As Boolean
        Return (Math.Abs(x - y) <= CDbl(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM06")))
    End Function
    Function Multiplo(ByVal Cantidad As Double, ByVal Equivale As Double) As Boolean
        Dim SuperEqu As Double = 0.0

        Do While SuperEqu <= Cantidad
            SuperEqu = SuperEqu + Equivale
            If AlmostEqual(Math.Round(SuperEqu, 3), Cantidad) Then Multiplo = True
        Loop

    End Function
    Public Function AjusteNumero(MyConn As MySqlConnection, Numero As Double) As Double

    End Function

    Public Sub MuestraCampo(ByVal txt As TextBox, ByVal Valor As Object)
        Valor.GetType()
    End Sub

    '/// DatumGlobals
    Public Sub ActualizarIVARenglonAlbaran(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTablaIVAMySQL As String, ByVal nTablaMySQL As String,
                                     ByVal CampoDocumento As String, ByVal NumeroDocumento As String,
                                     ByVal FechaEmision As Date, ByVal CampoTotalRenglon As String, Optional ByVal NumeroSerialFiscal As String = "")

        ft.Ejecutar_strSQL(MyConn, " delete from " & nTablaIVAMySQL _
                        & " where " & CampoDocumento & " = '" & NumeroDocumento & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

        ft.Ejecutar_strSQL(MyConn, " insert into " & nTablaIVAMySQL _
                                & " SELECT a." & CampoDocumento & ", " & IIf(nTablaIVAMySQL = "jsvenivapos", " '" & NumeroSerialFiscal & "' NUMSERIAL, 0 tipo, ", "") & " a.iva tipoiva, ROUND(IF( c.monto IS NULL, 0.00, c.monto), 2) poriva, " _
                                & " SUM(a." & CampoTotalRenglon & ") baseiva, ROUND(IF( c.monto IS NULL, 0.00, c.monto/100)*SUM(a." & CampoTotalRenglon & "),2) impiva, " _
                                & " a.ejercicio, a.id_emp " _
                                & " FROM " & nTablaMySQL & " a " _
                                & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaEmision) & ") c ON a.iva = c.tipo  " _
                                & " WHERE " _
                                & " a." & CampoDocumento & " = '" & NumeroDocumento & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY a.iva")

    End Sub
    Public Sub ActualizarRenglonesDocumento_NuevoIVA(ByVal MyConn As MySqlConnection, ByVal nTablaIVAMySQL As String, ByVal nTablaMySQL As String,
                                    ByVal CampoDocumento As String, ByVal NumeroDocumento As String)

        Dim TASA As String = ft.DevuelveScalarCadena(MyConn, " SELECT TIPOIVA FROM " & nTablaIVAMySQL & " " _
                                                     & " WHERE " _
                                                     & " " & CampoDocumento & " = '" & NumeroDocumento & "' AND " _
                                                     & " TIPOIVA <> '' LIMIT 1 ")
        If TASA <> "" Then
            ft.Ejecutar_strSQL(MyConn, " update " & nTablaMySQL & " set IVA = '" & TASA & "' " _
                               & " where " _
                               & " IVA <> '' and  " _
                               & " " & CampoDocumento & " = '" & NumeroDocumento & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

    End Sub

    Public Sub ActualizarIVARenglonAlbaranPlus(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTablaIVAMySQL As String, ByVal nTablaMySQL As String,
                                     ByVal CampoDocumento As String, ByVal NumeroDocumento As String,
                                     ByVal FechaEmision As Date, ByVal CampoTotalRenglon As String, Optional ByVal NumeroSerialFiscal As String = "")

        ft.Ejecutar_strSQL(MyConn, " delete from " & nTablaIVAMySQL _
                        & " where " & CampoDocumento & " = '" & NumeroDocumento & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

        ft.Ejecutar_strSQL(MyConn, " insert into " & nTablaIVAMySQL _
                                & " SELECT a." & CampoDocumento & ", " & IIf(nTablaIVAMySQL = "jsvenivapos", " '" & NumeroSerialFiscal & "' NUMSERIAL, 0 tipo, ", "") & " " _
                                & " IF( a.desde_1 < (a.baseiva + a.impiva) AND (a.baseiva + a.impiva) <= a.hasta_1, 'C', " _
                                & "      IF( a.desde_2 < (a.baseiva + a.impiva) AND (a.baseiva + a.impiva) <= a.hasta_2, 'B',  '' ) ) tipoiva,  " _
                                & " IF( a.desde_1 < (a.baseiva + a.impiva) AND (a.baseiva + a.impiva) <= a.hasta_1, a.monto_1, " _
                                & "      IF( a.desde_2 < (a.baseiva + a.impiva) AND (a.baseiva + a.impiva) <= a.hasta_2, a.monto_2, a.monto  ) ) poriva,  " _
                                & " a.baseiva, " _
                                & "  ROUND(IF( a.desde_1 < (a.baseiva + a.impiva) AND (a.baseiva + a.impiva) <= a.hasta_1, a.monto_1,  " _
                                & "      IF( a.desde_2 < (a.baseiva + a.impiva) AND (a.baseiva + a.impiva) <= a.hasta_2, a.monto_2, a.monto  ) )/100*a.baseiva, 2) impiva, " _
                                & " a.ejercicio, a.id_emp " _
                                & " FROM " _
                                & " (SELECT a." & CampoDocumento & ", " & IIf(nTablaIVAMySQL = "jsvenivapos", " '" & NumeroSerialFiscal & "' NUMSERIAL, 0 tipo, ", "") & " a.iva tipoiva, " _
                                & " SUM(a." & CampoTotalRenglon & ") baseiva, ROUND(IF( c.monto IS NULL, 0.00, c.monto/100)*SUM(a." & CampoTotalRenglon & "),2) impiva, " _
                                & " a.ejercicio, a.id_emp,  " _
                                & " IFNULL(c.desde, 0.00) desde, " _
                                & " IFNULL(c.hasta, 0.00) hasta, " _
                                & " IFNULL(c.monto, 0.00) monto, " _
                                & " IFNULL(c.desde_1, 0.00) desde_1, " _
                                & " IFNULL(c.hasta_1, 0.00) hasta_1, " _
                                & " IFNULL(c.monto_1, 0.00) monto_1, " _
                                & " IFNULL(c.desde_2, 0.00) desde_2, " _
                                & " IFNULL(c.hasta_2, 0.00) hasta_2, " _
                                & " IFNULL(c.monto_2, 0.00) monto_2 " _
                                & " FROM " & nTablaMySQL & " a " _
                                & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaEmision) & ") c ON a.iva = c.tipo  " _
                                & " WHERE " _
                                & " a." & CampoDocumento & " = '" & NumeroDocumento & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY a." & CampoDocumento & ", a.iva) a " _
                                & " ORDER BY a." & CampoDocumento & ", a.tipoiva")

    End Sub




    Public Function DocumentoImpreso(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label,
                                     ByVal nombreTablaEnBD As String,
                                     ByVal nombreCampoClave As String, ByVal Documento As String) As Boolean

        Dim aFld() As String = {nombreCampoClave, "id_emp"}
        Dim aStr() As String = {Documento, jytsistema.WorkID}
        DocumentoImpreso = CBool(qFoundAndSign(MyConn, lblInfo, nombreTablaEnBD, aFld, aStr, "impresa"))

    End Function
    Public Function CalculaPesoDocumento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label,
                                         ByVal TablaEnBaseDatos As String, ByVal CampoPeso As String, ByVal CampoDocumento As String, ByVal Documento As String) As Double
        CalculaPesoDocumento = ft.DevuelveScalarDoble(MyConn, " select sum(" & CampoPeso & ") from " & TablaEnBaseDatos & " where " & CampoDocumento & " = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' group by " & CampoDocumento & " ")
    End Function
    Public Function CopyDataTableColumnToArray(ByVal CopyDT As DataTable, ByVal FieldDT As String) As Object

        Dim dataArray As New ArrayList

        For Each dtRow As DataRow In CopyDT.Rows
            dataArray.Add(dtRow.Item(FieldDT))
        Next

        Return dataArray

    End Function
    Public Function NumeroControlR(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As String

        Dim num As String = ft.DevuelveScalarCadena(MyConn, " select numerocontrol from jsconnumcontrol where maquina = '" & SystemInformation.ComputerName & "' and id_emp = '" & jytsistema.WorkID & "' ")

        If num = "0" Then
            num = ""
        Else
            num = IncrementarCadena(num)
            InsertEditCONTROLNumeroControlPorMaquina(MyConn, lblInfo, False, SystemInformation.ComputerName, num)
        End If
        Return num

    End Function
    Public Sub ActualizaNumeroControl(bRet As Boolean, ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String,
                        ByVal EmisionIVADocumento As Date, ByVal origenModulo As String, ByVal OrigenGestion As String,
                        Optional ByVal CodigoClienteProveedor As String = "")

        Dim NumControlFound As String = ft.DevuelveScalarCadena(MyConn, " select num_control from jsconnumcon " _
                                                              & " where " _
                                                              & " numdoc = '" & NumeroDocumento & "' and " _
                                                              & " org = '" & origenModulo & "' and " _
                                                              & " origen = '" & OrigenGestion & "' and " _
                                                              & " id_emp = '" & jytsistema.WorkID & "' ")

        Dim origenNumeroControl As Integer = Int(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM28"))

        Dim ComputerName As String = SystemInformation.ComputerName

        If NumControlFound = "0" Then NumControlFound = ""
        If NumControlFound = "" Then
            Select Case origenNumeroControl
                Case TipoOrigenNumControl.iRegistroMaquina  'DESDE REGISTRO INDIVIDUAL DE LA MAQUINA (INDIVIDUAL)
                    NumControlFound = NumeroControlR(MyConn, lblInfo)
                Case TipoOrigenNumControl.iContadorDatum   'DESDE EL CONTADOR (GENERAL)
                    NumControlFound = Contador(MyConn, lblInfo, Gestion.iVentas, "vennumcontrol", "17")
                Case TipoOrigenNumControl.iContadorJytsuite
                    NumControlFound = ContadorJytsuite(MyConn, lblInfo, "VENNUMCONTROLFAC")
                Case TipoOrigenNumControl.iImpresoraFiscal 'DESDE LA IMPRESORA FISCAL (INDIVIDUAL IMPRESORA)
                    Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(MyConn, jytsistema.WorkBox)
                    Select Case nTipoImpreFiscal
                        Case 2, 5, 6
                            If bRet Then _
                                NumControlFound = IB.UltimoDocumentoFiscal(IIf(InStr("FAC.PVE.PFC.NDV", origenModulo) > 0,
                                                                                                           AclasBixolon.tipoDocumentoFiscal.Factura,
                                                                                                           AclasBixolon.tipoDocumentoFiscal.NotaCredito)) & NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)
                        Case 7
                            If bRet Then _
                                NumControlFound = IB.UltimoDocumentoFiscal(IIf(InStr("FAC.PVE.PFC.NDV", origenModulo) > 0,
                                                                                                              AclasBixolon.tipoDocumentoFiscal.FC_SRP812,
                                                                                                              AclasBixolon.tipoDocumentoFiscal.NC_SRP812)) & NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)
                        Case 4
                            NumControlFound = UltimaFCFiscalPnP(MyConn, lblInfo) & NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)
                    End Select
            End Select

            InsertEditCONTROLNumeroControl(MyConn, NumeroDocumento, CodigoClienteProveedor,
                                                        NumControlFound, EmisionIVADocumento, OrigenGestion,
                                                        origenModulo)


        End If

    End Sub
    Public Function ConvertirIntegerEnSiNo(ByVal str As String)
        Return IIf(str = "0", "NO", "SI")
    End Function
    '/// 
    Public Function DiasHabilesRestantesDelMes(ByVal MyConn As MySqlConnection, ByVal sFechaActual As Date, CalendarioModulo As Integer) As Integer
        Dim sFechaUltimoDia As Date

        sFechaUltimoDia = UltimoDiaMes(sFechaActual)
        DiasHabilesRestantesDelMes = DateDiff(DateInterval.Day, sFechaUltimoDia, sFechaActual) + 1

        DiasHabilesRestantesDelMes -= ft.DevuelveScalarEntero(MyConn, "select COUNT(*) from jsconcatper where  " _
                                                              & " MODULO = " & CalendarioModulo & " AND " _
                                                              & " DATE( CONCAT(ANO,'-',MES,'-', DIA) ) >= '" & ft.FormatoFechaMySQL(sFechaActual) & "' AND " _
                                                              & " DATE( CONCAT(ANO,'-',MES,'-', DIA) ) <= '" & ft.FormatoFechaMySQL(sFechaUltimoDia) & "' AND " _
                                                              & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Function
    Function DiasHabilesEnPeriodo(MyConn As MySqlConnection, FechaInicial As Date, FechaFinal As Date) As Integer

        Return DateDiff(DateInterval.Day, FechaInicial, FechaFinal) - DiasNoHabilesEnPeriodo(MyConn, FechaInicial, FechaFinal)

    End Function

    Function DiasNoHabilesEnPeriodo(MyConn As MySqlConnection, FechaInicial As Date, FechaFinal As Date) As Integer

        Return ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(*) FROM jsconcatper " _
                                                                  & " WHERE " _
                                                                  & " MODULO = 0 AND " _
                                                                  & " DATE_FORMAT( CONCAT(ano, '-', mes, '-', dia), '%Y-%m-%d' ) >= '" & ft.FormatoFechaMySQL(FechaInicial) & "' AND " _
                                                                  & " DATE_FORMAT( CONCAT(ano, '-', mes, '-', dia), '%Y-%m-%d' ) <= '" & ft.FormatoFechaMySQL(FechaFinal) & "' AND " _
                                                                  & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                                                  & " GROUP BY id_emp ")
    End Function

    Function DiasHabilesMes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sFechaActual As Date) As Integer

        Dim sFechaUltimoDia As Date = UltimoDiaMes(sFechaActual)
        DiasHabilesMes = sFechaUltimoDia.Day

        DiasHabilesMes -= ft.DevuelveScalarEntero(MyConn, "select COUNT(*) from jsconcatper where  " _
                                                  & " MODULO = 0 AND " _
                                                    & "MES = " & sFechaActual.Month & " AND " _
                                                    & "ANO = " & sFechaActual.Year & " AND " _
                                                    & "EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                                    & "ID_EMP = '" & jytsistema.WorkID & "' " _
                                                    & "GROUP BY MES ")

    End Function

    Function DiaHabil(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sFechaActual As Date) As Boolean

        DiaHabil = ft.DevuelveScalarBooleano(MyConn, "select count(*) from jsconcatper where  " _
                                             & " MODULO = 0 AND " _
            & " DIA = " & sFechaActual.Day & " AND  " _
            & " MES = " & sFechaActual.Month & " AND " _
            & " ANO = " & sFechaActual.Year & " AND " _
            & "EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & "ID_EMP = '" & jytsistema.WorkID & "' " _
            & "GROUP BY DIA ")

    End Function

    Public Function PuertoImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        PuertoImpresoraFiscal = "COM1"
        If ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(b.puerto) " _
                                                & " FROM jsvencatcaj a " _
                                                & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                & " WHERE " _
                                                & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                & " a.id_emp = '" & jytsistema.WorkID & "'") > 0 Then

            PuertoImpresoraFiscal = ft.DevuelveScalarCadena(MyConn, " SELECT b.puerto " _
                                                                         & " FROM jsvencatcaj a " _
                                                                         & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                         & " WHERE " _
                                                                         & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                         & " a.id_emp = '" & jytsistema.WorkID & "'")
        End If

        Return PuertoImpresoraFiscal

    End Function

    Public Function UltimaFACTURAImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        UltimaFACTURAImpresoraFiscal = ""

        Dim bRet As Boolean
        Select Case TipoImpresoraFiscal(MyConn, CodigoCaja)
            Case 2, 5, 6 'PP1F3
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                If bRet Then
                    UltimaFACTURAImpresoraFiscal = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.Factura)
                    IB.cerrarPuerto()
                End If
            Case 7 'BIXOLON SRP812
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                If bRet Then
                    UltimaFACTURAImpresoraFiscal = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.FC_SRP812)
                    IB.cerrarPuerto()
                End If
            Case 3 'Bematech
                Dim sr As New System.IO.StreamReader("C:\retorno.txt", System.Text.Encoding.Default, True)
                While sr.Peek() <> -1
                    Dim s As String = sr.ReadLine()
                    If String.IsNullOrEmpty(s) Then
                        Continue While
                    End If
                    If Mid(s, 1, 20) = "Contador de Factura:" Then UltimaFACTURAImpresoraFiscal = Right(RTrim(s), 6)
                End While
                sr.Close()
            Case Else
                UltimaFACTURAImpresoraFiscal = ft.DevuelveScalarCadena(MyConn, " SELECT b.ultima_factura " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "'")
        End Select

        Return UltimaFACTURAImpresoraFiscal

    End Function
    Public Function UltimaNOTACREDITOImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        Return ft.DevuelveScalarCadena(MyConn, " SELECT b.ultima_notacredito " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "'")
    End Function

    Public Function UltimaDOCNOFISCALImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        Return ft.DevuelveScalarCadena(MyConn, " SELECT b.ULTIMO_DOCNOFISCAL " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "'")

    End Function
    '/// DatumGlobals
    Public Function NumeroSERIALImpresoraFISCAL(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        Dim bRet As Boolean
        NumeroSERIALImpresoraFISCAL = ""

        Select Case TipoImpresoraFiscal(MyConn, CodigoCaja)
            Case 2, 6
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, CodigoCaja))
                If bRet Then
                    NumeroSERIALImpresoraFISCAL = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.numRegistro)
                    IB.cerrarPuerto()
                End If
            Case 5
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, CodigoCaja))
                If bRet Then
                    NumeroSERIALImpresoraFISCAL = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NR_SRP350)
                    IB.cerrarPuerto()
                End If
            Case 7
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, CodigoCaja))
                If bRet Then
                    NumeroSERIALImpresoraFISCAL = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NR_SRP812)
                    IB.cerrarPuerto()
                End If
            Case Else
                NumeroSERIALImpresoraFISCAL = ft.DevuelveScalarCadena(MyConn, " SELECT b.MAQUINAFISCAL " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "' group by a.codcaj")

        End Select

        Return NumeroSERIALImpresoraFISCAL

    End Function

    Public Function obtenerPuertosSeriePC() As List(Of String)
        Dim puertosSerie As List(Of String)

        puertosSerie = New List(Of String)
        Try
            puertosSerie = New List(Of String)
            For Each puertosSerieObtenidos As String In My.Computer.Ports.SerialPortNames
                puertosSerie.Add(puertosSerieObtenidos)
            Next
            obtenerPuertosSeriePC = puertosSerie
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical +
                   MsgBoxStyle.OkOnly)
            obtenerPuertosSeriePC = puertosSerie
        End Try
    End Function



    Public Function Codigo128(ByVal Cadena As String) As String

        Dim iCont As Integer
        Dim Peso As Integer
        Dim Caracter As String

        Peso = 104
        Codigo128 = Chr(154)
        For iCont = 1 To Len(Cadena)
            Caracter = Mid(Cadena, iCont, 1)
            Peso = Peso + iCont * AsciiToCode128B(Caracter)
            Codigo128 = Codigo128 & Caracter
        Next
        Codigo128 = Codigo128 & Code128BToAscii(Peso Mod 103) & Chr(156)

    End Function
    Public Function AsciiToCode128B(ByVal Caracter As String) As Integer
        If Asc(Caracter) = 128 Then
            Return 0
        ElseIf Asc(Caracter) >= 33 And Asc(Caracter) <= 126 Then
            Return Asc(Caracter) - 32
        ElseIf Asc(Caracter) > 126 And Asc(Caracter) <> 128 Then
            Return Asc(Caracter) - 50
        End If
    End Function
    Public Function Code128BToAscii(ByVal Valor128 As Integer) As String
        If Valor128 = 0 Then
            Code128BToAscii = Chr(128)
        ElseIf Valor128 >= 1 And Valor128 <= 94 Then
            Code128BToAscii = Chr(Valor128 + 32)
        Else
            Code128BToAscii = Chr(Valor128 + 50)
        End If
    End Function

    Public Function TraerPerfil(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal lblInfo As Label, ByVal CodigoPerfil As String) As Perfil

        Dim miPerfil As Perfil
        Dim nTablaPerfil As String = "tblPerfil" & ft.NumeroAleatorio(100000)
        Dim dtPerfil As New DataTable
        ds = DataSetRequery(ds, " select * from jsvenperven where codper = '" & CodigoPerfil & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaPerfil, lblInfo)
        dtPerfil = ds.Tables(nTablaPerfil)

        miPerfil.Contado = True
        miPerfil.Credito = False
        miPerfil.TarifaA = True
        miPerfil.TarifaB = False
        miPerfil.TarifaC = False
        miPerfil.TarifaD = False
        miPerfil.TarifaE = False
        miPerfil.TarifaF = False
        miPerfil.Almacen = "00001"
        miPerfil.Descuento = 0


        If dtPerfil.Rows.Count > 0 Then
            With dtPerfil.Rows(0)
                miPerfil.Contado = CBool(.Item("CO"))
                miPerfil.Credito = CBool(.Item("CR"))
                miPerfil.TarifaA = CBool(.Item("TARIFA_A"))
                miPerfil.TarifaB = CBool(.Item("TARIFA_B"))
                miPerfil.TarifaC = CBool(.Item("TARIFA_C"))
                miPerfil.TarifaD = CBool(.Item("TARIFA_D"))
                miPerfil.TarifaE = CBool(.Item("TARIFA_E"))
                miPerfil.TarifaF = CBool(.Item("TARIFA_F"))
                miPerfil.Almacen = .Item("ALMACEN")
                miPerfil.Descuento = .Item("DESCUENTO")
            End With
        End If

        Return miPerfil

    End Function

    Function addSlashes(str As String) As String
        Return str.Replace("'", "\")
    End Function

    Function stripSlashes(str As String) As String
        Return str.Replace("\", "'")
    End Function

    Function ReemplazarCampoEnCadena(str As String, dr As DataRow) As String
        Dim strX As String = ""
        Dim EntraACampo As Boolean = False
        Dim nCampo As String = ""

        For iCont As Integer = 0 To str.Length - 1

            If EntraACampo Then
                If str.Substring(iCont, 1) = "}" Then
                    EntraACampo = False
                    strX += " " + dr.Item(nCampo.Split(".")(1)) + " "
                    nCampo = ""
                Else
                    nCampo += str.Substring(iCont, 1)
                End If
            Else
                If str.Substring(iCont, 1) = "{" Then
                    EntraACampo = True
                Else
                    strX += str.Substring(iCont, 1)
                End If
            End If



        Next

        Return strX

    End Function

    Function LoginUser(MyConn As MySqlConnection, lblInfo As Label) As String
        Return ft.DevuelveScalarCadena(MyConn, " select usuario from jsconctausu where id_user = '" & jytsistema.sUsuario & "' ").ToString
    End Function

    Function CausaMueveInventarioNotasCredito(MyConn As MySqlConnection, lblInfo As Label, CodigoCausa As String) As Boolean
        Return ft.DevuelveScalarBooleano(MyConn, " select inventario from jsvencaudcr where codigo = '" _
                                           & CodigoCausa & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
    End Function
    Function CausaMueveInventarioNotasDebito(MyConn As MySqlConnection, lblInfo As Label, CodigoCausa As String) As Boolean
        Return ft.DevuelveScalarBooleano(MyConn, " select inventario from jsvencaudcr where codigo = '" _
                                           & CodigoCausa & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' ")
    End Function

    Public Function validarRifREGEXP(sRif As String) As Boolean
        Dim ER As New System.Text.RegularExpressions.Regex("^[JGVEP][-][0-9]{8}[-][0-9]$")
        'IsMatch nos devuelve true o false de según la expresión que le pasemos cumpla
        'o no con el patrón que le indicamos
        Return (ER.IsMatch(sRif))
    End Function
    Public Function validarCI(sCI As String) As Boolean
        Dim ER As New System.Text.RegularExpressions.Regex("^[VE][-][0-9]{4,8}$")
        Return (ER.IsMatch(sCI))
    End Function

    Public Function EsRIF(txt As String) As Boolean
        'EL tXT DE TENER LA FORMA V-11111111-1
        EsRIF = False
        Dim aCIF() As String = txt.Replace("_", "").Replace(" ", "").Split("-")
        If aCIF(2) <> "" Then EsRIF = True

    End Function
    Public Function Cedula_O_RIF(sCIF As String) As String

        Dim Identificador As String = sCIF.Replace("_", "").Replace(" ", "").Split("-")(2)

        Return sCIF.Replace("_", "").Replace(" ", "").Split("-")(0) + "-" +
                sCIF.Replace("_", "").Replace(" ", "").Split("-")(1) _
                + IIf(EsRIF(sCIF), "-" + Identificador, "")

    End Function
    Public Function validarRif(sRif As String) As Boolean

        Dim bResultado As Boolean = False
        Dim iFactor As Integer = 0

        sRif = sRif.Replace("-", "").Replace("_", "").Replace(" ", "")
        If Trim(sRif) = "" Then Return False

        If (sRif.Length < 10) Then _
            sRif = sRif.ToUpper().Substring(0, 1) + sRif.Substring(1, sRif.Length - 1).PadLeft(9, "0")

        Dim sPrimerCaracter As String = sRif.Substring(0, 1).ToUpper()

        Select Case sPrimerCaracter
            Case "V"
                iFactor = 1
            Case "E"
                iFactor = 2
            Case "J"
                iFactor = 3
            Case "P"
                iFactor = 4
            Case "G"
                iFactor = 5
        End Select

        If iFactor > 0 Then

            Dim suma As Integer = (Integer.Parse(sRif.Substring(8, 1)) * 2) _
                         + (Integer.Parse(sRif.Substring(7, 1)) * 3) _
                         + (Integer.Parse(sRif.Substring(6, 1)) * 4) _
                         + (Integer.Parse(sRif.Substring(5, 1)) * 5) _
                         + (Integer.Parse(sRif.Substring(4, 1)) * 6) _
                         + (Integer.Parse(sRif.Substring(3, 1)) * 7) _
                         + (Integer.Parse(sRif.Substring(2, 1)) * 2) _
                         + (Integer.Parse(sRif.Substring(1, 1)) * 3) _
                         + (iFactor * 4)

            Dim dividendo As Single = suma / 11
            Dim DividendoEntero As Integer = Int(dividendo)
            Dim resto As Integer = 11 - (suma - DividendoEntero * 11)

            If (resto >= 10 OrElse resto < 1) Then resto = 0
            If (sRif.Substring(9, 1).Equals(resto.ToString())) Then bResultado = True

        End If

        Return bResultado

    End Function

    Function SacarCadenaDeCadena(str As String, strInicial As String, strFinal As String) As ArrayList

        Dim iCont As Long = 0
        Dim aValores As New ArrayList()
        Dim posInicial As Integer = 0
        Dim posFinal As Integer = 0

        Do

            posInicial = InStr(iCont + 1, str, strInicial, CompareMethod.Text)

            If posInicial <> 0 Then
                posFinal = InStr(posInicial, str, strFinal, CompareMethod.Text)
                Dim newValue As String = Mid(str, posInicial, posFinal - posInicial + strFinal.Length)
                aValores.Add(newValue)
            End If

            iCont = posFinal + strFinal.Length

        Loop Until posInicial = 0

        Return aValores

    End Function


    Public Function CalculaDescuento(Monto As Double, porcentajeDescuento As Double) As Double
        Return Monto * (1 - porcentajeDescuento / 100)
    End Function

    Public Sub refrescaBarraprogresoEtiqueta(pb As ProgressBar, lbl As Label, progreso As Integer, textoProgreso As String)
        If pb IsNot Nothing Then pb.Value = progreso
        If lbl IsNot Nothing Then lbl.Text = textoProgreso
        If pb IsNot Nothing Then pb.Refresh()
        If lbl IsNot Nothing Then lbl.Refresh()
    End Sub

    Public Sub IniciarUnidadesDeMedida(MyConn As MySqlConnection)

        Dim ds As New DataSet
        Dim dtUM As DataTable = ft.AbrirDataTable(ds, "tblUnidadDeMedida", MyConn, " select * from jsconctatab where " _
                                                  & " modulo = '00035' and " _
                                                  & " id_emp = '" & jytsistema.WorkID & "' " _
                                                  & " order by codigo ")
        Dim iCont As Integer = 0
        For Each nrow As DataRow In dtUM.Rows
            With nrow
                ReDim Preserve aUnidad(iCont)
                ReDim Preserve aUnidadAbreviada(iCont)
                aUnidadAbreviada(iCont) = .Item("CODIGO")
                aUnidad(iCont) = .Item("DESCRIP")
                iCont += 1
            End With
        Next

        dtUM.Dispose()
        dtUM = Nothing

        ds.Dispose()
        ds = Nothing


    End Sub

    Public Sub InstallUpdateSyncWithInfo()

        Dim info As UpdateCheckInfo = Nothing

        If (ApplicationDeployment.IsNetworkDeployed) Then

            Dim AD As ApplicationDeployment = ApplicationDeployment.CurrentDeployment

            Try
                info = AD.CheckForDetailedUpdate()
            Catch dde As DeploymentDownloadException

            Catch ioe As InvalidOperationException

            End Try

            If (info.UpdateAvailable) Then

                Try
                    AD.Update()

                    Application.Restart()
                Catch dde As DeploymentDownloadException

                End Try

            End If

        End If
    End Sub

    Public Sub SetSelectedRowByIndex(dg As DataGridView, index As Long)
        dg.Rows().Item(index).Selected = True
        dg.FirstDisplayedScrollingRowIndex = index
    End Sub

End Module
