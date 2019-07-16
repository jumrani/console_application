Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Reflection
Imports System.Text
Imports System.Web
Imports System.Xml.Linq
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Module Module1
    Sub Main(ByVal args() As String)
        Dim txtfirmname As String = String.Empty 'date serial string 
        Dim txtname As String = String.Empty 'plain text string
        Dim txtboolean As Boolean = False
        For i = 0 To args.Count - 1
            txtfirmname = args(0)
            txtname = args(1)
        Next
        Dim netbool As Boolean = IsConnectionAvailable()
        If netbool = True Then
            txtboolean = regcalls(txtfirmname, txtname, "9509508007", "", 834, "", 2731, "this is for demo", 2887)
        Else
            MsgBox("Internet is not found")
        End If
        If txtboolean = True Then
            MsgBox("Call Is Registered Sucessfully")
        Else
            MsgBox("Call Is  Not Registered Please Try again ")
        End If
    End Sub
    Public Function IsConnectionAvailable() As Boolean
        ' Returns True if connection is available
        ' Replace www.yoursite.com with a site that
        ' is guaranteed to be online - perhaps your
        ' corporate site, or microsoft.com
        Dim objUrl As New System.Uri("http://www.google.com/")
        ' Setup WebRequest
        Dim objWebReq As System.Net.WebRequest
        objWebReq = System.Net.WebRequest.Create(objUrl)
        objWebReq.Proxy = Nothing
        Dim objResp As System.Net.WebResponse
        Try
            ' Attempt to get response and return True
            objResp = objWebReq.GetResponse
            objResp.Close()
            objWebReq = Nothing
            Return True
        Catch ex As Exception
            ' Error, exit and return False
            objWebReq = Nothing
            Return False
        End Try
    End Function
    Public Function regcalls(txtfirmname As String, txtname As String, txtmobileno As String, txtemailId As String, busstypeInt As Integer, txtlocation As String, IssuetypeInt As Integer, IssueDesTxt As String, P_customerInt As Integer) As Boolean
        Dim Bool As Boolean = False
        Try
            Dim json As String = "'{\'task\':\'create\',\'WebSessions_Key\':25184,\'FirmName\':\'" & txtfirmname & "\',\'Name\':\'" & txtname & "\',\'MobileNo\':\'" & txtmobileno & "\',\'Email\':\'" & txtemailId & "\',\'BussType\':" & busstypeInt & ",\'Location\':\'" & txtlocation & "\',\'IssueType\':" & IssuetypeInt & ",\'IssueDes\':\'" & IssueDesTxt & "\',\'P_customers\':" & P_customerInt & "}'"
            json = json.Replace("'", """")
            Dim request As WebRequest = WebRequest.Create("http://api.saralerp.com/api/crm/PostRegCalls")
            request.Method = "POST"
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(json)
            request.ContentType = "application/json"
            request.ContentLength = byteArray.Length
            Dim dataStream As Stream = request.GetRequestStream()
            dataStream.Write(byteArray, 0, byteArray.Length)
            dataStream.Close()
            Dim response As WebResponse = request.GetResponse()
            If (CType(response, HttpWebResponse).StatusCode = HttpStatusCode.OK) Then
                Using dataStreamval As Stream = response.GetResponseStream()
                    Dim reader As StreamReader = New StreamReader(dataStreamval)
                    Dim responseFromServer As String = reader.ReadToEnd()
                    responseFromServer = responseFromServer.Replace("""", "")
                    If responseFromServer.ToLower = "true" Then
                        Bool = True
                    End If
                    reader.Close()
                End Using
            End If
            response.Close()
        Catch ex As Exception
            Bool = False
        End Try
        Return Bool
    End Function
End Module