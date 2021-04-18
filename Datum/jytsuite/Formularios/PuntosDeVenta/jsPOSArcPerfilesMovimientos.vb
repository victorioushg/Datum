Imports MySql.Data.MySqlClient
Public Class jsPOSArcPerfilesMovimientos

    Private Const sModulo As String = "Movimiento perfil de facturación de caja punto de venta "
    Private Const nTabla As String = "Perfiles"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private aDescuento() As String = {"NO", "POR DEFECTO", "ASESOR"}
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
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

        txtCodigo.Text = ft.autoCodigo(MyConn, "codper", "jsvenperven", "id_emp", jytsistema.WorkID, 5)
        txtNombre.Text = ""
        chkCR.Checked = False
        chkCO.Checked = True
        chkA.Checked = False : chkB.Checked = True : chkC.Checked = True
        chkD.Checked = False : chkE.Checked = False : chkF.Checked = False
        txtAlmacen.Text = ""
        ft.RellenaCombo(aDescuento, cmbTipo)

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
            txtCodigo.Text = .Item("codper")
            txtNombre.Text = .Item("descrip")
            chkCR.Checked = CBool(.Item("CR"))
            chkCO.Checked = CBool(.Item("CO"))
            chkA.Checked = CBool(.Item("TARIFA_A"))
            chkB.Checked = CBool(.Item("TARIFA_B"))
            chkC.Checked = CBool(.Item("TARIFA_C"))
            chkD.Checked = CBool(.Item("TARIFA_D"))
            chkE.Checked = CBool(.Item("TARIFA_E"))
            chkF.Checked = CBool(.Item("TARIFA_F"))
            txtAlmacen.Text = (.Item("ALMACEN"))
            ft.RellenaCombo(aDescuento, cmbTipo, .Item("DESCUENTO"))
        End With
    End Sub
    Private Sub jsControlTarjetasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub


    Private Function Validado() As Boolean
        Validado = False

        If txtNombre.Text.Trim() = "" Then
            ft.mensajeAdvertencia("Debe indicar un NOMBRE VALIDO PARA ESTE PERFIL...")
            ft.enfocarTexto(txtNombre)
            Exit Function
        End If

        If Not chkCO.Checked And Not chkCR.Checked Then
            ft.mensajeAdvertencia("Debe indicar unA CONDICIÓN DE PAGO para este perfil ...")
            chkCO.Focus()
            Exit Function
        End If


        If Not chkA.Checked And Not chkB.Checked And Not chkC.Checked And _
                Not chkD.Checked And Not chkE.Checked And Not chkF.Checked Then
            ft.mensajeAdvertencia("Debe indicar UNA TARIFA válida para este perfil ...")
            chkB.Focus()
            Exit Function
        End If

        If lblNombreAlmacen.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un ALMACÉN DE SALIDA VALIDO ...")
            btnAlmacen.Focus()
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If

            InsertarModificarPOSPERFIL(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, IIf(chkCR.Checked, 1, 0), _
                                       IIf(chkCO.Checked, 1, 0), IIf(chkA.Checked, 1, 0), IIf(chkB.Checked, 1, 0), IIf(chkC.Checked, 1, 0), IIf(chkD.Checked, 1, 0), _
                                       IIf(chkE.Checked, 1, 0), IIf(chkF.Checked, 1, 0), txtAlmacen.Text, cmbTipo.SelectedIndex)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        Dim NOMBREALMACEN As String = ft.DevuelveScalarCadena(MyConn, " select DESALM from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        lblNombreAlmacen.Text = IIf(NOMBREALMACEN = "0", "", NOMBREALMACEN)
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, " select codalm codigo, desalm descripcion from jsmercatalm where codalm <> '00002' and id_emp = '" & jytsistema.WorkID & "' order by codalm ", "ALMACENES", txtAlmacen.Text)
    End Sub

End Class