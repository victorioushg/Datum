Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports fTransport
Public Class jsBanArcRemesasCestaTicket
    Private Const sModulo As String = "Remesas cheques de alimentación"
    Private Const nTablaR As String = "Remesa"

    Private strSQLRemesa As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dtRemesa As New DataTable
    Private ft As New Transportables

    Private nPosicion As Long
    Private iSel As Integer = 0, tSel As Integer = 0
    Private dSel As Double = 0.0

    Private CodigoCaja As String
    Private CodigoCorredor As String

    Private n_Apuntador As Long
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Remesas(ByVal MyCon As MySqlConnection, ByVal dsCaj As DataSet, ByVal CodCaja As String, ByVal Corredor As String)
        myConn = MyCon
        ds = dsCaj
        CodigoCorredor = Mid(Corredor, 1, 5)
        CodigoCaja = CodCaja

        Dim dates As SfDateTimeEdit() = {txtEmision}
        SetSizeDateObjects(dates)

        lblTituloCaja.Text = "Remesas de cheques de alimentación corredor "
        lblCaja.Text = Corredor
        ft.mensajeEtiqueta(lblInfo, " Indique el número de remesa y escoja el o los documentos  para la construcción de la misma ...", Transportables.TipoMensaje.iAyuda)
        ft.habilitarObjetos(False, True, txtDocSel, txtTickets, txtSaldoSel)


        txtEmision.Value = jytsistema.sFechadeTrabajo
        txtRemesa.Text = ""

        Me.ShowDialog()

    End Sub
    Private Sub btnDocs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDocs.Click
        If txtRemesa.Text <> "" Then
            AbrirRemesa(myConn, txtRemesa.Text)
        Else
            ft.MensajeCritico("Debe indicar un número de remesa válido...")
        End If
    End Sub
    Private Sub AbrirRemesa(ByVal MyConn As MySqlConnection, ByVal NumeroRemesa As String)

        Dim aCamSob() As String = {"corredor", "numsobre", "id_emp"}
        Dim aStrSob() As String = {CodigoCorredor, NumeroRemesa, jytsistema.WorkID}

        strSQLRemesa = " select a.fecha, a.NUMMOV, a.TIPOMOV, a.FORMPAG, a.NUMPAG, a.REFPAG, a.IMPORTE, a.CONCEPTO, " _
                & " a.ORIGEN, b.CORREDOR, b.NUMSOBRE, a.CANTIDAD, b.FECHASOBRE, b.NUMDEP, " _
                & " a.EJERCICIO, a.ID_EMP " _
                & " FROM jsbantracaj a " _
                & " left join jsventabtic b on ( a.NUMMOV = b.NUMCAN  AND a.ID_EMP = b.ID_EMP and a.REFPAG = b.CORREDOR ) " _
                & " where " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' AND " _
                & " b.CONDICION = 0 AND " _
                & " a.TIPOMOV = 'EN' and " _
                & " a.DEPOSITO = '' AND " _
                & " a.FORMPAG = 'CT' AND " _
                & " a.CAJA = '" & CodigoCaja & "' and " _
                & " b.CORREDOR = '" & CodigoCorredor & "' AND " _
                & " b.NUMSOBRE in ('" & NumeroRemesa & "', '') AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                & " GROUP BY a.NUMMOV " _
                & " ORDER by a.fecha "

        ds = DataSetRequery(ds, strSQLRemesa, MyConn, nTablaR, lblInfo)
        dtRemesa = ds.Tables(nTablaR)
        CargaListViewRemesaCT(lv, dtRemesa)

        txtDocSel.Text = ft.FormatoEntero(0)
        txtTickets.Text = ft.FormatoEntero(0)
        txtSaldoSel.Text = ft.FormatoNumero(0.0)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub lv_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lv.ItemChecked

        If e.Item.SubItems.Count > 1 Then
            If e.Item.Checked Then
                iSel += 1
                dSel += CDbl(e.Item.SubItems(6).Text)
                tSel += CInt(e.Item.SubItems(4).Text)
            Else
                iSel -= 1
                dSel -= CDbl(e.Item.SubItems(6).Text)
                tSel -= CInt(e.Item.SubItems(4).Text)
            End If
        End If

        If iSel < 0 Then
            iSel = 0
            tSel = 0
            dSel = 0.0
        End If

        txtDocSel.Text = ft.FormatoEntero(iSel)
        txtTickets.Text = ft.FormatoEntero(tSel)
        txtSaldoSel.Text = ft.FormatoNumero(dSel)

    End Sub

    Private Sub jsBanRemesasCestaTicket_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        InsertarAuditoria(myConn, MovAud.iSalir, sModulo, CodigoCaja & CodigoCorredor)
        dtRemesa.Dispose()
    End Sub

    Private Sub jsBanRemesasCestaTicket_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, CodigoCaja & CodigoCorredor)
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarRemesa()
            Me.Close()
        End If
    End Sub
    Private Sub GuardarRemesa()
        InsertarAuditoria(myConn, MovAud.iIncluir, sModulo, txtRemesa.Text)

        ft.Ejecutar_strSQL(myConn, " UPDATE jsventabtic SET NUMSOBRE = '' " _
           & ", FECHASOBRE = '0000-00-00' " _
           & " where " _
           & " NUMSOBRE = '" & txtRemesa.Text & "' AND " _
           & " ID_EMP ='" & jytsistema.WorkID & "' ")

        ft.Ejecutar_strSQL(myConn, "UPDATE jsventracob set REMESA = '' WHERE " _
            & " REMESA = '" & txtRemesa.Text & "' and " _
            & " ID_EMP ='" & jytsistema.WorkID & "' ")

        Dim iCont As Integer
        For iCont = 0 To lv.Items.Count - 1
            If lv.Items(iCont).Checked Then

                ft.Ejecutar_strSQL(myConn, " UPDATE jsventabtic SET NUMSOBRE = '" & txtRemesa.Text & "', " _
                                    & " FECHASOBRE = '" & ft.FormatoFechaMySQL(txtEmision.Value) & "' " _
                                    & " where " _
                                    & " CORREDOR = '" & CodigoCorredor & "' AND " _
                                    & " NUMCAN = '" & lv.Items(iCont).SubItems(2).Text & "' " _
                                    & " and ID_EMP ='" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(myConn, "UPDATE jsventracob SET REMESA = '" & txtRemesa.Text & "' " _
                                    & " where " _
                                    & " COMPROBA = '" & lv.Items(iCont).SubItems(2).Text & "' " _
                                    & " and ID_EMP ='" & jytsistema.WorkID & "' ")

            Else

                ft.Ejecutar_strSQL(myConn, "UPDATE jsventabtic SET NUMSOBRE = '', FECHASOBRE = '0000-00-00' " _
                    & " where " _
                    & " CORREDOR = '" & CodigoCorredor & "' AND " _
                    & " NUMCAN = '" & lv.Items(iCont).SubItems(2).Text & "' " _
                    & " and ID_EMP ='" & jytsistema.WorkID & "' ")

            End If
        Next


    End Sub
    Private Function Validado() As Boolean
        Validado = False
        If ValorEntero(txtDocSel.Text) = 0 Then
            ft.mensajeAdvertencia("Debe seleccionar al menos un documento ... ")
            Exit Function
        End If

        If txtRemesa.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un numero de depósito válido ...")
            ft.enfocarTexto(txtRemesa)
            Exit Function
        End If

        Validado = True

    End Function

    Private Sub txtRemesa_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRemesa.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de remesa ... ", Transportables.tipoMensaje.iInfo)
        txtRemesa.MaxLength = 15
    End Sub

    Private Sub btnEmision_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        ft.mensajeEtiqueta(lblInfo, "seleccione la fecha de emisión de este depósito...", Transportables.tipoMensaje.iInfo)
    End Sub
End Class