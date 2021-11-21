Imports MySql.Data.MySqlClient
Imports fTransport
Module QueryFunctionsBanks

    Public ft As New Transportables

    Public Function GetCashBoxes(MyConn As MySqlConnection) As List(Of Saving)

        Dim strSQL = " select caja Codigo, nomcaja Descripcion, codcon CodigoContable " _
            & " from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' " _
            & " order by caja "
        Return Lista(Of Saving)(MyConn, strSQL)

    End Function

    Public Function GetSavingLines(MyConn As MySqlConnection, savingCode As String) As List(Of SavingLines)

        Dim strSQL = "select c.caja Codigo, c.Id, c.Fecha, c.Origen, c.tipomov TipoMovimiento, c.nummov NumeroMovimiento, " _
            & " c.formpag FormaDePago, c.numpag NumeroDePago, c.refpag ReferenciaDePago, c.Importe, m.CodigoIso, c.Importe/m.Equivale  ImporteReal, " _
            & " c.currency, c.currency_date, c.codcon, c.concepto, c.deposito, c.fecha_dep FechaDeposito, c.cantidad CantidadDocumentos, " _
            & " c.codban CodigoBancario, c.multican MultiCancelacion, " _
            & " c.Asiento, c.fechasi FechaAsiento, c.prov_cli ProveedorCliente, c.codven CodigoVendedor, c.block_date FechaBloqueo " _
            & " from jsbantracaj c " _
            & " left join (" & SQLSelectCambiosYMonedas(jytsistema.sFechadeTrabajo) & " ) m on ( c.currency = m.moneda ) " _
            & " where " _
            & " c.caja  = '" & savingCode & "' and " _
            & " c.deposito = '' and " _
            & " c.fecha <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " c.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " c.id_emp = '" & jytsistema.WorkID & "' order by c.fecha desc, c.tipomov, c.nummov "
        Return Lista(Of SavingLines)(MyConn, strSQL)

    End Function

End Module
