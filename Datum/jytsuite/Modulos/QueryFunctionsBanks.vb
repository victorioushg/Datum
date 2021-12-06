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
            & " c.formpag FormaDePago, c.numpag NumeroDePago, c.refpag ReferenciaDePago, c.Importe, m.CodigoIso, " _
            & " IFNULL ( c.Importe/m.Equivale , c.importe ) ImporteReal, " _
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

    Public Function GetBankLines(MyConn As MySqlConnection, bankCode As String) As List(Of BankLine)

        Dim strSQL = "select a.Id, a.fechamov FechaMovimiento, a.tipomov TipoMovimiento, a.numdoc NumeroMovimiento, a.concepto, " _
                            & " a.importe, a.origen, a.benefic Beneficiario, a.prov_cli ProveedorCliente, '' Nombre,  a.codven CodigoVendedor, " _
                            & " a.numorg NumeroOrigen, a.tiporg TipoOrigen, a.codban CodigoBanco, a.caja CodigoCaja, " _
                            & " a.comproba Comprobante, a.multican Multicancelacion, a.conciliado, a.fecconcilia FechaConciliacion, " _
                            & " a.mesconcilia MesConciliacion, a.Currency, a.currency_date CurrencyDate, a.Block_Date FechaBloqueo, " _
                            & " m.Codigoiso, ifnull( a.Importe/m.Equivale , a.importe) ImporteReal, a.Asiento, a.Fechasi FechaAsiento " _
                            & " from jsbantraban a " _
                            & " left join (" & SQLSelectCambiosYMonedas(jytsistema.sFechadeTrabajo) & " ) m on ( a.currency = m.moneda ) " _
                            & " where " _
                            & " a.codban  = '" & bankCode & "' and " _
                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " order by a.fechamov desc, a.tipomov, a.numdoc "
        Return Lista(Of BankLine)(MyConn, strSQL)
    End Function

    Public Function GetBanksAccounts(MyConn As MySqlConnection) As List(Of BankAccount)
        Dim strSQL = " select codban CodigoBanco, nomban NombreCuenta, ctaban CodigoCuenta from jsbancatban where " _
                            & " estatus = 1 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codban "

        Return Lista(Of BankAccount)(MyConn, strSQL)
    End Function

End Module
