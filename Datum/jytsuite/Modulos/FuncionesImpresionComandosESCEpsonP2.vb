Module FuncionesImpresionComandosESCEpsonP2

    Private ESC As String = Chr(27)

    Public Enum Ep_Pasos
        EpPICA = 80
        EpELITE = 77
    End Enum

    Public Enum Ep_Fuentes
        EpDRAFT = 1
        EpROMAN
        EpSANS_SERIF
    End Enum

    Public Enum Ep_Alignment
        EpIZQUIERDA = 1
        EpDERECHA
        EpCENTRO
        EpJUSTIFICADO
    End Enum

    Public Enum Ep_InterEsp
        EpOcho
        EpSeis
    End Enum

    Public Function Alineación(Optional ByVal lngAlign As Ep_Alignment = Ep_Alignment.EpIZQUIERDA) As String
        Alineación = ESC & "a"
        Select Case lngAlign
            Case Ep_Alignment.EpIZQUIERDA
                Alineación = Alineación & "0"
            Case Ep_Alignment.EpDERECHA
                Alineación = Alineación & "2"
            Case Ep_Alignment.EpCENTRO
                Alineación = Alineación & "1"
            Case Ep_Alignment.EpJUSTIFICADO
                Alineación = Alineación & "3"
        End Select
    End Function

    Public Function Condensado(Optional ByVal Activado As Boolean = True) As String
        Select Case Activado
            Case True
                Condensado = ESC & "SI"
            Case Else
                Condensado = "DC2"
        End Select
    End Function

    Public Function CRLF() As String
        CRLF = Chr(13) & Chr(10)
    End Function

    Public Function DobleAncho(Optional ByVal Activado As Boolean = True) As String
        Select Case Activado
            Case True
                DobleAncho = ESC & "W1"
            Case Else
                DobleAncho = ESC & "W0"
        End Select
    End Function


    Public Function Exponente(Optional ByVal Activado As Boolean = True) As String
        If Activado Then
            Exponente = ESC & "S0"
        Else
            Exponente = ESC & "T"
        End If
    End Function
    Public Function Reset() As String
        Return Chr(27) + "@"
    End Function
    Public Function Letra12p() As String
        Return Chr(27) + "M"
    End Function
    Public Function Fuente(Optional ByVal lngFuente As Ep_Fuentes = Ep_Fuentes.EpDRAFT) As String
        Select Case lngFuente
            Case Ep_Fuentes.EpDRAFT
                Fuente = ESC & "x0"
            Case Ep_Fuentes.EpROMAN
                Fuente = ESC & "x1 " & ESC & "k0"
            Case Ep_Fuentes.EpSANS_SERIF
                Fuente = ESC & "x1 " & ESC & "k1"
            Case Else
                Fuente = ESC & "x0"
        End Select
    End Function

    Public Function Interespaciado(Optional ByVal Lineas As Ep_InterEsp = Ep_InterEsp.EpSeis) As String
        Interespaciado = ESC & "2"
        If Lineas = Ep_InterEsp.EpOcho Then Interespaciado = ESC & "0"
    End Function

    Public Function MargenDerecho(Optional ByVal Espacios As Long = 80) As String
        MargenDerecho = ESC & "Q" & Espacios
    End Function

    Public Function MargenIzquierdo(Optional ByVal Espacios As Long = 0) As String
        MargenIzquierdo = ESC & "l" & Espacios
    End Function

    Public Function Negrita(Optional ByVal Activado As Boolean = True) As String
        If Activado Then
            Negrita = ESC & "E"
        Else
            Negrita = ESC & "F"
        End If
    End Function

    Public Function SubÍndice(Optional ByVal Activado As Boolean = True) As String
        If Activado Then
            SubÍndice = ESC & "S1"
        Else
            SubÍndice = ESC & "T"
        End If
    End Function

    Public Function Paso(Optional ByVal lngPaso As Ep_Pasos = Ep_Pasos.EpPICA) As String
        Paso = ESC & Chr(lngPaso)
    End Function

    Public Function SaltoPágina() As String
        SaltoPágina = Chr(12)
    End Function

    Public Function Subrayado(Optional ByVal Activado As Boolean = True) As String
        If Activado Then
            Subrayado = ESC & "-1"
        Else
            Subrayado = ESC & "-0"
        End If
    End Function

    Public Function Tabulación() As String
        Tabulación = Chr(9)
    End Function


End Module
