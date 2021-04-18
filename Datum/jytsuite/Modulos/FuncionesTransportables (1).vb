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

Module FuncionesTransportables

    Private ImpresoraBixolon As New AclasBixolon

    'BASE DE DATOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    Function DataSetRequery(ByVal rDataset As DataSet, ByVal strSQL As String, _
         ByVal myConn As MySqlConnection, ByVal nTabla As String, ByVal lblInfo As Label) As DataSet

        Dim nDataAdapter As New MySqlDataAdapter

        rDataset.EnforceConstraints = False

        If Not IsNothing(rDataset.Tables(nTabla)) Then rDataset.Tables(nTabla).Clear()

        Try
            nDataAdapter.SelectCommand = New MySqlCommand(strSQL, myConn)
            nDataAdapter.Fill(rDataset, nTabla)

        Catch ex As MySqlException
            MensajeCritico(lblInfo, ex.Message + ". Error base de datos")
        End Try
        DataSetRequery = rDataset
        nDataAdapter = Nothing

    End Function
    Function NivelUsuario(MyConn As MySqlConnection, lblInfo As Label, CodigoUsuario As String) As Integer
        NivelUsuario = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select nivel from jsconctausu where id_user = '" & CodigoUsuario & "' "))
    End Function

    Function UsuarioClaveAESPlus(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ClaveEncriptada As String) As String
        Return EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT id_user FROM jsconctausu WHERE AES_DECRYPT(PASSWORD, usuario)  = '" & ClaveEncriptada & "'  ")
    End Function

    Function UsuarioClaveAES(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Usuario As String, ByVal ClaveEncriptada As String) As Boolean

        jytsistema.sUsuario = EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select id_user from jsconctausu " _
            & " Where aes_decrypt(password, usuario)  = '" & ClaveEncriptada & "' " _
            & " and usuario = '" & Usuario & "' " _
            & " and estatus = 1 ").ToString

        jytsistema.sNombreUsuario = EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select nombre from jsconctausu " _
            & " Where aes_decrypt(password, usuario)  = '" & ClaveEncriptada & "' " _
            & " and usuario = '" & Usuario & "' " _
            & " and estatus = 1 ").ToString

        If jytsistema.sUsuario <> "0" Then UsuarioClaveAES = True

    End Function

    Function UsuarioClave(ByVal MyConn As MySqlConnection, ByVal Usuario As String, ByVal ClaveEncriptada As String) As Boolean
        Dim myReader As MySqlDataReader
        Dim myCommand = New MySqlCommand( _
            " select id_user, usuario, aes_decrypt(password,usuario) password, nombre from jsconctausu where usuario = '" & _
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
        Dim myCommand = New MySqlCommand( _
            " select aes_decrypt(num_licencia,'capidoncella31051110') num_licencia from jsconlicencia where num_control = '" & _
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

    Public Sub MuestraItemMenu(ByVal MyConn As MySqlConnection, ByVal Usuario As String, _
                                        ByVal MenuItem As ToolStripMenuItem, ByVal SubMenuItem As ToolStripMenuItem)
        'Evalue si Usuario posee derecho del Subitem en el item 
        MenuItem.DropDownItems.Add(SubMenuItem)
    End Sub
    ' Arrays ////////////////////////////////////////////////
    Public Function DeleteArrayItem(ByVal arr() As Object, ByVal Index As Integer) As Object
        'Elimina el elemento <Index> del arreglo <arr>
        Dim iCont As Integer
        For iCont = Index To arr.GetUpperBound(0) - 1
            arr(iCont) = arr(iCont + 1)
        Next
        ReDim Preserve arr(arr.GetUpperBound(0) - 2)
        Return arr
    End Function
    Public Function InArray(ByVal aArray() As Object, ByVal Elemento As Object, Optional ByVal Inicio As Integer = 0) As Integer
        Dim iCont As Integer
        InArray = 0
        If Not IsNothing(aArray) Then
            For iCont = Inicio To UBound(aArray)
                If aArray(iCont) = Elemento.ToString Then
                    InArray = iCont + 1
                End If
            Next
        End If
    End Function
    Public Function InArrayPlus(ByVal aArray As ArrayList, ByVal Elemento As Object, Optional ByVal Inicio As Integer = 0) As Integer
        Dim iCont As Integer
        InArrayPlus = 0
        If Not IsNothing(aArray) Then
            For iCont = Inicio To aArray.Count - 1
                If aArray.Item(iCont) = Elemento Then InArrayPlus = iCont + 1
            Next
        End If
    End Function
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

        Dim iCont As Integer
        MinValueInArray = arr(0)
        For iCont = 1 To arr.GetUpperBound(0) - 1
            If arr(iCont) < MinValueInArray Then MinValueInArray = arr(iCont)
        Next
        Return MinValueInArray

    End Function

    'FORMATOS /////////////////////////////////////////////////////////////////////////////
    Function FormatoTablaSimple(ByVal iModulo As Modulo) As String
        FormatoTablaSimple = Format(CInt(iModulo), "00000")
    End Function
    Function FormatoFechaMySQL(ByVal Fecha As Date) As String
        FormatoFechaMySQL = Format(Fecha, sFormatoFechaMySQL)
    End Function
    Function FormatoNumero(ByVal sNumero As Double) As String
        FormatoNumero = Format(sNumero, sFormatoNumero)
    End Function
    Function FormatoCantidad(ByVal sNumero As Double) As String
        FormatoCantidad = Format(sNumero, sFormatoCantidad)
    End Function
    Function FormatoCantidadLarga(ByVal sNumero As Double) As String
        FormatoCantidadLarga = Format(sNumero, sFormatoCantidadLarga)
    End Function
    Function FormatoEntero(ByVal sNumero As Integer) As String
        FormatoEntero = IIf(sNumero = 0, "0", Format(sNumero, sFormatoEntero))
    End Function
    Function FormatoPorcentaje(ByVal snumero As Double) As String
        FormatoPorcentaje = Format(snumero, sFormatoPorcentaje)
    End Function
    Function FormatoPorcentajeLargo(ByVal snumero As Double) As String
        FormatoPorcentajeLargo = Format(snumero, sFormatoPorcentajeLargo)
    End Function
    Function FormatoFecha(ByVal sFecha As Date) As String
        FormatoFecha = Format(sFecha, sFormatoFecha)
    End Function
    Function FormatoFechaBarra(ByVal sFecha As Date) As String
        Return Format(sFecha, sFormatoFechaBarra)
    End Function
    Function FormatoFechaMedia(ByVal sFecha As Date) As String
        FormatoFechaMedia = Format(sFecha, sFormatoFechaMedia)
    End Function
    Function FormatoFechaCorta(ByVal sFecha As Date) As String
        FormatoFechaCorta = Format(sFecha, sFormatoFechaCorta)
    End Function
    Function FormatoFechaHoraMySQL(ByVal sFecha As Date) As String
        If sFecha.Hour = 0 Then
            FormatoFechaHoraMySQL = Format(sFecha, sFormatoFechaMySQL & " " _
                                           & Format(Now().Hour, "00") & ":" _
                                           & Format(Now().Minute, "00") & ":" _
                                           & Format(Now().Second, "00"))
        Else
            FormatoFechaHoraMySQL = Format(sFecha, sFormatoFechaMySQL & " " _
                                       & Format(sFecha.Hour, "00") & ":" _
                                       & Format(sFecha.Minute, "00") & ":" _
                                       & Format(sFecha.Second, "00"))
        End If
    End Function

    Function FormatoFechaHoraMySQLInicial(ByVal sFecha As Date) As String
        FormatoFechaHoraMySQLInicial = Format(sFecha, sFormatoFechaMySQL) & " 00:00:00"
    End Function
    Function FormatoFechaHoraMySQLFinal(ByVal sFecha As Date) As String
        FormatoFechaHoraMySQLFinal = Format(sFecha, sFormatoFechaMySQL) & " 23:59:59"
    End Function
    Function FormatoHora(ByVal sHora As Date) As String
        FormatoHora = Format(sHora, sFormatoHora)
    End Function
    Function FormatoHoraCorta(ByVal sHora As Date) As String
        FormatoHoraCorta = Format(sHora, sFormatoHoraCorta)
    End Function
    Public Sub FechaString(ByVal sender As Object, ByVal e As ConvertEventArgs)
        '  e.Value = CType(e.Value, DateTime).ToString("d")
    End Sub
    Public Function ValidaNumeroEnTextbox(ByVal e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        If Not Char.IsNumber(e.KeyChar) And _
            e.KeyChar <> "." And _
                e.KeyChar <> "-" And _
                    e.KeyChar <> vbBack Then ValidaNumeroEnTextbox = True
    End Function

    Public Function ValidaNumeroEnteroEnTextbox(ByVal e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        If Not Char.IsNumber(e.KeyChar) And e.KeyChar <> vbBack Then ValidaNumeroEnteroEnTextbox = True
    End Function
    Public Function ValidaAlfaNumericoEnTextbox(ByVal e As System.Windows.Forms.KeyPressEventArgs) As Boolean
        If Not Char.IsLetterOrDigit(e.KeyChar) Then ValidaAlfaNumericoEnTextbox = True
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
            ModificarCadena = " " & Campo & " = '" & Replace(Str, "'", "''") & "', "
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
        ModificarFecha = " " & Campo & " = '" & FormatoFechaMySQL(Fecha) & "', "
    End Function
    Function ModificarFechaTiempo(ByVal Fecha As Date, ByVal Campo As String) As String
        ModificarFechaTiempo = " " & Campo & " = '" & FormatoFechaHoraMySQL(Fecha) & "', "
    End Function
    Function ModificarFechaTiempoPlus(ByVal Fecha As Date, ByVal Campo As String) As String
        ModificarFechaTiempoPlus = " " & Campo & " = '" & FormatoFechaHoraMySQL(Fecha) & "', "
    End Function
    Function ModificarHora(ByVal Fecha As Date, ByVal Campo As String) As String
        ModificarHora = " " & Campo & " = '" & FormatoHoraCorta(Fecha) & "', "
    End Function
    Function ValorNumero(ByVal sNumero As String) As Double
        If sNumero = "" Then sNumero = "0.00"
        If sNumero = "-" Then sNumero = "0.00"
        If sNumero = "." Then sNumero = "0.00"
        ValorNumero = CDbl(sNumero.ToString)
    End Function
    Function ValorPorcentajeLargo(ByVal sNumero As String) As Double
        If sNumero = "" Then sNumero = "0.0000"
        If sNumero = "-" Then sNumero = "0.0000"
        If sNumero = "." Then sNumero = "0.0000"
        ValorPorcentajeLargo = CDbl(sNumero.ToString)
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
        PrimerDiaMes = CDate("01/" & Format(sFecha, "MM/yyyy"))
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

    Function AutoCodigoMensual(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String, _
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
    Function AutoCodigoDiario(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String, _
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

        Dim sRIF As String = EjecutarSTRSQL_Scalar(Myconn, lblInfo, " select RIF from jsconctaemp where id_emp = '01' ")
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
            MensajeAdvertencia(lblInfo, "Número de control NO Válido...")
        End If
        Return aLic

    End Function
    Function AutoCodigoPlus(MyConn As MySqlConnection, Campo As String, Tabla As String, CampoDocumento As String, Documento As String, longitudMascara As Integer) As String

        Dim strUltimo As Integer = CInt(EjecutarSTRSQL_ScalarPLUS(MyConn, " select " & Campo & " from " & Tabla & " WHERE  " _
                                                                  & IIf(CampoDocumento <> "", CampoDocumento & " = '" & Documento & "' AND ", "") _
                                                                  & " ID_EMP = '" & jytsistema.WorkID & "' ORDER BY " & Campo & " DESC ").ToString)
        Dim strMask As String = ""
        For iCont As Integer = 1 To longitudMascara
            strMask += "0"
        Next
        Return Format((strUltimo + 1), strMask)

    End Function

    Function AutoCodigoXPlus(MyConn As MySqlConnection, ByVal Campo As String, ByVal Tabla As String, _
        ByVal aWhereFields() As String, ByVal aWhereValues() As String, LongitudCampo As Integer, _
        Optional primerCodigoLibre As Boolean = False) As String

        Dim strUltimo As Integer = 0
        Dim strWhere As String = ""

        For row As Integer = 0 To UBound(aWhereFields)
            strWhere += aWhereFields(row) & " = '" & aWhereValues(row) & "' AND "
        Next

        If strWhere.Length > 0 Then strWhere = strWhere.Substring(0, strWhere.Length - 4)

        strUltimo = CInt(EjecutarSTRSQL_ScalarPLUS(MyConn, " select " & Campo & " from " & Tabla & _
                                                   IIf(strWhere.Length > 0, " WHERE  " & strWhere, "") _
                                                                  & " ORDER BY " & Campo _
                                                                  & IIf(primerCodigoLibre, "", " DESC ") _
                                                                  & " LIMIT 1 ").ToString)

        AutoCodigoXPlus = IncrementarCadena(strUltimo.ToString)
        Do While Len(AutoCodigoXPlus) < LongitudCampo
            AutoCodigoXPlus = "0" & AutoCodigoXPlus
        Loop
        While CInt(EjecutarSTRSQL_ScalarPLUS(MyConn, " select count(*) from " & Tabla & " WHERE  " _
                                         & Campo & " = '" & AutoCodigoXPlus & "' " & _
                                         IIf(strWhere.Length > 0, " AND  " & strWhere, "") _
                                         & " ORDER BY " & Campo)) > 0

            AutoCodigoXPlus = IncrementarCadena(AutoCodigoXPlus)


        End While


    End Function

    Function AutoCodigo(ByVal sLen As Integer, ByVal myDS As DataSet, _
        ByVal NombreTablaEnDataset As String, ByVal NombreCampoEnBaseDatos As String, _
        Optional ByVal Ultimo As Long = 0, Optional primerCodigoLibre As Boolean = False) As String

        ' Pasando la longitud deseada y el campo tipo String
        ' Se obtiene un autoincremento del mismo

        Dim strValor As String = ""
        Dim myDataset As New DataSet

        myDataset = myDS
        Dim table As DataTable = myDataset.Tables(NombreTablaEnDataset)

        Dim view As DataView = table.DefaultView
        view.Sort = NombreCampoEnBaseDatos

        Dim newTable As DataTable = view.ToTable("tblCodigoSort", True, NombreCampoEnBaseDatos)
        With newTable
            If primerCodigoLibre Then
                If .Rows.Count > 0 Then
                    Dim Encontro As Boolean
                    Dim iCont As Integer = 1
                    While Not Encontro

                    End While
                Else
                    strValor = "1"
                End If
            Else

                Dim lUltimo As Long = .Rows.Count - 1
                If Ultimo <> 0 Then lUltimo = Ultimo
                If .Rows.Count > 0 Then
                    If IsNumeric(.Rows(lUltimo).Item(NombreCampoEnBaseDatos).ToString) Then
                        strValor = CStr(CInt(Right(.Rows(lUltimo).Item(NombreCampoEnBaseDatos).ToString, sLen)) + 1)
                    Else
                        strValor = IncrementarCadena(.Rows(lUltimo).Item(NombreCampoEnBaseDatos).ToString)
                    End If
                Else
                    strValor = "1"
                End If

            End If

        End With
        AutoCodigo = strValor
        Do While Len(AutoCodigo) < sLen
            AutoCodigo = "0" & AutoCodigo
        Loop

        table = Nothing
        view = Nothing
        myDataset = Nothing

    End Function
    Public Sub MostrarItemsEnMenuBarra(ByVal MenuBarra As ToolStrip, ByVal rItems As String, _
    ByVal rlblItems As String, ByVal ItemPosicion As String, ByVal ItemCantidad As String)
        If ItemCantidad = 0 Then
            MenuBarra.Items(rItems).Text = "0"
            MenuBarra.Items(rlblItems).Text = "de 0"
        Else
            MenuBarra.Items(rItems).Text = FormatoEntero(CInt(ItemPosicion) + 1)
            MenuBarra.Items(rlblItems).Text = String.Format("de {0}", FormatoEntero(CInt(ItemCantidad)))
        End If

    End Sub
    Function RellenaCadenaConCaracter(ByVal Cadena As String, ByVal Lado As String, ByVal LongitudTotalCadena As Integer, _
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
            If IsNumeric(sChar) Then
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
    Public Function qFound(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String, _
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
            dsBuscar = DataSetRequery(dsBuscar, str, myConn, tblBuscar, lblInfo)
            If dsBuscar.Tables(tblBuscar).Rows.Count > 0 Then
                qFound = True
            End If
        End If
        dsBuscar = Nothing

    End Function
    Public Function qFoundAndSign(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTabla As String, _
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
            dsBuscar = DataSetRequery(dsBuscar, str, myConn, tblBuscar, lblInfo)
            dtBusca = dsBuscar.Tables(tblBuscar)

            If dtBusca.Rows.Count > 0 Then
                qFoundAndSign = dtBusca.Rows(0).Item(nCampoAsignable)
            Else
                qFoundAndSign = ""
            End If
        End If
        dsBuscar = Nothing
        dtBusca = Nothing

    End Function
    Function NumeroAleatorio(ByVal Base As Long) As Long
        Randomize()
        NumeroAleatorio = Int((Base * Rnd()) + 1)
    End Function

    '//////////////////////////////////////////////////////////////////////////////
    '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< PROCEDIMIENTOS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    Public Sub HabilitarObjetos(ByVal Habilitar As Boolean, ByVal CambiaColor As Boolean, ByVal ParamArray oObjetos() As Object)
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
    Public Sub VisualizarObjetos(ByVal Visualizar As Boolean, ByVal ParamArray oObjetos() As Object)
        Dim oObjeto As Object
        For Each oObjeto In oObjetos
            oObjeto.visible = Visualizar
        Next
    End Sub
    Public Sub IniciarTextoObjetos(ByVal Tipo As FormatoItemListView, ByVal ParamArray oObjetos() As Object)
        Dim oObjeto As Object
        For Each oObjeto In oObjetos
            Select Case Tipo
                Case FormatoItemListView.iCadena
                    oObjeto.text = ""
                    oObjeto.TextAlign = HorizontalAlignment.Left
                Case FormatoItemListView.iEntero
                    oObjeto.text = "0"
                    oObjeto.TextAlign = HorizontalAlignment.Right
                Case FormatoItemListView.iCantidad
                    oObjeto.text = "0.000"
                    oObjeto.TextAlign = HorizontalAlignment.Right
                Case FormatoItemListView.iNumero
                    oObjeto.text = "0.00"
                    oObjeto.TextAlign = HorizontalAlignment.Right
                Case FormatoItemListView.iFecha
                    oObjeto.text = FormatoFecha(jytsistema.sFechadeTrabajo)
                    oObjeto.TextAlign = HorizontalAlignment.Center
                Case FormatoItemListView.iHora
                    oObjeto.text = "00:00"
                    oObjeto.TextAlign = HorizontalAlignment.Center
                Case FormatoItemListView.iSino
                    oObjeto.Checked = False
                Case Else
                    oObjeto.text = ""
                    oObjeto.TextAlign = HorizontalAlignment.Left
            End Select

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

    Public Function EjecutarSTRSQL(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal strSQL As String) As Boolean

        Dim myComm As New MySqlCommand
        Try
            myComm.Connection = MyConn
            myComm.CommandText = strSQL
            myComm.ExecuteNonQuery()

            myComm = Nothing
            Return True
        Catch ex As MySqlException
            MsgBox(ex.Number & " Error Base de Datos : " & ex.Message & " // " & strSQL, MsgBoxStyle.Critical)
            Return False
        End Try


    End Function

    Public Function EjecutarSTRSQL_Scalar(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal strSQL As String) As Object
        Dim myComm As New MySqlCommand
        EjecutarSTRSQL_Scalar = 0
        Try
            myComm.Connection = MyConn
            myComm.CommandText = strSQL
            EjecutarSTRSQL_Scalar = myComm.ExecuteScalar()

            If EjecutarSTRSQL_Scalar Is Nothing Then Return 0
        Catch ex As MySqlException
            MensajeCritico(lblInfo, "Error Base de Datos : " & ex.Message)
        Finally
            myComm = Nothing
        End Try

    End Function

    Public Function EjecutarSTRSQL_ScalarPLUS(ByVal MyConn As MySqlConnection, ByVal strSQL As String) As Object
        Dim lbl As New Label
        Return EjecutarSTRSQL_Scalar(MyConn, lbl, strSQL)
    End Function

    Public Function EjecutarSTRSQL_Reader(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal strSQL As String) As ArrayList
        Dim myComm As New MySqlCommand

        Dim obj As ArrayList = New ArrayList()

        Try
            myComm.Connection = MyConn
            myComm.CommandText = strSQL
            Dim nReader As MySqlDataReader = myComm.ExecuteReader()
            If nReader.HasRows Then
                While nReader.Read
                    obj.Add(nReader.Item(0))
                End While

            End If
            nReader.Close()
            nReader = Nothing

        Catch ex As MySqlException
            MensajeCritico(lblInfo, "Error Base de Datos : " & ex.Message)
        Finally
            myComm = Nothing

        End Try

        Return obj

    End Function

    Public Sub DesactivarMenuBarra(ByVal BarraMenu As ToolStrip)
        Dim c As ToolStripItem
        For Each c In BarraMenu.Items
            c.Enabled = False
        Next
    End Sub
    Public Sub ActivarMenuBarraX(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
        ByVal ds As DataSet, ByVal dt As DataTable, ByVal BarraMenu As ToolStrip)

        Const tblUsuario As String = "usuario"
        Dim dtUser As New DataTable
        Dim strUsuario As String

        Try
            strUsuario = "select a.id_user, b.mapa, b.incluye, b.modifica, b.elimina from jsconctausu a, jsconencmap b " _
                    & " where " _
                    & " a.mapa = b.mapa and " _
                    & " a.id_user = '" & jytsistema.sUsuario & "'"

            ds = DataSetRequery(ds, strUsuario, myConn, tblUsuario, lblInfo)
            dtUser = ds.Tables(tblUsuario)

            Dim c As ToolStripItem
            For Each c In BarraMenu.Items
                If dt Is Nothing Then
                    c.Enabled = False
                Else
                    If dt.Rows.Count > 0 Then
                        c.Enabled = True
                        If dtUser.Rows.Count > 0 Then

                            If Mid(c.Name, 1, 10) = "btnAgregar" Then c.Enabled = ItemMenuActivo(dtUser, "INCLUYE")
                            If Mid(c.Name, 1, 9) = "btnEditar" Then c.Enabled = ItemMenuActivo(dtUser, "MODIFICA")
                            If Mid(c.Name, 1, 11) = "btnEliminar" Then c.Enabled = ItemMenuActivo(dtUser, "ELIMINA")
                        End If
                    Else
                        c.Enabled = False
                        If Mid(c.Name, 1, 10) = "btnAgregar" Or c.Name = "btnSalir" Then c.Enabled = True
                    End If
                    If Mid(c.Name, 1, 9) = "lblTitulo" Then c.Enabled = True

                End If
                If c.Name = "Items" Then HabilitarObjetos(False, True, c)
            Next

            dtUser.Dispose()
            dtUser = Nothing

        Catch ex As Exception
            MensajeCritico(lblInfo, "Activar menu barra")
        End Try

    End Sub
    Public Function UsuarioPuedeIncluir(MyConn As MySqlConnection, lblInfo As Label, Region As String, Usuario As String) As Boolean
        If Region.ToString.Trim = "" Then
            Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select if(b.incluye = '', 1 , b.incluye) from jsconctausu a, jsconencmap b " _
                                                          & " where " _
                                                          & " a.mapa = b.mapa and " _
                                                          & " a.id_user = '" & Usuario & "' "))
        Else
            Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.incluye FROM jsconctausu a, jsconrenglonesmapa b " _
                                            & " WHERE " _
                                            & " b.region = '" & Region & "' AND " _
                                            & " a.mapa = b.mapa AND " _
                                            & " a.id_user = '" & Usuario & "'"))
        End If

    End Function
    Public Function UsuarioPuedeModificar(MyConn As MySqlConnection, lblInfo As Label, Region As String, Usuario As String) As Boolean
        If Region.ToString.Trim = "" Then
            Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select if(b.modifica = '', 1 , b.modifica) from jsconctausu a, jsconencmap b " _
                                                          & " where " _
                                                          & " a.mapa = b.mapa and " _
                                                          & " a.id_user = '" & Usuario & "' "))
        Else
            Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.modifica FROM jsconctausu a, jsconrenglonesmapa b " _
                                            & " WHERE " _
                                            & " b.region = '" & Region & "' AND " _
                                            & " a.mapa = b.mapa AND " _
                                            & " a.id_user = '" & Usuario & "'"))
        End If

    End Function
    Public Function UsuarioPuedeEliminar(MyConn As MySqlConnection, lblInfo As Label, Region As String, Usuario As String) As Boolean
        If Region.ToString.Trim = "" Then
            Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select if(b.elimina = '', 1 , b.elimina) from jsconctausu a, jsconencmap b " _
                                                          & " where " _
                                                          & " a.mapa = b.mapa and " _
                                                          & " a.id_user = '" & Usuario & "' "))
        Else
            Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.elimina FROM jsconctausu a, jsconrenglonesmapa b " _
                                            & " WHERE " _
                                            & " b.region = '" & Region & "' AND " _
                                            & " a.mapa = b.mapa AND " _
                                            & " a.id_user = '" & Usuario & "'"))
        End If

    End Function
    Public Sub ActivarMenuBarra(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
                                    ByVal Region As String, ByVal ds As DataSet, ByVal dt As DataTable, _
                                    ByVal BarraMenu As ToolStrip, Optional objArray() As String = Nothing)

        Try

            Dim c As ToolStripItem
            For Each c In BarraMenu.Items
                Dim nombreBoton As String = c.Name.Trim()
                If dt Is Nothing Then
                    c.Enabled = False
                Else
                    If dt.Rows.Count > 0 Then
                        c.Enabled = True
                        If Mid(nombreBoton, 1, 10) = "btnAgregar" Then c.Enabled = UsuarioPuedeIncluir(myConn, lblInfo, Region, jytsistema.sUsuario)
                        If Mid(nombreBoton, 1, 9) = "btnEditar" Then c.Enabled = UsuarioPuedeModificar(myConn, lblInfo, Region, jytsistema.sUsuario)
                        If Mid(nombreBoton, 1, 11) = "btnEliminar" Then c.Enabled = UsuarioPuedeEliminar(myConn, lblInfo, Region, jytsistema.sUsuario)
                    Else
                        c.Enabled = False
                        If Mid(nombreBoton, 1, 10) = "btnAgregar" Then c.Enabled = UsuarioPuedeIncluir(myConn, lblInfo, Region, jytsistema.sUsuario)
                        If Mid(nombreBoton, 1, 8) = "btnSalir" Then c.Enabled = True
                    End If
                    If nombreBoton = "lblTitulo" Then c.Enabled = True
                    If nombreBoton = "Items" Then c.Enabled = False
                    If Not objArray Is Nothing Then _
                        If InArray(objArray, nombreBoton) > 0 Then c.Enabled = True

                End If

            Next


        Catch ex As Exception
            MensajeCritico(lblInfo, "Activar menu barra   " & ex.Message.ToString)
        End Try

    End Sub
    Public Sub MensajeCritico(ByVal lblInfo As Label, ByVal Mensaje As String)
        MsgBox(Mensaje, MsgBoxStyle.Critical)
        '' MensajeEtiqueta(lblInfo, TipoMensaje.iError)
    End Sub
    Public Sub MensajeAdvertencia(ByVal lblInfo As Label, ByVal Mensaje As String)
        MensajeEtiqueta(lblInfo, Mensaje, TipoMensaje.iAdvertencia)
        'MsgBox(Mensaje, MsgBoxStyle.Exclamation)
    End Sub
    Public Sub MensajeInformativo(ByVal lblInfo As Label, ByVal Mensaje As String)
        MensajeEtiqueta(lblInfo, Mensaje, TipoMensaje.iInfo)
        ''MsgBox(Mensaje, MsgBoxStyle.Information)
    End Sub
    Public Sub MensajeInformativoPlus(ByVal Mensaje As String)
        MsgBox(Mensaje, MsgBoxStyle.Information)
    End Sub
    Public Sub MensajeAyuda(ByVal lblInfo As Label, ByVal Mensaje As String)
        MensajeEtiqueta(lblInfo, Mensaje, TipoMensaje.iAyuda)
        'MsgBox(Mensaje, MsgBoxStyle.MsgBoxHelp)
    End Sub
    Public Sub MensajeEtiqueta(ByVal lbl As Label, ByVal Mensaje As String, ByVal Tipo As TipoMensaje)
        lbl.Text = "           " & Mensaje
        lbl.ImageAlign = ContentAlignment.MiddleLeft
        Select Case Tipo
            Case TipoMensaje.iAdvertencia
                lbl.Image = My.Resources.Resources.i_Advertencia
            Case TipoMensaje.iAyuda
                lbl.Image = My.Resources.Resources.i_Ayuda
            Case TipoMensaje.iError
                lbl.Image = My.Resources.Resources.i_Error
            Case TipoMensaje.iInfo
                lbl.Image = My.Resources.Resources.i_Info
            Case TipoMensaje.iNinguno
                lbl.Image = Nothing

        End Select
        PlaySound(Tipo)

    End Sub
    Public Sub PlaySound(ByVal Tipo As TipoMensaje)

        Select Case Tipo
            Case TipoMensaje.iInfo
                'System.Media.SystemSounds.Hand
            Case TipoMensaje.iError
                System.Media.SystemSounds.Beep.Play()
            Case TipoMensaje.iAdvertencia
                System.Media.SystemSounds.Exclamation.Play()
            Case Else
                'System.Media.SystemSounds.Beep.Play()
        End Select

    End Sub
    Public Sub EnfocarTexto(ByVal txtFoco As TextBox)
        If txtFoco.Text.Length > 0 Then txtFoco.SelectAll()
        txtFoco.Focus()
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
    Public Sub RellenaCombo(ByVal Items As Object, ByVal cmbListado As ComboBox, Optional ByVal ItemporDefecto As Integer = 0)
        Dim i As Integer
        cmbListado.Items.Clear()
        i = 0
        For i = 0 To UBound(Items)
            If Not Items(i) Is Nothing Then cmbListado.Items.Add(Items(i))
        Next
        If cmbListado.Items.Count > 0 Then cmbListado.SelectedIndex = ItemporDefecto 'cmbListado.Text = cmbListado.Items.Item(ItemporDefecto).ToString

    End Sub
    Public Sub RellenaComboConDatatable(ByVal cmbListado As ComboBox, _
                                        ByVal datTable As DataTable, ByVal DisplayMember As String, ByVal ValueMember As String, _
                                        Optional ByVal ItemporDefecto As Integer = 0)

        cmbListado.DataSource = Nothing
        cmbListado.Items.Clear()

        cmbListado.DataSource = datTable
        cmbListado.DisplayMember = DisplayMember
        cmbListado.ValueMember = ValueMember

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
    Public Sub IniciarTablaSeleccion(ByVal dg As DataGridView, ByVal dt As DataTable, _
    ByVal aCampos() As String, ByVal aNombres() As String, _
    ByVal aAnchos() As Long, Optional ByVal aAlineacion() As Integer = Nothing, _
    Optional ByVal aFormatos() As String = Nothing, Optional ByVal Encabezado As Boolean = True, _
    Optional ByVal EditaCampos As Boolean = False, Optional ByVal FontSize As Single = 9, _
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

    Public Sub IniciarTabla(ByVal dg As DataGridView, ByVal dt As DataTable, _
        ByVal aCampos() As String, ByVal aNombres() As String, _
        ByVal aAnchos() As Long, Optional ByVal aAlineacion() As Integer = Nothing, _
        Optional ByVal aFormatos() As String = Nothing, Optional ByVal Encabezado As Boolean = True, _
        Optional ByVal EditaCampos As Boolean = False, Optional ByVal FontSize As Single = 9, _
        Optional ByVal EncabezadoDeFila As Boolean = True, Optional AltoDeFila As Single = 18)
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

        dg.DataSource = dt

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
    Public Function EliminarRegistros(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal nTabla As String, ByVal nTablaBD As String, _
                                 ByVal strSQL As String, ByVal aCampos() As String, ByVal aValores() As String, _
                                 ByVal nRow As Long, _
                                 Optional ByVal Actualiza As Boolean = True) As Long

        Dim lCont As Integer
        Dim str As String = ""

        For lCont = 0 To UBound(aCampos)
            str = str & " " & aCampos(lCont) & " = '" & aValores(lCont) & "' and "
        Next

        EjecutarSTRSQL(MyConn, lblInfo, " DELETE FROM " & nTablaBD & " where " _
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
    Public Function PorcentajeIVA(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, _
                          ByVal dFecha As Date, ByVal sTipo As String) As Double

        PorcentajeIVA = 0.0
        PorcentajeIVA = CDbl(EjecutarSTRSQL_Scalar(Myconn, lblInfo, " select monto " _
                & " from jsconctaiva where fecha in ( select MAX(fecha) from jsconctaiva where fecha <= '" & FormatoFechaMySQL(dFecha) & "' and " _
                & " tipo = '" & sTipo & "' " _
                & " group by tipo) and " _
                & " tipo = '" & sTipo & "' "))

        'Dim nTablaIVA As String = "IVA"
        'ds = DataSetRequery(ds, " select fecha, tipo, monto " _
        '        & " from jsconctaiva where fecha in ( select MAX(fecha) from jsconctaiva where fecha <= '" & FormatoFechaMySQL(dFecha) & "' and " _
        '        & " tipo = '" & sTipo & "' " _
        '        & " group by tipo) and " _
        '        & " tipo = '" & sTipo & "' ", Myconn, nTablaIVA, lblInfo)


        'If ds.Tables(nTablaIVA).Rows.Count > 0 Then PorcentajeIVA = ds.Tables(nTablaIVA).Rows(0).Item("monto")
        'ds.Tables(nTablaIVA).Dispose()


    End Function

    Public Function ValorUnidadTributaria(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal dFEcha As Date) As Double

        ValorUnidadTributaria = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, _
                                                      " SELECT monto FROM jsconctaunt WHERE fecha IN ( SELECT MAX(fecha) FROM " _
                                                      & " jsconctaunt WHERE fecha <= '" & FormatoFechaMySQL(dFEcha) & "' AND " _
                                                      & " id_emp = '" & jytsistema.WorkID & "' ) AND " _
                                                      & " id_emp = '" & jytsistema.WorkID & "'"))

    End Function
    Public Function SeleccionaFecha(FechaInicial As Date, ByVal ParamArray oObjetos() As Object) As String

        Dim oObjeto As Object
        Dim nLeft As Integer = 0
        Dim nTop As Integer = 0
        For Each oObjeto In oObjetos
            nLeft += oObjeto.left
            nTop += oObjeto.top
        Next

        Dim f As New FechaSistema
        f.Fecha = FechaInicial
        f.Cargar(nLeft, nTop)

        SeleccionaFecha = FormatoFecha(f.Fecha)
        f = Nothing

    End Function
    Public Function SeleccionaFechaPlus(ByVal FechaInicial As Date, FechaFinal As Date, PermiteFechaAnterior As Boolean, _
                                    ByVal ParamArray oObjetos() As Object) As String

        Dim oObjeto As Object
        Dim nLeft As Integer = 0
        Dim nTop As Integer = 0
        For Each oObjeto In oObjetos
            nLeft += oObjeto.left
            nTop += oObjeto.top
        Next

        Dim f As New FechaSistemaPlus
        f.Fecha = FechaInicial
        f.Cargar(FechaFinal, PermiteFechaAnterior, nLeft, nTop)

        SeleccionaFechaPlus = FormatoFecha(f.Fecha)
        f = Nothing

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
    Public Function CargarTablaSimple(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal strSQL As String, ByVal Titulo As String, _
                                      ByVal textoInicial As String) As String
        CargarTablaSimple = textoInicial
        Dim dt As DataTable
        Dim nTable As String = "tbl" & NumeroAleatorio(10000)
        ds = DataSetRequery(ds, strSQL, MyConn, nTable, lblInfo)
        dt = ds.Tables(nTable)
        If dt.Rows.Count > 0 Then
            Dim f As New jsControlArcTablaSimple
            f.Cargar(Titulo, ds, dt, nTable, TipoCargaFormulario.iShowDialog, False, textoInicial)
            CargarTablaSimple = f.Seleccion    'If f.Seleccion <> "" Then 
            f = Nothing
        End If
        dt.Dispose()
        dt = Nothing
    End Function

    Public Sub CargaListViewDesdeTabla(ByVal LV As ListView, ByVal dt As DataTable, ByVal aNombres() As String, _
                                       ByVal aCampos() As String, ByVal aAnchos() As Integer, ByVal aAlineacion() As System.Windows.Forms.HorizontalAlignment, _
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
                FormatoCampoListView = FormatoCantidad(CDbl(Campo))
            Case FormatoItemListView.iEntero
                FormatoCampoListView = FormatoEntero(CInt(Campo))
            Case FormatoItemListView.iFecha
                FormatoCampoListView = FormatoFecha(CDate(Campo))
            Case FormatoItemListView.iFechaHora
                FormatoCampoListView = Campo
            Case FormatoItemListView.iNumero
                FormatoCampoListView = FormatoNumero(CDbl(Campo))
            Case FormatoItemListView.iSino
                FormatoCampoListView = IIf(Campo = "0", "No", "Si")
            Case Else
                FormatoCampoListView = ""
        End Select
    End Function
    Public Function CalculaDiferenciaFechas(ByVal FechaInicial As Date, ByVal FechaFinal As Date, ByVal TipoDiferencia As DiferenciaFechas) As String

        Dim time() As Integer = tiempo_transcurrido(FechaInicial, FechaFinal)
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
        Dim FechaActual As String = FormatoFechaMySQL(fecha_control)
        Dim array_nacimiento() As String = Split(FormatoFechaMySQL(fecha_nacimiento), "-")
        Dim array_actual() As String = Split(FormatoFechaMySQL(fecha_control), "-")

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
                If IsNumeric(string_x) And IsNumeric(string_y) Then
                    Return Val(string_x).CompareTo(Val(string_y))
                ElseIf IsDate(string_x) And IsDate(string_y) Then
                    Return DateTime.Parse(string_x).CompareTo(DateTime.Parse(string_y))
                Else
                    Return String.Compare(string_x, string_y)
                End If
            Else
                If IsNumeric(string_x) And IsNumeric(string_y) Then
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
                    If Mid(entero, 6, 1) <> "0" Or Mid(entero, 5, 1) <> "0" Or Mid(entero, 4, 1) <> "0" Or _
                    (Mid(entero, 6, 1) = "0" And Mid(entero, 5, 1) = "0" And Mid(entero, 4, 1) = "0" And _
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



    Public Function ArregloPreciosPlus(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                    ByVal CodigoItem As String, ByVal CodigoCliente As String, ByVal Equivalencia As Double, _
                    Optional ByVal CodigoVendedor As String = "", Optional ByVal TipoVendedor As Integer = 0, _
                    Optional FechaMovimiento As Date = MyDate) As String()

        Dim ListaPrecios() As String = {}
        Dim CodigoAsesor As String

        If CodigoCliente <> "" Then

            Dim TipoDeFacturacion As Integer = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

            If MercanciaRegulada(MyConn, lblInfo, CodigoItem) Then TipoDeFacturacion = 0

            If TipoDeFacturacion = 0 Then 'FACTURACION A PARTIR DE PRECIO

                Dim ListaEspecial As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lispre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Dim PrecioEspecial As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select a.precio " _
                    & " from jsmerrenlispre a " _
                    & " inner join jsmerenclispre b on (a.codlis = b.codlis and a.id_emp = b.id_emp) " _
                    & " Where " _
                    & " b.emision <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and  " _
                    & " b.vence >= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " a.codlis = '" & ListaEspecial & "' and " _
                    & " a.codart = '" & CodigoItem & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "'"))

                Dim iPosicion As Integer

                If PrecioEspecial = 0 Then

                    If CodigoVendedor = "" Then
                        CodigoAsesor = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Else
                        CodigoAsesor = CodigoVendedor
                    End If

                    Dim Lista As Integer
                    Dim Precio As Double

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_a from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_a from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_B from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_B from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_C from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_C from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_D from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_D from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_E from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_E from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_F from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_F from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If
                    If UBound(ListaPrecios) <= 0 Then
                        Dim Tarifa As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " Select TARIFA from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_" & IIf(Tarifa = "0", "A", Tarifa) & " from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                        ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                        ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(Precio / Equivalencia, 2))
                    End If

                Else
                    iPosicion = 0
                    'ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                    ListaPrecios = {FormatoNumero(PrecioEspecial)}
                    'ListaPrecios(UBound(ListaPrecios)) = 
                End If

                If UBound(ListaPrecios) = 0 Then ListaPrecios = {}

            Else ' A PARTIR DE COSTOS

                Dim ultimaCompra As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select montoultimacompra from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Dim porcentageGanancia As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select des_cli from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))
                ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
                ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(Math.Round(ultimaCompra * (1 + porcentageGanancia / 100), 2))

            End If
        Else

            'Dim ultimaCompra As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select montoultimacompra from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
            Dim ultimaCompra As Double = UltimoCostoAFecha(MyConn, lblInfo, CodigoItem, FechaMovimiento)

            ReDim Preserve ListaPrecios(UBound(ListaPrecios) + 1)
            ListaPrecios(UBound(ListaPrecios)) = FormatoNumero(ultimaCompra)

        End If

        ArregloPreciosPlus = ListaPrecios

    End Function


    Public Function ArregloPrecios(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String) As String()

        Dim aFld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {CodigoMercancia, jytsistema.WorkID}
        Dim aArreglo() As String = {0, 0, 0, 0, 0, 0}
        aArreglo(0) = FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_A"))
        aArreglo(1) = FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_B"))
        aArreglo(2) = FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_C"))
        aArreglo(3) = FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_D"))
        aArreglo(4) = FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_E"))
        aArreglo(5) = FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "Precio_F"))
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

    Public Function ArregloDescuentosPlus(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                ByVal CodigoItem As String, ByVal CodigoCliente As String, ByVal Equivalencia As Double, _
                Optional ByVal CodigoVendedor As String = "", Optional ByVal TipoVendedor As Integer = 0) As Double()

        Dim ListaDescuentos() As Double = {}
        Dim CodigoAsesor As String

        If CodigoCliente <> "" Then

            Dim TipoDeFacturacion As Integer = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

            If MercanciaRegulada(MyConn, lblInfo, CodigoItem) Then TipoDeFacturacion = 0

            If TipoDeFacturacion = 0 Then

                Dim ListaEspecial As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lispre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Dim PrecioEspecial As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select a.precio " _
                    & " from jsmerrenlispre a " _
                    & " inner join jsmerenclispre b on (a.codlis = b.codlis and a.id_emp = b.id_emp) " _
                    & " Where " _
                    & " b.emision <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and  " _
                    & " b.vence >= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " a.codlis = '" & ListaEspecial & "' and " _
                    & " a.codart = '" & CodigoItem & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "'"))


                If PrecioEspecial = 0 Then

                    If CodigoVendedor = "" Then
                        CodigoAsesor = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Else
                        CodigoAsesor = CodigoVendedor
                    End If

                    Dim Lista As Integer
                    Dim Precio As Double
                    Dim Descuento As Double

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_a from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select PRECIO_A from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Descuento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select DESC_A from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_B from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select PRECIO_B from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Descuento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select DESC_B from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_C from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select PRECIO_C from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Descuento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select DESC_C from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_D from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select PRECIO_D from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Descuento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select DESC_D from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_E from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select PRECIO_E from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Descuento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select DESC_E from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                    Lista = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lista_F from jsvencatven where codven = '" & CodigoAsesor & "' and tipo = " & TipoVendedor & "  and id_emp = '" & jytsistema.WorkID & "' "))
                    Precio = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select PRECIO_F from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    Descuento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select DESC_F from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If Lista = 1 And Precio > 0 Then
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If
                    If UBound(ListaDescuentos) <= 0 Then
                        Dim Tarifa As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " Select TARIFA from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Descuento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select DESC_" & IIf(Tarifa = "0", "A", Tarifa) & " from jsmerctainv where codart = '" & CodigoItem & "' and id_emp = '" & jytsistema.WorkID & "' "))
                        ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
                        ListaDescuentos(UBound(ListaDescuentos)) = Descuento
                    End If

                Else
                    'iPosicion = 0
                    'ReDim Preserve ListaDescuentos(UBound(ListaDescuentos) + 1)
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

    Public Sub DesactivarEnGrupodeControles(ByVal GrupoDeControles As Control.ControlCollection, _
                                 ByVal Control_como As Control)
        Dim c As Control
        For Each c In GrupoDeControles
            c.Enabled = False
            If c.GetType Is Control_como.GetType Then c.BackColor = Color.AliceBlue
        Next

    End Sub
    Public Sub ActivarEnGrupodeControles(ByVal GrupoDeControles As Control.ControlCollection, _
                                 ByVal Control_como As Control)
        Dim c As Control
        For Each c In GrupoDeControles
            c.Enabled = True
            If c.GetType Is Control_como.GetType Then c.BackColor = Color.White
        Next

    End Sub
    Function AlmostEqual(ByVal x, ByVal y) As Boolean
        AlmostEqual = (Math.Abs(x - y) <= 0.001)
    End Function
    Function Multiplo(ByVal Cantidad As Double, ByVal Equivale As Double) As Boolean
        Dim SuperEqu As Double = 0.0

        Do While SuperEqu <= Cantidad
            SuperEqu = SuperEqu + Equivale
            If AlmostEqual(Math.Round(SuperEqu, 3), Cantidad) Then Multiplo = True
        Loop

    End Function
    Public Sub MuestraCampo(ByVal txt As TextBox, ByVal Valor As Object)
        Valor.GetType()
    End Sub
    Function MuestraCampoTexto(ByVal Item As Object, Optional ByVal strDefecto As String = "") As String
        MuestraCampoTexto = IIf(IsDBNull(Item), strDefecto, Item.ToString)
    End Function

    Public Sub ActualizarIVARenglonAlbaran(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nTablaIVAMySQL As String, ByVal nTablaMySQL As String, _
                                     ByVal CampoDocumento As String, ByVal NumeroDocumento As String, _
                                     ByVal FechaEmision As Date, ByVal CampoTotalRenglon As String, Optional ByVal NumeroSerialFiscal As String = "")

        EjecutarSTRSQL(MyConn, lblInfo, " delete from " & nTablaIVAMySQL _
                        & " where " & CampoDocumento & " = '" & NumeroDocumento & "' and " _
                        & IIf(NumeroSerialFiscal <> "", " numserial = '" & NumeroSerialFiscal & "' AND ", "") _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

        EjecutarSTRSQL(MyConn, lblInfo, " insert into " & nTablaIVAMySQL _
                                & " SELECT a." & CampoDocumento & ", " & IIf(nTablaIVAMySQL = "jsvenivapos", " '" & NumeroSerialFiscal & "' NUMSERIAL, 0 tipo, ", "") & " a.iva tipoiva, ROUND(IF( c.monto IS NULL, 0.00, c.monto), 2) poriva, " _
                                & " SUM(a." & CampoTotalRenglon & ") baseiva, ROUND(IF( c.monto IS NULL, 0.00, c.monto/100)*SUM(a." & CampoTotalRenglon & "),2) impiva, " _
                                & " a.ejercicio, a.id_emp " _
                                & " FROM " & nTablaMySQL & " a " _
                                & " LEFT JOIN (SELECT fecha, tipo, monto FROM jsconctaiva WHERE fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaEmision) & "' GROUP BY tipo )) c ON a.iva = c.tipo  " _
                                & " WHERE " _
                                & " a." & CampoDocumento & " = '" & NumeroDocumento & "' AND " _
                                & IIf(NumeroSerialFiscal <> "", " NUMSERIAL = '" & NumeroSerialFiscal & "' AND ", "") _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY a." & CampoDocumento & ", a.iva")

    End Sub

    Public Function DocumentoImpreso(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                     ByVal nombreTablaEnBD As String, _
                                     ByVal nombreCampoClave As String, ByVal Documento As String) As Boolean

        Dim aFld() As String = {nombreCampoClave, "id_emp"}
        Dim aStr() As String = {Documento, jytsistema.WorkID}
        DocumentoImpreso = CBool(qFoundAndSign(MyConn, lblInfo, nombreTablaEnBD, aFld, aStr, "impresa"))

    End Function
    Public Function CalculaPesoDocumento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                         ByVal TablaEnBaseDatos As String, ByVal CampoPeso As String, ByVal CampoDocumento As String, ByVal Documento As String) As Double
        CalculaPesoDocumento = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(" & CampoPeso & ") peso from " & TablaEnBaseDatos & " where " & CampoDocumento & " = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' group by " & CampoDocumento & " "))
    End Function
    Public Function CopyDataTableColumnToArray(ByVal CopyDT As DataTable, ByVal FieldDT As String) As Object

        Dim dataArray As New ArrayList

        For Each dtRow As DataRow In CopyDT.Rows
            dataArray.Add(dtRow.Item(FieldDT))
        Next

        Return dataArray

    End Function
    Public Function NumeroControlR(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As String

        Dim num As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select numerocontrol from jsconnumcontrol where maquina = '" & SystemInformation.ComputerName & "' and id_emp = '" & jytsistema.WorkID & "' "))

        If num = "0" Then
            num = ""
        Else
            num = IncrementarCadena(num)
            InsertEditCONTROLNumeroControlPorMaquina(MyConn, lblInfo, False, SystemInformation.ComputerName, num)
        End If
        Return num

    End Function
    Public Sub ActualizaNumeroControl(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String, _
                        ByVal EmisionIVADocumento As Date, ByVal origenModulo As String, ByVal OrigenGestion As String, _
                        Optional ByVal CodigoClienteProveedor As String = "")

        Dim NumControlFound As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select num_control from jsconnumcon " _
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
                    Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox)
                    Select Case nTipoImpreFiscal
                        Case 2, 5, 6
                            NumControlFound = ImpresoraBixolon.UltimoDocumentoFiscalAclasBixolon(IIf(InStr(origenModulo, "FAC.PVE.PFC.NDV") > 0, _
                                                                                                       AclasBixolon.tipoDocumentoFiscal.Factura,
                                                                                                       AclasBixolon.tipoDocumentoFiscal.NotaCredito)) & NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)
                        Case 4
                            NumControlFound = UltimaFCFiscalPnP(MyConn, lblInfo) & NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)
                    End Select
            End Select

            InsertEditCONTROLNumeroControl(MyConn, lblInfo, True, NumeroDocumento, CodigoClienteProveedor, _
                                                        NumControlFound, EmisionIVADocumento, OrigenGestion, _
                                                        origenModulo, NumeroDocumento, "")


        End If

    End Sub
    Public Function ConvertirIntegerEnSiNo(ByVal str As String)
        Return IIf(str = "0", "NO", "SI")
    End Function
    Public Function DiasHabilesRestantesDelMes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sFechaActual As Date) As Integer
        Dim sFechaUltimoDia As Date

        sFechaUltimoDia = UltimoDiaMes(sFechaActual)
        DiasHabilesRestantesDelMes = DateDiff(DateInterval.Day, sFechaUltimoDia, sFechaActual) + 1

        DiasHabilesRestantesDelMes -= CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select count(*) As DiasNoHabiles from jsconcatper where  " _
            & "DIA >= " & sFechaActual.Day & " AND " _
            & "MES = " & sFechaActual.Month & " AND " _
            & "ANO = " & sFechaActual.Year & " AND " _
            & "EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & "ID_EMP = '" & jytsistema.WorkID & "' " _
            & "GROUP BY MES ").ToString)

    End Function
    Function DiasHabilesEnPeriodo(MyConn As MySqlConnection, FechaInicial As Date, FechaFinal As Date) As Integer

        Return DateDiff(DateInterval.Day, FechaInicial, FechaFinal) - DiasNoHabilesEnPeriodo(MyConn, FechaInicial, FechaFinal)

    End Function

    Function DiasNoHabilesEnPeriodo(MyConn As MySqlConnection, FechaInicial As Date, FechaFinal As Date) As Integer

        Return CInt(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT COUNT(*) FROM jsconcatper " _
                                                                  & " WHERE " _
                                                                  & " DATE_FORMAT( CONCAT(ano, '-', mes, '-', dia), '%Y-%m-%d' ) >= '" & FormatoFechaMySQL(FechaInicial) & "' AND " _
                                                                  & " DATE_FORMAT( CONCAT(ano, '-', mes, '-', dia), '%Y-%m-%d' ) <= '" & FormatoFechaMySQL(FechaFinal) & "' AND " _
                                                                  & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                                                  & " GROUP BY id_emp "))
    End Function

    Function DiasHabilesMes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sFechaActual As Date) As Integer

        Dim sFechaUltimoDia As Date = UltimoDiaMes(sFechaActual)
        DiasHabilesMes = sFechaUltimoDia.Day

        DiasHabilesMes -= CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select count(*) As DiasNoHabiles from jsconcatper where  " _
            & "MES = " & sFechaActual.Month & " AND " _
            & "ANO = " & sFechaActual.Year & " AND " _
            & "EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & "ID_EMP = '" & jytsistema.WorkID & "' " _
            & "GROUP BY MES ").ToString)

    End Function

    Function DiaHabil(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sFechaActual As Date) As Boolean

        DiaHabil = CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select count(*) As Cuenta  from jsconcatper where  " _
            & " DIA = " & sFechaActual.Day & " AND  " _
            & " MES = " & sFechaActual.Month & " AND " _
            & " ANO = " & sFechaActual.Year & " AND " _
            & "EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & "ID_EMP = '" & jytsistema.WorkID & "' " _
            & "GROUP BY DIA ").ToString)

    End Function
   
    Public Function TipoImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As Integer

        'Public aFiscal() As String = {"Factura Normal (FORMA LIBRE)", "Factura pre-impresa fiscal (FORMA FISCAL PRE-IMPRESA)", _
        '                          "Aclas PPF1F3", "Bematech MP-2100", "Epson/PnP PF-220-II", _
        '                          "Bixolon SRP-350", "Tally Dascon 1125"}

        Dim nn As Object = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.tipoimpresora " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "'")

        Return IIf(IsDBNull(nn), 99, CInt(nn))

    End Function

    Public Function PuertoImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        PuertoImpresoraFiscal = "COM1"
        If CInt(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT COUNT(b.puerto) " _
                                                & " FROM jsvencatcaj a " _
                                                & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                & " WHERE " _
                                                & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                & " a.id_emp = '" & jytsistema.WorkID & "'")) > 0 Then

            PuertoImpresoraFiscal = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.puerto " _
                                                                         & " FROM jsvencatcaj a " _
                                                                         & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                         & " WHERE " _
                                                                         & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                         & " a.id_emp = '" & jytsistema.WorkID & "'")
        End If

        Return PuertoImpresoraFiscal

    End Function

    Public Function UltimaFACTURAImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        Return CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.ultima_factura " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "'"))
    End Function
    Public Function UltimaNOTACREDITOImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        Return CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.ultima_notacredito " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "'"))
    End Function

    Public Function UltimaDOCNOFISCALImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        Return CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT b.ULTIMO_DOCNOFISCAL " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "'"))

    End Function

    Public Function NumeroSERIALImpresoraFISCAL(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCaja As String) As String

        Dim nn As Object = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT IF( b.MAQUINAFISCAL IS NULL, '', b.maquinafiscal) " _
                                                                     & " FROM jsvencatcaj a " _
                                                                     & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                                     & " WHERE " _
                                                                     & " a.codcaj = '" & CodigoCaja & "' AND " _
                                                                     & " a.id_emp = '" & jytsistema.WorkID & "' group by a.codcaj"))
        Return nn


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
            AsciiToCode128B = 0
        ElseIf Asc(Caracter) >= 33 And Asc(Caracter) <= 126 Then
            AsciiToCode128B = Asc(Caracter) - 32
        ElseIf Asc(Caracter) > 126 And Asc(Caracter) <> 128 Then
            AsciiToCode128B = Asc(Caracter) - 50
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
        Dim nTablaPerfil As String = "tblPerfil" & NumeroAleatorio(100000)
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
        Return EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select usuario from jsconctausu where id_user = '" & jytsistema.sUsuario & "' ").ToString
    End Function

    Function CausaMueveInventarioNotasCredito(MyConn As MySqlConnection, lblInfo As Label, CodigoCausa As String) As Boolean
        Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select inventario from jsvencaudcr where codigo = '" _
                                           & CodigoCausa & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' "))
    End Function
    Function CausaMueveInventarioNotasDebito(MyConn As MySqlConnection, lblInfo As Label, CodigoCausa As String) As Boolean
        Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select inventario from jsvencaudcr where codigo = '" _
                                           & CodigoCausa & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' "))
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

        Return sCIF.Replace("_", "").Replace(" ", "").Split("-")(0) + "-" + _
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
    Function NumeroDeDiasEnPeriodo(FechaInincial As Date, FechaFinal As Date, Dia As DayOfWeek) As Integer

        Dim nDias As Integer = 0
        Dim dFecha As Date = FechaInincial
        While dFecha <= FechaFinal
            If dFecha.DayOfWeek = Dia Then nDias += 1
            dFecha = dFecha.AddDays(1)
        End While

        Return nDias

    End Function

    Function NumeroDeDiasEnPeriodoNoLaborables(Myconn As MySqlConnection, lblInfo As Label, ds As DataSet, _
                                               FechaInincial As Date, FechaFinal As Date, Dia As DayOfWeek) As Integer

        Dim nDias As Integer = 0
        Dim ntablita As String = "tbl" & NumeroAleatorio(100000)
        Dim dtDias As DataTable

        ds = DataSetRequery(ds, " SELECT * FROM jsconcatper " _
                             & " WHERE " _
                             & " CONCAT(ANO,'-', LPAD(MES,2,'0'), '-', LPAD(DIA,2,'0')) >= '" & FormatoFechaMySQL(FechaInincial) & "' AND " _
                             & " CONCAT(ANO,'-', LPAD(MES,2,'0'), '-', LPAD(DIA,2,'0')) <= '" & FormatoFechaMySQL(FechaFinal) & "' AND " _
                             & " id_emp = '" & jytsistema.WorkID & "'  ", Myconn, ntablita, lblInfo)

        dtDias = ds.Tables(ntablita)

        For Each nRow As DataRow In dtDias.Rows
            With nRow
                If CDate(.Item("ano") & "-" & .Item("mes") & "-" & .Item("dia")).DayOfWeek = Dia Then nDias += 1
            End With
        Next

        dtDias.Dispose()
        dtDias = Nothing

        Return nDias

    End Function

    Public Sub refrescaBarraprogresoEtiqueta(Optional pb As ProgressBar = Nothing, Optional lbl As Label = Nothing)
        If pb IsNot Nothing Then pb.Refresh()
        If lbl IsNot Nothing Then lbl.Refresh()
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

End Module
