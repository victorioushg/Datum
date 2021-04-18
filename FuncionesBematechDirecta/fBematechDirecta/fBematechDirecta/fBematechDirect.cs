using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO.Ports;

namespace fBematechDirecta
{
    public class fBematechDirect
    {
        public Boolean colecoesVazias;
        public Boolean erro;
        public Boolean retorno;
        public Boolean timeout;
        List<string> descricoes = new List<string>(); //Public descricoes As New Collection
        public int tamanhoBufferResposta;
        public long tempo = 0;
        public String bytesPorta;
        public int TIME_OUT;

        static int Asc(char c)
        {
            int converted = c;
            if (converted >= 0x80)
            {
                byte[] buffer = new byte[2];
                // if the resulting conversion is 1 byte in length, just use the value
                if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
                {
                    converted = buffer[0];
                }
                else
                {
                    // byte swap bytes 1 and 2;
                    converted = buffer[0] << 16 | buffer[1];
                }
            }
            return converted;
        }
        ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ///'' La función PreparaComando hace todos los cálculos del NBL, NBH, CSL, CSL
        //'' además de hacer las conversiones de comandos para la forma string con la función Chr()
        //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        public void PreparaComando(string comando, int bytesToRead, Boolean retornoNoFinal)
        {
            int STX = 2;
            int NBL = 4;
            int NBH = 0;
            String CMD = "";
            int CSL = 0;
            int CSH = 0;
            Double aux;

            TIME_OUT = 40;

            if (comando.Length.Equals(4) || comando.Length.Equals(6))
            {
                //'Empeza la construción del comando, transformando el parámetro "comando"
                //'en bytes que puedan ser reconocidos por la impresora.
                //'Además de esto,recibe los valores decimales de estos bytes y suma en el CSL
                for (int i = 1; i <= comando.Length - 1; i += 2)
                {
                    CMD = CMD + (char)Convert.ToInt32(comando.Substring(i, 2));         //' & Chr(CInt(Mid(comando, i, 4)))
                    CSL = CSL + Convert.ToInt32(comando.Substring(i, 2));              //' + CInt(Mid(comando, i, 4))
                }
                //'Si el comando poseer parámetros,  la acción será recorrer miembro por miembro
                //'incrementando el NBL (suma de los bytes que serán enviados), montando el CMD
                //'caractere por caractere y sumando el valor ASC de cada un en el
                //'checksum bajo (CSL)

                if (descricoes.Count > 0)
                {
                    foreach (String membro in descricoes)
                    {
                        NBL = NBL + membro.Length;
                        for (int i = 0; i <= membro.Length - 1; i++)
                        {
                            CMD = CMD + (char)Asc(Convert.ToChar(membro.Substring(i, 1)));
                            CSL = CSL + Asc(Convert.ToChar(membro.Substring(i, 1)));
                        }
                    }
                    //'Calcula el número de bytes bajo y alto (NBL y NBH)
                    if (NBL > 256)
                    {
                        NBH = NBL / 256;
                        NBL = NBL % 256;
                    }
                    //'Calcula el checksum alto y bajo
                    if (CSL > 256)
                    {
                        aux = CSL / 256;
                        CSH = (int)aux;
                        CSL = CSL % 256;
                    }
                }
            }
            retorno = EnviaComandoImpressora(STX, NBL, NBH, CMD, CSL, CSH, bytesToRead, retornoNoFinal);
        }


        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //'' La función EnviaComandoImpressora haz la comunicación directa con el puerto
        //'' y envia los datos para él
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        public Boolean EnviaComandoImpressora(int STX, int NBL, int NBH, String CMD, int CSL, int CSH, int bytesToRead, Boolean retornoNoFinal)
        {
            SerialPort sp = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
          
            byte ACK;
            byte ST1;
            byte ST2;
            bool flag;
            bool recebeu;
          
            timeout = false;
            flag = false;

            //frmComunicacaoDireta.lblACK = ""
            //frmComunicacaoDireta.lblST1 = ""
            //frmComunicacaoDireta.lblST2 = ""
            bytesPorta = "";

        }


       

        

        //        //Dim clean As Variant

        //        //'Abre el puerto
        //        sp.DtrEnable = true; 
        //        sp.RtsEnable = true; 
        //        sp.Open();

        //    .InputLen = 0
        //    clean = .Input
        //    .OutBufferCount = 0

        //    'Envia los bytes STX, NBL y NBH para la impresora
        //    .Output = Chr(STX) + Chr(NBL) + Chr(NBH)
        //    'Envia cada byte del CMD para la impresora
        //    For i = 1 To Len(CMD)
        //        .Output = Mid(CMD, i, 1)
        //        If .OutBufferCount <> 0 Then
        //            MsgBox "La impresora no respondio correctamente"
        //            EnviaComandoImpressora = False
        //            .PortOpen = False
        //            Exit Function

        //        End If

        //    Next i
        //    'Envia los bytes CSL y CSH para la impresora
        //    .Output = Chr(CSL) + Chr(CSH)
        //    'El tamaño 0 es genérico (varia de acuerdo con el tipo de operación)


        //    'Es habilitado el timer, que posee un timeout de 40 segundos (puede ser
        //    'configurado directo en el código del form


        //    'Primero, es hecho un loop que aguarda el recebimiento de los bytes del puerto.
        //    'Este loop es para operaciones más rápidas, en que la respuesta es
        //    'instantanea


        //    'Do
        //     '   DoEvents
        //      '  bytesPorta = bytesPorta & .Input
        //    'Loop While (tempo < 1)

        //    'Después de esto, como el tiempo de respuesta varia entre un comando y otro,
        //    'como por ejemplo Lectura X en la MP20, es hecho otro loop para verificar
        //    'se aún existen bytes en el buffer. Este es o loop para operaciones más lentas


        //    .InputLen = 1

        //    Dim bytesLidos
        //    bytesLidos = 0
        //    Dim tmp() As Byte

        //    ReDim tmp(bytesToRead)
        //    Dim buffer As Variant

        //    i = 0
        //    tempo = 0
        //    TIME_OUT = 7 ' siete segundos para el ack - lee ack aqui
        //    timeout = False
        //    frmComunicacaoDireta.tmrTempo.Enabled = True

        //    While (timeout = False And bytesLidos < 1)
        //        DoEvents
        //        If .InBufferCount > 0 Then
        //            buffer = .Input
        //            'bytesPorta = bytesPorta + Hex(tmp)
        //            tmp(i) = buffer(0)
        //            If (tmp(0) <> 6 And tmp(0) <> 21) Then
        //                'continue
        //            Else
        //                bytesLidos = bytesLidos + 1
        //                i = i + 1
        //            End If


        //        End If

        //    Wend

        //    tempo = 0
        //    TIME_OUT = 50 '

        //    frmComunicacaoDireta.tmrTempo.Enabled = False

        //    If bytesLidos = 0 Or tmp(0) <> 6 Then
        //        MsgBox "La impresora no respondio correctamente"
        //        EnviaComandoImpressora = False
        //        .PortOpen = False
        //        Exit Function

        //    End If

        //    frmComunicacaoDireta.tmrTempo.Enabled = True

        //    i = 1

        //    ' lee los otros datos aqui
        //    While (timeout = False And bytesLidos < bytesToRead)
        //        DoEvents
        //        If .InBufferCount > 0 Then
        //            buffer = .Input
        //            'bytesPorta = bytesPorta + Hex(tmp)
        //            tmp(i) = buffer(0)
        //            bytesLidos = bytesLidos + 1
        //            i = i + 1
        //        End If

        //    Wend

        //    If timeout And bytesLidos < bytesToRead Then
        //        MsgBox "La impresora no respondio correctamente - Hubo timeout"
        //        EnviaComandoImpressora = False
        //        .PortOpen = False
        //        Exit Function

        //    End If


        //    'Después de la lectura del buffer, cierra el puerto y deshabilita el timer
        //    frmComunicacaoDireta.tmrTempo.Enabled = False
        //    .PortOpen = False
        //    'Efectua la verificación del ACK para saber se
        //    'no existe problemas con el protocolo

        //    If bytesLidos < 3 Then ' ack, st1 e st2
        //        MsgBox "La impresora no respondio correctamente"
        //        EnviaComandoImpressora = False
        //        Exit Function

        //    Else
        //        ACK = tmp(0)
        //        If ACK <> 6 Then
        //            MsgBox "La impresora no respondio correctamente"
        //            EnviaComandoImpressora = False
        //            Exit Function
        //        End If

        //        If retornoNoFinal = True Then
        //            ST1 = tmp(bytesLidos - 2)
        //            ST2 = tmp(bytesLidos - 1)
        //            If (bytesLidos > 3) Then
        //            Dim hexa As String

        //            For i = 1 To bytesLidos - 3 Step 1
        //                hexa = Hex((tmp(i)))
        //                If Len(hexa) = 1 Then
        //                    hexa = "0" & hexa
        //                End If

        //                bytesPorta = bytesPorta & hexa
        //            Next i

        //            End If

        //        Else
        //            ST1 = tmp(1)
        //            ST2 = tmp(2)
        //            For i = 3 To bytesLidos - 1 Step 1
        //                hexa = Hex((tmp(i)))
        //                If Len(hexa) = 1 Then
        //                    hexa = "0" & hexa
        //                End If

        //                bytesPorta = bytesPorta & hexa
        //            Next i


        //        End If

        //        frmComunicacaoDireta.lblACK = ACK
        //        frmComunicacaoDireta.lblST1 = ST1
        //        frmComunicacaoDireta.lblST2 = ST2
        //    End If

        //    'Verifica los 3 bytes ACK, ST1 y ST2
        //    retorno = VerificaRetornoImpressora(ACK, ST1, ST2)
        //    If retorno = True Then
        //        EnviaComandoImpressora = True
        //    Else
        //        EnviaComandoImpressora = False
        //    End If
        //End With
        //}
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //'' La función LimpaColecoes remove todos los artículos de la coleción descripciones
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //Public Function LimpaColecoes() As Boolean
        //    While descricoes.Count > 0
        //        descricoes.Remove (1)
        //    Wend
        //    LimpaColecoes = True
        //End Function
        //Private Function IniciarComando(ByVal tamanho As Integer) As Boolean
        //    colecoesVazias = LimpaColecoes
        //    tamanhoBufferResposta = tamanho
        //End Function
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //'' La función TrataParametros recibe la descripción, el tamaño y el tipo
        //'' de um parámetro y hace el tratamiento, devolvendo el parámetro con el
        //'' número correcto de caracteres para quien llama la función
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //Public Function TrataParametros(ByVal descricao As String, ByVal tamanho As Integer, _
        //                                ByVal tipo As String) As String

        //    Dim i As Integer
        //    Dim j As Integer
        //    Dim tamanhoFaltante As Integer

        //    If Len(descricao) < tamanho Then

        //        'Calcula cuantos bytes faltan pra completar la string
        //        tamanhoFaltante = tamanho - Len(descricao)

        //        'Aqui tiene de ser hecha la verificación del tipo de parámetro que está siendo enviado
        //        For j = 1 To tamanhoFaltante Step 1
        //            If tipo = "integer" Or tipo = "float" Then
        //                descricao = "0" & descricao
        //            ElseIf tipo = "string" Then
        //                descricao = descricao & " "
        //            End If
        //        Next j
        //        TrataParametros = descricao

        //    'Si el tamaño de la descripción fuera mayor que el limit de tamaño del campo...
        //    ElseIf Len(descricao) > tamanho Then
        //        MsgBox "Error en el parámetro"
        //        erro = True
        //        Exit Function

        //    ElseIf Len(descricao) = tamanho Then
        //        TrataParametros = descricao
        //    End If

        //End Function

        //Public Function TransformaRetorno(ByVal tamanho As Long, ByVal retorno As Variant) As String
        //    Dim i As Integer
        //    Dim dados As String
        //    Dim temp1 As Integer
        //    Dim temp2 As Integer
        //    dados = ""
        //    For i = 1 To tamanho Step 1
        //        dados = dados & IIf(Asc(Mid(retorno, i, 1)) < 10, "0" & Asc(Mid(retorno, i, 1)), Asc(Mid(retorno, i, 1)))
        //        'temp1 = CInt(Mid((dados / 16), i, 1))
        //        'temp2 = temp1 + CInt(Mid((dados Mod 16), i, 1))
        //    Next i
        //    TransformaRetorno = CStr(dados)
        //End Function
        //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //'La función VerificaRetornoImpressora haz el tratamiento de los bytes ACK, ST1 y ST2
        //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //Public Function VerificaRetornoImpressora(ByVal ACK As Byte, ByVal ST1 As Byte, ST2 As Byte) As Boolean

        //        Dim cMSGErro As String

        //        If ACK = 21 Then
        //            MsgBox "A Impressora retornou NAK !" & vbCrLf & _
        //                                       "Error de Protocolo de Comunicación !", vbOK, _
        //                                       "Atención"
        //            VerificaRetornoImpressora = False

        //        Else 'ELSEIF2-1
        //            If (ST1 <> 0) Or (ST2 <> 0) Then 'IF2-1-1
        //                  ' Analisa ST1
        //                If (ST1 >= 128) Then 'IF2-1-1-1
        //                    ST1 = ST1 - 128
        //                    cMSGErro = cMSGErro & "Fin de Papel" & vbCrLf
        //                End If 'ENDIF2-1-1-1
        //                If (ST1 >= 64) Then 'IF2-1-1-2
        //                    ST1 = ST1 - 64
        //                    cMSGErro = cMSGErro & "Poco Papel" & vbCrLf
        //                    'MsgBox cMSGErro, vbOKOnly, "Atención"
        //                    VerificaRetornoImpressora = True
        //                    Exit Function
        //                End If 'ENDIF2-1-1-2
        //                If (ST1 >= 32) Then 'IF2-1-1-3
        //                    ST1 = ST1 - 32
        //                    cMSGErro = cMSGErro & "Error en el Relój" & vbCrLf
        //                End If 'ENDIF2-1-1-3
        //                If (ST1 >= 16) Then 'IF2-1-1-4
        //                    ST1 = ST1 - 16
        //                    cMSGErro = cMSGErro & "Impresora en Error" & vbCrLf
        //                End If 'ENDIF2-1-1-4
        //                If (ST1 >= 8) Then 'IF2-1-1-5
        //                    ST1 = ST1 - 8
        //                    cMSGErro = cMSGErro & "Primero Dato del Comando no fue ESC" & vbCrLf
        //                End If 'ENDIF2-1-1-5
        //                If ST1 >= 4 Then 'IF2-1-1-6
        //                    ST1 = ST1 - 4
        //                    cMSGErro = cMSGErro & "Comando Inexistente" & vbCrLf
        //                End If 'ENDIF2-1-1-6
        //                If ST1 >= 2 Then 'IF2-1-1-7
        //                    ST1 = ST1 - 2
        //                    cMSGErro = cMSGErro & "Comprobante Fiscal Abierto" & vbCrLf
        //                End If 'ENDIF2-1-1-7
        //                If ST1 >= 1 Then 'IF2-1-1-8
        //                    ST1 = ST1 - 1
        //                    cMSGErro = cMSGErro & "Número de Parámetros Inválidos" & vbCrLf
        //                End If 'ENDIF2-1-1-8
        //                'Analisa ST2
        //                If ST2 >= 128 Then 'IF2-1-1-9
        //                    ST2 = ST2 - 128
        //                    cMSGErro = cMSGErro & "Tipo de Parámetro de Comando Inválido" & vbCrLf
        //                End If 'ENDIF2-1-1-9
        //                If ST2 >= 64 Then 'IF2-1-1-10
        //                    ST2 = ST2 - 64
        //                    cMSGErro = cMSGErro & "Memória Fiscal Llena" & vbCrLf
        //                End If 'ENDIF2-1-1-10
        //                If ST2 >= 32 Then 'IF2-1-1-11
        //                    ST2 = ST2 - 32
        //                    cMSGErro = cMSGErro & "Error en CMOS" & vbCrLf
        //                End If 'ENDIF2-1-1-11
        //                If ST2 >= 16 Then 'IF2-1-1-12
        //                    ST2 = ST2 - 16
        //                    cMSGErro = cMSGErro & "Alicuota no Programada" & vbCrLf
        //                End If 'ENDIF2-1-1-12
        //                If ST2 >= 8 Then 'IF2-1-1-13
        //                    ST2 = ST2 - 8
        //                    cMSGErro = cMSGErro & "Capacidad de Alicuota Programables Llena" & vbCrLf
        //                End If 'ENDIF2-1-1-13
        //                If ST2 >= 4 Then 'IF2-1-1-14
        //                     ST2 = ST2 - 4
        //                     cMSGErro = cMSGErro & "Cancelamiento no permitido" & vbCrLf
        //                End If 'ENDIF2-1-1-14
        //                If ST2 >= 2 Then 'IF2-1-1-15
        //                    ST2 = ST2 - 2
        //                    cMSGErro = cMSGErro & "RIF del Propietario no Programados" & vbCrLf
        //                End If 'ENDIF2-1-1-15
        //                If ST2 >= 1 Then 'IF2-1-1-16
        //                    ST2 = ST2 - 1
        //                    cMSGErro = cMSGErro & "Comando no ejecutado" & vbCrLf
        //                End If 'ENDIF2-1-1-16
        //                If (cMSGErro <> "") Then 'IF2-1-1-17

        //                    MsgBox cMSGErro, vbOK, "Atención"
        //                    If VerificaRetornoImpressora = True Then
        //                        VerificaRetornoImpressora = False
        //                    End If
        //                End If 'ENDIF2-1-1-17
        //            Else
        //                VerificaRetornoImpressora = True
        //            End If 'ENDIF2-1-1
        //        End If 'ENDIF2-1
        //End Function



    }
}
