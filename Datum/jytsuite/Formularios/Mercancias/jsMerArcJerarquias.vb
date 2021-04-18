Imports MySql.Data.MySqlClient
Public Class jsMerArcJerarquias
    Private Const sModulo As String = "Jerarquías o Referencias"
    Private Const lRegion As String = "RibbonButton136"
    Private Const nTabla As String = "tbltipjer"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private dtG1 As New DataTable
    Private dtG2 As New DataTable
    Private dtG3 As New DataTable
    Private dtG4 As New DataTable
    Private dtG5 As New DataTable
    Private dtG6 As New DataTable
    Private ft As New Transportables

    Private strSQL As String = " select * from jsmerencjer where id_emp = '" & jytsistema.WorkID & "' order by tipjer "

    Private Posicion As Long
    Private PosicionG1 As Long
    Private PosicionG2 As Long
    Private PosicionG3 As Long
    Private PosicionG4 As Long
    Private PosicionG5 As Long
    Private PosicionG6 As Long

    Private n_Grupo1 As String
    Private n_Grupo2 As String
    Private n_Grupo3 As String
    Private n_Grupo4 As String
    Private n_Grupo5 As String
    Private n_Grupo6 As String
    Private n_TipoJerarquia As String
    Private i_modo As Integer
    Public Property TipoJerarquia() As String
        Get
            Return n_TipoJerarquia
        End Get
        Set(ByVal value As String)
            n_TipoJerarquia = value
        End Set

    End Property

    Public Property Grupo1() As String
        Get
            Return n_Grupo1
        End Get
        Set(ByVal value As String)
            n_Grupo1 = value
        End Set
    End Property

    Public Property Grupo2() As String
        Get
            Return n_Grupo2
        End Get
        Set(ByVal value As String)
            n_Grupo2 = value
        End Set
    End Property

    Public Property Grupo3() As String
        Get
            Return n_Grupo3
        End Get
        Set(ByVal value As String)
            n_Grupo3 = value
        End Set
    End Property

    Public Property Grupo4() As String
        Get
            Return n_Grupo4
        End Get
        Set(ByVal value As String)
            n_Grupo4 = value
        End Set
    End Property

    Public Property Grupo5() As String
        Get
            Return n_Grupo5
        End Get
        Set(ByVal value As String)
            n_Grupo5 = value
        End Set

    End Property

    Public Property Grupo6() As String
        Get
            Return n_Grupo6
        End Get
        Set(ByVal value As String)
            n_Grupo6 = value
        End Set
    End Property

    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        ' 0 = show() ; 1 = showdialog()

        myConn = Mycon

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        DesactivarMarco0()
        If dt.Rows.Count > 0 Then
            If TipoJerarquia = "" Then
                Posicion = 0
            Else
                Dim row As DataRow = dt.Select(" tipjer = '" & TipoJerarquia & "' ")(0)
                Me.BindingContext(ds, nTabla).Position = dt.Rows.IndexOf(row)
                Posicion = dt.Rows.IndexOf(row)
            End If
            AsignaTXT(Posicion)
        Else
            IniciarTipoJerarquia(False)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
        AsignarTooltips()

        If TipoCarga = 0 Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub jsNomArcGrupos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        dtG1 = Nothing
        dtG2 = Nothing
        dtG3 = Nothing
        dtG4 = Nothing
        dtG5 = Nothing
        dtG6 = Nothing
        ds = Nothing
    End Sub

    Private Sub jsMerArcJerarquias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()

        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregarS1, "<B>Agregar</B> nueva jerarquía")
        C1SuperTooltip1.SetToolTip(btnEditarS1, "<B>Editar o mofificar</B> jerarquía actual")
        C1SuperTooltip1.SetToolTip(btnEliminarS1, "<B>Eliminar</B> jerarquia actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> jerarquía deseada")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> jerarquía")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a jerarquía <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a jerarquía <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última  jerarquía</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> movimientos de jerarquía actual")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True
        ft.habilitarObjetos(True, True, txtTipjerNombre, txtMascara1, _
                txtMascara2, txtMascara3, txtMascara4, txtMascara5, txtMascara6, txtDescrip1, txtDescrip2, _
                txtDescrip3, txtDescrip4, txtDescrip5, txtDescrip6, txtCodigoProveedor, btnProveedor)

        ft.habilitarObjetos(True, False, MenuBarraS1, MenuBarraS2, MenuBarraS3, MenuBarraS4, MenuBarraS5, MenuBarraS6)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()
        ft.habilitarObjetos(False, True, txtTipJer, txtTipjerNombre, txtJerarquia, txtMascara1, _
                txtMascara2, txtMascara3, txtMascara4, txtMascara5, txtMascara6, txtDescrip1, _
                txtDescrip2, txtDescrip3, txtDescrip4, txtDescrip5, txtDescrip6, txtCodigoProveedor, _
                btnProveedor, txtNombreProveedor, txtCuentaContable)
        ft.habilitarObjetos(False, False, MenuBarraS1, MenuBarraS2, MenuBarraS3, MenuBarraS4, MenuBarraS5, MenuBarraS6)
        grpAceptarSalir.Visible = False
        MenuBarra.Enabled = True

    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then
            Grupo1 = ""
            Grupo2 = ""
            Grupo3 = ""
            Grupo4 = ""
            Grupo5 = ""
            Grupo6 = ""
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
                With .Rows(nRow)
                    'Tipos de Jerarquías 
                    txtTipJer.Text = .Item("tipjer")
                    TipoJerarquia = .Item("tipjer")

                    txtTipjerNombre.Text = IIf(IsDBNull(.Item("descrip")), "", .Item("descrip"))

                    txtMascara1.Text = ft.muestraCampoTexto(.Item("mascara1"))
                    txtMascara2.Text = ft.muestraCampoTexto(.Item("mascara2"))
                    txtMascara3.Text = ft.muestraCampoTexto(.Item("mascara3"))
                    txtMascara4.Text = ft.muestraCampoTexto(.Item("mascara4"))
                    txtMascara5.Text = ft.muestraCampoTexto(.Item("mascara5"))
                    txtMascara6.Text = ft.muestraCampoTexto(.Item("mascara6"))

                    txtDescrip1.Text = ft.muestraCampoTexto(.Item("descrip1"))
                    txtDescrip2.Text = ft.muestraCampoTexto(.Item("descrip2"))
                    txtDescrip3.Text = ft.muestraCampoTexto(.Item("descrip3"))
                    txtDescrip4.Text = ft.muestraCampoTexto(.Item("descrip4"))
                    txtDescrip5.Text = ft.muestraCampoTexto(.Item("descrip5"))
                    txtDescrip6.Text = ft.muestraCampoTexto(.Item("descrip6"))

                    txtCodigoProveedor.Text = ft.muestraCampoTexto(.Item("proveedor"))

                    'Movimientos
                    dtG1 = AbrirMovimientosNivel(1, dg1, Grupo1)
                    PosicionG1 = Me.BindingContext(ds, "tablanivel1").Position
                    Grupo1 = ""
                    If PosicionG1 >= 0 Then
                        dg1.CurrentCell = dg1(0, CInt(PosicionG1))
                        Grupo1 = dtG1.Rows(PosicionG1).Item("codjer")
                    End If

                    dtG2 = AbrirMovimientosNivel(2, dg2, Grupo2)
                    PosicionG2 = Me.BindingContext(ds, "tablanivel2").Position
                    Grupo2 = ""
                    If PosicionG2 >= 0 Then
                        dg2.CurrentCell = dg2(0, CInt(PosicionG2))
                        Grupo2 = dtG2.Rows(PosicionG2).Item("codjer")
                    End If



                    dtG3 = AbrirMovimientosNivel(3, dg3, Grupo3)
                    PosicionG3 = Me.BindingContext(ds, "tablanivel3").Position
                    Grupo3 = ""
                    If PosicionG3 >= 0 Then
                        dg3.CurrentCell = dg3(0, CInt(PosicionG3))
                        Grupo3 = dtG3.Rows(PosicionG3).Item("codjer")
                    End If



                    dtG4 = AbrirMovimientosNivel(4, dg4, Grupo4)
                    PosicionG4 = Me.BindingContext(ds, "tablanivel4").Position
                    Grupo4 = ""
                    If PosicionG4 >= 0 Then
                        dg4.CurrentCell = dg4(0, CInt(PosicionG4))
                        Grupo4 = dtG4.Rows(PosicionG4).Item("codjer")
                    End If



                    dtG5 = AbrirMovimientosNivel(5, dg5, Grupo5)
                    PosicionG5 = Me.BindingContext(ds, "tablanivel5").Position
                    Grupo5 = ""
                    If PosicionG5 >= 0 Then
                        dg5.CurrentCell = dg5(0, CInt(PosicionG5))
                        Grupo5 = dtG5.Rows(PosicionG5).Item("codjer")
                    End If


                    dtG6 = AbrirMovimientosNivel(6, dg6, Grupo6)
                    PosicionG6 = Me.BindingContext(ds, "tablanivel6").Position
                    Grupo6 = ""
                    If PosicionG6 >= 0 Then
                        dg6.CurrentCell = dg6(0, CInt(PosicionG6))
                        Grupo6 = dtG6.Rows(PosicionG6).Item("codjer")
                    End If

                End With

                ColocaJerarquia()

            End With
        End If
    End Sub

    Private Sub IniciarTipoJerarquia(ByVal Inicio As Boolean)

        If Inicio Then
            txtTipJer.Text = ft.autoCodigo(myConn, "tipjer", "jsmerencjer", "id_emp", jytsistema.WorkID, 5)
        Else
            txtTipJer.Text = ""
        End If

        txtTipjerNombre.Text = ""
        txtJerarquia.Text = ""
        txtMascara1.Text = ""
        txtMascara2.Text = ""
        txtMascara3.Text = ""
        txtMascara4.Text = ""
        txtMascara5.Text = ""
        txtMascara6.Text = ""
        txtDescrip1.Text = ""
        txtDescrip2.Text = ""
        txtDescrip3.Text = ""
        txtDescrip4.Text = ""
        txtDescrip5.Text = ""
        txtDescrip6.Text = ""
        txtCodigoProveedor.Text = ""

        dg1.DataSource = Nothing
        dg2.DataSource = Nothing
        dg3.DataSource = Nothing
        dg4.DataSource = Nothing
        dg5.DataSource = Nothing
        dg6.DataSource = Nothing

        'Movimientos
        dtG1 = AbrirMovimientosNivel(1, dg1, Grupo1)
        PosicionG1 = Me.BindingContext(ds, "tablanivel1").Position
        If PosicionG1 >= 0 Then
            dg1.CurrentCell = dg1(0, CInt(PosicionG1))
            Grupo1 = dtG1.Rows(PosicionG1).Item("codjer")
        Else
            Grupo1 = ""
        End If

        dtG2 = AbrirMovimientosNivel(2, dg2, Grupo2)
        PosicionG2 = Me.BindingContext(ds, "tablanivel2").Position
        If PosicionG2 >= 0 Then
            dg2.CurrentCell = dg2(0, CInt(PosicionG2))
            Grupo2 = dtG2.Rows(PosicionG2).Item("codjer")
        Else
            Grupo2 = ""
        End If



        dtG3 = AbrirMovimientosNivel(3, dg3, Grupo3)
        PosicionG3 = Me.BindingContext(ds, "tablanivel3").Position
        If PosicionG3 >= 0 Then
            dg3.CurrentCell = dg3(0, CInt(PosicionG3))
            Grupo3 = dtG3.Rows(PosicionG3).Item("codjer")
        Else
            Grupo3 = ""
        End If



        dtG4 = AbrirMovimientosNivel(4, dg4, Grupo4)
        PosicionG4 = Me.BindingContext(ds, "tablanivel4").Position
        If PosicionG4 >= 0 Then
            dg4.CurrentCell = dg4(0, CInt(PosicionG4))
            Grupo4 = dtG4.Rows(PosicionG4).Item("codjer")
        Else
            Grupo4 = ""
        End If



        dtG5 = AbrirMovimientosNivel(5, dg5, Grupo5)
        PosicionG5 = Me.BindingContext(ds, "tablanivel5").Position
        If PosicionG5 >= 0 Then
            dg5.CurrentCell = dg5(0, CInt(PosicionG5))
            Grupo5 = dtG5.Rows(PosicionG5).Item("codjer")
        Else
            Grupo5 = ""
        End If


        dtG6 = AbrirMovimientosNivel(6, dg6, Grupo6)
        PosicionG6 = Me.BindingContext(ds, "tablanivel6").Position
        If PosicionG6 >= 0 Then
            dg6.CurrentCell = dg6(0, CInt(PosicionG6))
            Grupo6 = dtG6.Rows(PosicionG6).Item("codjer")
        Else
            Grupo6 = ""
        End If

        Grupo1 = ""
        Grupo2 = ""
        Grupo3 = ""
        Grupo4 = ""
        Grupo5 = ""
        Grupo6 = ""

        ColocaJerarquia()




    End Sub

    Private Sub ColocaJerarquia()
        If Grupo1 & Grupo2 & Grupo3 & Grupo4 & Grupo5 & Grupo6 <> "" Then
            txtJerarquia.Text = ColocaJerarquiaNivel(myConn, txtTipJer.Text, Grupo1, 1) + IIf(Grupo2 <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, Grupo2, 2) + IIf(Grupo3 <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, Grupo3, 3) + IIf(Grupo4 <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, Grupo4, 4) + IIf(Grupo5 <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, Grupo5, 5) + IIf(Grupo6 <> "", "->", "") _
                        + ColocaJerarquiaNivel(myConn, txtTipJer.Text, Grupo6, 6)
        Else
            txtJerarquia.Text = ""
        End If

    End Sub
    Private Function ColocaJerarquiaNivel(ByVal MyConn As MySqlConnection, ByVal TipoJerarquia As String, ByVal CodigoJerarquia As String, ByVal Nivel As Integer) As String
        Dim aCam() As String = {"tipjer", "codjer", "nivel", "id_emp"}
        Dim aStr() As String = {TipoJerarquia, CodigoJerarquia, Nivel, jytsistema.WorkID}
        ColocaJerarquiaNivel = qFoundAndSign(MyConn, lblInfo, "jsmerrenjer", aCam, aStr, "desjer")
    End Function
    Private Function AbrirMovimientosNivel(ByVal Nivel As Integer, ByVal dg As DataGridView, ByVal CodGrupo As String) As DataTable

        Dim dtt As New DataTable
        Dim nTablaNivel As String = "tablanivel" + CStr(Nivel)
        Dim strSQLNIvel As String = " select * from jsmerrenjer where tipjer = '" & txtTipJer.Text & "' and nivel = '" & Nivel & "' and id_emp = '" & jytsistema.WorkID & "' order by tipjer, codjer "
        ds = DataSetRequery(ds, strSQLNIvel, myConn, nTablaNivel, lblInfo)
        dtt = ds.Tables(nTablaNivel)

        Dim aCampos() As String = {"codjer", "desjer"}
        Dim aNombres() As String = {"", ""}
        Dim aAnchos() As Integer = {170, 170}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", ""}

        IniciarTabla(dg, dtt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False)
        AbrirMovimientosNivel = dtt

        If dtt.Rows.Count > 0 Then
            dg.Refresh()
            If CodGrupo <> "" Then

                Dim row As DataRow = dtt.Select(" tipjer = '" & txtTipJer.Text & "' and codjer = '" & CodGrupo & "' ")(0)
                Me.BindingContext(ds, nTablaNivel).Position = dtt.Rows.IndexOf(row)
                dg.CurrentCell = dg(0, dtt.Rows.IndexOf(row))
            Else
                Me.BindingContext(ds, nTablaNivel).Position = 0
                dg.CurrentCell = dg(0, 0)
            End If
        End If

    End Function


    Private Sub IniciarGrilla(ByVal dt As DataTable, ByVal dg As DataGridView)
        Dim aCampos() As String = {"des_nivel"}
        Dim aNombres() As String = {"Nombre Grupo/SubGrupo "}
        Dim aAnchos() As Integer = {500}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub


    Private Sub dg1_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg1.RowHeaderMouseClick, _
        dg1.CellMouseClick, dg1.RegionChanged, dg2.RowHeaderMouseClick, dg2.CellMouseClick, dg2.RegionChanged, _
        dg3.RowHeaderMouseClick, dg3.CellMouseClick, dg3.RegionChanged, _
        dg4.RowHeaderMouseClick, dg4.CellMouseClick, dg4.RegionChanged, _
        dg5.RowHeaderMouseClick, dg5.CellMouseClick, dg5.RegionChanged, _
        dg6.RowHeaderMouseClick, dg6.CellMouseClick, dg6.RegionChanged

        Dim posicionT As Long

        Me.BindingContext(ds, "tablanivel" & Microsoft.VisualBasic.Right(sender.name, 1)).Position = e.RowIndex
        posicionT = Me.BindingContext(ds, "tablanivel" & Microsoft.VisualBasic.Right(sender.name, 1)).Position
        If posicionT >= 0 Then
            sender.CurrentCell = sender(0, CInt(posicionT))

            Select Case Microsoft.VisualBasic.Right(sender.name, 1)
                Case 1
                    Grupo1 = dtG1.Rows(posicionT).Item("codjer")
                    PosicionG1 = posicionT
                Case 2
                    Grupo2 = dtG2.Rows(posicionT).Item("codjer")
                    PosicionG2 = posicionT
                Case 3
                    Grupo3 = dtG3.Rows(posicionT).Item("codjer")
                    PosicionG3 = posicionT
                Case 4
                    Grupo4 = dtG4.Rows(posicionT).Item("codjer")
                    PosicionG4 = posicionT
                Case 5
                    Grupo5 = dtG5.Rows(posicionT).Item("codjer")
                    PosicionG5 = posicionT
                Case 6
                    Grupo6 = dtG6.Rows(posicionT).Item("codjer")
                    PosicionG6 = posicionT
            End Select

        End If

        ColocaJerarquia()

    End Sub

    Private Sub txtJerarquia_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJerarquia.TextChanged

    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub
    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub
    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub
    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        Posicion = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarTipoJerarquia(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        Posicion = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Posicion = Me.BindingContext(ds, nTabla).Position

        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsmerctainv where tipjer = '" & txtTipJer.Text & "' and id_emp = '" & jytsistema.WorkID & "'  ") > 0 Then
            ft.mensajeCritico("Jerarquía NO puede ser eliminado!!!")
        Else

             If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(Posicion).Item("tipjer"))
                ft.Ejecutar_strSQL(myConn, " delete from jsmerencjer where " _
                    & " tipjer = '" & txtTipJer.Text & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                ft.Ejecutar_strSQL(myConn, " delete from jsmerrenjer where tipjer = '" & txtTipJer.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < Posicion Then Posicion = dt.Rows.Count - 1
                If Posicion >= 0 Then
                    AsignaTXT(Posicion)
                Else
                    IniciarTipoJerarquia(False)
                End If

            End If

        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"tipjer", "descrip"}
        Dim Nombres() As String = {"Código", "Descripción"}
        Dim Anchos() As Integer = {120, 1100}
        f.Text = "Tipos de Jerarquías"
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Jerarquias...")
        Me.BindingContext(ds, nTabla).Position = f.apuntador
        AsignaTXT(f.Apuntador)
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Me.Close()
    End Sub
    Private Function AgregarRenglon(ByVal dtG As DataTable, ByVal dgG As DataGridView, ByVal Nivel As Integer, ByVal txtMascara As String) As DataTable
        Dim f As New jsMerArcJerarquiasMovimientos
        f.Apuntador = Me.BindingContext(ds, dtG.TableName).Position
        f.Agregar(myConn, ds, dtG, txtTipJer.Text, Nivel, txtMascara)
        ds = DataSetRequery(ds, " select * from jsmerrenjer where tipjer = '" & txtTipJer.Text & "' " _
                        & " and nivel = " & Nivel & " and id_emp = '" & jytsistema.WorkID & "' order by tipjer, codjer ", myConn, _
                        dtG.TableName, lblInfo)
        AsignaMov(f.Apuntador, dtG, dgG, Nivel, True)
        dtG = ds.Tables(dtG.TableName)
        f = Nothing
        Return dtG
    End Function
    Private Function EditarRenglon(ByVal dtG As DataTable, ByVal dgG As DataGridView, ByVal Nivel As Integer) As DataTable
        If dtG.Rows.Count > 0 Then
            Dim f As New jsMerArcJerarquiasMovimientos
            f.Apuntador = Me.BindingContext(ds, dtG.TableName).Position
            f.Editar(myConn, ds, dtG, txtTipJer.Text, Nivel)
            ds = DataSetRequery(ds, " select * from jsmerrenjer where tipjer = '" & txtTipJer.Text & "' " _
                            & " and nivel = " & Nivel & " and id_emp = '" & jytsistema.WorkID & "' order by tipjer, codjer ", myConn, _
                            dtG.TableName, lblInfo)
            AsignaMov(f.Apuntador, dtG, dgG, Nivel, True)
            dtG = ds.Tables(dtG.TableName)
            f = Nothing
        End If
        Return dtG
    End Function
    Private Sub AsignaMov(ByVal nRow As Long, ByVal dtRenglones As DataTable, ByVal dg As DataGridView, ByVal Nivel As Integer, _
                          ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then
            ds = DataSetRequery(ds, " select * from jsmerrenjer where tipjer = '" & txtTipJer.Text & "' " _
                        & " and nivel = " & Nivel & " and id_emp = '" & jytsistema.WorkID & "' order by tipjer, codjer ", _
                        myConn, dtRenglones.TableName, lblInfo)
            dtRenglones = ds.Tables(dtRenglones.TableName)
        End If
        If c >= 0 AndAlso dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, dtRenglones.TableName).Position = c
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If
    End Sub
    Private Function EliminarRenglon(ByVal dtG As DataTable, ByVal dgG As DataGridView, ByVal Nivel As Integer) As DataTable

        Dim PosicionG As Long = Me.BindingContext(ds, dtG.TableName).Position
        If PosicionG >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtG.Rows(PosicionG)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("codjer"))
                    Dim aCamposDel() As String = {"tipjer", "codjer", "nivel", "id_emp"}
                    Dim aStringsDel() As String = {.Item("tipjer"), .Item("codjer"), Nivel, jytsistema.WorkID}
                    PosicionG = EliminarRegistros(myConn, lblInfo, ds, dtG1.TableName, "jsmerrenjer", _
                                                  " select * from jsmerrenjer where tipjer = '" & TipoJerarquia & "' " _
                                                & " and nivel = " & Nivel & " and id_emp = '" & jytsistema.WorkID & "' order by tipjer, codjer ", _
                                                aCamposDel, aStringsDel, _
                                                Me.BindingContext(ds, dtG.TableName).Position, True)

                    If dtG.Rows.Count - 1 < PosicionG Then PosicionG = dtG.Rows.Count - 1
                    AsignaMov(PosicionG, dtG, dgG, Nivel, True)
                End With

            End If
        End If
        Return dtG

    End Function
    Private Sub btnAgregarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS1.Click
        dtG1 = AgregarRenglon(dtG1, dg1, 1, txtMascara1.Text)
    End Sub

    Private Sub btnEditarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS1.Click
        dtG1 = EditarRenglon(dtG1, dg1, 1)
    End Sub

    Private Sub btnEliminarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarS1.Click
        dtG1 = EliminarRenglon(dtG1, dg1, 1)
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean
        Validado = True
    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Posicion = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditMERCASEncabezadoJerarquia(myConn, lblInfo, Inserta, txtTipJer.Text, _
                                                txtTipjerNombre.Text, txtMascara1.Text, txtMascara2.Text, _
                                                txtMascara3.Text, txtMascara4.Text, txtMascara5.Text, txtMascara6.Text, _
                                                txtDescrip1.Text, txtDescrip2.Text, txtDescrip3.Text, txtDescrip4.Text, _
                                                txtDescrip5.Text, txtDescrip6.Text, txtCodigoProveedor.Text, txtCuentaContable.Text)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = Posicion
        AsignaTXT(Posicion)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarTipoJerarquia(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(Posicion)
        End If
        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        ft.Ejecutar_strSQL(myconn, " delete from jsmerrenjer where tipjer = '" & txtTipJer.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtCodigoProveedor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoProveedor.TextChanged
        Dim aFld() As String = {"codpro", "id_emp"}
        Dim aStr() As String = {txtCodigoProveedor.Text, jytsistema.WorkID}
        txtNombreProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "nombre")
    End Sub

    Private Sub btnAgregarS2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS2.Click
        dtG2 = AgregarRenglon(dtG2, dg2, 2, txtMascara2.Text)
    End Sub

    Private Sub btnAgregarS3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS3.Click
        dtG3 = AgregarRenglon(dtG3, dg3, 3, txtMascara3.Text)
    End Sub

    Private Sub btnAgregarS4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS4.Click
        dtG4 = AgregarRenglon(dtG4, dg4, 4, txtMascara4.Text)
    End Sub

    Private Sub btnAgregarS5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS5.Click
        dtG5 = AgregarRenglon(dtG5, dg5, 5, txtMascara5.Text)
    End Sub

    Private Sub btnAgregarS6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS6.Click
        dtG6 = AgregarRenglon(dtG6, dg6, 6, txtMascara6.Text)
    End Sub

    Private Sub btnEditarS2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS2.Click
        dtG2 = EditarRenglon(dtG2, dg2, 2)
    End Sub

    Private Sub btnEditarS3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS3.Click
        dtG3 = EditarRenglon(dtG3, dg3, 3)
    End Sub

    Private Sub btnEditarS4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS4.Click
        dtG4 = EditarRenglon(dtG4, dg4, 4)
    End Sub

    Private Sub btnEditarS5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS5.Click
        dtG5 = EditarRenglon(dtG5, dg5, 5)
    End Sub

    Private Sub btnEditarS6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS6.Click
        dtG6 = EditarRenglon(dtG6, dg6, 6)
    End Sub

    Private Sub btnEliminarS2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarS2.Click
        dtG2 = EliminarRenglon(dtG2, dg2, 2)
    End Sub

    Private Sub btnEliminarS3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarS3.Click
        dtG3 = EliminarRenglon(dtG3, dg3, 3)
    End Sub

    Private Sub btnEliminarS4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarS4.Click
        dtG4 = EliminarRenglon(dtG4, dg4, 4)
    End Sub

    Private Sub btnEliminarS5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarS5.Click
        dtG5 = EliminarRenglon(dtG5, dg5, 5)
    End Sub

    Private Sub btnEliminarS6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarS6.Click
        dtG6 = EliminarRenglon(dtG6, dg6, 6)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cJerarquias, "Jerarquía", txtTipJer.Text)
        f = Nothing
    End Sub

    Private Sub btnProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click
        txtCodigoProveedor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where tipo = '0' and id_emp = '" & jytsistema.WorkID & "' order by codpro ", " LISTADO DE PROVEEDORES ....", txtCodigoProveedor.Text)
    End Sub

    Private Sub btnCuentaContable_Click(sender As Object, e As EventArgs) Handles btnCuentaContable.Click
        txtCuentaContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by codcon ", "Cuentas Contables", txtCuentaContable.Text)
    End Sub

    Private Sub txtCuentaContable_TextChanged(sender As Object, e As EventArgs) Handles txtCuentaContable.TextChanged
        lblCuentaContable.Text = ft.DevuelveScalarCadena(myConn, " select descripcion from jscotcatcon where codcon = '" + txtCuentaContable.Text + "' and id_emp = '" + jytsistema.WorkID + "' ")
    End Sub
End Class