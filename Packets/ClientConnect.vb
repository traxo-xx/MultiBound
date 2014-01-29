Public Class ClientConnectPacket
    Inherits Packet

    Sub New(ByVal Bytes As Byte())
        MyBase.New(Bytes)
    End Sub

    Public Overrides Function GetByteArray() As Byte()

    End Function
End Class
