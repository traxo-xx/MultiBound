Public Class ClientConnectPacket
    Inherits Packet

    Public Overrides Property OPCode As Byte = &H6
    Public Property AssetDigest As Byte()
    Public Property Claim As Object
    Public Property UUID As Byte()
    Public Property Name As String
    Public Property Species As String
    Public Property Shipworld As Byte()
    Public Property Account As String

    Sub New(ByVal Bytes As Byte())
        MyBase.New(Bytes)
        My.Computer.FileSystem.WriteAllBytes("C:\Users\Jesse\Documents\topkek.lelelel", Bytes, False)
        ReadByte()
        AssetDigest = ReadByteArray()
        GetByteArray() 'lel, this is placeholder for claim
        Dim u = ReadByte()
        If u = 1 Then UUID = ReadBytes(16)
        Name = ReadString()
        Species = ReadString()
        'There's no point in continuing here, but i'll finish this for another day
    End Sub

    Public Overrides Function GetByteArray() As Byte()
        Return Nothing
    End Function
End Class
