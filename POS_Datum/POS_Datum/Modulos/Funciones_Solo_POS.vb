Imports MySql.Data.MySqlClient
Module Funciones_Solo_POS
    Public Function supervisorValido(MyConn As MySqlConnection) As Boolean

        Dim dsSup As New DataSet
        Dim f = New dtSupervisor

        supervisorValido = False

        f.Cargar(MyConn, dsSup)
        If f.DialogResult = Windows.Forms.DialogResult.OK Then supervisorValido = True

        f.Dispose()
        f = Nothing
        dsSup.Dispose()
        dsSup = Nothing

    End Function

End Module
