Imports MySql.Data.MySqlClient

Module QueryFunctionAccounting

    Public Function GetResultAccounts(MyConn As MySqlConnection) As List(Of AccountBase)

        Dim strSQL = " select codcon CodigoContable, Descripcion, nivel, marca " _
            & " from jscotcatcon " _
            & " where " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " and RIGHT(codcon,1)  <> '.' " _
            & " order by codcon "
        Return Lista(Of AccountBase)(MyConn, strSQL)
        '& " and codcon not in ( select codcon from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' )" _
    End Function

End Module
