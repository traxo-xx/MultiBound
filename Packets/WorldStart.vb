Public Class WorldStartPacket
    Inherits Packet
    Public Overrides Property OPCode As Byte = &HC

    Sub New(ByVal World As World)

    End Sub

    Public Overrides Function GetByteArray() As Byte()

    End Function
End Class
