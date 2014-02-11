Public Class HandshakeResponse
    Inherits Packet
    Public Overrides Property OPCode As Byte = &H8
    Public Property ClaimResponse As String
    Public Property ProcessedHash As String
    Sub New(ByVal Bytes As Byte())
        MyBase.New(Bytes, False, True)
        Dim b = Payload
        ClaimResponse = ReadString()
        ProcessedHash = ReadString()
    End Sub

    Public Overrides Function GetByteArray() As Byte()
        Return Nothing
    End Function
End Class
