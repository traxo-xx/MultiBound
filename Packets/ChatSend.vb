Public Class ChatSendPacket
    Inherits Packet

    Public Property OPCode As Byte = &H5
    Public Property Length As Byte = 0
    Public Property NameLength As Byte = 4
    Public Property Name As String = "NAME"
    Public Property TextLength As Byte = 0
    Public Property Text As String = ""
    Public Overrides Function GetByteArray() As Byte()
        Length = (6 + Name.Length + 1 + Text.Length) * 2
        TextLength = Text.Length
        NameLength = Name.Length
        Dim aarray As Byte()
        Dim barray As Byte() = System.Text.Encoding.UTF8.GetBytes(Text)
        Dim carray As Byte() = System.Text.Encoding.UTF8.GetBytes(Name)
        Dim darray As Byte() = {OPCode, Length, 1, 0, 0, 0, 0, 1, NameLength}
        aarray = Misc.Combine({darray, carray})
        Dim earray = {TextLength}
        Return Misc.Combine({aarray, earray, barray})
    End Function

    Public Overrides Sub LoadByteArray(ByteArray() As Byte)
        
    End Sub
End Class
