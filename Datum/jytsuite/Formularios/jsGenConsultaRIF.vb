Imports MySql.Data.MySqlClient
Imports System.Security.Permissions
<PermissionSet(SecurityAction.Demand, Name:="FullTrust")> _
Public Class jsGenConsultaRIF
    Private sModulo As String = ""
    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private ft As New Transportables

    Private tipoWEB As TipoConsultaWEB
    Private aValores As New ArrayList()

    Private Property pageready As Boolean = False

    Public Property DireccionURL As String = ""
    Public Property CodigoCliente As String = ""
    Public Property RIF As String = ""
    Public Property NombreEmpresa As String = ""



    Public Sub Cargar(MyCon As MySqlConnection, nomModulo As String, tipoCarga As Integer)

        Me.Text = nomModulo
        lblTitulo.Text = nomModulo + " RIF : " + RIF
        myConn = MyCon
        tipoWEB = tipoCarga

        Me.ShowDialog()

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsGenConsultaRIF_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '
    End Sub

    Private Sub jsGenConsultaRIF_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        Try

            'wb..NavigateError += new 
            'WebBrowserNavigateErrorEventHandler(wb_NavigateError)
            'Controls.Add(wb)

            wb.Navigate(New Uri(DireccionURL))
            ''wb.Navigate("http://contribuyente.seniat.gob.ve/BuscaRif/BuscaRif.jsp")
            ''wb.Navigate("http://www.elrif.com/")
            ''Esperas a que se cargue la pagina
            WaitForPageLoad()
            ''completas el sitio con el numero
            If wb.Document IsNot Nothing Then

                Dim rifTextBox As HtmlElement = Nothing
                Dim btnOK As HtmlElement = Nothing
                Select Case tipoWEB
                    Case TipoConsultaWEB.iRIF
                        rifTextBox = wb.Document.All.Item("p_rif")
                        rifTextBox.InnerText = RIF
                    Case TipoConsultaWEB.iSADA
                        'wb.Document.GetElementById("rif_bdq").Focus()
                        rifTextBox = wb.Document.All.Item("rif_bdq")
                        'rifTextBox.InnerText = RIF
                        'System.Windows.Forms.SendKeys.Send(RIF)
                        'btnOK = wb.Document.All.Item("btn_buscar")
                        'btnOK.InvokeMember("click")
                        'WaitForPageLoad()
                End Select
            End If
        Catch ex As Exception
            ft.mensajeCritico(ex.Message)
        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            Dim HTMLDocument As HtmlDocument
            'Dim webclient As System.Net.WebClient = New System.Net.WebClient
            'Dim url As String = "http://www.somewebsite.com"
            'Dim myHTML As String '= webclient.DownloadString(url)

            'instead of downloading the html, lets get it from a file
            'Dim filePath As String = "C:\htmlsourcefile.txt"
            'Dim myStreamReader = New System.IO.StreamReader(filePath)
            'myHTML = myStreamReader.ReadToEnd

            ''wb.Navigate("about:blank")
            'Dim objectDoc = wb.Document
            'wb.Document.Write(myHTML)
            'wb.ScriptErrorsSuppressed = True
            HTMLDocument = wb.Document

            append("The document title is: " & HTMLDocument.Title)

            Dim headElementCollection As HtmlElementCollection = _
            HTMLDocument.GetElementsByTagName("head")

            'call the function (no value is returned)
            getChildren(headElementCollection)
            'append(vbCrLf)
            headElementCollection = HTMLDocument.GetElementsByTagName("body")
            'same function again, just for the body this time
            getChildren(headElementCollection)


        Catch ex As Exception

            append(ex.ToString)

        End Try

        ' For Each strr As String In aValores
        Select Case tipoWEB
            Case TipoConsultaWEB.iRIF
                NombreEmpresa = ""
                For Each ss As String In aValores
                    If Mid(ss, 1, 10) = RIF Then NombreEmpresa = Mid(ss, 12, ss.ToString.Length)
                Next
            Case TipoConsultaWEB.iSADA
                Dim si As Integer = aValores.IndexOf("Este RIF tiene las siguientes empresas asociadas inscritas en el SICA")
                Dim sf As Integer = aValores.IndexOf("(2)Registro de Empresa - Validación de email")
                Dim aaValores As New ArrayList
                If si > 0 AndAlso sf > 0 Then
                    For iCont As Integer = 0 To aValores.Count - 1
                        If iCont > si AndAlso iCont < sf Then
                            aaValores.Add(aValores(iCont))
                        End If
                    Next
                End If

                For jCont As Integer = 7 To aaValores.Count - 2 Step 7
                    ft.Ejecutar_strSQL(myConn, " REPLACE jsvencatclisada values( '" & CodigoCliente & "', " _
                                   & "'" & aaValores(jCont + 0) & "', " _
                                   & "'" & aaValores(jCont + 1) & "', " _
                                   & "'" & aaValores(jCont + 2) & "', " _
                                   & "'" & aaValores(jCont + 3) & "', " _
                                   & "'" & aaValores(jCont + 4) & "', " _
                                   & "'" & aaValores(jCont + 5) & "', " _
                                   & "'" & aaValores(jCont + 6) & "', " _
                                   & "'" & jytsistema.WorkID & "') ")
                Next

                ft.Ejecutar_strSQL(myConn, "Delete from jsvencatclisada where estatus = 'Eliminada' ")


        End Select

        Me.Close()

    End Sub

    Private Sub wb_NavigateError(sender As Object, e As MyExtendedBrowserControl.WebBrowserNavigateErrorEventArgs) Handles wb.NavigateError
        ' Display an error message to the user.
        ft.mensajeCritico("En estos momentos NO es posible navegar por " + e.Url)
    End Sub

    Private Sub wb_WBWantsToClose() Handles wb.WBWantsToClose
        Me.Close()
    End Sub

    Private Function MyInstrLast(ByVal pstrText As String, ByVal pstrSearch As String) As Double

        Dim pos As Integer = 0
        Dim Aux As Integer
        Do
            Aux = InStr(pos + 1, UCase(pstrText), UCase(pstrSearch))
            If Aux <> 0 Then pos = Aux
        Loop Until Aux = 0
        MyInstrLast = pos

    End Function

    Private Sub WaitForPageLoad()
        AddHandler wb.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        While Not pageready
            'refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso)
            Application.DoEvents()
        End While
        pageready = False
    End Sub

    Private Sub PageWaiter(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        If wb.ReadyState = WebBrowserReadyState.Complete Then
            pageready = True
            RemoveHandler wb.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        End If
    End Sub

    Private Function getChildren(ByVal xElementCollection As HtmlElementCollection)

        Dim xLabel As String
        Dim parentElement As HtmlElement

        For Each parentElement In xElementCollection
            If parentElement.Children.Count > 0 Then

                Select Case parentElement.TagName.ToLower
                    Case "tr" : xLabel = "Row"
                    Case "td" : xLabel = "Cell"
                    Case "th" : xLabel = "Header"
                    Case "a" : xLabel = "Anchor"
                    Case "tbody" : xLabel = "T-Body"
                    Case "div" : xLabel = "Division"
                    Case "head" : xLabel = "Head"
                    Case "body" : xLabel = "Body"
                    Case "table" : xLabel = "Table"
                    Case "p" : xLabel = "Paragraph"
                    Case Else
                        'append("-->" & parentElement.InnerText)
                        xLabel = "element not specified"

                End Select

                'append("<" & xLabel & ">")
                getChildren(parentElement.Children)
                'append("<" & xLabel & " />")

            Else
                append("" & parentElement.InnerText & "")
                'If parentElement.InnerText <> "" Then

                'Else
                '    ' append("     " & vbNull.ToString & "")
                'End If

                'If parentElement.GetAttribute("href").ToString <> "" Then
                '    'append("    " & parentElement.GetAttribute("href") & "")
                'End If

            End If
        Next

        Return Nothing

    End Function
    Private Sub append(ByVal myTextToAppend As String)
        'TextBox1.AppendText(myTextToAppend & vbCrLf)
        aValores.Add(myTextToAppend)
        'refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso)
        Application.DoEvents()
        ' outputXL = outputXL & myTextToAppend & vbCrLf
    End Sub


    Private Sub wb_DocumentCompleted(sender As System.Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles wb.DocumentCompleted

    End Sub
End Class

