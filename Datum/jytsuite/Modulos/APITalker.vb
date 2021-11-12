Imports Newtonsoft.Json
Imports System.Net.Http
Imports System.Net.Http.Headers

Module APITalker

    'Public Function ApiGet(myObject As t)

    '    Using client As HttpClient = New HttpClient()
    '        client.BaseAddress = New Uri("https://localhost:44356/")
    '        client.DefaultRequestHeaders.Accept.Clear()
    '        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
    '        Dim str As String = String.Format("api/customer/all/{0}", jytsistema.WorkID)
    '        Using response As HttpResponseMessage = Await client.GetAsync(str)
    '            Using content As HttpContent = response.Content
    '                ' Get contents of page as a String.
    '                Dim result As String = Await content.ReadAsStringAsync()
    '                ' If data exists, convert as a table
    '                If result IsNot Nothing Then
    '                    'Dim res = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(result)("result"))
    '                    ' DataTable
    '                    ' dt = JsonConvert.DeserializeObject(Of DataTable)(result)
    '                    ' IEnumarable 
    '                    Return JsonConvert.DeserializeObject(Of IEnumerable(Of itemType))(result)
    '                Else
    '                    Return Nothing

    '                End If
    '            End Using
    '        End Using
    '    End Using

    'End Function

End Module
