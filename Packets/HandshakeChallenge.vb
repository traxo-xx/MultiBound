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

    End Function
End Class
