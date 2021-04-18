Module ConsultasGenerales
    Public Function SeleccionGENTablaSimple(ByVal modulo As String) As String
        SeleccionGENTablaSimple = " select * from jsconctatab where modulo = '" & modulo & "' and id_emp = '" & jytsistema.WorkID & "' order by codigo "
    End Function
    Public Function SeleccionGENComentarios(ByVal Origen As String) As String
        Return " select codigo, comentario from jsconctacom where origen = '" & Origen & "' and id_emp = '" & jytsistema.WorkID & "' order by codigo "
    End Function

End Module
