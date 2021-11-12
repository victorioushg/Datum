Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Imports System.Net.Http
Imports System.Net.Http.Headers

Public Class GenericGet(Of T)

    Public Async Function GetList(strApi As String) As Threading.Tasks.Task(Of IEnumerable(Of T))

        Using client As HttpClient = New HttpClient()
            client.BaseAddress = New Uri("https://localhost:44356/")
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
            ' Dim str As String = String.Format("api/customer/all/{0}", jytsistema.WorkID)
            Using response As HttpResponseMessage = Await client.GetAsync(strApi)
                Using content As HttpContent = response.Content
                    ' Get contents as a String.
                    Dim result As String = Await content.ReadAsStringAsync()
                    ' If data exists, return list
                    If result IsNot Nothing Then
                        'Dim res = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(result)("result"))
                        ' DataTable
                        ' dt = JsonConvert.DeserializeObject(Of DataTable)(result)
                        ' IEnumarable 
                        Dim ret = JsonConvert.DeserializeObject(Of IEnumerable(Of T))(result)
                        Return ret
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        End Using
    End Function


End Class
