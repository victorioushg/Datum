Imports MySql.Data.MySqlClient


Module FuncionesImpresionFiscal
    Public ft As New Transportables
    Public Sub ImprimirDevolucionFiscal(MyConn As MySqlConnection, numDevolucion As String)

        Select Case TipoImpresoraFiscal(MyConn, jytsistema.WorkBox)
            Case 0 ''Devolución Normal (FORMA LIBRE)
                ft.mensajeInformativo("Imprime factura...")
            Case 1 ''DEvolucion Pre-impresa fiscal (FORMA FISCAL PRE-IMPRESA)"
                ft.mensajeInformativo("Imprimienso Factura fiscal")
            Case 2 ''Aclas PPF1F3 (PPF13)
                'ImprimirNotaCreditoFiscal_PP1F3_Aclas(MyConn, numDevolucion)
            Case 3 ''Bematech MP-2100 (BEMAFI32.DLL)
            Case 4 ''Epson/PnP PF-220
            Case 5 ''Tally Dascon 1125
            Case 6 ''Bixolon SRP-350
            Case 7 ''Bixolon SRP-812
        End Select


    End Sub


    Public Function TipoImpresoraFiscal(ByVal MyConn As MySqlConnection, ByVal CodigoCaja As String) As Integer

        Return ft.DevuelveScalarEntero(MyConn, " SELECT b.tipoimpresora " _
                                & " FROM jsvencatcaj a " _
                                & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                & " WHERE " _
                                & " a.codcaj = '" & CodigoCaja & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "'")

    End Function

End Module
