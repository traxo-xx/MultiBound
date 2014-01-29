Public Class ChatReceivePacket
    Inherits Packet

    Public Property OPCode As Byte = &H11
    Public Property Length As Byte = 0
    Public Property World As String = ""
    Public Property ChatChannel As ChatChannel
    Public Property ID As UInt32 = 9001
    Public Property Name As String = ""
    Public Property Text As String = ""

    Sub New(ByVal Bytes As Byte())
        MyBase.New(Bytes)
        'Length = Bytes(1) / 2
        'Dim a = BasicUFL.ArrayConversion.ToList(Bytes)
        'a.RemoveRange(0, 2)
        'Text = System.Text.Encoding.UTF8.GetChars(a.toarray)
        ReadBytes(1)
        'ChatChannel = ReadByte()
        'World = ReadString()
        'ID = ReadUInt32()
        'Name = ReadString()
        Text = ReadString(0.5)
    End Sub
    Public Overrides Function GetByteArray() As Byte()

    End Function
End Class
