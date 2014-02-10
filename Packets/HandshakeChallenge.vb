Public Class HandshakeChallenge
    Inherits Packet
    Public Property Salt As String
    Public Property Rounds As Integer
    Sub New(ByVal _Salt As String, Optional ByVal _Rounds As Integer = 5000)
        MyBase.New()
        Salt = _Salt
        Rounds = _Rounds
    End Sub
    Public Overrides Function GetByteArray() As Byte()
        Dim p As New PacketBuilder
        p.Write("")
        p.Write(Salt)
        p.Write(Rounds)
        Payload = p.GetBytes
        Return Package()
    End Function
End Class
