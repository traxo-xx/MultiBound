Public Class Player
    Public Property Name As String
    Public Property Species As String
    Public Property Account As String
    Public Property UUID As Byte()
    Public Property ChatLog As New List(Of String)
    Sub New(ByVal Connect As ClientConnectPacket)
        Name = Connect.Name
        Species = Connect.Species
        Account = Connect.Account
        UUID = Connect.UUID
    End Sub
End Class
