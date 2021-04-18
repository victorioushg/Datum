Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Public Class jsNomArcConstantesMovimientos
    Private Const sModulo As String = "Movimientos de Constantes de nómina"
    Private Const nTabla As String = "Constantes"

    Private MyConn As New MySqlConnection
    Private dsConstante As DataSet
    Private dtConstante As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private aTipo() As String = {"Numérica", "Fecha", "Caracter", "Booleana"}
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
        ft.habilitarObjetos(True, True, txtConstante, cmbTipo, txtValor)
        MyConn = MyCon
        dsConstante = ds
        dtConstante = dt
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtConstante.Text = ""
        txtValor.Text = ""
        ft.RellenaCombo(aTipo, cmbTipo)
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)
        i_modo = movimiento.iEditar
        ft.habilitarObjetos(False, True, txtConstante)

        MyConn = MyCon
        dsConstante = ds
        dtConstante = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtConstante

            txtConstante.Text = .Rows(nPosicion).Item("constante")
            txtValor.Text = .Rows(nPosicion).Item("valor")
            ft.RellenaCombo(aTipo, cmbTipo, .Rows(nPosicion).Item("tipo"))

        End With
    End Sub

    Private Sub jsNomArcConstantesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsConstante = Nothing
        ft = Nothing
        dtConstante = Nothing
    End Sub

    Private Sub jsNomArcConstantesMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtConstante.Text)
    End Sub

    Private Sub txtConstante_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConstante.GotFocus, _
        cmbTipo.GotFocus, txtValor.GotFocus
        Select Case sender.name
            Case "txtConstante"
                ft.mensajeEtiqueta(lblInfo, " Indique el código/nombre de la constante ... ", Transportables.TipoMensaje.iInfo)
            Case "txtValor"
                ft.mensajeEtiqueta(lblInfo, " Indique el valor de la constante ...", Transportables.TipoMensaje.iInfo)
            Case "cmbTipo"
                ft.mensajeEtiqueta(lblInfo, " Seleccione el tipo de constante ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean

        If Trim(txtConstante.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un codigo de constante válido...")
            txtConstante.Focus()
            Return False
        Else
            If i_modo = movimiento.iAgregar Then
                Dim afld() As String = {"constante", "id_emp"}
                Dim aFldN() As String = {txtConstante.Text, jytsistema.WorkID}
                If qFound(MyConn, lblInfo, "jsnomcatcot", afld, aFldN) Then
                    ft.mensajeAdvertencia("Código de constante YA se encuentra en archivo...")
                    txtConstante.Focus()
                    Return False
                End If
            End If
        End If

        If Trim(txtValor.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un valor para la constante ...")
            txtConstante.Focus()
            Return False
        Else
            Select Case cmbTipo.SelectedIndex
                Case 0 ' numerica
                    If Not ft.isNumeric(txtValor.Text) Then
                        ft.mensajeAdvertencia("Debe indicar un valor numérico para la constante ...")
                        txtConstante.Focus()
                        Return False
                    End If
                Case 1 ' fecha
                    If Not IsDate(txtValor.Text) Then
                        ft.mensajeAdvertencia("Debe indicar un valor fecha para la constante ...")
                        txtConstante.Focus()
                        Return False
                    End If
                Case 2 ' caracter
                    'Dim regexItem As Regex = New Regex("") '^[a-zA-Z0-9 ]*$
                    'If regexItem.IsMatch(txtValor.Text) Then
                    '    ft.mensajeCritico("Constante pose cracteres no válidos...")
                    '    Return False
                    'End If
                Case 3 ' booleana
                    If InStr("False.True", txtValor.Text, CompareMethod.Text) = 0 Then
                        ft.mensajeAdvertencia("Debe indicar un valor Boolenao (True/False) para la constante ...")
                        txtConstante.Focus()
                        Return False
                    End If
            End Select
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtConstante.Rows.Count
            End If
            InsertEditNOMINAConstante(MyConn, lblInfo, Insertar, txtConstante.Text, cmbTipo.SelectedIndex, txtValor.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
End Class