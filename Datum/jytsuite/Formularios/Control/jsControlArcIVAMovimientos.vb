Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsControlArcIVAMovimientos

    Private Const sModulo As String = "Movimiento impuesto al valor agregado"
    Private Const nTabla As String = "tblmovIVA"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private n_TIPO As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property TIPO() As String
        Get
            Return n_TIPO
        End Get
        Set(ByVal value As String)
            n_TIPO = value
        End Set
    End Property

    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtTasa.Text = "A"
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtDesde, txtHasta, txtMonto, txtDesde_1, txtDesde_2,
                               txtHasta_1, txtHasta_2, txtMonto_1, txtMonto_2)
        txtFecha.Value = jytsistema.sFechadeTrabajo

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        With dtLocal.Rows(nPosicion)

            txtTasa.Text = ft.muestraCampoTexto(.Item("tipo"))
            txtFecha.Value = .Item("fecha")

            txtDesde.Text = ft.muestraCampoNumero(.Item("desde"))
            txtHasta.Text = ft.muestraCampoNumero(.Item("hasta"))
            txtMonto.Text = ft.muestraCampoNumero(.Item("monto"))

            txtDesde_1.Text = ft.muestraCampoNumero(.Item("desde_1"))
            txtHasta_1.Text = ft.muestraCampoNumero(.Item("hasta_1"))
            txtMonto_1.Text = ft.muestraCampoNumero(.Item("monto_1"))

            txtDesde_2.Text = ft.muestraCampoNumero(.Item("desde_2"))
            txtHasta_2.Text = ft.muestraCampoNumero(.Item("hasta_2"))
            txtMonto_2.Text = ft.muestraCampoNumero(.Item("monto_2"))
        End With
    End Sub
    Private Sub jsControlArcIVAMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsControlArcIVAMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtTasa.Text)
    End Sub

    Private Function Validado() As Boolean


        If InStr("ABCDEFGHIJKLMNÑOPQRSTUVWXYZ", txtTasa.Text) = 0 Then
            ft.mensajeAdvertencia("Debe indicar una tasa de impuesto válida...")
            ft.enfocarTexto(txtTasa)
            Return False
        End If

        If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsconctaiva where " _
                                   & " tipo = '" & txtTasa.Text & "' and " _
                                   & " fecha = '" & ft.FormatoFechaMySQL(CDate(txtFecha.Text)) & "' ") > 0 And
                               i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Ya existe esta tasa de iva para esta fecha...")
            ft.enfocarTexto(txtTasa)
            Return False
        End If

        If ValorPorcentajeLargo(txtMonto.Text) < 0.0 Or ValorPorcentajeLargo(txtMonto.Text) > 100 Then
            ft.mensajeAdvertencia("Debe indicar un porcentaje válido ...")
            ft.enfocarTexto(txtMonto)
            Return False
        End If

        If ValorPorcentajeLargo(txtMonto_1.Text) < 0.0 Or ValorPorcentajeLargo(txtMonto_1.Text) > 100 Then
            ft.mensajeAdvertencia("Debe indicar un porcentaje válido ...")
            ft.enfocarTexto(txtMonto_1)
            Return False
        End If

        If ValorPorcentajeLargo(txtMonto_2.Text) < 0.0 Or ValorPorcentajeLargo(txtMonto_2.Text) > 100 Then
            ft.mensajeAdvertencia("Debe indicar un porcentaje válido ...")
            ft.enfocarTexto(txtMonto_2)
            Return False
        End If

        If ValorPorcentajeLargo(txtDesde.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar un importe válido ...")
            ft.enfocarTexto(txtDesde)
            Return False
        End If

        If ValorPorcentajeLargo(txtDesde_1.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar un importe válido ...")
            ft.enfocarTexto(txtDesde_1)
            Return False
        End If

        If ValorPorcentajeLargo(txtDesde_2.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar un importe válido ...")
            ft.enfocarTexto(txtDesde_2)
            Return False
        End If

        If ValorPorcentajeLargo(txtHasta.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar un importe válido ...")
            ft.enfocarTexto(txtHasta)
            Return False
        End If

        If ValorPorcentajeLargo(txtHasta_1.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar un importe válido ...")
            ft.enfocarTexto(txtHasta_1)
            Return False
        End If

        If ValorPorcentajeLargo(txtHasta_2.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar un importe válido ...")
            ft.enfocarTexto(txtHasta_2)
            Return False
        End If

        Return True


    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False

            If i_modo = movimiento.iAgregar Then
                Insertar = True

            End If
            InsertEditCONTROLIVA(MyConn, lblInfo, Insertar, txtTasa.Text, CDate(txtFecha.Text),
                                 ft.valorNumero(txtDesde.Text), ft.valorNumero(txtHasta.Text), ValorPorcentajeLargo(txtMonto.Text),
                                 ft.valorNumero(txtDesde_1.Text), ft.valorNumero(txtHasta_1.Text), ValorPorcentajeLargo(txtMonto_1.Text),
                                 ft.valorNumero(txtDesde_2.Text), ft.valorNumero(txtHasta_2.Text), ValorPorcentajeLargo(txtMonto_2.Text))

            TIPO = txtTasa.Text
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtTasa.Text)

            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Sub txtPorcentaje_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMonto.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtDesde_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDesde.KeyPress, txtHasta.KeyPress,
        txtMonto.KeyPress, txtDesde_1.KeyPress, txtHasta_1.KeyPress, txtDesde_2.KeyPress, txtDesde_2.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class