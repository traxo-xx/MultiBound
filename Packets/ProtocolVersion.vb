Public Class ProtocolVersionPacket
    Inherits Packet
    Public Overrides Property OPCode As Byte = &H0
    Public Const Length As Byte = &H8
    Public Const Version As UInt32 = 636

    Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function GetByteArray() As Byte()
        Dim p As New PacketBuilder
        p.Write(Version)
        Payload = p.GetBytes
        Return Package(False, True)
    End Function
End Class
