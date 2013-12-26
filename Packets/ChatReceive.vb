Public Class ChatReceivePacket
    Inherits Packet

    Public Property OPCode As Byte = &H11
    Public Property Length As Byte = 0
    Public Property NameLength As Byte = 4
    Public Property Name As String = "NAME"
    Public Property TextLength As Byte = 0
    Public Property Text As String = ""
    Public Property Terminator As Byte = 0
    Public Overrides Function GetByteArray() As Byte()

    End Function

    Public Overrides Sub LoadByteArray(ByteArray() As Byte)
        Length = ByteArray(1) / 2
        TextLength = ByteArray(2)
        Dim a = BasicUFL.ArrayConversion.ToList(ByteArray)
        a.RemoveRange(0, 3)
        Text = System.Text.Encoding.UTF8.GetChars(a.toarray)
    End Sub
End Class
