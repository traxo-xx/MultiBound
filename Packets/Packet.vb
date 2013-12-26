Partial Public MustInherit Class Packet
    Public MustOverride Function GetByteArray() As Byte()
    Public MustOverride Sub LoadByteArray(ByVal ByteArray As Byte())
End Class
