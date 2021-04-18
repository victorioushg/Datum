Imports MySql.Data.MySqlClient
Module TablaBaseDatos
    Dim aCampos() As String
    Private ft As New Transportables
    Public Sub VerificaBaseDatos(ByVal MyConn As MySqlConnection, ByVal NombreBaseDatos As String, ByVal lblInfo As Label)
        If ExisteBD(MyConn, NombreBaseDatos, lblInfo) Then

            '/////////////MERCANCIAS
            ModificaCampoTabla(MyConn, lblInfo, NombreBaseDatos, "jsmercatdiv", "color", "color.cadena.30.0")
            ModificaCampoTabla(MyConn, lblInfo, NombreBaseDatos, "jsmerequmer", "equivale", "equivale.doble.10.5")
            AgregarCampoTabla(MyConn, lblInfo, NombreBaseDatos, "jsmerenctra", "tipo.entero2", "pesototal")

            ModificaCampoTabla(MyConn, lblInfo, NombreBaseDatos, "jsmerrentra", "renglon", "renglon.cadena.5.0")

            Dim aFldExp() As String = {"codart.cadena.15.0", "fecha.fechahora", "comentario.memo.0.0", "condicion.cadena.1.0", "causa.cadena.5.0", "tipocondicion.cadena.1.0", "id_emp.cadena.2.0"}
            CrearTabla(MyConn, lblInfo, NombreBaseDatos, False, "jsmerexpmer", aFldExp)

        End If
    End Sub
    Public Function ExisteBD(ByVal MyConn As MySqlConnection, ByVal NombreBD As String, ByVal lblInfo As Label) As Boolean

        Dim strSQL As String = "SHOW DATABASES LIKE '" & NombreBD & "'"
        Dim nTabla As String = "basedatos"
        Dim ds As New DataSet

        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblinfo)
        If ds.Tables(nTabla).Rows.Count > 0 Then ExisteBD = True
        ds = Nothing

    End Function
    Public Function ExisteTabla(ByVal MyConn As MySqlConnection, ByVal NombreBD As String, ByVal NombreTabla As String) As Boolean
        Dim strSQL As String = "SHOW TABLES FROM " & NombreBD & " LIKE '" & NombreTabla & "'"
        Dim nTabla As String = "tabla"
        Dim ds As New DataSet

        ExisteTabla = False
        ds = ft.DataSetRequeryPlus(ds, nTabla, MyConn, strSQL)
        If ds.Tables(nTabla).Rows.Count > 0 Then ExisteTabla = True
        ds.Dispose()
        ds = Nothing

    End Function

    Public Function ExisteIndiceTabla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBD As String, ByVal NombreTabla As String, ByVal nombreIndice As String) As Boolean
        Dim strSQL As String = "SHOW INDEX FROM " & NombreTabla & " FROM " & NombreBD & " WHERE Key_name = '" & nombreIndice & "' "
        Dim ntabla As String = "tabla"
        Dim ds As New DataSet

        ds = DataSetRequery(ds, strSQL, MyConn, ntabla, lblinfo)
        If ds.Tables(ntabla).Rows.Count > 0 Then ExisteIndiceTabla = True

        ds.Dispose()
        ds = Nothing

    End Function
    Public Function ExisteCampo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBD As String, ByVal NombreTabla As String, ByVal nombreCampo As String) As Boolean

        Dim strSQL As String = "SHOW COLUMNS FROM " & NombreTabla & " FROM " & NombreBD & " LIKE '" & nombreCampo & "'"
        Dim nTabla As String = "tabla"
        Dim ds As New DataSet

        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblinfo)
        If ds.Tables(nTabla).Rows.Count > 0 Then ExisteCampo = True

        ds = Nothing

    End Function
    Public Function ExisteFunction(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBD As String, ByVal NombreFuncion As String) As Boolean
        Dim strSQL As String = "SHOW FUNCTION STATUS WHERE NAME = '" & NombreFuncion & "' AND DB = '" & NombreBD & "' "
        Dim ntabla As String = "tabla"
        Dim ds As New DataSet

        ds = DataSetRequery(ds, strSQL, MyConn, ntabla, lblInfo)
        If ds.Tables(ntabla).Rows.Count > 0 Then ExisteFunction = True

        ds.Dispose()
        ds = Nothing

    End Function

    Public Sub AgregarCampoTabla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBD As String, ByVal NombreTabla As String, ByVal sCampo As String, Optional ByVal CampoAfter As String = "")
        Dim afield() As String
        afield = Split(sCampo, ".")
        If ExisteTabla(MyConn, NombreBD, NombreTabla) Then
            Dim xx As String = IIf(UBound(afield) < 2, " COMMENT '' ", " COMMENT '" & afield(UBound(afield)) & "'")
            If Not ExisteCampo(MyConn, lblInfo, NombreBD, NombreTabla, CStr(afield(0))) Then _
                ft.Ejecutar_strSQL(MyConn, " alter table " & NombreTabla & " add column " & CStr(afield(0)) _
                    & TipoCampo(afield(1).ToString, CInt(afield(2)), CInt(afield(3))) & xx & IIf(CampoAfter <> "", " after " & CampoAfter, ""))
        End If
    End Sub
    Public Sub ModificaCampoTabla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBD As String, ByVal NombreTabla As String, ByVal CampoAnterior As String, ByVal CampoActual As String)
        Dim afield() As String
        afield = Split(CampoActual, ".")
        If ExisteTabla(MyConn, NombreBD, NombreTabla) Then
            If ExisteCampo(MyConn, lblInfo, NombreBD, NombreTabla, CampoAnterior) Then
                Dim modificO As Boolean = False
                While Not modificO


                    Dim xx As String = IIf(afield(UBound(afield)).ToString.Length <= 2, " COMMENT '' ", " COMMENT '" & afield(UBound(afield)).ToString & "' ")
                   
                    modificO = ft.Ejecutar_strSQL(MyConn, " alter table " & NombreTabla & " change " & CampoAnterior & _
                        " " & CStr(afield(0)) & " " & TipoCampo(afield(1).ToString, CInt(afield(2)), CInt(afield(3))) & xx)

                    If modificO Then
                        If CStr(afield(1)).Substring(0, 4) = "fech" Then ft.Ejecutar_strSQL(MyConn, " update " & NombreTabla & " set " & CStr(afield(0)) & " = '2009-01-01' where " & CStr(afield(0)) & " = '0000-00-00' ")
                    Else
                        Select Case CStr(afield(1)).Substring(0, 5)
                            Case "fecha"
                                ft.Ejecutar_strSQL(MyConn, " update " & NombreTabla & " set " & CStr(afield(0)) & " = '2009-01-01' where " & CStr(afield(0)) & " = '0000-00-00' ")
                            Case "caden"
                                ft.Ejecutar_strSQL(MyConn, " update " & NombreTabla & " set " & CStr(afield(0)) & " = '' where " & CStr(afield(0)) & " is null ")
                        End Select
                    End If

                End While

            End If
        End If

    End Sub
    Public Sub EliminaCampoTabla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBD As String, ByVal NombreTabla As String, ByVal CampoAnterior As String)
        If ExisteTabla(MyConn, NombreBD, NombreTabla) Then
            If ExisteCampo(MyConn, lblInfo, NombreBD, NombreTabla, CampoAnterior) Then _
                ft.Ejecutar_strSQL(MyConn, " alter table " & NombreTabla & " DROP COLUMN " & CampoAnterior)
        End If
    End Sub
    Public Sub EliminarTablaTemporal(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreTabla As String)
        ft.Ejecutar_strSQL(myconn, " DROP TEMPORARY TABLE IF EXISTS " & NombreTabla & "  ")
    End Sub
    Public Sub CrearTabla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBD As String, ByVal Temporal As Boolean, ByVal NombreTabla As String, ByVal aCampos As Object, Optional ByVal Indice As String = "")
        If Temporal Then EliminarTablaTemporal(MyConn, lblInfo, NombreTabla)
        If Not ExisteTabla(MyConn, NombreBD, NombreTabla) Then
            Dim lCont As Integer
            Dim Str As String
            If Temporal Then

                Str = " create temporary table " & NombreTabla & " ( "
            Else
                Str = " create table " & NombreTabla & " ( "
            End If

            For lCont = 0 To UBound(aCampos)
                Dim afield() As String
                afield = Split(aCampos(lCont), ".")
                If afield(0) <> "" Then _
                        Str = Str & " " & afield(0) & TipoCampo(afield(1).ToString, CInt(afield(2)), CInt(afield(3))) & ", "
            Next

            Str = Mid(Str, 1, Len(Str) - 2)

            If Indice <> "" Then _
                Str = Str & ", PRIMARY KEY (" & Indice & ")"

            Str = Str & ")"

            ft.Ejecutar_strSQL(MyConn, Str & " ENGINE = MYISAM ")
        End If

    End Sub
    Public Sub CrearIndice(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NombreBaseDAtos As String, ByVal NombreTAblaMySQL As String, _
                                ByVal NombreIndice As String, ByVal CamposIndice As String, Optional ByVal TipoIndice As Integer = 0)
        If ExisteIndiceTabla(MyConn, lblInfo, NombreBaseDAtos, NombreTAblaMySQL, NombreIndice) Then _
            ft.Ejecutar_strSQL(myconn, " ALTER TABLE `" & NombreTAblaMySQL & "` DROP INDEX `" & NombreIndice & "` ")
        If TipoIndice = 0 Then
            ft.Ejecutar_strSQL(myconn, " ALTER TABLE `" & NombreTAblaMySQL & "` ADD PRIMARY KEY (" & CamposIndice & ") ")
        Else
            ft.Ejecutar_strSQL(myconn, " CREATE INDEX " & NombreIndice & " ON " & NombreTAblaMySQL & " (" & Replace(CamposIndice, "`", "") & ") ")
        End If


    End Sub

    Function TipoCampo(ByVal strCampo As String, longitud As Integer, Decimales As Integer) As String
        Select Case CStr(strCampo)
            Case "blob"
                TipoCampo = " blob "
            Case "longblob"
                TipoCampo = " longblob "
            Case "memo"
                TipoCampo = " longtext not null "
            Case "cadena"
                Select Case longitud
                    Case 0
                        TipoCampo = " char(1) default '' not null "
                    Case 1, 2, 3
                        TipoCampo = " char(" & longitud.ToString & ") default '' not null "
                    Case Else
                        TipoCampo = " varchar(" & longitud.ToString & ") default '' not null "
                End Select
            Case "doble"
                TipoCampo = " double(" & longitud.ToString & "," & Decimales.ToString & ") default '0' not null "
            Case "entero"
                If longitud <= 1 Then
                    TipoCampo = " tinyint(" & longitud.ToString & ") default '" & Decimales.ToString & "' not null "
                Else
                    TipoCampo = " int(" & longitud.ToString & ") default '0' not null "
                End If
            Case "enterolargo10A"
                TipoCampo = " int(10) not null auto_increment"
            Case "fecha"
                TipoCampo = " date default '2009-01-01' not null "
            Case "tiempo"
                TipoCampo = " time default '12:12:12' not null "
            Case "fechahora"
                TipoCampo = " datetime default '2009-01-01 12:12:12' not null "
            Case Else
                ft.mensajeInformativo(" CAMPO " & strCampo & " NO VALIDO ...")
                TipoCampo = " "
        End Select

    End Function
End Module
